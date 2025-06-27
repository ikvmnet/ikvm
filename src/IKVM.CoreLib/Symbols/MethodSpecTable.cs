using System;
using System.Collections.Immutable;
using System.Runtime.InteropServices;
using System.Threading;

using IKVM.CoreLib.Collections;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Maintains specialized instances of a generic <see cref="MethodSymbol"/>.
    /// </summary>
    struct MethodSpecTable
    {

        readonly SymbolContext _context;
        readonly MethodSymbol _method;

        WeakHashTable<TypeSymbol[], SpecializedMethodSymbol>? _specialized;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="method"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public MethodSpecTable(SymbolContext context, MethodSymbol method)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _method = method ?? throw new ArgumentNullException(nameof(method));
        }

        /// <summary>
        /// Gets or creates a <see cref="MethodSymbol"/> representing a specialized generic of this method.
        /// </summary>
        /// <param name="typeArguments"></param>
        /// <returns></returns>
        public MethodSymbol GetOrCreateGenericMethodSymbol(ImmutableArray<TypeSymbol> typeArguments)
        {
            if (typeArguments.IsDefault)
                throw new ArgumentNullException(nameof(typeArguments));

            if (_method.IsGenericMethodDefinition == false)
                throw new InvalidOperationException();

            // initialize on first access
            if (_specialized == null)
                Interlocked.CompareExchange(ref _specialized, new(TypeSymbolListEqualityComparer.Instance), null);

            // check before creating delegate/closure
            if (_specialized.TryGetValue(ImmutableCollectionsMarshal.AsArray(typeArguments)!, out var existing))
                return existing;

            var context = _context;
            var method = _method;
            return _specialized.GetOrCreateValue(ImmutableCollectionsMarshal.AsArray(typeArguments)!, _ => new SpecializedMethodSymbol(context, method.DeclaringType, method, new GenericContext(default, typeArguments)));
        }
    }

}
