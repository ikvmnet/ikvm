using System;
using System.Collections.Generic;

namespace IKVM.Util.Jar
{

    /// <summary>
    /// The Attributes class maps Manifest attribute names to associated string values. Valid attribute names are
    /// case-insensitive, are restricted to the ASCII characters in the set [0-9a-zA-Z_-], and cannot exceed 70
    /// characters in length. Attribute values can contain any characters and will be UTF8-encoded when written to
    /// the output stream. See the JAR File Specification for more information about valid attribute names and values.
    /// </summary>
    public class Attributes : Dictionary<string, string>
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public Attributes() :
            base(StringComparer.OrdinalIgnoreCase)
        {

        }

    }

}
