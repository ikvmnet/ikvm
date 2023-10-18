namespace IKVM.MSBuild.Tasks
{

    using System;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Linq;
    using System.Security;

    static class IkvmTaskUtil
    {

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
        /// <param name="text"></param>
        /// <returns></returns>
        public static string ToHex(byte[] bytes)
        {
            char GetDigit(int value) => value < 10 ? (char)('0' + value) : (char)('7' + value);

            var t = new char[bytes.Length * 3];
            var p = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                if (p > 0)
                    t[p++] = ' ';

                t[p++] = GetDigit((bytes[i] >> 4) & 0xf);
                t[p++] = GetDigit(bytes[i] & 0xf);
            }

            return new string(t, 0, p);
        }

    }

}
