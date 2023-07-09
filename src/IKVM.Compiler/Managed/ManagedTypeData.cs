using System.Reflection;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Holds the data of a managed type.
    /// </summary>
    internal readonly struct ManagedTypeData
    {

        /// <summary>
        /// Gets the parent assembly of this type.
        /// </summary>
        public readonly ManagedAssembly Assembly;

        /// <summary>
        /// Gets the parent type of this type.
        /// </summary>
        public readonly ManagedType? DeclaringType;

        /// <summary>
        /// Gets the name of the managed type.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Gets the attributes for the type.
        /// </summary>
        public readonly TypeAttributes Attributes;

        /// <summary>
        /// Gets the generic parameters on the managed type.
        /// </summary>
        public readonly ReadOnlyFixedValueList1<ManagedGenericParameter> GenericParameters;

        /// <summary>
        /// Gets the set of custom attributes applied to the type.
        /// </summary>
        public readonly ReadOnlyFixedValueList1<ManagedCustomAttribute> CustomAttributes;

        /// <summary>
        /// Gets a reference to the base type.
        /// </summary>
        public readonly ManagedSignature? BaseType;

        /// <summary>
        /// Gets the set of interfaces implemented on the managed type.
        /// </summary>
        public readonly ReadOnlyFixedValueList2<ManagedInterface> Interfaces;

        /// <summary>
        /// Gets the set of fields declared on the managed type.
        /// </summary>
        public readonly ReadOnlyFixedValueList4<ManagedField> Fields;

        /// <summary>
        /// Gets the set of methods declared on the managed type.
        /// </summary>
        public readonly ReadOnlyFixedValueList4<ManagedMethod> Methods;

        /// <summary>
        /// Gets the set of properties declared on the managed type.
        /// </summary>
        public readonly ReadOnlyFixedValueList4<ManagedProperty> Properties;

        /// <summary>
        /// Gets the set of fields declared on the managed type.
        /// </summary>
        public readonly ReadOnlyFixedValueList1<ManagedEvent> Events;

        /// <summary>
        /// Gets the set of nested types within the managed type.
        /// </summary>
        public readonly ReadOnlyFixedValueList1<ManagedType> NestedTypes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="declaringType"></param>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="genericParameters"></param>
        /// <param name="customAttributes"></param>
        /// <param name="baseType"></param>
        /// <param name="interfaces"></param>
        /// <param name="fields"></param>
        /// <param name="methods"></param>
        /// <param name="properties"></param>
        /// <param name="events"></param>
        /// <param name="nestedTypes"></param>
        public ManagedTypeData(ManagedAssembly assembly, ManagedType? declaringType, string name, TypeAttributes attributes, in ReadOnlyFixedValueList1<ManagedGenericParameter> genericParameters, in ReadOnlyFixedValueList1<ManagedCustomAttribute> customAttributes, in ManagedSignature? baseType, in ReadOnlyFixedValueList2<ManagedInterface> interfaces, in ReadOnlyFixedValueList4<ManagedField> fields, in ReadOnlyFixedValueList4<ManagedMethod> methods, in ReadOnlyFixedValueList4<ManagedProperty> properties, in ReadOnlyFixedValueList1<ManagedEvent> events, in ReadOnlyFixedValueList1<ManagedType> nestedTypes)
        {
            Assembly = assembly;
            DeclaringType = declaringType;
            Name = name;
            Attributes = attributes;
            GenericParameters = genericParameters;
            CustomAttributes = customAttributes;
            BaseType = baseType;
            Interfaces = interfaces;
            Fields = fields;
            Methods = methods;
            Properties = properties;
            Events = events;
            NestedTypes = nestedTypes;
        }

    }

}
