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
            try
            {
#if NETFRAMEWORK
                // try to find an assembly that matches
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                    if (IsMatch(assembly.GetName(), name))
                        return assembly;

                // try to load it
                return Assembly.Load(name);
#else
                return GetAssembly(DependencyContext.Default, AssemblyLoadContext.Default, name);
#endif
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Returns <c>true</c> if the given assembly name reference the definition.
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="definition"></param>
        /// <returns></returns>
        bool IsMatch(AssemblyName reference, AssemblyName definition)
        {
            // match full name
            if (definition.Name != null && string.Equals(definition.Name, reference.Name, StringComparison.OrdinalIgnoreCase) == false)
                return false;

            // match version
            if (definition.Version != null && definition.Version != reference.Version)
                return false;

            // match culture
            if (definition.CultureName != null && string.Equals(definition.CultureName, reference.CultureName, StringComparison.OrdinalIgnoreCase) == false)
                return false;

            // match public key
            if (definition.GetPublicKeyToken() != null && definition.GetPublicKeyToken().AsSpan().SequenceEqual(reference.GetPublicKeyToken()) == false)
                return false;

            return true;
        }

        /// <summary>
        /// Gets the assembly names known to the runtime.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<AssemblyName> GetAssemblyNames()
        {
#if NETFRAMEWORK
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
                .Concat(AppDomain.CurrentDomain.GetAssemblies().Select(i => i.GetName()))
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
