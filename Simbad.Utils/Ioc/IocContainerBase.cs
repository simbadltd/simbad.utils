using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Ninject;
using Ninject.Parameters;

using Simbad.Utils.Helpers;

namespace Simbad.Utils.Ioc
{
    public class IocContainerBase
    {
        private readonly StandardKernel _kernel;

        public IocContainerBase()
        {
            var assemblies = GetAssemblies().Select(Assembly.LoadFrom);
            _kernel = LoadNinjectKernel(assemblies);
        }

        protected virtual string[] GetAssemblies()
        {
            return Directory.GetFiles(PathHelper.GetApplicationRoot());
        }

        public T Get<T>(params IParameter[] parameters)
        {
            return _kernel.Get<T>(parameters);
        }

        public static StandardKernel LoadNinjectKernel(IEnumerable<Assembly> assemblies)
        {
            var standardKernel = new StandardKernel();
            foreach (var assembly in assemblies)
            {
                assembly.GetTypes()
                    .Where(t => t.GetInterfaces().Any(i => i.Name == typeof(INinjectModuleBootstrapper).Name))
                    .ToList()
                    .ForEach(
                        t =>
                        {
                            var ninjectModuleBootstrapper = (INinjectModuleBootstrapper)Activator.CreateInstance(t);

                            standardKernel.Load(ninjectModuleBootstrapper.GetModules());
                        });
            }

            return standardKernel;
        }
    }
}
