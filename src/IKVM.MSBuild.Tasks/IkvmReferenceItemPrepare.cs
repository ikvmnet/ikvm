using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

using IKVM.Util.Jar;
using IKVM.Util.Modules;

using Microsoft.Build.Framework;
using Microsoft.Build.Globbing;
using Microsoft.Build.Utilities;

namespace IKVM.MSBuild.Tasks
{

    /// <summary>
    /// For each <see cref="IkvmReferenceItem"/> passed in, assigns default metadata if required.
    /// </summary>
    public class IkvmReferenceItemPrepare : Task
    {

        /// <summary>
        /// Topologically sorts the <see cref="IkvmReferenceItem"/> set.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        static IList<IkvmReferenceItem> Sort(IList<IkvmReferenceItem> items)
        {
            // construct a map of nodes to their indegrees
            var m = items.ToDictionary(i => i, i => 0);
            foreach (var item in items)
                foreach (var reference in item.References)
                    m[reference]++;

            // track nodes with no incoming edges
            var t = new Queue<IkvmReferenceItem>(items.Where(i => m[i] == 0));

            // initially no nodes in our ordering
            var l = new List<IkvmReferenceItem>();

            // as long as there are nodes with no incoming edges
            while (t.Count > 0)
            {
                // add one of those nodes to the ordering
                var node = t.Dequeue();
                l.Add(node);

                // decrement the indegree of that node's neightbors
                foreach (var neighbor in node.References)
                {
                    m[neighbor]--;
                    if (m[neighbor] == 0)
                        t.Enqueue(neighbor);
                }
            }

            // we're finished, no cycle
            if (l.Count == items.Count)
            {
                l.Reverse();
                return l;
            }

            throw new IkvmTaskMessageException("Error.IkvmCircularReference", l[0]);
        }

        readonly static MD5 md5 = MD5.Create();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public IkvmReferenceItemPrepare() :
            base(Resources.SR.ResourceManager, "IKVM:")
        {

        }

        /// <summary>
        /// <see cref="IkvmReferenceItem"/> items without assigned hashes.
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
        public string TargetFramework { get; set; }

        /// <summary>
        /// Other references that will be used to generate the assemblies.
        /// </summary>
        [Required]
        public string RuntimeAssembly { get; set; }

        /// <summary>
        /// Other references that will be used to generate the assemblies.
        /// </summary>
        [Required]
        public ITaskItem[] References { get; set; }

        /// <summary>
        /// Directory where output will be staged.
        /// </summary>
        [Required]
        public string StageDir { get; set; }

        /// <summary>
        /// Directory where the final output will be stored.
        /// </summary>
        [Required]
        public string CacheDir { get; set; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            var items = IkvmReferenceItemUtil.Import(Items);

            // populate and normalize metadata
            AssignMetadata(items);

            // validate what we can up front
            if (Validate(items) == false)
                return false;

            // calculate information required for build
            AssignBuildInfo(items);
            ResolveReferences(items);

            // return the items in build order
            Items = Sort(items).Select(i => i.Item).ToArray();
            return true;
        }

        /// <summary>
        /// Assigns metadata to the items.
        /// </summary>
        /// <param name="items"></param>
        internal static void AssignMetadata(IEnumerable<IkvmReferenceItem> items)
        {
            foreach (var item in items)
                AssignMetadata(item);
        }

        /// <summary>
        /// Assigns the metadata to the item.
        /// </summary>
        /// <param name="item"></param>
        internal static void AssignMetadata(IkvmReferenceItem item)
        {
            // if it's a jar or a directory, add the itemspec to Compile
            if (item.ItemSpec.EndsWith(".jar", StringComparison.OrdinalIgnoreCase) && File.Exists(item.ItemSpec) || Directory.Exists(item.ItemSpec))
                item.Compile.Insert(0, item.ItemSpec);

            // probe the classpath's for available metadata
            ExpandCompile(item);
            ExpandSources(item);
            AssignMetadataFromCompile(item);

            // default to fallback value
            if (string.IsNullOrWhiteSpace(item.AssemblyName))
                item.AssemblyName = item.FallbackAssemblyName;

            // default to fallback value
            if (string.IsNullOrWhiteSpace(item.AssemblyVersion))
                item.AssemblyVersion = item.FallbackAssemblyVersion;

            // save changes to item
            item.Save();
        }

        /// <summary>
        /// Expands each entry in the Compile metadata.
        /// </summary>
        /// <param name="item"></param>
        internal static void ExpandCompile(IkvmReferenceItem item)
        {
            var l = new List<string>();
            foreach (var c in item.Compile)
                l.AddRange(ExpandPath(c));

            item.Compile = l;
        }

        /// <summary>
        /// Expands each entry in the Sources metadata.
        /// </summary>
        /// <param name="item"></param>
        internal static void ExpandSources(IkvmReferenceItem item)
        {
            var l = new List<string>();
            foreach (var c in item.Sources)
                l.AddRange(ExpandPath(c));

            item.Sources = l;
        }

        /// <summary>
        /// Expands the path to real underlying files.
        /// </summary>
        /// <param name="path"></param>
        internal static IEnumerable<string> ExpandPath(string path)
        {
            // if the path is a glob, we're going to match items, else skip
            var glob = MSBuildGlob.Parse(path);
            if (glob.IsLegal == false)
            {
                path = IkvmTaskUtil.GetRelativePath(Environment.CurrentDirectory, path);
                yield return path;
                yield break;
            }

            // no fixed directory, nothing to match
            if (Directory.Exists(glob.FixedDirectoryPart) == false)
                yield break;

            // enumerate all files in the fixed part, and match them against the glob
            // results are our expanded options
            foreach (var i in Directory.EnumerateFileSystemEntries(glob.FixedDirectoryPart, "*", SearchOption.AllDirectories))
                if (File.Exists(i) && glob.IsMatch(i))
                    yield return IkvmTaskUtil.GetRelativePath(Environment.CurrentDirectory, i);
        }

        /// <summary>
        /// Assigns the metadata to the item derived from the Compile items.
        /// </summary>
        /// <param name="item"></param>
        internal static void AssignMetadataFromCompile(IkvmReferenceItem item)
        {
            foreach (var path in item.Compile)
                AssignMetadataFromCompile(item, path);
        }

        /// <summary>
        /// Assigns the metadata to the item which is a directory.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="path"></param>
        internal static void AssignMetadataFromCompile(IkvmReferenceItem item, string path)
        {
            if (item.DisableAutoAssemblyName == false || item.DisableAutoAssemblyVersion == false)
            {
                if (string.IsNullOrWhiteSpace(item.AssemblyName) || string.IsNullOrWhiteSpace(item.AssemblyVersion))
                {
                    var info = TryGetAssemblyNameFromPath(path);
                    if (info != null)
                    {
                        // attempt to derive a default assembly name from the compile item
                        if (string.IsNullOrWhiteSpace(item.AssemblyName) && item.DisableAutoAssemblyName == false)
                            item.AssemblyName = info.Name;

                        // attempt to derive a default assembly version from the compile item
                        if (string.IsNullOrWhiteSpace(item.AssemblyVersion) && item.DisableAutoAssemblyVersion == false)
                            item.AssemblyVersion = info.Version != null ? ToAssemblyVersion(info.Version)?.ToString() : null;
                    }
                }
            }
        }

        /// <summary>
        /// Converts a <see cref="ModuleVersion"/> to an assembly version.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        static Version ToAssemblyVersion(ModuleVersion version)
        {
            // only include major and minor by default
            var major = GetAssemblyVersionComponent(version, 0);
            var minor = GetAssemblyVersionComponent(version, 1);
            if (minor is not null && major is not null)
                return new Version(major ?? 0, minor ?? 0);

            return null;
        }

        /// <summary>
        /// Gets the assembly version compatible component at the given index.
        /// </summary>
        /// <param name="version"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        static int? GetAssemblyVersionComponent(ModuleVersion version, int index)
        {
            return version.Number.Count > index && version.Number[index] is int i ? i : null;
        }

        /// <summary>
        /// Attempts to get the module info from an of the compile path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static JarFileUtil.ModuleInfo TryGetAssemblyNameFromPath(string path)
        {
            if (path.EndsWith(".jar", StringComparison.OrdinalIgnoreCase) && File.Exists(path))
                return JarFileUtil.GetModuleInfo(path);

            return null;
        }

        /// <summary>
        /// Calculates the hash for the given item.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        internal string CalculateIkvmIdentity(IkvmReferenceItem item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            // identity is already computed
            if (item.IkvmIdentity is string id && string.IsNullOrWhiteSpace(id) == false)
                return id;

            if (string.IsNullOrWhiteSpace(item.AssemblyName))
                throw new IkvmTaskMessageException(Resources.SR.Error_IkvmInvalidAssemblyName, item, item.AssemblyName);
            if (string.IsNullOrWhiteSpace(item.AssemblyVersion))
                throw new IkvmTaskMessageException(Resources.SR.Error_IkvmInvalidAssemblyVersion, item, item.AssemblyVersion);

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
            if (reference.ItemSpec.Contains(Path.DirectorySeparatorChar.ToString()) == false)
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

        /// <summary>
        /// Validates the items.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        internal bool Validate(IEnumerable<IkvmReferenceItem> items)
        {
            foreach (var item in items)
                if (Validate(item) == false)
                    return false;

            return true;
        }

        /// <summary>
        /// Validates the item.
        /// </summary>
        /// <param name="item"></param>
        internal bool Validate(IkvmReferenceItem item)
        {
            var valid = true;

            if (string.IsNullOrWhiteSpace(item.AssemblyName))
            {
                Log.LogErrorWithCodeFromResources("Error.IkvmInvalidAssemblyName", item.ItemSpec, item.AssemblyName);
                valid = false;
            }

            if (string.IsNullOrWhiteSpace(item.AssemblyVersion))
            {
                Log.LogErrorWithCodeFromResources("Error.IkvmInvalidAssemblyVersion", item.ItemSpec, item.AssemblyVersion);
                valid = false;
            }
            else
            {
                if (Version.TryParse(item.AssemblyVersion, out _) == false)
                {
                    Log.LogErrorWithCodeFromResources("Error.IkvmInvalidAssemblyVersion", item.ItemSpec, item.AssemblyVersion);
                    valid = false;
                }
            }

            if (item.Compile.Count == 0)
            {
                Log.LogErrorWithCodeFromResources("Error.IkvmRequiresCompile", item.ItemSpec);
                valid = false;
            }
            else
            {
                foreach (var compile in item.Compile)
                {
                    if (Path.GetExtension(compile) is not ".jar" and not ".class")
                    {
                        Log.LogErrorWithCodeFromResources("Error.IkvmInvalidCompile", item.ItemSpec, compile);
                        valid = false;
                    }

                    if (File.Exists(compile) == false)
                    {
                        Log.LogErrorWithCodeFromResources("Error.IkvmMissingCompile", item.ItemSpec, compile);
                        valid = false;
                    }
                }
            }

            foreach (var source in item.Sources)
            {
                if (Path.GetExtension(source) is not ".java")
                {
                    Log.LogErrorWithCodeFromResources("Error.IkvmInvalidSources", item.ItemSpec, source);
                    valid = false;
                }
                else if (File.Exists(source) == false)
                {
                    Log.LogErrorWithCodeFromResources("Error.IkvmMissingSources", item.ItemSpec, source);
                    valid = false;
                }
            }

            // bail out early, no need to add more if these are already broken
            if (valid == false)
                return false;

            try
            {
                new AssemblyName($"{item.AssemblyName}, Version={item.AssemblyVersion}");
            }
            catch (Exception)
            {
                Log.LogErrorWithCodeFromResources("Error.IkvmInvalidAssemblyInfo", item.ItemSpec, item.AssemblyName, item.AssemblyVersion);
                valid = false;
            }

            return valid;
        }

        /// <summary>
        /// Assigns build information to the items.
        /// </summary>
        /// <param name="items"></param>
        internal void AssignBuildInfo(IEnumerable<IkvmReferenceItem> items)
        {
            foreach (var item in items)
                AssignBuildInfo(item);
        }

        /// <summary>
        /// Assigns build information to the item.
        /// </summary>
        /// <param name="item"></param>
        internal void AssignBuildInfo(IkvmReferenceItem item)
        {
            item.IkvmIdentity = CalculateIkvmIdentity(item);
            item.CachePath = Path.Combine(CacheDir, item.IkvmIdentity, item.AssemblyName + ".dll");
            item.StagePath = Path.Combine(StageDir, item.IkvmIdentity, item.AssemblyName + ".dll");
            item.Save();
        }

        /// <summary>
        /// Resolve the references into the final paths.
        /// </summary>
        /// <param name="items"></param>
        internal static void ResolveReferences(IEnumerable<IkvmReferenceItem> items)
        {
            foreach (var item in items)
            {
                item.ResolvedReferences = item.References.Select(i => i.CachePath).ToList();
                item.Save();
            }
        }

    }

}
