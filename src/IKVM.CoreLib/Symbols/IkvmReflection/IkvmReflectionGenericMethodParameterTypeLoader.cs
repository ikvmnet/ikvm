using System;
using System.Collections.Immutable;

using IKVM.Reflection;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionGenericMethodParameterTypeLoader : IGenericMethodParameterTypeLoader
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
        public IkvmReflectionGenericMethodParameterTypeLoader(IkvmReflectionSymbolContext context, Type underlyingType)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _underlyingType = underlyingType ?? throw new ArgumentNullException(nameof(underlyingType));

            if (underlyingType.IsGenericParameter == false || underlyingType.IsGenericMethodParameter() == false)
                throw new ArgumentException(nameof(underlyingType));
        }

        /// <inheritdoc />
        public bool GetIsMissing() => false;

        /// <inheritdoc />
        public MethodSymbol GetDeclaringMethod() => _declaringMethod.IsDefault ? _declaringMethod.InterlockedInitialize(_context.ResolveMethodSymbol((MethodInfo)_underlyingType.DeclaringMethod!)) : _declaringMethod.Value;

        /// <inheritdoc />
        public string GetName() => _underlyingType.Name;

        /// <inheritdoc />
        public int GetGenericParameterPosition() => _underlyingType.GenericParameterPosition;

        /// <inheritdoc />
        public global::System.Reflection.GenericParameterAttributes GetGenericParameterAttributes() => (global::System.Reflection.GenericParameterAttributes)_underlyingType.GenericParameterAttributes;

        /// <inheritdoc />
        public ImmutableArray<TypeSymbol> GetGenericParameterConstraints()
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

        /// <inheritdoc />
        public ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
        {
            if (_optionalCustomModifiers.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _optionalCustomModifiers, _context.ResolveTypeSymbols(_underlyingType.__GetOptionalCustomModifiers()));

            return _optionalCustomModifiers;
        }

        /// <inheritdoc />
        public ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
            if (_requiredCustomModifiers.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _requiredCustomModifiers, _context.ResolveTypeSymbols(_underlyingType.__GetRequiredCustomModifiers()));

            return _requiredCustomModifiers;
        }

        /// <inheritdoc />
        public ImmutableArray<CustomAttribute> GetCustomAttributes()
        {
            if (_customAttributes.IsDefault)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, _context.ResolveCustomAttributes(_underlyingType.GetCustomAttributesData()));

            return _customAttributes;
        }

    }

}
