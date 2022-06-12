using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace IKVM.Sdk.Tasks
{

    /// <summary>
    /// For each of the JavaReferenceItem items passed in, resolves the various dependencies and calculates the hash.
    /// </summary>
    public class IkvmAssignJavaReferenceItemIdentity : Task
    {

        readonly static MD5 md5 = MD5.Create();

        /// <summary>
        /// JavaReferenceItem items without assigned hashes.
        /// </summary>
        [Required]
        [Output]
        public ITaskItem[] Items { get; set; }

        /// <summary>
        /// Other references that will be used to generate the assemblies.
        /// </summary>
        [Required]
        public string RuntimeAssembly { get; set; }

        /// <summary>
        /// IKVM tool framework.
        /// </summary>
        [Required]
        public string ToolFramework { get; set; }

        /// <summary>
        /// Other references that will be used to generate the assemblies.
        /// </summary>
        [Required]
        public ITaskItem[] References { get; set; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            if (string.IsNullOrWhiteSpace(RuntimeAssembly))
                throw new IkvmTaskException("RuntimeAssembly is required.");
            if (string.IsNullOrWhiteSpace(ToolFramework))
                throw new IkvmTaskException("ToolFramework is required.");
            if (File.Exists(RuntimeAssembly) == false)
                throw new FileNotFoundException($"Could not find RuntimeAssembly at '{RuntimeAssembly}'.");

            foreach (var item in Items)
                GetHashForItem(item);

            return true;
        }

        /// <summary>
        /// Calculates the hash for the given item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        string GetHashForItem(ITaskItem item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            // identity is already computed
            if (item.GetMetadata(IkvmJavaReferenceItemMetadata.IkvmIdentity) is string id && string.IsNullOrWhiteSpace(id) == false)
                return id;

            if (item.GetMetadata(IkvmJavaReferenceItemMetadata.AssemblyName) is not string assemblyName)
                throw new IkvmTaskException($"Item '{item.ItemSpec}' missing AssemblyName value.");
            if (item.GetMetadata(IkvmJavaReferenceItemMetadata.AssemblyVersion) is not string assemblyVersion)
                throw new IkvmTaskException($"Item '{item.ItemSpec}' missing AssemblyVersion value.");
            if (item.GetMetadata(IkvmJavaReferenceItemMetadata.Debug) is not string debug)
                throw new IkvmTaskException($"Item '{item.ItemSpec}' missing Debug metadata.");

            var manifest = new StringWriter();
            manifest.WriteLine("ToolFramework={0}", ToolFramework);
            manifest.WriteLine("AssemblyName={0}", assemblyName);
            manifest.WriteLine("AssemblyVersion={0}", assemblyVersion);
            manifest.WriteLine("Debug={0}", debug);
            manifest.WriteLine("Runtime={0}", GetHashForFile(RuntimeAssembly));

            // each Compile item should be a jar or class file
            var compiles = new List<string>(16);
            foreach (var compile in item.GetMetadata(IkvmJavaReferenceItemMetadata.Compile).Split(IkvmJavaReferenceItemMetadata.PropertySeperatorCharArray, StringSplitOptions.RemoveEmptyEntries))
                if (compile.EndsWith(".jar") || compile.EndsWith(".class"))
                    compiles.Add(GetCompileLine(item, compile));

            // sort and write the compile lines
            foreach (var c in compiles.OrderBy(i => i))
                manifest.WriteLine(c);

            // gather reference lines
            var references = new List<string>(16);
            if (References != null)
                foreach (var reference in References)
                    references.Add(GetReferenceLine(item, reference));

            // gather reference lines from metadata
            foreach (var reference in item.GetMetadata(IkvmJavaReferenceItemMetadata.References).Split(IkvmJavaReferenceItemMetadata.PropertySeperatorCharArray, StringSplitOptions.RemoveEmptyEntries))
                if (string.IsNullOrWhiteSpace(reference) == false)
                    references.Add(GetReferenceLine(item, reference));

            // sort and write the reference lines
            foreach (var r in references.OrderBy(i => i))
                manifest.WriteLine(r);

            // hash the resulting manifest and set the identity
            item.SetMetadata(IkvmJavaReferenceItemMetadata.IkvmIdentity, id = GetHashForString(manifest.ToString()));
            return id;
        }

        /// <summary>
        /// Gets the hash value for the given file.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        string GetHashForString(string value)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            var hsh = md5.ComputeHash(Encoding.UTF8.GetBytes(value));
            var bld = new StringBuilder(hsh.Length * 2);
            foreach (var b in hsh)
                bld.Append(b.ToString("x2"));

            return bld.ToString();
        }

        /// <summary>
        /// Gets the hash value for the given file.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        string GetHashForFile(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
                throw new ArgumentException($"'{nameof(file)}' cannot be null or whitespace.", nameof(file));
            if (File.Exists(file) == false)
                throw new FileNotFoundException($"Could not find file '{file}'.");

            using var stm = File.OpenRead(file);
            var hsh = md5.ComputeHash(stm);
            var bld = new StringBuilder(hsh.Length * 2);
            foreach (var b in hsh)
                bld.Append(b.ToString("x2"));

            return bld.ToString();
        }

        /// <summary>
        /// Writes a File entry for the given path.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="path"></param>
        /// <exception cref="FileNotFoundException"></exception>
        string GetCompileLine(ITaskItem item, string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace.", nameof(path));
            if (File.Exists(path) == false)
                throw new FileNotFoundException($"Cannot generate hash for missing file '{path}' on '{item.ItemSpec}'.");

            return $"Compile={GetHashForFile(path)}";
        }

        /// <summary>
        /// Writes a Reference entry for the given Reference item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="reference"></param>
        string GetReferenceLine(ITaskItem item, ITaskItem reference)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (reference is null)
                throw new ArgumentNullException(nameof(reference));

            // Framework references may not be paths
            if (reference.ItemSpec.Contains(Path.DirectorySeparatorChar) == false)
                return $"Reference={reference.ItemSpec}";

            // others should exist
            if (File.Exists(reference.ItemSpec) == false)
                throw new FileNotFoundException($"Could not find reference file '{reference.ItemSpec}'.");

            return $"Reference={GetHashForFile(reference.ItemSpec)}";
        }

        /// <summary>
        /// Writes a Reference entry for the given path.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="reference"></param>
        string GetReferenceLine(ITaskItem item, string reference)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (string.IsNullOrWhiteSpace(reference))
                throw new ArgumentException($"'{nameof(reference)}' cannot be null or whitespace.", nameof(reference));

            var resolved = Items.FirstOrDefault(i => i.ItemSpec == reference);
            if (resolved == null)
                throw new IkvmTaskException($"Could not resolve reference '{reference}' on '{item.ItemSpec}'.");

            return $"Reference={GetHashForItem(resolved)}";
        }

    }

}
