namespace IKVM.MSBuild.Tasks
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Build.Framework;

    /// <summary>
    /// Models the required data of a <see cref="IkvmReferenceExportItem"/>.
    /// </summary>
    class IkvmReferenceExportItem
    {

        /// <summary>
        /// Attempts to import a set of <see cref="IkvmReferenceExportItem"/> instances from the given <see cref="ITaskItem"/> instances.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static IkvmReferenceExportItem[] Import(IEnumerable<ITaskItem> items)
        {
            if (items is null)
                throw new ArgumentNullException(nameof(items));

            // normalize itemspecs into a dictionary
            var map = new Dictionary<string, IkvmReferenceExportItem>();
            foreach (var item in items)
                map[item.ItemSpec] = new IkvmReferenceExportItem(item);

            // populate the properties of each item
            foreach (var item in map.Values)
            {
                item.ItemSpec = item.Item.ItemSpec;
                item.References = item.Item.GetMetadata(IkvmReferenceExportItemMetadata.References)?.Split(IkvmReferenceExportItemMetadata.PropertySeperatorCharArray, StringSplitOptions.RemoveEmptyEntries).ToList();
                item.Namespaces = item.Item.GetMetadata(IkvmReferenceExportItemMetadata.Namespaces)?.Split(IkvmReferenceExportItemMetadata.PropertySeperatorCharArray, StringSplitOptions.RemoveEmptyEntries).ToList();
                item.Shared = string.Equals(item.Item.GetMetadata(IkvmReferenceExportItemMetadata.Shared), "true", StringComparison.OrdinalIgnoreCase);
                item.NoStdLib = string.Equals(item.Item.GetMetadata(IkvmReferenceExportItemMetadata.NoStdLib), "true", StringComparison.OrdinalIgnoreCase);
                item.Forwarders = string.Equals(item.Item.GetMetadata(IkvmReferenceExportItemMetadata.Forwarders), "true", StringComparison.OrdinalIgnoreCase);
                item.IncludeNonPublicTypes = string.Equals(item.Item.GetMetadata(IkvmReferenceExportItemMetadata.IncludeNonPublicTypes), "true", StringComparison.OrdinalIgnoreCase);
                item.IncludeNonPublicInterfaces = string.Equals(item.Item.GetMetadata(IkvmReferenceExportItemMetadata.IncludeNonPublicInterfaces), "true", StringComparison.OrdinalIgnoreCase);
                item.IncludeNonPublicMembers = string.Equals(item.Item.GetMetadata(IkvmReferenceExportItemMetadata.IncludeNonPublicMembers), "true", StringComparison.OrdinalIgnoreCase);
                item.IncludeParameterNames = string.Equals(item.Item.GetMetadata(IkvmReferenceExportItemMetadata.IncludeParameterNames), "true", StringComparison.OrdinalIgnoreCase);
                item.Bootstrap = string.Equals(item.Item.GetMetadata(IkvmReferenceExportItemMetadata.Bootstrap), "true", StringComparison.OrdinalIgnoreCase);
                item.IkvmIdentity = item.Item.GetMetadata(IkvmReferenceExportItemMetadata.IkvmIdentity);
                item.RandomIndex = item.Item.GetMetadata(IkvmReferenceExportItemMetadata.RandomIndex) is string s && int.TryParse(s, out var i) ? i : null;
                item.Save();
            }

            // return the resulting imported references
            return map.Values.ToArray();
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="item"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReferenceExportItem(ITaskItem item)
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
        /// References required to export.
        /// </summary>
        public List<string> References { get; set; } = new List<string>();

        /// <summary>
        /// Namespaces to export.
        /// </summary>
        public List<string> Namespaces { get; set; }

        public bool Shared { get; set; }

        public bool NoStdLib { get; set; }

        public bool Forwarders { get; set; }

        public bool IncludeNonPublicTypes { get; set; }

        public bool IncludeNonPublicInterfaces { get; set; }

        public bool IncludeNonPublicMembers { get; set; }

        public bool IncludeParameterNames { get; set; }

        public bool Bootstrap { get; set; }

        /// <summary>
        /// Unique IKVM identity of the export.
        /// </summary>
        public string IkvmIdentity { get; set; }

        public int? RandomIndex { get; set; }

        /// <summary>
        /// Writes the metadata to the item.
        /// </summary>
        public void Save()
        {
            Item.ItemSpec = ItemSpec;
            Item.SetMetadata(IkvmReferenceExportItemMetadata.References, string.Join(IkvmReferenceExportItemMetadata.PropertySeperatorString, References));
            Item.SetMetadata(IkvmReferenceExportItemMetadata.Namespaces, string.Join(IkvmReferenceExportItemMetadata.PropertySeperatorString, Namespaces));
            Item.SetMetadata(IkvmReferenceExportItemMetadata.Shared, Shared ? "true" : "false");
            Item.SetMetadata(IkvmReferenceExportItemMetadata.NoStdLib, NoStdLib ? "true" : "false");
            Item.SetMetadata(IkvmReferenceExportItemMetadata.Forwarders, Forwarders ? "true" : "false");
            Item.SetMetadata(IkvmReferenceExportItemMetadata.IncludeNonPublicTypes, IncludeNonPublicTypes ? "true" : "false");
            Item.SetMetadata(IkvmReferenceExportItemMetadata.IncludeNonPublicInterfaces, IncludeNonPublicInterfaces ? "true" : "false");
            Item.SetMetadata(IkvmReferenceExportItemMetadata.IncludeNonPublicMembers, IncludeNonPublicMembers ? "true" : "false");
            Item.SetMetadata(IkvmReferenceExportItemMetadata.IncludeParameterNames, IncludeParameterNames ? "true" : "false");
            Item.SetMetadata(IkvmReferenceExportItemMetadata.Bootstrap, Bootstrap ? "true" : "false");
            Item.SetMetadata(IkvmReferenceExportItemMetadata.IkvmIdentity, IkvmIdentity);
            Item.SetMetadata(IkvmReferenceExportItemMetadata.RandomIndex, RandomIndex?.ToString());
        }

    }

}
