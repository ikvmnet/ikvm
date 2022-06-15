using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using CliWrap;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace IKVM.MSBuild.Tasks
{

    /// <summary>
    /// Executes the IKVM compiler.
    /// </summary>
    public class IkvmCompiler : Task
    {

        /// <summary>
        /// Path to the IKVMC executable file.
        /// </summary>
        [Required]
        public string ToolPath { get; set; }

        /// <summary>
        /// Whether we should execute IKVMC using 'dotnet exec'.
        /// </summary>
        [Required]
        public bool UseDotNetExec { get; set; }

        /// <summary>
        /// Number of milliseconds to wait for the command to execute.
        /// </summary>
        public int Timeout { get; set; } = System.Threading.Timeout.Infinite;

        /// <summary>
        /// Optional path to the response file to generate. If specified, the response file is not cleaned up.
        /// </summary>
        public string ResponseFile { get; set; }

        [Required]
        public ITaskItem[] Input { get; set; }

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

        public int? WarningLevel { get; set; }

        public string NoParameterReflection { get; set; }

        public override bool Execute()
        {
            using var w = new StringWriter();

            if (Output is not null)
                w.WriteLine($"-out:{Output}");

            if (Assembly is not null)
                w.WriteLine($"-assembly:{Assembly}");

            if (Version is not null)
                w.WriteLine($"-version:{Version}");

            if (Target is not null)
                w.WriteLine($"-target:{Target}");

            if (Platform is not null)
                w.WriteLine($"-platform:{Platform}");

            if (KeyFile is not null)
                w.WriteLine($"-keyfile:{KeyFile}");

            if (Key is not null)
                w.WriteLine($"-key:{Key}");

            if (DelaySign)
                w.WriteLine("-delay");

            if (References is not null)
                foreach (var reference in References)
                    w.WriteLine($"-reference:{reference.ItemSpec}");

            if (Recurse is not null)
                foreach (var recurse in Recurse)
                    w.WriteLine($"-recurse:{recurse.ItemSpec}");

            if (Exclude is not null)
                w.WriteLine($"-exclude:{Exclude}");

            if (FileVersion is not null)
                w.WriteLine($"-fileversion:{FileVersion}");

            if (Win32Icon is not null)
                w.WriteLine($"-win32icon:{Win32Icon}");

            if (Win32Manifest is not null)
                w.WriteLine($"-win32manifest:{Win32Manifest}");

            if (Resources is not null)
                foreach (var resource in Resources)
                    w.WriteLine($"-resource:{resource.GetMetadata("ResourcePath")}={resource.ItemSpec}");

            if (ExternalResources is not null)
                foreach (var resource in ExternalResources)
                    w.WriteLine($"-externalresource:{resource.GetMetadata("ResourcePath")}={resource.ItemSpec}");

            if (CompressResources)
                w.WriteLine("-compressresources");

            if (Debug)
                w.WriteLine("-debug");

            if (NoAutoSerialization)
                w.WriteLine("-noautoserialization");

            if (NoGlobbing)
                w.WriteLine("-noglobbing");

            if (NoJNI)
                w.WriteLine("-nojni");

            if (OptFields)
                w.WriteLine("-opt:fields");

            if (RemoveAssertions)
                w.WriteLine("-removeassertions");

            if (StrictFinalFieldSemantics)
                w.WriteLine("-strictfinalfieldsemantics");

            if (NoWarn is not null)
                foreach (var i in NoWarn.Split(';'))
                    w.WriteLine($"-nowarn:{i}");

            if (WarnAsError)
                w.WriteLine("-warnaserror");

            if (WarnAsErrorWarnings is not null)
                foreach (var i in WarnAsErrorWarnings.Split(';'))
                    w.WriteLine($"-warnaserror:{i}");

            if (WriteSuppressWarningsFile is not null)
                w.WriteLine($"-writesupresswarningsfile:{WriteSuppressWarningsFile}");

            if (Main is not null)
                w.WriteLine($"-main:{Main}");

            if (SrcPath is not null)
                w.WriteLine($"-srcpath:{SrcPath}");

            if (Apartment is not null)
                w.WriteLine($"-apartment:{Apartment}");

            if (SetProperties is not null)
                foreach (var p in SetProperties.Split(new[] { ';' }, 2))
                    if (p.Length == 2)
                        w.WriteLine($"-D{p[0]}={p[1]}");

            if (NoStackTraceInfo)
                w.WriteLine("-nostacktraceinfo");

            if (XTrace is not null)
                foreach (var i in XTrace.Split(';'))
                    w.WriteLine($"-Xtrace:{i}");

            if (XMethodTrace is not null)
                foreach (var i in XMethodTrace.Split(';'))
                    w.WriteLine($"-Xmethodtrace:{i}");

            if (PrivatePackages is not null)
                foreach (var i in PrivatePackages.Split(';'))
                    w.WriteLine($"-privatepackage:{i}");

            if (ClassLoader is not null)
                w.WriteLine($"-classloader:{ClassLoader}");

            if (SharedClassLoader)
                w.WriteLine("-sharedclassloader");

            if (BaseAddress is not null)
                w.WriteLine($"-baseaddress:{BaseAddress}");

            if (FileAlign is not null)
                w.WriteLine($"-filealign:{FileAlign}");

            if (NoPeerCrossReference)
                w.WriteLine("-nopeercrossreference");

            if (NoStdLib)
                w.WriteLine("-nostdlib");

            if (Lib is not null)
                foreach (var i in Lib)
                    w.WriteLine($"-lib:{i.ItemSpec}");

            if (HighEntropyVA)
                w.WriteLine("-highentropyva");

            if (Static)
                w.WriteLine("-static");

            if (AssemblyAttributes is not null)
                foreach (var i in Lib)
                    w.WriteLine($"-assemblyattributes:{i.ItemSpec}");

            if (Runtime is not null)
                w.WriteLine($"-runtime:{Runtime}");

            if (WarningLevel is not null)
                w.WriteLine($"-w{WarningLevel}");

            if (NoParameterReflection is not null)
                w.WriteLine($"-noparameterreflection");

            if (Input != null)
                foreach (var i in Input)
                    w.WriteLine(i.ItemSpec);

            // path to the temporary response file
            var response = (string)null;
            var cts = new CancellationTokenSource();

            try
            {
                // create response file
                response = ResponseFile ?? Path.GetTempFileName();
                File.WriteAllText(response, w.ToString());

                // we use a different path and args set based on which version we're running
                var cli = UseDotNetExec ? Cli.Wrap("dotnet") : Cli.Wrap(ToolPath);
                var args = new List<string>();
                if (UseDotNetExec)
                {
                    args.Add("exec");
                    args.Add(ToolPath);
                }

                // execute the contents of the response file
                args.Add($"@{response}");
                cli = cli.WithArguments(args);
                cli = cli.WithValidation(CommandResultValidation.None);

                // send output to MSBuild
                cli = cli.WithStandardErrorPipe(PipeTarget.ToDelegate(i => Log.LogMessage(i)));
                cli = cli.WithStandardOutputPipe(PipeTarget.ToDelegate(i => Log.LogMessage(i)));

                // combine manual cancellation with timeout
                var ctk = cts.Token;
                if (Timeout != System.Threading.Timeout.Infinite)
                    ctk = CancellationTokenSource.CreateLinkedTokenSource(ctk, new CancellationTokenSource(Timeout).Token).Token;

                // execute command
                Log.LogCommandLine(cli.ToString());
                var run = cli.ExecuteAsync(ctk);

                // yield and wait for the task to complete
                BuildEngine3.Yield();
                var rsl = run.GetAwaiter().GetResult();
                BuildEngine3.Reacquire();

                // collect final results
                if (rsl == null)
                    throw new NullReferenceException();

                // check that we exited successfully
                return rsl.ExitCode == 0;
            }
            finally
            {
                // cancel the execution if it is still running
                if (cts != null)
                    cts.Cancel();

                // clean up response file
                if (ResponseFile == null && response != null && File.Exists(response))
                    File.Delete(response);
            }
        }

    }

}
