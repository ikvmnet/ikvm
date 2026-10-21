using System;

using IKVM.CoreLib.Exceptions;

namespace IKVM.CoreLib.Linking
{

    /// <summary>
    /// Basic error during class linking.
    /// </summary>
    internal abstract class LinkingException : TranslatableJavaException
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public LinkingException()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        public LinkingException(string message) :
            base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public LinkingException(string message, Exception innerException) :
            base(message, innerException)
        {

        }

    }

}
