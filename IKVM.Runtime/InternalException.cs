using System;

namespace IKVM.Runtime
{

    /// <summary>
    /// Represents an internal error that occurred within IKVM.
    /// </summary>
    public class InternalException : Exception
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public InternalException() :
            base()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        public InternalException(string message) :
            base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public InternalException(string message, Exception innerException) :
            base(message, innerException)
        {

        }

    }

}
