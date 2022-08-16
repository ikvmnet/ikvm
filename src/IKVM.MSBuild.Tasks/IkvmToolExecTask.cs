using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

using IKVM.Tool;

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
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && Environment.Is64BitProcess)
                NativeLibrary.Load(Path.Combine(Path.GetDirectoryName(typeof(IkvmToolExecTask).Assembly.Location), "runtimes", "linux-x64", "native", "libMono.Unix.so"));
        }

#endif

        /// <summary>
        /// Root of the tools directory.
        /// </summary>
        [Required]
        public string ToolPath { get; set; }

        /// <summary>
        /// Whether we are generating a NetFramework or NetCore assembly.
        /// </summary>
        [Required]
        public string ToolFramework { get; set; } = "NetCore";

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
            var targetFramework = ToolFramework switch
            {
                "NetCore" => IkvmToolFramework.NetCore,
                "NetFramework" => IkvmToolFramework.NetFramework,
                _ => throw new IkvmTaskException("Invalid ToolFramework."),
            };

            // check that the tools exist
            if (ToolPath == null || Directory.Exists(ToolPath) == false)
                throw new IkvmTaskException("Missing tools path.");

            // kick off the launcher with the configured options
            var run = System.Threading.Tasks.Task.Run(() => ExecuteAsync(targetFramework, new IkvmToolTaskDiagnosticWriter(Log), CancellationToken.None));

            // yield and wait for the task to complete
            BuildEngine3.Yield();
            var rsl = run.GetAwaiter().GetResult();
            BuildEngine3.Reacquire();

            // check that we exited successfully
            return rsl;
        }

        /// <summary>
        /// Executes the tool.
        /// </summary>
        /// <param name="targetFramework"></param>
        /// <param name="writer"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected abstract System.Threading.Tasks.Task<bool> ExecuteAsync(IkvmToolFramework targetFramework, IkvmToolTaskDiagnosticWriter writer, CancellationToken cancellationToken);

    }

}
