using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionPropertySymbol : PropertySymbol
    {

        readonly PropertyInfo _underlyingProperty;

        TypeSymbol? _propertyType;
        MethodSymbol? _getMethod;
        MethodSymbol? _nonPublicGetMethod;
        MethodSymbol? _setMethod;
        MethodSymbol? _nonPublicSetMethod;
        ImmutableArray<MethodSymbol> _accessors;
        ImmutableArray<MethodSymbol> _nonPublicAccessors;
        ImmutableArray<ParameterSymbol> _indexParameters;
        ImmutableArray<TypeSymbol> _optionalCustomModifiers;
        ImmutableArray<TypeSymbol> _requiredCustomModifiers;
        ImmutableArray<CustomAttribute> _customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="underlyingProperty"></param>
        public ReflectionPropertySymbol(ReflectionSymbolContext context, ReflectionTypeSymbol declaringType, PropertyInfo underlyingProperty) :
            base(context, declaringType)
        {
            _underlyingProperty = underlyingProperty ?? throw new ArgumentNullException(nameof(underlyingProperty));
        }

        /// <summary>
        /// Gets the context that owns this parameter.
        /// </summary>
        new ReflectionSymbolContext Context => (ReflectionSymbolContext)base.Context;

        /// <inheritdoc />
        public override PropertyAttributes Attributes => _underlyingProperty.Attributes;

        /// <inheritdoc />
        public override TypeSymbol PropertyType => _propertyType ??= Context.ResolveTypeSymbol(_underlyingProperty.PropertyType);

        /// <inheritdoc />
        public override bool CanRead => _underlyingProperty.CanRead;

        /// <inheritdoc />
        public override bool CanWrite => _underlyingProperty.CanWrite;

        /// <inheritdoc />
        public override string Name => _underlyingProperty.Name;

        /// <inheritdoc />
        public override bool IsMissing => false;

        /// <inheritdoc />
        public override bool ContainsMissing => false;

        /// <inheritdoc />
        public override bool IsComplete => false;

        /// <inheritdoc />
        public override MethodSymbol? GetGetMethod(bool nonPublic)
        {
            if (nonPublic)
                return _nonPublicGetMethod ??= Context.ResolveMethodSymbol(_underlyingProperty.GetGetMethod(true));
            else
                return _getMethod ??= Context.ResolveMethodSymbol(_underlyingProperty.GetGetMethod(false));
        }

        /// <inheritdoc />
        public override MethodSymbol? GetSetMethod(bool nonPublic)
        {
            if (nonPublic)
                return _nonPublicSetMethod ??= Context.ResolveMethodSymbol(_underlyingProperty.GetSetMethod(true));
            else
                return _setMethod ??= Context.ResolveMethodSymbol(_underlyingProperty.GetSetMethod(false));
        }

        /// <inheritdoc />
        public override ImmutableArray<MethodSymbol> GetAccessors(bool nonPublic)
        {
            if (nonPublic)
            {
                if (_nonPublicAccessors == default)
                    ImmutableInterlocked.InterlockedInitialize(ref _nonPublicAccessors, Context.ResolveMethodSymbols(_underlyingProperty.GetAccessors(true)));

                return _nonPublicAccessors;
            }
            else
            {
                if (_accessors == default)
                    ImmutableInterlocked.InterlockedInitialize(ref _accessors, Context.ResolveMethodSymbols(_underlyingProperty.GetAccessors(false)));

                return _accessors;
            }
        }

        /// <inheritdoc />
        public override ImmutableArray<ParameterSymbol> GetIndexParameters()
        {
            if (_indexParameters == default)
            {
                var l = _underlyingProperty.GetIndexParameters();
                var b = ImmutableArray.CreateBuilder<ParameterSymbol>();
                for (int i = 0; i < l.Length; i++)
                    b.Add(new ReflectionParameterSymbol(Context, this, l[i]));

                ImmutableInterlocked.InterlockedInitialize(ref _indexParameters, b.ToImmutable());
            }

            return _indexParameters;
        }

        /// <inheritdoc />
        public override object? GetRawConstantValue()
        {
            return _underlyingProperty.GetRawConstantValue();
        }

        /// <inheritdoc />
        public override TypeSymbol GetModifiedPropertyType()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
        {
            if (_optionalCustomModifiers == default)
                ImmutableInterlocked.InterlockedInitialize(ref _optionalCustomModifiers, Context.ResolveTypeSymbols(_underlyingProperty.GetOptionalCustomModifiers()));

            return _optionalCustomModifiers;
        }

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
            if (_requiredCustomModifiers == default)
                ImmutableInterlocked.InterlockedInitialize(ref _requiredCustomModifiers, Context.ResolveTypeSymbols(_underlyingProperty.GetRequiredCustomModifiers()));

            return _requiredCustomModifiers;
        }

        /// <inheritdoc />
        internal override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            if (_customAttributes == default)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, Context.ResolveCustomAttributes(_underlyingProperty.GetCustomAttributesData()));

            return _customAttributes;
        }

    }

}
