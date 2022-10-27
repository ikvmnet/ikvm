using System;

using java.io;

namespace IKVM.JTReg.TestAdapter.Core
{

    /// <summary>
    /// <see cref="Writer"/> implementation that invokes a delegate for each action.
    /// </summary>
    class MessageLoggerWriter : Writer
    {

        readonly IJTRegLoggerContext logger;
        readonly JTRegTestMessageLevel messageLevel;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="messageLevel"></param>
        public MessageLoggerWriter(IJTRegLoggerContext logger, JTRegTestMessageLevel messageLevel)
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
