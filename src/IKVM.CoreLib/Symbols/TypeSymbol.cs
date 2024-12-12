using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;

namespace IKVM.CoreLib.Symbols
{

    public abstract class TypeSymbol : MemberSymbol
    {

        TypeSpecTable _typeSpecTable;

        string? _fullName;
        string? _assemblyQualifiedName;
        string? _toString;
        ImmutableArray<TypeSymbol> _genericTypeArguments;
        ImmutableArray<TypeSymbol> _interfaces;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="module"></param>
        protected TypeSymbol(SymbolContext context, ModuleSymbol module) :
            base(context, module)
        {
            _typeSpecTable = new TypeSpecTable(context, this);
        }

        /// <summary>
        /// Gets the attributes associated with the <see cref="TypeSymbol">.
        /// </summary>
        public abstract TypeAttributes Attributes { get; }

        /// <summary>
        /// Gets a <see cref="MethodSymbol"/> that represents the declaring method, if the current <see cref="TypeSymbol"/> represents a type parameter of a generic method.
        /// </summary>
        public abstract MethodSymbol? DeclaringMethod { get; }

        /// <summary>
        /// Gets the assembly-qualified name of the type, which includes the name of the assembly from which this <see cref="TypeSymbol"/> object was loaded.
        /// </summary>
        public string? AssemblyQualifiedName => _assemblyQualifiedName ??= TypeSymbolNameBuilder.ToString(this, TypeSymbolNameBuilder.Format.AssemblyQualifiedName);

        /// <summary>
        /// Gets the fully qualified name of the type, including its namespace but not its assembly.
        /// </summary>
        public string? FullName => _fullName ??= TypeSymbolNameBuilder.ToString(this, TypeSymbolNameBuilder.Format.FullName);

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
        /// Returns whether this type contains any missing types.
        /// </summary>
        public virtual bool ContainsMissingType => IsMissing || GenericArguments.Any(i => i.ContainsMissingType) || GetRequiredCustomModifiers().Any(i => i.ContainsMissingType) || GetOptionalCustomModifiers().Any(i => i.ContainsMissingType);

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
        public abstract ImmutableArray<TypeSymbol> GenericArguments { get; }

        /// <summary>
        /// Gets a value that indicates whether the type is a type definition.
        /// </summary>
        public abstract bool IsTypeDefinition { get; }

        /// <summary>
        /// Gets a value indicating whether the current Type represents a generic type definition, from which other generic types can be constructed.
        /// </summary>
        public abstract bool IsGenericTypeDefinition { get; }

        /// <summary>
        /// Gets an array of the generic type arguments for this type.
        /// </summary>
        public abstract TypeSymbol GenericTypeDefinition { get; }

        /// <summary>
        /// Gets a value that indicates whether this object represents a constructed generic type. You can create instances of a constructed generic type.
        /// </summary>
        public abstract bool IsConstructedGenericType { get; }

        /// <summary>
        /// Gets a value indicating whether the current type is a generic type.
        /// </summary>
        public bool IsGenericType => IsConstructedGenericType || IsGenericTypeDefinition;

        /// <summary>
        /// Gets a value indicating whether the current  represents a type parameter in the definition of a generic type or method.
        /// </summary>
        public bool IsGenericParameter => IsGenericTypeParameter || IsGenericMethodParameter;

        /// <summary>
        /// Gets a value that indicates whether the current <see cref="TypeSymbol"/> represents a type parameter in the definition of a generic type.
        /// </summary>
        public abstract bool IsGenericTypeParameter { get; }

        /// <summary>
        /// Gets a value that indicates whether the current <see cref="TypeSymbol"/> represents a type parameter in the definition of a generic method.
        /// </summary>
        public abstract bool IsGenericMethodParameter { get; }

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
        public bool IsValueType => IsSubclassOf(Context.ResolveCoreType("System.ValueType")) || IsGenericParameter && (GenericParameterAttributes & GenericParameterAttributes.NotNullableValueTypeConstraint) != 0;

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
        public virtual bool IsVisible => IsPublic || (IsNestedPublic && DeclaringType!.IsVisible);

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
        /// Gets a value indicating whether the type has a name that requires special handling.
        /// </summary>
        public bool IsSpecialName => (Attributes & TypeAttributes.SpecialName) == TypeAttributes.SpecialName;

        /// <summary>
        /// Gets the initializer for the type.
        /// </summary>
        public MethodSymbol? TypeInitializer => GetMethod(ConstructorInfo.TypeConstructorName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, CallingConventions.Any, default, default);

        /// <summary>
        /// Gets the number of dimensions in an array.
        /// </summary>
        /// <returns></returns>
        public abstract int GetArrayRank();

        /// <summary>
        /// Searches for a public instance constructor whose parameters match the types in the specified array.
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public MethodSymbol? GetConstructor(TypeSymbolSelectorList types) => GetConstructor(BindingFlags.Public | BindingFlags.Instance, types, default);

        /// <summary>
        /// Searches for a constructor whose parameters match the specified argument types, using the specified binding constraints.
        /// </summary>
        /// <param name="bindingFlags"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public MethodSymbol? GetConstructor(BindingFlags bindingFlags, TypeSymbolSelectorList types) => GetConstructor(bindingFlags, types, default);

        /// <summary>
        /// Searches for a constructor whose parameters match the specified argument types and modifiers, using the specified binding constraints.
        /// </summary>
        /// <param name="bindingFlags"></param>
        /// <param name="types"></param>
        /// <param name="modifiers"></param>
        /// <returns></returns>
        public MethodSymbol? GetConstructor(BindingFlags bindingFlags, TypeSymbolSelectorList types, ImmutableArray<ParameterModifier> modifiers) => GetConstructor(bindingFlags, CallingConventions.Any, types, modifiers);

        /// <summary>
        /// Searches for a constructor whose parameters match the specified argument types, using the specified binding constraints.
        /// </summary>
        /// <param name="bindingFlags"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public MethodSymbol? GetConstructor(BindingFlags bindingFlags, CallingConventions callConvention, TypeSymbolSelectorList types, ImmutableArray<ParameterModifier> modifiers)
        {
            return GetMethod(ConstructorInfo.ConstructorName, bindingFlags, callConvention, types, modifiers);
        }

        /// <summary>
        /// When overridden in a derived class, searches for the constructors defined for the current <see cref="TypeSymbol"/>, using the specified <see cref="BindingFlags"/>.
        /// </summary>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public IEnumerable<MethodSymbol> GetConstructors() => GetConstructors(DefaultLookup);

        /// <summary>
        /// When overridden in a derived class, searches for the constructors defined for the current <see cref="TypeSymbol"/>, using the specified <see cref="BindingFlags"/>.
        /// </summary>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public IEnumerable<MethodSymbol> GetConstructors(BindingFlags bindingFlags)
        {
            return new MemberQuery<TypeSymbol, MethodSymbol>(this, ConstructorInfo.ConstructorName, bindingFlags);
        }

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
        public abstract ImmutableArray<string> GetEnumNames();

        /// <summary>
        /// Returns the underlying type of the current enumeration type.
        /// </summary>
        /// <returns></returns>
        public abstract TypeSymbol GetEnumUnderlyingType();

        /// <summary>
        /// Returns all the declared events of the current <see cref="TypeSymbol"/>.
        /// </summary>
        /// <returns></returns>
        internal abstract ImmutableArray<EventSymbol> GetDeclaredEvents();

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
        public EventSymbol? GetEvent(string name, BindingFlags bindingFlags)
        {
            return new MemberQuery<TypeSymbol, EventSymbol>(this, name, bindingFlags).Disambiguate();
        }

        /// <summary>
        /// Returns all the public events that are declared or inherited by the current <see cref="TypeSymbol"/>.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EventSymbol> GetEvents() => GetEvents(DefaultLookup);

        /// <summary>
        /// When overridden in a derived class, searches for events that are declared or inherited by the current <see cref="TypeSymbol"/>, using the specified binding constraints.
        /// </summary>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public IEnumerable<EventSymbol> GetEvents(BindingFlags bindingFlags)
        {
            return new MemberQuery<TypeSymbol, EventSymbol>(this, null, bindingFlags);
        }

        /// <summary>
        /// Returns all the declared fields of the current <see cref="TypeSymbol"/>.
        /// </summary>
        /// <returns></returns>
        internal abstract ImmutableArray<FieldSymbol> GetDeclaredFields();

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
        public FieldSymbol? GetField(string name, BindingFlags bindingFlags)
        {
            return new MemberQuery<TypeSymbol, FieldSymbol>(this, name, bindingFlags).Disambiguate();
        }

        /// <summary>
        /// Returns all the public fields of the current <see cref="TypeSymbol"/>.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FieldSymbol> GetFields() => GetFields(DefaultLookup);

        /// <summary>
        /// When overridden in a derived class, searches for the fields defined for the current <see cref="TypeSymbol"/>, using the specified binding constraints.
        /// </summary>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public IEnumerable<FieldSymbol> GetFields(BindingFlags bindingFlags)
        {
            return new MemberQuery<TypeSymbol, FieldSymbol>(this, null, bindingFlags);
        }

        /// <summary>
        /// Returns an array of <see cref="TypeSymbol"/> objects that represent the constraints on the current generic type parameter.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<TypeSymbol> GenericParameterConstraints { get; }

        /// <summary>
        /// Searches for the interface with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TypeSymbol? GetInterface(string name)
        {
            foreach (var i in GetInterfaces())
                if (i.Name == name)
                    return i;

            return null;
        }

        /// <summary>
        /// When overridden in a derived class, gets all the interfaces implemented by the current <see cref="TypeSymbol"/>.
        /// </summary>
        /// <returns></returns>
        public ImmutableArray<TypeSymbol> GetInterfaces()
        {
            static void AddInterfaces(ImmutableArray<TypeSymbol>.Builder b, HashSet<TypeSymbol> s, TypeSymbol type)
            {
                foreach (var iface in type.GetDeclaredInterfaces())
                {
                    if (s.Add(iface))
                    {
                        b.Add(iface);
                        AddInterfaces(b, s, iface);
                    }
                }
            }
                
            if (_interfaces.IsDefault)
            {
                var h = new HashSet<TypeSymbol>();
                var b = ImmutableArray.CreateBuilder<TypeSymbol>();
                for (var type = this; type != null; type = type.BaseType)
                    AddInterfaces(b, h, type);

                ImmutableInterlocked.InterlockedInitialize(ref _interfaces, b.DrainToImmutable());
            }

            return _interfaces;
        }

        /// <summary>
        /// Returns an interface mapping for the specified interface type.
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        public InterfaceMapping GetInterfaceMap(TypeSymbol interfaceType)
        {
            var interfaceMethods = interfaceType.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public).ToImmutableArray();
            var targetMethods = ImmutableArray.CreateBuilder<MethodSymbol?>(interfaceMethods.Length);

            // fill in array with blanks
            for (int i = 0; i < interfaceMethods.Length; i++)
                targetMethods.Add(null);

            FillInInterfaceMethods(interfaceType, interfaceMethods, targetMethods);
            return new InterfaceMapping(interfaceType, interfaceMethods, this, targetMethods.DrainToImmutable().CastArray<MethodSymbol>());
        }

        /// <summary>
        /// Populates <paramref name="targetMethods"/>.
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="interfaceMethods"></param>
        /// <param name="targetMethods"></param>
        void FillInInterfaceMethods(TypeSymbol interfaceType, ImmutableArray<MethodSymbol> interfaceMethods, ImmutableArray<MethodSymbol?>.Builder targetMethods)
        {
            FillInExplicitInterfaceMethods(interfaceMethods, targetMethods);

            var direct = IsDirectlyImplementedInterface(interfaceType);
            if (direct)
                FillInImplicitInterfaceMethods(interfaceMethods, targetMethods);

            var baseType = BaseType;
            if (baseType != null)
            {
                baseType.FillInInterfaceMethods(interfaceType, interfaceMethods, targetMethods);
                ReplaceOverriddenMethods(targetMethods);
            }

            if (direct)
                for (var type = BaseType; type != null && type.Module == Module; type = type.BaseType)
                    type.FillInImplicitInterfaceMethods(interfaceMethods, targetMethods);
        }

        /// <summary>
        /// This returns true if this type directly (i.e. not inherited from the base class) implements the interface.
        /// </summary>
        /// <remarks>
        /// Note that a complicating factor is that the interface itself can be implemented by an interface that extends it.
        /// </remarks>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        bool IsDirectlyImplementedInterface(TypeSymbol interfaceType)
        {
            foreach (var iface in GetDeclaredInterfaces())
                if (interfaceType.IsAssignableFrom(iface))
                    return true;

            return false;
        }

        /// <summary>
        /// Populates <paramref name="targetMethods"/> with the methods from this type which implicitely implement the items in <paramref name="interfaceMethods"/>.
        /// </summary>
        /// <param name="interfaceMethods"></param>
        /// <param name="targetMethods"></param>
        void FillInImplicitInterfaceMethods(ImmutableArray<MethodSymbol> interfaceMethods, ImmutableArray<MethodSymbol?>.Builder targetMethods)
        {
            ImmutableArray<MethodSymbol> methods = default;

            for (int i = 0; i < targetMethods.Count; i++)
            {
                if (targetMethods[i] == null)
                {
                    // only retrieve declared methods if we enter the loop
                    if (methods.IsDefault)
                        methods = GetDeclaredMethods();

                    for (int j = 0; j < methods.Length; j++)
                    {
                        if (methods[j].IsVirtual && methods[j].Name == interfaceMethods[i].Name && methods[j].Signature.Equals(interfaceMethods[i].Signature))
                        {
                            targetMethods[i] = methods[j];
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Replace items in <paramref name="baseMethods"/> which have an overridden implementation in this type.
        /// </summary>
        /// <param name="baseMethods"></param>
        void ReplaceOverriddenMethods(ImmutableArray<MethodSymbol?>.Builder baseMethods)
        {
            var impl = GetMethodImplementations();

            for (int i = 0; i < baseMethods.Count; i++)
            {
                ref readonly var baseMethod = ref baseMethods.ItemRef(i);

                if (baseMethod != null && baseMethod.IsFinal == false)
                {
                    var def = baseMethod.BaseDefinition ?? baseMethod;

                    for (int j = 0; j < impl.Declarations.Length; j++)
                    {
                        for (int k = 0; k < impl.Declarations[j].Length; k++)
                        {
                            if (impl.Declarations[j][k].BaseDefinition == def)
                            {
                                baseMethods[i] = impl.Implementations[j];
                                goto next;
                            }
                        }
                    }

                    var candidate = FindMethod(def.Name, def.Signature);
                    if (candidate != null && candidate.IsVirtual && !candidate.IsNewSlot)
                        baseMethods[i] = candidate;
                }
            next:;
            }
        }

        /// <summary>
        /// Finds the declared method with the given name and signature.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="signature"></param>
        /// <returns></returns>
        internal MethodSymbol? FindMethod(string name, MethodSymbolSignature signature)
        {
            foreach (var method in GetDeclaredMethods())
                if (method.Name == name && method.Signature.Equals(signature))
                    return method;

            return null;
        }

        /// <summary>
        /// Populates <paramref name="targetMethods"/> with the methods from this type which explicitely implement the items in <paramref name="interfaceMethods"/>.
        /// </summary>
        /// <param name="interfaceMethods"></param>
        /// <param name="targetMethods"></param>
        internal void FillInExplicitInterfaceMethods(ImmutableArray<MethodSymbol> interfaceMethods, ImmutableArray<MethodSymbol?>.Builder targetMethods)
        {
            var impl = GetMethodImplementations();

            for (int i = 0; i < impl.Declarations.Length; i++)
            {
                for (int j = 0; j < impl.Declarations[i].Length; j++)
                {
                    int index = interfaceMethods.IndexOf(impl.Declarations[i][j]);
                    if (index != -1 && targetMethods[index] == null)
                        targetMethods[index] = impl.Implementations[i];
                }
            }
        }

        /// <summary>
        /// Returns all the declared interfaces of the current <see cref="TypeSymbol"/>.
        /// </summary>
        /// <returns></returns>
        internal abstract ImmutableArray<TypeSymbol> GetDeclaredInterfaces();

        /// <summary>
        /// Returns all the declared methods of the current <see cref="TypeSymbol"/>.
        /// </summary>
        /// <returns></returns>
        internal abstract ImmutableArray<MethodSymbol> GetDeclaredMethods();

        /// <summary>
        /// Gets the explicit method implementation mapping.
        /// </summary>
        /// <returns></returns>
        internal abstract MethodImplementationMapping GetMethodImplementations();

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
        public MethodSymbol? GetMethod(string name, TypeSymbolSelectorList types) => GetMethod(name, DefaultLookup, types);

        /// <summary>
        /// Searches for the specified method, using the specified binding constraints.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public MethodSymbol? GetMethod(string name, BindingFlags bindingFlags) => GetMethod(name, bindingFlags, default);

        /// <summary>
        /// Searches for the specified method whose parameters match the specified argument types and modifiers, using the specified binding constraints.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bindingAttr"></param>
        /// <param name="types"></param>
        /// <param name="modifiers"></param>
        /// <returns></returns>
        public MethodSymbol? GetMethod(string name, BindingFlags bindingAttr, TypeSymbolSelectorList types, ImmutableArray<ParameterModifier> modifiers) => GetMethod(name, bindingAttr, CallingConventions.Any, types, modifiers);

        /// <summary>
        /// Searches for the specified method whose parameters match the specified argument types, using the specified binding constraints.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bindingFlags"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public MethodSymbol? GetMethod(string name, BindingFlags bindingFlags, TypeSymbolSelectorList types) => GetMethod(name, bindingFlags, CallingConventions.Any, types, default);

        /// <summary>
        /// Searches for the specified method whose parameters match the specified argument types and modifiers, using the specified binding constraints and the specified calling convention.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bindingFlags"></param>
        /// <param name="callConvention"></param>
        /// <param name="types"></param>
        /// <param name="modifiers"></param>
        /// <returns></returns>
        public MethodSymbol? GetMethod(string name, BindingFlags bindingFlags, CallingConventions callConvention, TypeSymbolSelectorList types, ImmutableArray<ParameterModifier> modifiers)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            return GetMethod(name, -1, bindingFlags, callConvention, types, modifiers);
        }

        /// <summary>
        /// Searches for the specified method whose parameters match the specified generic parameter count, argument types and modifiers, using the specified binding constraints.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="genericParameterCount"></param>
        /// <param name="bindingFlags"></param>
        /// <param name="types"></param>
        /// <param name="modifiers"></param>
        /// <returns></returns>
        public MethodSymbol? GetMethod(string name, int genericParameterCount, BindingFlags bindingFlags, TypeSymbolSelectorList types, ImmutableArray<ParameterModifier> modifiers)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (types.Indexes.IsDefault)
                throw new ArgumentNullException(nameof(types));

            return GetMethod(name, genericParameterCount, bindingFlags, CallingConventions.Any, types, modifiers);
        }

        /// <summary>
        /// Searches for the specified public method whose parameters match the specified generic parameter count, argument types and modifiers.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="genericParameterCount"></param>
        /// <param name="types"></param>
        /// <param name="modifiers"></param>
        /// <returns></returns>
        public MethodSymbol? GetMethod(string name, int genericParameterCount, TypeSymbolSelectorList types, ImmutableArray<ParameterModifier> modifiers)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (types.Indexes.IsDefault)
                throw new ArgumentNullException(nameof(types));

            return GetMethod(name, genericParameterCount, DefaultLookup, CallingConventions.Any, types, modifiers);
        }

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
        public MethodSymbol? GetMethod(string name, int genericParameterCount, BindingFlags bindingFlags, CallingConventions callConvention, TypeSymbolSelectorList types, ImmutableArray<ParameterModifier> modifiers)
        {
            Debug.Assert(name != null);

            if (types.Indexes.IsDefault)
            {
                Debug.Assert(genericParameterCount == -1);
                Debug.Assert(callConvention == CallingConventions.Any);
                Debug.Assert(modifiers == null);
                return new MemberQuery<TypeSymbol, MethodSymbol>(this, name, bindingFlags).Disambiguate();
            }
            else
            {
                var results = new MemberQuery<TypeSymbol, MethodSymbol>(this, name, bindingFlags);
                var methods = new List<MethodSymbol>();
                foreach (var method in results)
                {
                    // generic parameter account must match, if specified
                    if (genericParameterCount != -1 && genericParameterCount != method.GenericArguments.Length)
                        continue;

                    // does method qualify based on parameter count?
                    if (QualifiesBasedOnParameterCount(method, bindingFlags, callConvention, types) == false)
                        continue;

                    methods.Add(method);
                }

                if (methods.Count == 0)
                    return null;

                // fast track check, no types filtered, or types filtered, but nothing to select among
                if (types.Indexes.IsDefault == false && types.Indexes.Length == 0 && methods.Count == 1)
                    return methods[0];

                return Context.DefaultBinder.SelectMethod(methods, bindingFlags, types, modifiers);
            }
        }

        /// <summary>
        /// Returns <c>true</c> if the given method symbol would qualify as a candidate in a query for the specified binding flags and argument type count.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="bindingFlags"></param>
        /// <param name="callConvention"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        static bool QualifiesBasedOnParameterCount(MethodSymbol method, BindingFlags bindingFlags, CallingConventions callConvention, TypeSymbolSelectorList types)
        {
            if ((callConvention & CallingConventions.Any) == 0)
            {
                if ((callConvention & CallingConventions.VarArgs) != 0 && (method.CallingConvention & CallingConventions.VarArgs) == 0)
                    return false;

                if ((callConvention & CallingConventions.Standard) != 0 && (method.CallingConvention & CallingConventions.Standard) == 0)
                    return false;
            }

            // no selectors passed, so always qualifies
            if (types.Indexes.IsDefault)
                return true;

            // check that count of selectors match count of parameters
            var parameterInfos = method.Parameters;
            if (types.Indexes.Length != parameterInfos.Length)
                return false;

            // supported?
            if ((bindingFlags & BindingFlags.ExactBinding) != 0)
                throw new NotSupportedException("ExactBinding is not supported on the Symbols API, since ParameterSymbolSelectors can handle it.");

            return true;
        }

        /// <summary>
        /// Returns all the public methods of the current <see cref="TypeSymbol"/>.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MethodSymbol> GetMethods() => GetMethods(DefaultLookup);

        /// <summary>
        /// When overridden in a derived class, searches for the methods defined for the current <see cref="TypeSymbol"/>, using the specified binding constraints.
        /// </summary>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public IEnumerable<MethodSymbol> GetMethods(BindingFlags bindingFlags)
        {
            return new MemberQuery<TypeSymbol, MethodSymbol>(this, null, bindingFlags);
        }

        /// <summary>
        /// Returns all the declared nested types of the current <see cref="TypeSymbol"/>.
        /// </summary>
        /// <returns></returns>
        internal abstract ImmutableArray<TypeSymbol> GetDeclaredNestedTypes();

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
        public TypeSymbol? GetNestedType(string name, BindingFlags bindingFlags)
        {
            return new MemberQuery<TypeSymbol, TypeSymbol>(this, name, bindingFlags).Disambiguate();
        }

        /// <summary>
        /// Returns the public types nested in the current <see cref="TypeSymbol"/>.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TypeSymbol> GetNestedTypes() => GetNestedTypes(DefaultLookup);

        /// <summary>
        /// When overridden in a derived class, searches for the types nested in the current <see cref="TypeSymbol"/>, using the specified binding constraints.
        /// </summary>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public IEnumerable<TypeSymbol> GetNestedTypes(BindingFlags bindingFlags)
        {
            return new MemberQuery<TypeSymbol, TypeSymbol>(this, null, bindingFlags);
        }

        /// <summary>
        /// Returns all the declared properties of the current <see cref="TypeSymbol"/>.
        /// </summary>
        /// <returns></returns>
        internal abstract ImmutableArray<PropertySymbol> GetDeclaredProperties();

        /// <summary>
        /// Searches for the public property with the specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public PropertySymbol? GetProperty(string name) => GetProperty(name, DefaultLookup, null, default, default);

        /// <summary>
        /// Searches for the specified property, using the specified binding constraints.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public PropertySymbol? GetProperty(string name, BindingFlags bindingFlags) => GetProperty(name, bindingFlags, null, default, default);

        /// <summary>
        /// Searches for the specified public property whose parameters match the specified argument types.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public PropertySymbol? GetProperty(string name, TypeSymbolSelectorList types) => GetProperty(name, DefaultLookup, null, types, default);

        /// <summary>
        /// Searches for the public property with the specified name and return type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="propertyType"></param>
        /// <returns></returns>
        public PropertySymbol? GetProperty(string name, TypeSymbol? propertyType) => GetProperty(name, DefaultLookup, propertyType, default, default);

        /// <summary>
        /// Searches for the specified public property whose parameters match the specified argument types.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="propertyType"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public PropertySymbol? GetProperty(string name, TypeSymbol? propertyType, TypeSymbolSelectorList types) => GetProperty(name, DefaultLookup, propertyType, types, default);

        /// <summary>
        /// Searches for the specified public property whose parameters match the specified argument types.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="propertyType"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public PropertySymbol? GetProperty(string name, BindingFlags bindingFlags, TypeSymbol? propertyType, TypeSymbolSelectorList types, ImmutableArray<ParameterModifier> modifiers)
        {
            if (types.Indexes.IsDefault && propertyType == null)
            {
                // Group #1: This group of api accept only a name and BindingFlags. The other parameters are hard-wired by the non-virtual api entrypoints.
                return new MemberQuery<TypeSymbol, PropertySymbol>(this, name, bindingFlags).Disambiguate();
            }
            else
            {
                // Group #2: This group of api takes a set of parameter types, a return type (both cannot be null) and an optional binder.
                var query = new MemberQuery<TypeSymbol, PropertySymbol>(this, name, bindingFlags).ToArray();

                // compose candidates, removing mismatched parameters
                var candidates = new List<PropertySymbol>(query.Length);
                foreach (var candidate in query)
                    if (types.Indexes.IsDefault || (candidate.GetIndexParameters().Length == types.Indexes.Length))
                        candidates.Add(candidate);

                // no candidates were found
                if (candidates.Count == 0)
                    return null;

                // For perf and desktop compat, fast-path these specific checks before calling on the binder to break ties.
                if (types.Indexes.Length == 0)
                {
                    // no arguments
                    var firstCandidate = candidates[0];

                    if (candidates.Count == 1)
                    {
                        if (propertyType is not null && propertyType != firstCandidate.PropertyType)
                            return null;

                        return firstCandidate;
                    }
                    else
                    {
                        if (propertyType is null)
                        {
                            // if we are here we have no args or property type to select over and we have more than one property with that name
                            throw new AmbiguousMatchException($"Ambiguous match found for '{firstCandidate.DeclaringType} {firstCandidate}'.");
                        }
                    }
                }

                if ((bindingFlags & BindingFlags.ExactBinding) != 0)
                    throw new InvalidOperationException("ExactBinding is not supported on BindingFlags, as ParameterSymbolSelectors can be made exact.");

                return Context.DefaultBinder.SelectProperty(bindingFlags, candidates, propertyType, types, modifiers);
            }
        }

        /// <summary>
        /// Returns all the public properties of the current <see cref="TypeSymbol"/>.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PropertySymbol> GetProperties() => GetProperties(DefaultLookup);

        /// <summary>
        /// When overridden in a derived class, searches for the properties of the current <see cref="TypeSymbol"/>, using the specified binding constraints.
        /// </summary>
        /// <param name="bindingFlags"></param>
        /// <returns></returns>
        public IEnumerable<PropertySymbol> GetProperties(BindingFlags bindingFlags)
        {
            return new MemberQuery<TypeSymbol, PropertySymbol>(this, null, bindingFlags);
        }

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
                var constraints = GenericParameterConstraints;
                for (int i = 0; i < constraints.Length; i++)
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
                    for (int i = 0; i < interfaces.Length; i++)
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
        /// When overridden in a derived class, returns the optional custom modifiers of the current Type.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<TypeSymbol> GetOptionalCustomModifiers();

        /// <summary>
        /// When overridden in a derived class, returns the required custom modifiers of the current Type.
        /// </summary>
        /// <returns></returns>
        public abstract ImmutableArray<TypeSymbol> GetRequiredCustomModifiers();

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
        public TypeSymbol MakeGenericType(ImmutableArray<TypeSymbol> typeArguments) => _typeSpecTable.GetOrCreateGenericTypeSymbol(typeArguments);

        /// <summary>
        /// Returns a <see cref="TypeSymbol"/> object that represents a pointer to the current type.
        /// </summary>
        /// <returns></returns>
        public TypeSymbol MakePointerType() => _typeSpecTable.GetOrCreatePointerTypeSymbol();

        /// <inheritdoc />
        internal sealed override ICustomAttributeProviderInternal? GetInheritedCustomAttributeProvider() => BaseType;

        /// <summary>
        /// Specializes the current <see cref="TypeSymbol"/> given the specified generic arguments.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        internal abstract TypeSymbol Specialize(GenericContext context);

        /// <inheritdoc />
        public sealed override string? ToString() => _toString ??= TypeSymbolNameBuilder.ToString(this, TypeSymbolNameBuilder.Format.ToString);

        /// <summary>
        /// This is used by the ToString() overrides of all reflection types. The legacy behavior has the following problems:
        ///  1. Use only Name for nested types, which can be confused with global types and generic parameters of the same name.
        ///  2. Use only Name for generic parameters, which can be confused with nested types and global types of the same name.
        ///  3. Use only Name for all primitive types, void and TypedReference
        ///  4. MethodBase.ToString() use "ByRef" for byref parameters which is different than Type.ToString().
        ///  5. ConstructorInfo.ToString() outputs "Void" as the return type. Why Void?
        /// </summary>
        /// <returns></returns>
        internal string? FormatTypeName()
        {
            var elementType = GetRootElementType();
            if (elementType.IsPrimitive ||
                elementType.IsNested ||
                elementType == Context.ResolveCoreType("System.Void") ||
                elementType == Context.ResolveCoreType("System.TypedReference"))
                return Name;

            return ToString();
        }

        /// <summary>
        /// Gets the root element type.
        /// </summary>
        /// <returns></returns>
        internal TypeSymbol GetRootElementType()
        {
            var rootElementType = this;

            while (rootElementType.HasElementType)
                rootElementType = rootElementType.GetElementType()!;

            return rootElementType;
        }

    }

}
