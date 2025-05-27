namespace IKVM.CoreLib.Symbols
{

    public class MissingPropertySymbolException : MissingMemberSymbolException
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="symbol"></param>
        public MissingPropertySymbolException(PropertySymbol symbol) :
            base(symbol)
        {

        }

        /// <summary>
        /// Gets the property symbol that is missing.
        /// </summary>
        public new PropertySymbol Symbol => (PropertySymbol)base.Symbol;

    }

}