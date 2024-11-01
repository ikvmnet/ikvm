using System;
using System.Diagnostics.Eventing.Reader;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;

using IKVM.CoreLib.Collections;
using IKVM.CoreLib.Reflection;
using IKVM.CoreLib.Symbols.Reflection.Emit;
using IKVM.CoreLib.Threading;

namespace IKVM.CoreLib.Symbols.Reflection
{

    struct ReflectionMethodTable
    {

        readonly ReflectionSymbolContext _context;
        readonly IReflectionModuleSymbol _module;
        readonly IReflectionTypeSymbol? _type;

        IndexRangeDictionary<IReflectionMethodBaseSymbol> _table = new();
        ReaderWriterLockSlim? _lock;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionMethodTable(ReflectionSymbolContext context, IReflectionModuleSymbol module, IReflectionTypeSymbol? type)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _type = type;
        }

        /// <summary>
        /// Gets or creates the <see cref="ReflectionMethodSymbol"/> cached for the type by method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IReflectionMethodBaseSymbol GetOrCreateMethodBaseSymbol(MethodBase method)
        {
            if (method is null)
                throw new ArgumentNullException(nameof(method));
            if (method.Module.MetadataToken != _module.MetadataToken)
                throw new ArgumentException(nameof(method));
            if (method is MethodInfo m_ && m_.IsMethodDefinition() == false)
                throw new ArgumentException(nameof(method));

            // create lock on demand
            if (_lock == null)
                Interlocked.CompareExchange(ref _lock, new(), null);

            using (_lock.CreateUpgradeableReadLock())
            {
                var row = method.GetMetadataTokenRowNumberSafe();
                if (_table[row] == null)
                    using (_lock.CreateWriteLock())
                        return _table[row] ??= CreateMethodBaseSymbol(_context, _module, _type, method);

                return _table[row] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Creates a new symbol.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        static IReflectionMethodBaseSymbol CreateMethodBaseSymbol(ReflectionSymbolContext context, IReflectionModuleSymbol module, IReflectionTypeSymbol? type, MethodBase method)
        {
            if (method is ConstructorInfo c)
            {
                if (type == null)
                    throw new InvalidOperationException();

                if (method is ConstructorBuilder builder)
                    return new ReflectionConstructorSymbolBuilder(context, (IReflectionModuleSymbolBuilder)module, (IReflectionTypeSymbolBuilder)type, builder);
                else
                    return new ReflectionConstructorSymbol(context, module, type, c);
            }
            else if (method is MethodInfo m)
            {
                if (method is MethodBuilder builder)
                    return new ReflectionMethodSymbolBuilder(context, (IReflectionModuleSymbolBuilder)module, (IReflectionTypeSymbolBuilder?)type, builder);
                else
                    return new ReflectionMethodSymbol(context, module, type, m);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates the <see cref="IReflectionConstructorSymbol"/> cached for the type by method.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        public IReflectionConstructorSymbol GetOrCreateConstructorSymbol(ConstructorInfo ctor)
        {
            return (IReflectionConstructorSymbol)GetOrCreateMethodBaseSymbol(ctor);
        }

        /// <summary>
        /// Gets or creates the <see cref="IReflectionConstructorSymbolBuilder"/> cached for the type by method.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        public IReflectionConstructorSymbolBuilder GetOrCreateConstructorSymbol(ConstructorBuilder ctor)
        {
            return (IReflectionConstructorSymbolBuilder)GetOrCreateMethodBaseSymbol(ctor);
        }

        /// <summary>
        /// Gets or creates the <see cref="IReflectionMethodSymbol"/> cached for the type by method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method)
        {
            return (IReflectionMethodSymbol)GetOrCreateMethodBaseSymbol(method);
        }

        /// <summary>
        /// Gets or creates the <see cref="IReflectionMethodSymbolBuilder"/> cached for the type by method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IReflectionMethodSymbolBuilder GetOrCreateMethodSymbol(MethodBuilder method)
        {
            return (IReflectionMethodSymbolBuilder)GetOrCreateMethodBaseSymbol(method);
        }

    }

}
