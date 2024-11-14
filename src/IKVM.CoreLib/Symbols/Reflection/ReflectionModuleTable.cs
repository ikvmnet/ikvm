using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

using IKVM.CoreLib.Collections;
using IKVM.CoreLib.Reflection;
using IKVM.CoreLib.Threading;

namespace IKVM.CoreLib.Symbols.Reflection
{

    struct ReflectionModuleTable
    {

        readonly ReflectionSymbolContext _context;
        readonly ReflectionAssemblySymbol _assembly;

        IndexRangeDictionary<ReflectionModuleSymbol> _moduleSymbols = new();
        ReaderWriterLockSlim? _moduleLock;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="assembly"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionModuleTable(ReflectionSymbolContext context, ReflectionAssemblySymbol assembly)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
        }

        /// <summary>
        /// Gets or creates the <see cref="ReflectionModuleSymbol"/> cached for the module.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        public ReflectionModuleSymbol GetOrCreateModuleSymbol(Module module)
        {
            if (module is null)
                throw new ArgumentNullException(nameof(module));
            if (module is ModuleBuilder)
                throw new ArgumentException(nameof(module));

            // create lock on demand
            if (_moduleLock == null)
                Interlocked.CompareExchange(ref _moduleLock, new ReaderWriterLockSlim(), null);

            using (_moduleLock.CreateUpgradeableReadLock())
            {
                var row = module.GetMetadataTokenRowNumberSafe();
                if (_moduleSymbols[row] == null)
                    using (_moduleLock.CreateWriteLock())
                        return _moduleSymbols[row] = new ReflectionModuleSymbol(_context, _assembly, module);

                return _moduleSymbols[row] ?? throw new InvalidOperationException();
            }
        }

    }

}
