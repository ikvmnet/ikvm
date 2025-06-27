using System.Collections.Immutable;

namespace IKVM.CoreLib.Symbols
{

    public readonly struct InterfaceMapping(TypeSymbol InterfaceType, ImmutableArray<MethodSymbol> InterfaceMethods, TypeSymbol TargetType, ImmutableArray<MethodSymbol> TargetMethods)
    {

        /// <summary>
        /// Creates a new empty instance.
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        public static InterfaceMapping CreateEmpty(TypeSymbol interfaceType, TypeSymbol targetType)
        {
            return new InterfaceMapping(interfaceType, ImmutableArray<MethodSymbol>.Empty, targetType, ImmutableArray<MethodSymbol>.Empty);
        }

        public readonly TypeSymbol InterfaceType = InterfaceType;
        public readonly ImmutableArray<MethodSymbol> InterfaceMethods = InterfaceMethods;
        public readonly TypeSymbol TargetType = TargetType;
        public readonly ImmutableArray<MethodSymbol> TargetMethods = TargetMethods;

    }

}
