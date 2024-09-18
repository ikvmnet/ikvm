using System;

namespace IKVM.Tools.Core.CommandLine
{

    class ToolErrorException : Exception
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        public ToolErrorException(string message) : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ToolErrorException(string message, Exception innerException) :
            base(message, innerException)
        {

        }

    }

}
