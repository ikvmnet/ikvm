using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace IKVM.MSBuild.Tasks
{

    public class IkvmReferenceExportPrepare : Task
    {

        readonly static MD5 md5 = MD5.Create();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public IkvmReferenceExportPrepare()
        {

        }

        /// <summary>
        /// ReferenceExport items without assigned hashes.
        /// </summary>
        [Required]
        [Output]
        public ITaskItem[] Items { get; set; }

        /// <summary>
        /// IKVM tool version.
        /// </summary>
        [Required]
        public string ToolVersion { get; set; }

        /// <summary>
        /// IKVM target framework.
        /// </summary>
        [Required]
        public string ToolFramework { get; set; }

        /// <summary>
        /// Other references that will be used to generate the assemblies.
        /// </summary>
        [Required]
        public ITaskItem[] References { get; set; }

        [Required]
        public bool Bootstrap { get; set; }

        [Required]
        public bool NoStdLib { get; set; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            AssignBuildInfo(Items);
            return true;
        }

        /// <summary>
        /// Calculates the hash for the given item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal string CalculateIkvmIdentity(ITaskItem item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            // identity is already computed
            if (item.GetMetadata("IkvmIdentity") is string id && string.IsNullOrWhiteSpace(id) == false)
                return id;

            var manifest = new StringWriter();
            manifest.WriteLine("ToolVersion={0}", ToolVersion);
            manifest.WriteLine("ToolFramework={0}", ToolFramework);
            manifest.WriteLine("Bootstrap={0}", item.GetMetadata("Bootstrap"));
            manifest.WriteLine("NoStdLib={0}", item.GetMetadata("NoStdLib"));
            manifest.WriteLine("Assembly={0}", GetHashForFile(item.ItemSpec));

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

            // others should exist
            if (File.Exists(reference.ItemSpec) == false)
                throw new FileNotFoundException($"Could not find reference file '{reference.ItemSpec}'.");

            return $"Reference={GetHashForFile(reference.ItemSpec)}";
        }

        /// <summary>
        /// Assigns build information to the items.
        /// </summary>
        /// <param name="items"></param>
        internal void AssignBuildInfo(ITaskItem[] items)
        {
            foreach (var item in items)
                item.SetMetadata("IkvmIdentity", CalculateIkvmIdentity(item));
        }

    }

}
