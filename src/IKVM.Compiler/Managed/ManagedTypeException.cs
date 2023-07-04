using System;

namespace IKVM.Compiler.Managed
{

    /// <summary>
    /// Describes an error regarding a managed type.
    /// </summary>
    public class ManagedTypeException : Exception
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ManagedTypeException()
        {

        }


        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        public ManagedTypeException(string? message) :
            base(message)
        {

        }

    }

}
