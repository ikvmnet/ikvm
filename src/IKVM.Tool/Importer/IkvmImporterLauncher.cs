using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using CliWrap;

using IKVM.Tool.Internal;

namespace IKVM.Tool.Importer
{

    /// <summary>
    /// Provides methods to launch the IKVM importer.
    /// </summary>
    public class IkvmImporterLauncher
    {

        readonly string toolPath;
        readonly IIkvmToolDiagnosticEventListener listener;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="toolPath"></param>
        /// <param name="listener"></param>
        public IkvmImporterLauncher(string toolPath, IIkvmToolDiagnosticEventListener listener)
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
        public IkvmImporterLauncher(IIkvmToolDiagnosticEventListener listener) :
            this(Path.Combine(Path.GetDirectoryName(typeof(IkvmImporterLauncher).Assembly.Location), "ikvmstub"), listener)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="toolPath"></param>
        public IkvmImporterLauncher(string toolPath) :
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
        public string GetToolDir(IkvmImporterTargetFramework framework, OSPlatform platform, Architecture architecture)
        {
            // determine the TFM of the tool to be executed
            var tfm = framework switch
            {
                IkvmImporterTargetFramework.NetFramework => "net461",
                IkvmImporterTargetFramework.NetCore => "netcoreapp3.1",
                _ => throw new NotImplementedException(),
            };

            // determine the RID of the tool to be executed
            var rid = architecture switch
            {
                Architecture.X86 => framework switch
                {
                    IkvmImporterTargetFramework.NetFramework => "any",
                    IkvmImporterTargetFramework.NetCore when platform == OSPlatform.Windows => "win7-x86",
                    IkvmImporterTargetFramework.NetCore when platform == OSPlatform.Linux => "linux-x86",
                    _ => throw new NotImplementedException(),
                },
                Architecture.X64 => framework switch
                {
                    IkvmImporterTargetFramework.NetFramework => "any",
                    IkvmImporterTargetFramework.NetCore when platform == OSPlatform.Windows => "win7-x64",
                    IkvmImporterTargetFramework.NetCore when platform == OSPlatform.Linux => "linux-x64",
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
        public string GetToolExe(IkvmImporterTargetFramework framework, OSPlatform platform, Architecture architecture)
        {
            return Path.Combine(GetToolDir(framework, platform, architecture), platform == OSPlatform.Windows ? "ikvmstub.exe" : "ikvmstub");
        }

        /// <summary>
        /// Gets the path to executable for the given environment.
        /// </summary>
        /// <param name="framework"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        public string GetToolExe(IkvmImporterTargetFramework framework)
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
        public string GetReferenceAssemblyDirectory(IkvmImporterTargetFramework framework, OSPlatform platform, Architecture architecture)
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
        public string GetReferenceAssemblyDirectory(IkvmImporterTargetFramework framework)
        {
            return GetReferenceAssemblyDirectory(framework, GetOSPlatform(), RuntimeInformation.OSArchitecture);
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
        public async Task<int> ExecuteAsync(IkvmImporterOptions options, CancellationToken cancellationToken = default)
        {
            if (options is null)
                throw new ArgumentNullException(nameof(options));

            var args = new List<string>();

            if (options.Output is not null)
                args.Add($"-out:{options.Output}");

            if (options.References is not null)
                foreach (var reference in options.References)
                    args.Add($"-reference:{reference}");

            if (options.Namespaces is not null)
                foreach (var ns in options.Namespaces)
                    args.Add($"-ns:{ns}");

            if (options.SkipError)
                args.Add("-skiperror");

            if (options.Shared)
                args.Add("-shared");

            if (options.Shared)
                args.Add("-shared");

            if (options.Shared)
                args.Add("-shared");

            if (options.NoStdLib)
                args.Add("-nostdlib");

            if (options.Lib is not null)
                foreach (var i in options.Lib)
                    args.Add($"-lib:{i}");

            if (options.Input is not null)
                args.Add(options.Input);

            // path to the temporary response file
            var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, new CancellationToken());

            try
            {
                // locate EXE file
                var exe = GetToolExe(options.TargetFramework);
                if (File.Exists(exe) == false)
                    throw new FileNotFoundException($"Could not locate tool at {exe}.");

                // check for supported platform
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) == false && options.TargetFramework == IkvmImporterTargetFramework.NetFramework)
                    throw new IkvmToolException("IKVM stub generation for .NET Framework assemblies is not supported on Linux operating system.");

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

                // configure CLI
                var cli = Cli.Wrap(exe).WithWorkingDirectory(Environment.CurrentDirectory);
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
            }
        }

    }

}
