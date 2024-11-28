using IKVM.CoreLib.Symbols;

namespace IKVM.CoreLib.Tests.Symbols
{

    public abstract class SymbolTestInit<TSymbols> where TSymbols : SymbolContext
    {

        public abstract TSymbols Symbols { get; }

    }

}
