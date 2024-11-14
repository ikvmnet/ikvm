using System;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Base type for symbols.
    /// </summary>
    abstract class Symbol
    {

        internal const BindingFlags DefaultLookup = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;
        internal const BindingFlags DeclaredOnlyLookup = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;

        readonly SymbolContext _context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException"></exception>
        protected Symbol(SymbolContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets the associated symbol context.
        /// </summary>
        public SymbolContext Context => _context;

    }

}
