using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Build.Framework;

namespace IKVM.MSBuild.Tasks
{

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
        /// Compile in debug mode.
        /// </summary>
        public bool Debug { get; set; }

        /// <summary>
        /// Set of sources to compile.
        /// </summary>
        public List<string> Compile { get; set; }

        /// <summary>
        /// Set of Java sources which can be used to generate documentation.
        /// </summary>
        public List<string> Sources { get; set; }

        /// <summary>
        /// References required to compile.
        /// </summary>
        public List<IkvmReferenceItem> References { get; set; }

        /// <summary>
        /// Unique IKVM identity of the reference.
        /// </summary>
        public string IkvmIdentity { get; set; }

        /// <summary>
        /// Writes the metadata to the item.
        /// </summary>
        public void Save()
        {
            Item.ItemSpec = ItemSpec;
            Item.SetMetadata(IkvmReferenceItemMetadata.AssemblyName, AssemblyName);
            Item.SetMetadata(IkvmReferenceItemMetadata.AssemblyVersion, AssemblyVersion);
            Item.SetMetadata(IkvmReferenceItemMetadata.Debug, Debug ? "true" : "false");
            Item.SetMetadata(IkvmReferenceItemMetadata.Compile, string.Join(IkvmReferenceItemMetadata.PropertySeperatorString, Compile));
            Item.SetMetadata(IkvmReferenceItemMetadata.Sources, string.Join(IkvmReferenceItemMetadata.PropertySeperatorString, Sources));
            Item.SetMetadata(IkvmReferenceItemMetadata.References, string.Join(IkvmReferenceItemMetadata.PropertySeperatorString, References.Select(i => i.ItemSpec)));
            Item.SetMetadata(IkvmReferenceItemMetadata.IkvmIdentity, IkvmIdentity);
        }

    }

}
