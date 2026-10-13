namespace IKVM.MSBuild.Tasks
{

    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Linq;
    using System.Security;
    using System.Xml.Linq;

    static class IkvmTaskUtil
    {

        static char[] hexChars => new char[16] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

        /// <summary>
        /// Name of the file within a cache entry whose timestamp records when the entry was last used.
        /// </summary>
        public const string CacheEntryUsedFileName = ".used";

        /// <summary>
        /// Records that the cache entry in the given directory was used, so that cleaning the cache keeps it. Cleaning
        /// measures age in days, so the record is refreshed at most once a day.
        /// </summary>
        /// <param name="entryDirectory"></param>
        public static void MarkCacheEntryUsed(string entryDirectory)
        {
            try
            {
                if (Directory.Exists(entryDirectory) == false)
                    return;

                var used = new FileInfo(Path.Combine(entryDirectory, CacheEntryUsedFileName));
                if (used.Exists && DateTime.UtcNow - used.LastWriteTimeUtc < TimeSpan.FromDays(1))
                    return;

                TouchFile(used.FullName);
            }
            catch (IOException)
            {
                // another build may be marking or removing the same entry
            }
            catch (UnauthorizedAccessException)
            {

            }
        }

        /// <summary>
        /// Saves the element to the given file, unless the file already holds the same content.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="path"></param>
        public static void SaveXmlIfChanged(XElement root, string path)
        {
            var xml = root.ToString();
            if (File.Exists(path) && File.ReadAllText(path) == xml)
                return;

            var dir = Path.GetDirectoryName(path);
            if (Directory.Exists(dir) == false)
                Directory.CreateDirectory(dir);

            File.WriteAllText(path, xml);
        }

        /// <summary>
        /// Sets the last write time of the given file to now, creating it if it does not exist.
        /// </summary>
        /// <param name="path"></param>
        public static void TouchFile(string path)
        {
            if (File.Exists(path))
                File.SetLastWriteTimeUtc(path, DateTime.UtcNow);
            else
                File.WriteAllBytes(path, new byte[0]);
        }

        /// <summary>
        /// Attempts to canonicalize the given path.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="cache"></param>
        /// <returns></returns>
        public static string CanonicalizePath(string path, ConcurrentDictionary<string, string> cache)
        {
            return cache.GetOrAdd(path, p => CanonicalizePathImpl(p, cache));
        }

        /// <summary>
        /// Implementation to canonicalize a path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static string CanonicalizePathImpl(string path, ConcurrentDictionary<string, string> cache)
        {
            try
            {
                // begin by processing parent element
                var parent = Path.GetDirectoryName(path);

                // root paths with drive letter should have driver letter upper cased
                if (parent == null)
                    return path.Length > 1 && path[1] == ':' ? $"{char.ToUpper(path[0])}:{Path.DirectorySeparatorChar}" : path;
                else
                    parent = CanonicalizePath(parent, cache);

                // trailing slash would result in a last path element of empty string
                var name = Path.GetFileName(path);
                if (name == "" || name == ".")
                    return parent;
                if (name == "..")
                    return Path.GetDirectoryName(parent);

                try
                {
                    // consult the file system for an actual node with the appropriate name
                    var all = Directory.EnumerateFileSystemEntries(parent, name);
                    if (all.FirstOrDefault() is string one)
                        name = Path.GetFileName(one);
                }
                catch (UnauthorizedAccessException)
                {

                }
                catch (IOException)
                {

                }

                return Path.Combine(parent, name);
            }
            catch (UnauthorizedAccessException)
            {

            }
            catch (IOException)
            {

            }
            catch (SecurityException)
            {

            }
            catch (NotSupportedException)
            {

            }

            return path;
        }

        /// <summary>
        /// Gets the relative path for the given path from the specified folder.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="relativeTo"></param>
        /// <returns></returns>
        public static string GetRelativePath(string relativeTo, string path)
        {
            var pathUri = new Uri(Path.GetFullPath(path));
            if (relativeTo.EndsWith(Path.DirectorySeparatorChar.ToString()) == false)
                relativeTo += Path.DirectorySeparatorChar;

            var relativeToUri = new Uri(Path.GetFullPath(relativeTo));
            return Uri.UnescapeDataString(relativeToUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        /// <summary>
        /// Converts a series of bytes to a hexidecimal string.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHex(byte[] bytes)
        {
            var t = new char[bytes.Length * 2];

            for (int i = 0; i < bytes.Length; i++)
            {
                t[i * 2] = hexChars[bytes[i] >> 4];
                t[i * 2 + 1] = hexChars[bytes[i] & 0xF];
            }

            return new string(t);
        }

    }

}
