using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    interface IIkvmReflectionAssemblySymbolBuilder : IIkvmReflectionSymbolBuilder, IAssemblySymbolBuilder, IIkvmReflectionAssemblySymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="AssemblyBuilder"/>.
        /// </summary>
        AssemblyBuilder UnderlyingAssemblyBuilder { get; }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionModuleSymbolBuilder"/> for the specified <see cref="ModuleBuilder"/>.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        IIkvmReflectionModuleSymbolBuilder GetOrCreateModuleSymbol(ModuleBuilder module);

    }

}
