using System;
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

            // create lock on demand
            if (_lock == null)
                Interlocked.CompareExchange(ref _lock, new ReaderWriterLockSlim(), null);

            using (_lock.CreateUpgradeableReadLock())
            {
                var row = MetadataTokens.GetRowNumber(MetadataTokens.FieldDefinitionHandle(field.MetadataToken));
                if (_table[row] == null)
                    using (_lock.CreateWriteLock())
                        if (field is FieldBuilder builder)
                            return _table[row] ??= new IkvmReflectionFieldSymbolBuilder(_context, (IIkvmReflectionModuleSymbolBuilder)_module, (IIkvmReflectionTypeSymbolBuilder?)_type, builder);
                        else
                            return _table[row] ??= new IkvmReflectionFieldSymbol(_context, _module, _type, field);
                else
                    return _table[row] ?? throw new InvalidOperationException();
            }
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
