using System;
using System.Threading;
using System.Threading.Tasks;

namespace IKVM.Tools.Runner.Diagnostics
{

    /// <summary>
    /// Diagnostic listener that invokes a delegate for each event.
    /// </summary>
    public class IkvmToolNullDiagnosticListener : IIkvmToolDiagnosticEventListener
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmToolNullDiagnosticListener()
        {

        }

        /// <inheritdoc />
        public ValueTask ReceiveAsync(IkvmToolDiagnosticEvent @event, CancellationToken cancellationToken)
        {
            return new ValueTask(Task.CompletedTask);
        }

    }

}
