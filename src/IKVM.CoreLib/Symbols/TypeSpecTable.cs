using System;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

using IKVM.CoreLib.Collections;
using IKVM.CoreLib.Threading;

namespace IKVM.CoreLib.Symbols
{

    /// <summary>
    /// Maintains specialized versions of a type.
    /// </summary>
    struct TypeSpecTable
    {

        readonly SymbolContext _context;
        readonly TypeSymbol _type;

        IndexRangeDictionary<ArrayTypeSymbol> _asArray;
        ReaderWriterLockSlim? _asArrayLock;
        SZArrayTypeSymbol? _asSZArray;
        PointerTypeSymbol? _asPointer;
        ByRefTypeSymbol? _asByRef;
        WeakHashTable<TypeSymbol[], SpecializedTypeSymbol>? _specialized;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public TypeSpecTable(SymbolContext context, TypeSymbol type)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _type = type ?? throw new ArgumentNullException(nameof(type));
            _asArray = new IndexRangeDictionary<ArrayTypeSymbol>();
        }

        /// <summary>
        /// Gets or creates a <see cref="TypeSymbol"/> representing an Array of this type.
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public ArrayTypeSymbol GetOrCreateArrayTypeSymbol(int rank)
        {
            if (_asArrayLock == null)
                Interlocked.CompareExchange(ref _asArrayLock, new(), null);

            using (_asArrayLock.CreateUpgradeableReadLock())
            {
                if (_asArray[rank] == null)
                    using (_asArrayLock.CreateWriteLock())
                        _asArray[rank] = new ArrayTypeSymbol(_context, _type, rank, [], Enumerable.Repeat(0, rank).ToImmutableArray());

                return _asArray[rank] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates a <see cref="SZArrayTypeSymbol"/> representing a SZArray of this type.
        /// </summary>
        /// <returns></returns>
        public SZArrayTypeSymbol GetOrCreateSZArrayTypeSymbol()
        {
            if (_asSZArray is null)
                Interlocked.CompareExchange(ref _asSZArray, new SZArrayTypeSymbol(_context, _type), null);

            return _asSZArray;
        }

        /// <summary>
        /// Gets or creates a <see cref="PointerTypeSymbol"/> representing a pointer of this type.
        /// </summary>
        /// <returns></returns>
        public PointerTypeSymbol GetOrCreatePointerTypeSymbol()
        {
            if (_asPointer is null)
                Interlocked.CompareExchange(ref _asPointer, new PointerTypeSymbol(_context, _type), null);

            return _asPointer;
        }

        /// <summary>
        /// Gets or creates a <see cref="ByRefTypeSymbol"/> representing a by-ref of this type.
        /// </summary>
        /// <returns></returns>
        public ByRefTypeSymbol GetOrCreateByRefTypeSymbol()
        {
            if (_asByRef is null)
                Interlocked.CompareExchange(ref _asByRef, new ByRefTypeSymbol(_context, _type), null);

            return _asByRef;
        }

        /// <summary>
        /// Gets or creates a <see cref="SpecializedTypeSymbol"/> representing a specialized generic of this type.
        /// </summary>
        /// <param name="typeArguments"></param>
        /// <returns></returns>
        public SpecializedTypeSymbol GetOrCreateGenericTypeSymbol(ImmutableArray<TypeSymbol> typeArguments)
        {
            if (typeArguments.IsDefault)
                throw new ArgumentNullException(nameof(typeArguments));

            if (_type.IsGenericTypeDefinition == false)
                throw new InvalidOperationException();

            // initialize on first access
            if (_specialized == null)
                Interlocked.CompareExchange(ref _specialized, new(TypeSymbolListEqualityComparer.Instance), null);

            // check before creating delegate/closure
            if (_specialized.TryGetValue(ImmutableCollectionsMarshal.AsArray(typeArguments)!, out var existing))
                return existing;

            var context = _context;
            var type = _type;
            return _specialized.GetOrCreateValue(ImmutableCollectionsMarshal.AsArray(typeArguments)!, _ => new SpecializedTypeSymbol(context, type, new GenericContext(typeArguments, default)));
        }

    }

}
