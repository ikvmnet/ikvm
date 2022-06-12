using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

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

        [Required]
        [Output]
        public ITaskItem[] Items { get; set; }

        public override bool Execute()
        {
            foreach (var item in Items)
                AssignMetadataForItem(item);

            return true;
        }

        /// <summary>
        /// Assigns the metadata to the item.
        /// </summary>
        /// <param name="item"></param>
        void AssignMetadataForItem(ITaskItem item)
        {
            // the identity may itself be a JAR or directory, if so, consider it a ClassPath
            if (Path.GetExtension(item.ItemSpec) == ".jar" && File.Exists(item.ItemSpec))
                PrependCompileToItem(item, item.ItemSpec);
            else if (Directory.Exists(item.ItemSpec))
                PrependCompileToItem(item, item.ItemSpec);

            // probe the classpath's for available metadata
            ExpandCompileMetadata(item);
            AssignMetadataFromCompile(item);

            // ensure the item references are populated
            ValidateReferences(item);
        }

        /// <summary>
        /// Expands each entry in the Compile metadata.
        /// </summary>
        /// <param name="item"></param>
        void ExpandCompileMetadata(ITaskItem item)
        {
            var l = new List<string>();
            foreach (var c in item.GetMetadata("Compile").Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                ExpandCompileMetadata(item, l, c);

            // reset Compile metadata to expanded values
            item.SetMetadata("Compile", string.Join(";", l.Distinct()));
        }

        /// <summary>
        /// Expands the path to real underlying files.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="list"></param>
        /// <param name="path"></param>
        /// <exception cref="InvalidOperationException"></exception>
        internal void ExpandCompileMetadata(ITaskItem item, List<string> list, string path)
        {
            // if the path is a glob, we're going to match items, else skip
            var glob = MSBuildGlob.Parse(path);
            if (glob.IsLegal == false)
            {
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
                    list.Add(i);
        }

        /// <summary>
        /// Prepends the given classpath as a member of the ClassPath metadata.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="classpath"></param>
        void PrependCompileToItem(ITaskItem item, string classpath)
        {
            // prepend identity to the classpath if it isn't already present
            var cp = item.GetMetadata("Compile").Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            cp.Insert(0, Path.GetFullPath(item.ItemSpec));
            item.SetMetadata("Compile", string.Join(";", cp.Where(s => !string.IsNullOrWhiteSpace(s)).Distinct()));
        }

        /// <summary>
        /// Assigns the metadata to the item derived from the Compile items.
        /// </summary>
        /// <param name="item"></param>
        void AssignMetadataFromCompile(ITaskItem item)
        {
            foreach (var cp in item.GetMetadata("Compile").Split(new[] { ';' }))
                AssignMetadataFromCompile(item, cp);
        }

        /// <summary>
        /// Assigns the metadata to the item which is a directory.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="path"></param>
        void AssignMetadataFromCompile(ITaskItem item, string path)
        {
            if (string.IsNullOrWhiteSpace(item.GetMetadata("AssemblyName")))
                item.SetMetadata("AssemblyName", ""); // TODO probe classpath

            if (string.IsNullOrWhiteSpace(item.GetMetadata("AssemblyVersion")))
                item.SetMetadata("AssemblyVersion", "0.0.0.0"); // TODO probe classpath
        }

        /// <summary>
        /// Resolves the reference metadata to the dependent reference items.
        /// </summary>
        /// <param name="item"></param>
        /// <exception cref="NotImplementedException"></exception>
        void ValidateReferences(ITaskItem item)
        {
            ValidateReferences(item, ImmutableHashSet<ITaskItem>.Empty.Add(item));
        }

        /// <summary>
        /// Resolves the reference metadata to the dependent reference items.
        /// </summary>
        /// <param name="item"></param>
        /// <exception cref="NotImplementedException"></exception>
        void ValidateReferences(ITaskItem item, ImmutableHashSet<ITaskItem> previous)
        {
            // check each reference of this item
            foreach (var reference in item.GetMetadata("References").Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
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
            resolved = Items.FirstOrDefault(i => i.ItemSpec == reference);
            return resolved != null;
        }

    }

}
