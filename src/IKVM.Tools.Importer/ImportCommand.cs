#nullable enable

using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.IO;

using IKVM.CoreLib.Diagnostics;

namespace IKVM.Tools.Importer
{

    class ImportCommand : RootCommand
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="enableDefaults"></param>
        public ImportCommand(bool enableDefaults = true)
        {
            Add(InputsArgument = new Argument<FileInfo[]>(
                name: "classOrJar",
                description: "Path or name of class or JAR file to import.")
                .LegalFilePathsOnly()
                .ExistingOnly());

            foreach (var option in new Option[] {
                (OutputOption = new Option<FileInfo?>(
                    aliases: ["-out", "-o"],
                    description: "Specify the output filename.")
                    .LegalFilePathsOnly()),
                (AssemblyNameOption = new Option<string?>(
                    name: "-assembly",
                    description: "Specify assembly name.")),
                (TargetOption = new Option<string?>(
                    name: "-target",
                    description: "Specifies the format of the output file.",
                    getDefaultValue: () => enableDefaults ? "library" : null)
                    .FromAmong("exe", "winexe", "library", "module")),
                (PlatformOption = new Option<string?>(
                    name: "-platform",
                    description: "Limit which platforms this code can run on: x86, x64, arm, anycpu32bitpreferred, or anycpu.\nThe default is anycpu.",
                    getDefaultValue: () => enableDefaults ? "anycpu" : null)
                    .FromAmong("x86", "x64", "arm", "arm64", "anycpu32bitpreferred", "anycpu")),
                (ApartmentOption = new Option<string?>(
                    name: "-apartment",
                    description: "Apply STAThreadAttribute to main.",
                    getDefaultValue: () => enableDefaults ? "none" : null)
                    .FromAmong("sta", "mta", "none")),
                (NoGlobbingOption = new Option<bool>(
                    name: "-noglobbing",
                    description: "Don't glob the arguments passed to main.")),
                (PropertiesOption = new Option<string[]>(
                    name: "-D",
                    description: "Set system property (at runtime).")),
                (EnableAssertionsOption = new Option<string?>(
                    aliases: [ "-enableassertions", "-ea" ],
                    parseArgument: ParseOptionalString,
                    isDefault: true,
                    description: "Set system property to enable assertions.") { Arity = ArgumentArity.ZeroOrMore, IsRequired = false }),
                (DisableAssertionsOption = new Option<string?>(
                    aliases: [ "-disableassertions", "-da"],
                    parseArgument: ParseOptionalString,
                    isDefault: true,
                    description: "Set system property to disable assertions.") { Arity = ArgumentArity.ZeroOrMore, IsRequired = false }),
                (RemoveAssertionsOption = new Option<bool>(
                    name: "-removeassertions",
                    description: "Remove all assert statements.")),
                (MainClassOption = new Option<string>(
                    name: "-main",
                    description: "Specify the class containing the main method.")),
                (ReferenceOption = new Option<string[]>(
                    aliases: ["-reference", "-r"],
                    description: "Reference an assembly.")),
                (RecurseOption = new Option<string[]>(
                    name: "-recurse",
                    description: "Recurse directory and include matching file.")),
                (ResourceOption = new Option<string[]>(
                    name: "-resource",
                    description: "Include file as Java resource.") { Arity = ArgumentArity.OneOrMore }),
                (ExternalResourceOption = new Option<string[]>(
                    aliases: ["-externalresource"],
                    description: "Reference file as Java resource.") { Arity = ArgumentArity.OneOrMore }),
                (NoJNIOption = new Option<bool>(
                    name: "-nojni",
                    description: "Do not generate JNI stub for native methods.")),
                (ExcludeOption = new Option<FileInfo?>(
                    name: "-exclude",
                    description: "A file containing a list of classes to exclude.")
                    .LegalFilePathsOnly()),
                (VersionOption = new Option<string?>(
                    name: "-version",
                    description: "Specify assembly version.")),
                (FileVersionOption = new Option<string?>(
                    name: "-fileversion",
                    description: "File version.")),
                (Win32IconOption = new Option<FileInfo?>(
                    name: "-win32icon",
                    description: "Embed specified icon in output.")
                    .LegalFilePathsOnly()),
                (Win32ManifestOption = new Option<FileInfo?>(
                    name: "-win32manifest",
                    description: "Specify a Win32 manifest file (.xml).")
                    .LegalFilePathsOnly()),
                (KeyFileOption = new Option<FileInfo?>(
                    aliases: ["-keyfile"],
                    description: "Use keyfile to sign the assembly.")
                    .LegalFilePathsOnly()),
                (KeyOption = new Option<string?>(
                    aliases: ["-key"],
                    description: "Use keycontainer to sign the assembly.")),
                (DelaySignOption = new Option<bool>(
                    aliases: ["-delaysign"],
                    description: "Delay-sign the assembly.")),
                (DebugOption = new Option<string?>(
                    aliases: ["-debug"],
                    description: "Specify debugging type ('portable' is default)\n'portable' is a cross-platform format,\n'embedded' is a cross-platform format embedded into the target dll or exe.",
                    getDefaultValue: () => enableDefaults ? "portable" : null)
                    .FromAmong("none", "portable", "embedded")),
                (DeterministicOption = new Option<bool>(
                    aliases: ["-deterministic"],
                    description: "Produce a deterministic assembly (including module version GUID and timestamp).")),
                (OptimizeOption = new Option<bool>(
                    aliases: ["-optimize"],
                    description: "")),
                (SourcePathOption = new Option<DirectoryInfo?>(
                    aliases: ["-srcpath"],
                    description: "Prepend path and package name to source file.")
                    .LegalFilePathsOnly()),
                (RemapOption = new Option<FileInfo?>(
                    aliases: ["-remap"],
                    description: "Specify a XML map file that rewrites entities.")
                    .LegalFilePathsOnly()),
                (NoStackTraceInfoOption = new Option<bool>(
                    aliases: ["-nostacktraceinfo"],
                    description: "Don't create metadata to emit rich stack traces.")),
                (RemoveUnusedPrivateFieldsOption = new Option<bool>(
                    aliases: ["-opt:fields"],
                    description: "Remove unused private fields.")),
                (CompressResourcesOption = new Option<bool>(
                    aliases: ["-compressresources"],
                    description: "Compress resources.")),
                (StrictFinalFieldSemanticsOption = new Option<bool>(
                    aliases: ["-strictfinalfieldsemantics"],
                    description: " Don't allow final fields to be modified outside of initializer methods.")),
                (PrivatePackageOption = new Option<string[]>(
                    aliases: ["-privatepackage"],
                    description: "Mark all classes with a package name starting with <prefix> as internal to the assembly.")),
                (PublicPackageOption = new Option<string[]>(
                    aliases: ["-publicpackage"],
                    description: "Mark all classes with a package name starting with <prefix> as public to the assembly.")),
                (NoWarnOption = new Option<Diagnostic[]>(
                    aliases: ["-nowarn"],
                    description: "Disable specific warning messages.",
                    parseArgument: ParseDiagnosticArray)),
                (WarnAsErrorOption = new Option<Diagnostic[]>(
                    aliases: ["-warnaserror"],
                    description: "Report all warnings as errors.",
                    parseArgument: ParseDiagnosticArray)),
                (RuntimeOption = new Option<FileInfo?>(
                    aliases: ["-runtime"],
                    description: "Path to the IKVM.Runtime assembly.")
                    .LegalFilePathsOnly()),
                (TimeOption = new Option<bool>(
                    aliases: ["-time"],
                    description: "Display timing statistics.")),
                (ClassLoaderOption = new Option<string?>(
                    aliases: ["-classloader"],
                    description: "Set custom class loader class for assembly.")),
                (SharedClassLoaderOption = new Option<bool>(
                    aliases: ["-sharedclassloader"],
                    description: "All targets below this level share a common class loader.")),
                (BaseAddressOption = new Option<string?>(
                    aliases: ["-baseaddress"],
                    description: "Base address for the library to be built.")),
                (FileAlignOption = new Option<string?>(
                    aliases: ["-filealign"],
                    description: "Specify the alignment used for output file.")),
                (NoPeerCrossReferenceOption = new Option<bool>(
                    aliases: ["-nopeercrossreference"],
                    description: "Do not automatically cross reference all peers.")),
                (NoStdLibOption = new Option<bool>(
                    aliases: ["-nostdlib"],
                    description: "Do not reference standard libraries")),
                (LibraryOption = new Option<DirectoryInfo[]>(
                    aliases: ["-lib"],
                    description: "Additional directories to search for references.")),
                (NoAutoSerializationOption = new Option<bool>(
                    aliases: ["-noautoserialization"],
                    description: "Disable automatic .NET serialization support.")),
                (HighEntropyVAOption = new Option<bool>(
                    aliases: ["-highentropyva"],
                    description: "Enable high entropy ASLR.")),
                (ProxyOption = new Option<string[]>(
                    aliases: ["-proxy"],
                    description: "Generate proxies for specified class.")),
                (AllowNonVirtualCallsOption = new Option<bool>(
                    aliases: ["-XX:+AllowNonVirtualCalls"],
                    description: "Allow non-virtual calls.")),
                (StaticOption = new Option<bool>(
                    aliases: ["-static"],
                    description: "Disable dynamic binding.")),
                (NoJarStubsOption = new Option<bool>(
                    aliases: ["-nojarstubs"],
                    description: "undocumented temporary option to mitigate risk.")),
                (AssemblyAttributesOption = new Option<FileInfo[]>(
                    aliases: ["-assemblyattributes"],
                    description: "Read assembly custom attributes from specified class file.")
                    .LegalFilePathsOnly()
                    .ExistingOnly()),
                (WarningLevel4Option = new Option<bool>(
                    aliases: ["-w4"],
                    description: "Undocumented option to always warn if a class isn't found.")),
                (NoParameterReflectionOption = new Option<bool>(
                    aliases: ["-noparameterreflection"],
                    description: "Undocumented option to compile core class libraries with, to disable MethodParameter attribute.")),
                (BootstrapOption = new Option<bool>(
                    aliases: ["-bootstrap"],
                    description: "Undocumented option to compile core class libraries with, to allow creation of base class libraries.")),
                (LogOption = new Option<string>(
                    aliases: ["-log"],
                    getDefaultValue: () => "text",
                    description: "Logging options."))
                })
                Add(option);

            ResourceOption.AddValidator(ValidateResource);
        }

        /// <summary>
        /// Parse an option set of Diagnostic ID values.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        Diagnostic[] ParseDiagnosticArray(ArgumentResult result)
        {
            var l = new List<Diagnostic>();

            foreach (var i in result.Tokens)
            {
                foreach (var j in i.Value.Split([';', ','], StringSplitOptions.RemoveEmptyEntries))
                {
                    if (int.TryParse(j, out var id) && Diagnostic.GetById(id) is { } diagnostic)
                        l.Add(diagnostic);

                    if (j.StartsWith("IKVM", StringComparison.OrdinalIgnoreCase) && int.TryParse(j["IKVM".Length..], out var id2) && Diagnostic.GetById(id2) is { } diagnostic2)
                        l.Add(diagnostic2);
                }
            }

            return l.ToArray();
        }

        /// <summary>
        /// Parses an optional string option. An optional string argument represents three states: not present (null),
        /// empty (present with no values) or specified (present with values).
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        string? ParseOptionalString(ArgumentResult result)
        {
            if (result.Tokens.Count == 0)
            {
                // no argument specified
                if (result.Parent is OptionResult { IsImplicit: true })
                    return null;

                // argument specified but no items
                return "";
            }

            // items
            var l = new string[result.Tokens.Count];
            for (int i = 0; i < result.Tokens.Count; i++)
                l[i] = result.Tokens[i].Value;

            return string.Join(";", l);
        }

        /// <summary>
        /// Validates the -resource option.
        /// </summary>
        /// <param name="symbolResult"></param>
        void ValidateResource(OptionResult symbolResult)
        {

        }

        public Argument<FileInfo[]> InputsArgument { get; set; }

        public Option<FileInfo?> OutputOption { get; set; }

        public Option<string?> AssemblyNameOption { get; set; }

        public Option<string?> TargetOption { get; set; }

        public Option<string?> PlatformOption { get; set; }

        public Option<string?> ApartmentOption { get; set; }

        public Option<bool> NoGlobbingOption { get; set; }

        public Option<string[]> PropertiesOption { get; set; }

        public Option<string?> EnableAssertionsOption { get; set; }

        public Option<string?> DisableAssertionsOption { get; set; }

        public Option<bool> RemoveAssertionsOption { get; set; }

        public Option<string> MainClassOption { get; set; }

        public Option<string[]> ReferenceOption { get; set; }

        public Option<string[]> RecurseOption { get; set; }

        public Option<string[]> ResourceOption { get; set; }

        public Option<string[]> ExternalResourceOption { get; set; }

        public Option<bool> NoJNIOption { get; set; }

        public Option<FileInfo?> ExcludeOption { get; set; }

        public Option<string?> VersionOption { get; set; }

        public Option<string?> FileVersionOption { get; set; }

        public Option<FileInfo?> Win32IconOption { get; set; }

        public Option<FileInfo?> Win32ManifestOption { get; set; }

        public Option<FileInfo?> KeyFileOption { get; set; }

        public Option<string?> KeyOption { get; set; }

        public Option<bool> DelaySignOption { get; set; }

        public Option<string?> DebugOption { get; set; }

        public Option<bool> DeterministicOption { get; set; }

        public Option<bool> OptimizeOption { get; set; }

        public Option<DirectoryInfo?> SourcePathOption { get; set; }

        public Option<FileInfo?> RemapOption { get; set; }

        public Option<bool> NoStackTraceInfoOption { get; set; }

        public Option<bool> RemoveUnusedPrivateFieldsOption { get; set; }

        public Option<bool> CompressResourcesOption { get; set; }

        public Option<bool> StrictFinalFieldSemanticsOption { get; set; }

        public Option<string[]> PrivatePackageOption { get; set; }

        public Option<string[]> PublicPackageOption { get; set; }

        public Option<Diagnostic[]> NoWarnOption { get; set; }

        public Option<Diagnostic[]> WarnAsErrorOption { get; set; }

        public Option<FileInfo?> RuntimeOption { get; set; }

        public Option<bool> TimeOption { get; set; }

        public Option<string?> ClassLoaderOption { get; set; }

        public Option<bool> SharedClassLoaderOption { get; set; }

        public Option<string?> BaseAddressOption { get; set; }

        public Option<string?> FileAlignOption { get; set; }

        public Option<bool> NoPeerCrossReferenceOption { get; set; }

        public Option<bool> NoStdLibOption { get; set; }

        public Option<DirectoryInfo[]> LibraryOption { get; set; }

        public Option<bool> NoAutoSerializationOption { get; set; }

        public Option<bool> HighEntropyVAOption { get; set; }

        public Option<string[]> ProxyOption { get; set; }

        public Option<bool> AllowNonVirtualCallsOption { get; set; }

        public Option<bool> StaticOption { get; set; }

        public Option<bool> NoJarStubsOption { get; set; }

        public Option<FileInfo[]> AssemblyAttributesOption { get; set; }

        public Option<bool> WarningLevel4Option { get; set; }

        public Option<bool> NoParameterReflectionOption { get; set; }

        public Option<bool> BootstrapOption { get; set; }

        public Option<string> LogOption { get; set; }

    }

}
