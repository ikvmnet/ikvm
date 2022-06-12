using System;
using System.Collections.Generic;
using System.IO;

namespace IKVM.Util.Jar
{

    /// <summary>
    /// The Manifest class is used to maintain Manifest entry names and their associated Attributes. There are main
    /// Manifest Attributes as well as per-entry Attributes. For information on the Manifest format, please see the
    /// Manifest format specification.
    /// </summary>
    public class Manifest
    {

        static readonly char[] ManifestNameValueSeperatorChars = new[] { ':' };

        readonly Attributes mainAttributes = new();
        readonly Dictionary<string, Attributes> attributes = new(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="reader"></param>
        public Manifest(TextReader reader)
        {
            // reads the mains section
            if (ReadSection(reader) is Attributes main)
            {
                this.mainAttributes = main;

                // read the remaining sections
                while (ReadSection(reader) is Attributes section)
                    if (section.TryGetValue("Name", out var name) && section.Remove("Name"))
                        attributes[name] = section;
            }
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="stream"></param>
        public Manifest(Stream stream) :
            this(new StreamReader(stream))
        {

        }

        /// <summary>
        /// Reads a section of the manifest data in from the reader.
        /// </summary>
        /// <param name="reader"></param>
        Attributes ReadSection(TextReader reader)
        {
            var attributes = new Attributes();

            // read until we hit an empty line or the end of the file
            while (reader.ReadLine() is string s && s.Trim() != "")
                if (s.Split(ManifestNameValueSeperatorChars, 2) is string[] p && p.Length == 2)
                    attributes[p[0].Trim()] = p[1].Trim();

            // only return attributes if there is any
            return attributes.Count > 0 ? attributes : null;
        }

        /// <summary>
        /// Returns the main Attributes for the Manifest.
        /// </summary>
        public Attributes MainAttributes => mainAttributes;

        /// <summary>
        /// Returns the Attributes for the specified entry name.
        /// </summary>
        public IDictionary<string, Attributes> Attributes => attributes;

    }

}
