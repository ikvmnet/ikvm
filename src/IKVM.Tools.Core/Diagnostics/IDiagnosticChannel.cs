using System.Text;

namespace IKVM.Tools.Core.Diagnostics
{

    /// <summary>
    /// Handles consuming and outputting diagnostic events.
    /// </summary>
    interface IDiagnosticChannel
    {

        /// <summary>
        /// Gets the writer that allows you to enqueue data to the channel.
        /// </summary>
        IDiagnosticChannelWriter Writer { get; }

        /// <summary>
        /// Gets the native encoding of the channel if the channel expects text.
        /// </summary>
        Encoding? Encoding { get; }

    }

}
