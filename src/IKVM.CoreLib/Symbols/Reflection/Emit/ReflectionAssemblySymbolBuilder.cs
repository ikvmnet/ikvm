using System;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionAssemblySymbolBuilder : ReflectionAssemblySymbol, IReflectionAssemblySymbolBuilder
    {

        readonly AssemblyBuilder _builder;
        Assembly? _assembly;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="builder"></param>
        public ReflectionAssemblySymbolBuilder(ReflectionSymbolContext context, AssemblyBuilder builder) :
            base(context, builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <inheritdoc />
        public AssemblyBuilder UnderlyingAssemblyBuilder => _builder;

        /// <inheritdoc />
        public override Assembly UnderlyingRuntimeAssembly => _assembly ?? throw new InvalidOperationException();

        #region IReflectionAssemblySymbolBuilder

        /// <inheritdoc />
        public IReflectionModuleSymbolBuilder GetOrCreateModuleSymbol(ModuleBuilder module)
        {
            return (IReflectionModuleSymbolBuilder)base.GetOrCreateModuleSymbol(module);
        }

        #endregion

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
            if (fileName == null)
                return GetOrCreateModuleSymbol(UnderlyingAssemblyBuilder.DefineDynamicModule(name));
            else
                return GetOrCreateModuleSymbol(UnderlyingAssemblyBuilder.DefineDynamicModule(name, fileName));
#else
            return GetOrCreateModuleSymbol(UnderlyingAssemblyBuilder.DefineDynamicModule(name));
#endif
        }

        /// <inheritdoc />
        public IModuleSymbolBuilder DefineModule(string name, string fileName, bool emitSymbolInfo)
        {
#if NETFRAMEWORK
            if (fileName == null)
                return GetOrCreateModuleSymbol(UnderlyingAssemblyBuilder.DefineDynamicModule(name, emitSymbolInfo));
            else
                return GetOrCreateModuleSymbol(UnderlyingAssemblyBuilder.DefineDynamicModule(name, fileName, emitSymbolInfo));
#else
            return GetOrCreateModuleSymbol(UnderlyingAssemblyBuilder.DefineDynamicModule(name));
#endif
        }

        /// <inheritdoc />
        public void SetCustomAttribute(CustomAttribute attribute)
        {
            UnderlyingAssemblyBuilder.SetCustomAttribute(attribute.Unpack());
        }

        /// <inheritdoc />
        public void DefineIconResource(byte[] bytes)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public void DefineManifestResource(byte[] bytes)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public void DefineVersionInfoResource()
        {
#if NETFRAMEWORK
            UnderlyingAssemblyBuilder.DefineVersionInfoResource();
#else
            throw new NotSupportedException();
#endif
        }

        /// <inheritdoc />
        public void SetEntryPoint(IMethodSymbol mainMethodProxy)
        {
#if NETFRAMEWORK
            UnderlyingAssemblyBuilder.SetEntryPoint(mainMethodProxy.Unpack());
#else
            throw new NotSupportedException();
#endif
        }

        /// <inheritdoc />
        public void SetEntryPoint(IMethodSymbol mainMethodProxy, IKVM.CoreLib.Symbols.Emit.PEFileKinds target)
        {
#if NETFRAMEWORK
            UnderlyingAssemblyBuilder.SetEntryPoint(mainMethodProxy.Unpack(), (System.Reflection.Emit.PEFileKinds)target);
#else
            throw new NotSupportedException();
#endif
        }

        /// <inheritdoc />
        public void AddTypeForwarder(ITypeSymbol type)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public void AddResourceFile(string name, string fileName)
        {
#if NETFRAMEWORK
            UnderlyingAssemblyBuilder.AddResourceFile(name, fileName);
#else
            throw new NotSupportedException();
#endif
        }

        /// <inheritdoc />
        public void Save(string assemblyFileName, PortableExecutableKinds portableExecutableKind, IKVM.CoreLib.Symbols.ImageFileMachine imageFileMachine)
        {
#if NETFRAMEWORK
            UnderlyingAssemblyBuilder.Save(assemblyFileName, portableExecutableKind, (System.Reflection.ImageFileMachine)imageFileMachine);
#else
            throw new NotSupportedException();
#endif
        }

        #endregion

        #region IAssemblySymbol

        /// <inheritdoc />
        public override bool IsComplete => _assembly != null;

        #endregion

    }

}
