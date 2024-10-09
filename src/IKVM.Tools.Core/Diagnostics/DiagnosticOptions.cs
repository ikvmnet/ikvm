using System.Collections.Immutable;

using IKVM.CoreLib.Diagnostics;

namespace IKVM.Tools.Core.Diagnostics
{

    class DiagnosticOptions
    {

        /// <summary>
        /// Gets or sets whether all warnings should be suppressed.
        /// </summary>
        public bool NoWarn { get; set; }

        /// <summary>
        /// Gets or sets whether the specified diagnostics should be suppressed.
        /// </summary>
        public ImmutableArray<Diagnostic> NoWarnDiagnostics { get; set; } = [];

        /// <summary>
        /// Gets or sets whether all warnings should be promoted to errors.
        /// </summary>
        public bool WarnAsError { get; set; }

        /// <summary>
        /// Gets or sets whether the specified diagnostics should be promoted to errors.
        /// </summary>
        public ImmutableArray<Diagnostic> WarnAsErrorDiagnostics { get; set; } = [];

    }

}