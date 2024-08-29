using System.CommandLine;
using System.IO;

namespace IKVM.Tools.Exporter
{
    /// <summary>
    /// Describes the default command of the Exporter.
    /// </summary>
    class ExportCommand : RootCommand
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ExportCommand()
        {
            AddOption(Output = new Option<FileInfo>(
                aliases: ["--out", "-out", "-o"],
                description: "Specify the output filename.") { IsRequired = true });
            AddOption(References = new Option<FileInfo[]>(
                aliases: ["--reference", "-reference", "-r"],
                description: "Reference an assembly."));
            AddOption(Libraries = new Option<FileInfo[]>(
                aliases: ["--lib", "-lib", "-l"],
                description: "Additional directories to search for references."));
            AddOption(Namespaces = new Option<string[]>(
                aliases: ["--ns", "-ns"],
                description: "Only include types from specified namespace."));
            AddOption(IncludeNonPublicTypes = new Option<bool>(
                aliases: ["--non-public-types", "-non-public-types"],
                description: "Include classes for non-public types."));
            AddOption(IncludeNonPublicInterfaces = new Option<bool>(
                aliases: ["--non-public-interfaces", "-non-public-interfaces"],
                description: "Include non-public interface implementations."));
            AddOption(IncludeNonPublicMembers = new Option<bool>(
                aliases: ["--non-public-members", "-non-public-members"],
                description: "Include non-public members."));
            AddOption(IncludeParameterNames = new Option<bool>(
                aliases: ["--parameters", "-parameters"],
                description: "Include parameter names."));
            AddOption(Forwarders = new Option<bool>(
                aliases: ["--forwarders", "-forwarders"],
                description: "Export forwarded types."));
            AddOption(NoStdLib = new Option<bool>(
                aliases: ["--nostdlib", "-nostdlib"],
                description: "Do not reference standard libraries."));
            AddOption(Shared = new Option<bool>(
                aliases: ["--shared", "-shared"],
                description: "Process all assemblies in shared group."));
            AddOption(Bootstrap = new Option<bool>(
                aliases: ["--bootstrap", "-bootstrap"],
                description: "Enabled bootstrap mode.") { IsHidden = true });
            AddOption(ContinueOnError = new Option<bool>(
                aliases: ["--skiperror", "-skiperror"],
                description: "Continue when errors are encountered."));
            AddArgument(Assembly = new Argument<string>(
                name: "assemblyNameOrPath",
                description: "Path or name of assembly to export."));
        }

        public Option<FileInfo> Output { get; }

        public Option<FileInfo[]> References { get; }

        public Option<FileInfo[]> Libraries { get; }

        public Option<string[]> Namespaces { get; }

        public Option<bool> Shared { get; set; }

        public Option<bool> NoStdLib { get; set; }

        public Option<bool> Forwarders { get; set; }

        public Option<bool> IncludeParameterNames { get; set; }

        public Option<bool> IncludeNonPublicTypes { get; set; }

        public Option<bool> IncludeNonPublicInterfaces { get; set; }

        public Option<bool> IncludeNonPublicMembers { get; set; }

        public Option<bool> SerialVersionUID { get; set; }

        public Option<bool> ContinueOnError { get; set; }

        public Option<bool> Bootstrap { get; set; }

        public Argument<string> Assembly { get; set; }

    }

}
