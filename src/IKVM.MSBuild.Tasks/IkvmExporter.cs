using System.Threading;
using System.Threading.Tasks;

using IKVM.Tools.Runner.Exporter;

using Microsoft.Build.Framework;

namespace IKVM.MSBuild.Tasks
{

    /// <summary>
    /// Executes the IKVM exporter.
    /// </summary>
    public class IkvmExporter : IkvmToolExecTask
    {

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

        /// <summary>
        /// References used to resolve assemblies while exporting.
        /// </summary>
        public ITaskItem[] References { get; set; }

        /// <summary>
        /// Namespaces to include.
        /// </summary>
        public ITaskItem[] Namespaces { get; set; }

        /// <summary>
        /// Whether errors should be skipped during export.
        /// </summary>
        public bool ContinueOnError { get; set; }

        public ITaskItem[] Lib { get; set; }

        public bool IncludeNonPublicTypes { get; set; }

        public bool IncludeNonPublicInterfaces { get; set; }

        public bool IncludeNonPublicMembers { get; set; }

        public bool IncludeParameterNames { get; set; }

        public bool Shared { get; set; }

        public bool NoStdLib { get; set; }

        public bool Forwarders { get; set; }

        public bool Bootstrap { get; set; }

        protected override async Task<bool> ExecuteAsync(IkvmToolTaskDiagnosticWriter writer, CancellationToken cancellationToken)
        {
            var options = new IkvmExporterOptions();
            options.Output = Output;
            options.Input = Input;

            if (References is not null)
                foreach (var reference in References)
                    if (options.References.Contains(reference.ItemSpec) == false)
                        options.References.Add(reference.ItemSpec);

            if (Namespaces is not null)
                foreach (var resource in Namespaces)
                    options.Namespaces.Add(resource.ItemSpec);

            options.ContinueOnError = ContinueOnError;

            if (Lib is not null)
                foreach (var i in Lib)
                    options.Lib.Add(i.ItemSpec);

            options.IncludeNonPublicTypes = IncludeNonPublicTypes;
            options.IncludeNonPublicInterfaces = IncludeNonPublicInterfaces;
            options.IncludeNonPublicMembers = IncludeNonPublicMembers;
            options.IncludeParameterNames = IncludeParameterNames;

            options.Shared = Shared;
            options.NoStdLib = NoStdLib;
            options.Forwarders = Forwarders;
            options.Bootstrap = Bootstrap;

            // kick off the launcher with the configured options
            return await new IkvmExporterLauncher(ToolPath, writer).ExecuteAsync(options, cancellationToken) == 0;
        }

    }

}
