using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    interface IReflectionModuleSymbolBuilder : IReflectionSymbolBuilder<IReflectionModuleSymbol>, IModuleSymbolBuilder, IReflectionModuleSymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="ModuleBuilder"/>.
        /// </summary>
        ModuleBuilder UnderlyingModuleBuilder { get; }

    }

}
