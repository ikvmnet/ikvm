namespace IKVM.MSBuild.Tasks
{

    using System.IO;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    /// <summary>
    /// Canonicalizes the paths either in the item specification or the specified metadata name.
    /// </summary>
    public class IkvmCanonicalizePath : Task
    {

        /// <summary>
        /// Items to canonicalize.
        /// </summary>
        [Required]
        [Output]
        public ITaskItem[] Items { get; set; }

        /// <summary>
        /// Name of the metadata to canonicalize.
        /// </summary>
        public string MetadataName { get; set; } = "Identity";

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            var metadataNames = MetadataName.Split(';');
            if (metadataNames.Length == 0)
                return true;

            for (int i = 0; i < Items.Length; i++)
            {
                foreach (var metadataName in metadataNames)
                {
                    if (metadataName == "Identity")
                        Items[i].ItemSpec = TryCanonicalizePath(Items[i].ItemSpec);
                    else
                        Items[i].SetMetadata(metadataName, TryCanonicalizePaths(Items[i].GetMetadata(metadataName)));
                }
            }

            return true;
        }

        /// <summary>
        /// Optionally concatinates the array of paths.
        /// </summary>
        /// <param name="paths"></param>
        /// <returns></returns>
        string TryCanonicalizePaths(string paths)
        {
            return paths != null ? string.Join(Path.PathSeparator.ToString(), paths.Split(Path.PathSeparator)) : null;
        }

        /// <summary>
        /// Optionally canonicalizes the path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        string TryCanonicalizePath(string path)
        {
            return path != null && Path.IsPathRooted(path) ? IkvmTaskUtil.CanonicalizePath(path) : path;
        }

    }

}
