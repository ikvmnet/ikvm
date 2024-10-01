using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionAssemblySymbol : ReflectionSymbol, IReflectionAssemblySymbol
    {

        readonly Assembly _assembly;

        ReflectionModuleTable _moduleTable;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="assembly"></param>
        public ReflectionAssemblySymbol(ReflectionSymbolContext context, Assembly assembly) :
            base(context)
        {
            _assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
            _moduleTable = new ReflectionModuleTable(context, this);
        }

        /// <summary>
        /// Gets the underlying <see cref="Assembly"/> instance.
        /// </summary>
        public Assembly UnderlyingAssembly => _assembly;

        #region IAssemblySymbol

        /// <inheritdoc />
        public IReflectionModuleSymbol GetOrCreateModuleSymbol(Module module)
        {
            return _moduleTable.GetOrCreateModuleSymbol(module);
        }

        #endregion

        #region IAssemblySymbol

        /// <inheritdoc />
        public IEnumerable<ITypeSymbol> DefinedTypes => ResolveTypeSymbols(UnderlyingAssembly.DefinedTypes);

        /// <inheritdoc />
        public IMethodSymbol? EntryPoint => ResolveMethodSymbol(UnderlyingAssembly.EntryPoint);

        /// <inheritdoc />
        public IEnumerable<ITypeSymbol> ExportedTypes => ResolveTypeSymbols(UnderlyingAssembly.ExportedTypes);

        /// <inheritdoc />
        public string? FullName => UnderlyingAssembly.FullName;

        /// <inheritdoc />
        public string ImageRuntimeVersion => UnderlyingAssembly.ImageRuntimeVersion;

        /// <inheritdoc />
        public string Location => UnderlyingAssembly.Location;

        /// <inheritdoc />
        public IModuleSymbol ManifestModule => ResolveModuleSymbol(UnderlyingAssembly.ManifestModule);

        /// <inheritdoc />
        public IEnumerable<IModuleSymbol> Modules => ResolveModuleSymbols(UnderlyingAssembly.Modules);

        /// <inheritdoc />
        public ITypeSymbol[] GetExportedTypes()
        {
            return ResolveTypeSymbols(UnderlyingAssembly.GetExportedTypes());
        }

        /// <inheritdoc />
        public IModuleSymbol? GetModule(string name)
        {
            return ResolveModuleSymbol(UnderlyingAssembly.GetModule(name));
        }

        /// <inheritdoc />
        public IModuleSymbol[] GetModules()
        {
            return ResolveModuleSymbols(UnderlyingAssembly.GetModules());
        }

        /// <inheritdoc />
        public IModuleSymbol[] GetModules(bool getResourceModules)
        {
            return ResolveModuleSymbols(UnderlyingAssembly.GetModules(getResourceModules));
        }

        /// <inheritdoc />
        public AssemblyNameInfo GetName()
        {
            return UnderlyingAssembly.GetName().Pack();
        }

        /// <inheritdoc />
        public AssemblyNameInfo[] GetReferencedAssemblies()
        {
            return UnderlyingAssembly.GetReferencedAssemblies().Pack();
        }

        /// <inheritdoc />
        public ITypeSymbol? GetType(string name, bool throwOnError)
        {
            return ResolveTypeSymbol(UnderlyingAssembly.GetType(name, throwOnError));
        }

        /// <inheritdoc />
        public ITypeSymbol? GetType(string name, bool throwOnError, bool ignoreCase)
        {
            return ResolveTypeSymbol(UnderlyingAssembly.GetType(name, throwOnError, ignoreCase));
        }

        /// <inheritdoc />
        public ITypeSymbol? GetType(string name)
        {
            return ResolveTypeSymbol(UnderlyingAssembly.GetType(name));
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetTypes()
        {
            return ResolveTypeSymbols(UnderlyingAssembly.GetTypes());
        }

        /// <inheritdoc />
        public CustomAttribute[] GetCustomAttributes(bool inherit = false)
        {
            return ResolveCustomAttributes(UnderlyingAssembly.GetCustomAttributesData());
        }

        /// <inheritdoc />
        public CustomAttribute[] GetCustomAttributes(ITypeSymbol attributeType, bool inherit = false)
        {
            var _attributeType = attributeType.Unpack();
            return ResolveCustomAttributes(UnderlyingAssembly.GetCustomAttributesData().Where(i => i.AttributeType == _attributeType).ToArray());
        }

        /// <inheritdoc />
        public CustomAttribute? GetCustomAttribute(ITypeSymbol attributeType, bool inherit = false)
        {
            var _attributeType = attributeType.Unpack();
            var a = UnderlyingAssembly.GetCustomAttributesData().Where(i => i.AttributeType == _attributeType).ToList();
            if (a.Count > 0)
                return ResolveCustomAttribute(a[0]);

            return null;
        }

        /// <inheritdoc />
        public bool IsDefined(ITypeSymbol attributeType, bool inherit = false)
        {
            return UnderlyingAssembly.IsDefined(attributeType.Unpack(), inherit);
        }

        /// <inheritdoc />
        public ManifestResourceInfo? GetManifestResourceInfo(string resourceName)
        {
            return ResolveManifestResourceInfo(UnderlyingAssembly.GetManifestResourceInfo(resourceName));
        }

        /// <inheritdoc />
        public Stream? GetManifestResourceStream(string name)
        {
            return UnderlyingAssembly.GetManifestResourceStream(name);
        }

        /// <inheritdoc />
        public Stream? GetManifestResourceStream(ITypeSymbol type, string name)
        {
            return UnderlyingAssembly.GetManifestResourceStream(type.Unpack(), name);
        }

        #endregion

    }

}
