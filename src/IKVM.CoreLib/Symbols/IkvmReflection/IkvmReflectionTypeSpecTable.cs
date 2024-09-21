using System;
using System.Collections.Concurrent;
using System.Threading;

using IKVM.CoreLib.Collections;
using IKVM.CoreLib.Threading;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    struct IkvmReflectionTypeSpecTable
    {

        readonly IkvmReflectionSymbolContext _context;
        readonly IIkvmReflectionModuleSymbol _module;
        readonly IIkvmReflectionTypeSymbol _elementType;

        IndexRangeDictionary<IIkvmReflectionTypeSymbol> _asArray;
        ReaderWriterLockSlim? _asArrayLock;
        IIkvmReflectionTypeSymbol? _asSZArray;
        IIkvmReflectionTypeSymbol? _asPointer;
        IIkvmReflectionTypeSymbol? _asByRef;
        ConcurrentDictionary<IIkvmReflectionTypeSymbol[], IIkvmReflectionTypeSymbol>? _genericTypeSymbols;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="elementType"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReflectionTypeSpecTable(IkvmReflectionSymbolContext context, IIkvmReflectionModuleSymbol module, IIkvmReflectionTypeSymbol elementType)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _elementType = elementType ?? throw new ArgumentNullException(nameof(elementType));
            _asArray = new IndexRangeDictionary<IIkvmReflectionTypeSymbol>();
        }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionTypeSymbol"/> representing an Array of this type.
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public IIkvmReflectionTypeSymbol GetOrCreateArrayTypeSymbol(int rank)
        {
            if (_asArrayLock == null)
                Interlocked.CompareExchange(ref _asArrayLock, new(), null);

            using (_asArrayLock.CreateUpgradeableReadLock())
            {
                if (_asArray[rank] == null)
                    using (_asArrayLock.CreateWriteLock())
                        _asArray[rank] = new IkvmReflectionTypeSymbol(_context, _module, _elementType.UnderlyingType.MakeArrayType(rank));

                return _asArray[rank] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionTypeSymbol"/> representing a SZArray of this type.
        /// </summary>
        /// <returns></returns>
        public IIkvmReflectionTypeSymbol GetOrCreateSZArrayTypeSymbol()
        {
            if (_asSZArray == null)
                Interlocked.CompareExchange(ref _asSZArray, new IkvmReflectionTypeSymbol(_context, _module, _elementType.UnderlyingType.MakeArrayType()), null);

            return _asSZArray;
        }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionTypeSymbol"/> representing a pointer of this type.
        /// </summary>
        /// <returns></returns>
        public IIkvmReflectionTypeSymbol GetOrCreatePointerTypeSymbol()
        {
            if (_asPointer == null)
                Interlocked.CompareExchange(ref _asPointer, new IkvmReflectionTypeSymbol(_context, _module, _elementType.UnderlyingType.MakePointerType()), null);

            return _asPointer;
        }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionTypeSymbol"/> representing a by-ref of this type.
        /// </summary>
        /// <returns></returns>
        public IIkvmReflectionTypeSymbol GetOrCreateByRefTypeSymbol()
        {
            if (_asByRef == null)
                Interlocked.CompareExchange(ref _asByRef, new IkvmReflectionTypeSymbol(_context, _module, _elementType.UnderlyingType.MakeByRefType()), null);

            return _asByRef;
        }

        /// <summary>
        /// Gets or creates a <see cref="IIkvmReflectionTypeSymbol"/> representing a specialized generic of this type.
        /// </summary>
        /// <param name="genericTypeArguments"></param>
        /// <returns></returns>
        public IIkvmReflectionTypeSymbol GetOrCreateGenericTypeSymbol(Type[] genericTypeArguments)
        {
            if (genericTypeArguments is null)
                throw new ArgumentNullException(nameof(genericTypeArguments));

            if (_elementType.IsGenericTypeDefinition == false)
                throw new InvalidOperationException();

            if (_genericTypeSymbols == null)
                Interlocked.CompareExchange(ref _genericTypeSymbols, new(TypeSymbolListEqualityComparer.Instance), null);

            return _genericTypeSymbols.GetOrAdd(_module.ResolveTypeSymbols(genericTypeArguments), CreateGenericTypeSymbol);
        }

        /// <summary>
        /// Creates a new <see cref="IIkvmReflectionTypeSymbol"/> representing a specialized generic of this type.
        /// </summary>
        /// <param name="genericTypeArguments"></param>
        /// <returns></returns>
        readonly IIkvmReflectionTypeSymbol CreateGenericTypeSymbol(IIkvmReflectionTypeSymbol[] genericTypeArguments)
        {
            return new IkvmReflectionTypeSymbol(_context, _module, _elementType.UnderlyingType.MakeGenericType(genericTypeArguments.Unpack()));
        }
    }

}
