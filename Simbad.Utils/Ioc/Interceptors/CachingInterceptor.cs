using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

using Ninject.Extensions.Interception;

namespace Simbad.Utils.Ioc.Interceptors
{
    public class CachingInterceptor : IInterceptor
    {
        private readonly int _durationInMinutes;

        private readonly bool _isSlidingExpiration;

        private static Cache Cache
        {
            get
            {
                return (HttpContext.Current == null)
                           ? HttpRuntime.Cache
                           : HttpContext.Current.Cache;
            }
        }

        public CachingInterceptor(int durationInMinutes, bool isSlidingExpiration)
        {
            _durationInMinutes = durationInMinutes;
            _isSlidingExpiration = isSlidingExpiration;
        }

        public void Intercept(IInvocation invocation)
        {
            var className = invocation.Request.Target.GetType().FullName;
            var methodName = invocation.Request.Method.Name;
            var arguments = invocation.Request.Arguments;

            var builder = new StringBuilder(100);
            builder.Append(className);
            builder.Append(".");
            builder.Append(methodName);

            arguments.ToList().ForEach(x =>
                {
                    builder.Append("_");
                    builder.Append(x);
                });

            var cacheKey = builder.ToString();
            var retrieve = Cache.Get(cacheKey);

            if (retrieve == null)
            {
                invocation.Proceed();
                retrieve = invocation.ReturnValue;
                AddToCache(cacheKey, retrieve);
            }
            else
            {
                invocation.ReturnValue = retrieve;
            }
        }

        private void AddToCache(string key, object value)
        {
            if (value == null)
            {
                return;
            }

            TimeSpan slidingExpiration;
            DateTime absoluteExpiration;

            if (_isSlidingExpiration)
            {
                slidingExpiration = TimeSpan.FromMinutes(_durationInMinutes);
                absoluteExpiration = Cache.NoAbsoluteExpiration;
            }
            else
            {
                slidingExpiration = Cache.NoSlidingExpiration;
                absoluteExpiration = DateTime.Now.AddMinutes(_durationInMinutes);
            }

            Cache.Add(key, value, null, absoluteExpiration, slidingExpiration, CacheItemPriority.Normal, null);
        }
    }
}