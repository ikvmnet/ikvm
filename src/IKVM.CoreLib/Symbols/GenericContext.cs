using System.Collections.Immutable;

namespace IKVM.CoreLib.Symbols
{

    readonly struct GenericContext(ImmutableList<TypeSymbol>? GenericTypeArguments, ImmutableList<TypeSymbol>? GenericMethodArguments)
    {

        public readonly ImmutableList<TypeSymbol>? GenericTypeArguments = GenericTypeArguments;

        public readonly ImmutableList<TypeSymbol>? GenericMethodArguments = GenericMethodArguments;

    }

}
