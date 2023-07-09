using System.Reflection;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a managed field.
    /// </summary>
    public readonly struct ManagedField
    {

        /// <summary>
        /// Gets the null value of a managed field.
        /// </summary>
        public static readonly ManagedField Nil = new ManagedField();

        /// <summary>
        /// If set the value is null.
        /// </summary>
        public readonly bool IsNil = true;

        /// <summary>
        /// Gets the name of the managed field.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Gets the set of custom attributes applied to the field.
        /// </summary>
        public readonly ReadOnlyFixedValueList1<ManagedCustomAttribute> CustomAttributes;

        /// <summary>
        /// Gets the attributes of the field.
        /// </summary>
        public readonly FieldAttributes Attributes;

        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        public readonly ManagedSignature FieldType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="customAttributes"></param>
        /// <param name="attributes"></param>
        /// <param name="fieldType"></param>
        public ManagedField(string name, in ReadOnlyFixedValueList1<ManagedCustomAttribute> customAttributes, FieldAttributes attributes, in ManagedSignature fieldType)
        {
            IsNil = false;
            Name = name;
            CustomAttributes = customAttributes;
            Attributes = attributes;
            FieldType = fieldType;
        }

        /// <inhericdoc />
        public override readonly string ToString() => Name;

    }

}
