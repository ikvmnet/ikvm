using System;

using IKVM.CoreLib.Symbols;

#if IMPORTER || EXPORTER
using Type = IKVM.Reflection.Type;
#endif

namespace IKVM.Runtime
{

    /// <summary>
    /// Provides an interface to resolve a managed type symbols extended for the Runtime classes.
    /// </summary>
    interface IRuntimeSymbolResolver : ISymbolResolver
    {

        /// <summary>
        /// Gets the <see cref="ITypeSymbol"/> associated with the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        ITypeSymbol ResolveType(Type type);

    }


}
