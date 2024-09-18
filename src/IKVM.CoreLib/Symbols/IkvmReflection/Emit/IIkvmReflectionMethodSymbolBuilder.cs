using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    interface IIkvmReflectionMethodSymbolBuilder : IIkvmReflectionSymbolBuilder<IIkvmReflectionMethodSymbol>, IIkvmReflectionMethodBaseSymbolBuilder, IMethodSymbolBuilder, IIkvmReflectionMethodSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="MethodBuilder"/>.
        /// </summary>
        MethodBuilder UnderlyingMethodBuilder { get; }

    }

}
