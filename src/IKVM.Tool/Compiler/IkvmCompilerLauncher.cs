using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using CliWrap;

using IKVM.Tool.Internal;

namespace IKVM.Tool.Compiler
{

    /// <summary>
    /// Provides methods to launch the IKVM compiler.
    /// </summary>
    public class IkvmCompilerLauncher
    {

        readonly string toolPath;
        readonly IIkvmToolDiagnosticEventListener listener;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="toolPath"></param>
        /// <param name="listener"></param>
        public IkvmCompilerLauncher(string toolPath, IIkvmToolDiagnosticEventListener listener)
        {
            this.toolPath = toolPath ?? throw new ArgumentNullException(nameof(toolPath));
            this.listener = listener;

            if (Directory.Exists(toolPath) == false)
                throw new DirectoryNotFoundException($"Could not locate tool path {toolPath}.");
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="listener"></param>
        public IkvmCompilerLauncher(IIkvmToolDiagnosticEventListener listener) :
            this(Path.Combine(Path.GetDirectoryName(typeof(IkvmCompilerLauncher).Assembly.Location), "ikvmc"), listener)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="toolPath"></param>
        public IkvmCompilerLauncher(string toolPath) :
            this(toolPath, new IkvmToolDelegateDiagnosticListener(evt => Task.CompletedTask))
        {

        }

        /// <summary>
        /// Logs an event if a listener is provided.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        Task LogEvent(IkvmToolDiagnosticEventLevel level, string message, params object[] args)
        {
            return listener?.ReceiveAsync(new IkvmToolDiagnosticEvent(level, message, args));
        }

        /// <summary>
        /// Gets the path to the tool directory for the given environment.
        /// </summary>
        /// <param name="framework"></param>
        /// <param name="platform"></param>
        /// <param name="architecture"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public string GetToolDir(IkvmCompilerTargetFramework framework, OSPlatform platform, Architecture architecture)
        {
            // determine the TFM of the tool to be executed
            var tfm = framework switch
            {
                IkvmCompilerTargetFramework.NetFramework => "net461",
                IkvmCompilerTargetFramework.NetCore => "netcoreapp3.1",
                _ => throw new NotImplementedException(),
            };

            // determine the RID of the tool to be executed
            var rid = architecture switch
            {
                Architecture.X86 => framework switch
                {
                    IkvmCompilerTargetFramework.NetFramework => "any",
                    IkvmCompilerTargetFramework.NetCore when platform == OSPlatform.Windows => "win7-x86",
                    IkvmCompilerTargetFramework.NetCore when platform == OSPlatform.Linux => "linux-x86",
                    _ => throw new NotImplementedException(),
                },
                Architecture.X64 => framework switch
                {
                    IkvmCompilerTargetFramework.NetFramework => "any",
                    IkvmCompilerTargetFramework.NetCore when platform == OSPlatform.Windows => "win7-x64",
                    IkvmCompilerTargetFramework.NetCore when platform == OSPlatform.Linux => "linux-x64",
                    _ => throw new NotImplementedException(),
                },
                _ => throw new NotImplementedException(),
            };

            // we use a different path and args set based on which version we're running
            return Path.Combine(toolPath, tfm, rid);
        }

        /// <summary>
        /// Gets the path to executable for the given environment.
        /// </summary>
        /// <param name="framework"></param>
        /// <param name="platform"></param>
        /// <param name="architecture"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public string GetToolExe(IkvmCompilerTargetFramework framework, OSPlatform platform, Architecture architecture)
        {
            return Path.Combine(GetToolDir(framework, platform, architecture), platform == OSPlatform.Windows ? "ikvmc.exe" : "ikvmc");
        }

        /// <summary>
        /// Gets the path to executable for the given environment.
        /// </summary>
        /// <param name="framework"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public string GetToolExe(IkvmCompilerTargetFramework framework)
        {
            return GetToolExe(framework, GetOSPlatform(), RuntimeInformation.OSArchitecture);
        }

        /// <summary>
        /// Gets the path to the reference assemblies for the given environment.
        /// </summary>
        /// <param name="framework"></param>
        /// <param name="platform"></param>
        /// <param name="architecture"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public string GetReferenceAssemblyDirectory(IkvmCompilerTargetFramework framework, OSPlatform platform, Architecture architecture)
        {
            return Path.Combine(GetToolDir(framework, platform, architecture), "refs");
        }

        /// <summary>
        /// Gets the path to the reference assemblies for the given environment.
        /// </summary>
        /// <param name="framework"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public string GetReferenceAssemblyDirectory(IkvmCompilerTargetFramework framework)
        {
            return GetReferenceAssemblyDirectory(framework, GetOSPlatform(), RuntimeInformation.OSArchitecture);
        }

        /// <summary>
        /// Gets the path to the runtime assembly for the given environment.
        /// </summary>
        /// <param name="framework"></param>
        /// <param name="platform"></param>
        /// <param name="architecture"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public string GetRuntimeAssemblyFile(IkvmCompilerTargetFramework framework, OSPlatform platform, Architecture architecture)
        {
            return Path.Combine(GetToolDir(framework, platform, architecture), "IKVM.Runtime.dll");
        }

        /// <summary>
        /// Gets the path to the runtime assembly for the given environment.
        /// </summary>
        /// <param name="framework"></param>
        /// <returns></returns>
        public string GetRuntimeAssemblyFile(IkvmCompilerTargetFramework framework)
        {
            return GetRuntimeAssemblyFile(framework, GetOSPlatform(), RuntimeInformation.OSArchitecture);
        }

        /// <summary>
        /// Gets the path to the Java base assembly for the given environment.
        /// </summary>
        /// <param name="framework"></param>
        /// <param name="platform"></param>
        /// <param name="architecture"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public string GetJavaBaseAssemblyFile(IkvmCompilerTargetFramework framework, OSPlatform platform, Architecture architecture)
        {
            return Path.Combine(GetToolDir(framework, platform, architecture), "IKVM.Java.dll");
        }

        /// <summary>
        /// Gets the path to the Java base assembly for the given environment.
        /// </summary>
        /// <param name="framework"></param>
        /// <returns></returns>
        public string GetJavaBaseAssemblyFile(IkvmCompilerTargetFramework framework)
        {
            return GetJavaBaseAssemblyFile(framework, GetOSPlatform(), RuntimeInformation.OSArchitecture);
        }

        /// <summary>
        /// Gets the current OS platform.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        OSPlatform GetOSPlatform()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return OSPlatform.Windows;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return OSPlatform.Linux;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return OSPlatform.OSX;

            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Executes the compiler.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public async Task<int> ExecuteAsync(IkvmCompilerOptions options, CancellationToken cancellationToken = default)
        {
            if (options is null)
                throw new ArgumentNullException(nameof(options));

            using var w = new StringWriter();

            if (options.Output is not null)
                w.WriteLine($"-out:{options.Output}");

            if (options.Assembly is not null)
                w.WriteLine($"-assembly:{options.Assembly}");

            if (options.Version is not null)
                w.WriteLine($"-version:{options.Version}");

            if (options.Target is not null)
            {
                switch (options.Target)
                {
                    case IkvmCompilerTarget.Library:
                        w.WriteLine($"-target:library");
                        break;
                    case IkvmCompilerTarget.Exe:
                        w.WriteLine($"-target:exe");
                        break;
                    case IkvmCompilerTarget.WinExe:
                        w.WriteLine($"-target:winexe");
                        break;
                    case IkvmCompilerTarget.Module:
                        w.WriteLine($"-target:module");
                        break;
                }
            }

            if (options.Platform is not null)
                w.WriteLine($"-platform:{options.Platform}");

            if (options.KeyFile is not null)
                w.WriteLine($"-keyfile:{options.KeyFile}");

            if (options.Key is not null)
                w.WriteLine($"-key:{options.Key}");

            if (options.DelaySign)
                w.WriteLine("-delay");

            if (options.References is not null)
                foreach (var reference in options.References)
                    w.WriteLine($"-reference:{reference}");

            if (options.Recurse is not null)
                foreach (var recurse in options.Recurse)
                    w.WriteLine($"-recurse:{recurse}");

            if (options.Exclude is not null)
                w.WriteLine($"-exclude:{options.Exclude}");

            if (options.FileVersion is not null)
                w.WriteLine($"-fileversion:{options.FileVersion}");

            if (options.Win32Icon is not null)
                w.WriteLine($"-win32icon:{options.Win32Icon}");

            if (options.Win32Manifest is not null)
                w.WriteLine($"-win32manifest:{options.Win32Manifest}");

            if (options.Resources is not null)
                foreach (var resource in options.Resources)
                    w.WriteLine($"-resource:{resource.ResourcePath}={resource.FilePath}");

            if (options.ExternalResources is not null)
                foreach (var resource in options.ExternalResources)
                    w.WriteLine($"-externalresource:{resource.ResourcePath}={resource.FilePath}");

            if (options.CompressResources)
                w.WriteLine("-compressresources");

            if (options.Debug)
                w.WriteLine("-debug");

            if (options.NoAutoSerialization)
                w.WriteLine("-noautoserialization");

            if (options.NoGlobbing)
                w.WriteLine("-noglobbing");

            if (options.NoJNI)
                w.WriteLine("-nojni");

            if (options.OptFields)
                w.WriteLine("-opt:fields");

            if (options.RemoveAssertions)
                w.WriteLine("-removeassertions");

            if (options.StrictFinalFieldSemantics)
                w.WriteLine("-strictfinalfieldsemantics");

            if (options.NoWarn is not null)
                foreach (var i in options.NoWarn)
                    w.WriteLine($"-nowarn:{i}");

            if (options.WarnAsError)
                w.WriteLine("-warnaserror");

            if (options.WarnAsErrorWarnings is not null)
                foreach (var i in options.WarnAsErrorWarnings)
                    w.WriteLine($"-warnaserror:{i}");

            if (options.WriteSuppressWarningsFile is not null)
                w.WriteLine($"-writesupresswarningsfile:{options.WriteSuppressWarningsFile}");

            if (options.Main is not null)
                w.WriteLine($"-main:{options.Main}");

            if (options.SrcPath is not null)
                w.WriteLine($"-srcpath:{options.SrcPath}");

            if (options.Apartment is not null)
                w.WriteLine($"-apartment:{options.Apartment}");

            if (options.SetProperties is not null)
                foreach (var kvp in options.SetProperties)
                    w.WriteLine($"-D{kvp.Key}={kvp.Value}");

            if (options.NoStackTraceInfo)
                w.WriteLine("-nostacktraceinfo");

            if (options.XTrace is not null)
                foreach (var i in options.XTrace)
                    w.WriteLine($"-Xtrace:{i}");

            if (options.XMethodTrace is not null)
                foreach (var i in options.XMethodTrace)
                    w.WriteLine($"-Xmethodtrace:{i}");

            if (options.PrivatePackages is not null)
                foreach (var i in options.PrivatePackages)
                    w.WriteLine($"-privatepackage:{i}");

            if (options.ClassLoader is not null)
                w.WriteLine($"-classloader:{options.ClassLoader}");

            if (options.SharedClassLoader)
                w.WriteLine("-sharedclassloader");

            if (options.BaseAddress is not null)
                w.WriteLine($"-baseaddress:{options.BaseAddress}");

            if (options.FileAlign is not null)
                w.WriteLine($"-filealign:{options.FileAlign}");

            if (options.NoPeerCrossReference)
                w.WriteLine("-nopeercrossreference");

            if (options.NoStdLib)
                w.WriteLine("-nostdlib");

            if (options.Lib is not null)
                foreach (var i in options.Lib)
                    w.WriteLine($"-lib:{i}");

            if (options.HighEntropyVA)
                w.WriteLine("-highentropyva");

            if (options.Static)
                w.WriteLine("-static");

            if (options.AssemblyAttributes is not null)
                foreach (var i in options.AssemblyAttributes)
                    w.WriteLine($"-assemblyattributes:{i}");

            if (options.Runtime is not null)
                w.WriteLine($"-runtime:{options.Runtime}");

            if (options.WarningLevel is not null)
                w.WriteLine($"-w{options.WarningLevel}");

            if (options.NoParameterReflection)
                w.WriteLine($"-noparameterreflection");

            if (options.Input != null)
                foreach (var i in options.Input)
                    w.WriteLine(i);

            // path to the temporary response file
            var response = (string)null;
            var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, new CancellationToken());

            try
            {
                // create response file
                response = options.ResponseFile ?? Path.GetTempFileName();
                File.WriteAllText(response, w.ToString());

                // locate EXE file
                var exe = GetToolExe(options.TargetFramework);
                if (File.Exists(exe) == false)
                    throw new FileNotFoundException($"Could not locate tool at {exe}.");

                // check for supported platform
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false && options.TargetFramework == IkvmCompilerTargetFramework.NetFramework)
                    throw new IkvmToolException("IKVM generation for .NET Framework assemblies is not supported on Linux operating system.");

                // if we're running on Linux, we might need to set the execute bit on the file,
                // since the NuGet package is built on Windows
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    try
                    {
                        var psx = Mono.Unix.UnixFileSystemInfo.GetFileSystemEntry(exe);
                        if (psx.FileAccessPermissions.HasFlag(Mono.Unix.FileAccessPermissions.UserExecute) == false)
                            psx.FileAccessPermissions |= Mono.Unix.FileAccessPermissions.UserExecute;
                    }
                    catch (Exception e)
                    {
                        throw new IkvmToolException($"Could not set user executable bit on '{exe}'.", e);
                    }
                }

                // we use a different path and args set based on which version we're running
                var cli = Cli.Wrap(exe);
                var args = new List<string>();

                // execute the contents of the response file
                args.Add($"@{response}");
                cli = cli.WithArguments(args);
                cli = cli.WithValidation(CommandResultValidation.None);

                // send output to MSBuild
                cli = cli.WithStandardErrorPipe(PipeTarget.ToDelegate(i => LogEvent(IkvmToolDiagnosticEventLevel.Error, i)));
                cli = cli.WithStandardOutputPipe(PipeTarget.ToDelegate(i => LogEvent(IkvmToolDiagnosticEventLevel.Debug, i)));

                // combine manual cancellation with timeout
                var ctk = cts.Token;
                if (options.Timeout != Timeout.Infinite)
                    ctk = CancellationTokenSource.CreateLinkedTokenSource(ctk, new CancellationTokenSource(options.Timeout).Token).Token;

                // execute command
                var pid = cli.ExecuteAsync(ctk);

                // windows provides special support for killing subprocesses on termination of parent
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    WindowsChildProcessTracker.AddProcess(Process.GetProcessById(pid.ProcessId));

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
