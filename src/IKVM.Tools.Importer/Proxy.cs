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
using IKVM.CoreLib.Exceptions;
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using IKVM.Runtime;

using Type = IKVM.Reflection.Type;

namespace IKVM.Tools.Importer
{

    class ProxyGenerator
    {

        readonly RuntimeContext context;
        internal readonly RuntimeJavaType proxyClass;
        internal readonly RuntimeJavaType errorClass;
        internal readonly RuntimeJavaType runtimeExceptionClass;
        internal readonly RuntimeJavaMethod undeclaredThrowableExceptionConstructor;
        internal readonly RuntimeJavaField invocationHandlerField;
        internal readonly RuntimeJavaType javaLangReflectMethod;
        internal readonly RuntimeJavaType javaLangNoSuchMethodException;
        internal readonly RuntimeJavaMethod javaLangNoClassDefFoundErrorConstructor;
        internal readonly RuntimeJavaMethod javaLangThrowable_getMessage;
        internal readonly RuntimeJavaMethod javaLangClass_getMethod;
        internal readonly RuntimeJavaType invocationHandlerClass;
        internal readonly RuntimeJavaMethod invokeMethod;
        internal readonly RuntimeJavaMethod proxyConstructor;
        internal readonly RuntimeJavaMethod hashCodeMethod;
        internal readonly RuntimeJavaMethod equalsMethod;
        internal readonly RuntimeJavaMethod toStringMethod;

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        public ProxyGenerator(RuntimeContext context)
        {
            this.context = context;

            var bootClassLoader = context.ClassLoaderFactory.GetBootstrapClassLoader();
            proxyClass = bootClassLoader.TryLoadClassByName("java.lang.reflect.Proxy");
            errorClass = bootClassLoader.TryLoadClassByName("java.lang.Error");
            runtimeExceptionClass = bootClassLoader.TryLoadClassByName("java.lang.RuntimeException");
            undeclaredThrowableExceptionConstructor = bootClassLoader.TryLoadClassByName("java.lang.reflect.UndeclaredThrowableException").GetMethod("<init>", "(Ljava.lang.Throwable;)V", false);
            undeclaredThrowableExceptionConstructor.Link();
            invocationHandlerField = proxyClass.GetFieldWrapper("h", "Ljava.lang.reflect.InvocationHandler;");
            invocationHandlerField.Link();
            javaLangReflectMethod = bootClassLoader.TryLoadClassByName("java.lang.reflect.Method");
            javaLangNoSuchMethodException = bootClassLoader.TryLoadClassByName("java.lang.NoSuchMethodException");
            javaLangNoClassDefFoundErrorConstructor = bootClassLoader.TryLoadClassByName("java.lang.NoClassDefFoundError").GetMethod("<init>", "(Ljava.lang.String;)V", false);
            javaLangNoClassDefFoundErrorConstructor.Link();
            javaLangThrowable_getMessage = bootClassLoader.TryLoadClassByName("java.lang.Throwable").GetMethod("getMessage", "()Ljava.lang.String;", false);
            javaLangThrowable_getMessage.Link();
            javaLangClass_getMethod = context.JavaBase.TypeOfJavaLangClass.GetMethod("getMethod", "(Ljava.lang.String;[Ljava.lang.Class;)Ljava.lang.reflect.Method;", false);
            javaLangClass_getMethod.Link();
            invocationHandlerClass = bootClassLoader.TryLoadClassByName("java.lang.reflect.InvocationHandler");
            invokeMethod = invocationHandlerClass.GetMethod("invoke", "(Ljava.lang.Object;Ljava.lang.reflect.Method;[Ljava.lang.Object;)Ljava.lang.Object;", false);
            proxyConstructor = proxyClass.GetMethod("<init>", "(Ljava.lang.reflect.InvocationHandler;)V", false);
            proxyConstructor.Link();
            hashCodeMethod = context.JavaBase.TypeOfJavaLangObject.GetMethod("hashCode", "()I", false);
            equalsMethod = context.JavaBase.TypeOfJavaLangObject.GetMethod("equals", "(Ljava.lang.Object;)Z", false);
            toStringMethod = context.JavaBase.TypeOfJavaLangObject.GetMethod("toString", "()Ljava.lang.String;", false);
        }

        internal void Create(ImportClassLoader loader, string proxy)
        {
            var interfaces = proxy.Split(',');
            var wrappers = new RuntimeJavaType[interfaces.Length];
            for (int i = 0; i < interfaces.Length; i++)
            {
                try
                {
                    wrappers[i] = loader.TryLoadClassByName(interfaces[i]);
                }
                catch (TranslatableJavaException)
                {

                }

                if (wrappers[i] == null)
                {
                    loader.Diagnostics.UnableToCreateProxy(proxy, "unable to load interface " + interfaces[i]);
                    return;
                }
            }

            Create(loader, proxy, wrappers);
        }

        private void Create(ImportClassLoader loader, string proxy, RuntimeJavaType[] interfaces)
        {
            List<ProxyMethod> methods;
            try
            {
                methods = CheckAndCollect(loader, interfaces);
            }
            catch (TranslatableJavaException e)
            {
                loader.Diagnostics.UnableToCreateProxy(proxy, e.Message);
                return;
            }
            catch (ProxyException e)
            {
                loader.Diagnostics.UnableToCreateProxy(proxy, e.Message);
                return;
            }

            CreateNoFail(loader, interfaces, methods);
        }

        private List<ProxyMethod> CheckAndCollect(ImportClassLoader loader, RuntimeJavaType[] interfaces)
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

        private bool MethodExists(List<RuntimeJavaMethod> methods, RuntimeJavaMethod mw)
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

        private void Add(ImportClassLoader loader, Dictionary<string, RuntimeJavaType[]> exceptions, RuntimeJavaMethod mw)
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

        void CreateNoFail(ImportClassLoader loader, RuntimeJavaType[] interfaces, List<ProxyMethod> methods)
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
            loader.Context.AttributeHelper.SetImplementsAttribute(tb, interfaces);
            // we apply an InnerClass attribute to avoid the CompiledTypeWrapper heuristics for figuring out the modifiers
            loader.Context.AttributeHelper.SetInnerClass(tb, null, ispublic ? Modifiers.Public | Modifiers.Final : Modifiers.Final);
            CreateConstructor(tb);
            for (int i = 0; i < methods.Count; i++)
                methods[i].fb = tb.DefineField("m" + i, javaLangReflectMethod.TypeAsSignatureType, FieldAttributes.Private | FieldAttributes.Static);

            foreach (var method in methods)
                CreateMethod(loader, tb, method);

            CreateStaticInitializer(tb, methods, loader);
        }

        void CreateConstructor(TypeBuilder tb)
        {
            var ilgen = context.CodeEmitterFactory.Create(ReflectUtil.DefineConstructor(tb, MethodAttributes.Public, new Type[] { invocationHandlerClass.TypeAsSignatureType }));
            ilgen.Emit(OpCodes.Ldarg_0);
            ilgen.Emit(OpCodes.Ldarg_1);
            proxyConstructor.EmitCall(ilgen);
            ilgen.Emit(OpCodes.Ret);
            ilgen.DoEmit();
        }

        void CreateMethod(ImportClassLoader loader, TypeBuilder tb, ProxyMethod pm)
        {
            var mb = pm.mw.GetDefineMethodHelper().DefineMethod(loader.GetTypeWrapperFactory(), tb, pm.mw.Name, MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Final);
            var exceptions = new List<string>();
            foreach (var tw in pm.exceptions)
                exceptions.Add(tw.Name);

            loader.Context.AttributeHelper.SetThrowsAttribute(mb, exceptions.ToArray());
            var ilgen = loader.Context.CodeEmitterFactory.Create(mb);
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
                ilgen.Emit(OpCodes.Newarr, loader.Context.Types.Object);
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
                        context.Boxer.EmitBox(ilgen, parameters[i]);
                    }
                    ilgen.Emit(OpCodes.Stelem_Ref);
                }
            }
            invokeMethod.EmitCallvirt(ilgen);
            var returnType = pm.mw.ReturnType;
            CodeEmitterLocal returnValue = null;
            if (returnType != context.PrimitiveJavaTypeFactory.VOID)
            {
                returnValue = ilgen.DeclareLocal(returnType.TypeAsSignatureType);
                if (returnType.IsNonPrimitiveValueType)
                {
                    returnType.EmitUnbox(ilgen);
                }
                else if (returnType.IsPrimitive)
                {
                    context.Boxer.EmitUnbox(ilgen, returnType, true);
                }
                else if (returnType != context.JavaBase.TypeOfJavaLangObject)
                {
                    ilgen.EmitCastclass(returnType.TypeAsSignatureType);
                }
                ilgen.Emit(OpCodes.Stloc, returnValue);
            }
            CodeEmitterLabel returnLabel = ilgen.DefineLabel();
            ilgen.EmitLeave(returnLabel);
            // TODO consider using a filter here (but we would need to add filter support to CodeEmitter)
            ilgen.BeginCatchBlock(loader.Context.Types.Exception);
            ilgen.EmitLdc_I4(0);
            ilgen.Emit(OpCodes.Call, loader.Context.ByteCodeHelperMethods.MapException.MakeGenericMethod(loader.Context.Types.Exception));
            CodeEmitterLocal exception = ilgen.DeclareLocal(context.Types.Exception);
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

        void CreateStaticInitializer(TypeBuilder tb, List<ProxyMethod> methods, ImportClassLoader loader)
        {
            var ilgen = loader.Context.CodeEmitterFactory.Create(ReflectUtil.DefineTypeInitializer(tb, loader));
            var callerID = ilgen.DeclareLocal(loader.Context.JavaBase.TypeOfIkvmInternalCallerID.TypeAsSignatureType);
            var tbCallerID = RuntimeByteCodeJavaType.FinishContext.EmitCreateCallerID(loader.Context, tb, ilgen);
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
                ilgen.Emit(OpCodes.Newarr, loader.Context.JavaBase.TypeOfJavaLangClass.TypeAsArrayType);
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

        IEnumerable<RuntimeJavaMethod> GetInterfaceMethods(RuntimeJavaType tw)
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

        RuntimeJavaType[] LoadTypes(RuntimeClassLoader loader, string[] classes)
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
