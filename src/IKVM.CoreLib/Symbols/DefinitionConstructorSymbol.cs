namespace IKVM.CoreLib.Symbols
{

    abstract class DefinitionConstructorSymbol : ConstructorSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringType"></param>
        protected DefinitionConstructorSymbol(SymbolContext context, DefinitionTypeSymbol declaringType) :
            base(context, declaringType)
        {

        }

    }

}
