using System.Buffers;

using IKVM.ByteCode.Buffers;

namespace IKVM.ByteCode
{

    /// <summary>
    /// Describes an attempt to parse an unsupported class file version.
    /// </summary>
    public class UnsupportedClassVersionException :
        ClassParserException
    {

        readonly int majorVersion;
        readonly int minorVersion;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="majorVersion"></param>
        /// <param name="minorVersion"></param>
        internal UnsupportedClassVersionException(in SequenceReader<byte> reader, int majorVersion, int minorVersion) :
            base(reader, $"Unsupported class version {majorVersion}.{minorVersion}.")
        {
            this.majorVersion = majorVersion;
            this.minorVersion = minorVersion;
        }

        /// <summary>
        /// Gets the major version that was found in the class file.
        /// </summary>
        public int MajorVersion => majorVersion;

        /// <summary>
        /// Gets the minor version that was found in the class file.
        /// </summary>
        public int MinorVersion => minorVersion;

    }

}
