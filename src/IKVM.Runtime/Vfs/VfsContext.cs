using System.Collections.Generic;
using System.Reflection;

namespace IKVM.Runtime.Vfs
{

    /// <summary>
    /// Provides information to the VFS about the environment.
    /// </summary>
    internal abstract class VfsContext
    {

        /// <summary>
        /// Gets all of the assemblies known by the VFS.
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<AssemblyName> GetAssemblyNames();

        /// <summary>
        /// Gets an assembly by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract Assembly GetAssembly(AssemblyName name);

    }

}
