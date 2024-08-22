namespace IKVM.CoreLib.Diagnostics
{

    /// <summary>
    /// Uniquely identifiers a diagnostic.
    /// </summary>
    /// <param name="Value"></param>
    record struct DiagnosticId(int Value)
    {

        public static implicit operator DiagnosticId(int id) => new DiagnosticId(id);

    }

}
