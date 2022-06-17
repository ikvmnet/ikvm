using System.Threading.Tasks;

namespace IKVM.Tool
{

    /// <summary>
    /// Handles diagnostic events from a tool.
    /// </summary>
    public interface IToolDiagnosticEventListener
    {

        /// <summary>
        /// Receives a diagnostic event.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        Task ReceiveAsync(ToolDiagnosticEvent @event);

    }

}
