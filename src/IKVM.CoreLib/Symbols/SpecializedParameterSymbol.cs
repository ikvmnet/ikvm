using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    class SpecializedParameterSymbol : ParameterSymbol
    {

        readonly MemberSymbol _member;
        readonly ParameterSymbol _definition;
        readonly GenericContext _genericContext;

        LazyField<TypeSymbol> _parameterType;
        ImmutableArray<TypeSymbol> _optionalCustomModifiers;
        ImmutableArray<TypeSymbol> _requiredCustomModifiers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="member"></param>
        /// <param name="definition"></param>
        /// <param name="genericContext"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SpecializedParameterSymbol(SymbolContext context, MemberSymbol member, ParameterSymbol definition, GenericContext genericContext) :
            base(context)
        {
            _member = member ?? throw new ArgumentNullException(nameof(member));
            _definition = definition ?? throw new ArgumentNullException(nameof(definition));
            _genericContext = genericContext;
        }

        /// <inheritdoc />
        public override MemberSymbol Member => _member;

        /// <inheritdoc />
        public override int Position => _definition.Position;

        /// <inheritdoc />
        public override ParameterAttributes Attributes => _definition.Attributes;

        /// <inheritdoc />
        public override TypeSymbol ParameterType => _parameterType.IsDefault ? _parameterType.InterlockedInitialize(_definition.ParameterType.Specialize(_genericContext)) : _parameterType.Value;

        /// <inheritdoc />
        public override string? Name => _definition.Name;

        /// <inheritdoc />
        public override object? DefaultValue => throw new NotImplementedException();

        /// <inheritdoc />
        public sealed override bool IsMissing => false;

        /// <inheritdoc />
        public sealed override bool ContainsMissing => false;

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
        {
            if (_optionalCustomModifiers.IsDefault)
            {
                var l = _definition.GetOptionalCustomModifiers();
                var b = ImmutableArray.CreateBuilder<TypeSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(i.Specialize(_genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _optionalCustomModifiers, b.DrainToImmutable());
            }

            return _optionalCustomModifiers;
        }

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
            if (_requiredCustomModifiers.IsDefault)
            {
                var l = _definition.GetRequiredCustomModifiers();
                var b = ImmutableArray.CreateBuilder<TypeSymbol>(l.Length);
                foreach (var i in l)
                    b.Add(i.Specialize(_genericContext));

                ImmutableInterlocked.InterlockedInitialize(ref _requiredCustomModifiers, b.DrainToImmutable());
            }

            return _requiredCustomModifiers;
        }

        /// <inheritdoc />
        internal override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            return _definition.GetDeclaredCustomAttributes();
        }

    }

}
