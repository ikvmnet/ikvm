using System.Collections.Generic;
using System.Collections.Immutable;

using IKVM.CoreLib.Diagnostics;

namespace IKVM.Tools.Core.Diagnostics
{

    /// <summary>
    /// Base class for common diagnostic options.
    /// </summary>
    abstract class DiagnosticFormatterOptions
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
        /// All warnings should be elevated to errors.
        /// </summary>
        public bool WarnAsError { get; set; }

        /// <summary>
        /// Set of warning diagnostics that should be elevated to errors.
        /// </summary>
        public ImmutableArray<Diagnostic> WarnAsErrorDiagnostics { get; set; } = [];

    }

}