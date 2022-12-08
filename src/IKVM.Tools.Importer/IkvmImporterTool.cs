using System.Threading.Tasks;

namespace IKVM.Tools.Importer
{

    /// <summary>
    /// Main entry point for the application.
    /// </summary>
    public class IkvmImporterTool
    {

        /// <summary>
        /// Executes the exporter.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static Task<int> Main(string[] args)
        {
            return new IkvmImporterTool().ExecuteAsync(args);
        }

        Task<int> ExecuteAsync(string[] args)
        {
            return Task.FromResult(IkvmImporterInternal.Execute(args));
        }

    }

}
