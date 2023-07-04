using System.Collections.Generic;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Provides an interface for a managed type to call back into its loader.
    /// </summary>
    public interface IManagedTypeContext
    {

        /// <summary>
        /// Gets a the set of nested types for the given declaring type.
        /// </summary>
        IEnumerable<ManagedType> ResolveNestedTypes(ManagedType type);

    }

}
