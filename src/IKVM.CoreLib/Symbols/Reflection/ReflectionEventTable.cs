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

    struct ReflectionEventTable
    {

        readonly ReflectionSymbolContext _context;
        readonly IReflectionModuleSymbol _module;
        readonly IReflectionTypeSymbol _type;

        IndexRangeDictionary<IReflectionEventSymbol> _table = new();
        ReaderWriterLockSlim? _lock;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionEventTable(ReflectionSymbolContext context, IReflectionModuleSymbol module, IReflectionTypeSymbol type)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _type = type ?? throw new ArgumentNullException(nameof(type));
        }

        /// <summary>
        /// Gets or creates the <see cref="IReflectionEventSymbol"/> cached for the type by event.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public IReflectionEventSymbol GetOrCreateEventSymbol(EventInfo @event)
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
                var row = @event.GetMetadataTokenRowNumberSafe();
                if (_table[row] == null)
                    using (_lock.CreateWriteLock())
                        return _table[row] ??= new ReflectionEventSymbol(_context, _module, _type, @event);

                return _table[row] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates the <see cref="IReflectionEventSymbolBuilder"/> cached for the type by event.
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        public IReflectionEventSymbolBuilder GetOrCreateEventSymbol(EventBuilder @event)
        {
            if (@event is null)
                throw new ArgumentNullException(nameof(@event));
            if (@event.GetModuleBuilder() != _module.UnderlyingModule)
                throw new ArgumentException(nameof(@event));

            // create lock on demand
            if (_lock == null)
                Interlocked.CompareExchange(ref _lock, new ReaderWriterLockSlim(), null);

            using (_lock.CreateUpgradeableReadLock())
            {
                var row = @event.GetMetadataTokenRowNumberSafe();
                if (_table[row] == null)
                    using (_lock.CreateWriteLock())
                        return (IReflectionEventSymbolBuilder)(_table[row] ??= new ReflectionEventSymbolBuilder(_context, (IReflectionModuleSymbolBuilder)_module, (IReflectionTypeSymbolBuilder)_type, @event));

                return (IReflectionEventSymbolBuilder?)_table[row] ?? throw new InvalidOperationException();
            }
        }

    }

}
