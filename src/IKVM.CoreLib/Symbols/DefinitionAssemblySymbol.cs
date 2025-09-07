using System;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Represents an assembly definition.
    /// </summary>
    public abstract class DefinitionAssemblySymbol : AssemblySymbol
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected DefinitionAssemblySymbol(SymbolContext context) :
            base(context)
        {

        }

    }

}
