using System.Collections.Immutable;

namespace IKVM.CoreLib.Symbols
{

    readonly struct InterfaceMapping
    {

        public readonly ImmutableArray<IMethodSymbol> InterfaceMethods;
        public readonly ITypeSymbol InterfaceType;
        public readonly ImmutableArray<IMethodSymbol> TargetMethods;
        public readonly ITypeSymbol TargetType;

    }

}
