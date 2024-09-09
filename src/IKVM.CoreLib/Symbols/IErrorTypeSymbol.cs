namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// An IErrorTypeSymbol is used when the compiler cannot determine a symbol object to return because
    /// of an error. For example, if a field is declared "Goo x;", and the type "Goo" cannot be
    /// found, an IErrorTypeSymbol is returned when asking the field "x" what it's type is.
    /// </summary>
    interface IErrorTypeSymbol : INamedTypeSymbol
    {



    }

}
