namespace IKVM.CoreLib.Symbols
{

	interface ICustomAttributeSymbolProvider
	{

		CustomAttributeSymbol[] GetCustomAttributes();

		CustomAttributeSymbol[] GetCustomAttributes(ITypeSymbol attributeType);

		bool IsDefined(ITypeSymbol attributeType);

	}

}
