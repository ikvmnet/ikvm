#nullable enable

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
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
        readonly ImportOptions _options;

        AssemblySymbol? _coreAssembly;
        readonly ConcurrentDictionary<string, TypeSymbol?> _coreTypeCache = new();

        AssemblySymbol[]? _systemAssemblies;
        readonly ConcurrentDictionary<string, TypeSymbol?> _systemTypeCache = new();

        AssemblySymbol? _runtimeAssembly;
        readonly ConcurrentDictionary<string, TypeSymbol?> _runtimeTypeCache = new();

        AssemblySymbol? _baseAssembly;
        readonly ConcurrentDictionary<string, TypeSymbol?> _baseTypeCache = new();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="diagnostics"></param>
        /// <param name="universe"></param>
        /// <param name="symbols"></param>
        /// <param name="options"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ImportRuntimeSymbolResolver(IDiagnosticHandler diagnostics, Universe universe, IkvmReflectionSymbolContext symbols, ImportOptions options)
        {
            _diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
            _universe = universe ?? throw new ArgumentNullException(nameof(universe));
            _symbols = symbols ?? throw new ArgumentNullException(nameof(symbols));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <inheritdoc />
        public SymbolContext Symbols => _symbols;

        /// <inheritdoc />
        public AssemblySymbol GetCoreAssembly()
        {
            return (_coreAssembly ??= GetSymbol(_universe.CoreLib)) ?? throw new DiagnosticEventException(DiagnosticEvent.CoreClassesMissing());
        }

        /// <inheritdoc />
        public AssemblySymbol GetRuntimeAssembly()
        {
            return (_runtimeAssembly ??= FindRuntimeAssembly()) ?? throw new DiagnosticEventException(DiagnosticEvent.RuntimeNotFound());
        }

        /// <summary>
        /// Attempts to load the runtime assembly.
        /// </summary>
        AssemblySymbol? FindRuntimeAssembly()
        {
            // search for already loaded runtime
            foreach (var assembly in _universe.GetAssemblies())
                if (assembly.GetType("IKVM.Runtime.JVM") is Type)
                    return GetSymbol(assembly);

            // user explicitly specified path to runtime
            if (_options.Runtime != null)
                if (_options.Runtime.Exists)
                    return GetSymbol(_universe.LoadFile(_options.Runtime.FullName));

            // fallback to resolution against universe
            try
            {
                return GetSymbol(_universe.Load("IKVM.Runtime"));
            }
            catch (FileNotFoundException)
            {
                // ignore, we tried
            }

            return null;
        }

        /// <inheritdoc />
        public AssemblySymbol? GetBaseAssembly()
        {
            // bootstrap mode is used for builing the main assembly, and thus should not return the existing base assembly
            if (_options.Bootstrap)
                return null;

            return (_baseAssembly ??= FindBaseAssembly()) ?? throw new DiagnosticEventException(DiagnosticEvent.BootstrapClassesMissing());
        }

        /// <summary>
        /// Attempts to load the base assembly.
        /// </summary>
        AssemblySymbol? FindBaseAssembly()
        {
            foreach (var assembly in _universe.GetAssemblies())
                if (assembly.GetType("java.lang.Object") is Type)
                    return GetSymbol(assembly);

            // fallback to resolution against universe
            try
            {
                return GetSymbol(_universe.Load("IKVM.Java"));
            }
            catch (FileNotFoundException)
            {
                // ignore, we tried
            }

            return null;
        }

        /// <inheritdoc />
        public TypeSymbol ResolveCoreType(string typeName)
        {
            return _coreTypeCache.GetOrAdd(typeName, _ => GetCoreAssembly().GetType(_)) ?? throw new DiagnosticEventException(DiagnosticEvent.CoreClassesMissing());
        }

        /// <summary>
        /// Resolves the set of system assemblies.
        /// </summary>
        /// <returns></returns>
        AssemblySymbol[] ResolveSystemAssemblies()
        {
            return _systemAssemblies ??= ResolveSystemAssembliesIter().Distinct().ToArray();
        }

        /// <summary>
        /// Resolves the set of system assemblies that contain the system types.
        /// </summary>
        /// <returns></returns>
        IEnumerable<AssemblySymbol> ResolveSystemAssembliesIter()
        {
            foreach (var assembly in _universe.GetAssemblies())
                foreach (var typeName in GetSystemTypeNames())
                    if (assembly.GetType(typeName) is Type t)
                        yield return GetSymbol(assembly);
        }

        /// <inheritdoc />
        public TypeSymbol ResolveSystemType(string typeName)
        {
            return _systemTypeCache.GetOrAdd(typeName, FindSystemType) ?? throw new DiagnosticEventException(DiagnosticEvent.ClassNotFound(typeName));
        }

        /// <summary>
        /// Attempts to load the specified system type.
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        TypeSymbol? FindSystemType(string typeName)
        {
            foreach (var assembly in ResolveSystemAssemblies())
                if (assembly.GetType(typeName) is { } t)
                    return t;

            return null;
        }

        /// <inheritdoc />
        public TypeSymbol ResolveRuntimeType(string typeName)
        {
            return GetRuntimeAssembly().GetType(typeName) ?? throw new TypeLoadException();
        }

        /// <inheritdoc />
        public bool TryResolveRuntimeType(string typeName, out TypeSymbol? type)
        {
            var t = GetRuntimeAssembly().GetType(typeName);
            if (t != null)
            {
                type = t;
                return true;
            }

            type = null;
            return false;
        }

        /// <inheritdoc />
        public TypeSymbol ResolveBaseType(string typeName)
        {
            return GetBaseAssembly()?.GetType(typeName) ?? throw new TypeLoadException();
        }

        /// <inheritdoc />
        public AssemblySymbol? ResolveAssembly(string assemblyName)
        {
            return _universe.Load(assemblyName) is { } a ? GetSymbol(a) : null;
        }

        /// <inheritdoc />
        public TypeSymbol? ResolveType(string typeName)
        {
            foreach (var assembly in _universe.GetAssemblies())
                if (assembly.GetType(typeName) is Type t)
                    return GetSymbol(t);

            return null;
        }

        /// <inheritdoc />
        public AssemblySymbol GetSymbol(Assembly assembly)
        {
            return _symbols.ResolveAssemblySymbol(assembly);
        }

        /// <inheritdoc />
        public ModuleSymbol GetSymbol(Module module)
        {
            return _symbols.ResolveModuleSymbol(module);
        }

        /// <inheritdoc />
        public TypeSymbol GetSymbol(Type type)
        {
            return _symbols.ResolveTypeSymbol(type);
        }

        /// <inheritdoc />
        public MemberSymbol GetSymbol(MemberInfo memberInfo)
        {
            return _symbols.ResolveMemberSymbol(memberInfo);
        }

        /// <inheritdoc />
        public FieldSymbol GetSymbol(FieldInfo field)
        {
            return _symbols.ResolveFieldSymbol(field);
        }

        /// <inheritdoc />
        public MethodSymbol GetSymbol(MethodBase method)
        {
            return _symbols.ResolveMethodSymbol((MethodInfo)method);
        }

        /// <inheritdoc />
        public PropertySymbol GetSymbol(PropertyInfo property)
        {
            return _symbols.ResolvePropertySymbol(property);
        }

        /// <inheritdoc />
        public EventSymbol GetSymbol(EventInfo @event)
        {
            return _symbols.ResolveEventSymbol(@event);
        }

    }

}