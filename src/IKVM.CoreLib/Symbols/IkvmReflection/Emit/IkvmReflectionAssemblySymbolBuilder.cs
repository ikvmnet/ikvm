using System;
using System.Collections.Generic;
using System.Linq;

using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    class IkvmReflectionAssemblySymbolBuilder : IkvmReflectionSymbolBuilder, IIkvmReflectionAssemblySymbolBuilder
    {

        readonly AssemblyBuilder _builder;
        IkvmReflectionAssemblyMetadata _metadata;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="builder"></param>
        public IkvmReflectionAssemblySymbolBuilder(IkvmReflectionSymbolContext context, AssemblyBuilder builder) :
            base(context)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _metadata = new IkvmReflectionAssemblyMetadata(this);
        }

        /// <inheritdoc />
        public Assembly UnderlyingAssembly => UnderlyingAssemblyBuilder;

        /// <inheritdoc />
        public AssemblyBuilder UnderlyingAssemblyBuilder => _builder;

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
            _builder.SetCustomAttribute(con.Unpack(), binaryAttribute);
        }

        /// <inheritdoc />
        public void SetCustomAttribute(ICustomAttributeBuilder customBuilder)
        {
            _builder.SetCustomAttribute(((IkvmReflectionCustomAttributeBuilder)customBuilder).UnderlyingBuilder);
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
        public System.Reflection.AssemblyName GetName()
        {
            return UnderlyingAssembly.GetName().ToAssemblyName();
        }

        /// <inheritdoc />
        public System.Reflection.AssemblyName GetName(bool copiedName)
        {
            return UnderlyingAssembly.GetName(copiedName).ToAssemblyName();
        }

        /// <inheritdoc />
        public System.Reflection.AssemblyName[] GetReferencedAssemblies()
        {
            return UnderlyingAssembly.GetReferencedAssemblies().ToAssemblyNames();
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
            return ResolveCustomAttribute(UnderlyingAssembly.__GetCustomAttributes(attributeType.Unpack(), inherit).FirstOrDefault());
        }

        /// <inheritdoc />
        public bool IsDefined(ITypeSymbol attributeType, bool inherit = false)
        {
            return UnderlyingAssembly.IsDefined(attributeType.Unpack(), inherit);
        }

        #endregion

        /// <inheritdoc />
        public IIkvmReflectionModuleSymbol GetOrCreateModuleSymbol(Module module)
        {
            return _metadata.GetOrCreateModuleSymbol(module);
        }

        /// <inheritdoc />
        public IIkvmReflectionModuleSymbolBuilder GetOrCreateModuleSymbol(ModuleBuilder module)
        {
            return (IIkvmReflectionModuleSymbolBuilder)_metadata.GetOrCreateModuleSymbol(module);
        }

    }

}
