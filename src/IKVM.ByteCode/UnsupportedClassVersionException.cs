namespace IKVM.ByteCode
{

    /// <summary>
    /// Describes an attempt to parse an unsupported class file version.
    /// </summary>
    public sealed class UnsupportedClassVersionException :
        ClassReaderException
    {

        readonly int majorVersion;
        readonly int minorVersion;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="majorVersion"></param>
        /// <param name="minorVersion"></param>
        internal UnsupportedClassVersionException(int majorVersion, int minorVersion) :
            base($"Unsupported class version {majorVersion}.{minorVersion}.")
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
