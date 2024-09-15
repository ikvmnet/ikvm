using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using IKVM.CoreLib.Symbols.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionAssemblySymbol : ReflectionSymbol, IAssemblySymbol
    {

        readonly Assembly _assembly;
        readonly ConditionalWeakTable<Module, ReflectionModuleSymbol> _modules = new();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assembly"></param>
        public ReflectionAssemblySymbol(ReflectionSymbolContext context, Assembly assembly) :
            base(context)
        {
            _assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
        }

        internal Assembly ReflectionObject => _assembly;

        /// <summary>
        /// Gets or creates the <see cref="IModuleSymbol"/> cached for the module.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        internal ReflectionModuleSymbol GetOrCreateModuleSymbol(Module module)
        {
            if (module is null)
                throw new ArgumentNullException(nameof(module));

            Debug.Assert(module.Assembly == _assembly);
            return _modules.GetValue(module, _ => new ReflectionModuleSymbol(Context, _));
        }

        /// <summary>
        /// Gets or creates the <see cref="IModuleSymbol"/> cached for the module.
        /// </summary>
        /// <param name="module"></param>
        /// <returns></returns>
        internal ReflectionModuleSymbol GetOrCreateModuleSymbol(ReflectionModuleSymbolBuilder module)
        {
            if (module is null)
                throw new ArgumentNullException(nameof(module));

            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IEnumerable<ITypeSymbol> DefinedTypes => ResolveTypeSymbols(_assembly.DefinedTypes);

        /// <inheritdoc />
        public IMethodSymbol? EntryPoint => _assembly.EntryPoint is { } m ? ResolveMethodSymbol(m) : null;

        /// <inheritdoc />
        public IEnumerable<ITypeSymbol> ExportedTypes => ResolveTypeSymbols(_assembly.ExportedTypes);

        /// <inheritdoc />
        public string? FullName => _assembly.FullName;

        /// <inheritdoc />
        public string ImageRuntimeVersion => _assembly.ImageRuntimeVersion;

        /// <inheritdoc />
        public IModuleSymbol ManifestModule => ResolveModuleSymbol(_assembly.ManifestModule);

        /// <inheritdoc />
        public IEnumerable<IModuleSymbol> Modules => ResolveModuleSymbols(_assembly.Modules);

        /// <inheritdoc />
        public ITypeSymbol[] GetExportedTypes()
        {
            return ResolveTypeSymbols(_assembly.GetExportedTypes());
        }

        /// <inheritdoc />
        public IModuleSymbol? GetModule(string name)
        {
            return _assembly.GetModule(name) is Module m ? GetOrCreateModuleSymbol(m) : null;
        }

        /// <inheritdoc />
        public IModuleSymbol[] GetModules()
        {
            return ResolveModuleSymbols(_assembly.GetModules());
        }

        /// <inheritdoc />
        public IModuleSymbol[] GetModules(bool getResourceModules)
        {
            return ResolveModuleSymbols(_assembly.GetModules(getResourceModules));
        }

        /// <inheritdoc />
        public AssemblyName GetName()
        {
            return _assembly.GetName();
        }

        /// <inheritdoc />
        public AssemblyName GetName(bool copiedName)
        {
            return _assembly.GetName(copiedName);
        }

        /// <inheritdoc />
        public AssemblyName[] GetReferencedAssemblies()
        {
            return _assembly.GetReferencedAssemblies();
        }

        /// <inheritdoc />
        public ITypeSymbol? GetType(string name, bool throwOnError)
        {
            return _assembly.GetType(name, throwOnError) is Type t ? Context.GetOrCreateTypeSymbol(t) : null;
        }

        /// <inheritdoc />
        public ITypeSymbol? GetType(string name, bool throwOnError, bool ignoreCase)
        {
            return _assembly.GetType(name, throwOnError, ignoreCase) is Type t ? Context.GetOrCreateTypeSymbol(t) : null;
        }

        /// <inheritdoc />
        public ITypeSymbol? GetType(string name)
        {
            return _assembly.GetType(name) is Type t ? Context.GetOrCreateTypeSymbol(t) : null;
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetTypes()
        {
            return ResolveTypeSymbols(_assembly.GetTypes());
        }

        /// <inheritdoc />
        public CustomAttribute[] GetCustomAttributes(bool inherit = false)
        {
            return ResolveCustomAttributes(_assembly.GetCustomAttributesData());
        }

        /// <inheritdoc />
        public CustomAttribute[] GetCustomAttributes(ITypeSymbol attributeType, bool inherit = false)
        {
            return ResolveCustomAttributes(_assembly.GetCustomAttributesData().Where(i => i.AttributeType == ((ReflectionTypeSymbol)attributeType).ReflectionObject));
        }

        /// <inheritdoc />
        public CustomAttribute? GetCustomAttribute(ITypeSymbol attributeType, bool inherit = false)
        {
            return GetCustomAttributes(attributeType, inherit).FirstOrDefault();
        }

        /// <inheritdoc />
        public bool IsDefined(ITypeSymbol attributeType, bool inherit = false)
        {
            return _assembly.IsDefined(((ReflectionTypeSymbol)attributeType).ReflectionObject, false);
        }

    }

}
