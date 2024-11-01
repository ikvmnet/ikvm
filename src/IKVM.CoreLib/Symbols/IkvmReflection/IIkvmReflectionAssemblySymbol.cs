using IKVM.Reflection;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    interface IIkvmReflectionAssemblySymbol : IIkvmReflectionSymbol, IAssemblySymbol
    {

        /// <summary>
        /// Gets a reference to the underlying <see cref="Assembly"/>.
        /// </summary>
        Assembly UnderlyingAssembly { get; }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionModuleSymbol"/> for the specified <see cref="Module"/>.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        IIkvmReflectionModuleSymbol GetOrCreateModuleSymbol(Module module);

    }

}
