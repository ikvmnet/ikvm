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

    static class IkvmTaskUtil
    {

        /// <summary>
        /// Gets the relative path for the given path from the specified folder.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="relativeTo"></param>
        /// <returns></returns>
        public static string GetRelativePath(string relativeTo, string path)
        {
            var pathUri = new Uri(Path.GetFullPath(path));
            if (relativeTo.EndsWith(Path.DirectorySeparatorChar.ToString()) == false)
                relativeTo += Path.DirectorySeparatorChar;

            var relativeToUri = new Uri(Path.GetFullPath(relativeTo));
            return Uri.UnescapeDataString(relativeToUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }

    }

    static class IkvmReferenceItemMetadata
    {

        public const char PropertySeperatorChar = ';';
        public static readonly string PropertySeperatorString = PropertySeperatorChar.ToString();
        public static readonly char[] PropertySeperatorCharArray = new[] { PropertySeperatorChar };
        public static readonly string AssemblyName = "AssemblyName";
        public static readonly string AssemblyVersion = "AssemblyVersion";
        public static readonly string AssemblyFileVersion = "AssemblyFileVersion";
        public static readonly string DisableAutoAssemblyName = "DisableAutoAssemblyName";
        public static readonly string DisableAutoAssemblyVersion = "DisableAutoAssemblyVersion";
        public static readonly string FallbackAssemblyName = "FallbackAssemblyName";
        public static readonly string FallbackAssemblyVersion = "FallbackAssemblyVersion";
        public static readonly string Compile = "Compile";
        public static readonly string Sources = "Sources";
        public static readonly string References = "References";
        public static readonly string ClassLoader = "ClassLoader";
        public static readonly string IkvmIdentity = "IkvmIdentity";
        public static readonly string CachePath = "CachePath";
        public static readonly string StagePath = "StagePath";
        public static readonly string ResolvedReferences = "ResolvedReferences";
        public static readonly string Debug = "Debug";
        public static readonly string KeyFile = "KeyFile";
        public static readonly string DelaySign = "DelaySign";
        public static readonly string Aliases = "Aliases";
        public static readonly string Private = "Private";
        public static readonly string ReferenceOutputAssembly = "ReferenceOutputAssembly";

    }

    /// <summary>
    /// Provides common utility methods for working with <see cref="IkvmReferenceItem"/> sets.
    /// </summary>
    static class IkvmReferenceItemUtil
    {

        /// <summary>
        /// Returns a normalized version of a <see cref="IkvmReferenceItem"/> itemspec.
        /// </summary>
        /// <param name="itemSpec"></param>
        /// <returns></returns>
        public static string NormalizeItemSpec(string itemSpec)
        {
            // the itemspec may be a jar file
            if (Path.GetExtension(itemSpec) == ".jar" && File.Exists(itemSpec))
                return IkvmTaskUtil.GetRelativePath(Environment.CurrentDirectory, Path.GetFullPath(itemSpec).TrimEnd(Path.DirectorySeparatorChar));

            // the itemspec may be a directory
            if (Directory.Exists(itemSpec))
                itemSpec = IkvmTaskUtil.GetRelativePath(Environment.CurrentDirectory, Path.GetFullPath(itemSpec).TrimEnd(Path.DirectorySeparatorChar)) + Path.DirectorySeparatorChar;

            return itemSpec;
        }

        /// <summary>
        /// Attempts to import a set of <see cref="IkvmReferenceItem"/> instances from the given <see cref="ITaskItem"/> instances.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static IkvmReferenceItem[] Import(IEnumerable<ITaskItem> items)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));

            // normalize itemspecs into a dictionary
            var map = new Dictionary<string, IkvmReferenceItem>();
            foreach (var item in items)
                map[NormalizeItemSpec(item.ItemSpec)] = new IkvmReferenceItem(item);

            // populate the properties of each item
            foreach (var item in map.Values)
            {
                item.ItemSpec = NormalizeItemSpec(item.Item.ItemSpec);
                item.AssemblyName = item.Item.GetMetadata(IkvmReferenceItemMetadata.AssemblyName);
                item.AssemblyVersion = item.Item.GetMetadata(IkvmReferenceItemMetadata.AssemblyVersion);
                item.DisableAutoAssemblyName = string.Equals(item.Item.GetMetadata(IkvmReferenceItemMetadata.DisableAutoAssemblyName), "true", StringComparison.OrdinalIgnoreCase);
                item.DisableAutoAssemblyVersion = string.Equals(item.Item.GetMetadata(IkvmReferenceItemMetadata.DisableAutoAssemblyVersion), "true", StringComparison.OrdinalIgnoreCase);
                item.FallbackAssemblyName = item.Item.GetMetadata(IkvmReferenceItemMetadata.FallbackAssemblyName);
                item.FallbackAssemblyVersion = item.Item.GetMetadata(IkvmReferenceItemMetadata.FallbackAssemblyVersion);
                item.Compile = item.Item.GetMetadata(IkvmReferenceItemMetadata.Compile)?.Split(IkvmReferenceItemMetadata.PropertySeperatorCharArray, StringSplitOptions.RemoveEmptyEntries).ToList();
                item.Sources = item.Item.GetMetadata(IkvmReferenceItemMetadata.Sources)?.Split(IkvmReferenceItemMetadata.PropertySeperatorCharArray, StringSplitOptions.RemoveEmptyEntries).ToList();
                item.References = ResolveReferences(map, item, item.Item.GetMetadata(IkvmReferenceItemMetadata.References)).ToList();
                item.ClassLoader = item.Item.GetMetadata(IkvmReferenceItemMetadata.ClassLoader);
                item.ResolvedReferences = item.Item.GetMetadata(IkvmReferenceItemMetadata.ResolvedReferences)?.Split(IkvmReferenceItemMetadata.PropertySeperatorCharArray, StringSplitOptions.RemoveEmptyEntries).ToList();
                item.Debug = string.Equals(item.Item.GetMetadata(IkvmReferenceItemMetadata.Debug), "true", StringComparison.OrdinalIgnoreCase);
                item.KeyFile = item.Item.GetMetadata(IkvmReferenceItemMetadata.KeyFile);
                item.DelaySign = string.Equals(item.Item.GetMetadata(IkvmReferenceItemMetadata.DelaySign), "true", StringComparison.OrdinalIgnoreCase);
                item.Aliases = item.Item.GetMetadata(IkvmReferenceItemMetadata.Aliases);
                item.Private = string.Equals(item.Item.GetMetadata(IkvmReferenceItemMetadata.Private), "true", StringComparison.OrdinalIgnoreCase);
                item.ReferenceOutputAssembly = string.Equals(item.Item.GetMetadata(IkvmReferenceItemMetadata.ReferenceOutputAssembly), "true", StringComparison.OrdinalIgnoreCase);
                item.Save();
            }

            // return the resulting imported references
            return map.Values.ToArray();
        }

        /// <summary>
        /// Attempts to resolve the references given by the reference string <paramref name="references"/> for
        /// <paramref name="item"/> against <paramref name="map"/>.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="item"></param>
        /// <param name="references"></param>
        /// <returns></returns>
        /// <exception cref="IkvmTaskException"></exception>
        static List<IkvmReferenceItem> ResolveReferences(Dictionary<string, IkvmReferenceItem> map, IkvmReferenceItem item, string references)
        {
            if (map is null)
                throw new ArgumentNullException(nameof(map));
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            var l = new List<IkvmReferenceItem>();
            foreach (var itemSpec in references.Split(IkvmReferenceItemMetadata.PropertySeperatorCharArray, StringSplitOptions.RemoveEmptyEntries))
                if (TryResolveReference(map, itemSpec, out var resolved))
                    l.Add(resolved);
                else
                    throw new Exception("Error.IkvmInvalidReference");

            return l;
        }

        /// <summary>
        /// Attempts to resolve the given <see cref="IkvmReferenceItem"/> itemspec against the set of  <see cref="IkvmReferenceItem"/> instances
        /// </summary>
        /// <param name="itemSpec"></param>
        /// <param name="resolved"></param>
        /// <returns></returns>
        static bool TryResolveReference(Dictionary<string, IkvmReferenceItem> map, string itemSpec, out IkvmReferenceItem resolved)
        {
            if (map is null)
                throw new ArgumentNullException(nameof(map));
            if (string.IsNullOrEmpty(itemSpec))
                throw new ArgumentException($"'{nameof(itemSpec)}' cannot be null or empty.", nameof(itemSpec));

            resolved = map.TryGetValue(NormalizeItemSpec(itemSpec), out var r) ? r : null;
            return resolved != null;
        }

    }

    /// <summary>
    /// Models the required data of a <see cref="IkvmReferenceItem"/>.
    /// </summary>
    class IkvmReferenceItem
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="item"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReferenceItem(ITaskItem item)
        {
            Item = item ?? throw new ArgumentNullException(nameof(item));
        }

        /// <summary>
        /// Referenced node.
        /// </summary>
        public ITaskItem Item { get; }

        /// <summary>
        /// Unique name of the item.
        /// </summary>
        public string ItemSpec { get; set; }

        /// <summary>
        /// Assembly name.
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// Assembly version.
        /// </summary>
        public string AssemblyVersion { get; set; }

        /// <summary>
        /// Assembly file version. If not specified, defaults to the assembly version.
        /// </summary>
        public string AssemblyFileVersion { get; set; }

        /// <summary>
        /// Disable automatic detection of the assembly name.
        /// </summary>
        public bool DisableAutoAssemblyName { get; set; } = false;

        /// <summary>
        /// Disable automatic detection of the assembly version.
        /// </summary>
        public bool DisableAutoAssemblyVersion { get; set; } = false;

        /// <summary>
        /// Assembly name to use if no other assembly name is available.
        /// </summary>
        public string FallbackAssemblyName { get; set; }

        /// <summary>
        /// Assembly version to use if no other assembly version is available.
        /// </summary>
        public string FallbackAssemblyVersion { get; set; }

        /// <summary>
        /// Set of sources to compile.
        /// </summary>
        public List<string> Compile { get; set; } = new List<string>();

        /// <summary>
        /// Set of Java sources which can be used to generate documentation.
        /// </summary>
        public List<string> Sources { get; set; } = new List<string>();

        /// <summary>
        /// References required to compile.
        /// </summary>
        public List<IkvmReferenceItem> References { get; set; } = new List<IkvmReferenceItem>();

        /// <summary>
        /// Name of the classloader to use.
        /// </summary>
        public string ClassLoader { get; set; }

        /// <summary>
        /// Unique IKVM identity of the reference.
        /// </summary>
        public string IkvmIdentity { get; set; }

        /// <summary>
        /// Path in cache where resulting item will be stored.
        /// </summary>
        public string CachePath { get; set; }

        /// <summary>
        /// Path to temporarily generate item.
        /// </summary>
        public string StagePath { get; set; }

        /// <summary>
        /// Aliases to make the assembly available under.
        /// </summary>
        public string Aliases { get; set; }

        /// <summary>
        /// Compile in debug mode.
        /// </summary>
        public bool Debug { get; set; }

        /// <summary>
        /// Path to the file to sign the assembly.
        /// </summary>
        public string KeyFile { get; set; }

        /// <summary>
        /// Whether to delay sign the produced assembly.
        /// </summary>
        public bool DelaySign { get; set; }

        /// <summary>
        /// Whether the itme will be copied along with the build output.
        /// </summary>
        public bool Private { get; set; } = true;

        /// <summary>
        /// Whether a reference should be added to this item.
        /// </summary>
        public bool ReferenceOutputAssembly { get; set; } = true;

        /// <summary>
        /// Paths to other referenced items.
        /// </summary>
        public List<string> ResolvedReferences { get; set; }

        /// <summary>
        /// Writes the metadata to the item.
        /// </summary>
        public void Save()
        {
            Item.ItemSpec = ItemSpec;
            Item.SetMetadata(IkvmReferenceItemMetadata.AssemblyName, AssemblyName);
            Item.SetMetadata(IkvmReferenceItemMetadata.AssemblyVersion, AssemblyVersion);
            Item.SetMetadata(IkvmReferenceItemMetadata.AssemblyFileVersion, AssemblyFileVersion);
            Item.SetMetadata(IkvmReferenceItemMetadata.DisableAutoAssemblyName, DisableAutoAssemblyName ? "true" : "false");
            Item.SetMetadata(IkvmReferenceItemMetadata.DisableAutoAssemblyVersion, DisableAutoAssemblyVersion ? "true" : "false");
            Item.SetMetadata(IkvmReferenceItemMetadata.FallbackAssemblyName, FallbackAssemblyName);
            Item.SetMetadata(IkvmReferenceItemMetadata.FallbackAssemblyVersion, FallbackAssemblyVersion);
            Item.SetMetadata(IkvmReferenceItemMetadata.Compile, string.Join(IkvmReferenceItemMetadata.PropertySeperatorString, Compile));
            Item.SetMetadata(IkvmReferenceItemMetadata.Sources, string.Join(IkvmReferenceItemMetadata.PropertySeperatorString, Sources));
            Item.SetMetadata(IkvmReferenceItemMetadata.References, string.Join(IkvmReferenceItemMetadata.PropertySeperatorString, References.Select(i => i.ItemSpec)));
            Item.SetMetadata(IkvmReferenceItemMetadata.ClassLoader, ClassLoader);
            Item.SetMetadata(IkvmReferenceItemMetadata.IkvmIdentity, IkvmIdentity);
            Item.SetMetadata(IkvmReferenceItemMetadata.CachePath, CachePath);
            Item.SetMetadata(IkvmReferenceItemMetadata.StagePath, StagePath);
            Item.SetMetadata(IkvmReferenceItemMetadata.Aliases, Aliases);
            Item.SetMetadata(IkvmReferenceItemMetadata.Debug, Debug ? "true" : "false");
            Item.SetMetadata(IkvmReferenceItemMetadata.KeyFile, KeyFile);
            Item.SetMetadata(IkvmReferenceItemMetadata.DelaySign, DelaySign ? "true" : "false");
            Item.SetMetadata(IkvmReferenceItemMetadata.Private, Private ? "true" : "false");
            Item.SetMetadata(IkvmReferenceItemMetadata.ReferenceOutputAssembly, ReferenceOutputAssembly ? "true" : "false");
            Item.SetMetadata(IkvmReferenceItemMetadata.ResolvedReferences, string.Join(IkvmReferenceItemMetadata.PropertySeperatorString, ResolvedReferences));
        }

    }


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

            throw new Exception();
        }

        readonly static MD5 md5 = MD5.Create();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public IkvmReferenceItemPrepare()
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
        public string ToolFramework { get; set; }

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

            // default to fallback value
            if (string.IsNullOrWhiteSpace(item.AssemblyName))
                item.AssemblyName = item.FallbackAssemblyName;

            // default to fallback value
            if (string.IsNullOrWhiteSpace(item.AssemblyVersion))
                item.AssemblyVersion = item.FallbackAssemblyVersion;

            // default to assembly version
            if (string.IsNullOrWhiteSpace(item.AssemblyFileVersion))
                item.AssemblyFileVersion = item.AssemblyVersion;

            // clean up values
            item.AssemblyVersion = NormalizeAssemblyVersion(item.AssemblyVersion);
            item.AssemblyFileVersion = NormalizeAssemblyFileVersion(item.AssemblyFileVersion);

            // save changes to item
            item.Save();
        }

        /// <summary>
        /// Normalizes an assembly version.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        static string NormalizeAssemblyVersion(string version)
        {
            if (string.IsNullOrWhiteSpace(version))
                return null;

            if (Version.TryParse(version, out var v))
            {
                var major = v.Major;
                var minor = v.Minor;
                var build = v.Build;
                var patch = v.Revision;
                return new Version(major, minor, build > -1 ? build : 0, patch > -1 ? patch : 0).ToString();
            }

            return null;
        }

        /// <summary>
        /// Normalizes an assembly version.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        static string NormalizeAssemblyFileVersion(string version)
        {
            if (string.IsNullOrWhiteSpace(version))
                return null;

            if (Version.TryParse(version, out var v))
            {
                var major = v.Major;
                var minor = v.Minor;
                var build = v.Build;
                var patch = v.Revision;
                return new Version(major, minor, build >= 0 ? build : 0, patch >= 0 ? patch : 0).ToString();
            }

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
                throw new Exception("Error_IkvmInvalidAssemblyName");
            if (string.IsNullOrWhiteSpace(item.AssemblyVersion))
                throw new Exception("Error_IkvmInvalidAssemblyVersion");

            var manifest = new StringWriter();
            manifest.WriteLine("ToolVersion={0}", ToolVersion);
            manifest.WriteLine("ToolFramework={0}", ToolFramework);
            manifest.WriteLine("RuntimeAssembly={0}", GetIdentityForFile(RuntimeAssembly));
            manifest.WriteLine("AssemblyName={0}", item.AssemblyName);
            manifest.WriteLine("AssemblyVersion={0}", item.AssemblyVersion);
            manifest.WriteLine("AssemblyFileVersion={0}", item.AssemblyFileVersion);
            manifest.WriteLine("ClassLoader={0}", item.ClassLoader);
            manifest.WriteLine("Debug={0}", item.Debug ? "true" : "false");
            manifest.WriteLine("KeyFile={0}", string.IsNullOrWhiteSpace(item.KeyFile) == false ? GetIdentityForFile(item.KeyFile) : "");
            manifest.WriteLine("DelaySign={0}", item.DelaySign ? "true" : "false");

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
            foreach (var r in references.Distinct())
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
        string GetIdentityForFile(string file)
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

            return $"Compile={GetIdentityForFile(path)}";
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

            return $"Reference={GetIdentityForFile(reference.ItemSpec)}";
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
                Log.LogError("Error.IkvmMissingAssemblyName");
                valid = false;
            }

            if (string.IsNullOrWhiteSpace(item.AssemblyVersion))
            {
                Log.LogError("Error.IkvmMissingAssemblyVersion");
                valid = false;
            }
            else
            {
                if (Version.TryParse(item.AssemblyVersion, out _) == false)
                {
                    Log.LogError("Error.IkvmInvalidAssemblyVersion");
                    valid = false;
                }
            }

            if (string.IsNullOrWhiteSpace(item.AssemblyFileVersion))
            {
                Log.LogError("Error.IkvmMissingAssemblyFileVersion");
                valid = false;
            }
            else
            {
                if (Version.TryParse(item.AssemblyFileVersion, out _) == false)
                {
                    Log.LogError("Error.IkvmInvalidAssemblyFileVersion");
                    valid = false;
                }
            }

            if (item.Compile.Count == 0)
            {
                Log.LogError("Error.IkvmRequiresCompile");
                valid = false;
            }
            else
            {
                foreach (var compile in item.Compile)
                {
                    if (Path.GetExtension(compile) is not ".jar" and not ".class")
                    {
                        Log.LogError("Error.IkvmInvalidCompile");
                        valid = false;
                    }

                    if (File.Exists(compile) == false)
                    {
                        Log.LogError("Error.IkvmMissingCompile");
                        valid = false;
                    }
                }
            }

            foreach (var source in item.Sources)
            {
                if (Path.GetExtension(source) is not ".java" && Path.GetExtension(source) is not ".jar")
                {
                    Log.LogError("Error.IkvmInvalidSources");
                    valid = false;
                }
                else if (File.Exists(source) == false)
                {
                    Log.LogError("Error.IkvmMissingSources");
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
                Log.LogError("Error.IkvmInvalidAssemblyInfo");
                valid = false;
            }

            if (string.IsNullOrWhiteSpace(item.KeyFile) == false && File.Exists(item.KeyFile) == false)
            {
                Log.LogError("Error.IkvmMissingKeyFile");
                valid = false;
            }

            if (item.DelaySign && string.IsNullOrWhiteSpace(item.KeyFile))
            {
                Log.LogError("Error.IkvmDelaySignRequiresKey");
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
                item.ResolvedReferences = item.References.Distinct().Select(i => i.CachePath).ToList();
                item.Save();
            }
        }

    }

}
