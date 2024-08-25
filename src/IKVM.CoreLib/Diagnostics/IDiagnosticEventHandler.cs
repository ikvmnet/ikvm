namespace IKVM.CoreLib.Diagnostics
{

    /// <summary>
    /// Exposes a method to accept diagnostic events.
    /// </summary>
    interface IDiagnosticEventHandler
    {

        /// <summary>
        /// Returns <c>true</c> if the specified <see cref="Diagnostic"/> is enabled.
        /// </summary>
        /// <param name="diagnostic"></param>
        /// <returns></returns>
        bool IsEnabled(Diagnostic diagnostic);

        /// <summary>
        /// Reports a diagnostic event.
        /// </summary>
        /// <param name="event"></param>
        void Report(in DiagnosticEvent @event);

    }

}
