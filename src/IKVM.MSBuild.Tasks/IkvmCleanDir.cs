namespace IKVM.MSBuild.Tasks
{

    using System;
    using System.IO;
    using System.Linq;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    /// <summary>
    /// Canonicalizes the paths either in the item specification or the specified metadata name.
    /// </summary>
    public class IkvmCleanDir : Task
    {

        /// <summary>
        /// Directory to clean.
        /// </summary>
        [Required]
        public string Directory { get; set; }

        /// <summary>
        /// Age of items to delete in minutes.
        /// </summary>
        [Required]
        public int MaxAge { get; set; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            if (string.IsNullOrWhiteSpace(Directory))
                return false;

            // fail on non-rooted directory
            if (Path.IsPathRooted(Directory) == false)
                return false;

            var dir = new DirectoryInfo(Directory);
            if (dir.Exists == false)
                return true;

            // skip directories that are not nested a couple layers to prevent accidental deletion of an entire drive
            if (dir.Parent == null || dir.Parent.Parent == null)
                return true;

            try
            {

                foreach (var file in dir.EnumerateFiles("*", SearchOption.AllDirectories))
                {
                    // skip if file access time was within maxage
                    if (DateTime.UtcNow - file.LastWriteTimeUtc <= TimeSpan.FromMinutes(MaxAge))
                        continue;

                    try
                    {
                        file.Delete();
                    }
                    catch
                    {
                        Log.LogMessage("Failed to remove file: {0}", file.FullName);
                    }
                }

                foreach (var path in dir.EnumerateDirectories("*", SearchOption.AllDirectories))
                {
                    // skip directory with contents
                    if (path.EnumerateFileSystemInfos().Any())
                        continue;

                    try
                    {
                        path.Delete(false);
                    }
                    catch
                    {
                        Log.LogMessage("Failed to remove directory: {0}", path.FullName);
                    }
                }
            }
            catch
            {
                Log.LogMessage("Failed to clean directory: {0}", dir.FullName);
            }

            return true;
        }

    }

}
