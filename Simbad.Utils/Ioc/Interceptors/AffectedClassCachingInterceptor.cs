using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;

using Ninject.Extensions.Interception;

namespace Simbad.Utils.Ioc.Interceptors
{
    public class AffectedCachingInterceptor : IInterceptor
    {
        private readonly string _cacheKeyStartsWith;

        private static Cache Cache
        {
            get
            {
                return (HttpContext.Current == null)
                           ? HttpRuntime.Cache
                           : HttpContext.Current.Cache;
            }
        }

        public AffectedCachingInterceptor(string cacheKeyStartsWith)
        {
            _cacheKeyStartsWith = cacheKeyStartsWith;
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();
            RemoveFromCache();
        }

        private void RemoveFromCache()
        {
            var keys = new List<string>();
            var enumerator = HttpRuntime.Cache.GetEnumerator();
            while (enumerator.MoveNext())
            {
                keys.Add((string)enumerator.Key);
            }

            foreach (var key in keys)
            {
                if (key.StartsWith(_cacheKeyStartsWith))
                {
                    Cache.Remove(key);
                }
            }
        }
    }
}