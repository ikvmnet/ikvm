using System.Collections.Immutable;

namespace IKVM.CoreLib.Symbols
{

	interface ICustomAttributeSymbolProvider
	{

		ICustomAttributeSymbol[] GetCustomAttributes(bool inherit);

		ICustomAttributeSymbol[] GetCustomAttributes(ITypeSymbol attributeType, bool inherit);

		bool IsDefined(ITypeSymbol attributeType, bool inherit);

	}

}
