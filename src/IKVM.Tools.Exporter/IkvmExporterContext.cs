using System;
using System.Threading;
using System.Threading.Tasks;

namespace IKVM.Tools.Exporter
{

    /// <summary>
    /// Exports a managed assembly to a JAR file.
    /// </summary>
    public partial class IkvmExporterContext : IDisposable
    {

        /// <summary>
        /// Executes the exporter logic.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public partial Task<int> ExecuteAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Finalizes the instance.
        /// </summary>
        ~IkvmExporterContext()
        {
            Dispose();
        }

    }

}
