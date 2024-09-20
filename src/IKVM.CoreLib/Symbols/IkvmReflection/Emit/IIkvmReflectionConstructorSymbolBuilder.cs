using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    interface IIkvmReflectionConstructorSymbolBuilder : IIkvmReflectionSymbolBuilder, IIkvmReflectionMethodBaseSymbolBuilder, IConstructorSymbolBuilder, IIkvmReflectionConstructorSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="ConstructorBuilder"/>.
        /// </summary>
        ConstructorBuilder UnderlyingConstructorBuilder { get; }

    }

}
