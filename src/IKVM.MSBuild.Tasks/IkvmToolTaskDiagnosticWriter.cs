using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using IKVM.CoreLib.Diagnostics;
using IKVM.Tools.Runner.Diagnostics;

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
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async ValueTask ReceiveAsync(IkvmToolDiagnosticEvent @event, CancellationToken cancellationToken)
        {
            try
            {
                switch (@event.Level)
                {
                    case IkvmToolDiagnosticEventLevel.Trace:
                        logger.LogMessage(null, $"{@event.Id:D4}", null, null, @event.Location.StartLine, @event.Location.StartColumn, @event.Location.EndLine, @event.Location.EndColumn, MessageImportance.Low, @event.Message, @event.Args);
                        break;
                    case IkvmToolDiagnosticEventLevel.Info:
                        logger.LogMessage(null, $"{@event.Id:D4}", null, null, @event.Location.StartLine, @event.Location.StartColumn, @event.Location.EndLine, @event.Location.EndColumn, MessageImportance.Normal, @event.Message, @event.Args);
                        break;
                    case IkvmToolDiagnosticEventLevel.Warning:
                        logger.LogWarning(null, $"{@event.Id:D4}", null, null, @event.Location.StartLine, @event.Location.StartColumn, @event.Location.EndLine, @event.Location.EndColumn, @event.Message, @event.Args);
                        break;
                    case IkvmToolDiagnosticEventLevel.Error:
                        logger.LogError(null, $"{@event.Id:D4}", null, null, @event.Location.StartLine, @event.Location.StartColumn, @event.Location.EndLine, @event.Location.EndColumn, @event.Message, @event.Args);
                        break;
                    case IkvmToolDiagnosticEventLevel.Fatal:
                        logger.LogError(null, $"{@event.Id:D4}", null, null, @event.Location.StartLine, @event.Location.StartColumn, @event.Location.EndLine, @event.Location.EndColumn, @event.Message, @event.Args);
                        break;
                }

                if (writer != null)
                    await WriteLogAsync(writer, @event, cancellationToken);
            }
            catch (Exception e)
            {
                logger.LogErrorFromException(e);
            }
        }

        /// <summary>
        /// Writes the log message for the event.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="event"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task WriteLogAsync(TextWriter writer, IkvmToolDiagnosticEvent @event, CancellationToken cancellationToken)
        {
            await TextDiagnosticFormat.WriteAsync(@event.Id, Convert(@event.Level), @event.Message, @event.Args, null, Convert(@event.Location), writer, cancellationToken);
            await writer.WriteLineAsync();
        }

        /// <summary>
        /// Converts to a <see cref="DiagnosticLevel"/>.
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        DiagnosticLevel Convert(IkvmToolDiagnosticEventLevel level)
        {
            return level switch
            {
                IkvmToolDiagnosticEventLevel.Trace => DiagnosticLevel.Trace,
                IkvmToolDiagnosticEventLevel.Info => DiagnosticLevel.Info,
                IkvmToolDiagnosticEventLevel.Warning => DiagnosticLevel.Warning,
                IkvmToolDiagnosticEventLevel.Error => DiagnosticLevel.Error,
                IkvmToolDiagnosticEventLevel.Fatal => DiagnosticLevel.Fatal,
                _ => throw new NotImplementedException(),
            };
        }

        /// <summary>
        /// Converts to a <see cref="DiagnosticLocation"/>.
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        DiagnosticLocation Convert(IkvmToolDiagnosticEventLocation location)
        {
            return new DiagnosticLocation(location.Path, location.StartLine, location.StartColumn, location.EndLine, location.EndColumn);
        }

    }

}
