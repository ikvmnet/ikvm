using System;
using System.Text;

namespace IKVM.CoreLib.Diagnostics
{

#if NET8_0_OR_GREATER
    partial record class Diagnostic(DiagnosticId Id, string Name, CompositeFormat Message, DiagnosticLevel Level)
#else
    partial record class Diagnostic(DiagnosticId Id, string Name, string Message, DiagnosticLevel Level)
#endif
    {

        /// <summary>
        /// Creates a new diagnostic event with the specified arguments.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public DiagnosticEvent Event(ReadOnlySpan<object?> args) => new DiagnosticEvent(this, args, null);

        /// <summary>
        /// Creates a new diagnostic event with the specified arguments and <see cref="Exception"/>.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public DiagnosticEvent Event(Exception e, ReadOnlySpan<object?> args) => new DiagnosticEvent(this, args, e);

    }

}
