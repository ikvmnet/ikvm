namespace IKVM.Runtime.Vfs
{

    /// <summary>
    /// Represents a directory entry in the virtual file system.
    /// </summary>
    internal abstract class VfsDirectory : VfsEntry
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public VfsDirectory(VfsContext context) : 
            base(context)
        {

        }

        /// <summary>
        /// Gets the entry with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public abstract VfsEntry GetEntry(string name);

        /// <summary>
        /// Returns the names of the entries within the directory.
        /// </summary>
        /// <returns></returns>
        public abstract string[] List();

    }

}
