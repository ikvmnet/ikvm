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
    internal class VfsRuntimeContext : VfsContext
    {

        /// <summary>
        /// Default runtime context.
        /// </summary>
        public static VfsContext Instance = new VfsRuntimeContext();

        /// <summary>
        /// Gets the assembly known by the given name.
        /// </summary>
        /// <returns></returns>
        public override Assembly GetAssembly(AssemblyName name)
        {
            try
            {
#if NETFRAMEWORK
                return AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(i => IsMatch(i.GetName(), name)) ?? Assembly.Load(name);
#else
                return AssemblyLoadContext.GetLoadContext(typeof(VfsRuntimeContext).Assembly).LoadFromAssemblyName(name);
#endif
            }
            catch
            {
                return null;
            }
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
            return AssemblyLoadContext.GetLoadContext(typeof(VfsRuntimeContext).Assembly).Assemblies.Select(i => i.GetName());
#endif
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

    }

}
