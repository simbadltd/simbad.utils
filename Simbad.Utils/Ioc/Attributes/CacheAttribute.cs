using Ninject;
using Ninject.Extensions.Interception;
using Ninject.Extensions.Interception.Attributes;
using Ninject.Extensions.Interception.Request;
using Ninject.Parameters;

using Simbad.Utils.Ioc.Interceptors;

namespace Simbad.Utils.Ioc.Attributes
{
    public class CacheAttribute : InterceptAttribute
    {
        public bool IsSlidingExpiration { get; private set; }

        public int DurationInMinutes { get; private set; }

        public CacheAttribute(int durationInMinutes = 5, bool isSlidingExpiration = false)
        {
            IsSlidingExpiration = isSlidingExpiration;
            DurationInMinutes = durationInMinutes;
        }

        public override IInterceptor CreateInterceptor(IProxyRequest request)
        {
            return request.Context.Kernel.Get<CachingInterceptor>(
                new ConstructorArgument("durationInMinutes", DurationInMinutes),
                new ConstructorArgument("isSlidingExpiration", IsSlidingExpiration));
        }
    }
}