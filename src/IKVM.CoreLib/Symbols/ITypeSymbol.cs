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
		/// Gets the namespace of the <see cref="ITypeSymbol"/>.
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
		/// Gets a value indicating whether the current <see cref="ITypeSymbol"/> represents a type parameter in the definition of a generic type or method.
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
		/// Gets a value indicating whether the <see cref="ITypeSymbol"/> is a class or a delegate; that is, not a value type or interface.
		/// </summary>
		bool IsClass { get; }

		/// <summary>
		/// Gets a value indicating whether the <see cref="ITypeSymbol"/> is a value type.
		/// </summary>
		bool IsValueType { get; }

		/// <summary>
		/// Gets a value indicating whether the <see cref="ITypeSymbol"/> is an interface; that is, not a class or a value type.
		/// </summary>
		bool IsInterface { get; }

		/// <summary>
		/// Gets a value indicating whether the <see cref="ITypeSymbol"/> is one of the primitive types.
		/// </summary>
		bool IsPrimitive { get; }

		/// <summary>
		/// Gets a value that indicates whether the type is an array.
		/// </summary>
		bool IsArray { get; }

		/// <summary>
		/// Gets a value indicating whether the current <see cref="ITypeSymbol"/> represents an enumeration.
		/// </summary>
		bool IsEnum { get; }

		/// <summary>
		/// Gets a value indicating whether the <see cref="ITypeSymbol"/> is a pointer.
		/// </summary>
		bool IsPointer { get; }

		/// <summary>
		/// Gets a value indicating whether the <see cref="ITypeSymbol"/> is passed by reference.
		/// </summary>
		bool IsByRef { get; }

		/// <summary>
		/// Gets a value indicating whether the <see cref="ITypeSymbol"/> is abstract and must be overridden.
		/// </summary>
		bool IsAbstract { get; }

		/// <summary>
		/// Gets a value indicating whether the <see cref="ITypeSymbol"/> is declared sealed.
		/// </summary>
		bool IsSealed { get; }

		/// <summary>
		/// Gets a value indicating whether the <see cref="ITypeSymbol"/> can be accessed by code outside the assembly.
		/// </summary>
		bool IsVisible { get; }

		/// <summary>
		/// Gets a value indicating whether the <see cref="ITypeSymbol"/> is declared public.
		/// </summary>
		bool IsPublic { get; }

		/// <summary>
		/// Gets a value indicating whether the <see cref="ITypeSymbol"/> is not declared public.
		/// </summary>
		bool IsNotPublic { get; }

		/// <summary>
		/// Gets a value indicating whether the current <see cref="ITypeSymbol"/> object represents a type whose definition is nested inside the definition of another type.
		/// </summary>
		bool IsNested { get; }

		/// <summary>
		/// Gets a value indicating whether the <see cref="ITypeSymbol"/> is nested and visible only within its own assembly.
		/// </summary>
		bool IsNestedAssembly { get; }

		/// <summary>
		/// Gets a value indicating whether the <see cref="ITypeSymbol"/> is nested and visible only to classes that belong to both its own family and its own assembly.
		/// </summary>
		bool IsNestedFamANDAssem { get; }

		/// <summary>
		/// Gets a value indicating whether the <see cref="ITypeSymbol"/> is nested and visible only within its own family.
		/// </summary>
		bool IsNestedFamily { get; }

		/// <summary>
		/// Gets a value indicating whether the <see cref="ITypeSymbol"/> is nested and visible only to classes that belong to either its own family or to its own assembly.
		/// </summary>
		bool IsNestedFamORAssem { get; }

		/// <summary>
		/// Gets a value indicating whether the <see cref="ITypeSymbol"/> is nested and declared private.
		/// </summary>
		bool IsNestedPrivate { get; }

		/// <summary>
		/// Gets a value indicating whether a class is nested and declared public.
		/// </summary>
		bool IsNestedPublic { get; }

		/// <summary>
		/// Gets a value indicating whether the <see cref="ITypeSymbol"/> is binary serializable.
		/// </summary>
		bool IsSerializable { get; }

		/// <summary>
		/// Gets the initializer for the type.
		/// </summary>
		IConstructorSymbol? TypeInitializer { get; }

		/// <summary>
		/// Gets the number of dimensions in an array.
		/// </summary>
		/// <returns></returns>
		int GetArrayRank();

		/// <summary>
		/// Searches for the members defined for the current <see cref="ITypeSymbol"/> whose DefaultMemberAttribute is set.
		/// </summary>
		/// <returns></returns>
		IMemberSymbol[] GetDefaultMembers();

		/// <summary>
		/// When overridden in a derived class, returns the <see cref="ITypeSymbol"/> of the object encompassed or referred to by the current array, pointer or reference type.
		/// </summary>
		/// <returns></returns>
		ITypeSymbol? GetElementType();

		/// <summary>
		/// Returns the name of the constant that has the specified value, for the current enumeration type.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		string? GetEnumName(object value);

		/// <summary>
		/// Returns the names of the members of the current enumeration type.
		/// </summary>
		/// <returns></returns>
		string[] GetEnumNames();

		/// <summary>
		/// Returns the underlying type of the current enumeration type.
		/// </summary>
		/// <returns></returns>
		ITypeSymbol GetEnumUnderlyingType();

		/// <summary>
		/// Returns an array of <see cref="ITypeSymbol"/> objects that represent the type arguments of a closed generic type or the type parameters of a generic type definition.
		/// </summary>
		/// <returns></returns>
		ITypeSymbol[] GetGenericArguments();

		/// <summary>
		/// Returns an array of <see cref="ITypeSymbol"/> objects that represent the constraints on the current generic type parameter.
		/// </summary>
		/// <returns></returns>
		ITypeSymbol[] GetGenericParameterConstraints();

		/// <summary>
		/// Returns a <see cref="ITypeSymbol"/> object that represents a generic type definition from which the current generic type can be constructed.
		/// </summary>
		/// <returns></returns>
		ITypeSymbol GetGenericTypeDefinition();

		/// <summary>
		/// Searches for the interface with the specified name.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		ITypeSymbol? GetInterface(string name);

		/// <summary>
		/// When overridden in a derived class, searches for the specified interface, specifying whether to do a case-insensitive search for the interface name.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="ignoreCase"></param>
		/// <returns></returns>
		ITypeSymbol? GetInterface(string name, bool ignoreCase);

		/// <summary>
		/// When overridden in a derived class, gets all the interfaces implemented or inherited by the current <see cref="ITypeSymbol"/>.
		/// </summary>
		/// <returns></returns>
		ITypeSymbol[] GetInterfaces();

		/// <summary>
		/// Returns an interface mapping for the specified interface type.
		/// </summary>
		/// <param name="interfaceType"></param>
		/// <returns></returns>
		InterfaceMapping GetInterfaceMap(ITypeSymbol interfaceType);

		/// <summary>
		/// Searches for the public members with the specified name.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		IMemberSymbol[] GetMember(string name);

		/// <summary>
		/// Searches for the specified members, using the specified binding constraints.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="bindingAttr"></param>
		/// <returns></returns>
		IMemberSymbol[] GetMember(string name, BindingFlags bindingAttr);

		/// <summary>
		/// Searches for the specified members of the specified member type, using the specified binding constraints.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="type"></param>
		/// <param name="bindingAttr"></param>
		/// <returns></returns>
		IMemberSymbol[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr);

		/// <summary>
		/// Returns all the public members of the current <see cref="ITypeSymbol"/>.
		/// </summary>
		/// <returns></returns>
		IMemberSymbol[] GetMembers();

		/// <summary>
		/// When overridden in a derived class, searches for the members defined for the current <see cref="ITypeSymbol"/>, using the specified binding constraints.
		/// </summary>
		/// <param name="bindingAttr"></param>
		/// <returns></returns>
		IMemberSymbol[] GetMembers(BindingFlags bindingAttr);

		/// <summary>
		/// Searches for a public instance constructor whose parameters match the types in the specified array.
		/// </summary>
		/// <param name="types"></param>
		/// <returns></returns>
		IConstructorSymbol? GetConstructor(ITypeSymbol[] types);

		/// <summary>
		/// Searches for a constructor whose parameters match the specified argument types, using the specified binding constraints.
		/// </summary>
		/// <param name="bindingAttr"></param>
		/// <param name="types"></param>
		/// <returns></returns>
		IConstructorSymbol? GetConstructor(BindingFlags bindingAttr, ITypeSymbol[] types);

		/// <summary>
		/// Returns all the public constructors defined for the current <see cref="ITypeSymbol"/>.
		/// </summary>
		/// <returns></returns>
		IConstructorSymbol[] GetConstructors();

		/// <summary>
		/// When overridden in a derived class, searches for the constructors defined for the current <see cref="ITypeSymbol"/>, using the specified BindingFlags.
		/// </summary>
		/// <param name="bindingAttr"></param>
		/// <returns></returns>
		IConstructorSymbol[] GetConstructors(BindingFlags bindingAttr);

		/// <summary>
		/// Searches for the public field with the specified name.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		IFieldSymbol? GetField(string name);

		/// <summary>
		/// Searches for the specified field, using the specified binding constraints.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="bindingAttr"></param>
		/// <returns></returns>
		IFieldSymbol? GetField(string name, BindingFlags bindingAttr);

		/// <summary>
		/// Returns all the public fields of the current <see cref="ITypeSymbol"/>.
		/// </summary>
		/// <returns></returns>
		IFieldSymbol[] GetFields();

		/// <summary>
		/// When overridden in a derived class, searches for the fields defined for the current <see cref="ITypeSymbol"/>, using the specified binding constraints.
		/// </summary>
		/// <param name="bindingAttr"></param>
		/// <returns></returns>
		IFieldSymbol[] GetFields(BindingFlags bindingAttr);

		/// <summary>
		/// Searches for the public method with the specified name.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		IMethodSymbol? GetMethod(string name);

		/// <summary>
		/// Searches for the specified public method whose parameters match the specified argument types.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="types"></param>
		/// <returns></returns>
		IMethodSymbol? GetMethod(string name, ITypeSymbol[] types);

		/// <summary>
		/// Searches for the specified method, using the specified binding constraints.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="bindingAttr"></param>
		/// <returns></returns>
		IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr);

		/// <summary>
		/// Searches for the specified method whose parameters match the specified argument types, using the specified binding constraints.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="bindingAttr"></param>
		/// <param name="types"></param>
		/// <returns></returns>
		IMethodSymbol? GetMethod(string name, BindingFlags bindingAttr, ITypeSymbol[] types);

		/// <summary>
		/// Returns all the public methods of the current <see cref="ITypeSymbol"/>.
		/// </summary>
		/// <returns></returns>
		IMethodSymbol[] GetMethods();

		/// <summary>
		/// When overridden in a derived class, searches for the methods defined for the current <see cref="ITypeSymbol"/>, using the specified binding constraints.
		/// </summary>
		/// <param name="bindingAttr"></param>
		/// <returns></returns>
		IMethodSymbol[] GetMethods(BindingFlags bindingAttr);

		/// <summary>
		/// Searches for the public property with the specified name.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		IPropertySymbol? GetProperty(string name);

		/// <summary>
		/// Searches for the specified property, using the specified binding constraints.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="bindingAttr"></param>
		/// <returns></returns>
		IPropertySymbol? GetProperty(string name, BindingFlags bindingAttr);

		/// <summary>
		/// Searches for the specified public property whose parameters match the specified argument types.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="types"></param>
		/// <returns></returns>
		IPropertySymbol? GetProperty(string name, ITypeSymbol[] types);

		/// <summary>
		/// Searches for the specified public property whose parameters match the specified argument types.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="returnType"></param>
		/// <param name="types"></param>
		/// <returns></returns>
		IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType, ITypeSymbol[] types);

		/// <summary>
		/// Searches for the public property with the specified name and return type.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="returnType"></param>
		/// <returns></returns>
		IPropertySymbol? GetProperty(string name, ITypeSymbol? returnType);

		/// <summary>
		/// Returns all the public properties of the current <see cref="ITypeSymbol"/>.
		/// </summary>
		/// <returns></returns>
		IPropertySymbol[] GetProperties();

		/// <summary>
		/// When overridden in a derived class, searches for the properties of the current <see cref="ITypeSymbol"/>, using the specified binding constraints.
		/// </summary>
		/// <param name="bindingAttr"></param>
		/// <returns></returns>
		IPropertySymbol[] GetProperties(BindingFlags bindingAttr);

		/// <summary>
		/// Returns the EventInfo object representing the specified public event.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		IEventSymbol? GetEvent(string name);

		/// <summary>
		/// When overridden in a derived class, returns the EventInfo object representing the specified event, using the specified binding constraints.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="bindingAttr"></param>
		/// <returns></returns>
		IEventSymbol? GetEvent(string name, BindingFlags bindingAttr);

		/// <summary>
		/// Returns all the public events that are declared or inherited by the current <see cref="ITypeSymbol"/>.
		/// </summary>
		/// <returns></returns>
		IEventSymbol[] GetEvents();

		/// <summary>
		/// When overridden in a derived class, searches for events that are declared or inherited by the current <see cref="ITypeSymbol"/>, using the specified binding constraints.
		/// </summary>
		/// <param name="bindingAttr"></param>
		/// <returns></returns>
		IEventSymbol[] GetEvents(BindingFlags bindingAttr);

		/// <summary>
		/// Searches for the public nested type with the specified name.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		ITypeSymbol? GetNestedType(string name);

		/// <summary>
		/// When overridden in a derived class, searches for the specified nested type, using the specified binding constraints.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="bindingAttr"></param>
		/// <returns></returns>
		ITypeSymbol? GetNestedType(string name, BindingFlags bindingAttr);

		/// <summary>
		/// Returns the public types nested in the current <see cref="ITypeSymbol"/>.
		/// </summary>
		/// <returns></returns>
		ITypeSymbol[] GetNestedTypes();

		/// <summary>
		/// When overridden in a derived class, searches for the types nested in the current <see cref="ITypeSymbol"/>, using the specified binding constraints.
		/// </summary>
		/// <param name="bindingAttr"></param>
		/// <returns></returns>
		ITypeSymbol[] GetNestedTypes(BindingFlags bindingAttr);

		/// <summary>
		/// Determines whether an instance of a specified type c can be assigned to a variable of the current type.
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		bool IsAssignableFrom(ITypeSymbol? c);

		/// <summary>
		/// Determines whether the current <see cref="ITypeSymbol"/> derives from the specified <see cref="ITypeSymbol"/>.
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		bool IsSubclassOf(ITypeSymbol c);

		/// <summary>
		/// Returns a value that indicates whether the specified value exists in the current enumeration type.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		bool IsEnumDefined(object value);

		/// <summary>
		/// Returns a <see cref="ITypeSymbol"/> object representing a one-dimensional array of the current type, with a lower bound of zero.
		/// </summary>
		/// <returns></returns>
		ITypeSymbol MakeArrayType();

		/// <summary>
		/// Returns a <see cref="ITypeSymbol"/> object representing an array of the current type, with the specified number of dimensions.
		/// </summary>
		/// <param name="rank"></param>
		/// <returns></returns>
		ITypeSymbol MakeArrayType(int rank);

		/// <summary>
		/// Substitutes the elements of an array of types for the type parameters of the current generic type definition and returns a <see cref="ITypeSymbol"/> object representing the resulting constructed type.
		/// </summary>
		/// <param name="typeArguments"></param>
		/// <returns></returns>
		ITypeSymbol MakeGenericType(params ITypeSymbol[] typeArguments);

		/// <summary>
		/// Returns a <see cref="ITypeSymbol"/> object that represents a pointer to the current type.
		/// </summary>
		/// <returns></returns>
		ITypeSymbol MakePointerType();

		/// <summary>
		/// Returns a <see cref="ITypeSymbol"/> object that represents the current type when passed as a ref parameter (ByRef parameter in Visual Basic).
		/// </summary>
		/// <returns></returns>
		ITypeSymbol MakeByRefType();

	}

}
