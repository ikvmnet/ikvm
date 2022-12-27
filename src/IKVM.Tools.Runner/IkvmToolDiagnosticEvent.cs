﻿using System;

namespace IKVM.Tools.Runner
{

    /// <summary>
    /// Describes an event emitted from a tool.
    /// </summary>
    public class IkvmToolDiagnosticEvent
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="messageArgs"></param>
        public IkvmToolDiagnosticEvent(IkvmToolDiagnosticEventLevel level, string message, params object[] messageArgs)
        {
            Level = level;
            Message = message ?? throw new ArgumentNullException(nameof(message));
            MessageArgs = messageArgs ?? throw new ArgumentNullException(nameof(messageArgs));
        }

        /// <summary>
        /// Gets the level of the event.
        /// </summary>
        public IkvmToolDiagnosticEventLevel Level { get;  }

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
