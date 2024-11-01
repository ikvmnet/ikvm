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

    struct IkvmReflectionEventTable
    {

        readonly IkvmReflectionSymbolContext _context;
        readonly IIkvmReflectionModuleSymbol _module;
        readonly IIkvmReflectionTypeSymbol _type;

        IndexRangeDictionary<IIkvmReflectionEventSymbol> _table = new();
        ReaderWriterLockSlim? _lock;

        ConcurrentDictionary<int, IIkvmReflectionEventSymbol>? _byToken;

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

            var token = @event is EventBuilder b ? b.GetEventToken().Token : @event.MetadataToken;
            if (token == 0)
                throw new InvalidOperationException();

            // pseudo tokens: since IKVM.Reflection does not remove pseudo tokens after creation, we can keep using these
            // but they are non-sequential per-type, so we need to use a real dictionary
            if (token < 0)
            {
                // we fall back to lookup by name if there is no valid metadata token
                if (_byToken == null)
                    Interlocked.CompareExchange(ref _byToken, new(), null);

                var value = (_context, _module, _type, @event);
                return _byToken.GetOrAdd(token, _ => CreateEventSymbol(value._context, value._module, value._type, value.@event));
            }

            // create lock on demand
            if (_lock == null)
                Interlocked.CompareExchange(ref _lock, new(), null);

            using (_lock.CreateUpgradeableReadLock())
            {
                var row = MetadataTokens.GetRowNumber(MetadataTokens.EventDefinitionHandle(token));
                if (_table[row] == null)
                    using (_lock.CreateWriteLock())
                        _table[row] ??= CreateEventSymbol(_context, _module, _type, @event);

                return _table[row] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Creates a new symbol.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <param name="event"></param>
        /// <returns></returns>
        static IIkvmReflectionEventSymbol CreateEventSymbol(IkvmReflectionSymbolContext context, IIkvmReflectionModuleSymbol module, IIkvmReflectionTypeSymbol type, EventInfo @event)
        {
            if (@event is EventBuilder builder)
                return new IkvmReflectionEventSymbolBuilder(context, (IIkvmReflectionModuleSymbolBuilder)module, (IIkvmReflectionTypeSymbolBuilder)type, builder);
            else
                return new IkvmReflectionEventSymbol(context, module, type, @event);
        }

        /// <summary>
        /// Gets or creates the <see cref="IIkvmReflectionEventSymbolBuilder"/> cached for the type by property.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public IIkvmReflectionEventSymbolBuilder GetOrCreateEventSymbol(EventBuilder property)
        {
            return (IIkvmReflectionEventSymbolBuilder)GetOrCreateEventSymbol((EventInfo)property);
        }

    }

}
