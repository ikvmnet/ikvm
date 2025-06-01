using System.Linq;

namespace IKVM.CoreLib.Symbols
{

    public abstract class DefinitionGenericMethodParameterTypeSymbol : GenericMethodParameterTypeSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        protected DefinitionGenericMethodParameterTypeSymbol(SymbolContext context) :
            base(context)
        {

        }

        /// <inheritdoc />
        public sealed override bool ContainsMissingType => GenericParameterConstraints.Any(i => i.ContainsMissingType);

    }

}
