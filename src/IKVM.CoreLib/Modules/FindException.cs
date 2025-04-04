using System;

namespace IKVM.CoreLib.Modules
{

    /// <summary>
    /// Thrown by a <see cref="IModuleFinder"/> when an error occurs finding a module. Also thrown by <see
    /// cref="ModuleConfiguration.Resolve"/> when resolution fails for observability-related reasons.
    /// </summary>
    internal class FindException : Exception
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        public FindException(string message) :
            base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="innerException"></param>
        public FindException(Exception innerException) :
            base(innerException.Message, innerException)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public FindException(string message, Exception innerException) :
            base(message, innerException)
        {

        }

    }

}
