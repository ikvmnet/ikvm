namespace IKVM.MSBuild.Tasks
{

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection.Metadata;
    using System.Reflection.PortableExecutable;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    /// <summary>
    /// For each <see cref="ReferenceExportItem"/> passed in, assigns default metadata if required.
    /// </summary>
    public class IkvmReferenceExportItemPrepare : Task
    {

        readonly static MD5 md5 = MD5.Create();

        /// <summary>
        /// Calculates the hash of the value.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        static byte[] ComputeHash(byte[] buffer)
        {
            lock (md5)
                return md5.ComputeHash(buffer);
        }

        /// <summary>
        /// Calculates the hash of the value.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        static byte[] ComputeHash(Stream stream)
        {
            lock (md5)
                return md5.ComputeHash(stream);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public IkvmReferenceExportItemPrepare()
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
        /// Optional set of additional references.
        /// </summary>
        public ITaskItem[] References { get; set; }

        /// <summary>
        /// Optional set of additional references.
        /// </summary>
        public ITaskItem[] Libraries { get; set; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            var items = IkvmReferenceExportItem.Import(Items);
            AssignBuildInfo(items);
            return true;
        }

        /// <summary>
        /// Assigns build information to the items.
        /// </summary>
        /// <param name="items"></param>
        internal void AssignBuildInfo(IEnumerable<IkvmReferenceExportItem> items)
        {
            items.AsParallel().ForAll(AssignBuildInfo);
        }

        /// <summary>
        /// Assigns build information to the item.
        /// </summary>
        /// <param name="item"></param>
        internal void AssignBuildInfo(IkvmReferenceExportItem item)
        {
            item.IkvmIdentity = CalculateIkvmIdentity(item);
            item.Save();
        }

        /// <summary>
        /// Calculates the hash for the given item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal string CalculateIkvmIdentity(IkvmReferenceExportItem item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            // identity is already computed
            if (string.IsNullOrWhiteSpace(item.IkvmIdentity) == false)
                return item.IkvmIdentity;

            var writer = new StringWriter();
            writer.WriteLine("ToolVersion={0}", ToolVersion);
            writer.WriteLine("ToolFramework={0}", ToolFramework);
            writer.WriteLine("Assembly={0}", item.ItemSpec);
            writer.WriteLine("Shared={0}", item.Shared);
            writer.WriteLine("NoStdLib={0}", item.NoStdLib);
            writer.WriteLine("Forwarders={0}", item.Forwarders);
            writer.WriteLine("Parameters={0}", item.Parameters);
            writer.WriteLine("JApi={0}", item.JApi);
            writer.WriteLine("Bootstrap={0}", item.Bootstrap);

            // gather reference lines
            var references = new List<string>(16);
            if (References != null)
                foreach (var reference in References)
                    references.Add(GetReferenceLine(item, reference.ItemSpec));
            if (item.References != null)
                foreach (var reference in item.References)
                    references.Add(GetReferenceLine(item, reference));

            // write sorted reference lines
            foreach (var reference in references.OrderBy(i => i))
                writer.WriteLine(reference);

            // gather library lines
            var libraries = new List<string>(16);
            if (item.Libraries != null)
                foreach (var library in item.Libraries.OrderBy(i => i))
                    libraries.Add($"Library={library}");

            // write sorted library lines
            foreach (var library in libraries.OrderBy(i => i))
                writer.WriteLine(library);

            // gather namespaces
            if (item.Namespaces != null)
                foreach (var ns in item.Namespaces.OrderBy(i => i))
                    libraries.Add($"Namespace={ns}");

            // hash the resulting manifest and set the identity
            return GetHashForString(writer.ToString());
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

            var hsh = ComputeHash(Encoding.UTF8.GetBytes(value));
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
        string GetIdentityForFile(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
                throw new ArgumentException($"'{nameof(file)}' cannot be null or whitespace.", nameof(file));
            if (File.Exists(file) == false)
                throw new FileNotFoundException($"Could not find file '{file}'.");

            // file might have a companion SHA1 hash, let's use it, no calculation required
            var sha1File = file + ".sha1";
            if (File.Exists(sha1File))
                if (File.ReadAllText(sha1File) is string h)
                    return $"SHA1:{Regex.Match(h.Trim(), @"^([\w\-]+)").Value}";

            // file might have a companion MD5 hash, let's use it, no calculation required
            var md5File = file + ".md5";
            if (File.Exists(md5File))
                if (File.ReadAllText(md5File) is string h)
                    return $"MD5:{Regex.Match(h.Trim(), @"^([\w\-]+)").Value}";

            // if the file is potentially a .NET assembly
            if (Path.GetExtension(file) == ".dll" || Path.GetExtension(file) == ".exe")
                if (TryGetIdentityForAssembly(file) is string h)
                    return h;

            // fallback to a standard full MD5 of the file
            using var stm = File.OpenRead(file);
            var hsh = ComputeHash(stm);
            var bld = new StringBuilder(hsh.Length * 2);
            foreach (var b in hsh)
                bld.Append(b.ToString("x2"));

            return bld.ToString();
        }

        /// <summary>
        /// Attempts to get an identity value for a file that might be an assembly.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        string TryGetIdentityForAssembly(string file)
        {
            try
            {
                using var fsstm = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read);
                using var perdr = new PEReader(fsstm);
                var mrdr = perdr.GetMetadataReader();
                var mvid = mrdr.GetGuid(mrdr.GetModuleDefinition().Mvid);
                return $"MVID:{mvid}";
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Writes a Reference entry for the given ReferenceExport item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="reference"></param>
        string GetReferenceLine(IkvmReferenceExportItem item, string reference)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (reference is null)
                throw new ArgumentNullException(nameof(reference));

            // Framework references may not be paths
            if (reference.Contains(Path.DirectorySeparatorChar.ToString()) == false)
                return $"Reference={reference}";

            // others should exist
            if (File.Exists(reference) == false)
                throw new FileNotFoundException($"Could not find reference file '{reference}'.");

            return $"Reference={GetIdentityForFile(reference)}";
        }

    }

}
