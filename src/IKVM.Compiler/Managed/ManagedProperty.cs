using System.Reflection;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a managed property.
    /// </summary>
    public readonly struct ManagedProperty
    {

        readonly string name;
        readonly PropertyAttributes attributes;
        readonly ReadOnlyFixedValueList<ManagedCustomAttribute> customAttributes;
        readonly ManagedTypeSignature propertyType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="customAttributes"></param>
        /// <param name="propertyType"></param>
        public ManagedProperty(string name,  PropertyAttributes attributes, in ReadOnlyFixedValueList<ManagedCustomAttribute> customAttributes, ManagedTypeSignature propertyType)
        {
            this.name = name;
            this.attributes = attributes;
            this.customAttributes = customAttributes;
            this.propertyType = propertyType;
        }

        /// <summary>
        /// Gets the name of the managed property.
        /// </summary>
        public readonly string Name => name;

        /// <summary>
        /// Gets the set of custom attributes applied to the property.
        /// </summary>
        public readonly ReadOnlyFixedValueList<ManagedCustomAttribute> CustomAttributes => customAttributes;

        /// <summary>
        /// Gets the attributes of the property.
        /// </summary>
        public readonly PropertyAttributes Attributes => attributes;

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        public readonly ManagedTypeSignature PropertyType => propertyType;

        /// <inhericdoc />
        public override readonly string ToString() => name;

    }

}
