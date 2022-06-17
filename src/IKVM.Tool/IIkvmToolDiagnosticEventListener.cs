using System.Threading.Tasks;

namespace IKVM.Tool
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
        /// <returns></returns>
        Task ReceiveAsync(IkvmToolDiagnosticEvent @event);

    }

}
