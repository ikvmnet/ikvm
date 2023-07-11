using System.Collections.Generic;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Provides an interface for a managed module to call back into its loader.
    /// </summary>
    internal interface IManagedModuleContext
    {

        /// <summary>
        /// Gets a the set of types available within the module.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        ManagedType? ResolveType(ManagedModule module, string typeName);

        /// <summary>
        /// Gets a the set of types available within the module.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        IEnumerable<ManagedType> ResolveTypes(ManagedModule module);

    }

}
