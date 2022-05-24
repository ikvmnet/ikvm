using System;
using System.Collections.Concurrent;
using System.Linq;

namespace IKVM.Runtime.Vfs
{

    /// <summary>
    /// Describes a directory that has entries added to it statically.
    /// </summary>
    sealed class VfsEntryDirectory : VfsDirectory
    {

        readonly ConcurrentDictionary<string, VfsEntry> entries = new();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public VfsEntryDirectory(VfsContext context) :
            base(context)
        {

        }

        /// <summary>
        /// Adds a new directory into the directory, or returns the existing one.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public VfsDirectory GetOrCreateDirectory(string name)
        {
            return (VfsDirectory)entries.GetOrAdd(name, _ => new VfsEntryDirectory(Context));
        }

        /// <summary>
        /// Adds a new entry into the directory.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="entry"></param>
        public VfsEntry AddEntry(string name, VfsEntry entry)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (entry is null)
                throw new ArgumentNullException(nameof(entry));

            return entries.AddOrUpdate(name, entry, (_, __) => entry);
        }

        /// <summary>
        /// Gets the entry with the given name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override VfsEntry GetEntry(string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            return entries.TryGetValue(name, out VfsEntry entry) ? entry : null;
        }

        /// <summary>
        /// Lists the contents of the directory.
        /// </summary>
        /// <returns></returns>
        public override string[] List() => entries.Keys.ToArray();

    }

}
