using System;

namespace IKVM.CoreLib.Diagnostics
{

    internal partial record class Diagnostic(int Id, string Name, string Message, DiagnosticLevel Level)
    {

        /// <summary>
        /// Creates a new diagnostic event with the specified arguments.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="exception"></param>
        /// <param name="location"></param>
        /// <returns></returns>
        public DiagnosticEvent Event(object?[] args, Exception? exception = null, DiagnosticLocation location = default) => new DiagnosticEvent(this, args, exception, location);

    }

}
