/*
  Copyright (C) 2002-2015 Jeroen Frijters

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
using System.Diagnostics;

using IKVM.Attributes;

#if IMPORTER || EXPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif

#if IMPORTER
using IKVM.Tools.Importer;
#endif

namespace IKVM.Runtime
{

    sealed class RuntimeManagedJavaType : RuntimeJavaType
    {

        const string NamePrefix = "cli.";

        internal const string DelegateInterfaceSuffix = "$Method";
        internal const string AttributeAnnotationSuffix = "$Annotation";
        internal const string AttributeAnnotationReturnValueSuffix = "$__ReturnValue";
        internal const string AttributeAnnotationMultipleSuffix = "$__Multiple";
        internal const string EnumEnumSuffix = "$__Enum";
        internal const string GenericEnumEnumTypeName = "ikvm.internal.EnumEnum`1";
        internal const string GenericDelegateInterfaceTypeName = "ikvm.internal.DelegateInterface`1";
        internal const string GenericAttributeAnnotationTypeName = "ikvm.internal.AttributeAnnotation`1";
        internal const string GenericAttributeAnnotationReturnValueTypeName = "ikvm.internal.AttributeAnnotationReturnValue`1";
        internal const string GenericAttributeAnnotationMultipleTypeName = "ikvm.internal.AttributeAnnotationMultiple`1";

        static readonly Dictionary<Type, RuntimeJavaType> types = new Dictionary<Type, RuntimeJavaType>();
        readonly Type type;
        RuntimeJavaType baseTypeWrapper;
        volatile RuntimeJavaType[] innerClasses;
        RuntimeJavaType outerClass;
        volatile RuntimeJavaType[] interfaces;

        private static Modifiers GetModifiers(Type type)
        {
            Modifiers modifiers = 0;
            if (type.IsPublic)
            {
                modifiers |= Modifiers.Public;
            }
            else if (type.IsNestedPublic)
            {
                modifiers |= Modifiers.Static;
                if (type.IsVisible)
                {
                    modifiers |= Modifiers.Public;
                }
            }
            else if (type.IsNestedPrivate)
            {
                modifiers |= Modifiers.Private | Modifiers.Static;
            }
            else if (type.IsNestedFamily || type.IsNestedFamORAssem)
            {
                modifiers |= Modifiers.Protected | Modifiers.Static;
            }
            else if (type.IsNestedAssembly || type.IsNestedFamANDAssem)
            {
                modifiers |= Modifiers.Static;
            }

            if (type.IsSealed)
            {
                modifiers |= Modifiers.Final;
            }
            else if (type.IsAbstract) // we can't be abstract if we're final
            {
                modifiers |= Modifiers.Abstract;
            }
            if (type.IsInterface)
            {
                modifiers |= Modifiers.Interface;
            }

            return modifiers;
        }

        // NOTE when this is called on a remapped type, the "warped" underlying type name is returned.
        // E.g. GetName(typeof(object)) returns "cli.System.Object".
        internal static string GetName(Type type)
        {
            Debug.Assert(!type.Name.EndsWith("[]") && !AttributeHelper.IsJavaModule(type.Module));

            var name = type.FullName;
            if (name == null)
            {
                // generic type parameters don't have a full name
                return null;
            }

            if (type.IsGenericType && !type.ContainsGenericParameters)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(MangleTypeName(type.GetGenericTypeDefinition().FullName));
                sb.Append("_$$$_");
                string sep = "";
                foreach (Type t1 in type.GetGenericArguments())
                {
                    Type t = t1;
                    sb.Append(sep);
                    // NOTE we can't use ClassLoaderWrapper.GetWrapperFromType() here to get t's name,
                    // because we might be resolving a generic type that refers to a type that is in
                    // the process of being constructed.
                    //
                    // For example:
                    //   class Base<T> { } 
                    //   class Derived : Base<Derived> { }
                    //
                    while (ReflectUtil.IsVector(t))
                    {
                        t = t.GetElementType();
                        sb.Append('A');
                    }
                    if (RuntimePrimitiveJavaType.IsPrimitiveType(t))
                    {
                        sb.Append(RuntimeClassLoader.GetWrapperFromType(t).SigName);
                    }
                    else
                    {
                        string s;
                        if (RuntimeClassLoader.IsRemappedType(t) || AttributeHelper.IsJavaModule(t.Module))
                        {
                            s = RuntimeClassLoader.GetWrapperFromType(t).Name;
                        }
                        else
                        {
                            s = RuntimeManagedJavaType.GetName(t);
                        }
                        // only do the mangling for non-generic types (because we don't want to convert
                        // the double underscores in two adjacent _$$$_ or _$$$$_ markers)
                        if (s.IndexOf("_$$$_") == -1)
                        {
                            s = s.Replace("__", "$$005F$$005F");
                            s = s.Replace(".", "__");
                        }
                        sb.Append('L').Append(s);
                    }
                    sep = "_$$_";
                }
                sb.Append("_$$$$_");
                return sb.ToString();
            }

            if (AttributeHelper.IsNoPackagePrefix(type) && name.IndexOf('$') == -1)
            {
                return name.Replace('+', '$');
            }

            return MangleTypeName(name);
        }

        static string MangleTypeName(string name)
        {
            var sb = new System.Text.StringBuilder(NamePrefix, NamePrefix.Length + name.Length);
            bool escape = false;
            bool nested = false;
            for (int i = 0; i < name.Length; i++)
            {
                char c = name[i];
                if (c == '+' && !escape && (sb.Length == 0 || sb[sb.Length - 1] != '$'))
                {
                    nested = true;
                    sb.Append('$');
                }
                else if ("_0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(c) != -1
                    || (c == '.' && !escape && !nested))
                {
                    sb.Append(c);
                }
                else
                {
                    sb.Append("$$");
                    sb.Append(string.Format("{0:X4}", (int)c));
                }
                if (c == '\\')
                {
                    escape = !escape;
                }
                else
                {
                    escape = false;
                }
            }

            return sb.ToString();
        }

        // NOTE if the name is not a valid mangled type name, no demangling is done and the
        // original string is returned
        // NOTE we don't enforce canonical form, this is not required, because we cannot
        // guarantee it for unprefixed names anyway, so the caller is responsible for
        // ensuring that the original name was in fact the canonical name.
        internal static string DemangleTypeName(string name)
        {
            if (!name.StartsWith(NamePrefix))
            {
                return name.Replace('$', '+');
            }
            System.Text.StringBuilder sb = new System.Text.StringBuilder(name.Length - NamePrefix.Length);
            for (int i = NamePrefix.Length; i < name.Length; i++)
            {
                char c = name[i];
                if (c == '$')
                {
                    if (i + 1 < name.Length && name[i + 1] != '$')
                    {
                        sb.Append('+');
                    }
                    else
                    {
                        i++;
                        if (i + 5 > name.Length)
                        {
                            return name;
                        }
                        int digit0 = "0123456789ABCDEF".IndexOf(name[++i]);
                        int digit1 = "0123456789ABCDEF".IndexOf(name[++i]);
                        int digit2 = "0123456789ABCDEF".IndexOf(name[++i]);
                        int digit3 = "0123456789ABCDEF".IndexOf(name[++i]);
                        if (digit0 == -1 || digit1 == -1 || digit2 == -1 || digit3 == -1)
                        {
                            return name;
                        }
                        sb.Append((char)((digit0 << 12) + (digit1 << 8) + (digit2 << 4) + digit3));
                    }
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        // TODO from a perf pov it may be better to allow creation of TypeWrappers,
        // but to simply make sure they don't have ClassObject
        internal static bool IsAllowedOutside(Type type)
        {
            // SECURITY we never expose types from IKVM.Runtime, because doing so would lead to a security hole,
            // since the reflection implementation lives inside this assembly, all internal members would
            // be accessible through Java reflection.
#if !FIRST_PASS && !IMPORTER && !EXPORTER
            if (type.Assembly == typeof(RuntimeManagedJavaType).Assembly)
            {
                return false;
            }
            if (type.Assembly == IKVM.Java.Externs.java.lang.SecurityManager.jniAssembly)
            {
                return false;
            }
#endif
            return true;
        }


        /// <summary>
        /// We allow open generic types to be visible to Java code as very limited classes (or interfaces).
        /// They are always package private and have the abstract and final modifiers set, this makes them
        /// inaccessible and invalid from a Java point of view. The intent is to avoid any usage of these
        /// classes. They exist solely for the purpose of stack walking, because the .NET runtime will report
        /// open generic types when walking the stack (as a performance optimization). We cannot (reliably) map
        /// these classes to their instantiations, so we report the open generic type class instead.
        /// Note also that these classes can only be used as a "handle" to the type, they expose no members,
        /// don't implement any interfaces and the base class is always object.
        /// </summary>
        sealed class OpenGenericJavaType : RuntimeJavaType
        {

            readonly Type type;

            private static Modifiers GetModifiers(Type type)
            {
                Modifiers modifiers = Modifiers.Abstract | Modifiers.Final;
                if (type.IsInterface)
                {
                    modifiers |= Modifiers.Interface;
                }
                return modifiers;
            }

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="type"></param>
            /// <param name="name"></param>
            internal OpenGenericJavaType(Type type, string name) :
                base(TypeFlags.None, GetModifiers(type), name)
            {
                this.type = type;
            }

            internal override RuntimeJavaType BaseTypeWrapper => type.IsInterface ? null : CoreClasses.java.lang.Object.Wrapper;

            internal override Type TypeAsTBD => type;

            internal override RuntimeClassLoader GetClassLoader()
            {
                return RuntimeAssemblyClassLoader.FromAssembly(type.Assembly);
            }

            protected override void LazyPublishMembers()
            {
                SetFields(Array.Empty<RuntimeJavaField>());
                SetMethods(Array.Empty<RuntimeJavaMethod>());
            }

        }

        internal abstract class FakeJavaType : RuntimeJavaType
        {

            readonly RuntimeJavaType baseWrapper;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="modifiers"></param>
            /// <param name="name"></param>
            /// <param name="baseWrapper"></param>
            protected FakeJavaType(Modifiers modifiers, string name, RuntimeJavaType baseWrapper) :
                base(TypeFlags.None, modifiers, name)
            {
                this.baseWrapper = baseWrapper;
            }

            internal sealed override RuntimeJavaType BaseTypeWrapper => baseWrapper;

            internal sealed override bool IsFakeNestedType => true;

            internal sealed override Modifiers ReflectiveModifiers => Modifiers | Modifiers.Static;

        }

        sealed class DelegateInnerClassJavaType : FakeJavaType
        {

            readonly Type fakeType;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="delegateType"></param>
            internal DelegateInnerClassJavaType(string name, Type delegateType) :
                base(Modifiers.Public | Modifiers.Interface | Modifiers.Abstract, name, null)
            {
#if IMPORTER || EXPORTER
                this.fakeType = FakeTypes.GetDelegateType(delegateType);
#elif !FIRST_PASS
                this.fakeType = typeof(ikvm.@internal.DelegateInterface<>).MakeGenericType(delegateType);
#endif
                MethodInfo invoke = delegateType.GetMethod("Invoke");
                ParameterInfo[] parameters = invoke.GetParameters();
                RuntimeJavaType[] argTypeWrappers = new RuntimeJavaType[parameters.Length];
                System.Text.StringBuilder sb = new System.Text.StringBuilder("(");
                MemberFlags flags = MemberFlags.None;
                for (int i = 0; i < parameters.Length; i++)
                {
                    Type parameterType = parameters[i].ParameterType;
                    if (parameterType.IsByRef)
                    {
                        flags |= MemberFlags.DelegateInvokeWithByRefParameter;
                        parameterType = RuntimeArrayJavaType.MakeArrayType(parameterType.GetElementType(), 1);
                    }
                    argTypeWrappers[i] = RuntimeClassLoader.GetWrapperFromType(parameterType);
                    sb.Append(argTypeWrappers[i].SigName);
                }
                RuntimeJavaType returnType = RuntimeClassLoader.GetWrapperFromType(invoke.ReturnType);
                sb.Append(")").Append(returnType.SigName);
                RuntimeJavaMethod invokeMethod = new DynamicOnlyJavaMethod(this, "Invoke", sb.ToString(), returnType, argTypeWrappers, flags);
                SetMethods(new RuntimeJavaMethod[] { invokeMethod });
                SetFields(Array.Empty<RuntimeJavaField>());
            }

            internal override RuntimeJavaType DeclaringTypeWrapper => RuntimeClassLoader.GetWrapperFromType(fakeType.GetGenericArguments()[0]);

            internal override RuntimeClassLoader GetClassLoader()
            {
                return DeclaringTypeWrapper.GetClassLoader();
            }

            internal override Type TypeAsTBD => fakeType;

            internal override bool IsFastClassLiteralSafe => true;

            internal override MethodParametersEntry[] GetMethodParameters(RuntimeJavaMethod mw)
            {
                return DeclaringTypeWrapper.GetMethodParameters(DeclaringTypeWrapper.GetMethodWrapper(mw.Name, mw.Signature, false));
            }

        }

        class DynamicOnlyJavaMethod : RuntimeJavaMethod
        {

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="declaringType"></param>
            /// <param name="name"></param>
            /// <param name="sig"></param>
            /// <param name="returnType"></param>
            /// <param name="parameterTypes"></param>
            /// <param name="flags"></param>
            internal DynamicOnlyJavaMethod(RuntimeJavaType declaringType, string name, string sig, RuntimeJavaType returnType, RuntimeJavaType[] parameterTypes, MemberFlags flags) :
                base(declaringType, name, sig, null, returnType, parameterTypes, Modifiers.Public | Modifiers.Abstract, flags)
            {

            }

            internal sealed override bool IsDynamicOnly => true;

#if !IMPORTER && !FIRST_PASS && !EXPORTER

            [HideFromJava]
            internal sealed override object Invoke(object obj, object[] args)
            {
                // a DynamicOnlyMethodWrapper is an interface method, but now that we've been called on an actual object instance,
                // we can resolve to a real method and call that instead
                var tw = RuntimeJavaType.FromClass(IKVM.Java.Externs.ikvm.runtime.Util.getClassFromObject(obj));
                var mw = tw.GetMethodWrapper(this.Name, this.Signature, true);
                if (mw == null || mw.IsStatic)
                    throw new java.lang.AbstractMethodError(tw.Name + "." + this.Name + this.Signature);

                if (!mw.IsPublic)
                    throw new java.lang.IllegalAccessError(tw.Name + "." + this.Name + this.Signature);

                mw.Link();
                mw.ResolveMethod();
                return mw.Invoke(obj, args);
            }

#endif // !IMPORTER && !FIRST_PASS && !EXPORTER

        }

        sealed class EnumEnumJavaType : FakeJavaType
        {

            readonly Type fakeType;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="enumType"></param>
            internal EnumEnumJavaType(string name, Type enumType) :
                base(Modifiers.Public | Modifiers.Enum | Modifiers.Final, name, RuntimeClassLoader.LoadClassCritical("java.lang.Enum"))
            {
#if IMPORTER || EXPORTER
                this.fakeType = FakeTypes.GetEnumType(enumType);
#elif !FIRST_PASS
                this.fakeType = typeof(ikvm.@internal.EnumEnum<>).MakeGenericType(enumType);
#endif
            }

#if !IMPORTER && !EXPORTER && !FIRST_PASS
            internal object GetUnspecifiedValue()
            {
                return GetFieldWrapper("__unspecified", this.SigName).GetValue(null);
            }
#endif

            sealed class EnumJavaField : RuntimeJavaField
            {

#if !IMPORTER && !EXPORTER

                readonly int ordinal;
                object val;

#endif

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                /// <param name="tw"></param>
                /// <param name="name"></param>
                /// <param name="ordinal"></param>
                internal EnumJavaField(RuntimeJavaType tw, string name, int ordinal) :
                    base(tw, tw, name, tw.SigName, Modifiers.Public | Modifiers.Static | Modifiers.Final | Modifiers.Enum, null, MemberFlags.None)
                {
#if !IMPORTER && !EXPORTER
                    this.ordinal = ordinal;
#endif
                }

#if !IMPORTER && !EXPORTER && !FIRST_PASS

                internal override object GetValue(object obj)
                {
                    if (val == null)
                        System.Threading.Interlocked.CompareExchange(ref val, Activator.CreateInstance(this.DeclaringType.TypeAsTBD, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, new object[] { this.Name, ordinal }, null), null);

                    return val;
                }

                internal override void SetValue(object obj, object value)
                {

                }

#endif

#if EMITTERS
                protected override void EmitGetImpl(CodeEmitter ilgen)
                {
#if IMPORTER
                    var typeofByteCodeHelper = StaticCompiler.GetRuntimeType("IKVM.Runtime.ByteCodeHelper");
#else
                    var typeofByteCodeHelper = typeof(IKVM.Runtime.ByteCodeHelper);
#endif
                    ilgen.Emit(OpCodes.Ldstr, this.Name);
                    ilgen.Emit(OpCodes.Call, typeofByteCodeHelper.GetMethod("GetDotNetEnumField").MakeGenericMethod(this.DeclaringType.TypeAsBaseType));
                }

                protected override void EmitSetImpl(CodeEmitter ilgen)
                {
                    throw new NotImplementedException();
                }

#endif

            }

            sealed class EnumValuesJavaMethod : RuntimeJavaMethod
            {

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                /// <param name="declaringType"></param>
                internal EnumValuesJavaMethod(RuntimeJavaType declaringType) :
                    base(declaringType, "values", "()[" + declaringType.SigName, null, declaringType.MakeArrayType(1), Array.Empty<RuntimeJavaType>(), Modifiers.Public | Modifiers.Static, MemberFlags.None)
                {

                }

                internal override bool IsDynamicOnly => true;

#if !IMPORTER && !FIRST_PASS && !EXPORTER

                internal override object Invoke(object obj, object[] args)
                {
                    RuntimeJavaField[] values = this.DeclaringType.GetFields();
                    object[] array = (object[])Array.CreateInstance(this.DeclaringType.TypeAsArrayType, values.Length);
                    for (int i = 0; i < values.Length; i++)
                    {
                        array[i] = values[i].GetValue(null);
                    }
                    return array;
                }

#endif

            }

            sealed class EnumValueOfJavaMethod : RuntimeJavaMethod
            {

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                /// <param name="declaringType"></param>
                internal EnumValueOfJavaMethod(RuntimeJavaType declaringType) :
                    base(declaringType, "valueOf", "(Ljava.lang.String;)" + declaringType.SigName, null, declaringType, new RuntimeJavaType[] { CoreClasses.java.lang.String.Wrapper }, Modifiers.Public | Modifiers.Static, MemberFlags.None)
                {

                }

                internal override bool IsDynamicOnly => true;

#if !IMPORTER && !FIRST_PASS && !EXPORTER

                internal override object Invoke(object obj, object[] args)
                {
                    RuntimeJavaField[] values = this.DeclaringType.GetFields();
                    for (int i = 0; i < values.Length; i++)
                    {
                        if (values[i].Name.Equals(args[0]))
                        {
                            return values[i].GetValue(null);
                        }
                    }
                    throw new java.lang.IllegalArgumentException("" + args[0]);
                }

#endif

            }

            protected override void LazyPublishMembers()
            {
                List<RuntimeJavaField> fields = new List<RuntimeJavaField>();
                int ordinal = 0;
                foreach (FieldInfo field in this.DeclaringTypeWrapper.TypeAsTBD.GetFields(BindingFlags.Static | BindingFlags.Public))
                {
                    if (field.IsLiteral)
                    {
                        fields.Add(new EnumJavaField(this, field.Name, ordinal++));
                    }
                }
                // TODO if the enum already has an __unspecified value, rename this one
                fields.Add(new EnumJavaField(this, "__unspecified", ordinal++));
                SetFields(fields.ToArray());
                SetMethods(new RuntimeJavaMethod[] { new EnumValuesJavaMethod(this), new EnumValueOfJavaMethod(this) });
                base.LazyPublishMembers();
            }

            internal override RuntimeJavaType DeclaringTypeWrapper => RuntimeClassLoader.GetWrapperFromType(fakeType.GetGenericArguments()[0]);

            internal override RuntimeClassLoader GetClassLoader()
            {
                return DeclaringTypeWrapper.GetClassLoader();
            }

            internal override Type TypeAsTBD => fakeType;

            internal override bool IsFastClassLiteralSafe => true;

        }

        internal abstract class AttributeAnnotationJavaTypeBase : FakeJavaType
        {

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="name"></param>
            internal AttributeAnnotationJavaTypeBase(string name) :
                base(Modifiers.Public | Modifiers.Interface | Modifiers.Abstract | Modifiers.Annotation, name, null)
            {

            }

            internal sealed override RuntimeClassLoader GetClassLoader()
            {
                return DeclaringTypeWrapper.GetClassLoader();
            }

            internal sealed override RuntimeJavaType[] Interfaces => new RuntimeJavaType[] { RuntimeClassLoader.GetBootstrapClassLoader().LoadClassByDottedName("java.lang.annotation.Annotation") };

            internal sealed override bool IsFastClassLiteralSafe => true;

            internal abstract AttributeTargets AttributeTargets { get; }

        }

        sealed class AttributeAnnotationJavaType : AttributeAnnotationJavaTypeBase
        {

            readonly Type fakeType;
            readonly Type attributeType;
            volatile RuntimeJavaType[] innerClasses;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="name"></param>
            /// <param name="attributeType"></param>
            internal AttributeAnnotationJavaType(string name, Type attributeType) :
                base(name)
            {
#if IMPORTER || EXPORTER
                this.fakeType = FakeTypes.GetAttributeType(attributeType);
#elif !FIRST_PASS
                this.fakeType = typeof(ikvm.@internal.AttributeAnnotation<>).MakeGenericType(attributeType);
#endif
                this.attributeType = attributeType;
            }

            static bool IsSupportedType(Type type)
            {
                // Java annotations only support one-dimensional arrays
                if (ReflectUtil.IsVector(type))
                    type = type.GetElementType();

                return type == Types.String
                    || type == Types.Boolean
                    || type == Types.Byte
                    || type == Types.Char
                    || type == Types.Int16
                    || type == Types.Int32
                    || type == Types.Single
                    || type == Types.Int64
                    || type == Types.Double
                    || type == Types.Type
                    || type.IsEnum;
            }

            internal static void GetConstructors(Type type, out ConstructorInfo defCtor, out ConstructorInfo singleOneArgCtor)
            {
                defCtor = null;
                int oneArgCtorCount = 0;
                ConstructorInfo oneArgCtor = null;
                ConstructorInfo[] constructors = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
                // HACK we have a special rule to make some additional custom attributes from mscorlib usable:
                // Attributes that have two constructors, one an enum and another one taking a byte, short or int,
                // we only expose the enum constructor.
                if (constructors.Length == 2 && type.Assembly == Types.Object.Assembly)
                {
                    ParameterInfo[] p0 = constructors[0].GetParameters();
                    ParameterInfo[] p1 = constructors[1].GetParameters();
                    if (p0.Length == 1 && p1.Length == 1)
                    {
                        Type t0 = p0[0].ParameterType;
                        Type t1 = p1[0].ParameterType;
                        bool swapped = false;
                        if (t1.IsEnum)
                        {
                            Type tmp = t0;
                            t0 = t1;
                            t1 = tmp;
                            swapped = true;
                        }
                        if (t0.IsEnum && (t1 == Types.Byte || t1 == Types.Int16 || t1 == Types.Int32))
                        {
                            if (swapped)
                            {
                                singleOneArgCtor = constructors[1];
                            }
                            else
                            {
                                singleOneArgCtor = constructors[0];
                            }
                            return;
                        }
                    }
                }
                if (type.Assembly == Types.Object.Assembly)
                {
                    if (type.FullName == "System.Runtime.CompilerServices.MethodImplAttribute")
                    {
                        foreach (ConstructorInfo ci in constructors)
                        {
                            ParameterInfo[] p = ci.GetParameters();
                            if (p.Length == 1 && p[0].ParameterType.IsEnum)
                            {
                                singleOneArgCtor = ci;
                                return;
                            }
                        }
                    }
                }
                foreach (ConstructorInfo ci in constructors)
                {
                    ParameterInfo[] args = ci.GetParameters();
                    if (args.Length == 0)
                    {
                        defCtor = ci;
                    }
                    else if (args.Length == 1)
                    {
                        if (IsSupportedType(args[0].ParameterType))
                        {
                            oneArgCtor = ci;
                            oneArgCtorCount++;
                        }
                        else
                        {
                            // set to two to make sure we don't see the oneArgCtor as viable
                            oneArgCtorCount = 2;
                        }
                    }
                }
                singleOneArgCtor = oneArgCtorCount == 1 ? oneArgCtor : null;
            }

            sealed class AttributeAnnotationJavaMethod : DynamicOnlyJavaMethod
            {

                readonly bool optional;

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                /// <param name="tw"></param>
                /// <param name="name"></param>
                /// <param name="type"></param>
                /// <param name="optional"></param>
                internal AttributeAnnotationJavaMethod(AttributeAnnotationJavaType tw, string name, Type type, bool optional) :
                    this(tw, name, MapType(type, false), optional)
                {

                }

                static RuntimeJavaType MapType(Type type, bool isArray)
                {
                    if (type == Types.String)
                    {
                        return CoreClasses.java.lang.String.Wrapper;
                    }
                    else if (type == Types.Boolean)
                    {
                        return RuntimePrimitiveJavaType.BOOLEAN;
                    }
                    else if (type == Types.Byte)
                    {
                        return RuntimePrimitiveJavaType.BYTE;
                    }
                    else if (type == Types.Char)
                    {
                        return RuntimePrimitiveJavaType.CHAR;
                    }
                    else if (type == Types.Int16)
                    {
                        return RuntimePrimitiveJavaType.SHORT;
                    }
                    else if (type == Types.Int32)
                    {
                        return RuntimePrimitiveJavaType.INT;
                    }
                    else if (type == Types.Single)
                    {
                        return RuntimePrimitiveJavaType.FLOAT;
                    }
                    else if (type == Types.Int64)
                    {
                        return RuntimePrimitiveJavaType.LONG;
                    }
                    else if (type == Types.Double)
                    {
                        return RuntimePrimitiveJavaType.DOUBLE;
                    }
                    else if (type == Types.Type)
                    {
                        return CoreClasses.java.lang.Class.Wrapper;
                    }
                    else if (type.IsEnum)
                    {
                        foreach (RuntimeJavaType tw in RuntimeClassLoader.GetWrapperFromType(type).InnerClasses)
                        {
                            if (tw is EnumEnumJavaType)
                            {
                                if (!isArray && type.IsDefined(JVM.Import(typeof(FlagsAttribute)), false))
                                {
                                    return tw.MakeArrayType(1);
                                }
                                return tw;
                            }
                        }
                        throw new InvalidOperationException();
                    }
                    else if (!isArray && ReflectUtil.IsVector(type))
                    {
                        return MapType(type.GetElementType(), true).MakeArrayType(1);
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                /// <param name="tw"></param>
                /// <param name="name"></param>
                /// <param name="returnType"></param>
                /// <param name="optional"></param>
                private AttributeAnnotationJavaMethod(AttributeAnnotationJavaType tw, string name, RuntimeJavaType returnType, bool optional) :
                    base(tw, name, "()" + returnType.SigName, returnType, Array.Empty<RuntimeJavaType>(), MemberFlags.None)
                {
                    this.optional = optional;
                }

                internal override bool IsOptionalAttributeAnnotationValue
                {
                    get { return optional; }
                }
            }

            protected override void LazyPublishMembers()
            {
                var methods = new List<RuntimeJavaMethod>();
                GetConstructors(attributeType, out var defCtor, out var singleOneArgCtor);

                if (singleOneArgCtor != null)
                    methods.Add(new AttributeAnnotationJavaMethod(this, "value", singleOneArgCtor.GetParameters()[0].ParameterType, defCtor != null));

                foreach (var pi in attributeType.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    // the getter and setter methods both need to be public
                    // the getter signature must be: <PropertyType> Getter()
                    // the setter signature must be: void Setter(<PropertyType>)
                    // the property type needs to be a supported type
                    var getter = pi.GetGetMethod();
                    var setter = pi.GetSetMethod();
                    ParameterInfo[] parameters;
                    if (getter != null && getter.GetParameters().Length == 0 && getter.ReturnType == pi.PropertyType && setter != null && (parameters = setter.GetParameters()).Length == 1 && parameters[0].ParameterType == pi.PropertyType && setter.ReturnType == Types.Void && IsSupportedType(pi.PropertyType))
                        AddMethodIfUnique(methods, new AttributeAnnotationJavaMethod(this, pi.Name, pi.PropertyType, true));
                }

                foreach (var fi in attributeType.GetFields(BindingFlags.Public | BindingFlags.Instance))
                    if (!fi.IsInitOnly && IsSupportedType(fi.FieldType))
                        AddMethodIfUnique(methods, new AttributeAnnotationJavaMethod(this, fi.Name, fi.FieldType, true));

                SetMethods(methods.ToArray());

                base.LazyPublishMembers();
            }

            static void AddMethodIfUnique(List<RuntimeJavaMethod> methods, RuntimeJavaMethod method)
            {
                foreach (var mw in methods)
                {
                    if (mw.Name == method.Name && mw.Signature == method.Signature)
                    {
                        // ignore duplicate
                        return;
                    }
                }

                methods.Add(method);
            }

#if !IMPORTER && !FIRST_PASS && !EXPORTER

            internal override object GetAnnotationDefault(RuntimeJavaMethod mw)
            {
                if (mw.IsOptionalAttributeAnnotationValue)
                {
                    if (mw.ReturnType == RuntimePrimitiveJavaType.BOOLEAN)
                    {
                        return java.lang.Boolean.FALSE;
                    }
                    else if (mw.ReturnType == RuntimePrimitiveJavaType.BYTE)
                    {
                        return java.lang.Byte.valueOf((byte)0);
                    }
                    else if (mw.ReturnType == RuntimePrimitiveJavaType.CHAR)
                    {
                        return java.lang.Character.valueOf((char)0);
                    }
                    else if (mw.ReturnType == RuntimePrimitiveJavaType.SHORT)
                    {
                        return java.lang.Short.valueOf((short)0);
                    }
                    else if (mw.ReturnType == RuntimePrimitiveJavaType.INT)
                    {
                        return java.lang.Integer.valueOf(0);
                    }
                    else if (mw.ReturnType == RuntimePrimitiveJavaType.FLOAT)
                    {
                        return java.lang.Float.valueOf(0F);
                    }
                    else if (mw.ReturnType == RuntimePrimitiveJavaType.LONG)
                    {
                        return java.lang.Long.valueOf(0L);
                    }
                    else if (mw.ReturnType == RuntimePrimitiveJavaType.DOUBLE)
                    {
                        return java.lang.Double.valueOf(0D);
                    }
                    else if (mw.ReturnType == CoreClasses.java.lang.String.Wrapper)
                    {
                        return "";
                    }
                    else if (mw.ReturnType == CoreClasses.java.lang.Class.Wrapper)
                    {
                        return (java.lang.Class)typeof(ikvm.@internal.__unspecified);
                    }
                    else if (mw.ReturnType is EnumEnumJavaType)
                    {
                        EnumEnumJavaType eetw = (EnumEnumJavaType)mw.ReturnType;
                        return eetw.GetUnspecifiedValue();
                    }
                    else if (mw.ReturnType.IsArray)
                    {
                        return Array.CreateInstance(mw.ReturnType.TypeAsArrayType, 0);
                    }
                }

                return null;
            }

#endif // !IMPORTER && !FIRST_PASS && !EXPORTER

            internal override RuntimeJavaType DeclaringTypeWrapper => RuntimeClassLoader.GetWrapperFromType(attributeType);

            internal override Type TypeAsTBD => fakeType;

            sealed class ReturnValueAnnotationJavaType : AttributeAnnotationJavaTypeBase
            {

                readonly Type fakeType;
                readonly AttributeAnnotationJavaType declaringType;

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                /// <param name="declaringType"></param>
                internal ReturnValueAnnotationJavaType(AttributeAnnotationJavaType declaringType) :
                    base(declaringType.Name + AttributeAnnotationReturnValueSuffix)
                {
#if IMPORTER || EXPORTER
                    this.fakeType = FakeTypes.GetAttributeReturnValueType(declaringType.attributeType);
#elif !FIRST_PASS
                    this.fakeType = typeof(ikvm.@internal.AttributeAnnotationReturnValue<>).MakeGenericType(declaringType.attributeType);
#endif
                    this.declaringType = declaringType;
                }

                protected override void LazyPublishMembers()
                {
                    RuntimeJavaType tw = declaringType;
                    if (declaringType.GetAttributeUsage().AllowMultiple)
                    {
                        tw = tw.MakeArrayType(1);
                    }
                    SetMethods(new RuntimeJavaMethod[] { new DynamicOnlyJavaMethod(this, "value", "()" + tw.SigName, tw, Array.Empty<RuntimeJavaType>(), MemberFlags.None) });
                    SetFields(Array.Empty<RuntimeJavaField>());
                }

                internal override RuntimeJavaType DeclaringTypeWrapper
                {
                    get
                    {
                        return declaringType;
                    }
                }

                internal override Type TypeAsTBD
                {
                    get
                    {
                        return fakeType;
                    }
                }

#if !IMPORTER && !FIRST_PASS && !EXPORTER
                internal override object[] GetDeclaredAnnotations()
                {
                    java.util.HashMap targetMap = new java.util.HashMap();
                    targetMap.put("value", new java.lang.annotation.ElementType[] { java.lang.annotation.ElementType.METHOD });
                    java.util.HashMap retentionMap = new java.util.HashMap();
                    retentionMap.put("value", java.lang.annotation.RetentionPolicy.RUNTIME);
                    return new object[] {
                        java.lang.reflect.Proxy.newProxyInstance(null, new java.lang.Class[] { typeof(java.lang.annotation.Target) }, new sun.reflect.annotation.AnnotationInvocationHandler(typeof(java.lang.annotation.Target), targetMap)),
                        java.lang.reflect.Proxy.newProxyInstance(null, new java.lang.Class[] { typeof(java.lang.annotation.Retention) }, new sun.reflect.annotation.AnnotationInvocationHandler(typeof(java.lang.annotation.Retention), retentionMap))
                    };
                }
#endif

                sealed class ReturnValueAnnotation : Annotation
                {

                    readonly AttributeAnnotationJavaType type;

                    /// <summary>
                    /// Initializes a new instance.
                    /// </summary>
                    /// <param name="type"></param>
                    internal ReturnValueAnnotation(AttributeAnnotationJavaType type)
                    {
                        this.type = type;
                    }

                    internal override void ApplyReturnValue(RuntimeClassLoader loader, MethodBuilder mb, ref ParameterBuilder pb, object annotation)
                    {
                        // TODO make sure the descriptor is correct
                        Annotation ann = type.Annotation;
                        object[] arr = (object[])annotation;
                        for (int i = 2; i < arr.Length; i += 2)
                        {
                            if ("value".Equals(arr[i]))
                            {
                                if (pb == null)
                                {
                                    pb = mb.DefineParameter(0, ParameterAttributes.None, null);
                                }
                                object[] value = (object[])arr[i + 1];
                                if (value[0].Equals(AnnotationDefaultAttribute.TAG_ANNOTATION))
                                {
                                    ann.Apply(loader, pb, value);
                                }
                                else
                                {
                                    for (int j = 1; j < value.Length; j++)
                                    {
                                        ann.Apply(loader, pb, value[j]);
                                    }
                                }
                                break;
                            }
                        }
                    }

                    internal override void Apply(RuntimeClassLoader loader, MethodBuilder mb, object annotation)
                    {
                    }

                    internal override void Apply(RuntimeClassLoader loader, AssemblyBuilder ab, object annotation)
                    {
                    }

                    internal override void Apply(RuntimeClassLoader loader, FieldBuilder fb, object annotation)
                    {
                    }

                    internal override void Apply(RuntimeClassLoader loader, ParameterBuilder pb, object annotation)
                    {
                    }

                    internal override void Apply(RuntimeClassLoader loader, TypeBuilder tb, object annotation)
                    {
                    }

                    internal override void Apply(RuntimeClassLoader loader, PropertyBuilder pb, object annotation)
                    {
                    }

                    internal override bool IsCustomAttribute
                    {
                        get { return type.Annotation.IsCustomAttribute; }
                    }
                }

                internal override Annotation Annotation => new ReturnValueAnnotation(declaringType);

                internal override AttributeTargets AttributeTargets => AttributeTargets.ReturnValue;

            }

            sealed class MultipleAnnotationJavaType : AttributeAnnotationJavaTypeBase
            {

                readonly Type fakeType;
                readonly AttributeAnnotationJavaType declaringType;

                internal MultipleAnnotationJavaType(AttributeAnnotationJavaType declaringType)
                    : base(declaringType.Name + AttributeAnnotationMultipleSuffix)
                {
#if IMPORTER || EXPORTER
                    this.fakeType = FakeTypes.GetAttributeMultipleType(declaringType.attributeType);
#elif !FIRST_PASS
                    this.fakeType = typeof(ikvm.@internal.AttributeAnnotationMultiple<>).MakeGenericType(declaringType.attributeType);
#endif
                    this.declaringType = declaringType;
                }

                protected override void LazyPublishMembers()
                {
                    RuntimeJavaType tw = declaringType.MakeArrayType(1);
                    SetMethods(new RuntimeJavaMethod[] { new DynamicOnlyJavaMethod(this, "value", "()" + tw.SigName, tw, Array.Empty<RuntimeJavaType>(), MemberFlags.None) });
                    SetFields(Array.Empty<RuntimeJavaField>());
                }

                internal override RuntimeJavaType DeclaringTypeWrapper => declaringType;

                internal override Type TypeAsTBD => fakeType;

#if !IMPORTER && !EXPORTER
                internal override object[] GetDeclaredAnnotations()
                {
                    return declaringType.GetDeclaredAnnotations();
                }
#endif

                sealed class MultipleAnnotation : Annotation
                {

                    readonly AttributeAnnotationJavaType type;

                    /// <summary>
                    /// Initializes a new instance.
                    /// </summary>
                    /// <param name="type"></param>
                    internal MultipleAnnotation(AttributeAnnotationJavaType type)
                    {
                        this.type = type;
                    }

                    static object[] UnwrapArray(object annotation)
                    {
                        // TODO make sure the descriptor is correct
                        object[] arr = (object[])annotation;
                        for (int i = 2; i < arr.Length; i += 2)
                        {
                            if ("value".Equals(arr[i]))
                            {
                                object[] value = (object[])arr[i + 1];
                                object[] rc = new object[value.Length - 1];
                                Array.Copy(value, 1, rc, 0, rc.Length);
                                return rc;
                            }
                        }

                        return new object[0];
                    }

                    internal override void Apply(RuntimeClassLoader loader, MethodBuilder mb, object annotation)
                    {
                        Annotation annot = type.Annotation;
                        foreach (object ann in UnwrapArray(annotation))
                        {
                            annot.Apply(loader, mb, ann);
                        }
                    }

                    internal override void Apply(RuntimeClassLoader loader, AssemblyBuilder ab, object annotation)
                    {
                        Annotation annot = type.Annotation;
                        foreach (object ann in UnwrapArray(annotation))
                        {
                            annot.Apply(loader, ab, ann);
                        }
                    }

                    internal override void Apply(RuntimeClassLoader loader, FieldBuilder fb, object annotation)
                    {
                        Annotation annot = type.Annotation;
                        foreach (object ann in UnwrapArray(annotation))
                        {
                            annot.Apply(loader, fb, ann);
                        }
                    }

                    internal override void Apply(RuntimeClassLoader loader, ParameterBuilder pb, object annotation)
                    {
                        Annotation annot = type.Annotation;
                        foreach (object ann in UnwrapArray(annotation))
                        {
                            annot.Apply(loader, pb, ann);
                        }
                    }

                    internal override void Apply(RuntimeClassLoader loader, TypeBuilder tb, object annotation)
                    {
                        Annotation annot = type.Annotation;
                        foreach (object ann in UnwrapArray(annotation))
                        {
                            annot.Apply(loader, tb, ann);
                        }
                    }

                    internal override void Apply(RuntimeClassLoader loader, PropertyBuilder pb, object annotation)
                    {
                        Annotation annot = type.Annotation;
                        foreach (object ann in UnwrapArray(annotation))
                        {
                            annot.Apply(loader, pb, ann);
                        }
                    }

                    internal override bool IsCustomAttribute
                    {
                        get { return type.Annotation.IsCustomAttribute; }
                    }

                }

                internal override Annotation Annotation => new MultipleAnnotation(declaringType);

                internal override AttributeTargets AttributeTargets => declaringType.AttributeTargets;

            }

            internal override RuntimeJavaType[] InnerClasses => innerClasses ??= GetInnerClasses();

            RuntimeJavaType[] GetInnerClasses()
            {
                List<RuntimeJavaType> list = new List<RuntimeJavaType>();
                AttributeUsageAttribute attr = GetAttributeUsage();
                if ((attr.ValidOn & AttributeTargets.ReturnValue) != 0)
                {
                    list.Add(GetClassLoader().RegisterInitiatingLoader(new ReturnValueAnnotationJavaType(this)));
                }
                if (attr.AllowMultiple)
                {
                    list.Add(GetClassLoader().RegisterInitiatingLoader(new MultipleAnnotationJavaType(this)));
                }
                return list.ToArray();
            }

            internal override bool IsFakeTypeContainer => true;

            AttributeUsageAttribute GetAttributeUsage()
            {
                AttributeTargets validOn = AttributeTargets.All;
                bool allowMultiple = false;
                bool inherited = true;
                foreach (CustomAttributeData cad in CustomAttributeData.GetCustomAttributes(attributeType))
                {
                    if (cad.Constructor.DeclaringType == JVM.Import(typeof(AttributeUsageAttribute)))
                    {
                        if (cad.ConstructorArguments.Count == 1 && cad.ConstructorArguments[0].ArgumentType == JVM.Import(typeof(AttributeTargets)))
                        {
                            validOn = (AttributeTargets)cad.ConstructorArguments[0].Value;
                        }
                        foreach (CustomAttributeNamedArgument cana in cad.NamedArguments)
                        {
                            if (cana.MemberInfo.Name == "AllowMultiple")
                            {
                                allowMultiple = (bool)cana.TypedValue.Value;
                            }
                            else if (cana.MemberInfo.Name == "Inherited")
                            {
                                inherited = (bool)cana.TypedValue.Value;
                            }
                        }
                    }
                }
                AttributeUsageAttribute attr = new AttributeUsageAttribute(validOn);
                attr.AllowMultiple = allowMultiple;
                attr.Inherited = inherited;
                return attr;
            }

#if !IMPORTER && !FIRST_PASS && !EXPORTER
            internal override object[] GetDeclaredAnnotations()
            {
                // note that AttributeUsageAttribute.Inherited does not map to java.lang.annotation.Inherited
                AttributeTargets validOn = GetAttributeUsage().ValidOn;
                List<java.lang.annotation.ElementType> targets = new List<java.lang.annotation.ElementType>();
                if ((validOn & (AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Delegate | AttributeTargets.Assembly)) != 0)
                {
                    targets.Add(java.lang.annotation.ElementType.TYPE);
                }
                if ((validOn & AttributeTargets.Constructor) != 0)
                {
                    targets.Add(java.lang.annotation.ElementType.CONSTRUCTOR);
                }
                if ((validOn & AttributeTargets.Field) != 0)
                {
                    targets.Add(java.lang.annotation.ElementType.FIELD);
                }
                if ((validOn & AttributeTargets.Method) != 0)
                {
                    targets.Add(java.lang.annotation.ElementType.METHOD);
                }
                if ((validOn & AttributeTargets.Parameter) != 0)
                {
                    targets.Add(java.lang.annotation.ElementType.PARAMETER);
                }
                java.util.HashMap targetMap = new java.util.HashMap();
                targetMap.put("value", targets.ToArray());
                java.util.HashMap retentionMap = new java.util.HashMap();
                retentionMap.put("value", java.lang.annotation.RetentionPolicy.RUNTIME);
                return new object[] {
                    java.lang.reflect.Proxy.newProxyInstance(null, new java.lang.Class[] { typeof(java.lang.annotation.Target) }, new sun.reflect.annotation.AnnotationInvocationHandler(typeof(java.lang.annotation.Target), targetMap)),
                    java.lang.reflect.Proxy.newProxyInstance(null, new java.lang.Class[] { typeof(java.lang.annotation.Retention) }, new sun.reflect.annotation.AnnotationInvocationHandler(typeof(java.lang.annotation.Retention), retentionMap))
                };
            }
#endif

            sealed class AttributeAnnotation : Annotation
            {

                readonly Type type;

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                /// <param name="type"></param>
                internal AttributeAnnotation(Type type)
                {
                    this.type = type;
                }

                CustomAttributeBuilder MakeCustomAttributeBuilder(RuntimeClassLoader loader, object annotation)
                {
                    object[] arr = (object[])annotation;
                    ConstructorInfo defCtor;
                    ConstructorInfo singleOneArgCtor;
                    object ctorArg = null;
                    GetConstructors(type, out defCtor, out singleOneArgCtor);
                    List<PropertyInfo> properties = new List<PropertyInfo>();
                    List<object> propertyValues = new List<object>();
                    List<FieldInfo> fields = new List<FieldInfo>();
                    List<object> fieldValues = new List<object>();
                    for (int i = 2; i < arr.Length; i += 2)
                    {
                        string name = (string)arr[i];
                        if (name == "value" && singleOneArgCtor != null)
                        {
                            ctorArg = ConvertValue(loader, singleOneArgCtor.GetParameters()[0].ParameterType, arr[i + 1]);
                        }
                        else
                        {
                            PropertyInfo pi = type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);
                            if (pi != null)
                            {
                                properties.Add(pi);
                                propertyValues.Add(ConvertValue(loader, pi.PropertyType, arr[i + 1]));
                            }
                            else
                            {
                                FieldInfo fi = type.GetField(name, BindingFlags.Public | BindingFlags.Instance);
                                if (fi != null)
                                {
                                    fields.Add(fi);
                                    fieldValues.Add(ConvertValue(loader, fi.FieldType, arr[i + 1]));
                                }
                            }
                        }
                    }
                    if (ctorArg == null && defCtor == null)
                    {
                        // TODO required argument is missing
                    }
                    return new CustomAttributeBuilder(ctorArg == null ? defCtor : singleOneArgCtor,
                        ctorArg == null ? new object[0] : new object[] { ctorArg },
                        properties.ToArray(),
                        propertyValues.ToArray(),
                        fields.ToArray(),
                        fieldValues.ToArray());
                }

                internal override void Apply(RuntimeClassLoader loader, TypeBuilder tb, object annotation)
                {
                    if (type == JVM.Import(typeof(System.Runtime.InteropServices.StructLayoutAttribute)) && tb.BaseType != Types.Object)
                    {
                        // we have to handle this explicitly, because if we apply an illegal StructLayoutAttribute,
                        // TypeBuilder.CreateType() will later on throw an exception.
#if IMPORTER
                        loader.IssueMessage(Message.IgnoredCustomAttribute, type.FullName, "Type '" + tb.FullName + "' does not extend cli.System.Object");
#else
                        Tracer.Error(Tracer.Runtime, "StructLayoutAttribute cannot be applied to {0}, because it does not directly extend cli.System.Object", tb.FullName);
#endif
                        return;
                    }

                    tb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
                }

                internal override void Apply(RuntimeClassLoader loader, MethodBuilder mb, object annotation)
                {
                    mb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
                }

                internal override void Apply(RuntimeClassLoader loader, FieldBuilder fb, object annotation)
                {
                    fb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
                }

                internal override void Apply(RuntimeClassLoader loader, ParameterBuilder pb, object annotation)
                {
                    // TODO with the current custom attribute annotation restrictions it is impossible to use this CA,
                    // but if we make it possible, we should also implement it here
                    if (type == JVM.Import(typeof(System.Runtime.InteropServices.DefaultParameterValueAttribute)))
                        throw new NotImplementedException();
                    else
                        pb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
                }

                internal override void Apply(RuntimeClassLoader loader, AssemblyBuilder ab, object annotation)
                {
#if IMPORTER
                    if (type == JVM.Import(typeof(System.Runtime.CompilerServices.TypeForwardedToAttribute)))
                    {
                        ab.__AddTypeForwarder((Type)ConvertValue(loader, Types.Type, ((object[])annotation)[3]));
                    }
                    else if (type == JVM.Import(typeof(System.Reflection.AssemblyVersionAttribute)))
                    {
                        string str = (string)ConvertValue(loader, Types.String, ((object[])annotation)[3]);
                        Version version;
                        if (IkvmImporterInternal.TryParseVersion(str, out version))
                        {
                            ab.__SetAssemblyVersion(version);
                        }
                        else
                        {
                            loader.IssueMessage(Message.InvalidCustomAttribute, type.FullName, "The version '" + str + "' is invalid.");
                        }
                    }
                    else if (type == JVM.Import(typeof(System.Reflection.AssemblyCultureAttribute)))
                    {
                        string str = (string)ConvertValue(loader, Types.String, ((object[])annotation)[3]);
                        if (str != "")
                        {
                            ab.__SetAssemblyCulture(str);
                        }
                    }
                    else if (type == JVM.Import(typeof(System.Reflection.AssemblyDelaySignAttribute))
                        || type == JVM.Import(typeof(System.Reflection.AssemblyKeyFileAttribute))
                        || type == JVM.Import(typeof(System.Reflection.AssemblyKeyNameAttribute)))
                    {
                        loader.IssueMessage(Message.IgnoredCustomAttribute, type.FullName, "Please use the corresponding compiler switch.");
                    }
                    else if (type == JVM.Import(typeof(System.Reflection.AssemblyAlgorithmIdAttribute)))
                    {
                        // this attribute is currently not exposed as an annotation and isn't very interesting
                        throw new NotImplementedException();
                    }
                    else if (type == JVM.Import(typeof(System.Reflection.AssemblyFlagsAttribute)))
                    {
                        // this attribute is currently not exposed as an annotation and isn't very interesting
                        throw new NotImplementedException();
                    }
                    else
                    {
                        ab.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
                    }
#else
                    ab.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
#endif
                }

                internal override void Apply(RuntimeClassLoader loader, PropertyBuilder pb, object annotation)
                {
                    pb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
                }

                internal override bool IsCustomAttribute
                {
                    get { return true; }
                }

            }

            internal override Annotation Annotation
            {
                get
                {
                    return new AttributeAnnotation(attributeType);
                }
            }

            internal override AttributeTargets AttributeTargets
            {
                get { return GetAttributeUsage().ValidOn; }
            }

        }

        internal static RuntimeJavaType GetWrapperFromDotNetType(Type type)
        {
            RuntimeJavaType tw;
            lock (types)
                types.TryGetValue(type, out tw);

            if (tw == null)
            {
                tw = RuntimeAssemblyClassLoader.FromAssembly(type.Assembly).GetWrapperFromAssemblyType(type);
                lock (types)
                    types[type] = tw;
            }

            return tw;
        }

        static RuntimeJavaType GetBaseTypeWrapper(Type type)
        {
            if (type.IsInterface)
            {
                return null;
            }
            else if (RuntimeClassLoader.IsRemappedType(type))
            {
                // Remapped types extend their alter ego
                // (e.g. cli.System.Object must appear to be derived from java.lang.Object)
                // except when they're sealed, of course.
                if (type.IsSealed)
                {
                    return CoreClasses.java.lang.Object.Wrapper;
                }
                return RuntimeClassLoader.GetWrapperFromType(type);
            }
            else if (RuntimeClassLoader.IsRemappedType(type.BaseType))
            {
                return GetWrapperFromDotNetType(type.BaseType);
            }
            else
            {
                return RuntimeClassLoader.GetWrapperFromType(type.BaseType);
            }
        }

        internal static RuntimeJavaType Create(Type type, string name)
        {
            if (type.ContainsGenericParameters)
            {
                return new OpenGenericJavaType(type, name);
            }
            else
            {
                return new RuntimeManagedJavaType(type, name);
            }
        }

        private RuntimeManagedJavaType(Type type, string name)
            : base(TypeFlags.None, GetModifiers(type), name)
        {
            Debug.Assert(!(type.IsByRef), type.FullName);
            Debug.Assert(!(type.IsPointer), type.FullName);
            Debug.Assert(!(type.Name.EndsWith("[]")), type.FullName);
            Debug.Assert(!(type is TypeBuilder), type.FullName);
            Debug.Assert(!(AttributeHelper.IsJavaModule(type.Module)));

            this.type = type;
        }

        internal override RuntimeJavaType BaseTypeWrapper => baseTypeWrapper ??= GetBaseTypeWrapper(type);

        internal override RuntimeClassLoader GetClassLoader() => type.IsGenericType ? RuntimeClassLoader.GetGenericClassLoader(this) : RuntimeAssemblyClassLoader.FromAssembly(type.Assembly);

        sealed class MulticastDelegateCtorJavaMethod : RuntimeJavaMethod
        {

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="declaringType"></param>
            internal MulticastDelegateCtorJavaMethod(RuntimeJavaType declaringType) :
                base(declaringType, "<init>", "()V", null, RuntimePrimitiveJavaType.VOID, Array.Empty<RuntimeJavaType>(), Modifiers.Protected, MemberFlags.None)
            {

            }

        }

        internal static string GetDelegateInvokeStubName(Type delegateType)
        {
            var delegateInvoke = delegateType.GetMethod("Invoke");
            var parameters = delegateInvoke.GetParameters();

            string name = null;
            for (int i = 0; i < parameters.Length; i++)
                if (parameters[i].ParameterType.IsByRef)
                    name = (name ?? "<Invoke>") + "_" + i;

            return name ?? "Invoke";
        }

        sealed class DelegateJavaMethod : RuntimeJavaMethod
        {

            readonly ConstructorInfo delegateConstructor;
            readonly DelegateInnerClassJavaType iface;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="declaringType"></param>
            /// <param name="iface"></param>
            internal DelegateJavaMethod(RuntimeJavaType declaringType, DelegateInnerClassJavaType iface) :
                base(declaringType, "<init>", "(" + iface.SigName + ")V", null, RuntimePrimitiveJavaType.VOID, new RuntimeJavaType[] { iface }, Modifiers.Public, MemberFlags.Intrinsic)
            {
                this.delegateConstructor = declaringType.TypeAsTBD.GetConstructor(new Type[] { Types.Object, Types.IntPtr });
                this.iface = iface;
            }

#if EMITTERS
            internal override bool EmitIntrinsic(EmitIntrinsicContext context)
            {
                var targetType = context.GetStackTypeWrapper(0, 0);
                if (targetType.IsUnloadable || targetType.IsInterface)
                {
                    return false;
                }
                // we know that a DelegateInnerClassTypeWrapper has only one method
                Debug.Assert(iface.GetMethods().Length == 1);
                var mw = targetType.GetMethodWrapper(GetDelegateInvokeStubName(DeclaringType.TypeAsTBD), iface.GetMethods()[0].Signature, true);
                if (mw == null || mw.IsStatic || !mw.IsPublic)
                {
                    context.Emitter.Emit(OpCodes.Ldftn, CreateErrorStub(context, targetType, mw == null || mw.IsStatic));
                    context.Emitter.Emit(OpCodes.Newobj, delegateConstructor);
                    return true;
                }
                // TODO linking here is not safe
                mw.Link();
                context.Emitter.Emit(OpCodes.Dup);
                context.Emitter.Emit(OpCodes.Ldvirtftn, mw.GetMethod());
                context.Emitter.Emit(OpCodes.Newobj, delegateConstructor);
                return true;
            }

            private MethodInfo CreateErrorStub(EmitIntrinsicContext context, RuntimeJavaType targetType, bool isAbstract)
            {
                var invoke = delegateConstructor.DeclaringType.GetMethod("Invoke");

                var parameters = invoke.GetParameters();
                var parameterTypes = new Type[parameters.Length + 1];
                parameterTypes[0] = Types.Object;
                for (int i = 0; i < parameters.Length; i++)
                    parameterTypes[i + 1] = parameters[i].ParameterType;

                var mb = context.Context.DefineDelegateInvokeErrorStub(invoke.ReturnType, parameterTypes);
                var ilgen = CodeEmitter.Create(mb);
                ilgen.EmitThrow(isAbstract ? "java.lang.AbstractMethodError" : "java.lang.IllegalAccessError", targetType.Name + ".Invoke" + iface.GetMethods()[0].Signature);
                ilgen.DoEmit();
                return mb;
            }

            internal override void EmitNewobj(CodeEmitter ilgen)
            {
                ilgen.Emit(OpCodes.Ldtoken, delegateConstructor.DeclaringType);
                ilgen.Emit(OpCodes.Call, Compiler.getTypeFromHandleMethod);
                ilgen.Emit(OpCodes.Ldstr, GetDelegateInvokeStubName(DeclaringType.TypeAsTBD));
                ilgen.Emit(OpCodes.Ldstr, iface.GetMethods()[0].Signature);
                ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.DynamicCreateDelegate);
                ilgen.Emit(OpCodes.Castclass, delegateConstructor.DeclaringType);
            }

            internal override void EmitCall(CodeEmitter ilgen)
            {
                // This is a bit of a hack. We bind the existing delegate to a new delegate to be able to reuse the DynamicCreateDelegate error
                // handling. This leaks out to the user because Delegate.Target will return the target delegate instead of the bound object.

                // create the target delegate
                EmitNewobj(ilgen);

                // invoke the constructor, binding the delegate to the target delegate
                ilgen.Emit(OpCodes.Dup);
                ilgen.Emit(OpCodes.Ldvirtftn, MethodHandleUtil.GetDelegateInvokeMethod(delegateConstructor.DeclaringType));
                ilgen.Emit(OpCodes.Call, delegateConstructor);
            }

#endif // EMITTERS

        }

        sealed class ByRefJavaMethod : RuntimeSmartJavaMethod
        {

#if !IMPORTER
            readonly bool[] byrefs;
#endif
            readonly Type[] args;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="args"></param>
            /// <param name="byrefs"></param>
            /// <param name="declaringType"></param>
            /// <param name="name"></param>
            /// <param name="sig"></param>
            /// <param name="method"></param>
            /// <param name="returnType"></param>
            /// <param name="parameterTypes"></param>
            /// <param name="modifiers"></param>
            /// <param name="hideFromReflection"></param>
            internal ByRefJavaMethod(Type[] args, bool[] byrefs, RuntimeJavaType declaringType, string name, string sig, MethodBase method, RuntimeJavaType returnType, RuntimeJavaType[] parameterTypes, Modifiers modifiers, bool hideFromReflection) :
                base(declaringType, name, sig, method, returnType, parameterTypes, modifiers, hideFromReflection ? MemberFlags.HideFromReflection : MemberFlags.None)
            {
                this.args = args;
#if !IMPORTER
                this.byrefs = byrefs;
#endif
            }

#if EMITTERS
            protected override void CallImpl(CodeEmitter ilgen)
            {
                ConvertByRefArgs(ilgen);
                ilgen.Emit(OpCodes.Call, GetMethod());
            }

            protected override void CallvirtImpl(CodeEmitter ilgen)
            {
                ConvertByRefArgs(ilgen);
                ilgen.Emit(OpCodes.Callvirt, GetMethod());
            }

            protected override void NewobjImpl(CodeEmitter ilgen)
            {
                ConvertByRefArgs(ilgen);
                ilgen.Emit(OpCodes.Newobj, GetMethod());
            }

            private void ConvertByRefArgs(CodeEmitter ilgen)
            {
                CodeEmitterLocal[] locals = new CodeEmitterLocal[args.Length];
                for (int i = args.Length - 1; i >= 0; i--)
                {
                    Type type = args[i];
                    if (type.IsByRef)
                    {
                        type = RuntimeArrayJavaType.MakeArrayType(type.GetElementType(), 1);
                    }
                    locals[i] = ilgen.DeclareLocal(type);
                    ilgen.Emit(OpCodes.Stloc, locals[i]);
                }
                for (int i = 0; i < args.Length; i++)
                {
                    ilgen.Emit(OpCodes.Ldloc, locals[i]);
                    if (args[i].IsByRef)
                    {
                        ilgen.Emit(OpCodes.Ldc_I4_0);
                        ilgen.Emit(OpCodes.Ldelema, args[i].GetElementType());
                    }
                }
            }
#endif // EMITTERS
        }

        private sealed class EnumWrapJavaMethod : RuntimeJavaMethod
        {

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="tw"></param>
            /// <param name="fieldType"></param>
            internal EnumWrapJavaMethod(RuntimeManagedJavaType tw, RuntimeJavaType fieldType) :
                base(tw, "wrap", "(" + fieldType.SigName + ")" + tw.SigName, null, tw, new RuntimeJavaType[] { fieldType }, Modifiers.Static | Modifiers.Public, MemberFlags.None)
            {

            }

#if EMITTERS

            internal override void EmitCall(CodeEmitter ilgen)
            {
                // We don't actually need to do anything here!
                // The compiler will insert a boxing operation after calling us and that will
                // result in our argument being boxed (since that's still sitting on the stack).
            }

#endif // EMITTERS

#if !IMPORTER && !EXPORTER && !FIRST_PASS

            internal override object Invoke(object obj, object[] args)
            {
                return Enum.ToObject(DeclaringType.TypeAsTBD, args[0]);
            }

#endif

        }

        internal sealed class EnumValueJavaField : RuntimeJavaField
        {

            readonly Type underlyingType;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="tw"></param>
            /// <param name="fieldType"></param>
            internal EnumValueJavaField(RuntimeManagedJavaType tw, RuntimeJavaType fieldType) :
                base(tw, fieldType, "Value", fieldType.SigName, new ExModifiers(Modifiers.Public | Modifiers.Final, false), null)
            {
                underlyingType = EnumHelper.GetUnderlyingType(tw.type);
            }

#if EMITTERS

            protected override void EmitGetImpl(CodeEmitter ilgen)
            {
                // NOTE if the reference on the stack is null, we *want* the NullReferenceException, so we don't use TypeWrapper.EmitUnbox
                ilgen.Emit(OpCodes.Unbox, underlyingType);
                ilgen.Emit(OpCodes.Ldobj, underlyingType);
            }

            protected override void EmitSetImpl(CodeEmitter ilgen)
            {
                // NOTE even though the field is final, JNI reflection can still be used to set its value!
                CodeEmitterLocal temp = ilgen.AllocTempLocal(underlyingType);
                ilgen.Emit(OpCodes.Stloc, temp);
                ilgen.Emit(OpCodes.Unbox, underlyingType);
                ilgen.Emit(OpCodes.Ldloc, temp);
                ilgen.Emit(OpCodes.Stobj, underlyingType);
                ilgen.ReleaseTempLocal(temp);
            }

#endif

#if !EXPORTER && !IMPORTER && !FIRST_PASS

            internal override object GetValue(object obj)
            {
                return obj;
            }

            internal override void SetValue(object obj, object value)
            {
                obj.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance)[0].SetValue(obj, value);
            }
#endif
        }

        sealed class ValueTypeDefaultCtorJavaMethod : RuntimeJavaMethod
        {

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="tw"></param>
            internal ValueTypeDefaultCtorJavaMethod(RuntimeManagedJavaType tw) :
                base(tw, "<init>", "()V", null, RuntimePrimitiveJavaType.VOID, Array.Empty<RuntimeJavaType>(), Modifiers.Public, MemberFlags.None)
            {

            }

#if EMITTERS

            internal override void EmitNewobj(CodeEmitter ilgen)
            {
                CodeEmitterLocal local = ilgen.DeclareLocal(DeclaringType.TypeAsTBD);
                ilgen.Emit(OpCodes.Ldloc, local);
                ilgen.Emit(OpCodes.Box, DeclaringType.TypeAsTBD);
            }

            internal override void EmitCall(CodeEmitter ilgen)
            {
                ilgen.Emit(OpCodes.Pop);
            }

#endif // EMITTERS

#if !IMPORTER && !EXPORTER && !FIRST_PASS

            internal override object CreateInstance(object[] args)
            {
                return Activator.CreateInstance(DeclaringType.TypeAsTBD);
            }

#endif

        }

        private sealed class FinalizeJavaMethod : RuntimeJavaMethod
        {

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="tw"></param>
            internal FinalizeJavaMethod(RuntimeManagedJavaType tw) :
                base(tw, "finalize", "()V", null, RuntimePrimitiveJavaType.VOID, Array.Empty<RuntimeJavaType>(), Modifiers.Protected | Modifiers.Final, MemberFlags.None)
            {

            }

#if EMITTERS
            internal override void EmitCall(CodeEmitter ilgen)
            {
                ilgen.Emit(OpCodes.Pop);
            }

            internal override void EmitCallvirt(CodeEmitter ilgen)
            {
                ilgen.Emit(OpCodes.Pop);
            }

#endif // EMITTERS

        }

        private sealed class CloneJavaMethod : RuntimeJavaMethod
        {

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="tw"></param>
            internal CloneJavaMethod(RuntimeManagedJavaType tw) :
                base(tw, "clone", "()Ljava.lang.Object;", null, CoreClasses.java.lang.Object.Wrapper, Array.Empty<RuntimeJavaType>(), Modifiers.Protected | Modifiers.Final, MemberFlags.None)
            {

            }

#if EMITTERS

            internal override void EmitCall(CodeEmitter ilgen)
            {
                ilgen.Emit(OpCodes.Dup);
                ilgen.Emit(OpCodes.Isinst, CoreClasses.java.lang.Cloneable.Wrapper.TypeAsBaseType);
                CodeEmitterLabel label1 = ilgen.DefineLabel();
                ilgen.EmitBrtrue(label1);
                CodeEmitterLabel label2 = ilgen.DefineLabel();
                ilgen.EmitBrfalse(label2);
                ilgen.EmitThrow("java.lang.CloneNotSupportedException");
                ilgen.MarkLabel(label2);
                ilgen.EmitThrow("java.lang.NullPointerException");
                ilgen.MarkLabel(label1);
                ilgen.Emit(OpCodes.Call, Types.Object.GetMethod("MemberwiseClone", BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, null));
            }

            internal override void EmitCallvirt(CodeEmitter ilgen)
            {
                EmitCall(ilgen);
            }

#endif // EMITTERS

        }

        protected override void LazyPublishMembers()
        {
            // special support for enums
            if (type.IsEnum)
            {
                Type underlyingType = EnumHelper.GetUnderlyingType(type);
                Type javaUnderlyingType;
                if (underlyingType == Types.SByte)
                {
                    javaUnderlyingType = Types.Byte;
                }
                else if (underlyingType == Types.UInt16)
                {
                    javaUnderlyingType = Types.Int16;
                }
                else if (underlyingType == Types.UInt32)
                {
                    javaUnderlyingType = Types.Int32;
                }
                else if (underlyingType == Types.UInt64)
                {
                    javaUnderlyingType = Types.Int64;
                }
                else
                {
                    javaUnderlyingType = underlyingType;
                }

                var fieldType = RuntimeClassLoader.GetWrapperFromType(javaUnderlyingType);
                var fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static);
                var fieldsList = new List<RuntimeJavaField>();
                for (int i = 0; i < fields.Length; i++)
                {
                    if (fields[i].FieldType == type)
                    {
                        var name = fields[i].Name;
                        if (name == "Value")
                            name = "_Value";
                        else if (name.StartsWith("_") && name.EndsWith("Value"))
                            name = "_" + name;

                        var val = EnumHelper.GetPrimitiveValue(underlyingType, fields[i].GetRawConstantValue());
                        fieldsList.Add(new RuntimeConstantJavaField(this, fieldType, name, fieldType.SigName, Modifiers.Public | Modifiers.Static | Modifiers.Final, fields[i], val, MemberFlags.None));
                    }
                }
                fieldsList.Add(new EnumValueJavaField(this, fieldType));
                SetFields(fieldsList.ToArray());
                SetMethods(new RuntimeJavaMethod[] { new EnumWrapJavaMethod(this, fieldType) });
            }
            else
            {
                var fieldsList = new List<RuntimeJavaField>();
                var fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                for (int i = 0; i < fields.Length; i++)
                {
                    // TODO for remapped types, instance fields need to be converted to static getter/setter methods
                    if (fields[i].FieldType.IsPointer)
                    {
                        // skip, pointer fields are not supported
                    }
                    else
                    {
                        // TODO handle name/signature clash
                        fieldsList.Add(CreateFieldWrapperDotNet(AttributeHelper.GetModifiers(fields[i], true).Modifiers, fields[i].Name, fields[i].FieldType, fields[i]));
                    }
                }
                SetFields(fieldsList.ToArray());

                var methodsList = new Dictionary<string, RuntimeJavaMethod>();

                // special case for delegate constructors!
                if (IsDelegate(type))
                {
                    var iface = InnerClasses[0];
                    var mw = new DelegateJavaMethod(this, (DelegateInnerClassJavaType)iface);
                    methodsList.Add(mw.Name + mw.Signature, mw);
                }

                // add a protected default constructor to MulticastDelegate to make it easier to define a delegate in Java
                if (type == Types.MulticastDelegate)
                    methodsList.Add("<init>()V", new MulticastDelegateCtorJavaMethod(this));

                var constructors = type.GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                for (int i = 0; i < constructors.Length; i++)
                {
                    if (MakeMethodDescriptor(constructors[i], out var name, out var sig, out var args, out var ret))
                    {
                        var mw = CreateMethodWrapper(name, sig, args, ret, constructors[i], false);
                        var key = mw.Name + mw.Signature;
                        if (!methodsList.ContainsKey(key))
                            methodsList.Add(key, mw);
                    }
                }

                if (type.IsValueType && !methodsList.ContainsKey("<init>()V"))
                {
                    // Value types have an implicit default ctor
                    methodsList.Add("<init>()V", new ValueTypeDefaultCtorJavaMethod(this));
                }

                var methods = type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                for (int i = 0; i < methods.Length; i++)
                {
                    if (methods[i].IsStatic && type.IsInterface)
                    {
                        // skip, Java cannot deal with static methods on interfaces
                    }
                    else
                    {
                        if (MakeMethodDescriptor(methods[i], out var name, out var sig, out var args, out var ret))
                        {
                            if (!methods[i].IsStatic && !methods[i].IsPrivate && BaseTypeWrapper != null)
                            {
                                var baseMethod = BaseTypeWrapper.GetMethodWrapper(name, sig, true);
                                if (baseMethod != null && baseMethod.IsFinal && !baseMethod.IsStatic && !baseMethod.IsPrivate)
                                {
                                    continue;
                                }
                            }

                            var mw = CreateMethodWrapper(name, sig, args, ret, methods[i], false);
                            var key = mw.Name + mw.Signature;
                            methodsList.TryGetValue(key, out var existing);

                            if (existing == null || existing is ByRefJavaMethod)
                                methodsList[key] = mw;
                        }
                        else if (methods[i].IsAbstract)
                        {
                            SetHasUnsupportedAbstractMethods();
                        }
                    }
                }

                // make sure that all the interface methods that we implement are available as public methods,
                // otherwise javac won't like the class.
                if (!type.IsInterface)
                {
                    var interfaces = type.GetInterfaces();
                    for (int i = 0; i < interfaces.Length; i++)
                    {
                        // we only handle public (or nested public) types, because we're potentially adding a
                        // method that should be callable by anyone through the interface
                        if (interfaces[i].IsVisible)
                        {
                            if (RuntimeClassLoader.IsRemappedType(interfaces[i]))
                            {
                                var tw = RuntimeClassLoader.GetWrapperFromType(interfaces[i]);
                                foreach (var mw in tw.GetMethods())
                                {
                                    // HACK we need to link here, because during a core library build we might reference java.lang.AutoCloseable (via IDisposable) before it has been linked
                                    mw.Link();
                                    InterfaceMethodStubHelper(methodsList, mw.GetMethod(), mw.Name, mw.Signature, mw.GetParameters(), mw.ReturnType);
                                }
                            }

                            var map = type.GetInterfaceMap(interfaces[i]);
                            for (int j = 0; j < map.InterfaceMethods.Length; j++)
                            {
                                if (map.TargetMethods[j] == null || ((!map.TargetMethods[j].IsPublic || map.TargetMethods[j].Name != map.InterfaceMethods[j].Name) && map.TargetMethods[j].DeclaringType == type))
                                {
                                    if (MakeMethodDescriptor(map.InterfaceMethods[j], out var name, out var sig, out var args, out var ret))
                                    {
                                        InterfaceMethodStubHelper(methodsList, map.InterfaceMethods[j], name, sig, args, ret);
                                    }
                                }
                            }
                        }
                    }
                }

                // for non-final remapped types, we need to add all the virtual methods in our alter ego (which
                // appears as our base class) and make them final (to prevent Java code from overriding these
                // methods, which don't really exist).
                if (RuntimeClassLoader.IsRemappedType(type) && !type.IsSealed && !type.IsInterface)
                {
                    var baseTypeWrapper = this.BaseTypeWrapper;
                    while (baseTypeWrapper != null)
                    {
                        foreach (var m in baseTypeWrapper.GetMethods())
                        {
                            if (!m.IsStatic && !m.IsFinal && (m.IsPublic || m.IsProtected) && m.Name != "<init>")
                            {
                                var key = m.Name + m.Signature;
                                if (!methodsList.ContainsKey(key))
                                {
                                    if (m.IsProtected)
                                    {
                                        if (m.Name == "finalize" && m.Signature == "()V")
                                        {
                                            methodsList.Add(key, new FinalizeJavaMethod(this));
                                        }
                                        else if (m.Name == "clone" && m.Signature == "()Ljava.lang.Object;")
                                        {
                                            methodsList.Add(key, new CloneJavaMethod(this));
                                        }
                                        else
                                        {
                                            // there should be a special MethodWrapper for this method
                                            throw new InvalidOperationException("Missing protected method support for " + baseTypeWrapper.Name + "::" + m.Name + m.Signature);
                                        }
                                    }
                                    else
                                    {
                                        methodsList.Add(key, new BaseFinalJavaMethod(this, m));
                                    }
                                }
                            }
                        }
                        baseTypeWrapper = baseTypeWrapper.BaseTypeWrapper;
                    }
                }

#if !IMPORTER && !EXPORTER && !FIRST_PASS

                // support serializing .NET exceptions (by replacing them with a placeholder exception)
                if (typeof(Exception).IsAssignableFrom(type) && !typeof(java.io.Serializable.__Interface).IsAssignableFrom(type) && !methodsList.ContainsKey("writeReplace()Ljava.lang.Object;"))
                {
                    methodsList.Add("writeReplace()Ljava.lang.Object;", new ExceptionWriteReplaceJavaMethod(this));
                }

#endif // !IMPORTER && !EXPORTER && !FIRST_PASS

                var methodArray = new RuntimeJavaMethod[methodsList.Count];
                methodsList.Values.CopyTo(methodArray, 0);
                SetMethods(methodArray);
            }
        }

#if !IMPORTER && !EXPORTER && !FIRST_PASS

        private sealed class ExceptionWriteReplaceJavaMethod : RuntimeJavaMethod
        {

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="declaringType"></param>
            internal ExceptionWriteReplaceJavaMethod(RuntimeJavaType declaringType) :
                base(declaringType, "writeReplace", "()Ljava.lang.Object;", null, CoreClasses.java.lang.Object.Wrapper, Array.Empty<RuntimeJavaType>(), Modifiers.Private, MemberFlags.None)
            {

            }

            internal override bool IsDynamicOnly => true;

            internal override object Invoke(object obj, object[] args)
            {
                var x = (Exception)obj;
                com.sun.xml.@internal.ws.developer.ServerSideException sse = new com.sun.xml.@internal.ws.developer.ServerSideException(ikvm.extensions.ExtensionMethods.getClass(x).getName(), x.Message);
                sse.initCause(x.InnerException);
                sse.setStackTrace(ikvm.extensions.ExtensionMethods.getStackTrace(x));
                return sse;
            }

        }

#endif // !IMPORTER && !EXPORTER && !FIRST_PASS

        private void InterfaceMethodStubHelper(Dictionary<string, RuntimeJavaMethod> methodsList, MethodBase method, string name, string sig, RuntimeJavaType[] args, RuntimeJavaType ret)
        {
            var key = name + sig;
            methodsList.TryGetValue(key, out var existing);
            if (existing == null && BaseTypeWrapper != null)
            {
                var baseMethod = BaseTypeWrapper.GetMethodWrapper(name, sig, true);
                if (baseMethod != null && !baseMethod.IsStatic && baseMethod.IsPublic)
                    return;
            }

            if (existing == null || existing is ByRefJavaMethod || existing.IsStatic || !existing.IsPublic)
            {
                // TODO if existing != null, we need to rename the existing method (but this is complicated because
                // it also affects subclasses). This is especially required is the existing method is abstract,
                // because otherwise we won't be able to create any subclasses in Java.
                methodsList[key] = CreateMethodWrapper(name, sig, args, ret, method, true);
            }
        }

        private sealed class BaseFinalJavaMethod : RuntimeJavaMethod
        {

            private readonly RuntimeJavaMethod m;

            internal BaseFinalJavaMethod(RuntimeManagedJavaType tw, RuntimeJavaMethod m)
                : base(tw, m.Name, m.Signature, null, null, null, (m.Modifiers & ~Modifiers.Abstract) | Modifiers.Final, MemberFlags.None)
            {
                this.m = m;
            }

            protected override void DoLinkMethod()
            {
            }

#if EMITTERS
            internal override void EmitCall(CodeEmitter ilgen)
            {
                // we direct EmitCall to EmitCallvirt, because we always want to end up at the instancehelper method
                // (EmitCall would go to our alter ego .NET type and that wouldn't be legal)
                m.EmitCallvirt(ilgen);
            }

            internal override void EmitCallvirt(CodeEmitter ilgen)
            {
                m.EmitCallvirt(ilgen);
            }
#endif // EMITTERS
        }

        internal static bool IsUnsupportedAbstractMethod(MethodBase mb)
        {
            if (mb.IsAbstract)
            {
                MethodInfo mi = (MethodInfo)mb;
                if (mi.ReturnType.IsByRef || IsPointerType(mi.ReturnType) || mb.IsGenericMethodDefinition)
                {
                    return true;
                }
                foreach (ParameterInfo p in mi.GetParameters())
                {
                    if (p.ParameterType.IsByRef || IsPointerType(p.ParameterType))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private static bool IsPointerType(Type type)
        {
            while (type.HasElementType)
            {
                if (type.IsPointer)
                {
                    return true;
                }
                type = type.GetElementType();
            }
#if IMPORTER || EXPORTER
            return type.__IsFunctionPointer;
#else
            return false;
#endif
        }

        private bool MakeMethodDescriptor(MethodBase mb, out string name, out string sig, out RuntimeJavaType[] args, out RuntimeJavaType ret)
        {
            if (mb.IsGenericMethodDefinition)
            {
                name = null;
                sig = null;
                args = null;
                ret = null;
                return false;
            }
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append('(');
            ParameterInfo[] parameters = mb.GetParameters();
            args = new RuntimeJavaType[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                Type type = parameters[i].ParameterType;
                if (IsPointerType(type))
                {
                    name = null;
                    sig = null;
                    args = null;
                    ret = null;
                    return false;
                }
                if (type.IsByRef)
                {
                    type = RuntimeArrayJavaType.MakeArrayType(type.GetElementType(), 1);
                    if (mb.IsAbstract)
                    {
                        // Since we cannot override methods with byref arguments, we don't report abstract
                        // methods with byref args.
                        name = null;
                        sig = null;
                        args = null;
                        ret = null;
                        return false;
                    }
                }
                RuntimeJavaType tw = RuntimeClassLoader.GetWrapperFromType(type);
                args[i] = tw;
                sb.Append(tw.SigName);
            }
            sb.Append(')');
            if (mb is ConstructorInfo)
            {
                ret = RuntimePrimitiveJavaType.VOID;
                if (mb.IsStatic)
                {
                    name = "<clinit>";
                }
                else
                {
                    name = "<init>";
                }
                sb.Append(ret.SigName);
                sig = sb.ToString();
                return true;
            }
            else
            {
                Type type = ((MethodInfo)mb).ReturnType;
                if (IsPointerType(type) || type.IsByRef)
                {
                    name = null;
                    sig = null;
                    ret = null;
                    return false;
                }
                ret = RuntimeClassLoader.GetWrapperFromType(type);
                sb.Append(ret.SigName);
                name = mb.Name;
                sig = sb.ToString();
                return true;
            }
        }

        internal override RuntimeJavaType[] Interfaces
        {
            get
            {
                if (interfaces == null)
                {
                    interfaces = GetImplementedInterfacesAsTypeWrappers(type);
                }
                return interfaces;
            }
        }

        private static bool IsAttribute(Type type)
        {
            if (!type.IsAbstract && type.IsSubclassOf(Types.Attribute) && type.IsVisible)
            {
                //
                // Based on the number of constructors and their arguments, we distinguish several types
                // of attributes:
                //                                   | def ctor | single 1-arg ctor
                // -----------------------------------------------------------------
                // complex only (i.e. Annotation{N}) |          |
                // all optional fields/properties    |    X     |
                // required "value"                  |          |   X
                // optional "value"                  |    X     |   X
                // -----------------------------------------------------------------
                // 
                // TODO currently we don't support "complex only" attributes.
                //
                ConstructorInfo defCtor;
                ConstructorInfo singleOneArgCtor;
                AttributeAnnotationJavaType.GetConstructors(type, out defCtor, out singleOneArgCtor);
                return defCtor != null || singleOneArgCtor != null;
            }
            return false;
        }

        private static bool IsDelegate(Type type)
        {
            // HACK non-public delegates do not get the special treatment (because they are likely to refer to
            // non-public types in the arg list and they're not really useful anyway)
            // NOTE we don't have to check in what assembly the type lives, because this is a DotNetTypeWrapper,
            // we know that it is a different assembly.
            if (!type.IsAbstract && type.IsSubclassOf(Types.MulticastDelegate) && type.IsVisible)
            {
                MethodInfo invoke = type.GetMethod("Invoke");
                if (invoke != null)
                {
                    foreach (ParameterInfo p in invoke.GetParameters())
                    {
                        // we don't support delegates with pointer parameters
                        if (IsPointerType(p.ParameterType))
                        {
                            return false;
                        }
                    }
                    return !IsPointerType(invoke.ReturnType);
                }
            }
            return false;
        }

        internal override RuntimeJavaType[] InnerClasses
        {
            get
            {
                if (innerClasses == null)
                {
                    innerClasses = GetInnerClasses();
                }
                return innerClasses;
            }
        }

        private RuntimeJavaType[] GetInnerClasses()
        {
            Type[] nestedTypes = type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic);
            List<RuntimeJavaType> list = new List<RuntimeJavaType>(nestedTypes.Length);
            for (int i = 0; i < nestedTypes.Length; i++)
            {
                if (!nestedTypes[i].IsGenericTypeDefinition)
                {
                    list.Add(RuntimeClassLoader.GetWrapperFromType(nestedTypes[i]));
                }
            }
            if (IsDelegate(type))
            {
                list.Add(GetClassLoader().RegisterInitiatingLoader(new DelegateInnerClassJavaType(Name + DelegateInterfaceSuffix, type)));
            }
            if (IsAttribute(type))
            {
                list.Add(GetClassLoader().RegisterInitiatingLoader(new AttributeAnnotationJavaType(Name + AttributeAnnotationSuffix, type)));
            }
            if (type.IsEnum && type.IsVisible)
            {
                list.Add(GetClassLoader().RegisterInitiatingLoader(new EnumEnumJavaType(Name + EnumEnumSuffix, type)));
            }
            return list.ToArray();
        }

        internal override bool IsFakeTypeContainer
        {
            get
            {
                return IsDelegate(type) || IsAttribute(type) || (type.IsEnum && type.IsVisible);
            }
        }

        internal override RuntimeJavaType DeclaringTypeWrapper
        {
            get
            {
                if (outerClass == null)
                {
                    Type outer = type.DeclaringType;
                    if (outer != null && !type.IsGenericType)
                    {
                        outerClass = GetWrapperFromDotNetType(outer);
                    }
                }
                return outerClass;
            }
        }

        internal override Modifiers ReflectiveModifiers
        {
            get
            {
                if (DeclaringTypeWrapper != null)
                {
                    return Modifiers | Modifiers.Static;
                }
                return Modifiers;
            }
        }

        private RuntimeJavaField CreateFieldWrapperDotNet(Modifiers modifiers, string name, Type fieldType, FieldInfo field)
        {
            RuntimeJavaType type = RuntimeClassLoader.GetWrapperFromType(fieldType);
            if (field.IsLiteral)
            {
                return new RuntimeConstantJavaField(this, type, name, type.SigName, modifiers, field, null, MemberFlags.None);
            }
            else
            {
                return RuntimeJavaField.Create(this, type, field, name, type.SigName, new ExModifiers(modifiers, false));
            }
        }

        // this method detects if type derives from our java.lang.Object or java.lang.Throwable implementation types
        private static bool IsRemappedImplDerived(Type type)
        {
            for (; type != null; type = type.BaseType)
            {
                if (!RuntimeClassLoader.IsRemappedType(type) && RuntimeClassLoader.GetWrapperFromType(type).IsRemapped)
                {
                    return true;
                }
            }
            return false;
        }

        private RuntimeJavaMethod CreateMethodWrapper(string name, string sig, RuntimeJavaType[] argTypeWrappers, RuntimeJavaType retTypeWrapper, MethodBase mb, bool privateInterfaceImplHack)
        {
            var exmods = AttributeHelper.GetModifiers(mb, true);
            var mods = exmods.Modifiers;

            if (name == "Finalize" && sig == "()V" && !mb.IsStatic && IsRemappedImplDerived(TypeAsBaseType))
            {
                // TODO if the .NET also has a "finalize" method, we need to hide that one (or rename it, or whatever)
                RuntimeJavaMethod mw = new RuntimeSimpleCallJavaMethod(this, "finalize", "()V", (MethodInfo)mb, RuntimePrimitiveJavaType.VOID, Array.Empty<RuntimeJavaType>(), mods, MemberFlags.None, SimpleOpCode.Call, SimpleOpCode.Callvirt);
                mw.SetDeclaredExceptions(new string[] { "java.lang.Throwable" });
                return mw;
            }

            var parameters = mb.GetParameters();
            var args = new Type[parameters.Length];
            bool hasByRefArgs = false;
            bool[] byrefs = null;

            for (int i = 0; i < parameters.Length; i++)
            {
                args[i] = parameters[i].ParameterType;
                if (parameters[i].ParameterType.IsByRef)
                {
                    if (byrefs == null)
                    {
                        byrefs = new bool[args.Length];
                    }
                    byrefs[i] = true;
                    hasByRefArgs = true;
                }
            }

            if (privateInterfaceImplHack)
            {
                mods &= ~Modifiers.Abstract;
                mods |= Modifiers.Final;
            }

            if (hasByRefArgs)
            {
                if (!(mb is ConstructorInfo) && !mb.IsStatic)
                {
                    mods |= Modifiers.Final;
                }
                return new ByRefJavaMethod(args, byrefs, this, name, sig, mb, retTypeWrapper, argTypeWrappers, mods, false);
            }
            else
            {
                return new RuntimeTypicalJavaMethod(this, name, sig, mb, retTypeWrapper, argTypeWrappers, mods, MemberFlags.None);
            }
        }

        internal override Type TypeAsTBD => type;

        internal override bool IsRemapped => RuntimeClassLoader.IsRemappedType(type);

#if EMITTERS

        internal override void EmitInstanceOf(CodeEmitter ilgen)
        {
            if (IsRemapped)
            {
                var shadow = RuntimeClassLoader.GetWrapperFromType(type);
                var method = shadow.TypeAsBaseType.GetMethod("__<instanceof>");
                if (method != null)
                {
                    ilgen.Emit(OpCodes.Call, method);
                    return;
                }
            }
            ilgen.Emit_instanceof(type);
        }

        internal override void EmitCheckcast(CodeEmitter ilgen)
        {
            if (IsRemapped)
            {
                var shadow = RuntimeClassLoader.GetWrapperFromType(type);
                var method = shadow.TypeAsBaseType.GetMethod("__<checkcast>");
                if (method != null)
                {
                    ilgen.Emit(OpCodes.Call, method);
                    return;
                }
            }
            ilgen.EmitCastclass(type);
        }

#endif // EMITTERS

        internal override MethodParametersEntry[] GetMethodParameters(RuntimeJavaMethod mw)
        {
            var mb = mw.GetMethod();
            if (mb == null)
            {
                return null;
            }
            var parameters = mb.GetParameters();
            if (parameters.Length == 0)
            {
                return null;
            }
            var mp = new MethodParametersEntry[parameters.Length];
            var hasName = false;
            for (int i = 0; i < mp.Length; i++)
            {
                string name = parameters[i].Name;
                var empty = string.IsNullOrEmpty(name);
                if (empty)
                {
                    name = "arg" + i;
                }
                mp[i].name = name;
                hasName |= !empty;
            }

            if (!hasName)
                return null;

            return mp;
        }

#if !IMPORTER && !EXPORTER

        internal override object[] GetDeclaredAnnotations()
        {
            return type.GetCustomAttributes(false);
        }

        internal override object[] GetFieldAnnotations(RuntimeJavaField fw)
        {
            var fi = fw.GetField();
            if (fi == null)
            {
                return null;
            }
            return fi.GetCustomAttributes(false);
        }

        internal override object[] GetMethodAnnotations(RuntimeJavaMethod mw)
        {
            var mb = mw.GetMethod();
            if (mb == null)
            {
                return null;
            }
            return mb.GetCustomAttributes(false);
        }

        internal override object[][] GetParameterAnnotations(RuntimeJavaMethod mw)
        {
            var mb = mw.GetMethod();
            if (mb == null)
                return null;

            var parameters = mb.GetParameters();
            var attribs = new object[parameters.Length][];
            for (int i = 0; i < parameters.Length; i++)
                attribs[i] = parameters[i].GetCustomAttributes(false);

            return attribs;
        }
#endif

        internal override bool IsFastClassLiteralSafe => type != Types.Void && !type.IsPrimitive && !IsRemapped;

#if !IMPORTER && !EXPORTER

        // this override is only relevant for the runtime, because it handles the scenario
        // where classes are dynamically loaded by the assembly class loader
        // (i.e. injected into the assembly)
        internal override bool IsPackageAccessibleFrom(RuntimeJavaType wrapper)
        {
            if (wrapper == DeclaringTypeWrapper)
                return true;

            if (!base.IsPackageAccessibleFrom(wrapper))
                return false;

            // check accessibility for nested types
            for (Type type = TypeAsTBD; type.IsNested; type = type.DeclaringType)
            {
                // we don't support family (protected) access
                if (!type.IsNestedAssembly && !type.IsNestedFamORAssem && !type.IsNestedPublic)
                {
                    return false;
                }
            }

            return true;
        }

#endif

    }

}
