using System.Collections.Generic;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Provides an interface for a managed assembly to call back into its loader.
    /// </summary>
    public interface IManagedAssemblyContext
    {

        /// <summary>
        /// Gets a the set of types available within the assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        ManagedType? ResolveType(ManagedAssembly assembly, string typeName);

        /// <summary>
        /// Gets a the set of types available within the assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IEnumerable<ManagedType> ResolveTypes(ManagedAssembly assembly);

    }

}
