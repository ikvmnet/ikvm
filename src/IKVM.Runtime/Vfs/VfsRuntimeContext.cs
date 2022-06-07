using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#if NETCOREAPP3_1_OR_GREATER
using System.Runtime.Loader;

using Microsoft.Extensions.DependencyModel;
#endif

namespace IKVM.Runtime.Vfs
{

    /// <summary>
    /// Provides information to the VFS based on the current runtime environment.
    /// </summary>
    class VfsRuntimeContext : VfsContext
    {

        /// <summary>
        /// Default runtime context.
        /// </summary>
        public static VfsContext Instance = new VfsRuntimeContext();

        /// <summary>
        /// Gets the assembly known by the given name.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override Assembly GetAssembly(AssemblyName name)
        {
#if NET461
            return AppDomain.CurrentDomain.Load(name);
#else
            return GetAssembly(DependencyContext.Default, AssemblyLoadContext.Default,name);
#endif
        }

        /// <summary>
        /// Gets the assembly names known to the runtime.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<AssemblyName> GetAssemblyNames()
        {
#if NET461
            return AppDomain.CurrentDomain.GetAssemblies().Select(i => i.GetName()).Distinct();
#else
            return GetAssemblyNames(DependencyContext.Default, AssemblyLoadContext.Default);
#endif
        }

#if NETCOREAPP3_1_OR_GREATER

        /// <summary>
        /// Gets the list of available assemblies.
        /// </summary>
        /// <returns></returns>
        IEnumerable<AssemblyName> GetAssemblyNames(DependencyContext dependencyContext, AssemblyLoadContext assemblyLoadContext)
        {
            if (dependencyContext is null)
                throw new ArgumentNullException(nameof(dependencyContext));
            if (assemblyLoadContext is null)
                throw new ArgumentNullException(nameof(assemblyLoadContext));

            // concatinate the assemblies loaded in the runtime to those available to the runtime
            return assemblyLoadContext.Assemblies
                .Select(i => i.GetName())
                .Concat(dependencyContext.RuntimeLibraries.SelectMany(i => i.GetDefaultAssemblyNames(dependencyContext)))
                .Distinct();
        }

        /// <summary>
        /// Gets the list of available assemblies.
        /// </summary>
        /// <param name="dependencyContext"></param>
        /// <param name="assemblyLoadContext"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        Assembly GetAssembly(DependencyContext dependencyContext, AssemblyLoadContext assemblyLoadContext, AssemblyName name)
        {
            if (dependencyContext is null)
                throw new ArgumentNullException(nameof(dependencyContext));
            if (assemblyLoadContext is null)
                throw new ArgumentNullException(nameof(assemblyLoadContext));
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            return assemblyLoadContext.LoadFromAssemblyName(name);
        }

#endif

    }

}
