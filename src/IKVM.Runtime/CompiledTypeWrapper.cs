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
using IKVM.Runtime;
using IKVM.Runtime.Syntax;

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

namespace IKVM.Internal
{
    class CompiledTypeWrapper : TypeWrapper
    {

        readonly Type type;

        TypeWrapper baseTypeWrapper = VerifierTypeWrapper.Null;
        volatile TypeWrapper[] interfaces;
        MethodInfo clinitMethod;
        volatile bool clinitMethodSet;
        Modifiers reflectiveModifiers;

        internal static CompiledTypeWrapper newInstance(string name, Type type)
        {
            // TODO since ghost and remapped types can only exist in the core library assembly, we probably
            // should be able to remove the Type.IsDefined() tests in most cases
            if (type.IsValueType && AttributeHelper.IsGhostInterface(type))
            {
                return new CompiledGhostTypeWrapper(name, type);
            }
            else if (AttributeHelper.IsRemappedType(type))
            {
                return new CompiledRemappedTypeWrapper(name, type);
            }
            else
            {
                return new CompiledTypeWrapper(name, type);
            }
        }

        private sealed class CompiledRemappedTypeWrapper : CompiledTypeWrapper
        {
            private readonly Type remappedType;

            internal CompiledRemappedTypeWrapper(string name, Type type)
                : base(name, type)
            {
                RemappedTypeAttribute attr = AttributeHelper.GetRemappedType(type);
                if (attr == null)
                {
                    throw new InvalidOperationException();
                }
                remappedType = attr.Type;
            }

            internal override Type TypeAsTBD
            {
                get
                {
                    return remappedType;
                }
            }

            internal override bool IsRemapped
            {
                get
                {
                    return true;
                }
            }

            protected override void LazyPublishMethods()
            {
                List<MethodWrapper> list = new List<MethodWrapper>();
                const BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
                foreach (ConstructorInfo ctor in type.GetConstructors(bindingFlags))
                {
                    AddMethod(list, ctor);
                }
                foreach (MethodInfo method in type.GetMethods(bindingFlags))
                {
                    AddMethod(list, method);
                }
                // if we're a remapped interface, we need to get the methods from the real interface
                if (remappedType.IsInterface)
                {
                    Type nestedHelper = type.GetNestedType("__Helper", BindingFlags.Public | BindingFlags.Static);
                    foreach (RemappedInterfaceMethodAttribute m in AttributeHelper.GetRemappedInterfaceMethods(type))
                    {
                        MethodInfo method = remappedType.GetMethod(m.MappedTo);
                        MethodInfo mbHelper = method;
                        ExModifiers modifiers = AttributeHelper.GetModifiers(method, false);
                        string name;
                        string sig;
                        TypeWrapper retType;
                        TypeWrapper[] paramTypes;
                        MemberFlags flags = MemberFlags.None;
                        GetNameSigFromMethodBase(method, out name, out sig, out retType, out paramTypes, ref flags);
                        if (nestedHelper != null)
                        {
                            mbHelper = nestedHelper.GetMethod(m.Name);
                            if (mbHelper == null)
                            {
                                mbHelper = method;
                            }
                        }
                        MethodWrapper mw = new CompiledRemappedMethodWrapper(this, m.Name, sig, method, retType, paramTypes, modifiers, false, mbHelper, null);
                        mw.SetDeclaredExceptions(m.Throws);
                        list.Add(mw);
                    }
                }
                SetMethods(list.ToArray());
            }

            private void AddMethod(List<MethodWrapper> list, MethodBase method)
            {
                HideFromJavaFlags flags = AttributeHelper.GetHideFromJavaFlags(method);
                if ((flags & HideFromJavaFlags.Code) == 0
                    && (remappedType.IsSealed || !method.Name.StartsWith("instancehelper_"))
                    && (!remappedType.IsSealed || method.IsStatic))
                {
                    list.Add(CreateRemappedMethodWrapper(method, flags));
                }
            }

            protected override void LazyPublishFields()
            {
                List<FieldWrapper> list = new List<FieldWrapper>();
                FieldInfo[] fields = type.GetFields(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    HideFromJavaFlags hideFromJavaFlags = AttributeHelper.GetHideFromJavaFlags(field);
                    if ((hideFromJavaFlags & HideFromJavaFlags.Code) == 0)
                    {
                        list.Add(CreateFieldWrapper(field, hideFromJavaFlags));
                    }
                }
                SetFields(list.ToArray());
            }

            private MethodWrapper CreateRemappedMethodWrapper(MethodBase mb, HideFromJavaFlags hideFromJavaflags)
            {
                ExModifiers modifiers = AttributeHelper.GetModifiers(mb, false);
                string name;
                string sig;
                TypeWrapper retType;
                TypeWrapper[] paramTypes;
                MemberFlags flags = MemberFlags.None;
                GetNameSigFromMethodBase(mb, out name, out sig, out retType, out paramTypes, ref flags);
                MethodInfo mbHelper = mb as MethodInfo;
                bool hideFromReflection = mbHelper != null && (hideFromJavaflags & HideFromJavaFlags.Reflection) != 0;
                MethodInfo mbNonvirtualHelper = null;
                if (!mb.IsStatic && !mb.IsConstructor)
                {
                    ParameterInfo[] parameters = mb.GetParameters();
                    Type[] argTypes = new Type[parameters.Length + 1];
                    argTypes[0] = remappedType;
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        argTypes[i + 1] = parameters[i].ParameterType;
                    }
                    MethodInfo helper = type.GetMethod("instancehelper_" + mb.Name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static, null, argTypes, null);
                    if (helper != null)
                    {
                        mbHelper = helper;
                    }
                    mbNonvirtualHelper = type.GetMethod("nonvirtualhelper/" + mb.Name, BindingFlags.NonPublic | BindingFlags.Static, null, argTypes, null);
                }
                return new CompiledRemappedMethodWrapper(this, name, sig, mb, retType, paramTypes, modifiers, hideFromReflection, mbHelper, mbNonvirtualHelper);
            }
        }

        private sealed class CompiledGhostTypeWrapper : CompiledTypeWrapper
        {
            private volatile FieldInfo ghostRefField;
            private volatile Type typeAsBaseType;

            internal CompiledGhostTypeWrapper(string name, Type type)
                : base(name, type)
            {
            }

            internal override Type TypeAsBaseType
            {
                get
                {
                    if (typeAsBaseType == null)
                    {
                        typeAsBaseType = type.GetNestedType("__Interface");
                    }
                    return typeAsBaseType;
                }
            }

            internal override FieldInfo GhostRefField
            {
                get
                {
                    if (ghostRefField == null)
                    {
                        ghostRefField = type.GetField("__<ref>");
                    }
                    return ghostRefField;
                }
            }

            internal override bool IsGhost
            {
                get
                {
                    return true;
                }
            }

#if !IMPORTER && !EXPORTER && !FIRST_PASS
            internal override object GhostWrap(object obj)
            {
                return type.GetMethod("Cast").Invoke(null, new object[] { obj });
            }

            internal override object GhostUnwrap(object obj)
            {
                return type.GetMethod("ToObject").Invoke(obj, new object[0]);
            }
#endif
        }

        /// <summary>
        /// Gets the Java type name of a Java type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="InternalException"></exception>
        internal static JavaTypeName? GetName(Type type)
        {
            if (type.HasElementType)
                return null;
            if (type.IsGenericType)
                return null;
            if (AttributeHelper.IsJavaModule(type.Module) == false)
                return null;

            // look for our custom attribute, that contains the real name of the type (for inner classes)
            var attr = AttributeHelper.GetInnerClass(type);
            if (attr != null)
            {
                var name = attr.InnerClassName;
                if (name != null)
                    return name;
            }

            // type is an inner type
            if (type.DeclaringType != null)
                return GetName(type.DeclaringType) + "$" + TypeNameUtil.Unescape(type.Name);

            return TypeNameUtil.Unescape(type.FullName);
        }

        static TypeWrapper GetBaseTypeWrapper(Type type)
        {
            if (type.IsInterface || AttributeHelper.IsGhostInterface(type))
            {
                return null;
            }
            else if (type.BaseType == null)
            {
                // System.Object must appear to be derived from java.lang.Object
                return CoreClasses.java.lang.Object.Wrapper;
            }
            else
            {
                var attr = AttributeHelper.GetRemappedType(type);
                if (attr != null)
                {
                    if (attr.Type == Types.Object)
                        return null;
                    else
                        return CoreClasses.java.lang.Object.Wrapper;
                }
                else if (ClassLoaderWrapper.IsRemappedType(type.BaseType))
                {
                    // if we directly extend System.Object or System.Exception, the base class must be cli.System.Object or cli.System.Exception
                    return DotNetTypeWrapper.GetWrapperFromDotNetType(type.BaseType);
                }

                TypeWrapper tw = null;
                while (tw == null)
                {
                    type = type.BaseType;
                    tw = ClassLoaderWrapper.GetWrapperFromType(type);
                }

                return tw;
            }
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="exmod"></param>
        /// <param name="name"></param>
        CompiledTypeWrapper(ExModifiers exmod, string name) :
            base(exmod.IsInternal ? TypeFlags.InternalAccess : TypeFlags.None, exmod.Modifiers, name)
        {

        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        CompiledTypeWrapper(string name, Type type) :
            this(GetModifiers(type), name)
        {
            Debug.Assert(!(type is TypeBuilder));
            Debug.Assert(!type.Name.EndsWith("[]"));

            this.type = type;
        }

        internal override TypeWrapper BaseTypeWrapper
        {
            get
            {
                if (baseTypeWrapper != VerifierTypeWrapper.Null)
                    return baseTypeWrapper;

                return baseTypeWrapper = GetBaseTypeWrapper(type);
            }
        }

        internal override ClassLoaderWrapper GetClassLoader()
        {
            return AssemblyClassLoader.FromAssembly(type.Assembly);
        }

        private static ExModifiers GetModifiers(Type type)
        {
            ModifiersAttribute attr = AttributeHelper.GetModifiersAttribute(type);
            if (attr != null)
            {
                return new ExModifiers(attr.Modifiers, attr.IsInternal);
            }
            // only returns public, protected, private, final, static, abstract and interface (as per
            // the documentation of Class.getModifiers())
            Modifiers modifiers = 0;
            if (type.IsPublic || type.IsNestedPublic)
            {
                modifiers |= Modifiers.Public;
            }
            if (type.IsSealed)
            {
                modifiers |= Modifiers.Final;
            }
            if (type.IsAbstract)
            {
                modifiers |= Modifiers.Abstract;
            }
            if (type.IsInterface)
            {
                modifiers |= Modifiers.Interface;
            }
            else
            {
                modifiers |= Modifiers.Super;
            }

            return new ExModifiers(modifiers, false);
        }

        internal override bool HasStaticInitializer
        {
            get
            {
                if (!clinitMethodSet)
                {
                    try
                    {
                        clinitMethod = type.GetMethod("__<clinit>", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    }
#if IMPORTER
                    catch (IKVM.Reflection.MissingMemberException) { }
#endif
                    finally { }
                    clinitMethodSet = true;
                }
                return clinitMethod != null;
            }
        }

        internal override TypeWrapper[] Interfaces
        {
            get
            {
                if (interfaces == null)
                {
                    interfaces = GetInterfaces();
                }
                return interfaces;
            }
        }

        private TypeWrapper[] GetInterfaces()
        {
            // NOTE instead of getting the interfaces list from Type, we use a custom
            // attribute to list the implemented interfaces, because Java reflection only
            // reports the interfaces *directly* implemented by the type, not the inherited
            // interfaces. This is significant for serialVersionUID calculation (for example).
            ImplementsAttribute attr = AttributeHelper.GetImplements(type);
            if (attr == null)
            {
                if (BaseTypeWrapper == CoreClasses.java.lang.Object.Wrapper)
                {
                    return GetImplementedInterfacesAsTypeWrappers(type);
                }
                return TypeWrapper.EmptyArray;
            }
            string[] interfaceNames = attr.Interfaces;
            TypeWrapper[] interfaceWrappers = new TypeWrapper[interfaceNames.Length];
            if (this.IsRemapped)
            {
                for (int i = 0; i < interfaceWrappers.Length; i++)
                {
                    interfaceWrappers[i] = ClassLoaderWrapper.LoadClassCritical(interfaceNames[i]);
                }
            }
            else
            {
                TypeWrapper[] typeWrappers = GetImplementedInterfacesAsTypeWrappers(type);
                for (int i = 0; i < interfaceWrappers.Length; i++)
                {
                    for (int j = 0; j < typeWrappers.Length; j++)
                    {
                        if (typeWrappers[j].Name == interfaceNames[i])
                        {
                            interfaceWrappers[i] = typeWrappers[j];
                            break;
                        }
                    }
                    if (interfaceWrappers[i] == null)
                    {
#if IMPORTER
                        throw new FatalCompilerErrorException(Message.UnableToResolveInterface, interfaceNames[i], this);
#else
                        throw new InternalException($"Unable to resolve interface {interfaceNames[i]} on type {this}");
#endif
                    }
                }
            }
            return interfaceWrappers;
        }

        private static bool IsNestedTypeAnonymousOrLocalClass(Type type)
        {
            switch (type.Attributes & (TypeAttributes.SpecialName | TypeAttributes.VisibilityMask))
            {
                case TypeAttributes.SpecialName | TypeAttributes.NestedPublic:
                case TypeAttributes.SpecialName | TypeAttributes.NestedAssembly:
                    return AttributeHelper.HasEnclosingMethodAttribute(type);
                default:
                    return false;
            }
        }

        private static bool IsAnnotationAttribute(Type type)
        {
            return type.Name.EndsWith("Attribute", StringComparison.Ordinal)
                && type.IsClass
                && type.BaseType.FullName == "ikvm.internal.AnnotationAttributeBase";
        }

        internal override TypeWrapper[] InnerClasses
        {
            get
            {
                List<TypeWrapper> wrappers = new List<TypeWrapper>();
                foreach (Type nested in type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
                {
                    if (IsAnnotationAttribute(nested))
                    {
                        // HACK it's the custom attribute we generated for a corresponding annotation, so we shouldn't surface it as an inner classes
                        // (we can't put a HideFromJavaAttribute on it, because we do want the class to be visible as a $Proxy)
                    }
                    else if (IsNestedTypeAnonymousOrLocalClass(nested))
                    {
                        // anonymous and local classes are not reported as inner classes
                    }
                    else if (AttributeHelper.IsHideFromJava(nested))
                    {
                        // ignore
                    }
                    else
                    {
                        wrappers.Add(ClassLoaderWrapper.GetWrapperFromType(nested));
                    }
                }
                foreach (string s in AttributeHelper.GetNonNestedInnerClasses(type))
                {
                    wrappers.Add(GetClassLoader().LoadClassByDottedName(s));
                }
                return wrappers.ToArray();
            }
        }

        internal override TypeWrapper DeclaringTypeWrapper
        {
            get
            {
                if (IsNestedTypeAnonymousOrLocalClass(type))
                {
                    return null;
                }
                Type declaringType = type.DeclaringType;
                if (declaringType != null)
                {
                    return ClassLoaderWrapper.GetWrapperFromType(declaringType);
                }
                string decl = AttributeHelper.GetNonNestedOuterClasses(type);
                if (decl != null)
                {
                    return GetClassLoader().LoadClassByDottedName(decl);
                }
                return null;
            }
        }

        // returns true iff name is of the form "...$<n>"
        private static bool IsAnonymousClassName(string name)
        {
            int index = name.LastIndexOf('$') + 1;
            if (index > 1 && index < name.Length)
            {
                while (index < name.Length)
                {
                    if ("0123456789".IndexOf(name[index++]) == -1)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        // This method uses some heuristics to predict the reflective modifiers and if the prediction matches
        // we can avoid storing the InnerClassesAttribute to record the modifiers.
        // The heuristics are based on javac from Java 7.
        internal static Modifiers PredictReflectiveModifiers(TypeWrapper tw)
        {
            Modifiers modifiers = Modifiers.Static | (tw.Modifiers & (Modifiers.Public | Modifiers.Abstract | Modifiers.Interface));
            // javac marks anonymous classes as final, but the InnerClasses attribute access_flags does not have the ACC_FINAL flag set
            if (tw.IsFinal && !IsAnonymousClassName(tw.Name))
            {
                modifiers |= Modifiers.Final;
            }
            // javac uses the this$0 field to store the outer instance reference for non-static inner classes
            foreach (FieldWrapper fw in tw.GetFields())
            {
                if (fw.Name == "this$0")
                {
                    modifiers &= ~Modifiers.Static;
                    break;
                }
            }
            return modifiers;
        }

        internal override Modifiers ReflectiveModifiers
        {
            get
            {
                if (reflectiveModifiers == 0)
                {
                    Modifiers mods;
                    InnerClassAttribute attr = AttributeHelper.GetInnerClass(type);
                    if (attr != null)
                    {
                        // the mask comes from RECOGNIZED_INNER_CLASS_MODIFIERS in src/hotspot/share/vm/classfile/classFileParser.cpp
                        // (minus ACC_SUPER)
                        mods = attr.Modifiers & (Modifiers)0x761F;
                    }
                    else if (type.DeclaringType != null)
                    {
                        mods = PredictReflectiveModifiers(this);
                    }
                    else
                    {
                        // the mask comes from JVM_RECOGNIZED_CLASS_MODIFIERS in src/hotspot/share/vm/prims/jvm.h
                        // (minus ACC_SUPER)
                        mods = Modifiers & (Modifiers)0x7611;
                    }
                    if (IsInterface)
                    {
                        mods |= Modifiers.Abstract;
                    }
                    reflectiveModifiers = mods;
                }
                return reflectiveModifiers;
            }
        }

        internal override Type TypeAsBaseType
        {
            get
            {
                return type;
            }
        }

        private void SigTypePatchUp(string sigtype, ref TypeWrapper type)
        {
            if (sigtype != type.SigName)
            {
                // if type is an array, we know that it is a ghost array, because arrays of unloadable are compiled
                // as object (not as arrays of object)
                if (type.IsArray)
                {
                    type = GetClassLoader().FieldTypeWrapperFromSig(sigtype, LoadMode.LoadOrThrow);
                }
                else if (type.IsPrimitive)
                {
                    type = DotNetTypeWrapper.GetWrapperFromDotNetType(type.TypeAsTBD);
                    if (sigtype != type.SigName)
                    {
                        throw new InvalidOperationException();
                    }
                }
                else if (type.IsNonPrimitiveValueType)
                {
                    // this can't happen and even if it does happen we cannot return
                    // UnloadableTypeWrapper because that would result in incorrect code
                    // being generated
                    throw new InvalidOperationException();
                }
                else
                {
                    if (sigtype[0] == 'L')
                    {
                        sigtype = sigtype.Substring(1, sigtype.Length - 2);
                    }
                    try
                    {
                        TypeWrapper tw = GetClassLoader().LoadClassByDottedNameFast(sigtype);
                        if (tw != null && tw.IsRemapped)
                        {
                            type = tw;
                            return;
                        }
                    }
                    catch (RetargetableJavaException)
                    {
                    }
                    type = new UnloadableTypeWrapper(sigtype);
                }
            }
        }

        private static void ParseSig(string sig, out string[] sigparam, out string sigret)
        {
            List<string> list = new List<string>();
            int pos = 1;
            for (; ; )
            {
                switch (sig[pos])
                {
                    case 'L':
                        {
                            int end = sig.IndexOf(';', pos) + 1;
                            list.Add(sig.Substring(pos, end - pos));
                            pos = end;
                            break;
                        }
                    case '[':
                        {
                            int skip = 1;
                            while (sig[pos + skip] == '[') skip++;
                            if (sig[pos + skip] == 'L')
                            {
                                int end = sig.IndexOf(';', pos) + 1;
                                list.Add(sig.Substring(pos, end - pos));
                                pos = end;
                            }
                            else
                            {
                                skip++;
                                list.Add(sig.Substring(pos, skip));
                                pos += skip;
                            }
                            break;
                        }
                    case ')':
                        sigparam = list.ToArray();
                        sigret = sig.Substring(pos + 1);
                        return;
                    default:
                        list.Add(sig.Substring(pos, 1));
                        pos++;
                        break;
                }
            }
        }

        private static bool IsCallerID(Type type)
        {
#if EXPORTER
            return type.FullName == "ikvm.internal.CallerID";
#else
            return type == CoreClasses.ikvm.@internal.CallerID.Wrapper.TypeAsSignatureType;
#endif
        }

        private static bool IsCallerSensitive(MethodBase mb)
        {
#if FIRST_PASS
            return false;
#elif IMPORTER || EXPORTER
            foreach (CustomAttributeData cad in mb.GetCustomAttributesData())
            {
                if (cad.AttributeType.FullName == "sun.reflect.CallerSensitiveAttribute")
                {
                    return true;
                }
            }
            return false;
#else
            return mb.IsDefined(typeof(global::sun.reflect.CallerSensitiveAttribute), false);
#endif
        }

        private void GetNameSigFromMethodBase(MethodBase method, out string name, out string sig, out TypeWrapper retType, out TypeWrapper[] paramTypes, ref MemberFlags flags)
        {
            retType = method is ConstructorInfo ? PrimitiveTypeWrapper.VOID : GetParameterTypeWrapper(((MethodInfo)method).ReturnParameter);
            ParameterInfo[] parameters = method.GetParameters();
            int len = parameters.Length;
            if (len > 0
                && IsCallerID(parameters[len - 1].ParameterType)
                && GetClassLoader() == ClassLoaderWrapper.GetBootstrapClassLoader()
                && IsCallerSensitive(method))
            {
                len--;
                flags |= MemberFlags.CallerID;
            }
            paramTypes = new TypeWrapper[len];
            for (int i = 0; i < len; i++)
            {
                paramTypes[i] = GetParameterTypeWrapper(parameters[i]);
            }
            NameSigAttribute attr = AttributeHelper.GetNameSig(method);
            if (attr != null)
            {
                name = attr.Name;
                sig = attr.Sig;
                string[] sigparams;
                string sigret;
                ParseSig(sig, out sigparams, out sigret);
                // HACK newhelper methods have a return type, but it should be void
                if (name == "<init>")
                {
                    retType = PrimitiveTypeWrapper.VOID;
                }
                SigTypePatchUp(sigret, ref retType);
                // if we have a remapped method, the paramTypes array contains an additional entry for "this" so we have
                // to remove that
                if (paramTypes.Length == sigparams.Length + 1)
                {
                    paramTypes = ArrayUtil.DropFirst(paramTypes);
                }
                Debug.Assert(sigparams.Length == paramTypes.Length);
                for (int i = 0; i < sigparams.Length; i++)
                {
                    SigTypePatchUp(sigparams[i], ref paramTypes[i]);
                }
            }
            else
            {
                if (method is ConstructorInfo)
                {
                    name = method.IsStatic ? "<clinit>" : "<init>";
                }
                else
                {
                    name = method.Name;
                    if (name.StartsWith(NamePrefix.Bridge, StringComparison.Ordinal))
                    {
                        name = name.Substring(NamePrefix.Bridge.Length);
                    }
                    if (method.IsSpecialName)
                    {
                        name = UnicodeUtil.UnescapeInvalidSurrogates(name);
                    }
                }
                if (method.IsSpecialName && method.Name.StartsWith(NamePrefix.DefaultMethod, StringComparison.Ordinal))
                {
                    paramTypes = ArrayUtil.DropFirst(paramTypes);
                }
                System.Text.StringBuilder sb = new System.Text.StringBuilder("(");
                foreach (TypeWrapper tw in paramTypes)
                {
                    sb.Append(tw.SigName);
                }
                sb.Append(")");
                sb.Append(retType.SigName);
                sig = sb.ToString();
            }
        }

        private sealed class DelegateConstructorMethodWrapper : MethodWrapper
        {
            private readonly ConstructorInfo constructor;
            private MethodInfo invoke;

            private DelegateConstructorMethodWrapper(TypeWrapper tw, TypeWrapper iface, ExModifiers mods)
                : base(tw, StringConstants.INIT, "(" + iface.SigName + ")V", null, PrimitiveTypeWrapper.VOID, new TypeWrapper[] { iface }, mods.Modifiers, mods.IsInternal ? MemberFlags.InternalAccess : MemberFlags.None)
            {
            }

            internal DelegateConstructorMethodWrapper(TypeWrapper tw, MethodBase method)
                : this(tw, tw.GetClassLoader().LoadClassByDottedName(tw.Name + DotNetTypeWrapper.DelegateInterfaceSuffix), AttributeHelper.GetModifiers(method, false))
            {
                constructor = (ConstructorInfo)method;
            }

            protected override void DoLinkMethod()
            {
                MethodWrapper mw = GetParameters()[0].GetMethods()[0];
                mw.Link();
                invoke = (MethodInfo)mw.GetMethod();
            }

#if EMITTERS
            internal override void EmitNewobj(CodeEmitter ilgen)
            {
                ilgen.Emit(OpCodes.Dup);
                ilgen.Emit(OpCodes.Ldvirtftn, invoke);
                ilgen.Emit(OpCodes.Newobj, constructor);
            }
#endif // EMITTERS
        }

        protected override void LazyPublishMethods()
        {
            bool isDelegate = type.BaseType == Types.MulticastDelegate;
            List<MethodWrapper> methods = new List<MethodWrapper>();
            const BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
            foreach (ConstructorInfo ctor in type.GetConstructors(flags))
            {
                HideFromJavaFlags hideFromJavaFlags = AttributeHelper.GetHideFromJavaFlags(ctor);
                if (isDelegate && !ctor.IsStatic && (hideFromJavaFlags & HideFromJavaFlags.Code) == 0)
                {
                    methods.Add(new DelegateConstructorMethodWrapper(this, ctor));
                }
                else
                {
                    AddMethodOrConstructor(ctor, hideFromJavaFlags, methods);
                }
            }
            AddMethods(type.GetMethods(flags), methods);
            if (type.IsInterface && (type.IsPublic || type.IsNestedPublic))
            {
                Type privateInterfaceMethods = type.GetNestedType(NestedTypeName.PrivateInterfaceMethods, BindingFlags.NonPublic);
                if (privateInterfaceMethods != null)
                {
                    AddMethods(privateInterfaceMethods.GetMethods(flags), methods);
                }
            }
            SetMethods(methods.ToArray());
        }

        private void AddMethods(MethodInfo[] add, List<MethodWrapper> methods)
        {
            foreach (MethodInfo method in add)
            {
                AddMethodOrConstructor(method, AttributeHelper.GetHideFromJavaFlags(method), methods);
            }
        }

        private void AddMethodOrConstructor(MethodBase method, HideFromJavaFlags hideFromJavaFlags, List<MethodWrapper> methods)
        {
            if ((hideFromJavaFlags & HideFromJavaFlags.Code) != 0)
            {
                if (method.Name.StartsWith(NamePrefix.Incomplete, StringComparison.Ordinal))
                {
                    SetHasIncompleteInterfaceImplementation();
                }
            }
            else
            {
                if (method.IsSpecialName && (method.Name.StartsWith("__<", StringComparison.Ordinal) || method.Name.StartsWith(NamePrefix.DefaultMethod, StringComparison.Ordinal)))
                {
                    // skip
                }
                else
                {
                    string name;
                    string sig;
                    TypeWrapper retType;
                    TypeWrapper[] paramTypes;
                    MethodInfo mi = method as MethodInfo;
                    bool hideFromReflection = mi != null && (hideFromJavaFlags & HideFromJavaFlags.Reflection) != 0;
                    MemberFlags flags = hideFromReflection ? MemberFlags.HideFromReflection : MemberFlags.None;
                    GetNameSigFromMethodBase(method, out name, out sig, out retType, out paramTypes, ref flags);
                    ExModifiers mods = AttributeHelper.GetModifiers(method, false);
                    if (mods.IsInternal)
                    {
                        flags |= MemberFlags.InternalAccess;
                    }
                    if (hideFromReflection && name.StartsWith(NamePrefix.AccessStub, StringComparison.Ordinal))
                    {
                        int id = Int32.Parse(name.Substring(NamePrefix.AccessStub.Length, name.IndexOf('|', NamePrefix.AccessStub.Length) - NamePrefix.AccessStub.Length));
                        name = name.Substring(name.IndexOf('|', NamePrefix.AccessStub.Length) + 1);
                        flags |= MemberFlags.AccessStub;
                        MethodInfo nonvirt = type.GetMethod(NamePrefix.NonVirtual + id, BindingFlags.NonPublic | BindingFlags.DeclaredOnly | BindingFlags.Instance);
                        methods.Add(new AccessStubMethodWrapper(this, name, sig, mi, mi, nonvirt ?? mi, retType, paramTypes, mods.Modifiers & ~Modifiers.Final, flags));
                        return;
                    }
                    MethodInfo impl;
                    MethodWrapper mw;
                    if (IsGhost && (mods.Modifiers & (Modifiers.Static | Modifiers.Private)) == 0)
                    {
                        Type[] types = new Type[paramTypes.Length];
                        for (int i = 0; i < types.Length; i++)
                        {
                            types[i] = paramTypes[i].TypeAsSignatureType;
                        }
                        MethodInfo ifmethod = TypeAsBaseType.GetMethod(method.Name, types);
                        mw = new GhostMethodWrapper(this, name, sig, ifmethod, (MethodInfo)method, retType, paramTypes, mods.Modifiers, flags);
                        if (!mw.IsAbstract)
                        {
                            ((GhostMethodWrapper)mw).SetDefaultImpl(TypeAsSignatureType.GetMethod(NamePrefix.DefaultMethod + method.Name, types));
                        }
                    }
                    else if (method.IsSpecialName && method.Name.StartsWith(NamePrefix.PrivateInterfaceInstanceMethod, StringComparison.Ordinal))
                    {
                        mw = new PrivateInterfaceMethodWrapper(this, name, sig, method, retType, paramTypes, mods.Modifiers, flags);
                    }
                    else if (IsInterface && method.IsAbstract && (mods.Modifiers & Modifiers.Abstract) == 0 && (impl = GetDefaultInterfaceMethodImpl(mi, sig)) != null)
                    {
                        mw = new DefaultInterfaceMethodWrapper(this, name, sig, mi, impl, retType, paramTypes, mods.Modifiers, flags);
                    }
                    else
                    {
                        mw = new TypicalMethodWrapper(this, name, sig, method, retType, paramTypes, mods.Modifiers, flags);
                    }
                    if (mw.HasNonPublicTypeInSignature)
                    {
                        if (mi != null)
                        {
                            MethodInfo stubVirt;
                            MethodInfo stubNonVirt;
                            if (GetType2AccessStubs(name, sig, out stubVirt, out stubNonVirt))
                            {
                                mw = new AccessStubMethodWrapper(this, name, sig, mi, stubVirt, stubNonVirt ?? stubVirt, retType, paramTypes, mw.Modifiers, flags);
                            }
                        }
                        else
                        {
                            ConstructorInfo stub;
                            if (GetType2AccessStub(sig, out stub))
                            {
                                mw = new AccessStubConstructorMethodWrapper(this, sig, (ConstructorInfo)method, stub, paramTypes, mw.Modifiers, flags);
                            }
                        }
                    }
                    methods.Add(mw);
                }
            }
        }

        private MethodInfo GetDefaultInterfaceMethodImpl(MethodInfo method, string expectedSig)
        {
            foreach (MethodInfo candidate in method.DeclaringType.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly))
            {
                if (candidate.IsSpecialName
                    && candidate.Name.StartsWith(NamePrefix.DefaultMethod, StringComparison.Ordinal)
                    && candidate.Name.Length == method.Name.Length + NamePrefix.DefaultMethod.Length
                    && candidate.Name.EndsWith(method.Name, StringComparison.Ordinal))
                {
                    string name;
                    string sig;
                    TypeWrapper retType;
                    TypeWrapper[] paramTypes;
                    MemberFlags flags = MemberFlags.None;
                    GetNameSigFromMethodBase(candidate, out name, out sig, out retType, out paramTypes, ref flags);
                    if (sig == expectedSig)
                    {
                        return candidate;
                    }
                }
            }
            return null;
        }

        private bool GetType2AccessStubs(string name, string sig, out MethodInfo stubVirt, out MethodInfo stubNonVirt)
        {
            stubVirt = null;
            stubNonVirt = null;
            const BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
            foreach (MethodInfo method in type.GetMethods(flags))
            {
                if (AttributeHelper.IsHideFromJava(method))
                {
                    NameSigAttribute attr = AttributeHelper.GetNameSig(method);
                    if (attr != null && attr.Name == name && attr.Sig == sig)
                    {
                        if (method.Name.StartsWith(NamePrefix.NonVirtual, StringComparison.Ordinal))
                        {
                            stubNonVirt = method;
                        }
                        else
                        {
                            stubVirt = method;
                        }
                    }
                }
            }
            return stubVirt != null;
        }

        private bool GetType2AccessStub(string sig, out ConstructorInfo stub)
        {
            stub = null;
            const BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            foreach (ConstructorInfo ctor in type.GetConstructors(flags))
            {
                if (AttributeHelper.IsHideFromJava(ctor))
                {
                    NameSigAttribute attr = AttributeHelper.GetNameSig(ctor);
                    if (attr != null && attr.Sig == sig)
                    {
                        stub = ctor;
                    }
                }
            }
            return stub != null;
        }

        private static int SortFieldByToken(FieldInfo field1, FieldInfo field2)
        {
            return field1.MetadataToken.CompareTo(field2.MetadataToken);
        }

        protected override void LazyPublishFields()
        {
            List<FieldWrapper> fields = new List<FieldWrapper>();
            const BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
            FieldInfo[] rawfields = type.GetFields(flags);
            Array.Sort(rawfields, SortFieldByToken);
            // FXBUG on .NET 3.5 and Mono Type.GetProperties() will not return "duplicate" properties (i.e. that have the same name and type, but differ in custom modifiers).
            // .NET 4.0 works as expected. We don't have a workaround, because that would require name mangling again and this situation is very unlikely anyway.
            PropertyInfo[] properties = type.GetProperties(flags);
            foreach (FieldInfo field in rawfields)
            {
                HideFromJavaFlags hideFromJavaFlags = AttributeHelper.GetHideFromJavaFlags(field);
                if ((hideFromJavaFlags & HideFromJavaFlags.Code) != 0)
                {
                    if (field.Name.StartsWith(NamePrefix.Type2AccessStubBackingField, StringComparison.Ordinal))
                    {
                        TypeWrapper tw = GetFieldTypeWrapper(field);
                        string name = field.Name.Substring(NamePrefix.Type2AccessStubBackingField.Length);
                        for (int i = 0; i < properties.Length; i++)
                        {
                            if (properties[i] != null
                                && name == properties[i].Name
                                && MatchTypes(tw, GetPropertyTypeWrapper(properties[i])))
                            {
                                fields.Add(new CompiledAccessStubFieldWrapper(this, properties[i], field, tw));
                                properties[i] = null;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (field.IsSpecialName && field.Name.StartsWith("__<", StringComparison.Ordinal))
                    {
                        // skip
                    }
                    else
                    {
                        fields.Add(CreateFieldWrapper(field, hideFromJavaFlags));
                    }
                }
            }
            foreach (PropertyInfo property in properties)
            {
                if (property != null)
                {
                    AddPropertyFieldWrapper(fields, property, null);
                }
            }
            SetFields(fields.ToArray());
        }

        private static bool MatchTypes(TypeWrapper tw1, TypeWrapper tw2)
        {
            return tw1 == tw2 || (tw1.IsUnloadable && tw2.IsUnloadable && tw1.Name == tw2.Name);
        }

        private void AddPropertyFieldWrapper(List<FieldWrapper> fields, PropertyInfo property, FieldInfo field)
        {
            // NOTE explictly defined properties (in map.xml) are decorated with HideFromJava,
            // so we don't need to worry about them here
            HideFromJavaFlags hideFromJavaFlags = AttributeHelper.GetHideFromJavaFlags(property);
            if ((hideFromJavaFlags & HideFromJavaFlags.Code) == 0)
            {
                // is it a type 1 access stub?
                if ((hideFromJavaFlags & HideFromJavaFlags.Reflection) != 0)
                {
                    fields.Add(new CompiledAccessStubFieldWrapper(this, property, GetPropertyTypeWrapper(property)));
                }
                else
                {
                    // It must be an explicit property
                    // (defined in Java source by an @ikvm.lang.Property annotation)
                    ModifiersAttribute mods = AttributeHelper.GetModifiersAttribute(property);
                    fields.Add(new CompiledPropertyFieldWrapper(this, property, new ExModifiers(mods.Modifiers, mods.IsInternal)));
                }
            }
        }

        private sealed class CompiledRemappedMethodWrapper : SmartMethodWrapper
        {
            private readonly MethodInfo mbHelper;
#if !IMPORTER
            private readonly MethodInfo mbNonvirtualHelper;
#endif

            internal CompiledRemappedMethodWrapper(TypeWrapper declaringType, string name, string sig, MethodBase method, TypeWrapper returnType, TypeWrapper[] parameterTypes, ExModifiers modifiers, bool hideFromReflection, MethodInfo mbHelper, MethodInfo mbNonvirtualHelper)
                : base(declaringType, name, sig, method, returnType, parameterTypes, modifiers.Modifiers,
                        (modifiers.IsInternal ? MemberFlags.InternalAccess : MemberFlags.None) | (hideFromReflection ? MemberFlags.HideFromReflection : MemberFlags.None))
            {
                this.mbHelper = mbHelper;
#if !IMPORTER
                this.mbNonvirtualHelper = mbNonvirtualHelper;
#endif
            }

#if EMITTERS
            protected override void CallImpl(CodeEmitter ilgen)
            {
                MethodBase mb = GetMethod();
                MethodInfo mi = mb as MethodInfo;
                if (mi != null)
                {
                    if (!IsStatic && IsFinal)
                    {
                        // When calling a final instance method on a remapped type from a class derived from a .NET class (i.e. a cli.System.Object or cli.System.Exception derived base class)
                        // then we can't call the java.lang.Object or java.lang.Throwable methods and we have to go through the instancehelper_ method. Note that since the method
                        // is final, this won't affect the semantics.
                        CallvirtImpl(ilgen);
                    }
                    else
                    {
                        ilgen.Emit(OpCodes.Call, mi);
                    }
                }
                else
                {
                    ilgen.Emit(OpCodes.Call, mb);
                }
            }

            protected override void CallvirtImpl(CodeEmitter ilgen)
            {
                Debug.Assert(!mbHelper.IsStatic || mbHelper.Name.StartsWith("instancehelper_") || mbHelper.DeclaringType.Name == "__Helper");
                if (mbHelper.IsPublic)
                {
                    ilgen.Emit(mbHelper.IsStatic ? OpCodes.Call : OpCodes.Callvirt, mbHelper);
                }
                else
                {
                    // HACK the helper is not public, this means that we're dealing with finalize or clone
                    ilgen.Emit(OpCodes.Callvirt, GetMethod());
                }
            }

            protected override void NewobjImpl(CodeEmitter ilgen)
            {
                MethodBase mb = GetMethod();
                MethodInfo mi = mb as MethodInfo;
                if (mi != null)
                {
                    Debug.Assert(mi.Name == "newhelper");
                    ilgen.Emit(OpCodes.Call, mi);
                }
                else
                {
                    ilgen.Emit(OpCodes.Newobj, mb);
                }
            }
#endif // EMITTERS

#if !IMPORTER && !FIRST_PASS && !EXPORTER
            [HideFromJava]
            internal override object Invoke(object obj, object[] args)
            {
                MethodBase mb = mbHelper != null ? mbHelper : GetMethod();
                if (mb.IsStatic && !IsStatic)
                {
                    args = ArrayUtil.Concat(obj, args);
                    obj = null;
                }
                return InvokeAndUnwrapException(mb, obj, args);
            }

            [HideFromJava]
            internal override object CreateInstance(object[] args)
            {
                MethodBase mb = mbHelper != null ? mbHelper : GetMethod();
                if (mb.IsStatic)
                {
                    return InvokeAndUnwrapException(mb, null, args);
                }
                return base.CreateInstance(args);
            }

            [HideFromJava]
            internal override object InvokeNonvirtualRemapped(object obj, object[] args)
            {
                MethodInfo mi = mbNonvirtualHelper;
                if (mi == null)
                {
                    mi = mbHelper;
                }
                return mi.Invoke(null, ArrayUtil.Concat(obj, args));
            }
#endif // !IMPORTER && !FIRST_PASS && !EXPORTER

#if EMITTERS
            internal override void EmitCallvirtReflect(CodeEmitter ilgen)
            {
                MethodBase mb = mbHelper != null ? mbHelper : GetMethod();
                ilgen.Emit(mb.IsStatic ? OpCodes.Call : OpCodes.Callvirt, mb);
            }
#endif // EMITTERS

            internal string GetGenericSignature()
            {
                SignatureAttribute attr = AttributeHelper.GetSignature(mbHelper != null ? mbHelper : GetMethod());
                if (attr != null)
                {
                    return attr.Signature;
                }
                return null;
            }
        }

        private static TypeWrapper TypeWrapperFromModOpt(Type[] modopt)
        {
            int rank = 0;
            TypeWrapper tw = null;
            foreach (Type type in modopt)
            {
                if (type == JVM.LoadType(typeof(IKVM.Attributes.AccessStub)))
                {
                    // ignore
                }
                else if (type == Types.Array)
                {
                    rank++;
                }
                else if (type == Types.Void || type.IsPrimitive || ClassLoaderWrapper.IsRemappedType(type))
                {
                    tw = DotNetTypeWrapper.GetWrapperFromDotNetType(type);
                }
                else if (type.DeclaringType != null && type.DeclaringType.FullName == UnloadableTypeWrapper.ContainerTypeName)
                {
                    tw = new UnloadableTypeWrapper(TypeNameUtil.UnmangleNestedTypeName(type.Name), type);
                }
                else
                {
                    tw = ClassLoaderWrapper.GetWrapperFromType(type);
                }
            }
            if (rank != 0)
            {
                tw = tw.MakeArrayType(rank);
            }
            return tw;
        }

        private static TypeWrapper GetPropertyTypeWrapper(PropertyInfo property)
        {
            return TypeWrapperFromModOpt(property.GetOptionalCustomModifiers())
                ?? ClassLoaderWrapper.GetWrapperFromType(property.PropertyType);
        }

        internal static TypeWrapper GetFieldTypeWrapper(FieldInfo field)
        {
            return TypeWrapperFromModOpt(field.GetOptionalCustomModifiers())
                ?? ClassLoaderWrapper.GetWrapperFromType(field.FieldType);
        }

        internal static TypeWrapper GetParameterTypeWrapper(ParameterInfo param)
        {
            TypeWrapper tw = TypeWrapperFromModOpt(param.GetOptionalCustomModifiers());
            if (tw != null)
            {
                return tw;
            }
            Type parameterType = param.ParameterType;
            if (parameterType.IsByRef)
            {
                // we only support ByRef parameters for automatically generated delegate invoke stubs
                parameterType = parameterType.GetElementType().MakeArrayType();
            }
            return ClassLoaderWrapper.GetWrapperFromType(parameterType);
        }

        private FieldWrapper CreateFieldWrapper(FieldInfo field, HideFromJavaFlags hideFromJavaFlags)
        {
            ExModifiers modifiers = AttributeHelper.GetModifiers(field, false);
            TypeWrapper type = GetFieldTypeWrapper(field);
            string name = field.Name;

            if (field.IsSpecialName)
            {
                name = UnicodeUtil.UnescapeInvalidSurrogates(name);
            }

            if (field.IsLiteral)
            {
                MemberFlags flags = MemberFlags.None;
                if ((hideFromJavaFlags & HideFromJavaFlags.Reflection) != 0)
                {
                    flags |= MemberFlags.HideFromReflection;
                }
                if (modifiers.IsInternal)
                {
                    flags |= MemberFlags.InternalAccess;
                }
                return new ConstantFieldWrapper(this, type, name, type.SigName, modifiers.Modifiers, field, null, flags);
            }
            else
            {
                return FieldWrapper.Create(this, type, field, name, type.SigName, modifiers);
            }
        }

        internal override Type TypeAsTBD
        {
            get
            {
                return type;
            }
        }

        internal override bool IsMapUnsafeException
        {
            get
            {
                return AttributeHelper.IsExceptionIsUnsafeForMapping(type);
            }
        }

#if EMITTERS
        internal override void EmitRunClassConstructor(CodeEmitter ilgen)
        {
            if (HasStaticInitializer)
            {
                ilgen.Emit(OpCodes.Call, clinitMethod);
            }
        }
#endif // EMITTERS

        internal override string GetGenericSignature()
        {
            SignatureAttribute attr = AttributeHelper.GetSignature(type);
            if (attr != null)
            {
                return attr.Signature;
            }
            return null;
        }

        internal override string GetGenericMethodSignature(MethodWrapper mw)
        {
            if (mw is CompiledRemappedMethodWrapper)
            {
                return ((CompiledRemappedMethodWrapper)mw).GetGenericSignature();
            }
            MethodBase mb = mw.GetMethod();
            if (mb != null)
            {
                SignatureAttribute attr = AttributeHelper.GetSignature(mb);
                if (attr != null)
                {
                    return attr.Signature;
                }
            }
            return null;
        }

        internal override string GetGenericFieldSignature(FieldWrapper fw)
        {
            FieldInfo fi = fw.GetField();
            if (fi != null)
            {
                SignatureAttribute attr = AttributeHelper.GetSignature(fi);
                if (attr != null)
                {
                    return attr.Signature;
                }
            }
            return null;
        }

        internal override MethodParametersEntry[] GetMethodParameters(MethodWrapper mw)
        {
            MethodBase mb = mw.GetMethod();
            if (mb == null)
            {
                // delegate constructor
                return null;
            }
            MethodParametersAttribute attr = AttributeHelper.GetMethodParameters(mb);
            if (attr == null)
            {
                return null;
            }
            if (attr.IsMalformed)
            {
                return MethodParametersEntry.Malformed;
            }
            ParameterInfo[] parameters = mb.GetParameters();
            MethodParametersEntry[] mp = new MethodParametersEntry[attr.Modifiers.Length];
            for (int i = 0; i < mp.Length; i++)
            {
                mp[i].name = i < parameters.Length ? parameters[i].Name : null;
                mp[i].flags = (ushort)attr.Modifiers[i];
            }
            return mp;
        }

#if !IMPORTER && !EXPORTER
        internal override string[] GetEnclosingMethod()
        {
            EnclosingMethodAttribute enc = AttributeHelper.GetEnclosingMethodAttribute(type);
            if (enc != null)
            {
                return new string[] { enc.ClassName, enc.MethodName, enc.MethodSignature };
            }
            return null;
        }

        internal override object[] GetDeclaredAnnotations()
        {
            return type.GetCustomAttributes(false);
        }

        internal override object[] GetMethodAnnotations(MethodWrapper mw)
        {
            MethodBase mb = mw.GetMethod();
            if (mb == null)
            {
                // delegate constructor
                return null;
            }
            return mb.GetCustomAttributes(false);
        }

        internal override object[][] GetParameterAnnotations(MethodWrapper mw)
        {
            MethodBase mb = mw.GetMethod();
            if (mb == null)
            {
                // delegate constructor
                return null;
            }
            ParameterInfo[] parameters = mb.GetParameters();
            int skip = 0;
            if (mb.IsStatic && !mw.IsStatic && mw.Name != "<init>")
            {
                skip = 1;
            }
            int skipEnd = 0;
            if (mw.HasCallerID)
            {
                skipEnd = 1;
            }
            object[][] attribs = new object[parameters.Length - skip - skipEnd][];
            for (int i = skip; i < parameters.Length - skipEnd; i++)
            {
                attribs[i - skip] = parameters[i].GetCustomAttributes(false);
            }
            return attribs;
        }

        internal override object[] GetFieldAnnotations(FieldWrapper fw)
        {
            FieldInfo field = fw.GetField();
            if (field != null)
            {
                return field.GetCustomAttributes(false);
            }
            CompiledPropertyFieldWrapper prop = fw as CompiledPropertyFieldWrapper;
            if (prop != null)
            {
                return prop.GetProperty().GetCustomAttributes(false);
            }
            return new object[0];
        }
#endif

        internal sealed class CompiledAnnotation : Annotation
        {
            private readonly ConstructorInfo constructor;

            internal CompiledAnnotation(Type type)
            {
                constructor = type.GetConstructor(new Type[] { JVM.Import(typeof(object[])) });
            }

            private CustomAttributeBuilder MakeCustomAttributeBuilder(ClassLoaderWrapper loader, object annotation)
            {
                return new CustomAttributeBuilder(constructor, new object[] { AnnotationDefaultAttribute.Escape(QualifyClassNames(loader, annotation)) });
            }

            internal override void Apply(ClassLoaderWrapper loader, TypeBuilder tb, object annotation)
            {
                tb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
            }

            internal override void Apply(ClassLoaderWrapper loader, MethodBuilder mb, object annotation)
            {
                mb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
            }

            internal override void Apply(ClassLoaderWrapper loader, FieldBuilder fb, object annotation)
            {
                fb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
            }

            internal override void Apply(ClassLoaderWrapper loader, ParameterBuilder pb, object annotation)
            {
                pb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
            }

            internal override void Apply(ClassLoaderWrapper loader, AssemblyBuilder ab, object annotation)
            {
                ab.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
            }

            internal override void Apply(ClassLoaderWrapper loader, PropertyBuilder pb, object annotation)
            {
                pb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
            }

            internal override bool IsCustomAttribute
            {
                get { return false; }
            }
        }

        internal override Annotation Annotation
        {
            get
            {
                string annotationAttribute = AttributeHelper.GetAnnotationAttributeType(type);
                if (annotationAttribute != null)
                {
                    return new CompiledAnnotation(type.Assembly.GetType(annotationAttribute, true));
                }
                return null;
            }
        }

        internal override Type EnumType
        {
            get
            {
                if ((this.Modifiers & Modifiers.Enum) != 0)
                {
                    return type.GetNestedType("__Enum");
                }
                return null;
            }
        }

#if !IMPORTER && !EXPORTER
        internal override string GetSourceFileName()
        {
            object[] attr = type.GetCustomAttributes(typeof(SourceFileAttribute), false);
            if (attr.Length == 1)
            {
                return ((SourceFileAttribute)attr[0]).SourceFile;
            }
            if (DeclaringTypeWrapper != null)
            {
                return DeclaringTypeWrapper.GetSourceFileName();
            }
            if (IsNestedTypeAnonymousOrLocalClass(type))
            {
                return ClassLoaderWrapper.GetWrapperFromType(type.DeclaringType).GetSourceFileName();
            }
            if (type.Module.IsDefined(typeof(SourceFileAttribute), false))
            {
                return type.Name + ".java";
            }
            return null;
        }

        internal override int GetSourceLineNumber(MethodBase mb, int ilOffset)
        {
            object[] attr = mb.GetCustomAttributes(typeof(LineNumberTableAttribute), false);
            if (attr.Length == 1)
            {
                return ((LineNumberTableAttribute)attr[0]).GetLineNumber(ilOffset);
            }
            return -1;
        }
#endif

        internal override bool IsFastClassLiteralSafe
        {
            get { return true; }
        }

        internal override object[] GetConstantPool()
        {
            return AttributeHelper.GetConstantPool(type);
        }

        internal override byte[] GetRawTypeAnnotations()
        {
            return AttributeHelper.GetRuntimeVisibleTypeAnnotations(type);
        }

        internal override byte[] GetMethodRawTypeAnnotations(MethodWrapper mw)
        {
            MethodBase mb = mw.GetMethod();
            return mb == null ? null : AttributeHelper.GetRuntimeVisibleTypeAnnotations(mb);
        }

        internal override byte[] GetFieldRawTypeAnnotations(FieldWrapper fw)
        {
            FieldInfo fi = fw.GetField();
            return fi == null ? null : AttributeHelper.GetRuntimeVisibleTypeAnnotations(fi);
        }
    }

}
