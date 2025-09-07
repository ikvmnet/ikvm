namespace IKVM.CoreLib.Symbols
{

    public class MissingModuleSymbolException : MissingSymbolException
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="symbol"></param>
        public MissingModuleSymbolException(ModuleSymbol symbol) :
            base(symbol)
        {

        }

        /// <summary>
        /// Gets the module symbol that is missing.
        /// </summary>
        public new ModuleSymbol Symbol => (ModuleSymbol)base.Symbol;

    }

}