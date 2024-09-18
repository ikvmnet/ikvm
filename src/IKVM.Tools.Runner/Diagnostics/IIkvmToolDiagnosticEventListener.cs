using System.Threading;
using System.Threading.Tasks;

namespace IKVM.Tools.Runner.Diagnostics
{

    /// <summary>
    /// Handles diagnostic events from a tool.
    /// </summary>
    public interface IIkvmToolDiagnosticEventListener
    {

        /// <summary>
        /// Receives a diagnostic event.
        /// </summary>
        /// <param name="event"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        ValueTask ReceiveAsync(IkvmToolDiagnosticEvent @event, CancellationToken cancellationToken);

    }

}
