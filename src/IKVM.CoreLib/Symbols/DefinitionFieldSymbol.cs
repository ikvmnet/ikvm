namespace IKVM.CoreLib.Symbols
{

    abstract class DefinitionFieldSymbol : FieldSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="declaringType"></param>
        protected DefinitionFieldSymbol(SymbolContext context, ModuleSymbol module, DefinitionTypeSymbol declaringType) :
            base(context, module, declaringType)
        {

        }

    }

}
