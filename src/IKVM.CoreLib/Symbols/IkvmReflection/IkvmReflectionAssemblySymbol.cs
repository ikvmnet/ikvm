using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using Assembly = IKVM.Reflection.Assembly;
using AssemblyName = IKVM.Reflection.AssemblyName;
using Module = IKVM.Reflection.Module;
using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection
{

    class IkvmReflectionAssemblySymbol : IkvmReflectionSymbol, IAssemblySymbol
    {

        readonly Assembly _underlyingAssembly;
        readonly ConditionalWeakTable<Module, IkvmReflectionModuleSymbol> _modules = new();

        global::System.Reflection.AssemblyName? _assemblyName;
        ImmutableArray<CustomAttributeSymbol> _customAttributes = default;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="underlyingAssembly"></param>
        public IkvmReflectionAssemblySymbol(IkvmReflectionSymbolContext context, Assembly underlyingAssembly) :
            base(context)
        {
            _underlyingAssembly = underlyingAssembly ?? throw new ArgumentNullException(nameof(underlyingAssembly));
        }

        /// <summary>
        /// Gets or creates the <see cref="IModuleSymbol"/> cached for the module.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        internal IkvmReflectionModuleSymbol GetOrCreateModuleSymbol(Module module)
        {
            Debug.Assert(module.Assembly == _underlyingAssembly);
            return _modules.GetValue(module, _ => new IkvmReflectionModuleSymbol(Context, _));
        }

        internal Assembly UnderlyingAssembly => _underlyingAssembly;

        public string? FullName => _underlyingAssembly.FullName;

        public IModuleSymbol ManifestModule => ResolveModuleSymbol(_underlyingAssembly.ManifestModule);

        public IEnumerable<IModuleSymbol> Modules => ResolveModuleSymbols(_underlyingAssembly.Modules);

        public IEnumerable<ITypeSymbol> DefinedTypes => ResolveTypeSymbols(_underlyingAssembly.DefinedTypes);

        public IEnumerable<ITypeSymbol> ExportedTypes => ResolveTypeSymbols(_underlyingAssembly.ExportedTypes);

        public IMethodSymbol? EntryPoint => _underlyingAssembly.EntryPoint is { } m ? ResolveMethodSymbol(m) : null;

        public override bool IsMissing => _underlyingAssembly.__IsMissing;

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

        global::System.Reflection.AssemblyName ToName(AssemblyName src)
        {
#pragma warning disable SYSLIB0037 // Type or member is obsolete
            return new global::System.Reflection.AssemblyName()
            {
                Name = src.Name,
                Version = src.Version,
                CultureName = src.CultureName,
                HashAlgorithm = (global::System.Configuration.Assemblies.AssemblyHashAlgorithm)src.HashAlgorithm,
                Flags = (global::System.Reflection.AssemblyNameFlags)src.Flags,
                ContentType = (global::System.Reflection.AssemblyContentType)src.ContentType,
            };
#pragma warning restore SYSLIB0037 // Type or member is obsolete
        }

        public global::System.Reflection.AssemblyName GetName()
        {
            return _assemblyName ??= ToName(_underlyingAssembly.GetName());
        }

        public ImmutableArray<global::System.Reflection.AssemblyName> GetReferencedAssemblies()
        {
            var l = _underlyingAssembly.GetReferencedAssemblies().ToImmutableArray();
            var a = ImmutableArray.CreateBuilder<global::System.Reflection.AssemblyName>(l.Length);
            for (int i = 0; i < l.Length; i++)
                a.Add(ToName(l[i]));

            return a.DrainToImmutable();
        }

        public ITypeSymbol? GetType(string name)
        {
            return _underlyingAssembly.GetType(name) is Type t ? Context.GetOrCreateTypeSymbol(t) : null;
        }

        public ITypeSymbol? GetType(string name, bool throwOnError)
        {
            return _underlyingAssembly.GetType(name, throwOnError) is Type t ? Context.GetOrCreateTypeSymbol(t) : null;
        }

        public ITypeSymbol[] GetTypes()
        {
            return ResolveTypeSymbols(_underlyingAssembly.GetTypes());
        }

        public ImmutableArray<CustomAttributeSymbol> GetCustomAttributes()
        {
            return _customAttributes.IsDefault ? _customAttributes = ResolveCustomAttributes(_underlyingAssembly.GetCustomAttributesData()) : _customAttributes;
        }

        public ImmutableArray<CustomAttributeSymbol> GetCustomAttributes(ITypeSymbol attributeType)
        {
            return ResolveCustomAttributes(_underlyingAssembly.__GetCustomAttributes(((IkvmReflectionTypeSymbol)attributeType).UnderlyingType, false));
        }

        public bool IsDefined(ITypeSymbol attributeType)
        {
            return _underlyingAssembly.IsDefined(((IkvmReflectionTypeSymbol)attributeType).UnderlyingType, false);
        }

    }

}
