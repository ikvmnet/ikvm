using System;
using System.Collections.Concurrent;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

using IKVM.CoreLib.Collections;
using IKVM.CoreLib.Symbols.IkvmReflection.Emit;
using IKVM.CoreLib.Threading;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    struct IkvmReflectionTypeTable
    {

        readonly IkvmReflectionSymbolContext _context;
        readonly IIkvmReflectionModuleSymbol _module;

        IndexRangeDictionary<IIkvmReflectionTypeSymbol> _table = new();
        ReaderWriterLockSlim? _lock;

        ConcurrentDictionary<string, IIkvmReflectionTypeSymbol>? _byName;
        ConcurrentDictionary<int, IIkvmReflectionTypeSymbol>? _byToken;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReflectionTypeTable(IkvmReflectionSymbolContext context, IIkvmReflectionModuleSymbol module)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _module = module ?? throw new ArgumentNullException(nameof(module));
        }

        /// <summary>
        /// Gets or creates the <see cref="IIkvmReflectionTypeSymbol"/> cached for the module by type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IIkvmReflectionTypeSymbol GetOrCreateTypeSymbol(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));
            if (type.Module.MetadataToken != _module.MetadataToken)
                throw new ArgumentException(nameof(type));
            if (type.IsGenericParameter)
                throw new ArgumentException(nameof(type));
            if (type.IsTypeDefinition() == false)
                throw new ArgumentException(nameof(type));

            // IKVM.Reflection may deliver a missing type, without a token, so we are forced to index this by name
            if (type.__IsMissing)
            {
                if (_byName == null)
                    Interlocked.CompareExchange(ref _byName, new(), null);

                var value = (_context, _module, type);
                return _byName.GetOrAdd(type.FullName, _ => CreateTypeSymbol(value._context, value._module, value.type));
            }

            var token = type.MetadataToken;
            if (token == 0)
                throw new InvalidOperationException();

            // pseudo tokens: since IKVM.Reflection does not remove pseudo tokens after creation, we can keep using these
            // but they are non-sequential per-type, so we need to use a real dictionary
            if (token < 0)
            {
                // we fall back to lookup by name if there is no valid metadata token
                if (_byToken == null)
                    Interlocked.CompareExchange(ref _byToken, new(), null);

                var value = (_context, _module, type);
                return _byToken.GetOrAdd(token, _ => CreateTypeSymbol(value._context, value._module, value.type));
            }

            // create lock on demand
            if (_lock == null)
                Interlocked.CompareExchange(ref _lock, new ReaderWriterLockSlim(), null);

            using (_lock.CreateUpgradeableReadLock())
            {
                var row = MetadataTokens.GetRowNumber(MetadataTokens.TypeDefinitionHandle(type.MetadataToken));
                if (_table[row] == null)
                    using (_lock.CreateWriteLock())
                        _table[row] ??= CreateTypeSymbol(_context, _module, type);

                return _table[row] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Creates a new symbol.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        static IIkvmReflectionTypeSymbol CreateTypeSymbol(IkvmReflectionSymbolContext context, IIkvmReflectionModuleSymbol module, Type type)
        {
            if (type is TypeBuilder builder)
                return new IkvmReflectionTypeSymbolBuilder(context, (IIkvmReflectionModuleSymbolBuilder)module, builder);
            else
                return new IkvmReflectionTypeSymbol(context, module, type);
        }

    }

}
