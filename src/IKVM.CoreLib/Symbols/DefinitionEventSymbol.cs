namespace IKVM.CoreLib.Symbols
{

    abstract class DefinitionEventSymbol : EventSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        protected DefinitionEventSymbol(SymbolContext context, DefinitionTypeSymbol declaringType) : 
            base(context, declaringType)
        {

        }

    }

}
