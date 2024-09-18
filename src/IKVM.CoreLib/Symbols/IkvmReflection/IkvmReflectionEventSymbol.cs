using System;

using EventInfo = IKVM.Reflection.EventInfo;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionEventSymbol : IkvmReflectionMemberSymbol, IEventSymbol
    {

        readonly EventInfo _event;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <param name="event"></param>
        public IkvmReflectionEventSymbol(IkvmReflectionSymbolContext context, IkvmReflectionTypeSymbol type, EventInfo @event) :
            base(context, type.ContainingModule, type, @event)
        {
            _event = @event ?? throw new ArgumentNullException(nameof(@event));
        }

        public IMethodSymbol? AddMethod => _event.AddMethod is { } m ? ResolveMethodSymbol(m) : null;

        public System.Reflection.EventAttributes Attributes => (System.Reflection.EventAttributes)_event.Attributes;

        public ITypeSymbol? EventHandlerType => _event.EventHandlerType is { } m ? ResolveTypeSymbol(m) : null;

        public bool IsSpecialName => _event.IsSpecialName;

        public IMethodSymbol? RaiseMethod => _event.RaiseMethod is { } m ? ResolveMethodSymbol(m) : null;

        public IMethodSymbol? RemoveMethod => _event.RemoveMethod is { } m ? ResolveMethodSymbol(m) : null;

        public IMethodSymbol? GetAddMethod()
        {
            return _event.GetAddMethod() is { } m ? ResolveMethodSymbol(m) : null;
        }

        public IMethodSymbol? GetAddMethod(bool nonPublic)
        {
            return _event.GetAddMethod(nonPublic) is { } m ? ResolveMethodSymbol(m) : null;
        }

        public IMethodSymbol[] GetOtherMethods()
        {
            return ResolveMethodSymbols(_event.GetOtherMethods());
        }

        public IMethodSymbol[] GetOtherMethods(bool nonPublic)
        {
            return ResolveMethodSymbols(_event.GetOtherMethods(nonPublic));
        }

        public IMethodSymbol? GetRaiseMethod()
        {
            return _event.GetRaiseMethod() is { } m ? ResolveMethodSymbol(m) : null;
        }

        public IMethodSymbol? GetRaiseMethod(bool nonPublic)
        {
            return _event.GetRaiseMethod(nonPublic) is { } m ? ResolveMethodSymbol(m) : null;
        }

        public IMethodSymbol? GetRemoveMethod(bool nonPublic)
        {
            return _event.GetRemoveMethod(nonPublic) is { } m ? ResolveMethodSymbol(m) : null;
        }

        public IMethodSymbol? GetRemoveMethod()
        {
            return _event.GetRemoveMethod() is { } m ? ResolveMethodSymbol(m) : null;
        }

    }

}