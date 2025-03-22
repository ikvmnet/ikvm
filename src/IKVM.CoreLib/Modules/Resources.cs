using System;
using System.IO;

namespace IKVM.CoreLib.Modules
{

    static class Resources
    {

        /// <summary>
        /// Derive a <c>package name</c> for a resource. The package name returned by this method may not be a legal
        /// package name. This method returns null if the resource name ends with a "/" (a directory) or the resource
        /// name does not contain a "/".
        /// </summary>
        public static string ToPackageName(string name)
        {
            int index = name.LastIndexOf('/');
            if (index == -1 || index == name.Length - 1)
            {
                return "";
            }
            else
            {
                return name.Substring(0, index).Replace('/', '.');
            }
        }

        /// <summary>
        /// Returns a resource name corresponding to the relative file path between <paramref name="dir"/> and
        /// <paramref name="file"/>. If the file is a directory then the name will end with a "/", except the top-level
        /// directory where the empty string is returned.
        /// </summary>
        public static string ToResourceName(string dir, string file)
        {
            // full dir path, with ending separator
            dir = Path.GetFullPath(dir);
            if (dir.EndsWith(Path.DirectorySeparatorChar) == false)
                dir += Path.DirectorySeparatorChar;

            // normalize relative path off of dir
            file = Path.Combine(dir, file);
            if (file.StartsWith(dir) == false)
                throw new InvalidOperationException("Resource file does not exist in directory.");

            // cut directory off of file path
            file = file[dir.Length..];
            if (Path.DirectorySeparatorChar != '/')
                file = file.Replace(Path.DirectorySeparatorChar, '/');

            // ensure directory path ends with separator
            if (file != "" && Directory.Exists(file))
                file += "/";

            return file;
        }

        /// <summary>
        /// Returns a file path to a resource in a file tree. If the resource name has a trailing "/" then the file
        /// path will locate a directory. Returns <c>null</c> if the resource does not map to a file in the tree file.
        /// </summary>
        /// <returns></returns>
        public static string? ToFilePath(string dir, string name)
        {
            var expectDirectory = name.EndsWith('/');
            if (expectDirectory)
                name = name.TrimEnd('/');

            var path = ToSafeFilePath(name);
            if (path is not null)
            {
                var file = Path.Combine(dir, path);
                if (Directory.Exists(file) || (Directory.Exists(file) == false && expectDirectory == false))
                    return file;
            }

            return null;
        }

        /// <summary>
        /// Map a resource name to a "safe" file path.Returns <c>null</c> if the resource name cannot be converted into
        /// a "safe" file path.
        ///
        /// Resource names with empty elements, or elements that are "." or ".." are rejected, as are resource names
        /// that translates to a file path with a root component.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static string? ToSafeFilePath(string name)
        {
            // scan elements of resource name
            int next;
            int off = 0;
            while ((next = name.IndexOf('/', off)) != -1)
            {
                int len = next - off;
                if (MayTranslate(name, off, len) == false)
                    return null;

                off = next + 1;
            }

            int rem = name.Length - off;
            if (MayTranslate(name, off, rem) == false)
                return null;

            // map resource name to a file path string
            string path;
            if (Path.DirectorySeparatorChar == '/')
            {
                path = name;
            }
            else
            {
                // not allowed to embed file separators
                if (name.Contains(Path.DirectorySeparatorChar))
                    return null;

                // convert to native separator
                path = name.Replace('/', Path.DirectorySeparatorChar);
            }

            // file path not allowed to have root component
            return Path.IsPathRooted(path) == false ? path : null;
        }

        /// <summary>
        /// Returns <c>true</c> if the element in a resource name is a candidate to translate to the element of a file
        /// path.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="off"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        static bool MayTranslate(string name, int off, int len)
        {
            if (len <= 2)
            {
                if (len == 0)
                    return false;

                var starsWithDot = (name[off] == '.');
                if (len == 1 && starsWithDot)
                    return false;
                if (len == 2 && starsWithDot && (name[off + 1] == '.'))
                    return false;
            }

            return true;
        }

    }

}
