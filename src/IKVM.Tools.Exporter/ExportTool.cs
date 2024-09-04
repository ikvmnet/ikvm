using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IKVM.CoreLib.Diagnostics;
using IKVM.Tools.Core.Diagnostics;

using Microsoft.Extensions.DependencyInjection;

namespace IKVM.Tools.Exporter
{

    /// <summary>
    /// Main entry point for the application.
    /// </summary>
    class ExportTool
    {

        /// <summary>
        /// Executes the exporter against a set of command line options.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<int> InvokeAsync(string[] args, CancellationToken cancellationToken = default)
        {
            return await new ExportTool().InvokeImplAsync(args, cancellationToken);
        }

        /// <summary>
        /// Executes the exporter.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="diagnostics"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<int> ExecuteAsync(ExportOptions options, IDiagnosticHandler diagnostics, CancellationToken cancellationToken = default)
        {
            return ExecuteImplAsync(options, _ => diagnostics, cancellationToken);
        }

        /// <summary>
        /// Executes the exporter.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="diagnostics"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        static Task<int> ExecuteImplAsync(ExportOptions options, Func<IServiceProvider, IDiagnosticHandler> diagnostics, CancellationToken cancellationToken)
        {
            var services = new ServiceCollection();
            services.AddToolsDiagnostics();
            services.AddSingleton(options);
            services.AddSingleton(diagnostics);
            services.AddSingleton<ExportImpl>();
            services.AddSingleton<ManagedTypeResolver>();
            services.AddSingleton<StaticCompiler>();
            using var provider = services.BuildServiceProvider();
            var exporter = provider.GetRequiredService<ExportImpl>();

            return Task.FromResult(exporter.Execute());
        }

        /// <summary>
        /// Executes the exporter.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        static Task<int> MainAsync(IServiceProvider services, CancellationToken cancellationToken)
        {
            if (services is null)
                throw new ArgumentNullException(nameof(services));

            var exporter = services.GetRequiredService<ExportImpl>();
            return Task.FromResult(exporter.Execute());
        }

        /// <summary>
        /// Executes the program.
        /// </summary>
        /// <param name="commandOptions"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        static Task ExecuteAsync(ExportCommandOptions commandOptions, CancellationToken cancellationToken)
        {
            if (commandOptions is null)
                throw new ArgumentNullException(nameof(commandOptions));

            var options = new ExportOptions()
            {
                Output = commandOptions.Output.FullName,
                IncludeNonPublicTypes = commandOptions.IncludeNonPublicTypes,
                IncludeNonPublicInterfaces = commandOptions.IncludeNonPublicInterfaces,
                IncludeNonPublicMembers = commandOptions.IncludeNonPublicMembers,
                IncludeParameterNames = commandOptions.IncludeParameterNames,
                Forwarders = commandOptions.Forwarders,
                NoStdLib = commandOptions.NoStdLib,
                Shared = commandOptions.Shared,
                Bootstrap = commandOptions.Bootstrap,
                ContinueOnError = commandOptions.ContinueOnError,
                Assembly = commandOptions.Assembly?.FullName,
            };

            foreach (var i in commandOptions.References)
                options.References.Add(i.FullName);

            foreach (var i in commandOptions.Libraries)
                options.Libraries.Add(i.FullName);

            return ExecuteImplAsync(options, p => GetDiagnostics(p, commandOptions.Log), cancellationToken);
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

        readonly RootCommand command;
        readonly Argument<FileInfo> assemblyArgument;
        readonly Option<FileInfo> outputOption;
        readonly Option<FileInfo[]> referenceOption;
        readonly Option<DirectoryInfo[]> libraryOption;
        readonly Option<string[]> namespaceOption;
        readonly Option<bool> includeNonPublicTypesOption;
        readonly Option<bool> includeNonPublicInterfacesOption;
        readonly Option<bool> includeNonPublicMembersOption;
        readonly Option<bool> includeParameterNamesOption;
        readonly Option<bool> noStdLibOption;
        readonly Option<bool> sharedOption;
        readonly Option<bool> forwardersOption;
        readonly Option<bool> bootstrapOption;
        readonly Option<bool> continueOnErrorOption;
        readonly Option<string> logOption;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ExportTool()
        {
            command = new RootCommand()
            {
                (outputOption = new Option<FileInfo>(
                    aliases: ["--out", "-out", "-o"],
                    description: "Specify the output filename.") { IsRequired = true }),
                (referenceOption = new Option<FileInfo[]>(
                    aliases: ["--reference", "-reference", "-r"],
                    description: "Reference an assembly.")),
                (libraryOption = new Option<DirectoryInfo[]>(
                    aliases: ["--lib", "-lib", "-l"],
                    description: "Additional directories to search for references.")),
                (namespaceOption = new Option<string[]>(
                    aliases: ["--ns", "-ns"],
                    description: "Only include types from specified namespace.")),
                (includeNonPublicTypesOption = new Option<bool>(
                    aliases: ["--non-public-types", "-non-public-types"],
                    description: "Include classes for non-public types.")),
                (includeNonPublicInterfacesOption = new Option<bool>(
                    aliases: ["--non-public-interfaces", "-non-public-interfaces"],
                    description: "Include non-public interface implementations.")),
                (includeNonPublicMembersOption = new Option<bool>(
                    aliases: ["--non-public-members", "-non-public-members"],
                    description: "Include non-public members.")),
                (includeParameterNamesOption = new Option<bool>(
                    aliases: ["--parameters", "-parameters"],
                    description: "Include parameter names.")),
                (forwardersOption = new Option<bool>(
                    aliases: ["--forwarders", "-forwarders"],
                    description: "Export forwarded types.")),
                (noStdLibOption = new Option<bool>(
                    aliases: ["--nostdlib", "-nostdlib"],
                    description: "Do not reference standard libraries.")),
                (sharedOption = new Option<bool>(
                    aliases: ["--shared", "-shared"],
                    description: "Process all assemblies in shared group.")),
                (bootstrapOption = new Option<bool>(
                    aliases: ["--bootstrap", "-bootstrap"],
                    description: "Enabled bootstrap mode.") { IsHidden = true }),
                (continueOnErrorOption = new Option<bool>(
                    aliases: ["--skiperror", "-skiperror"],
                    description: "Continue when errors are encountered.")),
                (logOption = new Option<string>(
                    aliases: ["--log", "-log"],
                    getDefaultValue: () => "text",
                    description: "Logging options.")),
                (assemblyArgument = new Argument<FileInfo>(
                    name: "assemblyNameOrPath",
                    description: "Path or name of assembly to export.")),
            };

            command.SetHandler(ExecuteImplAsync);
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

            return await command.InvokeAsync(args);
        }

        /// <summary>
        /// Executes the program.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task ExecuteImplAsync(InvocationContext context)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            var options = new ExportCommandOptions()
            {
                Output = context.ParseResult.GetValueForOption(outputOption),
                References = context.ParseResult.GetValueForOption(referenceOption),
                Libraries = context.ParseResult.GetValueForOption(libraryOption),
                Namespaces = context.ParseResult.GetValueForOption(namespaceOption).ToArray(),
                IncludeNonPublicTypes = context.ParseResult.GetValueForOption(includeNonPublicTypesOption),
                IncludeNonPublicInterfaces = context.ParseResult.GetValueForOption(includeNonPublicInterfacesOption),
                IncludeNonPublicMembers = context.ParseResult.GetValueForOption(includeNonPublicMembersOption),
                IncludeParameterNames = context.ParseResult.GetValueForOption(includeParameterNamesOption),
                Forwarders = context.ParseResult.GetValueForOption(forwardersOption),
                NoStdLib = context.ParseResult.GetValueForOption(noStdLibOption),
                Shared = context.ParseResult.GetValueForOption(sharedOption),
                Bootstrap = context.ParseResult.GetValueForOption(bootstrapOption),
                ContinueOnError = context.ParseResult.GetValueForOption(continueOnErrorOption),
                Assembly = context.ParseResult.GetValueForArgument(assemblyArgument),
                Log = context.ParseResult.GetValueForOption(logOption)
            };

            return ExecuteAsync(options, context.GetCancellationToken());
        }

    }

}
