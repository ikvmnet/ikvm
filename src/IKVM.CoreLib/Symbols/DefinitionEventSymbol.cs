using System;
using System.Collections.Immutable;
using System.Reflection;
using System.Threading;

namespace IKVM.CoreLib.Symbols
{

    class DefinitionEventSymbol : EventSymbol
    {

        readonly string _name;
        DefinitionTypeSymbol _declaringTypeDef;
        EventDefinition? _def;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        /// <param name="name"></param>
        protected DefinitionEventSymbol(SymbolContext context, DefinitionTypeSymbol declaringType, string name) :
            base(context, declaringType)
        {
            _declaringTypeDef = declaringType ?? throw new ArgumentNullException(nameof(declaringType));
            _name = name ?? throw new ArgumentNullException(nameof(name));
        }

        /// <summary>
        /// Gets the underlying source information. If the type source is missing, <c>null</c> is returned.
        /// </summary>
        EventDefinition? Def => GetDef();

        /// <summary>
        /// Attempts to resolve the symbol definition source.
        /// </summary>
        /// <returns></returns>
        EventDefinition? GetDef()
        {
            if (_def is null)
                Interlocked.CompareExchange(ref _def, _declaringTypeDef.ResolveEventDef(_name), null);

            return _def;
        }

        /// <summary>
        /// Attempts to resolve the symbol definition source, or throws.
        /// </summary>
        EventDefinition DefOrThrow => Def ?? throw new MissingEventSymbolException(this);

        /// <inheritdoc />
        public sealed override bool IsMissing => Def == null;

        /// <inheritdoc />
        public sealed override string Name => _name;

        /// <inheritdoc />
        public override EventAttributes Attributes => DefOrThrow.GetAttributes();

        /// <inheritdoc />
        public override TypeSymbol? EventHandlerType => DefOrThrow.GetEventHandlerType();

        /// <inheritdoc />
        public override MethodSymbol? AddMethod => DefOrThrow.GetAddMethod();

        /// <inheritdoc />
        public override MethodSymbol? RemoveMethod => DefOrThrow.GetRemoveMethod();

        /// <inheritdoc />
        public override MethodSymbol? RaiseMethod => DefOrThrow.GetRaiseMethod();

        /// <inheritdoc />
        public override ImmutableArray<MethodSymbol> OtherMethods => DefOrThrow.GetOtherMethods();

        /// <inheritdoc />
        internal override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes() => DefOrThrow.GetCustomAttributes();

    }

}
