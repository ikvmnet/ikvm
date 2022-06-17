using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using IKVM.MSBuild.Tasks.Resources;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace IKVM.MSBuild.Tasks
{

    /// <summary>
    /// For each of the <see cref="IkvmReferenceItem"/> items passed in, resolves the various dependencies and calculates the hash.
    /// </summary>
    public class IkvmReferenceItemAssignIdentity : Task
    {

        readonly static MD5 md5 = MD5.Create();

        /// <summary>
        /// IKVM tool version.
        /// </summary>
        [Required]
        public string ToolVersion { get; set; }

        /// <summary>
        /// IKVM target framework.
        /// </summary>
        [Required]
        public string TargetFramework { get; set; }

        /// <summary>
        /// Other references that will be used to generate the assemblies.
        /// </summary>
        [Required]
        public string RuntimeAssembly { get; set; }

        /// <summary>
        /// <see cref="IkvmReferenceItem"/> items without assigned hashes.
        /// </summary>
        [Required]
        [Output]
        public ITaskItem[] Items { get; set; }

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
            try
            {
                if (string.IsNullOrWhiteSpace(RuntimeAssembly))
                    throw new IkvmTaskException("RuntimeAssembly is required.");
                if (string.IsNullOrWhiteSpace(TargetFramework))
                    throw new IkvmTaskException("ToolFramework is required.");
                if (string.IsNullOrWhiteSpace(ToolVersion))
                    throw new IkvmTaskException("ToolVersion is required.");
                if (File.Exists(RuntimeAssembly) == false)
                    throw new FileNotFoundException($"Could not find RuntimeAssembly at '{RuntimeAssembly}'.");

                var items = IkvmReferenceItemUtil.Import(Items);

                // calculate the identity for each item
                foreach (var item in items)
                    item.IkvmIdentity = CalculateIkvmIdentity(item);

                // save each back to the original task item
                foreach (var item in items)
                    item.Save();

                return true;
            }
            catch (IkvmTaskMessageException e)
            {
                Log.LogErrorWithCodeFromResources(e.MessageResourceName, e.MessageArgs);
                return false;
            }
        }

        /// <summary>
        /// Calculates the hash for the given item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        string CalculateIkvmIdentity(IkvmReferenceItem item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            // identity is already computed
            if (item.IkvmIdentity is string id && string.IsNullOrWhiteSpace(id) == false)
                return id;

            if (string.IsNullOrWhiteSpace(item.AssemblyName))
                throw new IkvmTaskMessageException(SR.Error_IkvmInvalidAssemblyName, item, item.AssemblyName);
            if (string.IsNullOrWhiteSpace(item.AssemblyVersion))
                throw new IkvmTaskMessageException(SR.Error_IkvmInvalidAssemblyVersion, item, item.AssemblyVersion);

            var manifest = new StringWriter();
            manifest.WriteLine("ToolVersion={0}", ToolVersion);
            manifest.WriteLine("TargetFramework={0}", TargetFramework);
            manifest.WriteLine("RuntimeAssembly={0}", GetHashForFile(RuntimeAssembly));
            manifest.WriteLine("AssemblyName={0}", item.AssemblyName);
            manifest.WriteLine("AssemblyVersion={0}", item.AssemblyVersion);
            manifest.WriteLine("Debug={0}", item.Debug ? "true" : "false");

            // each Compile item should be a jar or class file
            var compiles = new List<string>(16);
            foreach (var path in item.Compile)
                if (path.EndsWith(".jar") || path.EndsWith(".class"))
                    compiles.Add(GetCompileLine(item, path));

            // sort and write the compile lines
            foreach (var c in compiles.OrderBy(i => i))
                manifest.WriteLine(c);

            // gather reference lines
            var references = new List<string>(16);
            if (References != null)
                foreach (var reference in References)
                    references.Add(GetReferenceLine(item, reference));

            // gather reference lines from metadata
            foreach (var reference in item.References)
                references.Add(GetReferenceLine(item, reference));

            // sort and write the reference lines
            foreach (var r in references.OrderBy(i => i))
                manifest.WriteLine(r);

            // hash the resulting manifest and set the identity
            return GetHashForString(manifest.ToString());
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
        string GetCompileLine(IkvmReferenceItem item, string path)
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
        string GetReferenceLine(IkvmReferenceItem item, ITaskItem reference)
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
        string GetReferenceLine(IkvmReferenceItem item, IkvmReferenceItem reference)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (reference is null)
                throw new ArgumentNullException(nameof(reference));

            return $"Reference={CalculateIkvmIdentity(reference)}";
        }

    }

}
