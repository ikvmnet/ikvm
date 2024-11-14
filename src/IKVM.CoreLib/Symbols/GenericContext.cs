using System.Collections.Immutable;

namespace IKVM.CoreLib.Symbols
{

    readonly struct GenericContext(ImmutableArray<TypeSymbol>? GenericTypeArguments, ImmutableArray<TypeSymbol>? GenericMethodArguments)
    {

        public readonly ImmutableArray<TypeSymbol>? GenericTypeArguments = GenericTypeArguments;

        public readonly ImmutableArray<TypeSymbol>? GenericMethodArguments = GenericMethodArguments;

    }

}
