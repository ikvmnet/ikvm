using System;

namespace IKVM.Sdk.Tasks
{

    /// <summary>
    /// Generic IKVM task exception.
    /// </summary>
    class IkvmTaskException : Exception
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        public IkvmTaskException(string message) : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public IkvmTaskException(string message, Exception innerException) :
            base(message, innerException)
        {

        }

    }

}