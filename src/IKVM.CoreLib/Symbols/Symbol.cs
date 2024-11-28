using System;
using System.Collections.Concurrent;
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
        ConcurrentDictionary<Type, object>? _state;

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

        /// <summary>
        /// Gets the state object of the specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T State<T>() where T : new()
        {
            if (_state is null)
                Interlocked.CompareExchange(ref _state, new ConcurrentDictionary<Type, object>(), null);

            return (T)_state.GetOrAdd(typeof(T), _ => new T());
        }

    }

}
