namespace IKVM.CoreLib.Symbols
{

    readonly struct InterfaceMapping(MethodSymbol[] InterfaceMethods, TypeSymbol InterfaceType, MethodSymbol[] TargetMethods, TypeSymbol TargetType)
    {

        public readonly MethodSymbol[] InterfaceMethods = InterfaceMethods;
        public readonly TypeSymbol InterfaceType = InterfaceType;
        public readonly MethodSymbol[] TargetMethods = TargetMethods;
        public readonly TypeSymbol TargetType = TargetType;

    }

}
