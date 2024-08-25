#if NETFRAMEWORK

using System;
using System.Threading;
using System.Threading.Tasks;

namespace IKVM.Tools.Importer
{

    public partial class IkvmImporterContext
    {

        /// <summary>
        /// Encapsulates a <see cref="IkvmImporterInternal"/> in a remote <see cref="AppDomain"/>.
        /// </summary>
        class IkvmImporterDispatcher : MarshalByRefObject
        {

            readonly string[] args;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="args"></param>
            /// <exception cref="ArgumentNullException"></exception>
            public IkvmImporterDispatcher(string[] args)
            {
                this.args = args ?? throw new ArgumentNullException(nameof(args));
            }

            /// <summary>
            /// Executes the exporter.
            /// </summary>
            /// <exception cref="NotImplementedException"></exception>
            public int Execute()
            {
                return IkvmImporterInternal.Execute(args);
            }

        }

        AppDomain appDomain;
        IkvmImporterDispatcher dispatcher;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmImporterContext(string[] args)
        {
            appDomain = AppDomain.CreateDomain(
                "IkvmImporter",
                AppDomain.CurrentDomain.Evidence,
                new AppDomainSetup()
                {
                    ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                    ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
                });

            dispatcher = (IkvmImporterDispatcher)appDomain.CreateInstanceAndUnwrap(
                typeof(IkvmImporterDispatcher).Assembly.GetName().FullName,
                typeof(IkvmImporterDispatcher).FullName,
                false,
                System.Reflection.BindingFlags.Default,
                null,
                [args],
                null,
                null);
        }

        /// <summary>
        /// Creates a new context for the execution.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public partial async Task<int> ExecuteAsync(CancellationToken cancellationToken)
        {
            return await Task.Run(dispatcher.Execute);
        }

        /// <summary>
        /// Disposes of the instance.
        /// </summary>
        public void Dispose()
        {
            try
            {
                dispatcher = null;
                if (appDomain != null)
                    AppDomain.Unload(appDomain);
            }
            catch
            {
                // ignore
            }
            finally
            {
                appDomain = null;
            }

            GC.SuppressFinalize(this);
        }

    }

}

#endif
