using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using CliWrap;

using IKVM.Tools.Runner.Compiler;
using IKVM.Tools.Runner.Internal;

namespace IKVM.Tools.Runner.Exporter
{

    /// <summary>
    /// Provides methods to launch the IKVM importer.
    /// </summary>
    public class IkvmExporterLauncher : IkvmToolLauncher
    {

        static readonly string TOOLNAME = "ikvmstub";

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="toolPath"></param>
        /// <param name="listener"></param>
        public IkvmExporterLauncher(string toolPath, IIkvmToolDiagnosticEventListener listener) :
            base(TOOLNAME, toolPath, listener)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="listener"></param>
        public IkvmExporterLauncher(IIkvmToolDiagnosticEventListener listener) :
            this(Path.Combine(Path.GetDirectoryName(typeof(IkvmCompilerLauncher).Assembly.Location), TOOLNAME), listener)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="toolPath"></param>
        public IkvmExporterLauncher(string toolPath) :
            this(toolPath, new IkvmToolDelegateDiagnosticListener(evt => Task.CompletedTask))
        {

        }

        /// <summary>
        /// Executes the compiler.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public async Task<int> ExecuteAsync(IkvmExporterOptions options, CancellationToken cancellationToken = default)
        {
            if (options is null)
                throw new ArgumentNullException(nameof(options));

            var args = new List<string>();

            if (options.Output is not null)
                args.Add($"--out:{options.Output}");

            if (options.References is not null)
                foreach (var reference in options.References)
                    args.Add($"--reference:{reference}");

            if (options.Namespaces is not null)
                foreach (var ns in options.Namespaces)
                    args.Add($"--ns:{ns}");

            if (options.Shared)
                args.Add("--shared");

            if (options.NoStdLib)
                args.Add("--nostdlib");

            if (options.Forwarders)
                args.Add("--forwarders");

            if (options.IncludeNonPublicTypes)
                args.Add("--non-public-types");

            if (options.IncludeNonPublicInterfaces)
                args.Add("--non-public-interfaces");

            if (options.IncludeNonPublicMembers)
                args.Add("--non-public-members");

            if (options.IncludeParameterNames)
                args.Add("--parameters");

            if (options.Bootstrap)
                args.Add("--bootstrap");

            if (options.Lib is not null)
                foreach (var i in options.Lib)
                    args.Add($"--lib:{i}");

            if (options.ContinueOnError)
                args.Add("--skiperror");

            if (options.Input is not null)
                args.Add(options.Input);

            // path to the temporary response file
            var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, new CancellationToken());

            try
            {
                // locate EXE file
                var exe = GetToolExe();
                if (File.Exists(exe) == false)
                    throw new FileNotFoundException($"Could not locate tool at '{exe}'.");

                // if we're running on Unix, we might need to set the execute bit on the file,
                // since the NuGet package is built on Windows
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
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
                await LogEvent(IkvmToolDiagnosticEventLevel.Debug, "Executing {0} {1}", cli.TargetFilePath, cli.Arguments);

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
