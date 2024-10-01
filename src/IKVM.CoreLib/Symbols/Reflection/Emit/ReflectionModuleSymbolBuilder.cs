using System;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Resources;

using IKVM.CoreLib.Reflection;
using IKVM.CoreLib.Symbols.Emit;

namespace IKVM.CoreLib.Symbols.Reflection.Emit
{

    class ReflectionModuleSymbolBuilder : ReflectionSymbolBuilder, IReflectionModuleSymbolBuilder
    {

        readonly IReflectionAssemblySymbol _resolvingAssembly;
        Module _module;
        ModuleBuilder? _builder;

        ReflectionTypeTable _typeTable;
        ReflectionMethodTable _methodTable;
        ReflectionFieldTable _fieldTable;

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
            _module = _builder;

            _typeTable = new ReflectionTypeTable(context, this, null);
            _methodTable = new ReflectionMethodTable(context, this, null);
            _fieldTable = new ReflectionFieldTable(context, this, null);
        }

        /// <inheritdoc />
        public Module UnderlyingModule => _module;

        /// <inheritdoc />
        public ModuleBuilder UnderlyingModuleBuilder => _builder ?? throw new InvalidOperationException();

        /// <inheritdoc />
        public IReflectionAssemblySymbol ResolvingAssembly => _resolvingAssembly;

        #region IReflectionModuleSymbol

        /// <inheritdoc />
        public IReflectionTypeSymbol GetOrCreateTypeSymbol(Type type)
        {
            if (type.IsTypeDefinition())
                return _typeTable.GetOrCreateTypeSymbol(type);
            else if (type.IsGenericType)
                return ResolveTypeSymbol(type.GetGenericTypeDefinition()).GetOrCreateGenericTypeSymbol(type.GetGenericArguments());
            else if (type.IsSZArray())
                return ResolveTypeSymbol(type.GetElementType()!).GetOrCreateSZArrayTypeSymbol();
            else if (type.IsArray)
                return ResolveTypeSymbol(type.GetElementType()!).GetOrCreateArrayTypeSymbol(type.GetArrayRank());
            else if (type.IsPointer)
                return ResolveTypeSymbol(type.GetElementType()!).GetOrCreatePointerTypeSymbol();
            else if (type.IsByRef)
                return ResolveTypeSymbol(type.GetElementType()!).GetOrCreateByRefTypeSymbol();
            else if (type.IsGenericParameter && type.DeclaringMethod is MethodInfo dm)
                return ResolveMethodSymbol(dm).GetOrCreateGenericTypeParameterSymbol(type);
            else if (type.IsGenericParameter)
                return ResolveTypeSymbol(type.DeclaringType!).GetOrCreateGenericTypeParameterSymbol(type);
            else
                throw new InvalidOperationException();
        }

        /// <inheritdoc />
        public IReflectionTypeSymbolBuilder GetOrCreateTypeSymbol(TypeBuilder type)
        {
            return (IReflectionTypeSymbolBuilder)_typeTable.GetOrCreateTypeSymbol((Type)type);
        }

        /// <inheritdoc />
        public IReflectionMethodBaseSymbol GetOrCreateMethodBaseSymbol(MethodBase method)
        {
            if (method.DeclaringType is { } dt)
                return ResolveTypeSymbol(dt).GetOrCreateMethodBaseSymbol(method);
            else
                return _methodTable.GetOrCreateMethodBaseSymbol(method);
        }

        /// <inheritdoc />
        public IReflectionConstructorSymbol GetOrCreateConstructorSymbol(ConstructorInfo ctor)
        {
            return ResolveTypeSymbol(ctor.DeclaringType!).GetOrCreateConstructorSymbol(ctor);
        }

        /// <inheritdoc />
        public IReflectionConstructorSymbolBuilder GetOrCreateConstructorSymbol(ConstructorBuilder ctor)
        {
            return ResolveTypeSymbol((TypeBuilder)ctor.DeclaringType!).GetOrCreateConstructorSymbol(ctor);
        }

        /// <inheritdoc />
        public IReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method)
        {
            if (method.DeclaringType is { } dt)
                return ResolveTypeSymbol(dt).GetOrCreateMethodSymbol(method);
            else
                return _methodTable.GetOrCreateMethodSymbol(method);
        }

        /// <inheritdoc />
        public IReflectionMethodSymbolBuilder GetOrCreateMethodSymbol(MethodBuilder method)
        {
            if (method.DeclaringType is { } dt)
                return ResolveTypeSymbol((TypeBuilder)dt).GetOrCreateMethodSymbol(method);
            else
                return _methodTable.GetOrCreateMethodSymbol(method);
        }

        /// <inheritdoc />
        public IReflectionFieldSymbol GetOrCreateFieldSymbol(FieldInfo field)
        {
            if (field.DeclaringType is { } dt)
                return ResolveTypeSymbol(dt).GetOrCreateFieldSymbol(field);
            else
                return _fieldTable.GetOrCreateFieldSymbol(field);
        }

        /// <inheritdoc />
        public IReflectionFieldSymbolBuilder GetOrCreateFieldSymbol(FieldBuilder field)
        {
            if (field.DeclaringType is { } dt)
                return ResolveTypeSymbol((TypeBuilder)dt).GetOrCreateFieldSymbol(field);
            else
                return _fieldTable.GetOrCreateFieldSymbol(field);
        }

        /// <inheritdoc />
        public IReflectionPropertySymbol GetOrCreatePropertySymbol(PropertyInfo property)
        {
            return ResolveTypeSymbol(property.DeclaringType!).GetOrCreatePropertySymbol(property);
        }

        /// <inheritdoc />
        public IReflectionPropertySymbolBuilder GetOrCreatePropertySymbol(PropertyBuilder property)
        {
            return ResolveTypeSymbol(property.GetTypeBuilder()).GetOrCreatePropertySymbol(property);
        }

        /// <inheritdoc />
        public IReflectionEventSymbol GetOrCreateEventSymbol(EventInfo @event)
        {
            return ResolveTypeSymbol(@event.DeclaringType!).GetOrCreateEventSymbol(@event);
        }

        /// <inheritdoc />
        public IReflectionEventSymbolBuilder GetOrCreateEventSymbol(EventBuilder @event)
        {
            return ResolveTypeSymbol(@event.GetTypeBuilder()).GetOrCreateEventSymbol(@event);
        }

        /// <inheritdoc />
        public IReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter)
        {
            return ResolveMemberSymbol(parameter.Member) switch
            {
                IReflectionMethodBaseSymbol method => method.GetOrCreateParameterSymbol(parameter),
                IReflectionPropertySymbol property => property.GetOrCreateParameterSymbol(parameter),
                _ => throw new InvalidOperationException(),
            };
        }

        /// <inheritdoc />
        public IReflectionParameterSymbolBuilder GetOrCreateParameterSymbol(IReflectionMemberSymbolBuilder member, ParameterBuilder parameter)
        {
            return member switch
            {
                IReflectionMethodBaseSymbolBuilder method => method.GetOrCreateParameterSymbol(parameter),
                IReflectionPropertySymbolBuilder property => property.GetOrCreateParameterSymbol(parameter),
                _ => throw new InvalidOperationException(),
            };
        }

        /// <inheritdoc />
        public IReflectionGenericTypeParameterSymbolBuilder GetOrCreateGenericTypeParameterSymbol(GenericTypeParameterBuilder genericParameterType)
        {
            if (genericParameterType.DeclaringMethod is MethodBuilder dm)
                return ResolveMethodSymbol(dm).GetOrCreateGenericTypeParameterSymbol(genericParameterType);
            else
                return ResolveTypeSymbol((TypeBuilder)genericParameterType.DeclaringType!).GetOrCreateGenericTypeParameterSymbol(genericParameterType);
        }

        #endregion

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
        public void SetCustomAttribute(IConstructorSymbol con, byte[] binaryAttribute)
        {
            UnderlyingModuleBuilder.SetCustomAttribute(con.Unpack(), binaryAttribute);
        }

        /// <inheritdoc />
        public void SetCustomAttribute(ICustomAttributeBuilder customBuilder)
        {
            UnderlyingModuleBuilder.SetCustomAttribute(((ReflectionCustomAttributeBuilder)customBuilder).UnderlyingBuilder);
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

            _builder = null;
        }

        /// <inheritdoc />
        public void Save(System.Reflection.PortableExecutableKinds portableExecutableKind, IKVM.CoreLib.Symbols.ImageFileMachine imageFileMachine)
        {
            throw new NotSupportedException();
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
        public System.Reflection.PortableExecutable.DllCharacteristics DllCharacteristics
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
        public IFieldSymbol? GetField(string name, System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveFieldSymbol(UnderlyingModule.GetField(name, (BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public IFieldSymbol[] GetFields(System.Reflection.BindingFlags bindingFlags)
        {
            return ResolveFieldSymbols(UnderlyingModule.GetFields((BindingFlags)bindingFlags))!;
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
        public IMethodSymbol? GetMethod(string name, System.Reflection.BindingFlags bindingAttr, System.Reflection.CallingConventions callConvention, ITypeSymbol[] types, System.Reflection.ParameterModifier[]? modifiers)
        {
            return ResolveMethodSymbol(UnderlyingModule.GetMethod(name, (BindingFlags)bindingAttr, null, (CallingConventions)callConvention, types.Unpack(), modifiers?.Unpack()));
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetMethods()
        {
            return ResolveMethodSymbols(UnderlyingModule.GetMethods())!;
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetMethods(System.Reflection.BindingFlags bindingFlags)
        {
            return ResolveMethodSymbols(UnderlyingModule.GetMethods((BindingFlags)bindingFlags))!;
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
        public virtual CustomAttribute[] GetCustomAttributes(ITypeSymbol attributeType, bool inherit = false)
        {
            var _attributeType = attributeType.Unpack();
            return ResolveCustomAttributes(UnderlyingModule.GetCustomAttributesData().Where(i => i.AttributeType == _attributeType).ToArray());
        }

        /// <inheritdoc />
        public virtual CustomAttribute? GetCustomAttribute(ITypeSymbol attributeType, bool inherit = false)
        {
            var _attributeType = attributeType.Unpack();
            return ResolveCustomAttribute(UnderlyingModule.GetCustomAttributesData().Where(i => i.AttributeType == _attributeType).FirstOrDefault());
        }

        /// <inheritdoc />
        public bool IsDefined(ITypeSymbol attributeType, bool inherit = false)
        {
            return UnderlyingModule.IsDefined(attributeType.Unpack(), false);
        }

        #endregion

    }

}
