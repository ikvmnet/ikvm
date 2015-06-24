/*
 * Copyright (c) 2012, 2013, Oracle and/or its affiliates. All rights reserved.
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.  Oracle designates this
 * particular file as subject to the "Classpath" exception as provided
 * by Oracle in the LICENSE file that accompanied this code.
 *
 * This code is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
 * version 2 for more details (a copy is included in the LICENSE file that
 * accompanied this code).
 *
 * You should have received a copy of the GNU General Public License version
 * 2 along with this work; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 *
 * Please contact Oracle, 500 Oracle Parkway, Redwood Shores, CA 94065 USA
 * or visit www.oracle.com if you need additional information or have any
 * questions.
 */

// [IKVM] Based on original from OpenJDK, but heavily modified to directly generate a DynamicMethod,
// instead of Java bytecode.
// Copyright (C) 2015 Jeroen Frijters

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using IKVM.Attributes;
using IKVM.Internal;
using java.lang.invoke;
#if !FIRST_PASS
using BasicType = java.lang.invoke.LambdaForm.BasicType;
using Class = java.lang.Class;
using Name = java.lang.invoke.LambdaForm.Name;
using Opcodes = jdk.@internal.org.objectweb.asm.Opcodes.__Fields;
using VerifyType = sun.invoke.util.VerifyType;
using Wrapper = sun.invoke.util.Wrapper;
#endif

sealed class NativeInvokerBytecodeGenerator
{
#if FIRST_PASS
    public static MemberName generateCustomizedCode(LambdaForm form, MethodType invokerType)
    {
        return null;
    }
#else
    private readonly LambdaForm lambdaForm;
    private readonly MethodType invokerType;
    private readonly Type delegateType;
    private readonly DynamicMethod dm;
    private readonly CodeEmitter ilgen;
    private readonly int packedArgPos;
    private readonly Type packedArgType;
    private readonly CodeEmitterLocal[] locals;
    private readonly List<object> constants = new List<object>();

    private enum Bailout
    {
        NotBasicType,
        UnsupportedIntrinsic,
        UnsupportedArrayType,
        UnsupportedRefKind,
        UnsupportedConstant,
        NotStaticallyInvocable,
        PreconditionViolated,
    }

    private sealed class BailoutException : Exception
    {
        internal BailoutException(Bailout reason, object data)
            : base("BAILOUT " + reason + ": " + data)
        {
        }
    }

    private NativeInvokerBytecodeGenerator(LambdaForm lambdaForm, MethodType invokerType)
    {
        if (invokerType != invokerType.basicType())
        {
            throw new BailoutException(Bailout.NotBasicType, invokerType);
        }
        this.lambdaForm = lambdaForm;
        this.invokerType = invokerType;
        this.delegateType = MethodHandleUtil.GetMemberWrapperDelegateType(invokerType);
        MethodInfo mi = MethodHandleUtil.GetDelegateInvokeMethod(delegateType);
        Type[] paramTypes = MethodHandleUtil.GetParameterTypes(typeof(object[]), mi);
        // HACK the code we generate is not verifiable (known issue: locals aren't typed correctly), so we stick the DynamicMethod into mscorlib (a security critical assembly)
        this.dm = new DynamicMethod(lambdaForm.debugName, mi.ReturnType, paramTypes, typeof(object).Module, true);
        this.ilgen = CodeEmitter.Create(this.dm);
        if (invokerType.parameterCount() > MethodHandleUtil.MaxArity)
        {
            this.packedArgType = paramTypes[paramTypes.Length - 1];
            this.packedArgPos = paramTypes.Length - 1;
        }
        else
        {
            this.packedArgPos = Int32.MaxValue;
        }

        locals = new CodeEmitterLocal[lambdaForm.names.Length];
        for (int i = lambdaForm._arity(); i < lambdaForm.names.Length; i++)
        {
            Name name = lambdaForm.names[i];
            if (name.index() != i)
            {
                throw new BailoutException(Bailout.PreconditionViolated, "name.index() != i");
            }
            switch (name.typeChar())
            {
                case 'L':
                    locals[i] = ilgen.DeclareLocal(Types.Object);
                    break;
                case 'I':
                    locals[i] = ilgen.DeclareLocal(Types.Int32);
                    break;
                case 'J':
                    locals[i] = ilgen.DeclareLocal(Types.Int64);
                    break;
                case 'F':
                    locals[i] = ilgen.DeclareLocal(Types.Single);
                    break;
                case 'D':
                    locals[i] = ilgen.DeclareLocal(Types.Double);
                    break;
                case 'V':
                    break;
                default:
                    throw new BailoutException(Bailout.PreconditionViolated, "Unsupported typeChar(): " + name.typeChar());
            }
        }
    }

    /*
     * Low-level emit helpers.
     */
    private void emitConst(object con) {
        if (con == null) {
            ilgen.Emit(OpCodes.Ldnull);
        } else if (con is string) {
            ilgen.Emit(OpCodes.Ldstr, (string)con);
        } else if (con is java.lang.Integer) {
            ilgen.EmitLdc_I4(((java.lang.Integer)con).intValue());
        } else if (con is java.lang.Long) {
            ilgen.EmitLdc_I8(((java.lang.Long)con).longValue());
        } else if (con is java.lang.Float) {
            ilgen.EmitLdc_R4(((java.lang.Float)con).floatValue());
        } else if (con is java.lang.Double) {
            ilgen.EmitLdc_R8(((java.lang.Double)con).doubleValue());
        } else if (con is java.lang.Boolean) {
            ilgen.EmitLdc_I4(((java.lang.Boolean)con).booleanValue() ? 1 : 0);
        } else {
            throw new BailoutException(Bailout.UnsupportedConstant, con);
        }
    }

    private void emitIconstInsn(int i) {
        ilgen.EmitLdc_I4(i);
    }

    /*
     * NOTE: These load/store methods use the localsMap to find the correct index!
     */
    private void emitLoadInsn(BasicType type, int index) {
        // [IKVM] we don't need the localsMap (it is used to correct for long/double taking two slots)
        if (locals[index] == null) {
            MethodHandleUtil.LoadPackedArg(ilgen, index, 1, packedArgPos, packedArgType);
        } else {
            ilgen.Emit(OpCodes.Ldloc, locals[index]);
        }
    }

    private void emitStoreInsn(BasicType type, int index) {
        ilgen.Emit(OpCodes.Stloc, locals[index]);
    }

    private void emitAstoreInsn(int index) {
        emitStoreInsn(BasicType.L_TYPE, index);
    }

    private byte arrayTypeCode(Wrapper elementType) {
        switch (elementType.name()) {
            case "BOOLEAN": return Opcodes.T_BOOLEAN;
            case "BYTE":    return Opcodes.T_BYTE;
            case "CHAR":    return Opcodes.T_CHAR;
            case "SHORT":   return Opcodes.T_SHORT;
            case "INT":     return Opcodes.T_INT;
            case "LONG":    return Opcodes.T_LONG;
            case "FLOAT":   return Opcodes.T_FLOAT;
            case "DOUBLE":  return Opcodes.T_DOUBLE;
            case "OBJECT":  return 0; // in place of Opcodes.T_OBJECT
            default:        throw new BailoutException(Bailout.PreconditionViolated, "elemendType = " + elementType);
        }
    }

    private OpCode arrayInsnOpcode(byte tcode)
    {
        switch (tcode)
        {
            case Opcodes.T_BOOLEAN:
            case Opcodes.T_BYTE:
                return OpCodes.Stelem_I1;
            case Opcodes.T_CHAR:
            case Opcodes.T_SHORT:
                return OpCodes.Stelem_I2;
            case Opcodes.T_INT:
                return OpCodes.Stelem_I4;
            case Opcodes.T_LONG:
                return OpCodes.Stelem_I8;
            case Opcodes.T_FLOAT:
                return OpCodes.Stelem_R4;
            case Opcodes.T_DOUBLE:
                return OpCodes.Stelem_R8;
            case 0:
                return OpCodes.Stelem_Ref;
            default:
                throw new BailoutException(Bailout.PreconditionViolated, "tcode = " + tcode);
        }
    }

    /**
     * Emit an implicit conversion for an argument which must be of the given pclass.
     * This is usually a no-op, except when pclass is a subword type or a reference other than Object or an interface.
     *
     * @param ptype type of value present on stack
     * @param pclass type of value required on stack
     * @param arg compile-time representation of value on stack (Node, constant) or null if none
     */
    private void emitImplicitConversion(BasicType ptype, Class pclass, object arg) {
        //assert(basicType(pclass) == ptype);  // boxing/unboxing handled by caller
        if (pclass == ptype.basicTypeClass() && ptype != BasicType.L_TYPE)
            return;   // nothing to do
        switch (ptype.name()) {
            case "L_TYPE":
                if (VerifyType.isNullConversion(CoreClasses.java.lang.Object.Wrapper.ClassObject, pclass, false)) {
                    //if (PROFILE_LEVEL > 0)
                    //    emitReferenceCast(Object.class, arg);
                    return;
                }
                emitReferenceCast(pclass, arg);
                return;
            case "I_TYPE":
                if (!VerifyType.isNullConversion(java.lang.Integer.TYPE, pclass, false))
                    emitPrimCast(ptype.basicTypeWrapper(), Wrapper.forPrimitiveType(pclass));
                return;
        }
        throw new BailoutException(Bailout.PreconditionViolated, "bad implicit conversion: tc=" + ptype + ": " + pclass);
    }

    /** Update localClasses type map.  Return true if the information is already present. */
    private void assertStaticType(Class cls, Name n) {
        // [IKVM] not implemented
    }

    private void emitReferenceCast(Class cls, object arg) {
        // [IKVM] handle the type system hole that is caused by arrays being both derived from cli.System.Array and directly from java.lang.Object
        if (cls != CoreClasses.cli.System.Object.Wrapper.ClassObject)
        {
            TypeWrapper.FromClass(cls).EmitCheckcast(ilgen);
        }
    }

    private sealed class AnonymousClass : TypeWrapper
    {
        internal static readonly Class Instance = new AnonymousClass().ClassObject;

        private AnonymousClass()
            : base(TypeFlags.Anonymous, Modifiers.Super | Modifiers.Final, "java.lang.invoke.LambdaForm$MH")
        {
        }

        internal override ClassLoaderWrapper GetClassLoader()
        {
            return ClassLoaderWrapper.GetBootstrapClassLoader();
        }

        internal override Type TypeAsTBD
        {
            get { throw new InvalidOperationException(); }
        }

        internal override TypeWrapper BaseTypeWrapper
        {
            get { return CoreClasses.java.lang.Object.Wrapper; }
        }
    }

    /**
     * Generate customized bytecode for a given LambdaForm.
     */
    public static MemberName generateCustomizedCode(LambdaForm form, MethodType invokerType)
    {
        try
        {
            MemberName memberName = new MemberName();
            memberName._clazz(AnonymousClass.Instance);
            memberName._name(form.debugName);
            memberName._type(invokerType);
            memberName._flags(MethodHandleNatives.Constants.MN_IS_METHOD | MethodHandleNatives.Constants.ACC_STATIC | (MethodHandleNatives.Constants.REF_invokeStatic << MethodHandleNatives.Constants.MN_REFERENCE_KIND_SHIFT));
            memberName.vmtarget = new NativeInvokerBytecodeGenerator(form, invokerType).generateCustomizedCodeBytes();
            return memberName;
        }
#if DEBUG
        catch (BailoutException x)
        {
            Console.WriteLine(x.Message);
            Console.WriteLine("generateCustomizedCode: " + form + ", " + invokerType);
        }
#else
        catch (BailoutException)
        {
        }
#endif
        return InvokerBytecodeGenerator.generateCustomizedCode(form, invokerType);
    }

    /**
     * Generate an invoker method for the passed {@link LambdaForm}.
     */
    private Delegate generateCustomizedCodeBytes() {
        // iterate over the form's names, generating bytecode instructions for each
        // start iterating at the first name following the arguments
        Name onStack = null;
        for (int i = lambdaForm._arity(); i < lambdaForm.names.Length; i++) {
            Name name = lambdaForm.names[i];

            emitStoreResult(onStack);
            onStack = name;  // unless otherwise modified below
            MethodHandleImpl.Intrinsic intr = name.function.intrinsicName();
            switch (intr.name()) {
                case "SELECT_ALTERNATIVE":
                    //assert isSelectAlternative(i);
                    onStack = emitSelectAlternative(name, lambdaForm.names[i+1]);
                    i++;  // skip MH.invokeBasic of the selectAlternative result
                    continue;
                case "GUARD_WITH_CATCH":
                    //assert isGuardWithCatch(i);
                    onStack = emitGuardWithCatch(i);
                    i = i+2; // Jump to the end of GWC idiom
                    continue;
                case "NEW_ARRAY":
                    Class rtype = name.function.methodType().returnType();
                    if (InvokerBytecodeGenerator.isStaticallyNameable(rtype)) {
                        emitNewArray(name);
                        continue;
                    }
                    break;
                case "ARRAY_LOAD":
                    emitArrayLoad(name);
                    continue;
                case "IDENTITY":
                    //assert(name.arguments.length == 1);
                    emitPushArguments(name);
                    continue;
                case "NONE":
                    // no intrinsic associated
                    break;
                // [IKVM] ARRAY_STORE and ZERO appear to be unused
                default:
                    throw new BailoutException(Bailout.UnsupportedIntrinsic, "Unknown intrinsic: "+intr);
            }

            MemberName member = name.function._member();
            if (isStaticallyInvocable(member)) {
                emitStaticInvoke(member, name);
            } else {
                emitInvoke(name);
            }
        }

        // return statement
        emitReturn(onStack);

        ilgen.DoEmit();
        return dm.CreateDelegate(delegateType, constants.ToArray());
    }

    void emitArrayLoad(Name name) {
        OpCode arrayOpcode = OpCodes.Ldelem_Ref;
        Class elementType = name.function.methodType().parameterType(0).getComponentType();
        emitPushArguments(name);
        if (elementType.isPrimitive()) {
            Wrapper w = Wrapper.forPrimitiveType(elementType);
            arrayOpcode = arrayLoadOpcode(arrayTypeCode(w));
        }
        ilgen.Emit(arrayOpcode);
    }

    /**
     * Emit an invoke for the given name.
     */
    void emitInvoke(Name name) {
        //assert(!isLinkerMethodInvoke(name));  // should use the static path for these
        if (true) {
            // push receiver
            MethodHandle target = name.function._resolvedHandle();
            //assert(target != null) : name.exprString();
            //mv.visitLdcInsn(constantPlaceholder(target));
            EmitConstant(target);
            emitReferenceCast(CoreClasses.java.lang.invoke.MethodHandle.Wrapper.ClassObject, target);
        } else {
            // load receiver
            //emitAloadInsn(0);
            //emitReferenceCast(MethodHandle.class, null);
            //mv.visitFieldInsn(Opcodes.GETFIELD, MH, "form", LF_SIG);
            //mv.visitFieldInsn(Opcodes.GETFIELD, LF, "names", LFN_SIG);
            // TODO more to come
        }

        // push arguments
        emitPushArguments(name);

        // invocation
        MethodType type = name.function.methodType();
        //mv.visitMethodInsn(Opcodes.INVOKEVIRTUAL, MH, "invokeBasic", type.basicType().toMethodDescriptorString(), false);
        EmitInvokeBasic(type.basicType());
    }

    static bool isStaticallyInvocable(MemberName member) {
        if (member == null)  return false;
        if (member.isConstructor())  return false;
        Class cls = member.getDeclaringClass();
        if (cls.isArray() || cls.isPrimitive())
            return false;  // FIXME
        /*
        if (cls.isAnonymousClass() || cls.isLocalClass())
            return false;  // inner class of some sort
        if (cls.getClassLoader() != MethodHandle.class.getClassLoader())
            return false;  // not on BCP
        if (ReflectUtil.isVMAnonymousClass(cls)) // FIXME: switch to supported API once it is added
            return false;
        MethodType mtype = member.getMethodOrFieldType();
        if (!isStaticallyNameable(mtype.returnType()))
            return false;
        for (Class<?> ptype : mtype.parameterArray())
            if (!isStaticallyNameable(ptype))
                return false;
        if (!member.isPrivate() && VerifyAccess.isSamePackage(MethodHandle.class, cls))
            return true;   // in java.lang.invoke package
        if (member.isPublic() && isStaticallyNameable(cls))
            return true;
        */
        if (member.isMethod()) {
            // [IKVM] If we can't call the method directly, invoke it via the invokeBasic infrastructure.
            return IsMethodHandleLinkTo(member)
                || IsMethodHandleInvokeBasic(member)
                || IsStaticallyInvocable(GetMethodWrapper(member));
        }
        if (member.isField()) {
            // [IKVM] If we can't access the field directly, use the invokeBasic infrastructure.
            return IsStaticallyInvocable(GetFieldWrapper(member));
        }
        return false;
    }

	/*
    static boolean isStaticallyNameable(Class<?> cls) {
        if (cls == Object.class)
            return true;
        while (cls.isArray())
            cls = cls.getComponentType();
        if (cls.isPrimitive())
            return true;  // int[].class, for example
        if (ReflectUtil.isVMAnonymousClass(cls)) // FIXME: switch to supported API once it is added
            return false;
        // could use VerifyAccess.isClassAccessible but the following is a safe approximation
        if (cls.getClassLoader() != Object.class.getClassLoader())
            return false;
        if (VerifyAccess.isSamePackage(MethodHandle.class, cls))
            return true;
        if (!Modifier.isPublic(cls.getModifiers()))
            return false;
        for (Class<?> pkgcls : STATICALLY_INVOCABLE_PACKAGES) {
            if (VerifyAccess.isSamePackage(pkgcls, cls))
                return true;
        }
        return false;
	}
	*/

    void emitStaticInvoke(Name name) {
        emitStaticInvoke(name.function._member(), name);
    }

    /**
     * Emit an invoke for the given name, using the MemberName directly.
     */
    void emitStaticInvoke(MemberName member, Name name) {
        // push arguments
        emitPushArguments(name);

        // invocation
        if (member.isMethod()) {
            if (IsMethodHandleLinkTo(member)) {
                MethodType mt = member.getMethodType();
                TypeWrapper[] args = new TypeWrapper[mt.parameterCount()];
                for (int j = 0; j < args.Length; j++) {
                    args[j] = TypeWrapper.FromClass(mt.parameterType(j));
                    args[j].Finish();
                }
                TypeWrapper ret = TypeWrapper.FromClass(mt.returnType());
                ret.Finish();
                Compiler.MethodHandleMethodWrapper.EmitLinkToCall(ilgen, args, ret);
                ret.EmitConvSignatureTypeToStackType(ilgen);
            } else if (IsMethodHandleInvokeBasic(member)) {
                EmitInvokeBasic(member.getMethodType());
            } else {
                switch (member.getReferenceKind()) {
                    case MethodHandleNatives.Constants.REF_invokeInterface:
                    case MethodHandleNatives.Constants.REF_invokeSpecial:
                    case MethodHandleNatives.Constants.REF_invokeStatic:
                    case MethodHandleNatives.Constants.REF_invokeVirtual:
                        break;
                    default:
                        throw new BailoutException(Bailout.UnsupportedRefKind, member);
                }
                MethodWrapper mw = GetMethodWrapper(member);
                if (!IsStaticallyInvocable(mw)) {
                    throw new BailoutException(Bailout.NotStaticallyInvocable, member);
                }
                mw.Link();
                mw.DeclaringType.Finish();
                mw.ResolveMethod();
                if (mw.HasCallerID) {
                    EmitConstant(DynamicCallerIDProvider.Instance);
                    ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.DynamicCallerID);
                }
                if (mw.IsStatic || member.getReferenceKind() == MethodHandleNatives.Constants.REF_invokeSpecial) {
                    mw.EmitCall(ilgen);
                } else {
                    mw.EmitCallvirt(ilgen);
                }
                mw.ReturnType.EmitConvSignatureTypeToStackType(ilgen);
            }
        } else if (member.isField()) {
            FieldWrapper fw = GetFieldWrapper(member);
            if (!IsStaticallyInvocable(fw)) {
                throw new BailoutException(Bailout.NotStaticallyInvocable, member);
            }
            fw.Link();
            fw.DeclaringType.Finish();
            fw.ResolveField();
            switch (member.getReferenceKind()) {
                case MethodHandleNatives.Constants.REF_getField:
                case MethodHandleNatives.Constants.REF_getStatic:
                    fw.EmitGet(ilgen);
                    fw.FieldTypeWrapper.EmitConvSignatureTypeToStackType(ilgen);
                    break;
                case MethodHandleNatives.Constants.REF_putField:
                case MethodHandleNatives.Constants.REF_putStatic:
                    fw.EmitSet(ilgen);
                    break;
                default:
                    throw new BailoutException(Bailout.UnsupportedRefKind, member);
            }
        } else {
            throw new BailoutException(Bailout.NotStaticallyInvocable, member);
        }
    }

    void emitNewArray(Name name) {
        Class rtype = name.function.methodType().returnType();
        if (name.arguments.Length == 0) {
            // The array will be a constant.
            object emptyArray;
            try {
                emptyArray = name.function._resolvedHandle().invoke();
            } catch (Exception ex) {
                throw new java.lang.InternalError(ex);
            }
            //assert(java.lang.reflect.Array.getLength(emptyArray) == 0);
            //assert(emptyArray.getClass() == rtype);  // exact typing
            //mv.visitLdcInsn(constantPlaceholder(emptyArray));
            EmitConstant(emptyArray);
            emitReferenceCast(rtype, emptyArray);
            return;
        }
        Class arrayElementType = rtype.getComponentType();
        //assert(arrayElementType != null);
        emitIconstInsn(name.arguments.Length);
        OpCode xas = OpCodes.Stelem_Ref;
        if (!arrayElementType.isPrimitive()) {
            TypeWrapper tw = TypeWrapper.FromClass(arrayElementType);
            if (tw.IsUnloadable || tw.IsGhost || tw.IsGhostArray || tw.IsNonPrimitiveValueType) {
                throw new BailoutException(Bailout.UnsupportedArrayType, tw);
            }
            ilgen.Emit(OpCodes.Newarr, tw.TypeAsArrayType);
        } else {
            byte tc = arrayTypeCode(Wrapper.forPrimitiveType(arrayElementType));
            xas = arrayInsnOpcode(tc);
            //mv.visitIntInsn(Opcodes.NEWARRAY, tc);
            ilgen.Emit(OpCodes.Newarr, TypeWrapper.FromClass(arrayElementType).TypeAsArrayType);
        }
        // store arguments
        for (int i = 0; i < name.arguments.Length; i++) {
            //mv.visitInsn(Opcodes.DUP);
            ilgen.Emit(OpCodes.Dup);
            emitIconstInsn(i);
            emitPushArgument(name, i);
            //mv.visitInsn(xas);
            ilgen.Emit(xas);
        }
        // the array is left on the stack
        assertStaticType(rtype, name);
    }

    /**
     * Emit bytecode for the selectAlternative idiom.
     *
     * The pattern looks like (Cf. MethodHandleImpl.makeGuardWithTest):
     * <blockquote><pre>{@code
     *   Lambda(a0:L,a1:I)=>{
     *     t2:I=foo.test(a1:I);
     *     t3:L=MethodHandleImpl.selectAlternative(t2:I,(MethodHandle(int)int),(MethodHandle(int)int));
     *     t4:I=MethodHandle.invokeBasic(t3:L,a1:I);t4:I}
     * }</pre></blockquote>
     */
    private Name emitSelectAlternative(Name selectAlternativeName, Name invokeBasicName) {
        //assert isStaticallyInvocable(invokeBasicName);

        Name receiver = (Name) invokeBasicName.arguments[0];

        CodeEmitterLabel L_fallback = ilgen.DefineLabel();
        CodeEmitterLabel L_done = ilgen.DefineLabel();

        // load test result
        emitPushArgument(selectAlternativeName, 0);

        // if_icmpne L_fallback
        ilgen.EmitBrfalse(L_fallback);

        // invoke selectAlternativeName.arguments[1]
        //Class<?>[] preForkClasses = localClasses.clone();
        emitPushArgument(selectAlternativeName, 1);  // get 2nd argument of selectAlternative
        emitAstoreInsn(receiver.index());  // store the MH in the receiver slot
        emitStaticInvoke(invokeBasicName);

        // goto L_done
        ilgen.EmitBr(L_done);

        // L_fallback:
        ilgen.MarkLabel(L_fallback);

        // invoke selectAlternativeName.arguments[2]
        //System.arraycopy(preForkClasses, 0, localClasses, 0, preForkClasses.length);
        emitPushArgument(selectAlternativeName, 2);  // get 3rd argument of selectAlternative
        emitAstoreInsn(receiver.index());  // store the MH in the receiver slot
        emitStaticInvoke(invokeBasicName);

        // L_done:
        ilgen.MarkLabel(L_done);
        // for now do not bother to merge typestate; just reset to the dominator state
        //System.arraycopy(preForkClasses, 0, localClasses, 0, preForkClasses.length);

        return invokeBasicName;  // return what's on stack
    }

    /**
      * Emit bytecode for the guardWithCatch idiom.
      *
      * The pattern looks like (Cf. MethodHandleImpl.makeGuardWithCatch):
      * <blockquote><pre>{@code
      *  guardWithCatch=Lambda(a0:L,a1:L,a2:L,a3:L,a4:L,a5:L,a6:L,a7:L)=>{
      *    t8:L=MethodHandle.invokeBasic(a4:L,a6:L,a7:L);
      *    t9:L=MethodHandleImpl.guardWithCatch(a1:L,a2:L,a3:L,t8:L);
      *   t10:I=MethodHandle.invokeBasic(a5:L,t9:L);t10:I}
      * }</pre></blockquote>
      *
      * It is compiled into bytecode equivalent of the following code:
      * <blockquote><pre>{@code
      *  try {
      *      return a1.invokeBasic(a6, a7);
      *  } catch (Throwable e) {
      *      if (!a2.isInstance(e)) throw e;
      *      return a3.invokeBasic(ex, a6, a7);
      *  }}
      */
    private Name emitGuardWithCatch(int pos) {
        Name args    = lambdaForm.names[pos];
        Name invoker = lambdaForm.names[pos+1];
        Name result  = lambdaForm.names[pos+2];

        CodeEmitterLabel L_handler = ilgen.DefineLabel();
        CodeEmitterLabel L_done = ilgen.DefineLabel();

        Class returnType = result.function._resolvedHandle().type().returnType();
        MethodType type = args.function._resolvedHandle().type()
                              .dropParameterTypes(0,1)
                              .changeReturnType(returnType);

        // Normal case
        ilgen.BeginExceptionBlock();
        // load target
        emitPushArgument(invoker, 0);
        emitPushArguments(args, 1); // skip 1st argument: method handle
        EmitInvokeBasic(type.basicType());
        CodeEmitterLocal returnValue = null;
        if (returnType != java.lang.Void.TYPE) {
            returnValue = ilgen.DeclareLocal(TypeWrapper.FromClass(returnType).TypeAsLocalOrStackType);
            ilgen.Emit(OpCodes.Stloc, returnValue);
        }
        ilgen.EmitLeave(L_done);

        // Exceptional case
        ilgen.BeginCatchBlock(typeof(Exception));

        // [IKVM] map the exception and store it in a local and exit the handler
        ilgen.EmitLdc_I4(0);
        ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.mapException.MakeGenericMethod(typeof(Exception)));
        CodeEmitterLocal exception = ilgen.DeclareLocal(typeof(Exception));
        ilgen.Emit(OpCodes.Stloc, exception);
        ilgen.EmitLeave(L_handler);
        ilgen.EndExceptionBlock();

        // Check exception's type
        ilgen.MarkLabel(L_handler);
        // load exception class
        emitPushArgument(invoker, 1);
        ilgen.Emit(OpCodes.Ldloc, exception);
        CoreClasses.java.lang.Class.Wrapper.GetMethodWrapper("isInstance", "(Ljava.lang.Object;)Z", false).EmitCall(ilgen);
        CodeEmitterLabel L_rethrow = ilgen.DefineLabel();
        ilgen.EmitBrfalse(L_rethrow);

        // Invoke catcher
        // load catcher
        emitPushArgument(invoker, 2);
        ilgen.Emit(OpCodes.Ldloc, exception);
        emitPushArguments(args, 1); // skip 1st argument: method handle
        MethodType catcherType = type.insertParameterTypes(0, CoreClasses.java.lang.Throwable.Wrapper.ClassObject);
        EmitInvokeBasic(catcherType.basicType());
        if (returnValue != null) {
            ilgen.Emit(OpCodes.Stloc, returnValue);
        }
        ilgen.EmitBr(L_done);

        ilgen.MarkLabel(L_rethrow);
        ilgen.Emit(OpCodes.Ldloc, exception);
        ilgen.Emit(OpCodes.Call, Compiler.unmapExceptionMethod);
        ilgen.Emit(OpCodes.Throw);

        ilgen.MarkLabel(L_done);
        if (returnValue != null) {
            ilgen.Emit(OpCodes.Ldloc, returnValue);
        }

        return result;
    }

    private void emitPushArguments(Name args) {
        emitPushArguments(args, 0);
    }

    private void emitPushArguments(Name args, int start) {
        for (int i = start; i < args.arguments.Length; i++) {
            emitPushArgument(args, i);
        }
    }

    private void emitPushArgument(Name name, int paramIndex) {
        object arg = name.arguments[paramIndex];
        Class ptype = name.function.methodType().parameterType(paramIndex);
        emitPushArgument(ptype, arg);
    }

    private void emitPushArgument(Class ptype, object arg) {
        BasicType bptype = BasicType.basicType(ptype);
        if (arg is Name) {
            Name n = (Name)arg;
            emitLoadInsn(n._type(), n.index());
            emitImplicitConversion(n._type(), ptype, n);
        } else if ((arg == null || arg is string) && bptype == BasicType.L_TYPE) {
            emitConst(arg);
        } else {
            if (Wrapper.isWrapperType(ikvm.extensions.ExtensionMethods.getClass(arg)) && bptype != BasicType.L_TYPE) {
                emitConst(arg);
            } else {
                EmitConstant(arg);
                emitImplicitConversion(BasicType.L_TYPE, ptype, arg);
            }
        }
    }

    /**
     * Store the name to its local, if necessary.
     */
    private void emitStoreResult(Name name) {
        if (name != null && name._type() != BasicType.V_TYPE) {
            // non-void: actually assign
            emitStoreInsn(name._type(), name.index());
        }
    }

    /**
     * Emits a return statement from a LF invoker. If required, the result type is cast to the correct return type.
     */
    private void emitReturn(Name onStack) {
        // return statement
        Class rclass = invokerType.returnType();
        BasicType rtype = lambdaForm.returnType();
        //assert(rtype == basicType(rclass));  // must agree
        if (rtype == BasicType.V_TYPE) {
            // [IKVM] unlike the JVM, the CLR doesn't like left over values on the stack
            if (onStack != null && onStack._type() != BasicType.V_TYPE) {
                ilgen.Emit(OpCodes.Pop);
            }
        } else {
            LambdaForm.Name rn = lambdaForm.names[lambdaForm.result];

            // put return value on the stack if it is not already there
            if (rn != onStack) {
                emitLoadInsn(rtype, lambdaForm.result);
            }

            emitImplicitConversion(rtype, rclass, rn);
        }
        ilgen.Emit(OpCodes.Ret);
    }

    /**
     * Emit a type conversion bytecode casting from "from" to "to".
     */
    private void emitPrimCast(Wrapper from, Wrapper to) {
        // Here's how.
        // -   indicates forbidden
        // <-> indicates implicit
        //      to ----> boolean  byte     short    char     int      long     float    double
        // from boolean    <->        -        -        -        -        -        -        -
        //      byte        -       <->       i2s      i2c      <->      i2l      i2f      i2d
        //      short       -       i2b       <->      i2c      <->      i2l      i2f      i2d
        //      char        -       i2b       i2s      <->      <->      i2l      i2f      i2d
        //      int         -       i2b       i2s      i2c      <->      i2l      i2f      i2d
        //      long        -     l2i,i2b   l2i,i2s  l2i,i2c    l2i      <->      l2f      l2d
        //      float       -     f2i,i2b   f2i,i2s  f2i,i2c    f2i      f2l      <->      f2d
        //      double      -     d2i,i2b   d2i,i2s  d2i,i2c    d2i      d2l      d2f      <->
        if (from == to) {
            // no cast required, should be dead code anyway
            return;
        }
        if (from.isSubwordOrInt()) {
            // cast from {byte,short,char,int} to anything
            emitI2X(to);
        } else {
            // cast from {long,float,double} to anything
            if (to.isSubwordOrInt()) {
                // cast to {byte,short,char,int}
                emitX2I(from);
                if (to.bitWidth() < 32) {
                    // targets other than int require another conversion
                    emitI2X(to);
                }
            } else {
                // cast to {long,float,double} - this is verbose
                bool error = false;
                switch (from.name()) {
                case "LONG":
                    switch (to.name()) {
                    case "FLOAT":   ilgen.Emit(OpCodes.Conv_R4);  break;
                    case "DOUBLE":  ilgen.Emit(OpCodes.Conv_R8);  break;
                    default:        error = true;                 break;
                    }
                    break;
                case "FLOAT":
                    switch (to.name()) {
                    case "LONG":    ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.f2l); break;
                    case "DOUBLE":  ilgen.Emit(OpCodes.Conv_R8);  break;
                    default:        error = true;                 break;
                    }
                    break;
                case "DOUBLE":
                    switch (to.name()) {
                    case "LONG" :   ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.d2l); break;
                    case "FLOAT":   ilgen.Emit(OpCodes.Conv_R4);  break;
                    default:        error = true;                 break;
                    }
                    break;
                default:
                    error = true;
                    break;
                }
                if (error) {
                    throw new BailoutException(Bailout.PreconditionViolated, "unhandled prim cast: " + from + "2" + to);
                }
            }
        }
    }

    private void emitI2X(Wrapper type) {
        switch (type.name()) {
        case "BYTE":    ilgen.Emit(OpCodes.Conv_I1);  break;
        case "SHORT":   ilgen.Emit(OpCodes.Conv_I2);  break;
        case "CHAR":    ilgen.Emit(OpCodes.Conv_U2);  break;
        case "INT":     /* naught */                  break;
        case "LONG":    ilgen.Emit(OpCodes.Conv_I8);  break;
        case "FLOAT":   ilgen.Emit(OpCodes.Conv_R4);  break;
        case "DOUBLE":  ilgen.Emit(OpCodes.Conv_R8);  break;
        case "BOOLEAN":
            // For compatibility with ValueConversions and explicitCastArguments:
            ilgen.EmitLdc_I4(1);
            ilgen.Emit(OpCodes.And);
            break;
        default:   throw new BailoutException(Bailout.PreconditionViolated, "unknown type: " + type);
        }
    }

    private void emitX2I(Wrapper type) {
        switch (type.name()) {
        case "LONG":    ilgen.Emit(OpCodes.Conv_I4);  break;
        case "FLOAT":   ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.f2i);  break;
        case "DOUBLE":  ilgen.Emit(OpCodes.Call, ByteCodeHelperMethods.d2i);  break;
        default:        throw new BailoutException(Bailout.PreconditionViolated, "unknown type: " + type);
        }
    }

    private void EmitConstant(object obj)
    {
        if (obj == null)
        {
            ilgen.Emit(OpCodes.Ldnull);
            return;
        }
        int index = constants.IndexOf(obj);
        if (index == -1)
        {
            index = constants.Count;
            constants.Add(obj);
        }
        ilgen.EmitLdarg(0);	// we want the bound value, not the real first parameter
        ilgen.EmitLdc_I4(index);
        ilgen.Emit(OpCodes.Ldelem_Ref);
    }

    private void EmitInvokeBasic(MethodType mt)
    {
        TypeWrapper[] args = new TypeWrapper[mt.parameterCount()];
        for (int i = 0; i < args.Length; i++)
        {
            args[i] = TypeWrapper.FromClass(mt.parameterType(i));
            args[i].Finish();
        }
        TypeWrapper ret = TypeWrapper.FromClass(mt.returnType());
        ret.Finish();
        Compiler.MethodHandleMethodWrapper.EmitInvokeBasic(ilgen, args, ret, false);
    }

    private OpCode arrayLoadOpcode(byte tcode)
    {
        switch (tcode)
        {
            case Opcodes.T_BOOLEAN:
            case Opcodes.T_BYTE:
                return OpCodes.Ldelem_I1;
            case Opcodes.T_CHAR:
                return OpCodes.Ldelem_U2;
            case Opcodes.T_SHORT:
                return OpCodes.Ldelem_I2;
            case Opcodes.T_INT:
                return OpCodes.Ldelem_I4;
            case Opcodes.T_LONG:
                return OpCodes.Ldelem_I8;
            case Opcodes.T_FLOAT:
                return OpCodes.Ldelem_R4;
            case Opcodes.T_DOUBLE:
                return OpCodes.Ldelem_R8;
            case 0:
                return OpCodes.Ldelem_Ref;
            default:
                throw new BailoutException(Bailout.PreconditionViolated, "tcode = " + tcode);
        }
    }

    private static bool IsMethodHandleLinkTo(MemberName member)
    {
        return member.getDeclaringClass() == CoreClasses.java.lang.invoke.MethodHandle.Wrapper.ClassObject
            && member.getName().StartsWith("linkTo", StringComparison.Ordinal);
    }

    private static bool IsMethodHandleInvokeBasic(MemberName member)
    {
        return member.getDeclaringClass() == CoreClasses.java.lang.invoke.MethodHandle.Wrapper.ClassObject
            && member.getName() == "invokeBasic";
    }

    private static MethodWrapper GetMethodWrapper(MemberName member)
    {
        return TypeWrapper.FromClass(member.getDeclaringClass()).GetMethodWrapper(member.getName(), member.getSignature().Replace('/', '.'), true);
    }

    private static bool IsStaticallyInvocable(MethodWrapper mw)
    {
        if (mw == null || mw.DeclaringType.IsUnloadable || mw.DeclaringType.IsGhost || mw.DeclaringType.IsNonPrimitiveValueType || mw.IsFinalizeOrClone || mw.IsDynamicOnly)
        {
            return false;
        }
        if (mw.ReturnType.IsUnloadable || mw.ReturnType.IsGhost || mw.ReturnType.IsNonPrimitiveValueType)
        {
            return false;
        }
        foreach (TypeWrapper tw in mw.GetParameters())
        {
            if (tw.IsUnloadable || tw.IsGhost || tw.IsNonPrimitiveValueType)
            {
                return false;
            }
        }
        return true;
    }

    private static FieldWrapper GetFieldWrapper(MemberName member)
    {
        return TypeWrapper.FromClass(member.getDeclaringClass()).GetFieldWrapper(member.getName(), member.getSignature().Replace('/', '.'));
    }

    private static bool IsStaticallyInvocable(FieldWrapper fw)
    {
        return fw != null
            && !fw.FieldTypeWrapper.IsUnloadable
            && !fw.FieldTypeWrapper.IsGhost
            && !fw.FieldTypeWrapper.IsNonPrimitiveValueType;
    }
#endif
}
