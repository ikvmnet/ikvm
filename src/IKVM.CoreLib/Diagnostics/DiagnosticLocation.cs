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
    readonly ref struct DiagnosticLocation(ReadOnlySpan<char> Path, int StartLine, int StartColumn, int EndLine, int EndColumn)
    {

        public readonly ReadOnlySpan<char> Path = Path;

        public readonly int StartLine = StartLine;

        public readonly int StartColumn = StartColumn;

        public readonly int EndLine = EndLine;

        public readonly int EndColumn = EndColumn;

    }

}
