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

using IKVM.Attributes;
using IKVM.CoreLib.Runtime;
using IKVM.CoreLib.Diagnostics;
using IKVM.CoreLib.Linking;


#if IMPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using IKVM.Tools.Importer;

using Type = IKVM.Reflection.Type;
using ProtectionDomain = System.Object;
#else
using System.Reflection;
using System.Reflection.Emit;

using ProtectionDomain = java.security.ProtectionDomain;
#endif

namespace IKVM.Runtime
{

#if IMPORTER
    abstract partial class RuntimeByteCodeJavaType : RuntimeJavaType
#else
#pragma warning disable 628 // don't complain about protected members in sealed type
    sealed partial class RuntimeByteCodeJavaType : RuntimeJavaType
#endif
    {

#if IMPORTER == false && NETCOREAPP

        static readonly PropertyInfo MetadataTokenInternalPropertyInfo = typeof(MethodBuilder).GetProperty("MetadataTokenInternal", BindingFlags.Instance | BindingFlags.NonPublic);

#endif

#if IMPORTER
        protected readonly ImportClassLoader classLoader;
#else
        protected readonly RuntimeClassLoader classLoader;
#endif
        volatile DynamicImpl impl;
        readonly RuntimeJavaType baseTypeWrapper;
        readonly RuntimeJavaType[] interfaces;
        readonly string sourceFileName;
#if !IMPORTER
        byte[][] lineNumberTables;
#endif
        MethodBase automagicSerializationCtor;

        RuntimeJavaType LoadTypeWrapper(RuntimeClassLoader classLoader, ProtectionDomain pd, ConstantPoolItemClass clazz)
        {
            // check for patched constant pool items
            var tw = clazz.GetClassType();
            if (tw == null || tw == classLoader.Context.VerifierJavaTypeFactory.Null)
                tw = classLoader.TryLoadClassByName(clazz.Name);
            if (tw == null)
                throw new NoClassDefFoundError(clazz.Name);

            CheckMissing(this, tw);
            classLoader.CheckPackageAccess(tw, pd);
            return tw;
        }

        private static void CheckMissing(RuntimeJavaType prev, RuntimeJavaType tw)
        {
#if IMPORTER
            do
            {
                RuntimeUnloadableJavaType missing = tw as RuntimeUnloadableJavaType;
                if (missing != null)
                {
                    Type mt = ReflectUtil.GetMissingType(missing.MissingType);
                    if (mt.Assembly.__IsMissing)
                    {
                        throw new FatalCompilerErrorException(DiagnosticEvent.MissingBaseTypeReference(mt.FullName, mt.Assembly.FullName));
                    }

                    throw new FatalCompilerErrorException(DiagnosticEvent.MissingBaseType(mt.FullName, mt.Assembly.FullName, prev.TypeAsBaseType.FullName, prev.TypeAsBaseType.Module.Name));
                }
                foreach (RuntimeJavaType iface in tw.Interfaces)
                {
                    CheckMissing(tw, iface);
                }
                prev = tw;
                tw = tw.BaseTypeWrapper;
            }
            while (tw != null);
#endif
        }

#if IMPORTER
        internal RuntimeByteCodeJavaType(RuntimeJavaType host, ClassFile f, ImportClassLoader classLoader, ProtectionDomain pd)
#else
        internal RuntimeByteCodeJavaType(RuntimeJavaType host, ClassFile f, RuntimeClassLoader classLoader, ProtectionDomain pd)
#endif
            : base(classLoader.Context, f.IsInternal ? TypeFlags.InternalAccess : host != null ? TypeFlags.Anonymous : TypeFlags.None, (Modifiers)f.AccessFlags, f.Name)
        {
            Profiler.Count("RuntimeByteCodeJavaType");
            this.classLoader = classLoader;
            this.sourceFileName = f.SourceFileAttribute;
            if (f.IsInterface)
            {
                // interfaces can't "override" final methods in object
                foreach (Method method in f.Methods)
                {
                    RuntimeJavaMethod mw;
                    if (method.IsVirtual
                        && (mw = Context.JavaBase.TypeOfJavaLangObject.GetMethod(method.Name, method.Signature, false)) != null
                        && mw.IsVirtual
                        && mw.IsFinal)
                    {
                        throw new VerifyError("class " + f.Name + " overrides final method " + method.Name + "." + method.Signature);
                    }
                }
            }
            else
            {
                this.baseTypeWrapper = LoadTypeWrapper(classLoader, pd, f.SuperClass);
                if (!BaseTypeWrapper.IsAccessibleFrom(this))
                {
                    throw new IllegalAccessError("Class " + f.Name + " cannot access its superclass " + BaseTypeWrapper.Name);
                }
                if (BaseTypeWrapper.IsFinal)
                {
                    throw new VerifyError("Class " + f.Name + " extends final class " + BaseTypeWrapper.Name);
                }
                if (BaseTypeWrapper.IsInterface)
                {
                    throw new IncompatibleClassChangeError("Class " + f.Name + " has interface " + BaseTypeWrapper.Name + " as superclass");
                }
                if (BaseTypeWrapper.TypeAsTBD == Context.Types.Delegate)
                {
                    throw new VerifyError(BaseTypeWrapper.Name + " cannot be used as a base class");
                }
                // NOTE defining value types, enums is not supported in IKVM v1
                if (BaseTypeWrapper.TypeAsTBD == Context.Types.ValueType || BaseTypeWrapper.TypeAsTBD == Context.Types.Enum)
                {
                    throw new VerifyError("Defining value types in Java is not implemented in IKVM v1");
                }
                if (IsDelegate)
                {
                    VerifyDelegate(f);
                }
            }

            ConstantPoolItemClass[] interfaces = f.Interfaces;
            this.interfaces = new RuntimeJavaType[interfaces.Length];
            for (int i = 0; i < interfaces.Length; i++)
            {
                RuntimeJavaType iface = LoadTypeWrapper(classLoader, pd, interfaces[i]);
                if (!iface.IsAccessibleFrom(this))
                {
                    throw new IllegalAccessError("Class " + f.Name + " cannot access its superinterface " + iface.Name);
                }
                if (!iface.IsInterface)
                {
                    throw new IncompatibleClassChangeError("Implementing class");
                }
                this.interfaces[i] = iface;
            }

            impl = new JavaTypeImpl(host, f, this);
        }

        private void VerifyDelegate(ClassFile f)
        {
            if (!f.IsFinal)
            {
                throw new VerifyError("Delegate must be final");
            }
            Method invoke = null;
            Method beginInvoke = null;
            Method endInvoke = null;
            Method constructor = null;
            foreach (Method m in f.Methods)
            {
                if (m.Name == "Invoke")
                {
                    if (invoke != null)
                    {
                        throw new VerifyError("Delegate may only have a single Invoke method");
                    }
                    invoke = m;
                }
                else if (m.Name == "BeginInvoke")
                {
                    if (beginInvoke != null)
                    {
                        throw new VerifyError("Delegate may only have a single BeginInvoke method");
                    }
                    beginInvoke = m;
                }
                else if (m.Name == "EndInvoke")
                {
                    if (endInvoke != null)
                    {
                        throw new VerifyError("Delegate may only have a single EndInvoke method");
                    }
                    endInvoke = m;
                }
                else if (m.Name == "<init>")
                {
                    if (constructor != null)
                    {
                        throw new VerifyError("Delegate may only have a single constructor");
                    }
                    constructor = m;
                }
                else if (m.IsNative)
                {
                    throw new VerifyError("Delegate may not have any native methods besides Invoke, BeginInvoke and EndInvoke");
                }
            }
            if (invoke == null || constructor == null)
            {
                throw new VerifyError("Delegate must have a constructor and an Invoke method");
            }
            if (!invoke.IsPublic || !invoke.IsNative || invoke.IsFinal || invoke.IsStatic)
            {
                throw new VerifyError("Delegate Invoke method must be a public native non-final instance method");
            }
            if ((beginInvoke != null && endInvoke == null) || (beginInvoke == null && endInvoke != null))
            {
                throw new VerifyError("Delegate must have both BeginInvoke and EndInvoke or neither");
            }
            if (!constructor.IsPublic)
            {
                throw new VerifyError("Delegate constructor must be public");
            }
            if (constructor.Instructions.Length < 3
                || constructor.Instructions[0].NormalizedOpCode != NormalizedOpCode.Aload
                || constructor.Instructions[0].NormalizedArg1 != 0
                || constructor.Instructions[1].NormalizedOpCode != NormalizedOpCode.InvokeSpecial
                || constructor.Instructions[2].NormalizedOpCode != NormalizedOpCode.Return)
            {
                throw new VerifyError("Delegate constructor must be empty");
            }
            if (f.Fields.Length != 0)
            {
                throw new VerifyError("Delegate may not declare any fields");
            }
            var iface = classLoader.TryLoadClassByName(f.Name + RuntimeManagedJavaType.DelegateInterfaceSuffix);
            DelegateInnerClassCheck(iface != null);
            DelegateInnerClassCheck(iface.IsInterface);
            DelegateInnerClassCheck(iface.IsPublic);
            DelegateInnerClassCheck(iface.ClassLoader == classLoader);
            RuntimeJavaMethod[] methods = iface.GetMethods();
            DelegateInnerClassCheck(methods.Length == 1 && methods[0].Name == "Invoke");
            if (methods[0].Signature != invoke.Signature)
            {
                throw new VerifyError("Delegate Invoke method signature must be identical to inner interface Invoke method signature");
            }
            if (iface.Interfaces.Length != 0)
            {
                throw new VerifyError("Delegate inner interface may not extend any interfaces");
            }
            if (constructor.Signature != "(" + iface.SignatureName + ")V")
            {
                throw new VerifyError("Delegate constructor must take a single argument of type inner Method interface");
            }
            if (beginInvoke != null && beginInvoke.Signature != invoke.Signature.Substring(0, invoke.Signature.IndexOf(')')) + "Lcli.System.AsyncCallback;Ljava.lang.Object;)Lcli.System.IAsyncResult;")
            {
                throw new VerifyError("Delegate BeginInvoke method has incorrect signature");
            }
            if (endInvoke != null && endInvoke.Signature != "(Lcli.System.IAsyncResult;)" + invoke.Signature.Substring(invoke.Signature.IndexOf(')') + 1))
            {
                throw new VerifyError("Delegate EndInvoke method has incorrect signature");
            }
        }

        private static void DelegateInnerClassCheck(bool cond)
        {
            if (!cond)
            {
                throw new VerifyError("Delegate must have a public inner interface named Method with a single method named Invoke");
            }
        }

        private bool IsDelegate
        {
            get
            {
                RuntimeJavaType baseTypeWrapper = BaseTypeWrapper;
                return baseTypeWrapper != null && baseTypeWrapper.TypeAsTBD == Context.Types.MulticastDelegate;
            }
        }

        internal sealed override RuntimeJavaType BaseTypeWrapper
        {
            get { return baseTypeWrapper; }
        }

        internal override RuntimeClassLoader ClassLoader => classLoader;

        internal override Modifiers ReflectiveModifiers
        {
            get
            {
                return impl.ReflectiveModifiers;
            }
        }

        internal override RuntimeJavaType[] Interfaces
        {
            get
            {
                return interfaces;
            }
        }

        internal override RuntimeJavaType[] InnerClasses
        {
            get
            {
                return impl.InnerClasses;
            }
        }

        internal override RuntimeJavaType DeclaringTypeWrapper
        {
            get
            {
                return impl.DeclaringTypeWrapper;
            }
        }

        internal override Type TypeAsTBD
        {
            get
            {
                return impl.Type;
            }
        }

        internal override void Finish()
        {
            // we don't need locking, because Finish is Thread safe
            impl = impl.Finish();
        }

        internal void CreateStep1()
        {
            ((JavaTypeImpl)impl).CreateStep1();
        }

        internal void CreateStep2()
        {
            ((JavaTypeImpl)impl).CreateStep2();
        }

        private bool IsSerializable
        {
            get
            {
                return this.IsSubTypeOf(Context.JavaBase.TypeOfJavaIoSerializable);
            }
        }

        static bool CheckRequireOverrideStub(RuntimeJavaMethod mw1, RuntimeJavaMethod mw2)
        {
            // TODO this is too late to generate LinkageErrors so we need to figure this out earlier
            if (!TypesMatchForOverride(mw1.ReturnType, mw2.ReturnType))
                return true;

            var args1 = mw1.GetParameters();
            var args2 = mw2.GetParameters();
            for (int i = 0; i < args1.Length; i++)
                if (!TypesMatchForOverride(args1[i], args2[i]))
                    return true;

            return false;
        }

        static bool TypesMatchForOverride(RuntimeJavaType tw1, RuntimeJavaType tw2)
        {
            if (tw1 == tw2)
                return true;
            else if (tw1.IsUnloadable && tw2.IsUnloadable)
                return ((RuntimeUnloadableJavaType)tw1).CustomModifier == ((RuntimeUnloadableJavaType)tw2).CustomModifier;
            else
                return false;
        }

        void GenerateOverrideStub(TypeBuilder typeBuilder, RuntimeJavaMethod baseMethod, MethodInfo target, RuntimeJavaMethod targetMethod)
        {
            Debug.Assert(!baseMethod.HasCallerID);

            var overrideStub = baseMethod.GetDefineMethodHelper().DefineMethod(this, typeBuilder, "__<overridestub>" + baseMethod.DeclaringType.Name + "::" + baseMethod.Name, MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.Final);
            typeBuilder.DefineMethodOverride(overrideStub, (MethodInfo)baseMethod.GetMethod());

            var stubargs = baseMethod.GetParameters();
            var targetArgs = targetMethod.GetParameters();
            var ilgen = Context.CodeEmitterFactory.Create(overrideStub);
            ilgen.Emit(OpCodes.Ldarg_0);

            for (int i = 0; i < targetArgs.Length; i++)
            {
                ilgen.EmitLdarg(i + 1);
                ConvertStubArg(stubargs[i], targetArgs[i], ilgen);
            }

            if (target != null)
                ilgen.Emit(OpCodes.Callvirt, target);
            else
                targetMethod.EmitCallvirt(ilgen);

            ConvertStubArg(targetMethod.ReturnType, baseMethod.ReturnType, ilgen);
            ilgen.Emit(OpCodes.Ret);
            ilgen.DoEmit();
        }

        static void ConvertStubArg(RuntimeJavaType src, RuntimeJavaType dst, CodeEmitter ilgen)
        {
            if (src != dst)
            {
                if (dst.IsUnloadable)
                {
                    if (!src.IsUnloadable && (src.IsGhost || src.IsNonPrimitiveValueType))
                    {
                        src.EmitConvSignatureTypeToStackType(ilgen);
                    }
                }
                else if (dst.IsGhost || dst.IsNonPrimitiveValueType)
                {
                    dst.EmitConvStackTypeToSignatureType(ilgen, null);
                }
                else
                {
                    dst.EmitCheckcast(ilgen);
                }
            }
        }

        static void GetParameterNamesFromMP(Method m, string[] parameterNames)
        {
            var methodParameters = m.MethodParameters;
            if (methodParameters != null)
            {
                for (int i = 0, count = Math.Min(parameterNames.Length, methodParameters.Length); i < count; i++)
                {
                    if (parameterNames[i] == null)
                    {
                        parameterNames[i] = methodParameters[i].name;
                    }
                }
            }
        }

        protected static void GetParameterNamesFromLVT(Method m, string[] parameterNames)
        {
            var localVars = m.LocalVariableTable;
            if (localVars.Count > 0)
            {
                for (int i = m.IsStatic ? 0 : 1, pos = 0; i < m.ArgMap.Length; i++)
                {
                    // skip double & long fillers
                    if (m.ArgMap[i] != -1)
                    {
                        if (parameterNames[pos] == null)
                        {
                            for (int j = 0; j < localVars.Count; j++)
                            {
                                if (localVars[j].Slot == i)
                                {
                                    parameterNames[pos] = m.ClassFile.GetConstantPoolUtf8String(localVars[j].Name);
                                    break;
                                }
                            }
                        }

                        pos++;
                    }
                }
            }
        }

        protected static void GetParameterNamesFromSig(string sig, string[] parameterNames)
        {
            var names = new List<string>();
            for (int i = 1; sig[i] != ')'; i++)
            {
                if (sig[i] == 'L')
                {
                    i++;
                    int end = sig.IndexOf(';', i);
                    names.Add(GetParameterName(sig.Substring(i, end - i)));
                    i = end;
                }
                else if (sig[i] == '[')
                {
                    while (sig[++i] == '[') ;
                    if (sig[i] == 'L')
                    {
                        i++;
                        int end = sig.IndexOf(';', i);
                        names.Add(GetParameterName(sig.Substring(i, end - i)) + "arr");
                        i = end;
                    }
                    else
                    {
                        switch (sig[i])
                        {
                            case 'B':
                            case 'Z':
                                names.Add("barr");
                                break;
                            case 'C':
                                names.Add("charr");
                                break;
                            case 'S':
                                names.Add("sarr");
                                break;
                            case 'I':
                                names.Add("iarr");
                                break;
                            case 'J':
                                names.Add("larr");
                                break;
                            case 'F':
                                names.Add("farr");
                                break;
                            case 'D':
                                names.Add("darr");
                                break;
                        }
                    }
                }
                else
                {
                    switch (sig[i])
                    {
                        case 'B':
                        case 'Z':
                            names.Add("b");
                            break;
                        case 'C':
                            names.Add("ch");
                            break;
                        case 'S':
                            names.Add("s");
                            break;
                        case 'I':
                            names.Add("i");
                            break;
                        case 'J':
                            names.Add("l");
                            break;
                        case 'F':
                            names.Add("f");
                            break;
                        case 'D':
                            names.Add("d");
                            break;
                    }
                }
            }

            for (int i = 0; i < parameterNames.Length; i++)
                if (parameterNames[i] == null)
                    parameterNames[i] = names[i];
        }

        protected static ParameterBuilder[] GetParameterBuilders(MethodBuilder mb, int parameterCount, string[] parameterNames)
        {
            var parameterBuilders = new ParameterBuilder[parameterCount];
            Dictionary<string, int> clashes = null;
            for (int i = 0; i < parameterBuilders.Length; i++)
            {
                string name = null;
                if (parameterNames != null && parameterNames[i] != null)
                {
                    name = parameterNames[i];
                    if (Array.IndexOf(parameterNames, name, i + 1) >= 0 || (clashes != null && clashes.ContainsKey(name)))
                    {
                        clashes ??= new Dictionary<string, int>();

                        int clash = 1;
                        if (clashes.ContainsKey(name))
                            clash = clashes[name] + 1;

                        clashes[name] = clash;
                        name += clash;
                    }
                }
                parameterBuilders[i] = mb.DefineParameter(i + 1, ParameterAttributes.None, name);
            }
            return parameterBuilders;
        }

        static string GetParameterName(string type)
        {
            if (type == "java.lang.String")
            {
                return "str";
            }
            else if (type == "java.lang.Object")
            {
                return "obj";
            }
            else
            {
                var sb = new ValueStringBuilder(type.Length);
                for (int i = type.LastIndexOf('.') + 1; i < type.Length; i++)
                    if (char.IsUpper(type, i))
                        sb.Append(char.ToLower(type[i]));

                return sb.ToString();
            }
        }

#if IMPORTER

        protected abstract void AddMapXmlFields(ref RuntimeJavaField[] fields);

        protected abstract bool EmitMapXmlMethodPrologueAndOrBody(CodeEmitter ilgen, ClassFile f, Method m);

        protected abstract void EmitMapXmlMetadata(TypeBuilder typeBuilder, ClassFile classFile, RuntimeJavaField[] fields, RuntimeJavaMethod[] methods);

        protected abstract MethodBuilder DefineGhostMethod(TypeBuilder typeBuilder, string name, MethodAttributes attribs, RuntimeJavaMethod mw);

        protected abstract void FinishGhost(TypeBuilder typeBuilder, RuntimeJavaMethod[] methods);

        protected abstract void FinishGhostStep2();

        protected abstract TypeBuilder DefineGhostType(string mangledTypeName, TypeAttributes typeAttribs);

#endif // IMPORTER

        private bool IsPInvokeMethod(Method m)
        {
#if IMPORTER
            Dictionary<string, IKVM.Tools.Importer.MapXml.Class> mapxml = classLoader.GetMapXmlClasses();
            if (mapxml != null)
            {
                IKVM.Tools.Importer.MapXml.Class clazz;
                if (mapxml.TryGetValue(this.Name, out clazz) && clazz.Methods != null)
                {
                    foreach (IKVM.Tools.Importer.MapXml.Method method in clazz.Methods)
                    {
                        if (method.Name == m.Name && method.Sig == m.Signature)
                        {
                            if (method.Attributes != null)
                            {
                                foreach (IKVM.Tools.Importer.MapXml.Attribute attr in method.Attributes)
                                {
                                    if (Context.StaticCompiler.GetType(classLoader, attr.Type) == Context.Resolver.ResolveCoreType(typeof(System.Runtime.InteropServices.DllImportAttribute).FullName).AsReflection())
                                    {
                                        return true;
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }
#endif
            if (m.Annotations != null)
            {
                foreach (object[] annot in m.Annotations)
                {
                    if ("Lcli/System/Runtime/InteropServices/DllImportAttribute$Annotation;".Equals(annot[1]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        internal override MethodBase LinkMethod(RuntimeJavaMethod mw)
        {
            mw.AssertLinked();
            return impl.LinkMethod(mw);
        }

        internal override FieldInfo LinkField(RuntimeJavaField fw)
        {
            fw.AssertLinked();
            return impl.LinkField(fw);
        }

        internal override void EmitRunClassConstructor(CodeEmitter ilgen)
        {
            impl.EmitRunClassConstructor(ilgen);
        }

        internal override string GetGenericSignature()
        {
            return impl.GetGenericSignature();
        }

        internal override string GetGenericMethodSignature(RuntimeJavaMethod method)
        {
            var methods = GetMethods();
            for (int i = 0; i < methods.Length; i++)
                if (methods[i] == method)
                    return impl.GetGenericMethodSignature(i);

            Debug.Fail("Unreachable code");
            return null;
        }

        internal override string GetGenericFieldSignature(RuntimeJavaField field)
        {
            var fields = GetFields();
            for (int i = 0; i < fields.Length; i++)
                if (fields[i] == field)
                    return impl.GetGenericFieldSignature(i);

            Debug.Fail("Unreachable code");
            return null;
        }

        internal override MethodParametersEntry[] GetMethodParameters(RuntimeJavaMethod method)
        {
            var methods = GetMethods();
            for (int i = 0; i < methods.Length; i++)
                if (methods[i] == method)
                    return impl.GetMethodParameters(i);

            Debug.Fail("Unreachable code");
            return null;
        }

#if !IMPORTER

        internal override string[] GetEnclosingMethod()
        {
            return impl.GetEnclosingMethod();
        }

        /// <summary>
        /// Gets the name of the source file that declared this type.
        /// </summary>
        /// <returns></returns>
        internal override string GetSourceFileName()
        {
            return sourceFileName;
        }

        /// <summary>
        /// Gets the metadata token of the specified method.
        /// </summary>
        /// <param name="mb"></param>
        /// <returns></returns>
        int GetMethodBaseToken(MethodBase mb)
        {
            if (mb is MethodBuilder mbld)
            {
#if NETFRAMEWORK
                return mbld.GetToken().Token;
#else
                try
                {
                    return mbld.GetMetadataToken();
                }
                catch (InvalidOperationException)
                {
                    if (MetadataTokenInternalPropertyInfo != null)
                        return (int)MetadataTokenInternalPropertyInfo.GetValue(mbld);
                }
#endif
            }

#if NETFRAMEWORK
            return mb.MetadataToken;
#else
            return mb.GetMetadataToken();
#endif
        }

        /// <summary>
        /// Gets the line number within the original source of the specified method that maps to the specified offset in the IL.
        /// </summary>
        /// <param name="mb"></param>
        /// <param name="ilOffset"></param>
        /// <returns></returns>
        internal override int GetSourceLineNumber(MethodBase mb, int ilOffset)
        {
            if (lineNumberTables != null)
            {
                var token = GetMethodBaseToken(mb);
                var methods = GetMethods();
                for (int i = 0; i < methods.Length; i++)
                {
                    if (GetMethodBaseToken(methods[i].GetMethod()) == token)
                    {
                        if (lineNumberTables[i] != null)
                            return new LineNumberTableAttribute(lineNumberTables[i]).GetLineNumber(ilOffset);

                        break;
                    }
                }
            }

            return -1;
        }

        object[] DecodeAnnotations(object[] definitions)
        {
            if (definitions == null)
                return null;

            var loader = ClassLoader.GetJavaClassLoader();
            var annotations = new List<object>();

            for (int i = 0; i < definitions.Length; i++)
            {
                var obj = JVM.NewAnnotation(loader, definitions[i]);
                if (obj != null)
                    annotations.Add(obj);
            }

            return annotations.ToArray();
        }

        internal override object[] GetDeclaredAnnotations()
        {
            return DecodeAnnotations(impl.GetDeclaredAnnotations());
        }

        internal override object[] GetMethodAnnotations(RuntimeJavaMethod mw)
        {
            RuntimeJavaMethod[] methods = GetMethods();
            for (int i = 0; i < methods.Length; i++)
            {
                if (methods[i] == mw)
                {
                    return DecodeAnnotations(impl.GetMethodAnnotations(i));
                }
            }
            Debug.Fail("Unreachable code");
            return null;
        }

        internal override object[][] GetParameterAnnotations(RuntimeJavaMethod mw)
        {
            RuntimeJavaMethod[] methods = GetMethods();
            for (int i = 0; i < methods.Length; i++)
            {
                if (methods[i] == mw)
                {
                    object[][] annotations = impl.GetParameterAnnotations(i);
                    if (annotations != null)
                    {
                        object[][] objs = new object[annotations.Length][];
                        for (int j = 0; j < annotations.Length; j++)
                        {
                            objs[j] = DecodeAnnotations(annotations[j]);
                        }
                        return objs;
                    }
                    return null;
                }
            }
            Debug.Fail("Unreachable code");
            return null;
        }

        internal override object[] GetFieldAnnotations(RuntimeJavaField fw)
        {
            RuntimeJavaField[] fields = GetFields();
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i] == fw)
                {
                    return DecodeAnnotations(impl.GetFieldAnnotations(i));
                }
            }
            Debug.Fail("Unreachable code");
            return null;
        }

        internal override object GetAnnotationDefault(RuntimeJavaMethod mw)
        {
            RuntimeJavaMethod[] methods = GetMethods();
            for (int i = 0; i < methods.Length; i++)
            {
                if (methods[i] == mw)
                {
                    object defVal = impl.GetMethodDefaultValue(i);
                    if (defVal != null)
                    {
                        return JVM.NewAnnotationElementValue(mw.DeclaringType.ClassLoader.GetJavaClassLoader(), mw.ReturnType.ClassObject, defVal);
                    }
                    return null;
                }
            }
            Debug.Fail("Unreachable code");
            return null;
        }

        private Type GetBaseTypeForDefineType()
        {
            return BaseTypeWrapper.TypeAsBaseType;
        }

#endif

#if IMPORTER

        protected virtual Type GetBaseTypeForDefineType()
        {
            return BaseTypeWrapper.TypeAsBaseType;
        }

        internal virtual RuntimeJavaMethod[] GetReplacedMethodsFor(RuntimeJavaMethod mw)
        {
            return null;
        }

#endif // IMPORTER

        internal override MethodBase GetSerializationConstructor()
        {
            return automagicSerializationCtor;
        }

        private Type[] GetModOpt(RuntimeJavaType tw, bool mustBePublic)
        {
            return GetModOpt(ClassLoader.GetTypeWrapperFactory(), tw, mustBePublic);
        }

        internal static Type[] GetModOpt(RuntimeJavaTypeFactory context, RuntimeJavaType tw, bool mustBePublic)
        {
            Type[] modopt = Type.EmptyTypes;
            if (tw.IsUnloadable)
            {
                if (((RuntimeUnloadableJavaType)tw).MissingType == null)
                {
                    modopt = new Type[] { ((RuntimeUnloadableJavaType)tw).GetCustomModifier(context) };
                }
            }
            else
            {
                RuntimeJavaType tw1 = tw.IsArray ? tw.GetUltimateElementTypeWrapper() : tw;
                if (tw1.IsErasedOrBoxedPrimitiveOrRemapped || tw.IsGhostArray || (mustBePublic && !tw1.IsPublic))
                {
                    // FXBUG Ref.Emit refuses arrays in custom modifiers, so we add an array type for each dimension
                    modopt = new Type[tw.ArrayRank + 1];
                    modopt[0] = GetModOptHelper(tw1);
                    for (int i = 1; i < modopt.Length; i++)
                    {
                        modopt[i] = tw.Context.Types.Array;
                    }
                }
            }
            return modopt;
        }

        private static Type GetModOptHelper(RuntimeJavaType tw)
        {
            Debug.Assert(!tw.IsUnloadable);
            if (tw.IsArray)
            {
                return RuntimeArrayJavaType.MakeArrayType(GetModOptHelper(tw.GetUltimateElementTypeWrapper()), tw.ArrayRank);
            }
            else if (tw.IsGhost)
            {
                return tw.TypeAsTBD;
            }
            else
            {
                return tw.TypeAsBaseType;
            }
        }

#if IMPORTER
        private bool NeedsType2AccessStub(RuntimeJavaField fw)
        {
            Debug.Assert(this.IsPublic && fw.DeclaringType == this);
            return fw.IsType2FinalField
                || (fw.HasNonPublicTypeInSignature
                    && (fw.IsPublic || (fw.IsProtected && !this.IsFinal))
                    && (fw.FieldTypeWrapper.IsUnloadable || fw.FieldTypeWrapper.IsAccessibleFrom(this) || fw.FieldTypeWrapper.InternalsVisibleTo(this)));
        }
#endif

        internal static bool RequiresDynamicReflectionCallerClass(string classFile, string method, string signature)
        {
            return (classFile == "java.lang.ClassLoader" && method == "getParent" && signature == "()Ljava.lang.ClassLoader;")
                || (classFile == "java.lang.Thread" && method == "getContextClassLoader" && signature == "()Ljava.lang.ClassLoader;")
                || (classFile == "java.io.ObjectStreamField" && method == "getType" && signature == "()Ljava.lang.Class;")
                || (classFile == "javax.sql.rowset.serial.SerialJavaObject" && method == "getFields" && signature == "()[Ljava.lang.reflect.Field;")
                ;
        }


        internal override object[] GetConstantPool()
        {
            Finish();
            return impl.GetConstantPool();
        }

        internal override byte[] GetRawTypeAnnotations()
        {
            Finish();
            return impl.GetRawTypeAnnotations();
        }

        internal override byte[] GetMethodRawTypeAnnotations(RuntimeJavaMethod mw)
        {
            Finish();
            return impl.GetMethodRawTypeAnnotations(Array.IndexOf(GetMethods(), mw));
        }

        internal override byte[] GetFieldRawTypeAnnotations(RuntimeJavaField fw)
        {
            Finish();
            return impl.GetFieldRawTypeAnnotations(Array.IndexOf(GetFields(), fw));
        }

#if !IMPORTER && !EXPORTER
        internal override RuntimeJavaType Host
        {
            get { return impl.Host; }
        }
#endif

        [Conditional("IMPORTER")]
        internal void EmitLevel4Warning(HardError error, string message)
        {
#if IMPORTER
            if (ClassLoader.WarningLevelHigh)
            {
                switch (error)
                {
                    case HardError.AbstractMethodError:
                        ClassLoader.Diagnostics.EmittedAbstractMethodError(this.Name, message);
                        break;
                    case HardError.IncompatibleClassChangeError:
                        ClassLoader.Diagnostics.EmittedIncompatibleClassChangeError(this.Name, message);
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
#endif
        }
    }

}