namespace IKVM.Tools.Core.Diagnostics
{

    interface IDiagnosticFormatterFactory
    {

        /// <summary>
        /// Returns a <see cref="IDiagnosticFormatter"/> if the given format can be handled by this factory.
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        IDiagnosticFormatter? GetFormatter(string format);

    }

}
