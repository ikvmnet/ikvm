using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    class ConstructedGenericParameterSymbol : ParameterSymbol
    {

        readonly ParameterSymbol _definition;
        readonly GenericContext _genericContext;

        ImmutableArray<TypeSymbol> _optionalCustomModifiers;
        ImmutableArray<TypeSymbol> _requiredCustomModifiers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringMember"></param>
        /// <param name="definition"></param>
        /// <param name="genericContext"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ConstructedGenericParameterSymbol(SymbolContext context, MemberSymbol declaringMember, ParameterSymbol definition, GenericContext genericContext) :
            base(context, declaringMember, definition.Position)
        {
            _definition = definition ?? throw new ArgumentNullException(nameof(definition));
            _genericContext = genericContext;
        }

        /// <inheritdoc />
        public override ParameterAttributes Attributes => _definition.Attributes;

        /// <inheritdoc />
        public override TypeSymbol ParameterType => _definition.ParameterType.Specialize(_genericContext);

        /// <inheritdoc />
        public override string? Name => _definition.Name;

        /// <inheritdoc />
        public override object? DefaultValue => _definition.DefaultValue;

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override bool ContainsMissing => false;

        /// <inheritdoc />
        public sealed override bool IsComplete => true;

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
        {
            if (_optionalCustomModifiers == default)
            {
                var b = ImmutableArray.CreateBuilder<TypeSymbol>();
                foreach (var i in _definition.GetOptionalCustomModifiers())
                    b.Add(i.Specialize(_genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _optionalCustomModifiers, b.ToImmutable());
            }

            return _optionalCustomModifiers;
        }

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
            if (_requiredCustomModifiers == default)
            {
                var b = ImmutableArray.CreateBuilder<TypeSymbol>();
                foreach (var i in _definition.GetRequiredCustomModifiers())
                    b.Add(i.Specialize(_genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _requiredCustomModifiers, b.ToImmutable());
            }

            return _requiredCustomModifiers;
        }

        /// <inheritdoc />
        internal override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            throw new NotImplementedException();
        }

    }

}
