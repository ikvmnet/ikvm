namespace IKVM.CoreLib.Symbols
{

    abstract class DefinitionPropertySymbol : PropertySymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        protected DefinitionPropertySymbol(SymbolContext context, DefinitionTypeSymbol declaringType) :
            base(context, declaringType)
        {

        }

    }

}
