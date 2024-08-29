using System;
using System.Threading;
using System.Threading.Tasks;

using IKVM.CoreLib.Diagnostics;

namespace IKVM.Tools.Core.Diagnostics
{

    /// <summary>
    /// <see cref="IDiagnosticHandler"/> implementation that outputs based on a format specification.
    /// </summary>
    class FormattedDiagnosticEventHandler : DiagnosticEventHandler, IDisposable
    {

        readonly string _spec;
        readonly DiagnosticFormatterProvider _formatters;
        readonly IDiagnosticFormatter _formatter;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="formatters"></param>
        public FormattedDiagnosticEventHandler(string spec, DiagnosticFormatterProvider formatters)
        {
            _spec = spec ?? throw new ArgumentNullException(nameof(spec));
            _formatters = formatters ?? throw new ArgumentNullException(nameof(formatters));
            _formatter = _formatters.GetFormatter(spec) ?? throw new InvalidOperationException("Cannot find formatter for specification.");
        }

        /// <inheritdoc />
        public override bool IsEnabled(Diagnostic diagnostic)
        {
            return true;
        }

        /// <inheritdoc />
        public override void Report(in DiagnosticEvent @event)
        {
            _formatter.Write(@event);
        }

        public void Dispose()
        {
            if (_formatter is IDisposable disposable)
                disposable.Dispose();
        }

    }

}
