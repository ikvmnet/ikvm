using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace IKVM.MSBuild.Tasks
{

    /// <summary>
    /// Base tool task.
    /// </summary>
    public abstract class IkvmToolExecTask : Task
    {

#if NETCOREAPP

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static IkvmToolExecTask()
        {
            // preload Mono.Unix native library, MSBuild isn't capable of following dependency context
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                if (RuntimeInformation.ProcessArchitecture == Architecture.X64)
                    NativeLibrary.Load(Path.Combine(Path.GetDirectoryName(typeof(IkvmToolExecTask).Assembly.Location), "runtimes", "linux-x64", "native", "libMono.Unix.so"));
                else if (RuntimeInformation.ProcessArchitecture == Architecture.Arm)
                    NativeLibrary.Load(Path.Combine(Path.GetDirectoryName(typeof(IkvmToolExecTask).Assembly.Location), "runtimes", "linux-arm", "native", "libMono.Unix.so"));
                else if (RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
                    NativeLibrary.Load(Path.Combine(Path.GetDirectoryName(typeof(IkvmToolExecTask).Assembly.Location), "runtimes", "linux-arm64", "native", "libMono.Unix.so"));
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                if (RuntimeInformation.ProcessArchitecture == Architecture.X64)
                    NativeLibrary.Load(Path.Combine(Path.GetDirectoryName(typeof(IkvmToolExecTask).Assembly.Location), "runtimes", "osx-x64", "native", "libMono.Unix.dylib"));
                else if (RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
                    NativeLibrary.Load(Path.Combine(Path.GetDirectoryName(typeof(IkvmToolExecTask).Assembly.Location), "runtimes", "osx-arm64", "native", "libMono.Unix.dylib"));
            }
        }

#endif

        /// <summary>
        /// Root of the tools directory.
        /// </summary>
        [Required]
        public string ToolPath { get; set; }

        /// <summary>
        /// Optional path to a log file to copy messages to.
        /// </summary>
        public string LogFile { get; set; }

        /// <summary>
        /// Number of milliseconds to wait for the command to execute.
        /// </summary>
        public int Timeout { get; set; } = System.Threading.Timeout.Infinite;

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public override bool Execute()
        {
            // check that the tools exist
            if (ToolPath == null || Directory.Exists(ToolPath) == false)
                throw new IkvmTaskException($"Missing tool path: '{ToolPath}'.");

            TextWriter log = null;

            try
            {
                // optionally copy output to a text file
                if (LogFile != null)
                    log = new StreamWriter(File.OpenWrite(LogFile));

                // kick off the launcher with the configured options
                var run = ExecuteAsync(new IkvmToolTaskDiagnosticWriter(Log, log), CancellationToken.None);
                if (run.IsCompleted)
                    return run.GetAwaiter().GetResult();

                BuildEngine3.Yield();

                try
                {
                    return run.GetAwaiter().GetResult();
                }
                finally
                {
                    BuildEngine3.Reacquire();
                }
            }
            finally
            {
                if (log != null)
                {
                    log.Dispose();
                    log = null;
                }
            }
        }

        /// <summary>
        /// Executes the tool.
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected abstract System.Threading.Tasks.Task<bool> ExecuteAsync(IkvmToolTaskDiagnosticWriter writer, CancellationToken cancellationToken);

    }

}
