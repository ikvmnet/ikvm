using System;
using System.Buffers;
using System.IO;
using System.Text;
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

            try
            {
                var text = new StringWriter();
                FormatDiagnosticEvent(text, @event);

                switch (@event.Level)
                {
                    case IkvmToolDiagnosticEventLevel.Trace:
                        logger.LogMessage(null, $"{@event.Id:D4}", null, null, @event.StartLine, @event.StartColumn, @event.EndLine, @event.EndColumn, MessageImportance.Low, @event.Message, @event.Args);
                        writer?.WriteLine(text.ToString());
                        break;
                    case IkvmToolDiagnosticEventLevel.Info:
                        logger.LogMessage(null, $"{@event.Id:D4}", null, null, @event.StartLine, @event.StartColumn, @event.EndLine, @event.EndColumn, MessageImportance.Normal, @event.Message, @event.Args);
                        writer?.WriteLine(text.ToString());
                        break;
                    case IkvmToolDiagnosticEventLevel.Warning:
                        logger.LogWarning(null, $"{@event.Id:D4}", null, null, @event.StartLine, @event.StartColumn, @event.EndLine, @event.EndColumn, @event.Message, @event.Args);
                        writer?.WriteLine(text.ToString());
                        break;
                    case IkvmToolDiagnosticEventLevel.Error:
                        logger.LogError(null, $"{@event.Id:D4}", null, null, @event.StartLine, @event.StartColumn, @event.EndLine, @event.EndColumn, MessageImportance.Normal, @event.Message, @event.Args);
                        writer?.WriteLine(text.ToString());
                        break;
                    case IkvmToolDiagnosticEventLevel.Fatal:
                        logger.LogError(null, $"{@event.Id:D4}", null, null, @event.StartLine, @event.StartColumn, @event.EndLine, @event.EndColumn, MessageImportance.High, @event.Message, @event.Args);
                        writer?.WriteLine(text.ToString());
                        break;
                }
            }
            catch
            {
                // ignore failure to log, not much we can do
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Returns the output text for the given <see cref="DiagnosticLevel"/>.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        protected virtual void FormatDiagnosticLevel(StringWriter writer, IkvmToolDiagnosticEventLevel level)
        {
            writer.Write(level switch
            {
                IkvmToolDiagnosticEventLevel.Trace => "trace",
                IkvmToolDiagnosticEventLevel.Info => "info",
                IkvmToolDiagnosticEventLevel.Warning => "warning",
                IkvmToolDiagnosticEventLevel.Error => "error",
                IkvmToolDiagnosticEventLevel.Fatal => "fatal",
                _ => throw new InvalidOperationException(),
            });
        }

        /// <summary>
        /// Formats the output text for the given diagnostic code.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected virtual void FormatDiagnosticCode(StringWriter writer, int code)
        {
            writer.Write("IKVM");
#if NETFRAMEWORK
            writer.Write($"{code:D4}");
#else
            var buf = (Span<char>)stackalloc char[16];
            if (code.TryFormat(buf, out var l, "D4") == false)
                throw new InvalidOperationException();

            writer.Write(buf);
#endif
        }

        /// <summary>
        /// Formats the specified message text.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        protected virtual void FormatDiagnosticMessage(StringWriter writer, string message, object?[] args)
        {
            writer.Write(string.Format(null, message, args));
        }

        /// <summary>
        /// Formats the <see cref="IkvmToolDiagnosticEvent"/>.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="event"></param>
        protected virtual void FormatDiagnosticEvent(StringWriter writer , IkvmToolDiagnosticEvent @event)
        {
            FormatDiagnosticLevel(writer, @event.Level);
            writer.Write(" ");
            FormatDiagnosticCode(writer, @event.Id);
            writer.Write(": ");
            FormatDiagnosticMessage(writer, @event.Message, @event.Args);
            writer.WriteLine();
        }

    }

}
