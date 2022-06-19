using System;

namespace IKVM.Tool
{

    public class IkvmToolException : Exception
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public IkvmToolException()
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        public IkvmToolException(string message) : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public IkvmToolException(string message, Exception innerException) : base(message, innerException)
        {

        }

    }

}
