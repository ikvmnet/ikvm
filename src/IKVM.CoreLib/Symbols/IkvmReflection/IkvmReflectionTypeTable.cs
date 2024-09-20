using System;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

using IKVM.CoreLib.Collections;
using IKVM.CoreLib.Symbols.IkvmReflection.Emit;
using IKVM.CoreLib.Threading;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    struct IkvmReflectionTypeTable
    {

        readonly IkvmReflectionSymbolContext _context;
        readonly IIkvmReflectionModuleSymbol _module;
        readonly IIkvmReflectionTypeSymbol? _type;

        IndexRangeDictionary<IIkvmReflectionTypeSymbol> _table = new();
        ReaderWriterLockSlim? _lock;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReflectionTypeTable(IkvmReflectionSymbolContext context, IIkvmReflectionModuleSymbol module, IIkvmReflectionTypeSymbol? type)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _type = type;
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

            // create lock on demand
            if (_lock == null)
                Interlocked.CompareExchange(ref _lock, new ReaderWriterLockSlim(), null);

            using (_lock.CreateUpgradeableReadLock())
            {
                var row = MetadataTokens.GetRowNumber(MetadataTokens.TypeDefinitionHandle(type.MetadataToken));
                if (_table[row] == null)
                    using (_lock.CreateWriteLock())
                        if (_table[row] == null)
                            if (type is TypeBuilder builder)
                                return _table[row] = new IkvmReflectionTypeSymbolBuilder(_context, (IIkvmReflectionModuleSymbolBuilder)_module, builder);
                            else
                                return _table[row] = new IkvmReflectionTypeSymbol(_context, _module, type);

                return _table[row] ?? throw new InvalidOperationException();
            }
        }

    }

}
