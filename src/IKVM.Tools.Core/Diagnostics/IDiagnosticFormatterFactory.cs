namespace IKVM.Tools.Core.Diagnostics
{

    interface IDiagnosticFormatterFactory
    {

        /// <summary>
        /// Returns a <see cref="IDiagnosticFormatter"/> if the given spec can be handled by this factory.
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        IDiagnosticFormatter? GetFormatter(string spec);

    }

}
