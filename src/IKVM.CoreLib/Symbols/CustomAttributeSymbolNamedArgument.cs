namespace IKVM.CoreLib.Symbols
{

	readonly record struct CustomAttributeSymbolNamedArgument(
		bool IsField, 
		IMemberSymbol MemberInfo, 
		string MemberName,
		CustomAttributeSymbolTypedArgument TypedValue);

}
