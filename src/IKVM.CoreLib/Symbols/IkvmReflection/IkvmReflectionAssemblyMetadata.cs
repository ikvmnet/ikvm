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

    struct IkvmReflectionAssemblyMetadata
    {

        readonly IIkvmReflectionAssemblySymbol _symbol;

        IndexRangeDictionary<IIkvmReflectionModuleSymbol> _moduleSymbols = new(initialCapacity: 1, maxCapacity: 32);
        ReaderWriterLockSlim? _moduleLock;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="symbol"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReflectionAssemblyMetadata(IIkvmReflectionAssemblySymbol symbol)
        {
            _symbol = symbol ?? throw new ArgumentNullException(nameof(symbol));
        }

        /// <summary>
        /// Gets or creates the <see cref="IModuleSymbol"/> cached for the module.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        public IIkvmReflectionModuleSymbol GetOrCreateModuleSymbol(Module module)
        {
            if (module is null)
                throw new ArgumentNullException(nameof(module));

            // create lock on demand
            if (_moduleLock == null)
                Interlocked.CompareExchange(ref _moduleLock, new ReaderWriterLockSlim(), null);

            using (_moduleLock.CreateUpgradeableReadLock())
            {
                var row = MetadataTokens.GetRowNumber(MetadataTokens.ModuleReferenceHandle(module.MetadataToken));
                if (_moduleSymbols[row] == null)
                    using (_moduleLock.CreateWriteLock())
                        if (module is ModuleBuilder builder)
                            return _moduleSymbols[row] = new IkvmReflectionModuleSymbolBuilder(_symbol.Context, _symbol, builder);
                        else
                            return _moduleSymbols[row] = new IkvmReflectionModuleSymbol(_symbol.Context, _symbol, module);
                else
                    return _moduleSymbols[row] ?? throw new InvalidOperationException();
            }
        }

    }

}
