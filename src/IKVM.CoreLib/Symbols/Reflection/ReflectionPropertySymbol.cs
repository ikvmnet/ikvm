using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    sealed class ReflectionPropertySymbol : DefinitionPropertySymbol
    {

        readonly ReflectionSymbolContext _context;
        readonly PropertyInfo _underlyingProperty;

        LazyField<TypeSymbol?> _declaringType;
        LazyField<TypeSymbol?> _propertyType;
        LazyField<MethodSymbol?> _getMethod;
        LazyField<MethodSymbol?> _setMethod;
        ImmutableArray<ParameterSymbol> _indexParameters;
        ImmutableArray<TypeSymbol> _optionalCustomModifiers;
        ImmutableArray<TypeSymbol> _requiredCustomModifiers;
        ImmutableArray<CustomAttribute> _customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="underlyingProperty"></param>
        public ReflectionPropertySymbol(ReflectionSymbolContext context, PropertyInfo underlyingProperty) :
            base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _underlyingProperty = underlyingProperty ?? throw new ArgumentNullException(nameof(underlyingProperty));
        }

        /// <summary>
        /// Gets the underlying property.
        /// </summary>
        public PropertyInfo UnderlyingProperty => _underlyingProperty;

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override TypeSymbol DeclaringType => _declaringType.IsDefault ? _declaringType.InterlockedInitialize(_context.ResolveTypeSymbol(_underlyingProperty.DeclaringType)) : _declaringType.Value;

        /// <inheritdoc />
        public sealed override PropertyAttributes Attributes => _underlyingProperty.Attributes;

        /// <inheritdoc />
        public sealed override string Name => _underlyingProperty.Name;

        /// <inheritdoc />
        public sealed override TypeSymbol PropertyType => _propertyType.IsDefault ? _propertyType.InterlockedInitialize(_context.ResolveTypeSymbol(_underlyingProperty.PropertyType)) : _propertyType.Value;

        /// <inheritdoc />
        public sealed override MethodSymbol? GetMethod => _getMethod.IsDefault ? _getMethod.InterlockedInitialize(_context.ResolveMethodSymbol(_underlyingProperty.GetMethod)) : _getMethod.Value;

        /// <inheritdoc />
        public sealed override MethodSymbol? SetMethod => _setMethod.IsDefault ? _setMethod.InterlockedInitialize(_context.ResolveMethodSymbol(_underlyingProperty.SetMethod)) : _setMethod.Value;

        /// <inheritdoc />
        public sealed override ImmutableArray<ParameterSymbol> GetIndexParameters()
        {
            if (_indexParameters.IsDefault)
            {
                var l = _underlyingProperty.GetIndexParameters();
                var b = ImmutableArray.CreateBuilder<ParameterSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(new ReflectionParameterSymbol(_context, i));

                ImmutableInterlocked.InterlockedInitialize(ref _indexParameters, b.DrainToImmutable());
            }

            return _indexParameters;
        }

        /// <inheritdoc />
        public sealed override object? GetRawConstantValue()
        {
            return _underlyingProperty.GetRawConstantValue();
        }

        /// <inheritdoc />
        public sealed override TypeSymbol GetModifiedPropertyType()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
        {
            if (_optionalCustomModifiers.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _optionalCustomModifiers, _context.ResolveTypeSymbols(_underlyingProperty.GetOptionalCustomModifiers()));

            return _optionalCustomModifiers;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
            if (_requiredCustomModifiers.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _requiredCustomModifiers, _context.ResolveTypeSymbols(_underlyingProperty.GetRequiredCustomModifiers()));

            return _requiredCustomModifiers;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            if (_customAttributes.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, _context.ResolveCustomAttributes(_underlyingProperty.GetCustomAttributesData()));

            return _customAttributes;
        }

    }

}
