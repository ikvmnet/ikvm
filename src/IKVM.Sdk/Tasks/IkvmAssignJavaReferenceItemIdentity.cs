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
        public ITaskItem[] References { get; set; }

        public override bool Execute()
        {
            foreach (var item in Items)
                GetHashForItem(item);

            return true;
        }

        /// <summary>
        /// Calculates the hash for the given item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        string GetHashForItem(ITaskItem item)
        {
            // identity is already computed
            if (item.GetMetadata("IkvmIdentity") is string id && string.IsNullOrWhiteSpace(id) == false)
                return id;

            var manifest = new StringWriter();
            manifest.WriteLine("AssemblyName={0}", item.GetMetadata("AssemblyName"));
            manifest.WriteLine("AssemblyVersion={0}", item.GetMetadata("AssemblyVersion"));
            manifest.WriteLine("Debug={0}", item.GetMetadata("Debug"));

            // each Compile item should be a jar or class file
            var compiles = new List<string>(16);
            foreach (var compile in item.GetMetadata("Compile").Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                if (compile.EndsWith(".jar") || compile.EndsWith(".class"))
                    compiles.Add(GetCompileLine(item, compile));

            // sort and write the compile lines
            foreach (var c in compiles.OrderBy(i => i))
                manifest.WriteLine(c);

            // each Reference item should resolve to a hash
            var references = new List<string>(16);
            foreach (var reference in item.GetMetadata("Reference").Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                if (string.IsNullOrWhiteSpace(reference) == false)
                    references.Add(GetReferenceLine(item, reference));

            // sort and write the reference lines
            foreach (var r in references.OrderBy(i => i))
                manifest.WriteLine(r);

            // hash the resulting manifest and set the identity
            item.SetMetadata("IkvmIdentity", id = GetHashForString(manifest.ToString()));
            return id;
        }

        /// <summary>
        /// Gets the hash value for the given file.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        string GetHashForString(string value)
        {
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
            if (File.Exists(path) == false)
                throw new FileNotFoundException($"Cannot generate hash for missing file '{path}' on '{item.ItemSpec}'.");

            return $"Compile={GetHashForFile(path)}";
        }

        /// <summary>
        /// Writes a File entry for the given path.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="reference"></param>
        /// <exception cref="FileNotFoundException"></exception>
        string GetReferenceLine(ITaskItem item, string reference)
        {
            var resolved = Items.FirstOrDefault(i => i.ItemSpec == reference);
            if (resolved == null)
                throw new IkvmTaskException($"Could not resolve reference '{reference}' on '{item.ItemSpec}'.");

            return $"Reference={GetHashForItem(resolved)}";
        }

    }

}
