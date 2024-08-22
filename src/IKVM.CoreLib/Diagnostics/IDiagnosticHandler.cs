namespace IKVM.CoreLib.Diagnostics
{

    /// <summary>
    /// Accepts a <see cref="DiagnosticEvent"> and routes it to the appropriate source.
    /// </summary>
    interface IDiagnosticHandler
    {

        /// <summary>
        /// Accepts a <see cref="DiagnosticEvent"/>.
        /// </summary>
        /// <param name="evnt"></param>
        void Report(in DiagnosticEvent evnt);

    }

}
