using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    interface IIkvmReflectionConstructorSymbolBuilder : IIkvmReflectionSymbolBuilder<IIkvmReflectionConstructorSymbol>, IIkvmReflectionMethodBaseSymbolBuilder, IConstructorSymbolBuilder, IIkvmReflectionConstructorSymbol
    {

        ConstructorBuilder UnderlyingConstructorBuilder { get; }

    }

}
