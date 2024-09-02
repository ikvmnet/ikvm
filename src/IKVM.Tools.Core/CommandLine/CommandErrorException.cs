using System;

namespace IKVM.Tools.Core.CommandLine
{

    class CommandErrorException : Exception
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        public CommandErrorException(string message) : base(message)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public CommandErrorException(string message, Exception innerException) :
            base(message, innerException)
        {

        }

    }

}
