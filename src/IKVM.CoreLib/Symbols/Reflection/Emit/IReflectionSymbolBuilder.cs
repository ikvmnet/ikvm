using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    interface IReflectionSymbolBuilder : ISymbolBuilder
    {

    }

    interface IReflectionSymbolBuilder<out TSymbol> : IReflectionSymbolBuilder, ISymbolBuilder<TSymbol>
        where TSymbol : IReflectionSymbol
    {



    }

}
