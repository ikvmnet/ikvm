using System;
using System.Collections.Immutable;

using IKVM.Reflection;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    sealed class IkvmReflectionGenericMethodParameterTypeSymbol : DefinitionGenericMethodParameterTypeSymbol
    {

        readonly IkvmReflectionSymbolContext _context;
        readonly Type _underlyingType;

        LazyField<MethodSymbol> _declaringMethod;
        ImmutableArray<TypeSymbol> _genericParameterConstraints;
        ImmutableArray<TypeSymbol> _optionalCustomModifiers;
        ImmutableArray<TypeSymbol> _requiredCustomModifiers;
        ImmutableArray<CustomAttribute> _customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="underlyingType"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReflectionGenericMethodParameterTypeSymbol(IkvmReflectionSymbolContext context, Type underlyingType) :
            base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _underlyingType = underlyingType ?? throw new ArgumentNullException(nameof(underlyingType));

            if (underlyingType.IsGenericParameter == false || underlyingType.IsGenericMethodParameter == false)
                throw new ArgumentException(nameof(underlyingType));
        }

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override MethodSymbol DeclaringMethod => _declaringMethod.IsDefault ? _declaringMethod.InterlockedInitialize(_context.ResolveMethodSymbol((MethodInfo)_underlyingType.DeclaringMethod!)) : _declaringMethod.Value;

        /// <inheritdoc />
        public sealed override string Name => _underlyingType.Name;

        /// <inheritdoc />
        public sealed override int GenericParameterPosition => _underlyingType.GenericParameterPosition;

        /// <inheritdoc />
        public sealed override global::System.Reflection.GenericParameterAttributes GenericParameterAttributes => (global::System.Reflection.GenericParameterAttributes)_underlyingType.GenericParameterAttributes;

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GenericParameterConstraints
        {
            get
            {
                if (_genericParameterConstraints.IsDefault)
                {
                    var l = _underlyingType.GetGenericParameterConstraints();
                    var b = ImmutableArray.CreateBuilder<TypeSymbol>(l.Length);
                    foreach (var i in l)
                        b.Add(_context.ResolveTypeSymbol(i));

                    ImmutableInterlocked.InterlockedInitialize(ref _genericParameterConstraints, b.DrainToImmutable());
                }

                return _genericParameterConstraints;
            }
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
        {
            if (_optionalCustomModifiers.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _optionalCustomModifiers, _context.ResolveTypeSymbols(_underlyingType.__GetOptionalCustomModifiers()));

            return _optionalCustomModifiers;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
            if (_requiredCustomModifiers.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _requiredCustomModifiers, _context.ResolveTypeSymbols(_underlyingType.__GetRequiredCustomModifiers()));

            return _requiredCustomModifiers;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            if (_customAttributes.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, _context.ResolveCustomAttributes(_underlyingType.GetCustomAttributesData()));

            return _customAttributes;
        }

    }

}
