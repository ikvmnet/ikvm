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

    struct ReflectionModuleTable
    {

        readonly ReflectionSymbolContext _context;
        readonly IReflectionAssemblySymbol _assembly;

        IndexRangeDictionary<IReflectionModuleSymbol> _moduleSymbols = new();
        ReaderWriterLockSlim? _moduleLock;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionModuleTable(ReflectionSymbolContext context, IReflectionAssemblySymbol assembly)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
        }

        /// <summary>
        /// Gets or creates the <see cref="IReflectionModuleSymbol"/> cached for the module.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public IReflectionModuleSymbol GetOrCreateModuleSymbol(Module module)
        {
            if (module is null)
                throw new ArgumentNullException(nameof(module));

            // create lock on demand
            if (_moduleLock == null)
                Interlocked.CompareExchange(ref _moduleLock, new ReaderWriterLockSlim(), null);

            using (_moduleLock.CreateUpgradeableReadLock())
            {
                var row = module.GetMetadataTokenRowNumberSafe();
                if (_moduleSymbols[row] == null)
                    using (_moduleLock.CreateWriteLock())
                        if (module is ModuleBuilder builder)
                            return _moduleSymbols[row] = new ReflectionModuleSymbolBuilder(_context, _assembly, builder);
                        else
                            return _moduleSymbols[row] = new ReflectionModuleSymbol(_context, _assembly, module);
                    
                return _moduleSymbols[row] ?? throw new InvalidOperationException();
            }
        }

    }

}
