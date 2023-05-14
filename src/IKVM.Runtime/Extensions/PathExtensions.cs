using System.IO;

namespace IKVM.Runtime.Extensions
{

    /// <summary>
    /// Various extension methods for working with paths.
    /// </summary>
    static class PathExtensions
    {

        public static readonly char[] DirectorySeparatorChars = new char[] { Path.DirectorySeparatorChar };

        /// <summary>
        /// Trims any ending directory separator characters are removed from the path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string TrimEndingDirectorySeparator(string path)
        {
#if NETFRAMEWORK
            return path.TrimEnd(DirectorySeparatorChars);
#else
            return Path.TrimEndingDirectorySeparator(path);
#endif
        }

        /// <summary>
        /// Ensures the given path ends with a directory separator.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string EnsureEndingDirectorySeparator(string path)
        {
            return path[path.Length - 1] != Path.DirectorySeparatorChar ? path + Path.DirectorySeparatorChar : path;
        }

    }

}
