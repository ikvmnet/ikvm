using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionParameterSymbol : DefinitionParameterSymbol
    {

        readonly ParameterInfo _underlyingParameter;

        TypeSymbol? _parameterType;
        ImmutableArray<TypeSymbol> _optionalCustomModifiers;
        ImmutableArray<TypeSymbol> _requiredCustomModifiers;
        ImmutableArray<CustomAttribute> _customAttributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringMember"></param>
        /// <param name="underlyingParameter"></param>
        public ReflectionParameterSymbol(ReflectionSymbolContext context, MemberSymbol declaringMember, ParameterInfo underlyingParameter) :
            base(context, declaringMember, underlyingParameter.Position)
        {
            _underlyingParameter = underlyingParameter ?? throw new ArgumentNullException(nameof(underlyingParameter));
        }

        /// <summary>
        /// Gets the context that owns this parameter.
        /// </summary>
        new ReflectionSymbolContext Context => (ReflectionSymbolContext)base.Context;

        /// <inheritdoc />
        public sealed override ParameterAttributes Attributes => _underlyingParameter.Attributes;

        /// <inheritdoc />
        public sealed override TypeSymbol ParameterType => _parameterType ??= Context.ResolveTypeSymbol(_underlyingParameter.ParameterType);

        /// <inheritdoc />
        public sealed override string? Name => _underlyingParameter.Name;

        /// <inheritdoc />
        public sealed override object? DefaultValue => _underlyingParameter.DefaultValue;

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override bool ContainsMissing => false;

        /// <inheritdoc />
        public sealed override bool IsComplete => true;

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
        {
            if (_optionalCustomModifiers == default)
                ImmutableInterlocked.InterlockedInitialize(ref _optionalCustomModifiers, Context.ResolveTypeSymbols(_underlyingParameter.GetOptionalCustomModifiers()));

            return _optionalCustomModifiers;
        }

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
            if (_requiredCustomModifiers == default)
                ImmutableInterlocked.InterlockedInitialize(ref _requiredCustomModifiers, Context.ResolveTypeSymbols(_underlyingParameter.GetRequiredCustomModifiers()));

            return _requiredCustomModifiers;
        }

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            if (_customAttributes == default)
                ImmutableInterlocked.InterlockedInitialize(ref _customAttributes, Context.ResolveCustomAttributes(_underlyingParameter.GetCustomAttributesData()));

            return _customAttributes;
        }

    }

}
