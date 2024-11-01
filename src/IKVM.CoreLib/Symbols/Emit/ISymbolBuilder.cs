namespace IKVM.CoreLib.Symbols.Emit
{

    /// <summary>
    /// Base interface for a builder of symbols.
    /// </summary>
    interface ISymbolBuilder
    {



    }

    /// <summary>
    /// Base interface for a builder of symbols.
    /// </summary>
    /// <typeparam name="TSymbol"></typeparam>
    interface ISymbolBuilder<out TSymbol> : ISymbolBuilder
        where TSymbol : ISymbol
    {



    }

}
