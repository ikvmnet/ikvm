using System.Collections.Immutable;

namespace IKVM.CoreLib.Symbols
{

	interface ICustomAttributeSymbolProvider
	{

		ImmutableArray<ICustomAttributeSymbol> GetCustomAttributes(bool inherit);

		ImmutableArray<ICustomAttributeSymbol> GetCustomAttributes(ITypeSymbol attributeType, bool inherit);

		bool IsDefined(ITypeSymbol attributeType, bool inherit);

	}

}
