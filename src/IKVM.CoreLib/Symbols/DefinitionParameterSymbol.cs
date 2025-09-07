using System;

namespace IKVM.CoreLib.Symbols
{

    public abstract class DefinitionParameterSymbol : ParameterSymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        protected DefinitionParameterSymbol(SymbolContext context) :
            base(context)
        {

        }

        /// <inheritdoc />
        public sealed override bool ContainsMissing => throw new NotImplementedException();

    }

}
