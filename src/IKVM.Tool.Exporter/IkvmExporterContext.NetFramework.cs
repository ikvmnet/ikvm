#if NETFRAMEWORK

using System;
using System.Threading;
using System.Threading.Tasks;

namespace IKVM.Tool.Exporter
{

    public partial class IkvmExporterContext
    {

        /// <summary>
        /// Reference to a remote CancellationToken.
        /// </summary>
        class CancellationTokenRef : MarshalByRefObject
        {

            readonly CancellationToken cancellationToken;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="cancellationToken"></param>
            public CancellationTokenRef(CancellationToken cancellationToken)
            {
                this.cancellationToken = cancellationToken;
            }

            public void Register(Action action)
            {
                cancellationToken.Register(action);
            }

        }

        /// <summary>
        /// Encapsulates a <see cref="IkvmExporterInternal"/> in a remote <see cref="AppDomain"/>.
        /// </summary>
        class IkvmExporterRef : MarshalByRefObject
        {

            readonly IkvmExporterOptions options;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="options"></param>
            /// <exception cref="ArgumentNullException"></exception>
            public IkvmExporterRef(IkvmExporterOptions options)
            {
                this.options = options ?? throw new ArgumentNullException(nameof(options));
            }


            /// <summary>
            /// Executes the exporter with the given options.
            /// </summary>
            /// <param name="cancellationTokenRef"></param>
            /// <returns></returns>
            public void BeginExecute(Action<int> onComplete, Action<Exception> onException, CancellationTokenRef cancellationTokenRef)
            {
                Task.Run(() => Execute(onComplete, onException, cancellationTokenRef));
            }

            /// <summary>
            /// Executes the export, signaling completion.
            /// </summary>
            /// <param name="onComplete"></param>
            /// <param name="onException"></param>
            /// <param name="cancellationTokenRef"></param>
            /// <exception cref="NotImplementedException"></exception>
            void Execute(Action<int> onComplete, Action<Exception> onException, CancellationTokenRef cancellationTokenRef)
            {
                try
                {
                    onComplete(IkvmExporterInternal.Execute(options));
                }
                catch (Exception e)
                {
                    onException(e);
                }
            }

        }

        readonly AppDomain appDomain;
        readonly IkvmExporterRef exporter;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmExporterContext(IkvmExporterOptions options)
        {
            appDomain = AppDomain.CreateDomain("IkvmExporter");
            exporter = (IkvmExporterRef)appDomain.CreateInstanceAndUnwrap(
                typeof(IkvmExporterRef).Assembly.GetName().Name, typeof(IkvmExporterRef).Name, false, System.Reflection.BindingFlags.DeclaredOnly, null, new[] { options }, null, null);
        }

        /// <summary>
        /// Creates a new context for the execution.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public partial Task<int> ExecuteAsync(CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<int>();
            exporter.BeginExecute(result => tcs.SetResult(result), e => tcs.SetException(e), new CancellationTokenRef(cancellationToken));
            return tcs.Task;
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
