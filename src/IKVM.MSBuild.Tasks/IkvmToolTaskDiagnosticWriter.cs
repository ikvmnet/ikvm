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

            try
            {
                switch (@event.Level)
                {
                    case IkvmToolDiagnosticEventLevel.Trace:
                        logger.LogMessage(null, @event.Id, null, null, 0, 0, 0, 0, MessageImportance.Low, @event.Message, @event.Args);
                        writer?.WriteLine("DEBUG: " + @event.Message, @event.Args);
                        break;
                    case IkvmToolDiagnosticEventLevel.Information:
                        logger.LogMessage(null, @event.Id, null, null, 0, 0, 0, 0, MessageImportance.Normal, @event.Message, @event.Args);
                        writer?.WriteLine("INFO: " + @event.Message, @event.Args);
                        break;
                    case IkvmToolDiagnosticEventLevel.Warning:
                        logger.LogWarning(null, @event.Id, null, null, 0, 0, 0, 0, @event.Message, @event.Args);
                        writer?.WriteLine("WARN: " + @event.Message, @event.Args);
                        break;
                    case IkvmToolDiagnosticEventLevel.Error:
                        logger.LogError(null, @event.Id, null, null, 0, 0, 0, 0, MessageImportance.Normal, @event.Message, @event.Args);
                        writer?.WriteLine("ERROR: " + @event.Message, @event.Args);
                        break;
                    case IkvmToolDiagnosticEventLevel.Fatal:
                        logger.LogError(null, @event.Id, null, null, 0, 0, 0, 0, MessageImportance.High, @event.Message, @event.Args);
                        writer?.WriteLine("FATAL: " + @event.Message, @event.Args);
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
