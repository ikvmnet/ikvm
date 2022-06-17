using System;
using System.Threading.Tasks;

using IKVM.Tool;


namespace IKVM.MSBuild.Tasks
{

    /// <summary>
    /// Logs diagnostic events to MSBuild.
    /// </summary>
    class IkvmToolTaskDiagnosticWriter : IToolDiagnosticEventListener
    {

        readonly Microsoft.Build.Utilities.TaskLoggingHelper log;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="log"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmToolTaskDiagnosticWriter(Microsoft.Build.Utilities.TaskLoggingHelper log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        /// <summary>
        /// Receives a diagnostic event and logs it to MSBuild.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public Task ReceiveAsync(ToolDiagnosticEvent @event)
        {
            switch (@event.Level)
            {
                case ToolDiagnosticEventLevel.Debug:
                    log.LogMessage(Microsoft.Build.Framework.MessageImportance.Low, @event.Message, @event.MessageArgs);
                    break;
                case ToolDiagnosticEventLevel.Information:
                    log.LogMessage(Microsoft.Build.Framework.MessageImportance.Normal, @event.Message, @event.MessageArgs);
                    break;
                case ToolDiagnosticEventLevel.Warning:
                    log.LogWarning(@event.Message, @event.MessageArgs);
                    break;
                case ToolDiagnosticEventLevel.Error:
                    log.LogError(@event.Message, @event.MessageArgs);
                    break;
            }

            return Task.CompletedTask;
        }

    }

}
