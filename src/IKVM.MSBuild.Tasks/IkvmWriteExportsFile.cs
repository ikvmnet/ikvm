namespace IKVM.MSBuild.Tasks
{

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection.Metadata;
    using System.Reflection.PortableExecutable;
    using System.Text;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    /// <summary>
    /// Writes an 'ikvm.exports' resource naming each of the given assemblies as a wildcard export.
    /// </summary>
    /// <remarks>
    /// The IKVM runtime builds the delegation set of an assembly class loader from the 'ikvm.exports' resource of
    /// the assembly, falling back to the references of the assembly when it carries no such resource. A compiler
    /// only emits a reference to an assembly whose types it actually uses, so a reference declared in the project
    /// but consumed only by name at runtime is absent from the compiled assembly, and the class loader has no way
    /// to reach it. Embedding the resource restores the declared set.
    /// </remarks>
    public class IkvmWriteExportsFile : Task
    {

        /// <summary>
        /// Assemblies to name as wildcard exports.
        /// </summary>
        [Required]
        public ITaskItem[] References { get; set; }

        /// <summary>
        /// Whether to write the references in the reverse of the order they are given in. A lookup walks the
        /// exports in the order they are written.
        /// </summary>
        public bool Reverse { get; set; }

        /// <summary>
        /// Path of the file to write.
        /// </summary>
        [Required]
        public string Output { get; set; }

        /// <summary>
        /// The file that was written, or an empty set when none of the references named an assembly to export.
        /// </summary>
        [Output]
        public ITaskItem[] ExportsFile { get; set; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            if (References is null)
                throw new ArgumentNullException(nameof(References));
            if (string.IsNullOrWhiteSpace(Output))
                throw new ArgumentException($"'{nameof(Output)}' cannot be null or whitespace.", nameof(Output));

            var names = new List<string>(References.Length);
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            var references = Reverse ? Enumerable.Reverse(References) : References;
            foreach (var reference in references)
            {
                var name = GetAssemblyFullName(reference.ItemSpec);
                if (name != null && seen.Add(name))
                    names.Add(name);
            }

            // an assembly that exports nothing is better off with no resource at all: the runtime then reads its
            // assembly references, which is what it would have done before the build started writing one
            if (names.Count == 0)
            {
                Log.LogMessage(MessageImportance.Low, "No assemblies to export, not writing '{0}'.", Output);
                Delete(Output);
                ExportsFile = Array.Empty<ITaskItem>();
                return true;
            }

            WriteIfDifferent(Output, GetExports(names));
            ExportsFile = new ITaskItem[] { new TaskItem(Output) };
            return true;
        }

        /// <summary>
        /// Deletes the file at the given path, if it exists.
        /// </summary>
        /// <param name="path"></param>
        void Delete(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));

            try
            {
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch (Exception e)
            {
                Log.LogMessage(MessageImportance.Low, "Could not remove stale exports file '{0}': {1}", path, e.Message);
            }
        }

        /// <summary>
        /// Gets the full name of the assembly at the given path, or <c>null</c> if it cannot be read.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string GetAssemblyFullName(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return null;

            // an item can name an assembly that was never compiled: it contributes nothing to delegate to
            if (File.Exists(path) == false)
            {
                Log.LogMessage(MessageImportance.Low, "Skipping missing assembly '{0}'.", path);
                return null;
            }

            try
            {
                using var stm = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                using var per = new PEReader(stm);
                var mdr = per.GetMetadataReader();
                if (mdr.IsAssembly == false)
                {
                    Log.LogWarning("Skipping '{0}': not an assembly.", path);
                    return null;
                }

                return mdr.GetAssemblyDefinition().GetAssemblyName().FullName;
            }
            catch (Exception e)
            {
                Log.LogWarning("Skipping '{0}': {1}", path, e.Message);
                return null;
            }
        }

        /// <summary>
        /// Gets the contents of an 'ikvm.exports' resource naming each of the given assemblies as a wildcard
        /// export. The format is that written by the IKVM importer: a count, followed by an assembly name and a
        /// type count per entry, where a type count of zero exports the whole assembly.
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        internal static byte[] GetExports(IReadOnlyList<string> names)
        {
            if (names is null)
                throw new ArgumentNullException(nameof(names));

            var stm = new MemoryStream();
            var wrt = new BinaryWriter(stm, Encoding.UTF8);
            wrt.Write(names.Count);

            foreach (var name in names)
            {
                wrt.Write(name);
                wrt.Write(0);
            }

            wrt.Flush();
            return stm.ToArray();
        }

        /// <summary>
        /// Writes the given contents to the given path, leaving the file untouched if it already has those
        /// contents, so an unchanged reference set does not force the project to recompile.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="contents"></param>
        void WriteIfDifferent(string path, byte[] contents)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));
            if (contents is null)
                throw new ArgumentNullException(nameof(contents));

            try
            {
                if (File.Exists(path) && IsEqual(File.ReadAllBytes(path), contents))
                {
                    Log.LogMessage(MessageImportance.Low, "Exports file '{0}' is up to date.", path);
                    return;
                }
            }
            catch (Exception e)
            {
                Log.LogMessage(MessageImportance.Low, "Could not read existing exports file '{0}': {1}", path, e.Message);
            }

            Log.LogMessage(MessageImportance.Low, "Writing exports file '{0}'.", path);
            File.WriteAllBytes(path, contents);
        }

        /// <summary>
        /// Returns <c>true</c> if the two buffers hold the same bytes.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        static bool IsEqual(byte[] a, byte[] b)
        {
            if (a is null)
                throw new ArgumentNullException(nameof(a));
            if (b is null)
                throw new ArgumentNullException(nameof(b));

            if (a.Length != b.Length)
                return false;

            for (int i = 0; i < a.Length; i++)
                if (a[i] != b[i])
                    return false;

            return true;
        }

    }

}
