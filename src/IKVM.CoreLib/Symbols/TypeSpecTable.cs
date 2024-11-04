using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Threading;

using IKVM.CoreLib.Collections;
using IKVM.CoreLib.Threading;

namespace IKVM.CoreLib.Symbols
{

    struct TypeSpecTable
    {

        readonly ISymbolContext _context;
        readonly TypeSymbol _elementType;

        IndexRangeDictionary<ArrayTypeSymbol> _asArray;
        ReaderWriterLockSlim? _asArrayLock;
        SZArrayTypeSymbol? _asSZArray;
        PointerTypeSymbol? _asPointer;
        ByRefTypeSymbol? _asByRef;
        ConcurrentDictionary<IImmutableList<TypeSymbol>, ConstructedGenericTypeSymbol>? _genericTypeSymbols;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="elementType"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public TypeSpecTable(ISymbolContext context, TypeSymbol elementType)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _elementType = elementType ?? throw new ArgumentNullException(nameof(elementType));
            _asArray = new IndexRangeDictionary<ArrayTypeSymbol>();
        }

        /// <summary>
        /// Gets or creates a <see cref="ITypeSymbol"/> representing an Array of this type.
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
                        _asArray[rank] = new ArrayTypeSymbol(_context, _elementType, rank);

                return _asArray[rank] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates a <see cref="SZArrayTypeSymbol"/> representing a SZArray of this type.
        /// </summary>
        /// <returns></returns>
        public SZArrayTypeSymbol GetOrCreateSZArrayTypeSymbol()
        {
            if (_asSZArray == null)
                Interlocked.CompareExchange(ref _asSZArray, new SZArrayTypeSymbol(_context, _elementType), null);

            return _asSZArray;
        }

        /// <summary>
        /// Gets or creates a <see cref="PointerTypeSymbol"/> representing a pointer of this type.
        /// </summary>
        /// <returns></returns>
        public PointerTypeSymbol GetOrCreatePointerTypeSymbol()
        {
            if (_asPointer == null)
                Interlocked.CompareExchange(ref _asPointer, new PointerTypeSymbol(_context, _elementType), null);

            return _asPointer;
        }

        /// <summary>
        /// Gets or creates a <see cref="ByRefTypeSymbol"/> representing a by-ref of this type.
        /// </summary>
        /// <returns></returns>
        public ByRefTypeSymbol GetOrCreateByRefTypeSymbol()
        {
            if (_asByRef == null)
                Interlocked.CompareExchange(ref _asByRef, new ByRefTypeSymbol(_context, _elementType), null);

            return _asByRef;
        }

        /// <summary>
        /// Gets or creates a <see cref="ConstructedGenericTypeSymbol"/> representing a specialized generic of this type.
        /// </summary>
        /// <param name="genericTypeArguments"></param>
        /// <returns></returns>
        public ConstructedGenericTypeSymbol GetOrCreateGenericTypeSymbol(IImmutableList<TypeSymbol> genericTypeArguments)
        {
            if (genericTypeArguments is null)
                throw new ArgumentNullException(nameof(genericTypeArguments));

            if (_elementType.IsGenericTypeDefinition == false)
                throw new InvalidOperationException();

            if (_genericTypeSymbols == null)
                Interlocked.CompareExchange(ref _genericTypeSymbols, new(TypeSymbolListEqualityComparer.Instance), null);

            return _genericTypeSymbols.GetOrAdd(genericTypeArguments, CreateGenericTypeSymbol);
        }

        /// <summary>
        /// Creates a new <see cref="ConstructedGenericTypeSymbol"/> representing a specialized generic of this type.
        /// </summary>
        /// <param name="genericTypeArguments"></param>
        /// <returns></returns>
        readonly ConstructedGenericTypeSymbol CreateGenericTypeSymbol(IImmutableList<TypeSymbol> genericTypeArguments)
        {
            return new ConstructedGenericTypeSymbol(_context, _elementType, genericTypeArguments);
        }

    }

}
