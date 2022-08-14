using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

using IKVM.Tool.Importer;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace IKVM.MSBuild.Tasks
{

    /// <summary>
    /// Executes the IKVM importer.
    /// </summary>
    public class IkvmImport : Task
    {

#if NETCOREAPP

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static IkvmImport()
        {
            // preload Mono.Unix native library, MSBuild isn't capable of following dependency context
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && Environment.Is64BitProcess)
                NativeLibrary.Load(Path.Combine(Path.GetDirectoryName(typeof(IkvmImport).Assembly.Location), "runtimes", "linux-x64", "native", "libMono.Unix.so"));
        }

#endif

        /// <summary>
        /// Root of the tools director.
        /// </summary>
        [Required]
        public string ToolsPath { get; set; }

        /// <summary>
        /// Whether we are generating a NetFramework or NetCore assembly.
        /// </summary>
        [Required]
        public string TargetFramework { get; set; } = "NetCore";

        /// <summary>
        /// Number of milliseconds to wait for the command to execute.
        /// </summary>
        public int Timeout { get; set; } = System.Threading.Timeout.Infinite;

        /// <summary>
        /// Input items to be compiled.
        /// </summary>
        [Required]
        public string Input { get; set; }

        /// <summary>
        /// Path of the output assembly.
        /// </summary>
        [Required]
        public string Output { get; set; }


        public ITaskItem[] References { get; set; }

        public ITaskItem[] Namespaces { get; set; }

        public bool SkipError { get; set; }

        public bool Shared { get; set; }

        public bool NoStdLib { get; set; }

        public ITaskItem[] Lib { get; set; }

        public bool Forwarders { get; set; }

        public bool Parameters { get; set; }

        public bool JApi { get; set; }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public override bool Execute()
        {
            var options = new IkvmImporterOptions();

            options.TargetFramework = TargetFramework switch
            {
                "NetCore" => IkvmImporterTargetFramework.NetCore,
                "NetFramework" => IkvmImporterTargetFramework.NetFramework,
                _ => throw new IkvmTaskException("Invalid TargetFramework."),
            };

            options.Output = Output;
            options.Input = Input;

            if (References is not null)
                foreach (var reference in References)
                    if (options.References.Contains(reference.ItemSpec) == false)
                        options.References.Add(reference.ItemSpec);

            if (Namespaces is not null)
                foreach (var resource in Namespaces)
                    options.Namespaces.Add(resource.ItemSpec);

            options.SkipError = SkipError;
            options.Shared = Shared;
            options.NoStdLib = NoStdLib;

            if (Lib is not null)
                foreach (var i in Lib)
                    options.Lib.Add(i.ItemSpec);

            options.Forwarders = Forwarders;
            options.Parameters = Parameters;
            options.JApi = JApi;

            // check that the tools exist
            if (ToolsPath == null || Directory.Exists(ToolsPath) == false)
                throw new IkvmTaskException("Missing tools path.");

            // kick off the launcher with the configured options
            var launcher = new IkvmImporterLauncher(ToolsPath, new IkvmToolTaskDiagnosticWriter(Log));
            var run = System.Threading.Tasks.Task.Run(() => launcher.ExecuteAsync(options, CancellationToken.None));

            // yield and wait for the task to complete
            BuildEngine3.Yield();
            var rsl = run.GetAwaiter().GetResult();
            BuildEngine3.Reacquire();

            // check that we exited successfully
            return rsl == 0;
        }

    }

}
