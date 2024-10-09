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

    struct ReflectionParameterTable
    {

        readonly ReflectionSymbolContext _context;
        readonly IReflectionModuleSymbol _module;
        readonly IReflectionMemberSymbol _member;

        IndexRangeDictionary<IReflectionParameterSymbol> _table = new();
        ReaderWriterLockSlim? _lock;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="member"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionParameterTable(ReflectionSymbolContext context, IReflectionModuleSymbol module, IReflectionMemberSymbol member)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _member = member ?? throw new ArgumentNullException(nameof(member));
        }

        /// <summary>
        /// Gets or creates the <see cref="IReflectionParameterSymbol"/> cached for the type by method.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter)
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
                if (_table[position] == null)
                    using (_lock.CreateWriteLock())
                        _table[position] ??= new ReflectionParameterSymbol(_context, _module, _member, parameter);

                var item = _table[position] ?? throw new InvalidOperationException();
                if (item.IsComplete == false && item is IReflectionParameterSymbolBuilder builder)
                    builder.OnComplete(parameter);

                return item;
            }
        }

        /// <summary>
        /// Gets or creates the <see cref="IReflectionParameterSymbolBuilder"/> cached for the type by method.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public IReflectionParameterSymbolBuilder GetOrCreateParameterSymbol(ParameterBuilder parameter)
        {
            if (parameter is null)
                throw new ArgumentNullException(nameof(parameter));
            if (parameter.GetMethodBuilder().Module != _module.UnderlyingModule)
                throw new ArgumentException(nameof(parameter));

            // create lock on demand
            if (_lock == null)
                Interlocked.CompareExchange(ref _lock, new ReaderWriterLockSlim(), null);

            using (_lock.CreateUpgradeableReadLock())
            {
                var position = parameter.Position - 1;
                if (_table[position] == null)
                    using (_lock.CreateWriteLock())
                        _table[position] ??= new ReflectionParameterSymbolBuilder(_context, _module, (IReflectionMethodBaseSymbol)_member, parameter);

                return (IReflectionParameterSymbolBuilder?)_table[position] ?? throw new InvalidOperationException();
            }
        }

    }

}
