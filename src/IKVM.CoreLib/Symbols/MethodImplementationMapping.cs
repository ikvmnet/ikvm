using System.Collections.Immutable;

namespace IKVM.CoreLib.Symbols
{

    readonly struct MethodImplementationMapping(TypeSymbol Type, ImmutableArray<MethodSymbol> Implementations, ImmutableArray<ImmutableArray<MethodSymbol>> Declarations)
    {

        /// <summary>
        /// Creates a new empty instance.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static MethodImplementationMapping CreateEmpty(TypeSymbol type)
        {
            return new MethodImplementationMapping(type, ImmutableArray<MethodSymbol>.Empty, ImmutableArray<ImmutableArray<MethodSymbol>>.Empty);
        }

        public readonly TypeSymbol Type = Type;
        public readonly ImmutableArray<MethodSymbol> Implementations = Implementations;
        public readonly ImmutableArray<ImmutableArray<MethodSymbol>> Declarations = Declarations;

    }

}
