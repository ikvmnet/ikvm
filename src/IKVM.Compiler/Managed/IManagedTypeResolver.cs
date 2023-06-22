using System.Reflection;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Provides an interface to resolve managed types.
    /// </summary>
    interface IManagedTypeResolver
    {

        /// <summary>
        /// Resolves the given full type name.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        IManagedType Resolve(AssemblyName assemblyName, string typeName);

    }

}
