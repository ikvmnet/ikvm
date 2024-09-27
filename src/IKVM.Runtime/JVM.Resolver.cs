using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

using IKVM.CoreLib.Symbols;
using IKVM.CoreLib.Symbols.Emit;
using IKVM.CoreLib.Symbols.Reflection;

namespace IKVM.Runtime
{

#if FIRST_PASS == false && IMPORTER == false && EXPORTER == false

    static partial class JVM
    {

        /// <summary>
        /// Provides support for resolving managed types from the current JVM environment.
        /// </summary>
        internal class Resolver : IRuntimeSymbolResolver
        {

            /// <summary>
            /// Gets the set of assemblies from which to load core types.
            /// </summary>
            /// <returns></returns>
            static IEnumerable<Assembly> GetSystemAssemblies()
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

            readonly ReflectionSymbolContext _symbols = new();

            IAssemblySymbol _coreAssembly;
            readonly ConcurrentDictionary<string, ITypeSymbol> _coreTypeCache = new();

            IAssemblySymbol[] _systemAssemblies;
            readonly ConcurrentDictionary<string, ITypeSymbol> _systemTypeCache = new();

            IAssemblySymbol _runtimeAssembly;
            readonly ConcurrentDictionary<string, ITypeSymbol> _runtimeTypeCache = new();

            IAssemblySymbol _baseAssembly;
            readonly ConcurrentDictionary<string, ITypeSymbol> _baseTypeCache = new();

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            public Resolver()
            {
                _systemAssemblies = GetSystemAssemblies().Distinct().ToArray().Select(GetSymbol).ToArray();
                _runtimeAssembly = GetSymbol(typeof(JVM).Assembly);
            }

            /// <inheritdoc />
            public ISymbolContext Symbols => _symbols;

            /// <inheritdoc />
            public IAssemblySymbol ResolveAssembly(string assemblyName)
            {
                return GetSymbol(Assembly.Load(assemblyName));
            }

            /// <inheritdoc />
            public IAssemblySymbol GetCoreAssembly()
            {
                return _coreAssembly ??= GetSymbol(typeof(object).Assembly);
            }

            /// <inheritdoc />
            public ITypeSymbol ResolveCoreType(string typeName)
            {
                return _coreTypeCache.GetOrAdd(typeName, ResolveCoreTypeImpl);
            }

            /// <summary>
            /// Resolves the specified core type.
            /// </summary>
            /// <param name="typeName"></param>
            /// <returns></returns>
            ITypeSymbol ResolveCoreTypeImpl(string typeName)
            {
                return _coreAssembly.GetType(typeName);
            }

            /// <inheritdoc />
            public ITypeSymbol ResolveSystemType(string typeName)
            {
                return _systemTypeCache.GetOrAdd(typeName, ResolveSystemTypeImpl);
            }

            /// <summary>
            /// Resolves the specified system type.
            /// </summary>
            /// <param name="typeName"></param>
            /// <returns></returns>
            ITypeSymbol ResolveSystemTypeImpl(string typeName)
            {
                foreach (var assembly in _systemAssemblies)
                    if (assembly.GetType(typeName) is ITypeSymbol t)
                        return t;

                return null;
            }

            /// <inheritdoc />
            public IAssemblySymbol GetRuntimeAssembly()
            {
                return _runtimeAssembly;
            }

            /// <inheritdoc />
            public ITypeSymbol ResolveRuntimeType(string typeName)
            {
                return _runtimeTypeCache.GetOrAdd(typeName, ResolveRuntimeTypeImpl);
            }

            /// <summary>
            /// Resolves the specified system type.
            /// </summary>
            /// <param name="typeName"></param>
            /// <returns></returns>
            ITypeSymbol ResolveRuntimeTypeImpl(string typeName)
            {
                return _runtimeAssembly.GetType(typeName);
            }

            /// <inheritdoc />
            public IAssemblySymbol GetBaseAssembly()
            {
                return _baseAssembly;
            }

            /// <inheritdoc />
            public ITypeSymbol ResolveBaseType(string typeName)
            {
                return _baseAssembly.GetType(typeName);
            }

            /// <inheritdoc />
            public ITypeSymbol ResolveType(string typeName)
            {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                    if (assembly.GetType(typeName) is Type t)
                        return GetSymbol(t);

                return null;
            }

            /// <inheritdoc />
            public IAssemblySymbol GetSymbol(Assembly assembly)
            {
                return _symbols.GetOrCreateAssemblySymbol(assembly);
            }

            /// <inheritdoc />
            public IAssemblySymbolBuilder GetSymbol(AssemblyBuilder assembly)
            {
                return _symbols.GetOrCreateAssemblySymbol(assembly);
            }

            /// <inheritdoc />
            public IModuleSymbol GetSymbol(Module module)
            {
                return _symbols.GetOrCreateModuleSymbol(module);
            }

            /// <inheritdoc />
            public IModuleSymbolBuilder GetSymbol(ModuleBuilder module)
            {
                return _symbols.GetOrCreateModuleSymbol(module);
            }

            /// <inheritdoc />
            public ITypeSymbol GetSymbol(Type type)
            {
                return _symbols.GetOrCreateTypeSymbol(type);
            }

            /// <inheritdoc />
            public IMemberSymbol GetSymbol(MemberInfo member)
            {
                return member switch
                {
                    MethodBase method => GetSymbol(method),
                    FieldInfo field => GetSymbol(field),
                    PropertyInfo property => GetSymbol(property),
                    EventInfo @event => GetSymbol(@event),
                    _ => throw new InvalidOperationException(),
                };
            }

            /// <inheritdoc />
            public IMethodBaseSymbol GetSymbol(MethodBase method)
            {
                return method switch
                {
                    ConstructorInfo ctor => GetSymbol(ctor),
                    MethodInfo mi => GetSymbol(mi),
                    _ => throw new InvalidOperationException(),
                };
            }

            /// <inheritdoc />
            public IConstructorSymbol GetSymbol(ConstructorInfo ctor)
            {
                return _symbols.GetOrCreateConstructorSymbol(ctor);
            }

            /// <inheritdoc />
            public IMethodSymbol GetSymbol(MethodInfo method)
            {
                return _symbols.GetOrCreateMethodSymbol(method);
            }

            /// <inheritdoc />
            public IFieldSymbol GetSymbol(FieldInfo field)
            {
                return _symbols.GetOrCreateFieldSymbol(field);
            }

            /// <inheritdoc />
            public IPropertySymbol GetSymbol(PropertyInfo property)
            {
                return _symbols.GetOrCreatePropertySymbol(property);
            }

            /// <inheritdoc />
            public IEventSymbol GetSymbol(EventInfo @event)
            {
                return _symbols.GetOrCreateEventSymbol(@event);
            }

        }

    }

#endif

}
