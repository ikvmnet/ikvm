using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Build.Framework;

namespace IKVM.Sdk.Tasks
{

    /// <summary>
    /// Models the required data of a JavaReferenceItem
    /// </summary>
    class JavaReferenceItem
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="item"></param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public JavaReferenceItem(ITaskItem item)
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
        public List<JavaReferenceItem> References { get; set; }

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
            Item.SetMetadata(IkvmJavaReferenceItemMetadata.AssemblyName, AssemblyName);
            Item.SetMetadata(IkvmJavaReferenceItemMetadata.AssemblyVersion, AssemblyVersion);
            Item.SetMetadata(IkvmJavaReferenceItemMetadata.Debug, Debug ? "true" : "false");
            Item.SetMetadata(IkvmJavaReferenceItemMetadata.Compile, string.Join(IkvmJavaReferenceItemMetadata.PropertySeperatorString, Compile));
            Item.SetMetadata(IkvmJavaReferenceItemMetadata.Sources, string.Join(IkvmJavaReferenceItemMetadata.PropertySeperatorString, Sources));
            Item.SetMetadata(IkvmJavaReferenceItemMetadata.References, string.Join(IkvmJavaReferenceItemMetadata.PropertySeperatorString, References.Select(i => i.ItemSpec)));
            Item.SetMetadata(IkvmJavaReferenceItemMetadata.IkvmIdentity, IkvmIdentity);
        }

    }

}
