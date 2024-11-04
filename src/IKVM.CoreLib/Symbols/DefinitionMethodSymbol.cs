namespace IKVM.CoreLib.Symbols
{

    abstract class DefinitionMethodSymbol : MethodSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="declaringType"></param>
        protected DefinitionMethodSymbol(ISymbolContext context, IModuleSymbol module, DefinitionTypeSymbol declaringType) : 
            base(context, module, declaringType)
        {

        }

    }

}
