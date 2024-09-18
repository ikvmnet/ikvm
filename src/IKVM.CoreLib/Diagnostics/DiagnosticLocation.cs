using System;

namespace IKVM.CoreLib.Diagnostics
{

    /// <summary>
    /// Describes the location of a diagnostic event.
    /// </summary>
    /// <param name="Path"></param>
    /// <param name="StartLine"></param>
    /// <param name="StartColumn"></param>
    /// <param name="EndLine"></param>
    /// <param name="EndColumn"></param>
    readonly struct DiagnosticLocation(string Path, int StartLine, int StartColumn, int EndLine, int EndColumn)
    {

        /// <summary>
        /// Gets the path to the file that the diagnostic refers to.
        /// </summary>
        public readonly string Path = Path;

        /// <summary>
        /// Gets the starting line that the diagnostic refers to. If no position, 0.
        /// </summary>
        public readonly int StartLine = StartLine;

        /// <summary>
        /// Gets the starting column that the diagnostic refers to. If no position, 0.
        /// </summary>
        public readonly int StartColumn = StartColumn;

        /// <summary>
        /// Gets the ending line that the diagnostic refers to. If no position, 0.
        /// </summary>
        public readonly int EndLine = EndLine;

        /// <summary>
        /// Gets the ending column that the diagnostic refers to. If no position, 0.
        /// </summary>
        public readonly int EndColumn = EndColumn;

    }

}
