namespace IKVM.ByteCode
{

    /// <summary>
    /// Describes an attempt to parse an unsupported class file.
    /// </summary>
    internal class InvalidClassException :
        ByteCodeException
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        internal InvalidClassException(string message) :
            base(message)
        {

        }

    }

}
