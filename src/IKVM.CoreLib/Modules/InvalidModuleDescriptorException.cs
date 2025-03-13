using System;

namespace IKVM.CoreLib.Modules
{

    /// <summary>
    /// Thrown when a module-info.class file cannot be read.
    /// </summary>
    internal class InvalidModuleDescriptorException : Exception
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public InvalidModuleDescriptorException(string message) : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public InvalidModuleDescriptorException(string message, Exception innerException) :
            base(message, innerException)
        {

        }

    }

}
