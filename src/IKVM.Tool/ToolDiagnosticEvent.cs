using System;

namespace IKVM.Tool
{

    /// <summary>
    /// Describes an event emitted from a tool.
    /// </summary>
    public class ToolDiagnosticEvent
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="messageArgs"></param>
        public ToolDiagnosticEvent(ToolDiagnosticEventLevel level, string message, params object[] messageArgs)
        {
            Level = level;
            Message = message ?? throw new ArgumentNullException(nameof(message));
            MessageArgs = messageArgs ?? throw new ArgumentNullException(nameof(messageArgs));
        }

        /// <summary>
        /// Gets the level of the event.
        /// </summary>
        public ToolDiagnosticEventLevel Level { get;  }

        /// <summary>
        /// Message format string.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Objects to include with format string.
        /// </summary>
        public object[] MessageArgs { get; }

    }

}
