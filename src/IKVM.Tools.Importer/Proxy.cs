/*
  Copyright (C) 2011-2014 Jeroen Frijters

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

using IKVM.Attributes;
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using IKVM.Runtime;

using Type = IKVM.Reflection.Type;

namespace IKVM.Tools.Importer
{

    static class ProxyGenerator
    {

        static readonly RuntimeJavaType proxyClass;
        static readonly RuntimeJavaType errorClass;
        static readonly RuntimeJavaType runtimeExceptionClass;
        static readonly RuntimeJavaMethod undeclaredThrowableExceptionConstructor;
        static readonly RuntimeJavaField invocationHandlerField;
        static readonly RuntimeJavaType javaLangReflectMethod;
        static readonly RuntimeJavaType javaLangNoSuchMethodException;
        static readonly RuntimeJavaMethod javaLangNoClassDefFoundErrorConstructor;
        static readonly RuntimeJavaMethod javaLangThrowable_getMessage;
        static readonly RuntimeJavaMethod javaLangClass_getMethod;
        static readonly RuntimeJavaType invocationHandlerClass;
        static readonly RuntimeJavaMethod invokeMethod;
        static readonly RuntimeJavaMethod proxyConstructor;
        static readonly RuntimeJavaMethod hashCodeMethod;
        static readonly RuntimeJavaMethod equalsMethod;
        static readonly RuntimeJavaMethod toStringMethod;

        static ProxyGenerator()
        {
            var bootClassLoader = RuntimeClassLoaderFactory.GetBootstrapClassLoader();
            proxyClass = bootClassLoader.TryLoadClassByName("java.lang.reflect.Proxy");
            errorClass = bootClassLoader.TryLoadClassByName("java.lang.Error");
            runtimeExceptionClass = bootClassLoader.TryLoadClassByName("java.lang.RuntimeException");
            undeclaredThrowableExceptionConstructor = bootClassLoader.TryLoadClassByName("java.lang.reflect.UndeclaredThrowableException").GetMethodWrapper("<init>", "(Ljava.lang.Throwable;)V", false);
            undeclaredThrowableExceptionConstructor.Link();
            invocationHandlerField = proxyClass.GetFieldWrapper("h", "Ljava.lang.reflect.InvocationHandler;");
            invocationHandlerField.Link();
            javaLangReflectMethod = bootClassLoader.TryLoadClassByName("java.lang.reflect.Method");
            javaLangNoSuchMethodException = bootClassLoader.TryLoadClassByName("java.lang.NoSuchMethodException");
            javaLangNoClassDefFoundErrorConstructor = bootClassLoader.TryLoadClassByName("java.lang.NoClassDefFoundError").GetMethodWrapper("<init>", "(Ljava.lang.String;)V", false);
            javaLangNoClassDefFoundErrorConstructor.Link();
            javaLangThrowable_getMessage = bootClassLoader.TryLoadClassByName("java.lang.Throwable").GetMethodWrapper("getMessage", "()Ljava.lang.String;", false);
            javaLangThrowable_getMessage.Link();
            javaLangClass_getMethod = CoreClasses.java.lang.Class.Wrapper.GetMethodWrapper("getMethod", "(Ljava.lang.String;[Ljava.lang.Class;)Ljava.lang.reflect.Method;", false);
            javaLangClass_getMethod.Link();
            invocationHandlerClass = bootClassLoader.TryLoadClassByName("java.lang.reflect.InvocationHandler");
            invokeMethod = invocationHandlerClass.GetMethodWrapper("invoke", "(Ljava.lang.Object;Ljava.lang.reflect.Method;[Ljava.lang.Object;)Ljava.lang.Object;", false);
            proxyConstructor = proxyClass.GetMethodWrapper("<init>", "(Ljava.lang.reflect.InvocationHandler;)V", false);
            proxyConstructor.Link();
            hashCodeMethod = CoreClasses.java.lang.Object.Wrapper.GetMethodWrapper("hashCode", "()I", false);
            equalsMethod = CoreClasses.java.lang.Object.Wrapper.GetMethodWrapper("equals", "(Ljava.lang.Object;)Z", false);
            toStringMethod = CoreClasses.java.lang.Object.Wrapper.GetMethodWrapper("toString", "()Ljava.lang.String;", false);
        }

        internal static void Create(CompilerClassLoader loader, string proxy)
        {
            var interfaces = proxy.Split(',');
            var wrappers = new RuntimeJavaType[interfaces.Length];
            for (int i = 0; i < interfaces.Length; i++)
            {
                try
                {
                    wrappers[i] = loader.TryLoadClassByName(interfaces[i]);
                }
                catch (RetargetableJavaException)
                {

                }

                if (wrappers[i] == null)
                {
                    StaticCompiler.IssueMessage(Message.UnableToCreateProxy, proxy, "unable to load interface " + interfaces[i]);
                    return;
                }
            }

            Create(loader, proxy, wrappers);
        }

        private static void Create(CompilerClassLoader loader, string proxy, RuntimeJavaType[] interfaces)
        {
            List<ProxyMethod> methods;
            try
            {
                methods = CheckAndCollect(loader, interfaces);
            }
            catch (RetargetableJavaException x)
            {
                StaticCompiler.IssueMessage(Message.UnableToCreateProxy, proxy, x.Message);
                return;
            }
            catch (ProxyException x)
            {
                StaticCompiler.IssueMessage(Message.UnableToCreateProxy, proxy, x.Message);
                return;
            }

            CreateNoFail(loader, interfaces, methods);
        }

        private static List<ProxyMethod> CheckAndCollect(CompilerClassLoader loader, RuntimeJavaType[] interfaces)
        {
            var methods = new List<RuntimeJavaMethod>();

            // The java.lang.Object methods precede any interface methods.
            methods.Add(equalsMethod);
            methods.Add(hashCodeMethod);
            methods.Add(toStringMethod);

            // Add the interfaces methods in order.
            foreach (var tw in interfaces)
            {
                if (!tw.IsInterface)
                {
                    throw new ProxyException(tw.Name + " is not an interface");
                }
                if (tw.IsRemapped)
                {
                    // TODO handle java.lang.Comparable
                    throw new ProxyException(tw.Name + " is a remapped interface (not currently supported)");
                }
                foreach (var mw in GetInterfaceMethods(tw))
                {
                    // Check for duplicates
                    if (!MethodExists(methods, mw))
                    {
                        mw.Link();
                        methods.Add(mw);
                    }
                }
            }

            // TODO verify restrictions

            // Collect declared exceptions.
            var exceptions = new Dictionary<string, RuntimeJavaType[]>();
            foreach (var mw in methods)
                Add(loader, exceptions, mw);

            // Build the definitive proxy method list.
            List<ProxyMethod> proxyMethods = new List<ProxyMethod>();
            foreach (var mw in methods)
            {
                proxyMethods.Add(new ProxyMethod(mw, exceptions[mw.Signature]));
            }
            return proxyMethods;
        }

        private static bool MethodExists(List<RuntimeJavaMethod> methods, RuntimeJavaMethod mw)
        {
            foreach (var mw1 in methods)
            {
                // TODO what do we do with differing return types?
                if (mw1.Name == mw.Name && mw1.Signature == mw.Signature)
                {
                    return true;
                }
            }
            return false;
        }

        private static void Add(CompilerClassLoader loader, Dictionary<string, RuntimeJavaType[]> exceptions, RuntimeJavaMethod mw)
        {
            string signature = mw.Signature;
            RuntimeJavaType[] newExceptionTypes = LoadTypes(loader, mw.GetDeclaredExceptions());
            RuntimeJavaType[] curExceptionTypes;
            if (exceptions.TryGetValue(signature, out curExceptionTypes))
            {
                exceptions[signature] = Merge(newExceptionTypes, curExceptionTypes);
            }
            else
            {
                exceptions.Add(signature, newExceptionTypes);
            }
        }

        static RuntimeJavaType[] Merge(RuntimeJavaType[] newExceptionTypes, RuntimeJavaType[] curExceptionTypes)
        {
            var list = new List<RuntimeJavaType>();
            foreach (var twNew in newExceptionTypes)
            {
                RuntimeJavaType match = null;
                foreach (var twCur in curExceptionTypes)
                    if (twNew.IsAssignableTo(twCur))
                        if (match == null || twCur.IsAssignableTo(match))
                            match = twCur;

                if (match != null && !list.Contains(match))
                    list.Add(match);
            }

            return list.ToArray();
        }

        static void CreateNoFail(CompilerClassLoader loader, RuntimeJavaType[] interfaces, List<ProxyMethod> methods)
        {
            var ispublic = true;
            var interfaceTypes = new Type[interfaces.Length];
            for (int i = 0; i < interfaceTypes.Length; i++)
            {
                ispublic &= interfaces[i].IsPublic;
                interfaceTypes[i] = interfaces[i].TypeAsBaseType;
            }
            var attr = TypeAttributes.Class | TypeAttributes.Sealed;
            attr |= ispublic ? TypeAttributes.NestedPublic : TypeAttributes.NestedAssembly;
            var factory = (DynamicClassLoader)loader.GetTypeWrapperFactory();
            var tb = factory.DefineProxy(TypeNameUtil.GetProxyNestedName(interfaces), attr, proxyClass.TypeAsBaseType, interfaceTypes);
            AttributeHelper.SetImplementsAttribute(tb, interfaces);
            // we apply an InnerClass attribute to avoid the CompiledTypeWrapper heuristics for figuring out the modifiers
            AttributeHelper.SetInnerClass(tb, null, ispublic ? Modifiers.Public | Modifiers.Final : Modifiers.Final);
            CreateConstructor(tb);
            for (int i = 0; i < methods.Count; i++)
                methods[i].fb = tb.DefineField("m" + i, javaLangReflectMethod.TypeAsSignatureType, FieldAttributes.Private | FieldAttributes.Static);

            foreach (var method in methods)
                CreateMethod(loader, tb, method);

            CreateStaticInitializer(tb, methods, loader);
        }

        static void CreateConstructor(TypeBuilder tb)
        {
            var ilgen = CodeEmitter.Create(ReflectUtil.DefineConstructor(tb, MethodAttributes.Public, new Type[] { invocationHandlerClass.TypeAsSignatureType }));
            ilgen.Emit(OpCodes.Ldarg_0);
            ilgen.Emit(OpCodes.Ldarg_1);
            proxyConstructor.EmitCall(ilgen);
            ilgen.Emit(OpCodes.Ret);
            ilgen.DoEmit();
        }

        private static void CreateMethod(CompilerClassLoader loader, TypeBuilder tb, ProxyMethod pm)
        {
            var mb = pm.mw.GetDefineMethodHelper().DefineMethod(loader.GetTypeWrapperFactory(), tb, pm.mw.Name, MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Final);
            var exceptions = new List<string>();
            foreach (var tw in pm.exceptions)
                exceptions.Add(tw.Name);

            AttributeHelper.SetThrowsAttribute(mb, exceptions.ToArray());
            var ilgen = CodeEmitter.Create(mb);
            ilgen.BeginExceptionBlock();
            ilgen.Emit(OpCodes.Ldarg_0);
            invocationHandlerField.EmitGet(ilgen);
            ilgen.Emit(OpCodes.Ldarg_0);
            ilgen.Emit(OpCodes.Ldsfld, pm.fb);
            var parameters = pm.mw.GetParameters();
            if (parameters.Length == 0)
            {
                ilgen.Emit(OpCodes.Ldnull);
            }
            else
            {
                ilgen.EmitLdc_I4(parameters.Length);
                ilgen.Emit(OpCodes.Newarr, Types.Object);
                for (int i = 0; i < parameters.Length; i++)
                {
                    ilgen.Emit(OpCodes.Dup);
                    ilgen.EmitLdc_I4(i);
                    ilgen.EmitLdarg(i);
                    if (parameters[i].IsNonPrimitiveValueType)
                    {
                        parameters[i].EmitBox(ilgen);
                    }
                    else if (parameters[i].IsPrimitive)
                    {
                        Boxer.EmitBox(ilgen, parameters[i]);
                    }
                    ilgen.Emit(OpCodes.Stelem_Ref);
                }
            }
            invokeMethod.EmitCallvirt(ilgen);
            var returnType = pm.mw.ReturnType;
            CodeEmitterLocal returnValue = null;
            if (returnType != RuntimePrimitiveJavaType.VOID)
            {
                returnValue = ilgen.DeclareLocal(returnType.TypeAsSignatureType);
                if (returnType.IsNonPrimitiveValueType)
                {
                    returnType.EmitUnbox(ilgen);
                }
                else if (returnType.IsPrimitive)
                {
                    Boxer.EmitUnbox(ilgen, returnType, true);
                }
                else if (returnType != CoreClasses.java.lang.Object.Wrapper)
                {
                    ilgen.EmitCastclass(returnType.TypeAsSignatureType);
                }
                ilgen.Emit(OpCodes.Stloc, returnValue);
            }
            CodeEmitterLabel returnLabel = ilgen.DefineLabel();
            ilgen.EmitLeave(returnLabel);
            // TODO consider using a filter here (but we would need to add filter support to CodeEmitter)
            ilgen.BeginCatchBlock(Types.Exception);
            ilgen.EmitLdc_I4(0);
            ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.mapException.MakeGenericMethod(Types.Exception));
            CodeEmitterLocal exception = ilgen.DeclareLocal(Types.Exception);
            ilgen.Emit(OpCodes.Stloc, exception);
            CodeEmitterLabel rethrow = ilgen.DefineLabel();
            ilgen.Emit(OpCodes.Ldloc, exception);
            errorClass.EmitInstanceOf(ilgen);
            ilgen.EmitBrtrue(rethrow);
            ilgen.Emit(OpCodes.Ldloc, exception);
            runtimeExceptionClass.EmitInstanceOf(ilgen);
            ilgen.EmitBrtrue(rethrow);
            foreach (var tw in pm.exceptions)
            {
                ilgen.Emit(OpCodes.Ldloc, exception);
                tw.EmitInstanceOf(ilgen);
                ilgen.EmitBrtrue(rethrow);
            }
            ilgen.Emit(OpCodes.Ldloc, exception);
            undeclaredThrowableExceptionConstructor.EmitNewobj(ilgen);
            ilgen.Emit(OpCodes.Throw);
            ilgen.MarkLabel(rethrow);
            ilgen.Emit(OpCodes.Rethrow);
            ilgen.EndExceptionBlock();
            ilgen.MarkLabel(returnLabel);
            if (returnValue != null)
            {
                ilgen.Emit(OpCodes.Ldloc, returnValue);
            }
            ilgen.Emit(OpCodes.Ret);
            ilgen.DoEmit();
        }

        static void CreateStaticInitializer(TypeBuilder tb, List<ProxyMethod> methods, CompilerClassLoader loader)
        {
            var ilgen = CodeEmitter.Create(ReflectUtil.DefineTypeInitializer(tb, loader));
            var callerID = ilgen.DeclareLocal(CoreClasses.ikvm.@internal.CallerID.Wrapper.TypeAsSignatureType);
            var tbCallerID = RuntimeByteCodeJavaType.FinishContext.EmitCreateCallerID(tb, ilgen);
            ilgen.Emit(OpCodes.Stloc, callerID);
            // HACK we shouldn't create the nested type here (the outer type must be created first)
            tbCallerID.CreateType();
            ilgen.BeginExceptionBlock();
            foreach (ProxyMethod method in methods)
            {
                method.mw.DeclaringType.EmitClassLiteral(ilgen);
                ilgen.Emit(OpCodes.Ldstr, method.mw.Name);
                var parameters = method.mw.GetParameters();
                ilgen.EmitLdc_I4(parameters.Length);
                ilgen.Emit(OpCodes.Newarr, CoreClasses.java.lang.Class.Wrapper.TypeAsArrayType);
                for (int i = 0; i < parameters.Length; i++)
                {
                    ilgen.Emit(OpCodes.Dup);
                    ilgen.EmitLdc_I4(i);
                    parameters[i].EmitClassLiteral(ilgen);
                    ilgen.Emit(OpCodes.Stelem_Ref);
                }
                if (javaLangClass_getMethod.HasCallerID)
                {
                    ilgen.Emit(OpCodes.Ldloc, callerID);
                }
                javaLangClass_getMethod.EmitCallvirt(ilgen);
                ilgen.Emit(OpCodes.Stsfld, method.fb);
            }
            CodeEmitterLabel label = ilgen.DefineLabel();
            ilgen.EmitLeave(label);
            ilgen.BeginCatchBlock(javaLangNoSuchMethodException.TypeAsExceptionType);
            javaLangThrowable_getMessage.EmitCallvirt(ilgen);
            javaLangNoClassDefFoundErrorConstructor.EmitNewobj(ilgen);
            ilgen.Emit(OpCodes.Throw);
            ilgen.EndExceptionBlock();
            ilgen.MarkLabel(label);
            ilgen.Emit(OpCodes.Ret);
            ilgen.DoEmit();
        }

        sealed class ProxyMethod
        {

            internal readonly RuntimeJavaMethod mw;
            internal readonly RuntimeJavaType[] exceptions;
            internal FieldBuilder fb;

            internal ProxyMethod(RuntimeJavaMethod mw, RuntimeJavaType[] exceptions)
            {
                this.mw = mw;
                this.exceptions = exceptions;
            }

        }

        static IEnumerable<RuntimeJavaMethod> GetInterfaceMethods(RuntimeJavaType tw)
        {
            var methods = new Dictionary<string, RuntimeJavaMethod>();
            foreach (var mw in tw.GetMethods())
                if (mw.IsVirtual)
                    methods.Add(mw.Name + mw.Signature, mw);

            foreach (var iface in tw.Interfaces)
                foreach (var mw in GetInterfaceMethods(iface))
                    if (!methods.ContainsKey(mw.Name + mw.Signature))
                        methods.Add(mw.Name + mw.Signature, mw);

            return methods.Values;
        }

        static RuntimeJavaType[] LoadTypes(RuntimeClassLoader loader, string[] classes)
        {
            if (classes == null || classes.Length == 0)
                return Array.Empty<RuntimeJavaType>();

            var tw = new RuntimeJavaType[classes.Length];
            for (int i = 0; i < tw.Length; i++)
                tw[i] = loader.LoadClassByName(classes[i]);

            return tw;
        }

        sealed class ProxyException : Exception
        {

            internal ProxyException(string msg) :
                base(msg)
            {

            }

        }

    }

}
