namespace IKVM.CoreLib.Diagnostics
{

    /// <summary>
    /// Accepts a <see cref="DiagnosticEvent"> and routes it to the appropriate source.
    /// </summary>
    interface IDiagnosticHandler
    {

        /// <summary>
        /// Returns <c>true</c> if the specified diagnostic is enabled. May help save time building arguments.
        /// </summary>
        /// <param name="diagnostic"></param>
        /// <returns></returns>
        bool IsDiagnosticEnabled(Diagnostic diagnostic);

        /// <summary>
        /// Accepts a <see cref="DiagnosticEvent"/>.
        /// </summary>
        /// <param name="evnt"></param>
        void ReportEvent(in DiagnosticEvent evnt);

    }

}
