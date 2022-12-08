using System;
using System.Threading;
using System.Threading.Tasks;

namespace IKVM.Tools.Importer
{

    /// <summary>
    /// Processes a set of arguments to import a Java library or class files into a .NET assembly.
    /// </summary>
    public class IkvmImporter : IDisposable
    {

        readonly IkvmImporterContext context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmImporter(string[] args)
        {
            this.context = new IkvmImporterContext(args ?? throw new ArgumentNullException(nameof(args)));
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

        /// <summary>
        /// Disposes of the instance.
        /// </summary>
        public void Dispose()
        {
            context.Dispose();
        }

    }

}
