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

#if IMPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using IKVM.Tools.Importer;

using Type = IKVM.Reflection.Type;
using DynamicOrAotTypeWrapper = IKVM.Tools.Importer.AotTypeWrapper;
using ProtectionDomain = System.Object;
#else
using System.Reflection;
using System.Reflection.Emit;

using DynamicOrAotTypeWrapper = IKVM.Internal.DynamicTypeWrapper;
#endif

namespace IKVM.Internal
{

#if IMPORTER
    abstract partial class DynamicTypeWrapper : TypeWrapper
#else
#pragma warning disable 628 // don't complain about protected members in sealed type
    sealed partial class DynamicTypeWrapper
#endif
    {
        private sealed class JavaTypeImpl : DynamicImpl
        {
            private readonly TypeWrapper host;
            private readonly ClassFile classFile;
            private readonly DynamicOrAotTypeWrapper wrapper;
            private TypeBuilder typeBuilder;
            private MethodWrapper[] methods;
            private MethodWrapper[][] baseMethods;
            private FieldWrapper[] fields;
            private FinishedTypeImpl finishedType;
            private bool finishInProgress;
            private MethodBuilder clinitMethod;
            private MethodBuilder finalizeMethod;
            private int recursionCount;
#if IMPORTER
            private DynamicTypeWrapper enclosingClassWrapper;
            private AnnotationBuilder annotationBuilder;
            private TypeBuilder enumBuilder;
            private TypeBuilder privateInterfaceMethods;
            private Dictionary<string, TypeWrapper> nestedTypeNames;	// only keys are used, values are always null
#endif

            internal JavaTypeImpl(TypeWrapper host, ClassFile f, DynamicTypeWrapper wrapper)
            {
                Tracer.Info(Tracer.Compiler, "constructing JavaTypeImpl for " + f.Name);
                this.host = host;
                this.classFile = f;
                this.wrapper = (DynamicOrAotTypeWrapper)wrapper;
            }

            internal void CreateStep1()
            {
                // process all methods (needs to be done first, because property fields depend on being able to lookup the accessor methods)
                bool hasclinit = wrapper.BaseTypeWrapper == null ? false : wrapper.BaseTypeWrapper.HasStaticInitializer;
                methods = new MethodWrapper[classFile.Methods.Length];
                baseMethods = new MethodWrapper[classFile.Methods.Length][];
                for (int i = 0; i < methods.Length; i++)
                {
                    MemberFlags flags = MemberFlags.None;
                    ClassFile.Method m = classFile.Methods[i];
                    if (m.IsClassInitializer)
                    {
#if IMPORTER
                        bool noop;
                        if (IsSideEffectFreeStaticInitializerOrNoop(m, out noop))
                        {
                            if (noop)
                            {
                                flags |= MemberFlags.NoOp;
                            }
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
                    {
                        flags |= MemberFlags.InternalAccess;
                    }
#if IMPORTER
                    if (m.IsCallerSensitive && SupportsCallerID(m))
                    {
                        flags |= MemberFlags.CallerID;
                    }
#endif
                    if (wrapper.IsGhost && m.IsVirtual)
                    {
                        // note that a GhostMethodWrapper can also represent a default interface method
                        methods[i] = new GhostMethodWrapper(wrapper, m.Name, m.Signature, null, null, null, null, m.Modifiers, flags);
                    }
                    else if (m.IsConstructor && wrapper.IsDelegate)
                    {
                        methods[i] = new DelegateConstructorMethodWrapper(wrapper, m);
                    }
                    else if (classFile.IsInterface && !m.IsStatic && !m.IsPublic)
                    {
                        // we can't use callvirt to call interface private instance methods (because we have to compile them as static methods,
                        // since the CLR doesn't support interface instance methods), so need a special MethodWrapper
                        methods[i] = new PrivateInterfaceMethodWrapper(wrapper, m.Name, m.Signature, null, null, null, m.Modifiers, flags);
                    }
                    else if (classFile.IsInterface && m.IsVirtual && !m.IsAbstract)
                    {
                        // note that a GhostMethodWrapper can also represent a default interface method
                        methods[i] = new DefaultInterfaceMethodWrapper(wrapper, m.Name, m.Signature, null, null, null, null, m.Modifiers, flags);
                    }
                    else
                    {
                        if (!classFile.IsInterface && m.IsVirtual)
                        {
                            bool explicitOverride;
                            baseMethods[i] = FindBaseMethods(m, out explicitOverride);
                            if (explicitOverride)
                            {
                                flags |= MemberFlags.ExplicitOverride;
                            }
                        }
                        methods[i] = new TypicalMethodWrapper(wrapper, m.Name, m.Signature, null, null, null, m.Modifiers, flags);
                    }
                }
                if (hasclinit)
                {
                    wrapper.SetHasStaticInitializer();
                }
                if (!wrapper.IsInterface || wrapper.IsPublic)
                {
                    List<MethodWrapper> methodsArray = new List<MethodWrapper>(methods);
                    List<MethodWrapper[]> baseMethodsArray = new List<MethodWrapper[]>(baseMethods);
                    AddMirandaMethods(methodsArray, baseMethodsArray, wrapper);
                    methods = methodsArray.ToArray();
                    baseMethods = baseMethodsArray.ToArray();
                }
                if (!wrapper.IsInterface)
                {
                    AddDelegateInvokeStubs(wrapper, ref methods);
                }
                wrapper.SetMethods(methods);

                fields = new FieldWrapper[classFile.Fields.Length];
                for (int i = 0; i < fields.Length; i++)
                {
                    ClassFile.Field fld = classFile.Fields[i];
                    if (fld.IsStaticFinalConstant)
                    {
                        TypeWrapper fieldType = null;
#if !IMPORTER
                        fieldType = ClassLoaderWrapper.GetBootstrapClassLoader().FieldTypeWrapperFromSig(fld.Signature, LoadMode.LoadOrThrow);
#endif
                        fields[i] = new ConstantFieldWrapper(wrapper, fieldType, fld.Name, fld.Signature, fld.Modifiers, null, fld.ConstantValue, MemberFlags.None);
                    }
                    else if (fld.IsProperty)
                    {
                        fields[i] = new DynamicPropertyFieldWrapper(wrapper, fld);
                    }
                    else
                    {
                        fields[i] = FieldWrapper.Create(wrapper, null, null, fld.Name, fld.Signature, new ExModifiers(fld.Modifiers, fld.IsInternal));
                    }
                }
#if IMPORTER
                wrapper.AddMapXmlFields(ref fields);
#endif
                wrapper.SetFields(fields);
            }

#if IMPORTER
            private bool SupportsCallerID(ClassFile.Method method)
            {
                if ((classFile.Name == "sun.reflect.Reflection" && method.Name == "getCallerClass")
                    || (classFile.Name == "java.lang.SecurityManager" && method.Name == "checkMemberAccess"))
                {
                    // ignore CallerSensitive on methods that don't need CallerID parameter
                    return false;
                }
                else if (method.IsStatic)
                {
                    return true;
                }
                else if ((classFile.IsFinal || classFile.Name == "java.lang.Runtime" || classFile.Name == "java.io.ObjectStreamClass")
                    && wrapper.BaseTypeWrapper.GetMethodWrapper(method.Name, method.Signature, true) == null
                    && !HasInterfaceMethod(wrapper, method.Name, method.Signature))
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
                    StaticCompiler.IssueMessage(Message.CallerSensitiveOnUnsupportedMethod, classFile.Name, method.Name, method.Signature);
                    return false;
                }
            }

            private static bool HasInterfaceMethod(TypeWrapper tw, string name, string signature)
            {
                for (; tw != null; tw = tw.BaseTypeWrapper)
                {
                    foreach (TypeWrapper iface in tw.Interfaces)
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
                bool hasclinit = wrapper.HasStaticInitializer;
                string mangledTypeName = wrapper.classLoader.GetTypeWrapperFactory().AllocMangledName(wrapper);
                ClassFile f = classFile;
                try
                {
                    TypeAttributes typeAttribs = 0;
                    if (f.IsAbstract)
                    {
                        typeAttribs |= TypeAttributes.Abstract;
                    }
                    if (f.IsFinal)
                    {
                        typeAttribs |= TypeAttributes.Sealed;
                    }
                    if (!hasclinit)
                    {
                        typeAttribs |= TypeAttributes.BeforeFieldInit;
                    }
#if IMPORTER
                    bool cantNest = false;
                    bool setModifiers = false;
                    TypeBuilder enclosing = null;
                    string enclosingClassName = null;
                    // we only compile inner classes as nested types in the static compiler, because it has a higher cost
                    // and doesn't buy us anything in dynamic mode (and if fact, due to an FXBUG it would make handling
                    // the TypeResolve event very hard)
                    ClassFile.InnerClass outerClass = getOuterClass();
                    if (outerClass.outerClass != 0)
                    {
                        enclosingClassName = classFile.GetConstantPoolClass(outerClass.outerClass);
                    }
                    else if (f.EnclosingMethod != null)
                    {
                        enclosingClassName = f.EnclosingMethod[0];
                    }
                    if (enclosingClassName != null)
                    {
                        if (!CheckInnerOuterNames(f.Name, enclosingClassName))
                        {
                            Tracer.Warning(Tracer.Compiler, "Incorrect {0} attribute on {1}", outerClass.outerClass != 0 ? "InnerClasses" : "EnclosingMethod", f.Name);
                        }
                        else
                        {
                            try
                            {
                                enclosingClassWrapper = wrapper.classLoader.LoadClassByDottedNameFast(enclosingClassName) as DynamicTypeWrapper;
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
                                JavaTypeImpl oimpl = enclosingClassWrapper.impl as JavaTypeImpl;
                                if (oimpl != null && enclosingClassWrapper.GetClassLoader() == wrapper.GetClassLoader())
                                {
                                    ClassFile outerClassFile = oimpl.classFile;
                                    ClassFile.InnerClass[] outerInnerClasses = outerClassFile.InnerClasses;
                                    if (outerInnerClasses == null)
                                    {
                                        enclosingClassWrapper = null;
                                    }
                                    else
                                    {
                                        bool ok = false;
                                        for (int i = 0; i < outerInnerClasses.Length; i++)
                                        {
                                            if (((outerInnerClasses[i].outerClass != 0 && outerClassFile.GetConstantPoolClass(outerInnerClasses[i].outerClass) == outerClassFile.Name)
                                                    || (outerInnerClasses[i].outerClass == 0 && outerClass.outerClass == 0))
                                                && outerInnerClasses[i].innerClass != 0
                                                && outerClassFile.GetConstantPoolClass(outerInnerClasses[i].innerClass) == f.Name)
                                            {
                                                ok = true;
                                                break;
                                            }
                                        }
                                        if (!ok)
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
                                    if (outerClass.outerClass == 0)
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
                    wrapper.GetClassLoader().SetWrapperForType(typeBuilder, wrapper);
                    if (outerClass.outerClass != 0)
                    {
                        if (enclosing != null && cantNest)
                        {
                            AttributeHelper.SetNonNestedInnerClass(enclosing, f.Name);
                        }
                        if (enclosing == null || cantNest)
                        {
                            AttributeHelper.SetNonNestedOuterClass(typeBuilder, enclosingClassName);
                        }
                    }
                    if (classFile.InnerClasses != null)
                    {
                        foreach (ClassFile.InnerClass inner in classFile.InnerClasses)
                        {
                            string name = classFile.GetConstantPoolClass(inner.innerClass);
                            bool exists = false;
                            try
                            {
                                exists = wrapper.GetClassLoader().LoadClassByDottedNameFast(name) != null;
                            }
                            catch (RetargetableJavaException) { }
                            if (!exists)
                            {
                                AttributeHelper.SetNonNestedInnerClass(typeBuilder, name);
                            }
                        }
                    }
                    if (typeBuilder.FullName != wrapper.Name
                        && wrapper.Name.Replace('$', '+') != typeBuilder.FullName)
                    {
                        wrapper.classLoader.AddNameMapping(wrapper.Name, typeBuilder.FullName);
                    }
                    if (f.IsAnnotation && Annotation.HasRetentionPolicyRuntime(f.Annotations))
                    {
                        annotationBuilder = new AnnotationBuilder(this, enclosing);
                        wrapper.SetAnnotation(annotationBuilder);
                    }
                    // For Java 5 Enum types, we generate a nested .NET enum.
                    // This is primarily to support annotations that take enum parameters.
                    if (f.IsEnum && f.IsPublic)
                    {
                        AddCliEnum();
                    }
                    AddInnerClassAttribute(enclosing != null, outerClass.innerClass != 0, mangledTypeName, outerClass.accessFlags);
                    if (classFile.DeprecatedAttribute && !Annotation.HasObsoleteAttribute(classFile.Annotations))
                    {
                        AttributeHelper.SetDeprecatedAttribute(typeBuilder);
                    }
                    if (classFile.GenericSignature != null)
                    {
                        AttributeHelper.SetSignatureAttribute(typeBuilder, classFile.GenericSignature);
                    }
                    if (classFile.EnclosingMethod != null)
                    {
                        if (outerClass.outerClass == 0 && enclosing != null && !cantNest)
                        {
                            // we don't need to record the enclosing type, if we're compiling the current type as a nested type because of the EnclosingMethod attribute
                            AttributeHelper.SetEnclosingMethodAttribute(typeBuilder, null, classFile.EnclosingMethod[1], classFile.EnclosingMethod[2]);
                        }
                        else
                        {
                            AttributeHelper.SetEnclosingMethodAttribute(typeBuilder, classFile.EnclosingMethod[0], classFile.EnclosingMethod[1], classFile.EnclosingMethod[2]);
                        }
                    }
                    if (classFile.RuntimeVisibleTypeAnnotations != null)
                    {
                        AttributeHelper.SetRuntimeVisibleTypeAnnotationsAttribute(typeBuilder, classFile.RuntimeVisibleTypeAnnotations);
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
                                AttributeHelper.SetSourceFile(typeBuilder, f.SourceFileAttribute);
                            }
                        }
                        else
                        {
                            AttributeHelper.SetSourceFile(typeBuilder, null);
                        }
                    }
                    // NOTE in Whidbey we can (and should) use CompilerGeneratedAttribute to mark Synthetic types
                    if (setModifiers || classFile.IsInternal || (classFile.Modifiers & (Modifiers.Synthetic | Modifiers.Annotation | Modifiers.Enum)) != 0)
                    {
                        AttributeHelper.SetModifiers(typeBuilder, classFile.Modifiers, classFile.IsInternal);
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
                        foreach (FieldWrapper fw in fields)
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

                if ((isInnerClass && CompiledTypeWrapper.PredictReflectiveModifiers(wrapper) != innerClassFlags) || name != null)
                {
                    // HACK we abuse the InnerClassAttribute to record to real name for non-inner classes as well
                    AttributeHelper.SetInnerClass(typeBuilder, name, isInnerClass ? innerClassFlags : wrapper.Modifiers);
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
                enumBuilder = typeBuilder.DefineNestedType(name, TypeAttributes.Class | TypeAttributes.Sealed | TypeAttributes.NestedPublic | TypeAttributes.Serializable, Types.Enum);
                AttributeHelper.HideFromJava(enumBuilder);
                enumBuilder.DefineField("value__", Types.Int32, FieldAttributes.Public | FieldAttributes.SpecialName | FieldAttributes.RTSpecialName);
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

            sealed class DelegateConstructorMethodWrapper : MethodWrapper
            {

                MethodBuilder constructor;
                MethodInfo invoke;

                /// <summary>
                /// Initializes a new instance.
                /// </summary>
                /// <param name="tw"></param>
                /// <param name="m"></param>
                internal DelegateConstructorMethodWrapper(DynamicTypeWrapper tw, ClassFile.Method m) :
                    base(tw, m.Name, m.Signature, null, null, null, m.Modifiers, MemberFlags.None)
                {

                }

                internal void DoLink(TypeBuilder typeBuilder)
                {
                    var attribs = MethodAttributes.HideBySig | MethodAttributes.Public;
                    constructor = ReflectUtil.DefineConstructor(typeBuilder, attribs, new Type[] { Types.Object, Types.IntPtr });
                    constructor.SetImplementationFlags(MethodImplAttributes.Runtime);
                    var mw = GetParameters()[0].GetMethods()[0];
                    mw.Link();
                    invoke = (MethodInfo)mw.GetMethod();
                }

                internal override void EmitNewobj(CodeEmitter ilgen)
                {
                    ilgen.Emit(OpCodes.Dup);
                    ilgen.Emit(OpCodes.Ldvirtftn, invoke);
                    ilgen.Emit(OpCodes.Newobj, constructor);
                }

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
            private ClassFile.InnerClass getOuterClass()
            {
                ClassFile.InnerClass[] innerClasses = classFile.InnerClasses;
                if (innerClasses != null)
                {
                    for (int j = 0; j < innerClasses.Length; j++)
                    {
                        if (innerClasses[j].innerClass != 0
                            && classFile.GetConstantPoolClass(innerClasses[j].innerClass) == classFile.Name)
                        {
                            return innerClasses[j];
                        }
                    }
                }
                return new ClassFile.InnerClass();
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
                    new MethodAnalyzer(null, wrapper, null, classFile, m, wrapper.classLoader);
                    return true;
                }
                catch (VerifyError)
                {
                    return false;
                }
            }
#endif // IMPORTER

            private MethodWrapper GetMethodWrapperDuringCtor(TypeWrapper lookup, IList<MethodWrapper> methods, string name, string sig)
            {
                if (lookup == wrapper)
                {
                    foreach (MethodWrapper mw in methods)
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

            private void AddMirandaMethods(List<MethodWrapper> methods, List<MethodWrapper[]> baseMethods, TypeWrapper tw)
            {
                foreach (TypeWrapper iface in tw.Interfaces)
                {
                    if (iface.IsPublic && this.wrapper.IsInterface)
                    {
                        // for interfaces, we only need miranda methods for non-public interfaces that we extend
                        continue;
                    }
                    AddMirandaMethods(methods, baseMethods, iface);
                    foreach (MethodWrapper ifmethod in iface.GetMethods())
                    {
                        // skip <clinit> and non-virtual interface methods introduced in Java 8
                        if (ifmethod.IsVirtual)
                        {
                            TypeWrapper lookup = wrapper;
                            while (lookup != null)
                            {
                                MethodWrapper mw = GetMethodWrapperDuringCtor(lookup, methods, ifmethod.Name, ifmethod.Signature);
                                if (mw == null || (mw.IsMirandaMethod && mw.DeclaringType != wrapper))
                                {
                                    mw = MirandaMethodWrapper.Create(wrapper, ifmethod);
                                    methods.Add(mw);
                                    baseMethods.Add(new MethodWrapper[] { ifmethod });
                                    break;
                                }
                                if (mw.IsMirandaMethod && mw.DeclaringType == wrapper)
                                {
                                    methods[methods.IndexOf(mw)] = ((MirandaMethodWrapper)mw).Update(ifmethod);
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

            private void AddDelegateInvokeStubs(TypeWrapper tw, ref MethodWrapper[] methods)
            {
                foreach (TypeWrapper iface in tw.Interfaces)
                {
                    if (iface.IsFakeNestedType
                        && iface.GetMethods().Length == 1
                        && iface.GetMethods()[0].IsDelegateInvokeWithByRefParameter)
                    {
                        MethodWrapper mw = new DelegateInvokeStubMethodWrapper(wrapper, iface.DeclaringTypeWrapper.TypeAsBaseType, iface.GetMethods()[0].Signature);
                        if (GetMethodWrapperDuringCtor(wrapper, methods, mw.Name, mw.Signature) == null)
                        {
                            methods = ArrayUtil.Concat(methods, mw);
                        }
                    }
                    AddDelegateInvokeStubs(iface, ref methods);
                }
            }

            private sealed class DelegateInvokeStubMethodWrapper : MethodWrapper
            {
                private readonly Type delegateType;

                internal DelegateInvokeStubMethodWrapper(TypeWrapper declaringType, Type delegateType, string sig)
                    : base(declaringType, DotNetTypeWrapper.GetDelegateInvokeStubName(delegateType), sig, null, null, null, Modifiers.Public | Modifiers.Final, MemberFlags.HideFromReflection)
                {
                    this.delegateType = delegateType;
                }

                internal MethodInfo DoLink(TypeBuilder tb)
                {
                    MethodWrapper mw = this.DeclaringType.GetMethodWrapper("Invoke", this.Signature, true);

                    MethodInfo invoke = delegateType.GetMethod("Invoke");
                    ParameterInfo[] parameters = invoke.GetParameters();
                    Type[] parameterTypes = new Type[parameters.Length];
                    for (int i = 0; i < parameterTypes.Length; i++)
                    {
                        parameterTypes[i] = parameters[i].ParameterType;
                    }
                    MethodBuilder mb = tb.DefineMethod(this.Name, MethodAttributes.Public, invoke.ReturnType, parameterTypes);
                    AttributeHelper.HideFromReflection(mb);
                    CodeEmitter ilgen = CodeEmitter.Create(mb);
                    if (mw == null || mw.IsStatic || !mw.IsPublic)
                    {
                        ilgen.EmitThrow(mw == null || mw.IsStatic ? "java.lang.AbstractMethodError" : "java.lang.IllegalAccessError", DeclaringType.Name + ".Invoke" + Signature);
                        ilgen.DoEmit();
                        return mb;
                    }
                    CodeEmitterLocal[] byrefs = new CodeEmitterLocal[parameters.Length];
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        if (parameters[i].ParameterType.IsByRef)
                        {
                            Type elemType = parameters[i].ParameterType.GetElementType();
                            CodeEmitterLocal local = ilgen.DeclareLocal(ArrayTypeWrapper.MakeArrayType(elemType, 1));
                            byrefs[i] = local;
                            ilgen.Emit(OpCodes.Ldc_I4_1);
                            ilgen.Emit(OpCodes.Newarr, elemType);
                            ilgen.Emit(OpCodes.Stloc, local);
                            ilgen.Emit(OpCodes.Ldloc, local);
                            ilgen.Emit(OpCodes.Ldc_I4_0);
                            ilgen.EmitLdarg(i + 1);
                            ilgen.Emit(OpCodes.Ldobj, elemType);
                            ilgen.Emit(OpCodes.Stelem, elemType);
                        }
                    }
                    ilgen.BeginExceptionBlock();
                    ilgen.Emit(OpCodes.Ldarg_0);
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        if (byrefs[i] != null)
                        {
                            ilgen.Emit(OpCodes.Ldloc, byrefs[i]);
                        }
                        else
                        {
                            ilgen.EmitLdarg(i + 1);
                        }
                    }
                    mw.Link();
                    mw.EmitCallvirt(ilgen);
                    CodeEmitterLocal returnValue = null;
                    if (mw.ReturnType != PrimitiveTypeWrapper.VOID)
                    {
                        returnValue = ilgen.DeclareLocal(mw.ReturnType.TypeAsSignatureType);
                        ilgen.Emit(OpCodes.Stloc, returnValue);
                    }
                    CodeEmitterLabel exit = ilgen.DefineLabel();
                    ilgen.EmitLeave(exit);
                    ilgen.BeginFinallyBlock();
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        if (byrefs[i] != null)
                        {
                            Type elemType = byrefs[i].LocalType.GetElementType();
                            ilgen.EmitLdarg(i + 1);
                            ilgen.Emit(OpCodes.Ldloc, byrefs[i]);
                            ilgen.Emit(OpCodes.Ldc_I4_0);
                            ilgen.Emit(OpCodes.Ldelem, elemType);
                            ilgen.Emit(OpCodes.Stobj, elemType);
                        }
                    }
                    ilgen.Emit(OpCodes.Endfinally);
                    ilgen.EndExceptionBlock();
                    ilgen.MarkLabel(exit);
                    if (returnValue != null)
                    {
                        ilgen.Emit(OpCodes.Ldloc, returnValue);
                    }
                    ilgen.Emit(OpCodes.Ret);
                    ilgen.DoEmit();
                    return mb;
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
                if (nestedTypeNames == null)
                {
                    nestedTypeNames = new Dictionary<string, TypeWrapper>();
                }
                return DynamicClassLoader.TypeNameMangleImpl(nestedTypeNames, inner.Substring(outer.Length + 1), null);
            }
#endif // IMPORTER

            private int GetMethodIndex(MethodWrapper mw)
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

            private static void CheckLoaderConstraints(MethodWrapper mw, MethodWrapper baseMethod)
            {
                if (mw.ReturnType != baseMethod.ReturnType)
                {
                    if (mw.ReturnType.IsUnloadable || baseMethod.ReturnType.IsUnloadable)
                    {
                        // unloadable types can never cause a loader constraint violation
                        if (mw.ReturnType.IsUnloadable && baseMethod.ReturnType.IsUnloadable)
                        {
                            ((UnloadableTypeWrapper)mw.ReturnType).SetCustomModifier(((UnloadableTypeWrapper)baseMethod.ReturnType).CustomModifier);
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
                TypeWrapper[] here = mw.GetParameters();
                TypeWrapper[] there = baseMethod.GetParameters();
                for (int i = 0; i < here.Length; i++)
                {
                    if (here[i] != there[i])
                    {
                        if (here[i].IsUnloadable || there[i].IsUnloadable)
                        {
                            // unloadable types can never cause a loader constraint violation
                            if (here[i].IsUnloadable && there[i].IsUnloadable)
                            {
                                ((UnloadableTypeWrapper)here[i]).SetCustomModifier(((UnloadableTypeWrapper)there[i]).CustomModifier);
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

            private int GetFieldIndex(FieldWrapper fw)
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

            internal override FieldInfo LinkField(FieldWrapper fw)
            {
                if (fw is DynamicPropertyFieldWrapper)
                {
                    ((DynamicPropertyFieldWrapper)fw).DoLink(typeBuilder);
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
                    CustomAttributeBuilder transientAttrib = new CustomAttributeBuilder(JVM.Import(typeof(NonSerializedAttribute)).GetConstructor(Type.EmptyTypes), new object[0]);
                    field.SetCustomAttribute(transientAttrib);
                }
#if IMPORTER
                {
                    // if the Java modifiers cannot be expressed in .NET, we emit the Modifiers attribute to store
                    // the Java modifiers
                    if (setModifiers)
                    {
                        AttributeHelper.SetModifiers(field, fld.Modifiers, fld.IsInternal);
                    }
                    if (fld.DeprecatedAttribute && !Annotation.HasObsoleteAttribute(fld.Annotations))
                    {
                        AttributeHelper.SetDeprecatedAttribute(field);
                    }
                    if (fld.GenericSignature != null)
                    {
                        AttributeHelper.SetSignatureAttribute(field, fld.GenericSignature);
                    }
                    if (fld.RuntimeVisibleTypeAnnotations != null)
                    {
                        AttributeHelper.SetRuntimeVisibleTypeAnnotationsAttribute(field, fld.RuntimeVisibleTypeAnnotations);
                    }
                }
#endif // IMPORTER
                return field;
            }

            private FieldBuilder DefineField(string name, TypeWrapper tw, FieldAttributes attribs, bool isVolatile)
            {
                Type[] modreq = isVolatile ? new Type[] { Types.IsVolatile } : Type.EmptyTypes;
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
                TypeWrapper baseTypeWrapper = wrapper.BaseTypeWrapper;
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
                foreach (TypeWrapper iface in wrapper.interfaces)
                {
                    iface.Finish();
                    iface.LinkAll();
                }
                // make sure all classes are loaded, before we start finishing the type. During finishing, we
                // may not run any Java code, because that might result in a request to finish the type that we
                // are in the process of finishing, and this would be a problem.
                // Prevent infinity recursion for broken class loaders by keeping a recursion count and falling
                // back to late binding if we recurse more than twice.
                LoadMode mode = System.Threading.Interlocked.Increment(ref recursionCount) > 2 || (JVM.DisableEagerClassLoading && wrapper.Name != "sun.reflect.misc.Trampoline")
                    ? LoadMode.ReturnUnloadable
                    : LoadMode.Link;
                try
                {
                    classFile.Link(wrapper, mode);
                    for (int i = 0; i < fields.Length; i++)
                    {
                        fields[i].Link(mode);
                    }
                    for (int i = 0; i < methods.Length; i++)
                    {
                        methods[i].Link(mode);
                    }
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

            private FinishedTypeImpl FinishCore()
            {
                // it is possible that the loading of the referenced classes triggered a finish of us,
                // if that happens, we just return
                if (finishedType != null)
                {
                    return finishedType;
                }
                if (finishInProgress)
                {
                    throw new InvalidOperationException("Recursive finish attempt for " + wrapper.Name);
                }
                finishInProgress = true;
                Tracer.Info(Tracer.Compiler, "Finishing: {0}", wrapper.Name);
                Profiler.Enter("JavaTypeImpl.Finish.Core");
                try
                {
                    TypeWrapper declaringTypeWrapper = null;
                    TypeWrapper[] innerClassesTypeWrappers = TypeWrapper.EmptyArray;
                    // if we're an inner class, we need to attach an InnerClass attribute
                    ClassFile.InnerClass[] innerclasses = classFile.InnerClasses;
                    if (innerclasses != null)
                    {
                        // TODO consider not pre-computing innerClassesTypeWrappers and declaringTypeWrapper here
                        List<TypeWrapper> wrappers = new List<TypeWrapper>();
                        for (int i = 0; i < innerclasses.Length; i++)
                        {
                            if (innerclasses[i].innerClass != 0 && innerclasses[i].outerClass != 0)
                            {
                                if (classFile.GetConstantPoolClassType(innerclasses[i].outerClass) == wrapper)
                                {
                                    wrappers.Add(classFile.GetConstantPoolClassType(innerclasses[i].innerClass));
                                }
                                if (classFile.GetConstantPoolClassType(innerclasses[i].innerClass) == wrapper)
                                {
                                    declaringTypeWrapper = classFile.GetConstantPoolClassType(innerclasses[i].outerClass);
                                }
                            }
                        }
                        innerClassesTypeWrappers = wrappers.ToArray();
#if IMPORTER
                        // before we bake our type, we need to link any inner annotations to allow them to create their attribute type (as a nested type)
                        foreach (TypeWrapper tw in innerClassesTypeWrappers)
                        {
                            DynamicTypeWrapper dtw = tw as DynamicTypeWrapper;
                            if (dtw != null)
                            {
                                JavaTypeImpl impl = dtw.impl as JavaTypeImpl;
                                if (impl != null)
                                {
                                    if (impl.annotationBuilder != null)
                                    {
                                        impl.annotationBuilder.Link();
                                    }
                                }
                            }
                        }
#endif //IMPORTER
                    }
#if IMPORTER
                    if (annotationBuilder != null)
                    {
                        CustomAttributeBuilder cab = new CustomAttributeBuilder(JVM.LoadType(typeof(AnnotationAttributeAttribute)).GetConstructor(new Type[] { Types.String }),
                            new object[] { UnicodeUtil.EscapeInvalidSurrogates(annotationBuilder.AttributeTypeName) });
                        typeBuilder.SetCustomAttribute(cab);
                    }
                    if (!wrapper.IsInterface && wrapper.IsMapUnsafeException)
                    {
                        // mark all exceptions that are unsafe for mapping with a custom attribute,
                        // so that at runtime we can quickly assertain if an exception type can be
                        // caught without filtering
                        AttributeHelper.SetExceptionIsUnsafeForMapping(typeBuilder);
                    }
#endif

                    FinishContext context = new FinishContext(host, classFile, wrapper, typeBuilder);
                    Type type = context.FinishImpl();
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
                    MethodInfo finishedClinitMethod = clinitMethod;
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
                {
                    type = type.Substring(1);
                }
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
                        TypeWrapper tw = wrapper.GetClassLoader().LoadClassByDottedNameFast(type.Substring(1, type.Length - 2));
                        if (tw != null)
                        {
                            if ((tw.Modifiers & Modifiers.Annotation) != 0)
                            {
                                return true;
                            }
                            if ((tw.Modifiers & Modifiers.Enum) != 0)
                            {
                                TypeWrapper enumType = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedNameFast("java.lang.Enum");
                                if (enumType != null && tw.IsSubTypeOf(enumType))
                                {
                                    return true;
                                }
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
                private JavaTypeImpl impl;
                private TypeBuilder outer;
                private TypeBuilder annotationTypeBuilder;
                private TypeBuilder attributeTypeBuilder;
                private MethodBuilder defineConstructor;

                internal AnnotationBuilder(JavaTypeImpl o, TypeBuilder outer)
                {
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

                    TypeWrapper annotationAttributeBaseType = ClassLoaderWrapper.LoadClassCritical("ikvm.internal.AnnotationAttributeBase");

                    // make sure we don't clash with another class name
                    CompilerClassLoader ccl = o.wrapper.classLoader;
                    string name = UnicodeUtil.EscapeInvalidSurrogates(o.classFile.Name);
                    while (!ccl.ReserveName(name + "Attribute"))
                    {
                        name += "_";
                    }

                    TypeAttributes typeAttributes = TypeAttributes.Class | TypeAttributes.Sealed;
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
                        AttributeHelper.SetModifiers(attributeTypeBuilder, Modifiers.Final, false);
                    }
                    // NOTE we "abuse" the InnerClassAttribute to add a custom attribute to name the class "$Proxy[Annotation]" in the Java world
                    int dotindex = o.classFile.Name.LastIndexOf('.') + 1;
                    AttributeHelper.SetInnerClass(attributeTypeBuilder, o.classFile.Name.Substring(0, dotindex) + "$Proxy" + o.classFile.Name.Substring(dotindex), Modifiers.Final);
                    attributeTypeBuilder.AddInterfaceImplementation(o.typeBuilder);
                    AttributeHelper.SetImplementsAttribute(attributeTypeBuilder, new TypeWrapper[] { o.wrapper });

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
                                            attributeUsageAttribute = new CustomAttributeBuilder(JVM.Import(typeof(AttributeUsageAttribute)).GetConstructor(new Type[] { JVM.Import(typeof(AttributeTargets)) }), new object[] { targets });
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

                    defineConstructor = ReflectUtil.DefineConstructor(attributeTypeBuilder, MethodAttributes.Public, new Type[] { JVM.Import(typeof(object[])) });
                    AttributeHelper.SetEditorBrowsableNever(defineConstructor);
                }

                private static Type TypeWrapperToAnnotationParameterType(TypeWrapper tw)
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
                        if (tw == CoreClasses.java.lang.Class.Wrapper)
                        {
                            argType = Types.Type;
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
                            argType = ArrayTypeWrapper.MakeArrayType(argType, 1);
                        }
                        return argType;
                    }
                }

                private static bool IsDotNetEnum(TypeWrapper tw)
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

                private static void EmitSetValueCall(TypeWrapper annotationAttributeBaseType, CodeEmitter ilgen, string name, TypeWrapper tw, int argIndex)
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
                    MethodWrapper setValueMethod = annotationAttributeBaseType.GetMethodWrapper("setValue", "(Ljava.lang.String;Ljava.lang.Object;)V", false);
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
                    TypeWrapper annotationAttributeBaseType = ClassLoaderWrapper.LoadClassCritical("ikvm.internal.AnnotationAttributeBase");
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
                            AttributeHelper.HideFromJava(reqArgConstructor);
                            ilgen = CodeEmitter.Create(reqArgConstructor);
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
                                AttributeHelper.HideFromJava(cb);
                                cb.DefineParameter(1, ParameterAttributes.None, "value");
                                ilgen = CodeEmitter.Create(cb);
                                ilgen.Emit(OpCodes.Ldarg_0);
                                ilgen.Emit(OpCodes.Call, defaultConstructor);
                                EmitSetValueCall(annotationAttributeBaseType, ilgen, "value", o.methods[valueArg].ReturnType, 1);
                                ilgen.Emit(OpCodes.Ret);
                                ilgen.DoEmit();
                            }
                        }
                    }

                    ilgen = CodeEmitter.Create(defaultConstructor);
                    ilgen.Emit(OpCodes.Ldarg_0);
                    o.wrapper.EmitClassLiteral(ilgen);
                    annotationAttributeBaseType.GetMethodWrapper("<init>", "(Ljava.lang.Class;)V", false).EmitCall(ilgen);
                    ilgen.Emit(OpCodes.Ret);
                    ilgen.DoEmit();

                    ilgen = CodeEmitter.Create(defineConstructor);
                    ilgen.Emit(OpCodes.Ldarg_0);
                    ilgen.Emit(OpCodes.Call, defaultConstructor);
                    ilgen.Emit(OpCodes.Ldarg_0);
                    ilgen.Emit(OpCodes.Ldarg_1);
                    annotationAttributeBaseType.GetMethodWrapper("setDefinition", "([Ljava.lang.Object;)V", false).EmitCall(ilgen);
                    ilgen.Emit(OpCodes.Ret);
                    ilgen.DoEmit();

                    MethodWrapper getValueMethod = annotationAttributeBaseType.GetMethodWrapper("getValue", "(Ljava.lang.String;)Ljava.lang.Object;", false);
                    MethodWrapper getByteValueMethod = annotationAttributeBaseType.GetMethodWrapper("getByteValue", "(Ljava.lang.String;)B", false);
                    MethodWrapper getBooleanValueMethod = annotationAttributeBaseType.GetMethodWrapper("getBooleanValue", "(Ljava.lang.String;)Z", false);
                    MethodWrapper getCharValueMethod = annotationAttributeBaseType.GetMethodWrapper("getCharValue", "(Ljava.lang.String;)C", false);
                    MethodWrapper getShortValueMethod = annotationAttributeBaseType.GetMethodWrapper("getShortValue", "(Ljava.lang.String;)S", false);
                    MethodWrapper getIntValueMethod = annotationAttributeBaseType.GetMethodWrapper("getIntValue", "(Ljava.lang.String;)I", false);
                    MethodWrapper getFloatValueMethod = annotationAttributeBaseType.GetMethodWrapper("getFloatValue", "(Ljava.lang.String;)F", false);
                    MethodWrapper getLongValueMethod = annotationAttributeBaseType.GetMethodWrapper("getLongValue", "(Ljava.lang.String;)J", false);
                    MethodWrapper getDoubleValueMethod = annotationAttributeBaseType.GetMethodWrapper("getDoubleValue", "(Ljava.lang.String;)D", false);
                    for (int i = 0; i < o.methods.Length; i++)
                    {
                        // skip <clinit> and non-virtual interface methods introduced in Java 8
                        if (o.methods[i].IsVirtual)
                        {
                            MethodBuilder mb = o.methods[i].GetDefineMethodHelper().DefineMethod(o.wrapper, attributeTypeBuilder, o.methods[i].Name, MethodAttributes.Private | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.NewSlot);
                            attributeTypeBuilder.DefineMethodOverride(mb, (MethodInfo)o.methods[i].GetMethod());
                            ilgen = CodeEmitter.Create(mb);
                            ilgen.Emit(OpCodes.Ldarg_0);
                            ilgen.Emit(OpCodes.Ldstr, o.methods[i].Name);
                            if (o.methods[i].ReturnType.IsPrimitive)
                            {
                                if (o.methods[i].ReturnType == PrimitiveTypeWrapper.BYTE)
                                {
                                    getByteValueMethod.EmitCall(ilgen);
                                }
                                else if (o.methods[i].ReturnType == PrimitiveTypeWrapper.BOOLEAN)
                                {
                                    getBooleanValueMethod.EmitCall(ilgen);
                                }
                                else if (o.methods[i].ReturnType == PrimitiveTypeWrapper.CHAR)
                                {
                                    getCharValueMethod.EmitCall(ilgen);
                                }
                                else if (o.methods[i].ReturnType == PrimitiveTypeWrapper.SHORT)
                                {
                                    getShortValueMethod.EmitCall(ilgen);
                                }
                                else if (o.methods[i].ReturnType == PrimitiveTypeWrapper.INT)
                                {
                                    getIntValueMethod.EmitCall(ilgen);
                                }
                                else if (o.methods[i].ReturnType == PrimitiveTypeWrapper.FLOAT)
                                {
                                    getFloatValueMethod.EmitCall(ilgen);
                                }
                                else if (o.methods[i].ReturnType == PrimitiveTypeWrapper.LONG)
                                {
                                    getLongValueMethod.EmitCall(ilgen);
                                }
                                else if (o.methods[i].ReturnType == PrimitiveTypeWrapper.DOUBLE)
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
                                    AttributeHelper.HideFromJava(pb);
                                    MethodBuilder setter = attributeTypeBuilder.DefineMethod("set_" + o.methods[i].Name, MethodAttributes.Public, Types.Void, new Type[] { argType });
                                    AttributeHelper.HideFromJava(setter);
                                    pb.SetSetMethod(setter);
                                    ilgen = CodeEmitter.Create(setter);
                                    EmitSetValueCall(annotationAttributeBaseType, ilgen, o.methods[i].Name, o.methods[i].ReturnType, 1);
                                    ilgen.Emit(OpCodes.Ret);
                                    ilgen.DoEmit();
                                    MethodBuilder getter = attributeTypeBuilder.DefineMethod("get_" + o.methods[i].Name, MethodAttributes.Public, argType, Type.EmptyTypes);
                                    AttributeHelper.HideFromJava(getter);
                                    pb.SetGetMethod(getter);
                                    // TODO implement the getter method
                                    ilgen = CodeEmitter.Create(getter);
                                    ilgen.ThrowException(JVM.Import(typeof(NotImplementedException)));
                                    ilgen.DoEmit();
                                }
                            }
                        }
                    }
                    attributeTypeBuilder.CreateType();
                }

                private CustomAttributeBuilder MakeCustomAttributeBuilder(ClassLoaderWrapper loader, object annotation)
                {
                    Link();
                    ConstructorInfo ctor = defineConstructor != null
                        ? defineConstructor.__AsConstructorInfo()
                        : StaticCompiler.GetRuntimeType("IKVM.Attributes.DynamicAnnotationAttribute").GetConstructor(new Type[] { Types.Object.MakeArrayType() });
                    return new CustomAttributeBuilder(ctor, new object[] { AnnotationDefaultAttribute.Escape(QualifyClassNames(loader, annotation)) });
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
#endif // IMPORTER

            internal override TypeWrapper[] InnerClasses
            {
                get
                {
                    throw new InvalidOperationException("InnerClasses is only available for finished types");
                }
            }

            internal override TypeWrapper DeclaringTypeWrapper
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
                    ClassFile.InnerClass[] innerclasses = classFile.InnerClasses;
                    if (innerclasses != null)
                    {
                        for (int i = 0; i < innerclasses.Length; i++)
                        {
                            if (innerclasses[i].innerClass != 0)
                            {
                                if (classFile.GetConstantPoolClass(innerclasses[i].innerClass) == wrapper.Name)
                                {
                                    // the mask comes from RECOGNIZED_INNER_CLASS_MODIFIERS in src/hotspot/share/vm/classfile/classFileParser.cpp
                                    // (minus ACC_SUPER)
                                    mods = innerclasses[i].accessFlags & (Modifiers)0x761F;
                                    if (classFile.IsInterface)
                                    {
                                        mods |= Modifiers.Abstract;
                                    }
                                    return mods;
                                }
                            }
                        }
                    }
                    // the mask comes from JVM_RECOGNIZED_CLASS_MODIFIERS in src/hotspot/share/vm/prims/jvm.h
                    // (minus ACC_SUPER)
                    mods = classFile.Modifiers & (Modifiers)0x7611;
                    if (classFile.IsInterface)
                    {
                        mods |= Modifiers.Abstract;
                    }
                    return mods;
                }
            }

            // this finds all methods that the specified name/sig is going to be overriding
            private MethodWrapper[] FindBaseMethods(ClassFile.Method m, out bool explicitOverride)
            {
                Debug.Assert(!classFile.IsInterface);
                Debug.Assert(m.Name != "<init>");

                // starting with Java 7 the algorithm changed
                return classFile.MajorVersion >= 51
                    ? FindBaseMethods7(m.Name, m.Signature, m.IsFinal && !m.IsPublic && !m.IsProtected, out explicitOverride)
                    : FindBaseMethodsLegacy(m.Name, m.Signature, out explicitOverride);
            }

            private MethodWrapper[] FindBaseMethods7(string name, string sig, bool packageFinal, out bool explicitOverride)
            {
                // NOTE this implements the (completely broken) OpenJDK 7 b147 HotSpot behavior,
                // not the algorithm specified in section 5.4.5 of the JavaSE7 JVM spec
                // see http://weblog.ikvm.net/PermaLink.aspx?guid=bde44d8b-7ba9-4e0e-b3a6-b735627118ff and subsequent posts
                // UPDATE as of JDK 7u65 and JDK 8u11, the algorithm changed again to handle package private methods differently
                // this code has not been updated to reflect these changes (we're still at JDK 8 GA level)
                explicitOverride = false;
                MethodWrapper topPublicOrProtectedMethod = null;
                TypeWrapper tw = wrapper.BaseTypeWrapper;
                while (tw != null)
                {
                    MethodWrapper baseMethod = tw.GetMethodWrapper(name, sig, true);
                    if (baseMethod == null)
                    {
                        break;
                    }
                    else if (baseMethod.IsAccessStub)
                    {
                        // ignore
                    }
                    else if (!baseMethod.IsStatic && (baseMethod.IsPublic || baseMethod.IsProtected))
                    {
                        topPublicOrProtectedMethod = baseMethod;
                    }
                    tw = baseMethod.DeclaringType.BaseTypeWrapper;
                }
                tw = wrapper.BaseTypeWrapper;
                while (tw != null)
                {
                    MethodWrapper baseMethod = tw.GetMethodWrapper(name, sig, true);
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
                            List<MethodWrapper> list = new List<MethodWrapper>();
                            list.Add(baseMethod);
                            // we might still have to override package methods from another package if the vtable streams are interleaved with ours
                            tw = wrapper.BaseTypeWrapper;
                            while (tw != null)
                            {
                                MethodWrapper baseMethod2 = tw.GetMethodWrapper(name, sig, true);
                                if (baseMethod2 == null || baseMethod2 == baseMethod)
                                {
                                    break;
                                }
                                MethodWrapper baseMethod3 = GetPackageBaseMethod(baseMethod.DeclaringType.BaseTypeWrapper, name, sig, baseMethod2.DeclaringType);
                                if (baseMethod3 != null)
                                {
                                    if (baseMethod2.IsFinal)
                                    {
                                        baseMethod2 = baseMethod3;
                                    }
                                    bool found = false;
                                    foreach (MethodWrapper mw in list)
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
                            return new MethodWrapper[] { baseMethod };
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
                            return new MethodWrapper[] { baseMethod, topPublicOrProtectedMethod };
                        }
                        else if (!topPublicOrProtectedMethod.DeclaringType.IsPackageAccessibleFrom(wrapper))
                        {
                            // check if there is another method (in the same package) that we should override
                            tw = topPublicOrProtectedMethod.DeclaringType.BaseTypeWrapper;
                            while (tw != null)
                            {
                                MethodWrapper baseMethod2 = tw.GetMethodWrapper(name, sig, true);
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
                                        return new MethodWrapper[] { baseMethod, baseMethod2 };
                                    }
                                }
                                tw = baseMethod2.DeclaringType.BaseTypeWrapper;
                            }
                        }
                        return new MethodWrapper[] { baseMethod };
                    }
                    tw = baseMethod.DeclaringType.BaseTypeWrapper;
                }
                return null;
            }

            private bool IsAccessibleInternal(MethodWrapper mw)
            {
                return mw.IsInternal && mw.DeclaringType.InternalsVisibleTo(wrapper);
            }

            private static MethodBase LinkAndGetMethod(MethodWrapper mw)
            {
                mw.Link();
                return mw.GetMethod();
            }

            private static bool TryGetClassFileVersion(TypeWrapper tw, ref int majorVersion)
            {
                DynamicTypeWrapper dtw = tw as DynamicTypeWrapper;
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

            private static MethodWrapper GetPackageBaseMethod(TypeWrapper tw, string name, string sig, TypeWrapper package)
            {
                while (tw != null)
                {
                    MethodWrapper mw = tw.GetMethodWrapper(name, sig, true);
                    if (mw == null)
                    {
                        break;
                    }
                    if (mw.DeclaringType.IsPackageAccessibleFrom(package))
                    {
                        return mw.IsFinal ? null : mw;
                    }
                    tw = mw.DeclaringType.BaseTypeWrapper;
                }
                return null;
            }

            private MethodWrapper[] FindBaseMethodsLegacy(string name, string sig, out bool explicitOverride)
            {
                explicitOverride = false;
                TypeWrapper tw = wrapper.BaseTypeWrapper;
                while (tw != null)
                {
                    MethodWrapper baseMethod = tw.GetMethodWrapper(name, sig, true);
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
                    else if (baseMethod.IsFinal
                        && !baseMethod.IsPrivate
                        && (baseMethod.IsPublic || baseMethod.IsProtected || baseMethod.DeclaringType.IsPackageAccessibleFrom(wrapper)))
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
                                MethodWrapper baseMethod2 = tw.GetMethodWrapper(name, sig, true);
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
                                        return new MethodWrapper[] { baseMethod, baseMethod2 };
                                    }
                                }
                                tw = baseMethod2.DeclaringType.BaseTypeWrapper;
                            }
                        }
                        return new MethodWrapper[] { baseMethod };
                    }
                    // RULE 3: private and static methods are ignored
                    else if (!baseMethod.IsPrivate)
                    {
                        // RULE 4: package methods can only be overridden in the same package
                        if (baseMethod.DeclaringType.IsPackageAccessibleFrom(wrapper)
                            || (baseMethod.IsInternal && baseMethod.DeclaringType.InternalsVisibleTo(wrapper)))
                        {
                            return new MethodWrapper[] { baseMethod };
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

            private static MethodInfo GetBaseFinalizeMethod(TypeWrapper wrapper)
            {
                for (; ; )
                {
                    // HACK we get called during method linking (which is probably a bad idea) and
                    // it is possible for the base type not to be finished yet, so we look at the
                    // private state of the unfinished base types to find the finalize method.
                    DynamicTypeWrapper dtw = wrapper as DynamicTypeWrapper;
                    if (dtw == null)
                    {
                        break;
                    }
                    MethodWrapper mw = dtw.GetMethodWrapper(StringConstants.FINALIZE, StringConstants.SIG_VOID, false);
                    if (mw != null)
                    {
                        mw.Link();
                    }
                    MethodInfo finalizeImpl = dtw.impl.GetFinalizeMethod();
                    if (finalizeImpl != null)
                    {
                        return finalizeImpl;
                    }
                    wrapper = wrapper.BaseTypeWrapper;
                }
                if (wrapper == CoreClasses.java.lang.Object.Wrapper || wrapper == CoreClasses.java.lang.Throwable.Wrapper)
                {
                    return Types.Object.GetMethod("Finalize", BindingFlags.NonPublic | BindingFlags.Instance);
                }
                Type type = wrapper.TypeAsBaseType;
                MethodInfo baseFinalize = type.GetMethod("__<Finalize>", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);
                if (baseFinalize != null)
                {
                    return baseFinalize;
                }
                while (type != null)
                {
                    foreach (MethodInfo m in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                    {
                        if (m.Name == "Finalize"
                            && m.ReturnType == Types.Void
                            && m.GetParameters().Length == 0)
                        {
                            if (m.GetBaseDefinition().DeclaringType == Types.Object)
                            {
                                return m;
                            }
                        }
                    }
                    type = type.BaseType;
                }
                return null;
            }

            private MethodAttributes GetPropertyAccess(MethodWrapper mw)
            {
                string sig = mw.ReturnType.SigName;
                if (sig == "V")
                {
                    sig = mw.GetParameters()[0].SigName;
                }
                int access = -1;
                foreach (ClassFile.Field field in classFile.Fields)
                {
                    if (field.IsProperty
                        && field.IsStatic == mw.IsStatic
                        && field.Signature == sig
                        && (field.PropertyGetter == mw.Name || field.PropertySetter == mw.Name))
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

            internal override MethodBase LinkMethod(MethodWrapper mw)
            {
                Debug.Assert(mw != null);
                if (mw is DelegateConstructorMethodWrapper)
                {
                    ((DelegateConstructorMethodWrapper)mw).DoLink(typeBuilder);
                    return null;
                }
                if (mw is DelegateInvokeStubMethodWrapper)
                {
                    return ((DelegateInvokeStubMethodWrapper)mw).DoLink(typeBuilder);
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
                    foreach (MethodWrapper baseMethod in baseMethods[index])
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
                    if (index >= classFile.Methods.Length)
                    {
                        if (methods[index].IsMirandaMethod)
                        {
                            // We're a Miranda method or we're an inherited default interface method
                            Debug.Assert(baseMethods[index].Length == 1 && baseMethods[index][0].DeclaringType.IsInterface);
                            MirandaMethodWrapper mmw = (MirandaMethodWrapper)methods[index];
                            MethodAttributes attr = MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.CheckAccessOnOverride;
                            MethodWrapper baseMiranda = null;
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
                            MethodBuilder mb = methods[index].GetDefineMethodHelper().DefineMethod(wrapper, typeBuilder, methods[index].Name, attr);
                            AttributeHelper.HideFromReflection(mb);
                            if (baseMirandaOverrideStub)
                            {
                                wrapper.GenerateOverrideStub(typeBuilder, baseMiranda, mb, methods[index]);
                            }
                            if ((!wrapper.IsAbstract && mmw.BaseMethod.IsAbstract) || (!wrapper.IsInterface && mmw.Error != null))
                            {
                                string message = mmw.Error ?? (wrapper.Name + "." + methods[index].Name + methods[index].Signature);
                                CodeEmitter ilgen = CodeEmitter.Create(mb);
                                ilgen.EmitThrow(mmw.IsConflictError ? "java.lang.IncompatibleClassChangeError" : "java.lang.AbstractMethodError", message);
                                ilgen.DoEmit();
                                wrapper.EmitLevel4Warning(mmw.IsConflictError ? HardError.IncompatibleClassChangeError : HardError.AbstractMethodError, message);
                            }
#if IMPORTER
                            if (wrapper.IsInterface && !mmw.IsAbstract)
                            {
                                // even though we're not visible to reflection., we need to record the fact that we have a default implementation
                                AttributeHelper.SetModifiers(mb, mmw.Modifiers, false);
                            }
#endif
                            return mb;
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }
                    }
                    ClassFile.Method m = classFile.Methods[index];
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
                        {
                            setModifiers = true;
                        }
                    }
                    else if (m.IsClassInitializer)
                    {
                        method = ReflectUtil.DefineTypeInitializer(typeBuilder, wrapper.classLoader);
                    }
                    else
                    {
                        method = GenerateMethod(index, m, ref setModifiers);
                    }
                    string[] exceptions = m.ExceptionsAttribute;
                    methods[index].SetDeclaredExceptions(exceptions);
#if IMPORTER
                    AttributeHelper.SetThrowsAttribute(method, exceptions);
                    if (setModifiers || m.IsInternal || (m.Modifiers & (Modifiers.Synthetic | Modifiers.Bridge)) != 0)
                    {
                        AttributeHelper.SetModifiers(method, m.Modifiers, m.IsInternal);
                    }
                    if ((m.Modifiers & (Modifiers.Synthetic | Modifiers.Bridge)) != 0
                        && (m.IsPublic || m.IsProtected)
                        && wrapper.IsPublic
                        && !IsAccessBridge(classFile, m))
                    {
                        AttributeHelper.SetEditorBrowsableNever(method);
                        // TODO on WHIDBEY apply CompilerGeneratedAttribute
                    }
                    if (m.DeprecatedAttribute && !Annotation.HasObsoleteAttribute(m.Annotations))
                    {
                        AttributeHelper.SetDeprecatedAttribute(method);
                    }
                    if (m.GenericSignature != null)
                    {
                        AttributeHelper.SetSignatureAttribute(method, m.GenericSignature);
                    }
                    if (wrapper.GetClassLoader().NoParameterReflection)
                    {
                        // ignore MethodParameters (except to extract parameter names)
                    }
                    else if (m.MalformedMethodParameters)
                    {
                        AttributeHelper.SetMethodParametersAttribute(method, null);
                    }
                    else if (m.MethodParameters != null)
                    {
                        Modifiers[] modifiers = new Modifiers[m.MethodParameters.Length];
                        for (int i = 0; i < modifiers.Length; i++)
                        {
                            modifiers[i] = (Modifiers)m.MethodParameters[i].accessFlags;
                        }
                        AttributeHelper.SetMethodParametersAttribute(method, modifiers);
                    }
                    if (m.RuntimeVisibleTypeAnnotations != null)
                    {
                        AttributeHelper.SetRuntimeVisibleTypeAnnotationsAttribute(method, m.RuntimeVisibleTypeAnnotations);
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

            MethodBuilder GenerateConstructor(MethodWrapper mw)
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
                    foreach (MethodWrapper mw in methods)
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
                        foreach (MethodWrapper baseMethodWrapper in baseMethods[index])
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
                        if (baseMethods[index][0].DeclaringType == CoreClasses.java.lang.Object.Wrapper)
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
                            if (baseFinalize.DeclaringType == Types.Object)
                            {
                                needDispatch = true;
                            }
                        }
                        else
                        {
                            // One of our base classes already has a finalize method, but it may have been an empty
                            // method so that the hookup to the real Finalize was optimized away, we need to check
                            // for that.
                            if (baseFinalize.DeclaringType == Types.Object)
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
                        TypeBuilder tb = typeBuilder;
#if IMPORTER
                        if (wrapper.IsPublic && wrapper.classLoader.WorkaroundInterfacePrivateMethods)
                        {
                            // FXBUG csc.exe doesn't like non-public methods in interfaces, so we put them in a nested type
                            if (privateInterfaceMethods == null)
                            {
                                privateInterfaceMethods = typeBuilder.DefineNestedType(NestedTypeName.PrivateInterfaceMethods,
                                    TypeAttributes.NestedPrivate | TypeAttributes.Sealed | TypeAttributes.Abstract | TypeAttributes.BeforeFieldInit);
                            }
                            tb = privateInterfaceMethods;
                            attribs &= ~MethodAttributes.MemberAccessMask;
                            attribs |= MethodAttributes.Assembly;
                        }
#endif
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
                            AttributeHelper.SetNameSig(mb, m.Name, m.Signature);
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
                        foreach (MethodWrapper baseMethod in baseMethods[index])
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

                        finalizeMethod = typeBuilder.DefineMethod(finalizeName, attr, CallingConventions.Standard, Types.Void, Type.EmptyTypes);
                        if (finalizeName != baseFinalize.Name)
                            typeBuilder.DefineMethodOverride(finalizeMethod, baseFinalize);

                        AttributeHelper.HideFromJava(finalizeMethod);

                        var ilgen = CodeEmitter.Create(finalizeMethod);
                        ilgen.EmitLdarg(0);
                        ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.SkipFinalizerOf);
                        var skip = ilgen.DefineLabel();
                        ilgen.EmitBrtrue(skip);

                        if (needDispatch)
                        {
                            ilgen.BeginExceptionBlock();
                            ilgen.Emit(OpCodes.Ldarg_0);
                            ilgen.Emit(OpCodes.Callvirt, mb);
                            ilgen.EmitLeave(skip);
                            ilgen.BeginCatchBlock(Types.Object);
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
                        CustomAttributeBuilder cab = new CustomAttributeBuilder(StaticCompiler.GetRuntimeType("IKVM.Attributes.AnnotationDefaultAttribute").GetConstructor(new Type[] { Types.Object }), new object[] { AnnotationDefaultAttribute.Escape(classFile.Methods[index].AnnotationDefault) });
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

                    AttributeHelper.HideFromJava(mb, flags);
                }

                if (classFile.IsInterface && methods[index].IsVirtual && !methods[index].IsAbstract)
                {
                    if (wrapper.IsGhost)
                    {
                        DefaultInterfaceMethodWrapper.SetImpl(methods[index], methods[index].GetDefineMethodHelper().DefineMethod(wrapper.GetClassLoader().GetTypeWrapperFactory(),
                            typeBuilder, NamePrefix.DefaultMethod + mb.Name, MethodAttributes.Public | MethodAttributes.SpecialName,
                            null, false));
                    }
                    else
                    {
                        DefaultInterfaceMethodWrapper.SetImpl(methods[index], methods[index].GetDefineMethodHelper().DefineMethod(wrapper.GetClassLoader().GetTypeWrapperFactory(),
                            typeBuilder, NamePrefix.DefaultMethod + mb.Name, MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.SpecialName,
                            typeBuilder, false));
                    }
                }

                return mb;
            }

            private static MethodAttributes GetMethodAccess(MethodWrapper mw)
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
                    foreach (ClassFile.Method.Instruction instr in m.Instructions)
                    {
                        if (instr.NormalizedOpCode == NormalizedByteCode.__invokespecial)
                        {
                            ClassFile.ConstantPoolItemMI cpi = classFile.SafeGetMethodref(instr.Arg1);
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

            internal override TypeWrapper Host
            {
                get { return host; }
            }
        }
    }

}