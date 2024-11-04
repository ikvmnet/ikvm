namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Describes a type definition.
    /// </summary>
    abstract class DefinitionTypeSymbol : TypeSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="declaringType"></param>
        protected DefinitionTypeSymbol(ISymbolContext context, IModuleSymbol module, TypeSymbol? declaringType) :
            base(context, module, declaringType)
        {

        }

    }

}
