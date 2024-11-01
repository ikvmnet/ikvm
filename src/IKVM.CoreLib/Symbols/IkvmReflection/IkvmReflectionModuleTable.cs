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

    struct IkvmReflectionModuleTable
    {

        readonly IkvmReflectionSymbolContext _context;
        readonly IIkvmReflectionAssemblySymbol _assembly;

        IndexRangeDictionary<IIkvmReflectionModuleSymbol> _moduleSymbols = new();
        ReaderWriterLockSlim? _moduleLock;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReflectionModuleTable(IkvmReflectionSymbolContext context, IIkvmReflectionAssemblySymbol assembly)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
        }

        /// <summary>
        /// Gets or creates the <see cref="IIkvmReflectionModuleSymbol"/> cached for the module.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
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
                            _moduleSymbols[row] = new IkvmReflectionModuleSymbolBuilder(_context, _assembly, builder);
                        else
                            _moduleSymbols[row] = new IkvmReflectionModuleSymbol(_context, _assembly, module);

                return _moduleSymbols[row] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates the <see cref="IIkvmReflectionModuleSymbolBuilder"/> cached for the module.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public IIkvmReflectionModuleSymbolBuilder GetOrCreateModuleSymbol(ModuleBuilder module)
        {
            return (IIkvmReflectionModuleSymbolBuilder)GetOrCreateModuleSymbol((Module)module);
        }

    }

}
