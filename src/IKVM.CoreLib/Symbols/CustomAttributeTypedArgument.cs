namespace IKVM.CoreLib.Symbols
{

    readonly record struct CustomAttributeTypedArgument(
        ITypeSymbol ArgumentType,
        object? Value);

}
