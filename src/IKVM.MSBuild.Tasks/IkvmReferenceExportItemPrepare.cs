namespace IKVM.MSBuild.Tasks
{

    using System;
    using System.Buffers.Binary;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    using Microsoft.Build.Framework;

    /// <summary>
    /// For each <see cref="ReferenceExportItem"/> passed in, assigns default metadata if required.
    /// </summary>
    public class IkvmReferenceExportItemPrepare : Microsoft.Build.Utilities.Task, ICancelableTask
    {

        const string XML_ROOT_ELEMENT_NAME = "IkvmReferenceExportItemPrepareState";
        const string XML_ASSEMBLY_INFO_STATE_ELEMENT_NAME = "AssemblyInfoState";
        const string XML_FILE_IDENTITY_STATE_ELEMENT_NAME = "FileIdentityState";

        /// <summary>
        /// Calculates the hash of the value.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        static byte[] ComputeHash(byte[] buffer)
        {
            using var md5 = MD5.Create();
            return md5.ComputeHash(buffer);
        }

        readonly CancellationTokenSource cts;
        readonly RandomNumberGenerator rng = RandomNumberGenerator.Create();
        readonly IkvmFileIdentityUtil fileIdentityUtil;
        readonly IkvmAssemblyInfoUtil assemblyInfoUtil;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public IkvmReferenceExportItemPrepare()
        {
            cts = new CancellationTokenSource();
            assemblyInfoUtil = new();
            fileIdentityUtil = new(assemblyInfoUtil);
        }

        /// <summary>
        /// Optional path to a state file to preserve between executions.
        /// </summary>
        public string StateFile { get; set; }

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
            if (StateFile != null)
                StateFile = Path.GetFullPath(StateFile);

            if (cts.IsCancellationRequested)
                return false;

            // wait for result, and ensure we reacquire in case of return value or exception
            Task<bool> run;

            try
            {
                // kick off the launcher with the configured options
                run = ExecuteAsync(cts.Token);
                if (run.IsCompleted)
                    return run.GetAwaiter().GetResult();
            }
            catch (OperationCanceledException)
            {
                return false;
            }

            // yield and wait for the task to complete
            BuildEngine3.Yield();

            var result = false;
            try
            {
                result = run.GetAwaiter().GetResult();
            }
            catch (OperationCanceledException)
            {
                return false;
            }
            finally
            {
                BuildEngine3.Reacquire();
            }

            // check that we exited successfully
            return result;
        }

        /// <summary>
        /// Signals the task to cancel.
        /// </summary>
        public void Cancel()
        {
            cts.Cancel();
        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        async Task<bool> ExecuteAsync(CancellationToken cancellationToken)
        {
            LoadState();

            // execute task and return newly sorted items
            var items = IkvmReferenceExportItem.Import(Items);
            await AssignBuildInfoAsync(items, cancellationToken);
            Items = items.OrderBy(i => i.RandomIndex).Select(i => i.Item).ToArray(); // randomize order to allow multiple processes to interleave

            await SaveStateAsync();
            return true;
        }

        /// <summary>
        /// Attempts to load the state file.
        /// </summary>
        /// <param name="cancellationToken"></param>
        internal void LoadState()
        {
            if (StateFile != null && File.Exists(StateFile))
            {
                try
                {
                    var stateFileXml = XDocument.Load(StateFile);
                    var stateFileRoot = stateFileXml.Element(XML_ROOT_ELEMENT_NAME);
                    if (stateFileRoot != null)
                    {
                        var assemblyInfoStateXml = stateFileRoot.Element(XML_ASSEMBLY_INFO_STATE_ELEMENT_NAME);
                        if (assemblyInfoStateXml != null)
                        {
                            assemblyInfoUtil.LoadStateXml(assemblyInfoStateXml);
                            Log.LogMessage(MessageImportance.Low, "Successfully loaded assembly info state.");
                        }

                        var fileIdentityStateXml = stateFileRoot.Element(XML_FILE_IDENTITY_STATE_ELEMENT_NAME);
                        if (fileIdentityStateXml != null)
                        {
                            fileIdentityUtil.LoadStateXml(fileIdentityStateXml);
                            Log.LogMessage(MessageImportance.Low, "Successfully loaded file identity state.");
                        }
                    }
                }
                catch
                {
                    Log.LogWarning("Could not load IkvmReferenceExportItemPrepare state file. File is potentially corrupt.");
                }
            }
        }

        /// <summary>
        /// Attempts to save the state file.
        /// </summary>
        /// <returns></returns>
        internal async Task SaveStateAsync()
        {
            if (StateFile != null)
            {
                var root = new XElement(XML_ROOT_ELEMENT_NAME);

                var assemblyInfoStateXml = new XElement(XML_ASSEMBLY_INFO_STATE_ELEMENT_NAME);
                await assemblyInfoUtil.SaveStateXmlAsync(assemblyInfoStateXml);
                root.Add(assemblyInfoStateXml);

                var fileIdentityStateXml = new XElement(XML_FILE_IDENTITY_STATE_ELEMENT_NAME);
                await fileIdentityUtil.SaveStateXmlAsync(fileIdentityStateXml);
                root.Add(fileIdentityStateXml);

                var dir = Path.GetDirectoryName(StateFile);
                if (Directory.Exists(dir) == false)
                    Directory.CreateDirectory(dir);

                root.Save(StateFile);
            }
        }

        /// <summary>
        /// Assigns build information to the items.
        /// </summary>
        /// <param name="items"></param>
        internal Task AssignBuildInfoAsync(IEnumerable<IkvmReferenceExportItem> items, CancellationToken cancellationToken)
        {
            return Task.WhenAll(items.Select(i => AssignBuildInfoAsync(i, cancellationToken)));
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
        internal async Task AssignBuildInfoAsync(IkvmReferenceExportItem item, CancellationToken cancellationToken)
        {
            item.References = await CalculateReferencesAsync(item, cancellationToken);
            item.Libraries = await CalculateLibrariesAsync(item, cancellationToken);
            item.IkvmIdentity = await CalculateIkvmIdentityAsync(item, cancellationToken);
            item.RandomIndex ??= GetRandomNumber();
            item.Save();
        }

        /// <summary>
        /// Calculates the direct references of the given item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<List<string>> CalculateReferencesAsync(IkvmReferenceExportItem item, CancellationToken cancellationToken)
        {
            var referencesList = new HashSet<string>() { item.ItemSpec };

            if (References != null)
                foreach (var reference in References)
                    referencesList.Add(reference.ItemSpec);

            if (item.References != null)
                foreach (var reference in item.References)
                    referencesList.Add(reference);

            var references = await GetAssemblyReferencesAsync(item.ItemSpec, referencesList, cancellationToken);
            return references.OrderBy(i => i).ToList();
        }

        /// <summary>
        /// Calculates the direct libraries of the given item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<string>> CalculateLibrariesAsync(IkvmReferenceExportItem item, CancellationToken cancellationToken)
        {
            var librariesList = new HashSet<string>();

            if (Libraries != null)
                foreach (var library in Libraries)
                    librariesList.Add(library.ItemSpec);

            if (item.Libraries != null)
                foreach (var library in item.Libraries)
                    librariesList.Add(library);

            var libraries = librariesList.OrderBy(i => i).ToList();
            return Task.FromResult(libraries);
        }

        /// <summary>
        /// Calculates the hash for the given item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal async Task<string> CalculateIkvmIdentityAsync(IkvmReferenceExportItem item, CancellationToken cancellationToken)
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
            writer.WriteLine("IncludeNonPublicTypes={0}", item.IncludeNonPublicTypes);
            writer.WriteLine("IncludeNonPublicInterfaces={0}", item.IncludeNonPublicInterfaces);
            writer.WriteLine("IncludeNonPublicMembers={0}", item.IncludeNonPublicMembers);
            writer.WriteLine("IncludeParameterNames={0}", item.IncludeParameterNames);
            writer.WriteLine("Bootstrap={0}", item.Bootstrap);

            // traverse the reference set for references that are actually referenced
            foreach (var reference in item.References)
                writer.WriteLine($"Reference={await GetIdentityAsync(reference, cancellationToken)}");

            // gather library lines
            foreach (var library in item.Libraries)
                writer.WriteLine($"Library={await GetIdentityAsync(library, cancellationToken)}");

            // gather namespaces
            if (item.Namespaces != null)
                foreach (var ns in item.Namespaces.Distinct().OrderBy(i => i))
                    writer.WriteLine($"Namespace={ns}");

            // hash the resulting manifest and set the identity
            return GetHashForString(writer.ToString());
        }

        /// <summary>
        /// Gets the identity for a given value. Value may be a file path.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cancellationToken"></param>
        async Task<string> GetIdentityAsync(string value, CancellationToken cancellationToken)
        {
            if (value is null)
                throw new ArgumentNullException(nameof(value));

            // Framework references may not be paths
            if (value.Contains(Path.DirectorySeparatorChar.ToString()) == false)
                return value;

            // resolve absolute directory path, but can't acquire an identity for a directory in any other way
            if (Directory.Exists(value))
                return value;

            // others should exist
            var identity = await fileIdentityUtil.GetIdentityForFileAsync(value, Log, cancellationToken);
            if (identity != null)
                return identity;

            throw new Exception($"Could not resolve identity for '{value}'.");
        }

        /// <summary>
        /// Finds all of the direct and indirect references of the given assembly.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="referencesList"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<IEnumerable<string>> GetAssemblyReferencesAsync(string path, IEnumerable<string> referencesList, CancellationToken cancellationToken)
        {
            var hs = new HashSet<string>();

            // recurse into references of path
            await BuildAssemblyReferencesAsync(path, referencesList, hs, cancellationToken);

            // ensure the required libraries are present
            foreach (var n in new[] { "IKVM.Runtime", "IKVM.Java" })
            {
                foreach (var i in referencesList)
                    if (await assemblyInfoUtil.GetAssemblyInfoAsync(i, Log, cancellationToken) is IkvmAssemblyInfoUtil.AssemblyInfo a && a.Name == n)
                        hs.Add(i);
            }

            return hs;
        }

        /// <summary>
        /// Finds all of the direct and indirect references of the given assembly.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="referencesList"></param>
        /// <param name="hs"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task BuildAssemblyReferencesAsync(string path, IEnumerable<string> referencesList, HashSet<string> hs, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (await assemblyInfoUtil.GetAssemblyInfoAsync(path, Log, cancellationToken) is IkvmAssemblyInfoUtil.AssemblyInfo a)
                foreach (var reference in a.References)
                    foreach (var i in referencesList)
                        if ((await assemblyInfoUtil.GetAssemblyInfoAsync(i, Log, cancellationToken)) is IkvmAssemblyInfoUtil.AssemblyInfo a2 && a2.Name == reference)
                            if (hs.Add(i))
                                await BuildAssemblyReferencesAsync(i, referencesList, hs, cancellationToken);
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

    }

}
