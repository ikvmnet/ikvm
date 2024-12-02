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

        /// <summary>
        /// Gets the underlying object.
        /// </summary>
        internal PropertyInfo UnderlyingProperty => _underlyingProperty;

        /// <inheritdoc />
        public sealed override PropertyAttributes Attributes => _underlyingProperty.Attributes;

        /// <inheritdoc />
        public sealed override TypeSymbol PropertyType => _propertyType ??= Context.ResolveTypeSymbol(_underlyingProperty.PropertyType);

        /// <inheritdoc />
        public sealed override bool CanRead => _underlyingProperty.CanRead;

        /// <inheritdoc />
        public sealed override bool CanWrite => _underlyingProperty.CanWrite;

        /// <inheritdoc />
        public sealed override string Name => _underlyingProperty.Name;

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override MethodSymbol? GetGetMethod(bool nonPublic)
        {
            if (nonPublic)
                return _nonPublicGetMethod ??= Context.ResolveMethodSymbol(_underlyingProperty.GetGetMethod(true));
            else
                return _getMethod ??= Context.ResolveMethodSymbol(_underlyingProperty.GetGetMethod(false));
        }

        /// <inheritdoc />
        public sealed override MethodSymbol? GetSetMethod(bool nonPublic)
        {
            if (nonPublic)
                return _nonPublicSetMethod ??= Context.ResolveMethodSymbol(_underlyingProperty.GetSetMethod(true));
            else
                return _setMethod ??= Context.ResolveMethodSymbol(_underlyingProperty.GetSetMethod(false));
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<MethodSymbol> GetAccessors(bool nonPublic)
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
        public sealed override ImmutableArray<ParameterSymbol> GetIndexParameters()
        {
            if (_indexParameters == default)
            {
                var l = _underlyingProperty.GetIndexParameters();
                var b = ImmutableArray.CreateBuilder<ParameterSymbol>(l.Length);
                for (int i = 0; i < l.Length; i++)
                    b.Add(new ReflectionParameterSymbol(Context, this, l[i]));

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
            if (_optionalCustomModifiers == default)
                ImmutableInterlocked.InterlockedInitialize(ref _optionalCustomModifiers, Context.ResolveTypeSymbols(_underlyingProperty.GetOptionalCustomModifiers()));

            return _optionalCustomModifiers;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
            if (_requiredCustomModifiers == default)
                ImmutableInterlocked.InterlockedInitialize(ref _requiredCustomModifiers, Context.ResolveTypeSymbols(_underlyingProperty.GetRequiredCustomModifiers()));

            return _requiredCustomModifiers;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            if (_customAttributes == default)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, Context.ResolveCustomAttributes(_underlyingProperty.GetCustomAttributesData()));

            return _customAttributes;
        }

    }

}
