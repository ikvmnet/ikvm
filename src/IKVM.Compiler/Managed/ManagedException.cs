using System;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes an error regarding a managed type.
    /// </summary>
    internal class ManagedException : Exception
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ManagedException()
        {

        }


        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        public ManagedException(string? message) :
            base(message)
        {

        }

    }

}
