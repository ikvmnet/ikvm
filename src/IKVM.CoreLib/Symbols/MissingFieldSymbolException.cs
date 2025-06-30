namespace IKVM.CoreLib.Symbols
{

    public class MissingFieldSymbolException : MissingMemberSymbolException
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="symbol"></param>
        public MissingFieldSymbolException(FieldSymbol symbol) :
            base(symbol)
        {

        }

        /// <summary>
        /// Gets the field symbol that is missing.
        /// </summary>
        public new FieldSymbol Symbol => (FieldSymbol)base.Symbol;

    }

}