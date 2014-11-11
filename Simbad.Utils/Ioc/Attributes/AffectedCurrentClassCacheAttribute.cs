using Ninject;
using Ninject.Extensions.Interception;
using Ninject.Extensions.Interception.Attributes;
using Ninject.Extensions.Interception.Request;
using Ninject.Parameters;

using Simbad.Utils.Ioc.Interceptors;

namespace Simbad.Utils.Ioc.Attributes
{
    public class AffectedCurrentClassCacheAttribute : InterceptAttribute
    {
        public override IInterceptor CreateInterceptor(IProxyRequest request)
        {
            return request.Context.Kernel.Get<AffectedCachingInterceptor>(
                new ConstructorArgument("cacheKeyStartsWith", request.Target.GetType().FullName));
        }
    }
}