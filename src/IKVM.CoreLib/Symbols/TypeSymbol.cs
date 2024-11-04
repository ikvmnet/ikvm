using System;
using System.Collections.Immutable;
using System.Reflection;

namespace IKVM.CoreLib.Symbols
{

    abstract class TypeSymbol : MemberSymbol
    {

        const BindingFlags DefaultLookup = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public;
        const BindingFlags DeclaredOnlyLookup = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;

        TypeSpecTable _typeSpecTable;

        string? _assemblyQualifiedName;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public TypeSymbol(ISymbolContext context, IModuleSymbol module, TypeSymbol? declaringType) :
            base(context, module, declaringType)
        {
            _typeSpecTable = new TypeSpecTable(Context, this);
        }

        /// <summary>
        /// Gets the attributes associated with the <see cref="TypeSymbol">.
        /// </summary>
        public abstract TypeAttributes Attributes { get; }

        /// <summary>
        /// Gets a <see cref="IMethodBaseSymbol"/> that represents the declaring method, if the current <see cref="TypeSymbol"/> represents a type parameter of a generic method.
        /// </summary>
        public abstract MethodBaseSymbol? DeclaringMethod { get; }

        /// <summary>
        /// Gets the assembly-qualified name of the type, which includes the name of the assembly from which this <see cref="TypeSymbol"/> object was loaded.
        /// </summary>
        public string? AssemblyQualifiedName => _assemblyQualifiedName ??= ComputeAssemblyQualifiedName();

        /// <summary>
        /// Computes the value of <see cref="AssemblyQualifiedName"/>.
        /// </summary>
        /// <returns></returns>
        string? ComputeAssemblyQualifiedName()
        {
            var fullName = FullName;
            if (fullName == null) // open types return null for FullName by design.
                return null;

            var assemblyName = Assembly.FullName;
            return fullName + ", " + assemblyName;
        }

        /// <summary>
        /// Gets the fully qualified name of the type, including its namespace but not its assembly.
        /// </summary>
        public abstract string? FullName { get; }

        /// <summary>
        /// Gets the namespace of the <see cref="TypeSymbol"/>.
        /// </summary>
        public abstract string? Namespace { get; }

        /// <summary>
        /// Gets the underlying type code of the specified Type.
        /// </summary>
        public abstract TypeCode TypeCode { get; }

        /// <inheritdoc />
        public sealed override MemberTypes MemberType => IsPublic || IsNotPublic ? MemberTypes.TypeInfo : MemberTypes.NestedType;

        /// <summary>
        /// Gets the type from which the current <see cref="TypeSymbol"/> directly inherits.
        /// </summary>
        public abstract TypeSymbol? BaseType { get; }

        /// <summary>
        /// Gets a value indicating whether the current <see cref="TypeSymbol"/> object has type parameters that have not been replaced by specific types.
        /// </summary>
        public abstract bool ContainsGenericParameters { get; }

        /// <summary>
        /// Gets a combination of <see cref="GenericParameterAttributes"/> flags that describe the covariance and special constraints of the current generic type parameter.
        /// </summary>
        public abstract GenericParameterAttributes GenericParameterAttributes { get; }

        /// <summary>
        /// Gets the position of the type parameter in the type parameter list of the generic type or method that declared the parameter, when the <see cref="TypeSymbol"/> object represents a type parameter of a generic type or a generic method.
        /// </summary>
        public abstract int GenericParameterPosition { get; }

        /// <summary>
        /// Gets an array of the generic type arguments for this type.
        /// </summary>
        public abstract ImmutableList<TypeSymbol> GenericTypeArguments { get; }

        /// <summary>
        /// Gets a value that indicates whether the type is a type definition.
        /// </summary>
        public abstract bool IsTypeDefinition { get; }

        /// <summary>
        /// Gets a value indicating whether the current Type represents a generic type definition, from which other generic types can be constructed.
        /// </summary>
        public abstract bool IsGenericTypeDefinition { get; }

        /// <summary>
        /// Gets a value that indicates whether this object represents a constructed generic type. You can create instances of a constructed generic type.
        /// </summary>
        public abstract bool IsConstructedGenericType { get; }

        /// <summary>
        /// Gets a value indicating whether the current type is a generic type.
        /// </summary>
        public bool IsGenericType => IsConstructedGenericType || IsGenericTypeDefinition;

        /// <summary>
        /// Gets a value indicating whether the current <see cref="TypeSymbol"/> represents a type parameter in the definition of a generic type or method.
        /// </summary>
        public abstract bool IsGenericParameter { get; }

        /// <summary>
        /// Gets a value indicating whether the fields of the current type are laid out automatically by the common language runtime.
        /// </summary>
        public bool IsAutoLayout => (Attributes & TypeAttributes.LayoutMask) == TypeAttributes.AutoLayout;

        /// <summary>
        /// Gets a value indicating whether the fields of the current type are laid out at explicitly specified offsets.
        /// </summary>
        public bool IsExplicitLayout => (Attributes & TypeAttributes.LayoutMask) == TypeAttributes.ExplicitLayout;

        /// <summary>
        /// Gets a value indicating whether the fields of the current type are laid out sequentially, in the order that they were defined or emitted to the metadata.
        /// </summary>
        public bool IsLayoutSequential => (Attributes & TypeAttributes.LayoutMask) == TypeAttributes.SequentialLayout;

        /// <summary>
        /// Gets a value indicating whether the current <see cref="TypeSymbol"/> encompasses or refers to another type; that is, whether the current <see cref="TypeSymbol"/> is an array, a pointer, or is passed by reference.
        /// </summary>
        public abstract bool HasElementType { get; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="TypeSymbol"/> is a class or a delegate; that is, not a value type or interface.
        /// </summary>
        public bool IsClass => (Attributes & TypeAttributes.ClassSemanticsMask) == TypeAttributes.Class && !IsValueType;

        /// <summary>
        /// Gets a value indicating whether the <see cref="TypeSymbol"/> is a value type.
        /// </summary>
        public bool IsValueType => IsSubclassOf(Context.ResolveCoreType("System.ValueType"));

        /// <summary>
        /// Gets a value indicating whether the <see cref="TypeSymbol"/> is an interface; that is, not a class or a value type.
        /// </summary>
        public bool IsInterface => (Attributes & TypeAttributes.Interface) != 0;

        /// <summary>
        /// Gets a value indicating whether the <see cref="TypeSymbol"/> is one of the primitive types.
        /// </summary>
        public abstract bool IsPrimitive { get; }

        /// <summary>
        /// Gets a value that indicates whether the type is an array type that can represent only a single-dimensional array with a zero lower bound.
        /// </summary>
        public abstract bool IsSZArray { get; }

        /// <summary>
        /// Gets a value that indicates whether the type is an array.
        /// </summary>
        public abstract bool IsArray { get; }

        /// <summary>
        /// Gets a value indicating whether the current <see cref="TypeSymbol"/> represents an enumeration.
        /// </summary>
        public abstract bool IsEnum { get; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="TypeSymbol"/> is a pointer.
        /// </summary>
        public abstract bool IsPointer { get; }

        /// <summary>
        /// Gets a value that indicates whether the current <see cref="TypeSymbol"/> is an unmanaged function pointer.
        /// </summary>
        public abstract bool IsFunctionPointer { get; }

        /// <summary>
        /// Gets a value that indicates whether the current <see cref="TypeSymbol"/> is an unmanaged function pointer.
        /// </summary>
        public abstract bool IsUnmanagedFunctionPointer { get; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="TypeSymbol"/> is passed by reference.
        /// </summary>
        public abstract bool IsByRef { get; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="TypeSymbol"/> is abstract and must be overridden.
        /// </summary>
        public bool IsAbstract => (Attributes & TypeAttributes.Abstract) == TypeAttributes.Abstract;

        /// <summary>
        /// Gets a value indicating whether the <see cref="TypeSymbol"/> is declared sealed.
        /// </summary>
        public bool IsSealed => (Attributes & TypeAttributes.Sealed) == TypeAttributes.Sealed;

        /// <summary>
        /// Gets a value indicating whether the <see cref="TypeSymbol"/> can be accessed by code outside the assembly.
        /// </summary>
        public bool IsVisible => ComputeIsVisible();

        /// <summary>
        /// Computes the value for <see cref="IsVisible"/>.
        /// </summary>
        /// <returns></returns>
        bool ComputeIsVisible()
        {
            if (IsGenericParameter)
                return true;

            if (HasElementType)
                return GetElementType()!.IsVisible;

            var type = (TypeSymbol)this;
            while (type.IsNested)
            {
                if (!type.IsNestedPublic)
                    return false;

                // this should be null for non-nested types.
                type = type.DeclaringType!;
            }

            // Now "type" should be a top level type
            if (!type.IsPublic)
                return false;

            if (IsGenericType && !IsGenericTypeDefinition)
            {
                foreach (var t in GetGenericArguments())
                {
                    if (!t.IsVisible)
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="TypeSymbol"/> is declared public.
        /// </summary>
        public bool IsPublic => (Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.Public;

        /// <summary>
        /// Gets a value indicating whether the <see cref="TypeSymbol"/> is not declared public.
        /// </summary>
        public bool IsNotPublic => (Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NotPublic;

        /// <summary>
        /// Gets a value indicating whether the current <see cref="TypeSymbol"/> object represents a type whose definition is nested inside the definition of another type.
        /// </summary>
        public bool IsNested => DeclaringType != null;

        /// <summary>
        /// Gets a value indicating whether the <see cref="TypeSymbol"/> is nested and visible only within its own assembly.
        /// </summary>
        public bool IsNestedAssembly => (Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedAssembly;

        /// <summary>
        /// Gets a value indicating whether the <see cref="TypeSymbol"/> is nested and visible only to classes that belong to both its own family and its own assembly.
        /// </summary>
        public bool IsNestedFamANDAssem => (Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedFamANDAssem;

        /// <summary>
        /// Gets a value indicating whether the <see cref="TypeSymbol"/> is nested and visible only within its own family.
        /// </summary>
        public bool IsNestedFamily => (Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedFamily;

        /// <summary>
        /// Gets a value indicating whether the <see cref="TypeSymbol"/> is nested and visible only to classes that belong to either its own family or to its own assembly.
        /// </summary>
        public bool IsNestedFamORAssem => (Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedFamORAssem;

        /// <summary>
        /// Gets a value indicating whether the <see cref="TypeSymbol"/> is nested and declared private.
        /// </summary>
        public bool IsNestedPrivate => (Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedPrivate;

        /// <summary>
        /// Gets a value indicating whether a class is nested and declared public.
        /// </summary>
        public bool IsNestedPublic => (Attributes & TypeAttributes.VisibilityMask) == TypeAttributes.NestedPublic;

        /// <summary>
        /// Gets a value indicating whether the <see cref="TypeSymbol"/> is binary serializable.
        /// </summary>
        public bool IsSerializable => (Attributes & TypeAttributes.Serializable) != 0;

        /// <summary>
        /// Gets a value that indicates whether the type is a signature type.
        /// </summary>
        public abstract bool IsSignatureType { get; }

        /// <summary>
        /// Gets a value indicating whether the type has a name that requires special handling.
        /// </summary>
        public bool IsSpecialName => (Attributes & TypeAttributes.SpecialName) == TypeAttributes.SpecialName;

        /// <summary>
        /// Gets the initializer for the type.
        /// </summary>
        public abstract ConstructorSymbol? TypeInitializer { get; }

        /// <summary>
        /// Gets the number of dimensions in an array.
        /// </summary>
        /// <returns></returns>
        public abstract int GetArrayRank();

        /// <summary>
        /// Returns all the declared constructors of the current <see cref="TypeSymbol"/>.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableList<ConstructorSymbol> GetDeclaredConstructors();

        /// <summary>
        /// Searches for a public instance constructor whose parameters match the types in the specified array.
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public ConstructorSymbol? GetConstructor(ImmutableList<TypeSymbol>? types) => GetConstructor(BindingFlags.Public | BindingFlags.Instance, types);

        /// <summary>
        /// Searches for a constructor whose parameters match the specified argument types, using the specified binding constraints.
        /// </summary>
        /// <param name="bindingFlags"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public abstract ConstructorSymbol? GetConstructor(BindingFlags bindingFlags, ImmutableList<TypeSymbol>? types);

        /// <summary>
        /// Returns all the public constructors defined for the current <see cref="TypeSymbol"/>.
        /// </summary>
        /// <returns></returns>
        public ImmutableList<ConstructorSymbol> GetConstructors() => GetConstructors(BindingFlags.Public | BindingFlags.Instance);

        /// <summary>
        /// When overridden in a derived class, searches for the constructors defined for the current <see cref="TypeSymbol"/>, using the specified BindingFlags.
        /// </summary>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public abstract ImmutableList<ConstructorSymbol> GetConstructors(BindingFlags bindingFlags);

        /// <summary>
        /// Searches for the members defined for the current <see cref="TypeSymbol"/> whose DefaultMemberAttribute is set.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableList<MemberSymbol> GetDefaultMembers();

        /// <summary>
        /// When overridden in a derived class, returns the <see cref="TypeSymbol"/> of the object encompassed or referred to by the current array, pointer or reference type.
        /// </summary>
        /// <returns></returns>
        public abstract TypeSymbol? GetElementType();

        /// <summary>
        /// Returns the name of the constant that has the specified value, for the current enumeration type.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract string? GetEnumName(object value);

        /// <summary>
        /// Returns the names of the members of the current enumeration type.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableList<string> GetEnumNames();

        /// <summary>
        /// Returns the underlying type of the current enumeration type.
        /// </summary>
        /// <returns></returns>
        public abstract TypeSymbol GetEnumUnderlyingType();

        /// <summary>
        /// Returns all the declared events of the current <see cref="TypeSymbol"/>.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableList<EventSymbol> GetDeclaredEvents();

        /// <summary>
        /// Returns the <see cref="IEventSymbol"/> object representing the specified public event.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public EventSymbol? GetEvent(string name) => GetEvent(name, DefaultLookup);

        /// <summary>
        /// When overridden in a derived class, returns the <see cref="IEventSymbol"/> object representing the specified event, using the specified binding constraints.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public abstract EventSymbol? GetEvent(string name, BindingFlags bindingFlags);

        /// <summary>
        /// Returns all the public events that are declared or inherited by the current <see cref="TypeSymbol"/>.
        /// </summary>
        /// <returns></returns>
        public ImmutableList<EventSymbol> GetEvents() => GetEvents(DefaultLookup);

        /// <summary>
        /// When overridden in a derived class, searches for events that are declared or inherited by the current <see cref="TypeSymbol"/>, using the specified binding constraints.
        /// </summary>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public abstract ImmutableList<EventSymbol> GetEvents(BindingFlags bindingFlags);

        /// <summary>
        /// Returns all the declared fields of the current <see cref="TypeSymbol"/>.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableList<FieldSymbol> GetDeclaredFields();

        /// <summary>
        /// Searches for the public field with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public FieldSymbol? GetField(string name) => GetField(name, DefaultLookup);

        /// <summary>
        /// Searches for the specified field, using the specified binding constraints.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public abstract FieldSymbol? GetField(string name, BindingFlags bindingFlags);

        /// <summary>
        /// Returns all the public fields of the current <see cref="TypeSymbol"/>.
        /// </summary>
        /// <returns></returns>
        public ImmutableList<FieldSymbol> GetFields() => GetFields(DefaultLookup);

        /// <summary>
        /// When overridden in a derived class, searches for the fields defined for the current <see cref="TypeSymbol"/>, using the specified binding constraints.
        /// </summary>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public abstract ImmutableList<FieldSymbol> GetFields(BindingFlags bindingFlags);

        /// <summary>
        /// Returns an array of <see cref="TypeSymbol"/> objects that represent the type arguments of a closed generic type or the type parameters of a generic type definition.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableList<TypeSymbol> GetGenericArguments();

        /// <summary>
        /// Returns an array of <see cref="TypeSymbol"/> objects that represent the constraints on the current generic type parameter.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableList<TypeSymbol> GetGenericParameterConstraints();

        /// <summary>
        /// Returns a <see cref="TypeSymbol"/> object that represents a generic type definition from which the current generic type can be constructed.
        /// </summary>
        /// <returns></returns>
        public abstract TypeSymbol GetGenericTypeDefinition();

        /// <summary>
        /// Searches for the interface with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TypeSymbol? GetInterface(string name) => GetInterface(name, false);

        /// <summary>
        /// When overridden in a derived class, searches for the specified interface, specifying whether to do a case-insensitive search for the interface name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public abstract TypeSymbol? GetInterface(string name, bool ignoreCase);

        /// <summary>
        /// When overridden in a derived class, gets all the interfaces implemented or if specified,inherited by the current <see cref="TypeSymbol"/>.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableList<TypeSymbol> GetInterfaces(bool inherit = true);

        /// <summary>
        /// Returns an interface mapping for the specified interface type.
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public abstract InterfaceMapping GetInterfaceMap(TypeSymbol interfaceType);

        /// <summary>
        /// Returns all the declared methods of the current <see cref="TypeSymbol"/>.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableList<MethodSymbol> GetDeclaredMethods();

        /// <summary>
        /// Searches for the public method with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public MethodSymbol? GetMethod(string name) => GetMethod(name, DefaultLookup);

        /// <summary>
        /// Searches for the specified public method whose parameters match the specified argument types.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public MethodSymbol? GetMethod(string name, ImmutableList<TypeSymbol>? types) => GetMethod(name, DefaultLookup, types);

        /// <summary>
        /// Searches for the specified method, using the specified binding constraints.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public MethodSymbol? GetMethod(string name, BindingFlags bindingFlags) => GetMethod(name, bindingFlags, null);

        /// <summary>
        /// Searches for the specified method whose parameters match the specified argument types and modifiers, using the specified binding constraints.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bindingAttr"></param>
        /// <param name="types"></param>
        /// <param name="modifiers"></param>
        /// <returns></returns>
        public MethodSymbol? GetMethod(string name, BindingFlags bindingAttr, ImmutableList<TypeSymbol>? types, ImmutableList<ParameterModifier>? modifiers) => GetMethod(name, bindingAttr, CallingConventions.Any, types, modifiers);

        /// <summary>
        /// Searches for the specified method whose parameters match the specified argument types, using the specified binding constraints.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bindingFlags"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public MethodSymbol? GetMethod(string name, BindingFlags bindingFlags, ImmutableList<TypeSymbol>? types) => GetMethod(name, bindingFlags, CallingConventions.Any, types, null);

        /// <summary>
        /// Searches for the specified method whose parameters match the specified argument types and modifiers, using the specified binding constraints and the specified calling convention.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bindingFlags"></param>
        /// <param name="callConvention"></param>
        /// <param name="types"></param>
        /// <param name="modifiers"></param>
        /// <returns></returns>
        public MethodSymbol? GetMethod(string name, BindingFlags bindingFlags, CallingConventions callConvention, ImmutableList<TypeSymbol>? types, ImmutableList<ParameterModifier>? modifiers) => GetMethod(name, 0, bindingFlags, callConvention, types, modifiers);

        /// <summary>
        /// Searches for the specified method whose parameters match the specified generic parameter count, argument types and modifiers, using the specified binding constraints.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="genericParameterCount"></param>
        /// <param name="bindingFlags"></param>
        /// <param name="types"></param>
        /// <param name="modifiers"></param>
        /// <returns></returns>
        public MethodSymbol? GetMethod(string name, int genericParameterCount, BindingFlags bindingFlags, ImmutableList<TypeSymbol>? types, ImmutableList<ParameterModifier>? modifiers) => GetMethod(name, genericParameterCount, bindingFlags, CallingConventions.Any, types, modifiers);

        /// <summary>
        /// Searches for the specified public method whose parameters match the specified generic parameter count, argument types and modifiers.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="genericParameterCount"></param>
        /// <param name="types"></param>
        /// <param name="modifiers"></param>
        /// <returns></returns>
        public MethodSymbol? GetMethod(string name, int genericParameterCount, ImmutableList<TypeSymbol>? types, ImmutableList<ParameterModifier>? modifiers) => GetMethod(name, genericParameterCount, DefaultLookup, CallingConventions.Any, types, modifiers);

        /// <summary>
        /// Searches for the specified method whose parameters match the specified generic parameter count, argument types and modifiers, using the specified binding constraints and the specified calling convention.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="genericParameterCount"></param>
        /// <param name="bindingFlags"></param>
        /// <param name="callConvention"></param>
        /// <param name="types"></param>
        /// <param name="modifiers"></param>
        /// <returns></returns>
        public abstract MethodSymbol? GetMethod(string name, int genericParameterCount, BindingFlags bindingFlags, CallingConventions callConvention, ImmutableList<TypeSymbol>? types, ImmutableList<ParameterModifier>? modifiers);

        /// <summary>
        /// Returns all the public methods of the current <see cref="TypeSymbol"/>.
        /// </summary>
        /// <returns></returns>
        public ImmutableList<MethodSymbol> GetMethods() => GetMethods(DefaultLookup);

        /// <summary>
        /// When overridden in a derived class, searches for the methods defined for the current <see cref="TypeSymbol"/>, using the specified binding constraints.
        /// </summary>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public abstract ImmutableList<MethodSymbol> GetMethods(BindingFlags bindingFlags);

        /// <summary>
        /// Searches for the public nested type with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TypeSymbol? GetNestedType(string name) => GetNestedType(name, DefaultLookup);

        /// <summary>
        /// When overridden in a derived class, searches for the specified nested type, using the specified binding constraints.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public abstract TypeSymbol? GetNestedType(string name, BindingFlags bindingFlags);

        /// <summary>
        /// Returns the public types nested in the current <see cref="TypeSymbol"/>.
        /// </summary>
        /// <returns></returns>
        public ImmutableList<TypeSymbol> GetNestedTypes() => GetNestedTypes(DefaultLookup);

        /// <summary>
        /// When overridden in a derived class, searches for the types nested in the current <see cref="TypeSymbol"/>, using the specified binding constraints.
        /// </summary>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public abstract ImmutableList<TypeSymbol> GetNestedTypes(BindingFlags bindingFlags);

        /// <summary>
        /// Returns all the declared properties of the current <see cref="TypeSymbol"/>.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableList<PropertySymbol> GetDeclaredProperties();

        /// <summary>
        /// Searches for the public property with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public PropertySymbol? GetProperty(string name) => GetProperty(name, DefaultLookup);

        /// <summary>
        /// Searches for the specified property, using the specified binding constraints.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public abstract PropertySymbol? GetProperty(string name, BindingFlags bindingFlags);

        /// <summary>
        /// Searches for the specified public property whose parameters match the specified argument types.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public PropertySymbol? GetProperty(string name, ImmutableList<TypeSymbol>? types) => GetProperty(name, null, types);

        /// <summary>
        /// Searches for the public property with the specified name and return type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="returnType"></param>
        /// <returns></returns>
        public PropertySymbol? GetProperty(string name, TypeSymbol? returnType) => GetProperty(name, returnType, null);

        /// <summary>
        /// Searches for the specified public property whose parameters match the specified argument types.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="returnType"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public abstract PropertySymbol? GetProperty(string name, TypeSymbol? returnType, ImmutableList<TypeSymbol>? types);

        /// <summary>
        /// Returns all the public properties of the current <see cref="TypeSymbol"/>.
        /// </summary>
        /// <returns></returns>
        public ImmutableList<PropertySymbol> GetProperties() => GetProperties(DefaultLookup);

        /// <summary>
        /// When overridden in a derived class, searches for the properties of the current <see cref="TypeSymbol"/>, using the specified binding constraints.
        /// </summary>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public abstract ImmutableList<PropertySymbol> GetProperties(BindingFlags bindingFlags);

        /// <summary>
        /// Determines whether an instance of a specified type c can be assigned to a variable of the current type.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool IsAssignableFrom(TypeSymbol? c)
        {
            if (c == null)
                return false;

            if (this == c)
                return true;

            // If c is a subclass of this class, then c can be cast to this type.
            if (c.IsSubclassOf(this))
                return true;

            if (IsInterface)
            {
                return c.ImplementInterface(this);
            }
            else if (IsGenericParameter)
            {
                var constraints = GetGenericParameterConstraints();
                for (int i = 0; i < constraints.Count; i++)
                    if (!constraints[i].IsAssignableFrom(c))
                        return false;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether an instance of a specified type c is an interface implemented by the current type.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        bool ImplementInterface(TypeSymbol? interfaceType)
        {
            var t = this;

            while (t != null)
            {
                var interfaces = t.GetInterfaces();
                if (interfaces != null)
                {
                    for (int i = 0; i < interfaces.Count; i++)
                    {
                        // Interfaces don't derive from other interfaces, they implement them.
                        // So instead of IsSubclassOf, we should use ImplementInterface instead.
                        if (interfaces[i] == interfaceType || (interfaces[i] != null && interfaces[i].ImplementInterface(interfaceType)))
                            return true;
                    }
                }

                t = t.BaseType;
            }

            return false;
        }

        /// <summary>
        /// Determines whether the current <see cref="TypeSymbol"/> derives from the specified <see cref="TypeSymbol"/>.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool IsSubclassOf(TypeSymbol c)
        {
            var p = (TypeSymbol?)this;
            if (p == c)
                return false;

            while (p != null)
            {
                if (p == c)
                    return true;

                p = p.BaseType;
            }

            return false;
        }

        /// <summary>
        /// Returns a value that indicates whether the specified value exists in the current enumeration type.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract bool IsEnumDefined(object value);

        /// <summary>
        /// Returns a <see cref="TypeSymbol"/> object representing a one-dimensional array of the current type, with a lower bound of zero.
        /// </summary>
        /// <returns></returns>
        public TypeSymbol MakeArrayType() => _typeSpecTable.GetOrCreateSZArrayTypeSymbol();

        /// <summary>
        /// Returns a <see cref="TypeSymbol"/> object representing an array of the current type, with the specified number of dimensions.
        /// </summary>
        /// <param name="rank"></param>
        /// <returns></returns>
        public TypeSymbol MakeArrayType(int rank) => _typeSpecTable.GetOrCreateArrayTypeSymbol(rank);

        /// <summary>
        /// Returns a <see cref="TypeSymbol"/> object that represents the current type when passed as a ref parameter (ByRef parameter in Visual Basic).
        /// </summary>
        /// <returns></returns>
        public TypeSymbol MakeByRefType() => _typeSpecTable.GetOrCreateByRefTypeSymbol();

        /// <summary>
        /// Substitutes the elements of an array of types for the type parameters of the current generic type definition and returns a <see cref="TypeSymbol"/> object representing the resulting constructed type.
        /// </summary>
        /// <param name="typeArguments"></param>
        /// <returns></returns>
        public TypeSymbol MakeGenericType(ImmutableList<TypeSymbol> typeArguments) => _typeSpecTable.GetOrCreateGenericTypeSymbol(typeArguments);

        /// <summary>
        /// Returns a <see cref="TypeSymbol"/> object that represents a pointer to the current type.
        /// </summary>
        /// <returns></returns>
        public TypeSymbol MakePointerType() => _typeSpecTable.GetOrCreatePointerTypeSymbol();

        /// <summary>
        /// Specializes the current <see cref="TypeSymbol"/> given the specified generic arguments.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal virtual TypeSymbol Specialize(GenericContext context)
        {
            if (ContainsGenericParameters == false)
                return this;

            var args = GetGenericArguments();
            for (int i = 0; i < args.Count; i++)
                if (args[i].ContainsGenericParameters)
                    args = args.SetItem(i, args[i].Specialize(context));

            return MakeGenericType(args);
        }

        ImmutableList<T> Query<T>() where T : MemberSymbol
        {

        }

    }

}
