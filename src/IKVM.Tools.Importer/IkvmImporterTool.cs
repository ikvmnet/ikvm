using System.Threading;
using System.Threading.Tasks;

namespace IKVM.Tools.Importer
{

    /// <summary>
    /// Main entry point for the application.
    /// </summary>
    public class IkvmImporterTool
    {

        /// <summary>
        /// Executes the importer.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<int> Main(string[] args, CancellationToken cancellationToken)
        {
            return new IkvmImporterTool().ExecuteAsync(args, cancellationToken);
        }

        /// <summary>
        /// Executes the exporter.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> ExecuteAsync(string[] args, CancellationToken cancellationToken)
        {
            using var context = new IkvmImporterContext(args);
            return context.ExecuteAsync(cancellationToken);
        }

    }

}
