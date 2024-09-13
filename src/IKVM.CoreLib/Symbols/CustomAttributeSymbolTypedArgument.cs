namespace IKVM.CoreLib.Symbols
{

    readonly record struct CustomAttributeSymbolTypedArgument(
        ITypeSymbol ArgumentType,
        object? Value);

}
