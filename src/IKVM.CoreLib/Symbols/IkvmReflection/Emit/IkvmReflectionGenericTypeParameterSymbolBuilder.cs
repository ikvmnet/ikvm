using System;

using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    class IkvmReflectionGenericTypeParameterSymbolBuilder : IkvmReflectionMemberSymbolBuilder, IIkvmReflectionGenericTypeParameterSymbolBuilder
    {

        readonly IIkvmReflectionMemberSymbol? _resolvingMember;

        GenericTypeParameterBuilder? _builder;
        Type _type;

        IkvmReflectionTypeSpecTable _specTable;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingMember"></param>
        /// <param name="builder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReflectionGenericTypeParameterSymbolBuilder(IkvmReflectionSymbolContext context, IIkvmReflectionModuleSymbolBuilder resolvingModule, IIkvmReflectionMemberSymbolBuilder resolvingMember, GenericTypeParameterBuilder builder) :
            base(context, resolvingModule, resolvingMember as IIkvmReflectionTypeSymbolBuilder)
        {
            _resolvingMember = resolvingMember ?? throw new ArgumentNullException(nameof(resolvingMember));
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _type = _builder;
            _specTable = new IkvmReflectionTypeSpecTable();
        }

        /// <inheritdoc />
        public Type UnderlyingType => _type;

        /// <inheritdoc />
        public override MemberInfo UnderlyingMember => UnderlyingType;

        #region IIkvmReflectionGenericTypeParameterSymbolBuilder

        /// <inheritdoc />
        public GenericTypeParameterBuilder UnderlyingGenericTypeParameterBuilder => _builder ?? throw new InvalidOperationException();

        #endregion

        #region IIkvmReflectionTypeSymbol

        /// <inheritdoc />
        public IIkvmReflectionMethodBaseSymbol GetOrCreateMethodBaseSymbol(MethodBase method)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public IIkvmReflectionConstructorSymbol GetOrCreateConstructorSymbol(ConstructorInfo ctor)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public IIkvmReflectionConstructorSymbolBuilder GetOrCreateConstructorSymbol(ConstructorBuilder ctor)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public IIkvmReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public IIkvmReflectionMethodSymbolBuilder GetOrCreateMethodSymbol(MethodBuilder method)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public IIkvmReflectionFieldSymbol GetOrCreateFieldSymbol(FieldInfo field)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public IIkvmReflectionFieldSymbolBuilder GetOrCreateFieldSymbol(FieldBuilder field)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public IIkvmReflectionPropertySymbol GetOrCreatePropertySymbol(PropertyInfo property)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public IIkvmReflectionPropertySymbolBuilder GetOrCreatePropertySymbol(PropertyBuilder property)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public IIkvmReflectionEventSymbol GetOrCreateEventSymbol(EventInfo @event)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public IIkvmReflectionEventSymbolBuilder GetOrCreateEventSymbol(EventBuilder @event)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public IIkvmReflectionTypeSymbol GetOrCreateGenericTypeParameterSymbol(Type type)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public IIkvmReflectionGenericTypeParameterSymbolBuilder GetOrCreateGenericTypeSymbol(GenericTypeParameterBuilder genericType)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        public IIkvmReflectionTypeSymbol GetOrCreateSZArrayTypeSymbol()
        {
            return _specTable.GetOrCreateSZArrayTypeSymbol();
        }

        /// <inheritdoc />
        public IIkvmReflectionTypeSymbol GetOrCreateArrayTypeSymbol(int rank)
        {
            return _specTable.GetOrCreateArrayTypeSymbol(rank);
        }

        /// <inheritdoc />
        public IIkvmReflectionTypeSymbol GetOrCreatePointerTypeSymbol()
        {
            return _specTable.GetOrCreatePointerTypeSymbol();
        }

        /// <inheritdoc />
        public IIkvmReflectionTypeSymbol GetOrCreateByRefTypeSymbol()
        {
            return _specTable.GetOrCreateByRefTypeSymbol();
        }

        /// <inheritdoc />
        public IIkvmReflectionTypeSymbol GetOrCreateGenericTypeSymbol(IIkvmReflectionTypeSymbol[] genericTypeDefinition)
        {
            return _specTable.GetOrCreateGenericTypeSymbol(genericTypeDefinition);
        }

        #endregion

        #region ITypeSymbolBuilder

        /// <inheritdoc />
        public void SetBaseTypeConstraint(ITypeSymbol? baseTypeConstraint)
        {
            UnderlyingGenericTypeParameterBuilder.SetBaseTypeConstraint(baseTypeConstraint?.Unpack());
        }

        /// <inheritdoc />
        public void SetGenericParameterAttributes(System.Reflection.GenericParameterAttributes genericParameterAttributes)
        {
            UnderlyingGenericTypeParameterBuilder.SetGenericParameterAttributes((GenericParameterAttributes)genericParameterAttributes);
        }

        /// <inheritdoc />
        public void SetInterfaceConstraints(params ITypeSymbol[]? interfaceConstraints)
        {
            UnderlyingGenericTypeParameterBuilder.SetInterfaceConstraints(interfaceConstraints?.Unpack());
        }

        #endregion

        #region ITypeSymbol

        /// <inheritdoc />
        public IAssemblySymbol Assembly => ResolveAssemblySymbol(UnderlyingType.Assembly);

        /// <inheritdoc />
        public string? AssemblyQualifiedName => UnderlyingType.AssemblyQualifiedName;

        /// <inheritdoc />
        public System.Reflection.TypeAttributes Attributes => (System.Reflection.TypeAttributes)UnderlyingType.Attributes;

        /// <inheritdoc />
        public ITypeSymbol? BaseType => ResolveTypeSymbol(UnderlyingType.BaseType);

        /// <inheritdoc />
        public bool ContainsGenericParameters => UnderlyingType.ContainsGenericParameters;

        /// <inheritdoc />
        public IMethodBaseSymbol? DeclaringMethod => ResolveMethodBaseSymbol(UnderlyingType.DeclaringMethod);

        /// <inheritdoc />
        public string? FullName => UnderlyingType.FullName;

        /// <inheritdoc />
        public string? Namespace => UnderlyingType.Namespace;

        /// <inheritdoc />
        public System.Reflection.GenericParameterAttributes GenericParameterAttributes => (System.Reflection.GenericParameterAttributes)UnderlyingType.GenericParameterAttributes;

        /// <inheritdoc />
        public int GenericParameterPosition => UnderlyingType.GenericParameterPosition;

        /// <inheritdoc />
        public ITypeSymbol[] GenericTypeArguments => ResolveTypeSymbols(UnderlyingType.GenericTypeArguments);

        /// <inheritdoc />
        public bool HasElementType => UnderlyingType.HasElementType;

        /// <inheritdoc />
        public TypeCode TypeCode => Type.GetTypeCode(UnderlyingType);

        /// <inheritdoc />
        public bool IsAbstract => UnderlyingType.IsAbstract;

        /// <inheritdoc />
        public bool IsSZArray => UnderlyingType.IsSZArray;

        /// <inheritdoc />
        public bool IsArray => UnderlyingType.IsArray;

        /// <inheritdoc />
        public bool IsAutoLayout => UnderlyingType.IsAutoLayout;

        /// <inheritdoc />
        public bool IsExplicitLayout => UnderlyingType.IsExplicitLayout;

        /// <inheritdoc />
        public bool IsByRef => UnderlyingType.IsByRef;

        /// <inheritdoc />
        public bool IsClass => UnderlyingType.IsClass;

        /// <inheritdoc />
        public bool IsEnum => UnderlyingType.IsEnum;

        /// <inheritdoc />
        public bool IsInterface => UnderlyingType.IsInterface;

        /// <inheritdoc />
        public bool IsConstructedGenericType => UnderlyingType.IsConstructedGenericType;

        /// <inheritdoc />
        public bool IsGenericParameter => UnderlyingType.IsGenericParameter;

        /// <inheritdoc />
        public bool IsGenericType => UnderlyingType.IsGenericType;

        /// <inheritdoc />
        public bool IsGenericTypeDefinition => UnderlyingType.IsGenericTypeDefinition;

        /// <inheritdoc />
        public bool IsLayoutSequential => UnderlyingType.IsLayoutSequential;

        /// <inheritdoc />
        public bool IsNested => UnderlyingType.IsNested;

        /// <inheritdoc />
        public bool IsNestedAssembly => UnderlyingType.IsNestedAssembly;

        /// <inheritdoc />
        public bool IsNestedFamANDAssem => UnderlyingType.IsNestedFamANDAssem;

        /// <inheritdoc />
        public bool IsNestedFamORAssem => UnderlyingType.IsNestedFamORAssem;

        /// <inheritdoc />
        public bool IsNestedFamily => UnderlyingType.IsNestedFamily;

        /// <inheritdoc />
        public bool IsNestedPrivate => UnderlyingType.IsNestedPrivate;

        /// <inheritdoc />
        public bool IsNestedPublic => UnderlyingType.IsNestedPublic;

        /// <inheritdoc />
        public bool IsNotPublic => UnderlyingType.IsNotPublic;

        /// <inheritdoc />
        public bool IsPointer => UnderlyingType.IsPointer;

        /// <inheritdoc />
        public bool IsFunctionPointer => UnderlyingType.IsFunctionPointer;

        /// <inheritdoc />
        public bool IsUnmanagedFunctionPointer => UnderlyingType.IsUnmanagedFunctionPointer;

        /// <inheritdoc />
        public bool IsPrimitive => UnderlyingType.IsPrimitive;

        /// <inheritdoc />
        public bool IsPublic => UnderlyingType.IsPublic;

        /// <inheritdoc />
        public bool IsSealed => UnderlyingType.IsSealed;

        /// <inheritdoc />
        public bool IsSerializable => UnderlyingType.IsSerializable;

        /// <inheritdoc />
        public bool IsValueType => UnderlyingType.IsValueType;

        /// <inheritdoc />
        public bool IsVisible => UnderlyingType.IsVisible;

        /// <inheritdoc />
        public bool IsSignatureType => throw new NotImplementedException();

        /// <inheritdoc />
        public bool IsSpecialName => UnderlyingType.IsSpecialName;

        /// <inheritdoc />
        public IConstructorSymbol? TypeInitializer => ResolveConstructorSymbol(UnderlyingType.TypeInitializer);

        /// <inheritdoc />
        public int GetArrayRank()
        {
            return UnderlyingType.GetArrayRank();
        }

        /// <inheritdoc />
        public IConstructorSymbol? GetConstructor(System.Reflection.BindingFlags bindingAttr, ITypeSymbol[] types)
        {
            return ResolveConstructorSymbol(UnderlyingType.GetConstructor((BindingFlags)bindingAttr, binder: null, types.Unpack(), modifiers: null));
        }

        /// <inheritdoc />
        public IConstructorSymbol? GetConstructor(ITypeSymbol[] types)
        {
            return ResolveConstructorSymbol(UnderlyingType.GetConstructor(types.Unpack()));
        }

        /// <inheritdoc />
        public IConstructorSymbol[] GetConstructors()
        {
            return ResolveConstructorSymbols(UnderlyingType.GetConstructors());
        }

        /// <inheritdoc />
        public IConstructorSymbol[] GetConstructors(System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveConstructorSymbols(UnderlyingType.GetConstructors((BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public IMemberSymbol[] GetDefaultMembers()
        {
            return ResolveMemberSymbols(UnderlyingType.GetDefaultMembers());
        }

        /// <inheritdoc />
        public ITypeSymbol? GetElementType()
        {
            return ResolveTypeSymbol(UnderlyingType.GetElementType());
        }

        /// <inheritdoc />
        public string? GetEnumName(object value)
        {
            return UnderlyingType.GetEnumName(value);
        }

        /// <inheritdoc />
        public string[] GetEnumNames()
        {
            return UnderlyingType.GetEnumNames();
        }

        /// <inheritdoc />
        public ITypeSymbol GetEnumUnderlyingType()
        {
            return ResolveTypeSymbol(UnderlyingType.GetEnumUnderlyingType());
        }

        /// <inheritdoc />
        public Array GetEnumValues()
        {
            return UnderlyingType.GetEnumValues();
        }

        /// <inheritdoc />
        public IEventSymbol? GetEvent(string name)
        {
            return ResolveEventSymbol(UnderlyingType.GetEvent(name));
        }

        /// <inheritdoc />
        public IEventSymbol? GetEvent(string name, System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveEventSymbol(UnderlyingType.GetEvent(name, (BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public IEventSymbol[] GetEvents()
        {
            return ResolveEventSymbols(UnderlyingType.GetEvents());
        }

        /// <inheritdoc />
        public IEventSymbol[] GetEvents(System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveEventSymbols(UnderlyingType.GetEvents((BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public IFieldSymbol? GetField(string name)
        {
            return ResolveFieldSymbol(UnderlyingType.GetField(name));
        }

        /// <inheritdoc />
        public IFieldSymbol? GetField(string name, System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveFieldSymbol(UnderlyingType.GetField(name, (BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public IFieldSymbol[] GetFields()
        {
            return ResolveFieldSymbols(UnderlyingType.GetFields());
        }

        /// <inheritdoc />
        public IFieldSymbol[] GetFields(System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveFieldSymbols(UnderlyingType.GetFields((BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetGenericArguments()
        {
            return ResolveTypeSymbols(UnderlyingType.GetGenericArguments());
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetGenericParameterConstraints()
        {
            return ResolveTypeSymbols(UnderlyingType.GetGenericParameterConstraints());
        }

        /// <inheritdoc />
        public ITypeSymbol GetGenericTypeDefinition()
        {
            return ResolveTypeSymbol(UnderlyingType.GetGenericTypeDefinition());
        }

        /// <inheritdoc />
        public ITypeSymbol? GetInterface(string name)
        {
            return ResolveTypeSymbol(UnderlyingType.GetInterface(name));
        }

        /// <inheritdoc />
        public ITypeSymbol? GetInterface(string name, bool ignoreCase)
        {
            return ResolveTypeSymbol(UnderlyingType.GetInterface(name, ignoreCase));
        }

        /// <inheritdoc />
        public InterfaceMapping GetInterfaceMap(ITypeSymbol interfaceType)
        {
            return ResolveInterfaceMapping(UnderlyingType.GetInterfaceMap(interfaceType.Unpack()));
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetInterfaces(bool inherit = true)
        {
            if (inherit)
                return ResolveTypeSymbols(UnderlyingType.GetInterfaces());
            else
                throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IMemberSymbol[] GetMember(string name)
        {
            return ResolveMemberSymbols(UnderlyingType.GetMember(name));
        }

        /// <inheritdoc />
        public IMemberSymbol[] GetMember(string name, System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveMemberSymbols(UnderlyingType.GetMember(name, (BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public IMemberSymbol[] GetMember(string name, System.Reflection.MemberTypes type, System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveMemberSymbols(UnderlyingType.GetMember(name, (MemberTypes)type, (BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public IMemberSymbol[] GetMembers(System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveMemberSymbols(UnderlyingType.GetMembers((BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public IMemberSymbol[] GetMembers()
        {
            return ResolveMemberSymbols(UnderlyingType.GetMembers());
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveMethodSymbol(UnderlyingType.GetMethod(name, (BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, ITypeSymbol[] types)
        {
            return ResolveMethodSymbol(UnderlyingType.GetMethod(name, types.Unpack()));
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, System.Reflection.BindingFlags bindingAttr, ITypeSymbol[] types)
        {
            return ResolveMethodSymbol(UnderlyingType.GetMethod(name, (BindingFlags)bindingAttr, null, types.Unpack(), null));
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name)
        {
            return ResolveMethodSymbol(UnderlyingType.GetMethod(name));
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, System.Reflection.BindingFlags bindingAttr, System.Reflection.CallingConventions callConvention, ITypeSymbol[] types, System.Reflection.ParameterModifier[]? modifiers)
        {
            return ResolveMethodSymbol(UnderlyingType.GetMethod(name, (BindingFlags)bindingAttr, null, (CallingConventions)callConvention, types.Unpack(), modifiers?.Unpack()));
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, int genericParameterCount, System.Reflection.BindingFlags bindingAttr, System.Reflection.CallingConventions callConvention, ITypeSymbol[] types, System.Reflection.ParameterModifier[]? modifiers)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, int genericParameterCount, System.Reflection.BindingFlags bindingAttr, ITypeSymbol[] types, System.Reflection.ParameterModifier[]? modifiers)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, System.Reflection.BindingFlags bindingAttr, ITypeSymbol[] types, System.Reflection.ParameterModifier[]? modifiers)
        {
            return ResolveMethodSymbol(UnderlyingType.GetMethod(name, (BindingFlags)bindingAttr, null, types.Unpack(), modifiers?.Unpack()));
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, int genericParameterCount, ITypeSymbol[] types, System.Reflection.ParameterModifier[]? modifiers)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetMethods(System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveMethodSymbols(UnderlyingType.GetMethods((BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetMethods()
        {
            return ResolveMethodSymbols(UnderlyingType.GetMethods());
        }

        /// <inheritdoc />
        public ITypeSymbol? GetNestedType(string name)
        {
            return ResolveTypeSymbol(UnderlyingType.GetNestedType(name));
        }

        /// <inheritdoc />
        public ITypeSymbol? GetNestedType(string name, System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveTypeSymbol(UnderlyingType.GetNestedType(name, (BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetNestedTypes()
        {
            return ResolveTypeSymbols(UnderlyingType.GetNestedTypes());
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetNestedTypes(System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveTypeSymbols(UnderlyingType.GetNestedTypes((BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public IPropertySymbol[] GetProperties()
        {
            return ResolvePropertySymbols(UnderlyingType.GetProperties());
        }

        /// <inheritdoc />
        public IPropertySymbol[] GetProperties(System.Reflection.BindingFlags bindingAttr)
        {
            return ResolvePropertySymbols(UnderlyingType.GetProperties((BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public IPropertySymbol? GetProperty(string name, ITypeSymbol[] types)
        {
            return ResolvePropertySymbol(UnderlyingType.GetProperty(name, types.Unpack()));
        }

        /// <inheritdoc />
        public IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType, ITypeSymbol[] types)
        {
            return ResolvePropertySymbol(UnderlyingType.GetProperty(name, returnType?.Unpack(), types.Unpack()));
        }

        /// <inheritdoc />
        public IPropertySymbol? GetProperty(string name, System.Reflection.BindingFlags bindingAttr)
        {
            return ResolvePropertySymbol(UnderlyingType.GetProperty(name, (BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public IPropertySymbol? GetProperty(string name)
        {
            return ResolvePropertySymbol(UnderlyingType.GetProperty(name));
        }

        /// <inheritdoc />
        public IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType)
        {
            return ResolvePropertySymbol(UnderlyingType.GetProperty(name, returnType?.Unpack()));
        }

        /// <inheritdoc />
        public bool IsAssignableFrom(ITypeSymbol? c)
        {
            return UnderlyingType.IsAssignableFrom(c?.Unpack());
        }

        /// <inheritdoc />
        public bool IsEnumDefined(object value)
        {
            return UnderlyingType.IsEnumDefined(value);
        }

        /// <inheritdoc />
        public bool IsSubclassOf(ITypeSymbol c)
        {
            return UnderlyingType.IsSubclassOf(c.Unpack());
        }

        /// <inheritdoc />
        public ITypeSymbol MakeArrayType()
        {
            return ResolveTypeSymbol(UnderlyingType.MakeArrayType());
        }

        /// <inheritdoc />
        public ITypeSymbol MakeArrayType(int rank)
        {
            return ResolveTypeSymbol(UnderlyingType.MakeArrayType(rank));
        }

        /// <inheritdoc />
        public ITypeSymbol MakeByRefType()
        {
            return ResolveTypeSymbol(UnderlyingType.MakeByRefType());
        }

        /// <inheritdoc />
        public ITypeSymbol MakePointerType()
        {
            return ResolveTypeSymbol(UnderlyingType.MakePointerType());
        }

        /// <inheritdoc />
        public ITypeSymbol MakeGenericType(params ITypeSymbol[] typeArguments)
        {
            return ResolveTypeSymbol(UnderlyingType.MakeGenericType(typeArguments.Unpack()));
        }

        #endregion

        /// <inheritdoc />
        public override void OnComplete()
        {
            
        }

    }

}
