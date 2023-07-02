using System.Collections.Generic;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a context in which one or more managed assemblies are loaded.
    /// </summary>
    internal interface IManagedAssemblyContext
    {

        /// <summary>
        /// Resolves the given type.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        ManagedType? ResolveType(string typeName);

        /// <summary>
        /// Gets a the set of types available within the managed assembly.
        /// </summary>
        IEnumerable<ManagedType> ResolveTypes();

    }

}
