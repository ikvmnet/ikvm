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

            var structuredLog = ParseEvent(@event);

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

        internal static StructuredLog ParseEvent(IkvmToolDiagnosticEvent @event)
        {
            StructuredLog result;

#pragma warning disable IDE0079 // Unused suppression (net472 doesn't produce IDE0057)
#pragma warning disable IDE0057 // Suppress net6 & net8 analyzer for range-operations
            // unspecified format: "Level: Message"
            // MSBuild Format: "Level IKVM0000: Message"
            // Longest preamble: "warning IKVM0000: "
            // Some write """
            // warning IKVMC0000: Message
            //     (additional information)
            // """
            // Skip these (additional information)
            // - StdErr is mapped to Warning
            // - StdOut is mapped to Debug.
            if (@event.Message.Length is { } messageLength and > 0
                && @event.Message[0] is char first
                && !char.IsWhiteSpace(first)
                && @event.Message.IndexOf(": ", 0, Math.Min(messageLength, 18)) is { } firstColon and not -1)
            {
                result = new()
                {
                    Message = @event.Message.Substring(firstColon + 2)
                };

                if (@event.Message.IndexOf("IKVM", 0, firstColon, StringComparison.OrdinalIgnoreCase) is { } codeIndex and not -1
                    && (@event.Message.Length - codeIndex) >= 8)
                {
                    result.Code = @event.Message.Substring(codeIndex, 8 /* IKVM0000 */);
                }
                else
                {
                    codeIndex = firstColon;
                    result.Code = "";
                }

                var eventLevelMixedCase = @event.Message.AsSpan(0, codeIndex).Trim();
                // eventLevelMixedCase is never longer than 18 chars.
                Span<char> eventLevel = stackalloc char[eventLevelMixedCase.Length];
                eventLevelMixedCase.ToUpperInvariant(eventLevel);
                result.Level = eventLevel switch
                {
                    "ERROR" => IkvmToolDiagnosticEventLevel.Error,
                    "WARNING" => IkvmToolDiagnosticEventLevel.Warning,
                    "INFO" => IkvmToolDiagnosticEventLevel.Information,
                    "TRACE" => IkvmToolDiagnosticEventLevel.Debug,
                    _ => @event.Level // Can't figure out event Level.
                                      //  Use what's been provided
                };
            }
            else
            {
                // Can't figure out level.
                result = new(@event.Level, null, @event.Message);
            }
#pragma warning restore

            return result;
        }

        internal record struct StructuredLog(
            IkvmToolDiagnosticEventLevel Level,
            string Code,
            string Message
        );

    }

}
