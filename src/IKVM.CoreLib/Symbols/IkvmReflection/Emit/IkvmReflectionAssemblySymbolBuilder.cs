using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    class IkvmReflectionAssemblySymbolBuilder : IkvmReflectionSymbolBuilder, IIkvmReflectionAssemblySymbolBuilder
    {

        readonly AssemblyBuilder _builder;

        IkvmReflectionModuleTable _moduleTable;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="builder"></param>
        public IkvmReflectionAssemblySymbolBuilder(IkvmReflectionSymbolContext context, AssemblyBuilder builder) :
            base(context)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _moduleTable = new IkvmReflectionModuleTable(context, this);
        }

        /// <inheritdoc />
        public Assembly UnderlyingAssembly => UnderlyingAssemblyBuilder;

        /// <inheritdoc />
        public AssemblyBuilder UnderlyingAssemblyBuilder => _builder;

        #region IIkvmReflectionAssemblySymbolBuilder

        /// <inheritdoc />
        public IIkvmReflectionModuleSymbolBuilder GetOrCreateModuleSymbol(ModuleBuilder module)
        {
            return _moduleTable.GetOrCreateModuleSymbol(module);
        }

        #endregion

        #region IIkvmReflectionAssemblySymbol

        /// <inheritdoc />
        public IIkvmReflectionModuleSymbol GetOrCreateModuleSymbol(Module module)
        {
            return _moduleTable.GetOrCreateModuleSymbol(module);
        }

        #endregion

        #region IAssemblySymbolBuilder

        /// <inheritdoc />
        public IModuleSymbolBuilder DefineModule(string name)
        {
            return GetOrCreateModuleSymbol(UnderlyingAssemblyBuilder.DefineDynamicModule(name, name + ".dll"));
        }

        /// <inheritdoc />
        public IModuleSymbolBuilder DefineModule(string name, string fileName)
        {
            return GetOrCreateModuleSymbol(UnderlyingAssemblyBuilder.DefineDynamicModule(name, fileName));
        }

        /// <inheritdoc />
        public IModuleSymbolBuilder DefineModule(string name, string fileName, bool emitSymbolInfo)
        {
            return GetOrCreateModuleSymbol(UnderlyingAssemblyBuilder.DefineDynamicModule(name, fileName, emitSymbolInfo));
        }

        /// <inheritdoc />
        public void SetCustomAttribute(IConstructorSymbol con, byte[] binaryAttribute)
        {
            UnderlyingAssemblyBuilder.SetCustomAttribute(con.Unpack(), binaryAttribute);
        }

        /// <inheritdoc />
        public void SetCustomAttribute(ICustomAttributeBuilder customBuilder)
        {
            UnderlyingAssemblyBuilder.SetCustomAttribute(((IkvmReflectionCustomAttributeBuilder)customBuilder).UnderlyingBuilder);
        }

        /// <inheritdoc />
        public void DefineIconResource(byte[] bytes)
        {
            UnderlyingAssemblyBuilder.__DefineIconResource(bytes);
        }

        /// <inheritdoc />
        public void DefineManifestResource(byte[] bytes)
        {
            UnderlyingAssemblyBuilder.__DefineManifestResource(bytes);
        }

        /// <inheritdoc />
        public void DefineVersionInfoResource()
        {
            UnderlyingAssemblyBuilder.DefineVersionInfoResource();
        }

        /// <inheritdoc />
        public void SetEntryPoint(IMethodSymbolBuilder mainMethodProxy)
        {
            UnderlyingAssemblyBuilder.SetEntryPoint(mainMethodProxy.Unpack());
        }

        /// <inheritdoc />
        public void SetEntryPoint(IMethodSymbolBuilder mainMethodProxy, IKVM.CoreLib.Symbols.Emit.PEFileKinds target)
        {
            UnderlyingAssemblyBuilder.SetEntryPoint(mainMethodProxy.Unpack(), (IKVM.Reflection.Emit.PEFileKinds)target);
        }

        /// <inheritdoc />
        public void AddTypeForwarder(ITypeSymbol type)
        {
            UnderlyingAssemblyBuilder.__AddTypeForwarder(type.Unpack());
        }

        /// <inheritdoc />
        public void AddResourceFile(string name, string value)
        {
            UnderlyingAssemblyBuilder.AddResourceFile(name, value);
        }

        /// <inheritdoc />
        public void Save(string assemblyFileName, System.Reflection.PortableExecutableKinds portableExecutableKind, IKVM.CoreLib.Symbols.ImageFileMachine imageFileMachine)
        {
            UnderlyingAssemblyBuilder.Save(assemblyFileName, (PortableExecutableKinds)portableExecutableKind, (IKVM.Reflection.ImageFileMachine)imageFileMachine);
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
        public IModuleSymbol ManifestModule => ResolveModuleSymbol(UnderlyingAssembly.ManifestModule);

        /// <inheritdoc />
        public IEnumerable<IModuleSymbol> Modules => ResolveModuleSymbols(UnderlyingAssembly.Modules);

        /// <inheritdoc />
        public override bool IsComplete => _builder == null;

        public string Location => throw new NotImplementedException();

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
            return ResolveCustomAttributes(UnderlyingAssembly.__GetCustomAttributes(attributeType.Unpack(), inherit));
        }

        /// <inheritdoc />
        public CustomAttribute? GetCustomAttribute(ITypeSymbol attributeType, bool inherit = false)
        {
            var _attributeType = attributeType.Unpack();
            var a = UnderlyingAssembly.__GetCustomAttributes(_attributeType, inherit);
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

        /// <inheritdoc />
        public override string ToString() => UnderlyingAssembly.ToString()!;

    }

}
