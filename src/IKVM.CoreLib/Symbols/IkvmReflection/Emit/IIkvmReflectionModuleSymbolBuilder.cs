using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    interface IIkvmReflectionModuleSymbolBuilder : IIkvmReflectionSymbolBuilder<IIkvmReflectionModuleSymbol>, IModuleSymbolBuilder, IIkvmReflectionModuleSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="ModuleBuilder"/>.
        /// </summary>
        ModuleBuilder UnderlyingModuleBuilder { get; }

    }

}
