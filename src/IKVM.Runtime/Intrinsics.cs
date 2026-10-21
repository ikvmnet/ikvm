/*
  Copyright (C) 2008-2013 Jeroen Frijters

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
using System.Collections.Frozen;
using System.Collections.Generic;

using IKVM.CoreLib.Diagnostics;
using IKVM.CoreLib.Linking;

#if IMPORTER
using IKVM.Reflection;
using IKVM.Reflection.Emit;
using IKVM.Tools.Importer;

using Type = IKVM.Reflection.Type;
#else
using System.Reflection;
using System.Reflection.Emit;
#endif

namespace IKVM.Runtime
{

    class Intrinsics
    {

        /// <summary>
        /// Defines a unique search criteria for locating a method that can be intrinsified.
        /// </summary>
        /// <param name="ClassName"></param>
        /// <param name="MethodName"></param>
        /// <param name="MethodSignature"></param>
        readonly record struct Key(string ClassName, string MethodName, string MethodSignature)
        {

            public readonly string ClassName = string.Intern(ClassName);

            public readonly string MethodName = string.Intern(MethodName);

            public readonly string MethodSignature = string.Intern(MethodSignature);

            /// <summary>
            /// Initializes the instance.
            /// </summary>
            /// <param name="mw"></param>
            internal Key(RuntimeJavaMethod mw) : this(mw.DeclaringType.Name, mw.Name, mw.Signature)
            {

            }

            /// <summary>
            /// Returns <c>true</c> if this instance is equal to the other instance.
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public readonly bool Equals(Key other)
            {
                return ReferenceEquals(ClassName, other.ClassName) && ReferenceEquals(MethodName, other.MethodName) && ReferenceEquals(MethodSignature, other.MethodSignature);
            }

            /// <summary>
            /// Returns <c>true</c> if this instance is equal to the other instance.
            /// </summary>
            /// <param name="other"></param>
            /// <returns></returns>
            public readonly override int GetHashCode()
            {
                return ClassName.GetHashCode() ^ MethodName.GetHashCode() ^ MethodSignature.GetHashCode();
            }

        }

#if EXPORTER == false

        readonly FrozenDictionary<Key, Func<EmitIntrinsicContext, bool>> _intrinsics;

#endif

        /// <summary>
        /// Initializes the instance.
        /// </summary>
        /// <param name="context"></param>
        public Intrinsics(RuntimeContext context)
        {
#if EXPORTER == false
            _intrinsics = new Dictionary<Key, Func<EmitIntrinsicContext, bool>>()
            {
                [new("java.lang.Object", "getClass", "()Ljava.lang.Class;")] = Object_getClass,
                [new("java.lang.Class", "desiredAssertionStatus", "()Z")] = Class_desiredAssertionStatus,
                [new("java.lang.Float", "floatToRawIntBits", "(F)I")] = Float_floatToRawIntBits,
                [new("java.lang.Float", "intBitsToFloat", "(I)F")] = Float_intBitsToFloat,
                [new("java.lang.Double", "doubleToRawLongBits", "(D)J")] = Double_doubleToRawLongBits,
                [new("java.lang.Double", "longBitsToDouble", "(J)D")] = Double_longBitsToDouble,
                [new("java.lang.System", "arraycopy", "(Ljava.lang.Object;ILjava.lang.Object;II)V")] = System_arraycopy,
                [new("java.util.concurrent.atomic.AtomicReferenceFieldUpdater", "newUpdater", "(Ljava.lang.Class;Ljava.lang.Class;Ljava.lang.String;)Ljava.util.concurrent.atomic.AtomicReferenceFieldUpdater;")] = AtomicReferenceFieldUpdater_newUpdater,
#if IMPORTER
                [new("sun.reflect.Reflection", "getCallerClass", "()Ljava.lang.Class;")] = Reflection_getCallerClass,
                [new("ikvm.internal.CallerID", "getCallerID", "()Likvm.internal.CallerID;")] = CallerID_getCallerID,
#endif
                [new("ikvm.runtime.Util", "getInstanceTypeFromClass", "(Ljava.lang.Class;)Lcli.System.Type;")] = Util_getInstanceTypeFromClass,
#if IMPORTER
                // this only applies to the core class library, so makes no sense in dynamic mode
                [new("java.lang.Class", "getPrimitiveClass", "(Ljava.lang.String;)Ljava.lang.Class;")] = Class_getPrimitiveClass,
#endif
                [new("java.lang.ThreadLocal", "<init>", "()V")] = ThreadLocal_new,
                [new("sun.misc.Unsafe", "ensureClassInitialized", "(Ljava.lang.Class;)V")] = Unsafe_ensureClassInitialized,
                // note that the following intrinsics don't pay off on CLR v2, but they do on CLR v4
                [new("sun.misc.Unsafe", "putObject", "(Ljava.lang.Object;JLjava.lang.Object;)V")] = Unsafe_putObject,
                [new("sun.misc.Unsafe", "putOrderedObject", "(Ljava.lang.Object;JLjava.lang.Object;)V")] = Unsafe_putOrderedObject,
                [new("sun.misc.Unsafe", "putObjectVolatile", "(Ljava.lang.Object;JLjava.lang.Object;)V")] = Unsafe_putObjectVolatile,
                [new("sun.misc.Unsafe", "getObjectVolatile", "(Ljava.lang.Object;J)Ljava.lang.Object;")] = Unsafe_getObjectVolatile,
                [new("sun.misc.Unsafe", "getObject", "(Ljava.lang.Object;J)Ljava.lang.Object;")] = Unsafe_getObjectVolatile,
                [new("sun.misc.Unsafe", "compareAndSwapObject", "(Ljava.lang.Object;JLjava.lang.Object;Ljava.lang.Object;)Z")] = Unsafe_compareAndSwapObject,
                [new("sun.misc.Unsafe", "getAndSetObject", "(Ljava.lang.Object;JLjava.lang.Object;)Ljava.lang.Object;")] = Unsafe_getAndSetObject,
                [new("sun.misc.Unsafe", "compareAndSwapInt", "(Ljava.lang.Object;JII)Z")] = Unsafe_compareAndSwapInt,
                [new("sun.misc.Unsafe", "getAndAddInt", "(Ljava.lang.Object;JI)I")] = Unsafe_getAndAddInt,
                [new("sun.misc.Unsafe", "compareAndSwapLong", "(Ljava.lang.Object;JJJ)Z")] = Unsafe_compareAndSwapLong,
            }.ToFrozenDictionary();
#endif
        }

#if EXPORTER == false

        /// <summary>
        /// Emits IL that pushes the index scale for the specified array type onto the stack.
        /// </summary>
        /// <param name="eic"></param>
        /// <param name="tw"></param>
        /// <returns></returns>
        void EmitArrayIndexScale(EmitIntrinsicContext eic, RuntimeJavaType tw)
        {
            var et = tw.ElementTypeWrapper;
            if (et == eic.Context.Context.PrimitiveJavaTypeFactory.BYTE || et == eic.Context.Context.PrimitiveJavaTypeFactory.BOOLEAN)
                eic.Emitter.EmitLdc_I4(1);
            else if (et == eic.Context.Context.PrimitiveJavaTypeFactory.CHAR || et == eic.Context.Context.PrimitiveJavaTypeFactory.SHORT)
                eic.Emitter.EmitLdc_I4(2);
            else if (et == eic.Context.Context.PrimitiveJavaTypeFactory.INT || et == eic.Context.Context.PrimitiveJavaTypeFactory.FLOAT)
                eic.Emitter.EmitLdc_I4(4);
            else if (et == eic.Context.Context.PrimitiveJavaTypeFactory.LONG || et == eic.Context.Context.PrimitiveJavaTypeFactory.DOUBLE)
                eic.Emitter.EmitLdc_I4(8);
            else if (et.IsPrimitive == false && et.IsNonPrimitiveValueType)
                eic.Emitter.Emit(OpCodes.Sizeof, et.TypeAsArrayType);
            else if (et.IsPrimitive == false && et.IsNonPrimitiveValueType == false)
                eic.Emitter.Emit(OpCodes.Sizeof, eic.Context.Context.Types.IntPtr);
            else
                eic.Emitter.EmitLdc_I4(1);
        }

        internal bool IsIntrinsic(RuntimeJavaMethod mw)
        {
            return _intrinsics.ContainsKey(new Key(mw)) && mw.DeclaringType.ClassLoader == mw.DeclaringType.Context.JavaBase.TypeOfJavaLangObject.ClassLoader;
        }

        internal bool Emit(EmitIntrinsicContext context)
        {
            // note that intrinsics can always refuse to emit code and the code generator will fall back to a normal method call
            return _intrinsics[new Key(context.Method)](context);
        }

        bool Object_getClass(EmitIntrinsicContext eic)
        {
            // this is the null-check idiom that javac uses (both in its own source and in the code it generates)
            if (eic.MatchRange(0, 2) && eic.Match(1, NormalizedOpCode.Pop))
            {
                eic.Emitter.Emit(OpCodes.Dup);
                eic.Emitter.EmitNullCheck();
                return true;
            }
            // this optimizes obj1.getClass() ==/!= obj2.getClass()
            else if (eic.MatchRange(0, 4) && eic.Match(1, NormalizedOpCode.Aload) && eic.Match(2, NormalizedOpCode.InvokeVirtual) && (eic.Match(3, NormalizedOpCode.IfAcmpeq) || eic.Match(3, NormalizedOpCode.IfAcmpne)) && (IsSafeForGetClassOptimization(eic.GetStackTypeWrapper(0, 0)) || IsSafeForGetClassOptimization(eic.GetStackTypeWrapper(2, 0))))
            {
                var cpi = eic.GetMethodref(2);
                if (cpi.Class == "java.lang.Object" && cpi.Name == "getClass" && cpi.Signature == "()Ljava.lang.Class;")
                {
                    // we can't patch the current opcode, so we have to emit the first call to GetTypeHandle here
                    eic.Emitter.Emit(OpCodes.Callvirt, eic.Method.DeclaringType.Context.CompilerFactory.GetTypeMethod);
                    eic.PatchOpCode(2, NormalizedOpCode.IntrinsicGettype);
                    return true;
                }
            }
            // this optimizes obj.getClass() == Xxx.class
            else if (eic.MatchRange(0, 3) && eic.Match(1, NormalizedOpCode.Ldc) && eic.GetConstantType(1) == ConstantType.Class && (eic.Match(2, NormalizedOpCode.IfAcmpeq) || eic.Match(2, NormalizedOpCode.IfAcmpne)))
            {
                var tw = eic.GetClassLiteral(1);
                if (tw.IsGhost || tw.IsGhostArray || tw.IsUnloadable || (tw.IsRemapped && tw.IsFinal && tw is RuntimeManagedJavaType))
                    return false;

                eic.Emitter.Emit(OpCodes.Callvirt, eic.Method.DeclaringType.Context.CompilerFactory.GetTypeMethod);
                eic.Emitter.Emit(OpCodes.Ldtoken, (tw.IsRemapped && tw.IsFinal) ? tw.TypeAsTBD : tw.TypeAsBaseType);
                eic.Emitter.Emit(OpCodes.Call, eic.Method.DeclaringType.Context.CompilerFactory.GetTypeFromHandleMethod);
                eic.PatchOpCode(1, NormalizedOpCode.Nop);
                return true;
            }

            return false;
        }

        bool Class_desiredAssertionStatus(EmitIntrinsicContext eic)
        {
            if (eic.MatchRange(-1, 2) && eic.Match(-1, NormalizedOpCode.Ldc))
            {
                var classLiteral = eic.GetClassLiteral(-1);
                if (!classLiteral.IsUnloadable && classLiteral.ClassLoader.RemoveAsserts)
                {
                    eic.Emitter.Emit(OpCodes.Pop);
                    eic.Emitter.EmitLdc_I4(0);
                    return true;
                }
            }

            return false;
        }

        bool IsSafeForGetClassOptimization(RuntimeJavaType tw)
        {
            // because of ghost arrays, we don't optimize if both types are either java.lang.Object or an array
            return tw != tw.Context.JavaBase.TypeOfJavaLangObject && !tw.IsArray;
        }

        bool Float_floatToRawIntBits(EmitIntrinsicContext eic)
        {
            EmitConversion(eic.Emitter, eic.Method.DeclaringType.Context.Resolver.ResolveRuntimeType("IKVM.Runtime.FloatConverter").AsReflection(), "ToInt");
            return true;
        }

        bool Float_intBitsToFloat(EmitIntrinsicContext eic)
        {
            EmitConversion(eic.Emitter, eic.Method.DeclaringType.Context.Resolver.ResolveRuntimeType("IKVM.Runtime.FloatConverter").AsReflection(), "ToFloat");
            return true;
        }

        bool Double_doubleToRawLongBits(EmitIntrinsicContext eic)
        {
            EmitConversion(eic.Emitter, eic.Method.DeclaringType.Context.Resolver.ResolveRuntimeType("IKVM.Runtime.DoubleConverter").AsReflection(), "ToLong");
            return true;
        }

        bool Double_longBitsToDouble(EmitIntrinsicContext eic)
        {
            EmitConversion(eic.Emitter, eic.Method.DeclaringType.Context.Resolver.ResolveRuntimeType("IKVM.Runtime.DoubleConverter").AsReflection(), "ToDouble");
            return true;
        }

        void EmitConversion(CodeEmitter ilgen, Type converterType, string method)
        {
            var converter = ilgen.UnsafeAllocTempLocal(converterType);
            ilgen.Emit(OpCodes.Ldloca, converter);
            ilgen.Emit(OpCodes.Call, converterType.GetMethod(method));
        }

        /// <summary>
        /// Calls to System.arraycopy can be replaced with directly optimized versions.
        /// </summary>
        /// <param name="eic"></param>
        /// <returns></returns>
        bool System_arraycopy(EmitIntrinsicContext eic)
        {
            // if the array arguments on the stack are of a known array type, we can redirect to an optimized version of arraycopy.
            var dst_type = eic.GetStackTypeWrapper(0, 2);
            var src_type = eic.GetStackTypeWrapper(0, 4);
            if (!dst_type.IsUnloadable && dst_type.IsArray && dst_type == src_type)
            {
                switch (dst_type.Name[1])
                {
                    case 'J':
                    case 'D':
                        eic.Emitter.Emit(OpCodes.Call, eic.Method.DeclaringType.Context.ByteCodeHelperMethods.arraycopy_primitive_8);
                        break;
                    case 'I':
                    case 'F':
                        eic.Emitter.Emit(OpCodes.Call, eic.Method.DeclaringType.Context.ByteCodeHelperMethods.arraycopy_primitive_4);
                        break;
                    case 'S':
                    case 'C':
                        eic.Emitter.Emit(OpCodes.Call, eic.Method.DeclaringType.Context.ByteCodeHelperMethods.arraycopy_primitive_2);
                        break;
                    case 'B':
                    case 'Z':
                        eic.Emitter.Emit(OpCodes.Call, eic.Method.DeclaringType.Context.ByteCodeHelperMethods.arraycopy_primitive_1);
                        break;
                    default:
                        // TODO once the verifier tracks actual types (i.e. it knows that
                        // a particular reference is the result of a "new" opcode) we can
                        // use the fast version if the exact destination type is known
                        // (in that case the "dst_type == src_type" above should
                        // be changed to "src_type.IsAssignableTo(dst_type)".
                        var elemtw = dst_type.ElementTypeWrapper;
                        // note that IsFinal returns true for array types, so we have to be careful!
                        if (elemtw.IsArray == false && elemtw.IsFinal)
                        {
                            eic.Emitter.Emit(OpCodes.Call, eic.Method.DeclaringType.Context.ByteCodeHelperMethods.arraycopy_fast);
                        }
                        else
                        {
                            eic.Emitter.Emit(OpCodes.Call, eic.Method.DeclaringType.Context.ByteCodeHelperMethods.arraycopy);
                        }
                        break;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        bool AtomicReferenceFieldUpdater_newUpdater(EmitIntrinsicContext eic)
        {
            return AtomicReferenceFieldUpdaterEmitter.Emit(eic.Context, eic.Caller.DeclaringType, eic.Emitter, eic.ClassFile, eic.OpcodeIndex, eic.Code, eic.Flags);
        }

#if IMPORTER

        bool Reflection_getCallerClass(EmitIntrinsicContext eic)
        {
            if (eic.Caller.HasCallerID)
            {
                int arg = eic.Caller.GetParametersForDefineMethod().Length - 1;
                if (!eic.Caller.IsStatic)
                {
                    arg++;
                }
                eic.Emitter.EmitLdarg(arg);
                RuntimeJavaMethod mw;
                if (MatchInvokeStatic(eic, 1, "java.lang.ClassLoader", "getClassLoader", "(Ljava.lang.Class;)Ljava.lang.ClassLoader;"))
                {
                    eic.PatchOpCode(1, NormalizedOpCode.Nop);
                    mw = eic.Context.TypeWrapper.Context.JavaBase.TypeOfIkvmInternalCallerID.GetMethod("getCallerClassLoader", "()Ljava.lang.ClassLoader;", false);
                }
                else
                {
                    mw = eic.Context.TypeWrapper.Context.JavaBase.TypeOfIkvmInternalCallerID.GetMethod("getCallerClass", "()Ljava.lang.Class;", false);
                }
                mw.Link();
                mw.EmitCallvirt(eic.Emitter);
                return true;
            }
            else if (RuntimeByteCodeJavaType.RequiresDynamicReflectionCallerClass(eic.ClassFile.Name, eic.Caller.Name, eic.Caller.Signature))
            {
                // since the non-intrinsic version of Reflection.getCallerClass() always throws an exception, we have to redirect to the dynamic version
                var getCallerClass = eic.Context.TypeWrapper.Context.ClassLoaderFactory.LoadClassCritical("sun.reflect.Reflection").GetMethod("getCallerClass", "(I)Ljava.lang.Class;", false);
                getCallerClass.Link();
                eic.Emitter.EmitLdc_I4(2);
                getCallerClass.EmitCall(eic.Emitter);
                return true;
            }
            else
            {
                eic.Context.TypeWrapper.ClassLoader.Diagnostics.ReflectionCallerClassRequiresCallerID(eic.ClassFile.Name, eic.Caller.Name, eic.Caller.Signature);
            }
            return false;
        }

        bool CallerID_getCallerID(EmitIntrinsicContext eic)
        {
            if (eic.Caller.HasCallerID)
            {
                int arg = eic.Caller.GetParametersForDefineMethod().Length - 1;
                if (!eic.Caller.IsStatic)
                {
                    arg++;
                }
                eic.Emitter.EmitLdarg(arg);
                return true;
            }
            else
            {
                throw new FatalCompilerErrorException(DiagnosticEvent.CallerIDRequiresHasCallerIDAnnotation());
            }
        }

#endif

        bool Util_getInstanceTypeFromClass(EmitIntrinsicContext eic)
        {
            if (eic.MatchRange(-1, 2) && eic.Match(-1, NormalizedOpCode.Ldc))
            {
                var tw = eic.GetClassLiteral(-1);
                if (tw.IsUnloadable == false)
                {
                    eic.Emitter.Emit(OpCodes.Pop);
                    if (tw.IsRemapped && tw.IsFinal)
                        eic.Emitter.Emit(OpCodes.Ldtoken, tw.TypeAsTBD);
                    else
                        eic.Emitter.Emit(OpCodes.Ldtoken, tw.TypeAsBaseType);

                    eic.Emitter.Emit(OpCodes.Call, eic.Method.DeclaringType.Context.CompilerFactory.GetTypeFromHandleMethod);
                    return true;
                }
            }

            return false;
        }

#if IMPORTER

        bool Class_getPrimitiveClass(EmitIntrinsicContext eic)
        {
            eic.Emitter.Emit(OpCodes.Pop);
            eic.Emitter.Emit(OpCodes.Ldnull);
            var mw = eic.Context.TypeWrapper.Context.JavaBase.TypeOfJavaLangClass.GetMethod("<init>", "(Lcli.System.Type;)V", false);
            mw.Link();
            mw.EmitNewobj(eic.Emitter);
            return true;
        }

#endif

        /// <summary>
        /// Replaces calls to ThreadLocal.new with the direct initialization of the ThreadLocal.
        /// </summary>
        /// <param name="eic"></param>
        /// <returns></returns>
        bool ThreadLocal_new(EmitIntrinsicContext eic)
        {
            // it is only valid to replace a ThreadLocal instantiation by our ThreadStatic based version, if we can prove that the instantiation only happens once
            // (which is the case when we're in <clinit> and there aren't any branches that lead to the current position)
            if (eic.Caller.IsClassInitializer == false)
                return false;
            for (int i = 0; i <= eic.OpcodeIndex; i++)
                if ((eic.Flags[i] & InstructionFlags.BranchTarget) != 0)
                    return false;

            eic.Emitter.Emit(OpCodes.Newobj, eic.Context.DefineThreadLocalType());
            return true;
        }

        /// <summary>
        /// Replaces calls to Unsafe.ensureClassInitialized with direct calls to run the class constructor.
        /// </summary>
        /// <param name="eic"></param>
        /// <returns></returns>
        bool Unsafe_ensureClassInitialized(EmitIntrinsicContext eic)
        {
            if (eic.MatchRange(-1, 2) && eic.Match(-1, NormalizedOpCode.Ldc))
            {
                var classLiteral = eic.GetClassLiteral(-1);
                if (classLiteral.IsUnloadable == false)
                {
                    eic.Emitter.Emit(OpCodes.Pop);
                    eic.Emitter.EmitNullCheck();
                    classLiteral.EmitRunClassConstructor(eic.Emitter);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true if the given <see cref="RuntimeJavaType"/> specifies an array type suitable for an unsafe operation.
        /// </summary>
        /// <param name="tw"></param>
        /// <returns></returns>
        internal bool IsSupportedArrayTypeForUnsafeOperation(RuntimeJavaType tw)
        {
            return tw.IsArray && !tw.IsGhostArray && !tw.ElementTypeWrapper.IsPrimitive && !tw.ElementTypeWrapper.IsNonPrimitiveValueType;
        }

        /// <summary>
        /// Attempts to replace an invocation of Unsafe.putObject with an intrinsic implementation.
        /// </summary>
        /// <param name="eic"></param>
        /// <returns></returns>
        bool Unsafe_putObject(EmitIntrinsicContext eic)
        {
            return Unsafe_putObjectImpl(eic, false);
        }

        /// <summary>
        /// Attempts to replace an invocation of Unsafe.putOrderedObject with an intrinsic implementation.
        /// </summary>
        /// <param name="eic"></param>
        /// <returns></returns>
        bool Unsafe_putOrderedObject(EmitIntrinsicContext eic)
        {
            return Unsafe_putObjectImpl(eic, false);
        }

        /// <summary>
        /// Attempts to replace an invocation of Unsafe.putObjectVolatile with an intrinsic implementation.
        /// </summary>
        /// <param name="eic"></param>
        /// <returns></returns>
        bool Unsafe_putObjectVolatile(EmitIntrinsicContext eic)
        {
            return Unsafe_putObjectImpl(eic, true);
        }

        /// <summary>
        /// Attempts to replace an invocation of an Unsafe put operation with an intrinsic implementation.
        /// </summary>
        /// <param name="eic"></param>
        /// <param name="isVolatile"></param>
        /// <returns></returns>
        bool Unsafe_putObjectImpl(EmitIntrinsicContext eic, bool isVolatile)
        {
            var tw = eic.GetStackTypeWrapper(0, 2);

            // check for non-primitive array case
            if (IsSupportedArrayTypeForUnsafeOperation(tw) && eic.GetStackTypeWrapper(0, 0).IsAssignableTo(tw.ElementTypeWrapper))
            {
                var value = eic.Emitter.AllocTempLocal(tw.ElementTypeWrapper.TypeAsLocalOrStackType);
                var index = eic.Emitter.AllocTempLocal(tw.Context.Types.Int32);
                var array = eic.Emitter.AllocTempLocal(tw.TypeAsLocalOrStackType);

                // consume existing call site
                eic.Emitter.Emit(OpCodes.Stloc, value);
                eic.Emitter.Emit(OpCodes.Conv_Ovf_I4);
                eic.Emitter.Emit(OpCodes.Stloc, index);
                eic.Emitter.Emit(OpCodes.Stloc, array);
                EmitConsumeUnsafe(eic);

                // emit new call that sets the element by index
                eic.Emitter.Emit(OpCodes.Ldloc, array);
                eic.Emitter.Emit(OpCodes.Ldloc, index);
                EmitArrayIndexScale(eic, tw);
                eic.Emitter.Emit(OpCodes.Div);
                eic.Emitter.Emit(OpCodes.Ldloc, value);
                eic.Emitter.Emit(OpCodes.Stelem_Ref);

                if (isVolatile)
                    eic.Emitter.EmitMemoryBarrier();

                eic.Emitter.ReleaseTempLocal(array);
                eic.Emitter.ReleaseTempLocal(index);
                eic.Emitter.ReleaseTempLocal(value);
                eic.NonLeaf = false;
                return true;
            }

            if ((eic.Flags[eic.OpcodeIndex] & InstructionFlags.BranchTarget) != 0 || (eic.Flags[eic.OpcodeIndex - 1] & InstructionFlags.BranchTarget) != 0)
                return false;

            if ((eic.Match(-1, NormalizedOpCode.Aload) || eic.Match(-1, NormalizedOpCode.AconstNull)) && eic.Match(-2, NormalizedOpCode.GetStatic))
            {
                var fw = GetUnsafeField(eic, eic.GetFieldref(-2));
                if (fw != null &&
                    (!fw.IsFinal || (!fw.IsStatic && eic.Caller.Name == "<init>") || (fw.IsStatic && eic.Caller.Name == "<clinit>")) &&
                    fw.IsAccessibleFrom(fw.DeclaringType, eic.Caller.DeclaringType, fw.DeclaringType) &&
                    eic.GetStackTypeWrapper(0, 0).IsAssignableTo(fw.FieldTypeWrapper) &&
                    (fw.IsStatic || fw.DeclaringType == eic.GetStackTypeWrapper(0, 2)))
                {
                    var value = eic.Emitter.AllocTempLocal(fw.FieldTypeWrapper.TypeAsLocalOrStackType);
                    eic.Emitter.Emit(OpCodes.Stloc, value);
                    eic.Emitter.Emit(OpCodes.Pop);      // discard offset field
                    if (fw.IsStatic)
                    {
                        eic.Emitter.Emit(OpCodes.Pop);  // discard object
                        EmitConsumeUnsafe(eic);
                    }
                    else
                    {
                        var target = eic.Emitter.AllocTempLocal(fw.DeclaringType.TypeAsLocalOrStackType);
                        eic.Emitter.Emit(OpCodes.Stloc, target);
                        EmitConsumeUnsafe(eic);
                        eic.Emitter.Emit(OpCodes.Ldloc, target);
                        eic.Emitter.ReleaseTempLocal(target);
                    }

                    eic.Emitter.Emit(OpCodes.Ldloc, value);
                    eic.Emitter.ReleaseTempLocal(value);

                    // note that we assume the CLR memory model where all writes are ordered,
                    // so we don't need a volatile store or a memory barrier and putOrderedObject
                    // is typically used with a volatile field, so to avoid the memory barrier,
                    // we don't use FieldWrapper.EmitSet(), but emit the store directly
                    eic.Emitter.Emit(fw.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, fw.GetField());
                    if (isVolatile)
                        eic.Emitter.EmitMemoryBarrier();

                    eic.NonLeaf = false;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Replaces calls to Unsafe.getObjectVolatile against an array with inline code.
        /// </summary>
        /// <param name="eic"></param>
        /// <returns></returns>
        bool Unsafe_getObjectVolatile(EmitIntrinsicContext eic)
        {
            // the check here must be kept in sync with the hack in MethodAnalyzer.AnalyzeTypeFlow()
            var tw = eic.GetStackTypeWrapper(0, 1);
            if (IsSupportedArrayTypeForUnsafeOperation(tw))
            {
                var offset = eic.Emitter.AllocTempLocal(tw.Context.Types.Int32);
                var target = eic.Emitter.AllocTempLocal(tw.TypeAsLocalOrStackType);

                // consume existing call site
                eic.Emitter.Emit(OpCodes.Conv_Ovf_I4);
                eic.Emitter.Emit(OpCodes.Stloc, offset);
                eic.Emitter.Emit(OpCodes.Stloc, target);
                EmitConsumeUnsafe(eic);

                // emit new call that gets the element by index
                eic.Emitter.Emit(OpCodes.Ldloc, target);
                eic.Emitter.Emit(OpCodes.Ldloc, offset);
                EmitArrayIndexScale(eic, tw);
                eic.Emitter.Emit(OpCodes.Div);
                eic.Emitter.Emit(OpCodes.Ldelema, tw.TypeAsLocalOrStackType.GetElementType());
                eic.Emitter.Emit(OpCodes.Volatile);
                eic.Emitter.Emit(OpCodes.Ldind_Ref);

                // remove the redundant checkcast that usually follows
                if (eic.Code[eic.OpcodeIndex + 1].NormalizedOpCode == NormalizedOpCode.Checkcast && tw.ElementTypeWrapper.IsAssignableTo(eic.ClassFile.GetConstantPoolClassType(eic.Code[eic.OpcodeIndex + 1].Arg1)))
                    eic.PatchOpCode(1, NormalizedOpCode.Nop);

                eic.Emitter.ReleaseTempLocal(target);
                eic.Emitter.ReleaseTempLocal(offset);
                eic.NonLeaf = false;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Replaces a call to Unsafe.compareAndSwapObject with IL that directly conducts the operation.
        /// </summary>
        /// <param name="eic"></param>
        /// <returns></returns>
        bool Unsafe_compareAndSwapObject(EmitIntrinsicContext eic)
        {
            var tw = eic.GetStackTypeWrapper(0, 3);

            // target object is an array type
            // convert offset to int32 index
            // directly compare/exchange value
            if (IsSupportedArrayTypeForUnsafeOperation(tw) &&
                eic.GetStackTypeWrapper(0, 0).IsAssignableTo(tw.ElementTypeWrapper) &&
                eic.GetStackTypeWrapper(0, 1).IsAssignableTo(tw.ElementTypeWrapper))
            {
                var type = tw.TypeAsLocalOrStackType.GetElementType();
                var update = eic.Emitter.AllocTempLocal(type);
                var expect = eic.Emitter.AllocTempLocal(type);
                var offset = eic.Emitter.AllocTempLocal(tw.Context.Types.Int32);
                var target = eic.Emitter.AllocTempLocal(tw.TypeAsLocalOrStackType);

                // consume existing call site
                eic.Emitter.Emit(OpCodes.Stloc, update);
                eic.Emitter.Emit(OpCodes.Stloc, expect);
                eic.Emitter.Emit(OpCodes.Conv_Ovf_I4);
                eic.Emitter.Emit(OpCodes.Stloc, offset);
                eic.Emitter.Emit(OpCodes.Stloc, target);
                EmitConsumeUnsafe(eic);

                // emit new call site
                eic.Emitter.Emit(OpCodes.Ldloc, target);
                eic.Emitter.Emit(OpCodes.Ldloc, offset);
                EmitArrayIndexScale(eic, tw);
                eic.Emitter.Emit(OpCodes.Div);
                eic.Emitter.Emit(OpCodes.Ldelema, type);
                eic.Emitter.Emit(OpCodes.Ldloc, update);
                eic.Emitter.Emit(OpCodes.Ldloc, expect);
                eic.Emitter.Emit(OpCodes.Call, AtomicReferenceFieldUpdaterEmitter.MakeCompareExchange(tw.Context, type));
                eic.Emitter.Emit(OpCodes.Ldloc, expect);
                eic.Emitter.Emit(OpCodes.Ceq);

                eic.Emitter.ReleaseTempLocal(target);
                eic.Emitter.ReleaseTempLocal(offset);
                eic.Emitter.ReleaseTempLocal(expect);
                eic.Emitter.ReleaseTempLocal(update);
                eic.NonLeaf = false;
                return true;
            }

            if ((eic.Flags[eic.OpcodeIndex] & InstructionFlags.BranchTarget) != 0 ||
                (eic.Flags[eic.OpcodeIndex - 1] & InstructionFlags.BranchTarget) != 0 ||
                (eic.Flags[eic.OpcodeIndex - 2] & InstructionFlags.BranchTarget) != 0)
                return false;

            if ((eic.Match(-1, NormalizedOpCode.Aload) || eic.Match(-1, NormalizedOpCode.AconstNull)) &&
                (eic.Match(-2, NormalizedOpCode.Aload) || eic.Match(-2, NormalizedOpCode.AconstNull)) &&
                eic.Match(-3, NormalizedOpCode.GetStatic))
            {
                var fw = GetUnsafeField(eic, eic.GetFieldref(-3));
                if (fw != null &&
                    fw.IsAccessibleFrom(fw.DeclaringType, eic.Caller.DeclaringType, fw.DeclaringType) &&
                    eic.GetStackTypeWrapper(0, 0).IsAssignableTo(fw.FieldTypeWrapper) &&
                    eic.GetStackTypeWrapper(0, 1).IsAssignableTo(fw.FieldTypeWrapper) &&
                    (fw.IsStatic || fw.DeclaringType == eic.GetStackTypeWrapper(0, 3)))
                {
                    var type = fw.FieldTypeWrapper.TypeAsLocalOrStackType;
                    var update = eic.Emitter.AllocTempLocal(type);
                    var expect = eic.Emitter.AllocTempLocal(type);
                    eic.Emitter.Emit(OpCodes.Stloc, update);
                    eic.Emitter.Emit(OpCodes.Stloc, expect);
                    eic.Emitter.Emit(OpCodes.Pop);          // discard index
                    if (fw.IsStatic)
                    {
                        eic.Emitter.Emit(OpCodes.Pop);      // discard obj
                        EmitConsumeUnsafe(eic);
                        eic.Emitter.Emit(OpCodes.Ldsflda, fw.GetField());
                    }
                    else
                    {
                        var obj = eic.Emitter.AllocTempLocal(eic.Caller.DeclaringType.TypeAsLocalOrStackType);
                        eic.Emitter.Emit(OpCodes.Stloc, obj);
                        EmitConsumeUnsafe(eic);
                        eic.Emitter.Emit(OpCodes.Ldloc, obj);
                        eic.Emitter.ReleaseTempLocal(obj);
                        eic.Emitter.Emit(OpCodes.Ldflda, fw.GetField());
                    }

                    eic.Emitter.Emit(OpCodes.Ldloc, update);
                    eic.Emitter.Emit(OpCodes.Ldloc, expect);
                    eic.Emitter.Emit(OpCodes.Call, AtomicReferenceFieldUpdaterEmitter.MakeCompareExchange(tw.Context, type));
                    eic.Emitter.Emit(OpCodes.Ldloc, expect);
                    eic.Emitter.Emit(OpCodes.Ceq);

                    eic.Emitter.ReleaseTempLocal(expect);
                    eic.Emitter.ReleaseTempLocal(update);
                    eic.NonLeaf = false;
                    return true;
                }
            }

            // stack layout at call site:
            // 4 Unsafe (receiver)
            // 3 Object (target)
            // 2 long (offset)
            // 1 Object (expect)
            // 0 Object (update)
            var twUnsafe = eic.GetStackTypeWrapper(0, 4);
            if (twUnsafe == eic.Context.Context.VerifierJavaTypeFactory.Null)
                return false;

            for (int i = 0; ; i--)
            {
                if ((eic.Flags[eic.OpcodeIndex + i] & InstructionFlags.BranchTarget) != 0)
                    return false;

                if (eic.GetStackTypeWrapper(i, 0) == twUnsafe)
                {
                    // the pattern we recognize is:
                    // aload
                    // getstatic <offset field>
                    if (eic.Match(i, NormalizedOpCode.Aload) &&
                        eic.GetStackTypeWrapper(i + 1, 0) == eic.Caller.DeclaringType &&
                        eic.Match(i + 1, NormalizedOpCode.GetStatic))
                    {
                        var fw = GetUnsafeField(eic, eic.GetFieldref(i + 1));
                        if (fw != null && !fw.IsStatic && fw.DeclaringType == eic.Caller.DeclaringType)
                        {
                            var update = eic.Emitter.AllocTempLocal(fw.FieldTypeWrapper.TypeAsLocalOrStackType);
                            var expect = eic.Emitter.AllocTempLocal(fw.FieldTypeWrapper.TypeAsLocalOrStackType);
                            var target = eic.Emitter.AllocTempLocal(eic.Caller.DeclaringType.TypeAsLocalOrStackType);

                            // consume existing call site
                            eic.Emitter.Emit(OpCodes.Stloc, update);
                            eic.Emitter.Emit(OpCodes.Stloc, expect);
                            eic.Emitter.Emit(OpCodes.Pop);          // discard offset
                            eic.Emitter.Emit(OpCodes.Stloc, target);
                            EmitConsumeUnsafe(eic);

                            // emit new call site
                            eic.Emitter.Emit(OpCodes.Ldloc, target);
                            eic.Emitter.Emit(OpCodes.Ldloc, expect);
                            eic.Emitter.Emit(OpCodes.Ldloc, update);
                            fw.EmitUnsafeCompareAndSwap(eic.Emitter);

                            eic.Emitter.ReleaseTempLocal(target);
                            eic.Emitter.ReleaseTempLocal(expect);
                            eic.Emitter.ReleaseTempLocal(update);
                            eic.NonLeaf = false;
                            return true;
                        }
                    }

                    return false;
                }
            }
        }

        /// <summary>
        /// Replaces calls to Unsafe.getAndSetObject against an array with inline code.
        /// </summary>
        /// <param name="eic"></param>
        /// <returns></returns>
        bool Unsafe_getAndSetObject(EmitIntrinsicContext eic)
        {
            var tw = eic.GetStackTypeWrapper(0, 2);
            if (IsSupportedArrayTypeForUnsafeOperation(tw) && eic.GetStackTypeWrapper(0, 0).IsAssignableTo(tw.ElementTypeWrapper))
            {
                var type = tw.TypeAsLocalOrStackType.GetElementType();
                var newValue = eic.Emitter.AllocTempLocal(type);
                var offset = eic.Emitter.AllocTempLocal(tw.Context.Types.Int32);
                var target = eic.Emitter.AllocTempLocal(tw.TypeAsLocalOrStackType);

                // consume existing call site
                eic.Emitter.Emit(OpCodes.Stloc, newValue);
                eic.Emitter.Emit(OpCodes.Conv_Ovf_I4);
                eic.Emitter.Emit(OpCodes.Stloc, offset);
                eic.Emitter.Emit(OpCodes.Stloc, target);
                EmitConsumeUnsafe(eic);

                // emit new call
                eic.Emitter.Emit(OpCodes.Ldloc, target);
                eic.Emitter.Emit(OpCodes.Ldloc, offset);
                EmitArrayIndexScale(eic, tw);
                eic.Emitter.Emit(OpCodes.Div);
                eic.Emitter.Emit(OpCodes.Ldelema, type);
                eic.Emitter.Emit(OpCodes.Ldloc, newValue);
                eic.Emitter.Emit(OpCodes.Call, MakeExchange(tw.Context, type));

                eic.Emitter.ReleaseTempLocal(target);
                eic.Emitter.ReleaseTempLocal(offset);
                eic.Emitter.ReleaseTempLocal(newValue);
                eic.NonLeaf = false;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Replaces a call to Unsafe.compareAndSwapInt with IL that directly conducts the operation.
        /// </summary>
        /// <param name="eic"></param>
        /// <returns></returns>
        bool Unsafe_compareAndSwapInt(EmitIntrinsicContext eic)
        {
            // stack layout at call site:
            // 4 Unsafe (receiver)
            // 3 Object (obj)
            // 2 long (offset)
            // 1 int (expect)
            // 0 int (update)

            var twUnsafe = eic.GetStackTypeWrapper(0, 4);
            if (twUnsafe == eic.Context.Context.VerifierJavaTypeFactory.Null)
                return false;

            for (int i = 0; ; i--)
            {
                if ((eic.Flags[eic.OpcodeIndex + i] & InstructionFlags.BranchTarget) != 0)
                    return false;

                if (eic.GetStackTypeWrapper(i, 0) == twUnsafe)
                {
                    // the pattern we recognize is:
                    // aload
                    // getstatic <offset field>
                    if (eic.Match(i, NormalizedOpCode.Aload) &&
                        eic.GetStackTypeWrapper(i + 1, 0) == eic.Caller.DeclaringType &&
                        eic.Match(i + 1, NormalizedOpCode.GetStatic))
                    {
                        var fw = GetUnsafeField(eic, eic.GetFieldref(i + 1));
                        if (fw != null && !fw.IsStatic && fw.DeclaringType == eic.Caller.DeclaringType)
                        {
                            var update = eic.Emitter.AllocTempLocal(eic.Method.DeclaringType.Context.Types.Int32);
                            var expect = eic.Emitter.AllocTempLocal(eic.Method.DeclaringType.Context.Types.Int32);
                            var target = eic.Emitter.AllocTempLocal(eic.Caller.DeclaringType.TypeAsLocalOrStackType);

                            eic.Emitter.Emit(OpCodes.Stloc, update);
                            eic.Emitter.Emit(OpCodes.Stloc, expect);
                            eic.Emitter.Emit(OpCodes.Pop); // discard offset
                            eic.Emitter.Emit(OpCodes.Stloc, target);
                            EmitConsumeUnsafe(eic);
                            eic.Emitter.Emit(OpCodes.Ldloc, target);
                            eic.Emitter.Emit(OpCodes.Ldloc, expect);
                            eic.Emitter.Emit(OpCodes.Ldloc, update);
                            fw.EmitUnsafeCompareAndSwap(eic.Emitter);

                            eic.Emitter.ReleaseTempLocal(target);
                            eic.Emitter.ReleaseTempLocal(expect);
                            eic.Emitter.ReleaseTempLocal(update);
                            eic.NonLeaf = false;
                            return true;
                        }
                    }

                    return false;
                }
            }
        }

        bool Unsafe_getAndAddInt(EmitIntrinsicContext eic)
        {
            // stack layout at call site:
            // 3 Unsafe (receiver)
            // 2 Object (obj)
            // 1 long (offset)
            // 0 int (delta)
            RuntimeJavaType twUnsafe = eic.GetStackTypeWrapper(0, 3);
            if (twUnsafe == eic.Context.Context.VerifierJavaTypeFactory.Null)
            {
                return false;
            }
            for (int i = 0; ; i--)
            {
                if ((eic.Flags[eic.OpcodeIndex + i] & InstructionFlags.BranchTarget) != 0)
                {
                    return false;
                }
                if (eic.GetStackTypeWrapper(i, 0) == twUnsafe)
                {
                    // the pattern we recognize is:
                    // aload_0 
                    // getstatic <offset field>
                    if (eic.Match(i, NormalizedOpCode.Aload, 0)
                        && eic.Match(i + 1, NormalizedOpCode.GetStatic))
                    {
                        RuntimeJavaField fw = GetUnsafeField(eic, eic.GetFieldref(i + 1));
                        if (fw != null && !fw.IsStatic && fw.DeclaringType == eic.Caller.DeclaringType)
                        {
                            CodeEmitterLocal delta = eic.Emitter.AllocTempLocal(eic.Method.DeclaringType.Context.Types.Int32);
                            eic.Emitter.Emit(OpCodes.Stloc, delta);
                            eic.Emitter.Emit(OpCodes.Pop);          // discard offset
                            eic.Emitter.Emit(OpCodes.Pop);          // discard obj
                            EmitConsumeUnsafe(eic);
                            eic.Emitter.Emit(OpCodes.Ldarg_0);
                            eic.Emitter.Emit(OpCodes.Ldflda, fw.GetField());
                            eic.Emitter.Emit(OpCodes.Ldloc, delta);
                            eic.Emitter.Emit(OpCodes.Call, twUnsafe.Context.InterlockedMethods.AddInt32);
                            eic.Emitter.Emit(OpCodes.Ldloc, delta);
                            eic.Emitter.Emit(OpCodes.Sub);
                            eic.Emitter.ReleaseTempLocal(delta);
                            eic.NonLeaf = false;
                            return true;
                        }
                    }
                    return false;
                }
            }
        }

        /// <summary>
        /// Replaces a call to Unsafe.compareAndSwapLong with IL that directly conducts the operation.
        /// </summary>
        /// <param name="eic"></param>
        /// <returns></returns>
        bool Unsafe_compareAndSwapLong(EmitIntrinsicContext eic)
        {
            // stack layout at call site:
            // 4 Unsafe (receiver)
            // 3 Object (obj)
            // 2 long (offset)
            // 1 long (expect)
            // 0 long (update)

            var twUnsafe = eic.GetStackTypeWrapper(0, 4);
            if (twUnsafe == eic.Context.Context.VerifierJavaTypeFactory.Null)
                return false;

            for (int i = 0; ; i--)
            {
                if ((eic.Flags[eic.OpcodeIndex + i] & InstructionFlags.BranchTarget) != 0)
                    return false;

                if (eic.GetStackTypeWrapper(i, 0) == twUnsafe)
                {
                    // the pattern we recognize is:
                    // aload
                    // getstatic <offset field>
                    if (eic.Match(i, NormalizedOpCode.Aload) &&
                        eic.GetStackTypeWrapper(i + 1, 0) == eic.Caller.DeclaringType &&
                        eic.Match(i + 1, NormalizedOpCode.GetStatic))
                    {
                        var fw = GetUnsafeField(eic, eic.GetFieldref(i + 1));
                        if (fw != null && !fw.IsStatic && fw.DeclaringType == eic.Caller.DeclaringType)
                        {
                            var update = eic.Emitter.AllocTempLocal(eic.Method.DeclaringType.Context.Types.Int64);
                            var expect = eic.Emitter.AllocTempLocal(eic.Method.DeclaringType.Context.Types.Int64);
                            var target = eic.Emitter.AllocTempLocal(eic.Caller.DeclaringType.TypeAsLocalOrStackType);

                            // consume existing call site
                            eic.Emitter.Emit(OpCodes.Stloc, update);
                            eic.Emitter.Emit(OpCodes.Stloc, expect);
                            eic.Emitter.Emit(OpCodes.Pop);          // discard offset
                            eic.Emitter.Emit(OpCodes.Stloc, target);
                            EmitConsumeUnsafe(eic);

                            // emit new call site
                            eic.Emitter.Emit(OpCodes.Ldloc, target);
                            eic.Emitter.Emit(OpCodes.Ldloc, expect);
                            eic.Emitter.Emit(OpCodes.Ldloc, update);
                            fw.EmitUnsafeCompareAndSwap(eic.Emitter);

                            eic.Emitter.ReleaseTempLocal(expect);
                            eic.Emitter.ReleaseTempLocal(update);
                            eic.NonLeaf = false;
                            return true;
                        }
                    }

                    return false;
                }
            }
        }

        internal MethodInfo MakeExchange(RuntimeContext context, Type type)
        {
            return context.InterlockedMethods.ExchangeOfT.MakeGenericMethod(type);
        }

        void EmitConsumeUnsafe(EmitIntrinsicContext eic)
        {
#if IMPORTER
            if (eic.Caller.DeclaringType.ClassLoader == eic.Context.TypeWrapper.Context.JavaBase.TypeOfJavaLangObject.ClassLoader)
            {
                // we're compiling the core library (which is obviously trusted), so we don't need to check
                // if we really have an Unsafe instance
                eic.Emitter.Emit(OpCodes.Pop);
            }
            else
#endif
            {
                eic.Emitter.EmitNullCheck();
            }
        }

        RuntimeJavaField GetUnsafeField(EmitIntrinsicContext eic, ConstantPoolItemFieldref field)
        {
            if (eic.Caller.DeclaringType.ClassLoader != eic.Method.DeclaringType.Context.JavaBase.TypeOfJavaLangObject.ClassLoader)
            {
                // this code does not solve the general problem and assumes non-hostile, well behaved static initializers
                // so we only support the core class library
                return null;
            }

            // the field offset field must be a static field inside the current class
            // (we don't need to check that the field is static, because the caller already ensured that)
            if (field.GetField().DeclaringType == eic.Caller.DeclaringType)
            {
                // now look inside the static initializer to see if we can found out what field it refers to
                foreach (var method in eic.ClassFile.Methods)
                {
                    if (method.IsClassInitializer)
                    {
                        // TODO should we first verify the method?
                        // TODO should we attempt to make sure the field is definitely assigned (and only once)?

                        // TODO special case/support the pattern used by:
                        //  - java.util.concurrent.atomic.AtomicMarkableReference
                        //  - java.util.concurrent.atomic.AtomicStampedReference
                        //  - java.util.concurrent.locks.AbstractQueuedLongSynchronizer

                        /*
						 *  ldc_w test
						 *  astore_0
						 *  ...
						 *  getstatic <Field test sun/misc/Unsafe UNSAFE>
						 *  aload_0 | ldc <Class>
						 *  ldc "next"
						 *  invokevirtual <Method java/lang/Class getDeclaredField(Ljava/lang/String;)Ljava/lang/reflect/Field;>
						 *  invokevirtual <Method sun/misc/Unsafe objectFieldOffset(Ljava/lang/reflect/Field;)J>
						 *  putstatic <Field test long nextOffset>
						 */
                        for (int i = 0; i < method.Instructions.Length; i++)
                        {
                            if (method.Instructions[i].NormalizedOpCode == NormalizedOpCode.PutStatic &&
                                eic.ClassFile.GetFieldref(checked((ushort)method.Instructions[i].Arg1)) == field)
                            {
                                if (MatchInvokeVirtual(eic, ref method.Instructions[i - 1], "sun.misc.Unsafe", "objectFieldOffset", "(Ljava.lang.reflect.Field;)J") &&
                                    MatchInvokeVirtual(eic, ref method.Instructions[i - 2], "java.lang.Class", "getDeclaredField", "(Ljava.lang.String;)Ljava.lang.reflect.Field;") &&
                                    MatchLdc(eic, ref method.Instructions[i - 3], ConstantType.String) &&
                                    (method.Instructions[i - 4].NormalizedOpCode == NormalizedOpCode.Aload || method.Instructions[i - 4].NormalizedOpCode == NormalizedOpCode.Ldc) &&
                                    method.Instructions[i - 5].NormalizedOpCode == NormalizedOpCode.GetStatic && eic.ClassFile.GetFieldref(checked((ushort)method.Instructions[i - 5].Arg1)).Signature == "Lsun.misc.Unsafe;")
                                {
                                    if (method.Instructions[i - 4].NormalizedOpCode == NormalizedOpCode.Ldc)
                                    {
                                        if (eic.ClassFile.GetConstantPoolClassType(method.Instructions[i - 4].Arg1) == eic.Caller.DeclaringType)
                                        {
                                            var fieldName = eic.ClassFile.GetConstantPoolConstantString(method.Instructions[i - 3].Arg1);
                                            RuntimeJavaField fw = null;
                                            foreach (var fw1 in eic.Caller.DeclaringType.GetFields())
                                            {
                                                if (fw1.Name == fieldName)
                                                {
                                                    if (fw == null)
                                                    {
                                                        fw = fw1;
                                                    }
                                                    else
                                                    {
                                                        // duplicate name
                                                        return null;
                                                    }
                                                }
                                            }

                                            return fw;
                                        }

                                        return null;
                                    }

                                    // search backward for the astore that corresponds to the aload (of the class object)
                                    for (int j = i - 6; j > 0; j--)
                                    {
                                        if (method.Instructions[j].NormalizedOpCode == NormalizedOpCode.Astore &&
                                            method.Instructions[j].Arg1 == method.Instructions[i - 4].Arg1 &&
                                            MatchLdc(eic, ref method.Instructions[j - 1], ConstantType.Class) &&
                                            eic.ClassFile.GetConstantPoolClassType(method.Instructions[j - 1].Arg1) == eic.Caller.DeclaringType)
                                        {
                                            var fieldName = eic.ClassFile.GetConstantPoolConstantString(method.Instructions[i - 3].Arg1);
                                            RuntimeJavaField fw = null;
                                            foreach (var fw1 in eic.Caller.DeclaringType.GetFields())
                                            {
                                                if (fw1.Name == fieldName)
                                                {
                                                    if (fw == null)
                                                    {
                                                        fw = fw1;
                                                    }
                                                    else
                                                    {
                                                        // duplicate name
                                                        return null;
                                                    }
                                                }
                                            }

                                            return fw;
                                        }
                                    }

                                    break;
                                }
                            }
                        }

                        break;
                    }
                }
            }

            return null;
        }

        bool MatchInvokeVirtual(EmitIntrinsicContext eic, ref Instruction instr, string clazz, string name, string sig)
        {
            return MatchInvoke(eic, ref instr, NormalizedOpCode.InvokeVirtual, clazz, name, sig);
        }

        bool MatchInvokeStatic(EmitIntrinsicContext eic, int offset, string clazz, string name, string sig)
        {
            return MatchInvoke(eic, ref eic.Code[eic.OpcodeIndex + offset], NormalizedOpCode.InvokeStatic, clazz, name, sig);
        }

        bool MatchInvoke(EmitIntrinsicContext eic, ref Instruction instr, NormalizedOpCode opcode, string clazz, string name, string sig)
        {
            if (instr.NormalizedOpCode == opcode)
            {
                var method = eic.ClassFile.GetMethodref(instr.Arg1);
                return method.Class == clazz && method.Name == name && method.Signature == sig;
            }

            return false;
        }

        bool MatchLdc(EmitIntrinsicContext eic, ref Instruction instr, ConstantType constantType)
        {
            return (instr.NormalizedOpCode == NormalizedOpCode.Ldc || instr.NormalizedOpCode == NormalizedOpCode.LdcNothrow) && eic.ClassFile.GetConstantPoolConstantType(instr.NormalizedArg1) == constantType;
        }

#else


        internal bool IsIntrinsic(RuntimeJavaMethod mw)
        {
            return false;
        }

#endif

    }

}
