using System;
using System.IO;
using System.Text.RegularExpressions;

namespace IKVM.Util.Jar
{

    /// <summary>
    /// Provides utilities for working with JAR files.
    /// </summary>
    public static class JarFileUtil
    {

        /// <summary>
        /// Attempts to get the module name from the given classpath entry.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        public static string GetModuleName(string path)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path));
            if (path.EndsWith(".jar", StringComparison.OrdinalIgnoreCase) == false)
                throw new ArgumentException("Path must refer to a JAR file.", nameof(path));
            if (File.Exists(path) == false)
                throw new FileNotFoundException("Cannot find JAR file.", path);

            using var jar = new JarFile(path);
            return jar.GetModuleName() ?? GetModuleNameFromFileName(path);
        }

        /// <summary>
        /// Derives the module name from the name of the file given by the path, as described by the specification.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static string GetModuleNameFromFileName(string name)
        {
            var vstr = (string)null;

            // The ".jar" suffix is removed.
            name = Path.GetFileNameWithoutExtension(name);
            var span = (Span<char>)stackalloc char[name.Length];
            name.AsSpan().CopyTo(span);

            // If the name matches the regular expression "-(\\d+(\\.|$))" then the module name will be derived from the subsequence preceding the hyphen of the first occurrence.
            if (Regex.Match(name, @"-(\d+(\.|$))") is Match m)
            {
                vstr = TryParseVersion(span.Slice(m.Index));
                span = span.Slice(0, m.Index);
            }

            // All non-alphanumeric characters ([^A-Za-z0-9]) in the module name are replaced with a dot ("."),
            for (var i = 0; i < span.Length; i++)
                if (char.IsLetterOrDigit(span[i]) == false)
                    span[i] = '.';

            // all repeating dots are replaced with one dot,
            name = span.ToString();
            while (name.IndexOf("..") is int i && i != -1)
                name = name.Remove(i, 1);

            // and all leading and trailing dots are removed.
            name = name.Trim('.');

            return name;
        }

        /// <summary>
        /// Parses a Version-String.
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        static string TryParseVersion(ReadOnlySpan<char> span)
        {
            return null;
        }

    }

}
