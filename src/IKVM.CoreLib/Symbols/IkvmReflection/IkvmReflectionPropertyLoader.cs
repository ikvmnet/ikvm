using System;
using System.Collections.Immutable;

using IKVM.Reflection;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionPropertyLoader : IPropertyLoader
    {

        readonly IkvmReflectionSymbolContext _context;
        readonly PropertyInfo _underlyingProperty;

        LazyField<AssemblySymbol> _assembly;
        LazyField<ModuleSymbol> _module;
        LazyField<TypeSymbol?> _declaringType;
        LazyField<TypeSymbol?> _propertyType;
        LazyField<MethodSymbol?> _getMethod;
        LazyField<MethodSymbol?> _setMethod;
        ImmutableArray<MethodSymbol> _accessors;
        ImmutableArray<ParameterSymbol> _indexParameters;
        ImmutableArray<TypeSymbol> _optionalCustomModifiers;
        ImmutableArray<TypeSymbol> _requiredCustomModifiers;
        ImmutableArray<CustomAttribute> _customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="underlyingProperty"></param>
        public IkvmReflectionPropertyLoader(IkvmReflectionSymbolContext context, PropertyInfo underlyingProperty)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _underlyingProperty = underlyingProperty ?? throw new ArgumentNullException(nameof(underlyingProperty));
        }

        /// <summary>
        /// Gets the underlying property.
        /// </summary>
        public PropertyInfo UnderlyingProperty => _underlyingProperty;

        /// <inheritdoc />
        public bool GetIsMissing() => false;

        /// <inheritdoc />
        public TypeSymbol GetDeclaringType() => _declaringType.IsDefault ? _declaringType.InterlockedInitialize(_context.ResolveTypeSymbol(_underlyingProperty.DeclaringType)) : _declaringType.Value;

        /// <inheritdoc />
        public global::System.Reflection.PropertyAttributes GetAttributes() => (global::System.Reflection.PropertyAttributes)_underlyingProperty.Attributes;

        /// <inheritdoc />
        public string GetName() => _underlyingProperty.Name;

        /// <inheritdoc />
        public TypeSymbol GetPropertyType() => _propertyType.IsDefault ? _propertyType.InterlockedInitialize(_context.ResolveTypeSymbol(_underlyingProperty.PropertyType)) : _propertyType.Value;

        /// <inheritdoc />
        public MethodSymbol? GetGetMethod() => _getMethod.IsDefault ? _getMethod.InterlockedInitialize(_context.ResolveMethodSymbol(_underlyingProperty.GetMethod)) : _getMethod.Value;

        /// <inheritdoc />
        public MethodSymbol? GetSetMethod() => _setMethod.IsDefault ? _setMethod.InterlockedInitialize(_context.ResolveMethodSymbol(_underlyingProperty.SetMethod)) : _setMethod.Value;

        /// <inheritdoc />
        public ImmutableArray<MethodSymbol> GetAccessors()
        {
            if (_accessors.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _accessors, _context.ResolveMethodSymbols(_underlyingProperty.GetAccessors(true)));

            return _accessors;
        }

        /// <inheritdoc />
        public ImmutableArray<ParameterSymbol> GetIndexParameters()
        {
            if (_indexParameters.IsDefault)
            {
                var l = _underlyingProperty.GetIndexParameters();
                var b = ImmutableArray.CreateBuilder<ParameterSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(new DefinitionParameterSymbol(_context, new IkvmReflectionParameterLoader(_context, i)));

                ImmutableInterlocked.InterlockedInitialize(ref _indexParameters, b.DrainToImmutable());
            }

            return _indexParameters;
        }

        /// <inheritdoc />
        public object? GetRawConstantValue()
        {
            return _underlyingProperty.GetRawConstantValue();
        }

        /// <inheritdoc />
        public TypeSymbol GetModifiedPropertyType()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
        {
            if (_optionalCustomModifiers.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _optionalCustomModifiers, _context.ResolveTypeSymbols(_underlyingProperty.GetOptionalCustomModifiers()));

            return _optionalCustomModifiers;
        }

        /// <inheritdoc />
        public ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
            if (_requiredCustomModifiers.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _requiredCustomModifiers, _context.ResolveTypeSymbols(_underlyingProperty.GetRequiredCustomModifiers()));

            return _requiredCustomModifiers;
        }

        /// <inheritdoc />
        public ImmutableArray<CustomAttribute> GetCustomAttributes()
        {
            if (_customAttributes.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, _context.ResolveCustomAttributes(_underlyingProperty.GetCustomAttributesData()));

            return _customAttributes;
        }

    }

}
