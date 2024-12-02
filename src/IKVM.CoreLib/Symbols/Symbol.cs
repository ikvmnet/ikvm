using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Base type for symbols.
    /// </summary>
    [DebuggerDisplay("{ToString()}")]
    public abstract class Symbol
    {

        internal const BindingFlags DefaultLookup = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;
        internal const BindingFlags DeclaredOnlyLookup = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;

        readonly SymbolContext _context;
        object? _state;

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
