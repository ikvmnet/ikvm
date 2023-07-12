using System.Reflection;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Represents a managed property.
    /// </summary>
    internal struct ManagedPropertyData
    {

        /// <summary>
        /// Gets the name of the managed property.
        /// </summary>
        public string Name;

        /// <summary>
        /// Gets the set of custom attributes applied to the property.
        /// </summary>
        public FixedValueList1<ManagedCustomAttributeData> CustomAttributes;

        /// <summary>
        /// Gets the attributes of the property.
        /// </summary>
        public PropertyAttributes Attributes;

        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        public ManagedSignature PropertyType;

        /// <inhericdoc />
        public override readonly string ToString() => Name;

    }

}
