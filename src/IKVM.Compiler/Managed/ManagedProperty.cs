using System.Reflection;
using System.Reflection.Metadata;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a managed property.
    /// </summary>
    public readonly struct ManagedProperty
    {

        /// <summary>
        /// Gets the name of the managed property.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Gets the set of custom attributes applied to the property.
        /// </summary>
        public readonly ReadOnlyFixedValueList1<ManagedCustomAttribute> CustomAttributes;

        /// <summary>
        /// Gets the attributes of the property.
        /// </summary>
        public readonly PropertyAttributes Attributes;

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        public readonly ManagedSignature PropertyType;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="customAttributes"></param>
        /// <param name="propertyType"></param>
        public ManagedProperty(string name, PropertyAttributes attributes, in ReadOnlyFixedValueList1<ManagedCustomAttribute> customAttributes, in ManagedSignature propertyType)
        {
            Name = name;
            Attributes = attributes;
            CustomAttributes = customAttributes;
            PropertyType = propertyType;
        }

        /// <inhericdoc />
        public override readonly string ToString() => Name;

    }

}
