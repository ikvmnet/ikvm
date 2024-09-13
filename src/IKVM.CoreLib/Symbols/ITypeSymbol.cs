using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

	/// <summary>
	/// Represents type declarations: class types, interface types, array types, value types, enumeration types, type parameters, generic type definitions, and open or closed constructed generic types.
	/// </summary>
	interface ITypeSymbol : IMemberSymbol
	{

		/// <summary>
		/// Gets the attributes associated with the <see cref="ITypeSymbol">.
		/// </summary>
		TypeAttributes Attributes { get; }

		/// <summary>
		/// Gets the <see cref="IAssemblySymbol" /> in which the type is declared. For generic types, gets the <see cref="IAssemblySymbol" /> in which the generic type is defined.
		/// </summary>
		IAssemblySymbol Assembly { get; }

		/// <summary>
		/// Gets a <see cref="IMethodBaseSymbol"/> that represents the declaring method, if the current <see cref="ITypeSymbol"/> represents a type parameter of a generic method.
		/// </summary>
		IMethodBaseSymbol? DeclaringMethod { get; }

		/// <summary>
		/// Gets the assembly-qualified name of the type, which includes the name of the assembly from which this <see cref="ITypeSymbol"/> object was loaded.
		/// </summary>
		string? AssemblyQualifiedName { get; }

		/// <summary>
		/// Gets the fully qualified name of the type, including its namespace but not its assembly.
		/// </summary>
		string? FullName { get; }

		/// <summary>
		/// Gets the namespace of the Type.
		/// </summary>
		string? Namespace { get; }

		/// <summary>
		/// Gets the type from which the current <see cref="ITypeSymbol"/> directly inherits.
		/// </summary>
		ITypeSymbol? BaseType { get; }

		/// <summary>
		/// Gets a value indicating whether the current <see cref="ITypeSymbol"/> object has type parameters that have not been replaced by specific types.
		/// </summary>
		bool ContainsGenericParameters { get; }

		/// <summary>
		/// Gets a combination of <see cref="GenericParameterAttributes"/> flags that describe the covariance and special constraints of the current generic type parameter.
		/// </summary>
		GenericParameterAttributes GenericParameterAttributes { get; }

		/// <summary>
		/// Gets the position of the type parameter in the type parameter list of the generic type or method that declared the parameter, when the <see cref="ITypeSymbol"/> object represents a type parameter of a generic type or a generic method.
		/// </summary>
		int GenericParameterPosition { get; }

		/// <summary>
		/// Gets an array of the generic type arguments for this type.
		/// </summary>
		ITypeSymbol[] GenericTypeArguments { get; }

		/// <summary>
		/// Gets a value that indicates whether this object represents a constructed generic type. You can create instances of a constructed generic type.
		/// </summary>
		bool IsConstructedGenericType { get; }

		/// <summary>
		/// Gets a value indicating whether the current Type represents a type parameter in the definition of a generic type or method.
		/// </summary>
		bool IsGenericParameter { get; }

		/// <summary>
		/// Gets a value indicating whether the current type is a generic type.
		/// </summary>
		bool IsGenericType { get; }

		/// <summary>
		/// Gets a value indicating whether the fields of the current type are laid out automatically by the common language runtime.
		/// </summary>
		bool IsAutoLayout { get; }

		/// <summary>
		/// Gets a value indicating whether the fields of the current type are laid out at explicitly specified offsets.
		/// </summary>
		bool IsExplicitLayout { get; }

		/// <summary>
		/// Gets a value indicating whether the fields of the current type are laid out sequentially, in the order that they were defined or emitted to the metadata.
		/// </summary>
		bool IsLayoutSequential { get; }

		/// <summary>
		/// Gets a value indicating whether the current <see cref="ITypeSymbol"/> encompasses or refers to another type; that is, whether the current <see cref="ITypeSymbol"/> is an array, a pointer, or is passed by reference.
		/// </summary>
		bool HasElementType { get; }

		/// <summary>
		/// Gets a value indicating whether the Type is a class or a delegate; that is, not a value type or interface.
		/// </summary>
		bool IsClass { get; }

		/// <summary>
		/// Gets a value indicating whether the Type is a value type.
		/// </summary>
		bool IsValueType { get; }

		/// <summary>
		/// Gets a value indicating whether the Type is an interface; that is, not a class or a value type.
		/// </summary>
		bool IsInterface { get; }

		/// <summary>
		/// Gets a value indicating whether the Type is one of the primitive types.
		/// </summary>
		bool IsPrimitive { get; }

		/// <summary>
		/// Gets a value that indicates whether the type is an array.
		/// </summary>
		bool IsArray { get; }

		/// <summary>
		/// Gets a value indicating whether the current Type represents an enumeration.
		/// </summary>
		bool IsEnum { get; }

		/// <summary>
		/// Gets a value indicating whether the Type is a pointer.
		/// </summary>
		bool IsPointer { get; }

		/// <summary>
		/// Gets a value indicating whether the Type is passed by reference.
		/// </summary>
		bool IsByRef { get; }

		/// <summary>
		/// Gets a value indicating whether the Type is abstract and must be overridden.
		/// </summary>
		bool IsAbstract { get; }

		/// <summary>
		/// Gets a value indicating whether the Type is declared sealed.
		/// </summary>
		bool IsSealed { get; }

		/// <summary>
		/// Gets a value indicating whether the Type can be accessed by code outside the assembly.
		/// </summary>
		bool IsVisible { get; }

		/// <summary>
		/// Gets a value indicating whether the Type is declared public.
		/// </summary>
		bool IsPublic { get; }

		/// <summary>
		/// Gets a value indicating whether the Type is not declared public.
		/// </summary>
		bool IsNotPublic { get; }

		/// <summary>
		/// Gets a value indicating whether the current Type object represents a type whose definition is nested inside the definition of another type.
		/// </summary>
		bool IsNested { get; }

		/// <summary>
		/// Gets a value indicating whether the Type is nested and visible only within its own assembly.
		/// </summary>
		bool IsNestedAssembly { get; }

		/// <summary>
		/// Gets a value indicating whether the Type is nested and visible only to classes that belong to both its own family and its own assembly.
		/// </summary>
		bool IsNestedFamANDAssem { get; }

		/// <summary>
		/// Gets a value indicating whether the Type is nested and visible only within its own family.
		/// </summary>
		bool IsNestedFamily { get; }

		/// <summary>
		/// Gets a value indicating whether the Type is nested and visible only to classes that belong to either its own family or to its own assembly.
		/// </summary>
		bool IsNestedFamORAssem { get; }

		/// <summary>
		/// Gets a value indicating whether the Type is nested and declared private.
		/// </summary>
		bool IsNestedPrivate { get; }

		/// <summary>
		/// Gets a value indicating whether a class is nested and declared public.
		/// </summary>
		bool IsNestedPublic { get; }

		/// <summary>
		/// Gets a value indicating whether the Type is binary serializable.
		/// </summary>
		bool IsSerializable { get; }

		/// <summary>
		/// Gets the initializer for the type.
		/// </summary>
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
