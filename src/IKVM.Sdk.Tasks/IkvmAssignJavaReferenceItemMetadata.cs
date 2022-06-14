using System;
using System.Collections.Generic;
using System.IO;

using IKVM.Util.Jar;

using Microsoft.Build.Framework;
using Microsoft.Build.Globbing;
using Microsoft.Build.Utilities;

namespace IKVM.Sdk.Tasks
{

    /// <summary>
    /// For each JavaReferenceItem passed in, assigns default metadata if required.
    /// </summary>
    public class IkvmAssignJavaReferenceItemMetadata : Task
    {

        /// <summary>
        /// JavaReferenceItem items to assign metadata to.
        /// </summary>
        [Required]
        [Output]
        public ITaskItem[] Items { get; set; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            var items = IkvmJavaReferenceItemUtil.Import(Items);

            // assign other metadata
            foreach (var item in items)
                AssignMetadata(item);

            // save each back to the original task item
            foreach (var item in items)
                item.Save();

            return true;
        }

        /// <summary>
        /// Assigns the metadata to the item.
        /// </summary>
        /// <param name="item"></param>
        void AssignMetadata(JavaReferenceItem item)
        {
            // if it's a jar or a directory, add the itemspec to Compile
            if (item.ItemSpec.EndsWith(".jar", StringComparison.OrdinalIgnoreCase) && File.Exists(item.ItemSpec) || Directory.Exists(item.ItemSpec))
                item.Compile.Insert(0, item.ItemSpec);

            // probe the classpath's for available metadata
            ExpandCompile(item);
            ExpandSources(item);
            AssignMetadataFromCompile(item);
        }

        /// <summary>
        /// Expands each entry in the Compile metadata.
        /// </summary>
        /// <param name="item"></param>
        void ExpandCompile(JavaReferenceItem item)
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
        void ExpandSources(JavaReferenceItem item)
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
        internal IEnumerable<string> ExpandPath(string path)
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
        void AssignMetadataFromCompile(JavaReferenceItem item)
        {
            foreach (var path in item.Compile)
                AssignMetadataFromCompile(item, path);
        }

        /// <summary>
        /// Assigns the metadata to the item which is a directory.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="path"></param>
        void AssignMetadataFromCompile(JavaReferenceItem item, string path)
        {
            var info = TryGetAssemblyNameFromPath(path);

            // attempt to derive a default assembly name from the compile item
            if (info != null && string.IsNullOrWhiteSpace(item.AssemblyName))
                item.AssemblyName = info.Name;

            // attempt to derive a default assembly version from the compile item
            if (info != null && string.IsNullOrWhiteSpace(item.AssemblyVersion))
                if (Version.TryParse(info.Version, out var v))
                    item.AssemblyVersion = v.ToString();
        }

        /// <summary>
        /// Attempts to get the module info from an of the compile path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        JarFileUtil.ModuleInfo TryGetAssemblyNameFromPath(string path)
        {
            if (path.EndsWith(".jar", StringComparison.OrdinalIgnoreCase) && File.Exists(path))
                return JarFileUtil.GetModuleInfo(path);

            return null;
        }

    }

}
