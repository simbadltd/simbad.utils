using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Ninject;
using Ninject.Parameters;
using Ninject.Planning.Bindings;

using Simbad.Utils.Utils;

namespace Simbad.Utils.Ioc
{
    public class IocContainerBase
    {
        protected readonly StandardKernel Kernel;

        public IocContainerBase()
        {
            var assemblies = GetAssemblies().Select(Assembly.LoadFrom);
            Kernel = LoadNinjectKernel(assemblies);
        }

        protected virtual string[] GetAssemblies()
        {
            return Directory.GetFiles(PathUtils.GetApplicationRoot());
        }

        public T Get<T>(params IParameter[] parameters)
        {
            return Kernel.Get<T>(parameters);
        }

        public T Get<T>(string name, params IParameter[] parameters)
        {
            return Kernel.Get<T>(name, parameters);
        }

        public T Get<T>(Func<IBindingMetadata, bool> constraint, params IParameter[] parameters)
        {
            return Kernel.Get<T>(constraint, parameters);
        }

        public T[] GetAll<T>(params IParameter[] parameters)
        {
            return Kernel.GetAll<T>(parameters).ToArray();
        }

        public T[] GetAll<T>(string name, params IParameter[] parameters)
        {
            return Kernel.GetAll<T>(name, parameters).ToArray();
        }

        public T[] GetAll<T>(Func<IBindingMetadata, bool> constraint, params IParameter[] parameters)
        {
            return Kernel.GetAll<T>(constraint, parameters).ToArray();
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
