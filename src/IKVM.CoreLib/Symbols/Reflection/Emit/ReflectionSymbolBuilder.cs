using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    /// <summary>
    /// Reflection-specific implementation of <see cref="ISymbolBuilder"/>.
    /// </summary>
    abstract class ReflectionSymbolBuilder : ReflectionSymbol, IReflectionSymbolBuilder
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        protected ReflectionSymbolBuilder(ReflectionSymbolContext context) :
            base(context)
        {

        }

    }

}
