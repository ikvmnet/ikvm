using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    interface IIkvmReflectionSymbolBuilder : ISymbolBuilder
    {

    }

    interface IIkvmReflectionSymbolBuilder<out TSymbol> : IIkvmReflectionSymbolBuilder, ISymbolBuilder<TSymbol>
        where TSymbol : IIkvmReflectionSymbol
    {



    }

}
