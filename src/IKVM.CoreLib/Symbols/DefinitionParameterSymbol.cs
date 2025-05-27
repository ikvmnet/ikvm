using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    class DefinitionParameterSymbol : ParameterSymbol
    {

        readonly IParameterLoader _loader;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="loader"></param>
        public DefinitionParameterSymbol(SymbolContext context, IParameterLoader loader) :
            base(context)
        {
            _loader = loader ?? throw new ArgumentNullException(nameof(loader));
        }

        /// <inheritdoc />
        public override bool IsMissing => _loader.GetIsMissing();

        /// <inheritdoc />
        public override bool ContainsMissing => throw new NotImplementedException();

        /// <inheritdoc />
        public override MemberSymbol Member => _loader.GetMember();

        /// <inheritdoc />
        public override string? Name => _loader.GetName();

        /// <inheritdoc />
        public override ParameterAttributes Attributes => _loader.GetAttributes();

        /// <inheritdoc />
        public override TypeSymbol ParameterType => _loader.GetParameterType();

        /// <inheritdoc />
        public override int Position => _loader.GetPosition();

        /// <inheritdoc />
        public override object? DefaultValue => _loader.GetDefaultValue();

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers() => _loader.GetOptionalCustomModifiers();

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers() => _loader.GetRequiredCustomModifiers();

        /// <inheritdoc />
        internal override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes() => _loader.GetCustomAttributes();

    }

}
