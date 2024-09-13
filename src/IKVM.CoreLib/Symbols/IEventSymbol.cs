using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

	/// <summary>
	/// Discovers the attributes of an event and provides access to event metadata.
	/// </summary>
	interface IEventSymbol : IMemberSymbol
	{

		/// <summary>
		/// Gets the attributes for this event.
		/// </summary>
		EventAttributes Attributes { get; }

		/// <summary>
		/// Gets the <see cref="ITypeSymbol"> object of the underlying event-handler delegate associated with this event.
		/// </summary>
		ITypeSymbol? EventHandlerType { get; }

		/// <summary>
		/// Gets a value indicating whether the <see cref="IEventSymbol"/> has a name with a special meaning.
		/// </summary>
		bool IsSpecialName { get; }

		/// <summary>
		/// Returns the method used to add an event handler delegate to the event source.
		/// </summary>
		IMethodSymbol? AddMethod { get; }

		/// <summary>
		/// Returns the method used to remove an event handler delegate from the event source.
		/// </summary>
		IMethodSymbol? RemoveMethod { get; }

		/// <summary>
		/// Returns the method that is called when the event is raised.
		/// </summary>
		IMethodSymbol? RaiseMethod { get; }

		/// <summary>
		/// Returns the method used to add an event handler delegate to the event source.
		/// </summary>
		/// <returns></returns>
		IMethodSymbol? GetAddMethod();

		/// <summary>
		/// Returns the method used to add an event handler delegate to the event source, specifying whether to return non-public methods.
		/// </summary>
		/// <param name="nonPublic"></param>
		/// <returns></returns>
		IMethodSymbol? GetAddMethod(bool nonPublic);

		/// <summary>
		/// Returns the method used to remove an event handler delegate from the event source.
		/// </summary>
		/// <returns></returns>
		IMethodSymbol? GetRemoveMethod();

		/// <summary>
		/// When overridden in a derived class, retrieves the <see cref="IMethodSymbol"/> object for removing a method of the event, specifying whether to return non-public methods.
		/// </summary>
		/// <param name="nonPublic"></param>
		/// <returns></returns>
		IMethodSymbol? GetRemoveMethod(bool nonPublic);

		/// <summary>
		/// Returns the method that is called when the event is raised.
		/// </summary>
		/// <returns></returns>
		IMethodSymbol? GetRaiseMethod();

		/// <summary>
		/// When overridden in a derived class, returns the method that is called when the event is raised, specifying whether to return non-public methods.
		/// </summary>
		/// <param name="nonPublic"></param>
		/// <returns></returns>
		IMethodSymbol? GetRaiseMethod(bool nonPublic);

		/// <summary>
		/// Returns the public methods that have been associated with an event in metadata using the .other directive.
		/// </summary>
		/// <returns></returns>
		IMethodSymbol[] GetOtherMethods();

		/// <summary>
		/// Returns the methods that have been associated with the event in metadata using the .other directive, specifying whether to include non-public methods.
		/// </summary>
		/// <param name="nonPublic"></param>
		/// <returns></returns>
		IMethodSymbol[] GetOtherMethods(bool nonPublic);

	}

}
