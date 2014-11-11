using System;

using Ninject;
using Ninject.Extensions.Interception;
using Ninject.Extensions.Interception.Attributes;
using Ninject.Extensions.Interception.Request;
using Ninject.Parameters;

using Simbad.Utils.Ioc.Interceptors;

namespace Simbad.Utils.Ioc.Attributes
{
    public class AffectedClassCacheAttribute : InterceptAttribute
    {
        private readonly Type _type;

        public AffectedClassCacheAttribute(Type type)
        {
            _type = type;
        }

        public override IInterceptor CreateInterceptor(IProxyRequest request)
        {
            return request.Context.Kernel.Get<AffectedCachingInterceptor>(
                new ConstructorArgument("cacheKeyStartsWith", _type.FullName));
        }
    }
}