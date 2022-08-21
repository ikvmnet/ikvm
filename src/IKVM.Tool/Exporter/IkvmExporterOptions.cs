using System.Collections.Generic;

namespace IKVM.Tool.Exporter
{

    /// <summary>
    /// Options available to the IKVM importer tool.
    /// </summary>
    public class IkvmExporterOptions
    {

        /// <summary>
        /// Version of the tool to use.
        /// </summary>
        public IkvmToolFramework ToolFramework { get; set; } = IkvmToolFramework.NetCore;

        /// <summary>
        /// Number of milliseconds to wait for the command to execute.
        /// </summary>
        public int Timeout { get; set; } = System.Threading.Timeout.Infinite;

        /// <summary>
        /// Files to be imported.
        /// </summary>
        public string Input { get; set; }

        /// <summary>
        /// Path of the output JAR.
        /// </summary>
        public string Output { get; set; }

        /// <summary>
        /// Set of paths to assemblies to add as references.
        /// </summary>
        public IList<string> References { get; } = new List<string>();

        /// <summary>
        /// Only include types from specified namespaces.
        /// </summary>
        public List<string> Namespaces { get; } = new List<string>();

        /// <summary>
        /// Continue when errors are encountered.
        /// </summary>
        public bool SkipError { get; set; }

        /// <summary>
        /// Process all assemblies in shared group.
        /// </summary>
        public bool Shared { get; set; }

        /// <summary>
        /// Do not reference standard libraries.
        /// </summary>
        public bool NoStdLib { get; set; }

        /// <summary>
        /// Additional directories to search for references.
        /// </summary>
        public IList<string> Lib { get; set; } = new List<string>();

        /// <summary>
        /// Export forwarded types too.
        /// </summary>
        public bool Forwarders { get; set; }

        /// <summary>
        /// Emit Java 8 classes with parameter names.
        /// </summary>
        public bool Parameters { get; set; }

        /// <summary>
        /// Generate jar suitable for comparison with japitools.
        /// </summary>
        public bool JApi { get; set; }

        /// <summary>
        /// Run in bootstrap mode.
        /// </summary>
        public bool Bootstrap { get; set; }

    }

}
