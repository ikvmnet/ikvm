using System;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionEventSymbol : ReflectionMemberSymbol, IEventSymbol
    {

        readonly EventInfo _underlyingEvent;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <param name="underlyingEvent"></param>
        public ReflectionEventSymbol(ReflectionSymbolContext context, ReflectionTypeSymbol type, EventInfo underlyingEvent) :
            base(context, type.ContainingModule, type, underlyingEvent)
        {
            _underlyingEvent = underlyingEvent ?? throw new ArgumentNullException(nameof(underlyingEvent));
        }

        /// <summary>
        /// Gets the underlying <see cref="EventInfo"/> wrapped by this symbol.
        /// </summary>
        internal EventInfo UnderlyingEvent => _underlyingEvent;

        /// <inheritdoc />
        public EventAttributes Attributes => _underlyingEvent.Attributes;

        /// <inheritdoc />
        public ITypeSymbol? EventHandlerType => _underlyingEvent.EventHandlerType is { } m ? ResolveTypeSymbol(m) : null;

        /// <inheritdoc />
        public bool IsSpecialName => _underlyingEvent.IsSpecialName;

        /// <inheritdoc />
        public IMethodSymbol? AddMethod => _underlyingEvent.AddMethod is { } m ? ResolveMethodSymbol(m) : null;

        /// <inheritdoc />
        public IMethodSymbol? RemoveMethod => _underlyingEvent.RemoveMethod is { } m ? ResolveMethodSymbol(m) : null;

        /// <inheritdoc />
        public IMethodSymbol? RaiseMethod => _underlyingEvent.RaiseMethod is { } m ? ResolveMethodSymbol(m) : null;

        /// <inheritdoc />
        public IMethodSymbol? GetAddMethod()
        {
            return _underlyingEvent.GetAddMethod() is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetAddMethod(bool nonPublic)
        {
            return _underlyingEvent.GetAddMethod(nonPublic) is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetRemoveMethod()
        {
            return _underlyingEvent.GetRemoveMethod() is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetRemoveMethod(bool nonPublic)
        {
            return _underlyingEvent.GetRemoveMethod(nonPublic) is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetRaiseMethod()
        {
            return _underlyingEvent.GetRaiseMethod() is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetRaiseMethod(bool nonPublic)
        {
            return _underlyingEvent.GetRaiseMethod(nonPublic) is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetOtherMethods()
        {
            return ResolveMethodSymbols(_underlyingEvent.GetOtherMethods());
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetOtherMethods(bool nonPublic)
        {
            return ResolveMethodSymbols(_underlyingEvent.GetOtherMethods(nonPublic));
        }

    }

}
