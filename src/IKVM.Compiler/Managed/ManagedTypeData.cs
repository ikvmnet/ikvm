using System.Reflection;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Holds the data of a managed type.
    /// </summary>
    public readonly struct ManagedTypeData
    {

        readonly ManagedAssembly assembly;
        readonly ManagedType? declaringType;
        readonly string name;
        readonly TypeAttributes attributes;
        readonly ReadOnlyFixedValueList<ManagedGenericParameter> genericParameters;
        readonly ReadOnlyFixedValueList<ManagedCustomAttribute> customAttributes;
        readonly ManagedTypeSignature? baseType;
        readonly ReadOnlyFixedValueList<ManagedInterface> interfaces;
        readonly ReadOnlyFixedValueList<ManagedField> fields;
        readonly ReadOnlyFixedValueList<ManagedMethod> methods;
        readonly ReadOnlyFixedValueList<ManagedProperty> properties;
        readonly ReadOnlyFixedValueList<ManagedEvent> events;
        readonly ReadOnlyFixedValueList<ManagedType> nestedTypes;

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
        public ManagedTypeData(ManagedAssembly assembly, ManagedType? declaringType, string name, TypeAttributes attributes, in ReadOnlyFixedValueList<ManagedGenericParameter> genericParameters, in ReadOnlyFixedValueList<ManagedCustomAttribute> customAttributes, in ManagedTypeSignature? baseType, in ReadOnlyFixedValueList<ManagedInterface> interfaces, in ReadOnlyFixedValueList<ManagedField> fields, in ReadOnlyFixedValueList<ManagedMethod> methods, in ReadOnlyFixedValueList<ManagedProperty> properties, in ReadOnlyFixedValueList<ManagedEvent> events,in ReadOnlyFixedValueList<ManagedType> nestedTypes)
        {
            this.assembly = assembly;
            this.declaringType = declaringType;
            this.name = name;
            this.attributes = attributes;
            this.genericParameters = genericParameters;
            this.customAttributes = customAttributes;
            this.baseType = baseType;
            this.interfaces = interfaces;
            this.fields = fields;
            this.methods = methods;
            this.properties = properties;
            this.events = events;
            this.nestedTypes = nestedTypes;
        }


        /// <summary>
        /// Gets the parent assembly of this type.
        /// </summary>
        public ManagedAssembly Assembly => assembly;

        /// <summary>
        /// Gets the parent type of this type.
        /// </summary>
        public ManagedType? DeclaringType => declaringType;

        /// <summary>
        /// Gets the name of the managed type.
        /// </summary>
        public string Name => name;

        /// <summary>
        /// Gets the attributes for the type.
        /// </summary>
        public TypeAttributes Attributes => attributes;

        /// <summary>
        /// Gets the generic parameters on the managed type.
        /// </summary>
        public readonly ReadOnlyFixedValueList<ManagedGenericParameter> GenericParameters => genericParameters;

        /// <summary>
        /// Gets the set of custom attributes applied to the type.
        /// </summary>
        public readonly ReadOnlyFixedValueList<ManagedCustomAttribute> CustomAttributes => customAttributes;

        /// <summary>
        /// Gets a reference to the base type.
        /// </summary>
        public readonly ManagedTypeSignature? BaseType => baseType;

        /// <summary>
        /// Gets the set of interfaces implemented on the managed type.
        /// </summary>
        public readonly ReadOnlyFixedValueList<ManagedInterface> Interfaces => interfaces;

        /// <summary>
        /// Gets the set of fields declared on the managed type.
        /// </summary>
        public readonly ReadOnlyFixedValueList<ManagedField> Fields => fields;

        /// <summary>
        /// Gets the set of methods declared on the managed type.
        /// </summary>
        public readonly ReadOnlyFixedValueList<ManagedMethod> Methods => methods;

        /// <summary>
        /// Gets the set of properties declared on the managed type.
        /// </summary>
        public readonly ReadOnlyFixedValueList<ManagedProperty> Properties => properties;

        /// <summary>
        /// Gets the set of fields declared on the managed type.
        /// </summary>
        public readonly ReadOnlyFixedValueList<ManagedEvent> Events => events;

        /// <summary>
        /// Gets the set of nested types within the managed type.
        /// </summary>
        public readonly ReadOnlyFixedValueList<ManagedType> NestedTypes => nestedTypes;

    }

}
