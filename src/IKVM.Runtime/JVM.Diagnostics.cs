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

            public void Report(in DiagnosticEvent evnt)
            {
                switch (evnt.Diagnostic.Level)
                {
                    case DiagnosticLevel.Trace:
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

    }

#endif

}
