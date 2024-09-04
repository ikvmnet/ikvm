namespace IKVM.Tools.Core.Diagnostics
{

    interface IDiagnosticChannelFactory
    {

        /// <summary>
        /// Attempts to parse the channel spec into a <see cref="IDiagnosticChannel" />
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        IDiagnosticChannel? GetChannel(string spec);

    }

}