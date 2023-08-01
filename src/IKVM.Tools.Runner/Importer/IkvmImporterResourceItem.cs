using System;
using System.Collections.Generic;
using System.Text;

namespace IKVM.Tools.Runner.Importer
{

    /// <summary>
    /// Describes a resource option.
    /// </summary>
    public class IkvmImporterResourceItem
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="resourcePath"></param>
        public IkvmImporterResourceItem(string filePath, string resourcePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentException($"'{nameof(filePath)}' cannot be null or empty.", nameof(filePath));
            if (string.IsNullOrEmpty(resourcePath))
                throw new ArgumentException($"'{nameof(resourcePath)}' cannot be null or empty.", nameof(resourcePath));

            FilePath = filePath;
            ResourcePath = resourcePath;
        }

        /// <summary>
        /// Path to the source file.
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Path to the resource within the Java environment.
        /// </summary>
        public string ResourcePath { get; set; }

    }

}
