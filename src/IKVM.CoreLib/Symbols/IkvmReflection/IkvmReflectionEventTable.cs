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

    struct IkvmReflectionEventTable
    {

        readonly IkvmReflectionSymbolContext _context;
        readonly IIkvmReflectionModuleSymbol _module;
        readonly IIkvmReflectionTypeSymbol _type;

        IndexRangeDictionary<IIkvmReflectionEventSymbol> _table = new();
        ReaderWriterLockSlim? _lock;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReflectionEventTable(IkvmReflectionSymbolContext context, IIkvmReflectionModuleSymbol module, IIkvmReflectionTypeSymbol type)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _type = type ?? throw new ArgumentNullException(nameof(type));
        }

        /// <summary>
        /// Gets or creates the <see cref="IIkvmReflectionEventSymbol"/> cached for the type by event.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public IIkvmReflectionEventSymbol GetOrCreateEventSymbol(EventInfo @event)
        {
            if (@event is null)
                throw new ArgumentNullException(nameof(@event));
            if (@event.Module.MetadataToken != _module.MetadataToken)
                throw new ArgumentException(nameof(@event));

            // create lock on demand
            if (_lock == null)
                Interlocked.CompareExchange(ref _lock, new ReaderWriterLockSlim(), null);

            using (_lock.CreateUpgradeableReadLock())
            {
                var row = MetadataTokens.GetRowNumber(MetadataTokens.EventDefinitionHandle(@event.MetadataToken));
                if (_table[row] == null)
                    using (_lock.CreateWriteLock())
                        return _table[row] ??= new IkvmReflectionEventSymbol(_context, _module, _type, @event);
                else
                    return _table[row] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates the <see cref="IIkvmReflectionEventSymbolBuilder"/> cached for the type by event.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public IIkvmReflectionEventSymbolBuilder GetOrCreateEventSymbol(EventBuilder @event)
        {
            if (@event is null)
                throw new ArgumentNullException(nameof(@event));
            if (@event.Module.MetadataToken != _module.MetadataToken)
                throw new ArgumentException(nameof(@event));

            // create lock on demand
            if (_lock == null)
                Interlocked.CompareExchange(ref _lock, new ReaderWriterLockSlim(), null);

            using (_lock.CreateUpgradeableReadLock())
            {
                var row = MetadataTokens.GetRowNumber(MetadataTokens.EventDefinitionHandle(@event.GetEventToken().Token));
                if (_table[row] == null)
                    using (_lock.CreateWriteLock())
                        return (IIkvmReflectionEventSymbolBuilder)(_table[row] ??= new IkvmReflectionEventSymbolBuilder(_context, (IIkvmReflectionModuleSymbolBuilder)_module, (IIkvmReflectionTypeSymbolBuilder)_type, @event));
                else
                    return (IIkvmReflectionEventSymbolBuilder?)_table[row] ?? throw new InvalidOperationException();
            }
        }

    }

}
