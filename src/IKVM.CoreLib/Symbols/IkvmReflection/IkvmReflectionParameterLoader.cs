using System;
using System.Collections.Immutable;

using IKVM.Reflection;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionParameterLoader : IParameterLoader
    {

        readonly IkvmReflectionSymbolContext _context;
        readonly ParameterInfo _underlyingParameter;

        LazyField<MemberSymbol> _member;
        LazyField<TypeSymbol> _parameterType;
        ImmutableArray<TypeSymbol> _optionalCustomModifiers;
        ImmutableArray<TypeSymbol> _requiredCustomModifiers;
        ImmutableArray<CustomAttribute> _customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="underlyingParameter"></param>
        public IkvmReflectionParameterLoader(IkvmReflectionSymbolContext context, ParameterInfo underlyingParameter)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _underlyingParameter = underlyingParameter ?? throw new ArgumentNullException(nameof(underlyingParameter));
        }

        /// <inheritdoc />
        public bool GetIsMissing() => false;

        /// <inheritdoc />
        public MemberSymbol GetMember() => _member.IsDefault ? _member.InterlockedInitialize(_context.ResolveMemberSymbol(_underlyingParameter.Member)) : _member.Value;

        /// <inheritdoc />
        public global::System.Reflection.ParameterAttributes GetAttributes() => (global::System.Reflection.ParameterAttributes)_underlyingParameter.Attributes;

        /// <inheritdoc />
        public TypeSymbol GetParameterType() => _parameterType.IsDefault ? _parameterType.InterlockedInitialize(_context.ResolveTypeSymbol(_underlyingParameter.ParameterType)) : _parameterType.Value;

        /// <inheritdoc />
        public string? GetName() => _underlyingParameter.Name;

        /// <inheritdoc />
        public int GetPosition() => _underlyingParameter.Position;

        /// <inheritdoc />
        public object? GetDefaultValue() => _underlyingParameter.RawDefaultValue;

        /// <inheritdoc />
        public ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
        {
            if (_optionalCustomModifiers == default)
                ImmutableInterlocked.InterlockedInitialize(ref _optionalCustomModifiers, _context.ResolveTypeSymbols(_underlyingParameter.GetOptionalCustomModifiers()));

            return _optionalCustomModifiers;
        }

        /// <inheritdoc />
        public ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
            if (_requiredCustomModifiers == default)
                ImmutableInterlocked.InterlockedInitialize(ref _requiredCustomModifiers, _context.ResolveTypeSymbols(_underlyingParameter.GetRequiredCustomModifiers()));

            return _requiredCustomModifiers;
        }

        /// <inheritdoc />
        public ImmutableArray<CustomAttribute> GetCustomAttributes()
        {
            if (_customAttributes == default)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, _context.ResolveCustomAttributes(_underlyingParameter.GetCustomAttributesData()));

            return _customAttributes;
        }

    }

}
