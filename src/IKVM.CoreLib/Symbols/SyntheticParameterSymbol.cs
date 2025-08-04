using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    class SyntheticParameterSymbol : ParameterSymbol
    {

        readonly MemberSymbol _declaringMember;
        readonly string? _name;
        readonly ParameterAttributes _attributes;
        readonly TypeSymbol _parameterType;
        readonly int _position;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringMember"></param>
        /// <param name="name"></param>
        /// <param name="attributes"></param>
        /// <param name="parameterType"></param>
        /// <param name="position"></param>
        public SyntheticParameterSymbol(SymbolContext context, MemberSymbol declaringMember, string? name, ParameterAttributes attributes, TypeSymbol parameterType, int position) :
            base(context)
        {
            _declaringMember = declaringMember ?? throw new ArgumentNullException(nameof(declaringMember));
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _attributes = attributes;
            _parameterType = parameterType ?? throw new ArgumentNullException(nameof(parameterType));
            _position = position;
        }

        /// <inheritdoc />
        public override MemberSymbol Member => _declaringMember;

        /// <inheritdoc />
        public override string? Name => _name;

        /// <inheritdoc />
        public override ParameterAttributes Attributes => _attributes;

        /// <inheritdoc />
        public override TypeSymbol ParameterType => _parameterType;

        /// <inheritdoc />
        public override int Position => _position;

        /// <inheritdoc />
        public override object? DefaultValue => null;

        /// <inheritdoc />
        public override bool IsMissing => false;

        /// <inheritdoc />
        public override bool ContainsMissing => false;

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
