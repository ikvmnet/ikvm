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

using System.Text;
using IKVM.CoreLib.Runtime;



#if IMPORTER || EXPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif

namespace IKVM.Runtime
{

    /// <summary>
    /// Implements a <see cref="RuntimeJavaType"/> that exposes existing managed .NET types not the result of static compilation.
    /// </summary>
    sealed partial class RuntimeManagedJavaType : RuntimeJavaType
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

        readonly Type type;
        RuntimeJavaType baseTypeWrapper;
        RuntimeJavaType[] innerClasses;
        RuntimeJavaType outerClass;
        RuntimeJavaType[] interfaces;

        static Modifiers GetModifiers(Type type)
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
        internal static string GetName(RuntimeContext context, Type type)
        {
            Debug.Assert(!type.Name.EndsWith("[]") && !context.AttributeHelper.IsJavaModule(type.Module));

            var name = type.FullName;
            if (name == null)
            {
                // generic type parameters don't have a full name
                return null;
            }

            if (type.IsGenericType && !type.ContainsGenericParameters)
            {
                var sb = new ValueStringBuilder();
                sb.Append(MangleTypeName(type.GetGenericTypeDefinition().FullName));
                sb.Append("_$$$_");
                var sep = "";
                foreach (var t1 in type.GetGenericArguments())
                {
                    var t = t1;
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

                    if (RuntimePrimitiveJavaType.IsPrimitiveType(context, t))
                    {
                        sb.Append(context.ClassLoaderFactory.GetJavaTypeFromType(t).SignatureName);
                    }
                    else
                    {
                        string s;
                        if (context.ClassLoaderFactory.IsRemappedType(t) || context.AttributeHelper.IsJavaModule(t.Module))
                            s = context.ClassLoaderFactory.GetJavaTypeFromType(t).Name;
                        else
                            s = RuntimeManagedJavaType.GetName(context, t);

                        // only do the mangling for non-generic types (because we don't want to convert
                        // the double underscores in two adjacent _$$$_ or _$$$$_ markers)
                        if (s.IndexOf("_$$$_") == -1)
                        {
                            s = s.Replace("__", "$$005F$$005F");
                            s = s.Replace(".", "__");
                        }

                        sb.Append('L');
                        sb.Append(s);
                    }

                    sep = "_$$_";
                }

                sb.Append("_$$$$_");
                return sb.ToString();
            }

            if (context.AttributeHelper.IsNoPackagePrefix(type) && name.IndexOf('$') == -1)
                return name.Replace('+', '$');

            return MangleTypeName(name);
        }

        static string MangleTypeName(string name)
        {
            var sb = new ValueStringBuilder(NamePrefix.Length + name.Length);
            sb.Append(NamePrefix);

            var escape = false;
            var nested = false;
            for (int i = 0; i < name.Length; i++)
            {
                var c = name[i];
                if (c == '+' && !escape && (sb.Length == 0 || sb[sb.Length - 1] != '$'))
                {
                    nested = true;
                    sb.Append('$');
                }
                else if ("_0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(c) != -1 || (c == '.' && !escape && !nested))
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
            if (name.StartsWith(NamePrefix, StringComparison.Ordinal) == false)
                return name.Replace('$', '+');

            var sb = new ValueStringBuilder(name.Length - NamePrefix.Length);
            for (int i = NamePrefix.Length; i < name.Length; i++)
            {
                var c = name[i];
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
                            return name;

                        int digit0 = "0123456789ABCDEF".IndexOf(name[++i]);
                        int digit1 = "0123456789ABCDEF".IndexOf(name[++i]);
                        int digit2 = "0123456789ABCDEF".IndexOf(name[++i]);
                        int digit3 = "0123456789ABCDEF".IndexOf(name[++i]);
                        if (digit0 == -1 || digit1 == -1 || digit2 == -1 || digit3 == -1)
                            return name;

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
                return false;
#endif

            return true;
        }

        internal static RuntimeJavaType Create(RuntimeContext context, Type type, string name)
        {
            if (type.ContainsGenericParameters)
            {
                return new OpenGenericJavaType(context, type, name);
            }
            else
            {
                return new RuntimeManagedJavaType(context, type, name);
            }
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <param name="name"></param>
        RuntimeManagedJavaType(RuntimeContext context, Type type, string name) :
            base(context, TypeFlags.None, GetModifiers(type), name)
        {
            Debug.Assert(!type.IsByRef, type.FullName);
            Debug.Assert(!type.IsPointer, type.FullName);
            Debug.Assert(!type.Name.EndsWith("[]"), type.FullName);
            Debug.Assert(type is not TypeBuilder, type.FullName);
            Debug.Assert(!Context.AttributeHelper.IsJavaModule(type.Module));

            this.type = type;
        }

        internal override RuntimeJavaType BaseTypeWrapper => baseTypeWrapper ??= Context.ManagedJavaTypeFactory.GetBaseJavaType(type);

        internal override RuntimeClassLoader ClassLoader => type.IsGenericType ? Context.ClassLoaderFactory.GetGenericClassLoader(this) : Context.AssemblyClassLoaderFactory.FromAssembly(type.Assembly);

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

        protected override void LazyPublishMembers()
        {
            // special support for enums
            if (type.IsEnum)
            {
                Type underlyingType = EnumHelper.GetUnderlyingType(type);
                Type javaUnderlyingType;
                if (underlyingType == Context.Types.SByte)
                {
                    javaUnderlyingType = Context.Types.Byte;
                }
                else if (underlyingType == Context.Types.UInt16)
                {
                    javaUnderlyingType = Context.Types.Int16;
                }
                else if (underlyingType == Context.Types.UInt32)
                {
                    javaUnderlyingType = Context.Types.Int32;
                }
                else if (underlyingType == Context.Types.UInt64)
                {
                    javaUnderlyingType = Context.Types.Int64;
                }
                else
                {
                    javaUnderlyingType = underlyingType;
                }

                var fieldType = Context.ClassLoaderFactory.GetJavaTypeFromType(javaUnderlyingType);
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

                        var val = EnumHelper.GetPrimitiveValue(Context, underlyingType, fields[i].GetRawConstantValue());
                        fieldsList.Add(new RuntimeConstantJavaField(this, fieldType, name, fieldType.SignatureName, Modifiers.Public | Modifiers.Static | Modifiers.Final, fields[i], val, MemberFlags.None));
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
                        fieldsList.Add(CreateFieldWrapperDotNet(Context.AttributeHelper.GetModifiers(fields[i], true).Modifiers, fields[i].Name, fields[i].FieldType, fields[i]));
                    }
                }
                SetFields(fieldsList.ToArray());

                var methodsList = new Dictionary<string, RuntimeJavaMethod>();

                // special case for delegate constructors!
                if (IsDelegate(Context, type))
                {
                    var iface = InnerClasses[0];
                    var mw = new DelegateJavaMethod(this, (DelegateInnerClassJavaType)iface);
                    methodsList.Add(mw.Name + mw.Signature, mw);
                }

                // add a protected default constructor to MulticastDelegate to make it easier to define a delegate in Java
                if (type == Context.Types.MulticastDelegate)
                    methodsList.Add("<init>()V", new MulticastDelegateCtorJavaMethod(this));

                var constructors = type.GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                for (int i = 0; i < constructors.Length; i++)
                {
                    if (MakeMethodDescriptor(constructors[i], out var name, out var sig, out var args, out var ret))
                    {
                        var mw = CreateMethodWrapper(name, sig, args, ret, constructors[i], false);
                        var key = mw.Name + mw.Signature;
                        if (methodsList.ContainsKey(key) == false)
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
                                var baseMethod = BaseTypeWrapper.GetMethod(name, sig, true);
                                if (baseMethod != null && baseMethod.IsFinal && !baseMethod.IsStatic && !baseMethod.IsPrivate)
                                    continue;
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
                            if (Context.ClassLoaderFactory.IsRemappedType(interfaces[i]))
                            {
                                var tw = Context.ClassLoaderFactory.GetJavaTypeFromType(interfaces[i]);
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
                if (Context.ClassLoaderFactory.IsRemappedType(type) && !type.IsSealed && !type.IsInterface)
                {
                    var baseTypeWrapper = BaseTypeWrapper;

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

#endif

                var methodArray = new RuntimeJavaMethod[methodsList.Count];
                methodsList.Values.CopyTo(methodArray, 0);
                SetMethods(methodArray);
            }
        }

        void InterfaceMethodStubHelper(Dictionary<string, RuntimeJavaMethod> methodsList, MethodBase method, string name, string sig, RuntimeJavaType[] args, RuntimeJavaType ret)
        {
            var key = name + sig;
            methodsList.TryGetValue(key, out var existing);
            if (existing == null && BaseTypeWrapper != null)
            {
                var baseMethod = BaseTypeWrapper.GetMethod(name, sig, true);
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

        internal static bool IsUnsupportedAbstractMethod(MethodBase mb)
        {
            if (mb.IsAbstract)
            {
                var mi = (MethodInfo)mb;
                if (mi.ReturnType.IsByRef || IsPointerType(mi.ReturnType) || mb.IsGenericMethodDefinition)
                    return true;

                foreach (var p in mi.GetParameters())
                    if (p.ParameterType.IsByRef || IsPointerType(p.ParameterType))
                        return true;
            }

            return false;
        }

        static bool IsPointerType(Type type)
        {
            while (type.HasElementType)
            {
                if (type.IsPointer)
                    return true;

                type = type.GetElementType();
            }

#if IMPORTER || EXPORTER
            return type.IsFunctionPointer;
#else
#if NET8_0_OR_GREATER
            return type.IsFunctionPointer;
#else
            return false;
#endif
#endif
        }

        bool MakeMethodDescriptor(MethodBase mb, out string name, out string sig, out RuntimeJavaType[] args, out RuntimeJavaType ret)
        {
            if (mb.IsGenericMethodDefinition)
            {
                name = null;
                sig = null;
                args = null;
                ret = null;
                return false;
            }

            var sb = new ValueStringBuilder();
            sb.Append('(');
            var parameters = mb.GetParameters();
            args = new RuntimeJavaType[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                var type = parameters[i].ParameterType;
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

                var tw = Context.ClassLoaderFactory.GetJavaTypeFromType(type);
                args[i] = tw;
                sb.Append(tw.SignatureName);
            }
            sb.Append(')');
            if (mb is ConstructorInfo)
            {
                ret = Context.PrimitiveJavaTypeFactory.VOID;
                name = mb.IsStatic ? "<clinit>" : "<init>";
                sb.Append(ret.SignatureName);
                sig = sb.ToString();
                return true;
            }
            else
            {
                var type = ((MethodInfo)mb).ReturnType;
                if (IsPointerType(type) || type.IsByRef)
                {
                    name = null;
                    sig = null;
                    ret = null;
                    return false;
                }
                ret = Context.ClassLoaderFactory.GetJavaTypeFromType(type);
                sb.Append(ret.SignatureName);
                name = mb.Name;
                sig = sb.ToString();
                return true;
            }
        }

        internal override RuntimeJavaType[] Interfaces => interfaces ??= GetImplementedInterfacesAsTypeWrappers(Context, type);

        static bool IsAttribute(RuntimeContext context, Type type)
        {
            if (!type.IsAbstract && type.IsSubclassOf(context.Types.Attribute) && type.IsVisible)
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
                AttributeAnnotationJavaType.GetConstructors(context, type, out var defCtor, out var singleOneArgCtor);
                return defCtor != null || singleOneArgCtor != null;
            }

            return false;
        }

        static bool IsDelegate(RuntimeContext context, Type type)
        {
            // HACK non-public delegates do not get the special treatment (because they are likely to refer to
            // non-public types in the arg list and they're not really useful anyway)
            // NOTE we don't have to check in what assembly the type lives, because this is a DotNetTypeWrapper,
            // we know that it is a different assembly.
            if (!type.IsAbstract && type.IsSubclassOf(context.Types.MulticastDelegate) && type.IsVisible)
            {
                var invoke = type.GetMethod("Invoke");
                if (invoke != null)
                {
                    foreach (var p in invoke.GetParameters())
                    {
                        // we don't support delegates with pointer parameters
                        if (IsPointerType(p.ParameterType))
                            return false;
                    }

                    return !IsPointerType(invoke.ReturnType);
                }
            }

            return false;
        }

        internal override RuntimeJavaType[] InnerClasses => innerClasses ??= GetInnerClasses();

        RuntimeJavaType[] GetInnerClasses()
        {
            var nestedTypes = type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic);
            var list = new List<RuntimeJavaType>(nestedTypes.Length);
            for (int i = 0; i < nestedTypes.Length; i++)
                if (!nestedTypes[i].IsGenericTypeDefinition)
                    list.Add(Context.ClassLoaderFactory.GetJavaTypeFromType(nestedTypes[i]));

            if (IsDelegate(Context, type))
                list.Add(ClassLoader.RegisterInitiatingLoader(new DelegateInnerClassJavaType(Context, Name + DelegateInterfaceSuffix, type)));

            if (IsAttribute(Context, type))
                list.Add(ClassLoader.RegisterInitiatingLoader(new AttributeAnnotationJavaType(Context, Name + AttributeAnnotationSuffix, type)));

            if (type.IsEnum && type.IsVisible)
                list.Add(ClassLoader.RegisterInitiatingLoader(new EnumEnumJavaType(Context, Name + EnumEnumSuffix, type)));

            return list.ToArray();
        }

        internal override bool IsFakeTypeContainer => IsDelegate(Context, type) || IsAttribute(Context, type) || (type.IsEnum && type.IsVisible);

        internal override RuntimeJavaType DeclaringTypeWrapper
        {
            get
            {
                if (outerClass == null)
                {
                    var outer = type.DeclaringType;
                    if (outer != null && !type.IsGenericType)
                        outerClass = Context.ManagedJavaTypeFactory.GetJavaTypeFromManagedType(outer);
                }

                return outerClass;
            }
        }

        internal override Modifiers ReflectiveModifiers => DeclaringTypeWrapper != null ? Modifiers | Modifiers.Static : Modifiers;

        RuntimeJavaField CreateFieldWrapperDotNet(Modifiers modifiers, string name, Type fieldType, FieldInfo field)
        {
            var type = Context.ClassLoaderFactory.GetJavaTypeFromType(fieldType);
            if (field.IsLiteral)
                return new RuntimeConstantJavaField(this, type, name, type.SignatureName, modifiers, field, null, MemberFlags.None);
            else
                return RuntimeJavaField.Create(this, type, field, name, type.SignatureName, new ExModifiers(modifiers, false));
        }

        /// <summary>
        /// This method detects if type derives from our java.lang.Object or java.lang.Throwable implementation types.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        static bool IsRemappedImplDerived(RuntimeContext context, Type type)
        {
            for (; type != null; type = type.BaseType)
                if (!context.ClassLoaderFactory.IsRemappedType(type) && context.ClassLoaderFactory.GetJavaTypeFromType(type).IsRemapped)
                    return true;

            return false;
        }

        RuntimeJavaMethod CreateMethodWrapper(string name, string sig, RuntimeJavaType[] argTypeWrappers, RuntimeJavaType retTypeWrapper, MethodBase mb, bool privateInterfaceImplHack)
        {
            var exmods = Context.AttributeHelper.GetModifiers(mb, true);
            var mods = exmods.Modifiers;

            if (name == "Finalize" && sig == "()V" && !mb.IsStatic && IsRemappedImplDerived(Context, TypeAsBaseType))
            {
                // TODO if the .NET also has a "finalize" method, we need to hide that one (or rename it, or whatever)
                var mw = new RuntimeSimpleCallJavaMethod(this, "finalize", "()V", (MethodInfo)mb, Context.PrimitiveJavaTypeFactory.VOID, Array.Empty<RuntimeJavaType>(), mods, MemberFlags.None, SimpleOpCode.Call, SimpleOpCode.Callvirt);
                mw.SetDeclaredExceptions(["java.lang.Throwable"]);
                return mw;
            }

            var parameters = mb.GetParameters();
            var args = new Type[parameters.Length];
            var hasByRefArgs = false;
            bool[] byrefs = null;

            for (int i = 0; i < parameters.Length; i++)
            {
                args[i] = parameters[i].ParameterType;
                if (parameters[i].ParameterType.IsByRef)
                {
                    byrefs ??= new bool[args.Length];
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
                if (mb is not ConstructorInfo && !mb.IsStatic)
                    mods |= Modifiers.Final;

                return new ByRefJavaMethod(args, byrefs, this, name, sig, mb, retTypeWrapper, argTypeWrappers, mods, false);
            }
            else
            {
                return new RuntimeTypicalJavaMethod(this, name, sig, mb, retTypeWrapper, argTypeWrappers, mods, MemberFlags.None);
            }
        }

        internal override Type TypeAsTBD => type;

        internal override bool IsRemapped => Context.ClassLoaderFactory.IsRemappedType(type);

#if EMITTERS

        internal override void EmitInstanceOf(CodeEmitter ilgen)
        {
            if (IsRemapped)
            {
                var shadow = Context.ClassLoaderFactory.GetJavaTypeFromType(type);
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
                var shadow = Context.ClassLoaderFactory.GetJavaTypeFromType(type);
                var method = shadow.TypeAsBaseType.GetMethod("__<checkcast>");
                if (method != null)
                {
                    ilgen.Emit(OpCodes.Call, method);
                    return;
                }
            }
            ilgen.EmitCastclass(type);
        }

#endif 

        internal override MethodParametersEntry[] GetMethodParameters(RuntimeJavaMethod mw)
        {
            var mb = mw.GetMethod();
            if (mb == null)
                return null;

            var parameters = mb.GetParameters();
            if (parameters.Length == 0)
                return null;

            var mp = new MethodParametersEntry[parameters.Length];
            var hasName = false;
            for (int i = 0; i < mp.Length; i++)
            {
                var name = parameters[i].Name;
                var empty = string.IsNullOrEmpty(name);
                if (empty)
                    name = "arg" + i;
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
                return null;

            return fi.GetCustomAttributes(false);
        }

        internal override object[] GetMethodAnnotations(RuntimeJavaMethod mw)
        {
            var mb = mw.GetMethod();
            if (mb == null)
                return null;

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

        internal override bool IsFastClassLiteralSafe => type != Context.Types.Void && !type.IsPrimitive && !IsRemapped;

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
