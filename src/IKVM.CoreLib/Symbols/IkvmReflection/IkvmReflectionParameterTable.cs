using System;
using System.Threading;

using IKVM.CoreLib.Collections;
using IKVM.CoreLib.Symbols.IkvmReflection.Emit;
using IKVM.CoreLib.Threading;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    struct IkvmReflectionParameterTable
    {

        readonly IkvmReflectionSymbolContext _context;
        readonly IIkvmReflectionModuleSymbol _module;
        readonly IIkvmReflectionMemberSymbol _member;

        IndexRangeDictionary<IIkvmReflectionParameterSymbol> _table = new();
        ReaderWriterLockSlim? _lock;
        IIkvmReflectionParameterSymbol? _returnParameter;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="member"></param>
        /// <param name="property"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReflectionParameterTable(IkvmReflectionSymbolContext context, IIkvmReflectionModuleSymbol module, IIkvmReflectionMemberSymbol member)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _member = member ?? throw new ArgumentNullException(nameof(member));
        }

        /// <summary>
        /// Gets or creates the <see cref="IIkvmReflectionParameterSymbol"/> cached for the type by method.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IIkvmReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter)
        {
            if (parameter is null)
                throw new ArgumentNullException(nameof(parameter));
            if (parameter.Member.Module.MetadataToken != _module.MetadataToken)
                throw new ArgumentException(nameof(parameter));

            // create lock on demand
            if (_lock == null)
                Interlocked.CompareExchange(ref _lock, new ReaderWriterLockSlim(), null);

            using (_lock.CreateUpgradeableReadLock())
            {
                var position = parameter.Position;
                if (position == -1)
                    return _returnParameter ??= new IkvmReflectionParameterSymbol(_context, _module, _member, parameter);

                if (_table[position] == null)
                    using (_lock.CreateWriteLock())
                        _table[position] ??= new IkvmReflectionParameterSymbol(_context, _module, _member, parameter);

                return _table[position] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates the <see cref="IIkvmReflectionParameterSymbolBuilder"/> cached for the type by method.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IIkvmReflectionParameterSymbolBuilder GetOrCreateParameterSymbol(ParameterBuilder parameter)
        {
            if (parameter is null)
                throw new ArgumentNullException(nameof(parameter));
            if (parameter.Module.MetadataToken != _module.MetadataToken)
                throw new ArgumentException(nameof(parameter));

            // create lock on demand
            if (_lock == null)
                Interlocked.CompareExchange(ref _lock, new ReaderWriterLockSlim(), null);

            using (_lock.CreateUpgradeableReadLock())
            {
                var position = parameter.Position - 1;
                if (position == -1)
                    return (IIkvmReflectionParameterSymbolBuilder)(_returnParameter ??= new IkvmReflectionParameterSymbolBuilder(_context, _module, (IIkvmReflectionMethodBaseSymbol)_member, parameter));

                if (_table[position] == null)
                    using (_lock.CreateWriteLock())
                        return (IIkvmReflectionParameterSymbolBuilder)(_table[position] ??= new IkvmReflectionParameterSymbolBuilder(_context, _module, (IIkvmReflectionMethodBaseSymbol)_member, parameter));

                return (IIkvmReflectionParameterSymbolBuilder?)_table[position] ?? throw new InvalidOperationException();
            }
        }

    }

}
