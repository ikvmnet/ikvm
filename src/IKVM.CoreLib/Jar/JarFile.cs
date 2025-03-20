using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;

using IKVM.CoreLib.Modules;

namespace IKVM.CoreLib.Jar
{

    /// <summary>
    /// Provides some utility methods for working with JAR files.
    /// </summary>
    internal class JarFile : IDisposable
    {

        const string META_INF = "META-INF/";
        const string META_INF_VERSIONS = META_INF + "versions/";
        const string MANIFEST_NAME = META_INF + "MANIFEST.MF";
        const string INDEX_NAME = "META-INF/INDEX.LIST";

        internal static readonly RuntimeVersion BASE_VERSION = RuntimeVersion.Parse("8");
        internal static readonly int BASE_VERSION_FEATURE = BASE_VERSION.Feature;
        internal static readonly RuntimeVersion RUNTIME_VERSION = RuntimeVersion.Parse("8");
        internal static readonly bool MULTI_RELEASE_ENABLED = false;
        internal static readonly bool MULTI_RELEASE_FORCED = false;

        readonly ZipArchive _archive;
        readonly RuntimeVersion _version;
        readonly int _versionFeature;
        readonly bool _dispose;

        Manifest? _manifest;
        bool _hasCheckedSpecialAttributes;
        bool _hasClassPathAttribute;
        bool _isMultiRelease;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="archive"></param>
        /// <param name="version"></param>
        /// <param name="dispose"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public JarFile(ZipArchive archive, RuntimeVersion version, bool dispose = false)
        {
            _archive = archive ?? throw new ArgumentNullException(nameof(archive));
            _dispose = dispose;

            if (MULTI_RELEASE_FORCED || version.Feature == RUNTIME_VERSION.Feature)
            {
                _version = RUNTIME_VERSION;
            }
            else if (version.Feature <= BASE_VERSION_FEATURE)
            {
                _version = BASE_VERSION;
            }
            else
            {
                _version = RuntimeVersion.Parse(version.Feature.ToString());
            }

            _versionFeature = _version.Feature;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="releaseVersion"></param>
        /// <param name="dispose"></param>
        public JarFile(Stream stream, RuntimeVersion releaseVersion, bool dispose = true) :
            this(new ZipArchive(stream, ZipArchiveMode.Read, dispose), releaseVersion, dispose)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="releaseVersion"></param>
        public JarFile(string path, RuntimeVersion releaseVersion) :
            this(File.OpenRead(path), releaseVersion, true)
        {

        }

        /// <summary>
        /// Returns the maximum version used when searching for versioned entries.
        /// </summary>
        public RuntimeVersion Version => IsMultiRelease ? _version : BASE_VERSION;

        /// <summary>
        /// Returns the jar file manifest, or null if none.
        /// </summary>
        public Manifest? Manifest => GetOrReadManifest();

        /// <summary>
        /// Gets the JAR file manifest, or null of null.
        /// </summary>
        /// <returns></returns>
        Manifest? GetOrReadManifest()
        {
            if (_manifest is null)
                Interlocked.CompareExchange(ref _manifest, GetManifest(), null);

            return _manifest;
        }

        /// <summary>
        /// Gets the jar file manifest, or null if none.
        /// </summary>
        /// <returns></returns>
        Manifest? GetManifest()
        {
            // find the manifest in the archive
            var e = _archive.GetEntry(MANIFEST_NAME);
            if (e == null)
                return null;

            // open and read the manifest
            using var s = e.Open();
            return new Manifest(s);
        }

        /// <summary>
        /// On first invocation, check if the JAR file has the Class-Path and Multi-Release attribute.
        /// </summary>
        void CheckForSpecialAttributes()
        {
            if (_hasCheckedSpecialAttributes)
                return;

            if (Manifest is not null)
            {
                if (Manifest.MainAttributes.TryGetValue("Class-Path", out _))
                    _hasClassPathAttribute = true;

                if (Manifest.MainAttributes.TryGetValue("Multi-Release", out var s))
                    _isMultiRelease = bool.Parse(s);
            }

            _hasCheckedSpecialAttributes = true;
        }

        /// <summary>
        /// Returns <c>true</c> iff this JAR file has a manifest with the Class-Path attribute.
        /// </summary>
        public bool HasClassPathAttribute
        {
            get
            {
                if (_hasClassPathAttribute)
                    return true;

                CheckForSpecialAttributes();
                return _hasClassPathAttribute;
            }
        }

        /// <summary>
        /// Indicates whether or not this jar file is a multi-release jar file.
        /// </summary>
        public bool IsMultiRelease
        {
            get
            {
                if (_isMultiRelease)
                    return true;

                CheckForSpecialAttributes();
                return _isMultiRelease;
            }
        }

        /// <summary>
        /// Gets the entries in the JAR file.
        /// </summary>
        public IEnumerable<JarFileEntry> GetEntries()
        {
            var hs = new HashSet<string>();

            foreach (var entry in _archive.Entries)
            {
                var bn = GetBaseName(entry.FullName);
                if (bn is null)
                    continue;

                if (hs.Add(bn))
                {
                    var je = GetEntry(bn);
                    if (je is not null)
                        yield return je.Value;
                }
            }
        }

        /// <summary>
        /// For a given full versioned entry name returns the base entry name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string? GetBaseName(string name)
        {
            if (name.StartsWith(META_INF_VERSIONS))
            {
                int off = META_INF_VERSIONS.Length;
                int idx = name.IndexOf('/', off);

                // filter out dir META-INF/versions/ and META-INF/versions/*/
                // and any entry with version > 'version'
                if (idx == -1 || idx == (name.Length - 1) || (int.TryParse(name[off..idx], out var i) && i > _versionFeature))
                    return null;

                // map to its base name
                return name.Substring(idx + 1);
            }

            return name;
        }

        /// <summary>
        /// Returns the entry with the given name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public JarFileEntry? GetEntry(string name)
        {
            if (IsMultiRelease)
            {
                var ve = GetVersionedEntry(name, null);
                if (ve is not null)
                    return ve;
            }

            var je = _archive.GetEntry(name);
            if (je is not null)
                return new JarFileEntry(this, je, name, _versionFeature);

            return null;
        }

        /// <summary>
        /// Finds a versioned entry for the 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultEntry"></param>
        /// <returns></returns>
        JarFileEntry? GetVersionedEntry(string name, JarFileEntry? defaultEntry)
        {
            if (BASE_VERSION_FEATURE < _versionFeature && name.StartsWith(META_INF) == false)
            {
                // start with version of JAR file itself
                var version = _versionFeature;

                // scan each version from JAR file version down to base version
                while (version >= BASE_VERSION_FEATURE)
                {
                    var entry = _archive.GetEntry(META_INF_VERSIONS + version + "/" + name);
                    if (entry != null)
                        return new JarFileEntry(this, entry, name, version);

                    version--;
                }
            }

            return defaultEntry;
        }

        /// <summary>
        /// Disposes of the instance.
        /// </summary>
        public void Dispose()
        {
            if (_dispose && _archive != null)
                _archive.Dispose();
        }

    }

}