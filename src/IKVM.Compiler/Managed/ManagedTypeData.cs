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
        public FixedValueList1<ManagedCustomAttribute> CustomAttributes;

        /// <summary>
        /// Gets a reference to the base type.
        /// </summary>
        public ManagedSignature? BaseType;

        /// <summary>
        /// Gets the set of interfaces implemented on the managed type.
        /// </summary>
        public FixedValueList2<ManagedInterface> Interfaces;

        /// <summary>
        /// Gets the set of fields declared on the managed type.
        /// </summary>
        public FixedValueList4<ManagedField> Fields;

        /// <summary>
        /// Gets the set of methods declared on the managed type.
        /// </summary>
        public FixedValueList4<ManagedMethod> Methods;

        /// <summary>
        /// Gets the set of properties declared on the managed type.
        /// </summary>
        public FixedValueList4<ManagedProperty> Properties;

        /// <summary>
        /// Gets the set of fields declared on the managed type.
        /// </summary>
        public FixedValueList1<ManagedEvent> Events;

        /// <summary>
        /// Gets the set of nested types within the managed type.
        /// </summary>
        public FixedValueList1<ManagedType> NestedTypes;

    }

}
