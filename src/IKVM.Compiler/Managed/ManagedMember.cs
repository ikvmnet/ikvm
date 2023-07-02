using System.Collections.Generic;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a managed member.
    /// </summary>
    public class ManagedMember
    {

        /// <summary>
        /// Gets the module of the managed field.
        /// </summary>
        ManagedType DeclaringType { get; }

        /// <summary>
        /// Gets the set of custom attributes applied to the type.
        /// </summary>
        IReadOnlyList<ManagedCustomAttribute> CustomAttributes { get; }

        /// <summary>
        /// Gets the name of the managed member.
        /// </summary>
        public string Name { get; }

    }

}
