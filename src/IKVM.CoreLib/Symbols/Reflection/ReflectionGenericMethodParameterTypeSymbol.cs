using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionGenericMethodParameterTypeSymbol : GenericMethodParameterTypeSymbol
    {

        readonly Type _underlyingType;

        ImmutableArray<TypeSymbol> _interfaces;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringMethod"></param>
        /// <param name="underlyingType"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionGenericMethodParameterTypeSymbol(ReflectionSymbolContext context, MethodSymbol declaringMethod, Type underlyingType) :
            base(context, declaringMethod)
        {
            _underlyingType = underlyingType ?? throw new ArgumentNullException(nameof(underlyingType));
        }

        /// <summary>
        /// Gets the context that owns this symbol.
        /// </summary>
        new ReflectionSymbolContext Context => (ReflectionSymbolContext)base.Context;

        /// <inheritdoc />
        public override string? FullName => _underlyingType.FullName;

        /// <inheritdoc />
        public override string? Namespace => _underlyingType.Namespace;

        /// <inheritdoc />
        public override GenericParameterAttributes GenericParameterAttributes => _underlyingType.GenericParameterAttributes;

        /// <inheritdoc />
        public override int GenericParameterPosition => _underlyingType.GenericParameterPosition;

        /// <inheritdoc />
        public override string Name => _underlyingType.Name;

        /// <inheritdoc />
        public override bool IsComplete => true;

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetInterfaces()
        {
            if (_interfaces == null)
                ImmutableInterlocked.InterlockedInitialize(ref _interfaces, Context.ResolveTypeSymbols(_underlyingType.GetInterfaces()));

            return _interfaces;
        }

    }

}
