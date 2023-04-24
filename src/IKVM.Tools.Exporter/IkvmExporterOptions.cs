using System;
using System.Collections.Generic;
using System.Security.Policy;

namespace IKVM.Tools.Exporter
{

    /// <summary>
    /// Options passed to the exporter.
    /// </summary>
    [Serializable]
    public class IkvmExporterOptions
    {

        /// <summary>
        /// Output JAR file.
        /// </summary>
        public string Output { get; set; }

        /// <summary>
        /// Assembly references.
        /// </summary>
        public List<string> References { get; set; } = new List<string>();

        /// <summary>
        /// Path to directories to search for assembly references.
        /// </summary>
        public List<string> Libraries { get; set; } = new List<string>();

        /// <summary>
        /// Set of namespaces to export.
        /// </summary>
        public List<string> Namespaces { get; set; } = new List<string>();

        public bool Shared { get; set; }

        public bool NoStdLib { get; set; }

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

        public bool ContinueOnError { get; set; }

        public bool Boostrap { get; set; }

        /// <summary>
        /// Paths to assembly to export.
        /// </summary>
        public string Assembly { get; set; }

    }

}
