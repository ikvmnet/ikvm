using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Invocation;
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
            using var context = new IkvmImporterContext(args);
            return await context.ExecuteAsync(cancellationToken);
        }

        /// <summary>
        /// Executes the importer against a set of command line options.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        internal static async Task<int> ExecuteInContext(string[] args, CancellationToken cancellationToken = default)
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
            return importer.ExecuteAsync(options, cancellationToken);
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

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ImportTool()
        {

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

            var stack = new Stack<ImportArgLevel>();
            var level = new ImportArgLevel(0);

            foreach (var token in new Parser().Parse(args).Tokens)
            {
                // open new level, set as current
                if (token.Value == "{")
                {
                    stack.Push(level);
                    level = new ImportArgLevel(level.Depth + 1);
                    continue;
                }

                // close current level, parent becomes current
                if (token.Value == "}")
                {
                    var parent = stack.Pop();
                    parent.Nested.Add(level);
                    level = parent;
                    continue;
                }

                // add arg to current level
                level.Args.Add(token.Value);
            }

            // root command binds the first level, and accepts the nested levels
            var command = new ImportCommand();
            command.SetHandler(options => ExecuteAsync(options), new ImportOptionsBinding(level.Nested.ToArray(), null));

            // execute command
            return await new CommandLineBuilder(command)
                .UseVersionOption(["-version", "-version"])
                .UseHelp()
                .UseParseDirective()
                .UseTypoCorrections()
                .UseParseErrorReporting()
                .UseExceptionHandler()
                .CancelOnProcessTermination()
                .UseCommandErrorExceptionHandler()
                .Build()
                .InvokeAsync(level.Args.ToArray());
        }

    }

}
