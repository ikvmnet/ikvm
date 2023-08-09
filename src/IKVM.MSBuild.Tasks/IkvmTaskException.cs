using System;
using System.Runtime.Serialization;

namespace IKVM.MSBuild.Tasks
{

    /// <summary>
    /// Generic IKVM task exception.
    /// </summary>
    [Serializable]
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

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected IkvmTaskException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {

        }

    }

}
