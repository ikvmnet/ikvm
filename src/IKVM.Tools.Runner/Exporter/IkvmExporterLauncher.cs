using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

using CliWrap;

using IKVM.Tools.Runner.Diagnostics;
using IKVM.Tools.Runner.Internal;

namespace IKVM.Tools.Runner.Exporter
{

    /// <summary>
    /// Provides methods to launch the IKVM importer.
    /// </summary>
    public class IkvmExporterLauncher : IkvmToolLauncher
    {

        static readonly string TOOLNAME = "ikvmstub";
        static readonly string TOOLPATH = typeof(IkvmExporterLauncher).Assembly.Location is string s ? Path.GetDirectoryName(s) ?? "" : "";

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
            this(TOOLPATH, listener)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="toolPath"></param>
        public IkvmExporterLauncher(string toolPath) :
            this(toolPath, new IkvmToolNullDiagnosticListener())
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
            {
                args.Add("--out");
                args.Add(options.Output);
            }

            if (options.References is not null)
            {
                foreach (var reference in options.References)
                {
                    args.Add("--reference");
                    args.Add(reference);
                }
            }

            if (options.Namespaces is not null)
            {
                foreach (var ns in options.Namespaces)
                {
                    args.Add("--ns");
                    args.Add(ns);
                }
            }

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
            {
                foreach (var i in options.Lib)
                {
                    args.Add("--lib");
                    args.Add(i);
                }
            }

            if (options.ContinueOnError)
                args.Add("--skiperror");

            args.Add("--log");
            args.Add("json,file=stderr");

            if (options.Input is not null)
                args.Add(options.Input);

            // path to the temporary response file
            var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, new CancellationToken());

            // combine manual cancellation with timeout
            var ctk = cts.Token;
            if (options.Timeout != Timeout.Infinite)
                ctk = CancellationTokenSource.CreateLinkedTokenSource(ctk, new CancellationTokenSource(options.Timeout).Token).Token;

            try
            {
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
                            var prm = mod | UnixFileMode.UserExecute;
                            if (mod != prm)
                                File.SetUnixFileMode(exe, prm);
#else
                            var psx = Mono.Unix.UnixFileSystemInfo.GetFileSystemEntry(exe);
                            var mod = psx.FileAccessPermissions;
                            var prm = mod | Mono.Unix.FileAccessPermissions.UserExecute;
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

                // configure CLI, with wrapper if required
                Command cli;
                if (wrap != null)
                {
                    cli = Cli.Wrap(wrap);
                    args.Insert(0, exe);
                }
                else
                    cli = Cli.Wrap(exe);

                // set configuration of CLI
                cli = cli.WithWorkingDirectory(Environment.CurrentDirectory);
                cli = cli.WithArguments(args);
                cli = cli.WithValidation(CommandResultValidation.None);

                // log the command we're about to run
                await LogEventAsync(IkvmToolDiagnosticEventLevel.Trace, "Executing {0} {1}", [cli.TargetFilePath, cli.Arguments], ctk);

                // send output to MSBuild (TODO, replace with binary reading)
                cli = cli.WithStandardErrorPipe(PipeTarget.ToDelegate(l => ParseAndLogEventAsync(l, cancellationToken).AsTask()));

                // execute command
                var pid = cli.ExecuteAsync(ctk);

                // windows provides special support for killing subprocesses on termination of parent
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    try
                    {
                        if (pid.Task.IsCompleted == false)
                            WindowsChildProcessTracker.AddProcess(Process.GetProcessById(pid.ProcessId));
                    }
                    catch
                    {
                        await LogEventAsync(IkvmToolDiagnosticEventLevel.Error, "Failed to attach child process.", [], ctk);
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
            }
        }

    }

}
