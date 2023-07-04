using System.Reflection;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a managed field.
    /// </summary>
    public readonly struct ManagedField
    {

        readonly string name;
        readonly ReadOnlyFixedValueList<ManagedCustomAttribute> customAttributes;
        readonly FieldAttributes attributes;
        readonly ManagedTypeSignature fieldType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="customAttributes"></param>
        /// <param name="attributes"></param>
        /// <param name="fieldType"></param>
        public ManagedField(string name, in ReadOnlyFixedValueList<ManagedCustomAttribute> customAttributes, FieldAttributes attributes, ManagedTypeSignature fieldType)
        {
            this.name = name;
            this.customAttributes = customAttributes;
            this.attributes = attributes;
            this.fieldType = fieldType;
        }

        /// <summary>
        /// Gets the name of the managed field.
        /// </summary>
        public readonly string Name => name;

        /// <summary>
        /// Gets the set of custom attributes applied to the field.
        /// </summary>
        public readonly ReadOnlyFixedValueList<ManagedCustomAttribute> CustomAttributes => customAttributes;

        /// <summary>
        /// Gets the attributes of the field.
        /// </summary>
        public readonly FieldAttributes Attributes => attributes;

        /// <summary>
        /// Gets the type of the field.
        /// </summary>
        public readonly ManagedTypeSignature FieldType => fieldType;

        /// <inhericdoc />
        public override readonly string ToString() => name;

    }

}
