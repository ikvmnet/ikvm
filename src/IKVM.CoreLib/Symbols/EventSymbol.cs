using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    abstract class EventSymbol : MemberSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        public EventSymbol(ISymbolContext context, TypeSymbol declaringType) : 
            base(context, declaringType.Module, declaringType)
        {

        }

        /// <summary>
        /// Gets the attributes for this event.
        /// </summary>
        public abstract EventAttributes Attributes { get; }

        /// <inheritdoc />
        public sealed override MemberTypes MemberType => MemberTypes.Event;

        /// <summary>
        /// Gets the <see cref="TypeSymbol"> object of the underlying event-handler delegate associated with this event.
        /// </summary>
        public abstract TypeSymbol? EventHandlerType { get; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="EventSymbol"/> has a name with a special meaning.
        /// </summary>
        public bool IsSpecialName => (Attributes & EventAttributes.SpecialName) != 0;

        /// <summary>
        /// Returns the method used to add an event handler delegate to the event source.
        /// </summary>
        public abstract MethodSymbol? AddMethod { get; }

        /// <summary>
        /// Returns the method used to remove an event handler delegate from the event source.
        /// </summary>
        public abstract MethodSymbol? RemoveMethod { get; }

        /// <summary>
        /// Returns the method that is called when the event is raised.
        /// </summary>
        public abstract MethodSymbol? RaiseMethod { get; }

        /// <summary>
        /// Returns the method used to add an event handler delegate to the event source.
        /// </summary>
        /// <returns></returns>
        public abstract MethodSymbol? GetAddMethod();

        /// <summary>
        /// Returns the method used to add an event handler delegate to the event source, specifying whether to return non-public methods.
        /// </summary>
        /// <param name="nonPublic"></param>
        /// <returns></returns>
        public abstract MethodSymbol? GetAddMethod(bool nonPublic);

        /// <summary>
        /// Returns the public methods that have been associated with an event in metadata using the .other directive.
        /// </summary>
        /// <returns></returns>
        public abstract MethodSymbol[] GetOtherMethods();

        /// <summary>
        /// Returns the methods that have been associated with the event in metadata using the .other directive, specifying whether to include non-public methods.
        /// </summary>
        /// <param name="nonPublic"></param>
        /// <returns></returns>
        public abstract MethodSymbol[] GetOtherMethods(bool nonPublic);

        /// <summary>
        /// Returns the method that is called when the event is raised.
        /// </summary>
        /// <returns></returns>
        public abstract MethodSymbol? GetRaiseMethod();

        /// <summary>
        /// When overridden in a derived class, returns the method that is called when the event is raised, specifying whether to return non-public methods.
        /// </summary>
        /// <param name="nonPublic"></param>
        /// <returns></returns>
        public abstract MethodSymbol? GetRaiseMethod(bool nonPublic);

        /// <summary>
        /// Returns the method used to remove an event handler delegate from the event source.
        /// </summary>
        /// <returns></returns>
        public abstract MethodSymbol? GetRemoveMethod();

        /// <summary>
        /// When overridden in a derived class, retrieves the <see cref="MethodSymbol"/> object for removing a method of the event, specifying whether to return non-public methods.
        /// </summary>
        /// <param name="nonPublic"></param>
        /// <returns></returns>
        public abstract MethodSymbol? GetRemoveMethod(bool nonPublic);

    }

}
