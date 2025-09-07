namespace IKVM.CoreLib.Symbols
{

    public abstract class DefinitionEventSymbol : EventSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        protected DefinitionEventSymbol(SymbolContext context) :
            base(context)
        {

        }

    }

}
