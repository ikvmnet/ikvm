using System;
using System.Collections.Immutable;
using System.Reflection;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionGenericTypeParameterTypeSymbol : GenericTypeParameterTypeSymbol
    {

        readonly Type _underlyingType;

        ImmutableArray<TypeSymbol> _interfaces;
        ImmutableArray<CustomAttribute> _customAttributes;
        ImmutableArray<TypeSymbol> _optionalCustomModifiers;
        ImmutableArray<TypeSymbol> _requiredCustomModifiers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReflectionGenericTypeParameterTypeSymbol(IkvmReflectionSymbolContext context, TypeSymbol declaringType, Type underlyingType) :
            base(context, declaringType)
        {
            _underlyingType = underlyingType ?? throw new ArgumentNullException(nameof(underlyingType));
        }

        /// <summary>
        /// Gets the context that owns this symbol.
        /// </summary>
        new IkvmReflectionSymbolContext Context => (IkvmReflectionSymbolContext)base.Context;

        /// <inheritdoc />
        public sealed override string Name => _underlyingType.Name;

        /// <inheritdoc />
        public sealed override string? Namespace => _underlyingType.Namespace;

        /// <inheritdoc />
        public sealed override GenericParameterAttributes GenericParameterAttributes => (GenericParameterAttributes)_underlyingType.GenericParameterAttributes;

        /// <inheritdoc />
        public sealed override int GenericParameterPosition => _underlyingType.GenericParameterPosition;

        /// <inheritdoc />
        public sealed override TypeSymbol? BaseType => Context.ResolveTypeSymbol(_underlyingType.BaseType);

        /// <inheritdoc />
        internal sealed override ImmutableArray<TypeSymbol> GetDeclaredInterfaces()
        {
            if (_interfaces == null)
                ImmutableInterlocked.InterlockedInitialize(ref _interfaces, Context.ResolveTypeSymbols(_underlyingType.__GetDeclaredInterfaces()));

            return _interfaces;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            if (_customAttributes == default)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, Context.ResolveCustomAttributes(_underlyingType.GetCustomAttributesData()));

            return _customAttributes;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
        {
            if (_optionalCustomModifiers .IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _optionalCustomModifiers, Context.ResolveTypeSymbols(_underlyingType.__GetOptionalCustomModifiers()));

            return _optionalCustomModifiers;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
            if (_requiredCustomModifiers.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _requiredCustomModifiers, Context.ResolveTypeSymbols(_underlyingType.__GetRequiredCustomModifiers()));

            return _requiredCustomModifiers;
        }

    }

}
