using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace IKVM.Tools.Importer
{

    /// <summary>
    /// Remembers the files in the directories that input files are read from, so that each directory is listed once,
    /// instead of once for every input file in it and again when looking for each class's source file next to it.
    /// Belongs to an <see cref="ImportState"/> and lives as long as it does.
    /// </summary>
    /// <remarks>
    /// Answers are only given where they are certain to match what the file system calls would have returned. In every
    /// other case the caller is told to make the file system call itself.
    /// </remarks>
    sealed class InputDirectoryCache
    {

        /// <summary>
        /// Whether <see cref="Directory.GetFiles(string, string)"/> matches file names without regard to case, which
        /// .NET does on Windows and macOS.
        /// </summary>
        static readonly bool MatchIgnoresCase = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        static readonly char[] InvalidFileNameChars = Path.GetInvalidFileNameChars();

        /// <summary>
        /// The files in a directory, as returned by <see cref="Directory.GetFiles(string)"/>, indexed by file name
        /// without regard to case.
        /// </summary>
        sealed class Listing
        {

            public Listing(string[] paths)
            {
                ByName = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
                foreach (var path in paths)
                {
                    var name = Path.GetFileName(path);
                    if (ByName.TryGetValue(name, out var list) == false)
                        ByName.Add(name, list = new List<string>(1));

                    list.Add(path);
                }
            }

            public Dictionary<string, List<string>> ByName { get; }

        }

        readonly ConcurrentDictionary<string, Listing> listings = new ConcurrentDictionary<string, Listing>(StringComparer.Ordinal);

        /// <summary>
        /// Returns what <c>Directory.GetFiles(directory, fileName)</c> returns, or <c>null</c> if the caller should call
        /// it instead. This answers only for plain file names that are found in the directory.
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string[] TryGetFiles(string directory, string fileName)
        {
            if (IsPlainFileName(fileName) == false)
                return null;

            var listing = GetListing(directory);
            if (listing == null || listing.ByName.TryGetValue(fileName, out var candidates) == false)
                return null;

            // the listing is indexed without regard to case, so narrow it down where matching is case-sensitive
            var matches = new List<string>(candidates.Count);
            foreach (var path in candidates)
                if (MatchIgnoresCase || string.Equals(Path.GetFileName(path), fileName, StringComparison.Ordinal))
                    matches.Add(path);

            return matches.Count > 0 ? matches.ToArray() : null;
        }

        /// <summary>
        /// Returns <c>false</c> if the directory certainly contains no file named <paramref name="fileName"/>, in any
        /// case, and <c>true</c> if it might, in which case the caller should check the file system.
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool MayContainFile(string directory, string fileName)
        {
            if (directory == null || IsPlainFileName(fileName) == false)
                return true;

            var listing = GetListing(directory);
            return listing == null || listing.ByName.ContainsKey(fileName);
        }

        /// <summary>
        /// Gets the listing of the directory, or <c>null</c> if it could not be listed.
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        Listing GetListing(string directory)
        {
            return listings.GetOrAdd(directory, static d =>
            {
                try
                {
                    return new Listing(Directory.GetFiles(d));
                }
                catch
                {
                    return null;
                }
            });
        }

        /// <summary>
        /// Returns whether the name can only match a file of exactly that name: it contains no wildcards or path
        /// separators, is not a relative directory name, and has no trailing dot or space, which Windows would remove.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        static bool IsPlainFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || fileName == "." || fileName == "..")
                return false;

            if (fileName.IndexOfAny(InvalidFileNameChars) >= 0)
                return false;

            // wildcards as Directory.GetFiles understands them, which are not invalid file name characters everywhere
            foreach (var c in fileName)
                if (c == '*' || c == '?' || c == '<' || c == '>' || c == '"')
                    return false;

            var last = fileName[fileName.Length - 1];
            return last != '.' && last != ' ';
        }

    }

}
