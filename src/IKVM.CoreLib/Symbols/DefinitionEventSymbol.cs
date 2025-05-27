using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    class DefinitionEventSymbol : EventSymbol
    {

        readonly IEventLoader _loader;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="loader"></param>
        public DefinitionEventSymbol(SymbolContext context, IEventLoader loader) :
            base(context)
        {
            _loader = loader ?? throw new ArgumentNullException(nameof(loader));
        }

        /// <summary>
        /// Gets the associated loader.
        /// </summary>
        public IEventLoader Loader => _loader;

        /// <inheritdoc />
        public sealed override TypeSymbol? DeclaringType => _loader.GetDeclaringType();

        /// <inheritdoc />
        public sealed override bool IsMissing => _loader.GetIsMissing();

        /// <inheritdoc />
        public sealed override string Name => _loader.GetName();

        /// <inheritdoc />
        public sealed override EventAttributes Attributes => _loader.GetAttributes();

        /// <inheritdoc />
        public sealed override TypeSymbol? EventHandlerType => _loader.GetEventHandlerType();

        /// <inheritdoc />
        public sealed override MethodSymbol? AddMethod => _loader.GetAddMethod();

        /// <inheritdoc />
        public sealed override MethodSymbol? RemoveMethod => _loader.GetRemoveMethod();

        /// <inheritdoc />
        public sealed override MethodSymbol? RaiseMethod => _loader.GetRaiseMethod();

        /// <inheritdoc />
        public sealed override ImmutableArray<MethodSymbol> OtherMethods => _loader.GetOtherMethods();

        /// <inheritdoc />
        internal sealed override ImmutableArray<CustomAttribute> GetDeclaredCustomAttributes() => _loader.GetCustomAttributes();

    }

}
