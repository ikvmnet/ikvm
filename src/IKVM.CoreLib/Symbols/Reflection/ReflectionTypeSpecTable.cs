using System;
using System.Collections.Concurrent;
using System.Threading;

using IKVM.CoreLib.Collections;
using IKVM.CoreLib.Threading;

namespace IKVM.CoreLib.Symbols.Reflection
{

    struct ReflectionTypeSpecTable
    {

        readonly ReflectionSymbolContext _context;
        readonly IReflectionModuleSymbol _module;
        readonly IReflectionTypeSymbol _elementType;

        IndexRangeDictionary<ReflectionArrayTypeSymbol> _asArray;
        ReaderWriterLockSlim? _asArrayLock;
        ReflectionSZArrayTypeSymbol? _asSZArray;
        ReflectionPointerTypeSymbol? _asPointer;
        ReflectionByRefTypeSymbol? _asByRef;
        ConcurrentDictionary<IReflectionTypeSymbol[], ReflectionGenericSpecTypeSymbol>? _genericTypeSymbols;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="elementType"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionTypeSpecTable(ReflectionSymbolContext context, IReflectionModuleSymbol module, IReflectionTypeSymbol elementType)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _elementType = elementType ?? throw new ArgumentNullException(nameof(elementType));
            _asArray = new IndexRangeDictionary<ReflectionArrayTypeSymbol>();
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionTypeSymbol"/> representing an Array of this type.
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public IReflectionTypeSymbol GetOrCreateArrayTypeSymbol(int rank)
        {
            if (_asArrayLock == null)
                Interlocked.CompareExchange(ref _asArrayLock, new(), null);

            using (_asArrayLock.CreateUpgradeableReadLock())
            {
                if (_asArray[rank] == null)
                    using (_asArrayLock.CreateWriteLock())
                        _asArray[rank] = new ReflectionArrayTypeSymbol(_context, _module, _elementType, rank);

                return _asArray[rank] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionTypeSymbol"/> representing a SZArray of this type.
        /// </summary>
        /// <returns></returns>
        public IReflectionTypeSymbol GetOrCreateSZArrayTypeSymbol()
        {
            if (_asSZArray == null)
                Interlocked.CompareExchange(ref _asSZArray, new ReflectionSZArrayTypeSymbol(_context, _module, _elementType), null);

            return _asSZArray;
        }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionTypeSymbol"/> representing a pointer of this type.
        /// </summary>
        /// <returns></returns>
        public IReflectionTypeSymbol GetOrCreatePointerTypeSymbol()
        {
            if (_asPointer == null)
                Interlocked.CompareExchange(ref _asPointer, new ReflectionPointerTypeSymbol(_context, _module, _elementType), null);

            return _asPointer;
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionTypeSymbol"/> representing a by-ref of this type.
        /// </summary>
        /// <returns></returns>
        public IReflectionTypeSymbol GetOrCreateByRefTypeSymbol()
        {
            if (_asByRef == null)
                Interlocked.CompareExchange(ref _asByRef, new ReflectionByRefTypeSymbol(_context, _module, _elementType), null);

            return _asByRef;
        }

        /// <summary>
        /// Gets or creates a <see cref="IReflectionTypeSymbol"/> representing a specialized generic of this type.
        /// </summary>
        /// <param name="genericTypeArguments"></param>
        /// <returns></returns>
        public IReflectionTypeSymbol GetOrCreateGenericTypeSymbol(IReflectionTypeSymbol[] genericTypeArguments)
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
        /// Creates a new <see cref="IReflectionTypeSymbol"/> representing a specialized generic of this type.
        /// </summary>
        /// <param name="genericTypeArguments"></param>
        /// <returns></returns>
        readonly ReflectionGenericSpecTypeSymbol CreateGenericTypeSymbol(IReflectionTypeSymbol[] genericTypeArguments)
        {
            return new ReflectionGenericSpecTypeSymbol(_context, _module, _elementType, genericTypeArguments);
        }

    }

}
