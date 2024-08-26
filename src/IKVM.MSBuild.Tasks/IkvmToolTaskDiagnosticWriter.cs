using System;
using System.IO;
using System.Threading.Tasks;

using IKVM.Tools.Runner;

using Microsoft.Build.Framework;

namespace IKVM.MSBuild.Tasks
{

    /// <summary>
    /// Logs diagnostic events to MSBuild.
    /// </summary>
    public class IkvmToolTaskDiagnosticWriter : IIkvmToolDiagnosticEventListener
    {

        readonly Microsoft.Build.Utilities.TaskLoggingHelper logger;
        readonly TextWriter writer;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="writer"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmToolTaskDiagnosticWriter(Microsoft.Build.Utilities.TaskLoggingHelper logger, TextWriter writer)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.writer = writer;
        }

        /// <summary>
        /// Receives a diagnostic event and logs it to MSBuild.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public Task ReceiveAsync(IkvmToolDiagnosticEvent @event)
        {
            if (@event == null)
                return Task.CompletedTask;

            (
                IkvmToolDiagnosticEventLevel Level,
                string Code,
                string Message
            ) structuredLog;

#pragma warning disable IDE0079 // Unused suppression (net472 doesn't produce IDE0057)
#pragma warning disable IDE0057 // Suppress net6 & net8 analyzer for range-operations
            // unspecified format: "Level: Message"
            // MSBuild Format: "Level IKVM0000: Message"
            // Longest preamble: "warning IKVM0000: "
            // Some write """
            // warning IKVMC0000: Message
            //     (additional information)
            // """
            // Skip these:
            // - StdErr is mapped to Information
            // - StdOut is mapped to Debug.
            if (@event.Message.Length is { } messageLength and > 0
                && @event.Message[0] is char first
                && !char.IsWhiteSpace(first)
                && @event.Message.IndexOf(": ", 0, Math.Min(messageLength, 18)) is { } firstColon and not -1)
            {
                structuredLog.Message = @event.Message.Substring(firstColon + 2);

                int levelLength;
                if (@event.Message.IndexOf("IKVM", 0, firstColon, StringComparison.OrdinalIgnoreCase) is { } codeIndex and not -1)
                {
                    levelLength = codeIndex - 1;
                    structuredLog.Code = @event.Message.Substring(codeIndex, 8 /* IKVM0000 */);
                }
                else
                {
                    levelLength = firstColon;
                    structuredLog.Code = "";
                }

                structuredLog.Level = @event.Message.Substring(0, levelLength).ToUpperInvariant() switch
                {
                    "ERROR" => IkvmToolDiagnosticEventLevel.Error,
                    "WARNING" => IkvmToolDiagnosticEventLevel.Warning,
                    _ => IkvmToolDiagnosticEventLevel.Information
                };
            }
            else
            {
                // Can't figure out level.
                structuredLog = (@event.Level, null, @event.Message);
            }
#pragma warning restore

            try
            {
                switch (structuredLog.Level)
                {
                    case IkvmToolDiagnosticEventLevel.Debug:
                        logger.LogMessage(null, structuredLog.Code, null, null, 0, 0, 0, 0, MessageImportance.Low, structuredLog.Message, @event.MessageArgs);
                        writer?.WriteLine("DEBUG: " + @event.Message, @event.MessageArgs);
                        break;
                    case IkvmToolDiagnosticEventLevel.Information:
                        logger.LogMessage(null, structuredLog.Code, null, null, 0, 0, 0, 0, MessageImportance.Normal, structuredLog.Message, @event.MessageArgs);
                        writer?.WriteLine("INFO: " + @event.Message, @event.MessageArgs);
                        break;
                    case IkvmToolDiagnosticEventLevel.Warning:
                        logger.LogWarning(null, structuredLog.Code, null, null, 0, 0, 0, 0, structuredLog.Message, @event.MessageArgs);
                        writer?.WriteLine("WARN: " + @event.Message, @event.MessageArgs);
                        break;
                    case IkvmToolDiagnosticEventLevel.Error:
                        logger.LogError(null, structuredLog.Code, null, null, 0, 0, 0, 0, structuredLog.Message, @event.MessageArgs);
                        writer?.WriteLine("ERROR: " + @event.Message, @event.MessageArgs);
                        break;
                }
            }
            catch
            {
                // ignore failure to log, not much we can do
            }

            return Task.CompletedTask;
        }

    }

}
