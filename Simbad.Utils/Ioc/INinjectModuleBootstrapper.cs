using System.Collections.Generic;

using Ninject.Modules;

namespace Simbad.Utils.Ioc
{
    public interface INinjectModuleBootstrapper
    {
        IList<INinjectModule> GetModules();
    }
}