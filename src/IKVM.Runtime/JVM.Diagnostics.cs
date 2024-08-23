using System.Diagnostics;

using IKVM.CoreLib.Diagnostics;

namespace IKVM.Runtime
{

#if FIRST_PASS == false && IMPORTER == false && EXPORTER == false

    static partial class JVM
    {

        /// <summary>
        /// Handles diagnostic events during runtime.
        /// </summary>
        class DiagnosticHandler : IDiagnosticHandler
        {

            public void ReportEvent(in DiagnosticEvent evnt)
            {
                switch (evnt.Diagnostic.Level)
                {
                    case DiagnosticLevel.Trace:
                        DiagnosticSource.Write()
                        break;
                    case DiagnosticLevel.Informational:
                        break;
                    case DiagnosticLevel.Warning:
                        break;
                    case DiagnosticLevel.Error:
                        break;
                    case DiagnosticLevel.Fatal:
                        break;
                }
            }

        }

        /// <summary>
        /// Gets the JVM diagnostic handler.
        /// </summary>
        static IDiagnosticHandler Diagnostics { get; } = new DiagnosticHandler();

        /// <summary>
        /// Source of diagnostics for the IKVM runtime.
        /// </summary>
        static DiagnosticSource DiagnosticSource { get; } = new DiagnosticListener("IKVM.Runtime");

    }

#endif

}
