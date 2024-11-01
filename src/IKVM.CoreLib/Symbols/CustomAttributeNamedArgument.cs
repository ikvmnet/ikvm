namespace IKVM.CoreLib.Symbols
{

    readonly record struct CustomAttributeNamedArgument(
        bool IsField,
        IMemberSymbol MemberInfo,
        string MemberName,
        CustomAttributeTypedArgument TypedValue);

}
