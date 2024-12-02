using System;
using System.Collections.Immutable;
using System.Reflection;

using IKVM.CoreLib.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionGenericTypeParameterTypeSymbol : GenericTypeParameterTypeSymbol
    {

        readonly Type _underlyingType;

        ImmutableArray<TypeSymbol> _interfaces;
        ImmutableArray<CustomAttribute> _customAttributes;
#if NET8_0_OR_GREATER
        ImmutableArray<TypeSymbol> _optionalCustomModifiers;
        ImmutableArray<TypeSymbol> _requiredCustomModifiers;
#endif

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionGenericTypeParameterTypeSymbol(ReflectionSymbolContext context, TypeSymbol declaringType, Type underlyingType) :
            base(context, declaringType)
        {
            _underlyingType = underlyingType ?? throw new ArgumentNullException(nameof(underlyingType));
        }

        /// <summary>
        /// Gets the context that owns this symbol.
        /// </summary>
        new ReflectionSymbolContext Context => (ReflectionSymbolContext)base.Context;

        /// <inheritdoc />
        public sealed override string Name => _underlyingType.Name;

        /// <inheritdoc />
        public sealed override string? Namespace => _underlyingType.Namespace;

        /// <inheritdoc />
        public sealed override GenericParameterAttributes GenericParameterAttributes => _underlyingType.GenericParameterAttributes;

        /// <inheritdoc />
        public sealed override int GenericParameterPosition => _underlyingType.GenericParameterPosition;

        /// <inheritdoc />
        public sealed override TypeSymbol? BaseType => Context.ResolveTypeSymbol(_underlyingType.BaseType);

        /// <inheritdoc />
        internal sealed override ImmutableArray<TypeSymbol> GetDeclaredInterfaces()
        {
            if (_interfaces.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _interfaces, Context.ResolveTypeSymbols(_underlyingType.GetDeclaredInterfaces()));

            return _interfaces;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            if (_customAttributes.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, Context.ResolveCustomAttributes(_underlyingType.GetCustomAttributesData()));

            return _customAttributes;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
        {
#if NET8_0_OR_GREATER
            if (_optionalCustomModifiers.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _optionalCustomModifiers, Context.ResolveTypeSymbols(_underlyingType.GetOptionalCustomModifiers()));

            return _optionalCustomModifiers;
#else
            return [];
#endif
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
#if NET8_0_OR_GREATER
            if (_requiredCustomModifiers.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _requiredCustomModifiers, Context.ResolveTypeSymbols(_underlyingType.GetRequiredCustomModifiers()));

            return _requiredCustomModifiers;
#else
            return [];
#endif
        }

    }

}
