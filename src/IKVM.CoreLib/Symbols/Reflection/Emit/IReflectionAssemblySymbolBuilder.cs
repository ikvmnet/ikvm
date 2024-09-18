using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    interface IReflectionAssemblySymbolBuilder : IReflectionSymbolBuilder<IReflectionAssemblySymbol>, IAssemblySymbolBuilder, IReflectionAssemblySymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="AssemblyBuilder"/>.
        /// </summary>
        AssemblyBuilder UnderlyingAssemblyBuilder { get; }

    }

}
