namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Provides an interface to resolve managed type symbols.
    /// </summary>
    interface ISymbolResolver
    {

        /// <summary>
        /// Resolves the named assembly from any reference source.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        IAssemblySymbol? ResolveAssembly(string assemblyName);

        /// <summary>
        /// Resolves the named type from the core assembly.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        TypeSymbol ResolveCoreType(string typeName);

        /// <summary>
        /// Resolves the named type from any reference source.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        TypeSymbol? ResolveType(string typeName);

    }

}
