using System;

namespace IKVM.CoreLib.Modules
{

    /// <summary>
    /// Thrown when an error occurs during module resolution.
    /// </summary>
    internal class ResolutionException : Exception
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ResolutionException(string message) : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ResolutionException(string message, Exception innerException) :
            base(message, innerException)
        {

        }

    }

}
