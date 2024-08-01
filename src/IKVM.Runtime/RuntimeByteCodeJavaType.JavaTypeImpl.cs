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
using System.Collections.Concurrent;
using IKVM.ByteCode.Reading;
using IKVM.ByteCode;



#if IMPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using IKVM.Tools.Importer;

using Type = IKVM.Reflection.Type;
using RuntimeDynamicOrImportJavaType = IKVM.Tools.Importer.RuntimeImportByteCodeJavaType;
#else
using System.Reflection;
using System.Reflection.Emit;

using RuntimeDynamicOrImportJavaType = IKVM.Runtime.RuntimeByteCodeJavaType;
#endif

namespace IKVM.Runtime
{

    partial class RuntimeByteCodeJavaType
    {

        private sealed partial class JavaTypeImpl : DynamicImpl
        {

            readonly RuntimeJavaType host;
            readonly ClassFile classFile;
            readonly RuntimeDynamicOrImportJavaType wrapper;
            TypeBuilder typeBuilder;
            RuntimeJavaMethod[] methods;
            RuntimeJavaMethod[][] baseMethods;
            RuntimeJavaField[] fields;
            FinishedTypeImpl finishedType;
            bool finishInProgress;
            MethodBuilder clinitMethod;
            MethodBuilder finalizeMethod;
            int recursionCount;
#if IMPORTER
            RuntimeByteCodeJavaType enclosingClassWrapper;
            AnnotationBuilder annotationBuilder;
            TypeBuilder enumBuilder;
            TypeBuilder privateInterfaceMethods;
            ConcurrentDictionary<string, RuntimeJavaType> nestedTypeNames;	// only keys are used, values are always null
#endif

            internal JavaTypeImpl(RuntimeJavaType host, ClassFile f, RuntimeByteCodeJavaType wrapper)
            {
                Tracer.Info(Tracer.Compiler, "constructing JavaTypeImpl for " + f.Name);
                this.host = host;
                this.classFile = f;
                this.wrapper = (RuntimeDynamicOrImportJavaType)wrapper;
            }

            internal void CreateStep1()
            {
                // process all methods (needs to be done first, because property fields depend on being able to lookup the accessor methods)
                var hasclinit = wrapper.BaseTypeWrapper == null ? false : wrapper.BaseTypeWrapper.HasStaticInitializer;
                methods = new RuntimeJavaMethod[classFile.Methods.Length];
                baseMethods = new RuntimeJavaMethod[classFile.Methods.Length][];
                for (int i = 0; i < methods.Length; i++)
                {
                    var flags = MemberFlags.None;
                    var m = classFile.Methods[i];
                    if (m.IsClassInitializer)
                    {
#if IMPORTER
                        if (IsSideEffectFreeStaticInitializerOrNoop(m, out var noop))
                        {
                            if (noop)
                                flags |= MemberFlags.NoOp;
                        }
                        else
                        {
                            hasclinit = true;
                        }
#else
                        hasclinit = true;
#endif
                    }

                    if (m.IsInternal)
                        flags |= MemberFlags.InternalAccess;

#if IMPORTER
                    if (m.IsCallerSensitive && SupportsCallerID(m))
                        flags |= MemberFlags.CallerID;

                    // set as module initializer
                    if (m.IsModuleInitializer)
                        flags |= MemberFlags.ModuleInitializer;
#endif

                    if (wrapper.IsGhost && m.IsVirtual)
                    {
                        // note that a GhostMethodWrapper can also represent a default interface method
                        methods[i] = new RuntimeGhostJavaMethod(wrapper, m.Name, m.Signature, null, null, null, null, m.Modifiers, flags);
                    }
                    else if (m.IsConstructor && wrapper.IsDelegate)
                    {
                        methods[i] = new DelegateConstructorMethodWrapper(wrapper, m);
                    }
                    else if (classFile.IsInterface && !m.IsStatic && !m.IsPublic)
                    {
                        // we can't use callvirt to call interface private instance methods (because we have to compile them as static methods,
                        // since the CLR doesn't support interface instance methods), so need a special MethodWrapper
                        methods[i] = new RuntimePrivateInterfaceJavaMethod(wrapper, m.Name, m.Signature, null, null, null, m.Modifiers, flags);
                    }
                    else if (classFile.IsInterface && m.IsVirtual && !m.IsAbstract)
                    {
                        // note that a GhostMethodWrapper can also represent a default interface method
                        methods[i] = new RuntimeDefaultInterfaceJavaMethod(wrapper, m.Name, m.Signature, null, null, null, null, m.Modifiers, flags);
                    }
                    else
                    {
                        if (!classFile.IsInterface && m.IsVirtual)
                        {
                            baseMethods[i] = FindBaseMethods(m, out var explicitOverride);
                            if (explicitOverride)
                                flags |= MemberFlags.ExplicitOverride;
                        }

                        methods[i] = new RuntimeTypicalJavaMethod(wrapper, m.Name, m.Signature, null, null, null, m.Modifiers, flags);
                    }
                }

                if (hasclinit)
                    wrapper.SetHasStaticInitializer();

                if (!wrapper.IsInterface || wrapper.IsPublic)
                {
                    var methodsArray = new List<RuntimeJavaMethod>(methods);
                    var baseMethodsArray = new List<RuntimeJavaMethod[]>(baseMethods);
                    AddMirandaMethods(methodsArray, baseMethodsArray, wrapper);
                    methods = methodsArray.ToArray();
                    baseMethods = baseMethodsArray.ToArray();
                }

                if (!wrapper.IsInterface)
                    AddDelegateInvokeStubs(wrapper, ref methods);

                wrapper.SetMethods(methods);

                fields = new RuntimeJavaField[classFile.Fields.Length];
                for (int i = 0; i < fields.Length; i++)
                {
                    var fld = classFile.Fields[i];
                    if (fld.IsStaticFinalConstant)
                    {
                        RuntimeJavaType fieldType = null;
#if !IMPORTER
                        fieldType = wrapper.Context.ClassLoaderFactory.GetBootstrapClassLoader().FieldTypeWrapperFromSig(fld.Signature, LoadMode.LoadOrThrow);
#endif
                        fields[i] = new RuntimeConstantJavaField(wrapper, fieldType, fld.Name, fld.Signature, fld.Modifiers, null, fld.ConstantValue, MemberFlags.None);
                    }
                    else if (fld.IsProperty)
                    {
                        fields[i] = new RuntimeByteCodePropertyJavaField(wrapper, fld);
                    }
                    else
                    {
                        fields[i] = RuntimeJavaField.Create(wrapper, null, null, fld.Name, fld.Signature, new ExModifiers(fld.Modifiers, fld.IsInternal));
                    }
                }
#if IMPORTER
                wrapper.AddMapXmlFields(ref fields);
#endif
                wrapper.SetFields(fields);
            }

#if IMPORTER

            bool SupportsCallerID(ClassFile.Method method)
            {
                if ((classFile.Name == "sun.reflect.Reflection" && method.Name == "getCallerClass") || (classFile.Name == "java.lang.SecurityManager" && method.Name == "checkMemberAccess"))
                {
                    // ignore CallerSensitive on methods that don't need CallerID parameter
                    return false;
                }
                else if (method.IsStatic)
                {
                    return true;
                }
                else if ((classFile.IsFinal || classFile.Name == "java.lang.Runtime" || classFile.Name == "java.io.ObjectStreamClass") && wrapper.BaseTypeWrapper.GetMethodWrapper(method.Name, method.Signature, true) == null && !HasInterfaceMethod(wrapper, method.Name, method.Signature))
                {
                    // We only support CallerID instance methods on final or effectively final types,
                    // because we don't support interface stubs with CallerID.
                    // We also don't support a CallerID method overriding a method or implementing an interface.
                    return true;
                }
                else if (RequiresDynamicReflectionCallerClass(classFile.Name, method.Name, method.Signature))
                {
                    // We don't support CallerID for virtual methods that can be overridden or implement an interface,
                    // so these methods will do a dynamic stack walk if when Reflection.getCallerClass() is used.
                    return false;
                }
                else
                {
                    // If we end up here, we either have to add support or add them to the white-list in the above clause
                    // to allow them to fall back to dynamic stack walking.
                    wrapper.Context.StaticCompiler.IssueMessage(Message.CallerSensitiveOnUnsupportedMethod, classFile.Name, method.Name, method.Signature);
                    return false;
                }
            }

            static bool HasInterfaceMethod(RuntimeJavaType tw, string name, string signature)
            {
                for (; tw != null; tw = tw.BaseTypeWrapper)
                {
                    foreach (var iface in tw.Interfaces)
                    {
                        if (iface.GetMethodWrapper(name, signature, false) != null)
                        {
                            return true;
                        }
                        if (HasInterfaceMethod(iface, name, signature))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
#endif

            internal void CreateStep2()
            {
#if IMPORTER
                if (typeBuilder != null)
                {
                    // in the static compiler we need to create the TypeBuilder from outer to inner
                    // and to avoid having to sort the classes this way, we instead call CreateStep2
                    // on demand for outer wrappers and this necessitates us to keep track of
                    // whether we've already been called
                    return;
                }
#endif

                // this method is not allowed to throw exceptions (if it does, the runtime will abort)
                var hasclinit = wrapper.HasStaticInitializer;
                var mangledTypeName = wrapper.classLoader.GetTypeWrapperFactory().AllocMangledName(wrapper);
                var f = classFile;

                try
                {
                    TypeAttributes typeAttribs = 0;
                    if (f.IsAbstract)
                        typeAttribs |= TypeAttributes.Abstract;
                    if (f.IsFinal)
                        typeAttribs |= TypeAttributes.Sealed;
                    if (!hasclinit)
                        typeAttribs |= TypeAttributes.BeforeFieldInit;
#if IMPORTER

                    bool cantNest = false;
                    bool setModifiers = false;
                    TypeBuilder enclosing = null;
                    string enclosingClassName = null;

                    // we only compile inner classes as nested types in the static compiler, because it has a higher cost
                    // and doesn't buy us anything in dynamic mode (and if fact, due to an FXBUG it would make handling
                    // the TypeResolve event very hard)
                    var outerClass = getOuterClass();
                    if (outerClass != null && outerClass.OuterClass != null)
                    {
                        enclosingClassName = classFile.GetConstantPoolClass(outerClass.OuterClass.Handle);
                    }
                    else if (f.EnclosingMethod != null)
                    {
                        enclosingClassName = f.EnclosingMethod[0];
                    }

                    if (enclosingClassName != null)
                    {
                        if (CheckInnerOuterNames(f.Name, enclosingClassName) == false)
                        {
                            Tracer.Warning(Tracer.Compiler, "Incorrect {0} attribute on {1}", outerClass != null && outerClass.OuterClass != null ? "InnerClasses" : "EnclosingMethod", f.Name);
                        }
                        else
                        {
                            try
                            {
                                enclosingClassWrapper = wrapper.classLoader.TryLoadClassByName(enclosingClassName) as RuntimeByteCodeJavaType;
                            }
                            catch (RetargetableJavaException x)
                            {
                                Tracer.Warning(Tracer.Compiler, "Unable to load outer class {0} for inner class {1} ({2}: {3})", enclosingClassName, f.Name, x.GetType().Name, x.Message);
                            }

                            if (enclosingClassWrapper != null)
                            {
                                // make sure the relationship is reciprocal (otherwise we run the risk of
                                // baking the outer type before the inner type) and that the inner and outer
                                // class live in the same class loader (when doing a multi target compilation,
                                // it is possible to split the two classes across assemblies)
                                var oimpl = enclosingClassWrapper.impl as JavaTypeImpl;
                                if (oimpl != null && enclosingClassWrapper.GetClassLoader() == wrapper.GetClassLoader())
                                {
                                    var outerClassFile = oimpl.classFile;
                                    var outerInnerClasses = outerClassFile.InnerClasses;
                                    if (outerInnerClasses == null)
                                    {
                                        enclosingClassWrapper = null;
                                    }
                                    else
                                    {
                                        var ok = false;
                                        foreach (var i in outerInnerClasses.Items)
                                        {
                                            if (((i.OuterClass != null && outerClassFile.GetConstantPoolClass(i.OuterClass.Handle) == outerClassFile.Name) || (i.OuterClass == null && outerClass.OuterClass == null)) &&
                                                  i.InnerClass != null && outerClassFile.GetConstantPoolClass(i.InnerClass.Handle) == f.Name)
                                            {
                                                ok = true;
                                                break;
                                            }
                                        }

                                        if (ok == false)
                                        {
                                            enclosingClassWrapper = null;
                                        }
                                    }
                                }
                                else
                                {
                                    enclosingClassWrapper = null;
                                }

                                if (enclosingClassWrapper != null)
                                {
                                    enclosingClassWrapper.CreateStep2();
                                    enclosing = oimpl.typeBuilder;
                                    if (outerClass.OuterClass != null)
                                    {
                                        // we need to record that we're not an inner classes, but an enclosed class
                                        typeAttribs |= TypeAttributes.SpecialName;
                                    }
                                }
                                else
                                {
                                    Tracer.Warning(Tracer.Compiler, "Non-reciprocal inner class {0}", f.Name);
                                }
                            }
                        }
                    }

                    if (f.IsPublic)
                    {
                        if (enclosing != null)
                        {
                            if (enclosingClassWrapper.IsPublic)
                            {
                                typeAttribs |= TypeAttributes.NestedPublic;
                            }
                            else
                            {
                                // We're a public type nested inside a non-public type, this means that we can't compile this type as a nested type,
                                // because that would mean it wouldn't be visible outside the assembly.
                                cantNest = true;
                                typeAttribs |= TypeAttributes.Public;
                            }
                        }
                        else
                        {
                            typeAttribs |= TypeAttributes.Public;
                        }
                    }
                    else if (enclosing != null)
                    {
                        typeAttribs |= TypeAttributes.NestedAssembly;
                    }
#else // IMPORTER
                    if (f.IsPublic)
                    {
                        typeAttribs |= TypeAttributes.Public;
                    }
#endif // IMPORTER
                    if (f.IsInterface)
                    {
                        typeAttribs |= TypeAttributes.Interface | TypeAttributes.Abstract;
#if IMPORTER
                        // if any "meaningless" bits are set, preserve them
                        setModifiers |= (f.Modifiers & (Modifiers)0x99CE) != 0;
                        // by default we assume interfaces are abstract, so in the exceptional case we need a ModifiersAttribute
                        setModifiers |= (f.Modifiers & Modifiers.Abstract) == 0;
                        if (enclosing != null && !cantNest)
                        {
                            if (wrapper.IsGhost)
                            {
                                // TODO this is low priority, since the current Java class library doesn't define any ghost interfaces
                                // as inner classes
                                throw new NotImplementedException();
                            }

                            // LAMESPEC the CLI spec says interfaces cannot contain nested types (Part.II, 9.6), but that rule isn't enforced
                            // (and broken by J# as well), so we'll just ignore it too.
                            typeBuilder = enclosing.DefineNestedType(AllocNestedTypeName(enclosingClassWrapper.Name, f.Name), typeAttribs);
                        }
                        else
                        {
                            if (wrapper.IsGhost)
                            {
                                typeBuilder = wrapper.DefineGhostType(mangledTypeName, typeAttribs);
                            }
                            else
                            {
                                typeBuilder = wrapper.classLoader.GetTypeWrapperFactory().ModuleBuilder.DefineType(mangledTypeName, typeAttribs);
                            }
                        }
#else // IMPORTER
                        typeBuilder = wrapper.classLoader.GetTypeWrapperFactory().ModuleBuilder.DefineType(mangledTypeName, typeAttribs);
#endif // IMPORTER
                    }
                    else
                    {
                        typeAttribs |= TypeAttributes.Class;
#if IMPORTER
                        // if any "meaningless" bits are set, preserve them
                        setModifiers |= (f.Modifiers & (Modifiers)0x99CE) != 0;
                        // by default we assume ACC_SUPER for classes, so in the exceptional case we need a ModifiersAttribute
                        setModifiers |= !f.IsSuper;
                        if (enclosing != null && !cantNest)
                        {
                            // LAMESPEC the CLI spec says interfaces cannot contain nested types (Part.II, 9.6), but that rule isn't enforced
                            // (and broken by J# as well), so we'll just ignore it too.
                            typeBuilder = enclosing.DefineNestedType(AllocNestedTypeName(enclosingClassWrapper.Name, f.Name), typeAttribs);
                        }
                        else
#endif // IMPORTER
                        {
                            typeBuilder = wrapper.classLoader.GetTypeWrapperFactory().ModuleBuilder.DefineType(mangledTypeName, typeAttribs);
                        }
                    }

#if IMPORTER
                    // When we're statically compiling, we associate the typeBuilder with the wrapper. This enables types in referenced assemblies to refer back to
                    // types that we're currently compiling (i.e. a cyclic dependency between the currently assembly we're compiling and a referenced assembly).
                    wrapper.Context.ClassLoaderFactory.SetWrapperForType(typeBuilder, wrapper);

                    if (outerClass.OuterClass != null)
                    {
                        if (enclosing != null && cantNest)
                        {
                            wrapper.Context.AttributeHelper.SetNonNestedInnerClass(enclosing, f.Name);
                        }
                        if (enclosing == null || cantNest)
                        {
                            wrapper.Context.AttributeHelper.SetNonNestedOuterClass(typeBuilder, enclosingClassName);
                        }
                    }

                    if (classFile.InnerClasses != null)
                    {
                        foreach (var inner in classFile.InnerClasses.Items)
                        {
                            var name = classFile.GetConstantPoolClass(inner.InnerClass.Handle);
                            var exists = false;

                            try
                            {
                                exists = wrapper.GetClassLoader().TryLoadClassByName(name) != null;
                            }
                            catch (RetargetableJavaException)
                            {

                            }

                            if (!exists)
                            {
                                wrapper.Context.AttributeHelper.SetNonNestedInnerClass(typeBuilder, name);
                            }
                        }
                    }

                    if (typeBuilder.FullName != wrapper.Name && wrapper.Name.Replace('$', '+') != typeBuilder.FullName)
                    {
                        wrapper.classLoader.AddNameMapping(wrapper.Name, typeBuilder.FullName);
                    }

                    if (f.IsAnnotation && Annotation.HasRetentionPolicyRuntime(f.Annotations))
                    {
                        annotationBuilder = new AnnotationBuilder(wrapper.Context, this, enclosing);
                        wrapper.SetAnnotation(annotationBuilder);
                    }

                    // For Java 5 Enum types, we generate a nested .NET enum.
                    // This is primarily to support annotations that take enum parameters.
                    if (f.IsEnum && f.IsPublic)
                    {
                        AddCliEnum();
                    }

                    AddInnerClassAttribute(enclosing != null, outerClass.InnerClass != null, mangledTypeName, (Modifiers)outerClass.InnerClassAccessFlags);
                    if (classFile.DeprecatedAttribute && !Annotation.HasObsoleteAttribute(classFile.Annotations))
                    {
                        wrapper.Context.AttributeHelper.SetDeprecatedAttribute(typeBuilder);
                    }

                    if (classFile.GenericSignature != null)
                    {
                        wrapper.Context.AttributeHelper.SetSignatureAttribute(typeBuilder, classFile.GenericSignature);
                    }
                    if (classFile.EnclosingMethod != null)
                    {
                        if (outerClass.OuterClass == null && enclosing != null && !cantNest)
                        {
                            // we don't need to record the enclosing type, if we're compiling the current type as a nested type because of the EnclosingMethod attribute
                            wrapper.Context.AttributeHelper.SetEnclosingMethodAttribute(typeBuilder, null, classFile.EnclosingMethod[1], classFile.EnclosingMethod[2]);
                        }
                        else
                        {
                            wrapper.Context.AttributeHelper.SetEnclosingMethodAttribute(typeBuilder, classFile.EnclosingMethod[0], classFile.EnclosingMethod[1], classFile.EnclosingMethod[2]);
                        }
                    }
                    if (classFile.RuntimeVisibleTypeAnnotations != null)
                    {
                        wrapper.Context.AttributeHelper.SetRuntimeVisibleTypeAnnotationsAttribute(typeBuilder, classFile.RuntimeVisibleTypeAnnotations);
                    }
                    if (wrapper.classLoader.EmitStackTraceInfo)
                    {
                        if (f.SourceFileAttribute != null)
                        {
                            if ((enclosingClassWrapper == null && f.SourceFileAttribute == typeBuilder.Name + ".java")
                                || (enclosingClassWrapper != null && f.SourceFileAttribute == enclosingClassWrapper.sourceFileName))
                            {
                                // we don't need to record the name because it matches our heuristic
                            }
                            else
                            {
                                wrapper.Context.AttributeHelper.SetSourceFile(typeBuilder, f.SourceFileAttribute);
                            }
                        }
                        else
                        {
                            wrapper.Context.AttributeHelper.SetSourceFile(typeBuilder, null);
                        }
                    }
                    // NOTE in Whidbey we can (and should) use CompilerGeneratedAttribute to mark Synthetic types
                    if (setModifiers || classFile.IsInternal || (classFile.Modifiers & (Modifiers.Synthetic | Modifiers.Annotation | Modifiers.Enum)) != 0)
                    {
                        wrapper.Context.AttributeHelper.SetModifiers(typeBuilder, classFile.Modifiers, classFile.IsInternal);
                    }
#endif // IMPORTER
                    if (hasclinit)
                    {
                        AddClinitTrigger();
                    }
                    if (HasStructLayoutAttributeAnnotation(classFile))
                    {
                        // when we have a StructLayoutAttribute, field order is significant,
                        // so we link all fields here to make sure they are created in class file order.
                        foreach (RuntimeJavaField fw in fields)
                        {
                            fw.Link();
                        }
                    }
                }
#if IMPORTER
                finally { }
#else
                catch (Exception x)
                {
                    throw new InternalException("Exception during JavaTypeImpl.CreateStep2", x);
                }
#endif
            }

#if IMPORTER

            private void AddInnerClassAttribute(bool isNestedType, bool isInnerClass, string mangledTypeName, Modifiers innerClassFlags)
            {
                string name = classFile.Name;

                if (isNestedType)
                {
                    if (name == enclosingClassWrapper.Name + "$" + typeBuilder.Name)
                    {
                        name = null;
                    }
                }
                else if (name == mangledTypeName)
                {
                    name = null;
                }

                if ((isInnerClass && RuntimeManagedByteCodeJavaType.PredictReflectiveModifiers(wrapper) != innerClassFlags) || name != null)
                {
                    // HACK we abuse the InnerClassAttribute to record to real name for non-inner classes as well
                    wrapper.Context.AttributeHelper.SetInnerClass(typeBuilder, name, isInnerClass ? innerClassFlags : wrapper.Modifiers);
                }
            }

            private void AddCliEnum()
            {
                CompilerClassLoader ccl = wrapper.classLoader;
                string name = "__Enum";
                while (!ccl.ReserveName(classFile.Name + "$" + name))
                {
                    name += "_";
                }
                enumBuilder = typeBuilder.DefineNestedType(name, TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.NestedPublic | TypeAttributes.Serializable, wrapper.Context.Types.Enum);
                wrapper.Context.AttributeHelper.HideFromJava(enumBuilder);
                enumBuilder.DefineField("value__", wrapper.Context.Types.Int32, FieldAttributes.Public | FieldAttributes.SpecialName | FieldAttributes.RTSpecialName);
                for (int i = 0; i < classFile.Fields.Length; i++)
                {
                    if (classFile.Fields[i].IsEnum)
                    {
                        FieldBuilder fieldBuilder = enumBuilder.DefineField(classFile.Fields[i].Name, enumBuilder, FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal);
                        fieldBuilder.SetConstant(i);
                    }
                }
                wrapper.SetEnumType(enumBuilder);
            }
#endif

            void AddClinitTrigger()
            {
                // We create a empty method that we can use to trigger our .cctor
                // (previously we used RuntimeHelpers.RunClassConstructor, but that is slow and requires additional privileges)
                var attribs = MethodAttributes.Static | MethodAttributes.SpecialName;
                if (classFile.IsAbstract)
                {
                    var hasfields = false;

                    // If we have any public static fields, the cctor trigger must (and may) be public as well
                    foreach (ClassFile.Field fld in classFile.Fields)
                    {
                        if (fld.IsPublic && fld.IsStatic)
                        {
                            hasfields = true;
                            break;
                        }
                    }

                    attribs |= hasfields ? MethodAttributes.Public : MethodAttributes.FamORAssem;
                }
                else
                {
                    attribs |= MethodAttributes.Public;
                }

                clinitMethod = typeBuilder.DefineMethod("__<clinit>", attribs, null, null);
                clinitMethod.GetILGenerator().Emit(OpCodes.Ret);
                clinitMethod.SetImplementationFlags(clinitMethod.GetMethodImplementationFlags());
            }

            private static bool HasStructLayoutAttributeAnnotation(ClassFile c)
            {
                if (c.Annotations != null)
                {
                    foreach (object[] annot in c.Annotations)
                    {
                        if ("Lcli/System/Runtime/InteropServices/StructLayoutAttribute$Annotation;".Equals(annot[1]))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

#if IMPORTER

            private InnerClassesAttributeItemReader getOuterClass()
            {
                var innerClasses = classFile.InnerClasses;
                if (innerClasses != null)
                    foreach (var i in innerClasses.Items)
                        if (i.InnerClass != null && classFile.GetConstantPoolClass(i.InnerClass.Handle) == classFile.Name)
                            return i;

                return null;
            }

            private bool IsSideEffectFreeStaticInitializerOrNoop(ClassFile.Method m, out bool noop)
            {
                if (m.ExceptionTable.Length != 0)
                {
                    noop = false;
                    return false;
                }
                noop = true;
                for (int i = 0; i < m.Instructions.Length; i++)
                {
                    NormalizedByteCode bc;
                    while ((bc = m.Instructions[i].NormalizedOpCode) == NormalizedByteCode.__goto)
                    {
                        int target = m.Instructions[i].TargetIndex;
                        if (target <= i)
                        {
                            // backward branch means we can't do anything
                            noop = false;
                            return false;
                        }
                        // we must skip the unused instructions because the "remove assertions" optimization
                        // uses a goto to remove the (now unused) code
                        i = target;
                    }
                    if (bc == NormalizedByteCode.__getstatic || bc == NormalizedByteCode.__putstatic)
                    {
                        ClassFile.ConstantPoolItemFieldref fld = classFile.SafeGetFieldref(m.Instructions[i].Arg1);
                        if (fld == null || fld.Class != classFile.Name)
                        {
                            noop = false;
                            return false;
                        }
                        // don't allow getstatic to load non-primitive fields, because that would
                        // cause the verifier to try to load the type
                        if (bc == NormalizedByteCode.__getstatic && "L[".IndexOf(fld.Signature[0]) != -1)
                        {
                            noop = false;
                            return false;
                        }
                        ClassFile.Field field = classFile.GetField(fld.Name, fld.Signature);
                        if (field == null)
                        {
                            noop = false;
                            return false;
                        }
                        if (bc == NormalizedByteCode.__putstatic)
                        {
                            if (field.IsProperty && field.PropertySetter != null)
                            {
                                noop = false;
                                return false;
                            }
                        }
                        else if (field.IsProperty && field.PropertyGetter != null)
                        {
                            noop = false;
                            return false;
                        }
                    }
                    else if (ByteCodeMetaData.CanThrowException(bc))
                    {
                        noop = false;
                        return false;
                    }
                    else if (bc == NormalizedByteCode.__aconst_null
                        || (bc == NormalizedByteCode.__iconst && m.Instructions[i].Arg1 == 0)
                        || bc == NormalizedByteCode.__return
                        || bc == NormalizedByteCode.__nop)
                    {
                        // valid instructions in a potential noop <clinit>
                    }
                    else
                    {
                        noop = false;
                    }
                }
                // the method needs to be verifiable to be side effect free, since we already analysed it,
                // we know that the verifier won't try to load any types (which isn't allowed at this time)
                try
                {
                    wrapper.Context.MethodAnalyzerFactory.Create(null, wrapper, null, classFile, m, wrapper.classLoader);
                    return true;
                }
                catch (VerifyError)
                {
                    return false;
                }
            }
#endif // IMPORTER

            private RuntimeJavaMethod GetMethodWrapperDuringCtor(RuntimeJavaType lookup, IList<RuntimeJavaMethod> methods, string name, string sig)
            {
                if (lookup == wrapper)
                {
                    foreach (RuntimeJavaMethod mw in methods)
                    {
                        if (mw.Name == name && mw.Signature == sig)
                        {
                            return mw;
                        }
                    }
                    if (lookup.BaseTypeWrapper == null)
                    {
                        return null;
                    }
                    else
                    {
                        return lookup.BaseTypeWrapper.GetMethodWrapper(name, sig, true);
                    }
                }
                else
                {
                    return lookup.GetMethodWrapper(name, sig, true);
                }
            }

            private void AddMirandaMethods(List<RuntimeJavaMethod> methods, List<RuntimeJavaMethod[]> baseMethods, RuntimeJavaType tw)
            {
                foreach (RuntimeJavaType iface in tw.Interfaces)
                {
                    if (iface.IsPublic && this.wrapper.IsInterface)
                    {
                        // for interfaces, we only need miranda methods for non-public interfaces that we extend
                        continue;
                    }
                    AddMirandaMethods(methods, baseMethods, iface);
                    foreach (RuntimeJavaMethod ifmethod in iface.GetMethods())
                    {
                        // skip <clinit> and non-virtual interface methods introduced in Java 8
                        if (ifmethod.IsVirtual)
                        {
                            RuntimeJavaType lookup = wrapper;
                            while (lookup != null)
                            {
                                RuntimeJavaMethod mw = GetMethodWrapperDuringCtor(lookup, methods, ifmethod.Name, ifmethod.Signature);
                                if (mw == null || (mw.IsMirandaMethod && mw.DeclaringType != wrapper))
                                {
                                    mw = RuntimeMirandaJavaMethod.Create(wrapper, ifmethod);
                                    methods.Add(mw);
                                    baseMethods.Add(new RuntimeJavaMethod[] { ifmethod });
                                    break;
                                }
                                if (mw.IsMirandaMethod && mw.DeclaringType == wrapper)
                                {
                                    methods[methods.IndexOf(mw)] = ((RuntimeMirandaJavaMethod)mw).Update(ifmethod);
                                    break;
                                }
                                if (!mw.IsStatic || mw.DeclaringType == wrapper)
                                {
                                    break;
                                }
                                lookup = mw.DeclaringType.BaseTypeWrapper;
                            }
                        }
                    }
                }
            }

            private void AddDelegateInvokeStubs(RuntimeJavaType tw, ref RuntimeJavaMethod[] methods)
            {
                foreach (RuntimeJavaType iface in tw.Interfaces)
                {
                    if (iface.IsFakeNestedType
                        && iface.GetMethods().Length == 1
                        && iface.GetMethods()[0].IsDelegateInvokeWithByRefParameter)
                    {
                        RuntimeJavaMethod mw = new DelegateInvokeStubMethodWrapper(wrapper, iface.DeclaringTypeWrapper.TypeAsBaseType, iface.GetMethods()[0].Signature);
                        if (GetMethodWrapperDuringCtor(wrapper, methods, mw.Name, mw.Signature) == null)
                        {
                            methods = ArrayUtil.Concat(methods, mw);
                        }
                    }
                    AddDelegateInvokeStubs(iface, ref methods);
                }
            }

#if IMPORTER
            private static bool CheckInnerOuterNames(string inner, string outer)
            {
                // do some sanity checks on the inner/outer class names
                return inner.Length > outer.Length + 1 && inner[outer.Length] == '$' && inner.StartsWith(outer, StringComparison.Ordinal);
            }

            private string AllocNestedTypeName(string outer, string inner)
            {
                Debug.Assert(CheckInnerOuterNames(inner, outer));
                nestedTypeNames ??= new ConcurrentDictionary<string, RuntimeJavaType>();
                return DynamicClassLoaderFactory.TypeNameMangleImpl(nestedTypeNames, inner.Substring(outer.Length + 1), null);
            }

#endif

            private int GetMethodIndex(RuntimeJavaMethod mw)
            {
                for (int i = 0; i < methods.Length; i++)
                {
                    if (methods[i] == mw)
                    {
                        return i;
                    }
                }

                throw new InvalidOperationException();
            }

            private static void CheckLoaderConstraints(RuntimeJavaMethod mw, RuntimeJavaMethod baseMethod)
            {
                if (mw.ReturnType != baseMethod.ReturnType)
                {
                    if (mw.ReturnType.IsUnloadable || baseMethod.ReturnType.IsUnloadable)
                    {
                        // unloadable types can never cause a loader constraint violation
                        if (mw.ReturnType.IsUnloadable && baseMethod.ReturnType.IsUnloadable)
                        {
                            ((RuntimeUnloadableJavaType)mw.ReturnType).SetCustomModifier(((RuntimeUnloadableJavaType)baseMethod.ReturnType).CustomModifier);
                        }
                    }
                    else
                    {
#if IMPORTER
                        StaticCompiler.LinkageError("Method \"{2}.{3}{4}\" has a return type \"{0}\" and tries to override method \"{5}.{3}{4}\" that has a return type \"{1}\"", mw.ReturnType, baseMethod.ReturnType, mw.DeclaringType.Name, mw.Name, mw.Signature, baseMethod.DeclaringType.Name);
#else
                        throw new LinkageError("Loader constraints violated");
#endif
                    }
                }
                RuntimeJavaType[] here = mw.GetParameters();
                RuntimeJavaType[] there = baseMethod.GetParameters();
                for (int i = 0; i < here.Length; i++)
                {
                    if (here[i] != there[i])
                    {
                        if (here[i].IsUnloadable || there[i].IsUnloadable)
                        {
                            // unloadable types can never cause a loader constraint violation
                            if (here[i].IsUnloadable && there[i].IsUnloadable)
                            {
                                ((RuntimeUnloadableJavaType)here[i]).SetCustomModifier(((RuntimeUnloadableJavaType)there[i]).CustomModifier);
                            }
                        }
                        else
                        {
#if IMPORTER
                            StaticCompiler.LinkageError("Method \"{2}.{3}{4}\" has an argument type \"{0}\" and tries to override method \"{5}.{3}{4}\" that has an argument type \"{1}\"", here[i], there[i], mw.DeclaringType.Name, mw.Name, mw.Signature, baseMethod.DeclaringType.Name);
#else
                            throw new LinkageError("Loader constraints violated");
#endif
                        }
                    }
                }
            }

            private int GetFieldIndex(RuntimeJavaField fw)
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    if (fields[i] == fw)
                    {
                        return i;
                    }
                }
                throw new InvalidOperationException();
            }

            internal override FieldInfo LinkField(RuntimeJavaField fw)
            {
                if (fw is RuntimeByteCodePropertyJavaField)
                {
                    ((RuntimeByteCodePropertyJavaField)fw).DoLink(typeBuilder);
                    return null;
                }
                int fieldIndex = GetFieldIndex(fw);
#if IMPORTER
                if (wrapper.GetClassLoader().RemoveUnusedFields
                    && fw.IsPrivate
                    && fw.IsStatic
                    && fw.IsFinal
                    && !fw.IsSerialVersionUID
                    && classFile.Fields[fieldIndex].Annotations == null
                    && !classFile.IsReferenced(classFile.Fields[fieldIndex]))
                {
                    // unused, so we skip it
                    Tracer.Info(Tracer.Compiler, "Unused field {0}::{1}", wrapper.Name, fw.Name);
                    return null;
                }

                // for compatibility with broken Java code that assumes that reflection returns the fields in class declaration
                // order, we emit the fields in class declaration order in the .NET metadata (and then when we retrieve them
                // using .NET reflection, we sort on metadata token.)
                if (fieldIndex > 0)
                {
                    if (!fields[fieldIndex - 1].IsLinked)
                    {
                        for (int i = 0; i < fieldIndex; i++)
                        {
                            fields[i].Link();
                        }
                    }
                }

                if (fieldIndex >= classFile.Fields.Length)
                {
                    // this must be a field defined in map.xml
                    FieldAttributes fieldAttribs = 0;
                    if (fw.IsPublic)
                    {
                        fieldAttribs |= FieldAttributes.Public;
                    }
                    else if (fw.IsProtected)
                    {
                        fieldAttribs |= FieldAttributes.FamORAssem;
                    }
                    else if (fw.IsPrivate)
                    {
                        fieldAttribs |= FieldAttributes.Private;
                    }
                    else
                    {
                        fieldAttribs |= FieldAttributes.Assembly;
                    }
                    if (fw.IsStatic)
                    {
                        fieldAttribs |= FieldAttributes.Static;
                    }
                    if (fw.IsFinal)
                    {
                        fieldAttribs |= FieldAttributes.InitOnly;
                    }
                    return DefineField(fw.Name, fw.FieldTypeWrapper, fieldAttribs, fw.IsVolatile);
                }
#endif // IMPORTER
                FieldBuilder field;
                ClassFile.Field fld = classFile.Fields[fieldIndex];
                FieldAttributes attribs = 0;
                string realFieldName = UnicodeUtil.EscapeInvalidSurrogates(fld.Name);
                if (!ReferenceEquals(realFieldName, fld.Name))
                {
                    attribs |= FieldAttributes.SpecialName;
                }
                MethodAttributes methodAttribs = MethodAttributes.HideBySig;
#if IMPORTER
                bool setModifiers = fld.IsInternal || (fld.Modifiers & (Modifiers.Synthetic | Modifiers.Enum)) != 0;
#endif
                if (fld.IsPrivate)
                {
                    attribs |= FieldAttributes.Private;
                }
                else if (fld.IsProtected)
                {
                    attribs |= FieldAttributes.FamORAssem;
                    methodAttribs |= MethodAttributes.FamORAssem;
                }
                else if (fld.IsPublic)
                {
                    attribs |= FieldAttributes.Public;
                    methodAttribs |= MethodAttributes.Public;
                }
                else
                {
                    attribs |= FieldAttributes.Assembly;
                    methodAttribs |= MethodAttributes.Assembly;
                }

                if (fld.IsStatic)
                {
                    attribs |= FieldAttributes.Static;
                    methodAttribs |= MethodAttributes.Static;
                }
                // NOTE "constant" static finals are converted into literals
                // TODO it would be possible for Java code to change the value of a non-blank static final, but I don't
                // know if we want to support this (since the Java JITs don't really support it either)
                if (fld.IsStaticFinalConstant)
                {
                    Profiler.Count("Static Final Constant");
                    attribs |= FieldAttributes.Literal;
                    field = DefineField(realFieldName, fw.FieldTypeWrapper, attribs, false);
                    field.SetConstant(fld.ConstantValue);
                }
                else
                {
#if IMPORTER
                    if (wrapper.IsPublic && wrapper.NeedsType2AccessStub(fw))
                    {
                        // this field is going to get a type 2 access stub, so we hide the actual field
                        attribs &= ~FieldAttributes.FieldAccessMask;
                        attribs |= FieldAttributes.Assembly;
                        // instead of adding HideFromJava we rename the field to avoid confusing broken compilers
                        // see https://sourceforge.net/tracker/?func=detail&atid=525264&aid=3056721&group_id=69637
                        // additional note: now that we maintain the ordering of the fields, we need to recognize
                        // these fields so that we know where to insert the corresponding accessor property FieldWrapper.
                        realFieldName = NamePrefix.Type2AccessStubBackingField + realFieldName;
                    }
                    else if (fld.IsFinal)
                    {
                        if (wrapper.IsInterface || wrapper.classLoader.StrictFinalFieldSemantics)
                        {
                            attribs |= FieldAttributes.InitOnly;
                        }
                        else
                        {
                            setModifiers = true;
                        }
                    }
#else
                    if (fld.IsFinal && wrapper.IsInterface)
                    {
                        attribs |= FieldAttributes.InitOnly;
                    }
#endif

                    field = DefineField(realFieldName, fw.FieldTypeWrapper, attribs, fld.IsVolatile);
                }
                if (fld.IsTransient)
                {
                    CustomAttributeBuilder transientAttrib = new CustomAttributeBuilder(wrapper.Context.Resolver.ResolveCoreType(typeof(NonSerializedAttribute).FullName).GetConstructor(Type.EmptyTypes), new object[0]);
                    field.SetCustomAttribute(transientAttrib);
                }
#if IMPORTER
                {
                    // if the Java modifiers cannot be expressed in .NET, we emit the Modifiers attribute to store
                    // the Java modifiers
                    if (setModifiers)
                    {
                        wrapper.Context.AttributeHelper.SetModifiers(field, fld.Modifiers, fld.IsInternal);
                    }
                    if (fld.DeprecatedAttribute && !Annotation.HasObsoleteAttribute(fld.Annotations))
                    {
                        wrapper.Context.AttributeHelper.SetDeprecatedAttribute(field);
                    }
                    if (fld.GenericSignature != null)
                    {
                        wrapper.Context.AttributeHelper.SetSignatureAttribute(field, fld.GenericSignature);
                    }
                    if (fld.RuntimeVisibleTypeAnnotations != null)
                    {
                        wrapper.Context.AttributeHelper.SetRuntimeVisibleTypeAnnotationsAttribute(field, fld.RuntimeVisibleTypeAnnotations);
                    }
                }
#endif

                return field;
            }

            private FieldBuilder DefineField(string name, RuntimeJavaType tw, FieldAttributes attribs, bool isVolatile)
            {
                Type[] modreq = isVolatile ? new Type[] { wrapper.Context.Types.IsVolatile } : Type.EmptyTypes;
                return typeBuilder.DefineField(name, tw.TypeAsSignatureType, modreq, wrapper.GetModOpt(tw, false), attribs);
            }

            internal override void EmitRunClassConstructor(CodeEmitter ilgen)
            {
                if (clinitMethod != null)
                {
                    ilgen.Emit(OpCodes.Call, clinitMethod);
                }
            }

            internal override DynamicImpl Finish()
            {
                var baseTypeWrapper = wrapper.BaseTypeWrapper;
                if (baseTypeWrapper != null)
                {
                    baseTypeWrapper.Finish();
                    baseTypeWrapper.LinkAll();
                }

                // NOTE there is a bug in the CLR (.NET 1.0 & 1.1 [1.2 is not yet available]) that
                // causes the AppDomain.TypeResolve event to receive the incorrect type name for nested types.
                // The Name in the ResolveEventArgs contains only the nested type name, not the full type name,
                // for example, if the type being resolved is "MyOuterType+MyInnerType", then the event only
                // receives "MyInnerType" as the name. Since we only compile inner classes as nested types
                // when we're statically compiling, we can only run into this bug when we're statically compiling.
                // NOTE To work around this bug, we have to make sure that all types that are going to be
                // required in finished form, are finished explicitly here. It isn't clear what other types are
                // required to be finished. I instrumented a static compilation of classpath.dll and this
                // turned up no other cases of the TypeResolve event firing.
                foreach (RuntimeJavaType iface in wrapper.interfaces)
                {
                    iface.Finish();
                    iface.LinkAll();
                }

                // make sure all classes are loaded, before we start finishing the type. During finishing, we
                // may not run any Java code, because that might result in a request to finish the type that we
                // are in the process of finishing, and this would be a problem.
                // Prevent infinity recursion for broken class loaders by keeping a recursion count and falling
                // back to late binding if we recurse more than twice.
                var mode = System.Threading.Interlocked.Increment(ref recursionCount) > 2 || (JVM.DisableEagerClassLoading && wrapper.Name != "sun.reflect.misc.Trampoline")
                    ? LoadMode.ReturnUnloadable
                    : LoadMode.Link;
                try
                {
                    classFile.Link(wrapper, mode);

                    for (int i = 0; i < fields.Length; i++)
                        fields[i].Link(mode);

                    for (int i = 0; i < methods.Length; i++)
                        methods[i].Link(mode);
                }
                finally
                {
                    System.Threading.Interlocked.Decrement(ref recursionCount);
                }

                // this is the correct lock, FinishCore doesn't call any user code and mutates global state,
                // so it needs to be protected by a lock.
                lock (this)
                {
                    FinishedTypeImpl impl;
                    try
                    {
                        // call FinishCore in the finally to avoid Thread.Abort interrupting the thread
                    }
                    finally
                    {
                        impl = FinishCore();
                    }
                    return impl;
                }
            }

            FinishedTypeImpl FinishCore()
            {
                // it is possible that the loading of the referenced classes triggered a finish of us,
                // if that happens, we just return
                if (finishedType != null)
                    return finishedType;

                if (finishInProgress)
                    throw new InvalidOperationException("Recursive finish attempt for " + wrapper.Name);

                finishInProgress = true;
                Tracer.Info(Tracer.Compiler, "Finishing: {0}", wrapper.Name);
                Profiler.Enter("JavaTypeImpl.Finish.Core");

                try
                {
                    RuntimeJavaType declaringTypeWrapper = null;
                    var innerClassesTypeWrappers = Array.Empty<RuntimeJavaType>();

                    // if we're an inner class, we need to attach an InnerClass attribute
                    var innerclasses = classFile.InnerClasses;
                    if (innerclasses != null)
                    {
                        // TODO consider not pre-computing innerClassesTypeWrappers and declaringTypeWrapper here
                        var wrappers = new List<RuntimeJavaType>();

                        foreach (var i in innerclasses.Items)
                        {
                            if (i.InnerClass != null && i.OuterClass != null)
                            {
                                if (classFile.GetConstantPoolClassType(i.OuterClass.Handle) == wrapper)
                                    wrappers.Add(classFile.GetConstantPoolClassType(i.InnerClass.Handle));
                                if (classFile.GetConstantPoolClassType(i.InnerClass.Handle) == wrapper)
                                    declaringTypeWrapper = classFile.GetConstantPoolClassType(i.OuterClass.Handle);
                            }
                        }

                        innerClassesTypeWrappers = wrappers.ToArray();
#if IMPORTER
                        // before we bake our type, we need to link any inner annotations to allow them to create their attribute type (as a nested type)
                        foreach (var tw in innerClassesTypeWrappers)
                        {
                            var dtw = tw as RuntimeByteCodeJavaType;
                            if (dtw != null)
                            {
                                var impl = dtw.impl as JavaTypeImpl;
                                if (impl != null)
                                    if (impl.annotationBuilder != null)
                                        impl.annotationBuilder.Link();
                            }
                        }
#endif
                    }
#if IMPORTER

                    if (annotationBuilder != null)
                    {
                        var cab = new CustomAttributeBuilder(wrapper.Context.Resolver.ResolveRuntimeType(typeof(AnnotationAttributeAttribute).FullName).GetConstructor(new Type[] { wrapper.Context.Types.String }), new object[] { UnicodeUtil.EscapeInvalidSurrogates(annotationBuilder.AttributeTypeName) });
                        typeBuilder.SetCustomAttribute(cab);
                    }

                    if (!wrapper.IsInterface && wrapper.IsMapUnsafeException)
                    {
                        // mark all exceptions that are unsafe for mapping with a custom attribute,
                        // so that at runtime we can quickly assertain if an exception type can be
                        // caught without filtering
                        wrapper.Context.AttributeHelper.SetExceptionIsUnsafeForMapping(typeBuilder);
                    }
#endif

                    var context = new FinishContext(wrapper.Context, host, classFile, wrapper, typeBuilder);
                    var type = context.FinishImpl();

#if IMPORTER
                    if (annotationBuilder != null)
                    {
                        annotationBuilder.Finish(this);
                    }
                    if (enumBuilder != null)
                    {
                        enumBuilder.CreateType();
                    }
                    if (privateInterfaceMethods != null)
                    {
                        privateInterfaceMethods.CreateType();
                    }
#endif
                    var finishedClinitMethod = (MethodInfo)clinitMethod;
#if !IMPORTER
                    if (finishedClinitMethod != null)
                    {
                        // In dynamic mode, we may need to emit a call to this method from a DynamicMethod which doesn't support calling unfinished methods,
                        // so we must resolve to the real method here.
                        finishedClinitMethod = type.GetMethod("__<clinit>", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                    }
#endif

                    finishedType = new FinishedTypeImpl(type, innerClassesTypeWrappers, declaringTypeWrapper, wrapper.ReflectiveModifiers, Metadata.Create(classFile), finishedClinitMethod, finalizeMethod, host);
                    return finishedType;
                }
#if !IMPORTER
                catch (Exception x)
                {
                    throw new InternalException($"Exception during finishing of: {wrapper.Name}", x);
                }
#endif
                finally
                {
                    Profiler.Leave("JavaTypeImpl.Finish.Core");
                }
            }

#if IMPORTER

            private bool IsValidAnnotationElementType(string type)
            {
                if (type[0] == '[')
                    type = type.Substring(1);

                switch (type)
                {
                    case "Z":
                    case "B":
                    case "S":
                    case "C":
                    case "I":
                    case "J":
                    case "F":
                    case "D":
                    case "Ljava.lang.String;":
                    case "Ljava.lang.Class;":
                        return true;
                }

                if (type.StartsWith("L") && type.EndsWith(";"))
                {
                    try
                    {
                        var tw = wrapper.GetClassLoader().TryLoadClassByName(type.Substring(1, type.Length - 2));
                        if (tw != null)
                        {
                            if ((tw.Modifiers & Modifiers.Annotation) != 0)
                                return true;

                            if ((tw.Modifiers & Modifiers.Enum) != 0)
                            {
                                var enumType = wrapper.Context.ClassLoaderFactory.GetBootstrapClassLoader().TryLoadClassByName("java.lang.Enum");
                                if (enumType != null && tw.IsSubTypeOf(enumType))
                                    return true;
                            }
                        }
                    }
                    catch
                    {

                    }
                }

                return false;
            }

            sealed class AnnotationBuilder : Annotation
            {

                readonly RuntimeContext context;

                JavaTypeImpl impl;
                TypeBuilder outer;
                TypeBuilder annotationTypeBuilder;
                TypeBuilder attributeTypeBuilder;
                MethodBuilder defineConstructor;

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                /// <param name="context"></param>
                /// <param name="o"></param>
                /// <param name="outer"></param>
                internal AnnotationBuilder(RuntimeContext context, JavaTypeImpl o, TypeBuilder outer)
                {
                    this.context = context;
                    this.impl = o;
                    this.outer = outer;
                }

                internal void Link()
                {
                    if (impl == null)
                    {
                        return;
                    }
                    JavaTypeImpl o = impl;
                    impl = null;

                    // Make sure the annotation type only has valid methods
                    for (int i = 0; i < o.methods.Length; i++)
                    {
                        if (!o.methods[i].IsStatic)
                        {
                            if (!o.methods[i].Signature.StartsWith("()"))
                            {
                                return;
                            }
                            if (!o.IsValidAnnotationElementType(o.methods[i].Signature.Substring(2)))
                            {
                                return;
                            }
                        }
                    }

                    // we only set annotationTypeBuilder if we're valid
                    annotationTypeBuilder = o.typeBuilder;

                    var annotationAttributeBaseType = context.ClassLoaderFactory.LoadClassCritical("ikvm.internal.AnnotationAttributeBase");

                    // make sure we don't clash with another class name
                    var ccl = o.wrapper.classLoader;
                    string name = UnicodeUtil.EscapeInvalidSurrogates(o.classFile.Name);
                    while (!ccl.ReserveName(name + "Attribute"))
                    {
                        name += "_";
                    }

                    var typeAttributes = TypeAttributes.Class | TypeAttributes.Sealed;
                    if (o.enclosingClassWrapper != null)
                    {
                        if (o.wrapper.IsPublic)
                        {
                            typeAttributes |= TypeAttributes.NestedPublic;
                        }
                        else
                        {
                            typeAttributes |= TypeAttributes.NestedAssembly;
                        }
                        attributeTypeBuilder = outer.DefineNestedType(o.AllocNestedTypeName(o.enclosingClassWrapper.Name, name + "Attribute"), typeAttributes, annotationAttributeBaseType.TypeAsBaseType);
                    }
                    else
                    {
                        if (o.wrapper.IsPublic)
                        {
                            typeAttributes |= TypeAttributes.Public;
                        }
                        else
                        {
                            typeAttributes |= TypeAttributes.NotPublic;
                        }
                        attributeTypeBuilder = o.wrapper.classLoader.GetTypeWrapperFactory().ModuleBuilder.DefineType(name + "Attribute", typeAttributes, annotationAttributeBaseType.TypeAsBaseType);
                    }
                    if (o.wrapper.IsPublic)
                    {
                        // In the Java world, the class appears as a non-public proxy class
                        context.AttributeHelper.SetModifiers(attributeTypeBuilder, Modifiers.Final, false);
                    }

                    // NOTE we "abuse" the InnerClassAttribute to add a custom attribute to name the class "$Proxy[Annotation]" in the Java world
                    int dotindex = o.classFile.Name.LastIndexOf('.') + 1;
                    context.AttributeHelper.SetInnerClass(attributeTypeBuilder, o.classFile.Name.Substring(0, dotindex) + "$Proxy" + o.classFile.Name.Substring(dotindex), Modifiers.Final);
                    attributeTypeBuilder.AddInterfaceImplementation(o.typeBuilder);
                    context.AttributeHelper.SetImplementsAttribute(attributeTypeBuilder, new RuntimeJavaType[] { o.wrapper });

                    if (o.classFile.Annotations != null)
                    {
                        CustomAttributeBuilder attributeUsageAttribute = null;
                        bool hasAttributeUsageAttribute = false;
                        foreach (object[] def in o.classFile.Annotations)
                        {
                            if (def[1].Equals("Ljava/lang/annotation/Target;") && !hasAttributeUsageAttribute)
                            {
                                for (int i = 2; i < def.Length; i += 2)
                                {
                                    if (def[i].Equals("value"))
                                    {
                                        object[] val = def[i + 1] as object[];
                                        if (val != null
                                            && val.Length > 0
                                            && val[0].Equals(AnnotationDefaultAttribute.TAG_ARRAY))
                                        {
                                            AttributeTargets targets = 0;
                                            for (int j = 1; j < val.Length; j++)
                                            {
                                                object[] eval = val[j] as object[];
                                                if (eval != null
                                                    && eval.Length == 3
                                                    && eval[0].Equals(AnnotationDefaultAttribute.TAG_ENUM)
                                                    && eval[1].Equals("Ljava/lang/annotation/ElementType;"))
                                                {
                                                    switch ((string)eval[2])
                                                    {
                                                        case "ANNOTATION_TYPE":
                                                            targets |= AttributeTargets.Interface;
                                                            break;
                                                        case "CONSTRUCTOR":
                                                            targets |= AttributeTargets.Constructor;
                                                            break;
                                                        case "FIELD":
                                                            targets |= AttributeTargets.Field;
                                                            break;
                                                        case "LOCAL_VARIABLE":
                                                            break;
                                                        case "METHOD":
                                                            targets |= AttributeTargets.Method;
                                                            break;
                                                        case "PACKAGE":
                                                            targets |= AttributeTargets.Interface;
                                                            break;
                                                        case "PARAMETER":
                                                            targets |= AttributeTargets.Parameter;
                                                            break;
                                                        case "TYPE":
                                                            targets |= AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct | AttributeTargets.Delegate | AttributeTargets.Enum;
                                                            break;
                                                    }
                                                }
                                            }
                                            attributeUsageAttribute = new CustomAttributeBuilder(context.Resolver.ResolveCoreType(typeof(AttributeUsageAttribute).FullName).GetConstructor(new Type[] { context.Resolver.ResolveCoreType(typeof(AttributeTargets).FullName) }), new object[] { targets });
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // apply any .NET custom attributes that are on the annotation to the custom attribute we synthesize
                                // (for example, to allow AttributeUsageAttribute to be overridden)
                                Annotation annotation = Annotation.Load(o.wrapper, def);
                                if (annotation != null && annotation.IsCustomAttribute)
                                {
                                    annotation.Apply(o.wrapper.GetClassLoader(), attributeTypeBuilder, def);
                                }
                                if (def[1].Equals("Lcli/System/AttributeUsageAttribute$Annotation;"))
                                {
                                    hasAttributeUsageAttribute = true;
                                }
                            }
                        }
                        if (attributeUsageAttribute != null && !hasAttributeUsageAttribute)
                        {
                            attributeTypeBuilder.SetCustomAttribute(attributeUsageAttribute);
                        }
                    }

                    defineConstructor = ReflectUtil.DefineConstructor(attributeTypeBuilder, MethodAttributes.Public, new Type[] { context.Resolver.ResolveCoreType(typeof(object).FullName).MakeArrayType() });
                    context.AttributeHelper.SetEditorBrowsableNever(defineConstructor);
                }

                private static Type TypeWrapperToAnnotationParameterType(RuntimeJavaType tw)
                {
                    bool isArray = false;
                    if (tw.IsArray)
                    {
                        isArray = true;
                        tw = tw.ElementTypeWrapper;
                    }
                    if (tw.Annotation != null)
                    {
                        // we don't support Annotation args
                        return null;
                    }
                    else
                    {
                        Type argType;
                        if (tw == tw.Context.JavaBase.TypeOfJavaLangClass)
                        {
                            argType = tw.Context.Types.Type;
                        }
                        else if (tw.EnumType != null)   // is it a Java enum?
                        {
                            argType = tw.EnumType;
                        }
                        else if (IsDotNetEnum(tw))
                        {
                            argType = tw.DeclaringTypeWrapper.TypeAsSignatureType;
                        }
                        else
                        {
                            argType = tw.TypeAsSignatureType;
                        }
                        if (isArray)
                        {
                            argType = RuntimeArrayJavaType.MakeArrayType(argType, 1);
                        }
                        return argType;
                    }
                }

                private static bool IsDotNetEnum(RuntimeJavaType tw)
                {
                    return tw.IsFakeNestedType && (tw.Modifiers & Modifiers.Enum) != 0;
                }

                internal string AttributeTypeName
                {
                    get
                    {
                        Link();
                        if (attributeTypeBuilder != null)
                        {
                            return attributeTypeBuilder.FullName;
                        }
                        return null;
                    }
                }

                private static void EmitSetValueCall(RuntimeJavaType annotationAttributeBaseType, CodeEmitter ilgen, string name, RuntimeJavaType tw, int argIndex)
                {
                    ilgen.Emit(OpCodes.Ldarg_0);
                    ilgen.Emit(OpCodes.Ldstr, name);
                    ilgen.EmitLdarg(argIndex);
                    if (tw.TypeAsSignatureType.IsValueType)
                    {
                        ilgen.Emit(OpCodes.Box, tw.TypeAsSignatureType);
                    }
                    else if (tw.EnumType != null)   // is it a Java enum?
                    {
                        ilgen.Emit(OpCodes.Box, tw.EnumType);
                    }
                    else if (IsDotNetEnum(tw))
                    {
                        ilgen.Emit(OpCodes.Box, tw.DeclaringTypeWrapper.TypeAsSignatureType);
                    }
                    var setValueMethod = annotationAttributeBaseType.GetMethodWrapper("setValue", "(Ljava.lang.String;Ljava.lang.Object;)V", false);
                    setValueMethod.Link();
                    setValueMethod.EmitCall(ilgen);
                }

                internal void Finish(JavaTypeImpl o)
                {
                    Link();
                    if (annotationTypeBuilder == null)
                    {
                        // not a valid annotation type
                        return;
                    }
                    RuntimeJavaType annotationAttributeBaseType = context.ClassLoaderFactory.LoadClassCritical("ikvm.internal.AnnotationAttributeBase");
                    annotationAttributeBaseType.Finish();

                    int requiredArgCount = 0;
                    int valueArg = -1;
                    bool unsupported = false;
                    for (int i = 0; i < o.methods.Length; i++)
                    {
                        if (!o.methods[i].IsStatic)
                        {
                            if (valueArg == -1 && o.methods[i].Name == "value")
                            {
                                valueArg = i;
                            }
                            if (o.classFile.Methods[i].AnnotationDefault == null)
                            {
                                if (TypeWrapperToAnnotationParameterType(o.methods[i].ReturnType) == null)
                                {
                                    unsupported = true;
                                    break;
                                }
                                requiredArgCount++;
                            }
                        }
                    }

                    MethodBuilder defaultConstructor = ReflectUtil.DefineConstructor(attributeTypeBuilder, unsupported || requiredArgCount > 0 ? MethodAttributes.Private : MethodAttributes.Public, Type.EmptyTypes);
                    CodeEmitter ilgen;

                    if (!unsupported)
                    {
                        if (requiredArgCount > 0)
                        {
                            Type[] args = new Type[requiredArgCount];
                            for (int i = 0, j = 0; i < o.methods.Length; i++)
                            {
                                if (!o.methods[i].IsStatic)
                                {
                                    if (o.classFile.Methods[i].AnnotationDefault == null)
                                    {
                                        args[j++] = TypeWrapperToAnnotationParameterType(o.methods[i].ReturnType);
                                    }
                                }
                            }
                            MethodBuilder reqArgConstructor = ReflectUtil.DefineConstructor(attributeTypeBuilder, MethodAttributes.Public, args);
                            context.AttributeHelper.HideFromJava(reqArgConstructor);
                            ilgen = context.CodeEmitterFactory.Create(reqArgConstructor);
                            ilgen.Emit(OpCodes.Ldarg_0);
                            ilgen.Emit(OpCodes.Call, defaultConstructor);
                            for (int i = 0, j = 0; i < o.methods.Length; i++)
                            {
                                if (!o.methods[i].IsStatic)
                                {
                                    if (o.classFile.Methods[i].AnnotationDefault == null)
                                    {
                                        reqArgConstructor.DefineParameter(++j, ParameterAttributes.None, o.methods[i].Name);
                                        EmitSetValueCall(annotationAttributeBaseType, ilgen, o.methods[i].Name, o.methods[i].ReturnType, j);
                                    }
                                }
                            }
                            ilgen.Emit(OpCodes.Ret);
                            ilgen.DoEmit();
                        }
                        else if (valueArg != -1)
                        {
                            // We don't have any required parameters, but we do have an optional "value" parameter,
                            // so we create an additional constructor (the default constructor will be public in this case)
                            // that accepts the value parameter.
                            Type argType = TypeWrapperToAnnotationParameterType(o.methods[valueArg].ReturnType);
                            if (argType != null)
                            {
                                MethodBuilder cb = ReflectUtil.DefineConstructor(attributeTypeBuilder, MethodAttributes.Public, new Type[] { argType });
                                context.AttributeHelper.HideFromJava(cb);
                                cb.DefineParameter(1, ParameterAttributes.None, "value");
                                ilgen = context.CodeEmitterFactory.Create(cb);
                                ilgen.Emit(OpCodes.Ldarg_0);
                                ilgen.Emit(OpCodes.Call, defaultConstructor);
                                EmitSetValueCall(annotationAttributeBaseType, ilgen, "value", o.methods[valueArg].ReturnType, 1);
                                ilgen.Emit(OpCodes.Ret);
                                ilgen.DoEmit();
                            }
                        }
                    }

                    ilgen = context.CodeEmitterFactory.Create(defaultConstructor);
                    ilgen.Emit(OpCodes.Ldarg_0);
                    o.wrapper.EmitClassLiteral(ilgen);
                    annotationAttributeBaseType.GetMethodWrapper("<init>", "(Ljava.lang.Class;)V", false).EmitCall(ilgen);
                    ilgen.Emit(OpCodes.Ret);
                    ilgen.DoEmit();

                    ilgen = context.CodeEmitterFactory.Create(defineConstructor);
                    ilgen.Emit(OpCodes.Ldarg_0);
                    ilgen.Emit(OpCodes.Call, defaultConstructor);
                    ilgen.Emit(OpCodes.Ldarg_0);
                    ilgen.Emit(OpCodes.Ldarg_1);
                    annotationAttributeBaseType.GetMethodWrapper("setDefinition", "([Ljava.lang.Object;)V", false).EmitCall(ilgen);
                    ilgen.Emit(OpCodes.Ret);
                    ilgen.DoEmit();

                    var getValueMethod = annotationAttributeBaseType.GetMethodWrapper("getValue", "(Ljava.lang.String;)Ljava.lang.Object;", false);
                    var getByteValueMethod = annotationAttributeBaseType.GetMethodWrapper("getByteValue", "(Ljava.lang.String;)B", false);
                    var getBooleanValueMethod = annotationAttributeBaseType.GetMethodWrapper("getBooleanValue", "(Ljava.lang.String;)Z", false);
                    var getCharValueMethod = annotationAttributeBaseType.GetMethodWrapper("getCharValue", "(Ljava.lang.String;)C", false);
                    var getShortValueMethod = annotationAttributeBaseType.GetMethodWrapper("getShortValue", "(Ljava.lang.String;)S", false);
                    var getIntValueMethod = annotationAttributeBaseType.GetMethodWrapper("getIntValue", "(Ljava.lang.String;)I", false);
                    var getFloatValueMethod = annotationAttributeBaseType.GetMethodWrapper("getFloatValue", "(Ljava.lang.String;)F", false);
                    var getLongValueMethod = annotationAttributeBaseType.GetMethodWrapper("getLongValue", "(Ljava.lang.String;)J", false);
                    var getDoubleValueMethod = annotationAttributeBaseType.GetMethodWrapper("getDoubleValue", "(Ljava.lang.String;)D", false);
                    for (int i = 0; i < o.methods.Length; i++)
                    {
                        // skip <clinit> and non-virtual interface methods introduced in Java 8
                        if (o.methods[i].IsVirtual)
                        {
                            MethodBuilder mb = o.methods[i].GetDefineMethodHelper().DefineMethod(o.wrapper, attributeTypeBuilder, o.methods[i].Name, MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.NewSlot);
                            attributeTypeBuilder.DefineMethodOverride(mb, (MethodInfo)o.methods[i].GetMethod());
                            ilgen = context.CodeEmitterFactory.Create(mb);
                            ilgen.Emit(OpCodes.Ldarg_0);
                            ilgen.Emit(OpCodes.Ldstr, o.methods[i].Name);
                            if (o.methods[i].ReturnType.IsPrimitive)
                            {
                                if (o.methods[i].ReturnType == context.PrimitiveJavaTypeFactory.BYTE)
                                {
                                    getByteValueMethod.EmitCall(ilgen);
                                }
                                else if (o.methods[i].ReturnType == context.PrimitiveJavaTypeFactory.BOOLEAN)
                                {
                                    getBooleanValueMethod.EmitCall(ilgen);
                                }
                                else if (o.methods[i].ReturnType == context.PrimitiveJavaTypeFactory.CHAR)
                                {
                                    getCharValueMethod.EmitCall(ilgen);
                                }
                                else if (o.methods[i].ReturnType == context.PrimitiveJavaTypeFactory.SHORT)
                                {
                                    getShortValueMethod.EmitCall(ilgen);
                                }
                                else if (o.methods[i].ReturnType == context.PrimitiveJavaTypeFactory.INT)
                                {
                                    getIntValueMethod.EmitCall(ilgen);
                                }
                                else if (o.methods[i].ReturnType == context.PrimitiveJavaTypeFactory.FLOAT)
                                {
                                    getFloatValueMethod.EmitCall(ilgen);
                                }
                                else if (o.methods[i].ReturnType == context.PrimitiveJavaTypeFactory.LONG)
                                {
                                    getLongValueMethod.EmitCall(ilgen);
                                }
                                else if (o.methods[i].ReturnType == context.PrimitiveJavaTypeFactory.DOUBLE)
                                {
                                    getDoubleValueMethod.EmitCall(ilgen);
                                }
                                else
                                {
                                    throw new InvalidOperationException();
                                }
                            }
                            else
                            {
                                getValueMethod.EmitCall(ilgen);
                                o.methods[i].ReturnType.EmitCheckcast(ilgen);
                            }
                            ilgen.Emit(OpCodes.Ret);
                            ilgen.DoEmit();

                            if (o.classFile.Methods[i].AnnotationDefault != null
                                && !(o.methods[i].Name == "value" && requiredArgCount == 0))
                            {
                                // now add a .NET property for this annotation optional parameter
                                Type argType = TypeWrapperToAnnotationParameterType(o.methods[i].ReturnType);
                                if (argType != null)
                                {
                                    PropertyBuilder pb = attributeTypeBuilder.DefineProperty(o.methods[i].Name, PropertyAttributes.None, argType, Type.EmptyTypes);
                                    context.AttributeHelper.HideFromJava(pb);
                                    MethodBuilder setter = attributeTypeBuilder.DefineMethod("set_" + o.methods[i].Name, MethodAttributes.Public, context.Types.Void, new Type[] { argType });
                                    context.AttributeHelper.HideFromJava(setter);
                                    pb.SetSetMethod(setter);
                                    ilgen = context.CodeEmitterFactory.Create(setter);
                                    EmitSetValueCall(annotationAttributeBaseType, ilgen, o.methods[i].Name, o.methods[i].ReturnType, 1);
                                    ilgen.Emit(OpCodes.Ret);
                                    ilgen.DoEmit();
                                    MethodBuilder getter = attributeTypeBuilder.DefineMethod("get_" + o.methods[i].Name, MethodAttributes.Public, argType, Type.EmptyTypes);
                                    context.AttributeHelper.HideFromJava(getter);
                                    pb.SetGetMethod(getter);
                                    // TODO implement the getter method
                                    ilgen = context.CodeEmitterFactory.Create(getter);
                                    ilgen.ThrowException(context.Resolver.ResolveCoreType(typeof(NotImplementedException).FullName));
                                    ilgen.DoEmit();
                                }
                            }
                        }
                    }
                    attributeTypeBuilder.CreateType();
                }

                private CustomAttributeBuilder MakeCustomAttributeBuilder(RuntimeClassLoader loader, object annotation)
                {
                    Link();
                    ConstructorInfo ctor = defineConstructor != null
                        ? defineConstructor.__AsConstructorInfo()
                        : context.Resolver.ResolveRuntimeType("IKVM.Attributes.DynamicAnnotationAttribute").GetConstructor(new Type[] { context.Types.Object.MakeArrayType() });
                    return new CustomAttributeBuilder(ctor, new object[] { AnnotationDefaultAttribute.Escape(QualifyClassNames(loader, annotation)) });
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
#endif // IMPORTER

            internal override RuntimeJavaType[] InnerClasses
            {
                get
                {
                    throw new InvalidOperationException("InnerClasses is only available for finished types");
                }
            }

            internal override RuntimeJavaType DeclaringTypeWrapper
            {
                get
                {
                    throw new InvalidOperationException("DeclaringTypeWrapper is only available for finished types");
                }
            }

            internal override Modifiers ReflectiveModifiers
            {
                get
                {
                    Modifiers mods;

                    var innerClasses = classFile.InnerClasses;
                    if (innerClasses != null)
                    {
                        foreach (var i in innerClasses.Items)
                        {
                            if (i.InnerClass != null)
                            {
                                if (classFile.GetConstantPoolClass(i.InnerClass.Handle) == wrapper.Name)
                                {
                                    // the mask comes from RECOGNIZED_INNER_CLASS_MODIFIERS in src/hotspot/share/vm/classfile/classFileParser.cpp
                                    // (minus ACC_SUPER)
                                    mods = (Modifiers)i.InnerClassAccessFlags & (Modifiers)0x761F;
                                    if (classFile.IsInterface)
                                        mods |= Modifiers.Abstract;

                                    return mods;
                                }
                            }
                        }
                    }

                    // the mask comes from JVM_RECOGNIZED_CLASS_MODIFIERS in src/hotspot/share/vm/prims/jvm.h
                    // (minus ACC_SUPER)
                    mods = classFile.Modifiers & (Modifiers)0x7611;
                    if (classFile.IsInterface)
                        mods |= Modifiers.Abstract;

                    return mods;
                }
            }

            /// <summary>
            /// Finds all methods that the specified name/sig is going to be overriding
            /// </summary>
            /// <param name="m"></param>
            /// <param name="explicitOverride"></param>
            /// <returns></returns>
            private RuntimeJavaMethod[] FindBaseMethods(ClassFile.Method m, out bool explicitOverride)
            {
                Debug.Assert(!classFile.IsInterface);
                Debug.Assert(m.Name != "<init>");

                // starting with Java 7 the algorithm changed
                return classFile.MajorVersion >= 51 ? FindBaseMethods7(m.Name, m.Signature, m.IsFinal && !m.IsPublic && !m.IsProtected, out explicitOverride) : FindBaseMethodsLegacy(m.Name, m.Signature, out explicitOverride);
            }

            RuntimeJavaMethod[] FindBaseMethods7(string name, string sig, bool packageFinal, out bool explicitOverride)
            {
                // NOTE this implements the (completely broken) OpenJDK 7 b147 HotSpot behavior,
                // not the algorithm specified in section 5.4.5 of the JavaSE7 JVM spec
                // see http://weblog.ikvm.net/PermaLink.aspx?guid=bde44d8b-7ba9-4e0e-b3a6-b735627118ff and subsequent posts
                // UPDATE as of JDK 7u65 and JDK 8u11, the algorithm changed again to handle package private methods differently
                // this code has not been updated to reflect these changes (we're still at JDK 8 GA level)
                explicitOverride = false;
                RuntimeJavaMethod topPublicOrProtectedMethod = null;
                var tw = wrapper.BaseTypeWrapper;
                while (tw != null)
                {
                    var baseMethod = tw.GetMethodWrapper(name, sig, true);
                    if (baseMethod == null)
                        break;
                    else if (baseMethod.IsAccessStub)
                    {
                        // ignore
                    }
                    else if (!baseMethod.IsStatic && (baseMethod.IsPublic || baseMethod.IsProtected))
                        topPublicOrProtectedMethod = baseMethod;
                    tw = baseMethod.DeclaringType.BaseTypeWrapper;
                }
                tw = wrapper.BaseTypeWrapper;
                while (tw != null)
                {
                    var baseMethod = tw.GetMethodWrapper(name, sig, true);
                    if (baseMethod == null)
                    {
                        break;
                    }
                    else if (baseMethod.IsAccessStub)
                    {
                        // ignore
                    }
                    else if (baseMethod.IsPrivate)
                    {
                        // skip
                    }
                    else if (baseMethod.IsFinal && (baseMethod.IsPublic || baseMethod.IsProtected || IsAccessibleInternal(baseMethod) || baseMethod.DeclaringType.IsPackageAccessibleFrom(wrapper)))
                    {
                        throw new VerifyError("final method " + baseMethod.Name + baseMethod.Signature + " in " + baseMethod.DeclaringType.Name + " is overridden in " + wrapper.Name);
                    }
                    else if (baseMethod.IsStatic)
                    {
                        // skip
                    }
                    else if (topPublicOrProtectedMethod == null && !baseMethod.IsPublic && !baseMethod.IsProtected && !IsAccessibleInternal(baseMethod) && !baseMethod.DeclaringType.IsPackageAccessibleFrom(wrapper))
                    {
                        // this is a package private method that we're not overriding (unless its vtable stream interleaves ours, which is a case we handle below)
                        explicitOverride = true;
                    }
                    else if (topPublicOrProtectedMethod != null && baseMethod.IsFinal && !baseMethod.IsPublic && !baseMethod.IsProtected && !IsAccessibleInternal(baseMethod) && !baseMethod.DeclaringType.IsPackageAccessibleFrom(wrapper))
                    {
                        // this is package private final method that we would override had it not been final, but which is ignored by HotSpot (instead of throwing a VerifyError)
                        explicitOverride = true;
                    }
                    else if (topPublicOrProtectedMethod == null)
                    {
                        if (explicitOverride)
                        {
                            var list = new List<RuntimeJavaMethod>();
                            list.Add(baseMethod);
                            // we might still have to override package methods from another package if the vtable streams are interleaved with ours
                            tw = wrapper.BaseTypeWrapper;
                            while (tw != null)
                            {
                                var baseMethod2 = tw.GetMethodWrapper(name, sig, true);
                                if (baseMethod2 == null || baseMethod2 == baseMethod)
                                {
                                    break;
                                }
                                var baseMethod3 = GetPackageBaseMethod(baseMethod.DeclaringType.BaseTypeWrapper, name, sig, baseMethod2.DeclaringType);
                                if (baseMethod3 != null)
                                {
                                    if (baseMethod2.IsFinal)
                                    {
                                        baseMethod2 = baseMethod3;
                                    }
                                    bool found = false;
                                    foreach (var mw in list)
                                    {
                                        if (mw.DeclaringType.IsPackageAccessibleFrom(baseMethod2.DeclaringType))
                                        {
                                            // we should only add each package once
                                            found = true;
                                            break;
                                        }
                                    }
                                    if (!found)
                                    {
                                        list.Add(baseMethod2);
                                    }
                                }
                                tw = baseMethod2.DeclaringType.BaseTypeWrapper;
                            }
                            return list.ToArray();
                        }
                        else
                        {
                            return new RuntimeJavaMethod[] { baseMethod };
                        }
                    }
                    else
                    {
                        if (packageFinal)
                        {
                            // when a package final method overrides a public or protected method, HotSpot does not mark that vtable slot as final,
                            // so we need an explicit override to force the MethodAttributes.NewSlot flag, otherwise the CLR won't allow us
                            // to override the original method in subsequent derived types
                            explicitOverride = true;
                        }

                        int majorVersion = 0;
                        if (!baseMethod.IsPublic && !baseMethod.IsProtected &&
                            ((TryGetClassFileVersion(baseMethod.DeclaringType, ref majorVersion) && majorVersion < 51)
                            // if TryGetClassFileVersion fails, we know that it is safe to call GetMethod() so we look at the actual method attributes here,
                            // because access widing ensures that if the method had overridden the top level method it would also be public or protected
                            || (majorVersion == 0 && (LinkAndGetMethod(baseMethod).Attributes & MethodAttributes.MemberAccessMask) == MethodAttributes.Assembly)))
                        {
                            // the method we're overriding is not public or protected, but there is a public or protected top level method,
                            // this means that baseMethod is part of a class with a major version < 51, so we have to explicitly override the top level method as well
                            // (we don't need to look for another package method to override, because by necessity baseMethod is already in our package)
                            return new RuntimeJavaMethod[] { baseMethod, topPublicOrProtectedMethod };
                        }
                        else if (!topPublicOrProtectedMethod.DeclaringType.IsPackageAccessibleFrom(wrapper))
                        {
                            // check if there is another method (in the same package) that we should override
                            tw = topPublicOrProtectedMethod.DeclaringType.BaseTypeWrapper;
                            while (tw != null)
                            {
                                RuntimeJavaMethod baseMethod2 = tw.GetMethodWrapper(name, sig, true);
                                if (baseMethod2 == null)
                                {
                                    break;
                                }
                                if (baseMethod2.IsAccessStub)
                                {
                                    // ignore
                                }
                                else if (baseMethod2.DeclaringType.IsPackageAccessibleFrom(wrapper) && !baseMethod2.IsPrivate)
                                {
                                    if (baseMethod2.IsFinal)
                                    {
                                        throw new VerifyError("final method " + baseMethod2.Name + baseMethod2.Signature + " in " + baseMethod2.DeclaringType.Name + " is overridden in " + wrapper.Name);
                                    }
                                    if (!baseMethod2.IsStatic)
                                    {
                                        if (baseMethod2.IsPublic || baseMethod2.IsProtected)
                                        {
                                            break;
                                        }
                                        return new RuntimeJavaMethod[] { baseMethod, baseMethod2 };
                                    }
                                }
                                tw = baseMethod2.DeclaringType.BaseTypeWrapper;
                            }
                        }
                        return new RuntimeJavaMethod[] { baseMethod };
                    }
                    tw = baseMethod.DeclaringType.BaseTypeWrapper;
                }
                return null;
            }

            bool IsAccessibleInternal(RuntimeJavaMethod mw)
            {
                return mw.IsInternal && mw.DeclaringType.InternalsVisibleTo(wrapper);
            }

            static MethodBase LinkAndGetMethod(RuntimeJavaMethod mw)
            {
                mw.Link();
                return mw.GetMethod();
            }

            static bool TryGetClassFileVersion(RuntimeJavaType tw, ref int majorVersion)
            {
                RuntimeByteCodeJavaType dtw = tw as RuntimeByteCodeJavaType;
                if (dtw != null)
                {
                    JavaTypeImpl impl = dtw.impl as JavaTypeImpl;
                    if (impl != null)
                    {
                        majorVersion = impl.classFile.MajorVersion;
                        return true;
                    }
                }
                return false;
            }

            static RuntimeJavaMethod GetPackageBaseMethod(RuntimeJavaType tw, string name, string sig, RuntimeJavaType package)
            {
                while (tw != null)
                {
                    var mw = tw.GetMethodWrapper(name, sig, true);
                    if (mw == null)
                        break;

                    if (mw.DeclaringType.IsPackageAccessibleFrom(package))
                        return mw.IsFinal ? null : mw;

                    tw = mw.DeclaringType.BaseTypeWrapper;
                }

                return null;
            }

            RuntimeJavaMethod[] FindBaseMethodsLegacy(string name, string sig, out bool explicitOverride)
            {
                explicitOverride = false;
                var tw = wrapper.BaseTypeWrapper;
                while (tw != null)
                {
                    var baseMethod = tw.GetMethodWrapper(name, sig, true);
                    if (baseMethod == null)
                    {
                        return null;
                    }
                    else if (baseMethod.IsAccessStub)
                    {
                        // ignore
                    }

                    // here are the complex rules for determining whether this method overrides the method we found
                    // RULE 1: final methods may not be overridden
                    // (note that we intentionally not check IsStatic here!)
                    else if (baseMethod.IsFinal && !baseMethod.IsPrivate && (baseMethod.IsPublic || baseMethod.IsProtected || baseMethod.DeclaringType.IsPackageAccessibleFrom(wrapper)))
                    {
                        throw new VerifyError("final method " + baseMethod.Name + baseMethod.Signature + " in " + baseMethod.DeclaringType.Name + " is overridden in " + wrapper.Name);
                    }
                    // RULE 1a: static methods are ignored (other than the RULE 1 check)
                    else if (baseMethod.IsStatic)
                    {
                    }
                    // RULE 2: public & protected methods can be overridden (package methods are handled by RULE 4)
                    // (by public, protected & *package* methods [even if they are in a different package])
                    else if (baseMethod.IsPublic || baseMethod.IsProtected)
                    {
                        // if we already encountered a package method, we cannot override the base method of
                        // that package method
                        if (explicitOverride)
                        {
                            explicitOverride = false;
                            return null;
                        }
                        if (!baseMethod.DeclaringType.IsPackageAccessibleFrom(wrapper))
                        {
                            // check if there is another method (in the same package) that we should override
                            tw = baseMethod.DeclaringType.BaseTypeWrapper;
                            while (tw != null)
                            {
                                RuntimeJavaMethod baseMethod2 = tw.GetMethodWrapper(name, sig, true);
                                if (baseMethod2 == null)
                                {
                                    break;
                                }
                                if (baseMethod2.IsAccessStub)
                                {
                                    // ignore
                                }
                                else if (baseMethod2.DeclaringType.IsPackageAccessibleFrom(wrapper) && !baseMethod2.IsPrivate)
                                {
                                    if (baseMethod2.IsFinal)
                                    {
                                        throw new VerifyError("final method " + baseMethod2.Name + baseMethod2.Signature + " in " + baseMethod2.DeclaringType.Name + " is overridden in " + wrapper.Name);
                                    }
                                    if (!baseMethod2.IsStatic)
                                    {
                                        if (baseMethod2.IsPublic || baseMethod2.IsProtected)
                                        {
                                            break;
                                        }
                                        return new RuntimeJavaMethod[] { baseMethod, baseMethod2 };
                                    }
                                }
                                tw = baseMethod2.DeclaringType.BaseTypeWrapper;
                            }
                        }
                        return new RuntimeJavaMethod[] { baseMethod };
                    }
                    // RULE 3: private and static methods are ignored
                    else if (!baseMethod.IsPrivate)
                    {
                        // RULE 4: package methods can only be overridden in the same package
                        if (baseMethod.DeclaringType.IsPackageAccessibleFrom(wrapper) || (baseMethod.IsInternal && baseMethod.DeclaringType.InternalsVisibleTo(wrapper)))
                        {
                            return new RuntimeJavaMethod[] { baseMethod };
                        }
                        // since we encountered a method with the same name/signature that we aren't overriding,
                        // we need to specify an explicit override
                        // NOTE we only do this if baseMethod isn't private, because if it is, Reflection.Emit
                        // will complain about the explicit MethodOverride (possibly a bug)
                        explicitOverride = true;
                    }
                    tw = baseMethod.DeclaringType.BaseTypeWrapper;
                }

                return null;
            }

            static MethodInfo GetBaseFinalizeMethod(RuntimeJavaType wrapper)
            {
                for (; ; )
                {
                    // HACK we get called during method linking (which is probably a bad idea) and
                    // it is possible for the base type not to be finished yet, so we look at the
                    // private state of the unfinished base types to find the finalize method.
                    var dtw = wrapper as RuntimeByteCodeJavaType;
                    if (dtw == null)
                        break;

                    var mw = dtw.GetMethodWrapper(StringConstants.FINALIZE, StringConstants.SIG_VOID, false);
                    if (mw != null)
                        mw.Link();

                    var finalizeImpl = dtw.impl.GetFinalizeMethod();
                    if (finalizeImpl != null)
                        return finalizeImpl;

                    wrapper = wrapper.BaseTypeWrapper;
                }

                if (wrapper == wrapper.Context.JavaBase.TypeOfJavaLangObject || wrapper == wrapper.Context.JavaBase.TypeOfjavaLangThrowable)
                {
                    return wrapper.Context.Types.Object.GetMethod("Finalize", BindingFlags.NonPublic | BindingFlags.Instance);
                }

                var type = wrapper.TypeAsBaseType;
                var baseFinalize = type.GetMethod("__<Finalize>", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
                if (baseFinalize != null)
                    return baseFinalize;

                while (type != null)
                {
                    foreach (MethodInfo m in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                    {
                        if (m.Name == "Finalize"
                            && m.ReturnType == wrapper.Context.Types.Void
                            && m.GetParameters().Length == 0)
                        {
                            if (m.GetBaseDefinition().DeclaringType == wrapper.Context.Types.Object)
                            {
                                return m;
                            }
                        }
                    }
                    type = type.BaseType;
                }
                return null;
            }

            MethodAttributes GetPropertyAccess(RuntimeJavaMethod mw)
            {
                var sig = mw.ReturnType.SigName;
                if (sig == "V")
                    sig = mw.GetParameters()[0].SigName;

                int access = -1;
                foreach (var field in classFile.Fields)
                {
                    if (field.IsProperty && field.IsStatic == mw.IsStatic && field.Signature == sig && (field.PropertyGetter == mw.Name || field.PropertySetter == mw.Name))
                    {
                        int nacc;
                        if (field.IsPublic)
                        {
                            nacc = 3;
                        }
                        else if (field.IsProtected)
                        {
                            nacc = 2;
                        }
                        else if (field.IsPrivate)
                        {
                            nacc = 0;
                        }
                        else
                        {
                            nacc = 1;
                        }
                        if (nacc > access)
                        {
                            access = nacc;
                        }
                    }
                }

                switch (access)
                {
                    case 0:
                        return MethodAttributes.Private;
                    case 1:
                        return MethodAttributes.Assembly;
                    case 2:
                        return MethodAttributes.FamORAssem;
                    case 3:
                        return MethodAttributes.Public;
                    default:
                        throw new InvalidOperationException();
                }
            }

            internal override MethodBase LinkMethod(RuntimeJavaMethod mw)
            {
                Debug.Assert(mw != null);

                if (mw is DelegateConstructorMethodWrapper dcmw)
                {
                    dcmw.DoLink(typeBuilder);
                    return null;
                }

                if (mw is DelegateInvokeStubMethodWrapper stub)
                {
                    return stub.DoLink(typeBuilder);
                }

                if (mw.IsClassInitializer && mw.IsNoOp && (!wrapper.IsSerializable || HasSerialVersionUID))
                {
                    // we don't need to emit the <clinit>, because it is empty and we're not serializable or have an explicit serialVersionUID
                    // (because we cannot affect serialVersionUID computation (which is the only way the presence of a <clinit> can surface)
                    // we cannot do this optimization if the class is serializable but doesn't have a serialVersionUID)
                    return null;
                }

                int index = GetMethodIndex(mw);
                if (baseMethods[index] != null)
                {
                    foreach (var baseMethod in baseMethods[index])
                    {
                        baseMethod.Link();
                        CheckLoaderConstraints(mw, baseMethod);
                    }
                }

                Debug.Assert(mw.GetMethod() == null);
                methods[index].AssertLinked();
                Profiler.Enter("JavaTypeImpl.GenerateMethod");

                try
                {
                    // index is outside the range of methods declared on class file
                    if (index >= classFile.Methods.Length)
                    {
                        // method is a miranda method
                        if (methods[index].IsMirandaMethod)
                        {
                            // we're a Miranda method or we're an inherited default interface method
                            Debug.Assert(baseMethods[index].Length == 1 && baseMethods[index][0].DeclaringType.IsInterface);

                            var mmw = (RuntimeMirandaJavaMethod)methods[index];
                            var attr = MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.CheckAccessOnOverride;

                            RuntimeJavaMethod baseMiranda = null;
                            bool baseMirandaOverrideStub = false;

                            if (wrapper.BaseTypeWrapper == null || (baseMiranda = wrapper.BaseTypeWrapper.GetMethodWrapper(mw.Name, mw.Signature, true)) == null || !baseMiranda.IsMirandaMethod)
                            {
                                // we're not overriding a miranda method in a base class, so can we set the newslot flag
                                attr |= MethodAttributes.NewSlot;
                            }
                            else
                            {
                                baseMiranda.Link();
                                if (CheckRequireOverrideStub(methods[index], baseMiranda))
                                {
                                    baseMirandaOverrideStub = true;
                                    attr |= MethodAttributes.NewSlot;
                                }
                            }

                            if (wrapper.IsInterface || (wrapper.IsAbstract && mmw.BaseMethod.IsAbstract && mmw.Error == null))
                            {
                                attr |= MethodAttributes.Abstract;
                            }

                            var mb = methods[index].GetDefineMethodHelper().DefineMethod(wrapper, typeBuilder, methods[index].Name, attr);
                            wrapper.Context.AttributeHelper.HideFromReflection(mb);

                            if (baseMirandaOverrideStub)
                            {
                                wrapper.GenerateOverrideStub(typeBuilder, baseMiranda, mb, methods[index]);
                            }

                            if ((!wrapper.IsAbstract && mmw.BaseMethod.IsAbstract) || (!wrapper.IsInterface && mmw.Error != null))
                            {
                                var message = mmw.Error ?? (wrapper.Name + "." + methods[index].Name + methods[index].Signature);
                                var ilgen = wrapper.Context.CodeEmitterFactory.Create(mb);
                                ilgen.EmitThrow(mmw.IsConflictError ? "java.lang.IncompatibleClassChangeError" : "java.lang.AbstractMethodError", message);
                                ilgen.DoEmit();
                                wrapper.EmitLevel4Warning(mmw.IsConflictError ? HardError.IncompatibleClassChangeError : HardError.AbstractMethodError, message);
                            }
#if IMPORTER
                            if (wrapper.IsInterface && !mmw.IsAbstract)
                            {
                                // even though we're not visible to reflection we need to record the fact that we have a default implementation
                                wrapper.Context.AttributeHelper.SetModifiers(mb, mmw.Modifiers, false);
                            }
#endif
                            return mb;
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }
                    }

                    var m = classFile.Methods[index];
                    MethodBuilder method;
                    bool setModifiers = false;

                    if (methods[index].HasCallerID && (m.Modifiers & Modifiers.VarArgs) != 0)
                    {
                        // the implicit callerID parameter was added at the end so that means we shouldn't use ParamArrayAttribute,
                        // so we need to explicitly record that the method is varargs
                        setModifiers = true;
                    }

                    if (m.IsConstructor)
                    {
                        method = GenerateConstructor(methods[index]);

                        // strictfp is the only modifier that a constructor can have
                        if (m.IsStrictfp)
                            setModifiers = true;
                    }
                    else if (m.IsClassInitializer)
                    {
                        method = ReflectUtil.DefineTypeInitializer(typeBuilder, wrapper.classLoader);
                    }
                    else
                    {
                        method = GenerateMethod(index, m, ref setModifiers);
                    }

                    // apply 'throws' Exceptions as attributes
                    var exceptions = m.ExceptionsAttribute;
                    methods[index].SetDeclaredExceptions(exceptions);

#if IMPORTER
                    wrapper.Context.AttributeHelper.SetThrowsAttribute(method, exceptions);

                    if (setModifiers || m.IsInternal || (m.Modifiers & (Modifiers.Synthetic | Modifiers.Bridge)) != 0)
                        wrapper.Context.AttributeHelper.SetModifiers(method, m.Modifiers, m.IsInternal);

                    // synthetic and bridge methods should not be visible to the user and set as compiler generated
                    if ((m.Modifiers & (Modifiers.Synthetic | Modifiers.Bridge)) != 0 && (m.IsPublic || m.IsProtected) && wrapper.IsPublic && !IsAccessBridge(classFile, m))
                    {
                        wrapper.Context.AttributeHelper.SetCompilerGenerated(method);
                        wrapper.Context.AttributeHelper.SetEditorBrowsableNever(method);
                    }

                    // ensure deprecated attribute appears on method if obsolete not specified
                    if (m.DeprecatedAttribute && !Annotation.HasObsoleteAttribute(m.Annotations))
                    {
                        wrapper.Context.AttributeHelper.SetDeprecatedAttribute(method);
                    }

                    // apply .NET attribute to record Java generic signature
                    if (m.GenericSignature != null)
                    {
                        wrapper.Context.AttributeHelper.SetSignatureAttribute(method, m.GenericSignature);
                    }

                    if (wrapper.GetClassLoader().NoParameterReflection)
                    {
                        // ignore MethodParameters (except to extract parameter names)
                    }
                    else if (m.MalformedMethodParameters)
                    {
                        wrapper.Context.AttributeHelper.SetMethodParametersAttribute(method, null);
                    }
                    else if (m.MethodParameters != null)
                    {
                        var modifiers = new Modifiers[m.MethodParameters.Length];
                        for (int i = 0; i < modifiers.Length; i++)
                            modifiers[i] = (Modifiers)m.MethodParameters[i].accessFlags;

                        wrapper.Context.AttributeHelper.SetMethodParametersAttribute(method, modifiers);
                    }

                    // copy runtime visible annotations as attributes
                    if (m.RuntimeVisibleTypeAnnotations != null)
                    {
                        wrapper.Context.AttributeHelper.SetRuntimeVisibleTypeAnnotationsAttribute(method, m.RuntimeVisibleTypeAnnotations);
                    }

#else // IMPORTER

                    if (setModifiers)
                    {
                        // shut up the compiler
                    }

#endif // IMPORTER

                    return method;
                }
                finally
                {
                    Profiler.Leave("JavaTypeImpl.GenerateMethod");
                }
            }

            private bool HasSerialVersionUID
            {
                get
                {
                    foreach (var field in fields)
                        if (field.IsSerialVersionUID)
                            return true;

                    return false;
                }
            }

            MethodBuilder GenerateConstructor(RuntimeJavaMethod mw)
            {
                return mw.GetDefineMethodHelper().DefineConstructor(wrapper, typeBuilder, GetMethodAccess(mw) | MethodAttributes.HideBySig);
            }

            MethodBuilder GenerateMethod(int index, ClassFile.Method m, ref bool setModifiers)
            {
                var attribs = MethodAttributes.HideBySig;
                if (m.IsNative)
                {
                    if (wrapper.IsPInvokeMethod(m))
                    {
                        // this doesn't appear to be necessary, but we use the flag in Finish to know
                        // that we shouldn't emit a method body
                        attribs |= MethodAttributes.PinvokeImpl;
                    }
                    else
                    {
                        setModifiers = true;
                    }
                }
                if (methods[index].IsPropertyAccessor)
                {
                    attribs |= GetPropertyAccess(methods[index]);
                    attribs |= MethodAttributes.SpecialName;
                    setModifiers = true;
                }
                else
                {
                    attribs |= GetMethodAccess(methods[index]);
                }

                if (m.IsAbstract || (!m.IsStatic && m.IsPublic && classFile.IsInterface))
                {
                    // only if the classfile is abstract, we make the CLR method abstract, otherwise,
                    // we have to generate a method that throws an AbstractMethodError (because the JVM
                    // allows abstract methods in non-abstract classes)
                    if (classFile.IsAbstract)
                    {
                        if (classFile.IsPublic && !classFile.IsFinal && !(m.IsPublic || m.IsProtected))
                        {
                            setModifiers = true;
                        }
                        else
                        {
                            if (!m.IsAbstract)
                            {
                                setModifiers = true;
                            }
                            attribs |= MethodAttributes.Abstract;
                        }
                    }
                    else
                    {
                        setModifiers = true;
                    }
                }
                if (m.IsFinal)
                {
                    if (m.IsVirtual)
                    {
                        attribs |= MethodAttributes.Final;
                    }
                    else
                    {
                        setModifiers = true;
                    }
                }
                if (m.IsStatic)
                {
                    attribs |= MethodAttributes.Static;
                    if (m.IsSynchronized)
                    {
                        setModifiers = true;
                    }
                }
                else if (!m.IsPrivate)
                {
                    attribs |= MethodAttributes.Virtual | MethodAttributes.CheckAccessOnOverride;
                }
                string name = UnicodeUtil.EscapeInvalidSurrogates(m.Name);
                if (!ReferenceEquals(name, m.Name))
                {
                    // mark as specialname to remind us to unescape the name
                    attribs |= MethodAttributes.SpecialName;
                }
#if IMPORTER
                if ((m.Modifiers & Modifiers.Bridge) != 0 && (m.IsPublic || m.IsProtected) && wrapper.IsPublic)
                {
                    string sigbase = m.Signature.Substring(0, m.Signature.LastIndexOf(')') + 1);
                    foreach (var mw in methods)
                    {
                        if (mw.Name == m.Name && mw.Signature.StartsWith(sigbase) && mw.Signature != m.Signature)
                        {
                            // To prevent bridge methods with covariant return types from confusing
                            // other .NET compilers (like C#), we rename the bridge method.
                            name = NamePrefix.Bridge + name;
                            break;
                        }
                    }
                }
#endif
                if ((attribs & MethodAttributes.Virtual) != 0 && !classFile.IsInterface)
                {
                    if (baseMethods[index] == null || (baseMethods[index].Length == 1 && baseMethods[index][0].DeclaringType.IsInterface))
                    {
                        // we need to set NewSlot here, to prevent accidentally overriding methods
                        // (for example, if a Java class has a method "boolean Equals(object)", we don't want that method
                        // to override System.Object.Equals)
                        attribs |= MethodAttributes.NewSlot;
                    }
                    else
                    {
                        // if we have a method overriding a more accessible method (the JVM allows this), we need to make the
                        // method more accessible, because otherwise the CLR will complain that we're reducing access
                        bool hasPublicBaseMethod = false;
                        foreach (RuntimeJavaMethod baseMethodWrapper in baseMethods[index])
                        {
                            MethodBase baseMethod = baseMethodWrapper.GetMethod();
                            if ((baseMethod.IsPublic && !m.IsPublic) ||
                                ((baseMethod.IsFamily || baseMethod.IsFamilyOrAssembly) && !m.IsPublic && !m.IsProtected) ||
                                (!m.IsPublic && !m.IsProtected && !baseMethodWrapper.DeclaringType.IsPackageAccessibleFrom(wrapper)))
                            {
                                hasPublicBaseMethod |= baseMethod.IsPublic;
                                attribs &= ~MethodAttributes.MemberAccessMask;
                                attribs |= hasPublicBaseMethod ? MethodAttributes.Public : MethodAttributes.FamORAssem;
                                setModifiers = true;
                            }
                        }
                    }
                }
                MethodBuilder mb = null;
#if IMPORTER
                mb = wrapper.DefineGhostMethod(typeBuilder, name, attribs, methods[index]);
#endif
                if (mb == null)
                {
                    bool needFinalize = false;
                    bool needDispatch = false;
                    MethodInfo baseFinalize = null;
                    if (baseMethods[index] != null && ReferenceEquals(m.Name, StringConstants.FINALIZE) && ReferenceEquals(m.Signature, StringConstants.SIG_VOID))
                    {
                        baseFinalize = GetBaseFinalizeMethod(wrapper.BaseTypeWrapper);
                        if (baseMethods[index][0].DeclaringType == wrapper.Context.JavaBase.TypeOfJavaLangObject)
                        {
                            // This type is the first type in the hierarchy to introduce a finalize method
                            // (other than the one in java.lang.Object obviously), so we need to override
                            // the real Finalize method and emit a dispatch call to our finalize method.
                            needFinalize = true;
                            needDispatch = true;
                        }
                        else if (m.IsFinal)
                        {
                            // One of our base classes already has a  finalize method, so we already are
                            // hooked into the real Finalize, but we need to override it again, to make it
                            // final (so that non-Java types cannot override it either).
                            needFinalize = true;
                            needDispatch = false;
                            // If the base class finalize was optimized away, we need a dispatch call after all.
                            if (baseFinalize.DeclaringType == wrapper.Context.Types.Object)
                            {
                                needDispatch = true;
                            }
                        }
                        else
                        {
                            // One of our base classes already has a finalize method, but it may have been an empty
                            // method so that the hookup to the real Finalize was optimized away, we need to check
                            // for that.
                            if (baseFinalize.DeclaringType == wrapper.Context.Types.Object)
                            {
                                needFinalize = true;
                                needDispatch = true;
                            }
                        }
                        if (needFinalize &&
                            !m.IsAbstract && !m.IsNative &&
                            (!m.IsFinal || classFile.IsFinal) &&
                            m.Instructions.Length > 0 &&
                            m.Instructions[0].NormalizedOpCode == NormalizedByteCode.__return)
                        {
                            // we've got an empty finalize method, so we don't need to override the real finalizer
                            // (not having a finalizer makes a huge perf difference)
                            needFinalize = false;
                        }
                    }
                    bool newslot = baseMethods[index] != null
                        && (methods[index].IsExplicitOverride || baseMethods[index][0].RealName != name || CheckRequireOverrideStub(methods[index], baseMethods[index][0]))
                        && !needFinalize;
                    if (newslot)
                    {
                        attribs |= MethodAttributes.NewSlot;
                    }
                    if (classFile.IsInterface && !m.IsPublic && !wrapper.IsGhost)
                    {
                        var tb = typeBuilder;
                        if (m.IsStatic)
                        {
                            mb = methods[index].GetDefineMethodHelper().DefineMethod(wrapper, tb, name, attribs);
                        }
                        else
                        {
                            // the CLR doesn't allow (non-virtual) instance methods in interfaces,
                            // so we need to turn it into a static method
                            mb = methods[index].GetDefineMethodHelper().DefineMethod(wrapper.GetClassLoader().GetTypeWrapperFactory(),
                                tb, NamePrefix.PrivateInterfaceInstanceMethod + name, attribs | MethodAttributes.Static | MethodAttributes.SpecialName,
                                typeBuilder, false);
#if IMPORTER
                            wrapper.Context.AttributeHelper.SetNameSig(mb, m.Name, m.Signature);
#endif
                        }
                        setModifiers = true;
                    }
                    else
                    {
                        mb = methods[index].GetDefineMethodHelper().DefineMethod(wrapper, typeBuilder, name, attribs);
                    }
                    if (baseMethods[index] != null && !needFinalize)
                    {
                        bool subsequent = false;
                        foreach (RuntimeJavaMethod baseMethod in baseMethods[index])
                        {
                            if (CheckRequireOverrideStub(methods[index], baseMethod))
                            {
                                wrapper.GenerateOverrideStub(typeBuilder, baseMethod, mb, methods[index]);
                            }
                            else if (subsequent || methods[index].IsExplicitOverride || baseMethod.RealName != name)
                            {
                                typeBuilder.DefineMethodOverride(mb, (MethodInfo)baseMethod.GetMethod());
                            }

                            // the non-primary base methods always need an explicit method override
                            subsequent = true;
                        }
                    }
                    // if we're overriding java.lang.Object.finalize we need to emit a stub to override System.Object.Finalize,
                    // or if we're subclassing a non-Java class that has a Finalize method, we need a new Finalize override
                    if (needFinalize)
                    {
                        var finalizeName = baseFinalize.Name;
                        var mwClash = wrapper.GetMethodWrapper(finalizeName, StringConstants.SIG_VOID, true);
                        if (mwClash != null && mwClash.GetMethod() != baseFinalize)
                            finalizeName = "__<Finalize>";

                        var attr = MethodAttributes.HideBySig | MethodAttributes.Virtual;
                        attr |= baseFinalize.IsPublic ? MethodAttributes.Public : MethodAttributes.Family;
                        if (m.IsFinal)
                            attr |= MethodAttributes.Final;

                        finalizeMethod = typeBuilder.DefineMethod(finalizeName, attr, CallingConventions.Standard, wrapper.Context.Types.Void, Type.EmptyTypes);
                        if (finalizeName != baseFinalize.Name)
                            typeBuilder.DefineMethodOverride(finalizeMethod, baseFinalize);

                        wrapper.Context.AttributeHelper.HideFromJava(finalizeMethod);

                        var ilgen = wrapper.Context.CodeEmitterFactory.Create(finalizeMethod);
                        ilgen.EmitLdarg(0);
                        ilgen.Emit(OpCodes.Call, wrapper.Context.ByteCodeHelperMethods.SkipFinalizerOf);
                        var skip = ilgen.DefineLabel();
                        ilgen.EmitBrtrue(skip);

                        if (needDispatch)
                        {
                            ilgen.BeginExceptionBlock();
                            ilgen.Emit(OpCodes.Ldarg_0);
                            ilgen.Emit(OpCodes.Callvirt, mb);
                            ilgen.EmitLeave(skip);
                            ilgen.BeginCatchBlock(wrapper.Context.Types.Object);
                            ilgen.EmitLeave(skip);
                            ilgen.EndExceptionBlock();
                        }
                        else
                        {
                            ilgen.Emit(OpCodes.Ldarg_0);
                            ilgen.Emit(OpCodes.Call, baseFinalize);
                        }

                        ilgen.MarkLabel(skip);
                        ilgen.Emit(OpCodes.Ret);
                        ilgen.DoEmit();
                    }
#if IMPORTER
                    if (classFile.Methods[index].AnnotationDefault != null)
                    {
                        CustomAttributeBuilder cab = new CustomAttributeBuilder(wrapper.Context.Resolver.ResolveRuntimeType("IKVM.Attributes.AnnotationDefaultAttribute").GetConstructor(new Type[] { wrapper.Context.Types.Object }), new object[] { AnnotationDefaultAttribute.Escape(classFile.Methods[index].AnnotationDefault) });
                        mb.SetCustomAttribute(cab);
                    }
#endif 
                }

                // method is a synchronized method
                if ((methods[index].Modifiers & (Modifiers.Synchronized | Modifiers.Static)) == Modifiers.Synchronized)
                    mb.SetImplementationFlags(mb.GetMethodImplementationFlags() | MethodImplAttributes.Synchronized);

                // java method specifies to force inline, the best we can do is set aggressive inlining
                if (classFile.Methods[index].IsForceInline)
                    mb.SetImplementationFlags(mb.GetMethodImplementationFlags() | MethodImplAttributes.AggressiveInlining);

                if (classFile.Methods[index].IsLambdaFormCompiled || classFile.Methods[index].IsLambdaFormHidden)
                {
                    var flags = HideFromJavaFlags.None;
                    if (classFile.Methods[index].IsLambdaFormCompiled)
                        flags |= HideFromJavaFlags.StackWalk;
                    if (classFile.Methods[index].IsLambdaFormHidden)
                        flags |= HideFromJavaFlags.StackTrace;

                    wrapper.Context.AttributeHelper.HideFromJava(mb, flags);
                }

                if (classFile.IsInterface && methods[index].IsVirtual && !methods[index].IsAbstract)
                {
                    if (wrapper.IsGhost)
                    {
                        RuntimeDefaultInterfaceJavaMethod.SetImpl(methods[index], methods[index].GetDefineMethodHelper().DefineMethod(wrapper.GetClassLoader().GetTypeWrapperFactory(),
                            typeBuilder, NamePrefix.DefaultMethod + mb.Name, MethodAttributes.Public | MethodAttributes.SpecialName,
                            null, false));
                    }
                    else
                    {
                        RuntimeDefaultInterfaceJavaMethod.SetImpl(methods[index], methods[index].GetDefineMethodHelper().DefineMethod(wrapper.GetClassLoader().GetTypeWrapperFactory(),
                            typeBuilder, NamePrefix.DefaultMethod + mb.Name, MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.SpecialName,
                            typeBuilder, false));
                    }
                }

                return mb;
            }

            private static MethodAttributes GetMethodAccess(RuntimeJavaMethod mw)
            {
                switch (mw.Modifiers & Modifiers.AccessMask)
                {
                    case Modifiers.Private:
                        return MethodAttributes.Private;
                    case Modifiers.Protected:
                        return MethodAttributes.FamORAssem;
                    case Modifiers.Public:
                        return MethodAttributes.Public;
                    default:
                        return MethodAttributes.Assembly;
                }
            }

#if IMPORTER

            // The classic example of an access bridge is StringBuilder.length(), the JDK 6 compiler
            // generates this to work around a reflection problem (which otherwise wouldn't surface the
            // length() method, because it is defined in the non-public base class AbstractStringBuilder.)
            private static bool IsAccessBridge(ClassFile classFile, ClassFile.Method m)
            {
                // HACK this is a pretty gross hack
                // We look at the method body to figure out if the bridge method calls another method with the exact
                // same name/signature and if that is the case, we assume that it is an access bridge.
                // This code is based on the javac algorithm in addBridgeIfNeeded(...) in com/sun/tools/javac/comp/TransTypes.java.
                if ((m.Modifiers & (Modifiers.Abstract | Modifiers.Native | Modifiers.Public | Modifiers.Bridge)) == (Modifiers.Public | Modifiers.Bridge))
                {
                    foreach (var instr in m.Instructions)
                    {
                        if (instr.NormalizedOpCode == NormalizedByteCode.__invokespecial)
                        {
                            var cpi = classFile.SafeGetMethodref(new MethodrefConstantHandle((ushort)instr.Arg1));
                            return cpi != null && cpi.Name == m.Name && cpi.Signature == m.Signature;
                        }
                    }
                }

                return false;
            }

#endif // IMPORTER

            internal override Type Type
            {
                get
                {
                    return typeBuilder;
                }
            }

            internal override string GetGenericSignature()
            {
                Debug.Fail("Unreachable code");
                return null;
            }

            internal override string[] GetEnclosingMethod()
            {
                Debug.Fail("Unreachable code");
                return null;
            }

            internal override string GetGenericMethodSignature(int index)
            {
                Debug.Fail("Unreachable code");
                return null;
            }

            internal override string GetGenericFieldSignature(int index)
            {
                Debug.Fail("Unreachable code");
                return null;
            }

            internal override object[] GetDeclaredAnnotations()
            {
                Debug.Fail("Unreachable code");
                return null;
            }

            internal override object GetMethodDefaultValue(int index)
            {
                Debug.Fail("Unreachable code");
                return null;
            }

            internal override object[] GetMethodAnnotations(int index)
            {
                Debug.Fail("Unreachable code");
                return null;
            }

            internal override object[][] GetParameterAnnotations(int index)
            {
                Debug.Fail("Unreachable code");
                return null;
            }

            internal override MethodParametersEntry[] GetMethodParameters(int index)
            {
                Debug.Fail("Unreachable code");
                return null;
            }

            internal override object[] GetFieldAnnotations(int index)
            {
                Debug.Fail("Unreachable code");
                return null;
            }

            internal override MethodInfo GetFinalizeMethod()
            {
                return finalizeMethod;
            }

            internal override object[] GetConstantPool()
            {
                Debug.Fail("Unreachable code");
                return null;
            }

            internal override byte[] GetRawTypeAnnotations()
            {
                Debug.Fail("Unreachable code");
                return null;
            }

            internal override byte[] GetMethodRawTypeAnnotations(int index)
            {
                Debug.Fail("Unreachable code");
                return null;
            }

            internal override byte[] GetFieldRawTypeAnnotations(int index)
            {
                Debug.Fail("Unreachable code");
                return null;
            }

            internal override RuntimeJavaType Host
            {
                get { return host; }
            }

        }

    }

}