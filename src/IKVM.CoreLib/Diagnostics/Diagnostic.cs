using System;
using System.Text;

namespace IKVM.CoreLib.Diagnostics
{

#if NET8_0_OR_GREATER
    internal partial record class Diagnostic(int Id, string Name, CompositeFormat Message, DiagnosticLevel Level)
#else
    internal partial record class Diagnostic(int Id, string Name, string Message, DiagnosticLevel Level)
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
