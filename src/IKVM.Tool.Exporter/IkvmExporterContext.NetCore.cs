#if NETCOREAPP

using System;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;

namespace IKVM.Tool.Exporter
{

    public partial class IkvmExporterContext
    {

        /// <summary>
        /// Invokes the static instance on the internal type.
        /// </summary>
        class IkvmExporterDispatcher
        {

            readonly IkvmExporterOptions options;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="options"></param>
            /// <exception cref="ArgumentNullException"></exception>
            public IkvmExporterDispatcher(IkvmExporterOptions options)
            {
                this.options = options ?? throw new ArgumentNullException(nameof(options));
            }

            public Task<int> ExecuteAsync(CancellationToken cancellationToken)
            {
                return Task.FromResult(IkvmExporterInternal.Execute(options));
            }

        }

        readonly IkvmExporterDispatcher dispatcher;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmExporterContext(IkvmExporterOptions options)
        {
            // load the exporter in a nested assembly context
            dispatcher = (IkvmExporterDispatcher)Activator.CreateInstance(new AssemblyLoadContext("IkvmExporter", true)
                    .LoadFromAssemblyName(typeof(IkvmExporterDispatcher).Assembly.GetName())
                    .GetType(typeof(IkvmExporterDispatcher).Name),
                options);
        }

        /// <summary>
        /// Creates a new context for the execution.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public partial Task<int> ExecuteAsync(CancellationToken cancellationToken)
        {
            return dispatcher.ExecuteAsync(cancellationToken);
        }

        /// <summary>
        /// Disposes of the instance.
        /// </summary>
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

    }

}

#endif
