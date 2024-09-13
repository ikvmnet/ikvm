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

		ITypeSymbol[] GenericTypeArguments { get; }

		bool HasElementType { get; }

		bool IsAbstract { get; }

		bool IsArray { get; }

		bool IsAutoLayout { get; }

		bool IsByRef { get; }

		bool IsClass { get; }

		bool IsConstructedGenericType { get; }

		bool IsEnum { get; }

		bool IsExplicitLayout { get; }

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

		bool IsSerializable { get; }

		bool IsValueType { get; }

		bool IsVisible { get; }

		string? Namespace { get; }

		IConstructorSymbol? TypeInitializer { get; }

		int GetArrayRank();

		IConstructorSymbol? GetConstructor(BindingFlags bindingAttr, ITypeSymbol[] types);

		IConstructorSymbol? GetConstructor(ITypeSymbol[] types);

		IConstructorSymbol[] GetConstructors();

		IMemberSymbol[] GetDefaultMembers();

		ITypeSymbol? GetElementType();

		string? GetEnumName(object value);

		string[] GetEnumNames();

		ITypeSymbol GetEnumUnderlyingType();

		IEventSymbol? GetEvent(string name, BindingFlags bindingAttr);

		IEventSymbol? GetEvent(string name);

		IEventSymbol[] GetEvents();

		IEventSymbol[] GetEvents(BindingFlags bindingAttr);

		IFieldSymbol? GetField(string name);

		IFieldSymbol? GetField(string name, BindingFlags bindingAttr);

		IFieldSymbol[] GetFields();

		IFieldSymbol[] GetFields(BindingFlags bindingAttr);

		ITypeSymbol[] GetGenericArguments();

		ITypeSymbol[] GetGenericParameterConstraints();

		ITypeSymbol GetGenericTypeDefinition();

		ITypeSymbol? GetInterface(string name);

		ITypeSymbol? GetInterface(string name, bool ignoreCase);

		InterfaceMapping GetInterfaceMap(ITypeSymbol interfaceType);

		ITypeSymbol[] GetInterfaces();

		IMemberSymbol[] GetMember(string name);

		IMemberSymbol[] GetMember(string name, BindingFlags bindingAttr);

		IMemberSymbol[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr);

		IMemberSymbol[] GetMembers(BindingFlags bindingAttr);

		IMemberSymbol[] GetMembers();
		IMethodSymbol? GetMethod(string name);

		IMethodSymbol? GetMethod(string name, ITypeSymbol[] types);

		IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr);

		IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr, ITypeSymbol[] types);

		IMethodSymbol[] GetMethods();

		IMethodSymbol[] GetMethods(BindingFlags bindingAttr);

		ITypeSymbol? GetNestedType(string name);

		ITypeSymbol? GetNestedType(string name, BindingFlags bindingAttr);

		ITypeSymbol[] GetNestedTypes();

		ITypeSymbol[] GetNestedTypes(BindingFlags bindingAttr);

		IPropertySymbol[] GetProperties();

		IPropertySymbol[] GetProperties(BindingFlags bindingAttr);

		IPropertySymbol? GetProperty(string name, ITypeSymbol[] types);

		IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType, ITypeSymbol[] types);

		IPropertySymbol? GetProperty(string name, BindingFlags bindingAttr);

		IPropertySymbol? GetProperty(string name);

		IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType);

		bool IsAssignableFrom(ITypeSymbol? c);

		bool IsEnumDefined(object value);

		bool IsSubclassOf(ITypeSymbol c);

		ITypeSymbol MakeArrayType();

		ITypeSymbol MakeArrayType(int rank);

		ITypeSymbol MakeByRefType();

		ITypeSymbol MakeGenericType(params ITypeSymbol[] typeArguments);

		ITypeSymbol MakePointerType();

	}

}
