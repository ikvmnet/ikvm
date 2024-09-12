using System.Collections.Immutable;

namespace IKVM.CoreLib.Symbols
{

	readonly record struct CustomAttributeSymbol(
		ITypeSymbol AttributeType,
		IConstructorSymbol Constructor,
		ImmutableArray<CustomAttributeSymbolTypedArgument> ConstructorArguments,
		ImmutableArray<CustomAttributeSymbolNamedArgument> NamedArguments);

}
