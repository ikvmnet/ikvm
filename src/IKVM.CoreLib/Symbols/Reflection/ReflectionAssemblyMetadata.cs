using System;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

using IKVM.CoreLib.Collections;
using IKVM.CoreLib.Reflection;
using IKVM.CoreLib.Symbols.Reflection.Emit;
using IKVM.CoreLib.Threading;

namespace IKVM.CoreLib.Symbols.Reflection
{

    struct ReflectionAssemblyMetadata
    {

        readonly IReflectionAssemblySymbol _symbol;

        IndexRangeDictionary<IReflectionModuleSymbol> _moduleSymbols = new(maxCapacity: 32);
        ReaderWriterLockSlim? _moduleLock;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="symbol"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionAssemblyMetadata(IReflectionAssemblySymbol symbol)
        {
            _symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
        }

        /// <summary>
        /// Gets or creates the <see cref="IModuleSymbol"/> cached for the module.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public IReflectionModuleSymbol GetOrCreateModuleSymbol(Module module)
        {
            if (module is null)
                throw new ArgumentNullException(nameof(module));

            Debug.Assert(AssemblyNameEqualityComparer.Instance.Equals(module.Assembly.GetName(), _symbol.UnderlyingAssembly.GetName()));

            // create lock on demand
            if (_moduleLock == null)
                Interlocked.CompareExchange(ref _moduleLock, new ReaderWriterLockSlim(), null);

            using (_moduleLock.CreateUpgradeableReadLock())
            {
                var row = module.GetMetadataTokenRowNumberSafe();
                if (_moduleSymbols[row] == null)
                    using (_moduleLock.CreateWriteLock())
                        if (module is ModuleBuilder builder)
                            return _moduleSymbols[row] = new ReflectionModuleSymbolBuilder(_symbol.Context, _symbol, builder);
                        else
                            return _moduleSymbols[row] = new ReflectionModuleSymbol(_symbol.Context, _symbol, module);
                else
                    return _moduleSymbols[row] ?? throw new InvalidOperationException();
            }
        }

    }

}
