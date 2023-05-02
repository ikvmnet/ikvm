using System;
using System.Reflection;

namespace IKVM.Runtime.Vfs
{

    /// <summary>
    /// Represents a virtual directory for assembly resources.
    /// </summary>
    internal sealed class VfsAssemblyResourceDirectory : VfsDirectory
    {

        readonly Assembly assembly;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assembly"></param>
        internal VfsAssemblyResourceDirectory(VfsContext context, Assembly assembly) :
            base(context)
        {
            this.assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
        }

        /// <summary>
        /// Gets the resource entry in the directory.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override VfsEntry GetEntry(string name)
        {
            return assembly.GetManifestResourceInfo(name) != null ? new VfsAssemblyResourceFile(Context, assembly, name) : null;
        }

        /// <summary>
        /// Gets the list of resource entries within the directory.
        /// </summary>
        /// <returns></returns>
        public override string[] List()
        {
            return assembly.GetManifestResourceNames();
        }

    }

}
