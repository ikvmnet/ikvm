using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Emit
{

    public sealed class GenericMethodParameterTypeSymbolBuilder : DefinitionGenericMethodParameterTypeSymbol, ICustomAttributeBuilder
    {

        readonly MethodSymbolBuilder _declaringMethod;
        readonly string _name;
        GenericParameterAttributes _attributes;
        readonly int _position;
        readonly ImmutableArray<CustomAttribute>.Builder _customAttributes = ImmutableArray.CreateBuilder<CustomAttribute>();
        TypeSymbol? _baseTypeConstraint;
        ImmutableArray<TypeSymbol> _interfaceConstraints = [];
        ImmutableArray<TypeSymbol> _constraints;

        bool _frozen;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringMethod"></param>
        /// <exception cref="ArgumentNullException"></exception>
        internal GenericMethodParameterTypeSymbolBuilder(SymbolContext context, MethodSymbolBuilder declaringMethod, string name, GenericParameterAttributes attributes, int position) :
            base(context)
        {
            _declaringMethod = declaringMethod ?? throw new ArgumentNullException(nameof(declaringMethod));
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _attributes = attributes;
            _position = position;
        }

        /// <inheritdoc />
        public sealed override MethodSymbol? DeclaringMethod => _declaringMethod;

        /// <inheritdoc />
        public sealed override string Name => _name;

        /// <inheritdoc />
        public sealed override string? Namespace => "";

        /// <inheritdoc />
        public sealed override GenericParameterAttributes GenericParameterAttributes => _attributes;

        /// <inheritdoc />
        public sealed override int GenericParameterPosition => _position;

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GenericParameterConstraints
        {
            get
            {
                if (_constraints.IsDefault)
                {
                    var n = _baseTypeConstraint != null ? 1 : 0;
                    var l = ImmutableArray.CreateBuilder<TypeSymbol>(n + _interfaceConstraints.Length);
                    if (_baseTypeConstraint != null)
                        l.Add(_baseTypeConstraint);

                    foreach (var i in _interfaceConstraints)
                        l.Add(i);

                    ImmutableInterlocked.InterlockedInitialize(ref _constraints, l.ToImmutable());
                }

                return _constraints;
            }
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers() => [];

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers() => [];

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes() => _customAttributes.ToImmutable();

        /// <summary>
        /// Freezes the type builder.
        /// </summary>
        internal void Freeze()
        {
            lock (this)
                _frozen = true;
        }

        /// <summary>
        /// Throws an exception if the builder is frozen.
        /// </summary>
        void ThrowIfFrozen()
        {
            lock (this)
                if (_frozen)
                    throw new InvalidOperationException("GenericMethodParameterTypeSymbolBuilder is frozen.");
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
            _constraints = default;
        }

        /// <summary>
        /// Sets the interfaces a type must implement in order to be substituted for the type parameter.
        /// </summary>
        /// <param name="interfaceConstraints"></param>
        public void SetInterfaceConstraints(ImmutableArray<TypeSymbol> interfaceConstraints)
        {
            ThrowIfFrozen();
            _interfaceConstraints = interfaceConstraints;
            _constraints = default;
        }

        /// <inheritdoc />
        public void SetCustomAttribute(CustomAttribute attribute)
        {
            ThrowIfFrozen();
            _customAttributes.Add(attribute);
        }

    }

}
