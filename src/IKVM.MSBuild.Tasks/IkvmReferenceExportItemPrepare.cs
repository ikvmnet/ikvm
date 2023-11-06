namespace IKVM.MSBuild.Tasks
{

    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    using Microsoft.Build.Framework;

    /// <summary>
    /// For each <see cref="IkvmReferenceExportItem"/> passed in, assigns default metadata if required.
    /// </summary>
    public class IkvmReferenceExportItemPrepare : IkvmAsyncTask
    {

        const string XML_ROOT_ELEMENT_NAME = "IkvmReferenceExportItemPrepareState";
        const string XML_ASSEMBLY_INFO_STATE_ELEMENT_NAME = "AssemblyInfoState";
        const string XML_FILE_IDENTITY_STATE_ELEMENT_NAME = "FileIdentityState";

        /// <summary>
        /// Generates a unique random number for this export item.
        /// </summary>
        /// <returns></returns>
        static int GetRandomNumber() => Guid.NewGuid().GetHashCode();

        readonly IkvmFileIdentityUtil fileIdentityUtil;
        readonly IkvmAssemblyInfoUtil assemblyInfoUtil;
        readonly Dictionary<string, IkvmAssemblyInfoUtil.AssemblyInfo> assemblyInfoByName = new();
        readonly ConcurrentDictionary<string, Task<string[]>> assemblyReferenceCache = new();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public IkvmReferenceExportItemPrepare()
        {
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
        [Required]
        public ITaskItem[] References { get; set; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            if (StateFile != null)
                StateFile = Path.GetFullPath(StateFile);

            return base.Execute();
        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        protected override async Task<bool> ExecuteAsync(CancellationToken cancellationToken)
        {
            var sw = new Stopwatch();
            sw.Start();

            LoadState();

            // execute task and return newly sorted items
            var items = IkvmReferenceExportItem.Import(Items);
            await PreLoadAssembliesByNameAsync(items, cancellationToken);
            await AssignBuildInfoAsync(items, cancellationToken);
            Items = items.OrderBy(i => i.RandomIndex).Select(i => i.Item).ToArray(); // randomize order to allow multiple processes to interleave

            await SaveStateAsync();

            sw.Stop();
            Log.LogMessage("Total time spent in IkvmReferenceExportItemPrepare: {0}", sw.Elapsed);

            return true;
        }

        /// <summary>
        /// Attempts to load the state file.
        /// </summary>
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
                catch (Exception e)
                {
                    Log.LogWarning("Could not load IkvmReferenceExportItemPrepare state file. File is potentially corrupt. {0}", e.Message);
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
        /// Preloads all of the known assemblies and generates a dictionary by name.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task PreLoadAssembliesByNameAsync(IEnumerable<IkvmReferenceExportItem> items, CancellationToken cancellationToken)
        {
            // upfront load all of the assemblies involved
            var itemInfoTasks = items.Select(i => assemblyInfoUtil.GetAssemblyInfoAsync(i.ItemSpec, Log, cancellationToken));
            var referenceInfoTasks = References.Select(i => assemblyInfoUtil.GetAssemblyInfoAsync(i.ItemSpec, Log, cancellationToken));
            var infos = await Task.WhenAll(Enumerable.Concat(itemInfoTasks, referenceInfoTasks));

            // build a dictionary of assembly name to assembly info
            foreach (var info in infos)
                if (info.HasValue)
                    assemblyInfoByName[info.Value.Name] = info.Value;
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
        /// Assigns build information to the item.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="cancellationToken"></param>
        internal async Task AssignBuildInfoAsync(IkvmReferenceExportItem item, CancellationToken cancellationToken)
        {
            item.References = await CalculateReferencesAsync(item, cancellationToken);
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
            return (await GetAssemblyReferencesAsync(item.ItemSpec, cancellationToken)).OrderBy(i => i).ToList();
        }

        /// <summary>
        /// Finds all of the direct and indirect references of the given assembly.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<string[]> GetAssemblyReferencesAsync(string path, CancellationToken cancellationToken)
        {
            return assemblyReferenceCache.GetOrAdd(path, _ => ResolveAssemblyReferencesAsync(_, cancellationToken));
        }

        /// <summary>
        /// Finds all of the direct and indirect references of the given assembly.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<string[]> ResolveAssemblyReferencesAsync(string path, CancellationToken cancellationToken)
        {
            var references = new SortedSet<string>() { path };
            await ResolveAssemblyReferencesAsync(path, references, cancellationToken);
            return references.ToArray();
        }

        /// <summary>
        /// Finds all of the direct and indirect references of the given assembly.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="references"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task ResolveAssemblyReferencesAsync(string path, SortedSet<string> references, CancellationToken cancellationToken)
        {
            var info = await assemblyInfoUtil.GetAssemblyInfoAsync(path, Log, cancellationToken);
            if (info != null)
                foreach (var reference in info.Value.References)
                    if (assemblyInfoByName.TryGetValue(reference, out var i))
                        if (references.Add(i.Path))
                            await ResolveAssemblyReferencesAsync(i.Path, references, cancellationToken);

            // ensure the required libraries are present
            foreach (var n in new[] { "IKVM.Runtime", "IKVM.Java" })
                if (assemblyInfoByName.TryGetValue(n, out var i))
                    references.Add(i.Path);
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

            using var md5 = MD5.Create();
            using var stream = new CryptoStream(Stream.Null, md5, CryptoStreamMode.Write);
            using var writer = new StreamWriter(stream);
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

            // gather namespaces
            if (item.Namespaces != null)
                foreach (var ns in item.Namespaces.Distinct().OrderBy(i => i))
                    writer.WriteLine($"Namespace={ns}");

            // hash the resulting manifest and reeturn the identity
            writer.Flush();
            stream.FlushFinalBlock();
            return IkvmTaskUtil.ToHex(md5.Hash);
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

    }

}
