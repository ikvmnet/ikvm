namespace IKVM.Compiler.Managed.Reader
{

    /// <summary>
    /// Provides an interface by which to load managed assemblies.
    /// </summary>
    internal interface IManagedAssemblyResolver
    {

        /// <summary>
        /// Resolves the specified assembly name.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        ManagedAssembly? ResolveAssembly(string assemblyName);

    }

}
