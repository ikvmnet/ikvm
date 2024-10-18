using System.Collections.Immutable;

namespace IKVM.CoreLib.Symbols
{

    readonly struct InterfaceMapping(IMethodSymbol[] InterfaceMethods, ITypeSymbol InterfaceType, IMethodSymbol[] TargetMethods, ITypeSymbol TargetType)
    {

        public readonly IMethodSymbol[] InterfaceMethods = InterfaceMethods;
        public readonly ITypeSymbol InterfaceType = InterfaceType;
        public readonly IMethodSymbol[] TargetMethods = TargetMethods;
        public readonly ITypeSymbol TargetType = TargetType;

    }

}
