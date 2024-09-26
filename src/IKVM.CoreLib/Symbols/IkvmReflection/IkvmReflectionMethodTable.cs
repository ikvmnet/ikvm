using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Threading;

using IKVM.CoreLib.Collections;
using IKVM.CoreLib.Symbols.IkvmReflection.Emit;
using IKVM.CoreLib.Threading;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    struct IkvmReflectionMethodTable
    {

        readonly IkvmReflectionSymbolContext _context;
        readonly IIkvmReflectionModuleSymbol _module;
        readonly IIkvmReflectionTypeSymbol? _type;

        IndexRangeDictionary<IIkvmReflectionMethodBaseSymbol> _table = new();
        ReaderWriterLockSlim? _lock;

        ConcurrentDictionary<string, IIkvmReflectionMethodBaseSymbol>? _byName;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        /// <param name="type"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReflectionMethodTable(IkvmReflectionSymbolContext context, IIkvmReflectionModuleSymbol module, IIkvmReflectionTypeSymbol? type)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _module = module ?? throw new ArgumentNullException(nameof(module));
            _type = type;
        }

        /// <summary>
        /// Gets or creates the <see cref="IkvmReflectionMethodSymbol"/> cached for the type by method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IIkvmReflectionMethodBaseSymbol GetOrCreateMethodBaseSymbol(MethodBase method)
        {
            if (method is null)
                throw new ArgumentNullException(nameof(method));
            if (method.Module.MetadataToken != _module.MetadataToken)
                throw new ArgumentException(nameof(method));
            if (method is MethodInfo m_ && m_.IsMethodDefinition() == false)
                throw new ArgumentException(nameof(method));

            var token = method.MetadataToken;
            if (token <= 0)
            {
                // we fall back to lookup by name if there is no valid metadata token
                if (_byName == null)
                    Interlocked.CompareExchange(ref _byName, new(), null);

                var context_ = _context;
                var module_ = _module;
                var type_ = _type;
                var method_ = method;
                return _byName.GetOrAdd(method.ToString() ?? throw new InvalidOperationException(), _ => CreateMethodBaseSymbol(context_, module_, type_, method_));
            }

            // create lock on demand
            if (_lock == null)
                Interlocked.CompareExchange(ref _lock, new(), null);

            using (_lock.CreateUpgradeableReadLock())
            {
                if (_table[token] == null)
                    using (_lock.CreateWriteLock())
                        return _table[token] ??= CreateMethodBaseSymbol(_context, _module, _type, method);

                return _table[token] ?? throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Creates a new symbol.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        static IIkvmReflectionMethodBaseSymbol CreateMethodBaseSymbol(IkvmReflectionSymbolContext context, IIkvmReflectionModuleSymbol module, IIkvmReflectionTypeSymbol? type, MethodBase method)
        {
            if (method is ConstructorInfo c)
            {
                if (type == null)
                    throw new InvalidOperationException();

                if (method is ConstructorBuilder builder)
                    return new IkvmReflectionConstructorSymbolBuilder(context, (IIkvmReflectionModuleSymbolBuilder)module, (IIkvmReflectionTypeSymbolBuilder)type, builder);
                else
                    return new IkvmReflectionConstructorSymbol(context, module, type, c);
            }
            else if (method is MethodInfo m)
            {
                if (method is MethodBuilder builder)
                    return new IkvmReflectionMethodSymbolBuilder(context, (IIkvmReflectionModuleSymbolBuilder)module, (IIkvmReflectionTypeSymbolBuilder?)type, builder);
                else
                    return new IkvmReflectionMethodSymbol(context, module, type, m);
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Gets or creates the <see cref="IIkvmReflectionConstructorSymbol"/> cached for the type by method.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        public IIkvmReflectionConstructorSymbol GetOrCreateConstructorSymbol(ConstructorInfo ctor)
        {
            return (IIkvmReflectionConstructorSymbol)GetOrCreateMethodBaseSymbol(ctor);
        }

        /// <summary>
        /// Gets or creates the <see cref="IIkvmReflectionConstructorSymbolBuilder"/> cached for the type by method.
        /// </summary>
        /// <param name="ctor"></param>
        /// <returns></returns>
        public IIkvmReflectionConstructorSymbolBuilder GetOrCreateConstructorSymbol(ConstructorBuilder ctor)
        {
            return (IIkvmReflectionConstructorSymbolBuilder)GetOrCreateMethodBaseSymbol(ctor);
        }

        /// <summary>
        /// Gets or creates the <see cref="IIkvmReflectionMethodSymbol"/> cached for the type by method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IIkvmReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method)
        {
            return (IIkvmReflectionMethodSymbol)GetOrCreateMethodBaseSymbol(method);
        }

        /// <summary>
        /// Gets or creates the <see cref="IIkvmReflectionMethodSymbolBuilder"/> cached for the type by method.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        public IIkvmReflectionMethodSymbolBuilder GetOrCreateMethodSymbol(MethodBuilder method)
        {
            return (IIkvmReflectionMethodSymbolBuilder)GetOrCreateMethodBaseSymbol(method);
        }

    }

}
