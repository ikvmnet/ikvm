namespace IKVM.CoreLib.Symbols
{

    abstract class DefinitionParameterSymbol : ParameterSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="declaringMember"></param>
        /// <param name="position"></param>
        protected DefinitionParameterSymbol(SymbolContext context, MemberSymbol declaringMember, int position) : 
            base(context, declaringMember, position)
        {

        }

    }

}
