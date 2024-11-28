#nullable enable

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

            AssemblySymbol _coreAssembly;
            readonly ConcurrentDictionary<string, TypeSymbol?> _coreTypeCache = new();

            AssemblySymbol[] _systemAssemblies;
            readonly ConcurrentDictionary<string, TypeSymbol?> _systemTypeCache = new();

            AssemblySymbol _runtimeAssembly;
            readonly ConcurrentDictionary<string, TypeSymbol?> _runtimeTypeCache = new();

            AssemblySymbol _baseAssembly;
            readonly ConcurrentDictionary<string, TypeSymbol?> _baseTypeCache = new();

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            public Resolver()
            {
                _coreAssembly = GetSymbol(typeof(object).Assembly);
                _systemAssemblies = GetSystemAssemblies().Distinct().ToArray().Select(GetSymbol).ToArray();
                _runtimeAssembly = GetSymbol(typeof(JVM).Assembly);
                _baseAssembly = GetSymbol(typeof(java.lang.Object).Assembly);
            }

            /// <inheritdoc />
            public SymbolContext Symbols => _symbols;

            /// <inheritdoc />
            public AssemblySymbol ResolveAssembly(string assemblyName)
            {
                return GetSymbol(Assembly.Load(assemblyName));
            }

            /// <inheritdoc />
            public AssemblySymbol GetCoreAssembly()
            {
                return _coreAssembly;
            }

            /// <inheritdoc />
            public TypeSymbol ResolveCoreType(string typeName)
            {
                return _coreTypeCache.GetOrAdd(typeName, ResolveCoreTypeImpl) ?? throw new InvalidOperationException();
            }

            /// <summary>
            /// Resolves the specified core type.
            /// </summary>
            /// <param name="typeName"></param>
            /// <returns></returns>
            TypeSymbol? ResolveCoreTypeImpl(string typeName)
            {
                return _coreAssembly.GetType(typeName);
            }

            /// <inheritdoc />
            public TypeSymbol ResolveSystemType(string typeName)
            {
                return _systemTypeCache.GetOrAdd(typeName, FindSystemType) ?? throw new InvalidOperationException();
            }

            /// <summary>
            /// Resolves the specified system type.
            /// </summary>
            /// <param name="typeName"></param>
            /// <returns></returns>
            TypeSymbol? FindSystemType(string typeName)
            {
                foreach (var assembly in _systemAssemblies)
                    if (assembly.GetType(typeName) is TypeSymbol t)
                        return t;

                return null;
            }

            /// <inheritdoc />
            public AssemblySymbol GetRuntimeAssembly()
            {
                return _runtimeAssembly;
            }

            /// <inheritdoc />
            public TypeSymbol ResolveRuntimeType(string typeName)
            {
                return _runtimeTypeCache.GetOrAdd(typeName, FindRuntimeType) ?? throw new InvalidOperationException();
            }

            /// <inheritdoc />
            public bool TryResolveRuntimeType(string typeName, out TypeSymbol? type)
            {
                var t = _runtimeTypeCache.GetOrAdd(typeName, FindRuntimeType);
                if (t != null)
                {
                    type = t;
                    return true;
                }

                type = null;
                return false;
            }

            /// <summary>
            /// Resolves the specified system type.
            /// </summary>
            /// <param name="typeName"></param>
            /// <returns></returns>
            TypeSymbol? FindRuntimeType(string typeName)
            {
                return _runtimeAssembly.GetType(typeName);
            }

            /// <inheritdoc />
            public AssemblySymbol GetBaseAssembly()
            {
                return _baseAssembly;
            }

            /// <inheritdoc />
            public TypeSymbol ResolveBaseType(string typeName)
            {
                return _baseAssembly.GetType(typeName) ?? throw new InvalidOperationException();
            }

            /// <inheritdoc />
            public TypeSymbol? ResolveType(string typeName)
            {
                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                    if (assembly.GetType(typeName) is Type t)
                        return GetSymbol(t);

                return null;
            }

            /// <inheritdoc />
            public AssemblySymbol GetSymbol(Assembly assembly)
            {
                return _symbols.GetOrCreateAssemblySymbol(assembly);
            }

            /// <inheritdoc />
            public AssemblySymbolBuilder GetSymbol(AssemblyBuilder assembly)
            {
                return _symbols.GetOrCreateAssemblySymbol(assembly);
            }

            /// <inheritdoc />
            public ModuleSymbol GetSymbol(Module module)
            {
                return _symbols.GetOrCreateModuleSymbol(module);
            }

            /// <inheritdoc />
            public ModuleSymbolBuilder GetSymbol(ModuleBuilder module)
            {
                return _symbols.GetOrCreateModuleSymbol(module);
            }

            /// <inheritdoc />
            public TypeSymbol GetSymbol(Type type)
            {
                return _symbols.GetOrCreateTypeSymbol(type);
            }

            /// <inheritdoc />
            public MemberSymbol GetSymbol(MemberInfo member)
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
            public MethodSymbol GetSymbol(MethodBase method)
            {
                return method switch
                {
                    ConstructorInfo ctor => GetSymbol(ctor),
                    MethodInfo mi => GetSymbol(mi),
                    _ => throw new InvalidOperationException(),
                };
            }

            /// <inheritdoc />
            public MethodSymbol GetSymbol(ConstructorInfo ctor)
            {
                return _symbols.GetOrCreateConstructorSymbol(ctor);
            }

            /// <inheritdoc />
            public MethodSymbol GetSymbol(MethodInfo method)
            {
                return _symbols.GetOrCreateMethodSymbol(method);
            }

            /// <inheritdoc />
            public FieldSymbol GetSymbol(FieldInfo field)
            {
                return _symbols.GetOrCreateFieldSymbol(field);
            }

            /// <inheritdoc />
            public PropertySymbol GetSymbol(PropertyInfo property)
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
