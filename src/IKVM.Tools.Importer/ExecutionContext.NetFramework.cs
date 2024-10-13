#if NETFRAMEWORK

using System;
using System.Threading;
using System.Threading.Tasks;

namespace IKVM.Tools.Importer
{

    partial class ExecutionContext
    {

        /// <summary>
        /// Encapsulates a <see cref="ImportContextFactory"/> in a remote <see cref="AppDomain"/>.
        /// </summary>
        class ExecutionContextDispatcher : MarshalByRefObject
        {

            readonly string[] _args;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="args"></param>
            /// <exception cref="ArgumentNullException"></exception>
            public ExecutionContextDispatcher(string[] args)
            {
                _args = args ?? throw new ArgumentNullException(nameof(args));
            }

            /// <summary>
            /// Executes the exporter.
            /// </summary>
            /// <exception cref="NotImplementedException"></exception>
            public int Execute()
            {
                return Task.Run(() => ImportTool.ExecuteInContext(_args)).GetAwaiter().GetResult();
            }

        }

        AppDomain appDomain;
        ExecutionContextDispatcher dispatcher;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="args"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ExecutionContext(string[] args)
        {
            appDomain = AppDomain.CreateDomain(
                "IkvmImporter",
                AppDomain.CurrentDomain.Evidence,
                new AppDomainSetup()
                {
                    ApplicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase,
                    ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile,
                });

            dispatcher = (ExecutionContextDispatcher)appDomain.CreateInstanceAndUnwrap(
                typeof(ExecutionContextDispatcher).Assembly.GetName().FullName,
                typeof(ExecutionContextDispatcher).FullName,
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
