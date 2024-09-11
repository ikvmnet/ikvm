using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

	interface IPropertySymbol : IMemberSymbol
	{

		PropertyAttributes Attributes { get; }

		bool CanRead { get; }

		bool CanWrite { get; }

		IMethodSymbol? GetMethod { get; }

		bool IsSpecialName { get; }

		ITypeSymbol PropertyType { get; }

		IMethodSymbol? SetMethod { get; }

	}

}
