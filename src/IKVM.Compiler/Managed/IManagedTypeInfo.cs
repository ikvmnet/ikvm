using System.Collections.Generic;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a managed type.
    /// </summary>
    public interface IManagedTypeInfo
    {

        /// <summary>
        /// Gets the module of the managed type.
        /// </summary>
        IManagedModuleInfo Module { get; }

        /// <summary>
        /// Gets the name of the managed type.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the set of fields declared on the managed type.
        /// </summary>
        IReadOnlyList<IManagedFieldInfo> Fields { get; }

    }

}
