namespace IKVM.MSBuild.Tasks
{

    using System;
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
        /// Initializes a new instance.
        /// </summary>
        public IkvmReferenceItemDescribe() :
            base(Resources.SR.ResourceManager, "IKVM:")
        {

        }

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
        internal ITaskItem Describe(ITaskItem source)
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

                // the problems a build would report, worded as it would report them
                var diagnostics = IkvmReferenceItemPrepare.GetDiagnostics(item);
                result.SetMetadata(IsResolvedMetadataName, diagnostics.Count == 0 ? "true" : "false");
                result.SetMetadata(DiagnosticMetadataName, string.Join(Environment.NewLine, diagnostics.Select(i => Log.FormatResourceString(i.Resource, i.Args))));
            }
            catch (Exception e)
            {
                result.SetMetadata(IsResolvedMetadataName, "false");
                result.SetMetadata(DiagnosticMetadataName, e.Message);
            }

            return result;
        }

    }

}
