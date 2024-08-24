namespace IKVM.CoreLib.Diagnostics
{

    /// <summary>
    /// Base <see cref="IDiagnosticHandler"/> that packages messages into an event type.
    /// </summary>
    internal abstract partial class DiagnosticEventHandler : IDiagnosticHandler
    {

        /// <inheritdoc />
        public abstract bool IsEnabled(Diagnostic diagnostic);

        /// <inheritdoc />
        public abstract void Report(in DiagnosticEvent @event);

    }

}
