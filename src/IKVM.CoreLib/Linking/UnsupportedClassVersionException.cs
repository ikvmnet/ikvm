using IKVM.ByteCode;

namespace IKVM.CoreLib.Linking
{

    /// <summary>
    /// Thrown during linking when an unsupported class version is encounted.
    /// </summary>
    internal class UnsupportedClassVersionException : ClassFormatException
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="version"></param>
        public UnsupportedClassVersionException(ClassFormatVersion version) :
            base(version.ToString())
        {

        }

    }

}
