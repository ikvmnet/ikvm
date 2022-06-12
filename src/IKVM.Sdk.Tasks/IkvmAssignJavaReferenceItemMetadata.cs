using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

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
            // normalize the item specs so references can be resolved
            foreach (var item in Items)
                NormalizeItemSpec(item);

            // assign other metadata
            foreach (var item in Items)
                AssignMetadataForItem(item);

            return true;
        }

        /// <summary>
        /// Normalizes the ItemSpec of an item.
        /// </summary>
        /// <param name="itemSpec"></param>
        string NormalizeItemSpec(string itemSpec)
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
        /// Normalizes the ItemSpec of an item.
        /// </summary>
        /// <param name="item"></param>
        void NormalizeItemSpec(ITaskItem item)
        {
            item.ItemSpec = NormalizeItemSpec(item.ItemSpec);
        }

        /// <summary>
        /// Assigns the metadata to the item.
        /// </summary>
        /// <param name="item"></param>
        void AssignMetadataForItem(ITaskItem item)
        {
            // itemspec may be normalized to a jar or directory path
            NormalizeItemSpec(item);

            // if it's a jar or a directory, add the itemspec to Compile
            if (item.ItemSpec.EndsWith(".jar", StringComparison.OrdinalIgnoreCase) && File.Exists(item.ItemSpec) || Directory.Exists(item.ItemSpec))
                PrependCompileToItem(item, item.ItemSpec);

            // probe the classpath's for available metadata
            ExpandCompileMetadata(item);
            AssignMetadataFromCompile(item);

            // ensure the item references are populated
            NormalizeReferences(item);
            ValidateReferences(item);
        }

        /// <summary>
        /// Expands each entry in the Compile metadata.
        /// </summary>
        /// <param name="item"></param>
        void ExpandCompileMetadata(ITaskItem item)
        {
            var l = new List<string>();
            foreach (var c in item.GetMetadata(IkvmJavaReferenceItemMetadata.Compile).Split(IkvmJavaReferenceItemMetadata.PropertySeperatorCharArray, StringSplitOptions.RemoveEmptyEntries))
                ExpandCompileMetadata(item, l, c);

            // reset Compile metadata to expanded values
            item.SetMetadata(IkvmJavaReferenceItemMetadata.Compile, string.Join(IkvmJavaReferenceItemMetadata.PropertySeperatorString, l.Distinct()));
        }

        /// <summary>
        /// Expands the path to real underlying files.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="list"></param>
        /// <param name="path"></param>
        internal void ExpandCompileMetadata(ITaskItem item, List<string> list, string path)
        {
            // if the path is a glob, we're going to match items, else skip
            var glob = MSBuildGlob.Parse(path);
            if (glob.IsLegal == false)
            {
                path = IkvmTaskUtil.GetRelativePath(Environment.CurrentDirectory, path);
                list.Add(path);
                return;
            }

            // no fixed directory, nothing to match
            if (Directory.Exists(glob.FixedDirectoryPart) == false)
                return;

            // enumerate all files in the fixed part, and match them against the glob
            // results are out expanded options
            foreach (var i in Directory.EnumerateFileSystemEntries(glob.FixedDirectoryPart, "*", SearchOption.AllDirectories))
                if (File.Exists(i) && glob.IsMatch(i))
                    list.Add(IkvmTaskUtil.GetRelativePath(Environment.CurrentDirectory, i));
        }

        /// <summary>
        /// Prepends the given classpath as a member of the ClassPath metadata.
        /// </summary>
        /// <param name="item"></param>
        void PrependCompileToItem(ITaskItem item, string compile)
        {
            // prepend identity to the classpath if it isn't already present
            var cp = item.GetMetadata(IkvmJavaReferenceItemMetadata.Compile).Split(IkvmJavaReferenceItemMetadata.PropertySeperatorCharArray, StringSplitOptions.RemoveEmptyEntries).ToList();
            cp.Insert(0, compile);
            item.SetMetadata(IkvmJavaReferenceItemMetadata.Compile, string.Join(IkvmJavaReferenceItemMetadata.PropertySeperatorString, cp.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct()));
        }

        /// <summary>
        /// Assigns the metadata to the item derived from the Compile items.
        /// </summary>
        /// <param name="item"></param>
        void AssignMetadataFromCompile(ITaskItem item)
        {
            foreach (var path in item.GetMetadata(IkvmJavaReferenceItemMetadata.Compile).Split(IkvmJavaReferenceItemMetadata.PropertySeperatorCharArray, StringSplitOptions.RemoveEmptyEntries))
                AssignMetadataFromCompile(item, path);
        }

        /// <summary>
        /// Assigns the metadata to the item which is a directory.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="path"></param>
        void AssignMetadataFromCompile(ITaskItem item, string path)
        {
            // attempt to derive a default assembly name from the compile item
            if (string.IsNullOrWhiteSpace(item.GetMetadata(IkvmJavaReferenceItemMetadata.AssemblyName)))
                item.SetMetadata(IkvmJavaReferenceItemMetadata.AssemblyName, TryGetAssemblyNameFromPath(path));

            if (string.IsNullOrWhiteSpace(item.GetMetadata(IkvmJavaReferenceItemMetadata.AssemblyVersion)))
                item.SetMetadata(IkvmJavaReferenceItemMetadata.AssemblyVersion, "0.0.0.0"); // TODO probe classpath
        }

        /// <summary>
        /// Attempts to get the assembly name from the given Compile path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string TryGetAssemblyNameFromPath(string path)
        {
            if (path.EndsWith(".jar", StringComparison.OrdinalIgnoreCase) && File.Exists(path))
                return JarFileUtil.GetModuleName(path);

            return null;
        }

        /// <summary>
        /// Normalizes the reference metadata.
        /// </summary>
        /// <param name="item"></param>
        void NormalizeReferences(ITaskItem item)
        {
            var l = new List<string>();
            foreach (var reference in item.GetMetadata(IkvmJavaReferenceItemMetadata.References).Split(IkvmJavaReferenceItemMetadata.PropertySeperatorCharArray, StringSplitOptions.RemoveEmptyEntries))
                l.Add(NormalizeItemSpec(reference));

            // reset references to new values
            item.SetMetadata(IkvmJavaReferenceItemMetadata.References, string.Join(IkvmJavaReferenceItemMetadata.PropertySeperatorString, l.Distinct()));
        }

        /// <summary>
        /// Resolves the reference metadata to the dependent reference items.
        /// </summary>
        /// <param name="item"></param>
        void ValidateReferences(ITaskItem item)
        {
            ValidateReferences(item, ImmutableHashSet<ITaskItem>.Empty.Add(item));
        }

        /// <summary>
        /// Resolves the reference metadata to the dependent reference items.
        /// </summary>
        /// <param name="item"></param>
        void ValidateReferences(ITaskItem item, ImmutableHashSet<ITaskItem> previous)
        {
            // check each reference of this item
            foreach (var reference in item.GetMetadata(IkvmJavaReferenceItemMetadata.References).Split(IkvmJavaReferenceItemMetadata.PropertySeperatorCharArray, StringSplitOptions.RemoveEmptyEntries))
            {
                // attempt to resolve the reference
                if (TryResolveReference(reference, out var resolved) == false)
                    throw new IkvmTaskException("Could not resolve reference '{0}' on '{1}'.", reference, item.ItemSpec);

                // check that we've not encountered this reference before
                if (previous.Contains(resolved))
                    throw new IkvmTaskException("Detected a circular dependency '{0}' starting at '{1}'.", reference, item.ItemSpec);

                // descend into references
                ValidateReferences(resolved, previous.Add(resolved));
            }
        }

        /// <summary>
        /// Attempts to resolve the given reference.
        /// </summary>
        /// <param name="reference"></param>
        /// <param name="resolved"></param>
        /// <returns></returns>
        bool TryResolveReference(string reference, out ITaskItem resolved)
        {
            reference = NormalizeItemSpec(reference);
            resolved = Items.FirstOrDefault(i => i.ItemSpec == reference);
            return resolved != null;
        }

    }

}
