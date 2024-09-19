using System;

using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;

namespace IKVM.CoreLib.Symbols.IkvmReflection.Emit
{

    class IkvmReflectionGenericTypeParameterSymbolBuilder : IkvmReflectionMemberSymbolBuilder, IIkvmReflectionGenericTypeParameterSymbolBuilder
    {

        readonly IIkvmReflectionMethodSymbol? _resolvingMethod;

        GenericTypeParameterBuilder? _builder;
        Type _type;
        IkvmReflectionTypeImpl _impl;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="resolvingModule"></param>
        /// <param name="resolvingType"></param>
        /// <param name="resolvingMethod"></param>
        /// <param name="builder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public IkvmReflectionGenericTypeParameterSymbolBuilder(IkvmReflectionSymbolContext context, IIkvmReflectionModuleSymbol resolvingModule, IIkvmReflectionTypeSymbol? resolvingType, IIkvmReflectionMethodSymbol? resolvingMethod, GenericTypeParameterBuilder builder) :
            base(context, resolvingModule, resolvingType)
        {
            _resolvingMethod = resolvingMethod;
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
            _type = _builder;
            _impl = new IkvmReflectionTypeImpl(this);
        }

        /// <inheritdoc />
        public Type UnderlyingType => _type;

        /// <inheritdoc />
        public override MemberInfo UnderlyingMember => UnderlyingType;

        /// <inheritdoc />
        public GenericTypeParameterBuilder UnderlyingGenericTypeParameterBuilder => _builder ?? throw new InvalidOperationException();

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
        public System.Reflection.TypeAttributes Attributes => (System.Reflection.TypeAttributes)_impl.Attributes;

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
        public System.Reflection.GenericParameterAttributes GenericParameterAttributes => (System.Reflection.GenericParameterAttributes)_impl.GenericParameterAttributes;

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
        public IConstructorSymbol? TypeInitializer => _impl.TypeInitializer;

        /// <inheritdoc />
        public override bool IsComplete => _builder == null;

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
        public IMemberSymbol[] GetMember(string name, System.Reflection.BindingFlags bindingAttr)
        {
            return _impl.GetMember(name, bindingAttr);
        }

        /// <inheritdoc />
        public IMemberSymbol[] GetMember(string name, System.Reflection.MemberTypes type, System.Reflection.BindingFlags bindingAttr)
        {
            return _impl.GetMember(name, type, bindingAttr);
        }

        /// <inheritdoc />
        public IMemberSymbol[] GetMembers()
        {
            return _impl.GetMembers();
        }

        /// <inheritdoc />
        public IMemberSymbol[] GetMembers(System.Reflection.BindingFlags bindingAttr)
        {
            return _impl.GetMembers(bindingAttr);
        }

        public IConstructorSymbol? GetConstructor(ITypeSymbol[] types)
        {
            return _impl.GetConstructor(types);
        }

        /// <inheritdoc />
        public IConstructorSymbol? GetConstructor(System.Reflection.BindingFlags bindingAttr, ITypeSymbol[] types)
        {
            return _impl.GetConstructor(bindingAttr, types);
        }

        /// <inheritdoc />
        public IConstructorSymbol[] GetConstructors()
        {
            return _impl.GetConstructors();
        }

        /// <inheritdoc />
        public IConstructorSymbol[] GetConstructors(System.Reflection.BindingFlags bindingAttr)
        {
            return _impl.GetConstructors(bindingAttr);
        }

        /// <inheritdoc />
        public IFieldSymbol? GetField(string name)
        {
            return _impl.GetField(name);
        }

        /// <inheritdoc />
        public IFieldSymbol? GetField(string name, System.Reflection.BindingFlags bindingAttr)
        {
            return _impl.GetField(name, bindingAttr);
        }

        /// <inheritdoc />
        public IFieldSymbol[] GetFields()
        {
            return _impl.GetFields();
        }

        /// <inheritdoc />
        public IFieldSymbol[] GetFields(System.Reflection.BindingFlags bindingAttr)
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
        public IMethodSymbol? GetMethod(string name, System.Reflection.BindingFlags bindingAttr)
        {
            return _impl.GetMethod(name, bindingAttr);
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, System.Reflection.BindingFlags bindingAttr, ITypeSymbol[] types)
        {
            return _impl.GetMethod(name, bindingAttr, types);
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, System.Reflection.BindingFlags bindingAttr, System.Reflection.CallingConventions callConvention, ITypeSymbol[] types, System.Reflection.ParameterModifier[]? modifiers)
        {
            return _impl.GetMethod(name, bindingAttr, callConvention, types, modifiers);
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, int genericParameterCount, System.Reflection.BindingFlags bindingAttr, System.Reflection.CallingConventions callConvention, ITypeSymbol[] types, System.Reflection.ParameterModifier[]? modifiers)
        {
            return _impl.GetMethod(name, genericParameterCount, bindingAttr, callConvention, types, modifiers);
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, int genericParameterCount, System.Reflection.BindingFlags bindingAttr, ITypeSymbol[] types, System.Reflection.ParameterModifier[]? modifiers)
        {
            return _impl.GetMethod(name, genericParameterCount, bindingAttr, types, modifiers);
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, System.Reflection.BindingFlags bindingAttr, ITypeSymbol[] types, System.Reflection.ParameterModifier[]? modifiers)
        {
            return _impl.GetMethod(name, bindingAttr, types, modifiers);
        }

        /// <inheritdoc />
        public IMethodSymbol? GetMethod(string name, int genericParameterCount, ITypeSymbol[] types, System.Reflection.ParameterModifier[]? modifiers)
        {
            return _impl.GetMethod(name, genericParameterCount, types, modifiers);
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetMethods()
        {
            return _impl.GetMethods();
        }

        /// <inheritdoc />
        public IMethodSymbol[] GetMethods(System.Reflection.BindingFlags bindingAttr)
        {
            return _impl.GetMethods(bindingAttr);
        }

        /// <inheritdoc />
        public IPropertySymbol? GetProperty(string name)
        {
            return _impl.GetProperty(name);
        }

        /// <inheritdoc />
        public IPropertySymbol? GetProperty(string name, System.Reflection.BindingFlags bindingAttr)
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
        public IPropertySymbol[] GetProperties(System.Reflection.BindingFlags bindingAttr)
        {
            return _impl.GetProperties(bindingAttr);
        }

        /// <inheritdoc />
        public IEventSymbol? GetEvent(string name)
        {
            return _impl.GetEvent(name);
        }

        /// <inheritdoc />
        public IEventSymbol? GetEvent(string name, System.Reflection.BindingFlags bindingAttr)
        {
            return _impl.GetEvent(name, bindingAttr);
        }

        /// <inheritdoc />
        public IEventSymbol[] GetEvents()
        {
            return _impl.GetEvents();
        }

        /// <inheritdoc />
        public IEventSymbol[] GetEvents(System.Reflection.BindingFlags bindingAttr)
        {
            return _impl.GetEvents(bindingAttr);
        }

        /// <inheritdoc />
        public ITypeSymbol? GetNestedType(string name)
        {
            return _impl.GetNestedType(name);
        }

        /// <inheritdoc />
        public ITypeSymbol? GetNestedType(string name, System.Reflection.BindingFlags bindingAttr)
        {
            return _impl.GetNestedType(name, bindingAttr);
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetNestedTypes()
        {
            return _impl.GetNestedTypes();
        }

        /// <inheritdoc />
        public ITypeSymbol[] GetNestedTypes(System.Reflection.BindingFlags bindingAttr)
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

        /// <inheritdoc />
        public override void OnComplete()
        {
            throw new NotImplementedException();
        }

    }

}
