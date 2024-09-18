namespace IKVM.CoreLib.Diagnostics
{

    partial class NullDiagnosticHandler : IDiagnosticHandler
    {

        public bool IsEnabled(Diagnostic diagnostic)
        {
            return false;
        }

    }

}
