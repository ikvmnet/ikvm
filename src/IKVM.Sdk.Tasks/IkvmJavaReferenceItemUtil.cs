using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Build.Framework;

namespace IKVM.Sdk.Tasks
{

    /// <summary>
    /// Provides common utility methods for working with <see cref="JavaReferenceItem"/> sets.
    /// </summary>
    static class IkvmJavaReferenceItemUtil
    {

        /// <summary>
        /// Returns a normalized version of a <see cref="JavaReferenceItem"/> itemspec.
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
        /// Attempts to import a set of <see cref="JavaReferenceItem"/> instances from the given <see cref="ITaskItem"/> instances.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static JavaReferenceItem[] Import(IEnumerable<ITaskItem> items)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));

            // normalize itemspecs into a dictionary
            var map = new Dictionary<string, JavaReferenceItem>();
            foreach (var item in items)
                map[NormalizeItemSpec(item.ItemSpec)] = new JavaReferenceItem(item);

            // populate the properties of each item
            foreach (var item in map.Values)
            {
                item.ItemSpec = NormalizeItemSpec(item.Item.ItemSpec);
                item.AssemblyName = item.Item.GetMetadata(IkvmJavaReferenceItemMetadata.AssemblyName);
                item.AssemblyVersion = item.Item.GetMetadata(IkvmJavaReferenceItemMetadata.AssemblyVersion);
                item.Debug = bool.TryParse(item.Item.GetMetadata(IkvmJavaReferenceItemMetadata.Debug), out var b) && b;
                item.Compile = item.Item.GetMetadata(IkvmJavaReferenceItemMetadata.Compile)?.Split(IkvmJavaReferenceItemMetadata.PropertySeperatorCharArray, StringSplitOptions.RemoveEmptyEntries).ToList();
                item.Sources = item.Item.GetMetadata(IkvmJavaReferenceItemMetadata.Sources)?.Split(IkvmJavaReferenceItemMetadata.PropertySeperatorCharArray, StringSplitOptions.RemoveEmptyEntries).ToList();
                item.References = ResolveReferences(map, item, item.Item.GetMetadata(IkvmJavaReferenceItemMetadata.References)).ToList();
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
        static List<JavaReferenceItem> ResolveReferences(Dictionary<string, JavaReferenceItem> map, JavaReferenceItem item, string references)
        {
            if (map is null)
                throw new ArgumentNullException(nameof(map));
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            var l = new List<JavaReferenceItem>();
            foreach (var itemSpec in references.Split(IkvmJavaReferenceItemMetadata.PropertySeperatorCharArray, StringSplitOptions.RemoveEmptyEntries))
                if (TryResolveReference(map, itemSpec, out var resolved))
                    l.Add(resolved);
                else
                    throw new IkvmTaskException($"Could not resolve reference '{itemSpec}' on '{item.ItemSpec}'.");

            return l;
        }

        /// <summary>
        /// Attempts to resolve the given <see cref="JavaReferenceItem"/> itemspec against the set of  <see cref="JavaReferenceItem"/> instances
        /// </summary>
        /// <param name="itemSpec"></param>
        /// <param name="resolved"></param>
        /// <returns></returns>
        static bool TryResolveReference(Dictionary<string, JavaReferenceItem> map, string itemSpec, out JavaReferenceItem resolved)
        {
            if (map is null)
                throw new ArgumentNullException(nameof(map));
            if (string.IsNullOrEmpty(itemSpec))
                throw new ArgumentException($"'{nameof(itemSpec)}' cannot be null or empty.", nameof(itemSpec));

            resolved = map.TryGetValue(NormalizeItemSpec(itemSpec), out var r) ? r : null;
            return resolved != null;
        }

    }

}
