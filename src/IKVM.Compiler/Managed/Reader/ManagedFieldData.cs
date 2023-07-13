using System.Reflection;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed.Reader
{

    /// <summary>
    /// Represents a managed field.
    /// </summary>
    internal struct ManagedFieldData
    {

        /// <summary>
        /// Gets the name of the managed field.
        /// </summary>
        public string? Name;

        /// <summary>
        /// Gets the attributes of the field.
        /// </summary>
        public FieldAttributes Attributes;

        /// <summary>
        /// Gets the set of custom attributes applied to the field.
        /// </summary>
        public FixedValueList1<ManagedCustomAttributeData> CustomAttributes;

        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        public ManagedSignature FieldType;

    }

}
