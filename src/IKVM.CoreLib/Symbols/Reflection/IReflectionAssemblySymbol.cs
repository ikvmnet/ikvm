using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.Reflection
{

    interface IReflectionAssemblySymbol : IReflectionSymbol, IAssemblySymbol
    {

        /// <summary>
        /// Gets a reference to the underlying <see cref="Assembly"/>.
        /// </summary>
        Assembly UnderlyingAssembly { get; }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionModuleSymbol"/> for the specified <see cref="Module"/>.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        IReflectionModuleSymbol GetOrCreateModuleSymbol(Module module);

        /// <summary>
        /// Gets or creates a <see cref="IReflectionModuleSymbolBuilder"/> for the specified <see cref="ModuleBuilder"/>.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        IReflectionModuleSymbolBuilder GetOrCreateModuleSymbol(ModuleBuilder module);

    }

}
