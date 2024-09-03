using System;
using System.Threading;
using System.Threading.Tasks;

namespace IKVM.Tools.Importer
{

    /// <summary>
    /// Imports a Java classpath into a .NET assembly.
    /// </summary>
    partial class IkvmImporterContext : IDisposable
    {

        /// <summary>
        /// Executes the importer logic.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public partial Task<int> ExecuteAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Finalizes the instance.
        /// </summary>
        ~IkvmImporterContext()
        {
            Dispose();
        }

    }

}