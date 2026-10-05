namespace IKVM.MSBuild.Tasks
{

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    /// <summary>
    /// Describes <see cref="IkvmReferenceItem"/> items for design-time consumers such as Visual Studio. Assigns the
    /// metadata that would be calculated during a build, but never fails: each item instead reports whether it could
    /// be resolved, and why not.
    /// </summary>
    public class IkvmReferenceItemDescribe : Task
    {

        public const string IsResolvedMetadataName = "IkvmIsResolved";
        public const string DiagnosticMetadataName = "IkvmDiagnostic";
        public const string OriginalItemSpecMetadataName = "OriginalItemSpec";

        /// <summary>
        /// <see cref="IkvmReferenceItem"/> items to describe.
        /// </summary>
        [Required]
        public ITaskItem[] Items { get; set; }

        /// <summary>
        /// Described items, with the same item specs as the input items.
        /// </summary>
        [Output]
        public ITaskItem[] DescribedItems { get; set; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            DescribedItems = Items.Select(Describe).ToArray();
            return true;
        }

        /// <summary>
        /// Describes a single item.
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static ITaskItem Describe(ITaskItem source)
        {
            var result = new TaskItem(source.ItemSpec);
            source.CopyMetadataTo(result);
            result.SetMetadata(OriginalItemSpecMetadataName, source.ItemSpec);

            try
            {
                // work on a copy, since import and assignment rewrite the item spec and metadata; references between
                // items are not needed to describe a single item
                var copy = new TaskItem(source);
                copy.SetMetadata(IkvmReferenceItemMetadata.References, "");
                var item = IkvmReferenceItem.Import(new[] { copy })[0];
                IkvmReferenceItemPrepare.AssignMetadata(item);

                result.SetMetadata(IkvmReferenceItemMetadata.AssemblyName, item.AssemblyName ?? "");
                result.SetMetadata(IkvmReferenceItemMetadata.AssemblyVersion, item.AssemblyVersion ?? "");
                result.SetMetadata(IkvmReferenceItemMetadata.AssemblyFileVersion, item.AssemblyFileVersion ?? "");
                result.SetMetadata(IkvmReferenceItemMetadata.Compile, string.Join(IkvmReferenceItemMetadata.PropertySeperatorString, item.Compile));
                result.SetMetadata(IkvmReferenceItemMetadata.Sources, string.Join(IkvmReferenceItemMetadata.PropertySeperatorString, item.Sources));

                var diagnostic = GetDiagnostic(source.ItemSpec, item);
                result.SetMetadata(IsResolvedMetadataName, diagnostic == null ? "true" : "false");
                result.SetMetadata(DiagnosticMetadataName, diagnostic ?? "");
            }
            catch (Exception e)
            {
                result.SetMetadata(IsResolvedMetadataName, "false");
                result.SetMetadata(DiagnosticMetadataName, e.Message);
            }

            return result;
        }

        /// <summary>
        /// Gets a message describing why the item cannot be built, or <c>null</c> if it can be.
        /// </summary>
        /// <param name="itemSpec"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        static string GetDiagnostic(string itemSpec, IkvmReferenceItem item)
        {
            if (item.Compile.Count == 0)
                return $"'{itemSpec}' was not found, and no Compile paths are specified.";

            foreach (var compile in item.Compile)
                if (File.Exists(compile) == false && Directory.Exists(compile) == false)
                    return $"Compile path '{compile}' was not found.";

            foreach (var source in item.Sources)
                if (File.Exists(source) == false && Directory.Exists(source) == false)
                    return $"Sources path '{source}' was not found.";

            if (string.IsNullOrWhiteSpace(item.AssemblyName))
                return "No assembly name could be determined. Set AssemblyName or FallbackAssemblyName.";

            if (string.IsNullOrWhiteSpace(item.AssemblyVersion))
                return "No assembly version could be determined. Set AssemblyVersion or FallbackAssemblyVersion.";

            return null;
        }

    }

}
