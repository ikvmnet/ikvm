using System.Collections.Generic;

namespace IKVM.Tools.Exporter
{

    /// <summary>
    /// Options passed to the exporter.
    /// </summary>
    class ExportOptions
    {

        /// <summary>
        /// Output JAR file.
        /// </summary>
        public string Output { get; set; }

        /// <summary>
        /// Assembly references.
        /// </summary>
        public List<string> References { get; } = [];

        /// <summary>
        /// Path to directories to search for assembly references.
        /// </summary>
        public List<string> Libraries { get; } = [];

        /// <summary>
        /// Set of namespaces to export.
        /// </summary>
        public List<string> Namespaces { get; } = [];

        /// <summary>
        /// Are we doing a shared export.
        /// </summary>
        public bool Shared { get; set; }

        /// <summary>
        /// Whether to include the standard library references.
        /// </summary>
        public bool NoStdLib { get; set; }

        /// <summary>
        /// Whether to include forwarder types.
        /// </summary>
        public bool Forwarders { get; set; }

        /// <summary>
        /// Whether to export parameter names.
        /// </summary>
        public bool IncludeParameterNames { get; set; }

        /// <summary>
        /// Whehter to export non-public types.
        /// </summary>
        public bool IncludeNonPublicTypes { get; set; }

        /// <summary>
        /// Whether to export non-public interface implementations.
        /// </summary>
        public bool IncludeNonPublicInterfaces { get; set; }

        /// <summary>
        /// Whether to export non-public members.
        /// </summary>
        public bool IncludeNonPublicMembers { get; set; }

        /// <summary>
        /// Whether to export serialVersionUID fields.
        /// </summary>
        public bool SerialVersionUID { get; set; }

        /// <summary>
        /// Whether to continue execution on error.
        /// </summary>
        public bool ContinueOnError { get; set; }

        /// <summary>
        /// Whether we are to run in bootstrap mode.
        /// </summary>
        public bool Bootstrap { get; set; }

        /// <summary>
        /// Paths to assembly to export.
        /// </summary>
        public string Assembly { get; set; }

    }

}
