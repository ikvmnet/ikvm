namespace IKVM.CoreLib.Symbols.Emit
{

    /// <summary>
    /// Base interface for a builder of symbols.
    /// </summary>
    interface ISymbolBuilder
    {

        /// <summary>
        /// Gets the <see cref="ISymbol"/> that is currently being built. Portions of this interface will be non-functional until the build is completed.
        /// </summary>
        ISymbol Symbol { get; }

    }

    /// <summary>
    /// Base interface for a builder of symbols.
    /// </summary>
    /// <typeparam name="TSymbol"></typeparam>
    interface ISymbolBuilder<out TSymbol> : ISymbolBuilder
        where TSymbol : ISymbol
    {

        /// <summary>
        /// Gets the <see cref="TSymbol"/> that is currently being built. Portions of this interface will be non-functional until the build is completed.
        /// </summary>
        new TSymbol Symbol { get; }

    }

}
