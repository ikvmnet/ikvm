using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    class SyntheticParameterSymbol : ParameterSymbol
    {

        readonly string? _name;
        readonly ParameterAttributes _attributes;
        readonly TypeSymbol _type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringMember"></param>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="type"></param>
        /// <param name="position"></param>
        public SyntheticParameterSymbol(SymbolContext context, MemberSymbol declaringMember, string? name, ParameterAttributes attributes, TypeSymbol type, int position) :
            base(context, declaringMember, position)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _attributes = attributes;
            _type = type;
        }

        /// <inheritdoc />
        public override ParameterAttributes Attributes => _attributes;

        /// <inheritdoc />
        public override TypeSymbol ParameterType => _type;

        /// <inheritdoc />
        public override string? Name => _name;

        /// <inheritdoc />
        public override object? DefaultValue => null;

        /// <inheritdoc />
        public override bool IsMissing => false;

        /// <inheritdoc />
        public override bool ContainsMissing => false;

        /// <inheritdoc />
        public override bool IsComplete => true;

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers()
        {
            return ImmutableArray<TypeSymbol>.Empty;
        }

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers()
        {
            return ImmutableArray<TypeSymbol>.Empty;
        }

        /// <inheritdoc />
        internal override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes()
        {
            return ImmutableArray<CustomAttribute>.Empty;
        }

    }

}
