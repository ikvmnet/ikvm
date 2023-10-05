using System;
using System.IO;
using System.Threading.Tasks;

using IKVM.Tools.Runner;

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
                    case IkvmToolDiagnosticEventLevel.Debug:
                        logger.LogMessage(Microsoft.Build.Framework.MessageImportance.Low, @event.Message, @event.MessageArgs);
                        writer?.WriteLine("DEBUG: " + @event.Message, @event.MessageArgs);
                        break;
                    case IkvmToolDiagnosticEventLevel.Information:
                        logger.LogMessage(Microsoft.Build.Framework.MessageImportance.Normal, @event.Message, @event.MessageArgs);
                        writer?.WriteLine("INFO: " + @event.Message, @event.MessageArgs);
                        break;
                    case IkvmToolDiagnosticEventLevel.Warning:
                        logger.LogWarning(@event.Message, @event.MessageArgs);
                        writer?.WriteLine("WARN: " + @event.Message, @event.MessageArgs);
                        break;
                    case IkvmToolDiagnosticEventLevel.Error:
                        logger.LogWarning(@event.Message, @event.MessageArgs);
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
