using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace IKVM.CoreLib.Symbols.Reflection
{

    class ReflectionTypeSymbol : ReflectionMemberSymbol, IReflectionTypeSymbol
    {

        readonly Type _type;
        ReflectionTypeImpl _impl;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="type"></param>
        public ReflectionTypeSymbol(ReflectionSymbolContext context, IReflectionModuleSymbol resolvingModule, Type type) :
            base(context, resolvingModule, null, type)
        {
            _type = type ?? throw new ArgumentNullException(nameof(type));
            _impl = new ReflectionTypeImpl(this);
        }

        /// <inheritdoc />
        public Type UnderlyingType => _type;

        /// <summary>
        /// Resolves the symbol for the specified type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [return: NotNullIfNotNull(nameof(type))]
        public override IReflectionTypeSymbol? ResolveTypeSymbol(Type? type)
        {
            if (type == _type)
                return this;
            else
                return base.ResolveTypeSymbol(type);
        }

        #region ITypeSymbol

        /// <inheritdoc />
        public TypeAttributes Attributes => _impl.Attributes;

        /// <inheritdoc />
        public IAssemblySymbol Assembly => _impl.Assembly;

        /// <inheritdoc />
        public IMethodBaseSymbol? DeclaringMethod => _impl.DeclaringMethod;

        /// <inheritdoc />
        public string? AssemblyQualifiedName => _impl.AssemblyQualifiedName;

        /// <inheritdoc />
        public string? FullName => _impl.FullName;

        /// <inheritdoc />
        public string? Namespace => _impl.Namespace;

        /// <inheritdoc />
        public TypeCode TypeCode => _impl.TypeCode;

        /// <inheritdoc />
        public ITypeSymbol? BaseType => _impl.BaseType;

        /// <inheritdoc />
        public bool ContainsGenericParameters => _impl.ContainsGenericParameters;

        /// <inheritdoc />
        public GenericParameterAttributes GenericParameterAttributes => _impl.GenericParameterAttributes;

        /// <inheritdoc />
        public int GenericParameterPosition => _impl.GenericParameterPosition;

        /// <inheritdoc />
        public ITypeSymbol[] GenericTypeArguments => _impl.GenericTypeArguments;

        /// <inheritdoc />
        public bool IsConstructedGenericType => _impl.IsConstructedGenericType;

        /// <inheritdoc />
        public bool IsGenericType => _impl.IsGenericType;

        /// <inheritdoc />
        public bool IsGenericTypeDefinition => _impl.IsGenericTypeDefinition;

        /// <inheritdoc />
        public bool IsGenericParameter => _impl.IsGenericParameter;

        /// <inheritdoc />
        public bool IsAutoLayout => _impl.IsAutoLayout;

        /// <inheritdoc />
        public bool IsExplicitLayout => _impl.IsExplicitLayout;

        /// <inheritdoc />
        public bool IsLayoutSequential => _impl.IsLayoutSequential;

        /// <inheritdoc />
        public bool HasElementType => _impl.HasElementType;

        /// <inheritdoc />
        public bool IsClass => _impl.IsClass;

        /// <inheritdoc />
        public bool IsValueType => _impl.IsValueType;

        /// <inheritdoc />
        public bool IsInterface => _impl.IsInterface;

        /// <inheritdoc />
        public bool IsPrimitive => _impl.IsPrimitive;

        /// <inheritdoc />
        public bool IsSZArray => _impl.IsSZArray;

        /// <inheritdoc />
        public bool IsArray => _impl.IsArray;

        /// <inheritdoc />
        public bool IsEnum => _impl.IsEnum;

        /// <inheritdoc />
        public bool IsPointer => _impl.IsPointer;

        /// <inheritdoc />
        public bool IsFunctionPointer => _impl.IsFunctionPointer;

        /// <inheritdoc />
        public bool IsUnmanagedFunctionPointer => _impl.IsUnmanagedFunctionPointer;

        /// <inheritdoc />
        public bool IsByRef => _impl.IsByRef;

        /// <inheritdoc />
        public bool IsAbstract => _impl.IsAbstract;

        /// <inheritdoc />
        public bool IsSealed => _impl.IsSealed;

        /// <inheritdoc />
        public bool IsVisible => _impl.IsVisible;

        /// <inheritdoc />
        public bool IsPublic => _impl.IsPublic;

        /// <inheritdoc />
        public bool IsNotPublic => _impl.IsNotPublic;

        /// <inheritdoc />
        public bool IsNested => _impl.IsNested;

        /// <inheritdoc />
        public bool IsNestedAssembly => _impl.IsNestedAssembly;

        /// <inheritdoc />
        public bool IsNestedFamANDAssem => _impl.IsNestedFamANDAssem;

        /// <inheritdoc />
        public bool IsNestedFamily => _impl.IsNestedFamily;

        /// <inheritdoc />
        public bool IsNestedFamORAssem => _impl.IsNestedFamORAssem;

        /// <inheritdoc />
        public bool IsNestedPrivate => _impl.IsNestedPrivate;

        /// <inheritdoc />
        public bool IsNestedPublic => _impl.IsNestedPublic;

        /// <inheritdoc />
        public bool IsSerializable => _impl.IsSerializable;

        /// <inheritdoc />
        public bool IsSignatureType => _impl.IsSignatureType;

        /// <inheritdoc />
        public bool IsSpecialName => _impl.IsSpecialName;

        /// <inheritdoc />
        public IConstructorSymbol? TypeInitializer => throw new NotImplementedException();

        /// <inheritdoc />
        public int GetArrayRank()
        {
            return _impl.GetArrayRank();
        }

        /// <inheritdoc />
        public IMemberSymbol[] GetDefaultMembers()
        {
            return _impl.GetDefaultMembers();
        }

        /// <inheritdoc />
        public ITypeSymbol? GetElementType()
        {
            return _impl.GetElementType();
        }

        /// <inheritdoc />
        public string? GetEnumName(object value)
        {
            return _impl.GetEnumName(value);
        }

        /// <inheritdoc />
        public string[] GetEnumNames()
        {
            return _impl.GetEnumNames();
        }

        /// <inheritdoc />
        public ITypeSymbol GetEnumUnderlyingType()
        {
            return _impl.GetEnumUnderlyingType();
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetGenericArguments()
        {
            return _impl.GetGenericArguments();
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetGenericParameterConstraints()
        {
            return _impl.GetGenericParameterConstraints();
        }

        /// <inheritdoc />
        public ITypeSymbol GetGenericTypeDefinition()
        {
            return _impl.GetGenericTypeDefinition();
        }

        /// <inheritdoc />
        public ITypeSymbol? GetInterface(string name)
        {
            return _impl.GetInterface(name);
        }

        /// <inheritdoc />
        public ITypeSymbol? GetInterface(string name, bool ignoreCase)
        {
            return _impl.GetInterface(name, ignoreCase);
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetInterfaces(bool inherit = true)
        {
            return _impl.GetInterfaces(inherit);
        }

        /// <inheritdoc />
        public InterfaceMapping GetInterfaceMap(ITypeSymbol interfaceType)
        {
            return _impl.GetInterfaceMap(interfaceType);
        }

        /// <inheritdoc />
        public IMemberSymbol[] GetMember(string name)
        {
            return _impl.GetMember(name);
        }

        /// <inheritdoc />
        public IMemberSymbol[] GetMember(string name, BindingFlags bindingAttr)
        {
            return _impl.GetMember(name, bindingAttr);
        }

        /// <inheritdoc />
        public IMemberSymbol[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
        {
            return _impl.GetMember(name, type, bindingAttr);
        }

        /// <inheritdoc />
        public IMemberSymbol[] GetMembers()
        {
            return _impl.GetMembers();
        }

        /// <inheritdoc />
        public IMemberSymbol[] GetMembers(BindingFlags bindingAttr)
        {
            return _impl.GetMembers(bindingAttr);
        }

        public IConstructorSymbol? GetConstructor(ITypeSymbol[] types)
        {
            return _impl.GetConstructor(types);
        }

        /// <inheritdoc />
        public IConstructorSymbol? GetConstructor(BindingFlags bindingAttr, ITypeSymbol[] types)
        {
            return _impl.GetConstructor(bindingAttr, types);
        }

        /// <inheritdoc />
        public IConstructorSymbol[] GetConstructors()
        {
            return _impl.GetConstructors();
        }

        /// <inheritdoc />
        public IConstructorSymbol[] GetConstructors(BindingFlags bindingAttr)
        {
            return _impl.GetConstructors(bindingAttr);
        }

        /// <inheritdoc />
        public IFieldSymbol? GetField(string name)
        {
            return _impl.GetField(name);
        }

        /// <inheritdoc />
        public IFieldSymbol? GetField(string name, BindingFlags bindingAttr)
        {
            return _impl.GetField(name, bindingAttr);
        }

        /// <inheritdoc />
        public IFieldSymbol[] GetFields()
        {
            return _impl.GetFields();
        }

        /// <inheritdoc />
        public IFieldSymbol[] GetFields(BindingFlags bindingAttr)
        {
            return _impl.GetFields(bindingAttr);
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name)
        {
            return _impl.GetMethod(name);
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, ITypeSymbol[] types)
        {
            return _impl.GetMethod(name, types);
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr)
        {
            return _impl.GetMethod(name, bindingAttr);
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr, ITypeSymbol[] types)
        {
            return _impl.GetMethod(name, bindingAttr, types);
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr, CallingConventions callConvention, ITypeSymbol[] types, ParameterModifier[]? modifiers)
        {
            return _impl.GetMethod(name, bindingAttr, callConvention, types, modifiers);
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, int genericParameterCount, BindingFlags bindingAttr, CallingConventions callConvention, ITypeSymbol[] types, ParameterModifier[]? modifiers)
        {
            return _impl.GetMethod(name, genericParameterCount, bindingAttr, callConvention, types, modifiers);
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, int genericParameterCount, BindingFlags bindingAttr, ITypeSymbol[] types, ParameterModifier[]? modifiers)
        {
            return _impl.GetMethod(name, genericParameterCount, bindingAttr, types, modifiers);
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr, ITypeSymbol[] types, ParameterModifier[]? modifiers)
        {
            return _impl.GetMethod(name, bindingAttr, types, modifiers);
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, int genericParameterCount, ITypeSymbol[] types, ParameterModifier[]? modifiers)
        {
            return _impl.GetMethod(name, genericParameterCount, types, modifiers);
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetMethods()
        {
            return _impl.GetMethods();
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetMethods(BindingFlags bindingAttr)
        {
            return _impl.GetMethods(bindingAttr);
        }

        /// <inheritdoc />
        public IPropertySymbol? GetProperty(string name)
        {
            return _impl.GetProperty(name);
        }

        /// <inheritdoc />
        public IPropertySymbol? GetProperty(string name, BindingFlags bindingAttr)
        {
            return _impl.GetProperty(name, bindingAttr);
        }

        /// <inheritdoc />
        public IPropertySymbol? GetProperty(string name, ITypeSymbol[] types)
        {
            return _impl.GetProperty(name, types);
        }

        /// <inheritdoc />
        public IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType, ITypeSymbol[] types)
        {
            return _impl.GetProperty(name, returnType, types);
        }

        /// <inheritdoc />
        public IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType)
        {
            return _impl.GetProperty(name, returnType);
        }

        /// <inheritdoc />
        public IPropertySymbol[] GetProperties()
        {
            return _impl.GetProperties();
        }

        /// <inheritdoc />
        public IPropertySymbol[] GetProperties(BindingFlags bindingAttr)
        {
            return _impl.GetProperties(bindingAttr);
        }

        /// <inheritdoc />
        public IEventSymbol? GetEvent(string name)
        {
            return _impl.GetEvent(name);
        }

        /// <inheritdoc />
        public IEventSymbol? GetEvent(string name, BindingFlags bindingAttr)
        {
            return _impl.GetEvent(name, bindingAttr);
        }

        /// <inheritdoc />
        public IEventSymbol[] GetEvents()
        {
            return _impl.GetEvents();
        }

        /// <inheritdoc />
        public IEventSymbol[] GetEvents(BindingFlags bindingAttr)
        {
            return _impl.GetEvents(bindingAttr);
        }

        /// <inheritdoc />
        public ITypeSymbol? GetNestedType(string name)
        {
            return _impl.GetNestedType(name);
        }

        /// <inheritdoc />
        public ITypeSymbol? GetNestedType(string name, BindingFlags bindingAttr)
        {
            return _impl.GetNestedType(name, bindingAttr);
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetNestedTypes()
        {
            return _impl.GetNestedTypes();
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetNestedTypes(BindingFlags bindingAttr)
        {
            return _impl.GetNestedTypes();
        }

        /// <inheritdoc />
        public bool IsAssignableFrom(ITypeSymbol? c)
        {
            return _impl.IsAssignableFrom(c);
        }

        /// <inheritdoc />
        public bool IsSubclassOf(ITypeSymbol c)
        {
            return _impl.IsSubclassOf(c);
        }

        /// <inheritdoc />
        public bool IsEnumDefined(object value)
        {
            return _impl.IsEnumDefined(value);
        }

        /// <inheritdoc />
        public ITypeSymbol MakeArrayType()
        {
            return _impl.MakeArrayType();
        }

        /// <inheritdoc />
        public ITypeSymbol MakeArrayType(int rank)
        {
            return _impl.MakeArrayType(rank);
        }

        /// <inheritdoc />
        public ITypeSymbol MakeGenericType(params ITypeSymbol[] typeArguments)
        {
            return _impl.MakeGenericType(typeArguments);
        }

        /// <inheritdoc />
        public ITypeSymbol MakePointerType()
        {
            return _impl.MakePointerType();
        }

        /// <inheritdoc />
        public ITypeSymbol MakeByRefType()
        {
            return _impl.MakeByRefType();
        }

        #endregion

    }

}
