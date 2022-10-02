using System;
using System.Threading;
using System.Threading.Tasks;

namespace IKVM.Tools.Exporter
{

    /// <summary>
    /// 
    /// </summary>
    public class IkvmExporter
    {

        public IkvmExporterContext context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmExporter(IkvmExporterOptions options)
        {
            this.context = new IkvmExporterContext(options ?? throw new ArgumentNullException(nameof(options)));
        }

        /// <summary>
        /// Executes the exporter.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<int> ExecuteAsync(CancellationToken cancellationToken)
        {
            return context.ExecuteAsync(cancellationToken);
        }

    }

}
