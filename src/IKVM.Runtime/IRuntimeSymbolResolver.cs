using System;

using IKVM.CoreLib.Symbols;

#if IMPORTER || EXPORTER
using IKVM.Reflection;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
#endif

namespace IKVM.Runtime
{

    /// <summary>
    /// Provides an interface to resolve a managed type symbols extended for the Runtime classes.
    /// </summary>
    interface IRuntimeSymbolResolver : ISymbolResolver
    {

        /// <summary>
        /// Gets the <see cref="ISymbolContext"/> that manages access to symbols.
        /// </summary>
        ISymbolContext Symbols { get; }

        /// <summary>
        /// Gets the <see cref="IAssemblySymbol"/> associated with the specified type.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        IAssemblySymbol ResolveAssembly(Assembly assembly);

        /// <summary>
        /// Gets the <see cref="IModuleSymbol"/> associated with the specified type.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        IModuleSymbol ResolveModule(Module module);

        /// <summary>
        /// Gets the <see cref="ITypeSymbol"/> associated with the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        ITypeSymbol ResolveType(Type type);

    }


}
