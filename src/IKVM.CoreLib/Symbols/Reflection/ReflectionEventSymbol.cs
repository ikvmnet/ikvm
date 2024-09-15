using System;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionEventSymbol : ReflectionMemberSymbol, IEventSymbol
    {

        readonly EventInfo _event;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <param name="event"></param>
        public ReflectionEventSymbol(ReflectionSymbolContext context, ReflectionTypeSymbol type, EventInfo @event) :
            base(context, type, @event)
        {
            _event = @event ?? throw new ArgumentNullException(nameof(@event));
        }

        /// <summary>
        /// Gets the underlying <see cref="EventInfo"/> wrapped by this symbol.
        /// </summary>
        internal new EventInfo ReflectionObject => _event;

        /// <inheritdoc />
        public EventAttributes Attributes => _event.Attributes;

        /// <inheritdoc />
        public ITypeSymbol? EventHandlerType => _event.EventHandlerType is { } m ? ResolveTypeSymbol(m) : null;

        /// <inheritdoc />
        public bool IsSpecialName => _event.IsSpecialName;

        /// <inheritdoc />
        public IMethodSymbol? AddMethod => _event.AddMethod is { } m ? ResolveMethodSymbol(m) : null;

        /// <inheritdoc />
        public IMethodSymbol? RemoveMethod => _event.RemoveMethod is { } m ? ResolveMethodSymbol(m) : null;

        /// <inheritdoc />
        public IMethodSymbol? RaiseMethod => _event.RaiseMethod is { } m ? ResolveMethodSymbol(m) : null;

        /// <inheritdoc />
        public IMethodSymbol? GetAddMethod()
        {
            return _event.GetAddMethod() is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetAddMethod(bool nonPublic)
        {
            return _event.GetAddMethod(nonPublic) is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetRemoveMethod()
        {
            return _event.GetRemoveMethod() is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetRemoveMethod(bool nonPublic)
        {
            return _event.GetRemoveMethod(nonPublic) is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetRaiseMethod()
        {
            return _event.GetRaiseMethod() is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetRaiseMethod(bool nonPublic)
        {
            return _event.GetRaiseMethod(nonPublic) is { } m ? ResolveMethodSymbol(m) : null;
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetOtherMethods()
        {
            return ResolveMethodSymbols(_event.GetOtherMethods());
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetOtherMethods(bool nonPublic)
        {
            return ResolveMethodSymbols(_event.GetOtherMethods(nonPublic));
        }

    }

}
