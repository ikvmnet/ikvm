using System;
using System.Reflection;
using System.Reflection.Emit;

using IKVM.CoreLib.Reflection;
using IKVM.CoreLib.Symbols.Reflection.Emit;

namespace IKVM.CoreLib.Symbols.Reflection
{

    abstract class ReflectionTypeSymbolBase : ReflectionMemberSymbol, IReflectionTypeSymbol
    {

        protected ReflectionMethodTable _methodTable;
        protected ReflectionFieldTable _fieldTable;
        protected ReflectionPropertyTable _propertyTable;
        protected ReflectionEventTable _eventTable;
        protected ReflectionGenericTypeParameterTable _genericTypeParameterTable;
        protected ReflectionTypeSpecTable _specTable;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        public ReflectionTypeSymbolBase(ReflectionSymbolContext context, IReflectionModuleSymbol resolvingModule) :
            base(context, resolvingModule, null)
        {
            _methodTable = new ReflectionMethodTable(context, resolvingModule, this);
            _fieldTable = new ReflectionFieldTable(context, resolvingModule, this);
            _propertyTable = new ReflectionPropertyTable(context, resolvingModule, this);
            _eventTable = new ReflectionEventTable(context, resolvingModule, this);
            _genericTypeParameterTable = new ReflectionGenericTypeParameterTable(context, resolvingModule, this);
            _specTable = new ReflectionTypeSpecTable(context, resolvingModule, this);
        }

        /// <inheritdoc />
        public abstract Type UnderlyingType { get; }

        /// <inheritdoc />
        public virtual Type UnderlyingEmitType => UnderlyingType;

        /// <inheritdoc />
        public virtual Type UnderlyingDynamicEmitType => UnderlyingEmitType;

        /// <inheritdoc />
        public override MemberInfo UnderlyingMember => UnderlyingType;

        #region IReflectionTypeSymbol

        /// <inheritdoc />
        public IReflectionConstructorSymbol GetOrCreateConstructorSymbol(ConstructorInfo ctor)
        {
            return _methodTable.GetOrCreateConstructorSymbol(ctor);
        }

        /// <inheritdoc />
        public IReflectionMethodBaseSymbol GetOrCreateMethodBaseSymbol(MethodBase method)
        {
            if (method is ConstructorInfo ctor)
                return GetOrCreateConstructorSymbol(ctor);
            else
                return GetOrCreateMethodSymbol((MethodInfo)method);
        }

        /// <inheritdoc />
        public IReflectionMethodSymbol GetOrCreateMethodSymbol(MethodInfo method)
        {
            if (method.IsMethodDefinition())
                return _methodTable.GetOrCreateMethodSymbol(method);
            else
                return ResolveMethodSymbol(method.GetGenericMethodDefinition()).GetOrCreateGenericMethodSymbol(method);
        }

        /// <inheritdoc />
        public IReflectionFieldSymbol GetOrCreateFieldSymbol(FieldInfo field)
        {
            return _fieldTable.GetOrCreateFieldSymbol(field);
        }

        /// <inheritdoc />
        public IReflectionPropertySymbol GetOrCreatePropertySymbol(PropertyInfo property)
        {
            return _propertyTable.GetOrCreatePropertySymbol(property);
        }

        /// <inheritdoc />
        public IReflectionEventSymbol GetOrCreateEventSymbol(EventInfo @event)
        {
            return _eventTable.GetOrCreateEventSymbol(@event);
        }

        /// <inheritdoc />
        public IReflectionTypeSymbol GetOrCreateGenericTypeParameterSymbol(Type genericType)
        {
            return _genericTypeParameterTable.GetOrCreateGenericTypeParameterSymbol(genericType);
        }

        /// <inheritdoc />
        public IReflectionTypeSymbol GetOrCreateSZArrayTypeSymbol()
        {
            return _specTable.GetOrCreateSZArrayTypeSymbol();
        }

        /// <inheritdoc />
        public IReflectionTypeSymbol GetOrCreateArrayTypeSymbol(int rank)
        {
            return _specTable.GetOrCreateArrayTypeSymbol(rank);
        }

        /// <inheritdoc />
        public IReflectionTypeSymbol GetOrCreatePointerTypeSymbol()
        {
            return _specTable.GetOrCreatePointerTypeSymbol();
        }

        /// <inheritdoc />
        public IReflectionTypeSymbol GetOrCreateByRefTypeSymbol()
        {
            return _specTable.GetOrCreateByRefTypeSymbol();
        }

        /// <inheritdoc />
        public IReflectionTypeSymbol GetOrCreateGenericTypeSymbol(IReflectionTypeSymbol[] genericTypeDefinition)
        {
            return _specTable.GetOrCreateGenericTypeSymbol(genericTypeDefinition);
        }

        #endregion

        #region IReflectionTypeSymbolBuilder

        /// <inheritdoc />
        public IReflectionGenericTypeParameterSymbolBuilder GetOrCreateGenericTypeParameterSymbol(GenericTypeParameterBuilder genericTypeParameter)
        {
            return _genericTypeParameterTable.GetOrCreateGenericTypeParameterSymbol(genericTypeParameter);
        }

        #endregion

        #region ITypeSymbol

        /// <inheritdoc />
        public virtual IAssemblySymbol Assembly => ResolveAssemblySymbol(UnderlyingType.Assembly);

        /// <inheritdoc />
        public virtual string? AssemblyQualifiedName => UnderlyingType.AssemblyQualifiedName;

        /// <inheritdoc />
        public virtual TypeAttributes Attributes => UnderlyingType.Attributes;

        /// <inheritdoc />
        public virtual ITypeSymbol? BaseType => ResolveTypeSymbol(UnderlyingType.BaseType);

        /// <inheritdoc />
        public virtual bool ContainsGenericParameters => UnderlyingType.ContainsGenericParameters;

        /// <inheritdoc />
        public virtual IMethodBaseSymbol? DeclaringMethod => ResolveMethodBaseSymbol(UnderlyingType.DeclaringMethod);

        /// <inheritdoc />
        public virtual string? FullName => UnderlyingType.FullName;

        /// <inheritdoc />
        public virtual string? Namespace => UnderlyingType.Namespace;

        /// <inheritdoc />
        public virtual GenericParameterAttributes GenericParameterAttributes => UnderlyingType.GenericParameterAttributes;

        /// <inheritdoc />
        public virtual int GenericParameterPosition => UnderlyingType.GenericParameterPosition;

        /// <inheritdoc />
        public virtual ITypeSymbol[] GenericTypeArguments => ResolveTypeSymbols(UnderlyingType.GenericTypeArguments);

        /// <inheritdoc />
        public virtual bool HasElementType => UnderlyingType.HasElementType;

        /// <inheritdoc />
        public virtual TypeCode TypeCode => Type.GetTypeCode(UnderlyingType);

        /// <inheritdoc />
        public virtual bool IsAbstract => UnderlyingType.IsAbstract;

        /// <inheritdoc />
        public virtual bool IsSZArray => UnderlyingType.IsSZArray();

        /// <inheritdoc />
        public virtual bool IsArray => UnderlyingType.IsArray;

        /// <inheritdoc />
        public virtual bool IsAutoLayout => UnderlyingType.IsAutoLayout;

        /// <inheritdoc />
        public virtual bool IsExplicitLayout => UnderlyingType.IsExplicitLayout;

        /// <inheritdoc />
        public virtual bool IsByRef => UnderlyingType.IsByRef;

        /// <inheritdoc />
        public virtual bool IsClass => UnderlyingType.IsClass;

        /// <inheritdoc />
        public virtual bool IsEnum => UnderlyingType.IsEnum;

        /// <inheritdoc />
        public virtual bool IsInterface => UnderlyingType.IsInterface;

        /// <inheritdoc />
        public virtual bool IsConstructedGenericType => UnderlyingType.IsConstructedGenericType;

        /// <inheritdoc />
        public virtual bool IsGenericParameter => UnderlyingType.IsGenericParameter;

        /// <inheritdoc />
        public virtual bool IsGenericType => UnderlyingType.IsGenericType;

        /// <inheritdoc />
        public virtual bool IsGenericTypeDefinition => UnderlyingType.IsGenericTypeDefinition;

        /// <inheritdoc />
        public virtual bool IsLayoutSequential => UnderlyingType.IsLayoutSequential;

        /// <inheritdoc />
        public virtual bool IsNested => UnderlyingType.IsNested;

        /// <inheritdoc />
        public virtual bool IsNestedAssembly => UnderlyingType.IsNestedAssembly;

        /// <inheritdoc />
        public virtual bool IsNestedFamANDAssem => UnderlyingType.IsNestedFamANDAssem;

        /// <inheritdoc />
        public virtual bool IsNestedFamORAssem => UnderlyingType.IsNestedFamORAssem;

        /// <inheritdoc />
        public virtual bool IsNestedFamily => UnderlyingType.IsNestedFamily;

        /// <inheritdoc />
        public virtual bool IsNestedPrivate => UnderlyingType.IsNestedPrivate;

        /// <inheritdoc />
        public virtual bool IsNestedPublic => UnderlyingType.IsNestedPublic;

        /// <inheritdoc />
        public virtual bool IsNotPublic => UnderlyingType.IsNotPublic;

        /// <inheritdoc />
        public virtual bool IsPointer => UnderlyingType.IsPointer;

#if NET8_0_OR_GREATER

        /// <inheritdoc />
        public virtual bool IsFunctionPointer => UnderlyingType.IsFunctionPointer;

        /// <inheritdoc />
        public virtual bool IsUnmanagedFunctionPointer => UnderlyingType.IsUnmanagedFunctionPointer;

#else

        /// <inheritdoc />
        public virtual  bool IsFunctionPointer => throw new NotImplementedException();

        /// <inheritdoc />
        public virtual  bool IsUnmanagedFunctionPointer => throw new NotImplementedException();

#endif

        /// <inheritdoc />
        public virtual bool IsPrimitive => UnderlyingType.IsPrimitive;

        /// <inheritdoc />
        public virtual bool IsPublic => UnderlyingType.IsPublic;

        /// <inheritdoc />
        public virtual bool IsSealed => UnderlyingType.IsSealed;

        /// <inheritdoc />
        public virtual bool IsSerializable => UnderlyingType.IsSerializable;

        /// <inheritdoc />
        public virtual bool IsValueType => UnderlyingType.IsValueType;

        /// <inheritdoc />
        public virtual bool IsVisible => UnderlyingType.IsVisible;

        /// <inheritdoc />
        public virtual bool IsSignatureType => throw new NotImplementedException();

        /// <inheritdoc />
        public virtual bool IsSpecialName => UnderlyingType.IsSpecialName;

        /// <inheritdoc />
        public virtual IConstructorSymbol? TypeInitializer => ResolveConstructorSymbol(UnderlyingType.TypeInitializer);

        /// <inheritdoc />
        public virtual int GetArrayRank()
        {
            return UnderlyingType.GetArrayRank();
        }

        /// <inheritdoc />
        public virtual IConstructorSymbol? GetConstructor(BindingFlags bindingAttr, ITypeSymbol[] types)
        {
            return ResolveConstructorSymbol(UnderlyingType.GetConstructor(bindingAttr, binder: null, types.Unpack(), modifiers: null));
        }

        /// <inheritdoc />
        public virtual IConstructorSymbol? GetConstructor(ITypeSymbol[] types)
        {
            return ResolveConstructorSymbol(UnderlyingType.GetConstructor(types.Unpack()));
        }

        /// <inheritdoc />
        public virtual IConstructorSymbol[] GetConstructors()
        {
            return ResolveConstructorSymbols(UnderlyingType.GetConstructors());
        }

        /// <inheritdoc />
        public virtual IConstructorSymbol[] GetConstructors(BindingFlags bindingAttr)
        {
            return ResolveConstructorSymbols(UnderlyingType.GetConstructors(bindingAttr));
        }

        /// <inheritdoc />
        public virtual IMemberSymbol[] GetDefaultMembers()
        {
            return ResolveMemberSymbols(UnderlyingType.GetDefaultMembers());
        }

        /// <inheritdoc />
        public virtual ITypeSymbol? GetElementType()
        {
            return ResolveTypeSymbol(UnderlyingType.GetElementType());
        }

        /// <inheritdoc />
        public virtual string? GetEnumName(object value)
        {
            return UnderlyingType.GetEnumName(value);
        }

        /// <inheritdoc />
        public virtual string[] GetEnumNames()
        {
            return UnderlyingType.GetEnumNames();
        }

        /// <inheritdoc />
        public virtual ITypeSymbol GetEnumUnderlyingType()
        {
            return ResolveTypeSymbol(UnderlyingType.GetEnumUnderlyingType());
        }

        /// <inheritdoc />
        public virtual Array GetEnumValues()
        {
            return UnderlyingType.GetEnumValues();
        }

        /// <inheritdoc />
        public virtual IEventSymbol? GetEvent(string name)
        {
            return ResolveEventSymbol(UnderlyingType.GetEvent(name));
        }

        /// <inheritdoc />
        public virtual IEventSymbol? GetEvent(string name, BindingFlags bindingAttr)
        {
            return ResolveEventSymbol(UnderlyingType.GetEvent(name, bindingAttr));
        }

        /// <inheritdoc />
        public virtual IEventSymbol[] GetEvents()
        {
            return ResolveEventSymbols(UnderlyingType.GetEvents());
        }

        /// <inheritdoc />
        public virtual IEventSymbol[] GetEvents(BindingFlags bindingAttr)
        {
            return ResolveEventSymbols(UnderlyingType.GetEvents(bindingAttr));
        }

        /// <inheritdoc />
        public virtual IFieldSymbol? GetField(string name)
        {
            return ResolveFieldSymbol(UnderlyingType.GetField(name));
        }

        /// <inheritdoc />
        public virtual IFieldSymbol? GetField(string name, BindingFlags bindingAttr)
        {
            return ResolveFieldSymbol(UnderlyingType.GetField(name, bindingAttr));
        }

        /// <inheritdoc />
        public virtual IFieldSymbol[] GetFields()
        {
            return ResolveFieldSymbols(UnderlyingType.GetFields());
        }

        /// <inheritdoc />
        public virtual IFieldSymbol[] GetFields(BindingFlags bindingAttr)
        {
            return ResolveFieldSymbols(UnderlyingType.GetFields(bindingAttr));
        }

        /// <inheritdoc />
        public virtual ITypeSymbol[] GetGenericArguments()
        {
            return ResolveTypeSymbols(UnderlyingType.GetGenericArguments());
        }

        /// <inheritdoc />
        public virtual ITypeSymbol[] GetGenericParameterConstraints()
        {
            return ResolveTypeSymbols(UnderlyingType.GetGenericParameterConstraints());
        }

        /// <inheritdoc />
        public virtual ITypeSymbol GetGenericTypeDefinition()
        {
            return ResolveTypeSymbol(UnderlyingType.GetGenericTypeDefinition());
        }

        /// <inheritdoc />
        public virtual ITypeSymbol? GetInterface(string name)
        {
            return ResolveTypeSymbol(UnderlyingType.GetInterface(name));
        }

        /// <inheritdoc />
        public virtual ITypeSymbol? GetInterface(string name, bool ignoreCase)
        {
            return ResolveTypeSymbol(UnderlyingType.GetInterface(name, ignoreCase));
        }

        /// <inheritdoc />
        public virtual InterfaceMapping GetInterfaceMap(ITypeSymbol interfaceType)
        {
            return ResolveInterfaceMapping(UnderlyingType.GetInterfaceMap(interfaceType.Unpack()));
        }

        /// <inheritdoc />
        public virtual ITypeSymbol[] GetInterfaces(bool inherit = true)
        {
            if (inherit)
                return ResolveTypeSymbols(UnderlyingType.GetInterfaces());
            else
                throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual IMemberSymbol[] GetMember(string name)
        {
            return ResolveMemberSymbols(UnderlyingType.GetMember(name));
        }

        /// <inheritdoc />
        public virtual IMemberSymbol[] GetMember(string name, BindingFlags bindingAttr)
        {
            return ResolveMemberSymbols(UnderlyingType.GetMember(name, bindingAttr));
        }

        /// <inheritdoc />
        public virtual IMemberSymbol[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
        {
            return ResolveMemberSymbols(UnderlyingType.GetMember(name, type, bindingAttr));
        }

        /// <inheritdoc />
        public virtual IMemberSymbol[] GetMembers(BindingFlags bindingAttr)
        {
            return ResolveMemberSymbols(UnderlyingType.GetMembers(bindingAttr));
        }

        /// <inheritdoc />
        public virtual IMemberSymbol[] GetMembers()
        {
            return ResolveMemberSymbols(UnderlyingType.GetMembers());
        }

        /// <inheritdoc />
        public virtual IMethodSymbol? GetMethod(string name)
        {
            return ResolveMethodSymbol(UnderlyingType.GetMethod(name));
        }

        /// <inheritdoc />
        public virtual IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr)
        {
            return ResolveMethodSymbol(UnderlyingType.GetMethod(name, bindingAttr));
        }

        /// <inheritdoc />
        public virtual IMethodSymbol? GetMethod(string name, ITypeSymbol[] types)
        {
            return ResolveMethodSymbol(UnderlyingType.GetMethod(name, types.Unpack()));
        }

        /// <inheritdoc />
        public virtual IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr, ITypeSymbol[] types)
        {
            return ResolveMethodSymbol(UnderlyingType.GetMethod(name, bindingAttr, null, types.Unpack(), null));
        }

        /// <inheritdoc />
        public virtual IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr, ITypeSymbol[] types, ParameterModifier[]? modifiers)
        {
            return ResolveMethodSymbol(UnderlyingType.GetMethod(name, bindingAttr, null, types.Unpack(), modifiers?.Unpack()));
        }

        /// <inheritdoc />
        public virtual IMethodSymbol? GetMethod(string name, int genericParameterCount, BindingFlags bindingAttr, ITypeSymbol[] types, ParameterModifier[]? modifiers)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr, CallingConventions callConvention, ITypeSymbol[] types, ParameterModifier[]? modifiers)
        {
            return ResolveMethodSymbol(UnderlyingType.GetMethod(name, bindingAttr, null, callConvention, types.Unpack(), modifiers?.Unpack()));
        }

        /// <inheritdoc />
        public virtual IMethodSymbol? GetMethod(string name, int genericParameterCount, ITypeSymbol[] types, ParameterModifier[]? modifiers)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual IMethodSymbol? GetMethod(string name, int genericParameterCount, BindingFlags bindingAttr, CallingConventions callConvention, ITypeSymbol[] types, ParameterModifier[]? modifiers)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual IMethodSymbol[] GetMethods(System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveMethodSymbols(UnderlyingType.GetMethods((BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public virtual IMethodSymbol[] GetMethods()
        {
            return ResolveMethodSymbols(UnderlyingType.GetMethods());
        }

        /// <inheritdoc />
        public virtual ITypeSymbol? GetNestedType(string name)
        {
            return ResolveTypeSymbol(UnderlyingType.GetNestedType(name));
        }

        /// <inheritdoc />
        public virtual ITypeSymbol? GetNestedType(string name, System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveTypeSymbol(UnderlyingType.GetNestedType(name, (BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public virtual ITypeSymbol[] GetNestedTypes()
        {
            return ResolveTypeSymbols(UnderlyingType.GetNestedTypes());
        }

        /// <inheritdoc />
        public virtual ITypeSymbol[] GetNestedTypes(System.Reflection.BindingFlags bindingAttr)
        {
            return ResolveTypeSymbols(UnderlyingType.GetNestedTypes((BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public virtual IPropertySymbol[] GetProperties()
        {
            return ResolvePropertySymbols(UnderlyingType.GetProperties());
        }

        /// <inheritdoc />
        public virtual IPropertySymbol[] GetProperties(System.Reflection.BindingFlags bindingAttr)
        {
            return ResolvePropertySymbols(UnderlyingType.GetProperties((BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public virtual IPropertySymbol? GetProperty(string name, ITypeSymbol[] types)
        {
            return ResolvePropertySymbol(UnderlyingType.GetProperty(name, types.Unpack()));
        }

        /// <inheritdoc />
        public virtual IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType, ITypeSymbol[] types)
        {
            return ResolvePropertySymbol(UnderlyingType.GetProperty(name, returnType?.Unpack(), types.Unpack()));
        }

        /// <inheritdoc />
        public virtual IPropertySymbol? GetProperty(string name, System.Reflection.BindingFlags bindingAttr)
        {
            return ResolvePropertySymbol(UnderlyingType.GetProperty(name, (BindingFlags)bindingAttr));
        }

        /// <inheritdoc />
        public virtual IPropertySymbol? GetProperty(string name)
        {
            return ResolvePropertySymbol(UnderlyingType.GetProperty(name));
        }

        /// <inheritdoc />
        public virtual IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType)
        {
            return ResolvePropertySymbol(UnderlyingType.GetProperty(name, returnType?.Unpack()));
        }

        /// <inheritdoc />
        public virtual bool IsAssignableFrom(ITypeSymbol? c)
        {
            return UnderlyingType.IsAssignableFrom(c?.Unpack());
        }

        /// <inheritdoc />
        public virtual bool IsEnumDefined(object value)
        {
            return UnderlyingType.IsEnumDefined(value);
        }

        /// <inheritdoc />
        public virtual bool IsSubclassOf(ITypeSymbol c)
        {
            return UnderlyingType.IsSubclassOf(c.Unpack());
        }

        /// <inheritdoc />
        public virtual ITypeSymbol MakeArrayType()
        {
            return ResolveTypeSymbol(UnderlyingType.MakeArrayType());
        }

        /// <inheritdoc />
        public virtual ITypeSymbol MakeArrayType(int rank)
        {
            return ResolveTypeSymbol(UnderlyingType.MakeArrayType(rank));
        }

        /// <inheritdoc />
        public virtual ITypeSymbol MakeByRefType()
        {
            return ResolveTypeSymbol(UnderlyingType.MakeByRefType());
        }

        /// <inheritdoc />
        public virtual ITypeSymbol MakePointerType()
        {
            return ResolveTypeSymbol(UnderlyingType.MakePointerType());
        }

        /// <inheritdoc />
        public virtual ITypeSymbol MakeGenericType(params ITypeSymbol[] typeArguments)
        {
            return ResolveTypeSymbol(UnderlyingType.MakeGenericType(typeArguments.Unpack()));
        }

        #endregion

    }

}
