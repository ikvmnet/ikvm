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

using IKVM.Attributes;
using IKVM.CoreLib.Diagnostics;
using IKVM.CoreLib.Runtime;
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using IKVM.Runtime;
using IKVM.Tools.Importer.MapXml;

using Type = IKVM.Reflection.Type;

namespace IKVM.Tools.Importer
{

    /// <summary>
    /// Implementation of <see cref="RuntimeByteCodeJavaType"/> that customizes output for the importer.
    /// </summary>
    sealed class RuntimeImportByteCodeJavaType : RuntimeByteCodeJavaType
    {

        FieldInfo ghostRefField;
        MethodBuilder ghostIsInstanceMethod;
        MethodBuilder ghostIsInstanceArrayMethod;
        MethodBuilder ghostCastMethod;
        MethodBuilder ghostCastArrayMethod;
        TypeBuilder typeBuilderGhostInterface;
        Annotation annotation;
        Type enumType;
        RuntimeJavaMethod[] replacedMethods;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="f"></param>
        /// <param name="loader"></param>
        internal RuntimeImportByteCodeJavaType(ClassFile f, ImportClassLoader loader) :
            base(null, f, loader, null)
        {

        }

        internal override void Finish()
        {
            base.Finish();
        }

        sealed class ConstructorForwarder : RuntimeJavaMethod
        {

            readonly RuntimeJavaTypeFactory context;
            readonly TypeBuilder typeBuilder;
            readonly RuntimeJavaMethod ctor;
            MethodBuilder constructorBuilder;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="context"></param>
            /// <param name="typeBuilder"></param>
            /// <param name="ctor"></param>
            internal ConstructorForwarder(RuntimeJavaTypeFactory context, TypeBuilder typeBuilder, RuntimeJavaMethod ctor) :
                base(ctor.DeclaringType, ctor.Name, ctor.Signature, null, null, null, ctor.Modifiers, MemberFlags.None)
            {
                this.context = context;
                this.typeBuilder = typeBuilder;
                this.ctor = ctor;
            }

            protected override void DoLinkMethod()
            {
                ctor.Link();
                DefineMethodHelper dmh = ctor.GetDefineMethodHelper();
                constructorBuilder = dmh.DefineConstructor(context, typeBuilder, MethodAttributes.PrivateScope);
                ctor.DeclaringType.Context.AttributeHelper.HideFromJava(constructorBuilder);
                CodeEmitter ilgen = ctor.DeclaringType.Context.CodeEmitterFactory.Create(constructorBuilder);
                ilgen.Emit(OpCodes.Ldarg_0);
                for (int i = 1; i <= dmh.ParameterCount; i++)
                {
                    ilgen.EmitLdarg(i);
                }
                ctor.EmitCall(ilgen);
                ilgen.Emit(OpCodes.Ret);
                ilgen.DoEmit();
            }

            internal override void EmitCall(CodeEmitter ilgen)
            {
                ilgen.Emit(OpCodes.Call, constructorBuilder);
            }
        }

        internal override bool IsGhost => classLoader.IsGhost(this);

        internal override bool IsMapUnsafeException => classLoader.IsMapUnsafeException(this);

        internal override Type TypeAsBaseType => typeBuilderGhostInterface != null ? typeBuilderGhostInterface : base.TypeAsBaseType;

        internal void GetParameterNamesFromXml(string methodName, string methodSig, string[] parameterNames)
        {
            var parameters = classLoader.GetXmlMapParameters(Name, methodName, methodSig);
            if (parameters != null)
                for (int i = 0; i < parameters.Length; i++)
                    if (parameters[i].Name != null)
                        parameterNames[i] = parameters[i].Name;
        }

        internal void AddXmlMapParameterAttributes(MethodBuilder method, string className, string methodName, string methodSig, ref ParameterBuilder[] parameterBuilders)
        {
            var parameters = classLoader.GetXmlMapParameters(className, methodName, methodSig);
            if (parameters != null)
            {
                parameterBuilders ??= GetParameterBuilders(method, parameters.Length, null);
                if (parameters.Length > parameterBuilders.Length)
                    throw new InvalidOperationException($"Number of parameters in map XML exceeds the number of parameters being built for '{className}:{methodName}' ({parameters.Length} > {parameterBuilders.Length}).");

                for (int i = 0; i < parameters.Length; i++)
                    if (parameters[i].Attributes != null)
                        foreach (var attr in parameters[i].Attributes)
                            Context.AttributeHelper.SetCustomAttribute(classLoader, parameterBuilders[i], attr);
            }
        }

        private void AddParameterMetadata(MethodBuilder method, RuntimeJavaMethod mw)
        {
            ParameterBuilder[] pbs;
            if ((mw.DeclaringType.IsPublic && (mw.IsPublic || mw.IsProtected)) || classLoader.EmitSymbols)
            {
                string[] parameterNames = new string[mw.GetParameters().Length];
                GetParameterNamesFromXml(mw.Name, mw.Signature, parameterNames);
                GetParameterNamesFromSig(mw.Signature, parameterNames);
                pbs = GetParameterBuilders(method, parameterNames.Length, parameterNames);
            }
            else
            {
                pbs = GetParameterBuilders(method, mw.GetParameters().Length, null);
            }

            if ((mw.Modifiers & Modifiers.VarArgs) != 0 && pbs.Length > 0)
                Context.AttributeHelper.SetParamArrayAttribute(pbs[pbs.Length - 1]);

            AddXmlMapParameterAttributes(method, Name, mw.Name, mw.Signature, ref pbs);
        }

        /// <summary>
        /// Applies fields from the map XML.
        /// </summary>
        /// <param name="fields"></param>
        protected override void AddMapXmlFields(ref RuntimeJavaField[] fields)
        {
            var mapxml = classLoader.GetMapXmlClasses();
            if (mapxml != null && mapxml.TryGetValue(Name, out var clazz) && clazz.Fields != null)
            {
                foreach (var field in clazz.Fields)
                {
                    // are we adding a new field?
                    bool found = false;
                    foreach (var fw in fields)
                    {
                        if (fw.Name == field.Name && fw.Signature == field.Sig)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (found == false)
                        fields = ArrayUtil.Concat(fields, RuntimeJavaField.Create(this, null, null, field.Name, field.Sig, new ExModifiers((Modifiers)field.Modifiers, false)));
                }
            }
        }

        protected override bool EmitMapXmlMethodPrologueAndOrBody(CodeEmitter ilgen, ClassFile f, Method m)
        {
            var prologue = classLoader.GetMethodPrologue(new MethodKey(f.Name, m.Name, m.Signature));
            if (prologue != null)
                prologue.Emit(classLoader, ilgen);

            var mapxml = classLoader.GetMapXmlMethodBodies();
            if (mapxml != null && mapxml.TryGetValue(new MethodKey(f.Name, m.Name, m.Signature), out var opcodes))
            {
                opcodes.Emit(classLoader, ilgen);
                return true;
            }

            return false;
        }

        void PublishAttributes(TypeBuilder typeBuilder, Class clazz)
        {
            foreach (var attr in clazz.Attributes)
                Context.AttributeHelper.SetCustomAttribute(classLoader, typeBuilder, attr);
        }

        /// <summary>
        /// Returns <c>true</c> if the given type arrays are equal.
        /// </summary>
        /// <param name="args1"></param>
        /// <param name="args2"></param>
        /// <returns></returns>
        static bool CheckPropertyArgs(Type[] args1, Type[] args2)
        {
            if (args1.Length == args2.Length)
            {
                for (int i = 0; i < args1.Length; i++)
                    if (args1[i] != args2[i])
                        return false;

                return true;
            }

            return false;
        }

        static MethodAttributes GetPropertyMethodAttributes(RuntimeJavaMethod mw, bool final)
        {
            var attribs = MethodAttributes.HideBySig;
            if (mw.IsStatic)
            {
                attribs |= MethodAttributes.Static;
            }
            else
            {
                // NOTE in order for IntelliSense to consider the property a "real" property,
                // the getter and setter methods need to have substantially the same method attributes,
                // so we may need to look at our peer to determine whether we should be final
                // or not (and vice versa).
                attribs |= MethodAttributes.Virtual | MethodAttributes.NewSlot | MethodAttributes.CheckAccessOnOverride;
                if (final)
                    attribs |= MethodAttributes.Final;
            }

            // TODO what happens if accessibility doesn't match our peer?
            if (mw.IsPublic)
                attribs |= MethodAttributes.Public;
            else if (mw.IsProtected)
                attribs |= MethodAttributes.FamORAssem;
            else if (mw.IsPrivate)
                attribs |= MethodAttributes.Private;
            else
                attribs |= MethodAttributes.Assembly;

            return attribs;
        }

        private void PublishProperties(TypeBuilder typeBuilder, Class clazz)
        {
            foreach (var prop in clazz.Properties)
            {
                var typeWrapper = ClassLoader.RetTypeWrapperFromSig(prop.Sig, LoadMode.Link);
                var propargs = ClassLoader.ArgJavaTypeListFromSig(prop.Sig, LoadMode.Link);
                Type[] indexer = new Type[propargs.Length];
                for (int i = 0; i < propargs.Length; i++)
                {
                    indexer[i] = propargs[i].TypeAsSignatureType;
                }
                PropertyBuilder propbuilder = typeBuilder.DefineProperty(prop.Name, PropertyAttributes.None, typeWrapper.TypeAsSignatureType, indexer);
                Context.AttributeHelper.HideFromJava(propbuilder);
                if (prop.Attributes != null)
                {
                    foreach (IKVM.Tools.Importer.MapXml.Attribute attr in prop.Attributes)
                    {
                        Context.AttributeHelper.SetCustomAttribute(classLoader, propbuilder, attr);
                    }
                }

                RuntimeJavaMethod getter = null;
                RuntimeJavaMethod setter = null;
                if (prop.Getter != null)
                {
                    getter = GetMethod(prop.Getter.Name, prop.Getter.Sig, true);
                    if (getter == null)
                    {
                        Console.Error.WriteLine("Warning: getter not found for {0}::{1}", clazz.Name, prop.Name);
                    }
                }

                if (prop.Setter != null)
                {
                    setter = GetMethod(prop.Setter.Name, prop.Setter.Sig, true);
                    if (setter == null)
                        Console.Error.WriteLine("Warning: setter not found for {0}::{1}", clazz.Name, prop.Name);
                }

                var final = (getter != null && getter.IsFinal) || (setter != null && setter.IsFinal);
                if (getter != null)
                {
                    var mw = getter;

                    if (!CheckPropertyArgs(mw.GetParametersForDefineMethod(), indexer) || mw.ReturnType != typeWrapper)
                    {
                        Console.Error.WriteLine("Warning: ignoring invalid property getter for {0}::{1}", clazz.Name, prop.Name);
                    }
                    else
                    {
                        var mb = mw.GetMethod() as MethodBuilder;
                        if (mb == null || mb.DeclaringType != typeBuilder || (!mb.IsFinal && final))
                        {
                            mb = typeBuilder.DefineMethod("get_" + prop.Name, GetPropertyMethodAttributes(mw, final), typeWrapper.TypeAsSignatureType, indexer);
                            Context.AttributeHelper.HideFromJava(mb);
                            var ilgen = Context.CodeEmitterFactory.Create(mb);
                            if (mw.IsStatic)
                            {
                                for (int i = 0; i < indexer.Length; i++)
                                {
                                    ilgen.EmitLdarg(i);
                                }
                                mw.EmitCall(ilgen);
                            }
                            else
                            {
                                ilgen.Emit(OpCodes.Ldarg_0);
                                for (int i = 0; i < indexer.Length; i++)
                                    ilgen.EmitLdarg(i + 1);
                                mw.EmitCallvirt(ilgen);
                            }
                            ilgen.Emit(OpCodes.Ret);
                            ilgen.DoEmit();
                        }
                        propbuilder.SetGetMethod(mb);
                    }
                }
                if (setter != null)
                {
                    var mw = setter;
                    var args = ArrayUtil.Concat(indexer, typeWrapper.TypeAsSignatureType);
                    if (!CheckPropertyArgs(args, mw.GetParametersForDefineMethod()))
                    {
                        Console.Error.WriteLine("Warning: ignoring invalid property setter for {0}::{1}", clazz.Name, prop.Name);
                    }
                    else
                    {
                        var mb = mw.GetMethod() as MethodBuilder;
                        if (mb == null || mb.DeclaringType != typeBuilder || (!mb.IsFinal && final))
                        {
                            mb = typeBuilder.DefineMethod("set_" + prop.Name, GetPropertyMethodAttributes(mw, final), mw.ReturnTypeForDefineMethod, args);
                            Context.AttributeHelper.HideFromJava(mb);
                            var ilgen = Context.CodeEmitterFactory.Create(mb);
                            if (mw.IsStatic)
                            {
                                for (int i = 0; i <= indexer.Length; i++)
                                    ilgen.EmitLdarg(i);
                                mw.EmitCall(ilgen);
                            }
                            else
                            {
                                ilgen.Emit(OpCodes.Ldarg_0);
                                for (int i = 0; i <= indexer.Length; i++)
                                    ilgen.EmitLdarg(i + 1);
                                mw.EmitCallvirt(ilgen);
                            }
                            ilgen.Emit(OpCodes.Ret);
                            ilgen.DoEmit();
                        }

                        propbuilder.SetSetMethod(mb);
                    }
                }
            }
        }

        private static void MapModifiers(MapModifiers mapmods, bool isConstructor, out bool setmodifiers, ref MethodAttributes attribs, bool isNewSlot)
        {
            setmodifiers = false;
            var modifiers = (Modifiers)mapmods;
            if ((modifiers & Modifiers.Public) != 0)
            {
                attribs |= MethodAttributes.Public;
            }
            else if ((modifiers & Modifiers.Protected) != 0)
            {
                attribs |= MethodAttributes.FamORAssem;
            }
            else if ((modifiers & Modifiers.Private) != 0)
            {
                attribs |= MethodAttributes.Private;
            }
            else
            {
                attribs |= MethodAttributes.Assembly;
            }

            if ((modifiers & Modifiers.Static) != 0)
            {
                attribs |= MethodAttributes.Static;
                if ((modifiers & Modifiers.Final) != 0)
                {
                    setmodifiers = true;
                }
            }
            else if (!isConstructor)
            {
                // NOTE we're abusing the MethodAttributes.NewSlot and Modifiers.Final combination to mean non-virtual
                if ((modifiers & Modifiers.Final) != 0 && (attribs & MethodAttributes.NewSlot) != 0 && (attribs & MethodAttributes.Virtual) == 0)
                {
                    // remove NewSlot, because it doesn't make sense on a non-virtual method
                    attribs &= ~MethodAttributes.NewSlot;
                }
                else if (((modifiers & (Modifiers.Public | Modifiers.Final)) == Modifiers.Final && isNewSlot && (attribs & MethodAttributes.Virtual) == 0))
                {
                    // final method that doesn't need to be virtual
                }
                else
                {
                    if ((modifiers & Modifiers.Private) == 0)
                    {
                        attribs |= MethodAttributes.Virtual;
                    }
                    if ((modifiers & Modifiers.Final) != 0)
                    {
                        attribs |= MethodAttributes.Final;
                    }
                    else if ((modifiers & Modifiers.Abstract) != 0)
                    {
                        attribs |= MethodAttributes.Abstract;
                    }
                }
            }

            if ((modifiers & Modifiers.Synchronized) != 0)
            {
                throw new NotImplementedException();
            }
        }

        private void MapSignature(string sig, out Type returnType, out Type[] parameterTypes)
        {
            returnType = ClassLoader.RetTypeWrapperFromSig(sig, LoadMode.Link).TypeAsSignatureType;
            var parameterTypeWrappers = ClassLoader.ArgJavaTypeListFromSig(sig, LoadMode.Link);
            parameterTypes = new Type[parameterTypeWrappers.Length];
            for (int i = 0; i < parameterTypeWrappers.Length; i++)
            {
                parameterTypes[i] = parameterTypeWrappers[i].TypeAsSignatureType;
            }
        }

        protected override void EmitMapXmlMetadata(TypeBuilder typeBuilder, ClassFile classFile, RuntimeJavaField[] fields, RuntimeJavaMethod[] methods)
        {
            var mapxml = classLoader.GetMapXmlClasses();
            if (mapxml != null)
            {
                if (mapxml.TryGetValue(classFile.Name, out var clazz))
                {
                    if (clazz.Attributes != null)
                        PublishAttributes(typeBuilder, clazz);

                    if (clazz.Properties != null)
                        PublishProperties(typeBuilder, clazz);

                    if (clazz.Fields != null)
                    {
                        foreach (var field in clazz.Fields)
                        {
                            if (field.Attributes != null)
                            {
                                foreach (var fw in fields)
                                {
                                    if (fw.Name == field.Name && fw.Signature == field.Sig)
                                    {
                                        var fb = fw.GetField() as FieldBuilder;
                                        if (fb != null)
                                            foreach (var attr in field.Attributes)
                                                Context.AttributeHelper.SetCustomAttribute(classLoader, fb, attr);
                                    }
                                }
                            }
                        }
                    }
                    if (clazz.Constructors != null)
                    {
                        // HACK this isn't the right place to do this, but for now it suffices
                        foreach (var constructor in clazz.Constructors)
                        {
                            // are we adding a new constructor?
                            if (GetMethod(StringConstants.INIT, constructor.Sig, false) == null)
                            {
                                if (constructor.Body == null)
                                {
                                    Console.Error.WriteLine("Error: Constructor {0}.<init>{1} in xml remap file doesn't have a body.", clazz.Name, constructor.Sig);
                                    continue;
                                }

                                MethodAttributes attribs = 0;
                                MapModifiers(constructor.Modifiers, true, out var setmodifiers, ref attribs, false);
                                MapSignature(constructor.Sig, out var returnType, out var parameterTypes);
                                var cb = ReflectUtil.DefineConstructor(typeBuilder, attribs, parameterTypes);
                                if (setmodifiers)
                                    Context.AttributeHelper.SetModifiers(cb, (Modifiers)constructor.Modifiers, false);

                                ImportClassLoader.AddDeclaredExceptions(Context, cb, constructor.Throws);
                                var ilgen = Context.CodeEmitterFactory.Create(cb);
                                constructor.Emit(classLoader, ilgen);
                                ilgen.DoEmit();
                                if (constructor.Attributes != null)
                                    foreach (var attr in constructor.Attributes)
                                        Context.AttributeHelper.SetCustomAttribute(classLoader, cb, attr);
                            }
                        }

                        foreach (var constructor in clazz.Constructors)
                        {
                            if (constructor.Attributes != null)
                            {
                                foreach (var mw in methods)
                                {
                                    if (mw.Name == "<init>" && mw.Signature == constructor.Sig)
                                    {
                                        var mb = mw.GetMethod() as MethodBuilder;
                                        if (mb != null)
                                            foreach (var attr in constructor.Attributes)
                                                Context.AttributeHelper.SetCustomAttribute(classLoader, mb, attr);
                                    }
                                }
                            }
                        }
                    }
                    if (clazz.Methods != null)
                    {
                        // HACK this isn't the right place to do this, but for now it suffices
                        foreach (var method in clazz.Methods)
                        {
                            // are we adding a new method?
                            if (GetMethod(method.Name, method.Sig, false) == null)
                            {
                                var attribs = method.MethodAttributes;
                                MapModifiers(method.Modifiers, false, out var setmodifiers, ref attribs, BaseTypeWrapper == null || BaseTypeWrapper.GetMethod(method.Name, method.Sig, true) == null);
                                if (method.Body == null && (attribs & MethodAttributes.Abstract) == 0)
                                {
                                    Console.Error.WriteLine("Error: Method {0}.{1}{2} in xml remap file doesn't have a body.", clazz.Name, method.Name, method.Sig);
                                    continue;
                                }

                                MapSignature(method.Sig, out var returnType, out var parameterTypes);
                                var mb = typeBuilder.DefineMethod(method.Name, attribs, returnType, parameterTypes);
                                if (setmodifiers)
                                {
                                    Context.AttributeHelper.SetModifiers(mb, (Modifiers)method.Modifiers, false);
                                }

                                if (method.Override != null)
                                {
                                    var mw = ClassLoader.LoadClassByName(method.Override.Class).GetMethod(method.Override.Name, method.Sig, true);
                                    mw.Link();
                                    typeBuilder.DefineMethodOverride(mb, (MethodInfo)mw.GetMethod());
                                }

                                ImportClassLoader.AddDeclaredExceptions(Context, mb, method.Throws);
                                if (method.Body != null)
                                {
                                    CodeEmitter ilgen = Context.CodeEmitterFactory.Create(mb);
                                    method.Emit(classLoader, ilgen);
                                    ilgen.DoEmit();
                                }

                                if (method.Attributes != null)
                                    foreach (var attr in method.Attributes)
                                        Context.AttributeHelper.SetCustomAttribute(classLoader, mb, attr);
                            }
                        }

                        foreach (IKVM.Tools.Importer.MapXml.Method method in clazz.Methods)
                        {
                            if (method.Attributes != null)
                            {
                                foreach (var mw in methods)
                                {
                                    if (mw.Name == method.Name && mw.Signature == method.Sig)
                                    {
                                        MethodBuilder mb = mw.GetMethod() as MethodBuilder;
                                        if (mb != null)
                                        {
                                            foreach (IKVM.Tools.Importer.MapXml.Attribute attr in method.Attributes)
                                            {
                                                Context.AttributeHelper.SetCustomAttribute(classLoader, mb, attr);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (clazz.Interfaces != null)
                    {
                        foreach (Implements iface in clazz.Interfaces)
                        {
                            var tw = ClassLoader.LoadClassByName(iface.Class);
                            // NOTE since this interface won't be part of the list in the ImplementAttribute,
                            // it won't be visible from Java that the type implements this interface.
                            typeBuilder.AddInterfaceImplementation(tw.TypeAsBaseType);
                            if (iface.Methods != null)
                            {
                                foreach (var m in iface.Methods)
                                {
                                    var mw = tw.GetMethod(m.Name, m.Sig, false);
                                    if (mw == null)
                                        throw new InvalidOperationException("Method " + m.Name + m.Sig + " not found in interface " + tw.Name);

                                    mw.Link();
                                    MethodBuilder mb = mw.GetDefineMethodHelper().DefineMethod(this, typeBuilder, tw.Name + "/" + m.Name, MethodAttributes.Private | MethodAttributes.NewSlot | MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.CheckAccessOnOverride);
                                    Context.AttributeHelper.HideFromJava(mb);
                                    typeBuilder.DefineMethodOverride(mb, (MethodInfo)mw.GetMethod());
                                    CodeEmitter ilgen = Context.CodeEmitterFactory.Create(mb);
                                    m.Emit(classLoader, ilgen);
                                    ilgen.DoEmit();
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override MethodBuilder DefineGhostMethod(TypeBuilder typeBuilder, string name, MethodAttributes attribs, RuntimeJavaMethod mw)
        {
            if (typeBuilderGhostInterface != null && mw.IsVirtual)
            {
                var helper = mw.GetDefineMethodHelper();
                var stub = helper.DefineMethod(this, typeBuilder, name, MethodAttributes.Public);
                ((RuntimeGhostJavaMethod)mw).SetGhostMethod(stub);
                return helper.DefineMethod(this, typeBuilderGhostInterface, name, attribs);
            }

            return null;
        }

        protected override void FinishGhost(TypeBuilder typeBuilder, RuntimeJavaMethod[] methods)
        {
            if (typeBuilderGhostInterface != null)
            {
                // TODO consider adding methods from base interface and java.lang.Object as well
                for (int i = 0; i < methods.Length; i++)
                {
                    // skip <clinit> and non-virtual interface methods introduced in Java 8
                    if (methods[i] is RuntimeGhostJavaMethod gmw)
                    {
                        var args = methods[i].GetParameters();
                        var stub = gmw.GetGhostMethod();
                        AddParameterMetadata(stub, methods[i]);
                        Context.AttributeHelper.SetModifiers(stub, methods[i].Modifiers, methods[i].IsInternal);
                        var ilgen = Context.CodeEmitterFactory.Create(stub);
                        var end = ilgen.DefineLabel();
                        var implementers = classLoader.GetGhostImplementers(this);
                        ilgen.Emit(OpCodes.Ldarg_0);
                        ilgen.Emit(OpCodes.Ldfld, ghostRefField);
                        ilgen.Emit(OpCodes.Dup);
                        ilgen.Emit(OpCodes.Isinst, typeBuilderGhostInterface);
                        var label = ilgen.DefineLabel();
                        ilgen.EmitBrfalse(label);
                        ilgen.Emit(OpCodes.Castclass, typeBuilderGhostInterface);
                        for (int k = 0; k < args.Length; k++)
                        {
                            ilgen.EmitLdarg(k + 1);
                        }
                        ilgen.Emit(OpCodes.Callvirt, (MethodInfo)methods[i].GetMethod());
                        ilgen.EmitBr(end);
                        ilgen.MarkLabel(label);
                        for (int j = 0; j < implementers.Length; j++)
                        {
                            ilgen.Emit(OpCodes.Dup);
                            ilgen.Emit(OpCodes.Isinst, implementers[j].TypeAsTBD);
                            label = ilgen.DefineLabel();
                            ilgen.EmitBrfalse(label);
                            var mw = implementers[j].GetMethod(methods[i].Name, methods[i].Signature, true);
                            if (mw == null)
                            {
                                if (methods[i].IsAbstract)
                                {
                                    // This should only happen for remapped types (defined in map.xml), because normally you'd get a miranda method.
                                    throw new FatalCompilerErrorException(DiagnosticEvent.GhostInterfaceMethodMissing(implementers[j].Name, Name, methods[i].Name, methods[i].Signature));
                                }
                                // We're inheriting a default method
                                ilgen.Emit(OpCodes.Pop);
                                ilgen.Emit(OpCodes.Ldarg_0);
                                for (int k = 0; k < args.Length; k++)
                                {
                                    ilgen.EmitLdarg(k + 1);
                                }
                                ilgen.Emit(OpCodes.Call, RuntimeDefaultInterfaceJavaMethod.GetImpl(methods[i]));
                            }
                            else
                            {
                                ilgen.Emit(OpCodes.Castclass, implementers[j].TypeAsTBD);
                                for (int k = 0; k < args.Length; k++)
                                {
                                    ilgen.EmitLdarg(k + 1);
                                }
                                mw.EmitCallvirt(ilgen);
                            }
                            ilgen.EmitBr(end);
                            ilgen.MarkLabel(label);
                        }
                        // we need to do a null check (null fails all the isinst checks)
                        ilgen.EmitNullCheck();
                        ilgen.EmitThrow("java.lang.IncompatibleClassChangeError", Name);
                        ilgen.MarkLabel(end);
                        ilgen.Emit(OpCodes.Ret);
                        ilgen.DoEmit();
                    }
                }

                // HACK create a scope to enable reuse of "implementers" name
                if (true)
                {
                    MethodBuilder mb;
                    CodeEmitter ilgen;
                    CodeEmitterLocal local;
                    // add implicit conversions for all the ghost implementers
                    var implementers = classLoader.GetGhostImplementers(this);
                    for (int i = 0; i < implementers.Length; i++)
                    {
                        mb = typeBuilder.DefineMethod("op_Implicit", MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.SpecialName, TypeAsSignatureType, new Type[] { implementers[i].TypeAsSignatureType });
                        Context.AttributeHelper.HideFromJava(mb);
                        ilgen = Context.CodeEmitterFactory.Create(mb);
                        local = ilgen.DeclareLocal(TypeAsSignatureType);
                        ilgen.Emit(OpCodes.Ldloca, local);
                        ilgen.Emit(OpCodes.Ldarg_0);
                        ilgen.Emit(OpCodes.Stfld, ghostRefField);
                        ilgen.Emit(OpCodes.Ldloca, local);
                        ilgen.Emit(OpCodes.Ldobj, TypeAsSignatureType);
                        ilgen.Emit(OpCodes.Ret);
                        ilgen.DoEmit();
                    }
                    // Implement the "IsInstance" method
                    mb = ghostIsInstanceMethod;
                    Context.AttributeHelper.HideFromJava(mb);
                    ilgen = Context.CodeEmitterFactory.Create(mb);
                    CodeEmitterLabel end = ilgen.DefineLabel();
                    for (int i = 0; i < implementers.Length; i++)
                    {
                        ilgen.Emit(OpCodes.Ldarg_0);
                        ilgen.Emit(OpCodes.Isinst, implementers[i].TypeAsTBD);
                        CodeEmitterLabel label = ilgen.DefineLabel();
                        ilgen.EmitBrfalse(label);
                        ilgen.Emit(OpCodes.Ldc_I4_1);
                        ilgen.EmitBr(end);
                        ilgen.MarkLabel(label);
                    }
                    ilgen.Emit(OpCodes.Ldarg_0);
                    ilgen.Emit(OpCodes.Isinst, typeBuilderGhostInterface);
                    ilgen.Emit(OpCodes.Ldnull);
                    ilgen.Emit(OpCodes.Cgt_Un);
                    ilgen.MarkLabel(end);
                    ilgen.Emit(OpCodes.Ret);
                    ilgen.DoEmit();
                    // Implement the "IsInstanceArray" method
                    mb = ghostIsInstanceArrayMethod;
                    Context.AttributeHelper.HideFromJava(mb);
                    ilgen = Context.CodeEmitterFactory.Create(mb);
                    var localType = ilgen.DeclareLocal(Context.Types.Type);
                    var localRank = ilgen.DeclareLocal(Context.Types.Int32);
                    ilgen.Emit(OpCodes.Ldarg_0);
                    var skip = ilgen.DefineLabel();
                    ilgen.EmitBrtrue(skip);
                    ilgen.Emit(OpCodes.Ldc_I4_0);
                    ilgen.Emit(OpCodes.Ret);
                    ilgen.MarkLabel(skip);
                    ilgen.Emit(OpCodes.Ldarg_0);
                    ilgen.Emit(OpCodes.Call, Context.CompilerFactory.GetTypeMethod);
                    ilgen.Emit(OpCodes.Stloc, localType);
                    ilgen.Emit(OpCodes.Ldarg_1);
                    ilgen.Emit(OpCodes.Stloc, localRank);
                    skip = ilgen.DefineLabel();
                    ilgen.EmitBr(skip);
                    var iter = ilgen.DefineLabel();
                    ilgen.MarkLabel(iter);
                    ilgen.Emit(OpCodes.Ldloc, localType);
                    ilgen.Emit(OpCodes.Callvirt, Context.Types.Type.GetMethod("GetElementType"));
                    ilgen.Emit(OpCodes.Stloc, localType);
                    ilgen.Emit(OpCodes.Ldloc, localRank);
                    ilgen.Emit(OpCodes.Ldc_I4_1);
                    ilgen.Emit(OpCodes.Sub);
                    ilgen.Emit(OpCodes.Stloc, localRank);
                    ilgen.Emit(OpCodes.Ldloc, localRank);
                    var typecheck = ilgen.DefineLabel();
                    ilgen.EmitBrfalse(typecheck);
                    ilgen.MarkLabel(skip);
                    ilgen.Emit(OpCodes.Ldloc, localType);
                    ilgen.Emit(OpCodes.Callvirt, Context.Types.Type.GetMethod("get_IsArray"));
                    ilgen.EmitBrtrue(iter);
                    ilgen.Emit(OpCodes.Ldc_I4_0);
                    ilgen.Emit(OpCodes.Ret);
                    ilgen.MarkLabel(typecheck);
                    for (int i = 0; i < implementers.Length; i++)
                    {
                        ilgen.Emit(OpCodes.Ldtoken, implementers[i].TypeAsTBD);
                        ilgen.Emit(OpCodes.Call, Context.Types.Type.GetMethod("GetTypeFromHandle"));
                        ilgen.Emit(OpCodes.Ldloc, localType);
                        ilgen.Emit(OpCodes.Callvirt, Context.Types.Type.GetMethod("IsAssignableFrom"));
                        var label = ilgen.DefineLabel();
                        ilgen.EmitBrfalse(label);
                        ilgen.Emit(OpCodes.Ldc_I4_1);
                        ilgen.Emit(OpCodes.Ret);
                        ilgen.MarkLabel(label);
                    }
                    ilgen.Emit(OpCodes.Ldtoken, typeBuilderGhostInterface);
                    ilgen.Emit(OpCodes.Call, Context.Types.Type.GetMethod("GetTypeFromHandle"));
                    ilgen.Emit(OpCodes.Ldloc, localType);
                    ilgen.Emit(OpCodes.Callvirt, Context.Types.Type.GetMethod("IsAssignableFrom"));
                    skip = ilgen.DefineLabel();
                    ilgen.EmitBrfalse(skip);
                    ilgen.Emit(OpCodes.Ldc_I4_1);
                    ilgen.Emit(OpCodes.Ret);
                    ilgen.MarkLabel(skip);
                    ilgen.Emit(OpCodes.Ldarg_0);
                    ilgen.Emit(OpCodes.Ldtoken, typeBuilder);
                    ilgen.Emit(OpCodes.Ldarg_1);
                    ilgen.Emit(OpCodes.Call, Context.Resolver.ResolveRuntimeType(typeof(IKVM.Runtime.GhostTag).FullName).AsReflection().GetMethod("IsGhostArrayInstance", BindingFlags.NonPublic | BindingFlags.Static));
                    ilgen.Emit(OpCodes.Ret);
                    ilgen.DoEmit();

                    // Implement the "Cast" method
                    mb = ghostCastMethod;
                    Context.AttributeHelper.HideFromJava(mb);
                    ilgen = Context.CodeEmitterFactory.Create(mb);
                    end = ilgen.DefineLabel();
                    for (int i = 0; i < implementers.Length; i++)
                    {
                        ilgen.Emit(OpCodes.Ldarg_0);
                        ilgen.Emit(OpCodes.Isinst, implementers[i].TypeAsTBD);
                        ilgen.EmitBrtrue(end);
                    }
                    ilgen.Emit(OpCodes.Ldarg_0);
                    ilgen.Emit(OpCodes.Castclass, typeBuilderGhostInterface);
                    ilgen.Emit(OpCodes.Pop);
                    ilgen.MarkLabel(end);
                    local = ilgen.DeclareLocal(TypeAsSignatureType);
                    ilgen.Emit(OpCodes.Ldloca, local);
                    ilgen.Emit(OpCodes.Ldarg_0);
                    ilgen.Emit(OpCodes.Stfld, ghostRefField);
                    ilgen.Emit(OpCodes.Ldloca, local);
                    ilgen.Emit(OpCodes.Ldobj, TypeAsSignatureType);
                    ilgen.Emit(OpCodes.Ret);
                    ilgen.DoEmit();
                    // Add "ToObject" methods
                    mb = typeBuilder.DefineMethod("ToObject", MethodAttributes.HideBySig | MethodAttributes.Public, Context.Types.Object, Type.EmptyTypes);
                    Context.AttributeHelper.HideFromJava(mb);
                    ilgen = Context.CodeEmitterFactory.Create(mb);
                    ilgen.Emit(OpCodes.Ldarg_0);
                    ilgen.Emit(OpCodes.Ldfld, ghostRefField);
                    ilgen.Emit(OpCodes.Ret);
                    ilgen.DoEmit();

                    // Implement the "CastArray" method
                    // NOTE unlike "Cast" this doesn't return anything, it just throws a ClassCastException if the
                    // cast is unsuccessful. Also, because of the complexity of this test, we call IsInstanceArray
                    // instead of reimplementing the check here.
                    mb = ghostCastArrayMethod;
                    Context.AttributeHelper.HideFromJava(mb);
                    ilgen = Context.CodeEmitterFactory.Create(mb);
                    end = ilgen.DefineLabel();
                    ilgen.Emit(OpCodes.Ldarg_0);
                    ilgen.EmitBrfalse(end);
                    ilgen.Emit(OpCodes.Ldarg_0);
                    ilgen.Emit(OpCodes.Ldarg_1);
                    ilgen.Emit(OpCodes.Call, ghostIsInstanceArrayMethod);
                    ilgen.EmitBrtrue(end);
                    ilgen.Emit(OpCodes.Ldarg_0);
                    ilgen.Emit(OpCodes.Ldtoken, typeBuilder);
                    ilgen.Emit(OpCodes.Ldarg_1);
                    ilgen.Emit(OpCodes.Call, Context.Resolver.ResolveRuntimeType(typeof(IKVM.Runtime.GhostTag).FullName).AsReflection().GetMethod("ThrowClassCastException", BindingFlags.NonPublic | BindingFlags.Static));
                    ilgen.MarkLabel(end);
                    ilgen.Emit(OpCodes.Ret);
                    ilgen.DoEmit();

                    // Implement the "Equals" method
                    mb = typeBuilder.DefineMethod("Equals", MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Virtual, Context.Types.Boolean, new Type[] { Context.Types.Object });
                    Context.AttributeHelper.HideFromJava(mb);
                    ilgen = Context.CodeEmitterFactory.Create(mb);
                    ilgen.Emit(OpCodes.Ldarg_0);
                    ilgen.Emit(OpCodes.Ldfld, ghostRefField);
                    ilgen.Emit(OpCodes.Ldarg_1);
                    ilgen.Emit(OpCodes.Ceq);
                    ilgen.Emit(OpCodes.Ret);
                    ilgen.DoEmit();

                    // Implement the "GetHashCode" method
                    mb = typeBuilder.DefineMethod("GetHashCode", MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Virtual, Context.Types.Int32, Type.EmptyTypes);
                    Context.AttributeHelper.HideFromJava(mb);
                    ilgen = Context.CodeEmitterFactory.Create(mb);
                    ilgen.Emit(OpCodes.Ldarg_0);
                    ilgen.Emit(OpCodes.Ldfld, ghostRefField);
                    ilgen.Emit(OpCodes.Callvirt, Context.Types.Object.GetMethod("GetHashCode"));
                    ilgen.Emit(OpCodes.Ret);
                    ilgen.DoEmit();

                    // Implement the "op_Equality" method
                    mb = typeBuilder.DefineMethod("op_Equality", MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.SpecialName, Context.Types.Boolean, new Type[] { typeBuilder, typeBuilder });
                    Context.AttributeHelper.HideFromJava(mb);
                    ilgen = Context.CodeEmitterFactory.Create(mb);
                    ilgen.EmitLdarga(0);
                    ilgen.Emit(OpCodes.Ldfld, ghostRefField);
                    ilgen.EmitLdarga(1);
                    ilgen.Emit(OpCodes.Ldfld, ghostRefField);
                    ilgen.Emit(OpCodes.Ceq);
                    ilgen.Emit(OpCodes.Ret);
                    ilgen.DoEmit();

                    // Implement the "op_Inequality" method
                    mb = typeBuilder.DefineMethod("op_Inequality", MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Static | MethodAttributes.SpecialName, Context.Types.Boolean, new Type[] { typeBuilder, typeBuilder });
                    Context.AttributeHelper.HideFromJava(mb);
                    ilgen = Context.CodeEmitterFactory.Create(mb);
                    ilgen.EmitLdarga(0);
                    ilgen.Emit(OpCodes.Ldfld, ghostRefField);
                    ilgen.EmitLdarga(1);
                    ilgen.Emit(OpCodes.Ldfld, ghostRefField);
                    ilgen.Emit(OpCodes.Ceq);
                    ilgen.Emit(OpCodes.Ldc_I4_0);
                    ilgen.Emit(OpCodes.Ceq);
                    ilgen.Emit(OpCodes.Ret);
                    ilgen.DoEmit();
                }
            }
        }

        protected override void FinishGhostStep2()
        {
            typeBuilderGhostInterface?.CreateType();
        }

        protected override TypeBuilder DefineGhostType(string mangledTypeName, TypeAttributes typeAttribs)
        {
            typeAttribs &= ~(TypeAttributes.Interface | TypeAttributes.Abstract);
            typeAttribs |= TypeAttributes.Class | TypeAttributes.Sealed;
            TypeBuilder typeBuilder = classLoader.GetTypeWrapperFactory().ModuleBuilder.DefineType(mangledTypeName, typeAttribs, Context.Types.ValueType);
            Context.AttributeHelper.SetGhostInterface(typeBuilder);
            Context.AttributeHelper.SetModifiers(typeBuilder, Modifiers, IsInternal);
            ghostRefField = typeBuilder.DefineField("__<ref>", Context.Types.Object, FieldAttributes.Public | FieldAttributes.SpecialName);
            typeBuilderGhostInterface = typeBuilder.DefineNestedType("__Interface", TypeAttributes.Interface | TypeAttributes.Abstract | TypeAttributes.NestedPublic);
            Context.AttributeHelper.HideFromJava(typeBuilderGhostInterface);
            ghostIsInstanceMethod = typeBuilder.DefineMethod("IsInstance", MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Static, Context.Types.Boolean, new Type[] { Context.Types.Object });
            ghostIsInstanceMethod.DefineParameter(1, ParameterAttributes.None, "obj");
            ghostIsInstanceArrayMethod = typeBuilder.DefineMethod("IsInstanceArray", MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Static, Context.Types.Boolean, new Type[] { Context.Types.Object, Context.Types.Int32 });
            ghostIsInstanceArrayMethod.DefineParameter(1, ParameterAttributes.None, "obj");
            ghostIsInstanceArrayMethod.DefineParameter(2, ParameterAttributes.None, "rank");
            ghostCastMethod = typeBuilder.DefineMethod("Cast", MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Static, typeBuilder, new Type[] { Context.Types.Object });
            ghostCastMethod.DefineParameter(1, ParameterAttributes.None, "obj");
            ghostCastArrayMethod = typeBuilder.DefineMethod("CastArray", MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Static, Context.Types.Void, new Type[] { Context.Types.Object, Context.Types.Int32 });
            ghostCastArrayMethod.DefineParameter(1, ParameterAttributes.None, "obj");
            ghostCastArrayMethod.DefineParameter(2, ParameterAttributes.None, "rank");
            return typeBuilder;
        }

        internal override FieldInfo GhostRefField => ghostRefField;

        internal override void EmitCheckcast(CodeEmitter ilgen)
        {
            if (IsGhost)
            {
                ilgen.Emit(OpCodes.Dup);
                ilgen.Emit(OpCodes.Call, ghostCastMethod);
                ilgen.Emit(OpCodes.Pop);
            }
            else if (IsGhostArray)
            {
                ilgen.Emit(OpCodes.Dup);
                RuntimeJavaType tw = this;
                int rank = 0;
                while (tw.IsArray)
                {
                    rank++;
                    tw = tw.ElementTypeWrapper;
                }
                ilgen.EmitLdc_I4(rank);
                ilgen.Emit(OpCodes.Call, ghostCastArrayMethod);
                ilgen.Emit(OpCodes.Castclass, RuntimeArrayJavaType.MakeArrayType(Context.Types.Object, rank));
            }
            else
            {
                base.EmitCheckcast(ilgen);
            }
        }

        internal override void EmitInstanceOf(CodeEmitter ilgen)
        {
            if (IsGhost)
            {
                ilgen.Emit(OpCodes.Call, ghostIsInstanceMethod);
            }
            else if (IsGhostArray)
            {
                ilgen.Emit(OpCodes.Call, ghostIsInstanceArrayMethod);
            }
            else
            {
                base.EmitInstanceOf(ilgen);
            }
        }

        internal void SetAnnotation(Annotation annotation)
        {
            this.annotation = annotation;
        }

        internal override Annotation Annotation => annotation;

        internal void SetEnumType(Type enumType)
        {
            this.enumType = enumType;
        }

        internal override Type EnumType => enumType;

        sealed class ReplacedMethodWrapper : RuntimeJavaMethod
        {

            readonly InstructionList code;

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="tw"></param>
            /// <param name="name"></param>
            /// <param name="sig"></param>
            /// <param name="code"></param>
            internal ReplacedMethodWrapper(RuntimeJavaType tw, string name, string sig, IKVM.Tools.Importer.MapXml.InstructionList code) :
                base(tw, name, sig, null, null, null, Modifiers.Public, MemberFlags.None)
            {
                this.code = code;
            }

            /// <summary>
            /// Initializes a new instance.
            /// </summary>
            /// <param name="cpi"></param>
            /// <param name="code"></param>
            internal ReplacedMethodWrapper(ConstantPoolItemMI cpi, IKVM.Tools.Importer.MapXml.InstructionList code) :
                base(cpi.GetClassType(), cpi.Name, cpi.Signature, null, cpi.GetRetType(), cpi.GetArgTypes(), Modifiers.Public, MemberFlags.None)
            {
                this.code = code;
            }

            protected override void DoLinkMethod()
            {

            }

            private void DoEmit(CodeEmitter ilgen)
            {
                var context = new IKVM.Tools.Importer.MapXml.CodeGenContext(DeclaringType.ClassLoader);
                // we don't want the line numbers from map.xml, so we have our own emit loop
                for (int i = 0; i < code.Instructions.Length; i++)
                    code.Instructions[i].Generate(context, ilgen);
            }

            internal override void EmitCall(CodeEmitter ilgen)
            {
                DoEmit(ilgen);
            }

            internal override void EmitCallvirt(CodeEmitter ilgen)
            {
                DoEmit(ilgen);
            }

            internal override void EmitNewobj(CodeEmitter ilgen)
            {
                DoEmit(ilgen);
            }

        }

        internal override RuntimeJavaMethod[] GetReplacedMethodsFor(RuntimeJavaMethod mw)
        {
            var replacedMethods = ((ImportClassLoader)ClassLoader).GetReplacedMethodsFor(mw);
            var baseReplacedMethodWrappers = base.GetReplacedMethodsFor(mw);
            if (replacedMethods != null || baseReplacedMethodWrappers != null || this.replacedMethods != null)
            {
                var list = new List<RuntimeJavaMethod>();

                if (replacedMethods != null)
                    for (int i = 0; i < replacedMethods.Length; i++)
                        list.Add(new ReplacedMethodWrapper(ClassLoader.LoadClassByName(replacedMethods[i].Class), replacedMethods[i].Name, replacedMethods[i].Sig, replacedMethods[i].Code));

                if (baseReplacedMethodWrappers != null)
                    list.AddRange(baseReplacedMethodWrappers);

                if (this.replacedMethods != null)
                    list.AddRange(this.replacedMethods);

                return list.ToArray();
            }

            return null;
        }

        internal override bool IsFastClassLiteralSafe
        {
            get { return true; }
        }

        internal RuntimeJavaMethod ReplaceMethodWrapper(RuntimeJavaMethod mw)
        {
            if (replacedMethods != null)
                foreach (var r in replacedMethods)
                    if (mw.DeclaringType == r.DeclaringType && mw.Name == r.Name && mw.Signature == r.Signature)
                        return r;

            return mw;
        }

    }

}
