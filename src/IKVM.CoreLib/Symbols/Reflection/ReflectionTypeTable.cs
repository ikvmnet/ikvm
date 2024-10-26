using System;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

using IKVM.CoreLib.Collections;
using IKVM.CoreLib.Reflection;
using IKVM.CoreLib.Symbols.Reflection.Emit;
using IKVM.CoreLib.Threading;

namespace IKVM.CoreLib.Symbols.Reflection
{

    struct ReflectionTypeTable
    {

        readonly ReflectionSymbolContext _context;
        readonly IReflectionModuleSymbol _module;
        readonly IReflectionTypeSymbol? _type;

        IndexRangeDictionary<IReflectionTypeSymbol> _table = new();
        ReaderWriterLockSlim? _lock;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionTypeTable(ReflectionSymbolContext context, IReflectionModuleSymbol module, IReflectionTypeSymbol? type)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _type = type;
        }

        /// <summary>
        /// Gets or creates the <see cref="IReflectionTypeSymbol"/> cached for the module by type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IReflectionTypeSymbol GetOrCreateTypeSymbol(Type type)
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
                var row = type.GetMetadataTokenRowNumberSafe();
                if (_table[row] == null)
                    using (_lock.CreateWriteLock())
                        if (_table[row] == null)
                            if (type is TypeBuilder builder)
                                return _table[row] = new ReflectionTypeSymbolBuilder(_context, (IReflectionModuleSymbolBuilder)_module, builder);
                            else
                                return _table[row] = new ReflectionTypeDefSymbol(_context, _module, type);

                return _table[row] ?? throw new InvalidOperationException();
            }
        }

    }

}
