using System;

namespace IKVM.Tools.Runner.Compiler
{

    /// <summary>
    /// Describes an external resource option.
    /// </summary>
    public class IkvmCompilerExternalResourceItem
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="resourcePath"></param>
        /// <exception cref="ArgumentException"></exception>
        public IkvmCompilerExternalResourceItem(string filePath, string resourcePath)
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
