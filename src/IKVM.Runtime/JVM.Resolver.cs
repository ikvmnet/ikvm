﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using IKVM.CoreLib.Symbols;
using IKVM.CoreLib.Symbols.Reflection;

namespace IKVM.Runtime
{

#if FIRST_PASS == false && IMPORTER == false && EXPORTER == false

    static partial class JVM
    {

        /// <summary>
        /// Provides support for resolving managed types from the current JVM environment.
        /// </summary>
        internal class Resolver : ISymbolResolver
        {

            /// <summary>
            /// Gets the set of assemblies from which to load core types.
            /// </summary>
            /// <returns></returns>
            static IEnumerable<Assembly> GetCoreAssemblies()
            {
                yield return typeof(object).Assembly;
                yield return typeof(RuntimeCompatibilityAttribute).Assembly;
                yield return typeof(Console).Assembly;
                yield return typeof(System.Threading.Interlocked).Assembly;
                yield return typeof(EditorBrowsableAttribute).Assembly;
                yield return typeof(System.Collections.IEnumerable).Assembly;
                yield return typeof(IEnumerable<>).Assembly;
                yield return typeof(Environment).Assembly;
            }

            readonly static Assembly[] coreAssemblies = GetCoreAssemblies().Distinct().ToArray();

            readonly ReflectionSymbolContext _context = new();
            readonly IAssemblySymbol[] _coreAssemblies;
            readonly ConcurrentDictionary<string, TypeSymbol> _typeCache = new();

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            public Resolver()
            {
                _coreAssemblies = coreAssemblies.Select(_context.GetOrCreateAssemblySymbol).ToArray();
            }

            /// <inheritdoc />
            public IAssemblySymbol ResolveAssembly(string assemblyName)
            {
                return Assembly.Load(assemblyName) is { } a ? _context.GetOrCreateAssemblySymbol(a) : null;
            }

            /// <inheritdoc />
            public IAssemblySymbol ResolveBaseAssembly()
            {
                return _context.GetOrCreateAssemblySymbol(typeof(java.lang.Object).Assembly);
            }

            /// <inheritdoc />
            public TypeSymbol ResolveCoreType(string typeName)
            {
                return _typeCache.GetOrAdd(typeName, ResolveCoreTypeImpl);
            }

            TypeSymbol ResolveCoreTypeImpl(string typeName)
            {
                // loop over core assemblies searching for type
                foreach (var assembly in _coreAssemblies)
                    if (assembly.GetType(typeName) is TypeSymbol t)
                        return t;

                return null;
            }

            /// <inheritdoc />
            public TypeSymbol ResolveRuntimeType(string typeName)
            {
                return typeof(Resolver).Assembly.GetType(typeName) is { } t ? _context.GetOrCreateTypeSymbol(t) : null;
            }

        }

    }

#endif

}
