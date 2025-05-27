using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Emit
{

    public sealed class GenericTypeParameterTypeSymbolBuilder : GenericTypeParameterTypeSymbol, ICustomAttributeBuilder
    {

        readonly string _name;
        GenericParameterAttributes _attributes;
        readonly int _position;
        readonly ImmutableArray<CustomAttribute>.Builder _customAttributes = ImmutableArray.CreateBuilder<CustomAttribute>();
        TypeSymbol? _baseTypeConstraint;
        ImmutableArray<TypeSymbol> _interfaceConstraints = ImmutableArray<TypeSymbol>.Empty;

        bool _frozen;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal GenericTypeParameterTypeSymbolBuilder(SymbolContext context, TypeSymbolBuilder declaringType, string name, GenericParameterAttributes attributes, int position) :
            base(context, declaringType)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _attributes = attributes;
            _position = position;
        }

        /// <inheritdoc />
        public sealed override string Name => _name;

        /// <inheritdoc />
        public sealed override string? Namespace => "";

        /// <inheritdoc />
        public sealed override GenericParameterAttributes GenericParameterAttributes => _attributes;

        /// <inheritdoc />
        public sealed override int GenericParameterPosition => _position;

        /// <inheritdoc />
        public override TypeSymbol? BaseType => _baseTypeConstraint;

        /// <inheritdoc />
        internal override ImmutableArray<TypeSymbol> GetDeclaredInterfaces()
        {
            return _interfaceConstraints;
        }

        /// <inheritdoc />
        internal override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            return _customAttributes.ToImmutable();
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
        {
            return [];
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
            return [];
        }

        /// <summary>
        /// Freezes the type builder.
        /// </summary>
        internal void Freeze()
        {
            _frozen = true;
        }

        /// <summary>
        /// Throws an exception if the builder is frozen.
        /// </summary>
        void ThrowIfFrozen()
        {
            if (_frozen)
                throw new InvalidOperationException("GenericTypeParameterTypeSymbolBuilder is frozen.");
        }

        /// <summary>
        /// Sets the variance characteristics and special constraints of the generic parameter, such as the parameterless constructor constraint.
        /// </summary>
        /// <param name="genericParameterAttributes"></param>
        public void SetGenericParameterAttributes(GenericParameterAttributes genericParameterAttributes)
        {
            ThrowIfFrozen();
            _attributes = genericParameterAttributes;
        }

        /// <summary>
        /// Sets the base type that a type must inherit in order to be substituted for the type parameter.
        /// </summary>
        /// <param name="baseTypeConstraint"></param>
        public void SetBaseTypeConstraint(TypeSymbol? baseTypeConstraint)
        {
            ThrowIfFrozen();
            _baseTypeConstraint = baseTypeConstraint;
            ClearGenericParameterConstraints();
        }

        /// <summary>
        /// Sets the interfaces a type must implement in order to be substituted for the type parameter.
        /// </summary>
        /// <param name="interfaceConstraints"></param>
        public void SetInterfaceConstraints(ImmutableArray<TypeSymbol> interfaceConstraints)
        {
            ThrowIfFrozen();
            _interfaceConstraints = interfaceConstraints;
            ClearGenericParameterConstraints();
        }

        /// <inheritdoc />
        public void SetCustomAttribute(CustomAttribute attribute)
        {
            ThrowIfFrozen();
            _customAttributes.Add(attribute);
        }

    }

}
