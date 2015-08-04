using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using Ninject;
using Ninject.Modules;
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
        }

        public IocContainerBase(string assembliesWildCard, INinjectSettings settings, params INinjectModule[] modules)
            : this(PathUtils.GetApplicationRoot(), assembliesWildCard, settings, modules)
        {
        }

        public IocContainerBase(string path, string assembliesWildCard, INinjectSettings settings, params INinjectModule[] modules)
        {
            var assemblies = GetAssemblies(path, assembliesWildCard).Select(Assembly.LoadFrom).ToList();
            Kernel = LoadNinjectKernel(assemblies, settings, modules);
        }

        protected ICollection<string> GetAssemblies(string path, string assembliesWildCard)
        {
            
            return Directory.GetFiles(path).Where(f => StringUtils.MatchWildcard(assembliesWildCard, f)).ToList();
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

        protected StandardKernel LoadNinjectKernel(IEnumerable<Assembly> assemblies, INinjectSettings settings, params INinjectModule[] modules)
        {
            var standardKernel = new StandardKernel(settings, modules);
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
