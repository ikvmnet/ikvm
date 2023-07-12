using System.Reflection;

namespace IKVM.Compiler.Managed.Reflection
{

    /// <summary>
    /// Provides the capability of resolver a <see cref="Assembly"/>.
    /// </summary>
    public interface IReflectionAssemblyResolver
    {

        /// <summary>
        /// Loads the reflection assembly for the given assembly name.
        /// </summary>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        Assembly? Resolve(string assemblyName);

    }

}