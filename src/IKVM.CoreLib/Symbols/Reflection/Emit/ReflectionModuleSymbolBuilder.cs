using System;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Resources;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionModuleSymbolBuilder : ReflectionModuleSymbol, IReflectionModuleSymbolBuilder
    {

        readonly ModuleBuilder _builder;
        Module? _module;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingAssembly"></param>
        /// <param name="builder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionModuleSymbolBuilder(ReflectionSymbolContext context, IReflectionAssemblySymbol resolvingAssembly, ModuleBuilder builder) :
            base(context, resolvingAssembly, builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        /// <inheritdoc />
        public ModuleBuilder UnderlyingModuleBuilder => _builder ?? throw new InvalidOperationException();

        /// <inheritdoc />
        public override Module UnderlyingRuntimeModule => _module ?? throw new InvalidOperationException();

        #region IModuleSymbol

        /// <inheritdoc />
        public override bool IsComplete => _module != null;

        #endregion

        #region IModuleSymbolBuilder

        /// <inheritdoc />
        public ulong ImageBase
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        /// <inheritdoc />
        public uint FileAlignment
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        /// <inheritdoc />
        public System.Reflection.PortableExecutable.DllCharacteristics DllCharacteristics
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        /// <inheritdoc />
        public ISymbolDocumentWriter? DefineDocument(string url, Guid language, Guid languageVendor, Guid documentType)
        {
#if NETFRAMEWORK
            return UnderlyingModuleBuilder.DefineDocument(url, language, languageVendor, documentType);
#else
            return null;
#endif
        }

        /// <inheritdoc />
        public void DefineManifestResource(string name, Stream stream, System.Reflection.ResourceAttributes attribute)
        {
#if NETFRAMEWORK
            UnderlyingModuleBuilder.DefineManifestResource(name, stream, attribute);
#else
            throw new NotSupportedException();
#endif
        }

        /// <inheritdoc />
        public IResourceWriter DefineResource(string name, string description)
        {
#if NETFRAMEWORK
            return UnderlyingModuleBuilder.DefineResource(name, description);
#else
            throw new NotImplementedException();
#endif
        }

        /// <inheritdoc />
        public IResourceWriter DefineResource(string name, string description, System.Reflection.ResourceAttributes attribute)
        {
#if NETFRAMEWORK
            return UnderlyingModuleBuilder.DefineResource(name, description, attribute);
#else
            throw new NotImplementedException();
#endif
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefineGlobalMethod(string name, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, ITypeSymbol? returnType, ITypeSymbol[]? parameterTypes)
        {
            return ResolveMethodSymbol(UnderlyingModuleBuilder.DefineGlobalMethod(name, (MethodAttributes)attributes, (CallingConventions)callingConvention, returnType?.Unpack(), parameterTypes?.Unpack()));
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefineGlobalMethod(string name, System.Reflection.MethodAttributes attributes, System.Reflection.CallingConventions callingConvention, ITypeSymbol? returnType, ITypeSymbol[]? requiredReturnTypeCustomModifiers, ITypeSymbol[]? optionalReturnTypeCustomModifiers, ITypeSymbol[]? parameterTypes, ITypeSymbol[][]? requiredParameterTypeCustomModifiers, ITypeSymbol[][]? optionalParameterTypeCustomModifiers)
        {
            return ResolveMethodSymbol(UnderlyingModuleBuilder.DefineGlobalMethod(name, (MethodAttributes)attributes, (CallingConventions)callingConvention, returnType?.Unpack(), requiredReturnTypeCustomModifiers?.Unpack(), optionalReturnTypeCustomModifiers?.Unpack(), parameterTypes?.Unpack(), requiredParameterTypeCustomModifiers?.Unpack(), optionalParameterTypeCustomModifiers?.Unpack()));
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefineGlobalMethod(string name, System.Reflection.MethodAttributes attributes, ITypeSymbol? returnType, ITypeSymbol[]? parameterTypes)
        {
            return ResolveMethodSymbol(UnderlyingModuleBuilder.DefineGlobalMethod(name, (MethodAttributes)attributes, returnType?.Unpack(), parameterTypes?.Unpack()));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name)
        {
            return ResolveTypeSymbol(UnderlyingModuleBuilder.DefineType(name));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name, System.Reflection.TypeAttributes attr, ITypeSymbol? parent, int typesize)
        {
            return ResolveTypeSymbol(UnderlyingModuleBuilder.DefineType(name, (TypeAttributes)attr, parent?.Unpack(), typesize));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name, System.Reflection.TypeAttributes attr, ITypeSymbol? parent)
        {
            return ResolveTypeSymbol(UnderlyingModuleBuilder.DefineType(name, (TypeAttributes)attr, parent?.Unpack()));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name, System.Reflection.TypeAttributes attr)
        {
            return ResolveTypeSymbol(UnderlyingModuleBuilder.DefineType(name, (TypeAttributes)attr));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name, System.Reflection.TypeAttributes attr, ITypeSymbol? parent, System.Reflection.Emit.PackingSize packsize)
        {
            return ResolveTypeSymbol(UnderlyingModuleBuilder.DefineType(name, (TypeAttributes)attr, parent?.Unpack(), (PackingSize)packsize));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name, System.Reflection.TypeAttributes attr, ITypeSymbol? parent, System.Reflection.Emit.PackingSize packingSize, int typesize)
        {
            return ResolveTypeSymbol(UnderlyingModuleBuilder.DefineType(name, (TypeAttributes)attr, parent?.Unpack(), (PackingSize)packingSize, typesize));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name, System.Reflection.TypeAttributes attr, ITypeSymbol? parent, ITypeSymbol[]? interfaces)
        {
            return ResolveTypeSymbol(UnderlyingModuleBuilder.DefineType(name, (TypeAttributes)attr, parent?.Unpack(), interfaces?.Unpack()));
        }

        /// <inheritdoc />
        public void SetCustomAttribute(CustomAttribute attribute)
        {
            UnderlyingModuleBuilder.SetCustomAttribute(attribute.Unpack());
        }

        /// <inheritdoc />
        public void AddReference(IAssemblySymbol assembly)
        {
#if NETFRAMEWORK
            var t = ((IReflectionAssemblySymbol)assembly).GetExportedTypes();
            if (t.Length > 0)
                UnderlyingModuleBuilder.GetTypeToken(t[0].Unpack());
#else
            throw new NotSupportedException();
#endif
        }

        /// <inheritdoc />
        public void Complete()
        {
            UnderlyingModuleBuilder.CreateGlobalFunctions();

            foreach (var type in GetTypes())
                if (type is IReflectionTypeSymbolBuilder t)
                    t.Complete();

            _module = _builder;
        }

        /// <inheritdoc />
        public void Save(System.Reflection.PortableExecutableKinds portableExecutableKind, IKVM.CoreLib.Symbols.ImageFileMachine imageFileMachine)
        {
            throw new NotSupportedException();
        }

        #endregion

    }

}
