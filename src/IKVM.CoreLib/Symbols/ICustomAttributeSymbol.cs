using System.Collections.Immutable;

namespace IKVM.CoreLib.Symbols
{

	interface ICustomAttributeSymbol
	{

		ITypeSymbol AttributeType { get; }

		IConstructorSymbol Constructor { get; }

		ImmutableArray<CustomAttributeTypedArgument> ConstructorArguments { get; }

		ImmutableArray<CustomAttributeNamedArgument> NamedArguments { get; }

	}

}
