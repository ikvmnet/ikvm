using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    interface IReflectionAssemblySymbolBuilder : IReflectionSymbolBuilder, IAssemblySymbolBuilder, IReflectionAssemblySymbol
    {

        /// <summary>
        /// Gets the underlying <see cref="AssemblyBuilder"/>.
        /// </summary>
        AssemblyBuilder UnderlyingAssemblyBuilder { get; }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionModuleSymbolBuilder"/> for the specified <see cref="ModuleBuilder"/>.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        IReflectionModuleSymbolBuilder GetOrCreateModuleSymbol(ModuleBuilder module);

    }

}
