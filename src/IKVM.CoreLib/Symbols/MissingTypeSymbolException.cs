namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Thrown when an access to a missing type is attempted.
    /// </summary>
    public class MissingTypeSymbolException : MissingMemberSymbolException
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public MissingTypeSymbolException(TypeSymbol type) :
            base(type)
        {

        }

        /// <summary>
        /// Gets the type symbol that is missing.
        /// </summary>
        public new TypeSymbol Symbol => (TypeSymbol)base.Symbol;

    }

}
