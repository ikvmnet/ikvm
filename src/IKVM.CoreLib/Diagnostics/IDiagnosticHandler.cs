namespace IKVM.CoreLib.Diagnostics
{

    /// <summary>
    /// Exposes methods to accept diagnostic invocations.
    /// </summary>
    partial interface IDiagnosticHandler
    {

        /// <summary>
        /// Returns <c>true</c> if the specified <see cref="Diagnostic"/> is enabled.
        /// </summary>
        /// <param name="diagnostic"></param>
        /// <returns></returns>
        bool IsEnabled(Diagnostic diagnostic);

    }

}
