/*
  Copyright (C) 2009-2015 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace IKVM.Reflection
{

    /// <summary>
    /// Represents type declarations: class types, interface types, array types, value types, enumeration types, type
    /// parameters, generic type definitions, and open or closed constructed generic types.
    /// </summary>
    abstract class Type : MemberInfo, IGenericContext, IGenericBinder
    {

        /// <summary>
        /// Represents an empty array of type <see cref="Type"/>.
        /// </summary>
        public static readonly Type[] EmptyTypes = Array.Empty<Type>();

        /// <summary>
        /// The type this instance stands for. It is the instance itself, except for wrappers such as
        /// <see cref="Emit.BakedType"/>, which forwards its identity to the <see cref="Emit.TypeBuilder"/> it was created from.
        /// </summary>
        protected readonly Type underlyingType;

        /// <summary>
        /// Cached and lazily computed facts about the type; see <see cref="TypeFlags"/>.
        /// </summary>
        protected TypeFlags typeFlags;

        // only used if (__IsBuiltIn || HasElementType || __IsFunctionPointer || IsGenericParameter)
        byte _sigElementType;

        /// <summary>
        /// Flags that cache facts about a type, some of which are computed lazily.
        /// </summary>
        [Flags]
        protected enum TypeFlags : ushort
        {
            // for use by TypeBuilder or TypeDefImpl
            IsGenericTypeDefinition = 1,

            // for use by TypeBuilder
            HasNestedTypes = 2,
            Baked = 4,

            // for use by IsValueType to cache result of IsValueTypeImpl
            ValueType = 8,
            NotValueType = 16,

            // for use by TypeDefImpl, TypeBuilder or MissingType
            PotentialEnumOrValueType = 32,
            EnumOrValueType = 64,

            // for use by TypeDefImpl
            NotGenericTypeDefinition = 128,

            // used to cache __ContainsMissingType
            ContainsMissingType_Unknown = 0,
            ContainsMissingType_Pending = 256,
            ContainsMissingType_Yes = 512,
            ContainsMissingType_No = 256 | 512,
            ContainsMissingType_Mask = 256 | 512,

            // built-in type support
            PotentialBuiltIn = 1024,
            BuiltIn = 2048,
        }

        /// <summary>
        /// Initializes a new instance that is its own underlying type.
        /// </summary>
        internal Type()
        {
            underlyingType = this;
        }

        /// <summary>
        /// Initializes a new instance that forwards its identity to another type and starts with that type's flags.
        /// </summary>
        /// <param name="underlyingType">The type this instance stands for; must be its own underlying type.</param>
        internal Type(Type underlyingType)
        {
            System.Diagnostics.Debug.Assert(underlyingType.underlyingType == underlyingType);
            this.underlyingType = underlyingType;
            typeFlags = underlyingType.typeFlags;
        }

        /// <summary>
        /// Initializes a new instance with the specified signature element type, for array, by-ref, pointer, function
        /// pointer and generic parameter types.
        /// </summary>
        /// <param name="sigElementType">The <c>ELEMENT_TYPE_*</c> value that encodes this kind of type in a signature.</param>
        internal Type(byte sigElementType) :
            this()
        {
            _sigElementType = sigElementType;
        }

        /// <summary>
        /// Gets a reference to the default binder, which implements internal rules for selecting the appropriate members
        /// to be called by reflection.
        /// </summary>
        public static Binder DefaultBinder => new DefaultBinder();

        /// <summary>
        /// Gets a <see cref="MemberTypes"/> value indicating that this member is a type or a nested type.
        /// </summary>
        public sealed override MemberTypes MemberType => IsNested ? MemberTypes.NestedType : MemberTypes.TypeInfo;

        /// <summary>
        /// Gets the assembly-qualified name of the type, which includes the name of the assembly from which this type
        /// was loaded. The assembly name is not escaped here, only when used in a generic type instantiation.
        /// </summary>
        public virtual string AssemblyQualifiedName => FullName + ", " + Assembly.FullName;

        /// <summary>
        /// Gets the type from which the current type directly inherits.
        /// </summary>
        public abstract Type BaseType
        {
            get;
        }

        /// <summary>
        /// Gets the attributes associated with the type.
        /// </summary>
        public abstract TypeAttributes Attributes
        {
            get;
        }

        /// <summary>
        /// When overridden in a derived class, returns the type of the object encompassed or referred to by the current
        /// array, pointer or reference type.
        /// </summary>
        /// <returns>The type of the object encompassed or referred to by the current array, pointer, or reference type, or <see langword="null"/> if the current type is not an array or a pointer, or is not passed by reference.</returns>
        public virtual Type GetElementType() => null;

        /// <summary>
        /// Throws if the type is still being built and cannot yet be queried. Does nothing by default.
        /// </summary>
        internal virtual void CheckBaked()
        {
        }

        /// <summary>
        /// Gets the nested types declared directly by this type, regardless of accessibility.
        /// </summary>
        /// <returns>The nested types declared by this type.</returns>
        public virtual Type[] __GetDeclaredTypes() => Type.EmptyTypes;

        /// <summary>
        /// Gets the interfaces listed in this type's own definition, not including those inherited from the base type
        /// or from other interfaces.
        /// </summary>
        /// <returns>The interfaces declared by this type.</returns>
        public virtual Type[] __GetDeclaredInterfaces() => Type.EmptyTypes;

        /// <summary>
        /// Gets the methods and constructors declared directly by this type, regardless of accessibility.
        /// </summary>
        /// <returns>The methods and constructors declared by this type.</returns>
        public virtual MethodBase[] __GetDeclaredMethods() => Array.Empty<MethodBase>();

        /// <summary>
        /// Gets the explicit method implementations (the MethodImpl table entries) of this type.
        /// </summary>
        /// <returns>The method bodies and the method declarations each of them implements.</returns>
        /// <exception cref="NotSupportedException">The type does not support method implementation maps.</exception>
        public virtual __MethodImplMap __GetMethodImplMap() => throw new NotSupportedException();

        /// <summary>
        /// Gets the fields declared directly by this type, regardless of accessibility.
        /// </summary>
        /// <returns>The fields declared by this type.</returns>
        public virtual FieldInfo[] __GetDeclaredFields() => Array.Empty<FieldInfo>();

        /// <summary>
        /// Gets the events declared directly by this type, regardless of accessibility.
        /// </summary>
        /// <returns>The events declared by this type.</returns>
        public virtual EventInfo[] __GetDeclaredEvents() => Array.Empty<EventInfo>();

        /// <summary>
        /// Gets the properties declared directly by this type, regardless of accessibility.
        /// </summary>
        /// <returns>The properties declared by this type.</returns>
        public virtual PropertyInfo[] __GetDeclaredProperties() => Array.Empty<PropertyInfo>();

        /// <summary>
        /// Gets the custom modifiers applied to this type where it appears in a signature.
        /// </summary>
        /// <returns>The custom modifiers; empty by default.</returns>
        public virtual CustomModifiers __GetCustomModifiers() => new CustomModifiers();

        /// <summary>
        /// Gets the signature of a function pointer type.
        /// </summary>
        /// <exception cref="InvalidOperationException">The type is not a function pointer type.</exception>
        public virtual __StandAloneMethodSig __MethodSignature => throw new InvalidOperationException();

        /// <summary>
        /// Gets a value indicating whether the current type encompasses or refers to another type; that is, whether the
        /// current type is an array, a pointer, or is passed by reference.
        /// </summary>
        public bool HasElementType => IsArray || IsByRef || IsPointer;

        /// <summary>
        /// Gets a value that indicates whether the type is an array.
        /// </summary>
        public bool IsArray => _sigElementType == Signature.ELEMENT_TYPE_ARRAY || _sigElementType == Signature.ELEMENT_TYPE_SZARRAY;

        /// <summary>
        /// Gets a value that indicates whether the type is an array type that can represent only a single-dimensional
        /// array with a zero lower bound.
        /// </summary>
        public bool IsSZArray => _sigElementType == Signature.ELEMENT_TYPE_SZARRAY;

        /// <summary>
        /// Gets a value indicating whether the type is passed by reference.
        /// </summary>
        public bool IsByRef => _sigElementType == Signature.ELEMENT_TYPE_BYREF;

        /// <summary>
        /// Gets a value indicating whether the type is a pointer.
        /// </summary>
        public bool IsPointer => _sigElementType == Signature.ELEMENT_TYPE_PTR;

        /// <summary>
        /// Gets a value that indicates whether the current type is a function pointer.
        /// </summary>
        public bool IsFunctionPointer => _sigElementType == Signature.ELEMENT_TYPE_FNPTR;

        /// <summary>
        /// Gets a value indicating whether the type is a value type. A value recorded from how a signature referred
        /// to the type takes precedence over <see cref="IsValueTypeImpl"/>.
        /// </summary>
        public bool IsValueType
        {
            get
            {
                // both flags are set when signatures disagree about the type, and the type itself decides
                return (typeFlags & (TypeFlags.ValueType | TypeFlags.NotValueType)) switch
                {
                    0 or TypeFlags.ValueType | TypeFlags.NotValueType => IsValueTypeImpl,
                    _ => (typeFlags & TypeFlags.ValueType) != 0,
                };
            }
        }

        /// <summary>
        /// Implements <see cref="IsValueType"/> and determines whether the type is a value type.
        /// </summary>
        protected abstract bool IsValueTypeImpl
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the current type represents a type parameter in the definition of a generic
        /// type or method.
        /// </summary>
        public bool IsGenericParameter => _sigElementType == Signature.ELEMENT_TYPE_VAR || _sigElementType == Signature.ELEMENT_TYPE_MVAR;

        /// <summary>
        /// Gets the position of the type parameter in the type parameter list of the generic type or method that
        /// declared the parameter.
        /// </summary>
        /// <exception cref="NotSupportedException">The current type does not represent a type parameter.</exception>
        public virtual int GenericParameterPosition => throw new NotSupportedException();

        /// <summary>
        /// Gets a <see cref="MethodBase"/> that represents the declaring method, if the current type represents a
        /// type parameter of a generic method.
        /// </summary>
        public virtual MethodBase DeclaringMethod => null;

        /// <summary>
        /// Indicates the type provided by the common language runtime that represents this type.
        /// </summary>
        public Type UnderlyingSystemType => underlyingType;

        /// <summary>
        /// Gets the type that declares the current nested type or generic type parameter.
        /// </summary>
        public override Type DeclaringType => null;

        /// <summary>
        /// Gets the unescaped namespace and name of the type.
        /// </summary>
        /// <exception cref="InvalidOperationException">The type has no name of its own, such as an array or constructed generic type.</exception>
        internal virtual TypeName TypeName => throw new InvalidOperationException();

        /// <summary>
        /// Gets the unescaped name of the type.
        /// </summary>
        public string __Name => TypeName.Name;

        /// <summary>
        /// Gets the unescaped namespace of the type as recorded in metadata. Unlike <see cref="Namespace"/>, for a
        /// nested type this is the type's own namespace, which is normally <see langword="null"/>.
        /// </summary>
        public string __Namespace => TypeName.Namespace;

        /// <summary>
        /// Gets the name of the current type.
        /// </summary>
        public abstract override string Name
        {
            get;
        }

        /// <summary>
        /// Gets the namespace of the type. For a nested type, this is the namespace of the outermost declaring type.
        /// </summary>
        public virtual string Namespace
        {
            get
            {
                if (IsNested)
                    return DeclaringType.Namespace;

                return __Namespace;
            }
        }

        /// <summary>
        /// Gets the token by which a <see cref="Emit.ModuleBuilder"/> refers to this type.
        /// </summary>
        /// <returns>The metadata token.</returns>
        /// <exception cref="InvalidOperationException">The type is not a type being built.</exception>
        internal virtual int GetModuleBuilderToken() => throw new InvalidOperationException();

        /// <summary>
        /// Indicates whether two <see cref="Type"/> objects are equal, by comparing their underlying types.
        /// </summary>
        /// <param name="t1">The first object to compare.</param>
        /// <param name="t2">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="t1"/> is equal to <paramref name="t2"/>; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(Type t1, Type t2)
        {
            // Casting to object results in smaller code than calling ReferenceEquals and makes
            // this method more likely to be inlined.
            // On CLR v2 x86, microbenchmarks show this to be faster than calling ReferenceEquals.
            return (object)t1 == (object)t2
                || (t1 is not null && t2 is not null && (object)t1.underlyingType == (object)t2.underlyingType);
        }

        /// <summary>
        /// Indicates whether two <see cref="Type"/> objects are not equal.
        /// </summary>
        /// <param name="t1">The first object to compare.</param>
        /// <param name="t2">The second object to compare.</param>
        /// <returns><see langword="true"/> if <paramref name="t1"/> is not equal to <paramref name="t2"/>; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(Type t1, Type t2) => !(t1 == t2);

        /// <summary>
        /// Determines if the underlying system type of the current type is the same as the underlying system type of
        /// the specified type.
        /// </summary>
        /// <param name="type">The object whose underlying system type is to be compared with the underlying system type of the current type.</param>
        /// <returns><see langword="true"/> if the underlying system type of <paramref name="type"/> is the same as the underlying system type of the current type; otherwise, <see langword="false"/>.</returns>
        public bool Equals(Type type) => this == type;

        /// <summary>
        /// Determines if the underlying system type of the current type is the same as the underlying system type of
        /// the specified object.
        /// </summary>
        /// <param name="obj">The object whose underlying system type is to be compared with the underlying system type of the current type.</param>
        /// <returns><see langword="true"/> if <paramref name="obj"/> is a <see cref="Type"/> with the same underlying system type; otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object obj) => Equals(obj as Type);

        /// <summary>
        /// Returns the hash code for this instance, which is that of the underlying system type.
        /// </summary>
        /// <returns>The hash code for this instance.</returns>
        public override int GetHashCode()
        {
            Type type = UnderlyingSystemType;
            return ReferenceEquals(type, this) ? base.GetHashCode() : type.GetHashCode();
        }

        /// <summary>
        /// Gets an array of the generic type arguments for this type.
        /// </summary>
        public Type[] GenericTypeArguments => IsConstructedGenericType ? GetGenericArguments() : Type.EmptyTypes;

        /// <summary>
        /// Returns an array of <see cref="Type"/> objects that represent the type arguments of a closed generic type or
        /// the type parameters of a generic type definition.
        /// </summary>
        /// <returns>An array of <see cref="Type"/> objects that represent the type arguments of a generic type, or an empty array if the current type is not a generic type.</returns>
        public virtual Type[] GetGenericArguments() => Type.EmptyTypes;

        /// <summary>
        /// Gets the custom modifiers applied to each type argument of a constructed generic type.
        /// </summary>
        /// <returns>The custom modifiers of each type argument, or an empty array if the type is not generic.</returns>
        public virtual CustomModifiers[] __GetGenericArgumentsCustomModifiers() => Array.Empty<CustomModifiers>();

        /// <summary>
        /// Returns a <see cref="Type"/> object that represents a generic type definition from which the current
        /// generic type can be constructed.
        /// </summary>
        /// <returns>A <see cref="Type"/> object representing a generic type from which the current type can be constructed.</returns>
        /// <exception cref="InvalidOperationException">The current type is not a generic type.</exception>
        public virtual Type GetGenericTypeDefinition() => throw new InvalidOperationException();

        /// <summary>
        /// Gets a value indicating whether the current type is a generic type.
        /// </summary>
        public virtual bool IsGenericType => false;

        /// <summary>
        /// Gets a value indicating whether the current <see cref="Type"/> represents a generic type definition, from
        /// which other generic types can be constructed.
        /// </summary>
        public virtual bool IsGenericTypeDefinition => false;

        /// <summary>
        /// Gets a value that indicates whether this object represents a constructed generic type.
        /// </summary>
        public virtual bool IsConstructedGenericType => false;

        /// <summary>
        /// Gets a value indicating whether the current <see cref="Type"/> object has type parameters that have not been
        /// replaced by specific types.
        /// </summary>
        public virtual bool ContainsGenericParameters
        {
            get
            {
                if (IsGenericParameter)
                    return true;

                foreach (var arg in GetGenericArguments())
                    if (arg.ContainsGenericParameters)
                        return true;

                return false;
            }
        }

        /// <summary>
        /// Returns an array of <see cref="Type"/> objects that represent the constraints on the current generic type
        /// parameter.
        /// </summary>
        /// <returns>An array of <see cref="Type"/> objects that represent the constraints on the current generic type parameter.</returns>
        /// <exception cref="InvalidOperationException">The current type is not a generic type parameter.</exception>
        public virtual Type[] GetGenericParameterConstraints() => throw new InvalidOperationException();

        /// <summary>
        /// Gets a combination of <see cref="GenericParameterAttributes"/> flags that describe the covariance and
        /// special constraints of the current generic type parameter.
        /// </summary>
        /// <exception cref="InvalidOperationException">The current type is not a generic type parameter.</exception>
        public virtual GenericParameterAttributes GenericParameterAttributes => throw new InvalidOperationException();

        /// <summary>
        /// Gets the number of dimensions in an array.
        /// </summary>
        /// <returns>An integer that contains the number of dimensions in the current type.</returns>
        /// <exception cref="NotSupportedException">The current type is not an array.</exception>
        public virtual int GetArrayRank() => throw new NotSupportedException();

        /// <summary>
        /// Gets the declared sizes of the dimensions of an array type, as encoded in its signature.
        /// </summary>
        /// <returns>The sizes of the dimensions that have one.</returns>
        /// <exception cref="NotSupportedException">The current type is not an array.</exception>
        public virtual int[] __GetArraySizes() => throw new NotSupportedException();

        /// <summary>
        /// Gets the declared lower bounds of the dimensions of an array type, as encoded in its signature.
        /// </summary>
        /// <returns>The lower bounds of the dimensions that have one.</returns>
        /// <exception cref="NotSupportedException">The current type is not an array.</exception>
        public virtual int[] __GetArrayLowerBounds() => throw new NotSupportedException();

        /// <summary>
        /// Returns the underlying type of the current enumeration type.
        /// </summary>
        /// <returns>The underlying type of the current enumeration.</returns>
        /// <exception cref="ArgumentException">The current type is not an enumeration.</exception>
        public virtual Type GetEnumUnderlyingType()
        {
            if (!IsEnum)
                throw new ArgumentException();

            CheckBaked();
            return GetEnumUnderlyingTypeImpl();
        }

        /// <summary>
        /// Gets the underlying type of an enumeration from the type of its instance field, without checking that the
        /// type is an enumeration or is complete.
        /// </summary>
        /// <returns>The type of the first instance field.</returns>
        /// <exception cref="InvalidOperationException">The type has no instance field.</exception>
        internal Type GetEnumUnderlyingTypeImpl()
        {
            // the CLR assumes that an enum has only one instance field, so we can do the same
            foreach (var field in __GetDeclaredFields())
                if (!field.IsStatic)
                    return field.FieldType;

            throw new InvalidOperationException();
        }

        /// <summary>
        /// Returns the names of the members of the current enumeration type.
        /// </summary>
        /// <returns>An array that contains the names of the members of the enumeration.</returns>
        /// <exception cref="ArgumentException">The current type is not an enumeration.</exception>
        public string[] GetEnumNames()
        {
            if (!IsEnum)
                throw new ArgumentException();

            var names = new List<string>();
            foreach (var field in __GetDeclaredFields())
                if (field.IsLiteral)
                    names.Add(field.Name);

            return names.ToArray();
        }

        /// <summary>
        /// Returns the name of the constant that has the specified value, for the current enumeration type.
        /// </summary>
        /// <param name="value">The value whose name is to be retrieved.</param>
        /// <returns>The name of the member of the current enumeration type that has the specified value, or <see langword="null"/> if no such constant is found.</returns>
        /// <exception cref="ArgumentException">The current type is not an enumeration, or <paramref name="value"/> cannot be converted to the underlying type.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        public string GetEnumName(object value)
        {
            if (!IsEnum)
                throw new ArgumentException();
            if (value == null)
                throw new ArgumentNullException();

            try
            {
                value = Convert.ChangeType(value, __GetSystemType(GetTypeCode(GetEnumUnderlyingType())));
            }
            catch (FormatException)
            {
                throw new ArgumentException();
            }
            catch (OverflowException)
            {
                return null;
            }
            catch (InvalidCastException)
            {
                return null;
            }

            foreach (var field in __GetDeclaredFields())
                if (field.IsLiteral && field.GetRawConstantValue().Equals(value))
                    return field.Name;

            return null;
        }

        /// <summary>
        /// Returns a value that indicates whether the specified value exists in the current enumeration type.
        /// </summary>
        /// <param name="value">The value to be tested; either a name or a value of the underlying type.</param>
        /// <returns><see langword="true"/> if the specified value is a member of the current enumeration type; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="ArgumentException">The current type is not an enumeration, or <paramref name="value"/> is not of the underlying type.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
        public bool IsEnumDefined(object value)
        {
            if (value is string)
                return Array.IndexOf(GetEnumNames(), value) != -1;
            if (!IsEnum)
                throw new ArgumentException();
            if (value == null)
                throw new ArgumentNullException();
            if (value.GetType() != __GetSystemType(GetTypeCode(GetEnumUnderlyingType())))
                throw new ArgumentException();

            foreach (var field in __GetDeclaredFields())
                if (field.IsLiteral && field.GetRawConstantValue().Equals(value))
                    return true;

            return false;
        }

        /// <summary>
        /// Returns a string representing the name of the current type.
        /// </summary>
        /// <returns>The full name of the current type.</returns>
        public override string ToString() => FullName;

        /// <summary>
        /// Gets the fully qualified name of the type, including its namespace but not its assembly.
        /// </summary>
        public abstract string FullName
        {
            get;
        }

        /// <summary>
        /// Builds the full name of a type that has a name of its own, from its escaped namespace and name and the full
        /// name of its declaring type.
        /// </summary>
        /// <returns>The full name of the type.</returns>
        protected string GetFullName()
        {
            var ns = TypeNameParser.Escape(__Namespace);
            var decl = DeclaringType;
            if (decl == null)
            {
                if (ns == null)
                    return Name;
                else
                    return ns + "." + Name;
            }
            else
            {
                if (ns == null)
                    return decl.FullName + "+" + Name;
                else
                    return decl.FullName + "+" + ns + "." + Name;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the type is the <c>&lt;Module&gt;</c> type that holds a module's global
        /// fields and methods.
        /// </summary>
        internal virtual bool IsModulePseudoType => false;

        /// <summary>
        /// Gets the type argument or type parameter at the specified position of a generic type.
        /// </summary>
        /// <param name="index">The zero-based position.</param>
        /// <returns>The type argument at <paramref name="index"/>.</returns>
        /// <exception cref="InvalidOperationException">The type is not generic.</exception>
        internal virtual Type GetGenericTypeArgument(int index) => throw new InvalidOperationException();

        /// <summary>
        /// Searches for the members defined for the current type whose <see cref="System.Reflection.DefaultMemberAttribute"/>
        /// is set.
        /// </summary>
        /// <returns>An array of <see cref="MemberInfo"/> objects representing all default members of the current type, or an empty array if the current type does not have default members.</returns>
        public MemberInfo[] GetDefaultMembers()
        {
            var defaultMemberAttribute = Module.Universe.Import(typeof(System.Reflection.DefaultMemberAttribute));
            foreach (var cad in CustomAttributeData.GetCustomAttributes(this))
                if (cad.Constructor.DeclaringType.Equals(defaultMemberAttribute))
                    return GetMember((string)cad.ConstructorArguments[0].Value);

            return Array.Empty<MemberInfo>();
        }

        /// <summary>
        /// Searches for the public members with the specified name.
        /// </summary>
        /// <param name="name">The string containing the name of the public members to get.</param>
        /// <returns>An array of <see cref="MemberInfo"/> objects representing the public members with the specified name, if found; otherwise, an empty array.</returns>
        public MemberInfo[] GetMember(string name) => GetMember(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

        /// <summary>
        /// Searches for the specified members, using the specified binding constraints.
        /// </summary>
        /// <param name="name">The string containing the name of the members to get.</param>
        /// <param name="bindingAttr">A bitwise combination of the enumeration values that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="MemberInfo"/> objects representing the members with the specified name, if found; otherwise, an empty array.</returns>
        public MemberInfo[] GetMember(string name, BindingFlags bindingAttr) => GetMember(name, MemberTypes.All, bindingAttr);

        /// <summary>
        /// Returns all the public members of the current type.
        /// </summary>
        /// <returns>An array of <see cref="MemberInfo"/> objects representing all the public members of the current type.</returns>
        public MemberInfo[] GetMembers() => GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

        /// <summary>
        /// Searches for the members defined for the current type, using the specified binding constraints.
        /// </summary>
        /// <param name="bindingAttr">A bitwise combination of the enumeration values that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="MemberInfo"/> objects representing all members defined for the current type that match the specified binding constraints.</returns>
        public MemberInfo[] GetMembers(BindingFlags bindingAttr)
        {
            var members = new List<MemberInfo>();
            members.AddRange(GetConstructors(bindingAttr));
            members.AddRange(GetMethods(bindingAttr));
            members.AddRange(GetFields(bindingAttr));
            members.AddRange(GetProperties(bindingAttr));
            members.AddRange(GetEvents(bindingAttr));
            members.AddRange(GetNestedTypes(bindingAttr));
            return members.ToArray();
        }

        /// <summary>
        /// Searches for the specified members of the specified member type, using the specified binding constraints.
        /// </summary>
        /// <param name="name">The string containing the name of the members to get.</param>
        /// <param name="type">The value to search for.</param>
        /// <param name="bindingAttr">A bitwise combination of the enumeration values that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="MemberInfo"/> objects representing the members with the specified name, if found; otherwise, an empty array.</returns>
        public MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
        {
            MemberFilter filter;
            if ((bindingAttr & BindingFlags.IgnoreCase) != 0)
            {
                name = name.ToLowerInvariant();
                filter = delegate (MemberInfo member, object filterCriteria)
                { return member.Name.ToLowerInvariant().Equals(filterCriteria); };
            }
            else
            {
                filter = delegate (MemberInfo member, object filterCriteria)
                { return member.Name.Equals(filterCriteria); };
            }

            return FindMembers(type, bindingAttr, filter, name);
        }

        /// <summary>
        /// Adds the members that pass a filter to a list.
        /// </summary>
        /// <param name="list">The list to add to.</param>
        /// <param name="filter">The filter, or <see langword="null"/> to add every member.</param>
        /// <param name="filterCriteria">The criteria passed to <paramref name="filter"/>.</param>
        /// <param name="members">The members to filter.</param>
        static void AddMembers(List<MemberInfo> list, MemberFilter filter, object filterCriteria, MemberInfo[] members)
        {
            foreach (var member in members)
                if (filter == null || filter(member, filterCriteria))
                    list.Add(member);
        }

        /// <summary>
        /// Returns a filtered array of <see cref="MemberInfo"/> objects of the specified member type.
        /// </summary>
        /// <param name="memberType">A bitwise combination of the enumeration values that indicates the type of member to search for.</param>
        /// <param name="bindingAttr">A bitwise combination of the enumeration values that specify how the search is conducted.</param>
        /// <param name="filter">The delegate that does the comparisons, returning <see langword="true"/> if the member currently being inspected matches the <paramref name="filterCriteria"/> and <see langword="false"/> otherwise.</param>
        /// <param name="filterCriteria">The search criteria that determines whether a member is returned in the array of <see cref="MemberInfo"/> objects.</param>
        /// <returns>A filtered array of <see cref="MemberInfo"/> objects of the specified member type, or an empty array if none match.</returns>
        public MemberInfo[] FindMembers(MemberTypes memberType, BindingFlags bindingAttr, MemberFilter filter, object filterCriteria)
        {
            var members = new List<MemberInfo>();
            if ((memberType & MemberTypes.Constructor) != 0)
                AddMembers(members, filter, filterCriteria, GetConstructors(bindingAttr));
            if ((memberType & MemberTypes.Method) != 0)
                AddMembers(members, filter, filterCriteria, GetMethods(bindingAttr));
            if ((memberType & MemberTypes.Field) != 0)
                AddMembers(members, filter, filterCriteria, GetFields(bindingAttr));
            if ((memberType & MemberTypes.Property) != 0)
                AddMembers(members, filter, filterCriteria, GetProperties(bindingAttr));
            if ((memberType & MemberTypes.Event) != 0)
                AddMembers(members, filter, filterCriteria, GetEvents(bindingAttr));
            if ((memberType & MemberTypes.NestedType) != 0)
                AddMembers(members, filter, filterCriteria, GetNestedTypes(bindingAttr));

            return members.ToArray();
        }

        /// <summary>
        /// Gets the members of this type itself that are of the kind <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The kind of member: <see cref="ConstructorInfo"/>, <see cref="MethodInfo"/>, <see cref="FieldInfo"/>, <see cref="PropertyInfo"/>, <see cref="EventInfo"/> or <see cref="Type"/>.</typeparam>
        /// <returns>The declared members of that kind. For constructors and methods, both are returned, and the caller filters by type.</returns>
        /// <exception cref="InvalidOperationException"><typeparamref name="T"/> is not a supported kind of member.</exception>
        MemberInfo[] GetMembers<T>()
        {
            if (typeof(T) == typeof(ConstructorInfo) || typeof(T) == typeof(MethodInfo))
                return __GetDeclaredMethods();
            else if (typeof(T) == typeof(FieldInfo))
                return __GetDeclaredFields();
            else if (typeof(T) == typeof(PropertyInfo))
                return __GetDeclaredProperties();
            else if (typeof(T) == typeof(EventInfo))
                return __GetDeclaredEvents();
            else if (typeof(T) == typeof(Type))
                return __GetDeclaredTypes();
            else
                throw new InvalidOperationException();
        }

        /// <summary>
        /// Gets the members of the kind <typeparamref name="T"/> that match the binding flags, including inherited
        /// members unless <see cref="BindingFlags.DeclaredOnly"/> is specified. Inherited members are returned with
        /// this type as their reflected type, and those hidden by a more derived member are left out.
        /// </summary>
        /// <typeparam name="T">The kind of member.</typeparam>
        /// <param name="flags">The binding flags that select the members.</param>
        /// <returns>The matching members.</returns>
        T[] GetMembers<T>(BindingFlags flags)
            where T : MemberInfo
        {
            CheckBaked();

            var list = new List<T>();
            foreach (var member in GetMembers<T>())
                if (member is T m && m.BindingFlagsMatch(flags))
                    list.Add(m);

            if ((flags & BindingFlags.DeclaredOnly) == 0)
            {
                for (var type = BaseType; type != null; type = type.BaseType)
                {
                    type.CheckBaked();

                    foreach (var member in type.GetMembers<T>())
                        if (member is T m && m.BindingFlagsMatchInherited(flags) && IsHiddenByDerived(m, list) == false)
                            list.Add((T)m.SetReflectedType(this));
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// Gets whether an inherited member is hidden by one already collected from a more derived type. Like
        /// System.Reflection, properties hide by name and signature, and events hide by name. Methods are handled by
        /// <see cref="GetMethods(BindingFlags)"/>, and fields and nested types are never hidden.
        /// </summary>
        /// <typeparam name="T">The kind of member.</typeparam>
        /// <param name="member">The inherited member to test.</param>
        /// <param name="derived">The members already collected from this type and more derived base types.</param>
        /// <returns><see langword="true"/> if <paramref name="member"/> is hidden; otherwise, <see langword="false"/>.</returns>
        static bool IsHiddenByDerived<T>(T member, List<T> derived)
            where T : MemberInfo
        {
            switch (member)
            {
                case PropertyInfo property:
                    foreach (var i in derived)
                        if (i.Name == property.Name && ((PropertyInfo)(MemberInfo)i).PropertySignature.Equals(property.PropertySignature))
                            return true;

                    return false;
                case EventInfo @event:
                    foreach (var i in derived)
                        if (i.Name == @event.Name)
                            return true;

                    return false;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Finds the single member of the kind <typeparamref name="T"/> with the specified name that matches the
        /// binding flags and the optional filter, searching base types unless <see cref="BindingFlags.DeclaredOnly"/>
        /// is specified. An inherited method with the same parameter types as one already found is treated as
        /// overridden rather than ambiguous.
        /// </summary>
        /// <typeparam name="T">The kind of member.</typeparam>
        /// <param name="name">The name of the member.</param>
        /// <param name="flags">The binding flags that select the members.</param>
        /// <param name="filter">An additional condition the member must satisfy, or <see langword="null"/>.</param>
        /// <returns>The member, or <see langword="null"/> if none is found.</returns>
        /// <exception cref="AmbiguousMatchException">More than one member matches.</exception>
        T GetMemberByName<T>(string name, BindingFlags flags, Predicate<T> filter)
            where T : MemberInfo
        {
            CheckBaked();

            if ((flags & BindingFlags.IgnoreCase) != 0)
                name = name.ToLowerInvariant();

            T found = null;

            foreach (var member in GetMembers<T>())
            {
                if (member is T m && m.BindingFlagsMatch(flags))
                {
                    var memberName = m.Name;
                    if ((flags & BindingFlags.IgnoreCase) != 0)
                        memberName = memberName.ToLowerInvariant();

                    if (memberName == name && (filter == null || filter(m)))
                    {
                        if (found != null)
                            throw new AmbiguousMatchException();

                        found = m;
                    }
                }
            }

            if ((flags & BindingFlags.DeclaredOnly) == 0)
            {
                for (var type = BaseType; (found == null || typeof(T) == typeof(MethodInfo)) && type != null; type = type.BaseType)
                {
                    type.CheckBaked();

                    foreach (var member in type.GetMembers<T>())
                    {
                        if (member is T m && m.BindingFlagsMatchInherited(flags))
                        {
                            var memberName = m.Name;

                            if ((flags & BindingFlags.IgnoreCase) != 0)
                                memberName = memberName.ToLowerInvariant();

                            if (memberName == name && (filter == null || filter(m)))
                            {
                                if (found != null)
                                {
                                    // TODO does this depend on HideBySig vs HideByName?
                                    if (found is MethodInfo mi && mi.MethodSignature.MatchParameterTypes(((MethodBase)member).MethodSignature))
                                        continue;

                                    throw new AmbiguousMatchException();
                                }

                                found = (T)member.SetReflectedType(this);
                            }
                        }
                    }
                }
            }

            return found;
        }

        /// <summary>
        /// Finds the single member of the kind <typeparamref name="T"/> with the specified name that matches the
        /// binding flags.
        /// </summary>
        /// <typeparam name="T">The kind of member.</typeparam>
        /// <param name="name">The name of the member.</param>
        /// <param name="flags">The binding flags that select the members.</param>
        /// <returns>The member, or <see langword="null"/> if none is found.</returns>
        /// <exception cref="AmbiguousMatchException">More than one member matches.</exception>
        T GetMemberByName<T>(string name, BindingFlags flags)
            where T : MemberInfo => GetMemberByName<T>(name, flags, null);

        /// <summary>
        /// Returns the <see cref="EventInfo"/> object representing the specified public event.
        /// </summary>
        /// <param name="name">The string containing the name of an event that is declared or inherited by the current type.</param>
        /// <returns>The object representing the specified public event that is declared or inherited by the current type, if found; otherwise, <see langword="null"/>.</returns>
        public EventInfo GetEvent(string name) => GetEvent(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

        /// <summary>
        /// Returns the <see cref="EventInfo"/> object representing the specified event, using the specified binding
        /// constraints.
        /// </summary>
        /// <param name="name">The string containing the name of an event which is declared or inherited by the current type.</param>
        /// <param name="bindingAttr">A bitwise combination of the enumeration values that specify how the search is conducted.</param>
        /// <returns>The object representing the specified event that is declared or inherited by the current type, if found; otherwise, <see langword="null"/>.</returns>
        public EventInfo GetEvent(string name, BindingFlags bindingAttr) => GetMemberByName<EventInfo>(name, bindingAttr);

        /// <summary>
        /// Returns all the public events that are declared or inherited by the current type.
        /// </summary>
        /// <returns>An array of <see cref="EventInfo"/> objects representing all the public events which are declared or inherited by the current type.</returns>
        public EventInfo[] GetEvents() => GetEvents(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

        /// <summary>
        /// Searches for events that are declared or inherited by the current type, using the specified binding
        /// constraints.
        /// </summary>
        /// <param name="bindingAttr">A bitwise combination of the enumeration values that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="EventInfo"/> objects representing all events that are declared or inherited by the current type that match the specified binding constraints.</returns>
        public EventInfo[] GetEvents(BindingFlags bindingAttr) => GetMembers<EventInfo>(bindingAttr);

        /// <summary>
        /// Searches for the public field with the specified name.
        /// </summary>
        /// <param name="name">The string containing the name of the data field to get.</param>
        /// <returns>An object representing the public field with the specified name, if found; otherwise, <see langword="null"/>.</returns>
        public FieldInfo GetField(string name) => GetField(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

        /// <summary>
        /// Searches for the specified field, using the specified binding constraints.
        /// </summary>
        /// <param name="name">The string containing the name of the data field to get.</param>
        /// <param name="bindingAttr">A bitwise combination of the enumeration values that specify how the search is conducted.</param>
        /// <returns>An object representing the field that matches the specified requirements, if found; otherwise, <see langword="null"/>.</returns>
        public FieldInfo GetField(string name, BindingFlags bindingAttr) => GetMemberByName<FieldInfo>(name, bindingAttr);

        /// <summary>
        /// Returns all the public fields of the current type.
        /// </summary>
        /// <returns>An array of <see cref="FieldInfo"/> objects representing all the public fields defined for the current type.</returns>
        public FieldInfo[] GetFields() => GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

        /// <summary>
        /// Searches for the fields defined for the current type, using the specified binding constraints.
        /// </summary>
        /// <param name="bindingAttr">A bitwise combination of the enumeration values that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="FieldInfo"/> objects representing all fields defined for the current type that match the specified binding constraints.</returns>
        public FieldInfo[] GetFields(BindingFlags bindingAttr) => GetMembers<FieldInfo>(bindingAttr);

        /// <summary>
        /// Gets all the interfaces implemented or inherited by the current type.
        /// </summary>
        /// <returns>An array of <see cref="Type"/> objects representing all the interfaces implemented or inherited by the current type, or an empty array if none are.</returns>
        public Type[] GetInterfaces()
        {
            var list = new List<Type>();
            for (var type = this; type != null; type = type.BaseType)
                AddInterfaces(list, type);

            return list.ToArray();
        }

        /// <summary>
        /// Adds the interfaces declared by a type, and the interfaces they extend, to a list without duplicates.
        /// </summary>
        /// <param name="list">The list to add to.</param>
        /// <param name="type">The type whose declared interfaces to add.</param>
        static void AddInterfaces(List<Type> list, Type type)
        {
            foreach (Type iface in type.__GetDeclaredInterfaces())
            {
                if (!list.Contains(iface))
                {
                    list.Add(iface);
                    AddInterfaces(list, iface);
                }
            }
        }

        /// <summary>
        /// Searches for the methods defined for the current type, using the specified binding constraints. Inherited
        /// virtual methods that are overridden by a more derived method are left out.
        /// </summary>
        /// <param name="bindingAttr">A bitwise combination of the enumeration values that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="MethodInfo"/> objects representing all methods defined for the current type that match the specified binding constraints.</returns>
        public MethodInfo[] GetMethods(BindingFlags bindingAttr)
        {
            CheckBaked();

            var list = new List<MethodInfo>();

            foreach (var mb in __GetDeclaredMethods())
            {
                var mi = mb as MethodInfo;
                if (mi != null && mi.BindingFlagsMatch(bindingAttr))
                    list.Add(mi);
            }

            if ((bindingAttr & BindingFlags.DeclaredOnly) == 0)
            {
                var baseMethods = new List<MethodInfo>();
                foreach (var mi in list)
                    if (mi.IsVirtual)
                        baseMethods.Add(mi.GetBaseDefinition());

                for (var type = BaseType; type != null; type = type.BaseType)
                {
                    type.CheckBaked();

                    foreach (var mb in type.__GetDeclaredMethods())
                    {
                        var mi = mb as MethodInfo;
                        if (mi != null && mi.BindingFlagsMatchInherited(bindingAttr))
                        {
                            if (mi.IsVirtual)
                            {
                                if (baseMethods.Contains(mi.GetBaseDefinition()))
                                    continue;

                                baseMethods.Add(mi.GetBaseDefinition());
                            }

                            list.Add((MethodInfo)mi.SetReflectedType(this));
                        }
                    }
                }
            }

            return list.ToArray();
        }

        /// <summary>
        /// Returns all the public methods of the current type.
        /// </summary>
        /// <returns>An array of <see cref="MethodInfo"/> objects representing all the public methods defined for the current type.</returns>
        public MethodInfo[] GetMethods() => GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

        /// <summary>
        /// Searches for the public method with the specified name.
        /// </summary>
        /// <param name="name">The string containing the name of the public method to get.</param>
        /// <returns>An object that represents the public method with the specified name, if found; otherwise, <see langword="null"/>.</returns>
        /// <exception cref="AmbiguousMatchException">More than one method is found with the specified name.</exception>
        public MethodInfo GetMethod(string name) => GetMethod(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

        /// <summary>
        /// Searches for the specified method, using the specified binding constraints.
        /// </summary>
        /// <param name="name">The string containing the name of the method to get.</param>
        /// <param name="bindingAttr">A bitwise combination of the enumeration values that specify how the search is conducted.</param>
        /// <returns>An object representing the method that matches the specified requirements, if found; otherwise, <see langword="null"/>.</returns>
        /// <exception cref="AmbiguousMatchException">More than one method is found with the specified name and matching the specified binding constraints.</exception>
        public MethodInfo GetMethod(string name, BindingFlags bindingAttr) => GetMemberByName<MethodInfo>(name, bindingAttr);

        /// <summary>
        /// Searches for the specified public method whose parameters match the specified argument types.
        /// </summary>
        /// <param name="name">The string containing the name of the public method to get.</param>
        /// <param name="types">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the method to get.</param>
        /// <returns>An object representing the public method whose parameters match the specified argument types, if found; otherwise, <see langword="null"/>.</returns>
        public MethodInfo GetMethod(string name, Type[] types) => GetMethod(name, types, null);

        /// <summary>
        /// Searches for the specified public method whose parameters match the specified argument types and
        /// modifiers.
        /// </summary>
        /// <param name="name">The string containing the name of the public method to get.</param>
        /// <param name="types">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the method to get.</param>
        /// <param name="modifiers">An array of <see cref="ParameterModifier"/> objects representing the attributes associated with the corresponding element in the <paramref name="types"/> array.</param>
        /// <returns>An object representing the public method that matches the specified requirements, if found; otherwise, <see langword="null"/>.</returns>
        public MethodInfo GetMethod(string name, Type[] types, ParameterModifier[] modifiers) =>
            GetMethod(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, types, modifiers);

        /// <summary>
        /// Searches for the specified method whose parameters match the specified argument types and modifiers, using
        /// the specified binding constraints. A method whose parameter types match exactly is preferred; otherwise
        /// the binder chooses among the methods with the specified name.
        /// </summary>
        /// <param name="name">The string containing the name of the method to get.</param>
        /// <param name="bindingAttr">A bitwise combination of the enumeration values that specify how the search is conducted.</param>
        /// <param name="binder">An object that defines a set of properties and enables binding, or <see langword="null"/> to use the <see cref="DefaultBinder"/>.</param>
        /// <param name="types">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the method to get.</param>
        /// <param name="modifiers">An array of <see cref="ParameterModifier"/> objects representing the attributes associated with the corresponding element in the <paramref name="types"/> array.</param>
        /// <returns>An object representing the method that matches the specified requirements, if found; otherwise, <see langword="null"/>.</returns>
        public MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers) =>
            // first we try an exact match and only if that fails we fall back to using the binder
            GetMemberByName(name, bindingAttr, (MethodInfo m) => m.MethodSignature.MatchParameterTypes(types)) ?? GetMethodWithBinder<MethodInfo>(name, bindingAttr, binder ?? DefaultBinder, types, modifiers);

        /// <summary>
        /// Collects every method or constructor with the specified name that matches the binding flags and lets the
        /// binder select one.
        /// </summary>
        /// <typeparam name="T">The kind of method.</typeparam>
        /// <param name="name">The name of the method.</param>
        /// <param name="bindingAttr">The binding flags that select the candidates.</param>
        /// <param name="binder">The binder that selects among the candidates.</param>
        /// <param name="types">The argument types to match.</param>
        /// <param name="modifiers">The parameter modifiers, or <see langword="null"/>.</param>
        /// <returns>The method selected by the binder, or <see langword="null"/> if none matches.</returns>
        T GetMethodWithBinder<T>(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
            where T : MethodBase
        {
            var list = new List<MethodBase>();

            GetMemberByName(name, bindingAttr, delegate (T method)
            {
                list.Add(method);
                return false;
            });

            return (T)binder.SelectMethod(bindingAttr, list.ToArray(), types, modifiers);
        }

        /// <summary>
        /// Searches for the specified method whose parameters match the specified argument types and modifiers, using
        /// the specified binding constraints and the specified calling convention.
        /// </summary>
        /// <param name="name">The string containing the name of the method to get.</param>
        /// <param name="bindingAttr">A bitwise combination of the enumeration values that specify how the search is conducted.</param>
        /// <param name="binder">An object that defines a set of properties and enables binding, or <see langword="null"/> to use the <see cref="DefaultBinder"/>.</param>
        /// <param name="callConvention">The calling convention. Ignored, as it is by .NET.</param>
        /// <param name="types">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the method to get.</param>
        /// <param name="modifiers">An array of <see cref="ParameterModifier"/> objects representing the attributes associated with the corresponding element in the <paramref name="types"/> array.</param>
        /// <returns>An object representing the method that matches the specified requirements, if found; otherwise, <see langword="null"/>.</returns>
        public MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers) =>
            // FXBUG callConvention seems to be ignored
            GetMethod(name, bindingAttr, binder, types, modifiers);

        /// <summary>
        /// Returns all the public instance constructors defined for the current type.
        /// </summary>
        /// <returns>An array of <see cref="ConstructorInfo"/> objects representing all the public instance constructors defined for the current type.</returns>
        public ConstructorInfo[] GetConstructors() => GetConstructors(BindingFlags.Public | BindingFlags.Instance);

        /// <summary>
        /// Searches for the constructors defined for the current type, using the specified binding constraints.
        /// Constructors are never inherited.
        /// </summary>
        /// <param name="bindingAttr">A bitwise combination of the enumeration values that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="ConstructorInfo"/> objects representing all constructors defined for the current type that match the specified binding constraints.</returns>
        public ConstructorInfo[] GetConstructors(BindingFlags bindingAttr) => GetMembers<ConstructorInfo>(bindingAttr | BindingFlags.DeclaredOnly);

        /// <summary>
        /// Searches for a public instance constructor whose parameters match the types in the specified array.
        /// </summary>
        /// <param name="types">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the desired constructor.</param>
        /// <returns>An object representing the public instance constructor whose parameters match the types in the parameter type array, if found; otherwise, <see langword="null"/>.</returns>
        public ConstructorInfo GetConstructor(Type[] types) =>
            GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, CallingConventions.Standard, types, null);

        /// <summary>
        /// Searches for a constructor whose parameters match the specified argument types and modifiers, using the
        /// specified binding constraints. Instance constructors are searched when <see cref="BindingFlags.Instance"/>
        /// is specified and the type initializer when <see cref="BindingFlags.Static"/> is specified.
        /// </summary>
        /// <param name="bindingAttr">A bitwise combination of the enumeration values that specify how the search is conducted.</param>
        /// <param name="binder">An object that defines a set of properties and enables binding, or <see langword="null"/> to use the <see cref="DefaultBinder"/>.</param>
        /// <param name="types">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the constructor to get.</param>
        /// <param name="modifiers">An array of <see cref="ParameterModifier"/> objects representing the attributes associated with the corresponding element in the parameter type array.</param>
        /// <returns>A <see cref="ConstructorInfo"/> object representing the constructor that matches the specified requirements, if found; otherwise, <see langword="null"/>.</returns>
        /// <exception cref="AmbiguousMatchException">Both an instance constructor and the type initializer match.</exception>
        public ConstructorInfo GetConstructor(BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
        {
            ConstructorInfo ci1 = null;
            if ((bindingAttr & BindingFlags.Instance) != 0)
                ci1 = GetConstructorImpl(ConstructorInfo.ConstructorName, bindingAttr, binder, types, modifiers);

            if ((bindingAttr & BindingFlags.Static) != 0)
            {
                var ci2 = GetConstructorImpl(ConstructorInfo.TypeConstructorName, bindingAttr, binder, types, modifiers);
                if (ci2 != null)
                {
                    if (ci1 != null)
                        throw new AmbiguousMatchException();

                    return ci2;
                }
            }

            return ci1;
        }

        /// <summary>
        /// Searches for a constructor with the specified name, first for an exact parameter type match and then
        /// through the binder.
        /// </summary>
        /// <param name="name"><c>.ctor</c> for an instance constructor or <c>.cctor</c> for the type initializer.</param>
        /// <param name="bindingAttr">The binding flags that select the candidates.</param>
        /// <param name="binder">The binder, or <see langword="null"/> to use the <see cref="DefaultBinder"/>.</param>
        /// <param name="types">The parameter types to match.</param>
        /// <param name="modifiers">The parameter modifiers, or <see langword="null"/>.</param>
        /// <returns>The constructor, or <see langword="null"/> if none matches.</returns>
        ConstructorInfo GetConstructorImpl(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
        {
            // first we try an exact match and only if that fails we fall back to using the binder
            return GetMemberByName<ConstructorInfo>(name, bindingAttr | BindingFlags.DeclaredOnly,
                delegate (ConstructorInfo ctor)
                { return ctor.MethodSignature.MatchParameterTypes(types); })
                ?? GetMethodWithBinder<ConstructorInfo>(name, bindingAttr | BindingFlags.DeclaredOnly, binder ?? DefaultBinder, types, modifiers);
        }

        /// <summary>
        /// Searches for a constructor whose parameters match the specified argument types and modifiers, using the
        /// specified binding constraints and the specified calling convention.
        /// </summary>
        /// <param name="bindingAttr">A bitwise combination of the enumeration values that specify how the search is conducted.</param>
        /// <param name="binder">An object that defines a set of properties and enables binding, or <see langword="null"/> to use the <see cref="DefaultBinder"/>.</param>
        /// <param name="callingConvention">The calling convention. Ignored, as it is by .NET.</param>
        /// <param name="types">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the constructor to get.</param>
        /// <param name="modifiers">An array of <see cref="ParameterModifier"/> objects representing the attributes associated with the corresponding element in the <paramref name="types"/> array.</param>
        /// <returns>An object representing the constructor that matches the specified requirements, if found; otherwise, <see langword="null"/>.</returns>
        public ConstructorInfo GetConstructor(BindingFlags bindingAttr, Binder binder, CallingConventions callingConvention, Type[] types, ParameterModifier[] modifiers) =>
            // FXBUG callConvention seems to be ignored
            GetConstructor(bindingAttr, binder, types, modifiers);

        /// <summary>
        /// Finds a nested type by namespace and name, or creates a missing type for it if it cannot be found and the
        /// universe allows missing members.
        /// </summary>
        /// <param name="requester">The module whose reference is being resolved, used for diagnostics.</param>
        /// <param name="typeName">The namespace and name of the nested type.</param>
        /// <returns>The nested type, or a missing type standing in for it.</returns>
        internal Type ResolveNestedType(Module requester, TypeName typeName) =>
            FindNestedType(typeName) ?? Module.Universe.GetMissingTypeOrThrow(requester, Module, this, typeName);

        /// <summary>
        /// Finds a nested type by namespace and name. Unlike the public API, this takes the namespace into account.
        /// </summary>
        /// <param name="name">The namespace and name of the nested type.</param>
        /// <returns>The nested type, or <see langword="null"/> if none is found.</returns>
        internal virtual Type FindNestedType(TypeName name)
        {
            foreach (var type in __GetDeclaredTypes())
                if (type.TypeName == name)
                    return type;

            return null;
        }

        /// <summary>
        /// Finds a nested type by namespace and name, ignoring case.
        /// </summary>
        /// <param name="lowerCaseName">The lower-cased namespace and name of the nested type.</param>
        /// <returns>The nested type, or <see langword="null"/> if none is found.</returns>
        internal virtual Type FindNestedTypeIgnoreCase(TypeName lowerCaseName)
        {
            foreach (var type in __GetDeclaredTypes())
                if (type.TypeName.ToLowerInvariant() == lowerCaseName)
                    return type;

            return null;
        }

        /// <summary>
        /// Searches for the public nested type with the specified name.
        /// </summary>
        /// <param name="name">The string containing the name of the nested type to get.</param>
        /// <returns>An object representing the public nested type with the specified name, if found; otherwise, <see langword="null"/>.</returns>
        public Type GetNestedType(string name) => GetNestedType(name, BindingFlags.Public);

        /// <summary>
        /// Searches for the specified nested type, using the specified binding constraints.
        /// </summary>
        /// <param name="name">The string containing the name of the nested type to get.</param>
        /// <param name="bindingAttr">A bitwise combination of the enumeration values that specify how the search is conducted.</param>
        /// <returns>An object representing the nested type that matches the specified requirements, if found; otherwise, <see langword="null"/>.</returns>
        public Type GetNestedType(string name, BindingFlags bindingAttr) =>
            // FXBUG the namespace is ignored, so we can use GetMemberByName
            GetMemberByName<Type>(name, bindingAttr | BindingFlags.DeclaredOnly);

        /// <summary>
        /// Returns the public types nested in the current type.
        /// </summary>
        /// <returns>An array of <see cref="Type"/> objects representing the public types nested in the current type, or an empty array if there are none.</returns>
        public Type[] GetNestedTypes() => GetNestedTypes(BindingFlags.Public);

        /// <summary>
        /// Searches for the types nested in the current type, using the specified binding constraints.
        /// </summary>
        /// <param name="bindingAttr">A bitwise combination of the enumeration values that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="Type"/> objects representing all the types nested in the current type that match the specified binding constraints, or an empty array if there are none.</returns>
        public Type[] GetNestedTypes(BindingFlags bindingAttr) =>
            // FXBUG the namespace is ignored, so we can use GetMember
            GetMembers<Type>(bindingAttr | BindingFlags.DeclaredOnly);

        /// <summary>
        /// Returns all the public properties of the current type.
        /// </summary>
        /// <returns>An array of <see cref="PropertyInfo"/> objects representing all public properties of the current type.</returns>
        public PropertyInfo[] GetProperties() => GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

        /// <summary>
        /// Searches for the properties of the current type, using the specified binding constraints.
        /// </summary>
        /// <param name="bindingAttr">A bitwise combination of the enumeration values that specify how the search is conducted.</param>
        /// <returns>An array of <see cref="PropertyInfo"/> objects representing all properties of the current type that match the specified binding constraints.</returns>
        public PropertyInfo[] GetProperties(BindingFlags bindingAttr) => GetMembers<PropertyInfo>(bindingAttr);

        /// <summary>
        /// Searches for the public property with the specified name.
        /// </summary>
        /// <param name="name">The string containing the name of the public property to get.</param>
        /// <returns>An object representing the public property with the specified name, if found; otherwise, <see langword="null"/>.</returns>
        public PropertyInfo GetProperty(string name) => GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

        /// <summary>
        /// Searches for the specified property, using the specified binding constraints.
        /// </summary>
        /// <param name="name">The string containing the name of the property to get.</param>
        /// <param name="bindingAttr">A bitwise combination of the enumeration values that specify how the search is conducted.</param>
        /// <returns>An object representing the property that matches the specified requirements, if found; otherwise, <see langword="null"/>.</returns>
        public PropertyInfo GetProperty(string name, BindingFlags bindingAttr) => GetMemberByName<PropertyInfo>(name, bindingAttr);

        /// <summary>
        /// Searches for the public property with the specified name and return type.
        /// </summary>
        /// <param name="name">The string containing the name of the public property to get.</param>
        /// <param name="returnType">The return type of the property.</param>
        /// <returns>An object representing the public property with the specified name, if found; otherwise, <see langword="null"/>.</returns>
        public PropertyInfo GetProperty(string name, Type returnType)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

            return GetMemberByName<PropertyInfo>(name, flags, delegate (PropertyInfo prop)
            { return prop.PropertyType.Equals(returnType); })
                ?? GetPropertyWithBinder(name, flags, DefaultBinder, returnType, null, null);
        }

        /// <summary>
        /// Searches for the specified public property whose parameters match the specified argument types.
        /// </summary>
        /// <param name="name">The string containing the name of the public property to get.</param>
        /// <param name="types">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the indexed property to get.</param>
        /// <returns>An object representing the public property whose parameters match the specified argument types, if found; otherwise, <see langword="null"/>.</returns>
        public PropertyInfo GetProperty(string name, Type[] types)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

            return GetMemberByName<PropertyInfo>(name, flags, delegate (PropertyInfo prop)
            { return prop.PropertySignature.MatchParameterTypes(types); })
                ?? GetPropertyWithBinder(name, flags, DefaultBinder, null, types, null);
        }

        /// <summary>
        /// Searches for the specified public property whose parameters match the specified argument types.
        /// </summary>
        /// <param name="name">The string containing the name of the public property to get.</param>
        /// <param name="returnType">The return type of the property.</param>
        /// <param name="types">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the indexed property to get.</param>
        /// <returns>An object representing the public property whose parameters match the specified argument types, if found; otherwise, <see langword="null"/>.</returns>
        public PropertyInfo GetProperty(string name, Type returnType, Type[] types) => GetProperty(name, returnType, types, null);

        /// <summary>
        /// Searches for the specified public property whose parameters match the specified argument types and
        /// modifiers.
        /// </summary>
        /// <param name="name">The string containing the name of the public property to get.</param>
        /// <param name="returnType">The return type of the property.</param>
        /// <param name="types">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the indexed property to get.</param>
        /// <param name="modifiers">An array of <see cref="ParameterModifier"/> objects representing the attributes associated with the corresponding element in the <paramref name="types"/> array.</param>
        /// <returns>An object representing the public property that matches the specified requirements, if found; otherwise, <see langword="null"/>.</returns>
        public PropertyInfo GetProperty(string name, Type returnType, Type[] types, ParameterModifier[] modifiers) =>
            GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static, null, returnType, types, modifiers);

        /// <summary>
        /// Searches for the specified property whose parameters match the specified argument types and modifiers,
        /// using the specified binding constraints. A property whose return and parameter types match exactly is
        /// preferred; otherwise the binder chooses among the properties with the specified name.
        /// </summary>
        /// <param name="name">The string containing the name of the property to get.</param>
        /// <param name="bindingAttr">A bitwise combination of the enumeration values that specify how the search is conducted.</param>
        /// <param name="binder">An object that defines a set of properties and enables binding, or <see langword="null"/> to use the <see cref="DefaultBinder"/>.</param>
        /// <param name="returnType">The return type of the property.</param>
        /// <param name="types">An array of <see cref="Type"/> objects representing the number, order, and type of the parameters for the indexed property to get.</param>
        /// <param name="modifiers">An array of <see cref="ParameterModifier"/> objects representing the attributes associated with the corresponding element in the <paramref name="types"/> array.</param>
        /// <returns>An object representing the property that matches the specified requirements, if found; otherwise, <see langword="null"/>.</returns>
        public PropertyInfo GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
        {
            return GetMemberByName<PropertyInfo>(name, bindingAttr,
                delegate (PropertyInfo prop)
                {
                    return prop.PropertyType.Equals(returnType) && prop.PropertySignature.MatchParameterTypes(types);
                })
                ?? GetPropertyWithBinder(name, bindingAttr, binder ?? DefaultBinder, returnType, types, modifiers);
        }

        /// <summary>
        /// Collects every property with the specified name that matches the binding flags and lets the binder select
        /// one.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="bindingAttr">The binding flags that select the candidates.</param>
        /// <param name="binder">The binder that selects among the candidates.</param>
        /// <param name="returnType">The return type to match, or <see langword="null"/>.</param>
        /// <param name="types">The index parameter types to match, or <see langword="null"/>.</param>
        /// <param name="modifiers">The parameter modifiers, or <see langword="null"/>.</param>
        /// <returns>The property selected by the binder, or <see langword="null"/> if none matches.</returns>
        PropertyInfo GetPropertyWithBinder(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
        {
            List<PropertyInfo> list = [];
            GetMemberByName<PropertyInfo>(name, bindingAttr, delegate (PropertyInfo property)
            {
                list.Add(property);
                return false;
            });
            return binder.SelectProperty(bindingAttr, list.ToArray(), returnType, types, modifiers);
        }

        /// <summary>
        /// Searches for the interface with the specified full name.
        /// </summary>
        /// <param name="name">The string containing the full name of the interface to get.</param>
        /// <returns>An object representing the interface with the specified name, implemented or inherited by the current type, if found; otherwise, <see langword="null"/>.</returns>
        /// <exception cref="AmbiguousMatchException">The current type represents a type that implements the same generic interface with different type arguments.</exception>
        public Type GetInterface(string name) => GetInterface(name, false);

        /// <summary>
        /// Searches for the specified interface, specifying whether to do a case-insensitive search for the interface
        /// name.
        /// </summary>
        /// <param name="name">The string containing the full name of the interface to get.</param>
        /// <param name="ignoreCase"><see langword="true"/> to ignore the case of <paramref name="name"/>; <see langword="false"/> to perform a case-sensitive search.</param>
        /// <returns>An object representing the interface with the specified name, implemented or inherited by the current type, if found; otherwise, <see langword="null"/>.</returns>
        /// <exception cref="AmbiguousMatchException">The current type represents a type that implements the same generic interface with different type arguments.</exception>
        public Type GetInterface(string name, bool ignoreCase)
        {
            if (ignoreCase)
                name = name.ToLowerInvariant();

            Type found = null;

            foreach (var type in GetInterfaces())
            {
                var typeName = type.FullName;
                if (ignoreCase)
                    typeName = typeName.ToLowerInvariant();

                if (typeName == name)
                {
                    if (found != null)
                        throw new AmbiguousMatchException();

                    found = type;
                }
            }

            return found;
        }

        /// <summary>
        /// Gets the initializer for the type.
        /// </summary>
        public ConstructorInfo TypeInitializer =>
            GetConstructor(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);

        /// <summary>
        /// Gets a value indicating whether the type is one of the primitive types: <see cref="bool"/>,
        /// <see cref="char"/>, the integral and floating-point types, <see cref="IntPtr"/> and <see cref="UIntPtr"/>.
        /// </summary>
        public bool IsPrimitive
        {
            get
            {
                return __IsBuiltIn
                    && ((_sigElementType >= Signature.ELEMENT_TYPE_BOOLEAN && _sigElementType <= Signature.ELEMENT_TYPE_R8)
                        || _sigElementType == Signature.ELEMENT_TYPE_I
                        || _sigElementType == Signature.ELEMENT_TYPE_U);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the type is one of the built-in types of ECMA-335 that have their own
        /// signature element type, such as <see cref="int"/>, <see cref="string"/>, <see cref="object"/> and
        /// <c>System.Void</c>, as defined by the universe's core library.
        /// </summary>
        public bool __IsBuiltIn
        {
            get
            {
                return (typeFlags & (TypeFlags.BuiltIn | TypeFlags.PotentialBuiltIn)) != 0
                    && ((typeFlags & TypeFlags.BuiltIn) != 0 || ResolvePotentialBuiltInType());
            }
        }

        /// <summary>
        /// Gets the <c>ELEMENT_TYPE_*</c> value that encodes this type in a signature.
        /// </summary>
        internal byte SigElementType
        {
            get
            {
                // this property can only be called after __IsBuiltIn, HasElementType, __IsFunctionPointer or IsGenericParameter returned true
                System.Diagnostics.Debug.Assert((typeFlags & TypeFlags.BuiltIn) != 0 || HasElementType || IsFunctionPointer || IsGenericParameter);
                return _sigElementType;
            }
        }

        /// <summary>
        /// Decides whether a type whose namespace and name match a built-in type really is that built-in type, by
        /// comparing it with the corresponding type of the universe, and caches the outcome.
        /// </summary>
        /// <returns><see langword="true"/> if the type is the built-in type; otherwise, <see langword="false"/>.</returns>
        /// <exception cref="InvalidOperationException">The name of the type is not that of a built-in type.</exception>
        bool ResolvePotentialBuiltInType()
        {
            // [ECMA 335] 8.2.2 Built-in value and reference types
            typeFlags &= ~TypeFlags.PotentialBuiltIn;

            return __Name switch
            {
                "Boolean" => ResolvePotentialBuiltInType(Universe.System_Boolean, Signature.ELEMENT_TYPE_BOOLEAN),
                "Char" => ResolvePotentialBuiltInType(Universe.System_Char, Signature.ELEMENT_TYPE_CHAR),
                "Object" => ResolvePotentialBuiltInType(Universe.System_Object, Signature.ELEMENT_TYPE_OBJECT),
                "String" => ResolvePotentialBuiltInType(Universe.System_String, Signature.ELEMENT_TYPE_STRING),
                "Single" => ResolvePotentialBuiltInType(Universe.System_Single, Signature.ELEMENT_TYPE_R4),
                "Double" => ResolvePotentialBuiltInType(Universe.System_Double, Signature.ELEMENT_TYPE_R8),
                "SByte" => ResolvePotentialBuiltInType(Universe.System_SByte, Signature.ELEMENT_TYPE_I1),
                "Int16" => ResolvePotentialBuiltInType(Universe.System_Int16, Signature.ELEMENT_TYPE_I2),
                "Int32" => ResolvePotentialBuiltInType(Universe.System_Int32, Signature.ELEMENT_TYPE_I4),
                "Int64" => ResolvePotentialBuiltInType(Universe.System_Int64, Signature.ELEMENT_TYPE_I8),
                "IntPtr" => ResolvePotentialBuiltInType(Universe.System_IntPtr, Signature.ELEMENT_TYPE_I),
                "UIntPtr" => ResolvePotentialBuiltInType(Universe.System_UIntPtr, Signature.ELEMENT_TYPE_U),
                "TypedReference" => ResolvePotentialBuiltInType(Universe.System_TypedReference, Signature.ELEMENT_TYPE_TYPEDBYREF),
                "Byte" => ResolvePotentialBuiltInType(Universe.System_Byte, Signature.ELEMENT_TYPE_U1),
                "UInt16" => ResolvePotentialBuiltInType(Universe.System_UInt16, Signature.ELEMENT_TYPE_U2),
                "UInt32" => ResolvePotentialBuiltInType(Universe.System_UInt32, Signature.ELEMENT_TYPE_U4),
                "UInt64" => ResolvePotentialBuiltInType(Universe.System_UInt64, Signature.ELEMENT_TYPE_U8),
                // [LAMESPEC] missing from ECMA list for some reason
                "Void" => ResolvePotentialBuiltInType(Universe.System_Void, Signature.ELEMENT_TYPE_VOID),
                _ => throw new InvalidOperationException(),
            };
        }

        /// <summary>
        /// Marks this type as built in with the specified signature element type if it is the specified built-in type.
        /// </summary>
        /// <param name="builtIn">The built-in type of the universe to compare with.</param>
        /// <param name="elementType">The <c>ELEMENT_TYPE_*</c> value of the built-in type.</param>
        /// <returns><see langword="true"/> if this type is <paramref name="builtIn"/>; otherwise, <see langword="false"/>.</returns>
        bool ResolvePotentialBuiltInType(Type builtIn, byte elementType)
        {
            if (this == builtIn)
            {
                typeFlags |= TypeFlags.BuiltIn;
                _sigElementType = elementType;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gets a value indicating whether the current type represents an enumeration, that is, whether its base type
        /// is the core library's <c>System.Enum</c>.
        /// </summary>
        public bool IsEnum
        {
            get
            {
                var baseType = BaseType;
                return baseType != null
                    && baseType.IsEnumOrValueType
                    && baseType.__Name[0] == 'E';
            }
        }

        /// <summary>
        /// Gets a value indicating whether the type is declared sealed.
        /// </summary>
        public bool IsSealed => (Attributes & TypeAttributes.Sealed) != 0;

        /// <summary>
        /// Gets a value indicating whether the type is abstract and must be overridden.
        /// </summary>
        public bool IsAbstract => (Attributes & TypeAttributes.Abstract) != 0;

        /// <summary>
        /// Determines whether the visibility of the type is exactly the specified visibility.
        /// </summary>
        /// <param name="access">A visibility value within <see cref="TypeAttributes.VisibilityMask"/>.</param>
        /// <returns><see langword="true"/> if the type has that visibility; otherwise, <see langword="false"/>.</returns>
        bool CheckVisibility(TypeAttributes access) => (Attributes & TypeAttributes.VisibilityMask) == access;

        /// <summary>
        /// Gets a value indicating whether the type is declared public.
        /// </summary>
        public bool IsPublic => CheckVisibility(TypeAttributes.Public);

        /// <summary>
        /// Gets a value indicating whether a class is nested and declared public.
        /// </summary>
        public bool IsNestedPublic => CheckVisibility(TypeAttributes.NestedPublic);

        /// <summary>
        /// Gets a value indicating whether the type is nested and declared private.
        /// </summary>
        public bool IsNestedPrivate => CheckVisibility(TypeAttributes.NestedPrivate);

        /// <summary>
        /// Gets a value indicating whether the type is nested and visible only within its own family.
        /// </summary>
        public bool IsNestedFamily => CheckVisibility(TypeAttributes.NestedFamily);

        /// <summary>
        /// Gets a value indicating whether the type is nested and visible only within its own assembly.
        /// </summary>
        public bool IsNestedAssembly => CheckVisibility(TypeAttributes.NestedAssembly);

        /// <summary>
        /// Gets a value indicating whether the type is nested and visible only to classes that belong to both its own
        /// family and its own assembly.
        /// </summary>
        public bool IsNestedFamANDAssem => CheckVisibility(TypeAttributes.NestedFamANDAssem);

        /// <summary>
        /// Gets a value indicating whether the type is nested and visible only to classes that belong to either its
        /// own family or to its own assembly.
        /// </summary>
        public bool IsNestedFamORAssem => CheckVisibility(TypeAttributes.NestedFamORAssem);

        /// <summary>
        /// Gets a value indicating whether the type is not declared public.
        /// </summary>
        public bool IsNotPublic => CheckVisibility(TypeAttributes.NotPublic);

        /// <summary>
        /// Gets a value indicating whether the type can be accessed by code outside the assembly.
        /// </summary>
        public virtual bool IsVisible => IsPublic || (IsNestedPublic && DeclaringType.IsVisible);

        /// <summary>
        /// Gets a value indicating whether the fields of the current type are laid out automatically by the common
        /// language runtime.
        /// </summary>
        public bool IsAutoLayout => (Attributes & TypeAttributes.LayoutMask) == TypeAttributes.AutoLayout;

        /// <summary>
        /// Gets a value indicating whether the fields of the current type are laid out sequentially, in the order that
        /// they were defined or emitted to the metadata.
        /// </summary>
        public bool IsLayoutSequential => (Attributes & TypeAttributes.LayoutMask) == TypeAttributes.SequentialLayout;

        /// <summary>
        /// Gets a value indicating whether the fields of the current type are laid out at explicitly specified offsets.
        /// </summary>
        public bool IsExplicitLayout => (Attributes & TypeAttributes.LayoutMask) == TypeAttributes.ExplicitLayout;

        /// <summary>
        /// Gets whether the type is serializable. Like System.Reflection, enums and delegates are always serializable,
        /// whether or not they carry the metadata flag.
        /// </summary>
        public bool IsSerializable =>
            (Attributes & TypeAttributes.Serializable) != 0 || IsEnum || (BaseType is { } baseType && baseType == Universe.System_MulticastDelegate);

        /// <summary>
        /// Gets a value indicating whether the type is a class or a delegate; that is, not a value type or interface.
        /// </summary>
        public bool IsClass => !IsInterface && !IsValueType;

        /// <summary>
        /// Gets a value indicating whether the type is an interface; that is, not a class or a value type.
        /// </summary>
        public bool IsInterface => (Attributes & TypeAttributes.Interface) != 0;

        /// <summary>
        /// Gets a value indicating whether the current type object represents a type whose definition is nested inside
        /// the definition of another type.
        /// </summary>
        public bool IsNested
            // FXBUG we check the declaring type (like .NET) and this results
            // in IsNested returning true for a generic type parameter
            => DeclaringType != null;

        /// <summary>
        /// Gets a value indicating whether the type is a missing type or refers to one, through its generic arguments
        /// or custom modifiers. The result is computed once and cached.
        /// </summary>
        public bool __ContainsMissingType
        {
            get
            {
                if ((typeFlags & TypeFlags.ContainsMissingType_Mask) == TypeFlags.ContainsMissingType_Unknown)
                {
                    // Generic parameter constraints can refer back to the type parameter they are part of,
                    // so to prevent infinite recursion, we set the Pending flag during computation.
                    typeFlags |= TypeFlags.ContainsMissingType_Pending;
                    typeFlags = (typeFlags & ~TypeFlags.ContainsMissingType_Mask) | (ContainsMissingTypeImpl ? TypeFlags.ContainsMissingType_Yes : TypeFlags.ContainsMissingType_No);
                }

                return (typeFlags & TypeFlags.ContainsMissingType_Mask) == TypeFlags.ContainsMissingType_Yes;
            }
        }

        /// <summary>
        /// Determines whether any of the specified types is or refers to a missing type.
        /// </summary>
        /// <param name="types">The types to check, or <see langword="null"/>.</param>
        /// <returns><see langword="true"/> if any type contains a missing type; otherwise, <see langword="false"/>.</returns>
        internal static bool ContainsMissingType(Type[] types)
        {
            if (types == null)
                return false;

            foreach (var type in types)
                if (type.__ContainsMissingType)
                    return true;

            return false;
        }

        /// <summary>
        /// Implements <see cref="__ContainsMissingType"/> and determines whether the type is missing or refers to a
        /// missing type through its generic arguments or custom modifiers.
        /// </summary>
        protected virtual bool ContainsMissingTypeImpl
        {
            get
            {
                return __IsMissing
                    || ContainsMissingType(GetGenericArguments())
                    || __GetCustomModifiers().ContainsMissingType;
            }
        }

        /// <summary>
        /// Returns a <see cref="Type"/> object representing a one-dimensional array of the current type, with a lower
        /// bound of zero.
        /// </summary>
        /// <returns>A <see cref="Type"/> object representing a one-dimensional array of the current type, with a lower bound of zero.</returns>
        public Type MakeArrayType() => ArrayType.Make(this, new CustomModifiers());

        /// <summary>
        /// Returns a <see cref="Type"/> object representing a one-dimensional array with a lower bound of zero of the
        /// current type with the specified custom modifiers.
        /// </summary>
        /// <param name="customModifiers">The custom modifiers applied to the element type.</param>
        /// <returns>The single-dimensional array type.</returns>
        public Type __MakeArrayType(CustomModifiers customModifiers) => ArrayType.Make(this, customModifiers);

        /// <summary>
        /// Returns a <see cref="Type"/> object representing an array of the current type, with the specified number of
        /// dimensions.
        /// </summary>
        /// <param name="rank">The number of dimensions for the array.</param>
        /// <returns>An object representing an array of the current type, with the specified number of dimensions.</returns>
        public Type MakeArrayType(int rank) => __MakeArrayType(rank, new CustomModifiers());

        /// <summary>
        /// Returns a <see cref="Type"/> object representing a multi-dimensional array of the current type with the
        /// specified rank and custom modifiers, and with every lower bound set to zero.
        /// </summary>
        /// <param name="rank">The number of dimensions for the array.</param>
        /// <param name="customModifiers">The custom modifiers applied to the element type.</param>
        /// <returns>The multi-dimensional array type.</returns>
        public Type __MakeArrayType(int rank, CustomModifiers customModifiers) =>
            MultiArrayType.Make(this, rank, Array.Empty<int>(), new int[rank], customModifiers);

        /// <summary>
        /// Returns a <see cref="Type"/> object representing a multi-dimensional array of the current type with the
        /// specified rank, sizes, lower bounds and custom modifiers, as they are encoded in an array shape.
        /// </summary>
        /// <param name="rank">The number of dimensions for the array.</param>
        /// <param name="sizes">The sizes of the leading dimensions, or <see langword="null"/> for none.</param>
        /// <param name="lobounds">The lower bounds of the leading dimensions, or <see langword="null"/> for none.</param>
        /// <param name="customModifiers">The custom modifiers applied to the element type.</param>
        /// <returns>The multi-dimensional array type.</returns>
        public Type __MakeArrayType(int rank, int[] sizes, int[] lobounds, CustomModifiers customModifiers) =>
            MultiArrayType.Make(this, rank, sizes ?? Array.Empty<int>(), lobounds ?? Array.Empty<int>(), customModifiers);

        /// <summary>
        /// Returns a <see cref="Type"/> object that represents the current type when passed as a <c>ref</c>
        /// parameter.
        /// </summary>
        /// <returns>A <see cref="Type"/> object that represents the current type when passed as a <c>ref</c> parameter.</returns>
        public Type MakeByRefType() => ByRefType.Make(this, new CustomModifiers());

        /// <summary>
        /// Returns a <see cref="Type"/> object that represents the current type, with the specified custom modifiers,
        /// when passed as a <c>ref</c> parameter.
        /// </summary>
        /// <param name="customModifiers">The custom modifiers applied to the referenced type.</param>
        /// <returns>The by-ref type.</returns>
        public Type __MakeByRefType(CustomModifiers customModifiers) => ByRefType.Make(this, customModifiers);

        /// <summary>
        /// Returns a <see cref="Type"/> object that represents a pointer to the current type.
        /// </summary>
        /// <returns>A <see cref="Type"/> object that represents a pointer to the current type.</returns>
        public Type MakePointerType() => PointerType.Make(this, new CustomModifiers());

        /// <summary>
        /// Returns a <see cref="Type"/> object that represents a pointer to the current type with the specified custom
        /// modifiers.
        /// </summary>
        /// <param name="customModifiers">The custom modifiers applied to the pointed-to type.</param>
        /// <returns>The pointer type.</returns>
        public Type __MakePointerType(CustomModifiers customModifiers) => PointerType.Make(this, customModifiers);

        /// <summary>
        /// Substitutes the elements of an array of types for the type parameters of the current generic type
        /// definition and returns a <see cref="Type"/> object representing the resulting constructed type.
        /// </summary>
        /// <param name="typeArguments">An array of types to be substituted for the type parameters of the current generic type.</param>
        /// <returns>A <see cref="Type"/> representing the constructed type formed by substituting the elements of <paramref name="typeArguments"/> for the type parameters of the current generic type.</returns>
        /// <exception cref="InvalidOperationException">The current type does not represent a generic type definition.</exception>
        public Type MakeGenericType(params Type[] typeArguments) => __MakeGenericType(typeArguments, null);

        /// <summary>
        /// Substitutes the elements of an array of types for the type parameters of the current generic type
        /// definition, with custom modifiers on the type arguments. A missing type is accepted as the generic type
        /// definition, since whether it is generic is unknown.
        /// </summary>
        /// <param name="typeArguments">An array of types to be substituted for the type parameters of the current generic type.</param>
        /// <param name="customModifiers">The custom modifiers for each type argument, or <see langword="null"/>.</param>
        /// <returns>The constructed type.</returns>
        /// <exception cref="InvalidOperationException">The current type is neither missing nor a generic type definition.</exception>
        public Type __MakeGenericType(Type[] typeArguments, CustomModifiers[] customModifiers)
        {
            if (!__IsMissing && !IsGenericTypeDefinition)
            {
                throw new InvalidOperationException();
            }
            return GenericTypeInstance.Make(this, Util.Copy(typeArguments), customModifiers == null ? null : (CustomModifiers[])customModifiers.Clone());
        }

        /// <summary>
        /// Gets the runtime type that corresponds to a <see cref="TypeCode"/>.
        /// </summary>
        /// <param name="typeCode">The type code.</param>
        /// <returns>The runtime type for <paramref name="typeCode"/>, or <see langword="null"/> for <see cref="TypeCode.Empty"/>.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="typeCode"/> is not a defined type code.</exception>
        public static System.Type __GetSystemType(TypeCode typeCode)
        {
            return typeCode switch
            {
                TypeCode.Boolean => typeof(bool),
                TypeCode.Byte => typeof(byte),
                TypeCode.Char => typeof(char),
                TypeCode.DBNull => typeof(System.DBNull),
                TypeCode.DateTime => typeof(System.DateTime),
                TypeCode.Decimal => typeof(decimal),
                TypeCode.Double => typeof(double),
                TypeCode.Empty => null,
                TypeCode.Int16 => typeof(short),
                TypeCode.Int32 => typeof(int),
                TypeCode.Int64 => typeof(long),
                TypeCode.Object => typeof(object),
                TypeCode.SByte => typeof(sbyte),
                TypeCode.Single => typeof(float),
                TypeCode.String => typeof(string),
                TypeCode.UInt16 => typeof(ushort),
                TypeCode.UInt32 => typeof(uint),
                TypeCode.UInt64 => typeof(ulong),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }

        /// <summary>
        /// Gets the underlying type code of the specified type. For an enumeration, this is the type code of its
        /// underlying type.
        /// </summary>
        /// <param name="type">The type whose underlying type code to get.</param>
        /// <returns>The code of the underlying type, or <see cref="TypeCode.Empty"/> if <paramref name="type"/> is <see langword="null"/>.</returns>
        /// <exception cref="MissingMemberException"><paramref name="type"/> is a missing type that is not one of the types with a type code.</exception>
        public static TypeCode GetTypeCode(Type type)
        {
            if (type == null)
                return TypeCode.Empty;

            if (!type.__IsMissing && type.IsEnum)
                type = type.GetEnumUnderlyingType();

            Universe u = type.Module.Universe;
            if (type == u.System_Boolean)
            {
                return TypeCode.Boolean;
            }
            else if (type == u.System_Char)
            {
                return TypeCode.Char;
            }
            else if (type == u.System_SByte)
            {
                return TypeCode.SByte;
            }
            else if (type == u.System_Byte)
            {
                return TypeCode.Byte;
            }
            else if (type == u.System_Int16)
            {
                return TypeCode.Int16;
            }
            else if (type == u.System_UInt16)
            {
                return TypeCode.UInt16;
            }
            else if (type == u.System_Int32)
            {
                return TypeCode.Int32;
            }
            else if (type == u.System_UInt32)
            {
                return TypeCode.UInt32;
            }
            else if (type == u.System_Int64)
            {
                return TypeCode.Int64;
            }
            else if (type == u.System_UInt64)
            {
                return TypeCode.UInt64;
            }
            else if (type == u.System_Single)
            {
                return TypeCode.Single;
            }
            else if (type == u.System_Double)
            {
                return TypeCode.Double;
            }
            else if (type == u.System_DateTime)
            {
                return TypeCode.DateTime;
            }
            else if (type == u.System_DBNull)
            {
                return TypeCode.DBNull;
            }
            else if (type == u.System_Decimal)
            {
                return TypeCode.Decimal;
            }
            else if (type == u.System_String)
            {
                return TypeCode.String;
            }
            else if (type.__IsMissing)
            {
                throw new MissingMemberException(type);
            }
            else
            {
                return TypeCode.Object;
            }
        }

        /// <summary>
        /// Gets the <see cref="IKVM.Reflection.Assembly"/> in which the type is declared.
        /// </summary>
        public Assembly Assembly => Module.Assembly;

        /// <summary>
        /// Determines whether an instance of a specified type can be assigned to a variable of the current type.
        /// </summary>
        /// <param name="type">The type to compare with the current type.</param>
        /// <returns><see langword="true"/> if <paramref name="type"/> is the same type, derives from or implements the current type, is an array type compatible with it, or is assignable through generic variance; otherwise, <see langword="false"/>.</returns>
        public bool IsAssignableFrom(Type type)
        {
            if (Equals(type))
            {
                return true;
            }
            else if (type == null)
            {
                return false;
            }
            else if (IsArray && type.IsArray)
            {
                if (GetArrayRank() != type.GetArrayRank())
                {
                    return false;
                }
                else if (IsSZArray && !type.IsSZArray)
                {
                    return false;
                }

                var e1 = GetElementType();
                var e2 = type.GetElementType();
                return e1.IsValueType == e2.IsValueType && e1.IsAssignableFrom(e2);
            }
            else if (IsCovariant(type))
            {
                return true;
            }
            else if (IsSealed)
            {
                return false;
            }
            else if (IsInterface)
            {
                foreach (Type iface in type.GetInterfaces())
                {
                    if (Equals(iface) || IsCovariant(iface))
                    {
                        return true;
                    }
                }
                return false;
            }
            else if (type.IsInterface)
            {
                return this == Module.Universe.System_Object;
            }
            else if (type.IsPointer)
            {
                return this == Module.Universe.System_Object || this == Module.Universe.System_ValueType;
            }
            else
            {
                return type.IsSubclassOf(this);
            }
        }

        /// <summary>
        /// Determines whether another constructed generic type is assignable to this one through the variance of the
        /// type parameters of their common generic type definition.
        /// </summary>
        /// <param name="other">The type to compare with.</param>
        /// <returns><see langword="true"/> if both are constructed from the same generic type definition and every type argument of <paramref name="other"/> is compatible with the corresponding one of this type; otherwise, <see langword="false"/>.</returns>
        bool IsCovariant(Type other)
        {
            if (IsConstructedGenericType
                && other.IsConstructedGenericType
                && GetGenericTypeDefinition() == other.GetGenericTypeDefinition())
            {
                Type[] typeParameters = GetGenericTypeDefinition().GetGenericArguments();
                for (var i = 0; i < typeParameters.Length; i++)
                {
                    Type t1 = GetGenericTypeArgument(i);
                    Type t2 = other.GetGenericTypeArgument(i);
                    if (t1.IsValueType != t2.IsValueType)
                    {
                        return false;
                    }
                    switch (typeParameters[i].GenericParameterAttributes & GenericParameterAttributes.VarianceMask)
                    {
                        case GenericParameterAttributes.Covariant:
                            if (!t1.IsAssignableFrom(t2))
                            {
                                return false;
                            }
                            break;
                        case GenericParameterAttributes.Contravariant:
                            if (!t2.IsAssignableFrom(t1))
                            {
                                return false;
                            }
                            break;
                        case GenericParameterAttributes.None:
                            if (t1 != t2)
                            {
                                return false;
                            }
                            break;
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Determines whether the current type derives from the specified type.
        /// </summary>
        /// <param name="type">The type to compare with the current type.</param>
        /// <returns><see langword="true"/> if the current type derives from <paramref name="type"/>; otherwise, <see langword="false"/>. This method also returns <see langword="false"/> if <paramref name="type"/> and the current type are equal.</returns>
        public bool IsSubclassOf(Type type)
        {
            Type thisType = BaseType;
            while (thisType != null)
            {
                if (thisType.Equals(type))
                    return true;

                thisType = thisType.BaseType;
            }
            return false;
        }

        /// <summary>
        /// Determines whether this type directly (that is, not inherited from the base class) implements the interface.
        /// </summary>
        /// <remarks>
        /// Note that a complicating factor is that the interface itself can be implemented by an interface that extends it.
        /// </remarks>
        /// <param name="interfaceType">The interface to look for.</param>
        /// <returns><see langword="true"/> if one of the interfaces declared by this type is or extends <paramref name="interfaceType"/>; otherwise, <see langword="false"/>.</returns>
        bool IsDirectlyImplementedInterface(Type interfaceType)
        {
            foreach (var iface in __GetDeclaredInterfaces())
                if (interfaceType.IsAssignableFrom(iface))
                    return true;

            return false;
        }

        /// <summary>
        /// Returns an interface mapping for the specified interface type.
        /// </summary>
        /// <param name="interfaceType">The interface type to retrieve a mapping for.</param>
        /// <returns>An object that represents the interface mapping for <paramref name="interfaceType"/>.</returns>
        public InterfaceMapping GetInterfaceMap(Type interfaceType)
        {
            CheckBaked();

            var map = new InterfaceMapping
            {
                InterfaceMethods = interfaceType.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public),
                InterfaceType = interfaceType
            };
            map.TargetMethods = new MethodInfo[map.InterfaceMethods.Length];
            map.TargetType = this;
            FillInInterfaceMethods(interfaceType, map.InterfaceMethods, map.TargetMethods);
            return map;
        }

        /// <summary>
        /// Fills in the methods of this type and its base types that implement the methods of an interface: explicit
        /// implementations first, then implicit ones if this type declares the interface, then the base type's
        /// implementations with overrides in this type substituted, and finally implicit implementations in base types
        /// of the same module.
        /// </summary>
        /// <param name="interfaceType">The interface type.</param>
        /// <param name="interfaceMethods">The methods of the interface.</param>
        /// <param name="targetMethods">The implementing methods, filled in for each entry that is still <see langword="null"/>.</param>
        void FillInInterfaceMethods(Type interfaceType, MethodInfo[] interfaceMethods, MethodInfo[] targetMethods)
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
        /// Fills in implementations by matching the virtual methods declared by this type on name and signature.
        /// </summary>
        /// <param name="interfaceMethods">The methods of the interface.</param>
        /// <param name="targetMethods">The implementing methods, filled in for each entry that is still <see langword="null"/>.</param>
        void FillInImplicitInterfaceMethods(MethodInfo[] interfaceMethods, MethodInfo[] targetMethods)
        {
            MethodBase[] methods = null;

            for (var i = 0; i < targetMethods.Length; i++)
            {
                if (targetMethods[i] == null)
                {
                    methods ??= __GetDeclaredMethods();

                    for (var j = 0; j < methods.Length; j++)
                    {
                        if (methods[j].IsVirtual && methods[j].Name == interfaceMethods[i].Name && methods[j].MethodSignature.Equals(interfaceMethods[i].MethodSignature))
                        {
                            targetMethods[i] = (MethodInfo)methods[j];
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Replaces base type implementations that this type overrides, either explicitly through a method
        /// implementation or implicitly by declaring a virtual method with the same name and signature that does not
        /// introduce a new slot.
        /// </summary>
        /// <param name="baseMethods">The implementing methods found in the base type, updated in place.</param>
        void ReplaceOverriddenMethods(MethodInfo[] baseMethods)
        {
            var impl = __GetMethodImplMap();
            for (var i = 0; i < baseMethods.Length; i++)
            {
                if (baseMethods[i] != null && !baseMethods[i].IsFinal)
                {
                    var def = baseMethods[i].GetBaseDefinition();
                    for (var j = 0; j < impl.MethodDeclarations.Length; j++)
                    {
                        for (var k = 0; k < impl.MethodDeclarations[j].Length; k++)
                        {
                            if (impl.MethodDeclarations[j][k].GetBaseDefinition() == def)
                            {
                                baseMethods[i] = impl.MethodBodies[j];
                                goto next;
                            }
                        }
                    }

                    var candidate = FindMethod(def.Name, def.MethodSignature) as MethodInfo;
                    if (candidate != null && candidate.IsVirtual && !candidate.IsNewSlot)
                        baseMethods[i] = candidate;
                }
            next:
                ;
            }
        }

        /// <summary>
        /// Fills in implementations from this type's method implementation map (explicit interface implementations).
        /// </summary>
        /// <param name="interfaceMethods">The methods of the interface.</param>
        /// <param name="targetMethods">The implementing methods, filled in for each entry that is still <see langword="null"/>.</param>
        internal void FillInExplicitInterfaceMethods(MethodInfo[] interfaceMethods, MethodInfo[] targetMethods)
        {
            var impl = __GetMethodImplMap();
            for (var i = 0; i < impl.MethodDeclarations.Length; i++)
            {
                for (var j = 0; j < impl.MethodDeclarations[i].Length; j++)
                {
                    var index = Array.IndexOf(interfaceMethods, impl.MethodDeclarations[i][j]);
                    if (index != -1 && targetMethods[index] == null)
                        targetMethods[index] = impl.MethodBodies[i];
                }
            }
        }

        /// <summary>
        /// Resolves a type parameter reference in a signature to the type argument of this type.
        /// </summary>
        /// <param name="index">The zero-based position of the type parameter.</param>
        /// <returns>The type argument at <paramref name="index"/>.</returns>
        Type IGenericContext.GetGenericTypeArgument(int index) => GetGenericTypeArgument(index);

        /// <summary>
        /// Throws, because a type provides no context for method type parameters.
        /// </summary>
        /// <param name="index">The zero-based position of the method type parameter.</param>
        /// <returns>This method does not return.</returns>
        /// <exception cref="BadImageFormatException">Always.</exception>
        Type IGenericContext.GetGenericMethodArgument(int index) => throw new BadImageFormatException();

        /// <summary>
        /// Binds a type parameter to the type argument of this type at the same position.
        /// </summary>
        /// <param name="type">The type parameter.</param>
        /// <returns>The corresponding type argument of this type.</returns>
        Type IGenericBinder.BindTypeParameter(Type type) => GetGenericTypeArgument(type.GenericParameterPosition);

        /// <summary>
        /// Throws, because a type provides no binding for method type parameters.
        /// </summary>
        /// <param name="type">The method type parameter.</param>
        /// <returns>This method does not return.</returns>
        /// <exception cref="BadImageFormatException">Always.</exception>
        Type IGenericBinder.BindMethodParameter(Type type) => throw new BadImageFormatException();

        /// <summary>
        /// Replaces the type parameters this type refers to with the arguments supplied by a binder. A generic type
        /// definition is instantiated over its bound type parameters; other types are returned unchanged unless a
        /// derived class overrides this.
        /// </summary>
        /// <param name="binder">The binder that supplies the generic arguments.</param>
        /// <returns>The bound type.</returns>
        internal virtual Type BindTypeParameters(IGenericBinder binder)
        {
            if (IsGenericTypeDefinition)
            {
                var args = GetGenericArguments();
                Type.InplaceBindTypeParameters(binder, args);
                return GenericTypeInstance.Make(this, args, null);
            }
            else
            {
                return this;
            }
        }

        /// <summary>
        /// Binds the type parameters of each element of an array in place.
        /// </summary>
        /// <param name="binder">The binder that supplies the generic arguments.</param>
        /// <param name="types">The types to bind, replaced with the bound types.</param>
        static void InplaceBindTypeParameters(IGenericBinder binder, Type[] types)
        {
            for (var i = 0; i < types.Length; i++)
                types[i] = types[i].BindTypeParameters(binder);
        }

        /// <summary>
        /// Finds a method or constructor declared by this type with the specified name and exact signature.
        /// </summary>
        /// <param name="name">The name of the method.</param>
        /// <param name="signature">The signature of the method.</param>
        /// <returns>The method, or <see langword="null"/> if none is found.</returns>
        internal virtual MethodBase FindMethod(string name, MethodSignature signature)
        {
            foreach (var method in __GetDeclaredMethods())
                if (method.Name == name && method.MethodSignature.Equals(signature))
                    return method;

            return null;
        }

        /// <summary>
        /// Finds a field declared by this type with the specified name and exact signature.
        /// </summary>
        /// <param name="name">The name of the field.</param>
        /// <param name="signature">The signature of the field.</param>
        /// <returns>The field, or <see langword="null"/> if none is found.</returns>
        internal virtual FieldInfo FindField(string name, FieldSignature signature)
        {
            foreach (var field in __GetDeclaredFields())
                if (field.Name == name && field.FieldSignature.Equals(signature))
                    return field;

            return null;
        }

        /// <summary>
        /// Gets a value indicating whether this attribute type allows multiple instances on the same target, as set by
        /// the <c>AllowMultiple</c> property of its <see cref="System.AttributeUsageAttribute"/>.
        /// </summary>
        internal bool IsAllowMultipleCustomAttribute
        {
            get
            {
                var cad = CustomAttributeData.__GetCustomAttributes(this, Module.Universe.System_AttributeUsageAttribute, false);
                if (cad.Count == 1)
                    foreach (CustomAttributeNamedArgument arg in cad[0].NamedArguments)
                        if (arg.MemberInfo.Name == "AllowMultiple")
                            return (bool)arg.TypedValue.Value;

                return false;
            }
        }

        /// <summary>
        /// Records that a signature referred to this type as a reference type (<c>ELEMENT_TYPE_CLASS</c>).
        /// </summary>
        /// <returns>This instance.</returns>
        internal Type MarkNotValueType()
        {
            typeFlags |= TypeFlags.NotValueType;
            return this;
        }

        /// <summary>
        /// Records that a signature referred to this type as a value type (<c>ELEMENT_TYPE_VALUETYPE</c>).
        /// </summary>
        /// <returns>This instance.</returns>
        internal Type MarkValueType()
        {
            typeFlags |= TypeFlags.ValueType;
            return this;
        }

        /// <summary>
        /// Gets the instance constructor of this attribute type with the specified parameter types, for building a
        /// custom attribute from a pseudo custom attribute. A missing constructor is created if it cannot be found
        /// and the universe allows missing members.
        /// </summary>
        /// <param name="parameterTypes">The parameter types of the constructor.</param>
        /// <returns>The constructor.</returns>
        internal ConstructorInfo GetPseudoCustomAttributeConstructor(params Type[] parameterTypes)
        {
            var u = Module.Universe;
            var methodSig = MethodSignature.MakeFromBuilder(u.System_Void, parameterTypes, new PackedCustomModifiers(), CallingConventions.Standard | CallingConventions.HasThis, 0);
            var mb = FindMethod(".ctor", methodSig) ?? u.GetMissingMethodOrThrow(null, this, ".ctor", methodSig);
            return (ConstructorInfo)mb;
        }

        /// <summary>
        /// Creates a missing method or constructor on this type with the specified signature, for referring to a
        /// member that is known to exist but cannot be resolved.
        /// </summary>
        /// <param name="name">The name of the method; <c>.ctor</c> or <c>.cctor</c> creates a constructor.</param>
        /// <param name="callingConvention">The calling convention of the method.</param>
        /// <param name="returnType">The return type, or <see langword="null"/> for <c>System.Void</c>.</param>
        /// <param name="returnTypeCustomModifiers">The custom modifiers of the return type.</param>
        /// <param name="parameterTypes">The parameter types.</param>
        /// <param name="parameterTypeCustomModifiers">The custom modifiers of each parameter, or <see langword="null"/>.</param>
        /// <returns>The missing method, or a constructor wrapping it.</returns>
        public MethodBase __CreateMissingMethod(string name, CallingConventions callingConvention, Type returnType, CustomModifiers returnTypeCustomModifiers, Type[] parameterTypes, CustomModifiers[] parameterTypeCustomModifiers) =>
            CreateMissingMethod(name, callingConvention, returnType, parameterTypes, PackedCustomModifiers.CreateFromExternal(returnTypeCustomModifiers, parameterTypeCustomModifiers, parameterTypes.Length));

        /// <summary>
        /// Creates a missing method or constructor on this type with the specified signature.
        /// </summary>
        /// <param name="name">The name of the method; <c>.ctor</c> or <c>.cctor</c> creates a constructor.</param>
        /// <param name="callingConvention">The calling convention of the method.</param>
        /// <param name="returnType">The return type, or <see langword="null"/> for <c>System.Void</c>.</param>
        /// <param name="parameterTypes">The parameter types, which are copied.</param>
        /// <param name="customModifiers">The custom modifiers of the return type and parameters.</param>
        /// <returns>The missing method, or a constructor wrapping it.</returns>
        MethodBase CreateMissingMethod(string name, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, PackedCustomModifiers customModifiers)
        {
            var sig = new MethodSignature(returnType ?? Module.Universe.System_Void, Util.Copy(parameterTypes), customModifiers, callingConvention, 0);
            var method = new MissingMethod(this, name, sig);

            if (name == ".ctor" || name == ".cctor")
                return new ConstructorInfoImpl(method);

            return method;
        }

        /// <summary>
        /// Records the token and type attributes of the reference that resolved to this type, if it is a missing type.
        /// Does nothing for other types.
        /// </summary>
        /// <param name="token">The metadata token to record.</param>
        /// <returns>This instance.</returns>
        internal virtual Type SetMetadataTokenForMissing(int token) => this;

        /// <summary>
        /// Marks a type whose namespace and name match a built-in type, <c>System.Enum</c> or <c>System.ValueType</c>
        /// as potentially being that type. Whether it is the core library's type is decided lazily, when first needed.
        /// </summary>
        /// <param name="typeNamespace">The namespace of the type.</param>
        /// <param name="typeName">The name of the type.</param>
        protected void MarkKnownType(string typeNamespace, string typeName)
        {
            // we assume that mscorlib won't have nested types with these names,
            // so we don't check that we're not a nested type
            if (typeNamespace == "System")
            {
                switch (typeName)
                {
                    case "Boolean":
                    case "Char":
                    case "Object":
                    case "String":
                    case "Single":
                    case "Double":
                    case "SByte":
                    case "Int16":
                    case "Int32":
                    case "Int64":
                    case "IntPtr":
                    case "UIntPtr":
                    case "TypedReference":
                    case "Byte":
                    case "UInt16":
                    case "UInt32":
                    case "UInt64":
                    case "Void":
                        typeFlags |= TypeFlags.PotentialBuiltIn;
                        break;
                    case "Enum":
                    case "ValueType":
                        typeFlags |= TypeFlags.PotentialEnumOrValueType;
                        break;
                }
            }
        }

        /// <summary>
        /// Decides whether a type named <c>System.Enum</c> or <c>System.ValueType</c> is the core library's type,
        /// either because it is defined in the core library or mscorlib, or because the core library resolves that
        /// name to it, and caches the outcome.
        /// </summary>
        /// <returns><see langword="true"/> if the type is the core library's <c>System.Enum</c> or <c>System.ValueType</c>; otherwise, <see langword="false"/>.</returns>
        bool ResolvePotentialEnumOrValueType()
        {
            if (Assembly == Universe.CoreLib || Assembly.GetName().Name.Equals("mscorlib", StringComparison.OrdinalIgnoreCase)
                // check if mscorlib forwards the type (.NETCore profile reference mscorlib forwards System.Enum and System.ValueType to System.Runtime.dll)
                || Universe.CoreLib.FindType(TypeName) == this)
            {
                typeFlags = (typeFlags & ~TypeFlags.PotentialEnumOrValueType) | TypeFlags.EnumOrValueType;
                return true;
            }
            else
            {
                typeFlags &= ~TypeFlags.PotentialEnumOrValueType;
                return false;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this type is the core library's <c>System.Enum</c> or
        /// <c>System.ValueType</c>.
        /// </summary>
        internal bool IsEnumOrValueType
        {
            get
            {
                return (typeFlags & (TypeFlags.EnumOrValueType | TypeFlags.PotentialEnumOrValueType)) != 0
                    && ((typeFlags & TypeFlags.EnumOrValueType) != 0 || ResolvePotentialEnumOrValueType());
            }
        }

        /// <summary>
        /// Gets the universe the type belongs to.
        /// </summary>
        internal virtual Universe Universe => Module.Universe;

        /// <summary>
        /// Determines whether the type, as a nested type, matches the public or non-public binding flags.
        /// </summary>
        /// <param name="flags">The binding flags to match.</param>
        /// <returns><see langword="true"/> if the type matches; otherwise, <see langword="false"/>.</returns>
        internal sealed override bool BindingFlagsMatch(BindingFlags flags) =>
            BindingFlagsMatch(IsNestedPublic, flags, BindingFlags.Public, BindingFlags.NonPublic);

        /// <summary>
        /// Throws, because nested types are never inherited and so never have a different reflected type.
        /// </summary>
        /// <param name="type">The reflected type.</param>
        /// <returns>This method does not return.</returns>
        /// <exception cref="InvalidOperationException">Always.</exception>
        internal sealed override MemberInfo SetReflectedType(Type type) => throw new InvalidOperationException();

        /// <summary>
        /// Gets the current metadata token of the type, which is its <see cref="MemberInfo.MetadataToken"/>.
        /// </summary>
        /// <returns>The metadata token.</returns>
        internal override int GetCurrentToken() => MetadataToken;

        /// <summary>
        /// Gets the pseudo custom attributes of the type. Types have none.
        /// </summary>
        /// <param name="attributeType">The attribute type to filter by.</param>
        /// <returns><see langword="null"/>.</returns>
        internal sealed override List<CustomAttributeData> GetPseudoCustomAttributes(Type attributeType) =>
            // types don't have pseudo custom attributes
            null;

        /// <summary>
        /// Returns the <see cref="TypeInfo"/> representation of the current type. In .NET this is an extension
        /// method; here it is an instance method.
        /// </summary>
        /// <returns>The current type as a <see cref="TypeInfo"/>.</returns>
        /// <exception cref="MissingMemberException">The type is not a <see cref="TypeInfo"/>, such as a missing type.</exception>
        public TypeInfo GetTypeInfo() => this as TypeInfo ?? throw new MissingMemberException(this);

    }

}
