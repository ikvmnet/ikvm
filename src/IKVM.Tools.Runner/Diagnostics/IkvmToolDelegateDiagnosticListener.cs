using System;
using System.Threading;
using System.Threading.Tasks;

namespace IKVM.Tools.Runner.Diagnostics
{

    /// <summary>
    /// Diagnostic listener that invokes a delegate for each event.
    /// </summary>
    public class IkvmToolDelegateDiagnosticListener : IIkvmToolDiagnosticEventListener
    {

        readonly Func<IkvmToolDiagnosticEvent, CancellationToken, Task> func;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="func"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmToolDelegateDiagnosticListener(Func<IkvmToolDiagnosticEvent, CancellationToken, Task> func)
        {
            this.func = func ?? throw new ArgumentNullException(nameof(func));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="action"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmToolDelegateDiagnosticListener(Action<IkvmToolDiagnosticEvent> action) :
            this((evt, cancellationToken) => { action(evt); return Task.CompletedTask; })
        {

        }

        /// <inheritdoc />
        public ValueTask ReceiveAsync(in IkvmToolDiagnosticEvent @event, CancellationToken cancellationToken)
        {
            return new ValueTask(func(@event, cancellationToken));
        }

    }

}
