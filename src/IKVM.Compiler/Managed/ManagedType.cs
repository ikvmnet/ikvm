using System.Collections.Generic;
using System.Reflection;

using IKVM.Compiler.Collections;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a managed type.
    /// </summary>
    public sealed class ManagedType
    {

        readonly IManagedTypeContext context;
        readonly ManagedAssembly assembly;
        readonly ManagedType? declaringType;
        readonly string name;
        readonly TypeAttributes attributes;
        readonly ReadOnlyFixedValueList<ManagedCustomAttribute> customAttributes;
        readonly ReadOnlyFixedValueList<ManagedGenericParameter> genericParameters;
        readonly ManagedTypeRef? baseType;
        readonly ReadOnlyFixedValueList<ManagedInterface> interfaces;
        readonly ReadOnlyFixedValueList<ManagedField> fields;
        readonly ReadOnlyFixedValueList<ManagedMethod> methods;

        IEnumerable<ManagedType>? nestedTypes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assembly"></param>
        public ManagedType(IManagedTypeContext context, ManagedAssembly assembly)
        {
            this.context = context;
            this.assembly = assembly;
        }

        /// <summary>
        /// Provides the context responsible for loading this type.
        /// </summary>
        public IManagedTypeContext Context => context;

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
        /// Gets the set of custom attributes applied to the type.
        /// </summary>
        public ref readonly ReadOnlyFixedValueList<ManagedCustomAttribute> CustomAttributes => ref customAttributes;

        /// <summary>
        /// Gets the generic parameters on the managed type.
        /// </summary>
        public ref readonly ReadOnlyFixedValueList<ManagedGenericParameter> GenericParameters => ref genericParameters;

        /// <summary>
        /// Gets a reference to the base type.
        /// </summary>
        public ManagedTypeRef? BaseType => baseType;

        /// <summary>
        /// Gets the set of interfaces implemented on the managed type.
        /// </summary>
        public ref readonly ReadOnlyFixedValueList<ManagedInterface> Interfaces => ref interfaces;

        /// <summary>
        /// Gets the set of fields declared on the managed type.
        /// </summary>
        public ref readonly ReadOnlyFixedValueList<ManagedField> Fields => ref fields;

        /// <summary>
        /// Gets the set of methods declared on the managed type.
        /// </summary>
        public ref readonly ReadOnlyFixedValueList<ManagedMethod> Methods => ref methods;

        /// <summary>
        /// Gets the set of nested types within the managed type.
        /// </summary>
        public ref readonly ReadOnlyFixedValueList<ManagedType> NestedTypes => nestedTypes;

        /// <inhericdoc />
        public override string ToString() => name;

    }

}
