using System;

namespace IKVM.CoreLib.Exceptions
{

    /// <summary>
    /// Marks an exception as one that should be retargeted when thrown into Java code.
    /// </summary>
    internal abstract class TranslatableJavaException : Exception
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public TranslatableJavaException()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        public TranslatableJavaException(string message) :
            base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public TranslatableJavaException(string message, Exception innerException) :
            base(message, innerException)
        {

        }

    }

}
