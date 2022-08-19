using System;
using System.Collections.Generic;

namespace IKVM.Tool.Exporter
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
        public List<string> References { get; set; }

        /// <summary>
        /// Path to directories to search for assembly references.
        /// </summary>
        public List<string> Libraries { get; set; }

        public bool JApi { get; set; }

        public bool Shared { get; set; }

        public bool NoStdLib { get; set; }

        public List<string> Namespaces { get; set; }

        public bool Forwarders { get; set; }

        public bool Parameters { get; set; }

        public bool SkipError { get; set; }

        public bool Boostrap { get; set; }

        /// <summary>
        /// Paths to assembly to export.
        /// </summary>
        public string Assembly { get; set; }

    }

}
