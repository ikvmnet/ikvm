using System;
using System.Collections.Immutable;
using System.IO;

namespace IKVM.CoreLib.Jar
{

    /// <summary>
    /// The Manifest class is used to maintain Manifest entry names and their associated Attributes. There are main
    /// Manifest Attributes as well as per-entry Attributes. For information on the Manifest format, please see the
    /// Manifest format specification.
    /// </summary>
    public class Manifest
    {

        static readonly char[] ManifestNameValueSeparatorChars = [':'];

        readonly ImmutableDictionary<string, string> _mainAttributes;
        readonly ImmutableDictionary<string, ImmutableDictionary<string, string>> _attributes;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="reader"></param>
        public Manifest(TextReader reader)
        {
            var attributes = ImmutableDictionary.CreateBuilder<string, ImmutableDictionary<string, string>>();

            // read the remaining sections
            while (ReadSection(reader) is { } section)
            {
                if (_mainAttributes is null)
                {
                    section = section.Remove("Name");
                    _mainAttributes = section;
                }
                else
                {
                    if (section.TryGetValue("Name", out var name))
                    {
                        section = section.Remove("Name");
                        attributes[name] = section;
                    }
                    else
                    {
                        throw new FormatException("Non-main manifest section missing Name attribute.");
                    }
                }
            }

            if (_mainAttributes is null)
                throw new FormatException("Could not find main manifest section.");

            // finalize non-main sections
            _attributes = attributes.ToImmutable();
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
        /// Initializes a new instance.
        /// </summary>
        /// <param name="value"></param>
        public Manifest(string value) :
            this(new StringReader(value))
        {

        }

        /// <summary>
        /// Reads a section of the manifest data in from the reader.
        /// </summary>
        /// <param name="reader"></param>
        ImmutableDictionary<string, string>? ReadSection(TextReader reader)
        {
            var attributes = ImmutableDictionary.CreateBuilder<string, string>(StringComparer.OrdinalIgnoreCase);

            // read until we hit an empty line or the end of the file
            while (reader.ReadLine() is string s && s.Trim() != "")
                if (s.Split(ManifestNameValueSeparatorChars, 2) is string[] p && p.Length == 2)
                    attributes[p[0].Trim()] = p[1].Trim();

            // only return attributes if there is any
            return attributes.Count > 0 ? attributes.ToImmutable() : null;
        }

        /// <summary>
        /// Returns the main Attributes for the Manifest.
        /// </summary>
        public ImmutableDictionary<string, string> MainAttributes => _mainAttributes;

        /// <summary>
        /// Returns the Attributes for the specified entry name.
        /// </summary>
        public ImmutableDictionary<string, ImmutableDictionary<string, string>> Attributes => _attributes;

    }

}
