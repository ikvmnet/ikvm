using System;
using System.Collections.Generic;
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

        readonly Assembly _assembly;
        readonly ConditionalWeakTable<Module, IkvmReflectionModuleSymbol> _modules = new();

        System.Reflection.AssemblyName? _assemblyName;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assembly"></param>
        public IkvmReflectionAssemblySymbol(IkvmReflectionSymbolContext context, Assembly assembly) :
            base(context)
        {
            _assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
        }

        /// <summary>
        /// Gets or creates the <see cref="IModuleSymbol"/> cached for the module.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        internal IkvmReflectionModuleSymbol GetOrCreateModuleSymbol(Module module)
        {
            Debug.Assert(module.Assembly == _assembly);
            return _modules.GetValue(module, _ => new IkvmReflectionModuleSymbol(Context, _));
        }

        internal Assembly ReflectionObject => _assembly;

        public IEnumerable<ITypeSymbol> DefinedTypes => ResolveTypeSymbols(_assembly.DefinedTypes);

        public IMethodSymbol? EntryPoint => _assembly.EntryPoint is { } m ? ResolveMethodSymbol(m) : null;

        public IEnumerable<ITypeSymbol> ExportedTypes => ResolveTypeSymbols(_assembly.ExportedTypes);

        public string? FullName => _assembly.FullName;

        public string ImageRuntimeVersion => _assembly.ImageRuntimeVersion;

        public IModuleSymbol ManifestModule => ResolveModuleSymbol(_assembly.ManifestModule);

        public IEnumerable<IModuleSymbol> Modules => ResolveModuleSymbols(_assembly.Modules);

        public override bool IsMissing => _assembly.__IsMissing;

        public ITypeSymbol[] GetExportedTypes()
        {
            return ResolveTypeSymbols(_assembly.GetExportedTypes());
        }

        public IModuleSymbol? GetModule(string name)
        {
            return _assembly.GetModule(name) is Module m ? GetOrCreateModuleSymbol(m) : null;
        }

        public IModuleSymbol[] GetModules()
        {
            return ResolveModuleSymbols(_assembly.GetModules());
        }

        public IModuleSymbol[] GetModules(bool getResourceModules)
        {
            return ResolveModuleSymbols(_assembly.GetModules(getResourceModules));
        }

        System.Reflection.AssemblyName ToName(AssemblyName src)
        {
#pragma warning disable SYSLIB0037 // Type or member is obsolete
            return new System.Reflection.AssemblyName()
            {
                Name = src.Name,
                Version = src.Version,
                CultureName = src.CultureName,
                HashAlgorithm = (System.Configuration.Assemblies.AssemblyHashAlgorithm)src.HashAlgorithm,
                Flags = (System.Reflection.AssemblyNameFlags)src.Flags,
                ContentType = (System.Reflection.AssemblyContentType)src.ContentType,
            };
#pragma warning restore SYSLIB0037 // Type or member is obsolete
        }

        public System.Reflection.AssemblyName GetName()
        {
            return _assemblyName ??= ToName(_assembly.GetName());
        }

        public System.Reflection.AssemblyName GetName(bool copiedName)
        {
            return ToName(_assembly.GetName());
        }

        public System.Reflection.AssemblyName[] GetReferencedAssemblies()
        {
            var l = _assembly.GetReferencedAssemblies();
            var a = new System.Reflection.AssemblyName[l.Length];
            for (int i = 0; i < l.Length; i++)
                a[i] = ToName(l[i]);

            return a;
        }

        public ITypeSymbol? GetType(string name, bool throwOnError)
        {
            return _assembly.GetType(name, throwOnError) is Type t ? Context.GetOrCreateTypeSymbol(t) : null;
        }

        public ITypeSymbol? GetType(string name, bool throwOnError, bool ignoreCase)
        {
            return _assembly.GetType(name, throwOnError, ignoreCase) is Type t ? Context.GetOrCreateTypeSymbol(t) : null;
        }

        public ITypeSymbol? GetType(string name)
        {
            return _assembly.GetType(name) is Type t ? Context.GetOrCreateTypeSymbol(t) : null;
        }

        public ITypeSymbol[] GetTypes()
        {
            return ResolveTypeSymbols(_assembly.GetTypes());
        }

        public CustomAttributeSymbol[] GetCustomAttributes()
        {
            return ResolveCustomAttributes(_assembly.GetCustomAttributesData());
        }

        public CustomAttributeSymbol[] GetCustomAttributes(ITypeSymbol attributeType)
        {
            return ResolveCustomAttributes(_assembly.__GetCustomAttributes(((IkvmReflectionTypeSymbol)attributeType).ReflectionObject, false));
        }

        public bool IsDefined(ITypeSymbol attributeType)
        {
            return _assembly.IsDefined(((IkvmReflectionTypeSymbol)attributeType).ReflectionObject, false);
        }

    }

}
