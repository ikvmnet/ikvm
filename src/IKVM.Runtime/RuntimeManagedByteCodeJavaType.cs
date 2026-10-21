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
using System.Text;

using IKVM.CoreLib.Diagnostics;
using IKVM.Attributes;
using IKVM.Runtime.Syntax;
using IKVM.ByteCode;
using IKVM.CoreLib.Runtime;

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

    class RuntimeManagedByteCodeJavaTypeFactory
    {

        readonly RuntimeContext context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        public RuntimeManagedByteCodeJavaTypeFactory(RuntimeContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Creates a new instance of the appropriate runtime type.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public RuntimeManagedByteCodeJavaType newInstance(string name, Type type)
        {
            // TODO since ghost and remapped types can only exist in the core library assembly, we probably
            // should be able to remove the Type.IsDefined() tests in most cases
            if (type.IsValueType && context.AttributeHelper.IsGhostInterface(type))
            {
                return new RuntimeManagedByteCodeJavaType.GhostJavaType(context, name, type);
            }
            else if (context.AttributeHelper.IsRemappedType(type))
            {
                return new RuntimeManagedByteCodeJavaType.RemappedJavaType(context, name, type);
            }
            else
            {
                return new RuntimeManagedByteCodeJavaType(context, name, type);
            }
        }

    }

    /// <summary>
    /// Represents a runtime Java type derived from a .NET assembly which was the result of the IKVM compiler.
    /// </summary>
    internal partial class RuntimeManagedByteCodeJavaType : RuntimeJavaType
    {

        readonly Type type;

        RuntimeJavaType baseTypeWrapper;
        volatile RuntimeJavaType[] interfaces;
        MethodInfo clinitMethod;
        volatile bool clinitMethodSet;
        Modifiers reflectiveModifiers;

        /// <summary>
        /// Gets the Java type name of a Java type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="InternalException"></exception>
        internal static JavaTypeName? GetName(RuntimeContext context, Type type)
        {
            if (type.HasElementType)
                return null;
            if (type.IsGenericType)
                return null;
            if (context.AttributeHelper.IsJavaModule(type.Module) == false)
                return null;

            // look for our custom attribute, that contains the real name of the type (for inner classes)
            var attr = context.AttributeHelper.GetInnerClass(type);
            if (attr != null)
            {
                var name = attr.InnerClassName;
                if (name != null)
                    return name;
            }

            // type is an inner type
            if (type.DeclaringType != null)
                return GetName(context, type.DeclaringType) + "$" + TypeNameUtil.Unescape(type.Name);

            return TypeNameUtil.Unescape(type.FullName);
        }

        static RuntimeJavaType GetBaseTypeWrapper(RuntimeContext context, Type type)
        {
            if (type.IsInterface || context.AttributeHelper.IsGhostInterface(type))
            {
                return null;
            }
            else if (type.BaseType == null)
            {
                // System.Object must appear to be derived from java.lang.Object
                return context.JavaBase.TypeOfJavaLangObject;
            }
            else
            {
                var attr = context.AttributeHelper.GetRemappedType(type);
                if (attr != null)
                {
                    if (attr.Type == context.Types.Object)
                        return null;
                    else
                        return context.JavaBase.TypeOfJavaLangObject;
                }
                else if (context.ClassLoaderFactory.IsRemappedType(type.BaseType))
                {
                    // if we directly extend System.Object or System.Exception, the base class must be cli.System.Object or cli.System.Exception
                    return context.ManagedJavaTypeFactory.GetJavaTypeFromManagedType(type.BaseType);
                }

                RuntimeJavaType tw = null;
                while (tw == null)
                {
                    type = type.BaseType;
                    tw = context.ClassLoaderFactory.GetJavaTypeFromType(type);
                }

                return tw;
            }
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exmod"></param>
        /// <param name="name"></param>
        public RuntimeManagedByteCodeJavaType(RuntimeContext context, ExModifiers exmod, string name) :
            base(context, exmod.IsInternal ? TypeFlags.InternalAccess : TypeFlags.None, exmod.Modifiers, name)
        {
            baseTypeWrapper = context.VerifierJavaTypeFactory.Null;
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        public RuntimeManagedByteCodeJavaType(RuntimeContext context, string name, Type type) :
            this(context, GetModifiers(context, type), name)
        {
            Debug.Assert(!(type is TypeBuilder));
            Debug.Assert(!type.Name.EndsWith("[]"));

            this.type = type;
        }

        internal override RuntimeJavaType BaseTypeWrapper
        {
            get
            {
                if (baseTypeWrapper != Context.VerifierJavaTypeFactory.Null)
                    return baseTypeWrapper;

                return baseTypeWrapper = GetBaseTypeWrapper(Context, type);
            }
        }

        internal override RuntimeClassLoader ClassLoader => Context.AssemblyClassLoaderFactory.FromAssembly(type.Assembly);

        static ExModifiers GetModifiers(RuntimeContext context, Type type)
        {
            ModifiersAttribute attr = context.AttributeHelper.GetModifiersAttribute(type);
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

        internal override RuntimeJavaType[] Interfaces
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

        private RuntimeJavaType[] GetInterfaces()
        {
            // NOTE instead of getting the interfaces list from Type, we use a custom
            // attribute to list the implemented interfaces, because Java reflection only
            // reports the interfaces *directly* implemented by the type, not the inherited
            // interfaces. This is significant for serialVersionUID calculation (for example).
            var attr = Context.AttributeHelper.GetImplements(type);
            if (attr == null)
            {
                if (BaseTypeWrapper == Context.JavaBase.TypeOfJavaLangObject)
                    return GetImplementedInterfacesAsTypeWrappers(Context, type);

                return Array.Empty<RuntimeJavaType>();
            }

            var interfaceNames = attr.Interfaces;
            var interfaceWrappers = new RuntimeJavaType[interfaceNames.Length];
            if (IsRemapped)
            {
                for (int i = 0; i < interfaceWrappers.Length; i++)
                    interfaceWrappers[i] = Context.ClassLoaderFactory.LoadClassCritical(interfaceNames[i]);
            }
            else
            {
                var typeWrappers = GetImplementedInterfacesAsTypeWrappers(Context, type);
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
                        throw new FatalCompilerErrorException(DiagnosticEvent.UnableToResolveInterface(interfaceNames[i], ToString()));
#else
                        throw new InternalException($"Unable to resolve interface {interfaceNames[i]} on type {this}");
#endif
                    }
                }
            }

            return interfaceWrappers;
        }

        private bool IsNestedTypeAnonymousOrLocalClass(Type type)
        {
            switch (type.Attributes & (TypeAttributes.SpecialName | TypeAttributes.VisibilityMask))
            {
                case TypeAttributes.SpecialName | TypeAttributes.NestedPublic:
                case TypeAttributes.SpecialName | TypeAttributes.NestedAssembly:
                    return Context.AttributeHelper.HasEnclosingMethodAttribute(type);
                default:
                    return false;
            }
        }

        private bool IsAnnotationAttribute(Type type)
        {
            return type.Name.EndsWith("Attribute", StringComparison.Ordinal) && type.IsClass && type.BaseType.FullName == "ikvm.internal.AnnotationAttributeBase";
        }

        internal override RuntimeJavaType[] InnerClasses
        {
            get
            {
                var wrappers = new List<RuntimeJavaType>();
                foreach (var nested in type.GetNestedTypes(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly))
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
                    else if (Context.AttributeHelper.IsHideFromJava(nested))
                    {
                        // ignore
                    }
                    else
                    {
                        wrappers.Add(Context.ClassLoaderFactory.GetJavaTypeFromType(nested));
                    }
                }
                foreach (string s in Context.AttributeHelper.GetNonNestedInnerClasses(type))
                {
                    wrappers.Add(ClassLoader.LoadClassByName(s));
                }

                return wrappers.ToArray();
            }
        }

        internal override RuntimeJavaType DeclaringTypeWrapper
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
                    return Context.ClassLoaderFactory.GetJavaTypeFromType(declaringType);
                }
                string decl = Context.AttributeHelper.GetNonNestedOuterClasses(type);
                if (decl != null)
                {
                    return ClassLoader.LoadClassByName(decl);
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
        internal static Modifiers PredictReflectiveModifiers(RuntimeJavaType tw)
        {
            Modifiers modifiers = Modifiers.Static | (tw.Modifiers & (Modifiers.Public | Modifiers.Abstract | Modifiers.Interface));
            // javac marks anonymous classes as final, but the InnerClasses attribute access_flags does not have the ACC_FINAL flag set
            if (tw.IsFinal && !IsAnonymousClassName(tw.Name))
            {
                modifiers |= Modifiers.Final;
            }
            // javac uses the this$0 field to store the outer instance reference for non-static inner classes
            foreach (RuntimeJavaField fw in tw.GetFields())
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
                    InnerClassAttribute attr = Context.AttributeHelper.GetInnerClass(type);
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

        private void SigTypePatchUp(string sigtype, ref RuntimeJavaType type)
        {
            if (sigtype != type.SignatureName)
            {
                // if type is an array, we know that it is a ghost array, because arrays of unloadable are compiled
                // as object (not as arrays of object)
                if (type.IsArray)
                {
                    type = ClassLoader.FieldTypeWrapperFromSig(sigtype, LoadMode.LoadOrThrow);
                }
                else if (type.IsPrimitive)
                {
                    type = Context.ManagedJavaTypeFactory.GetJavaTypeFromManagedType(type.TypeAsTBD);
                    if (sigtype != type.SignatureName)
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
                        RuntimeJavaType tw = ClassLoader.TryLoadClassByName(sigtype);
                        if (tw != null && tw.IsRemapped)
                        {
                            type = tw;
                            return;
                        }
                    }
                    catch (RetargetableJavaException)
                    {
                    }
                    type = new RuntimeUnloadableJavaType(Context, sigtype);
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

        private static bool IsCallerID(RuntimeContext context, Type type)
        {
#if EXPORTER
            return type.FullName == "ikvm.internal.CallerID";
#else
            return type == context.JavaBase.TypeOfIkvmInternalCallerID.TypeAsSignatureType;
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

        void GetNameSigFromMethodBase(MethodBase method, out string name, out string sig, out RuntimeJavaType retType, out RuntimeJavaType[] paramTypes, ref MemberFlags flags)
        {
            retType = method is ConstructorInfo ? Context.PrimitiveJavaTypeFactory.VOID : GetParameterTypeWrapper(Context, ((MethodInfo)method).ReturnParameter);
            var parameters = method.GetParameters();
            int len = parameters.Length;
            if (len > 0 && IsCallerID(Context, parameters[len - 1].ParameterType) && ClassLoader == Context.ClassLoaderFactory.GetBootstrapClassLoader() && IsCallerSensitive(method))
            {
                len--;
                flags |= MemberFlags.CallerID;
            }
            paramTypes = new RuntimeJavaType[len];
            for (int i = 0; i < len; i++)
                paramTypes[i] = GetParameterTypeWrapper(Context, parameters[i]);

            var attr = Context.AttributeHelper.GetNameSig(method);
            if (attr != null)
            {
                name = attr.Name;
                sig = attr.Sig;
                ParseSig(sig, out var sigparams, out var sigret);
                // HACK newhelper methods have a return type, but it should be void
                if (name == "<init>")
                    retType = Context.PrimitiveJavaTypeFactory.VOID;
                SigTypePatchUp(sigret, ref retType);
                // if we have a remapped method, the paramTypes array contains an additional entry for "this" so we have
                // to remove that
                if (paramTypes.Length == sigparams.Length + 1)
                    paramTypes = ArrayUtil.DropFirst(paramTypes);

                Debug.Assert(sigparams.Length == paramTypes.Length);
                for (int i = 0; i < sigparams.Length; i++)
                    SigTypePatchUp(sigparams[i], ref paramTypes[i]);
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
                        name = name.Substring(NamePrefix.Bridge.Length);
                    if (method.IsSpecialName)
                        name = UnicodeUtil.UnescapeInvalidSurrogates(name);
                }

                if (method.IsSpecialName && method.Name.StartsWith(NamePrefix.DefaultMethod, StringComparison.Ordinal))
                    paramTypes = ArrayUtil.DropFirst(paramTypes);

                var sb = new ValueStringBuilder();
                sb.Append("(");
                foreach (var tw in paramTypes)
                    sb.Append(tw.SignatureName);
                sb.Append(")");
                sb.Append(retType.SignatureName);
                sig = sb.ToString();
            }
        }

        protected override void LazyPublishMethods()
        {
            const BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

            var isDelegate = type.BaseType == Context.Types.MulticastDelegate;
            var methods = new List<RuntimeJavaMethod>();

            foreach (var ctor in type.GetConstructors(flags))
            {
                var hideFromJavaFlags = Context.AttributeHelper.GetHideFromJavaFlags(ctor);
                if (isDelegate && !ctor.IsStatic && (hideFromJavaFlags & HideFromJavaFlags.Code) == 0)
                    methods.Add(new DelegateConstructorJavaMethod(this, ctor));
                else
                    AddMethodOrConstructor(ctor, hideFromJavaFlags, methods);
            }

            AddMethods(type.GetMethods(flags), methods);

            if (type.IsInterface && (type.IsPublic || type.IsNestedPublic))
            {
                var privateInterfaceMethods = type.GetNestedType(NestedTypeName.PrivateInterfaceMethods, BindingFlags.NonPublic);
                if (privateInterfaceMethods != null)
                    AddMethods(privateInterfaceMethods.GetMethods(flags), methods);
            }

            SetMethods(methods.ToArray());
        }

        private void AddMethods(MethodInfo[] add, List<RuntimeJavaMethod> methods)
        {
            foreach (var method in add)
                AddMethodOrConstructor(method, Context.AttributeHelper.GetHideFromJavaFlags(method), methods);
        }

        private void AddMethodOrConstructor(MethodBase method, HideFromJavaFlags hideFromJavaFlags, List<RuntimeJavaMethod> methods)
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
                    var mi = method as MethodInfo;
                    var hideFromReflection = mi != null && (hideFromJavaFlags & HideFromJavaFlags.Reflection) != 0;
                    var flags = hideFromReflection ? MemberFlags.HideFromReflection : MemberFlags.None;
                    GetNameSigFromMethodBase(method, out var name, out var sig, out var retType, out var paramTypes, ref flags);
                    var mods = Context.AttributeHelper.GetModifiers(method, false);
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
                        methods.Add(new RuntimeAccessStubJavaMethod(this, name, sig, mi, mi, nonvirt ?? mi, retType, paramTypes, mods.Modifiers & ~Modifiers.Final, flags));
                        return;
                    }

                    MethodInfo impl;
                    RuntimeJavaMethod mw;
                    if (IsGhost && (mods.Modifiers & (Modifiers.Static | Modifiers.Private)) == 0)
                    {
                        var types = new Type[paramTypes.Length];
                        for (int i = 0; i < types.Length; i++)
                            types[i] = paramTypes[i].TypeAsSignatureType;

                        var ifmethod = TypeAsBaseType.GetMethod(method.Name, types);
                        mw = new RuntimeGhostJavaMethod(this, name, sig, ifmethod, (MethodInfo)method, retType, paramTypes, mods.Modifiers, flags);
                        if (!mw.IsAbstract)
                            ((RuntimeGhostJavaMethod)mw).SetDefaultImpl(TypeAsSignatureType.GetMethod(NamePrefix.DefaultMethod + method.Name, types));
                    }
                    else if (method.IsSpecialName && method.Name.StartsWith(NamePrefix.PrivateInterfaceInstanceMethod, StringComparison.Ordinal))
                    {
                        mw = new RuntimePrivateInterfaceJavaMethod(this, name, sig, method, retType, paramTypes, mods.Modifiers, flags);
                    }
                    else if (IsInterface && method.IsAbstract && (mods.Modifiers & Modifiers.Abstract) == 0 && (impl = GetDefaultInterfaceMethodImpl(mi, sig)) != null)
                    {
                        mw = new RuntimeDefaultInterfaceJavaMethod(this, name, sig, mi, impl, retType, paramTypes, mods.Modifiers, flags);
                    }
                    else
                    {
                        mw = new RuntimeTypicalJavaMethod(this, name, sig, method, retType, paramTypes, mods.Modifiers, flags);
                    }
                    if (mw.HasNonPublicTypeInSignature)
                    {
                        if (mi != null)
                        {
                            MethodInfo stubVirt;
                            MethodInfo stubNonVirt;
                            if (GetType2AccessStubs(name, sig, out stubVirt, out stubNonVirt))
                            {
                                mw = new RuntimeAccessStubJavaMethod(this, name, sig, mi, stubVirt, stubNonVirt ?? stubVirt, retType, paramTypes, mw.Modifiers, flags);
                            }
                        }
                        else
                        {
                            ConstructorInfo stub;
                            if (GetType2AccessStub(sig, out stub))
                            {
                                mw = new RuntimeConstructorAccessStubJavaMethod(this, sig, (ConstructorInfo)method, stub, paramTypes, mw.Modifiers, flags);
                            }
                        }
                    }
                    methods.Add(mw);
                }
            }
        }

        MethodInfo GetDefaultInterfaceMethodImpl(MethodInfo method, string expectedSig)
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
                    RuntimeJavaType retType;
                    RuntimeJavaType[] paramTypes;
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

        bool GetType2AccessStubs(string name, string sig, out MethodInfo stubVirt, out MethodInfo stubNonVirt)
        {
            const BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

            stubVirt = null;
            stubNonVirt = null;
            foreach (var method in type.GetMethods(flags))
            {
                if (Context.AttributeHelper.IsHideFromJava(method))
                {
                    var attr = Context.AttributeHelper.GetNameSig(method);
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

        bool GetType2AccessStub(string sig, out ConstructorInfo stub)
        {
            const BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

            stub = null;
            foreach (var ctor in type.GetConstructors(flags))
            {
                if (Context.AttributeHelper.IsHideFromJava(ctor))
                {
                    var attr = Context.AttributeHelper.GetNameSig(ctor);
                    if (attr != null && attr.Sig == sig)
                    {
                        stub = ctor;
                    }
                }
            }

            return stub != null;
        }

        static int SortFieldByToken(FieldInfo field1, FieldInfo field2)
        {
            return field1.MetadataToken.CompareTo(field2.MetadataToken);
        }

        protected override void LazyPublishFields()
        {
            const BindingFlags flags = BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

            var fields = new List<RuntimeJavaField>();
            var rawfields = type.GetFields(flags);
            Array.Sort(rawfields, SortFieldByToken);

            // FXBUG on .NET 3.5 and Mono Type.GetProperties() will not return "duplicate" properties (i.e. that have the same name and type, but differ in custom modifiers).
            // .NET 4.0 works as expected. We don't have a workaround, because that would require name mangling again and this situation is very unlikely anyway.
            var properties = type.GetProperties(flags);
            foreach (var field in rawfields)
            {
                var hideFromJavaFlags = Context.AttributeHelper.GetHideFromJavaFlags(field);
                if ((hideFromJavaFlags & HideFromJavaFlags.Code) != 0)
                {
                    if (field.Name.StartsWith(NamePrefix.Type2AccessStubBackingField, StringComparison.Ordinal))
                    {
                        var tw = GetFieldTypeWrapper(Context, field);
                        var name = field.Name.Substring(NamePrefix.Type2AccessStubBackingField.Length);
                        for (int i = 0; i < properties.Length; i++)
                        {
                            if (properties[i] != null && name == properties[i].Name && MatchTypes(tw, GetPropertyTypeWrapper(properties[i])))
                            {
                                fields.Add(new RuntimeManagedByteCodeAccessStubJavaField(this, properties[i], field, tw));
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

            foreach (var property in properties)
                if (property != null)
                    AddPropertyFieldWrapper(fields, property, null);

            SetFields(fields.ToArray());
        }

        static bool MatchTypes(RuntimeJavaType tw1, RuntimeJavaType tw2)
        {
            return tw1 == tw2 || (tw1.IsUnloadable && tw2.IsUnloadable && tw1.Name == tw2.Name);
        }

        void AddPropertyFieldWrapper(List<RuntimeJavaField> fields, PropertyInfo property, FieldInfo field)
        {
            // NOTE explictly defined properties (in map.xml) are decorated with HideFromJava,
            // so we don't need to worry about them here
            var hideFromJavaFlags = Context.AttributeHelper.GetHideFromJavaFlags(property);
            if ((hideFromJavaFlags & HideFromJavaFlags.Code) == 0)
            {
                // is it a type 1 access stub?
                if ((hideFromJavaFlags & HideFromJavaFlags.Reflection) != 0)
                {
                    fields.Add(new RuntimeManagedByteCodeAccessStubJavaField(this, property, GetPropertyTypeWrapper(property)));
                }
                else
                {
                    // It must be an explicit property
                    // (defined in Java source by an @ikvm.lang.Property annotation)
                    var mods = Context.AttributeHelper.GetModifiersAttribute(property);
                    fields.Add(new RuntimeManagedByteCodePropertyJavaField(this, property, new ExModifiers(mods.Modifiers, mods.IsInternal)));
                }
            }
        }

        static RuntimeJavaType TypeWrapperFromModOpt(RuntimeContext context, Type[] modopt)
        {
            int rank = 0;
            RuntimeJavaType tw = null;
            foreach (var type in modopt)
            {
                if (type == context.Resolver.ResolveRuntimeType(typeof(IKVM.Attributes.AccessStub).FullName).AsReflection())
                {
                    // ignore
                }
                else if (type == context.Types.Array)
                {
                    rank++;
                }
                else if (type == context.Types.Void || type.IsPrimitive || context.ClassLoaderFactory.IsRemappedType(type))
                {
                    tw = context.ManagedJavaTypeFactory.GetJavaTypeFromManagedType(type);
                }
                else if (type.DeclaringType != null && type.DeclaringType.FullName == RuntimeUnloadableJavaType.ContainerTypeName)
                {
                    tw = new RuntimeUnloadableJavaType(context, TypeNameUtil.UnmangleNestedTypeName(type.Name), type);
                }
                else
                {
                    tw = context.ClassLoaderFactory.GetJavaTypeFromType(type);
                }
            }
            if (rank != 0)
            {
                tw = tw.MakeArrayType(rank);
            }
            return tw;
        }

        RuntimeJavaType GetPropertyTypeWrapper(PropertyInfo property)
        {
            return TypeWrapperFromModOpt(Context, property.GetOptionalCustomModifiers()) ?? Context.ClassLoaderFactory.GetJavaTypeFromType(property.PropertyType);
        }

        internal static RuntimeJavaType GetFieldTypeWrapper(RuntimeContext context, FieldInfo field)
        {
            return TypeWrapperFromModOpt(context, field.GetOptionalCustomModifiers()) ?? context.ClassLoaderFactory.GetJavaTypeFromType(field.FieldType);
        }

        internal static RuntimeJavaType GetParameterTypeWrapper(RuntimeContext context, ParameterInfo param)
        {
            var tw = TypeWrapperFromModOpt(context, param.GetOptionalCustomModifiers());
            if (tw != null)
                return tw;

            var parameterType = param.ParameterType;
            if (parameterType.IsByRef)
            {
                // we only support ByRef parameters for automatically generated delegate invoke stubs
                parameterType = parameterType.GetElementType().MakeArrayType();
            }

            return context.ClassLoaderFactory.GetJavaTypeFromType(parameterType);
        }

        RuntimeJavaField CreateFieldWrapper(FieldInfo field, HideFromJavaFlags hideFromJavaFlags)
        {
            var modifiers = Context.AttributeHelper.GetModifiers(field, false);
            var type = GetFieldTypeWrapper(Context, field);
            var name = field.Name;

            if (field.IsSpecialName)
                name = UnicodeUtil.UnescapeInvalidSurrogates(name);

            if (field.IsLiteral)
            {
                var flags = MemberFlags.None;
                if ((hideFromJavaFlags & HideFromJavaFlags.Reflection) != 0)
                {
                    flags |= MemberFlags.HideFromReflection;
                }
                if (modifiers.IsInternal)
                {
                    flags |= MemberFlags.InternalAccess;
                }
                return new RuntimeConstantJavaField(this, type, name, type.SignatureName, modifiers.Modifiers, field, null, flags);
            }
            else
            {
                return RuntimeJavaField.Create(this, type, field, name, type.SignatureName, modifiers);
            }
        }

        internal override Type TypeAsTBD => type;

        internal override bool IsMapUnsafeException => Context.AttributeHelper.IsExceptionIsUnsafeForMapping(type);

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
            var attr = Context.AttributeHelper.GetSignature(type);
            if (attr != null)
                return attr.Signature;

            return null;
        }

        internal override string GetGenericMethodSignature(RuntimeJavaMethod method)
        {
            if (method is RemappedJavaMethod remappedMethod)
                return remappedMethod.GetGenericSignature();

            var methodBase = method.GetMethod();
            if (methodBase != null)
            {
                var attr = Context.AttributeHelper.GetSignature(methodBase);
                if (attr != null)
                    return attr.Signature;
            }

            return null;
        }

        internal override string GetGenericFieldSignature(RuntimeJavaField field)
        {
            var fi = field.GetField();
            if (fi != null)
            {
                var attr = Context.AttributeHelper.GetSignature(fi);
                if (attr != null)
                    return attr.Signature;
            }

            return null;
        }

        internal override MethodParametersEntry[] GetMethodParameters(RuntimeJavaMethod method)
        {
            var mb = method.GetMethod();
            if (mb == null)
                return null; // delegate constructor

            var attr = Context.AttributeHelper.GetMethodParameters(mb);
            if (attr == null)
                return null;
            if (attr.IsMalformed)
                return MethodParametersEntry.Malformed;

            var parameters = mb.GetParameters();
            var mp = new MethodParametersEntry[attr.Modifiers.Length];
            for (int i = 0; i < mp.Length; i++)
            {
                mp[i].name = i < parameters.Length ? parameters[i].Name : null;
                mp[i].accessFlags = (AccessFlag)attr.Modifiers[i];
            }

            return mp;
        }

#if !IMPORTER && !EXPORTER

        internal override string[] GetEnclosingMethod()
        {
            var enc = Context.AttributeHelper.GetEnclosingMethodAttribute(type);
            if (enc != null)
                return new string[] { enc.ClassName, enc.MethodName, enc.MethodSignature };

            return null;
        }

        internal override object[] GetDeclaredAnnotations()
        {
            return type.GetCustomAttributes(false);
        }

        internal override object[] GetMethodAnnotations(RuntimeJavaMethod mw)
        {
            var mb = mw.GetMethod();
            if (mb == null)
            {
                // delegate constructor
                return null;
            }

            return mb.GetCustomAttributes(false);
        }

        internal override object[][] GetParameterAnnotations(RuntimeJavaMethod mw)
        {
            var mb = mw.GetMethod();
            if (mb == null)
            {
                // delegate constructor
                return null;
            }

            var parameters = mb.GetParameters();
            int skip = 0;
            if (mb.IsStatic && !mw.IsStatic && mw.Name != "<init>")
                skip = 1;

            int skipEnd = 0;
            if (mw.HasCallerID)
                skipEnd = 1;

            var attribs = new object[parameters.Length - skip - skipEnd][];
            for (int i = skip; i < parameters.Length - skipEnd; i++)
                attribs[i - skip] = parameters[i].GetCustomAttributes(false);

            return attribs;
        }

        internal override object[] GetFieldAnnotations(RuntimeJavaField fw)
        {
            var field = fw.GetField();
            if (field != null)
                return field.GetCustomAttributes(false);

            if (fw is RuntimeManagedByteCodePropertyJavaField prop)
                return prop.GetProperty().GetCustomAttributes(false);

            return Array.Empty<object>();
        }

#endif

        internal sealed class CompiledAnnotation : Annotation
        {

            readonly RuntimeContext context;
            readonly ConstructorInfo constructor;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="context"></param>
            /// <param name="type"></param>
            internal CompiledAnnotation(RuntimeContext context, Type type)
            {
                this.context = context ?? throw new ArgumentNullException(nameof(context));
                constructor = type.GetConstructor(new Type[] { context.Resolver.ResolveCoreType(typeof(object).FullName).MakeArrayType().AsReflection() });
            }

            private CustomAttributeBuilder MakeCustomAttributeBuilder(RuntimeClassLoader loader, object annotation)
            {
                return new CustomAttributeBuilder(constructor, new object[] { AnnotationDefaultAttribute.Escape(QualifyClassNames(loader, annotation)) });
            }

            internal override void Apply(RuntimeClassLoader loader, TypeBuilder tb, object annotation)
            {
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
                pb.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
            }

            internal override void Apply(RuntimeClassLoader loader, AssemblyBuilder ab, object annotation)
            {
                ab.SetCustomAttribute(MakeCustomAttributeBuilder(loader, annotation));
            }

            internal override void Apply(RuntimeClassLoader loader, PropertyBuilder pb, object annotation)
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
                var annotationAttribute = Context.AttributeHelper.GetAnnotationAttributeType(type);
                if (annotationAttribute != null)
                    return new CompiledAnnotation(Context, type.Assembly.GetType(annotationAttribute, true));

                return null;
            }
        }

        internal override Type EnumType
        {
            get
            {
                if ((this.Modifiers & Modifiers.Enum) != 0)
                    return type.GetNestedType("__Enum");

                return null;
            }
        }

#if !IMPORTER && !EXPORTER

        internal override string GetSourceFileName()
        {
            var attr = type.GetCustomAttributes(typeof(SourceFileAttribute), false);
            if (attr.Length == 1)
                return ((SourceFileAttribute)attr[0]).SourceFile;

            if (DeclaringTypeWrapper != null)
                return DeclaringTypeWrapper.GetSourceFileName();

            if (IsNestedTypeAnonymousOrLocalClass(type))
                return Context.ClassLoaderFactory.GetJavaTypeFromType(type.DeclaringType).GetSourceFileName();

            if (type.Module.IsDefined(typeof(SourceFileAttribute), false))
                return type.Name + ".java";

            return null;
        }

        internal override int GetSourceLineNumber(MethodBase mb, int ilOffset)
        {
            var attr = mb.GetCustomAttributes(typeof(LineNumberTableAttribute), false);
            if (attr.Length == 1)
                return ((LineNumberTableAttribute)attr[0]).GetLineNumber(ilOffset);

            return -1;
        }
#endif

        internal override bool IsFastClassLiteralSafe
        {
            get { return true; }
        }

        internal override object[] GetConstantPool()
        {
            return Context.AttributeHelper.GetConstantPool(type);
        }

        internal override byte[] GetRawTypeAnnotations()
        {
            return Context.AttributeHelper.GetRuntimeVisibleTypeAnnotations(type);
        }

        internal override byte[] GetMethodRawTypeAnnotations(RuntimeJavaMethod mw)
        {
            MethodBase mb = mw.GetMethod();
            return mb == null ? null : Context.AttributeHelper.GetRuntimeVisibleTypeAnnotations(mb);
        }

        internal override byte[] GetFieldRawTypeAnnotations(RuntimeJavaField fw)
        {
            FieldInfo fi = fw.GetField();
            return fi == null ? null : Context.AttributeHelper.GetRuntimeVisibleTypeAnnotations(fi);
        }

    }

}
