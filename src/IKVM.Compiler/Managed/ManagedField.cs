using System.Reflection;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a managed field.
    /// </summary>
    internal struct ManagedField
    {

        /// <summary>
        /// Gets the null value of a managed field.
        /// </summary>
        public static readonly ManagedField Nil = new ManagedField(true);

        /// <summary>
        /// If set the value is null.
        /// </summary>
        public bool IsNil = true;

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
        public FixedValueList1<ManagedCustomAttribute> CustomAttributes;

        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        public ManagedSignature FieldType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ManagedField(bool isNil)
        {
            IsNil = isNil;
        }

        /// <inhericdoc />
        public override readonly string ToString() => Name;

    }

}
