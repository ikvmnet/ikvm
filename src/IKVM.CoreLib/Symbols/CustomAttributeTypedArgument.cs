namespace IKVM.CoreLib.Symbols
{

	struct CustomAttributeTypedArgument
	{

		ITypeSymbol ArgumentType { get; }

		object? Value { get; }

	}

}