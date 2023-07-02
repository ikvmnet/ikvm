using System;
using System.Reflection;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a managed type.
    /// </summary>
    internal class ManagedType
    {

        readonly Func<ReadOnlyValueList<ManagedType>> getNestedTypes;
        ReadOnlyValueList<ManagedType>? nestedTypes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="declaringType"></param>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="customAttributes"></param>
        /// <param name="fields"></param>
        /// <param name="methods"></param>
        /// <param name="nestedTypes"></param>
        public ManagedType(ManagedType? declaringType, string name, TypeAttributes attributes, ReadOnlyValueList<ManagedCustomAttribute> customAttributes, ReadOnlyValueList<ManagedField> fields, ReadOnlyValueList<ManagedMethod> methods, Func<ReadOnlyValueList<ManagedType>> getNestedTypes)
        {
            DeclaringType = declaringType;
            Name = name;
            Attributes = attributes;
            CustomAttributes = customAttributes;
            Fields = fields;
            Methods = methods;

            this.getNestedTypes = getNestedTypes;
        }

        /// <summary>
        /// Gets the parent type of this type.
        /// </summary>
        public ManagedTypeReference? DeclaringType { get; }

        /// <summary>
        /// Gets the name of the managed type.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the attributes for the type.
        /// </summary>
        public TypeAttributes Attributes { get; }

        /// <summary>
        /// Gets the set of custom attributes applied to the type.
        /// </summary>
        public ReadOnlyValueList<ManagedCustomAttribute> CustomAttributes { get; }

        /// <summary>
        /// Gets the set of fields declared on the managed type.
        /// </summary>
        public ReadOnlyValueList<ManagedField> Fields { get; }

        /// <summary>
        /// Gets the set of methods declared on the managed type.
        /// </summary>
        public ReadOnlyValueList<ManagedMethod> Methods { get; }

        /// <summary>
        /// Gets the set of nested types within the managed type.
        /// </summary>
        public ReadOnlyValueList<ManagedType> NestedTypes => nestedTypes ??= getNestedTypes();

    }

}
