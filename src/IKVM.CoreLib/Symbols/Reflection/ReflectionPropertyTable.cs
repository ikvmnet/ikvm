using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

using IKVM.CoreLib.Collections;
using IKVM.CoreLib.Reflection;
using IKVM.CoreLib.Symbols.Reflection.Emit;
using IKVM.CoreLib.Threading;

namespace IKVM.CoreLib.Symbols.Reflection
{

    struct ReflectionPropertyTable
    {

        readonly ReflectionSymbolContext _context;
        readonly IReflectionModuleSymbol _module;
        readonly IReflectionTypeSymbol _type;

        IndexRangeDictionary<IReflectionPropertySymbol> _table = new();
        ReaderWriterLockSlim? _lock;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionPropertyTable(ReflectionSymbolContext context, IReflectionModuleSymbol module, IReflectionTypeSymbol type)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _type = type ?? throw new ArgumentNullException(nameof(type));
        }

        /// <summary>
        /// Gets or creates the <see cref="IReflectionPropertySymbol"/> cached for the type by property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public IReflectionPropertySymbol GetOrCreatePropertySymbol(PropertyInfo property)
        {
            if (property is null)
                throw new ArgumentNullException(nameof(property));
            if (property.Module.MetadataToken != _module.MetadataToken)
                throw new ArgumentException(nameof(property));

            // create lock on demand
            if (_lock == null)
                Interlocked.CompareExchange(ref _lock, new ReaderWriterLockSlim(), null);

            using (_lock.CreateUpgradeableReadLock())
            {
                var row = property.GetMetadataTokenRowNumberSafe();
                if (_table[row] == null)
                    using (_lock.CreateWriteLock())
                        if (property is PropertyBuilder builder)
                            return _table[row] ??= new ReflectionPropertySymbolBuilder(_context, (IReflectionModuleSymbolBuilder)_module, (IReflectionTypeSymbolBuilder)_type, builder);
                        else
                            return _table[row] ??= new ReflectionPropertySymbol(_context, _module, _type, property);

                return _table[row] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates the <see cref="IReflectionPropertySymbolBuilder"/> cached for the type by property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public IReflectionPropertySymbolBuilder GetOrCreatePropertySymbol(PropertyBuilder property)
        {
            return (IReflectionPropertySymbolBuilder)GetOrCreatePropertySymbol((PropertyInfo)property);
        }

    }

}
