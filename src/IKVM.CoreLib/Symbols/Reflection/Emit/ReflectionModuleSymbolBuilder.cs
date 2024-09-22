using System;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Reflection.PortableExecutable;
using System.Resources;

using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionModuleSymbolBuilder : ReflectionSymbolBuilder, IReflectionModuleSymbolBuilder
    {

        readonly IReflectionAssemblySymbol _resolvingAssembly;

        readonly ModuleBuilder _builder;
        ReflectionModuleMetadata _metadata;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingAssembly"></param>
        /// <param name="builder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public ReflectionModuleSymbolBuilder(ReflectionSymbolContext context, IReflectionAssemblySymbol resolvingAssembly, ModuleBuilder builder) :
            base(context)
        {
            _resolvingAssembly = resolvingAssembly ?? throw new ArgumentNullException(nameof(resolvingAssembly));
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _metadata = new ReflectionModuleMetadata(this);
        }

        /// <inheritdoc />
        public Module UnderlyingModule => UnderlyingModuleBuilder;

        /// <inheritdoc />
        public ModuleBuilder UnderlyingModuleBuilder => _builder;

        /// <inheritdoc />
        public IReflectionAssemblySymbol ResolvingAssembly => _resolvingAssembly;

        #region IModuleSymbolBuilder

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
        public void DefineManifestResource(string name, Stream stream, ResourceAttributes attribute)
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
        public IResourceWriter DefineResource(string name, string description, ResourceAttributes attribute)
        {
#if NETFRAMEWORK
            return UnderlyingModuleBuilder.DefineResource(name, description, attribute);
#else
            throw new NotImplementedException();
#endif
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefineGlobalMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol returnType, ITypeSymbol[] parameterTypes)
        {
            return ResolveMethodSymbol(UnderlyingModuleBuilder.DefineGlobalMethod(name, attributes, callingConvention, returnType.Unpack(), parameterTypes.Unpack()));
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefineGlobalMethod(string name, MethodAttributes attributes, CallingConventions callingConvention, ITypeSymbol returnType, ITypeSymbol[] requiredReturnTypeCustomModifiers, ITypeSymbol[] optionalReturnTypeCustomModifiers, ITypeSymbol[] parameterTypes, ITypeSymbol[][] requiredParameterTypeCustomModifiers, ITypeSymbol[][] optionalParameterTypeCustomModifiers)
        {
            return ResolveMethodSymbol(UnderlyingModuleBuilder.DefineGlobalMethod(name, attributes, callingConvention, returnType.Unpack(), requiredReturnTypeCustomModifiers.Unpack(), optionalReturnTypeCustomModifiers.Unpack(), parameterTypes.Unpack(), requiredParameterTypeCustomModifiers.Unpack(), optionalParameterTypeCustomModifiers.Unpack()));
        }

        /// <inheritdoc />
        public IMethodSymbolBuilder DefineGlobalMethod(string name, MethodAttributes attributes, ITypeSymbol returnType, ITypeSymbol[] parameterTypes)
        {
            return ResolveMethodSymbol(UnderlyingModuleBuilder.DefineGlobalMethod(name, attributes, returnType.Unpack(), parameterTypes.Unpack()));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name)
        {
            return (ITypeSymbolBuilder)ResolveTypeSymbol(UnderlyingModuleBuilder.DefineType(name));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name, TypeAttributes attr, ITypeSymbol? parent, int typesize)
        {
            return (ITypeSymbolBuilder)ResolveTypeSymbol(UnderlyingModuleBuilder.DefineType(name, attr, parent?.Unpack(), typesize));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name, TypeAttributes attr, ITypeSymbol? parent)
        {
            return (ITypeSymbolBuilder)ResolveTypeSymbol(UnderlyingModuleBuilder.DefineType(name, attr, parent?.Unpack()));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name, TypeAttributes attr)
        {
            return (ITypeSymbolBuilder)ResolveTypeSymbol(UnderlyingModuleBuilder.DefineType(name, attr));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name, TypeAttributes attr, ITypeSymbol? parent, PackingSize packsize)
        {
            return (ITypeSymbolBuilder)ResolveTypeSymbol(UnderlyingModuleBuilder.DefineType(name, attr, parent?.Unpack(), packsize));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name, TypeAttributes attr, ITypeSymbol? parent, PackingSize packingSize, int typesize)
        {
            return (ITypeSymbolBuilder)ResolveTypeSymbol(UnderlyingModuleBuilder.DefineType(name, attr, parent?.Unpack(), packingSize, typesize));
        }

        /// <inheritdoc />
        public ITypeSymbolBuilder DefineType(string name, TypeAttributes attr, ITypeSymbol? parent, ITypeSymbol[]? interfaces)
        {
            return (ITypeSymbolBuilder)ResolveTypeSymbol(UnderlyingModuleBuilder.DefineType(name, attr, parent?.Unpack(), interfaces?.Unpack()));
        }

        /// <inheritdoc />
        public void SetCustomAttribute(IConstructorSymbol con, byte[] binaryAttribute)
        {
            UnderlyingModuleBuilder.SetCustomAttribute(con.Unpack(), binaryAttribute);
        }

        /// <inheritdoc />
        public void SetCustomAttribute(ICustomAttributeBuilder customBuilder)
        {
            UnderlyingModuleBuilder.SetCustomAttribute(((ReflectionCustomAttributeBuilder)customBuilder).UnderlyingBuilder);
        }

        #endregion

        #region IModuleSymbol

        /// <inheritdoc />
        public IAssemblySymbol Assembly => ResolveAssemblySymbol(UnderlyingModule.Assembly);

        /// <inheritdoc />
        public string FullyQualifiedName => UnderlyingModule.FullyQualifiedName;

        /// <inheritdoc />
        public int MetadataToken => UnderlyingModule.MetadataToken;

        /// <inheritdoc />
        public Guid ModuleVersionId => UnderlyingModule.ModuleVersionId;

        /// <inheritdoc />
        public string Name => UnderlyingModule.Name;

        /// <inheritdoc />
        public string ScopeName => UnderlyingModule.ScopeName;

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
        public DllCharacteristics DllCharacteristics
        {
            get => throw new NotSupportedException();
            set => throw new NotSupportedException();
        }

        /// <inheritdoc />
        public override bool IsComplete => _builder == null;

        /// <inheritdoc />
        public IFieldSymbol? GetField(string name)
        {
            return ResolveFieldSymbol(UnderlyingModule.GetField(name));
        }

        /// <inheritdoc />
        public IFieldSymbol? GetField(string name, BindingFlags bindingAttr)
        {
            return ResolveFieldSymbol(UnderlyingModule.GetField(name, bindingAttr));
        }

        /// <inheritdoc />
        public IFieldSymbol[] GetFields(BindingFlags bindingFlags)
        {
            return ResolveFieldSymbols(UnderlyingModule.GetFields(bindingFlags))!;
        }

        /// <inheritdoc />
        public IFieldSymbol[] GetFields()
        {
            return ResolveFieldSymbols(UnderlyingModule.GetFields())!;
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name)
        {
            return ResolveMethodSymbol(UnderlyingModule.GetMethod(name));
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, ITypeSymbol[] types)
        {
            return ResolveMethodSymbol(UnderlyingModule.GetMethod(name, types.Unpack()));
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr, CallingConventions callConvention, ITypeSymbol[] types, ParameterModifier[]? modifiers)
        {
            return ResolveMethodSymbol(UnderlyingModule.GetMethod(name, bindingAttr, null, callConvention, types.Unpack(), modifiers));
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetMethods()
        {
            return ResolveMethodSymbols(UnderlyingModule.GetMethods())!;
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetMethods(BindingFlags bindingFlags)
        {
            return ResolveMethodSymbols(UnderlyingModule.GetMethods(bindingFlags))!;
        }

        /// <inheritdoc />
        public ITypeSymbol? GetType(string className)
        {
            return ResolveTypeSymbol(UnderlyingModule.GetType(className));
        }

        /// <inheritdoc />
        public ITypeSymbol? GetType(string className, bool ignoreCase)
        {
            return ResolveTypeSymbol(UnderlyingModule.GetType(className, ignoreCase));
        }

        /// <inheritdoc />
        public ITypeSymbol? GetType(string className, bool throwOnError, bool ignoreCase)
        {
            return ResolveTypeSymbol(UnderlyingModule.GetType(className, throwOnError, ignoreCase));
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetTypes()
        {
            return ResolveTypeSymbols(UnderlyingModule.GetTypes())!;
        }

        /// <inheritdoc />
        public bool IsResource()
        {
            return UnderlyingModule.IsResource();
        }

        /// <inheritdoc />
        public IFieldSymbol? ResolveField(int metadataToken)
        {
            return ResolveFieldSymbol(UnderlyingModule.ResolveField(metadataToken));
        }

        /// <inheritdoc />
        public IFieldSymbol? ResolveField(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments)
        {
            return ResolveFieldSymbol(UnderlyingModule.ResolveField(metadataToken, genericTypeArguments?.Unpack(), genericMethodArguments?.Unpack()));
        }

        /// <inheritdoc />
        public IMemberSymbol? ResolveMember(int metadataToken)
        {
            return ResolveMemberSymbol(UnderlyingModule.ResolveMember(metadataToken));
        }

        /// <inheritdoc />
        public IMemberSymbol? ResolveMember(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments)
        {
            return ResolveMemberSymbol(UnderlyingModule.ResolveMember(metadataToken, genericTypeArguments?.Unpack(), genericMethodArguments?.Unpack()));
        }

        /// <inheritdoc />
        public IMethodBaseSymbol? ResolveMethod(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments)
        {
            return ResolveMethodBaseSymbol(UnderlyingModule.ResolveMethod(metadataToken, genericTypeArguments?.Unpack(), genericMethodArguments?.Unpack()));
        }

        /// <inheritdoc />
        public IMethodBaseSymbol? ResolveMethod(int metadataToken)
        {
            return ResolveMethodBaseSymbol(UnderlyingModule.ResolveMethod(metadataToken));
        }

        /// <inheritdoc />
        public byte[] ResolveSignature(int metadataToken)
        {
            return UnderlyingModule.ResolveSignature(metadataToken);
        }

        /// <inheritdoc />
        public string ResolveString(int metadataToken)
        {
            return UnderlyingModule.ResolveString(metadataToken);
        }

        /// <inheritdoc />
        public ITypeSymbol ResolveType(int metadataToken)
        {
            return ResolveTypeSymbol(UnderlyingModule.ResolveType(metadataToken));
        }

        /// <inheritdoc />
        public ITypeSymbol ResolveType(int metadataToken, ITypeSymbol[]? genericTypeArguments, ITypeSymbol[]? genericMethodArguments)
        {
            return ResolveTypeSymbol(UnderlyingModule.ResolveType(metadataToken, genericTypeArguments?.Unpack(), genericMethodArguments?.Unpack()));
        }

        /// <inheritdoc />
        public CustomAttribute[] GetCustomAttributes(bool inherit = false)
        {
            return ResolveCustomAttributes(UnderlyingModule.GetCustomAttributesData());
        }

        /// <inheritdoc />
        public CustomAttribute[] GetCustomAttributes(ITypeSymbol attributeType, bool inherit = false)
        {
            var _attributeType = attributeType.Unpack();
            return ResolveCustomAttributes(UnderlyingModule.GetCustomAttributesData().Where(i => i.AttributeType == _attributeType).ToArray());
        }

        /// <inheritdoc />
        public CustomAttribute? GetCustomAttribute(ITypeSymbol attributeType, bool inherit = false)
        {
            return GetCustomAttributes(attributeType, inherit).FirstOrDefault();
        }

        /// <inheritdoc />
        public bool IsDefined(ITypeSymbol attributeType, bool inherit = false)
        {
            return UnderlyingModule.IsDefined(attributeType.Unpack(), false);
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
        public void AddTypeForwarder(ITypeSymbol type)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public void Complete()
        {
            UnderlyingModuleBuilder.CreateGlobalFunctions();
        }

        /// <inheritdoc />
        public void Save(PortableExecutableKinds pekind, ImageFileMachine imageFileMachine)
        {
            throw new NotSupportedException();
        }

        #endregion

        /// <inheritdoc />
        public IReflectionTypeSymbol GetOrCreateTypeSymbol(Type type)
        {
            return _metadata.GetOrCreateTypeSymbol(type);
        }

        /// <inheritdoc />
        public IReflectionTypeSymbolBuilder GetOrCreateTypeSymbol(TypeBuilder type)
        {
            return _metadata.GetOrCreateTypeSymbol(type);
        }

        /// <inheritdoc />
        public IReflectionConstructorSymbol GetOrCreateConstructorSymbol(ConstructorInfo ctor)
        {
            return _metadata.GetOrCreateConstructorSymbol(ctor);
        }

        /// <inheritdoc />
        public IReflectionConstructorSymbolBuilder GetOrCreateConstructorSymbol(ConstructorBuilder ctor)
        {
            return _metadata.GetOrCreateConstructorSymbol(ctor);
        }

        /// <inheritdoc />
        public IReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method)
        {
            return _metadata.GetOrCreateMethodSymbol(method);
        }

        /// <inheritdoc />
        public IReflectionMethodSymbolBuilder GetOrCreateMethodSymbol(MethodBuilder method)
        {
            return _metadata.GetOrCreateMethodSymbol(method);
        }

        /// <inheritdoc />
        public IReflectionFieldSymbol GetOrCreateFieldSymbol(FieldInfo field)
        {
            return _metadata.GetOrCreateFieldSymbol(field);
        }

        /// <inheritdoc />
        public IReflectionFieldSymbolBuilder GetOrCreateFieldSymbol(FieldBuilder field)
        {
            return _metadata.GetOrCreateFieldSymbol(field);
        }

        /// <inheritdoc />
        public IReflectionPropertySymbol GetOrCreatePropertySymbol(PropertyInfo property)
        {
            return _metadata.GetOrCreatePropertySymbol(property);
        }

        /// <inheritdoc />
        public IReflectionPropertySymbolBuilder GetOrCreatePropertySymbol(PropertyBuilder property)
        {
            return _metadata.GetOrCreatePropertySymbol(property);
        }

        /// <inheritdoc />
        public IReflectionEventSymbol GetOrCreateEventSymbol(EventInfo @event)
        {
            return _metadata.GetOrCreateEventSymbol(@event);
        }

        /// <inheritdoc />
        public IReflectionEventSymbolBuilder GetOrCreateEventSymbol(EventBuilder @event)
        {
            return _metadata.GetOrCreateEventSymbol(@event);
        }

        /// <inheritdoc />
        public IReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter)
        {
            return _metadata.GetOrCreateParameterSymbol(parameter);
        }

        /// <inheritdoc />
        public IReflectionParameterSymbolBuilder GetOrCreateParameterSymbol(ParameterBuilder parameter)
        {
            return _metadata.GetOrCreateParameterSymbol(parameter);
        }

    }

}
