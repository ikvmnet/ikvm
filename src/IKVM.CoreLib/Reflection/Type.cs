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

    abstract class Type : MemberInfo, IGenericContext, IGenericBinder
    {

        public static readonly Type[] EmptyTypes = Array.Empty<Type>();
        protected readonly Type underlyingType;
        protected TypeFlags typeFlags;
        byte _sigElementType;    // only used if (__IsBuiltIn || HasElementType || __IsFunctionPointer || IsGenericParameter)

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
        /// Initializes a new instance.
        /// </summary>
        internal Type()
        {
            underlyingType = this;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="underlyingType"></param>
        internal Type(Type underlyingType)
        {
            System.Diagnostics.Debug.Assert(underlyingType.underlyingType == underlyingType);
            this.underlyingType = underlyingType;
            typeFlags = underlyingType.typeFlags;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="sigElementType"></param>
        internal Type(byte sigElementType) :
            this()
        {
            _sigElementType = sigElementType;
        }

        public static Binder DefaultBinder => new DefaultBinder();

        public sealed override MemberTypes MemberType => IsNested ? MemberTypes.NestedType : MemberTypes.TypeInfo;

        public virtual string AssemblyQualifiedName
            // NOTE the assembly name is not escaped here, only when used in a generic type instantiation
            => FullName + ", " + Assembly.FullName;

        public abstract Type BaseType
        {
            get;
        }

        public abstract TypeAttributes Attributes
        {
            get;
        }

        public virtual Type GetElementType() => null;

        internal virtual void CheckBaked()
        {
        }

        public virtual Type[] __GetDeclaredTypes() => Type.EmptyTypes;

        public virtual Type[] __GetDeclaredInterfaces() => Type.EmptyTypes;

        public virtual MethodBase[] __GetDeclaredMethods() => Array.Empty<MethodBase>();

        public virtual __MethodImplMap __GetMethodImplMap() => throw new NotSupportedException();

        public virtual FieldInfo[] __GetDeclaredFields() => Array.Empty<FieldInfo>();

        public virtual EventInfo[] __GetDeclaredEvents() => Array.Empty<EventInfo>();

        public virtual PropertyInfo[] __GetDeclaredProperties() => Array.Empty<PropertyInfo>();

        public virtual CustomModifiers __GetCustomModifiers() => new CustomModifiers();

        public virtual __StandAloneMethodSig __MethodSignature => throw new InvalidOperationException();

        public bool HasElementType => IsArray || IsByRef || IsPointer;

        public bool IsArray => _sigElementType == Signature.ELEMENT_TYPE_ARRAY || _sigElementType == Signature.ELEMENT_TYPE_SZARRAY;

        public bool IsSZArray => _sigElementType == Signature.ELEMENT_TYPE_SZARRAY;

        public bool IsByRef => _sigElementType == Signature.ELEMENT_TYPE_BYREF;

        public bool IsPointer => _sigElementType == Signature.ELEMENT_TYPE_PTR;

        public bool IsFunctionPointer => _sigElementType == Signature.ELEMENT_TYPE_FNPTR;

        public bool IsValueType
        {
            get
            {
                // MissingType sets both flags for WinRT projection types
                return (typeFlags & (TypeFlags.ValueType | TypeFlags.NotValueType)) switch
                {
                    0 or TypeFlags.ValueType | TypeFlags.NotValueType => IsValueTypeImpl,
                    _ => (typeFlags & TypeFlags.ValueType) != 0,
                };
            }
        }

        protected abstract bool IsValueTypeImpl
        {
            get;
        }

        public bool IsGenericParameter => _sigElementType == Signature.ELEMENT_TYPE_VAR || _sigElementType == Signature.ELEMENT_TYPE_MVAR;

        public virtual int GenericParameterPosition => throw new NotSupportedException();

        public virtual MethodBase DeclaringMethod => null;

        public Type UnderlyingSystemType => underlyingType;

        public override Type DeclaringType => null;

        internal virtual TypeName TypeName => throw new InvalidOperationException();

        public string __Name => TypeName.Name;

        public string __Namespace => TypeName.Namespace;

        public abstract override string Name
        {
            get;
        }

        public virtual string Namespace
        {
            get
            {
                if (IsNested)
                    return DeclaringType.Namespace;

                return __Namespace;
            }
        }

        internal virtual int GetModuleBuilderToken() => throw new InvalidOperationException();

        public static bool operator ==(Type t1, Type t2)
        {
            // Casting to object results in smaller code than calling ReferenceEquals and makes
            // this method more likely to be inlined.
            // On CLR v2 x86, microbenchmarks show this to be faster than calling ReferenceEquals.
            return (object)t1 == (object)t2
                || (t1 is not null && t2 is not null && (object)t1.underlyingType == (object)t2.underlyingType);
        }

        public static bool operator !=(Type t1, Type t2) => !(t1 == t2);

        public bool Equals(Type type) => this == type;

        public override bool Equals(object obj) => Equals(obj as Type);

        public override int GetHashCode()
        {
            Type type = UnderlyingSystemType;
            return ReferenceEquals(type, this) ? base.GetHashCode() : type.GetHashCode();
        }

        public Type[] GenericTypeArguments => IsConstructedGenericType ? GetGenericArguments() : Type.EmptyTypes;

        public virtual Type[] GetGenericArguments() => Type.EmptyTypes;

        public virtual CustomModifiers[] __GetGenericArgumentsCustomModifiers() => Array.Empty<CustomModifiers>();

        public virtual Type GetGenericTypeDefinition() => throw new InvalidOperationException();

        public virtual bool IsGenericType => false;

        public virtual bool IsGenericTypeDefinition => false;

        // .NET 4.5 API
        public virtual bool IsConstructedGenericType => false;

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

        public virtual Type[] GetGenericParameterConstraints() => throw new InvalidOperationException();

        public virtual GenericParameterAttributes GenericParameterAttributes => throw new InvalidOperationException();

        public virtual int GetArrayRank() => throw new NotSupportedException();

        public virtual int[] __GetArraySizes() => throw new NotSupportedException();

        public virtual int[] __GetArrayLowerBounds() => throw new NotSupportedException();

        // .NET 4.0 API
        public virtual Type GetEnumUnderlyingType()
        {
            if (!IsEnum)
                throw new ArgumentException();

            CheckBaked();
            return GetEnumUnderlyingTypeImpl();
        }

        internal Type GetEnumUnderlyingTypeImpl()
        {
            foreach (var field in __GetDeclaredFields())
                if (!field.IsStatic)
                    return field.FieldType; // the CLR assumes that an enum has only one instance field, so we can do the same

            throw new InvalidOperationException();
        }

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

        public override string ToString() => FullName;

        public abstract string FullName
        {
            get;
        }

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

        internal virtual bool IsModulePseudoType => false;

        internal virtual Type GetGenericTypeArgument(int index) => throw new InvalidOperationException();

        public MemberInfo[] GetDefaultMembers()
        {
            var defaultMemberAttribute = Module.Universe.Import(typeof(System.Reflection.DefaultMemberAttribute));
            foreach (var cad in CustomAttributeData.GetCustomAttributes(this))
                if (cad.Constructor.DeclaringType.Equals(defaultMemberAttribute))
                    return GetMember((string)cad.ConstructorArguments[0].Value);

            return Array.Empty<MemberInfo>();
        }

        public MemberInfo[] GetMember(string name) => GetMember(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

        public MemberInfo[] GetMember(string name, BindingFlags bindingAttr) => GetMember(name, MemberTypes.All, bindingAttr);

        public MemberInfo[] GetMembers() => GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

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

        static void AddMembers(List<MemberInfo> list, MemberFilter filter, object filterCriteria, MemberInfo[] members)
        {
            foreach (var member in members)
                if (filter == null || filter(member, filterCriteria))
                    list.Add(member);
        }

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
        /// <typeparam name="T"></typeparam>
        /// <param name="member"></param>
        /// <param name="derived"></param>
        /// <returns></returns>
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

        T GetMemberByName<T>(string name, BindingFlags flags)
            where T : MemberInfo => GetMemberByName<T>(name, flags, null);

        public EventInfo GetEvent(string name) => GetEvent(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

        public EventInfo GetEvent(string name, BindingFlags bindingAttr) => GetMemberByName<EventInfo>(name, bindingAttr);

        public EventInfo[] GetEvents() => GetEvents(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

        public EventInfo[] GetEvents(BindingFlags bindingAttr) => GetMembers<EventInfo>(bindingAttr);

        public FieldInfo GetField(string name) => GetField(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

        public FieldInfo GetField(string name, BindingFlags bindingAttr) => GetMemberByName<FieldInfo>(name, bindingAttr);

        public FieldInfo[] GetFields() => GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

        public FieldInfo[] GetFields(BindingFlags bindingAttr) => GetMembers<FieldInfo>(bindingAttr);

        public Type[] GetInterfaces()
        {
            var list = new List<Type>();
            for (var type = this; type != null; type = type.BaseType)
                AddInterfaces(list, type);

            return list.ToArray();
        }

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
                                if (baseMethods == null)
                                    baseMethods = [];
                                else if (baseMethods.Contains(mi.GetBaseDefinition()))
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

        public MethodInfo[] GetMethods() => GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

        public MethodInfo GetMethod(string name) => GetMethod(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);

        public MethodInfo GetMethod(string name, BindingFlags bindingAttr) => GetMemberByName<MethodInfo>(name, bindingAttr);

        public MethodInfo GetMethod(string name, Type[] types) => GetMethod(name, types, null);

        public MethodInfo GetMethod(string name, Type[] types, ParameterModifier[] modifiers) =>
            GetMethod(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, types, modifiers);

        public MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers) =>
            // first we try an exact match and only if that fails we fall back to using the binder
            GetMemberByName(name, bindingAttr, (MethodInfo m) => m.MethodSignature.MatchParameterTypes(types)) ?? GetMethodWithBinder<MethodInfo>(name, bindingAttr, binder ?? DefaultBinder, types, modifiers);

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

        public MethodInfo GetMethod(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers) =>
            // FXBUG callConvention seems to be ignored
            GetMethod(name, bindingAttr, binder, types, modifiers);

        public ConstructorInfo[] GetConstructors() => GetConstructors(BindingFlags.Public | BindingFlags.Instance);

        public ConstructorInfo[] GetConstructors(BindingFlags bindingAttr) => GetMembers<ConstructorInfo>(bindingAttr | BindingFlags.DeclaredOnly);

        public ConstructorInfo GetConstructor(Type[] types) =>
            GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, CallingConventions.Standard, types, null);

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

        ConstructorInfo GetConstructorImpl(string name, BindingFlags bindingAttr, Binder binder, Type[] types, ParameterModifier[] modifiers)
        {
            // first we try an exact match and only if that fails we fall back to using the binder
            return GetMemberByName<ConstructorInfo>(name, bindingAttr | BindingFlags.DeclaredOnly,
                delegate (ConstructorInfo ctor)
                { return ctor.MethodSignature.MatchParameterTypes(types); })
                ?? GetMethodWithBinder<ConstructorInfo>(name, bindingAttr, binder ?? DefaultBinder, types, modifiers);
        }

        public ConstructorInfo GetConstructor(BindingFlags bindingAttr, Binder binder, CallingConventions callingConvention, Type[] types, ParameterModifier[] modifiers) =>
            // FXBUG callConvention seems to be ignored
            GetConstructor(bindingAttr, binder, types, modifiers);

        internal Type ResolveNestedType(Module requester, TypeName typeName) =>
            FindNestedType(typeName) ?? Module.Universe.GetMissingTypeOrThrow(requester, Module, this, typeName);

        // unlike the public API, this takes the namespace and name into account
        internal virtual Type FindNestedType(TypeName name)
        {
            foreach (var type in __GetDeclaredTypes())
                if (type.TypeName == name)
                    return type;

            return null;
        }

        internal virtual Type FindNestedTypeIgnoreCase(TypeName lowerCaseName)
        {
            foreach (var type in __GetDeclaredTypes())
                if (type.TypeName.ToLowerInvariant() == lowerCaseName)
                    return type;

            return null;
        }

        public Type GetNestedType(string name) => GetNestedType(name, BindingFlags.Public);

        public Type GetNestedType(string name, BindingFlags bindingAttr) =>
            // FXBUG the namespace is ignored, so we can use GetMemberByName
            GetMemberByName<Type>(name, bindingAttr | BindingFlags.DeclaredOnly);

        public Type[] GetNestedTypes() => GetNestedTypes(BindingFlags.Public);

        public Type[] GetNestedTypes(BindingFlags bindingAttr) =>
            // FXBUG the namespace is ignored, so we can use GetMember
            GetMembers<Type>(bindingAttr | BindingFlags.DeclaredOnly);

        public PropertyInfo[] GetProperties() => GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

        public PropertyInfo[] GetProperties(BindingFlags bindingAttr) => GetMembers<PropertyInfo>(bindingAttr);

        public PropertyInfo GetProperty(string name) => GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

        public PropertyInfo GetProperty(string name, BindingFlags bindingAttr) => GetMemberByName<PropertyInfo>(name, bindingAttr);

        public PropertyInfo GetProperty(string name, Type returnType)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

            return GetMemberByName<PropertyInfo>(name, flags, delegate (PropertyInfo prop)
            { return prop.PropertyType.Equals(returnType); })
                ?? GetPropertyWithBinder(name, flags, DefaultBinder, returnType, null, null);
        }

        public PropertyInfo GetProperty(string name, Type[] types)
        {
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;

            return GetMemberByName<PropertyInfo>(name, flags, delegate (PropertyInfo prop)
            { return prop.PropertySignature.MatchParameterTypes(types); })
                ?? GetPropertyWithBinder(name, flags, DefaultBinder, null, types, null);
        }

        public PropertyInfo GetProperty(string name, Type returnType, Type[] types) => GetProperty(name, returnType, types, null);

        public PropertyInfo GetProperty(string name, Type returnType, Type[] types, ParameterModifier[] modifiers) =>
            GetProperty(name, BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static, null, returnType, types, modifiers);

        public PropertyInfo GetProperty(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
        {
            return GetMemberByName<PropertyInfo>(name, bindingAttr,
                delegate (PropertyInfo prop)
                {
                    return prop.PropertyType.Equals(returnType) && prop.PropertySignature.MatchParameterTypes(types);
                })
                ?? GetPropertyWithBinder(name, bindingAttr, binder ?? DefaultBinder, returnType, types, modifiers);
        }

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

        public Type GetInterface(string name) => GetInterface(name, false);

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

        public ConstructorInfo TypeInitializer =>
            GetConstructor(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, null, Type.EmptyTypes, null);

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

        public bool __IsBuiltIn
        {
            get
            {
                return (typeFlags & (TypeFlags.BuiltIn | TypeFlags.PotentialBuiltIn)) != 0
                    && ((typeFlags & TypeFlags.BuiltIn) != 0 || ResolvePotentialBuiltInType());
            }
        }

        internal byte SigElementType
        {
            get
            {
                // this property can only be called after __IsBuiltIn, HasElementType, __IsFunctionPointer or IsGenericParameter returned true
                System.Diagnostics.Debug.Assert((typeFlags & TypeFlags.BuiltIn) != 0 || HasElementType || IsFunctionPointer || IsGenericParameter);
                return _sigElementType;
            }
        }

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

        public bool IsSealed => (Attributes & TypeAttributes.Sealed) != 0;

        public bool IsAbstract => (Attributes & TypeAttributes.Abstract) != 0;

        bool CheckVisibility(TypeAttributes access) => (Attributes & TypeAttributes.VisibilityMask) == access;

        public bool IsPublic => CheckVisibility(TypeAttributes.Public);

        public bool IsNestedPublic => CheckVisibility(TypeAttributes.NestedPublic);

        public bool IsNestedPrivate => CheckVisibility(TypeAttributes.NestedPrivate);

        public bool IsNestedFamily => CheckVisibility(TypeAttributes.NestedFamily);

        public bool IsNestedAssembly => CheckVisibility(TypeAttributes.NestedAssembly);

        public bool IsNestedFamANDAssem => CheckVisibility(TypeAttributes.NestedFamANDAssem);

        public bool IsNestedFamORAssem => CheckVisibility(TypeAttributes.NestedFamORAssem);

        public bool IsNotPublic => CheckVisibility(TypeAttributes.NotPublic);

        public virtual bool IsVisible => IsPublic || (IsNestedPublic && DeclaringType.IsVisible);

        public bool IsAutoLayout => (Attributes & TypeAttributes.LayoutMask) == TypeAttributes.AutoLayout;

        public bool IsLayoutSequential => (Attributes & TypeAttributes.LayoutMask) == TypeAttributes.SequentialLayout;

        public bool IsExplicitLayout => (Attributes & TypeAttributes.LayoutMask) == TypeAttributes.ExplicitLayout;

        /// <summary>
        /// Gets whether the type is serializable. Like System.Reflection, enums and delegates are always serializable,
        /// whether or not they carry the metadata flag.
        /// </summary>
        public bool IsSerializable =>
            (Attributes & TypeAttributes.Serializable) != 0 || IsEnum || (BaseType is { } baseType && baseType == Universe.System_MulticastDelegate);

        public bool IsClass => !IsInterface && !IsValueType;

        public bool IsInterface => (Attributes & TypeAttributes.Interface) != 0;

        public bool IsNested
            // FXBUG we check the declaring type (like .NET) and this results
            // in IsNested returning true for a generic type parameter
            => DeclaringType != null;

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

        internal static bool ContainsMissingType(Type[] types)
        {
            if (types == null)
                return false;

            foreach (var type in types)
                if (type.__ContainsMissingType)
                    return true;

            return false;
        }

        protected virtual bool ContainsMissingTypeImpl
        {
            get
            {
                return __IsMissing
                    || ContainsMissingType(GetGenericArguments())
                    || __GetCustomModifiers().ContainsMissingType;
            }
        }

        public Type MakeArrayType() => ArrayType.Make(this, new CustomModifiers());

        public Type __MakeArrayType(CustomModifiers customModifiers) => ArrayType.Make(this, customModifiers);

        public Type MakeArrayType(int rank) => __MakeArrayType(rank, new CustomModifiers());

        public Type __MakeArrayType(int rank, CustomModifiers customModifiers) =>
            MultiArrayType.Make(this, rank, Array.Empty<int>(), new int[rank], customModifiers);

        public Type __MakeArrayType(int rank, int[] sizes, int[] lobounds, CustomModifiers customModifiers) =>
            MultiArrayType.Make(this, rank, sizes ?? Array.Empty<int>(), lobounds ?? Array.Empty<int>(), customModifiers);

        public Type MakeByRefType() => ByRefType.Make(this, new CustomModifiers());

        public Type __MakeByRefType(CustomModifiers customModifiers) => ByRefType.Make(this, customModifiers);

        public Type MakePointerType() => PointerType.Make(this, new CustomModifiers());

        public Type __MakePointerType(CustomModifiers customModifiers) => PointerType.Make(this, customModifiers);

        public Type MakeGenericType(params Type[] typeArguments) => __MakeGenericType(typeArguments, null);

        public Type __MakeGenericType(Type[] typeArguments, CustomModifiers[] customModifiers)
        {
            if (!__IsMissing && !IsGenericTypeDefinition)
            {
                throw new InvalidOperationException();
            }
            return GenericTypeInstance.Make(this, Util.Copy(typeArguments), customModifiers == null ? null : (CustomModifiers[])customModifiers.Clone());
        }

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

        public Assembly Assembly => Module.Assembly;

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
        /// This returns true if this type directly (i.e. not inherited from the base class) implements the interface.
        /// </summary>
        /// <remarks>
        /// Note that a complicating factor is that the interface itself can be implemented by an interface that extends it.
        /// </remarks>
        /// <param name="interfaceType"></param>
        /// <returns></returns>
        bool IsDirectlyImplementedInterface(Type interfaceType)
        {
            foreach (var iface in __GetDeclaredInterfaces())
                if (interfaceType.IsAssignableFrom(iface))
                    return true;

            return false;
        }

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

        Type IGenericContext.GetGenericTypeArgument(int index) => GetGenericTypeArgument(index);

        Type IGenericContext.GetGenericMethodArgument(int index) => throw new BadImageFormatException();

        Type IGenericBinder.BindTypeParameter(Type type) => GetGenericTypeArgument(type.GenericParameterPosition);

        Type IGenericBinder.BindMethodParameter(Type type) => throw new BadImageFormatException();

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

        static void InplaceBindTypeParameters(IGenericBinder binder, Type[] types)
        {
            for (var i = 0; i < types.Length; i++)
                types[i] = types[i].BindTypeParameters(binder);
        }

        internal virtual MethodBase FindMethod(string name, MethodSignature signature)
        {
            foreach (var method in __GetDeclaredMethods())
                if (method.Name == name && method.MethodSignature.Equals(signature))
                    return method;

            return null;
        }

        internal virtual FieldInfo FindField(string name, FieldSignature signature)
        {
            foreach (var field in __GetDeclaredFields())
                if (field.Name == name && field.FieldSignature.Equals(signature))
                    return field;

            return null;
        }

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

        internal Type MarkNotValueType()
        {
            typeFlags |= TypeFlags.NotValueType;
            return this;
        }

        internal Type MarkValueType()
        {
            typeFlags |= TypeFlags.ValueType;
            return this;
        }

        internal ConstructorInfo GetPseudoCustomAttributeConstructor(params Type[] parameterTypes)
        {
            var u = Module.Universe;
            var methodSig = MethodSignature.MakeFromBuilder(u.System_Void, parameterTypes, new PackedCustomModifiers(), CallingConventions.Standard | CallingConventions.HasThis, 0);
            var mb = FindMethod(".ctor", methodSig) ?? u.GetMissingMethodOrThrow(null, this, ".ctor", methodSig);
            return (ConstructorInfo)mb;
        }

        public MethodBase __CreateMissingMethod(string name, CallingConventions callingConvention, Type returnType, CustomModifiers returnTypeCustomModifiers, Type[] parameterTypes, CustomModifiers[] parameterTypeCustomModifiers) =>
            CreateMissingMethod(name, callingConvention, returnType, parameterTypes, PackedCustomModifiers.CreateFromExternal(returnTypeCustomModifiers, parameterTypeCustomModifiers, parameterTypes.Length));

        MethodBase CreateMissingMethod(string name, CallingConventions callingConvention, Type returnType, Type[] parameterTypes, PackedCustomModifiers customModifiers)
        {
            var sig = new MethodSignature(returnType ?? Module.Universe.System_Void, Util.Copy(parameterTypes), customModifiers, callingConvention, 0);
            var method = new MissingMethod(this, name, sig);

            if (name == ".ctor" || name == ".cctor")
                return new ConstructorInfoImpl(method);

            return method;
        }

        internal virtual Type SetMetadataTokenForMissing(int token, int flags) => this;

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

        internal bool IsEnumOrValueType
        {
            get
            {
                return (typeFlags & (TypeFlags.EnumOrValueType | TypeFlags.PotentialEnumOrValueType)) != 0
                    && ((typeFlags & TypeFlags.EnumOrValueType) != 0 || ResolvePotentialEnumOrValueType());
            }
        }

        internal virtual Universe Universe => Module.Universe;

        internal sealed override bool BindingFlagsMatch(BindingFlags flags) =>
            BindingFlagsMatch(IsNestedPublic, flags, BindingFlags.Public, BindingFlags.NonPublic);

        internal sealed override MemberInfo SetReflectedType(Type type) => throw new InvalidOperationException();

        internal override int GetCurrentToken() => MetadataToken;

        internal sealed override List<CustomAttributeData> GetPseudoCustomAttributes(Type attributeType) =>
            // types don't have pseudo custom attributes
            null;

        // in .NET this is an extension method, but we target .NET 2.0, so we have an instance method
        public TypeInfo GetTypeInfo() => this as TypeInfo ?? throw new MissingMemberException(this);

    }

}
