using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

using IKVM.CoreLib.Collections;
using IKVM.CoreLib.Reflection;
using IKVM.CoreLib.Symbols.Reflection.Emit;
using IKVM.CoreLib.Threading;

namespace IKVM.CoreLib.Symbols.Reflection
{

    struct ReflectionFieldTable
    {

        readonly ReflectionSymbolContext _context;
        readonly IReflectionModuleSymbol _module;
        readonly IReflectionTypeSymbol? _type;

        IndexRangeDictionary<IReflectionFieldSymbol> _table = new();
        ReaderWriterLockSlim? _lock;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionFieldTable(ReflectionSymbolContext context, IReflectionModuleSymbol module, IReflectionTypeSymbol? type)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _type = type;
        }

        /// <summary>
        /// Gets or creates the <see cref="IReflectionFieldSymbol"/> cached for the type by field.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="requiredCustomModifiers"></param>
        /// <param name="optionalCustomModifiers"></param>
        /// <returns></returns>
        public IReflectionFieldSymbol GetOrCreateFieldSymbol(FieldInfo field, ITypeSymbol[]? requiredCustomModifiers, ITypeSymbol[]? optionalCustomModifiers)
        {
            if (field is null)
                throw new ArgumentNullException(nameof(field));
            if (field.Module.MetadataToken != _module.MetadataToken)
                throw new ArgumentException(nameof(field));

            // create lock on demand
            if (_lock == null)
                Interlocked.CompareExchange(ref _lock, new ReaderWriterLockSlim(), null);

            using (_lock.CreateUpgradeableReadLock())
            {
                var row = field.GetMetadataTokenRowNumberSafe();
                if (_table[row] == null)
                    using (_lock.CreateWriteLock())
                        if (field is FieldBuilder builder)
                            return _table[row] ??= new ReflectionFieldSymbolBuilder(_context, (IReflectionModuleSymbolBuilder)_module, (IReflectionTypeSymbolBuilder?)_type, builder, requiredCustomModifiers, optionalCustomModifiers);
                        else
                            return _table[row] ??= new ReflectionFieldSymbol(_context, _module, _type, field);

                return _table[row] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates the <see cref="IReflectionFieldSymbolBuilder"/> cached for the type by field.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="requiredCustomModifiers"></param>
        /// <param name="optionalCustomModifiers"></param>
        /// <returns></returns>
        public IReflectionFieldSymbolBuilder GetOrCreateFieldSymbol(FieldBuilder field, ITypeSymbol[]? requiredCustomModifiers, ITypeSymbol[]? optionalCustomModifiers)
        {
            return (IReflectionFieldSymbolBuilder)GetOrCreateFieldSymbol((FieldInfo)field, requiredCustomModifiers, optionalCustomModifiers);
        }

    }

}
