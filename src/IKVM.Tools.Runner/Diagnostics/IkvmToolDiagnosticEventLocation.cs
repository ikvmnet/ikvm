namespace IKVM.Tools.Runner.Diagnostics
{

    /// <summary>
    /// Represents the location of a diagnostic event.
    /// </summary>
    /// <param name="StartLine"></param>
    /// <param name="StartColumn"></param>
    /// <param name="EndLine"></param>
    /// <param name="EndColumn"></param>
    public readonly record struct IkvmToolDiagnosticEventLocation(int StartLine, int StartColumn, int EndLine, int EndColumn)
    {

        /// <summary>
        /// Start line of the effected region.
        /// </summary>
        public int StartLine { get; } = StartLine;

        /// <summary>
        /// Start column of the effected region.
        /// </summary>
        public int StartColumn { get; } = StartColumn;

        /// <summary>
        /// End line of the effected region.
        /// </summary>
        public int EndLine { get; } = EndLine;

        /// <summary>
        /// End column of the effected region.
        /// </summary>
        public int EndColumn { get; } = EndColumn;

    }

}
