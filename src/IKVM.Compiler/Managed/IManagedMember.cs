using System.Collections.Generic;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a managed member.
    /// </summary>
    public interface IManagedMember
    {

        /// <summary>
        /// Gets the module of the managed field.
        /// </summary>
        IManagedType DeclaringType { get; }

        /// <summary>
        /// Gets the set of custom attributes applied to the type.
        /// </summary>
        IReadOnlyList<IManagedCustomAttribute> CustomAttributes { get; }

        /// <summary>
        /// Gets the name of the managed member.
        /// </summary>
        string Name { get; }

    }

}
