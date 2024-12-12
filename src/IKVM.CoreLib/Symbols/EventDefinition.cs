using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    abstract class EventDefinition
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        protected EventDefinition(SymbolContext context)
        {

        }

        /// <summary>
        /// Gets the event attributes.
        /// </summary>
        /// <returns></returns>
        public abstract EventAttributes GetAttributes();

        /// <summary>
        /// Gets the event handler type.
        /// </summary>
        /// <returns></returns>
        public abstract TypeSymbol? GetEventHandlerType();

        /// <summary>
        /// Gets the add method.
        /// </summary>
        /// <returns></returns>
        public abstract MethodSymbol? GetAddMethod();

        /// <summary>
        /// Gets the remove method.
        /// </summary>
        /// <returns></returns>
        public abstract MethodSymbol? GetRemoveMethod();

        /// <summary>
        /// Gets the raise method.
        /// </summary>
        /// <returns></returns>
        public abstract MethodSymbol? GetRaiseMethod();

        /// <summary>
        /// Gets the other methods.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<MethodSymbol> GetOtherMethods();

        /// <summary>
        /// Gets the custom attributes.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<CustomAttribute> GetCustomAttributes();
    }

}
