using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using CliWrap;

using IKVM.Tools.Runner.Diagnostics;
using IKVM.Tools.Runner.Internal;

namespace IKVM.Tools.Runner.Importer
{

    /// <summary>
    /// Provides methods to launch the IKVM compiler.
    /// </summary>
    public class IkvmImporterLauncher : IkvmToolLauncher
    {

        static readonly string TOOLNAME = "ikvmc";
        static readonly string TOOLPATH = typeof(IkvmImporterLauncher).Assembly.Location is string s ? Path.GetDirectoryName(s) ?? "" : "";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="toolPath"></param>
        /// <param name="listener"></param>
        public IkvmImporterLauncher(string toolPath, IIkvmToolDiagnosticEventListener listener) :
            base(TOOLNAME, toolPath, listener)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="listener"></param>
        public IkvmImporterLauncher(IIkvmToolDiagnosticEventListener listener) :
            this(TOOLPATH, listener)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="toolPath"></param>
        public IkvmImporterLauncher(string toolPath) :
            this(toolPath, new IkvmToolNullDiagnosticListener())
        {

        }

        /// <summary>
        /// Executes the compiler.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> ExecuteAsync(IkvmImporterOptions options, CancellationToken cancellationToken = default)
        {
            if (options is null)
                throw new ArgumentNullException(nameof(options));

            using var w = new StringWriter();

            if (options.Output is not null)
                w.WriteLine($"-out \"{options.Output}\"");

            if (options.Assembly is not null)
                w.WriteLine($"-assembly \"{options.Assembly}\"");

            if (options.Version is not null)
                w.WriteLine($"-version {options.Version}");

            if (options.Target is not null)
            {
                switch (options.Target)
                {
                    case IkvmImporterTarget.Library:
                        w.WriteLine($"-target library");
                        break;
                    case IkvmImporterTarget.Exe:
                        w.WriteLine($"-target exe");
                        break;
                    case IkvmImporterTarget.WinExe:
                        w.WriteLine($"-target winexe");
                        break;
                    case IkvmImporterTarget.Module:
                        w.WriteLine($"-target module");
                        break;
                }
            }

            if (options.Platform is IkvmImporterPlatform platform)
                w.WriteLine($"-platform {platform.ToString().ToLowerInvariant()}");

            if (options.KeyFile is not null)
                w.WriteLine($"-keyfile \"{options.KeyFile}\"");

            if (options.Key is not null)
                w.WriteLine($"-key {options.Key}");

            if (options.DelaySign)
                w.WriteLine("-delay");

            if (options.References is not null)
                foreach (var reference in options.References)
                    w.WriteLine($"-reference \"{reference}\"");

            if (options.Recurse is not null)
                foreach (var recurse in options.Recurse)
                    w.WriteLine($"-recurse \"{recurse}\"");

            if (options.Exclude is not null)
                w.WriteLine($"-exclude \"{options.Exclude}\"");

            if (options.FileVersion is not null)
                w.WriteLine($"-fileversion {options.FileVersion}");

            if (options.Win32Icon is not null)
                w.WriteLine($"-win32icon {options.Win32Icon}");

            if (options.Win32Manifest is not null)
                w.WriteLine($"-win32manifest {options.Win32Manifest}");

            if (options.Resources is not null)
                foreach (var resource in options.Resources)
                    w.WriteLine($"-resource \"{resource.ResourcePath}={resource.FilePath}\"");

            if (options.ExternalResources is not null)
                foreach (var resource in options.ExternalResources)
                    w.WriteLine($"-externalresource \"{resource.ResourcePath}={resource.FilePath}\"");

            if (options.CompressResources)
                w.WriteLine("-compressresources");

            if (options.Debug == IkvmImporterDebugMode.Portable)
                w.WriteLine("-debug portable");
            else if (options.Debug == IkvmImporterDebugMode.Embedded)
                w.WriteLine("-debug embedded");

            if (options.NoAutoSerialization)
                w.WriteLine("-noautoserialization");

            if (options.NoGlobbing)
                w.WriteLine("-noglobbing");

            if (options.NoJNI)
                w.WriteLine("-nojni");

            if (options.Optimize)
                w.WriteLine("-optimize");

            if (options.OptFields)
                w.WriteLine("-opt:fields");

            if (options.RemoveAssertions)
                w.WriteLine("-removeassertions");

            if (options.StrictFinalFieldSemantics)
                w.WriteLine("-strictfinalfieldsemantics");

            if (options.NoWarn is not null)
            {
                if (options.NoWarn.Count == 0)
                    w.WriteLine("-nowarn");
                else
                    w.WriteLine($"-nowarn {string.Join(",", options.NoWarn)}");
            }

            if (options.WarningsAsErrors is not null)
            {
                if (options.WarningsAsErrors.Count == 0)
                    w.WriteLine("-warnaserror");
                else
                    w.WriteLine($"-warnaserror {string.Join(",", options.WarningsAsErrors)}");
            }

            if (options.Main is not null)
                w.WriteLine($"-main {options.Main}");

            if (options.SrcPath is not null)
                w.WriteLine($"-srcpath {options.SrcPath}");

            if (options.Apartment is not null)
                w.WriteLine($"-apartment {options.Apartment}");

            if (options.SetProperties is not null)
                foreach (var kvp in options.SetProperties)
                    w.WriteLine($"-D \"{kvp.Key}={kvp.Value}\"");

            if (options.NoStackTraceInfo)
                w.WriteLine("-nostacktraceinfo");

            if (options.PrivatePackages is not null)
                foreach (var i in options.PrivatePackages)
                    w.WriteLine($"-privatepackage {i}");

            if (options.ClassLoader is not null)
                w.WriteLine($"-classloader {options.ClassLoader}");

            if (options.SharedClassLoader)
                w.WriteLine("-sharedclassloader");

            if (options.BaseAddress is not null)
                w.WriteLine($"-baseaddress {options.BaseAddress}");

            if (options.FileAlign is not null)
                w.WriteLine($"-filealign {options.FileAlign}");

            if (options.NoPeerCrossReference)
                w.WriteLine("-nopeercrossreference");

            if (options.NoStdLib)
                w.WriteLine("-nostdlib");

            if (options.Lib is not null)
                foreach (var i in options.Lib)
                    w.WriteLine($"-lib \"{i}\"");

            if (options.HighEntropyVA)
                w.WriteLine("-highentropyva");

            if (options.Static)
                w.WriteLine("-static");

            if (options.AssemblyAttributes is not null)
                foreach (var i in options.AssemblyAttributes)
                    w.WriteLine($"-assemblyattributes \"{i}\"");

            if (options.Runtime is not null)
                w.WriteLine($"-runtime \"{options.Runtime}\"");

            if (options.WarningLevel is not null)
                w.WriteLine($"-w{options.WarningLevel}");

            if (options.NoParameterReflection)
                w.WriteLine($"-noparameterreflection");

            if (options.Remap is not null)
                w.WriteLine($"-remap \"{options.Remap}\"");

            w.WriteLine($"-log json,file=stderr");

            if (options.Input != null)
                foreach (var i in options.Input)
                    w.Write($"\"{i}\"");

            // prepare path to response file
            var response = string.IsNullOrWhiteSpace(options.ResponseFile) == false ? Path.GetFullPath(options.ResponseFile) : Path.GetTempFileName();

            // to cancel the executable
            var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, new CancellationToken());

            // combine manual cancellation with timeout
            var ctk = cts.Token;
            if (options.Timeout != Timeout.Infinite)
                ctk = CancellationTokenSource.CreateLinkedTokenSource(ctk, new CancellationTokenSource(options.Timeout).Token).Token;

            try
            {
                // create response file
                Directory.CreateDirectory(Path.GetDirectoryName(response) ?? throw new InvalidOperationException());
                File.WriteAllText(response, w.ToString());

                // locate EXE file
                string? wrap = null;
                var exe = GetToolExe();
                if (exe is null || File.Exists(exe) == false)
                    throw new FileNotFoundException($"Could not locate tool at '{exe}'.");

                // executing on Unix requires some considerations
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    // tool executables on Unix need to be invoked through Mono
                    if (exe.EndsWith(".exe"))
                    {
                        wrap = "mono";
                    }
                    else
                    {
                        // else we need to ensure executable bit is set

                        try
                        {
#if NET7_0_OR_GREATER
                            var mod = File.GetUnixFileMode(exe);
                            var prm = mod | UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute;
                            if (mod != prm)
                                File.SetUnixFileMode(exe, prm);
#else
                            var psx = Mono.Unix.UnixFileSystemInfo.GetFileSystemEntry(exe);
                            var mod = psx.FileAccessPermissions;
                            var prm = mod | Mono.Unix.FileAccessPermissions.UserExecute | Mono.Unix.FileAccessPermissions.GroupExecute | Mono.Unix.FileAccessPermissions.OtherExecute;
                            if (mod != prm)
                                psx.FileAccessPermissions = prm;
#endif
                        }
                        catch (Exception e)
                        {
                            throw new IkvmToolException($"Could not set user executable bit on '{exe}'.", e);
                        }
                    }
                }

                // assemble list of args
                var args = new List<string>();
                args.Add($"@{response}");

                // configure CLI, with wrapper if required
                Command cli;
                if (wrap != null)
                {
                    cli = Cli.Wrap(wrap);
                    args.Insert(0, exe);
                }
                else
                    cli = Cli.Wrap(exe);

                // set working directory
                cli = cli.WithWorkingDirectory(Environment.CurrentDirectory);

                // set configuration of CLI
                cli = cli.WithArguments(args);
                cli = cli.WithValidation(CommandResultValidation.None);

                // log the command we're about to run
                await LogEventAsync(IkvmToolDiagnosticEventLevel.Trace, "Executing {0} {1}", [cli.TargetFilePath, cli.Arguments], ctk);

                // send output to MSBuild (TODO, replace with binary reading)
                cli = cli.WithStandardErrorPipe(PipeTarget.ToDelegate(l => ParseAndLogEventAsync(l, ctk).AsTask()));

                // execute command
                using var pid = cli.ExecuteAsync(ctk);

                // windows provides special support for killing subprocesses on termination of parent
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    try
                    {
                        if (pid.Task.IsCompleted == false)
                            WindowsChildProcessTracker.AddProcess(Process.GetProcessById(pid.ProcessId));
                    }
                    catch
                    {
                        await LogEventAsync(IkvmToolDiagnosticEventLevel.Error, "Failed to attach child process.", [], ctk);
                    }
                }

                // wait for the execution to finish
                var ret = await pid;

                // check that we exited successfully
                return ret.ExitCode;
            }
            finally
            {
                // cancel the execution if it is still running
                if (cts != null)
                    cts.Cancel();

                // clean up response file
                if (options.ResponseFile == null && response != null && File.Exists(response))
                {
                    try
                    {
                        File.Delete(response);
                    }
                    catch (IOException)
                    {
                        // did our best
                    }
                }
            }
        }

    }

}
