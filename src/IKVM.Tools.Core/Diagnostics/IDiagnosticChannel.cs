using System.IO.Pipelines;
using System.Text;

namespace IKVM.Tools.Core.Diagnostics
{

    /// <summary>
    /// Handles consuming and outputting diagnostic events.
    /// </summary>
    interface IDiagnosticChannel
    {

        /// <summary>
        /// Gets the <see cref="PipeWriter"/> that writes bytes to the channel.
        /// </summary>
        /// <returns></returns>
        PipeWriter Writer { get; }

        /// <summary>
        /// Gets the native encoding of the channel if the channel expects text.
        /// </summary>
        Encoding? Encoding { get; }

    }

}
