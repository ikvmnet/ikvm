using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionAssemblySymbolBuilder : ReflectionSymbolBuilder, IReflectionAssemblySymbolBuilder
    {

        readonly AssemblyBuilder _builder;
        ReflectionAssemblyMetadata _metadata;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="builder"></param>
        public ReflectionAssemblySymbolBuilder(ReflectionSymbolContext context, AssemblyBuilder builder) :
            base(context)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _metadata = new ReflectionAssemblyMetadata(this);
        }

        /// <inheritdoc />
        public Assembly UnderlyingAssembly => UnderlyingAssemblyBuilder;

        /// <inheritdoc />
        public AssemblyBuilder UnderlyingAssemblyBuilder => _builder;

        #region IAssemblySymbolBuilder

        /// <inheritdoc />
        public IModuleSymbolBuilder DefineModule(string name)
        {
            return GetOrCreateModuleSymbol(UnderlyingAssemblyBuilder.DefineDynamicModule(name));
        }

        /// <inheritdoc />
        public IModuleSymbolBuilder DefineModule(string name, string fileName)
        {
#if NETFRAMEWORK
            return GetOrCreateModuleSymbol(UnderlyingAssemblyBuilder.DefineDynamicModule(name, fileName));
#else
            return GetOrCreateModuleSymbol(UnderlyingAssemblyBuilder.DefineDynamicModule(name));
#endif
        }

        /// <inheritdoc />
        public IModuleSymbolBuilder DefineModule(string name, string fileName, bool emitSymbolInfo)
        {
#if NETFRAMEWORK
            return GetOrCreateModuleSymbol(UnderlyingAssemblyBuilder.DefineDynamicModule(name, fileName, emitSymbolInfo));
#else
            return GetOrCreateModuleSymbol(UnderlyingAssemblyBuilder.DefineDynamicModule(name));
#endif
        }

        /// <inheritdoc />
        public void SetCustomAttribute(IConstructorSymbol con, byte[] binaryAttribute)
        {
            _builder.SetCustomAttribute(con.Unpack(), binaryAttribute);
        }

        /// <inheritdoc />
        public void SetCustomAttribute(ICustomAttributeBuilder customBuilder)
        {
            _builder.SetCustomAttribute(((ReflectionCustomAttributeBuilder)customBuilder).UnderlyingBuilder);
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
        public AssemblyName GetName()
        {
            return UnderlyingAssembly.GetName();
        }

        /// <inheritdoc />
        public AssemblyName GetName(bool copiedName)
        {
            return UnderlyingAssembly.GetName(copiedName);
        }

        /// <inheritdoc />
        public AssemblyName[] GetReferencedAssemblies()
        {
            return UnderlyingAssembly.GetReferencedAssemblies();
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
            return GetCustomAttributes(attributeType, inherit).FirstOrDefault();
        }

        /// <inheritdoc />
        public bool IsDefined(ITypeSymbol attributeType, bool inherit = false)
        {
            return UnderlyingAssembly.IsDefined(attributeType.Unpack(), inherit);
        }

        #endregion

        /// <inheritdoc />
        public IReflectionModuleSymbol GetOrCreateModuleSymbol(Module module)
        {
            return _metadata.GetOrCreateModuleSymbol(module);
        }

        /// <inheritdoc />
        public IReflectionModuleSymbolBuilder GetOrCreateModuleSymbol(ModuleBuilder module)
        {
            return (IReflectionModuleSymbolBuilder)_metadata.GetOrCreateModuleSymbol(module);
        }

    }

}
