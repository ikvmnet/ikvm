using System;
using System.Collections.Immutable;
using System.Reflection;
using System.Threading;

namespace IKVM.CoreLib.Symbols
{

    class DefinitionFieldSymbol : FieldSymbol
    {

        readonly string _name;
        readonly DefinitionModuleSymbol _moduleDef;
        readonly DefinitionTypeSymbol? _declaringTypeDef;
        FieldDefinition? _def;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="moduleDef"></param>
        /// <param name="declaringTypeDef"></param>
        /// <param name="name"></param>
        /// <param name="def"></param>
        public DefinitionFieldSymbol(SymbolContext context, DefinitionModuleSymbol moduleDef, DefinitionTypeSymbol? declaringTypeDef, string name, FieldDefinition? def) :
            base(context, moduleDef, declaringTypeDef)
        {
            _moduleDef = moduleDef ?? throw new ArgumentNullException(nameof(moduleDef));
            _declaringTypeDef = declaringTypeDef;
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _def = def;
        }

        /// <summary>
        /// Gets the underlying source information. If the type source is missing, <c>null</c> is returned.
        /// </summary>
        FieldDefinition? Def => GetDef();

        /// <summary>
        /// Attempts to resolve the symbol definition source.
        /// </summary>
        /// <returns></returns>
        FieldDefinition? GetDef()
        {
            if (_def is null)
                if (_declaringTypeDef is { } dt)
                    Interlocked.CompareExchange(ref _def, dt.ResolveFieldDef(_name), null);
                else
                    Interlocked.CompareExchange(ref _def, _moduleDef.ResolveFieldDef(_name), null);

            return _def;
        }

        /// <summary>
        /// Attempts to resolve the symbol definition source, or throws.
        /// </summary>
        FieldDefinition DefOrThrow => Def ?? throw new MissingFieldSymbolException(this);

        /// <inheritdoc />
        public sealed override bool IsMissing => Def == null;

        /// <inheritdoc />
        public sealed override string Name => _name;

        /// <inheritdoc />
        public override FieldAttributes Attributes => DefOrThrow.GetAttributes();

        /// <inheritdoc />
        public override TypeSymbol FieldType => DefOrThrow.GetFieldType();

        /// <inheritdoc />
        public override object? GetRawConstantValue() => DefOrThrow.GetConstantValue();

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetRequiredCustomModifiers() => DefOrThrow.GetRequiredCustomModifiers();

        /// <inheritdoc />
        public override ImmutableArray<TypeSymbol> GetOptionalCustomModifiers() => DefOrThrow.GetOptionalCustomModifiers();

        /// <inheritdoc />
        internal override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes() => DefOrThrow.GetCustomAttributes();

    }

}
