using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Holds the data of a managed type.
    /// </summary>
    internal struct ManagedTypeData
    {

        /// <summary>
        /// Gets the parent type of this type.
        /// </summary>
        public ManagedType? DeclaringType;

        /// <summary>
        /// Gets the generic parameters on the managed type.
        /// </summary>
        public FixedValueList1<ManagedGenericParameter> GenericParameters;

        /// <summary>
        /// Gets the set of custom attributes applied to the type.
        /// </summary>
        public FixedValueList1<ManagedCustomAttributeData> CustomAttributes;

        /// <summary>
        /// Gets a reference to the base type.
        /// </summary>
        public ManagedSignature? BaseType;

        /// <summary>
        /// Gets the set of interfaces implemented on the managed type.
        /// </summary>
        public FixedValueList2<ManagedInterfaceData> Interfaces;

        /// <summary>
        /// Gets the set of fields declared on the managed type.
        /// </summary>
        public FixedValueList4<ManagedFieldData> Fields;

        /// <summary>
        /// Gets the set of methods declared on the managed type.
        /// </summary>
        public FixedValueList4<ManagedMethodData> Methods;

        /// <summary>
        /// Gets the set of properties declared on the managed type.
        /// </summary>
        public FixedValueList4<ManagedPropertyData> Properties;

        /// <summary>
        /// Gets the set of fields declared on the managed type.
        /// </summary>
        public FixedValueList1<ManagedEventData> Events;

    }

}
