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

    struct IkvmReflectionFieldTable
    {

        readonly IkvmReflectionSymbolContext _context;
        readonly IIkvmReflectionModuleSymbol _module;
        readonly IIkvmReflectionTypeSymbol? _type;

        IndexRangeDictionary<IIkvmReflectionFieldSymbol> _table = new();
        ReaderWriterLockSlim? _lock;

        ConcurrentDictionary<int, IIkvmReflectionFieldSymbol>? _byToken;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReflectionFieldTable(IkvmReflectionSymbolContext context, IIkvmReflectionModuleSymbol module, IIkvmReflectionTypeSymbol? type)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _type = type;
        }

        /// <summary>
        /// Gets or creates the <see cref="IIkvmReflectionFieldSymbol"/> cached for the type by field.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public IIkvmReflectionFieldSymbol GetOrCreateFieldSymbol(FieldInfo field)
        {
            if (field is null)
                throw new ArgumentNullException(nameof(field));
            if (field.Module.MetadataToken != _module.MetadataToken)
                throw new ArgumentException(nameof(field));

            var token = field.MetadataToken;
            if (token == 0)
                throw new InvalidOperationException();

            // pseudo tokens: since IKVM.Reflection does not remove pseudo tokens after creation, we can keep using these
            // but they are non-sequential per-type, so we need to use a real dictionary
            if (token < 0)
            {
                // we fall back to lookup by name if there is no valid metadata token
                if (_byToken == null)
                    Interlocked.CompareExchange(ref _byToken, new(), null);

                var value = (_context, _module, _type, field);
                return _byToken.GetOrAdd(token, _ => CreateFieldSymbol(value._context, value._module, value._type, value.field));
            }

            // create lock on demand
            if (_lock == null)
                Interlocked.CompareExchange(ref _lock, new ReaderWriterLockSlim(), null);

            using (_lock.CreateUpgradeableReadLock())
            {
                var row = MetadataTokens.GetRowNumber(MetadataTokens.FieldDefinitionHandle(token));
                if (_table[row] == null)
                    using (_lock.CreateWriteLock())
                        _table[row] ??= CreateFieldSymbol(_context, _module, _type, field);

                return _table[row] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Creates a new symbol.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        static IIkvmReflectionFieldSymbol CreateFieldSymbol(IkvmReflectionSymbolContext context, IIkvmReflectionModuleSymbol module, IIkvmReflectionTypeSymbol? type, FieldInfo field)
        {
            if (field is FieldBuilder builder)
                return new IkvmReflectionFieldSymbolBuilder(context, (IIkvmReflectionModuleSymbolBuilder)module, (IIkvmReflectionTypeSymbolBuilder?)type, builder);
            else
                return new IkvmReflectionFieldSymbol(context, module, type, field);
        }

        /// <summary>
        /// Gets or creates the <see cref="IIkvmReflectionFieldSymbolBuilder"/> cached for the type by field.
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public IIkvmReflectionFieldSymbolBuilder GetOrCreateFieldSymbol(FieldBuilder field)
        {
            return (IIkvmReflectionFieldSymbolBuilder)GetOrCreateFieldSymbol((FieldInfo)field);
        }

    }

}
