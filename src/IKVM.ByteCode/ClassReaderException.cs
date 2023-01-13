using System;

namespace IKVM.ByteCode
{

    /// <summary>
    /// Represents an error that occurred with the format of the class file.
    /// </summary>
    public class ClassReaderException : Exception
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal ClassReaderException()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        internal ClassReaderException(string message) :
            base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="innerException"></param>
        internal ClassReaderException(Exception innerException) :
            base(innerException.Message, innerException)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        internal ClassReaderException(string message, Exception innerException) :
            base(message, innerException)
        {

        }

    }

}
