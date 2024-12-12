namespace IKVM.CoreLib.Symbols
{

    public class MissingMemberSymbolException : MissingSymbolException
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="symbol"></param>
        public MissingMemberSymbolException(MemberSymbol symbol) :
            base(symbol)
        {

        }

        /// <summary>
        /// Gets the member symbol that is missing.
        /// </summary>
        public new MemberSymbol Symbol => (MemberSymbol)base.Symbol;

    }

}