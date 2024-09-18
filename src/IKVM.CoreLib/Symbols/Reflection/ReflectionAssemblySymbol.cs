using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionAssemblySymbol : ReflectionSymbol, IReflectionAssemblySymbol
    {

        readonly Assembly _assembly;
        ReflectionAssemblyMetadata _impl;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assembly"></param>
        public ReflectionAssemblySymbol(ReflectionSymbolContext context, Assembly assembly) :
            base(context)
        {
            _assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
            _impl = new ReflectionAssemblyMetadata(this);
        }

        /// <summary>
        /// Gets the underlying <see cref="Assembly"/> instance.
        /// </summary>
        public Assembly UnderlyingAssembly => _assembly;

        #region IAssemblySymbol

        /// <inheritdoc />
        public IEnumerable<ITypeSymbol> DefinedTypes => ResolveTypeSymbols(_assembly.DefinedTypes);

        /// <inheritdoc />
        public IMethodSymbol? EntryPoint => ResolveMethodSymbol(_assembly.EntryPoint);

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
            return ResolveModuleSymbol(_assembly.GetModule(name));
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
            return ResolveTypeSymbol(_assembly.GetType(name, throwOnError));
        }

        /// <inheritdoc />
        public ITypeSymbol? GetType(string name, bool throwOnError, bool ignoreCase)
        {
            return ResolveTypeSymbol(_assembly.GetType(name, throwOnError, ignoreCase));
        }

        /// <inheritdoc />
        public ITypeSymbol? GetType(string name)
        {
            return ResolveTypeSymbol(_assembly.GetType(name));
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
            var _attributeType = attributeType.Unpack();
            return ResolveCustomAttributes(_assembly.GetCustomAttributesData().Where(i => i.AttributeType == _attributeType).ToArray());
        }

        /// <inheritdoc />
        public CustomAttribute? GetCustomAttribute(ITypeSymbol attributeType, bool inherit = false)
        {
            return GetCustomAttributes(attributeType, inherit).FirstOrDefault();
        }

        /// <inheritdoc />
        public bool IsDefined(ITypeSymbol attributeType, bool inherit = false)
        {
            return _assembly.IsDefined(attributeType.Unpack(), inherit);
        }

        #endregion

        public IReflectionModuleSymbol GetOrCreateModuleSymbol(Module module)
        {
            return _impl.GetOrCreateModuleSymbol(module);
        }

        public IReflectionModuleSymbolBuilder GetOrCreateModuleSymbol(ModuleBuilder module)
        {
            return (IReflectionModuleSymbolBuilder)_impl.GetOrCreateModuleSymbol(module);
        }

    }

}
