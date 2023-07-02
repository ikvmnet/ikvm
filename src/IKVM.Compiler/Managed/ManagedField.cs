using System.Reflection;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a managed field.
    /// </summary>
    internal sealed class ManagedField : ManagedMember
    {

        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        public ManagedTypeSignature FieldType { get; }

        /// <summary>
        /// Gets the attributes of the field.
        /// </summary>
        public FieldAttributes Attributes { get; }

    }

}
