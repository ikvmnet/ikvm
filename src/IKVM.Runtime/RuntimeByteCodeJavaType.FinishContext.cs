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
using IKVM.ByteCode;
using IKVM.ByteCode.Decoding;

#if IMPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using IKVM.Tools.Importer;

using Type = IKVM.Reflection.Type;
using DynamicOrAotTypeWrapper = IKVM.Tools.Importer.RuntimeImportByteCodeJavaType;
#else
using System.Reflection;
using System.Reflection.Emit;

using DynamicOrAotTypeWrapper = IKVM.Runtime.RuntimeByteCodeJavaType;
#endif

namespace IKVM.Runtime
{

    partial class RuntimeByteCodeJavaType
    {

        internal sealed class FinishContext
        {

            readonly RuntimeContext context;
            readonly RuntimeJavaType host;
            readonly ClassFile classFile;
            readonly DynamicOrAotTypeWrapper wrapper;
            readonly TypeBuilder typeBuilder;
            List<TypeBuilder> nestedTypeBuilders;
            MethodInfo callerIDMethod;
            List<Item> items;
            Dictionary<RuntimeJavaField, MethodBuilder> arfuMap;
            Dictionary<MethodKey, MethodInfo> invokespecialstubcache;
            Dictionary<string, MethodInfo> dynamicClassLiteral;
#if IMPORTER
            TypeBuilder interfaceHelperMethodsTypeBuilder;
#else
            List<object> liveObjects;
#endif

            private struct Item
            {
                internal int key;
                internal object value;
            }

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="context"></param>
            /// <param name="host"></param>
            /// <param name="classFile"></param>
            /// <param name="wrapper"></param>
            /// <param name="typeBuilder"></param>
            internal FinishContext(RuntimeContext context, RuntimeJavaType host, ClassFile classFile, DynamicOrAotTypeWrapper wrapper, TypeBuilder typeBuilder)
            {
                this.context = context;
                this.host = host;
                this.classFile = classFile;
                this.wrapper = wrapper;
                this.typeBuilder = typeBuilder;
            }

            /// <summary>
            /// Gets the <see cref="RuntimeContext"/> under which this instance is allocated.
            /// </summary>
            public RuntimeContext Context => context;

            internal RuntimeByteCodeJavaType TypeWrapper
            {
                get { return wrapper; }
            }

            internal T GetValue<T>(int key)
                where T : class, new()
            {
                items ??= new List<Item>();
                for (int i = 0; i < items.Count; i++)
                    if (items[i].key == key && items[i].value is T value)
                        return value;

                Item item;
                item.key = key;
                T val = new T();
                item.value = val;
                items.Add(item);
                return val;
            }

            internal void EmitDynamicClassLiteral(CodeEmitter ilgen, RuntimeJavaType tw, bool dynamicCallerID)
            {
                Debug.Assert(tw.IsUnloadable);

                dynamicClassLiteral ??= new Dictionary<string, MethodInfo>();

                var cacheKey = tw.Name;
                if (dynamicCallerID)
                    cacheKey += ";dynamic";

                if (!dynamicClassLiteral.TryGetValue(cacheKey, out var method))
                {
                    var fb = typeBuilder.DefineField("__<>class", context.JavaBase.TypeOfJavaLangClass.TypeAsSignatureType, FieldAttributes.PrivateScope | FieldAttributes.Static);
                    var mb = DefineHelperMethod("__<>class", context.JavaBase.TypeOfJavaLangClass.TypeAsSignatureType, Type.EmptyTypes);
                    var ilgen2 = context.CodeEmitterFactory.Create(mb);
                    ilgen2.Emit(OpCodes.Ldsfld, fb);
                    var label = ilgen2.DefineLabel();
                    ilgen2.EmitBrtrue(label);
                    ilgen2.Emit(OpCodes.Ldstr, tw.Name);
                    EmitCallerID(ilgen2, dynamicCallerID);
                    ilgen2.Emit(OpCodes.Call, context.ByteCodeHelperMethods.DynamicClassLiteral);
                    ilgen2.Emit(OpCodes.Stsfld, fb);
                    ilgen2.MarkLabel(label);
                    ilgen2.Emit(OpCodes.Ldsfld, fb);
                    ilgen2.Emit(OpCodes.Ret);
                    ilgen2.DoEmit();
                    method = mb;
                    dynamicClassLiteral.Add(cacheKey, method);
                }

                ilgen.Emit(OpCodes.Call, method);
            }

            internal void EmitHostCallerID(CodeEmitter ilgen)
            {
#if IMPORTER || FIRST_PASS
                throw new InvalidOperationException();
#else
                EmitLiveObjectLoad(ilgen, DynamicCallerIDProvider.CreateCallerID(host));
                context.JavaBase.TypeOfIkvmInternalCallerID.EmitCheckcast(ilgen);
#endif
            }

            internal void EmitCallerID(CodeEmitter ilgen, bool dynamic)
            {
#if !FIRST_PASS && !IMPORTER
                if (dynamic)
                {
                    EmitLiveObjectLoad(ilgen, DynamicCallerIDProvider.Instance);
                    ilgen.Emit(OpCodes.Call, context.ByteCodeHelperMethods.DynamicCallerID);
                    return;
                }
#endif
                if (callerIDMethod == null)
                {
                    CreateGetCallerID();
                }

                ilgen.Emit(OpCodes.Call, callerIDMethod);
            }

            private void CreateGetCallerID()
            {
                RuntimeJavaType tw = context.JavaBase.TypeOfIkvmInternalCallerID;
                FieldBuilder callerIDField = typeBuilder.DefineField("__<callerID>", tw.TypeAsSignatureType, FieldAttributes.Private | FieldAttributes.Static | FieldAttributes.SpecialName);
                MethodBuilder mb = DefineHelperMethod("__<GetCallerID>", tw.TypeAsSignatureType, Type.EmptyTypes);
                callerIDMethod = mb;
                CodeEmitter ilgen = context.CodeEmitterFactory.Create(mb);
                ilgen.Emit(OpCodes.Ldsfld, callerIDField);
                CodeEmitterLabel done = ilgen.DefineLabel();
                ilgen.EmitBrtrue(done);
                EmitCallerIDInitialization(ilgen, callerIDField);
                ilgen.MarkLabel(done);
                ilgen.Emit(OpCodes.Ldsfld, callerIDField);
                ilgen.Emit(OpCodes.Ret);
                ilgen.DoEmit();
            }

            private void RegisterNestedTypeBuilder(TypeBuilder tb)
            {
                nestedTypeBuilders ??= new List<TypeBuilder>();
                nestedTypeBuilders.Add(tb);
            }

            internal Type FinishImpl()
            {
                var methods = wrapper.GetMethods();
                var fields = wrapper.GetFields();
#if IMPORTER
                wrapper.FinishGhost(typeBuilder, methods);
#endif

                if (!classFile.IsInterface)
                {
                    // set the base type (this needs to be done before we emit any methods, because in the static compiler
                    // GetBaseTypeForDefineType() has the side effect of inserting the __WorkaroundBaseClass__ when necessary)
                    typeBuilder.SetParent(wrapper.GetBaseTypeForDefineType());
                }

                // if we're not abstract make sure we don't inherit any abstract methods
                if (!wrapper.IsAbstract)
                {
                    var parent = wrapper.BaseTypeWrapper;

                    // if parent is not abstract, the .NET implementation will never have abstract methods (only
                    // stubs that throw AbstractMethodError)
                    // NOTE interfaces are supposed to be abstract, but the VM doesn't enforce this, so
                    // we have to check for a null parent (interfaces have no parent).
                    while (parent != null && parent.IsAbstract)
                    {
                        foreach (RuntimeJavaMethod mw in parent.GetMethods())
                        {
                            var mi = mw.GetMethod() as MethodInfo;
                            if (mi != null && mi.IsAbstract && !mi.DeclaringType.IsInterface)
                            {
                                bool needStub = false;
                                bool needRename = false;
                                if (mw.IsPublic || mw.IsProtected)
                                {
                                    var fmw = wrapper.GetMethodWrapper(mw.Name, mw.Signature, true);
                                    while (fmw != mw && (fmw.IsStatic || fmw.IsPrivate))
                                    {
                                        needRename = true;
                                        fmw = fmw.DeclaringType.BaseTypeWrapper.GetMethodWrapper(mw.Name, mw.Signature, true);
                                    }
                                    if (fmw == mw && fmw.DeclaringType != wrapper)
                                    {
                                        needStub = true;
                                    }
                                }
                                else
                                {
                                    var fmw = wrapper.GetMethodWrapper(mw.Name, mw.Signature, true);
                                    while (fmw != mw && (fmw.IsStatic || fmw.IsPrivate || !(mw.DeclaringType.IsPackageAccessibleFrom(fmw.DeclaringType) || (mw.IsInternal && mw.DeclaringType.InternalsVisibleTo(fmw.DeclaringType)))))
                                    {
                                        needRename = true;
                                        fmw = fmw.DeclaringType.BaseTypeWrapper.GetMethodWrapper(mw.Name, mw.Signature, true);
                                    }
                                    if (fmw == mw && fmw.DeclaringType != wrapper)
                                    {
                                        needStub = true;
                                    }
                                }
                                if (needStub)
                                {
                                    // NOTE in Sun's JRE 1.4.1 this method cannot be overridden by subclasses,
                                    // but I think this is a bug, so we'll support it anyway.
                                    string name = mi.Name;
                                    MethodAttributes attr = mi.Attributes & ~(MethodAttributes.Abstract | MethodAttributes.NewSlot);
                                    if (needRename)
                                    {
                                        name = "__<>" + name + "/" + mi.DeclaringType.FullName;
                                        attr = MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.NewSlot;
                                    }
                                    MethodBuilder mb = mw.GetDefineMethodHelper().DefineMethod(wrapper, typeBuilder, name, attr);
                                    if (needRename)
                                    {
                                        typeBuilder.DefineMethodOverride(mb, mi);
                                    }
                                    context.AttributeHelper.HideFromJava(mb);
                                    CodeEmitter ilgen = context.CodeEmitterFactory.Create(mb);
                                    ilgen.EmitThrow("java.lang.AbstractMethodError", mw.DeclaringType.Name + "." + mw.Name + mw.Signature);
                                    ilgen.DoEmit();
                                    wrapper.EmitLevel4Warning(HardError.AbstractMethodError, mw.DeclaringType.Name + "." + mw.Name + mw.Signature);
                                }
                            }
                        }

                        parent = parent.BaseTypeWrapper;
                    }
                }
#if IMPORTER
                TypeBuilder tbDefaultMethods = null;
#endif
                bool basehasclinit = wrapper.BaseTypeWrapper != null && wrapper.BaseTypeWrapper.HasStaticInitializer;
                int clinitIndex = -1;
                bool hasConstructor = false;
                for (int i = 0; i < classFile.Methods.Length; i++)
                {
                    var m = classFile.Methods[i];
                    var mb = (MethodBuilder)methods[i].GetMethod();
                    if (mb == null)
                    {
                        // method doesn't really exist (e.g. delegate constructor or <clinit> that is optimized away)
                        if (m.IsConstructor)
                        {
                            hasConstructor = true;
                        }
                    }
                    else if (m.IsClassInitializer)
                    {
                        // we handle the <clinit> after we've done the other methods,
                        // to make it easier to inject code needed by the other methods
                        clinitIndex = i;
                        continue;
                    }
                    else if (m.IsConstructor)
                    {
                        hasConstructor = true;
                        var ilGenerator = context.CodeEmitterFactory.Create(mb);
                        CompileConstructorBody(this, ilGenerator, i);
                    }
                    else
                    {
#if IMPORTER
                        if (methods[i].GetParameters().Length > MethodHandleUtil.MaxArity && methods[i].RequiresNonVirtualDispatcher && wrapper.GetClassLoader().EmitNoRefEmitHelpers)
                        {
                            wrapper.GetClassLoader().GetTypeWrapperFactory().DefineDelegate(methods[i].GetParameters().Length, methods[i].ReturnType == context.PrimitiveJavaTypeFactory.VOID);
                        }
#endif
                        if (m.IsAbstract)
                        {
                            var stub = false;
                            if (!classFile.IsAbstract)
                            {
                                // NOTE in the JVM it is apparently legal for a non-abstract class to have abstract methods, but
                                // the CLR doens't allow this, so we have to emit a method that throws an AbstractMethodError
                                stub = true;
                                wrapper.EmitLevel4Warning(HardError.AbstractMethodError, classFile.Name + "." + m.Name + m.Signature);
                            }
                            else if (classFile.IsPublic && !classFile.IsFinal && !(m.IsPublic || m.IsProtected))
                            {
                                // We have an abstract package accessible method in our public class. To allow a class in another
                                // assembly to subclass this class, we must fake the abstractness of this method.
                                stub = true;
                            }

                            if (stub)
                            {
                                var ilGenerator = context.CodeEmitterFactory.Create(mb);
                                new TraceHelper(context).EmitMethodTrace(ilGenerator, classFile.Name + "." + m.Name + m.Signature);
                                ilGenerator.EmitThrow("java.lang.AbstractMethodError", classFile.Name + "." + m.Name + m.Signature);
                                ilGenerator.DoEmit();
                            }
                        }
                        else if (m.IsNative)
                        {
                            if ((mb.Attributes & MethodAttributes.PinvokeImpl) != 0)
                            {
                                continue;
                            }

                            if (wrapper.IsDelegate)
                            {
                                mb.SetImplementationFlags(mb.GetMethodImplementationFlags() | MethodImplAttributes.Runtime);
                                continue;
                            }

                            Profiler.Enter("JavaTypeImpl.Finish.Native");
                            try
                            {
                                var ilGenerator = context.CodeEmitterFactory.Create(mb);
                                new TraceHelper(context).EmitMethodTrace(ilGenerator, classFile.Name + "." + m.Name + m.Signature);
#if IMPORTER
                                // do we have an implementation in map.xml?
                                if (wrapper.EmitMapXmlMethodPrologueAndOrBody(ilGenerator, classFile, m))
                                {
                                    ilGenerator.DoEmit();
                                    continue;
                                }

                                if (m.InterlockedCompareAndSetField != null && EmitInterlockedCompareAndSet(methods[i], m.InterlockedCompareAndSetField, ilGenerator))
                                {
                                    ilGenerator.DoEmit();
                                    continue;
                                }
#endif

                                MethodInfo nativeMethod = null;
                                var args = methods[i].GetParameters();
                                var nargs = args;

#if IMPORTER
                                // see if there exists a "managed JNI" class for this type
                                var nativeCodeType = context.Resolver.ResolveRuntimeType("IKVM.Java.Externs." + classFile.Name.Replace("$", "+"));
                                if (nativeCodeType != null)
                                {
                                    if (!m.IsStatic)
                                        nargs = ArrayUtil.Concat(wrapper, args);

                                    if (methods[i].HasCallerID)
                                        nargs = ArrayUtil.Concat(nargs, context.JavaBase.TypeOfIkvmInternalCallerID);

                                    foreach (var method in nativeCodeType.GetMethods(BindingFlags.Static | BindingFlags.Public))
                                    {
                                        var param = method.GetParameters();
                                        int paramLength = param.Length;

                                        while (paramLength != 0 && (param[paramLength - 1].IsIn || param[paramLength - 1].ParameterType.IsByRef))
                                            paramLength--;

                                        var match = new RuntimeJavaType[paramLength];
                                        for (int j = 0; j < paramLength; j++)
                                            match[j] = context.ClassLoaderFactory.GetJavaTypeFromType(param[j].ParameterType);

                                        if (m.Name == method.Name && IsCompatibleArgList(nargs, match))
                                        {
                                            nativeMethod = method;
                                            break;
                                        }
                                    }
                                }

#endif

                                if (nativeMethod != null)
                                {
#if IMPORTER
                                    for (int j = 0; j < nargs.Length; j++)
                                        ilGenerator.EmitLdarg(j);

                                    var param = nativeMethod.GetParameters();
                                    for (int j = nargs.Length; j < param.Length; j++)
                                    {
                                        var paramType = param[j].ParameterType;
                                        var fieldTypeWrapper = context.ClassLoaderFactory.GetJavaTypeFromType(paramType.IsByRef ? paramType.GetElementType() : paramType);
                                        var field = wrapper.GetFieldWrapper(param[j].Name, fieldTypeWrapper.SigName);
                                        if (field == null)
                                        {
                                            Console.Error.WriteLine("Error: Native method field binding not found: {0}.{1}{2}", classFile.Name, param[j].Name, fieldTypeWrapper.SigName);
                                            wrapper.Context.StaticCompiler.errorCount++;
                                            continue;
                                        }
                                        if (m.IsStatic && !field.IsStatic)
                                        {
                                            Console.Error.WriteLine("Error: Native method field binding cannot access instance field from static method: {0}.{1}{2}", classFile.Name, param[j].Name, fieldTypeWrapper.SigName);
                                            wrapper.Context.StaticCompiler.errorCount++;
                                            continue;
                                        }
                                        if (!field.IsAccessibleFrom(wrapper, wrapper, wrapper))
                                        {
                                            Console.Error.WriteLine("Error: Native method field binding not accessible: {0}.{1}{2}", classFile.Name, param[j].Name, fieldTypeWrapper.SigName);
                                            wrapper.Context.StaticCompiler.errorCount++;
                                            continue;
                                        }
                                        if (paramType.IsByRef && field.IsFinal)
                                        {
                                            Console.Error.WriteLine("Error: Native method field binding cannot use ByRef for final field: {0}.{1}{2}", classFile.Name, param[j].Name, fieldTypeWrapper.SigName);
                                            wrapper.Context.StaticCompiler.errorCount++;
                                            continue;
                                        }
                                        field.Link();
                                        if (paramType.IsByRef && field.GetField() == null)
                                        {
                                            Console.Error.WriteLine("Error: Native method field binding cannot use ByRef on field without backing field: {0}.{1}{2}", classFile.Name, param[j].Name, fieldTypeWrapper.SigName);
                                            wrapper.Context.StaticCompiler.errorCount++;
                                            continue;
                                        }
                                        if (!field.IsStatic)
                                        {
                                            ilGenerator.EmitLdarg(0);
                                        }
                                        if (paramType.IsByRef)
                                        {
                                            ilGenerator.Emit(field.IsStatic ? OpCodes.Ldsflda : OpCodes.Ldflda, field.GetField());
                                        }
                                        else
                                        {
                                            field.EmitGet(ilGenerator);
                                        }
                                    }
                                    ilGenerator.Emit(OpCodes.Call, nativeMethod);
                                    var retTypeWrapper = methods[i].ReturnType;
                                    if (!retTypeWrapper.TypeAsTBD.Equals(nativeMethod.ReturnType) && !retTypeWrapper.IsGhost)
                                        ilGenerator.Emit(OpCodes.Castclass, retTypeWrapper.TypeAsTBD);

                                    ilGenerator.Emit(OpCodes.Ret);
#endif
                                }
                                else
                                {
                                    if (wrapper.classLoader.NoJNI)
                                    {
                                        // since NoJniStubs can only be set when we're statically compiling, it is safe to use the "compiler" trace switch
                                        Tracer.Warning(Tracer.Compiler, "Native method not implemented: {0}.{1}.{2}", classFile.Name, m.Name, m.Signature);
                                        ilGenerator.EmitThrow("java.lang.UnsatisfiedLinkError", "Native method not implemented (compiled with -nojni): " + classFile.Name + "." + m.Name + m.Signature);
                                    }
                                    else
                                    {
                                        new JniBuilder(context).Generate(this, ilGenerator, wrapper, methods[i], typeBuilder, classFile, m, args, false);
                                    }
                                }

                                ilGenerator.DoEmit();
                            }
                            finally
                            {
                                Profiler.Leave("JavaTypeImpl.Finish.Native");
                            }
                        }
                        else
                        {
                            if (m.IsVirtual && classFile.IsInterface)
                            {
                                mb = (MethodBuilder)RuntimeDefaultInterfaceJavaMethod.GetImpl(methods[i]);
#if IMPORTER
                                CreateDefaultMethodInterop(ref tbDefaultMethods, mb, methods[i]);
#endif
                            }

                            var ilGenerator = context.CodeEmitterFactory.Create(mb);
                            if (!m.IsStatic && !m.IsPublic && classFile.IsInterface)
                            {
                                // Java 8 non-virtual interface method that we compile as a static method,
                                // we need to make sure the passed in this reference isn't null
                                ilGenerator.EmitLdarg(0);
                                if (wrapper.IsGhost)
                                    ilGenerator.Emit(OpCodes.Ldfld, wrapper.GhostRefField);
                                ilGenerator.EmitNullCheck();
                            }

                            new TraceHelper(context).EmitMethodTrace(ilGenerator, classFile.Name + "." + m.Name + m.Signature);
#if IMPORTER
                            // do we have an implementation in map.xml?
                            if (wrapper.EmitMapXmlMethodPrologueAndOrBody(ilGenerator, classFile, m))
                            {
                                ilGenerator.DoEmit();
                                continue;
                            }
#endif
                            var nonleaf = false;
                            Compiler.Compile(this, host, wrapper, methods[i], classFile, m, ilGenerator, ref nonleaf);
                            ilGenerator.CheckLabels();
                            ilGenerator.DoEmit();
                            if (nonleaf)
                                mb.SetImplementationFlags(mb.GetMethodImplementationFlags());
#if IMPORTER
                            ilGenerator.EmitLineNumberTable(mb);
#else
                            var linenumbers = ilGenerator.GetLineNumberTable();
                            if (linenumbers != null)
                            {
                                wrapper.lineNumberTables ??= new byte[methods.Length][];
                                wrapper.lineNumberTables[i] = linenumbers;
                            }
#endif // IMPORTER
                        }
                    }
                }

                AddInheritedDefaultInterfaceMethods(methods);

                if (clinitIndex != -1 || (basehasclinit && !classFile.IsInterface) || classFile.HasInitializedFields)
                {
                    MethodBuilder cb;
                    if (clinitIndex != -1)
                    {
                        cb = (MethodBuilder)methods[clinitIndex].GetMethod();
                    }
                    else
                    {
                        cb = ReflectUtil.DefineTypeInitializer(typeBuilder, wrapper.classLoader);
                        context.AttributeHelper.HideFromJava(cb);
                    }
                    var ilGenerator = context.CodeEmitterFactory.Create(cb);

                    // before we call the base class initializer, we need to set the non-final static ConstantValue fields
                    EmitConstantValueInitialization(fields, ilGenerator);
                    if (basehasclinit)
                        wrapper.BaseTypeWrapper.EmitRunClassConstructor(ilGenerator);

                    if (clinitIndex != -1)
                    {
                        CompileConstructorBody(this, ilGenerator, clinitIndex);
                    }
                    else
                    {
                        ilGenerator.Emit(OpCodes.Ret);
                        ilGenerator.DoEmit();
                    }

                    ilGenerator.CheckLabels();
                }

                // add all interfaces that we implement (including the magic ones) and handle ghost conversions
                ImplementInterfaces(wrapper.Interfaces, new List<RuntimeJavaType>());

                // NOTE non-final fields aren't allowed in interfaces so we don't have to initialize constant fields
                if (!classFile.IsInterface)
                {
                    // if a class has no constructor, we generate one otherwise Ref.Emit will create a default ctor
                    // and that has several problems:
                    // - base type may not have an accessible default constructor
                    // - Ref.Emit uses BaseType.GetConstructors() which may trigger a TypeResolve event
                    // - we don't want the synthesized constructor to show up in Java
                    if (!hasConstructor)
                    {
                        var ilgen = context.CodeEmitterFactory.Create(ReflectUtil.DefineConstructor(typeBuilder, MethodAttributes.PrivateScope, Type.EmptyTypes));
                        ilgen.Emit(OpCodes.Ldnull);
                        ilgen.Emit(OpCodes.Throw);
                        ilgen.DoEmit();
                    }

                    // here we loop thru all the interfaces to explicitly implement any methods that we inherit from
                    // base types that may have a different name from the name in the interface
                    // (e.g. interface that has an equals() method that should override System.Object.Equals())
                    // also deals with interface methods that aren't implemented (generate a stub that throws AbstractMethodError)
                    // and with methods that aren't public (generate a stub that throws IllegalAccessError)
                    var doneSet = new Dictionary<RuntimeJavaType, RuntimeJavaType>();
                    var interfaces = wrapper.Interfaces;
                    for (int i = 0; i < interfaces.Length; i++)
                        ImplementInterfaceMethodStubs(doneSet, interfaces[i], false);

                    // if any of our base classes has an incomplete interface implementation we need to look through all
                    // the base class interfaces to see if we've got an implementation now
                    var baseTypeWrapper = wrapper.BaseTypeWrapper;
                    while (baseTypeWrapper.HasIncompleteInterfaceImplementation)
                    {
                        for (int i = 0; i < baseTypeWrapper.Interfaces.Length; i++)
                            ImplementInterfaceMethodStubs(doneSet, baseTypeWrapper.Interfaces[i], true);

                        baseTypeWrapper = baseTypeWrapper.BaseTypeWrapper;
                    }

                    if (!wrapper.IsAbstract && wrapper.HasUnsupportedAbstractMethods)
                        AddUnsupportedAbstractMethods();

                    if (!wrapper.GetClassLoader().NoAutomagicSerialization)
                        wrapper.automagicSerializationCtor = context.Serialization.AddAutomagicSerialization(wrapper, typeBuilder);
                }

#if IMPORTER

                // If we're an interface that has public/protected fields, we create an inner class
                // to expose these fields to C# (which stubbornly refuses to see fields in interfaces).
                AddInterfaceFieldsInterop(fields);

                // If we're a Java 8 interface with static methods, we create an inner class
                // to expose these methods to C#.
                AddInterfaceMethodsInterop(methods);

                // See if there is any additional metadata
                wrapper.EmitMapXmlMetadata(typeBuilder, classFile, fields, methods);

                // if we inherit public members from non-public base classes or have public members with non-public types in their signature, we need access stubs
                if (wrapper.IsPublic)
                    AddAccessStubs();

                AddConstantPoolAttributeIfNecessary(context, classFile, typeBuilder);

#endif

                for (int i = 0; i < classFile.Methods.Length; i++)
                {
                    var m = classFile.Methods[i];
                    var mb = (MethodBuilder)methods[i].GetMethod();
                    if (mb == null)
                        continue;

                    if (m.Annotations != null)
                    {
                        ParameterBuilder returnParameter = null;
                        foreach (object[] def in m.Annotations)
                        {
                            var annotation = Annotation.Load(wrapper, def);
                            if (annotation != null)
                            {
                                annotation.Apply(wrapper.GetClassLoader(), mb, def);
                                annotation.ApplyReturnValue(wrapper.GetClassLoader(), mb, ref returnParameter, def);
                            }
                        }
                    }

                    AddMethodParameterInfo(m, methods[i], mb, out var parameterNames);

#if IMPORTER
                    if (methods[i].HasCallerID)
                    {
                        context.AttributeHelper.SetEditorBrowsableNever(mb);
                        EmitCallerIDStub(methods[i], parameterNames);
                    }
#endif
                }

                for (int i = 0; i < classFile.Fields.Length; i++)
                {
                    if (classFile.Fields[i].Annotations != null)
                    {
                        foreach (object[] def in classFile.Fields[i].Annotations)
                        {
                            var annotation = Annotation.Load(wrapper, def);
                            if (annotation != null)
                            {
                                {
                                    var prop = fields[i] as RuntimeByteCodePropertyJavaField;
                                    if (prop != null)
                                    {
                                        annotation.Apply(wrapper.GetClassLoader(), prop.GetPropertyBuilder(), def);
                                    }
                                    else
                                    {
                                        annotation.Apply(wrapper.GetClassLoader(), (FieldBuilder)fields[i].GetField(), def);
                                    }
                                }
                            }
                        }
                    }
                }

                if (classFile.Annotations != null)
                {
                    foreach (object[] def in classFile.Annotations)
                    {
                        var annotation = Annotation.Load(wrapper, def);
                        if (annotation != null)
                            annotation.Apply(wrapper.GetClassLoader(), typeBuilder, def);
                    }
                }

#if IMPORTER
                AddImplementsAttribute();
#endif

                Type type;
                Profiler.Enter("TypeBuilder.CreateType");

                try
                {
                    type = typeBuilder.CreateType();
                    if (nestedTypeBuilders != null)
                    {
                        context.ClassLoaderFactory.LoadClassCritical("ikvm.internal.IntrinsicAtomicReferenceFieldUpdater").Finish();
                        context.ClassLoaderFactory.LoadClassCritical("ikvm.internal.IntrinsicThreadLocal").Finish();
                        foreach (TypeBuilder tb in nestedTypeBuilders)
                            tb.CreateType();
                    }
#if !IMPORTER
                    if (liveObjects != null)
                        typeof(IKVM.Runtime.LiveObjectHolder<>).MakeGenericType(type).GetField("values", BindingFlags.Static | BindingFlags.Public).SetValue(null, liveObjects.ToArray());
#endif
                }
                finally
                {
                    Profiler.Leave("TypeBuilder.CreateType");
                }
#if !IMPORTER
                // When we're statically compiling we don't need to set the wrapper here, because we've already done so for the typeBuilder earlier.
                context.ClassLoaderFactory.SetWrapperForType(type, wrapper);
#endif

#if IMPORTER
                wrapper.FinishGhostStep2();
#endif

                return type;
            }

#if IMPORTER

            static void AddConstantPoolAttributeIfNecessary(RuntimeContext context, ClassFile classFile, TypeBuilder typeBuilder)
            {
                object[] constantPool = null;
                bool[] inUse = null;

                ref readonly var runtimeVisibleTypeAnnotations = ref classFile.RuntimeVisibleTypeAnnotations;
                MarkConstantPoolUsage(classFile, in runtimeVisibleTypeAnnotations, ref constantPool, ref inUse);

                foreach (var method in classFile.Methods)
                {
                    ref readonly var methodRuntimeVisibleTypeAnnotations = ref method.RuntimeVisibleTypeAnnotations;
                    MarkConstantPoolUsage(classFile, in methodRuntimeVisibleTypeAnnotations, ref constantPool, ref inUse);
                }

                foreach (var field in classFile.Fields)
                {
                    ref readonly var fieldRuntimeVisibleTypeAnnotations = ref field.RuntimeVisibleTypeAnnotations;
                    MarkConstantPoolUsage(classFile, in field.RuntimeVisibleTypeAnnotations, ref constantPool, ref inUse);
                }

                if (constantPool != null)
                {
                    // to save space, we clear out the items that aren't used by the RuntimeVisibleTypeAnnotations and
                    // use an RLE for the empty slots
                    context.AttributeHelper.SetConstantPoolAttribute(typeBuilder, ConstantPoolAttribute.Compress(constantPool, inUse));
                }
            }

            /// <summary>
            /// Scans the specified <see cref="TypeAnnotationTable"/> for used constant handles and marks them as in-use.
            /// </summary>
            /// <param name="classFile"></param>
            /// <param name="runtimeVisibleTypeAnnotations"></param>
            /// <param name="constantPool"></param>
            /// <param name="inUse"></param>
            static void MarkConstantPoolUsage(ClassFile classFile, ref readonly TypeAnnotationTable runtimeVisibleTypeAnnotations, ref object[] constantPool, ref bool[] inUse)
            {
                if (runtimeVisibleTypeAnnotations.Count > 0)
                {
                    if (constantPool == null)
                    {
                        constantPool = classFile.GetConstantPool();
                        inUse = new bool[constantPool.Length];
                    }
                    try
                    {
                        foreach (ref readonly var annotation in runtimeVisibleTypeAnnotations)
                        {
                            ref readonly var i = ref annotation;
                            MarkConstantPoolUsageForTypeAnnotation(in i, inUse);
                        }

                        return;
                    }
                    catch (ClassFormatError)
                    {

                    }
                    catch (IndexOutOfRangeException)
                    {

                    }
                    catch (ByteCodeException)
                    {

                    }

                    // if we fail to parse the annotations (e.g. due to a malformed attribute), we simply keep all the constant pool entries
                    for (int i = 0; i < inUse.Length; i++)
                        inUse[i] = true;
                }
            }

            static void MarkConstantPoolUsageForAnnotation(ref readonly IKVM.ByteCode.Decoding.Annotation annotation, bool[] inUse)
            {
                inUse[annotation.Type.Slot] = true;

                for (int i = 0; i < annotation.Elements.Count; i++)
                {
                    ref readonly var element = ref annotation.Elements[i];
                    ref readonly var elementValue = ref element.Value;
                    inUse[element.Name.Slot] = true;
                    MarkConstantPoolUsageForAnnotationComponentValue(in elementValue, inUse);
                }
            }

            static void MarkConstantPoolUsageForTypeAnnotation(ref readonly TypeAnnotation annotation, bool[] inUse)
            {
                inUse[annotation.Type.Slot] = true;

                for (int i = 0; i < annotation.Elements.Count; i++)
                {
                    ref readonly var element = ref annotation.Elements[i];
                    ref readonly var elementValue = ref element.Value;
                    inUse[element.Name.Slot] = true;
                    MarkConstantPoolUsageForAnnotationComponentValue(in elementValue, inUse);
                }
            }

            static void MarkConstantPoolUsageForAnnotationComponentValue(ref readonly ElementValue element, bool[] inUse)
            {
                switch (element.Kind)
                {
                    case ElementValueKind.Boolean:
                    case ElementValueKind.Byte:
                    case ElementValueKind.Char:
                    case ElementValueKind.Double:
                    case ElementValueKind.Float:
                    case ElementValueKind.Integer:
                    case ElementValueKind.Long:
                    case ElementValueKind.Short:
                    case ElementValueKind.String:
                        inUse[((ConstantElementValue)element).Handle.Slot] = true;
                        break;
                    case ElementValueKind.Class:
                        inUse[((ClassElementValue)element).Class.Slot] = true;
                        break;
                    case ElementValueKind.Enum:
                        var enum_ = (EnumElementValue)element;
                        inUse[enum_.TypeName.Slot] = true;
                        inUse[enum_.ConstantName.Slot] = true;
                        break;
                    case ElementValueKind.Annotation:
                        var annotation = ((AnnotationElementValue)element).Annotation;
                        MarkConstantPoolUsageForAnnotation(ref annotation, inUse);
                        break;
                    case ElementValueKind.Array:
                        foreach (ref readonly var i in (ArrayElementValue)element)
                            MarkConstantPoolUsageForAnnotationComponentValue(in i, inUse);
                        break;
                }
            }

            private bool EmitInterlockedCompareAndSet(RuntimeJavaMethod method, string fieldName, CodeEmitter ilGenerator)
            {
                if (method.ReturnType != context.PrimitiveJavaTypeFactory.BOOLEAN)
                    return false;

                var parameters = method.GetParameters();
                RuntimeJavaType target;
                int firstValueIndex;

                if (method.IsStatic)
                {
                    if (parameters.Length != 3)
                        return false;

                    target = parameters[0];
                    firstValueIndex = 1;
                }
                else
                {
                    if (parameters.Length != 2)
                        return false;

                    target = method.DeclaringType;
                    firstValueIndex = 0;
                }

                if (target.IsUnloadable || target.IsPrimitive || target.IsNonPrimitiveValueType || target.IsGhost)
                {
                    return false;
                }

                var fieldType = parameters[firstValueIndex];
                if (fieldType != parameters[firstValueIndex + 1])
                {
                    return false;
                }
                if (fieldType.IsUnloadable || fieldType.IsNonPrimitiveValueType || fieldType.IsGhost)
                {
                    return false;
                }
                if (fieldType.IsPrimitive && fieldType != context.PrimitiveJavaTypeFactory.LONG && fieldType != context.PrimitiveJavaTypeFactory.INT)
                {
                    return false;
                }

                RuntimeJavaField casField = null;
                foreach (var fw in target.GetFields())
                {
                    if (fw.Name == fieldName)
                    {
                        if (casField != null)
                            return false;

                        casField = fw;
                    }
                }

                if (casField == null)
                    return false;

                if (casField.IsStatic)
                    return false;

                casField.Link();
                if (casField.FieldTypeWrapper != fieldType)
                    return false;

                if (casField.IsPropertyAccessor)
                    return false;

                if (casField.DeclaringType.TypeAsBaseType == typeBuilder.DeclaringType)
                {
                    // allow access to fields in outer class
                }
                else if (!casField.IsAccessibleFrom(casField.DeclaringType, wrapper, casField.DeclaringType))
                {
                    return false;
                }

                FieldInfo fi = casField.GetField();
                if (fi == null)
                {
                    return false;
                }
                ilGenerator.EmitLdarg(0);
                ilGenerator.Emit(OpCodes.Ldflda, fi);
                ilGenerator.EmitLdarg(2);
                ilGenerator.EmitLdarg(1);
                if (fieldType == context.PrimitiveJavaTypeFactory.LONG)
                {
                    ilGenerator.Emit(OpCodes.Call, context.InterlockedMethods.CompareExchangeInt64);
                }
                else if (fieldType == context.PrimitiveJavaTypeFactory.INT)
                {
                    ilGenerator.Emit(OpCodes.Call, context.InterlockedMethods.CompareExchangeInt32);
                }
                else
                {
                    ilGenerator.Emit(OpCodes.Call, AtomicReferenceFieldUpdaterEmitter.MakeCompareExchange(context, casField.FieldTypeWrapper.TypeAsSignatureType));
                }
                ilGenerator.EmitLdarg(1);
                ilGenerator.Emit(OpCodes.Ceq);
                ilGenerator.Emit(OpCodes.Ret);
                return true;
            }
#endif

            private void AddMethodParameterInfo(ClassFile.Method m, RuntimeJavaMethod mw, MethodBuilder mb, out string[] parameterNames)
            {
                parameterNames = null;
                ParameterBuilder[] parameterBuilders = null;

                if (wrapper.GetClassLoader().EmitSymbols
#if IMPORTER
                    || (classFile.IsPublic && (m.IsPublic || m.IsProtected))
                    || (m.MethodParameters != null && !wrapper.GetClassLoader().NoParameterReflection)
#endif
                    )
                {
                    parameterNames = new string[mw.GetParameters().Length];
                    GetParameterNamesFromMP(m, parameterNames);
#if IMPORTER
                    if (m.MethodParameters == null)
#endif
                    {
                        GetParameterNamesFromLVT(m, parameterNames);
                        GetParameterNamesFromSig(m.Signature, parameterNames);
                    }
#if IMPORTER
                    wrapper.GetParameterNamesFromXml(m.Name, m.Signature, parameterNames);
#endif
                    parameterBuilders = GetParameterBuilders(mb, parameterNames.Length, parameterNames);
                }

#if IMPORTER
                if ((m.Modifiers & Modifiers.VarArgs) != 0 && !mw.HasCallerID)
                {
                    parameterBuilders ??= GetParameterBuilders(mb, mw.GetParameters().Length, null);
                    if (parameterBuilders.Length > 0)
                        context.AttributeHelper.SetParamArrayAttribute(parameterBuilders[parameterBuilders.Length - 1]);
                }

                wrapper.AddXmlMapParameterAttributes(mb, classFile.Name, m.Name, m.Signature, ref parameterBuilders);
#endif

                if (m.ParameterAnnotations != null)
                {
                    parameterBuilders ??= GetParameterBuilders(mb, mw.GetParameters().Length, null);

                    object[][] defs = m.ParameterAnnotations;
                    for (int j = 0; j < defs.Length; j++)
                    {
                        foreach (object[] def in defs[j])
                        {
                            var annotation = Annotation.Load(wrapper, def);
                            if (annotation != null)
                                annotation.Apply(wrapper.GetClassLoader(), parameterBuilders[j], def);
                        }
                    }
                }
            }

#if IMPORTER

            private void AddImplementsAttribute()
            {
                var interfaces = wrapper.Interfaces;
                if (wrapper.BaseTypeWrapper == context.JavaBase.TypeOfJavaLangObject)
                {
                    // We special case classes extending java.lang.Object to optimize the metadata encoding
                    // for anonymous classes that implement an interface.
                    Type[] actualInterfaces = typeBuilder.GetInterfaces();
                    if (actualInterfaces.Length == 0)
                    {
                        return;
                    }
                    else if (actualInterfaces.Length == 1
                        && interfaces.Length == 1
                        && !interfaces[0].IsRemapped
                        && interfaces[0].TypeAsBaseType == actualInterfaces[0])
                    {
                        // We extend java.lang.Object and implement only a single (non-remapped) interface,
                        // in this case we can omit the ImplementAttribute since the runtime will be able
                        // to reliable reproduce the "list" of implemented interfaces.
                        return;
                    }
                }
                else if (interfaces.Length == 0)
                {
                    return;
                }
                context.AttributeHelper.SetImplementsAttribute(typeBuilder, interfaces);
            }

            private TypeBuilder DefineNestedInteropType(string name)
            {
                CompilerClassLoader ccl = wrapper.classLoader;
                while (!ccl.ReserveName(classFile.Name + "$" + name))
                {
                    name += "_";
                }
                TypeBuilder tb = typeBuilder.DefineNestedType(name, TypeAttributes.Class | TypeAttributes.NestedPublic | TypeAttributes.Sealed | TypeAttributes.Abstract);
                RegisterNestedTypeBuilder(tb);
                context.AttributeHelper.HideFromJava(tb);
                return tb;
            }

            void AddInterfaceFieldsInterop(RuntimeJavaField[] fields)
            {

            }

            void AddInterfaceMethodsInterop(RuntimeJavaMethod[] methods)
            {

            }

            private void CreateDefaultMethodInterop(ref TypeBuilder tbDefaultMethods, MethodBuilder defaultMethod, RuntimeJavaMethod mw)
            {
                if (!ParametersAreAccessible(mw))
                {
                    return;
                }

                if (tbDefaultMethods == null)
                {
                    tbDefaultMethods = DefineNestedInteropType(NestedTypeName.DefaultMethods);
                }

                var mb = mw.GetDefineMethodHelper().DefineMethod(wrapper.GetClassLoader().GetTypeWrapperFactory(), tbDefaultMethods, mw.Name, MethodAttributes.Public | MethodAttributes.Static, wrapper.TypeAsSignatureType, true);
                var ilgen = context.CodeEmitterFactory.Create(mb);
                if (wrapper.IsGhost)
                {
                    ilgen.EmitLdarga(0);
                    ilgen.Emit(OpCodes.Ldfld, wrapper.GhostRefField);
                    ilgen.EmitNullCheck();
                    ilgen.EmitLdarga(0);
                }
                else
                {
                    ilgen.EmitLdarg(0);
                    ilgen.Emit(OpCodes.Dup);
                    ilgen.EmitNullCheck();
                }
                RuntimeJavaType[] parameters = mw.GetParameters();
                for (int i = 0; i < parameters.Length; i++)
                {
                    ilgen.EmitLdarg(i + 1);
                    if (!parameters[i].IsUnloadable && !parameters[i].IsPublic)
                    {
                        parameters[i].EmitCheckcast(ilgen);
                    }
                }
                ilgen.Emit(OpCodes.Call, defaultMethod);
                ilgen.Emit(OpCodes.Ret);
                ilgen.DoEmit();
            }
#endif

            private void AddInheritedDefaultInterfaceMethods(RuntimeJavaMethod[] methods)
            {
                // look at the miranda methods to see if we inherit any default interface methods
                for (int i = classFile.Methods.Length; i < methods.Length; i++)
                {
                    if (methods[i].IsMirandaMethod)
                    {
                        RuntimeMirandaJavaMethod mmw = (RuntimeMirandaJavaMethod)methods[i];
                        if (mmw.Error == null && !mmw.BaseMethod.IsAbstract)
                        {
                            // we inherited a default interface method, so we need to forward the miranda method to the default method
                            MethodBuilder mb = (MethodBuilder)mmw.GetMethod();
                            if (classFile.IsInterface)
                            {
                                // if we're an interface with a default miranda method, we need to create a new default method that forwards to the original
                                mb = methods[i].GetDefineMethodHelper().DefineMethod(wrapper.GetClassLoader().GetTypeWrapperFactory(),
                                    typeBuilder, NamePrefix.DefaultMethod + mb.Name, MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.SpecialName, typeBuilder, false);
                            }
                            EmitCallDefaultInterfaceMethod(mb, mmw.BaseMethod);
                        }
                    }
                }
            }

            internal void EmitCallDefaultInterfaceMethod(MethodBuilder mb, RuntimeJavaMethod defaultMethod)
            {
                CodeEmitter ilgen = context.CodeEmitterFactory.Create(mb);
                if (defaultMethod.DeclaringType.IsGhost)
                {
                    CodeEmitterLocal local = ilgen.DeclareLocal(defaultMethod.DeclaringType.TypeAsSignatureType);
                    ilgen.Emit(OpCodes.Ldloca, local);
                    ilgen.EmitLdarg(0);
                    ilgen.Emit(OpCodes.Stfld, defaultMethod.DeclaringType.GhostRefField);
                    ilgen.Emit(OpCodes.Ldloca, local);
                }
                else
                {
                    ilgen.EmitLdarg(0);
                }
                for (int j = 0, count = defaultMethod.GetParameters().Length; j < count; j++)
                {
                    ilgen.EmitLdarg(j + 1);
                }
                ilgen.Emit(OpCodes.Call, RuntimeDefaultInterfaceJavaMethod.GetImpl(defaultMethod));
                ilgen.Emit(OpCodes.Ret);
                ilgen.DoEmit();
            }

#if IMPORTER
            void AddAccessStubs()
            {
                /*
				 * There are two types of access stubs:
				 * 
				 * Type 1   These are required when a public class extends a non-public class.
				 *			In that case we need access stubs for all public and protected members
				 *			of the non-public base classes.
				 *
				 * Type 2	When a public class exposes a member that contains a non-public type in
				 *			its signature, we need an access stub for that member (where we convert
				 *			the non-public type in the signature to the first public base type).
				 *			Additionally, when a public or protected final field is compiled
				 *			without -strictfinalfieldsemantics, a the field will be wrapper with a
				 *			read-only property.
				 *
				 * Note that type 1 access stubs may also need the type 2 signature type widening
				 * if the signature contains non-public types.
				 * 
				 * Type 1 access stubs are always required, because the JVM allow access to these
				 * members via the derived class while the CLR doesn't. Historically, we've exposed
				 * these access stubs in such a way that they are also consumable from other .NET
				 * languages (when feasible), so we'll continue to do that for back compat.
				 * 
				 * Type 2 access stubs are only required by the CLR when running on CLR v4 and the
				 * caller assembly is security transparent code (level 2). We also want the access
				 * stubs to allow other .NET languages (e.g. C#) to consume broken APIs that
				 * (accidentally) expose these members.
				 */
                AddType2FieldAccessStubs();
                AddType1FieldAccessStubs(wrapper);
                if (!wrapper.IsInterface)
                {
                    int id = 0;
                    AddType2MethodAccessStubs(ref id);
                    AddType1MethodAccessStubs(ref id);
                }
            }

            void AddType1FieldAccessStubs(RuntimeJavaType tw)
            {
                do
                {
                    if (!tw.IsPublic)
                    {
                        foreach (RuntimeJavaField fw in tw.GetFields())
                        {
                            if ((fw.IsPublic || (fw.IsProtected && !wrapper.IsFinal)) && wrapper.GetFieldWrapper(fw.Name, fw.Signature) == fw)
                            {
                                GenerateAccessStub(fw, true);
                            }
                        }
                    }

                    foreach (RuntimeJavaType iface in tw.Interfaces)
                        AddType1FieldAccessStubs(iface);

                    tw = tw.BaseTypeWrapper;
                } while (tw != null && !tw.IsPublic);
            }

            void AddType2FieldAccessStubs()
            {
                foreach (var fw in wrapper.GetFields())
                    if (wrapper.NeedsType2AccessStub(fw))
                        GenerateAccessStub(fw, false);
            }

            void GenerateAccessStub(RuntimeJavaField fw, bool type1)
            {
                if (fw is RuntimeConstantJavaField cjf)
                {
                    // constants cannot have a type 2 access stub, because constant types are always public
                    Debug.Assert(type1);

                    var attribs = fw.IsPublic ? FieldAttributes.Public : FieldAttributes.FamORAssem;
                    attribs |= FieldAttributes.Static | FieldAttributes.Literal;

                    // we attach the AccessStub custom modifier because the C# compiler prefers fields without custom modifiers
                    // so if this class defines a field with the same name, that will be preferred over this one by the C# compiler
                    var fb = typeBuilder.DefineField(fw.Name, fw.FieldTypeWrapper.TypeAsSignatureType, null, new Type[] { context.Resolver.ResolveRuntimeType(typeof(IKVM.Attributes.AccessStub).FullName) }, attribs);
                    context.AttributeHelper.HideFromReflection(fb);
                    fb.SetConstant(cjf.GetConstantValue());
                }
                else
                {
                    var propType = fw.FieldTypeWrapper.TypeAsPublicSignatureType;
                    var modopt = wrapper.GetModOpt(fw.FieldTypeWrapper, true);
                    var pb = typeBuilder.DefineProperty(fw.Name, PropertyAttributes.None, propType, null, modopt, Type.EmptyTypes, null, null);
                    if (type1)
                        context.AttributeHelper.HideFromReflection(pb);
                    else
                        context.AttributeHelper.SetModifiers(pb, fw.Modifiers, fw.IsInternal);
                    var attribs = fw.IsPublic ? MethodAttributes.Public : MethodAttributes.FamORAssem;
                    attribs |= MethodAttributes.HideBySig | MethodAttributes.SpecialName;
                    if (fw.IsStatic)
                        attribs |= MethodAttributes.Static;

                    // we append the IKVM.Attributes.AccessStub type to the modopt array for use in the property accessor method signature
                    // to make sure they never conflict with any user defined methhods
                    var modopt2 = ArrayUtil.Concat(modopt, context.Resolver.ResolveRuntimeType(typeof(IKVM.Attributes.AccessStub).FullName));
                    var getter = typeBuilder.DefineMethod("get_" + fw.Name, attribs, CallingConventions.Standard, propType, null, modopt2, Type.EmptyTypes, null, null);
                    context.AttributeHelper.HideFromJava(getter);
                    pb.SetGetMethod(getter);
                    var ilgen = context.CodeEmitterFactory.Create(getter);
                    if (!fw.IsStatic)
                        ilgen.Emit(OpCodes.Ldarg_0);
                    fw.EmitGet(ilgen);
                    ilgen.Emit(OpCodes.Ret);
                    ilgen.DoEmit();

                    if (!fw.IsFinal || (!fw.IsStatic && !type1))
                    {
                        if (fw.IsFinal)
                        {
                            // we need to generate a (private) setter for final fields for reflection and serialization
                            attribs &= ~MethodAttributes.MemberAccessMask;
                            attribs |= MethodAttributes.Private;
                        }

                        var setter = typeBuilder.DefineMethod("set_" + fw.Name, attribs, CallingConventions.Standard, null, null, null, new Type[] { propType }, null, new Type[][] { modopt2 });
                        context.AttributeHelper.HideFromJava(setter);
                        pb.SetSetMethod(setter);
                        ilgen = context.CodeEmitterFactory.Create(setter);
                        ilgen.Emit(OpCodes.Ldarg_0);
                        if (!fw.IsStatic)
                            ilgen.Emit(OpCodes.Ldarg_1);

                        // we don't do a DynamicCast if fw.FieldTypeWrapper is unloadable, because for normal unloadable fields we don't enfore the type either
                        if (propType != fw.FieldTypeWrapper.TypeAsSignatureType)
                            ilgen.Emit(OpCodes.Castclass, fw.FieldTypeWrapper.TypeAsSignatureType);
                        fw.EmitSet(ilgen);
                        ilgen.Emit(OpCodes.Ret);
                        ilgen.DoEmit();
                    }
                }
            }

            void AddType1MethodAccessStubs(ref int id)
            {
                for (var tw = wrapper.BaseTypeWrapper; tw != null && !tw.IsPublic; tw = tw.BaseTypeWrapper)
                {
                    foreach (var mw in tw.GetMethods())
                    {
                        if ((mw.IsPublic || (mw.IsProtected && !wrapper.IsFinal))
                            && (!mw.IsAbstract || wrapper.IsAbstract)
                            && mw.Name != StringConstants.INIT
                            && wrapper.GetMethodWrapper(mw.Name, mw.Signature, true) == mw
                            && ParametersAreAccessible(mw))
                        {
                            GenerateAccessStub(id, mw, true, true);
                            if (!mw.IsStatic && !mw.IsFinal && !mw.IsAbstract && !wrapper.IsFinal)
                            {
                                GenerateAccessStub(id, mw, false, true);
                            }
                            id++;
                        }
                    }
                }
            }

            void AddType2MethodAccessStubs(ref int id)
            {
                foreach (var mw in wrapper.GetMethods())
                {
                    if (mw.HasNonPublicTypeInSignature && (mw.IsPublic || (mw.IsProtected && !wrapper.IsFinal)) && ParametersAreAccessible(mw))
                    {
                        GenerateAccessStub(id, mw, true, false);
                        if (!mw.IsStatic && !mw.IsFinal && !mw.IsAbstract && mw.Name != StringConstants.INIT && !wrapper.IsFinal)
                            GenerateAccessStub(id, mw, false, false);

                        id++;
                    }
                }
            }

            void GenerateAccessStub(int id, RuntimeJavaMethod mw, bool virt, bool type1)
            {
                Debug.Assert(!mw.HasCallerID);

                var stubattribs = mw.IsPublic && virt ? MethodAttributes.Public : MethodAttributes.FamORAssem;
                stubattribs |= MethodAttributes.HideBySig;
                if (mw.IsStatic)
                    stubattribs |= MethodAttributes.Static;

                var parameters = mw.GetParameters();
                var realParameterTypes = new Type[parameters.Length];
                var parameterTypes = new Type[parameters.Length];
                var modopt = new Type[parameters.Length][];
                for (int i = 0; i < parameters.Length; i++)
                {
                    realParameterTypes[i] = parameters[i].TypeAsSignatureType;
                    parameterTypes[i] = parameters[i].TypeAsPublicSignatureType;
                    modopt[i] = wrapper.GetModOpt(parameters[i], true);
                }

                var returnType = mw.ReturnType.TypeAsPublicSignatureType;
                var modoptReturnType = ArrayUtil.Concat(wrapper.GetModOpt(mw.ReturnType, true), context.Resolver.ResolveRuntimeType(typeof(IKVM.Attributes.AccessStub).FullName));

                string name;
                if (mw.Name == StringConstants.INIT)
                {
                    name = ConstructorInfo.ConstructorName;
                    stubattribs |= MethodAttributes.SpecialName | MethodAttributes.RTSpecialName;
                }
                else
                {
                    name = virt ? (mw.Modifiers & Modifiers.Bridge) == 0 ? mw.Name : NamePrefix.Bridge + mw.Name : NamePrefix.NonVirtual + id;
                }

                var mb = typeBuilder.DefineMethod(name, stubattribs, CallingConventions.Standard, returnType, null, modoptReturnType, parameterTypes, null, modopt);
                if (virt && type1)
                {
                    context.AttributeHelper.HideFromReflection(mb);
                    context.AttributeHelper.SetNameSig(mb, NamePrefix.AccessStub + id + "|" + mw.Name, mw.Signature);
                }
                else
                {
                    context.AttributeHelper.HideFromJava(mb);
                    if (!type1)
                    {
                        context.AttributeHelper.SetNameSig(mb, mw.Name, mw.Signature);
                    }
                }

                var ilgen = context.CodeEmitterFactory.Create(mb);
                int argpos = 0;
                if (!mw.IsStatic)
                    ilgen.EmitLdarg(argpos++);

                for (int i = 0; i < parameterTypes.Length; i++)
                {
                    ilgen.EmitLdarg(argpos++);

                    // we don't need to do a DynamicCast if for unloadables, because the method itself will already do that
                    if (parameterTypes[i] != realParameterTypes[i])
                        ilgen.Emit(OpCodes.Castclass, realParameterTypes[i]);
                }

                if (mw.IsStatic || !virt || mw.Name == StringConstants.INIT)
                    mw.EmitCall(ilgen);
                else
                    mw.EmitCallvirt(ilgen);

                ilgen.Emit(OpCodes.Ret);
                ilgen.DoEmit();
            }

            bool ParametersAreAccessible(RuntimeJavaMethod mw)
            {
                foreach (var tw in mw.GetParameters())
                    if (!tw.IsAccessibleFrom(wrapper))
                        return false;

                return true;
            }

#endif // IMPORTER

            private void ImplementInterfaceMethodStubs(Dictionary<RuntimeJavaType, RuntimeJavaType> doneSet, RuntimeJavaType interfaceTypeWrapper, bool baseClassInterface)
            {
                Debug.Assert(interfaceTypeWrapper.IsInterface);

                // make sure we don't do the same method twice
                if (doneSet.ContainsKey(interfaceTypeWrapper))
                {
                    return;
                }
                doneSet.Add(interfaceTypeWrapper, interfaceTypeWrapper);
                foreach (RuntimeJavaMethod method in interfaceTypeWrapper.GetMethods())
                {
                    if (!method.IsStatic && method.IsPublic && !method.IsDynamicOnly)
                    {
                        ImplementInterfaceMethodStubImpl(method, baseClassInterface);
                    }
                }
                RuntimeJavaType[] interfaces = interfaceTypeWrapper.Interfaces;
                for (int i = 0; i < interfaces.Length; i++)
                {
                    ImplementInterfaceMethodStubs(doneSet, interfaces[i], baseClassInterface);
                }
            }

            void ImplementInterfaceMethodStubImpl(RuntimeJavaMethod ifmethod, bool baseClassInterface)
            {
                // we're mangling the name to prevent subclasses from accidentally overriding this method and to
                // prevent clashes with overloaded method stubs that are erased to the same signature (e.g. unloadable types and ghost arrays)
                // HACK the signature and name are the wrong way around to work around a C++/CLI bug (apparantely it looks looks at the last n
                // characters of the method name, or something bizarre like that)
                // https://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=234167
                var mangledName = ifmethod.DeclaringType.Name + "/" + ifmethod.Signature + ifmethod.Name;
                RuntimeJavaMethod mce = null;

                var lookup = (RuntimeJavaType)wrapper;
                while (lookup != null)
                {
                    mce = lookup.GetMethodWrapper(ifmethod.Name, ifmethod.Signature, true);
                    if (mce == null || !mce.IsStatic)
                        break;

                    lookup = mce.DeclaringType.BaseTypeWrapper;
                }

                if (mce != null)
                {
                    Debug.Assert(!mce.HasCallerID);
                    if (mce.DeclaringType != wrapper)
                    {
                        // check the loader constraints
                        var error = false;
                        if (mce.ReturnType != ifmethod.ReturnType && !mce.ReturnType.IsUnloadable && !ifmethod.ReturnType.IsUnloadable)
                            error = true;

                        var mceparams = mce.GetParameters();
                        var ifparams = ifmethod.GetParameters();
                        for (int i = 0; i < mceparams.Length; i++)
                        {
                            if (mceparams[i] != ifparams[i] && !mceparams[i].IsUnloadable && !ifparams[i].IsUnloadable)
                            {
                                error = true;
                                break;
                            }
                        }

                        if (error)
                        {
                            var mb = DefineInterfaceStubMethod(mangledName, ifmethod);
                            context.AttributeHelper.HideFromJava(mb);
                            var ilgen = context.CodeEmitterFactory.Create(mb);
                            ilgen.EmitThrow("java.lang.LinkageError", wrapper.Name + "." + ifmethod.Name + ifmethod.Signature);
                            ilgen.DoEmit();
                            typeBuilder.DefineMethodOverride(mb, (MethodInfo)ifmethod.GetMethod());
                            return;
                        }
                    }

                    if (!mce.IsPublic && !mce.IsInternal)
                    {
                        // NOTE according to the ECMA spec it isn't legal for a privatescope method to be virtual, but this works and
                        // it makes sense, so I hope the spec is wrong
                        // UPDATE unfortunately, according to Serge Lidin the spec is correct, and it is not allowed to have virtual privatescope
                        // methods. Sigh! So I have to use private methods and mangle the name

                        var mb = DefineInterfaceStubMethod(NamePrefix.Incomplete + mangledName, ifmethod);
                        context.AttributeHelper.HideFromJava(mb);
                        var ilgen = context.CodeEmitterFactory.Create(mb);
                        ilgen.EmitThrow("java.lang.IllegalAccessError", wrapper.Name + "." + ifmethod.Name + ifmethod.Signature);
                        ilgen.DoEmit();
                        typeBuilder.DefineMethodOverride(mb, (MethodInfo)ifmethod.GetMethod());
                        wrapper.SetHasIncompleteInterfaceImplementation();
                    }
                    else if (mce.GetMethod() == null || mce.RealName != ifmethod.RealName || mce.IsInternal || !ReflectUtil.IsSameAssembly(mce.DeclaringType.TypeAsTBD, typeBuilder) || CheckRequireOverrideStub(mce, ifmethod))
                    {
                        // NOTE methods inherited from base classes in a different assembly do *not* automatically implement
                        // interface methods, so we have to generate a stub here that doesn't do anything but call the base
                        // implementation
                        wrapper.GenerateOverrideStub(typeBuilder, ifmethod, null, mce);
                    }
                    else if (baseClassInterface && mce.DeclaringType == wrapper)
                    {
                        typeBuilder.DefineMethodOverride((MethodInfo)mce.GetMethod(), (MethodInfo)ifmethod.GetMethod());
                    }
                }
                else
                {
                    if (!wrapper.IsAbstract || (!baseClassInterface && wrapper.GetMethodWrapper(ifmethod.Name, ifmethod.Signature, false) != null))
                    {
                        // the type doesn't implement the interface method and isn't abstract either. The JVM allows this, but the CLR doesn't,
                        // so we have to create a stub method that throws an AbstractMethodError
                        var mb = DefineInterfaceStubMethod(NamePrefix.Incomplete + mangledName, ifmethod);
                        context.AttributeHelper.HideFromJava(mb);
                        var ilgen = context.CodeEmitterFactory.Create(mb);
                        ilgen.EmitThrow("java.lang.AbstractMethodError", wrapper.Name + "." + ifmethod.Name + ifmethod.Signature);
                        ilgen.DoEmit();
                        typeBuilder.DefineMethodOverride(mb, (MethodInfo)ifmethod.GetMethod());
                        wrapper.SetHasIncompleteInterfaceImplementation();
                        wrapper.EmitLevel4Warning(HardError.AbstractMethodError, wrapper.Name + "." + ifmethod.Name + ifmethod.Signature);
                    }
                }
            }

            MethodBuilder DefineInterfaceStubMethod(string name, RuntimeJavaMethod mw)
            {
                return mw.GetDefineMethodHelper().DefineMethod(wrapper, typeBuilder, name, MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.Final);
            }

#if FIRST_PASS == false && IMPORTER == false

            internal static class JniProxyBuilder
            {

                readonly static ModuleBuilder mod;
                static int count;

                /// <summary>
                /// Initializes the static instance.
                /// </summary>
                static JniProxyBuilder()
                {
                    mod = DynamicClassLoader.CreateJniProxyModuleBuilder();
                    var cab = new CustomAttributeBuilder(JVM.Context.Resolver.ResolveRuntimeType(typeof(JavaModuleAttribute).FullName).GetConstructor(Type.EmptyTypes), new object[0]);
                    mod.SetCustomAttribute(cab);
                }

                internal static void Generate(RuntimeByteCodeJavaType.FinishContext context, CodeEmitter ilGenerator, RuntimeByteCodeJavaType wrapper, RuntimeJavaMethod mw, TypeBuilder typeBuilder, ClassFile classFile, ClassFile.Method m, RuntimeJavaType[] args)
                {
                    TypeBuilder tb = mod.DefineType("__<jni>" + System.Threading.Interlocked.Increment(ref count), TypeAttributes.Public | TypeAttributes.Class);
                    int instance = m.IsStatic ? 0 : 1;
                    Type[] argTypes = new Type[args.Length + instance + 1];
                    if (instance != 0)
                    {
                        argTypes[0] = typeof(object);
                    }
                    for (int i = 0; i < args.Length; i++)
                    {
                        // NOTE we take a shortcut here by assuming that all "special" types (i.e. ghost or value types)
                        // are public and so we can get away with replacing all other types with object.
                        argTypes[i + instance] = !args[i].IsUnloadable && (args[i].IsPrimitive || args[i].IsGhost || args[i].IsNonPrimitiveValueType) ? args[i].TypeAsSignatureType : typeof(object);
                    }
                    argTypes[argTypes.Length - 1] = context.context.JavaBase.TypeOfIkvmInternalCallerID.TypeAsSignatureType;
                    Type retType = !mw.ReturnType.IsUnloadable && (mw.ReturnType.IsPrimitive || mw.ReturnType.IsGhost || mw.ReturnType.IsNonPrimitiveValueType) ? mw.ReturnType.TypeAsSignatureType : typeof(object);
                    MethodBuilder mb = tb.DefineMethod("method", MethodAttributes.Public | MethodAttributes.Static, retType, argTypes);
                    context.context.AttributeHelper.HideFromJava(mb);
                    CodeEmitter ilgen = context.context.CodeEmitterFactory.Create(mb);
                    new JniBuilder(context.context).Generate(context, ilgen, wrapper, mw, tb, classFile, m, args, true);
                    ilgen.DoEmit();
                    tb.CreateType();
                    for (int i = 0; i < argTypes.Length - 1; i++)
                    {
                        ilGenerator.EmitLdarg(i);
                    }
                    context.EmitCallerID(ilGenerator, m.IsLambdaFormCompiled);
                    ilGenerator.Emit(OpCodes.Call, mb);
                    if (!mw.ReturnType.IsUnloadable && !mw.ReturnType.IsPrimitive && !mw.ReturnType.IsGhost && !mw.ReturnType.IsNonPrimitiveValueType)
                    {
                        ilGenerator.Emit(OpCodes.Castclass, mw.ReturnType.TypeAsSignatureType);
                    }
                    ilGenerator.Emit(OpCodes.Ret);
                }
            }
#endif

            class JniBuilder
            {

                readonly RuntimeContext context;

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                /// <param name="context"></param>
                public JniBuilder(RuntimeContext context)
                {
                    this.context = context ?? throw new ArgumentNullException(nameof(context));
                }

                Type LocalRefStructType => context.Resolver.ResolveRuntimeType("IKVM.Runtime.JNI.JNIFrame");

                MethodInfo JniFuncPtrMethod => LocalRefStructType.GetMethod("GetFuncPtr");

                MethodInfo EnterLocalRefStructMethod => LocalRefStructType.GetMethod("Enter");

                MethodInfo LeaveLocalRefStructMethod => LocalRefStructType.GetMethod("Leave");

                MethodInfo MakeLocalRefMethod => LocalRefStructType.GetMethod("MakeLocalRef");

                MethodInfo UnwrapLocalRefMethod => LocalRefStructType.GetMethod("UnwrapLocalRef");

                MethodInfo WriteLineMethod => context.Resolver.ResolveCoreType(typeof(Console).FullName).GetMethod("WriteLine", new Type[] { context.Types.Object });

                MethodInfo MonitorEnterMethod => context.Resolver.ResolveCoreType(typeof(System.Threading.Monitor).FullName).GetMethod("Enter", new Type[] { context.Types.Object });

                MethodInfo MonitorExitMethod => context.Resolver.ResolveCoreType(typeof(System.Threading.Monitor).FullName).GetMethod("Exit", new Type[] { context.Types.Object });

                internal void Generate(RuntimeByteCodeJavaType.FinishContext context, CodeEmitter ilGenerator, RuntimeByteCodeJavaType wrapper, RuntimeJavaMethod mw, TypeBuilder typeBuilder, ClassFile classFile, ClassFile.Method m, RuntimeJavaType[] args, bool thruProxy)
                {
                    CodeEmitterLocal syncObject = null;
                    if (m.IsSynchronized && m.IsStatic)
                    {
                        wrapper.EmitClassLiteral(ilGenerator);
                        ilGenerator.Emit(OpCodes.Dup);
                        syncObject = ilGenerator.DeclareLocal(context.context.Types.Object);
                        ilGenerator.Emit(OpCodes.Stloc, syncObject);
                        ilGenerator.Emit(OpCodes.Call, MonitorEnterMethod);
                        ilGenerator.BeginExceptionBlock();
                    }

                    var sig = m.Signature.Replace('.', '/');
                    // TODO use/unify JNI.METHOD_PTR_FIELD_PREFIX
                    var methodPtr = typeBuilder.DefineField("__<jniptr>" + m.Name + sig, context.context.Types.IntPtr, FieldAttributes.Static | FieldAttributes.PrivateScope);
                    var localRefStruct = ilGenerator.DeclareLocal(LocalRefStructType);
                    ilGenerator.Emit(OpCodes.Ldloca, localRefStruct);
                    ilGenerator.Emit(OpCodes.Initobj, LocalRefStructType);
                    ilGenerator.Emit(OpCodes.Ldsfld, methodPtr);
                    var oklabel = ilGenerator.DefineLabel();
                    ilGenerator.EmitBrtrue(oklabel);

                    if (thruProxy)
                        ilGenerator.EmitLdarg(args.Length + (mw.IsStatic ? 0 : 1));
                    else
                        context.EmitCallerID(ilGenerator, m.IsLambdaFormCompiled);
                    ilGenerator.Emit(OpCodes.Ldstr, classFile.Name.Replace('.', '/'));
                    ilGenerator.Emit(OpCodes.Ldstr, m.Name);
                    ilGenerator.Emit(OpCodes.Ldstr, sig);
                    ilGenerator.Emit(OpCodes.Call, JniFuncPtrMethod);
                    ilGenerator.Emit(OpCodes.Stsfld, methodPtr);
                    ilGenerator.MarkLabel(oklabel);
                    ilGenerator.Emit(OpCodes.Ldloca, localRefStruct);
                    if (thruProxy)
                        ilGenerator.EmitLdarg(args.Length + (mw.IsStatic ? 0 : 1));
                    else
                        context.EmitCallerID(ilGenerator, m.IsLambdaFormCompiled);
                    ilGenerator.Emit(OpCodes.Call, EnterLocalRefStructMethod);

                    var jnienv = ilGenerator.DeclareLocal(context.context.Types.IntPtr);
                    ilGenerator.Emit(OpCodes.Stloc, jnienv);

                    ilGenerator.BeginExceptionBlock();
                    var retTypeWrapper = mw.ReturnType;
                    if (retTypeWrapper.IsUnloadable || !retTypeWrapper.IsPrimitive)
                    {
                        // this one is for use after we return from "calli"
                        ilGenerator.Emit(OpCodes.Ldloca, localRefStruct);
                    }

                    ilGenerator.Emit(OpCodes.Ldloc, jnienv);

                    var modargs = new Type[args.Length + 2];
                    modargs[0] = context.context.Types.IntPtr;
                    modargs[1] = context.context.Types.IntPtr;
                    for (int i = 0; i < args.Length; i++)
                        modargs[i + 2] = args[i].TypeAsSignatureType;
                    int add = 0;
                    if (!m.IsStatic)
                    {
                        ilGenerator.Emit(OpCodes.Ldloca, localRefStruct);
                        ilGenerator.Emit(OpCodes.Ldarg_0);
                        ilGenerator.Emit(OpCodes.Call, MakeLocalRefMethod);
                        add = 1;
                    }
                    else
                    {
                        ilGenerator.Emit(OpCodes.Ldloca, localRefStruct);
                        wrapper.EmitClassLiteral(ilGenerator);
                        ilGenerator.Emit(OpCodes.Call, MakeLocalRefMethod);
                    }

                    for (int j = 0; j < args.Length; j++)
                    {
                        if (args[j].IsUnloadable || !args[j].IsPrimitive)
                        {
                            ilGenerator.Emit(OpCodes.Ldloca, localRefStruct);
                            if (!args[j].IsUnloadable && args[j].IsNonPrimitiveValueType)
                            {
                                ilGenerator.EmitLdarg(j + add);
                                args[j].EmitBox(ilGenerator);
                            }
                            else if (!args[j].IsUnloadable && args[j].IsGhost)
                            {
                                ilGenerator.EmitLdarga(j + add);
                                ilGenerator.Emit(OpCodes.Ldfld, args[j].GhostRefField);
                            }
                            else
                            {
                                ilGenerator.EmitLdarg(j + add);
                            }
                            ilGenerator.Emit(OpCodes.Call, MakeLocalRefMethod);
                            modargs[j + 2] = context.context.Types.IntPtr;
                        }
                        else
                        {
                            ilGenerator.EmitLdarg(j + add);
                        }
                    }

                    ilGenerator.Emit(OpCodes.Ldsfld, methodPtr);

                    Type realRetType;
                    if (retTypeWrapper == context.Context.PrimitiveJavaTypeFactory.BOOLEAN)
                        realRetType = context.Context.Types.Byte;
                    else if (retTypeWrapper.IsPrimitive)
                        realRetType = retTypeWrapper.TypeAsSignatureType;
                    else
                        realRetType = context.Context.Types.IntPtr;
                    ilGenerator.EmitCalli(OpCodes.Calli, System.Runtime.InteropServices.CallingConvention.StdCall, realRetType, modargs);

                    CodeEmitterLocal retValue = null;
                    if (retTypeWrapper != context.Context.PrimitiveJavaTypeFactory.VOID)
                    {
                        if (retTypeWrapper.IsUnloadable)
                        {
                            ilGenerator.Emit(OpCodes.Call, UnwrapLocalRefMethod);
                        }
                        else if (!retTypeWrapper.IsPrimitive)
                        {
                            ilGenerator.Emit(OpCodes.Call, UnwrapLocalRefMethod);
                            if (retTypeWrapper.IsNonPrimitiveValueType)
                            {
                                retTypeWrapper.EmitUnbox(ilGenerator);
                            }
                            else if (retTypeWrapper.IsGhost)
                            {
                                var ghost = ilGenerator.DeclareLocal(retTypeWrapper.TypeAsSignatureType);
                                var obj = ilGenerator.DeclareLocal(context.Context.Types.Object);
                                ilGenerator.Emit(OpCodes.Stloc, obj);
                                ilGenerator.Emit(OpCodes.Ldloca, ghost);
                                ilGenerator.Emit(OpCodes.Ldloc, obj);
                                ilGenerator.Emit(OpCodes.Stfld, retTypeWrapper.GhostRefField);
                                ilGenerator.Emit(OpCodes.Ldloc, ghost);
                            }
                            else
                            {
                                ilGenerator.Emit(OpCodes.Castclass, retTypeWrapper.TypeAsTBD);
                            }
                        }

                        retValue = ilGenerator.DeclareLocal(retTypeWrapper.TypeAsSignatureType);
                        ilGenerator.Emit(OpCodes.Stloc, retValue);
                    }

                    var retLabel = ilGenerator.DefineLabel();
                    ilGenerator.EmitLeave(retLabel);
                    ilGenerator.BeginCatchBlock(context.context.Types.Object);
                    ilGenerator.Emit(OpCodes.Ldstr, "*** exception in native code ***");
                    ilGenerator.Emit(OpCodes.Call, WriteLineMethod);
                    ilGenerator.Emit(OpCodes.Call, WriteLineMethod);
                    ilGenerator.Emit(OpCodes.Rethrow);
                    ilGenerator.BeginFinallyBlock();
                    ilGenerator.Emit(OpCodes.Ldloca, localRefStruct);
                    ilGenerator.Emit(OpCodes.Call, LeaveLocalRefStructMethod);
                    ilGenerator.Emit(OpCodes.Endfinally);
                    ilGenerator.EndExceptionBlock();
                    if (m.IsSynchronized && m.IsStatic)
                    {
                        ilGenerator.BeginFinallyBlock();
                        ilGenerator.Emit(OpCodes.Ldloc, syncObject);
                        ilGenerator.Emit(OpCodes.Call, MonitorExitMethod);
                        ilGenerator.Emit(OpCodes.Endfinally);
                        ilGenerator.EndExceptionBlock();
                    }
                    ilGenerator.MarkLabel(retLabel);
                    if (retTypeWrapper != context.Context.PrimitiveJavaTypeFactory.VOID)
                        ilGenerator.Emit(OpCodes.Ldloc, retValue);

                    ilGenerator.Emit(OpCodes.Ret);
                }
            }

            class TraceHelper
            {

                readonly RuntimeContext context;

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                /// <param name="context"></param>
                public TraceHelper(RuntimeContext context)
                {
                    this.context = context ?? throw new ArgumentNullException(nameof(context));
                }

#if IMPORTER
                MethodInfo MethodIsTracedMethod => context.Resolver.ResolveRuntimeType(typeof(Tracer).FullName).GetMethod("IsTracedMethod");
#endif
                MethodInfo MethodMethodInfo => context.Resolver.ResolveRuntimeType(typeof(Tracer).FullName).GetMethod("MethodInfo");

                internal void EmitMethodTrace(CodeEmitter ilgen, string tracemessage)
                {
                    if (Tracer.IsTracedMethod(tracemessage))
                    {
                        var label = ilgen.DefineLabel();
#if IMPORTER
                        // TODO this should be a boolean field test instead of a call to Tracer.IsTracedMessage
                        ilgen.Emit(OpCodes.Ldstr, tracemessage);
                        ilgen.Emit(OpCodes.Call, MethodIsTracedMethod);
                        ilgen.EmitBrfalse(label);
#endif
                        ilgen.Emit(OpCodes.Ldstr, tracemessage);
                        ilgen.Emit(OpCodes.Call, MethodMethodInfo);
                        ilgen.MarkLabel(label);
                    }
                }
            }

#if IMPORTER

            void EmitCallerIDStub(RuntimeJavaMethod mw, string[] parameterNames)
            {
                // we don't need to support custom modifiers, because there aren't any callerid methods that have parameter types that require a custom modifier
                var parameters = mw.GetParameters();
                var parameterTypes = new Type[parameters.Length];
                for (int i = 0; i < parameterTypes.Length; i++)
                    parameterTypes[i] = parameters[i].TypeAsSignatureType;

                var attribs = MethodAttributes.HideBySig;
                int argcount = parameterTypes.Length;
                if (mw.IsStatic)
                {
                    attribs |= MethodAttributes.Static;
                }
                else
                {
                    argcount++;
                }
                if (mw.IsPublic)
                {
                    attribs |= MethodAttributes.Public;
                }
                else if (mw.IsProtected)
                {
                    attribs |= MethodAttributes.FamORAssem;
                }
                else if (mw.IsPrivate)
                {
                    attribs |= MethodAttributes.Private;
                }
                else
                {
                    attribs |= MethodAttributes.Assembly;
                }

                var mb = typeBuilder.DefineMethod(mw.Name, attribs, mw.ReturnType.TypeAsSignatureType, parameterTypes);
                context.AttributeHelper.HideFromJava(mb);
                mb.SetImplementationFlags(MethodImplAttributes.NoInlining);

                var ilgen = context.CodeEmitterFactory.Create(mb);
                for (int i = 0; i < argcount; i++)
                {
                    if (parameterNames != null && (mw.IsStatic || i > 0))
                    {
                        var pb = mb.DefineParameter(mw.IsStatic ? i + 1 : i, ParameterAttributes.None, parameterNames[mw.IsStatic ? i : i - 1]);
                        if (i == argcount - 1 && (mw.Modifiers & Modifiers.VarArgs) != 0)
                            context.AttributeHelper.SetParamArrayAttribute(pb);
                    }

                    ilgen.EmitLdarg(i);
                }
                ilgen.Emit(OpCodes.Ldc_I4_1);
                ilgen.Emit(OpCodes.Ldc_I4_0);
                ilgen.Emit(OpCodes.Newobj, context.Resolver.ResolveCoreType(typeof(StackFrame).FullName).GetConstructor(new Type[] { context.Types.Int32, context.Types.Boolean }));
                var callerID = context.JavaBase.TypeOfIkvmInternalCallerID.GetMethodWrapper("create", "(Lcli.System.Diagnostics.StackFrame;)Likvm.internal.CallerID;", false);
                callerID.Link();
                callerID.EmitCall(ilgen);
                if (mw.IsStatic)
                    mw.EmitCall(ilgen);
                else
                    mw.EmitCallvirt(ilgen);
                ilgen.Emit(OpCodes.Ret);
                ilgen.DoEmit();
            }

#endif // IMPORTER

            void ImplementInterfaces(RuntimeJavaType[] interfaces, List<RuntimeJavaType> interfaceList)
            {
                foreach (var iface in interfaces)
                {
                    if (!interfaceList.Contains(iface))
                    {
                        interfaceList.Add(iface);
                        if (!iface.IsAccessibleFrom(wrapper))
                        {
                            continue;
                        }
                        // NOTE we're using TypeAsBaseType for the interfaces!
                        typeBuilder.AddInterfaceImplementation(iface.TypeAsBaseType);
#if IMPORTER
                        if (!wrapper.IsInterface)
                        {
                            // look for "magic" interfaces that imply a .NET interface
                            if (iface.GetClassLoader() == context.JavaBase.TypeOfJavaLangObject.GetClassLoader())
                            {
                                if (iface.Name == "java.lang.Iterable" && !wrapper.ImplementsInterface(context.ClassLoaderFactory.GetJavaTypeFromType(context.Resolver.ResolveCoreType(typeof(System.Collections.IEnumerable).FullName))))
                                {
                                    var enumeratorType = context.ClassLoaderFactory.GetBootstrapClassLoader().TryLoadClassByName("ikvm.lang.IterableEnumerator");
                                    if (enumeratorType != null)
                                    {
                                        typeBuilder.AddInterfaceImplementation(context.Resolver.ResolveCoreType(typeof(System.Collections.IEnumerable).FullName));
                                        // FXBUG we're using the same method name as the C# compiler here because both the .NET and Mono implementations of Xml serialization depend on this method name
                                        var mb = typeBuilder.DefineMethod("System.Collections.IEnumerable.GetEnumerator", MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.Final | MethodAttributes.SpecialName, context.Resolver.ResolveCoreType(typeof(System.Collections.IEnumerator).FullName), Type.EmptyTypes);
                                        context.AttributeHelper.HideFromJava(mb);
                                        typeBuilder.DefineMethodOverride(mb, context.Resolver.ResolveCoreType(typeof(System.Collections.IEnumerable).FullName).GetMethod("GetEnumerator"));
                                        var ilgen = context.CodeEmitterFactory.Create(mb);
                                        ilgen.Emit(OpCodes.Ldarg_0);
                                        var mw = enumeratorType.GetMethodWrapper("<init>", "(Ljava.lang.Iterable;)V", false);
                                        mw.Link();
                                        mw.EmitNewobj(ilgen);
                                        ilgen.Emit(OpCodes.Ret);
                                        ilgen.DoEmit();
                                    }
                                }
                            }
                            // if we implement a ghost interface, add an implicit conversion to the ghost reference value type
                            if (iface.IsGhost && wrapper.IsPublic)
                            {
                                var mb = typeBuilder.DefineMethod("op_Implicit", MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.SpecialName, iface.TypeAsSignatureType, new Type[] { wrapper.TypeAsSignatureType });
                                context.AttributeHelper.HideFromJava(mb);
                                var ilgen = context.CodeEmitterFactory.Create(mb);
                                var local = ilgen.DeclareLocal(iface.TypeAsSignatureType);
                                ilgen.Emit(OpCodes.Ldloca, local);
                                ilgen.Emit(OpCodes.Ldarg_0);
                                ilgen.Emit(OpCodes.Stfld, iface.GhostRefField);
                                ilgen.Emit(OpCodes.Ldloca, local);
                                ilgen.Emit(OpCodes.Ldobj, iface.TypeAsSignatureType);
                                ilgen.Emit(OpCodes.Ret);
                                ilgen.DoEmit();
                            }
                        }
#endif // IMPORTER
                        // NOTE we're recursively "implementing" all interfaces that we inherit from the interfaces we implement.
                        // The C# compiler also does this and the Compact Framework requires it.
                        ImplementInterfaces(iface.Interfaces, interfaceList);
                    }
                }
            }

            void AddUnsupportedAbstractMethods()
            {
                foreach (var mb in wrapper.BaseTypeWrapper.TypeAsBaseType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                    if (RuntimeManagedJavaType.IsUnsupportedAbstractMethod(mb))
                        GenerateUnsupportedAbstractMethodStub(mb);

                var h = new Dictionary<MethodBase, MethodBase>();
                var tw = (RuntimeJavaType)wrapper;
                while (tw != null)
                {
                    foreach (var iface in tw.Interfaces)
                    {
                        foreach (var mb in iface.TypeAsBaseType.GetMethods(BindingFlags.Public | BindingFlags.Instance))
                        {
                            if (!h.ContainsKey(mb))
                            {
                                h.Add(mb, mb);
                                if (RuntimeManagedJavaType.IsUnsupportedAbstractMethod(mb))
                                    GenerateUnsupportedAbstractMethodStub(mb);
                            }
                        }
                    }

                    tw = tw.BaseTypeWrapper;
                }
            }

            void GenerateUnsupportedAbstractMethodStub(MethodBase mb)
            {
                var parameters = mb.GetParameters();
                var parameterTypes = new Type[parameters.Length];
                for (int i = 0; i < parameters.Length; i++)
                    parameterTypes[i] = parameters[i].ParameterType;

                var attr = MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Private;
                var m = typeBuilder.DefineMethod("__<unsupported>" + mb.DeclaringType.FullName + "/" + mb.Name, attr, ((MethodInfo)mb).ReturnType, parameterTypes);
                if (mb.IsGenericMethodDefinition)
                    CopyGenericArguments(mb, m);

                var ilgen = context.CodeEmitterFactory.Create(m);
                ilgen.EmitThrow("java.lang.AbstractMethodError", "Method " + mb.DeclaringType.FullName + "." + mb.Name + " is unsupported by IKVM.");
                ilgen.DoEmit();
                typeBuilder.DefineMethodOverride(m, (MethodInfo)mb);
            }

            void CopyGenericArguments(MethodBase mi, MethodBuilder mb)
            {
                var genericParameters = mi.GetGenericArguments();
                var genParamNames = new string[genericParameters.Length];
                for (int i = 0; i < genParamNames.Length; i++)
                    genParamNames[i] = genericParameters[i].Name;

                var genParamBuilders = mb.DefineGenericParameters(genParamNames);
                for (int i = 0; i < genParamBuilders.Length; i++)
                {
                    // NOTE apparently we don't need to set the interface constraints
                    // (and if we do, it fails for some reason)
                    if (genericParameters[i].BaseType != context.Types.Object)
                        genParamBuilders[i].SetBaseTypeConstraint(genericParameters[i].BaseType);

                    genParamBuilders[i].SetGenericParameterAttributes(genericParameters[i].GenericParameterAttributes);
                }
            }

            void CompileConstructorBody(FinishContext context, CodeEmitter ilGenerator, int methodIndex)
            {
                var methods = wrapper.GetMethods();
                var m = classFile.Methods[methodIndex];
                new TraceHelper(context.context).EmitMethodTrace(ilGenerator, classFile.Name + "." + m.Name + m.Signature);
#if IMPORTER
                // do we have an implementation in map.xml?
                if (wrapper.EmitMapXmlMethodPrologueAndOrBody(ilGenerator, classFile, m))
                {
                    ilGenerator.DoEmit();
                    return;
                }
#endif
                var nonLeaf = false;
                Compiler.Compile(context, host, wrapper, methods[methodIndex], classFile, m, ilGenerator, ref nonLeaf);
                ilGenerator.DoEmit();
#if IMPORTER
                ilGenerator.EmitLineNumberTable((MethodBuilder)methods[methodIndex].GetMethod());
#else // IMPORTER
                var linenumbers = ilGenerator.GetLineNumberTable();
                if (linenumbers != null)
                {
                    if (wrapper.lineNumberTables == null)
                        wrapper.lineNumberTables = new byte[methods.Length][];

                    wrapper.lineNumberTables[methodIndex] = linenumbers;
                }
#endif // IMPORTER
            }

            static bool IsCompatibleArgList(RuntimeJavaType[] caller, RuntimeJavaType[] callee)
            {
                if (caller.Length == callee.Length)
                {
                    for (int i = 0; i < caller.Length; i++)
                    {
                        if (caller[i].IsUnloadable || callee[i].IsUnloadable)
                            return false;
                        if (!caller[i].IsAssignableTo(callee[i]))
                            return false;
                    }

                    return true;
                }

                return false;
            }

            void EmitCallerIDInitialization(CodeEmitter ilGenerator, FieldInfo callerIDField)
            {
                var tw = context.JavaBase.TypeOfIkvmInternalCallerID;
                if (tw.InternalsVisibleTo(wrapper))
                {
                    var create = tw.GetMethodWrapper("create", "(Lcli.System.RuntimeTypeHandle;)Likvm.internal.CallerID;", false);
                    ilGenerator.Emit(OpCodes.Ldtoken, this.typeBuilder);
                    create.Link();
                    create.EmitCall(ilGenerator);
                }
                else
                {
                    RegisterNestedTypeBuilder(EmitCreateCallerID(context, typeBuilder, ilGenerator));
                }

                ilGenerator.Emit(OpCodes.Stsfld, callerIDField);
            }

            internal static TypeBuilder EmitCreateCallerID(RuntimeContext context, TypeBuilder typeBuilder, CodeEmitter ilGenerator)
            {
                var tw = context.JavaBase.TypeOfIkvmInternalCallerID;
                var typeCallerID = typeBuilder.DefineNestedType(NestedTypeName.CallerID, TypeAttributes.Sealed | TypeAttributes.NestedPrivate, tw.TypeAsBaseType);
                var cb = ReflectUtil.DefineConstructor(typeCallerID, MethodAttributes.Assembly, null);
                var ctorIlgen = context.CodeEmitterFactory.Create(cb);
                ctorIlgen.Emit(OpCodes.Ldarg_0);
                var mw = tw.GetMethodWrapper("<init>", "()V", false);
                mw.Link();
                mw.EmitCall(ctorIlgen);
                ctorIlgen.Emit(OpCodes.Ret);
                ctorIlgen.DoEmit();
                ilGenerator.Emit(OpCodes.Newobj, cb);
                return typeCallerID;
            }

            void EmitConstantValueInitialization(RuntimeJavaField[] fields, CodeEmitter ilGenerator)
            {
                ClassFile.Field[] flds = classFile.Fields;
                for (int i = 0; i < flds.Length; i++)
                {
                    ClassFile.Field f = flds[i];
                    if (f.IsStatic && !f.IsFinal)
                    {
                        object constant = f.ConstantValue;
                        if (constant != null)
                        {
                            if (constant is int)
                            {
                                ilGenerator.EmitLdc_I4((int)constant);
                            }
                            else if (constant is bool)
                            {
                                ilGenerator.EmitLdc_I4((bool)constant ? 1 : 0);
                            }
                            else if (constant is byte)
                            {
                                ilGenerator.EmitLdc_I4((byte)constant);
                            }
                            else if (constant is char)
                            {
                                ilGenerator.EmitLdc_I4((char)constant);
                            }
                            else if (constant is short)
                            {
                                ilGenerator.EmitLdc_I4((short)constant);
                            }
                            else if (constant is long)
                            {
                                ilGenerator.EmitLdc_I8((long)constant);
                            }
                            else if (constant is double)
                            {
                                ilGenerator.EmitLdc_R8((double)constant);
                            }
                            else if (constant is float)
                            {
                                ilGenerator.EmitLdc_R4((float)constant);
                            }
                            else if (constant is string)
                            {
                                ilGenerator.Emit(OpCodes.Ldstr, (string)constant);
                            }
                            else
                            {
                                throw new InvalidOperationException();
                            }
                            fields[i].EmitSet(ilGenerator);
                        }
                    }
                }
            }

            internal MethodBuilder DefineThreadLocalType()
            {
                var threadLocal = context.ClassLoaderFactory.LoadClassCritical("ikvm.internal.IntrinsicThreadLocal");
                int id = nestedTypeBuilders == null ? 0 : nestedTypeBuilders.Count;
                var tb = typeBuilder.DefineNestedType(NestedTypeName.ThreadLocal + id, TypeAttributes.NestedPrivate | TypeAttributes.Sealed, threadLocal.TypeAsBaseType);
                var fb = tb.DefineField("field", context.Types.Object, FieldAttributes.Private | FieldAttributes.Static);
                fb.SetCustomAttribute(new CustomAttributeBuilder(context.Resolver.ResolveCoreType(typeof(ThreadStaticAttribute).FullName).GetConstructor(Type.EmptyTypes), new object[0]));

                var mbGet = tb.DefineMethod("get", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Final, context.Types.Object, Type.EmptyTypes);
                var ilgen = mbGet.GetILGenerator();
                ilgen.Emit(OpCodes.Ldsfld, fb);
                ilgen.Emit(OpCodes.Ret);

                var mbSet = tb.DefineMethod("set", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.Final, null, new Type[] { context.Types.Object });
                ilgen = mbSet.GetILGenerator();
                ilgen.Emit(OpCodes.Ldarg_1);
                ilgen.Emit(OpCodes.Stsfld, fb);
                ilgen.Emit(OpCodes.Ret);

                var cb = ReflectUtil.DefineConstructor(tb, MethodAttributes.Assembly, Type.EmptyTypes);
                var ctorilgen = context.CodeEmitterFactory.Create(cb);
                ctorilgen.Emit(OpCodes.Ldarg_0);
                var basector = threadLocal.GetMethodWrapper("<init>", "()V", false);
                basector.Link();
                basector.EmitCall(ctorilgen);
                ctorilgen.Emit(OpCodes.Ret);
                ctorilgen.DoEmit();

                RegisterNestedTypeBuilder(tb);
                return cb;
            }

            internal MethodBuilder GetAtomicReferenceFieldUpdater(RuntimeJavaField field)
            {
                arfuMap ??= new Dictionary<RuntimeJavaField, MethodBuilder>();

                if (!arfuMap.TryGetValue(field, out var cb))
                {
                    RuntimeJavaType arfuTypeWrapper = context.ClassLoaderFactory.LoadClassCritical("ikvm.internal.IntrinsicAtomicReferenceFieldUpdater");
                    TypeBuilder tb = typeBuilder.DefineNestedType(NestedTypeName.AtomicReferenceFieldUpdater + arfuMap.Count, TypeAttributes.NestedPrivate | TypeAttributes.Sealed, arfuTypeWrapper.TypeAsBaseType);
                    AtomicReferenceFieldUpdaterEmitter.EmitImpl(context, tb, field.GetField());
                    cb = ReflectUtil.DefineConstructor(tb, MethodAttributes.Assembly, Type.EmptyTypes);
                    arfuMap.Add(field, cb);
                    CodeEmitter ctorilgen = context.CodeEmitterFactory.Create(cb);
                    ctorilgen.Emit(OpCodes.Ldarg_0);
                    RuntimeJavaMethod basector = arfuTypeWrapper.GetMethodWrapper("<init>", "()V", false);
                    basector.Link();
                    basector.EmitCall(ctorilgen);
                    ctorilgen.Emit(OpCodes.Ret);
                    ctorilgen.DoEmit();
                    RegisterNestedTypeBuilder(tb);
                }
                return cb;
            }

            internal TypeBuilder DefineIndyCallSiteType()
            {
                int id = nestedTypeBuilders == null ? 0 : nestedTypeBuilders.Count;
                var tb = typeBuilder.DefineNestedType(NestedTypeName.IndyCallSite + id, TypeAttributes.NestedPrivate | TypeAttributes.Abstract | TypeAttributes.Sealed | TypeAttributes.BeforeFieldInit);
                RegisterNestedTypeBuilder(tb);
                return tb;
            }

            internal TypeBuilder DefineMethodHandleConstantType(MethodHandleConstantHandle handle)
            {
                var tb = typeBuilder.DefineNestedType(NestedTypeName.MethodHandleConstant + handle.Slot, TypeAttributes.NestedPrivate | TypeAttributes.Sealed | TypeAttributes.Abstract | TypeAttributes.BeforeFieldInit); ;
                RegisterNestedTypeBuilder(tb);
                return tb;
            }

            internal TypeBuilder DefineMethodTypeConstantType(MethodTypeConstantHandle handle)
            {
                var tb = typeBuilder.DefineNestedType(NestedTypeName.MethodTypeConstant + handle.Slot, TypeAttributes.NestedPrivate | TypeAttributes.Sealed | TypeAttributes.Abstract | TypeAttributes.BeforeFieldInit);
                RegisterNestedTypeBuilder(tb);
                return tb;
            }

            // this is used to define intrinsified anonymous classes (in the Unsafe.defineAnonymousClass() sense)
            internal TypeBuilder DefineAnonymousClass()
            {
                int id = nestedTypeBuilders == null ? 0 : nestedTypeBuilders.Count;
                var tb = typeBuilder.DefineNestedType(NestedTypeName.IntrinsifiedAnonymousClass + id, TypeAttributes.NestedPrivate | TypeAttributes.Sealed | TypeAttributes.SpecialName | TypeAttributes.BeforeFieldInit);
                RegisterNestedTypeBuilder(tb);
                return tb;
            }

            MethodBuilder DefineHelperMethod(string name, Type returnType, Type[] parameterTypes)
            {
                return typeBuilder.DefineMethod(name, MethodAttributes.PrivateScope | MethodAttributes.Static, returnType, parameterTypes);
            }

            internal MethodBuilder DefineMethodHandleDispatchStub(Type returnType, Type[] parameterTypes)
            {
                return DefineHelperMethod("__<>MHC", returnType, parameterTypes);
            }

            internal FieldBuilder DefineMethodHandleInvokeCacheField(Type fieldType)
            {
                return typeBuilder.DefineField("__<>invokeCache", fieldType, FieldAttributes.Static | FieldAttributes.PrivateScope);
            }

            internal FieldBuilder DefineDynamicMethodHandleCacheField()
            {
                return typeBuilder.DefineField("__<>dynamicMethodHandleCache", context.JavaBase.TypeOfJavaLangInvokeMethodHandle.TypeAsSignatureType, FieldAttributes.Static | FieldAttributes.PrivateScope);
            }

            internal FieldBuilder DefineDynamicMethodTypeCacheField()
            {
                return typeBuilder.DefineField("__<>dynamicMethodTypeCache", context.JavaBase.TypeOfJavaLangInvokeMethodType.TypeAsSignatureType, FieldAttributes.Static | FieldAttributes.PrivateScope);
            }

            internal MethodBuilder DefineDelegateInvokeErrorStub(Type returnType, Type[] parameterTypes)
            {
                return DefineHelperMethod("__<>", returnType, parameterTypes);
            }

            internal MethodInfo GetInvokeSpecialStub(RuntimeJavaMethod method)
            {
                invokespecialstubcache ??= new Dictionary<MethodKey, MethodInfo>();

                var key = new MethodKey(method.DeclaringType.Name, method.Name, method.Signature);

                if (!invokespecialstubcache.TryGetValue(key, out var mi))
                {
                    var dmh = method.GetDefineMethodHelper();
                    var stub = dmh.DefineMethod(wrapper, typeBuilder, "__<>", MethodAttributes.PrivateScope);
                    var ilgen = context.CodeEmitterFactory.Create(stub);
                    ilgen.Emit(OpCodes.Ldarg_0);
                    for (int i = 1; i <= dmh.ParameterCount; i++)
                        ilgen.EmitLdarg(i);
                    method.EmitCall(ilgen);
                    ilgen.Emit(OpCodes.Ret);
                    ilgen.DoEmit();
                    invokespecialstubcache[key] = stub;
                    mi = stub;
                }

                return mi;
            }

#if !IMPORTER

            internal void EmitLiveObjectLoad(CodeEmitter ilgen, object value)
            {
                liveObjects ??= new List<object>();
                var fi = TypeBuilder.GetField(typeof(IKVM.Runtime.LiveObjectHolder<>).MakeGenericType(typeBuilder), typeof(IKVM.Runtime.LiveObjectHolder<>).GetField("values", BindingFlags.Static | BindingFlags.Public));
                ilgen.Emit(OpCodes.Ldsfld, fi);
                ilgen.EmitLdc_I4(liveObjects.Count);
                ilgen.Emit(OpCodes.Ldelem_Ref);
                liveObjects.Add(value);
            }

#endif
        }
    }

}