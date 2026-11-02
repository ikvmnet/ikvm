using System;

namespace IKVM.CoreLib.Linking
{

    /// <summary>
    /// Thrown when a class fails verification. Corresponds to 'java.lang.VerifyError'.
    /// </summary>
    internal class VerifyException : LinkingException
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public VerifyException()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        public VerifyException(string message) :
            base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public VerifyException(string message, Exception innerException) :
            base(message, innerException)
        {

        }

    }

}
