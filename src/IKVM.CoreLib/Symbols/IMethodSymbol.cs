namespace IKVM.CoreLib.Symbols
{

	interface IMethodSymbol : IMethodBaseSymbol
	{

		IParameterSymbol ReturnParameter { get; }

		ITypeSymbol ReturnType { get; }

		ICustomAttributeSymbolProvider ReturnTypeCustomAttributes { get; }

		IMethodSymbol GetBaseDefinition();

		IMethodSymbol GetGenericMethodDefinition();

		IMethodSymbol MakeGenericMethod(params ITypeSymbol[] typeArguments);

	}

}