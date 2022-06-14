using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;

namespace IKVM.Util.Jar
{

    /// <summary>
    /// Provides some utility methods for working with JAR files.
    /// </summary>
    public class JarFile : IDisposable
    {

        readonly ZipArchive archive;
        readonly bool dispose;
        readonly Lazy<Manifest> manifest;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public JarFile(ZipArchive archive, bool dispose = false)
        {
            this.archive = archive ?? throw new ArgumentNullException(nameof(archive));
            this.dispose = dispose;

            manifest = new Lazy<Manifest>(GetManifest);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="stream"></param>
        public JarFile(Stream stream, bool dispose = true) :
            this(new ZipArchive(stream, ZipArchiveMode.Read, dispose == false), dispose)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="path"></param>
        public JarFile(string path) :
            this(File.OpenRead(path), true)
        {

        }

        /// <summary>
        /// Returns the jar file manifest, or null if none.
        /// </summary>
        public Manifest Manifest => manifest.Value;

        /// <summary>
        /// Gets the jar file manifest, or null if none.
        /// </summary>
        /// <returns></returns>
        Manifest GetManifest()
        {
            // find the manifest in the archive
            var e = archive.GetEntry("META-INF/MANIFEST.MF");
            if (e == null)
                return null;

            // open and read the manifest
            using var s = e.Open();
            return new Manifest(s);
        }

        /// <summary>
        /// Disposes of the instance.
        /// </summary>
        public void Dispose()
        {
            if (dispose && archive != null)
                archive.Dispose();
        }

    }

}