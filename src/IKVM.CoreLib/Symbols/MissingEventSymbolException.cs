namespace IKVM.CoreLib.Symbols
{

    public class MissingEventSymbolException : MissingMemberSymbolException
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="symbol"></param>
        public MissingEventSymbolException(EventSymbol symbol) : 
            base(symbol)
        {

        }

        /// <summary>
        /// Gets the event symbol that is missing.
        /// </summary>
        public new EventSymbol Symbol => (EventSymbol)base.Symbol;

    }

}