namespace IKVM.CoreLib.Symbols
{

    public class MissingAssemblySymbolException : MissingSymbolException
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="symbol"></param>
        public MissingAssemblySymbolException(AssemblySymbol symbol) :
            base(symbol)
        {

        }

        /// <summary>
        /// Gets the assembly symbol that is missing.
        /// </summary>
        public new AssemblySymbol Symbol => (AssemblySymbol)base.Symbol;

    }

}