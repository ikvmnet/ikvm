namespace IKVM.CoreLib.Symbols
{

    public class MissingMethodSymbolException : MissingMemberSymbolException
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="symbol"></param>
        public MissingMethodSymbolException(MethodSymbol symbol) :
            base(symbol)
        {

        }

        /// <summary>
        /// Gets the method symbol that is missing.
        /// </summary>
        public new MethodSymbol Symbol => (MethodSymbol)base.Symbol;

    }

}