using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

	interface ITypeSymbol : IMemberSymbol
	{

		IAssemblySymbol Assembly { get; }

		string? AssemblyQualifiedName { get; }

		TypeAttributes Attributes { get; }

		ITypeSymbol? BaseType { get; }

		bool ContainsGenericParameters { get; }

		IMethodBaseSymbol? DeclaringMethod { get; }

		string? FullName { get; }

		GenericParameterAttributes GenericParameterAttributes { get; }

		int GenericParameterPosition { get; }

		ImmutableArray<ITypeSymbol> GenericTypeArguments { get; }

		bool HasElementType { get; }

		bool IsAbstract { get; }

		bool IsArray { get; }

		bool IsAutoLayout { get; }

		bool IsByRef { get; }

		bool IsByRefLike { get; }

		bool IsClass { get; }

		bool IsConstructedGenericType { get; }

		bool IsEnum { get; }

		bool IsExplicitLayout { get; }

		bool IsFunctionPointer { get; }

		bool IsGenericMethodParameter { get; }

		bool IsGenericParameter { get; }

		bool IsGenericType { get; }

		bool IsInterface { get; }

		bool IsLayoutSequential { get; }

		bool IsNested { get; }

		bool IsNestedAssembly { get; }

		bool IsNestedFamANDAssem { get; }

		bool IsNestedFamily { get; }

		bool IsNestedPrivate { get; }

		bool IsNestedPublic { get; }

		bool IsNotPublic { get; }

		bool IsPointer { get; }

		bool IsPrimitive { get; }

		bool IsPublic { get; }

		bool IsSealed { get; }

		bool IsSecurityCritical { get; }

		bool IsSecuritySafeCritical { get; }

		bool IsSecurityTransparent { get; }

		bool IsSerializable { get; }

		bool IsSignatureType { get; }

		bool IsSZArray { get; }

		bool IsTypeDefinition { get; }

		bool IsUnmanagedFunctionPointer { get; }

		bool IsValueType { get; }

		bool IsVariableBoundArray { get; }

		bool IsVisible { get; }

		string? Namespace { get; }

		IConstructorSymbol? TypeInitializer { get; }

		int GetArrayRank();

		System.Reflection.TypeAttributes GetAttributeFlagsImpl();

		IConstructorSymbol? GetConstructor(BindingFlags bindingAttr, ITypeSymbol[] types);

		IConstructorSymbol? GetConstructor(ITypeSymbol[] types);

		IConstructorSymbol[] GetConstructors();

		IMemberSymbol[] GetDefaultMembers();

		ITypeSymbol? GetElementType();

		string? GetEnumName(object value);

		string[] GetEnumNames();

		ITypeSymbol GetEnumUnderlyingType();

		Array GetEnumValues();

		Array GetEnumValuesAsUnderlyingType();

		IEventSymbol? GetEvent(string name, BindingFlags bindingAttr);

		IEventSymbol? GetEvent(string name);

		IEventSymbol[] GetEvents();

		IEventSymbol[] GetEvents(BindingFlags bindingAttr);

		IFieldSymbol? GetField(string name);

		IFieldSymbol? GetField(string name, BindingFlags bindingAttr);

		IFieldSymbol[] GetFields();

		IFieldSymbol[] GetFields(BindingFlags bindingAttr);

		ITypeSymbol[] GetFunctionPointerCallingConventions();

		ITypeSymbol[] GetFunctionPointerParameterTypes();

		ITypeSymbol GetFunctionPointerReturnType();

		ITypeSymbol[] GetGenericArguments();

		ITypeSymbol[] GetGenericParameterConstraints();

		ITypeSymbol GetGenericTypeDefinition();

		ITypeSymbol? GetInterface(string name);

		ITypeSymbol? GetInterface(string name, bool ignoreCase);

		InterfaceMapping GetInterfaceMap(ITypeSymbol interfaceType);

		ITypeSymbol[] GetInterfaces();

		IMemberSymbol[] GetMember(string name);

		IMemberSymbol[] GetMember(string name, BindingFlags bindingAttr);

		IMemberSymbol[] GetMember(string name, System.Reflection.MemberTypes type, BindingFlags bindingAttr);

		IMemberSymbol[] GetMembers(BindingFlags bindingAttr);

		IMemberSymbol[] GetMembers();

		IMethodSymbol? GetMethod(string name, int genericParameterCount, ITypeSymbol[] types, ParameterModifier[]? modifiers);

		IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr);

		IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr, ITypeSymbol[] types);

		IMethodSymbol? GetMethod(string name, int genericParameterCount, ITypeSymbol[] types);

		IMethodSymbol? GetMethod(string name, ITypeSymbol[] types);

		IMethodSymbol? GetMethod(string name);

		IMethodSymbol? GetMethod(string name, ITypeSymbol[] types, ParameterModifier[]? modifiers);

		IMethodSymbol[] GetMethods(BindingFlags bindingAttr);

		IMethodSymbol[] GetMethods();

		ITypeSymbol? GetNestedType(string name);

		ITypeSymbol? GetNestedType(string name, BindingFlags bindingAttr);

		ITypeSymbol[] GetNestedTypes();

		ITypeSymbol[] GetNestedTypes(BindingFlags bindingAttr);

		ITypeSymbol[] GetOptionalCustomModifiers();

		IPropertySymbol[] GetProperties();

		IPropertySymbol[] GetProperties(BindingFlags bindingAttr);

		IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType, ITypeSymbol[] types, ParameterModifier[]? modifiers);

		IPropertySymbol? GetProperty(string name, ITypeSymbol[] types);

		IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType, ITypeSymbol[] types);

		IPropertySymbol? GetProperty(string name, BindingFlags bindingAttr);

		IPropertySymbol? GetProperty(string name);

		IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType);

		ITypeSymbol[] GetRequiredCustomModifiers();

		bool IsAssignableFrom(ITypeSymbol? c);

		bool IsAssignableTo(ITypeSymbol? targetType);

		bool IsEnumDefined(object value);

		bool IsSubclassOf(ITypeSymbol c);

		ITypeSymbol MakeArrayType();

		ITypeSymbol MakeArrayType(int rank);

		ITypeSymbol MakeByRefType();

		ITypeSymbol MakeGenericType(params ITypeSymbol[] typeArguments);

		ITypeSymbol MakePointerType();

	}

}
