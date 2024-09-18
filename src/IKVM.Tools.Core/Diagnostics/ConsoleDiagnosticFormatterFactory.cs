namespace IKVM.Tools.Core.Diagnostics
{

    /// <summary>
    /// Provides a <see cref="TextDiagnosticFormatter"/>
    /// </summary>
    class ConsoleDiagnosticFormatterFactory : IDiagnosticFormatterFactory
    {


        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public ConsoleDiagnosticFormatterFactory()
        {

        }

        /// <inheritdoc />
        public IDiagnosticFormatter? GetFormatter(string spec)
        {
            return spec == "console" || spec.StartsWith("console,") ? new ConsoleDiagnosticFormatter(new ConsoleDiagnosticFormatterOptions()) : null;
        }

    }

}
