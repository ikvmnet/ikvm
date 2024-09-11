using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

	interface IEventSymbol : IMemberSymbol
	{

		IMethodSymbol? AddMethod { get; }

		EventAttributes Attributes { get; }

		ITypeSymbol? EventHandlerType { get; }

		bool IsMulticast { get; }

		bool IsSpecialName { get; }

		IMethodSymbol? RaiseMethod { get; }

		IMethodSymbol? RemoveMethod { get; }

		IMethodSymbol? GetAddMethod();

		IMethodSymbol? GetAddMethod(bool nonPublic);

		IMethodSymbol[] GetOtherMethods();

		IMethodSymbol[] GetOtherMethods(bool nonPublic);

		IMethodSymbol? GetRaiseMethod();

		IMethodSymbol? GetRaiseMethod(bool nonPublic);

		IMethodSymbol? GetRemoveMethod(bool nonPublic);

		IMethodSymbol? GetRemoveMethod();

	}

}
