using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using IKVM.Tools.Runner.Importer;

using Microsoft.Build.Framework;

namespace IKVM.MSBuild.Tasks
{

    /// <summary>
    /// Executes the IKVM compiler.
    /// </summary>
    public class IkvmCompiler : IkvmToolExecTask
    {

        /// <summary>
        /// Optional path to the response file to generate. If specified, the response file is not cleaned up.
        /// </summary>
        public string ResponseFile { get; set; }

        /// <summary>
        /// Input items to be compiled.
        /// </summary>
        [Required]
        public ITaskItem[] Input { get; set; }

        /// <summary>
        /// Path of the output assembly.
        /// </summary>
        [Required]
        public string Output { get; set; }

        /// <summary>
        /// Name of the assembly to output.
        /// </summary>
        public string Assembly { get; set; }

        /// <summary>
        /// Version of the assembly to output.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Target type of the assembly to output.
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// Platform of the assembly to output.
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// Key file with which to strong name the generated assembly.
        /// </summary>
        public string KeyFile { get; set; }

        /// <summary>
        /// Key with which to strong name the generated assembly.
        /// </summary>
        public string Key { get; set; }

        public bool DelaySign { get; set; }

        /// <summary>
        /// References to use when compiling.
        /// </summary>
        public ITaskItem[] References { get; set; }

        public ITaskItem[] Recurse { get; set; }

        /// <summary>
        /// Path to class exclusion file.
        /// </summary>
        public string Exclude { get; set; }

        /// <summary>
        /// File version of assembly to output.
        /// </summary>
        public string FileVersion { get; set; }

        /// <summary>
        /// Path to the Win32 icon file.
        /// </summary>
        public string Win32Icon { get; set; }

        /// <summary>
        /// Path to the Win32 manifest file.
        /// </summary>
        public string Win32Manifest { get; set; }

        public ITaskItem[] Resources { get; set; }

        public ITaskItem[] ExternalResources { get; set; }

        public bool CompressResources { get; set; }

        public string Debug { get; set; }

        public bool NoAutoSerialization { get; set; }

        public bool NoGlobbing { get; set; }

        public bool NoJNI { get; set; }

        public bool OptFields { get; set; }

        public bool RemoveAssertions { get; set; }

        public bool StrictFinalFieldSemantics { get; set; }

        public string NoWarn { get; set; }

        public bool WarnAsError { get; set; }

        public string WarnAsErrorWarnings { get; set; }

        public string WriteSuppressWarningsFile { get; set; }

        public string Main { get; set; }

        public string SrcPath { get; set; }

        public string Apartment { get; set; }

        public string SetProperties { get; set; }

        public bool NoStackTraceInfo { get; set; }

        public string XTrace { get; set; }

        public string XMethodTrace { get; set; }

        public string PrivatePackages { get; set; }

        public string ClassLoader { get; set; }

        public bool SharedClassLoader { get; set; }

        public string BaseAddress { get; set; }

        public string FileAlign { get; set; }

        public bool NoPeerCrossReference { get; set; }

        public bool NoStdLib { get; set; }

        public ITaskItem[] Lib { get; set; }

        public bool HighEntropyVA { get; set; }

        public bool Static { get; set; }

        /// <summary>
        /// Paths to the assembly attributes file.
        /// </summary>
        public ITaskItem[] AssemblyAttributes { get; set; }

        /// <summary>
        /// Path to the IKVM.Runtime assembly to incorporate.
        /// </summary>
        public string Runtime { get; set; }

        public string WarningLevel { get; set; }

        public bool NoParameterReflection { get; set; }

        /// <summary>
        /// Path to the map.xml file to use.
        /// </summary>
        public string Remap { get; set; }

        public override bool Execute()
        {
            if (ResponseFile != null)
                ResponseFile = Path.GetFullPath(ResponseFile);

            if (Output != null)
                Output = Path.GetFullPath(Output);

            if (KeyFile != null)
                KeyFile = Path.GetFullPath(KeyFile);

            if (References != null)
                foreach (var reference in References)
                    if (reference.ItemSpec != null)
                        reference.ItemSpec = Path.GetFullPath(reference.ItemSpec);

            if (Lib != null)
                foreach (var lib in Lib)
                    if (lib.ItemSpec != null)
                        lib.ItemSpec = Path.GetFullPath(lib.ItemSpec);

            if (Resources != null)
                foreach (var i in Resources)
                    if (i.ItemSpec != null)
                        i.ItemSpec = Path.GetFullPath(i.ItemSpec);

            if (ExternalResources != null)
                foreach (var i in ExternalResources)
                    if (i.ItemSpec != null)
                        i.ItemSpec = Path.GetFullPath(i.ItemSpec);

            if (AssemblyAttributes != null)
                foreach (var i in AssemblyAttributes)
                    if (i.ItemSpec != null)
                        i.ItemSpec = Path.GetFullPath(i.ItemSpec);

            if (Runtime != null)
                Runtime = Path.GetFullPath(Runtime);

            if (Remap != null)
                Remap = Path.GetFullPath(Remap);

            if (Input != null)
                foreach (var i in Input)
                    if (i.ItemSpec != null)
                        i.ItemSpec = Path.GetFullPath(i.ItemSpec);

            if (Recurse != null)
                foreach (var i in Recurse)
                    if (i.ItemSpec != null)
                        i.ItemSpec = Path.GetFullPath(i.ItemSpec);

            if (Exclude != null)
                Exclude = Path.GetFullPath(Exclude);

            if (Win32Icon != null)
                Win32Icon = Path.GetFullPath(Win32Icon);

            if (Win32Manifest != null)
                Win32Manifest = Path.GetFullPath(Win32Manifest);

            return base.Execute();
        }

        protected override async Task<bool> ExecuteAsync(IkvmToolTaskDiagnosticWriter writer, CancellationToken cancellationToken)
        {
            var options = new IkvmImporterOptions();
            options.ResponseFile = ResponseFile;
            options.Output = Output;
            options.Assembly = Assembly;
            options.Version = Version;

            options.Target = Target?.ToLowerInvariant() switch
            {
                null => null,
                "library" => IkvmImporterTarget.Library,
                "exe" => IkvmImporterTarget.Exe,
                "winexe" => IkvmImporterTarget.WinExe,
                "module" => IkvmImporterTarget.Module,
                _ => throw new NotImplementedException(),
            };

            options.Platform = Platform?.ToLowerInvariant() switch
            {
                null => null,
                "anycpu" => IkvmImporterPlatform.AnyCPU,
                "anycpu32bitpreferred" => IkvmImporterPlatform.AnyCPU32BitPreferred,
                "x86" => IkvmImporterPlatform.X86,
                "x64" => IkvmImporterPlatform.X64,
                "arm" => IkvmImporterPlatform.ARM,
                "arm64" => IkvmImporterPlatform.ARM64,
                _ => throw new NotImplementedException(),
            };

            options.KeyFile = KeyFile;
            options.Key = Key;
            options.DelaySign = DelaySign;

            if (References != null)
                foreach (var reference in References)
                    if (options.References.Contains(reference.ItemSpec) == false)
                        options.References.Add(reference.ItemSpec);

            if (Recurse != null)
                foreach (var recurse in Recurse)
                    options.Recurse.Add(recurse.ItemSpec);

            options.Exclude = Exclude;
            options.FileVersion = FileVersion;
            options.Win32Icon = Win32Icon;
            options.Win32Manifest = Win32Manifest;

            if (Resources is not null)
                foreach (var resource in Resources)
                    options.Resources.Add(new IkvmImporterResourceItem(resource.ItemSpec, resource.GetMetadata("ResourcePath")));

            if (ExternalResources is not null)
                foreach (var resource in ExternalResources)
                    options.ExternalResources.Add(new IkvmImporterExternalResourceItem(resource.ItemSpec, resource.GetMetadata("ResourcePath")));

            options.CompressResources = CompressResources;

            options.Debug = Debug?.ToLower() switch
            {
                "none" or "" or null => IkvmImporterDebugMode.None,
                "portable" => IkvmImporterDebugMode.Portable,
                "full" or "pdbonly" => IkvmImporterDebugMode.Full,
                "embedded" => IkvmImporterDebugMode.Embedded,
                _ => throw new NotImplementedException($"Unknown Debug option '{Debug}'.")
            };

            options.NoAutoSerialization = NoAutoSerialization;
            options.NoGlobbing = NoGlobbing;
            options.NoJNI = NoJNI;
            options.OptFields = OptFields;
            options.RemoveAssertions = RemoveAssertions;
            options.StrictFinalFieldSemantics = StrictFinalFieldSemantics;

            if (NoWarn != null)
                foreach (var i in NoWarn.Split(';'))
                    options.NoWarn.Add(i);

            options.WarnAsError = WarnAsError;

            if (WarnAsErrorWarnings != null)
                foreach (var i in WarnAsErrorWarnings.Split(';'))
                    options.WarnAsErrorWarnings.Add(i);

            options.WriteSuppressWarningsFile = WriteSuppressWarningsFile;
            options.Main = Main;
            options.SrcPath = SrcPath;
            options.Apartment = Apartment;

            if (SetProperties != null)
                foreach (var p in SetProperties.Split(new[] { ';' }).Select(i => i.Split(new[] { '=' }, 2)))
                    options.SetProperties[p[0]] = p.Length == 2 ? p[1] : "";

            options.NoStackTraceInfo = NoStackTraceInfo;

            if (XTrace != null)
                foreach (var i in XTrace.Split(';'))
                    options.XTrace.Add(i);

            if (XMethodTrace != null)
                foreach (var i in XMethodTrace.Split(';'))
                    options.XMethodTrace.Add(i);

            if (PrivatePackages != null)
                foreach (var i in PrivatePackages.Split(';'))
                    options.PrivatePackages.Add(i);

            options.ClassLoader = ClassLoader;
            options.SharedClassLoader = SharedClassLoader;
            options.BaseAddress = BaseAddress;
            options.FileAlign = FileAlign;
            options.NoPeerCrossReference = NoPeerCrossReference;
            options.NoStdLib = NoStdLib;

            if (Lib != null)
                foreach (var i in Lib)
                    options.Lib.Add(i.ItemSpec);

            options.HighEntropyVA = HighEntropyVA;
            options.Static = Static;

            if (AssemblyAttributes != null)
                foreach (var i in AssemblyAttributes)
                    options.AssemblyAttributes.Add(i.ItemSpec);

            options.Runtime = Runtime;

            if (options.WarningLevel != null)
                options.WarningLevel = int.Parse(WarningLevel);

            options.NoParameterReflection = NoParameterReflection;
            options.Remap = Remap;
            options.NoLogo = true;

            if (Input != null)
                foreach (var i in Input)
                    options.Input.Add(i.ItemSpec);

            // kick off the launcher with the configured options
            return await new IkvmImporterLauncher(ToolPath, writer).ExecuteAsync(options, cancellationToken) == 0;
        }

    }

}
