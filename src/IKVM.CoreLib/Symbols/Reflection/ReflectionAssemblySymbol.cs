using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionAssemblySymbol : ReflectionSymbol, IAssemblySymbol
    {

        readonly Assembly _underlyingAssembly;
        readonly ConditionalWeakTable<Module, ReflectionModuleSymbol> _modules = new();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assembly"></param>
        public ReflectionAssemblySymbol(ReflectionSymbolContext context, Assembly assembly) :
            base(context)
        {
            _underlyingAssembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
        }

        internal Assembly UnderlyingAssembly => _underlyingAssembly;

        /// <summary>
        /// Gets or creates the <see cref="IModuleSymbol"/> cached for the module.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        internal ReflectionModuleSymbol GetOrCreateModuleSymbol(Module module)
        {
            Debug.Assert(module.Assembly == _underlyingAssembly);
            return _modules.GetValue(module, _ => new ReflectionModuleSymbol(Context, _));
        }

        public IEnumerable<ITypeSymbol> DefinedTypes => ResolveTypeSymbols(_underlyingAssembly.DefinedTypes);

        public IMethodSymbol? EntryPoint => _underlyingAssembly.EntryPoint is { } m ? ResolveMethodSymbol(m) : null;

        public IEnumerable<ITypeSymbol> ExportedTypes => ResolveTypeSymbols(_underlyingAssembly.ExportedTypes);

        public string? FullName => _underlyingAssembly.FullName;

        public string ImageRuntimeVersion => _underlyingAssembly.ImageRuntimeVersion;

        public IModuleSymbol ManifestModule => ResolveModuleSymbol(_underlyingAssembly.ManifestModule);

        public IEnumerable<IModuleSymbol> Modules => ResolveModuleSymbols(_underlyingAssembly.Modules);

        public ITypeSymbol[] GetExportedTypes()
        {
            return ResolveTypeSymbols(_underlyingAssembly.GetExportedTypes());
        }

        public IModuleSymbol? GetModule(string name)
        {
            return _underlyingAssembly.GetModule(name) is Module m ? GetOrCreateModuleSymbol(m) : null;
        }

        public ImmutableArray<IModuleSymbol> GetModules()
        {
            return ResolveModuleSymbols(_underlyingAssembly.GetModules()).CastArray<IModuleSymbol>();
        }

        public ImmutableArray<IModuleSymbol> GetModules(bool getResourceModules)
        {
            return ResolveModuleSymbols(_underlyingAssembly.GetModules(getResourceModules)).CastArray<IModuleSymbol>();
        }

        public AssemblyName GetName()
        {
            return _underlyingAssembly.GetName();
        }

        public AssemblyName GetName(bool copiedName)
        {
            return _underlyingAssembly.GetName(copiedName);
        }

        public AssemblyName[] GetReferencedAssemblies()
        {
            return _underlyingAssembly.GetReferencedAssemblies();
        }

        public ITypeSymbol? GetType(string name, bool throwOnError)
        {
            return _underlyingAssembly.GetType(name, throwOnError) is Type t ? Context.GetOrCreateTypeSymbol(t) : null;
        }

        public ITypeSymbol? GetType(string name, bool throwOnError, bool ignoreCase)
        {
            return _underlyingAssembly.GetType(name, throwOnError, ignoreCase) is Type t ? Context.GetOrCreateTypeSymbol(t) : null;
        }

        public ITypeSymbol? GetType(string name)
        {
            return _underlyingAssembly.GetType(name) is Type t ? Context.GetOrCreateTypeSymbol(t) : null;
        }

        public ITypeSymbol[] GetTypes()
        {
            return ResolveTypeSymbols(_underlyingAssembly.GetTypes());
        }

        public ImmutableArray<CustomAttributeSymbol> GetCustomAttributes()
        {
            return ResolveCustomAttributes(_underlyingAssembly.GetCustomAttributesData());
        }

        public ImmutableArray<CustomAttributeSymbol> GetCustomAttributes(ITypeSymbol attributeType)
        {
            return ResolveCustomAttributes(_underlyingAssembly.GetCustomAttributesData()).Where(i => i.AttributeType == attributeType).ToImmutableArray();
        }

        public bool IsDefined(ITypeSymbol attributeType)
        {
            return _underlyingAssembly.IsDefined(((ReflectionTypeSymbol)attributeType).UnderlyingType);
        }

    }

}
