using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes a source of information for a <see cref="DefinitionEventSymbol"/>.
    /// </summary>
    interface IEventLoader
    {

        /// <summary>
        /// Gets whether the event is missing.
        /// </summary>
        bool GetIsMissing();

        /// <summary>
        /// Gets the declaring type.
        /// </summary>
        /// <returns></returns>
        TypeSymbol GetDeclaringType();

        /// <summary>
        /// Gets the event name.
        /// </summary>
        string GetName();

        /// <summary>
        /// Gets the event attributes.
        /// </summary>
        /// <returns></returns>
        EventAttributes GetAttributes();

        /// <summary>
        /// Gets the event handler type.
        /// </summary>
        /// <returns></returns>
        TypeSymbol? GetEventHandlerType();

        /// <summary>
        /// Gets the add method.
        /// </summary>
        /// <returns></returns>
        MethodSymbol? GetAddMethod();

        /// <summary>
        /// Gets the remove method.
        /// </summary>
        /// <returns></returns>
        MethodSymbol? GetRemoveMethod();

        /// <summary>
        /// Gets the raise method.
        /// </summary>
        /// <returns></returns>
        MethodSymbol? GetRaiseMethod();

        /// <summary>
        /// Gets the other methods.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<MethodSymbol> GetOtherMethods();

        /// <summary>
        /// Loads the custom attributes of the event.
        /// </summary>
        /// <returns></returns>
        ImmutableArray<CustomAttribute> GetCustomAttributes();
    }

}
