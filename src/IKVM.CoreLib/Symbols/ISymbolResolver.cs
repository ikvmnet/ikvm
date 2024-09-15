namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Provides an interface to resolve a managed type symbols.
    /// </summary>
    interface ISymbolResolver
    {

        /// <summary>
        /// Resolves the named type from the IKVM runtime assembly.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        ITypeSymbol? ResolveRuntimeType(string typeName);

        /// <summary>
        /// Resolves the named assembly from any reference source.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        IAssemblySymbol? ResolveAssembly(string assemblyName);

        /// <summary>
        /// Resolves the named type from any reference source.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        ITypeSymbol? ResolveCoreType(string typeName);

        /// <summary>
        /// Resolves the known Java base assembly.
        /// </summary>
        /// <returns></returns>
        IAssemblySymbol? ResolveBaseAssembly();

    }

}
