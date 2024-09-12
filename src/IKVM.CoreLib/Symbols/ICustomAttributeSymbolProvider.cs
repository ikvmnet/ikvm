using System.Collections.Immutable;

namespace IKVM.CoreLib.Symbols
{

	interface ICustomAttributeSymbolProvider
	{

		CustomAttributeSymbol[] GetCustomAttributes(bool inherit);

		CustomAttributeSymbol[] GetCustomAttributes(ITypeSymbol attributeType, bool inherit);

		bool IsDefined(ITypeSymbol attributeType, bool inherit);

	}

}
