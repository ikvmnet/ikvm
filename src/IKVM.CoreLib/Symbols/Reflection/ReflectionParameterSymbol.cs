using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    sealed class ReflectionParameterSymbol : DefinitionParameterSymbol
    {

        readonly ReflectionSymbolContext _context;
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
        public ReflectionParameterSymbol(ReflectionSymbolContext context, ParameterInfo underlyingParameter) :
            base(context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _underlyingParameter = underlyingParameter ?? throw new ArgumentNullException(nameof(underlyingParameter));
        }

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override MemberSymbol Member => _member.IsDefault ? _member.InterlockedInitialize(_context.ResolveMemberSymbol(_underlyingParameter.Member)) : _member.Value;

        /// <inheritdoc />
        public sealed override ParameterAttributes Attributes => _underlyingParameter.Attributes;

        /// <inheritdoc />
        public sealed override TypeSymbol ParameterType => _parameterType.IsDefault ? _parameterType.InterlockedInitialize(_context.ResolveTypeSymbol(_underlyingParameter.ParameterType)) : _parameterType.Value;

        /// <inheritdoc />
        public sealed override string? Name => _underlyingParameter.Name;

        /// <inheritdoc />
        public sealed override int Position => _underlyingParameter.Position;

        /// <inheritdoc />
        public sealed override object? DefaultValue => _underlyingParameter.DefaultValue;

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
        {
            if (_optionalCustomModifiers == default)
                ImmutableInterlocked.InterlockedInitialize(ref _optionalCustomModifiers, _context.ResolveTypeSymbols(_underlyingParameter.GetOptionalCustomModifiers()));

            return _optionalCustomModifiers;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
            if (_requiredCustomModifiers == default)
                ImmutableInterlocked.InterlockedInitialize(ref _requiredCustomModifiers, _context.ResolveTypeSymbols(_underlyingParameter.GetRequiredCustomModifiers()));

            return _requiredCustomModifiers;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            if (_customAttributes == default)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, _context.ResolveCustomAttributes(_underlyingParameter.GetCustomAttributesData()));

            return _customAttributes;
        }

    }

}
