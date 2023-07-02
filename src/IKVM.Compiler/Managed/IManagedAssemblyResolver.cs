using System.Reflection;
using System.Threading.Tasks;

namespace IKVM.Compiler.Managed
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
        ValueTask<ManagedAssembly?> ResolveAsync(AssemblyName assemblyName);

    }

}
