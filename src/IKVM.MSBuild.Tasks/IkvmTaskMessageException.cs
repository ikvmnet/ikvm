using System;
using System.Runtime.Serialization;

using IKVM.MSBuild.Tasks.Resources;

namespace IKVM.MSBuild.Tasks
{

    /// <summary>
    /// IKVM task exception with error message.
    /// </summary>
    [Serializable]
    class IkvmTaskMessageException : IkvmTaskException
    {

        readonly string messageResourceName;
        readonly object[] messageArgs;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="messageResourceName"></param>
        /// <param name="messageArgs"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmTaskMessageException(string messageResourceName, params object[] messageArgs) :
            base(string.Format(SR.ResourceManager.GetString(messageResourceName), messageArgs))
        {
            this.messageResourceName = messageResourceName ?? throw new ArgumentNullException(nameof(messageResourceName));
            this.messageArgs = messageArgs ?? throw new ArgumentNullException(nameof(messageArgs));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="messageResourceName"></param>
        /// <param name="messageArgs"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmTaskMessageException(Exception innerException, string messageResourceName, params object[] messageArgs) :
            base(string.Format(SR.ResourceManager.GetString(messageResourceName), messageArgs), innerException)
        {
            this.messageResourceName = messageResourceName ?? throw new ArgumentNullException(nameof(messageResourceName));
            this.messageArgs = messageArgs ?? throw new ArgumentNullException(nameof(messageArgs));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected IkvmTaskMessageException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
            this.messageResourceName = info.GetString("MessageResourceName");
            this.messageArgs = (object[])info.GetValue("MessageArgs", typeof(object[]));
        }

        /// <summary>
        /// Gets the resource name of the message.
        /// </summary>
        public string MessageResourceName => messageResourceName;

        /// <summary>
        /// Gets the arguments of the message.
        /// </summary>
        public object[] MessageArgs => messageArgs;

        /// <inheritdoc />
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));

            info.AddValue("MessageResourceName", MessageResourceName);
            info.AddValue("MessageArgs", MessageArgs, typeof(object[]));
            base.GetObjectData(info, context);
        }

    }

}
