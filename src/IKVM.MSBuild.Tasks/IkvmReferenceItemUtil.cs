using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Build.Framework;

namespace IKVM.MSBuild.Tasks
{

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
                item.Aliases = item.Item.GetMetadata(IkvmReferenceItemMetadata.Aliases);
                item.Debug = string.Equals(item.Item.GetMetadata(IkvmReferenceItemMetadata.Debug), "true", StringComparison.OrdinalIgnoreCase);
                item.KeyFile = item.Item.GetMetadata(IkvmReferenceItemMetadata.KeyFile);
                item.DelaySign = string.Equals(item.Item.GetMetadata(IkvmReferenceItemMetadata.DelaySign), "true", StringComparison.OrdinalIgnoreCase);
                item.Compile = item.Item.GetMetadata(IkvmReferenceItemMetadata.Compile)?.Split(IkvmReferenceItemMetadata.PropertySeperatorCharArray, StringSplitOptions.RemoveEmptyEntries).ToList();
                item.Sources = item.Item.GetMetadata(IkvmReferenceItemMetadata.Sources)?.Split(IkvmReferenceItemMetadata.PropertySeperatorCharArray, StringSplitOptions.RemoveEmptyEntries).ToList();
                item.References = ResolveReferences(map, item, item.Item.GetMetadata(IkvmReferenceItemMetadata.References)).ToList();
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
                    throw new IkvmTaskMessageException("Error.IkvmInvalidReference", item.ItemSpec, itemSpec);

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

}
