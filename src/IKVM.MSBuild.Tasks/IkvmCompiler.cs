using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using IKVM.Tools.Runner;
using IKVM.Tools.Runner.Compiler;

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

        public string Assembly { get; set; }

        public string Version { get; set; }

        public string Target { get; set; }

        public string Platform { get; set; }

        public string KeyFile { get; set; }

        public string Key { get; set; }

        public bool DelaySign { get; set; }

        public ITaskItem[] References { get; set; }

        public ITaskItem[] Recurse { get; set; }

        public string Exclude { get; set; }

        public string FileVersion { get; set; }

        public string Win32Icon { get; set; }

        public string Win32Manifest { get; set; }

        public ITaskItem[] Resources { get; set; }

        public ITaskItem[] ExternalResources { get; set; }

        public bool CompressResources { get; set; }

        public bool Debug { get; set; }

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

        public ITaskItem[] AssemblyAttributes { get; set; }

        public string Runtime { get; set; }

        public string JNI { get; set; }

        public string WarningLevel { get; set; }

        public bool NoParameterReflection { get; set; }

        public string Remap { get; set; }

        protected override async Task<bool> ExecuteAsync(IkvmToolTaskDiagnosticWriter writer, CancellationToken cancellationToken)
        {
            if (Debug && RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false)
            {
                Log.LogWarning("Emitting debug symbols from ikvmc is not supported on platforms other than Windows. Continuing without.");
                Debug = false;
            }

            var options = new IkvmCompilerOptions();
            options.ResponseFile = ResponseFile;
            options.Output = Output;
            options.Assembly = Assembly;
            options.Version = Version;

            options.Target = Target?.ToLowerInvariant() switch
            {
                null => null,
                "library" => IkvmCompilerTarget.Library,
                "exe" => IkvmCompilerTarget.Exe,
                "winexe" => IkvmCompilerTarget.WinExe,
                "module" => IkvmCompilerTarget.Module,
                _ => throw new NotImplementedException(),
            };

            options.Platform = Platform?.ToLowerInvariant() switch
            {
                null => null,
                "anycpu" => IkvmCompilerPlatform.AnyCPU,
                "anycpu32bitpreferred" => IkvmCompilerPlatform.AnyCPU32BitPreferred,
                "x86" => IkvmCompilerPlatform.X86,
                "x64" => IkvmCompilerPlatform.X64,
                "arm" => IkvmCompilerPlatform.ARM,
                "arm64" => IkvmCompilerPlatform.ARM64,
                _ => throw new NotImplementedException(),
            };

            options.KeyFile = KeyFile;
            options.Key = Key;
            options.DelaySign = DelaySign;

            if (References is not null)
                foreach (var reference in References)
                    if (options.References.Contains(reference.ItemSpec) == false)
                        options.References.Add(reference.ItemSpec);

            if (Recurse is not null)
                foreach (var recurse in Recurse)
                    options.Recurse.Add(recurse.ItemSpec);

            options.Exclude = Exclude;
            options.FileVersion = FileVersion;
            options.Win32Icon = Win32Icon;
            options.Win32Manifest = Win32Manifest;

            if (Resources is not null)
                foreach (var resource in Resources)
                    options.Resources.Add(new IkvmCompilerResourceItem(resource.ItemSpec, resource.GetMetadata("ResourcePath")));

            if (ExternalResources is not null)
                foreach (var resource in ExternalResources)
                    options.ExternalResources.Add(new IkvmCompilerExternalResourceItem(resource.ItemSpec, resource.GetMetadata("ResourcePath")));

            options.CompressResources = CompressResources;
            options.Debug = Debug;
            options.NoAutoSerialization = NoAutoSerialization;
            options.NoGlobbing = NoGlobbing;
            options.NoJNI = NoJNI;
            options.OptFields = OptFields;
            options.RemoveAssertions = RemoveAssertions;
            options.StrictFinalFieldSemantics = StrictFinalFieldSemantics;

            if (NoWarn is not null)
                foreach (var i in NoWarn.Split(';'))
                    options.NoWarn.Add(i);

            options.WarnAsError = WarnAsError;

            if (WarnAsErrorWarnings is not null)
                foreach (var i in WarnAsErrorWarnings.Split(';'))
                    options.WarnAsErrorWarnings.Add(i);

            options.WriteSuppressWarningsFile = WriteSuppressWarningsFile;
            options.Main = Main;
            options.SrcPath = SrcPath;
            options.Apartment = Apartment;

            if (SetProperties is not null)
                foreach (var p in SetProperties.Split(new[] { ';' }).Select(i => i.Split(new[] { '=' }, 2)))
                    options.SetProperties[p[0]] = p.Length == 2 ? p[1] : "";

            options.NoStackTraceInfo = NoStackTraceInfo;

            if (XTrace is not null)
                foreach (var i in XTrace.Split(';'))
                    options.XTrace.Add(i);

            if (XMethodTrace is not null)
                foreach (var i in XMethodTrace.Split(';'))
                    options.XMethodTrace.Add(i);

            if (PrivatePackages is not null)
                foreach (var i in PrivatePackages.Split(';'))
                    options.PrivatePackages.Add(i);

            options.ClassLoader = ClassLoader;
            options.SharedClassLoader = SharedClassLoader;
            options.BaseAddress = BaseAddress;
            options.FileAlign = FileAlign;
            options.NoPeerCrossReference = NoPeerCrossReference;
            options.NoStdLib = NoStdLib;

            if (Lib is not null)
                foreach (var i in Lib)
                    options.Lib.Add(i.ItemSpec);

            options.HighEntropyVA = HighEntropyVA;
            options.Static = Static;

            if (AssemblyAttributes is not null)
                foreach (var i in AssemblyAttributes)
                    options.AssemblyAttributes.Add(i.ItemSpec);

            options.Runtime = Runtime;
            options.JNI = JNI;

            if (options.WarningLevel is not null)
                options.WarningLevel = int.Parse(WarningLevel);

            options.NoParameterReflection = NoParameterReflection;
            options.Remap = Remap;

            if (Input != null)
                foreach (var i in Input)
                    options.Input.Add(i.ItemSpec);

            // kick off the launcher with the configured options
            return await new IkvmCompilerLauncher(ToolPath, writer).ExecuteAsync(options, cancellationToken) == 0;
        }

    }

}
