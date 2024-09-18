using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    interface IIkvmReflectionAssemblySymbolBuilder : IIkvmReflectionSymbolBuilder<IIkvmReflectionAssemblySymbol>, IAssemblySymbolBuilder, IIkvmReflectionAssemblySymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="AssemblyBuilder"/>.
        /// </summary>
        AssemblyBuilder UnderlyingAssemblyBuilder { get; }

    }

}
