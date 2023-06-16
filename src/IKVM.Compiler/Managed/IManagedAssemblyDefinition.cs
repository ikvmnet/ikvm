using System.Reflection;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes a manged assembly.
    /// </summary>
    public interface IManagedAssemblyDefinition
    {

        /// <summary>
        /// Gets the name of the assembly.
        /// </summary>
        AssemblyName Name { get; }

    }

}
