using System;

using java.io;

using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace IKVM.JTReg.TestAdapter
{

    /// <summary>
    /// <see cref="Writer"/> implementation that invokes a delegate for each action.
    /// </summary>
    class MessageLoggerWriter : Writer
    {

        readonly IMessageLogger logger;
        readonly TestMessageLevel messageLevel;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="messageLevel"></param>
        public MessageLoggerWriter(IMessageLogger logger, TestMessageLevel messageLevel)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.messageLevel = messageLevel;
        }

        public override void write(char[] cbuf, int off, int len)
        {
            if (new string(cbuf, off, len) is string s)
            {
                s = s.Trim();
                if (s.Length > 0)
                    logger.SendMessage(messageLevel, s);
            }
        }

        public override void flush()
        {

        }

        public override void close()
        {

        }

    }

}
