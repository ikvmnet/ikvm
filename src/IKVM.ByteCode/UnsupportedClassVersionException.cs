namespace IKVM.ByteCode
{

    /// <summary>
    /// Describes an attempt to parse an unsupported class file version.
    /// </summary>
    internal sealed class UnsupportedClassVersionException :
        InvalidClassException
    {

        readonly ClassFormatVersion version;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="version"></param>
        internal UnsupportedClassVersionException(ClassFormatVersion version) :
            base($"Unsupported class version {version}.")
        {
            this.version = version;
        }

        /// <summary>
        /// Gets the version that was found in the class file.
        /// </summary>
        public ClassFormatVersion Version => version;

    }

}
