using System;

using EventInfo = IKVM.Reflection.EventInfo;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionEventSymbol : IkvmReflectionMemberSymbol, IEventSymbol
    {

        readonly EventInfo _underlyingEvent;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <param name="underlyingEvent"></param>
        public IkvmReflectionEventSymbol(IkvmReflectionSymbolContext context, IkvmReflectionTypeSymbol type, EventInfo underlyingEvent) :
            base(context, type.ContainingModule, type, underlyingEvent)
        {
            _underlyingEvent = underlyingEvent ?? throw new ArgumentNullException(nameof(underlyingEvent));
        }

        public IMethodSymbol? AddMethod => _underlyingEvent.AddMethod is { } m ? ResolveMethodSymbol(m) : null;

        public global::System.Reflection.EventAttributes Attributes => (global::System.Reflection.EventAttributes)_underlyingEvent.Attributes;

        public ITypeSymbol? EventHandlerType => _underlyingEvent.EventHandlerType is { } m ? ResolveTypeSymbol(m) : null;

        public bool IsSpecialName => _underlyingEvent.IsSpecialName;

        public IMethodSymbol? RaiseMethod => _underlyingEvent.RaiseMethod is { } m ? ResolveMethodSymbol(m) : null;

        public IMethodSymbol? RemoveMethod => _underlyingEvent.RemoveMethod is { } m ? ResolveMethodSymbol(m) : null;

        public IMethodSymbol? GetAddMethod()
        {
            return _underlyingEvent.GetAddMethod() is { } m ? ResolveMethodSymbol(m) : null;
        }

        public IMethodSymbol? GetAddMethod(bool nonPublic)
        {
            return _underlyingEvent.GetAddMethod(nonPublic) is { } m ? ResolveMethodSymbol(m) : null;
        }

        public IMethodSymbol[] GetOtherMethods()
        {
            return ResolveMethodSymbols(_underlyingEvent.GetOtherMethods());
        }

        public IMethodSymbol[] GetOtherMethods(bool nonPublic)
        {
            return ResolveMethodSymbols(_underlyingEvent.GetOtherMethods(nonPublic));
        }

        public IMethodSymbol? GetRaiseMethod()
        {
            return _underlyingEvent.GetRaiseMethod() is { } m ? ResolveMethodSymbol(m) : null;
        }

        public IMethodSymbol? GetRaiseMethod(bool nonPublic)
        {
            return _underlyingEvent.GetRaiseMethod(nonPublic) is { } m ? ResolveMethodSymbol(m) : null;
        }

        public IMethodSymbol? GetRemoveMethod(bool nonPublic)
        {
            return _underlyingEvent.GetRemoveMethod(nonPublic) is { } m ? ResolveMethodSymbol(m) : null;
        }

        public IMethodSymbol? GetRemoveMethod()
        {
            return _underlyingEvent.GetRemoveMethod() is { } m ? ResolveMethodSymbol(m) : null;
        }

    }

}