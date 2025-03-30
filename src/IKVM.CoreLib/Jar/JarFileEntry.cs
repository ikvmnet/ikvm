using System;
using System.Collections.Immutable;
using System.IO;
using System.IO.Compression;

namespace IKVM.CoreLib.Jar
{

    /// <summary>
    /// Describes an entry in a <see cref="JarFile"/>.
    /// </summary>
    public readonly struct JarFileEntry
    {

        readonly JarFile _jar;
        readonly ZipArchiveEntry _entry;
        readonly string _name;
        readonly int _versionFeature;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="jar"></param>
        /// <param name="entry"></param>
        /// <param name="name"></param>
        /// <param name="versionFeature"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public JarFileEntry(JarFile jar, ZipArchiveEntry entry, string name, int versionFeature)
        {
            _jar = jar ?? throw new ArgumentNullException(nameof(jar));
            _entry = entry ?? throw new ArgumentNullException(nameof(entry));
            _name = name ?? throw new ArgumentNullException(nameof(name));
            _versionFeature = versionFeature;
        }

        /// <summary>
        /// Gets the name of the JAR file entry. This may not be the actual name of the archive entry.
        /// </summary>
        public readonly string Name => _name;

        /// <summary>
        /// Gets the version feature of this entry.
        /// </summary>
        public readonly int VersionFeature => _versionFeature;

        /// <summary>
        /// Gets the attributes for this entry.
        /// </summary>
        public readonly ImmutableDictionary<string, string> Attributes => _jar.Manifest is { } m && m.Attributes.TryGetValue(_entry.FullName, out var attr) ? attr : ImmutableDictionary<string, string>.Empty;

        /// <summary>
        /// Opens the entry for read.
        /// </summary>
        /// <returns></returns>
        public readonly Stream Open() => _entry.Open();

    }

}
