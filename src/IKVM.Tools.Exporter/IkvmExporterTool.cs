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
        public static Task<int> Main(string[] args, CancellationToken cancellationToken)
        {
            return new IkvmExporterTool().ExecuteAsync(args, cancellationToken);
        }

        /// <summary>
        /// Executes the exporter.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public static Task<int> ExecuteAsync(IkvmExporterOptions options, CancellationToken cancellationToken)
        {
            using var exporter = new IkvmExporterContext(options);
            return exporter.ExecuteAsync(cancellationToken);
        }

        readonly RootCommand command;
        readonly Option<FileInfo> outputOption;
        readonly Option<FileInfo[]> referenceOption;
        readonly Option<bool> japiOption;
        readonly Option<bool> skipErrorOption;
        readonly Option<bool> sharedOption;
        readonly Option<bool> noStdLibOption;
        readonly Option<FileInfo[]> libraryOption;
        readonly Option<string[]> namespaceOption;
        readonly Option<bool> forwardersOption;
        readonly Option<bool> parametersOption;
        readonly Option<bool> bootstrapOption;
        readonly Argument<string> assemblyAttribute;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public IkvmExporterTool()
        {
            command = new RootCommand()
            {
                (outputOption = new Option<FileInfo>(
                    aliases: new[] { "--out", "-out", "-o" },
                    description: "Specify the output filename.")
                {
                    IsRequired = true
                }),
                (referenceOption = new Option<FileInfo[]>(
                    aliases: new [] { "--reference", "-reference", "-r" },
                    description: "Reference an assembly.")),
                (japiOption = new Option<bool>(
                    aliases: new [] {  "--japi", "-japi" },
                    description: "Generate jar suitable for comparison with japitools.")),
                (skipErrorOption = new Option<bool>(
                    aliases: new [] { "--skiperror", "-skiperror" },
                    description: "Continue when errors are encountered.")),
                (sharedOption = new Option<bool>(
                    aliases: new [] { "--shared", "-shared" },
                    description: "Process all assemblies in shared group.")),
                (noStdLibOption = new Option<bool>(
                    aliases: new [] { "--nostdlib", "-nostdlib" },
                    description: "Do not reference standard libraries.")),
                (libraryOption = new Option<FileInfo[]>(
                    aliases: new [] { "--lib", "-lib", "-l" },
                    description: "Additional directories to search for references.")),
                (namespaceOption = new Option<string[]>(
                    aliases: new [] { "--ns", "-ns" },
                    description: "Only include types from specified namespace.")),
                (forwardersOption = new Option<bool>(
                    aliases: new [] { "--forwarders", "-forwarders" },
                    description: "Export forwarded types too.")),
                (parametersOption = new Option<bool>(
                    aliases: new [] { "--parameters", "-parameters" },
                    description: "Emit Java 8 classes with parameter names.")),
                (bootstrapOption = new Option<bool>(
                    aliases: new [] { "--bootstrap", "-bootstrap" },
                    description: "Enabled bootstrap mode.")
                {
                    IsHidden = true,
                }),
                (assemblyAttribute = new Argument<string>(
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
        Task<int> ExecuteAsync(string[] args, CancellationToken cancellationToken)
        {
            return command.InvokeAsync(args);
        }

        /// <summary>
        /// Executes the program.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        Task ExecuteAsync(InvocationContext context)
        {
            return ExecuteAsync(new IkvmExporterOptions()
            {
                Output = context.ParseResult.GetValueForOption(outputOption).FullName,
                References = context.ParseResult.GetValueForOption(referenceOption).Select(i => i.FullName).ToList(),
                JApi = context.ParseResult.GetValueForOption(japiOption),
                SkipError = context.ParseResult.GetValueForOption(skipErrorOption),
                Shared = context.ParseResult.GetValueForOption(sharedOption),
                NoStdLib = context.ParseResult.GetValueForOption(noStdLibOption),
                Libraries = context.ParseResult.GetValueForOption(libraryOption).Select(i => i.FullName).ToList(),
                Namespaces = context.ParseResult.GetValueForOption(namespaceOption).ToList(),
                Forwarders = context.ParseResult.GetValueForOption(forwardersOption),
                Parameters = context.ParseResult.GetValueForOption(parametersOption),
                Boostrap = context.ParseResult.GetValueForOption(bootstrapOption),
                Assembly = context.ParseResult.GetValueForArgument(assemblyAttribute)
            }, CancellationToken.None);
        }

    }

}
