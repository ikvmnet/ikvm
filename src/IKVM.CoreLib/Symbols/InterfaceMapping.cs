using System.Collections.Immutable;

namespace IKVM.CoreLib.Symbols
{

    readonly struct InterfaceMapping(ImmutableList<MethodSymbol> InterfaceMethods, TypeSymbol InterfaceType, ImmutableList<MethodSymbol> TargetMethods, TypeSymbol TargetType)
    {

        public readonly ImmutableList<MethodSymbol> InterfaceMethods = InterfaceMethods;
        public readonly TypeSymbol InterfaceType = InterfaceType;
        public readonly ImmutableList<MethodSymbol> TargetMethods = TargetMethods;
        public readonly TypeSymbol TargetType = TargetType;

    }

}
