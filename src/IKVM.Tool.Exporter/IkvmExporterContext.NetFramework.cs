#if NETFRAMEWORK

using System;
using System.Threading;
using System.Threading.Tasks;

namespace IKVM.Tool.Exporter
{

    public partial class IkvmExporterContext
    {

        /// <summary>
        /// Encapsulates a <see cref="IkvmExporterInternal"/> in a remote <see cref="AppDomain"/>.
        /// </summary>
        class IkvmExporterDispatcher : MarshalByRefObject
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

            /// <summary>
            /// Executes the exporter.
            /// </summary>
            /// <exception cref="NotImplementedException"></exception>
            public int Execute()
            {
                return IkvmExporterInternal.Execute(options);
            }

        }

        readonly AppDomain appDomain;
        readonly IkvmExporterDispatcher dispatcher;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmExporterContext(IkvmExporterOptions options)
        {
            appDomain = AppDomain.CreateDomain(
                "IkvmExporter",
                AppDomain.CurrentDomain.Evidence,
                new AppDomainSetup() { ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase });

            dispatcher = (IkvmExporterDispatcher)appDomain.CreateInstanceAndUnwrap(
                typeof(IkvmExporterDispatcher).Assembly.GetName().FullName,
                typeof(IkvmExporterDispatcher).FullName,
                false,
                System.Reflection.BindingFlags.Default,
                null,
                new[] { options },
                null,
                null);
        }

        /// <summary>
        /// Creates a new context for the execution.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public partial Task<int> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(dispatcher.Execute());
        }

        /// <summary>
        /// Disposes of the instance.
        /// </summary>
        public void Dispose()
        {
            AppDomain.Unload(appDomain);
            GC.SuppressFinalize(this);
        }

    }

}

#endif
