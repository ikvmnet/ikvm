namespace IKVM.MSBuild.Tasks
{

    using System;
    using System.Buffers.Binary;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection.Metadata;
    using System.Reflection.PortableExecutable;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    /// <summary>
    /// For each <see cref="ReferenceExportItem"/> passed in, assigns default metadata if required.
    /// </summary>
    public class IkvmReferenceExportItemPrepare : Task
    {

        readonly static RandomNumberGenerator rng = RandomNumberGenerator.Create();

        readonly static MD5 md5 = MD5.Create();
        readonly static ConcurrentDictionary<(string, DateTime), System.Threading.Tasks.Task<string>> fileIdentityCache = new ConcurrentDictionary<(string, DateTime), System.Threading.Tasks.Task<string>>();

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
        /// <exception cref="NullReferenceException"></exception>
        public override bool Execute()
        {
            // kick off the launcher with the configured options
            var run = ExecuteAsync(CancellationToken.None);

            // return immediately if finished
            if (run.IsCompleted)
                return run.GetAwaiter().GetResult();

            // yield and wait for the task to complete
            BuildEngine3.Yield();
            var rsl = run.GetAwaiter().GetResult();
            BuildEngine3.Reacquire();

            // check that we exited successfully
            return rsl;
        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        async System.Threading.Tasks.Task<bool> ExecuteAsync(CancellationToken cancellationToken)
        {
            var items = IkvmReferenceExportItem.Import(Items);
            await AssignBuildInfoAsync(items, cancellationToken);
            Items = items.OrderBy(i => i.RandomIndex).Select(i => i.Item).ToArray(); // randomize order to allow multiple processes to interleave
            return true;
        }

        /// <summary>
        /// Assigns build information to the items.
        /// </summary>
        /// <param name="items"></param>
        internal System.Threading.Tasks.Task AssignBuildInfoAsync(IEnumerable<IkvmReferenceExportItem> items, CancellationToken cancellationToken)
        {
            return System.Threading.Tasks.Task.WhenAll(items.Select(i => AssignBuildInfoAsync(i, cancellationToken)));
        }

        /// <summary>
        /// Generates a unique random number for this export item.
        /// </summary>
        /// <returns></returns>
        int GetRandomNumber()
        {
            var b = new byte[4];
            rng.GetBytes(b);
            return BinaryPrimitives.ReadInt32LittleEndian(b);
        }

        /// <summary>
        /// Assigns build information to the item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="cancellationToken"></param>
        internal async System.Threading.Tasks.Task AssignBuildInfoAsync(IkvmReferenceExportItem item, CancellationToken cancellationToken)
        {
            item.IkvmIdentity = await CalculateIkvmIdentityAsync(item, cancellationToken);
            item.RandomIndex ??= GetRandomNumber();
            item.Save();
        }

        /// <summary>
        /// Calculates the hash for the given item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal async System.Threading.Tasks.Task<string> CalculateIkvmIdentityAsync(IkvmReferenceExportItem item, CancellationToken cancellationToken)
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
                    references.Add(await GetIdentityAsync(item, reference.ItemSpec, cancellationToken));
            if (item.References != null)
                foreach (var reference in item.References)
                    references.Add(await GetIdentityAsync(item, reference, cancellationToken));

            // write sorted reference lines
            foreach (var reference in references.OrderBy(i => i))
                writer.WriteLine($"Reference={reference}");

            // gather library lines
            var libraries = new List<string>(16);
            if (Libraries != null)
                foreach (var library in Libraries)
                    libraries.Add(await GetIdentityAsync(item, library.ItemSpec, cancellationToken));
            if (item.Libraries != null)
                foreach (var library in item.Libraries)
                    libraries.Add(await GetIdentityAsync(item, library, cancellationToken));

            // write sorted library lines
            foreach (var library in libraries.OrderBy(i => i))
                writer.WriteLine($"Library={library}");

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
        async System.Threading.Tasks.Task<string> CreateIdentityForFileAsync(string file)
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
                if (await TryGetIdentityForAssemblyAsync(file) is string h)
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
        System.Threading.Tasks.Task<string> TryGetIdentityForAssemblyAsync(string file)
        {
            return System.Threading.Tasks.Task.Run(() =>
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
            });
        }

        /// <summary>
        /// Gets the hash value for the given file.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        System.Threading.Tasks.Task<string> GetIdentityForFileAsync(string file, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(file))
                throw new ArgumentException($"'{nameof(file)}' cannot be null or whitespace.", nameof(file));
            if (File.Exists(file) == false)
                throw new FileNotFoundException($"Could not find file '{file}'.");

            return fileIdentityCache.GetOrAdd((file, File.GetLastWriteTimeUtc(file)), _ => CreateIdentityForFileAsync(_.Item1));
        }

        /// <summary>
        /// Gets the identity for a given value. Value may be a file path.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        async System.Threading.Tasks.Task<string> GetIdentityAsync(IkvmReferenceExportItem item, string value, CancellationToken cancellationToken)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            // Framework references may not be paths
            if (value.Contains(Path.DirectorySeparatorChar.ToString()) == false)
                return value;

            // resolve absolute directory path, but can't acquire an identity for a directory in any other way
            if (Directory.Exists(value))
                return value;

            // others should exist
            if (File.Exists(value))
                return await GetIdentityForFileAsync(value, cancellationToken);

            throw new Exception($"Could not resolve identity for '{value}'.");
        }

    }

}
