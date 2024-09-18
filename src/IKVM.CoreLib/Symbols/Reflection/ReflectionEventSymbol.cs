using System;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionEventSymbol : ReflectionMemberSymbol, IReflectionEventSymbol
    {

        readonly EventInfo _event;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingType"></param>
        /// <param name="event"></param>
        public ReflectionEventSymbol(ReflectionSymbolContext context, IReflectionModuleSymbol resolvingModule, IReflectionTypeSymbol resolvingType, EventInfo @event) :
            base(context, resolvingModule, resolvingType, @event)
        {
            _event = @event ?? throw new ArgumentNullException(nameof(@event));
        }

        /// <summary>
        /// Gets the underlying <see cref="EventInfo"/> wrapped by this symbol.
        /// </summary>
        public EventInfo UnderlyingEvent => _event;

        #region IEventSymbol

        /// <inheritdoc />
        public EventAttributes Attributes => UnderlyingEvent.Attributes;

        /// <inheritdoc />
        public ITypeSymbol? EventHandlerType => ResolveTypeSymbol(UnderlyingEvent.EventHandlerType);

        /// <inheritdoc />
        public bool IsSpecialName => UnderlyingEvent.IsSpecialName;

        /// <inheritdoc />
        public IMethodSymbol? AddMethod => ResolveMethodSymbol(UnderlyingEvent.AddMethod);

        /// <inheritdoc />
        public IMethodSymbol? RemoveMethod => ResolveMethodSymbol(UnderlyingEvent.RemoveMethod);

        /// <inheritdoc />
        public IMethodSymbol? RaiseMethod => ResolveMethodSymbol(UnderlyingEvent.RaiseMethod);

        /// <inheritdoc />
        public IMethodSymbol? GetAddMethod()
        {
            return ResolveMethodSymbol(UnderlyingEvent.GetAddMethod());
        }

        /// <inheritdoc />
        public IMethodSymbol? GetAddMethod(bool nonPublic)
        {
            return ResolveMethodSymbol(UnderlyingEvent.GetAddMethod(nonPublic));
        }

        /// <inheritdoc />
        public IMethodSymbol? GetRemoveMethod()
        {
            return ResolveMethodSymbol(UnderlyingEvent.GetRemoveMethod());
        }

        /// <inheritdoc />
        public IMethodSymbol? GetRemoveMethod(bool nonPublic)
        {
            return ResolveMethodSymbol(UnderlyingEvent.GetRemoveMethod(nonPublic));
        }

        /// <inheritdoc />
        public IMethodSymbol? GetRaiseMethod()
        {
            return ResolveMethodSymbol(UnderlyingEvent.GetRaiseMethod());
        }

        /// <inheritdoc />
        public IMethodSymbol? GetRaiseMethod(bool nonPublic)
        {
            return ResolveMethodSymbol(UnderlyingEvent.GetRaiseMethod(nonPublic));
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetOtherMethods()
        {
            return ResolveMethodSymbols(UnderlyingEvent.GetOtherMethods());
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetOtherMethods(bool nonPublic)
        {
            return ResolveMethodSymbols(UnderlyingEvent.GetOtherMethods(nonPublic));
        }

        #endregion

    }

}
