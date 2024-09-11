namespace IKVM.CoreLib.Symbols
{

	struct CustomAttributeNamedArgument
	{

		bool IsField { get; }

		IMemberSymbol MemberInfo { get; }

		string MemberName { get; }

		CustomAttributeTypedArgument TypedValue { get; }

	}

}