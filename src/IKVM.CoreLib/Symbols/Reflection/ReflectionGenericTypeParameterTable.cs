using System;
using System.Reflection.Emit;
using System.Threading;

using IKVM.CoreLib.Collections;
using IKVM.CoreLib.Symbols.Reflection.Emit;
using IKVM.CoreLib.Threading;

namespace IKVM.CoreLib.Symbols.Reflection
{

    struct ReflectionGenericTypeParameterTable
    {

        readonly ReflectionSymbolContext _context;
        readonly IReflectionModuleSymbol _module;
        readonly IReflectionMemberSymbol _member;

        IndexRangeDictionary<IReflectionTypeSymbol> _table = new();
        ReaderWriterLockSlim? _lock;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="member"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionGenericTypeParameterTable(ReflectionSymbolContext context, IReflectionModuleSymbol module, IReflectionMemberSymbol member)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _member = member;
        }

        /// <summary>
        /// Gets or creates the <see cref="ReflectionTypeSymbol"/> cached for the module by type.
        /// </summary>
        /// <param name="genericTypeParameter"></param>
        /// <returns></returns>
        public IReflectionTypeSymbol GetOrCreateGenericTypeParameterSymbol(Type genericTypeParameter)
        {
            if (genericTypeParameter is null)
                throw new ArgumentNullException(nameof(genericTypeParameter));
            if (genericTypeParameter.Module.MetadataToken != _module.MetadataToken)
                throw new ArgumentException(nameof(genericTypeParameter));

            // create lock on demand
            if (_lock == null)
                Interlocked.CompareExchange(ref _lock, new ReaderWriterLockSlim(), null);

            using (_lock.CreateUpgradeableReadLock())
            {
                var pos = genericTypeParameter.GenericParameterPosition;
                if (_table[pos] == null)
                    using (_lock.CreateWriteLock())
                        if (genericTypeParameter is GenericTypeParameterBuilder builder)
                            return _table[pos] ??= new ReflectionGenericTypeParameterSymbolBuilder(_context, (IReflectionModuleSymbolBuilder)_module, (IReflectionMemberSymbolBuilder)_member, builder);
                        else
                            return _table[pos] ??= new ReflectionGenericTypeParameterSymbol(_context, _module, _member, genericTypeParameter);

                return _table[pos] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates the <see cref="IReflectionGenericTypeParameterSymbolBuilder"/> cached for the module.
        /// </summary>
        /// <param name="genericTypeParameter"></param>
        /// <returns></returns>
        public IReflectionGenericTypeParameterSymbolBuilder GetOrCreateGenericTypeParameterSymbol(GenericTypeParameterBuilder genericTypeParameter)
        {
            return (IReflectionGenericTypeParameterSymbolBuilder)GetOrCreateGenericTypeParameterSymbol((Type)genericTypeParameter);
        }

    }

}
