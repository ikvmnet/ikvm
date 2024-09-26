using System;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Resources;

using IKVM.CoreLib.Symbols.Emit;
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    class IkvmReflectionModuleSymbolBuilder : IkvmReflectionSymbolBuilder, IIkvmReflectionModuleSymbolBuilder
    {

        readonly IIkvmReflectionAssemblySymbol _resolvingAssembly;
        readonly ModuleBuilder _module;

        IkvmReflectionTypeTable _typeTable;
        IkvmReflectionMethodTable _methodTable;
        IkvmReflectionFieldTable _fieldTable;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingAssembly"></param>
        /// <param name="module"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReflectionModuleSymbolBuilder(IkvmReflectionSymbolContext context, IIkvmReflectionAssemblySymbol resolvingAssembly, ModuleBuilder module) :
            base(context)
        {
            _resolvingAssembly = resolvingAssembly ?? throw new ArgumentNullException(nameof(resolvingAssembly));
            _module = module ?? throw new ArgumentNullException(nameof(module));

            _typeTable = new IkvmReflectionTypeTable(context, this, null);
            _methodTable = new IkvmReflectionMethodTable(context, this, null);
            _fieldTable = new IkvmReflectionFieldTable(context, this, null);
        }

        /// <inheritdoc />
        public Module UnderlyingModule => UnderlyingModuleBuilder;

        /// <inheritdoc />
        public ModuleBuilder UnderlyingModuleBuilder => _module;

        /// <inheritdoc />
        public IIkvmReflectionAssemblySymbol ResolvingAssembly => _resolvingAssembly;

        #region IIkvmReflectionModuleSymbol

        /// <inheritdoc />
        public IIkvmReflectionTypeSymbol GetOrCreateTypeSymbol(Type type)
        {
            if (type.IsTypeDefinition())
                return _typeTable.GetOrCreateTypeSymbol(type);
            else if (type.IsGenericType)
                return ResolveTypeSymbol(type.GetGenericTypeDefinition()).GetOrCreateGenericTypeSymbol(type.GetGenericArguments());
            else if (type.IsSZArray)
                return ResolveTypeSymbol(type.GetElementType()).GetOrCreateSZArrayTypeSymbol();
            else if (type.IsArray)
                return ResolveTypeSymbol(type.GetElementType()).GetOrCreateArrayTypeSymbol(type.GetArrayRank());
            else if (type.IsPointer)
                return ResolveTypeSymbol(type.GetElementType()).GetOrCreatePointerTypeSymbol();
            else if (type.IsByRef)
                return ResolveTypeSymbol(type.GetElementType()).GetOrCreateByRefTypeSymbol();
            else if (type.IsGenericParameter && type.DeclaringMethod is MethodInfo dm)
                return ResolveMethodSymbol(dm).GetOrCreateGenericTypeParameterSymbol(type);
            else if (type.IsGenericParameter)
                return ResolveTypeSymbol(type.DeclaringType).GetOrCreateGenericTypeParameterSymbol(type);
            else
                throw new InvalidOperationException();
        }

        /// <inheritdoc />
        public IIkvmReflectionTypeSymbolBuilder GetOrCreateTypeSymbol(TypeBuilder type)
        {
            return (IIkvmReflectionTypeSymbolBuilder)_typeTable.GetOrCreateTypeSymbol((Type)type);
        }

        /// <inheritdoc />
        public IIkvmReflectionMethodBaseSymbol GetOrCreateMethodBaseSymbol(MethodBase method)
        {
            if (method.DeclaringType is { } dt)
                return ResolveTypeSymbol(dt).GetOrCreateMethodBaseSymbol(method);
            else
                return _methodTable.GetOrCreateMethodBaseSymbol(method);
        }

        /// <inheritdoc />
        public IIkvmReflectionConstructorSymbol GetOrCreateConstructorSymbol(ConstructorInfo ctor)
        {
            return ResolveTypeSymbol(ctor.DeclaringType).GetOrCreateConstructorSymbol(ctor);
        }

        /// <inheritdoc />
        public IIkvmReflectionConstructorSymbolBuilder GetOrCreateConstructorSymbol(ConstructorBuilder ctor)
        {
            return ResolveTypeSymbol((TypeBuilder)ctor.DeclaringType).GetOrCreateConstructorSymbol(ctor);
        }

        /// <inheritdoc />
        public IIkvmReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method)
        {
            if (method.DeclaringType is { } dt)
                return ResolveTypeSymbol(dt).GetOrCreateMethodSymbol(method);
            else
                return _methodTable.GetOrCreateMethodSymbol(method);
        }

        /// <inheritdoc />
        public IIkvmReflectionMethodSymbolBuilder GetOrCreateMethodSymbol(MethodBuilder method)
        {
            if (method.DeclaringType is { } dt)
                return ResolveTypeSymbol((TypeBuilder)dt).GetOrCreateMethodSymbol(method);
            else
                return _methodTable.GetOrCreateMethodSymbol(method);
        }

        /// <inheritdoc />
        public IIkvmReflectionFieldSymbol GetOrCreateFieldSymbol(FieldInfo field)
        {
            if (field.DeclaringType is { } dt)
                return ResolveTypeSymbol(dt).GetOrCreateFieldSymbol(field);
            else
                return _fieldTable.GetOrCreateFieldSymbol(field);
        }

        /// <inheritdoc />
        public IIkvmReflectionFieldSymbolBuilder GetOrCreateFieldSymbol(FieldBuilder field)
        {
            if (field.DeclaringType is { } dt)
                return ResolveTypeSymbol((TypeBuilder)dt).GetOrCreateFieldSymbol(field);
            else
                return _fieldTable.GetOrCreateFieldSymbol(field);
        }

        /// <inheritdoc />
        public IIkvmReflectionPropertySymbol GetOrCreatePropertySymbol(PropertyInfo property)
        {
            return ResolveTypeSymbol(property.DeclaringType).GetOrCreatePropertySymbol(property);
        }

        /// <inheritdoc />
        public IIkvmReflectionPropertySymbolBuilder GetOrCreatePropertySymbol(PropertyBuilder property)
        {
            return ResolveTypeSymbol((TypeBuilder)property.DeclaringType).GetOrCreatePropertySymbol(property);
        }

        /// <inheritdoc />
        public IIkvmReflectionEventSymbol GetOrCreateEventSymbol(EventInfo @event)
        {
            return ResolveTypeSymbol(@event.DeclaringType).GetOrCreateEventSymbol(@event);
        }

        /// <inheritdoc />
        public IIkvmReflectionEventSymbolBuilder GetOrCreateEventSymbol(EventBuilder @event)
        {
            return ResolveTypeSymbol((TypeBuilder)@event.DeclaringType).GetOrCreateEventSymbol(@event);
        }

        /// <inheritdoc />
        public IIkvmReflectionParameterSymbol GetOrCreateParameterSymbol(ParameterInfo parameter)
        {
            return ResolveMemberSymbol(parameter.Member) switch
            {
                IIkvmReflectionMethodBaseSymbol method => method.GetOrCreateParameterSymbol(parameter),
                IIkvmReflectionPropertySymbol property => property.GetOrCreateParameterSymbol(parameter),
                _ => throw new InvalidOperationException(),
            };
        }

        /// <inheritdoc />
        public IIkvmReflectionParameterSymbolBuilder GetOrCreateParameterSymbol(IIkvmReflectionMemberSymbolBuilder member, ParameterBuilder parameter)
        {
            return member switch
            {
                IIkvmReflectionMethodBaseSymbolBuilder method => method.GetOrCreateParameterSymbol(parameter),
                IIkvmReflectionPropertySymbolBuilder property => property.GetOrCreateParameterSymbol(parameter),
                _ => throw new InvalidOperationException(),
            };
        }

        /// <inheritdoc />
        public IIkvmReflectionGenericTypeParameterSymbolBuilder GetOrCreateGenericTypeParameterSymbol(GenericTypeParameterBuilder genericParameterType)
        {
            if (genericParameterType.DeclaringMethod is MethodBuilder dm)
                return ResolveMethodSymbol(dm).GetOrCreateGenericTypeParameterSymbol(genericParameterType);
            else
                return ResolveTypeSymbol((TypeBuilder)genericParameterType.DeclaringType).GetOrCreateGenericTypeParameterSymbol(genericParameterType);
        }

        #endregion

        #region IModuleSymbolBuilder

        /// <inheritdoc />
        public ISymbolDocumentWriter? DefineDocument(string url, Guid language, Guid languageVendor, Guid documentType)
        {
            return UnderlyingModuleBuilder.DefineDocument(url, language, languageVendor, documentType);
        }

        /// <inheritdoc />
        public void DefineManifestResource(string name, Stream stream, System.Reflection.ResourceAttributes attribute)
        {
            UnderlyingModuleBuilder.DefineManifestResource(name, stream, (ResourceAttributes)attribute);
        }

        /// <inheritdoc />
        public IResourceWriter DefineResource(string name, string description)
        {
            return UnderlyingModuleBuilder.DefineResource(name, description);
        }

        /// <inheritdoc />
        public IResourceWriter DefineResource(string name, string description, System.Reflection.ResourceAttributes attribute)
        {
            return UnderlyingModuleBuilder.DefineResource(name, description, (ResourceAttributes)attribute);
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
            UnderlyingModuleBuilder.SetCustomAttribute(((IkvmReflectionCustomAttributeBuilder)customBuilder).UnderlyingBuilder);
        }

        /// <inheritdoc />
        public void AddReference(IAssemblySymbol assembly)
        {
            var t = ((IIkvmReflectionAssemblySymbol)assembly).GetExportedTypes();
            if (t.Length > 0)
                UnderlyingModuleBuilder.GetTypeToken(t[0].Unpack());
        }

        /// <inheritdoc />
        public void Complete()
        {
            UnderlyingModuleBuilder.CreateGlobalFunctions();
        }

        /// <inheritdoc />
        public void Save(System.Reflection.PortableExecutableKinds portableExecutableKind, IKVM.CoreLib.Symbols.ImageFileMachine imageFileMachine)
        {
            UnderlyingModuleBuilder.__Save((PortableExecutableKinds)portableExecutableKind, (IKVM.Reflection.ImageFileMachine)imageFileMachine);
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
        public override bool IsComplete => _module == null;

        /// <inheritdoc />
        public ulong ImageBase
        {
            get => UnderlyingModule.__ImageBase;
            set => UnderlyingModuleBuilder.__ImageBase = value;
        }

        /// <inheritdoc />
        public uint FileAlignment
        {
            get => UnderlyingModule.__FileAlignment;
            set => UnderlyingModuleBuilder.__FileAlignment = value;
        }

        /// <inheritdoc />
        public System.Reflection.PortableExecutable.DllCharacteristics DllCharacteristics
        {
            get => (System.Reflection.PortableExecutable.DllCharacteristics)UnderlyingModule.__DllCharacteristics;
            set => UnderlyingModuleBuilder.__DllCharacteristics = (IKVM.Reflection.DllCharacteristics)value;
        }

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
            return ResolveCustomAttributes(UnderlyingModule.__GetCustomAttributes(attributeType.Unpack(), inherit));
        }

        /// <inheritdoc />
        public virtual CustomAttribute? GetCustomAttribute(ITypeSymbol attributeType, bool inherit = false)
        {
            return ResolveCustomAttribute(UnderlyingModule.__GetCustomAttributes(attributeType.Unpack(), inherit).FirstOrDefault());
        }

        /// <inheritdoc />
        public bool IsDefined(ITypeSymbol attributeType, bool inherit = false)
        {
            return UnderlyingModule.IsDefined(attributeType.Unpack(), false);
        }

        #endregion

    }

}
