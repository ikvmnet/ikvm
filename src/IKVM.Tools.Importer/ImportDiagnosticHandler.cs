using System;
using System.Linq;

using IKVM.CoreLib.Diagnostics;
using IKVM.Tools.Core.Diagnostics;

namespace IKVM.Tools.Importer
{

    /// <summary>
    /// <see cref="IDiagnosticHandler"/> for the importer.
    /// </summary>
    class ImportDiagnosticHandler : FormattedDiagnosticHandler
    {

        readonly ImportOptions _options;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="spec"></param>
        /// <param name="formatters"></param>
        public ImportDiagnosticHandler(ImportOptions options, string spec, DiagnosticFormatterProvider formatters) :
            base(spec, formatters)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <inheritdoc />
        public override bool IsEnabled(Diagnostic diagnostic)
        {
            //if (diagnostic.Level is DiagnosticLevel.Trace or DiagnosticLevel.Info)
            //    return false;

            if (diagnostic.Level == DiagnosticLevel.Warning && _options.NoWarn.Any(i => i.Id == diagnostic.Id))
                return false;

            return true;
        }

        /// <inheritdoc />
        public override void Report(in DiagnosticEvent @event)
        {
            base.Report(@event);
        }

    }

}
