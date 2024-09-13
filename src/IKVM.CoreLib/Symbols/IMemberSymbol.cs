using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

	/// <summary>
	/// Obtains information about the attributes of a member and provides access to member metadata.
	/// </summary>
	interface IMemberSymbol : ISymbol, ICustomAttributeSymbolProvider
	{

		/// <summary>
		/// Gets the module in which the type that declares the member represented by the current <see cref="IMemberSymbol"> is defined.
		/// </summary>
		IModuleSymbol Module { get; }

		/// <summary>
		/// Gets the class that declares this member.
		/// </summary>
		ITypeSymbol? DeclaringType { get; }

		/// <summary>
		/// When overridden in a derived class, gets a <see cref="MemberTypes"> value indicating the type of the member - method, constructor, event, and so on.
		/// </summary>
		MemberTypes MemberType { get; }

		/// <summary>
		/// Gets a value that identifies a metadata element.
		/// </summary>
		int MetadataToken { get; }

		/// <summary>
		/// Gets the name of the current member.
		/// </summary>
		string Name { get; }

	}

}
