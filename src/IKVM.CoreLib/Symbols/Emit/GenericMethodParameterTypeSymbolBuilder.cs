using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Emit
{

    public sealed class GenericMethodParameterTypeSymbolBuilder : GenericMethodParameterTypeSymbol, ICustomAttributeBuilder
    {

        readonly string _name;
        GenericParameterAttributes _attributes;
        readonly int _position;
        readonly ImmutableArray<CustomAttribute>.Builder _customAttributes = ImmutableArray.CreateBuilder<CustomAttribute>();
        TypeSymbol? _baseTypeConstraint;
        ImmutableArray<TypeSymbol> _interfaceConstraints = ImmutableArray<TypeSymbol>.Empty;
        ImmutableArray<TypeSymbol> _constraints;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringMethod"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public GenericMethodParameterTypeSymbolBuilder(SymbolContext context, MethodSymbolBuilder declaringMethod, string name, GenericParameterAttributes attributes, int position) :
            base(context, declaringMethod)
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
        public sealed override bool IsComplete => throw new NotImplementedException();

        /// <inheritdoc />
        internal sealed override ImmutableArray<TypeSymbol> GetDeclaredInterfaces()
        {
            return _interfaceConstraints;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
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
        /// Sets the variance characteristics and special constraints of the generic parameter, such as the parameterless constructor constraint.
        /// </summary>
        /// <param name="genericParameterAttributes"></param>
        public void SetGenericParameterAttributes(GenericParameterAttributes genericParameterAttributes)
        {
            _attributes = genericParameterAttributes;
        }

        /// <summary>
        /// Sets the base type that a type must inherit in order to be substituted for the type parameter.
        /// </summary>
        /// <param name="baseTypeConstraint"></param>
        public void SetBaseTypeConstraint(TypeSymbol? baseTypeConstraint)
        {
            _baseTypeConstraint = baseTypeConstraint;
            ClearGenericParameterConstraints();
        }

        /// <summary>
        /// Sets the interfaces a type must implement in order to be substituted for the type parameter.
        /// </summary>
        /// <param name="interfaceConstraints"></param>
        public void SetInterfaceConstraints(ImmutableArray<TypeSymbol> interfaceConstraints)
        {
            _interfaceConstraints = interfaceConstraints;
            ClearGenericParameterConstraints();
        }

        /// <inheritdoc />
        public void SetCustomAttribute(CustomAttribute attribute)
        {
            _customAttributes.Add(attribute);
        }

    }

}
