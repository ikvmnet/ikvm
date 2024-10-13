using System;
using System.Collections.Concurrent;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

using IKVM.CoreLib.Collections;
using IKVM.CoreLib.Symbols.IkvmReflection.Emit;
using IKVM.CoreLib.Threading;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    struct IkvmReflectionPropertyTable
    {

        readonly IkvmReflectionSymbolContext _context;
        readonly IIkvmReflectionModuleSymbol _module;
        readonly IIkvmReflectionTypeSymbol _type;

        IndexRangeDictionary<IIkvmReflectionPropertySymbol> _table = new();
        ReaderWriterLockSlim? _lock;

        ConcurrentDictionary<int, IIkvmReflectionPropertySymbol>? _byToken;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReflectionPropertyTable(IkvmReflectionSymbolContext context, IIkvmReflectionModuleSymbol module, IIkvmReflectionTypeSymbol type)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _type = type ?? throw new ArgumentNullException(nameof(type));
        }

        /// <summary>
        /// Gets or creates the <see cref="IIkvmReflectionPropertySymbol"/> cached for the type by property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public IIkvmReflectionPropertySymbol GetOrCreatePropertySymbol(PropertyInfo property)
        {
            if (property is null)
                throw new ArgumentNullException(nameof(property));
            if (property.Module.MetadataToken != _module.MetadataToken)
                throw new ArgumentException(nameof(property));

            var token = property.MetadataToken;
            if (token == 0)
                throw new InvalidOperationException();

            // pseudo tokens: since IKVM.Reflection does not remove pseudo tokens after creation, we can keep using these
            // but they are non-sequential per-type, so we need to use a real dictionary
            if (token < 0)
            {
                // we fall back to lookup by name if there is no valid metadata token
                if (_byToken == null)
                    Interlocked.CompareExchange(ref _byToken, new(), null);

                var value = (_context, _module, _type, property);
                return _byToken.GetOrAdd(token, _ => CreatePropertySymbol(value._context, value._module, value._type, value.property));
            }

            // create lock on demand
            if (_lock == null)
                Interlocked.CompareExchange(ref _lock, new ReaderWriterLockSlim(), null);

            using (_lock.CreateUpgradeableReadLock())
            {
                var row = MetadataTokens.GetRowNumber(MetadataTokens.PropertyDefinitionHandle(token));
                if (_table[row] == null)
                    using (_lock.CreateWriteLock())
                        _table[row] ??= CreatePropertySymbol(_context, _module, _type, property);

                return _table[row] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Creates a new symbol.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        static IIkvmReflectionPropertySymbol CreatePropertySymbol(IkvmReflectionSymbolContext context, IIkvmReflectionModuleSymbol module, IIkvmReflectionTypeSymbol type, PropertyInfo property)
        {
            if (property is PropertyBuilder builder)
                return new IkvmReflectionPropertySymbolBuilder(context, (IIkvmReflectionModuleSymbolBuilder)module, (IIkvmReflectionTypeSymbolBuilder)type, builder);
            else
                return new IkvmReflectionPropertySymbol(context, module, type, property);
        }

        /// <summary>
        /// Gets or creates the <see cref="IIkvmReflectionPropertySymbolBuilder"/> cached for the type by property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public IIkvmReflectionPropertySymbolBuilder GetOrCreatePropertySymbol(PropertyBuilder property)
        {
            return (IIkvmReflectionPropertySymbolBuilder)GetOrCreatePropertySymbol((PropertyInfo)property);
        }

    }

}
