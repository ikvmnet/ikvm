using System;
using System.IO;
using System.Text.RegularExpressions;

using IKVM.Util.Modules;

namespace IKVM.Util.Jar
{

    /// <summary>
    /// Provides utilities for working with JAR files.
    /// </summary>
    public static class JarFileUtil
    {

        /// <summary>
        /// Describes a module.
        /// </summary>
        public class ModuleInfo
        {

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            public ModuleInfo()
            {

            }

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="version"></param>
            public ModuleInfo(string name, ModuleVersion version)
            {
                Name = name;
                Version = version;
            }

            /// <summary>
            /// Name of the module.
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Version of the module.
            /// </summary>
            public ModuleVersion Version { get; set; }

        }

        /// <summary>
        /// Attempts to get the module name from the given classpath entry.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        public static ModuleInfo GetModuleInfo(string path)
        {
            if (path is null)
                throw new ArgumentNullException(nameof(path));
            if (path.EndsWith(".jar", StringComparison.OrdinalIgnoreCase) == false)
                throw new ArgumentException("Path must refer to a JAR file.", nameof(path));
            if (File.Exists(path) == false)
                throw new FileNotFoundException("Cannot find JAR file.", path);

            using var jar = new JarFile(path);
            var info = jar.GetModuleInfo() ?? new ModuleInfo();
            if (info.Name is null || info.Version is null)
            {
                var fromFileName = GetModuleNameAndVersionFromFileName(path);
                if (info.Name is null && fromFileName.Name is not null)
                    info.Name = fromFileName.Name;
                if (info.Version is null && fromFileName.Version is not null)
                    info.Version = fromFileName.Version;
            }

            return info;
        }

        /// <summary>
        /// Derives the module name from the name of the file given by the path, as described by the specification.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static ModuleInfo GetModuleNameAndVersionFromFileName(string name)
        {
            var info = new ModuleInfo();

            // The ".jar" suffix is removed.
            name = Path.GetFileNameWithoutExtension(name);
            var span = (Span<char>)stackalloc char[name.Length];
            name.AsSpan().CopyTo(span);

            // If the name matches the regular expression "-(\\d+(\\.|$))" then the module name will be derived from the subsequence preceding the hyphen of the first occurrence.
            if (Regex.Match(name, @"-(\d+(\.|$))") is Match m)
            {
                info.Version = TryParseVersion(span.Slice(m.Index + 1));
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
            info.Name = name.Trim('.');

            return info;
        }

        /// <summary>
        /// Parses a Version-String.
        /// </summary>
        /// <param name="span"></param>
        /// <returns></returns>
        static ModuleVersion TryParseVersion(ReadOnlySpan<char> span)
        {
            return ModuleVersion.TryParse(span, out var v) ? v : null;
        }

    }

}
