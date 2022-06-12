using System;
using System.Runtime.Serialization;

namespace IKVM.Sdk.Tasks
{
    [Serializable]
    internal class IkvmTaskException : Exception
    {
        private string v;
        private string reference;
        private string itemSpec;

        public IkvmTaskException()
        {
        }

        public IkvmTaskException(string message) : base(message)
        {
        }

        public IkvmTaskException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public IkvmTaskException(string v, string reference, string itemSpec)
        {
            this.v = v;
            this.reference = reference;
            this.itemSpec = itemSpec;
        }

        protected IkvmTaskException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}