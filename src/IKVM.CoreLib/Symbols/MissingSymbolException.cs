using System;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Thrown when an access to a missing symbol is attempted.
    /// </summary>
    public abstract class MissingSymbolException : InvalidOperationException
    {

        readonly Symbol _symbol;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public MissingSymbolException(Symbol symbol)
        {
            _symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
        }

        /// <summary>
        /// Gets the symbol that is missing.
        /// </summary>
        public Symbol Symbol => _symbol;

    }

}