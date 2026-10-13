namespace IKVM.MSBuild.Tasks
{

    using System;
    using System.IO;

    using Microsoft.Build.Framework;
    using Microsoft.Build.Utilities;

    /// <summary>
    /// Removes the entries of a cache directory that have gone unused for longer than the maximum age. Each directory
    /// directly beneath the cache directory is a single entry, and is removed as a whole.
    /// </summary>
    public class IkvmCleanDir : Task
    {

        /// <summary>
        /// Name of the file within the cleaned directory whose timestamp records when it was last cleaned.
        /// </summary>
        public const string LastCleanFileName = ".lastclean";

        /// <summary>
        /// Gets the time the cache entry was last used: the latest time anything within it was written, which includes
        /// the marker refreshed by <see cref="IkvmTaskUtil.MarkCacheEntryUsed(string)"/>, and the creation of the entry
        /// itself.
        /// </summary>
        /// <param name="entry"></param>
        /// <returns></returns>
        internal static DateTime GetLastUsedTimeUtc(DirectoryInfo entry)
        {
            var last = entry.LastWriteTimeUtc;
            foreach (var file in entry.EnumerateFiles("*", SearchOption.AllDirectories))
                if (file.LastWriteTimeUtc > last)
                    last = file.LastWriteTimeUtc;

            return last;
        }

        /// <summary>
        /// Directory to clean.
        /// </summary>
        [Required]
        public string Directory { get; set; }

        /// <summary>
        /// Age of entries to delete in minutes.
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

            var now = DateTime.UtcNow;
            var maxAge = TimeSpan.FromMinutes(MaxAge);

            try
            {
                // an entry still being created by another build was written recently, and so is kept
                foreach (var entry in dir.EnumerateDirectories())
                {
                    try
                    {
                        if (now - GetLastUsedTimeUtc(entry) <= maxAge)
                            continue;

                        Log.LogMessage(MessageImportance.Low, "Removing cache entry: {0}", entry.FullName);
                        entry.Delete(true);
                    }
                    catch
                    {
                        Log.LogMessage("Failed to remove cache entry: {0}", entry.FullName);
                    }
                }

                // record the clean, so that cleaning can be skipped until it is due again
                IkvmTaskUtil.TouchFile(Path.Combine(dir.FullName, LastCleanFileName));
            }
            catch
            {
                Log.LogMessage("Failed to clean directory: {0}", dir.FullName);
            }

            return true;
        }

    }

}
