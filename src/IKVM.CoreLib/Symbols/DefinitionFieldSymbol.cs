using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    class DefinitionFieldSymbol : FieldSymbol
    {

        readonly IFieldLoader _loader;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="loader"></param>
        public DefinitionFieldSymbol(SymbolContext context, IFieldLoader loader) :
            base(context)
        {
            _loader = loader ?? throw new ArgumentNullException(nameof(loader));
        }

        /// <summary>
        /// Gets the associated loader.
        /// </summary>
        public IFieldLoader Loader => _loader;

        /// <inheritdoc />
        public override ModuleSymbol Module => _loader.GetModule();

        /// <inheritdoc />
        public override TypeSymbol? DeclaringType => _loader.GetDeclaringType();

        /// <inheritdoc />
        public sealed override bool IsMissing => _loader.GetIsMissing();

        /// <inheritdoc />
        public sealed override string Name => _loader.GetName();

        /// <inheritdoc />
        public sealed override FieldAttributes Attributes => _loader.GetAttributes();

        /// <inheritdoc />
        public sealed override TypeSymbol FieldType => _loader.GetFieldType();

        /// <inheritdoc />
        public sealed override object? GetRawConstantValue() => _loader.GetConstantValue();

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers() => _loader.GetRequiredCustomModifiers();

        /// <inheritdoc />
        public sealed override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers() => _loader.GetOptionalCustomModifiers();

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes() => _loader.GetCustomAttributes();

    }

}
