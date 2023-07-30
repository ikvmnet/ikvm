using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IKVM.Tools.Exporter
{

    /// <summary>
    /// Main entry point for the application.
    /// </summary>
    public class IkvmExporterTool
    {

        /// <summary>
        /// Executes the exporter.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task<int> Main(string[] args, CancellationToken cancellationToken)
        {
            return await new IkvmExporterTool().ExecuteAsync(args, cancellationToken);
        }

        /// <summary>
        /// Executes the exporter.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static async Task<int> ExecuteAsync(IkvmExporterOptions options, CancellationToken cancellationToken)
        {
            using var exporter = new IkvmExporterContext(options);
            return await exporter.ExecuteAsync(cancellationToken);
        }

        readonly RootCommand command;
        readonly Argument<string> assemblyArgument;
        readonly Option<FileInfo> outputOption;
        readonly Option<FileInfo[]> referenceOption;
        readonly Option<FileInfo[]> libraryOption;
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

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public IkvmExporterTool()
        {
            command = new RootCommand()
            {
                (outputOption = new Option<FileInfo>(
                    aliases: new[] { "--out", "-out", "-o" },
                    description: "Specify the output filename.") { IsRequired = true }),
                (referenceOption = new Option<FileInfo[]>(
                    aliases: new [] { "--reference", "-reference", "-r" },
                    description: "Reference an assembly.")),
                (libraryOption = new Option<FileInfo[]>(
                    aliases: new [] { "--lib", "-lib", "-l" },
                    description: "Additional directories to search for references.")),
                (namespaceOption = new Option<string[]>(
                    aliases: new [] { "--ns", "-ns" },
                    description: "Only include types from specified namespace.")),
                (includeNonPublicTypesOption = new Option<bool>(
                    aliases: new [] { "--non-public-types", "-non-public-types" },
                    description: "Include classes for non-public types.")),
                (includeNonPublicInterfacesOption = new Option<bool>(
                    aliases: new [] { "--non-public-interfaces", "-non-public-interfaces" },
                    description: "Include non-public interface implementations.")),
                (includeNonPublicMembersOption = new Option<bool>(
                    aliases: new [] { "--non-public-members", "-non-public-members" },
                    description: "Include non-public members.")),
                (includeParameterNamesOption = new Option<bool>(
                    aliases: new [] { "--parameters", "-parameters" },
                    description: "Include parameter names.")),
                (forwardersOption = new Option<bool>(
                    aliases: new [] { "--forwarders", "-forwarders" },
                    description: "Export forwarded types.")),
                (noStdLibOption = new Option<bool>(
                    aliases: new [] { "--nostdlib", "-nostdlib" },
                    description: "Do not reference standard libraries.")),
                (sharedOption = new Option<bool>(
                    aliases: new [] { "--shared", "-shared" },
                    description: "Process all assemblies in shared group.")),
                (bootstrapOption = new Option<bool>(
                    aliases: new [] { "--bootstrap", "-bootstrap" },
                    description: "Enabled bootstrap mode.") { IsHidden = true }),
                (continueOnErrorOption = new Option<bool>(
                    aliases: new [] { "--skiperror", "-skiperror" },
                    description: "Continue when errors are encountered.")),
                (assemblyArgument = new Argument<string>(
                    name: "assemblyNameOrPath",
                    description: "Path or name of assembly to export.")),
            };

            command.SetHandler(ExecuteAsync);
        }

        /// <summary>
        /// Executes the exporter.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<int> ExecuteAsync(string[] args, CancellationToken cancellationToken)
        {
            return await command.InvokeAsync(args);
        }

        /// <summary>
        /// Executes the program.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        async Task ExecuteAsync(InvocationContext context) => await ExecuteAsync(new IkvmExporterOptions()
        {
            Output = context.ParseResult.GetValueForOption(outputOption).FullName,
            References = context.ParseResult.GetValueForOption(referenceOption).Select(i => i.FullName).ToList(),
            Libraries = context.ParseResult.GetValueForOption(libraryOption).Select(i => i.FullName).ToList(),
            Namespaces = context.ParseResult.GetValueForOption(namespaceOption).ToList(),
            IncludeNonPublicTypes = context.ParseResult.GetValueForOption(includeNonPublicTypesOption),
            IncludeNonPublicInterfaces = context.ParseResult.GetValueForOption(includeNonPublicInterfacesOption),
            IncludeNonPublicMembers = context.ParseResult.GetValueForOption(includeNonPublicMembersOption),
            IncludeParameterNames = context.ParseResult.GetValueForOption(includeParameterNamesOption),
            Forwarders = context.ParseResult.GetValueForOption(forwardersOption),
            NoStdLib = context.ParseResult.GetValueForOption(noStdLibOption),
            Shared = context.ParseResult.GetValueForOption(sharedOption),
            Bootstrap = context.ParseResult.GetValueForOption(bootstrapOption),
            ContinueOnError = context.ParseResult.GetValueForOption(continueOnErrorOption),
            Assembly = context.ParseResult.GetValueForArgument(assemblyArgument)
        }, CancellationToken.None);

    }

}
