#nullable enable

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

using IKVM.CoreLib.Diagnostics;
using IKVM.CoreLib.Symbols;
using IKVM.CoreLib.Symbols.Emit;
using IKVM.CoreLib.Symbols.IkvmReflection;
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using IKVM.Runtime;

using Type = IKVM.Reflection.Type;

namespace IKVM.Tools.Importer
{

    class ImportRuntimeSymbolResolver : IRuntimeSymbolResolver
    {

        /// <summary>
        /// Gets the set of types from which to discover system assemblies.
        /// </summary>
        /// <returns></returns>
        static IEnumerable<string> GetSystemTypeNames()
        {
            yield return "System.Object";
            yield return "System.Runtime.CompilerServices.RuntimeCompatibilityAttribute";
            yield return "System.Console";
            yield return "System.Threading.Interlocked";
            yield return "System.ComponentModel.EditorBrowsableAttribute";
            yield return "System.Collections.IEnumerable";
            yield return "System.Collections.Generic.IEnumerable`1";
            yield return "System.Environment";
        }

        readonly IDiagnosticHandler _diagnostics;
        readonly Universe _universe;
        readonly IkvmReflectionSymbolContext _symbols;
        readonly bool _bootstrap;

        IAssemblySymbol? _coreAssembly;
        readonly ConcurrentDictionary<string, ITypeSymbol?> _coreTypeCache = new();

        IAssemblySymbol[]? _systemAssemblies;
        readonly ConcurrentDictionary<string, ITypeSymbol?> _systemTypeCache = new();

        IAssemblySymbol? _runtimeAssembly;
        readonly ConcurrentDictionary<string, ITypeSymbol?> _runtimeTypeCache = new();

        IAssemblySymbol? _baseAssembly;
        readonly ConcurrentDictionary<string, ITypeSymbol?> _baseTypeCache = new();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="diagnostics"></param>
        /// <param name="universe"></param>
        /// <param name="symbols"></param>
        /// <param name="bootstrap"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ImportRuntimeSymbolResolver(IDiagnosticHandler diagnostics, Universe universe, IkvmReflectionSymbolContext symbols, bool bootstrap)
        {
            _diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
            _universe = universe ?? throw new ArgumentNullException(nameof(universe));
            _symbols = symbols ?? throw new ArgumentNullException(nameof(symbols));
            _bootstrap = bootstrap;
        }

        /// <inheritdoc />
        public ISymbolContext Symbols => _symbols;

        /// <inheritdoc />
        public IAssemblySymbol GetCoreAssembly()
        {
            return (_coreAssembly ??= GetSymbol(_universe.CoreLib)) ?? throw new DiagnosticEventException(DiagnosticEvent.CoreClassesMissing());
        }

        /// <inheritdoc />
        public IAssemblySymbol GetRuntimeAssembly()
        {
            return (_runtimeAssembly ??= FindRuntimeAssembly()) ?? throw new DiagnosticEventException(DiagnosticEvent.RuntimeNotFound());
        }

        /// <summary>
        /// Attempts to load the runtime assembly.
        /// </summary>
        IAssemblySymbol? FindRuntimeAssembly()
        {
            foreach (var assembly in _universe.GetAssemblies())
                if (assembly.GetType("IKVM.Runtime.JVM") is Type)
                    return GetSymbol(assembly);

            return null;
        }

        /// <inheritdoc />
        public IAssemblySymbol? GetBaseAssembly()
        {
            // bootstrap mode is used for builing the main assembly, and thus should not return the existing base assembly
            if (_bootstrap)
                return null;

            return (_baseAssembly ??= FindBaseAssembly()) ?? throw new DiagnosticEventException(DiagnosticEvent.BootstrapClassesMissing());
        }

        /// <summary>
        /// Attempts to load the base assembly.
        /// </summary>
        IAssemblySymbol? FindBaseAssembly()
        {
            foreach (var assembly in _universe.GetAssemblies())
                if (assembly.GetType("java.lang.Object") is Type)
                    return GetSymbol(assembly);

            return null;
        }

        /// <inheritdoc />
        public ITypeSymbol ResolveCoreType(string typeName)
        {
            return _coreTypeCache.GetOrAdd(typeName, _ => GetCoreAssembly().GetType(_)) ?? throw new DiagnosticEventException(DiagnosticEvent.CoreClassesMissing());
        }

        /// <summary>
        /// Resolves the set of system assemblies.
        /// </summary>
        /// <returns></returns>
        IAssemblySymbol[] ResolveSystemAssemblies()
        {
            return _systemAssemblies ??= ResolveSystemAssembliesIter().Distinct().ToArray();
        }

        /// <summary>
        /// Resolves the set of system assemblies that contain the system types.
        /// </summary>
        /// <returns></returns>
        IEnumerable<IAssemblySymbol> ResolveSystemAssembliesIter()
        {
            foreach (var assembly in _universe.GetAssemblies())
                foreach (var typeName in GetSystemTypeNames())
                    if (assembly.GetType(typeName) is Type t)
                        yield return GetSymbol(assembly);
        }

        /// <inheritdoc />
        public ITypeSymbol ResolveSystemType(string typeName)
        {
            return _systemTypeCache.GetOrAdd(typeName, FindSystemType) ?? throw new DiagnosticEventException(DiagnosticEvent.ClassNotFound(typeName));
        }

        /// <summary>
        /// Attempts to load the specified system type.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        ITypeSymbol? FindSystemType(string typeName)
        {
            foreach (var assembly in ResolveSystemAssemblies())
                if (assembly.GetType(typeName) is { } t)
                    return t;

            return null;
        }

        /// <inheritdoc />
        public ITypeSymbol? ResolveRuntimeType(string typeName)
        {
            return GetRuntimeAssembly().GetType(typeName);
        }

        /// <inheritdoc />
        public ITypeSymbol? ResolveBaseType(string typeName)
        {
            return GetBaseAssembly().GetType(typeName);
        }

        /// <inheritdoc />
        public IAssemblySymbol? ResolveAssembly(string assemblyName)
        {
            return _universe.Load(assemblyName) is { } a ? GetSymbol(a) : null;
        }

        /// <inheritdoc />
        public ITypeSymbol? ResolveType(string typeName)
        {
            foreach (var assembly in _universe.GetAssemblies())
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
        public IMemberSymbol GetSymbol(MemberInfo memberInfo)
        {
            return _symbols.GetOrCreateMemberSymbol(memberInfo);
        }

        /// <inheritdoc />
        public IMethodBaseSymbol GetSymbol(MethodBase type)
        {
            return _symbols.GetOrCreateMethodBaseSymbol(type);
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