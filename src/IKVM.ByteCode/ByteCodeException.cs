using System;

namespace IKVM.ByteCode
{

    /// <summary>
    /// Represents an error that occurred with manipulation of Java byte code or class formats.
    /// </summary>
    public class ByteCodeException : Exception
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal ByteCodeException()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        internal ByteCodeException(string message) :
            base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="innerException"></param>
        internal ByteCodeException(Exception innerException) :
            base(innerException.Message, innerException)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        internal ByteCodeException(string message, Exception innerException) :
            base(message, innerException)
        {

        }

    }

}
