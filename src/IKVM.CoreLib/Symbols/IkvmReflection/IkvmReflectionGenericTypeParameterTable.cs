using System;
using System.Threading;

using IKVM.CoreLib.Collections;
using IKVM.CoreLib.Symbols.IkvmReflection.Emit;
using IKVM.CoreLib.Threading;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    struct IkvmReflectionGenericTypeParameterTable
    {

        readonly IkvmReflectionSymbolContext _context;
        readonly IIkvmReflectionModuleSymbol _module;
        readonly IIkvmReflectionMemberSymbol _member;

        IndexRangeDictionary<IIkvmReflectionTypeSymbol> _table = new();
        ReaderWriterLockSlim? _lock;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="member"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReflectionGenericTypeParameterTable(IkvmReflectionSymbolContext context, IIkvmReflectionModuleSymbol module, IIkvmReflectionMemberSymbol member)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _member = member;
        }

        /// <summary>
        /// Gets or creates the <see cref="IkvmReflectionTypeSymbol"/> cached for the module by type.
        /// </summary>
        /// <param name="genericTypeParameter"></param>
        /// <returns></returns>
        public IIkvmReflectionTypeSymbol GetOrCreateGenericTypeParameterSymbol(Type genericTypeParameter)
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
                            return _table[pos] ??= new IkvmReflectionGenericTypeParameterSymbolBuilder(_context, (IIkvmReflectionModuleSymbolBuilder)_module, (IIkvmReflectionMemberSymbolBuilder)_member, builder);
                        else
                            return _table[pos] ??= new IkvmReflectionGenericTypeParameterSymbol(_context, _module, _member, genericTypeParameter);

                return _table[pos] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates the <see cref="IIkvmReflectionGenericTypeParameterSymbolBuilder"/> cached for the module.
        /// </summary>
        /// <param name="genericTypeParameter"></param>
        /// <returns></returns>
        public IIkvmReflectionGenericTypeParameterSymbolBuilder GetOrCreateGenericTypeParameterSymbol(GenericTypeParameterBuilder genericTypeParameter)
        {
            return (IIkvmReflectionGenericTypeParameterSymbolBuilder)GetOrCreateGenericTypeParameterSymbol((Type)genericTypeParameter);
        }

    }

}
