namespace IKVM.CoreLib.Symbols
{

    abstract class DefinitionEventSymbol : EventSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="declaringType"></param>
        protected DefinitionEventSymbol(ISymbolContext context, IModuleSymbol module, DefinitionTypeSymbol declaringType) : 
            base(context, module, declaringType)
        {

        }

    }

}
