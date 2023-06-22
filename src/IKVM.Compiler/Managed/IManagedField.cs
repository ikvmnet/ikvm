using System.Reflection;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a managed field.
    /// </summary>
    internal interface IManagedField : IManagedMember
    {

        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        ManagedTypeSignature FieldType { get; }

        /// <summary>
        /// Gets the attributes of the field.
        /// </summary>
        FieldAttributes Attributes { get; }

    }

}
