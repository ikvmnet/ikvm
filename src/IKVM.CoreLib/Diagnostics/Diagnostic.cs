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
        /// <param name="location"></param>
        /// <returns></returns>
        public DiagnosticEvent Event(object?[] args, DiagnosticLocation location = default) => new DiagnosticEvent(this, args, location);

    }

}
