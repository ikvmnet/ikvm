using System;
using System.Threading.Tasks;

namespace IKVM.Tools.Runner
{

    /// <summary>
    /// Diagnostic listener that invokes a delegate for each event.
    /// </summary>
    public class IkvmToolDelegateDiagnosticListener : IIkvmToolDiagnosticEventListener
    {

        readonly Func<IkvmToolDiagnosticEvent, Task> func;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="func"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmToolDelegateDiagnosticListener(Func<IkvmToolDiagnosticEvent, Task> func)
        {
            this.func = func ?? throw new ArgumentNullException(nameof(func));
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="action"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmToolDelegateDiagnosticListener(Action<IkvmToolDiagnosticEvent> action) :
            this(evt => { action(evt); return Task.CompletedTask; })
        {

        }

        public Task ReceiveAsync(IkvmToolDiagnosticEvent @event)
        {
            return func(@event);
        }

    }

}
