using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.Threading;
using System.Threading.Tasks;

using IKVM.CoreLib.Diagnostics;
using IKVM.Tools.Core.CommandLine;
using IKVM.Tools.Core.Diagnostics;

using Microsoft.Extensions.DependencyInjection;

namespace IKVM.Tools.Importer
{

    /// <summary>
    /// Main entry point for the application.
    /// </summary>
    public class ImportTool
    {

        /// <summary>
        /// Executes the importer against a set of command line options.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<int> InvokeAsync(string[] args, CancellationToken cancellationToken = default)
        {
            return await new ImportTool().InvokeImplAsync(args, cancellationToken);
        }

        /// <summary>
        /// Executes the importer.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="diagnostics"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        static Task<int> ExecuteAsync(ImportOptions options, IDiagnosticHandler diagnostics, CancellationToken cancellationToken = default)
        {
            return ExecuteImplAsync(options, _ => diagnostics, cancellationToken);
        }

        /// <summary>
        /// Executes the importer.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="diagnostics"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        static Task<int> ExecuteImplAsync(ImportOptions options, Func<IServiceProvider, IDiagnosticHandler> diagnostics, CancellationToken cancellationToken)
        {
            var services = new ServiceCollection();
            services.AddToolsDiagnostics();
            services.AddSingleton(options);
            services.AddSingleton(diagnostics);
            services.AddSingleton<ImportImpl>();
            using var provider = services.BuildServiceProvider();
            return ExecuteImplAsync(options, provider, cancellationToken);
        }

        /// <summary>
        /// Executes the importer.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        static Task<int> ExecuteImplAsync(ImportOptions options, IServiceProvider services, CancellationToken cancellationToken)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));

            var importer = services.GetRequiredService<ImportImpl>();
            return Task.FromResult(importer.Execute(options));
        }

        /// <summary>
        /// Executes the program.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        static Task ExecuteAsync(ImportOptions options, CancellationToken cancellationToken = default)
        {
            if (options is null)
                throw new ArgumentNullException(nameof(options));

            return ExecuteImplAsync(options, p => GetDiagnostics(p, options.Log), cancellationToken);
        }

        /// <summary>
        /// Generates a diagnostic instance from the diagnostics options.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="spec"></param>
        /// <returns></returns>
        static FormattedDiagnosticHandler GetDiagnostics(IServiceProvider services, string spec)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));
            if (string.IsNullOrWhiteSpace(spec))
                throw new ArgumentException($"'{nameof(spec)}' cannot be null or whitespace.", nameof(spec));

            return ActivatorUtilities.CreateInstance<FormattedDiagnosticHandler>(services, spec);
        }

        readonly RootCommand _command;
        readonly Parser _parser;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ImportTool()
        {
            _command = new ImportCommand();
            _command.SetHandler((options) => ExecuteAsync(options), new ImportOptionsBinding());

            _parser = new CommandLineBuilder(_command).UseDefaults().UseCommandExceptionHandler().Build();
        }

        /// <summary>
        /// Executes the exporter.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<int> InvokeImplAsync(string[] args, CancellationToken cancellationToken)
        {
            if (args is null)
                throw new ArgumentNullException(nameof(args));

            return await _parser.InvokeAsync(args);
        }

    }

}
