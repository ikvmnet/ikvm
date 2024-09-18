using System;
using System.Threading;
using System.Threading.Tasks;

namespace IKVM.Tools.Importer
{

    /// <summary>
    /// Communicates command line arguments across either an <see cref="AppDomain"/> or <see cref="AssemblyLoadContext"/>
    /// boundary. Needed because the compiler infrastructure still makes use of some static data structures.
    /// </summary>
    partial class ExecutionContext : IDisposable
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
        ~ExecutionContext()
        {
            Dispose();
        }

    }

}