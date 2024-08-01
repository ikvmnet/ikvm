/*
  Copyright (C) 2002-2014 Jeroen Frijters

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

using IKVM.ByteCode;
using IKVM.ByteCode.Writing;


#if IMPORTER
using IKVM.Tools.Importer;
#endif

using ExceptionTableEntry = IKVM.Runtime.ClassFile.Method.ExceptionTableEntry;
using InstructionFlags = IKVM.Runtime.ClassFile.Method.InstructionFlags;

namespace IKVM.Runtime
{

    sealed class MethodAnalyzer
    {

        readonly RuntimeContext context;
        readonly RuntimeJavaType host;  // used to by Unsafe.defineAnonymousClass() to provide access to private members of the host
        readonly RuntimeJavaType wrapper;
        readonly RuntimeJavaMethod mw;
        readonly ClassFile classFile;
        readonly ClassFile.Method method;
        readonly RuntimeClassLoader classLoader;
        readonly RuntimeJavaType thisType;
        readonly InstructionState[] state;
        List<string> errorMessages;
        readonly Dictionary<int, RuntimeJavaType> newTypes = new Dictionary<int, RuntimeJavaType>();
        readonly Dictionary<int, RuntimeJavaType> faultTypes = new Dictionary<int, RuntimeJavaType>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        MethodAnalyzer(RuntimeContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets the <see cref="RuntimeContext"/> that hosts this method analyzer.
        /// </summary>
        public RuntimeContext Context => context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="host"></param>
        /// <param name="wrapper"></param>
        /// <param name="mw"></param>
        /// <param name="classFile"></param>
        /// <param name="method"></param>
        /// <param name="classLoader"></param>
        /// <exception cref="VerifyError"></exception>
        /// <exception cref="ClassFormatError"></exception>
        internal MethodAnalyzer(RuntimeContext context, RuntimeJavaType host, RuntimeJavaType wrapper, RuntimeJavaMethod mw, ClassFile classFile, ClassFile.Method method, RuntimeClassLoader classLoader) :
            this(context)
        {
            if (method.VerifyError != null)
            {
                throw new VerifyError(method.VerifyError);
            }

            this.host = host;
            this.wrapper = wrapper;
            this.mw = mw;
            this.classFile = classFile;
            this.method = method;
            this.classLoader = classLoader;
            state = new InstructionState[method.Instructions.Length];

            try
            {
                // ensure that exception blocks and handlers start and end at instruction boundaries
                for (int i = 0; i < method.ExceptionTable.Length; i++)
                {
                    int start = method.ExceptionTable[i].StartIndex;
                    int end = method.ExceptionTable[i].EndIndex;
                    int handler = method.ExceptionTable[i].HandlerIndex;
                    if (start >= end || start == -1 || end == -1 || handler <= 0)
                    {
                        throw new IndexOutOfRangeException();
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                // TODO figure out if we should throw this during class loading
                throw new ClassFormatError(string.Format("Illegal exception table (class: {0}, method: {1}, signature: {2}", classFile.Name, method.Name, method.Signature));
            }

            // start by computing the initial state, the stack is empty and the locals contain the arguments
            state[0] = new InstructionState(context, method.MaxLocals, method.MaxStack);
            int firstNonArgLocalIndex = 0;
            if (!method.IsStatic)
            {
                thisType = RuntimeVerifierJavaType.MakeThis(wrapper);
                // this reference. If we're a constructor, the this reference is uninitialized.
                if (method.IsConstructor)
                {
                    state[0].SetLocalType(firstNonArgLocalIndex++, context.VerifierJavaTypeFactory.UninitializedThis, -1);
                    state[0].SetUnitializedThis(true);
                }
                else
                {
                    state[0].SetLocalType(firstNonArgLocalIndex++, thisType, -1);
                }
            }
            else
            {
                thisType = null;
            }
            // mw can be null when we're invoked from IsSideEffectFreeStaticInitializer
            var argTypeWrappers = mw == null ? Array.Empty<RuntimeJavaType>() : mw.GetParameters();
            for (int i = 0; i < argTypeWrappers.Length; i++)
            {
                var type = argTypeWrappers[i];
                if (type.IsIntOnStackPrimitive)
                    type = context.PrimitiveJavaTypeFactory.INT;

                state[0].SetLocalType(firstNonArgLocalIndex++, type, -1);
                if (type.IsWidePrimitive)
                {
                    firstNonArgLocalIndex++;
                }
            }
            AnalyzeTypeFlow();
            VerifyPassTwo();
            PatchLoadConstants();
        }

        private void PatchLoadConstants()
        {
            var code = method.Instructions;
            for (int i = 0; i < code.Length; i++)
            {
                if (state[i] != null)
                {
                    switch (code[i].NormalizedOpCode)
                    {
                        case NormalizedOpCode._ldc:
                            switch (VerifyGetConstantPoolConstantType(new((ushort)code[i].Arg1)))
                            {
                                case ClassFile.ConstantType.Double:
                                case ClassFile.ConstantType.Float:
                                case ClassFile.ConstantType.Integer:
                                case ClassFile.ConstantType.Long:
                                case ClassFile.ConstantType.String:
                                case ClassFile.ConstantType.LiveObject:
                                    code[i].PatchOpCode(NormalizedOpCode.__ldc_nothrow);
                                    break;
                            }
                            break;
                    }
                }
            }
        }

        internal CodeInfo GetCodeInfoAndErrors(UntangledExceptionTable exceptions, out List<string> errors)
        {
            CodeInfo codeInfo = new CodeInfo(context, state);
            OptimizationPass(codeInfo, classFile, method, exceptions, wrapper, classLoader);
            PatchHardErrorsAndDynamicMemberAccess(wrapper, mw);
            errors = errorMessages;
            if (AnalyzePotentialFaultBlocks(codeInfo, method, exceptions))
            {
                AnalyzeTypeFlow();
            }
            ConvertFinallyBlocks(codeInfo, method, exceptions);
            return codeInfo;
        }

        private void AnalyzeTypeFlow()
        {
            InstructionState s = new InstructionState(context, method.MaxLocals, method.MaxStack);
            bool done = false;
            ClassFile.Method.Instruction[] instructions = method.Instructions;
            while (!done)
            {
                done = true;
                for (int i = 0; i < instructions.Length; i++)
                {
                    if (state[i] != null && state[i].changed)
                    {
                        try
                        {
                            //Console.WriteLine(method.Instructions[i].PC + ": " + method.Instructions[i].OpCode.ToString());
                            done = false;
                            state[i].changed = false;
                            // mark the exception handlers reachable from this instruction
                            for (int j = 0; j < method.ExceptionTable.Length; j++)
                            {
                                if (method.ExceptionTable[j].StartIndex <= i && i < method.ExceptionTable[j].EndIndex)
                                {
                                    MergeExceptionHandler(j, state[i]);
                                }
                            }
                            state[i].CopyTo(s);
                            ClassFile.Method.Instruction instr = instructions[i];
                            switch (instr.NormalizedOpCode)
                            {
                                case NormalizedOpCode._aload:
                                    {
                                        RuntimeJavaType type = s.GetLocalType(instr.NormalizedArg1);
                                        if (type == context.VerifierJavaTypeFactory.Invalid || type.IsPrimitive)
                                        {
                                            throw new VerifyError("Object reference expected");
                                        }
                                        s.PushType(type);
                                        break;
                                    }
                                case NormalizedOpCode._astore:
                                    {
                                        if (RuntimeVerifierJavaType.IsFaultBlockException(s.PeekType()))
                                        {
                                            s.SetLocalType(instr.NormalizedArg1, s.PopFaultBlockException(), i);
                                            break;
                                        }
                                        // NOTE since the reference can be uninitialized, we cannot use PopObjectType
                                        RuntimeJavaType type = s.PopType();
                                        if (type.IsPrimitive)
                                        {
                                            throw new VerifyError("Object reference expected");
                                        }
                                        s.SetLocalType(instr.NormalizedArg1, type, i);
                                        break;
                                    }
                                case NormalizedOpCode._aconst_null:
                                    s.PushType(context.VerifierJavaTypeFactory.Null);
                                    break;
                                case NormalizedOpCode._aaload:
                                    {
                                        s.PopInt();
                                        RuntimeJavaType type = s.PopArrayType();
                                        if (type == context.VerifierJavaTypeFactory.Null)
                                        {
                                            // if the array is null, we have use null as the element type, because
                                            // otherwise the rest of the code will not verify correctly
                                            s.PushType(context.VerifierJavaTypeFactory.Null);
                                        }
                                        else if (type.IsUnloadable)
                                        {
                                            s.PushType(context.VerifierJavaTypeFactory.Unloadable);
                                        }
                                        else
                                        {
                                            type = type.ElementTypeWrapper;
                                            if (type.IsPrimitive)
                                            {
                                                throw new VerifyError("Object array expected");
                                            }
                                            s.PushType(type);
                                        }
                                        break;
                                    }
                                case NormalizedOpCode._aastore:
                                    s.PopObjectType();
                                    s.PopInt();
                                    s.PopArrayType();
                                    // TODO check that elem is assignable to the array
                                    break;
                                case NormalizedOpCode._baload:
                                    {
                                        s.PopInt();
                                        RuntimeJavaType type = s.PopArrayType();
                                        if (!RuntimeVerifierJavaType.IsNullOrUnloadable(type) &&
                                            type != context.MethodAnalyzerFactory.ByteArrayType &&
                                            type != context.MethodAnalyzerFactory.BooleanArrayType)
                                        {
                                            throw new VerifyError();
                                        }
                                        s.PushInt();
                                        break;
                                    }
                                case NormalizedOpCode._bastore:
                                    {
                                        s.PopInt();
                                        s.PopInt();
                                        RuntimeJavaType type = s.PopArrayType();
                                        if (!RuntimeVerifierJavaType.IsNullOrUnloadable(type) &&
                                            type != context.MethodAnalyzerFactory.ByteArrayType &&
                                            type != context.MethodAnalyzerFactory.BooleanArrayType)
                                        {
                                            throw new VerifyError();
                                        }
                                        break;
                                    }
                                case NormalizedOpCode._caload:
                                    s.PopInt();
                                    s.PopObjectType(context.MethodAnalyzerFactory.CharArrayType);
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode._castore:
                                    s.PopInt();
                                    s.PopInt();
                                    s.PopObjectType(context.MethodAnalyzerFactory.CharArrayType);
                                    break;
                                case NormalizedOpCode._saload:
                                    s.PopInt();
                                    s.PopObjectType(context.MethodAnalyzerFactory.ShortArrayType);
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode._sastore:
                                    s.PopInt();
                                    s.PopInt();
                                    s.PopObjectType(context.MethodAnalyzerFactory.ShortArrayType);
                                    break;
                                case NormalizedOpCode._iaload:
                                    s.PopInt();
                                    s.PopObjectType(context.MethodAnalyzerFactory.IntArrayType);
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode._iastore:
                                    s.PopInt();
                                    s.PopInt();
                                    s.PopObjectType(context.MethodAnalyzerFactory.IntArrayType);
                                    break;
                                case NormalizedOpCode._laload:
                                    s.PopInt();
                                    s.PopObjectType(context.MethodAnalyzerFactory.LongArrayType);
                                    s.PushLong();
                                    break;
                                case NormalizedOpCode._lastore:
                                    s.PopLong();
                                    s.PopInt();
                                    s.PopObjectType(context.MethodAnalyzerFactory.LongArrayType);
                                    break;
                                case NormalizedOpCode._daload:
                                    s.PopInt();
                                    s.PopObjectType(context.MethodAnalyzerFactory.DoubleArrayType);
                                    s.PushDouble();
                                    break;
                                case NormalizedOpCode._dastore:
                                    s.PopDouble();
                                    s.PopInt();
                                    s.PopObjectType(context.MethodAnalyzerFactory.DoubleArrayType);
                                    break;
                                case NormalizedOpCode._faload:
                                    s.PopInt();
                                    s.PopObjectType(context.MethodAnalyzerFactory.FloatArrayType);
                                    s.PushFloat();
                                    break;
                                case NormalizedOpCode._fastore:
                                    s.PopFloat();
                                    s.PopInt();
                                    s.PopObjectType(context.MethodAnalyzerFactory.FloatArrayType);
                                    break;
                                case NormalizedOpCode._arraylength:
                                    s.PopArrayType();
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode.__iconst:
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode._if_icmpeq:
                                case NormalizedOpCode._if_icmpne:
                                case NormalizedOpCode._if_icmplt:
                                case NormalizedOpCode._if_icmpge:
                                case NormalizedOpCode._if_icmpgt:
                                case NormalizedOpCode._if_icmple:
                                    s.PopInt();
                                    s.PopInt();
                                    break;
                                case NormalizedOpCode._ifeq:
                                case NormalizedOpCode._ifge:
                                case NormalizedOpCode._ifgt:
                                case NormalizedOpCode._ifle:
                                case NormalizedOpCode._iflt:
                                case NormalizedOpCode._ifne:
                                    s.PopInt();
                                    break;
                                case NormalizedOpCode._ifnonnull:
                                case NormalizedOpCode._ifnull:
                                    // TODO it might be legal to use an unitialized ref here
                                    s.PopObjectType();
                                    break;
                                case NormalizedOpCode._if_acmpeq:
                                case NormalizedOpCode._if_acmpne:
                                    // TODO it might be legal to use an unitialized ref here
                                    s.PopObjectType();
                                    s.PopObjectType();
                                    break;
                                case NormalizedOpCode._getstatic:
                                case NormalizedOpCode.__dynamic_getstatic:
                                    // special support for when we're being called from IsSideEffectFreeStaticInitializer
                                    if (mw == null)
                                    {
                                        switch (VerifyGetFieldref(instr.Arg1).Signature[0])
                                        {
                                            case 'B':
                                            case 'Z':
                                            case 'C':
                                            case 'S':
                                            case 'I':
                                                s.PushInt();
                                                break;
                                            case 'F':
                                                s.PushFloat();
                                                break;
                                            case 'D':
                                                s.PushDouble();
                                                break;
                                            case 'J':
                                                s.PushLong();
                                                break;
                                            case 'L':
                                            case '[':
                                                throw new VerifyError();
                                            default:
                                                throw new InvalidOperationException();
                                        }
                                    }
                                    else
                                    {
                                        ClassFile.ConstantPoolItemFieldref cpi = VerifyGetFieldref(instr.Arg1);
                                        if (cpi.GetField() != null && cpi.GetField().FieldTypeWrapper.IsUnloadable)
                                        {
                                            s.PushType(cpi.GetField().FieldTypeWrapper);
                                        }
                                        else
                                        {
                                            s.PushType(cpi.GetFieldType());
                                        }
                                    }
                                    break;
                                case NormalizedOpCode._putstatic:
                                case NormalizedOpCode.__dynamic_putstatic:
                                    // special support for when we're being called from IsSideEffectFreeStaticInitializer
                                    if (mw == null)
                                    {
                                        switch (VerifyGetFieldref(instr.Arg1).Signature[0])
                                        {
                                            case 'B':
                                            case 'Z':
                                            case 'C':
                                            case 'S':
                                            case 'I':
                                                s.PopInt();
                                                break;
                                            case 'F':
                                                s.PopFloat();
                                                break;
                                            case 'D':
                                                s.PopDouble();
                                                break;
                                            case 'J':
                                                s.PopLong();
                                                break;
                                            case 'L':
                                            case '[':
                                                if (s.PopAnyType() != context.VerifierJavaTypeFactory.Null)
                                                {
                                                    throw new VerifyError();
                                                }
                                                break;
                                            default:
                                                throw new InvalidOperationException();
                                        }
                                    }
                                    else
                                    {
                                        s.PopType(VerifyGetFieldref(instr.Arg1).GetFieldType());
                                    }
                                    break;
                                case NormalizedOpCode._getfield:
                                case NormalizedOpCode.__dynamic_getfield:
                                    {
                                        s.PopObjectType(VerifyGetFieldref(instr.Arg1).GetClassType());
                                        ClassFile.ConstantPoolItemFieldref cpi = VerifyGetFieldref(instr.Arg1);
                                        if (cpi.GetField() != null && cpi.GetField().FieldTypeWrapper.IsUnloadable)
                                        {
                                            s.PushType(cpi.GetField().FieldTypeWrapper);
                                        }
                                        else
                                        {
                                            s.PushType(cpi.GetFieldType());
                                        }
                                        break;
                                    }
                                case NormalizedOpCode._putfield:
                                case NormalizedOpCode.__dynamic_putfield:
                                    s.PopType(VerifyGetFieldref(instr.Arg1).GetFieldType());
                                    // putfield is allowed to access the uninitialized this
                                    if (s.PeekType() == context.VerifierJavaTypeFactory.UninitializedThis
                                        && wrapper.IsAssignableTo(VerifyGetFieldref(instr.Arg1).GetClassType()))
                                    {
                                        s.PopType();
                                    }
                                    else
                                    {
                                        s.PopObjectType(VerifyGetFieldref(instr.Arg1).GetClassType());
                                    }
                                    break;
                                case NormalizedOpCode.__ldc_nothrow:
                                case NormalizedOpCode._ldc:
                                    {
                                        switch (VerifyGetConstantPoolConstantType(new((ushort)instr.Arg1)))
                                        {
                                            case ClassFile.ConstantType.Double:
                                                s.PushDouble();
                                                break;
                                            case ClassFile.ConstantType.Float:
                                                s.PushFloat();
                                                break;
                                            case ClassFile.ConstantType.Integer:
                                                s.PushInt();
                                                break;
                                            case ClassFile.ConstantType.Long:
                                                s.PushLong();
                                                break;
                                            case ClassFile.ConstantType.String:
                                                s.PushType(context.JavaBase.TypeOfJavaLangString);
                                                break;
                                            case ClassFile.ConstantType.LiveObject:
                                                s.PushType(context.JavaBase.TypeOfJavaLangObject);
                                                break;
                                            case ClassFile.ConstantType.Class:
                                                if (classFile.MajorVersion < 49)
                                                {
                                                    throw new VerifyError("Illegal type in constant pool");
                                                }
                                                s.PushType(context.JavaBase.TypeOfJavaLangClass);
                                                break;
                                            case ClassFile.ConstantType.MethodHandle:
                                                s.PushType(context.JavaBase.TypeOfJavaLangInvokeMethodHandle);
                                                break;
                                            case ClassFile.ConstantType.MethodType:
                                                s.PushType(context.JavaBase.TypeOfJavaLangInvokeMethodType);
                                                break;
                                            default:
                                                // NOTE this is not a VerifyError, because it cannot happen (unless we have
                                                // a bug in ClassFile.GetConstantPoolConstantType)
                                                throw new InvalidOperationException();
                                        }
                                        break;
                                    }
                                case NormalizedOpCode.__clone_array:
                                case NormalizedOpCode._invokevirtual:
                                case NormalizedOpCode._invokespecial:
                                case NormalizedOpCode._invokeinterface:
                                case NormalizedOpCode._invokestatic:
                                case NormalizedOpCode.__dynamic_invokevirtual:
                                case NormalizedOpCode.__dynamic_invokespecial:
                                case NormalizedOpCode.__dynamic_invokeinterface:
                                case NormalizedOpCode.__dynamic_invokestatic:
                                case NormalizedOpCode.__privileged_invokevirtual:
                                case NormalizedOpCode.__privileged_invokespecial:
                                case NormalizedOpCode.__privileged_invokestatic:
                                case NormalizedOpCode.__methodhandle_invoke:
                                case NormalizedOpCode.__methodhandle_link:
                                    {
                                        ClassFile.ConstantPoolItemMI cpi = VerifyGetMethodref(instr.Arg1);
                                        RuntimeJavaType retType = cpi.GetRetType();
                                        // HACK to allow the result of Unsafe.getObjectVolatile() (on an array)
                                        // to be used with Unsafe.putObject() we need to propagate the
                                        // element type here as the return type (instead of object)
                                        if (cpi.GetMethod() != null
                                            && cpi.GetMethod().IsIntrinsic
                                            && cpi.Class == "sun.misc.Unsafe"
                                            && cpi.Name == "getObjectVolatile"
                                            && CodeIntrinsics.IsSupportedArrayTypeForUnsafeOperation(s.GetStackSlot(1)))
                                        {
                                            retType = s.GetStackSlot(1).ElementTypeWrapper;
                                        }
                                        s.MultiPopAnyType(cpi.GetArgTypes().Length);
                                        if (instr.NormalizedOpCode != NormalizedOpCode._invokestatic
                                            && instr.NormalizedOpCode != NormalizedOpCode.__dynamic_invokestatic)
                                        {
                                            RuntimeJavaType type = s.PopType();
                                            if (ReferenceEquals(cpi.Name, StringConstants.INIT))
                                            {
                                                // after we've invoked the constructor, the uninitialized references
                                                // are now initialized
                                                if (type == context.VerifierJavaTypeFactory.UninitializedThis)
                                                {
                                                    if (s.GetLocalTypeEx(0) == type)
                                                    {
                                                        s.SetLocalType(0, thisType, i);
                                                    }
                                                    s.MarkInitialized(type, wrapper, i);
                                                    s.SetUnitializedThis(false);
                                                }
                                                else if (RuntimeVerifierJavaType.IsNew(type))
                                                {
                                                    s.MarkInitialized(type, ((RuntimeVerifierJavaType)type).UnderlyingType, i);
                                                }
                                                else
                                                {
                                                    // This is a VerifyError, but it will be caught by our second pass
                                                }
                                            }
                                        }
                                        if (retType != context.PrimitiveJavaTypeFactory.VOID)
                                        {
                                            if (cpi.GetMethod() != null && cpi.GetMethod().ReturnType.IsUnloadable)
                                            {
                                                s.PushType(cpi.GetMethod().ReturnType);
                                            }
                                            else if (retType == context.PrimitiveJavaTypeFactory.DOUBLE)
                                            {
                                                s.PushExtendedDouble();
                                            }
                                            else if (retType == context.PrimitiveJavaTypeFactory.FLOAT)
                                            {
                                                s.PushExtendedFloat();
                                            }
                                            else
                                            {
                                                s.PushType(retType);
                                            }
                                        }
                                        break;
                                    }
                                case NormalizedOpCode._invokedynamic:
                                    {
                                        var cpi = VerifyGetInvokeDynamic(new((ushort)instr.Arg1));
                                        s.MultiPopAnyType(cpi.GetArgTypes().Length);
                                        var retType = cpi.GetRetType();
                                        if (retType != context.PrimitiveJavaTypeFactory.VOID)
                                        {
                                            if (retType == context.PrimitiveJavaTypeFactory.DOUBLE)
                                            {
                                                s.PushExtendedDouble();
                                            }
                                            else if (retType == context.PrimitiveJavaTypeFactory.FLOAT)
                                            {
                                                s.PushExtendedFloat();
                                            }
                                            else
                                            {
                                                s.PushType(retType);
                                            }
                                        }
                                        break;
                                    }
                                case NormalizedOpCode._goto:
                                    break;
                                case NormalizedOpCode._istore:
                                    s.PopInt();
                                    s.SetLocalInt(instr.NormalizedArg1, i);
                                    break;
                                case NormalizedOpCode._iload:
                                    s.GetLocalInt(instr.NormalizedArg1);
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode._ineg:
                                    s.PopInt();
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode._iadd:
                                case NormalizedOpCode._isub:
                                case NormalizedOpCode._imul:
                                case NormalizedOpCode._idiv:
                                case NormalizedOpCode._irem:
                                case NormalizedOpCode._iand:
                                case NormalizedOpCode._ior:
                                case NormalizedOpCode._ixor:
                                case NormalizedOpCode._ishl:
                                case NormalizedOpCode._ishr:
                                case NormalizedOpCode._iushr:
                                    s.PopInt();
                                    s.PopInt();
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode._lneg:
                                    s.PopLong();
                                    s.PushLong();
                                    break;
                                case NormalizedOpCode._ladd:
                                case NormalizedOpCode._lsub:
                                case NormalizedOpCode._lmul:
                                case NormalizedOpCode._ldiv:
                                case NormalizedOpCode._lrem:
                                case NormalizedOpCode._land:
                                case NormalizedOpCode._lor:
                                case NormalizedOpCode._lxor:
                                    s.PopLong();
                                    s.PopLong();
                                    s.PushLong();
                                    break;
                                case NormalizedOpCode._lshl:
                                case NormalizedOpCode._lshr:
                                case NormalizedOpCode._lushr:
                                    s.PopInt();
                                    s.PopLong();
                                    s.PushLong();
                                    break;
                                case NormalizedOpCode._fneg:
                                    if (s.PopFloat())
                                    {
                                        s.PushExtendedFloat();
                                    }
                                    else
                                    {
                                        s.PushFloat();
                                    }
                                    break;
                                case NormalizedOpCode._fadd:
                                case NormalizedOpCode._fsub:
                                case NormalizedOpCode._fmul:
                                case NormalizedOpCode._fdiv:
                                case NormalizedOpCode._frem:
                                    s.PopFloat();
                                    s.PopFloat();
                                    s.PushExtendedFloat();
                                    break;
                                case NormalizedOpCode._dneg:
                                    if (s.PopDouble())
                                    {
                                        s.PushExtendedDouble();
                                    }
                                    else
                                    {
                                        s.PushDouble();
                                    }
                                    break;
                                case NormalizedOpCode._dadd:
                                case NormalizedOpCode._dsub:
                                case NormalizedOpCode._dmul:
                                case NormalizedOpCode._ddiv:
                                case NormalizedOpCode._drem:
                                    s.PopDouble();
                                    s.PopDouble();
                                    s.PushExtendedDouble();
                                    break;
                                case NormalizedOpCode._new:
                                    {
                                        // mark the type, so that we can ascertain that it is a "new object"
                                        RuntimeJavaType type;
                                        if (!newTypes.TryGetValue(i, out type))
                                        {
                                            type = VerifyGetConstantPoolClassType(new((ushort)instr.Arg1));
                                            if (type.IsArray)
                                            {
                                                throw new VerifyError("Illegal use of array type");
                                            }
                                            type = RuntimeVerifierJavaType.MakeNew(type, i);
                                            newTypes[i] = type;
                                        }
                                        s.PushType(type);
                                        break;
                                    }
                                case NormalizedOpCode._multianewarray:
                                    {
                                        if (instr.Arg2 < 1)
                                        {
                                            throw new VerifyError("Illegal dimension argument");
                                        }
                                        for (int j = 0; j < instr.Arg2; j++)
                                        {
                                            s.PopInt();
                                        }
                                        RuntimeJavaType type = VerifyGetConstantPoolClassType(new((ushort)instr.Arg1));
                                        if (type.ArrayRank < instr.Arg2)
                                        {
                                            throw new VerifyError("Illegal dimension argument");
                                        }
                                        s.PushType(type);
                                        break;
                                    }
                                case NormalizedOpCode._anewarray:
                                    {
                                        s.PopInt();
                                        RuntimeJavaType type = VerifyGetConstantPoolClassType(new((ushort)instr.Arg1));
                                        if (type.IsUnloadable)
                                        {
                                            s.PushType(new RuntimeUnloadableJavaType(context, "[" + type.SigName));
                                        }
                                        else
                                        {
                                            s.PushType(type.MakeArrayType(1));
                                        }
                                        break;
                                    }
                                case NormalizedOpCode._newarray:
                                    s.PopInt();
                                    switch (instr.Arg1)
                                    {
                                        case 4:
                                            s.PushType(context.MethodAnalyzerFactory.BooleanArrayType);
                                            break;
                                        case 5:
                                            s.PushType(context.MethodAnalyzerFactory.CharArrayType);
                                            break;
                                        case 6:
                                            s.PushType(context.MethodAnalyzerFactory.FloatArrayType);
                                            break;
                                        case 7:
                                            s.PushType(context.MethodAnalyzerFactory.DoubleArrayType);
                                            break;
                                        case 8:
                                            s.PushType(context.MethodAnalyzerFactory.ByteArrayType);
                                            break;
                                        case 9:
                                            s.PushType(context.MethodAnalyzerFactory.ShortArrayType);
                                            break;
                                        case 10:
                                            s.PushType(context.MethodAnalyzerFactory.IntArrayType);
                                            break;
                                        case 11:
                                            s.PushType(context.MethodAnalyzerFactory.LongArrayType);
                                            break;
                                        default:
                                            throw new VerifyError("Bad type");
                                    }
                                    break;
                                case NormalizedOpCode._swap:
                                    {
                                        RuntimeJavaType t1 = s.PopType();
                                        RuntimeJavaType t2 = s.PopType();
                                        s.PushType(t1);
                                        s.PushType(t2);
                                        break;
                                    }
                                case NormalizedOpCode._dup:
                                    {
                                        RuntimeJavaType t = s.PopType();
                                        s.PushType(t);
                                        s.PushType(t);
                                        break;
                                    }
                                case NormalizedOpCode._dup2:
                                    {
                                        RuntimeJavaType t = s.PopAnyType();
                                        if (t.IsWidePrimitive || t == context.VerifierJavaTypeFactory.ExtendedDouble)
                                        {
                                            s.PushType(t);
                                            s.PushType(t);
                                        }
                                        else
                                        {
                                            RuntimeJavaType t2 = s.PopType();
                                            s.PushType(t2);
                                            s.PushType(t);
                                            s.PushType(t2);
                                            s.PushType(t);
                                        }
                                        break;
                                    }
                                case NormalizedOpCode._dup_x1:
                                    {
                                        RuntimeJavaType value1 = s.PopType();
                                        RuntimeJavaType value2 = s.PopType();
                                        s.PushType(value1);
                                        s.PushType(value2);
                                        s.PushType(value1);
                                        break;
                                    }
                                case NormalizedOpCode._dup2_x1:
                                    {
                                        RuntimeJavaType value1 = s.PopAnyType();
                                        if (value1.IsWidePrimitive || value1 == context.VerifierJavaTypeFactory.ExtendedDouble)
                                        {
                                            RuntimeJavaType value2 = s.PopType();
                                            s.PushType(value1);
                                            s.PushType(value2);
                                            s.PushType(value1);
                                        }
                                        else
                                        {
                                            RuntimeJavaType value2 = s.PopType();
                                            RuntimeJavaType value3 = s.PopType();
                                            s.PushType(value2);
                                            s.PushType(value1);
                                            s.PushType(value3);
                                            s.PushType(value2);
                                            s.PushType(value1);
                                        }
                                        break;
                                    }
                                case NormalizedOpCode._dup_x2:
                                    {
                                        RuntimeJavaType value1 = s.PopType();
                                        RuntimeJavaType value2 = s.PopAnyType();
                                        if (value2.IsWidePrimitive || value2 == context.VerifierJavaTypeFactory.ExtendedDouble)
                                        {
                                            s.PushType(value1);
                                            s.PushType(value2);
                                            s.PushType(value1);
                                        }
                                        else
                                        {
                                            RuntimeJavaType value3 = s.PopType();
                                            s.PushType(value1);
                                            s.PushType(value3);
                                            s.PushType(value2);
                                            s.PushType(value1);
                                        }
                                        break;
                                    }
                                case NormalizedOpCode._dup2_x2:
                                    {
                                        RuntimeJavaType value1 = s.PopAnyType();
                                        if (value1.IsWidePrimitive || value1 == context.VerifierJavaTypeFactory.ExtendedDouble)
                                        {
                                            RuntimeJavaType value2 = s.PopAnyType();
                                            if (value2.IsWidePrimitive || value2 == context.VerifierJavaTypeFactory.ExtendedDouble)
                                            {
                                                // Form 4
                                                s.PushType(value1);
                                                s.PushType(value2);
                                                s.PushType(value1);
                                            }
                                            else
                                            {
                                                // Form 2
                                                RuntimeJavaType value3 = s.PopType();
                                                s.PushType(value1);
                                                s.PushType(value3);
                                                s.PushType(value2);
                                                s.PushType(value1);
                                            }
                                        }
                                        else
                                        {
                                            RuntimeJavaType value2 = s.PopType();
                                            RuntimeJavaType value3 = s.PopAnyType();
                                            if (value3.IsWidePrimitive || value3 == context.VerifierJavaTypeFactory.ExtendedDouble)
                                            {
                                                // Form 3
                                                s.PushType(value2);
                                                s.PushType(value1);
                                                s.PushType(value3);
                                                s.PushType(value2);
                                                s.PushType(value1);
                                            }
                                            else
                                            {
                                                // Form 4
                                                RuntimeJavaType value4 = s.PopType();
                                                s.PushType(value2);
                                                s.PushType(value1);
                                                s.PushType(value4);
                                                s.PushType(value3);
                                                s.PushType(value2);
                                                s.PushType(value1);
                                            }
                                        }
                                        break;
                                    }
                                case NormalizedOpCode._pop:
                                    s.PopType();
                                    break;
                                case NormalizedOpCode._pop2:
                                    {
                                        RuntimeJavaType type = s.PopAnyType();
                                        if (!type.IsWidePrimitive && type != context.VerifierJavaTypeFactory.ExtendedDouble)
                                        {
                                            s.PopType();
                                        }
                                        break;
                                    }
                                case NormalizedOpCode._monitorenter:
                                case NormalizedOpCode._monitorexit:
                                    // TODO these bytecodes are allowed on an uninitialized object, but
                                    // we don't support that at the moment...
                                    s.PopObjectType();
                                    break;
                                case NormalizedOpCode._return:
                                    // mw is null if we're called from IsSideEffectFreeStaticInitializer
                                    if (mw != null)
                                    {
                                        if (mw.ReturnType != context.PrimitiveJavaTypeFactory.VOID)
                                        {
                                            throw new VerifyError("Wrong return type in function");
                                        }
                                        // if we're a constructor, make sure we called the base class constructor
                                        s.CheckUninitializedThis();
                                    }
                                    break;
                                case NormalizedOpCode._areturn:
                                    s.PopObjectType(mw.ReturnType);
                                    break;
                                case NormalizedOpCode._ireturn:
                                    {
                                        s.PopInt();
                                        if (!mw.ReturnType.IsIntOnStackPrimitive)
                                        {
                                            throw new VerifyError("Wrong return type in function");
                                        }
                                        break;
                                    }
                                case NormalizedOpCode._lreturn:
                                    s.PopLong();
                                    if (mw.ReturnType != context.PrimitiveJavaTypeFactory.LONG)
                                    {
                                        throw new VerifyError("Wrong return type in function");
                                    }
                                    break;
                                case NormalizedOpCode._freturn:
                                    s.PopFloat();
                                    if (mw.ReturnType != context.PrimitiveJavaTypeFactory.FLOAT)
                                    {
                                        throw new VerifyError("Wrong return type in function");
                                    }
                                    break;
                                case NormalizedOpCode._dreturn:
                                    s.PopDouble();
                                    if (mw.ReturnType != context.PrimitiveJavaTypeFactory.DOUBLE)
                                    {
                                        throw new VerifyError("Wrong return type in function");
                                    }
                                    break;
                                case NormalizedOpCode._fload:
                                    s.GetLocalFloat(instr.NormalizedArg1);
                                    s.PushFloat();
                                    break;
                                case NormalizedOpCode._fstore:
                                    s.PopFloat();
                                    s.SetLocalFloat(instr.NormalizedArg1, i);
                                    break;
                                case NormalizedOpCode._dload:
                                    s.GetLocalDouble(instr.NormalizedArg1);
                                    s.PushDouble();
                                    break;
                                case NormalizedOpCode._dstore:
                                    s.PopDouble();
                                    s.SetLocalDouble(instr.NormalizedArg1, i);
                                    break;
                                case NormalizedOpCode._lload:
                                    s.GetLocalLong(instr.NormalizedArg1);
                                    s.PushLong();
                                    break;
                                case NormalizedOpCode._lstore:
                                    s.PopLong();
                                    s.SetLocalLong(instr.NormalizedArg1, i);
                                    break;
                                case NormalizedOpCode._lconst_0:
                                case NormalizedOpCode._lconst_1:
                                    s.PushLong();
                                    break;
                                case NormalizedOpCode._fconst_0:
                                case NormalizedOpCode._fconst_1:
                                case NormalizedOpCode._fconst_2:
                                    s.PushFloat();
                                    break;
                                case NormalizedOpCode._dconst_0:
                                case NormalizedOpCode._dconst_1:
                                    s.PushDouble();
                                    break;
                                case NormalizedOpCode._lcmp:
                                    s.PopLong();
                                    s.PopLong();
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode._fcmpl:
                                case NormalizedOpCode._fcmpg:
                                    s.PopFloat();
                                    s.PopFloat();
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode._dcmpl:
                                case NormalizedOpCode._dcmpg:
                                    s.PopDouble();
                                    s.PopDouble();
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode._checkcast:
                                    s.PopObjectType();
                                    s.PushType(VerifyGetConstantPoolClassType(new((ushort)instr.Arg1)));
                                    break;
                                case NormalizedOpCode._instanceof:
                                    s.PopObjectType();
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode._iinc:
                                    s.GetLocalInt(instr.Arg1);
                                    break;
                                case NormalizedOpCode._athrow:
                                    if (RuntimeVerifierJavaType.IsFaultBlockException(s.PeekType()))
                                    {
                                        s.PopFaultBlockException();
                                    }
                                    else
                                    {
                                        s.PopObjectType(context.JavaBase.TypeOfjavaLangThrowable);
                                    }
                                    break;
                                case NormalizedOpCode._tableswitch:
                                case NormalizedOpCode._lookupswitch:
                                    s.PopInt();
                                    break;
                                case NormalizedOpCode._i2b:
                                    s.PopInt();
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode._i2c:
                                    s.PopInt();
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode._i2s:
                                    s.PopInt();
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode._i2l:
                                    s.PopInt();
                                    s.PushLong();
                                    break;
                                case NormalizedOpCode._i2f:
                                    s.PopInt();
                                    s.PushFloat();
                                    break;
                                case NormalizedOpCode._i2d:
                                    s.PopInt();
                                    s.PushDouble();
                                    break;
                                case NormalizedOpCode._l2i:
                                    s.PopLong();
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode._l2f:
                                    s.PopLong();
                                    s.PushFloat();
                                    break;
                                case NormalizedOpCode._l2d:
                                    s.PopLong();
                                    s.PushDouble();
                                    break;
                                case NormalizedOpCode._f2i:
                                    s.PopFloat();
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode._f2l:
                                    s.PopFloat();
                                    s.PushLong();
                                    break;
                                case NormalizedOpCode._f2d:
                                    s.PopFloat();
                                    s.PushDouble();
                                    break;
                                case NormalizedOpCode._d2i:
                                    s.PopDouble();
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode._d2f:
                                    s.PopDouble();
                                    s.PushFloat();
                                    break;
                                case NormalizedOpCode._d2l:
                                    s.PopDouble();
                                    s.PushLong();
                                    break;
                                case NormalizedOpCode._nop:
                                    if (i + 1 == instructions.Length)
                                    {
                                        throw new VerifyError("Falling off the end of the code");
                                    }
                                    break;
                                case NormalizedOpCode.__static_error:
                                    break;
                                case NormalizedOpCode._jsr:
                                case NormalizedOpCode._ret:
                                    throw new VerifyError("Bad instruction");
                                default:
                                    throw new NotImplementedException(instr.NormalizedOpCode.ToString());
                            }
                            if (s.GetStackHeight() > method.MaxStack)
                            {
                                throw new VerifyError("Stack size too large");
                            }
                            for (int j = 0; j < method.ExceptionTable.Length; j++)
                            {
                                if (method.ExceptionTable[j].EndIndex == i + 1)
                                {
                                    MergeExceptionHandler(j, s);
                                }
                            }
                            try
                            {
                                switch (OpCodeMetaData.GetFlowControl(instr.NormalizedOpCode))
                                {
                                    case ByteCodeFlowControl.Switch:
                                        for (int j = 0; j < instr.SwitchEntryCount; j++)
                                        {
                                            state[instr.GetSwitchTargetIndex(j)] += s;
                                        }
                                        state[instr.DefaultTarget] += s;
                                        break;
                                    case ByteCodeFlowControl.CondBranch:
                                        state[i + 1] += s;
                                        state[instr.TargetIndex] += s;
                                        break;
                                    case ByteCodeFlowControl.Branch:
                                        state[instr.TargetIndex] += s;
                                        break;
                                    case ByteCodeFlowControl.Return:
                                    case ByteCodeFlowControl.Throw:
                                        break;
                                    case ByteCodeFlowControl.Next:
                                        state[i + 1] += s;
                                        break;
                                    default:
                                        throw new InvalidOperationException();
                                }
                            }
                            catch (IndexOutOfRangeException)
                            {
                                // we're going to assume that this always means that we have an invalid branch target
                                // NOTE because PcIndexMap returns -1 for illegal PCs (in the middle of an instruction) and
                                // we always use that value as an index into the state array, any invalid PC will result
                                // in an IndexOutOfRangeException
                                throw new VerifyError("Illegal target of jump or branch");
                            }
                        }
                        catch (VerifyError x)
                        {
                            string opcode = instructions[i].NormalizedOpCode.ToString();
                            if (opcode.StartsWith("__"))
                            {
                                opcode = opcode.Substring(2);
                            }
                            throw new VerifyError(string.Format("{5} (class: {0}, method: {1}, signature: {2}, offset: {3}, instruction: {4})",
                                classFile.Name, method.Name, method.Signature, instructions[i].PC, opcode, x.Message), x);
                        }
                    }
                }
            }
        }

        private void MergeExceptionHandler(int exceptionIndex, InstructionState curr)
        {
            int idx = method.ExceptionTable[exceptionIndex].HandlerIndex;
            InstructionState ex = curr.CopyLocals();
            var catch_type = method.ExceptionTable[exceptionIndex].CatchType;
            if (catch_type.IsNil)
            {
                if (!faultTypes.TryGetValue(idx, out var tw))
                {
                    tw = RuntimeVerifierJavaType.MakeFaultBlockException(this, idx);
                    faultTypes.Add(idx, tw);
                }

                ex.PushType(tw);
            }
            else
            {
                // TODO if the exception type is unloadable we should consider pushing
                // Throwable as the type and recording a loader constraint
                ex.PushType(VerifyGetConstantPoolClassType(catch_type));
            }

            state[idx] += ex;
        }

        // this verification pass must run on the unmodified bytecode stream
        private void VerifyPassTwo()
        {
            ClassFile.Method.Instruction[] instructions = method.Instructions;
            for (int i = 0; i < instructions.Length; i++)
            {
                if (state[i] != null)
                {
                    try
                    {
                        switch (instructions[i].NormalizedOpCode)
                        {
                            case NormalizedOpCode._invokeinterface:
                            case NormalizedOpCode._invokespecial:
                            case NormalizedOpCode._invokestatic:
                            case NormalizedOpCode._invokevirtual:
                                VerifyInvokePassTwo(i);
                                break;
                            case NormalizedOpCode._invokedynamic:
                                VerifyInvokeDynamic(i);
                                break;
                        }
                    }
                    catch (VerifyError x)
                    {
                        string opcode = instructions[i].NormalizedOpCode.ToString();
                        if (opcode.StartsWith("__"))
                        {
                            opcode = opcode.Substring(2);
                        }
                        throw new VerifyError(string.Format("{5} (class: {0}, method: {1}, signature: {2}, offset: {3}, instruction: {4})",
                            classFile.Name, method.Name, method.Signature, instructions[i].PC, opcode, x.Message), x);
                    }
                }
            }
        }

        private void VerifyInvokePassTwo(int index)
        {
            StackState stack = new StackState(state[index]);
            NormalizedOpCode invoke = method.Instructions[index].NormalizedOpCode;
            ClassFile.ConstantPoolItemMI cpi = VerifyGetMethodref(method.Instructions[index].Arg1);
            if ((invoke == NormalizedOpCode._invokestatic || invoke == NormalizedOpCode._invokespecial) && classFile.MajorVersion >= 52)
            {
                // invokestatic and invokespecial may be used to invoke interface methods in Java 8
                // but invokespecial can only invoke methods in the current interface or a directly implemented interface
                if (invoke == NormalizedOpCode._invokespecial && cpi is ClassFile.ConstantPoolItemInterfaceMethodref)
                {
                    if (cpi.GetClassType() == host)
                    {
                        // ok
                    }
                    else if (cpi.GetClassType() != wrapper && Array.IndexOf(wrapper.Interfaces, cpi.GetClassType()) == -1)
                    {
                        throw new VerifyError("Bad invokespecial instruction: interface method reference is in an indirect superinterface.");
                    }
                }
            }
            else if ((cpi is ClassFile.ConstantPoolItemInterfaceMethodref) != (invoke == NormalizedOpCode._invokeinterface))
            {
                throw new VerifyError("Illegal constant pool index");
            }
            if (invoke != NormalizedOpCode._invokespecial && ReferenceEquals(cpi.Name, StringConstants.INIT))
            {
                throw new VerifyError("Must call initializers using invokespecial");
            }
            if (ReferenceEquals(cpi.Name, StringConstants.CLINIT))
            {
                throw new VerifyError("Illegal call to internal method");
            }
            RuntimeJavaType[] args = cpi.GetArgTypes();
            for (int j = args.Length - 1; j >= 0; j--)
            {
                stack.PopType(args[j]);
            }
            if (invoke == NormalizedOpCode._invokeinterface)
            {
                int argcount = args.Length + 1;
                for (int j = 0; j < args.Length; j++)
                {
                    if (args[j].IsWidePrimitive)
                    {
                        argcount++;
                    }
                }
                if (method.Instructions[index].Arg2 != argcount)
                {
                    throw new VerifyError("Inconsistent args size");
                }
            }
            bool isnew = false;
            RuntimeJavaType thisType;
            if (invoke == NormalizedOpCode._invokestatic)
            {
                thisType = null;
            }
            else
            {
                thisType = SigTypeToClassName(stack.PeekType(), cpi.GetClassType(), wrapper);
                if (ReferenceEquals(cpi.Name, StringConstants.INIT))
                {
                    RuntimeJavaType type = stack.PopType();
                    isnew = RuntimeVerifierJavaType.IsNew(type);
                    if ((isnew && ((RuntimeVerifierJavaType)type).UnderlyingType != cpi.GetClassType()) ||
                        (type == context.VerifierJavaTypeFactory.UninitializedThis && cpi.GetClassType() != wrapper.BaseTypeWrapper && cpi.GetClassType() != wrapper) ||
                        (!isnew && type != context.VerifierJavaTypeFactory.UninitializedThis))
                    {
                        // TODO oddly enough, Java fails verification for the class without
                        // even running the constructor, so maybe constructors are always
                        // verified...
                        // NOTE when a constructor isn't verifiable, the static initializer
                        // doesn't run either
                        throw new VerifyError("Call to wrong initialization method");
                    }
                }
                else
                {
                    if (invoke != NormalizedOpCode._invokeinterface)
                    {
                        RuntimeJavaType refType = stack.PopObjectType();
                        RuntimeJavaType targetType = cpi.GetClassType();
                        if (!RuntimeVerifierJavaType.IsNullOrUnloadable(refType) &&
                            !targetType.IsUnloadable &&
                            !refType.IsAssignableTo(targetType))
                        {
                            throw new VerifyError("Incompatible object argument for function call");
                        }
                        // for invokespecial we also need to make sure we're calling ourself or a base class
                        if (invoke == NormalizedOpCode._invokespecial)
                        {
                            if (RuntimeVerifierJavaType.IsNullOrUnloadable(refType))
                            {
                                // ok
                            }
                            else if (refType.IsSubTypeOf(wrapper))
                            {
                                // ok
                            }
                            else if (host != null && refType.IsSubTypeOf(host))
                            {
                                // ok
                            }
                            else
                            {
                                throw new VerifyError("Incompatible target object for invokespecial");
                            }
                            if (targetType.IsUnloadable)
                            {
                                // ok
                            }
                            else if (wrapper.IsSubTypeOf(targetType))
                            {
                                // ok
                            }
                            else if (host != null && host.IsSubTypeOf(targetType))
                            {
                                // ok
                            }
                            else
                            {
                                throw new VerifyError("Invokespecial cannot call subclass methods");
                            }
                        }
                    }
                    else /* __invokeinterface */
                    {
                        // NOTE unlike in the above case, we also allow *any* interface target type
                        // regardless of whether it is compatible or not, because if it is not compatible
                        // we want an IncompatibleClassChangeError at runtime
                        RuntimeJavaType refType = stack.PopObjectType();
                        RuntimeJavaType targetType = cpi.GetClassType();
                        if (!RuntimeVerifierJavaType.IsNullOrUnloadable(refType)
                            && !targetType.IsUnloadable
                            && !refType.IsAssignableTo(targetType)
                            && !targetType.IsInterface)
                        {
                            throw new VerifyError("Incompatible object argument for function call");
                        }
                    }
                }
            }
        }

        private void VerifyInvokeDynamic(int index)
        {
            StackState stack = new StackState(state[index]);
            ClassFile.ConstantPoolItemInvokeDynamic cpi = VerifyGetInvokeDynamic(new((ushort)method.Instructions[index].Arg1));
            RuntimeJavaType[] args = cpi.GetArgTypes();
            for (int j = args.Length - 1; j >= 0; j--)
            {
                stack.PopType(args[j]);
            }
        }

        private static void OptimizationPass(CodeInfo codeInfo, ClassFile classFile, ClassFile.Method method, UntangledExceptionTable exceptions, RuntimeJavaType wrapper, RuntimeClassLoader classLoader)
        {
            // Optimization pass
            if (classLoader.RemoveAsserts)
            {
                // While the optimization is general, in practice it never happens that a getstatic is used on a final field,
                // so we only look for this if assert initialization has been optimized out.
                if (classFile.HasAssertions)
                {
                    // compute branch targets
                    InstructionFlags[] flags = MethodAnalyzer.ComputePartialReachability(codeInfo, method.Instructions, exceptions, 0, false);
                    ClassFile.Method.Instruction[] instructions = method.Instructions;
                    for (int i = 0; i < instructions.Length; i++)
                    {
                        if (instructions[i].NormalizedOpCode == NormalizedOpCode._getstatic
                            && instructions[i + 1].NormalizedOpCode == NormalizedOpCode._ifne
                            && instructions[i + 1].TargetIndex > i
                            && (flags[i + 1] & InstructionFlags.BranchTarget) == 0)
                        {
                            var field = classFile.GetFieldref(instructions[i].Arg1).GetField() as RuntimeConstantJavaField;
                            if (field != null && field.FieldTypeWrapper == classLoader.Context.PrimitiveJavaTypeFactory.BOOLEAN && (bool)field.GetConstantValue())
                            {
                                // We know the branch will always be taken, so we replace the getstatic/ifne by a goto.
                                instructions[i].PatchOpCode(NormalizedOpCode._goto, instructions[i + 1].TargetIndex);
                            }
                        }
                    }
                }
            }
        }

        private void PatchHardErrorsAndDynamicMemberAccess(RuntimeJavaType wrapper, RuntimeJavaMethod mw)
        {
            // Now we do another pass to find "hard error" instructions
            if (true)
            {
                ClassFile.Method.Instruction[] instructions = method.Instructions;
                for (int i = 0; i < instructions.Length; i++)
                {
                    if (state[i] != null)
                    {
                        StackState stack = new StackState(state[i]);
                        switch (instructions[i].NormalizedOpCode)
                        {
                            case NormalizedOpCode._invokeinterface:
                            case NormalizedOpCode._invokespecial:
                            case NormalizedOpCode._invokestatic:
                            case NormalizedOpCode._invokevirtual:
                                PatchInvoke(wrapper, ref instructions[i], stack);
                                break;
                            case NormalizedOpCode._getfield:
                            case NormalizedOpCode._putfield:
                            case NormalizedOpCode._getstatic:
                            case NormalizedOpCode._putstatic:
                                PatchFieldAccess(wrapper, mw, ref instructions[i], stack);
                                break;
                            case NormalizedOpCode._ldc:
                                switch (classFile.GetConstantPoolConstantType(new((ushort)instructions[i].Arg1)))
                                {
                                    case ClassFile.ConstantType.Class:
                                        {
                                            RuntimeJavaType tw = classFile.GetConstantPoolClassType(new((ushort)instructions[i].Arg1));
                                            if (tw.IsUnloadable)
                                            {
                                                ConditionalPatchNoClassDefFoundError(ref instructions[i], tw);
                                            }
                                            break;
                                        }
                                    case ClassFile.ConstantType.MethodType:
                                        {
                                            ClassFile.ConstantPoolItemMethodType cpi = classFile.GetConstantPoolConstantMethodType(new((ushort)instructions[i].Arg1));
                                            RuntimeJavaType[] args = cpi.GetArgTypes();
                                            RuntimeJavaType tw = cpi.GetRetType();
                                            for (int j = 0; !tw.IsUnloadable && j < args.Length; j++)
                                            {
                                                tw = args[j];
                                            }
                                            if (tw.IsUnloadable)
                                            {
                                                ConditionalPatchNoClassDefFoundError(ref instructions[i], tw);
                                            }
                                            break;
                                        }
                                    case ClassFile.ConstantType.MethodHandle:
                                        PatchLdcMethodHandle(ref instructions[i]);
                                        break;
                                }
                                break;
                            case NormalizedOpCode._new:
                                {
                                    RuntimeJavaType tw = classFile.GetConstantPoolClassType(new((ushort)instructions[i].Arg1));
                                    if (tw.IsUnloadable)
                                    {
                                        ConditionalPatchNoClassDefFoundError(ref instructions[i], tw);
                                    }
                                    else if (!tw.IsAccessibleFrom(wrapper))
                                    {
                                        SetHardError(wrapper.GetClassLoader(), ref instructions[i], HardError.IllegalAccessError, "Try to access class {0} from class {1}", tw.Name, wrapper.Name);
                                    }
                                    else if (tw.IsAbstract)
                                    {
                                        SetHardError(wrapper.GetClassLoader(), ref instructions[i], HardError.InstantiationError, "{0}", tw.Name);
                                    }
                                    break;
                                }
                            case NormalizedOpCode._multianewarray:
                            case NormalizedOpCode._anewarray:
                                {
                                    RuntimeJavaType tw = classFile.GetConstantPoolClassType(new((ushort)instructions[i].Arg1));
                                    if (tw.IsUnloadable)
                                    {
                                        ConditionalPatchNoClassDefFoundError(ref instructions[i], tw);
                                    }
                                    else if (!tw.IsAccessibleFrom(wrapper))
                                    {
                                        SetHardError(wrapper.GetClassLoader(), ref instructions[i], HardError.IllegalAccessError, "Try to access class {0} from class {1}", tw.Name, wrapper.Name);
                                    }
                                    break;
                                }
                            case NormalizedOpCode._checkcast:
                            case NormalizedOpCode._instanceof:
                                {
                                    RuntimeJavaType tw = classFile.GetConstantPoolClassType(new((ushort)instructions[i].Arg1));
                                    if (tw.IsUnloadable)
                                    {
                                        // If the type is unloadable, we always generate the dynamic code
                                        // (regardless of ClassLoaderWrapper.DisableDynamicBinding), because at runtime,
                                        // null references should always pass thru without attempting
                                        // to load the type (for Sun compatibility).
                                    }
                                    else if (!tw.IsAccessibleFrom(wrapper))
                                    {
                                        SetHardError(wrapper.GetClassLoader(), ref instructions[i], HardError.IllegalAccessError, "Try to access class {0} from class {1}", tw.Name, wrapper.Name);
                                    }
                                    break;
                                }
                            case NormalizedOpCode._aaload:
                                {
                                    stack.PopInt();
                                    RuntimeJavaType tw = stack.PopArrayType();
                                    if (tw.IsUnloadable)
                                    {
                                        ConditionalPatchNoClassDefFoundError(ref instructions[i], tw);
                                    }
                                    break;
                                }
                            case NormalizedOpCode._aastore:
                                {
                                    stack.PopObjectType();
                                    stack.PopInt();
                                    RuntimeJavaType tw = stack.PopArrayType();
                                    if (tw.IsUnloadable)
                                    {
                                        ConditionalPatchNoClassDefFoundError(ref instructions[i], tw);
                                    }
                                    break;
                                }
                            default:
                                break;
                        }
                    }
                }
            }
        }

        private void PatchLdcMethodHandle(ref ClassFile.Method.Instruction instr)
        {
            ClassFile.ConstantPoolItemMethodHandle cpi = classFile.GetConstantPoolConstantMethodHandle(new((ushort)instr.Arg1));
            if (cpi.GetClassType().IsUnloadable)
            {
                ConditionalPatchNoClassDefFoundError(ref instr, cpi.GetClassType());
            }
            else if (!cpi.GetClassType().IsAccessibleFrom(wrapper))
            {
                SetHardError(wrapper.GetClassLoader(), ref instr, HardError.IllegalAccessError, "tried to access class {0} from class {1}", cpi.Class, wrapper.Name);
            }
            else if (cpi.Kind == ReferenceKind.InvokeVirtual
                && cpi.GetClassType() == context.JavaBase.TypeOfJavaLangInvokeMethodHandle
                && (cpi.Name == "invoke" || cpi.Name == "invokeExact"))
            {
                // it's allowed to use ldc to create a MethodHandle invoker
            }
            else if (cpi.Member == null || cpi.Member.IsStatic != (cpi.Kind == ReferenceKind.GetStatic || cpi.Kind == ReferenceKind.PutStatic || cpi.Kind == ReferenceKind.InvokeStatic))
            {
                HardError err;
                string msg;
                switch (cpi.Kind)
                {
                    case ReferenceKind.GetField:
                    case ReferenceKind.GetStatic:
                    case ReferenceKind.PutField:
                    case ReferenceKind.PutStatic:
                        err = HardError.NoSuchFieldError;
                        msg = cpi.Name;
                        break;
                    default:
                        err = HardError.NoSuchMethodError;
                        msg = cpi.Class + "." + cpi.Name + cpi.Signature;
                        break;
                }
                SetHardError(wrapper.GetClassLoader(), ref instr, err, msg, cpi.Class, cpi.Name, SigToString(cpi.Signature));
            }
            else if (!cpi.Member.IsAccessibleFrom(cpi.GetClassType(), wrapper, cpi.GetClassType()))
            {
                if (cpi.Member.IsProtected && wrapper.IsSubTypeOf(cpi.Member.DeclaringType))
                {
                    // this is allowed, the receiver will be narrowed to current type
                }
                else
                {
                    SetHardError(wrapper.GetClassLoader(), ref instr, HardError.IllegalAccessException, "member is private: {0}.{1}/{2}/{3}, from {4}", cpi.Class, cpi.Name, SigToString(cpi.Signature), cpi.Kind, wrapper.Name);
                }
            }
        }

        private static string SigToString(string sig)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string sep = "";
            int dims = 0;
            for (int i = 0; i < sig.Length; i++)
            {
                if (sig[i] == '(' || sig[i] == ')')
                {
                    sb.Append(sig[i]);
                    sep = "";
                    continue;
                }
                else if (sig[i] == '[')
                {
                    dims++;
                    continue;
                }
                sb.Append(sep);
                sep = ",";
                switch (sig[i])
                {
                    case 'V':
                        sb.Append("void");
                        break;
                    case 'B':
                        sb.Append("byte");
                        break;
                    case 'Z':
                        sb.Append("boolean");
                        break;
                    case 'S':
                        sb.Append("short");
                        break;
                    case 'C':
                        sb.Append("char");
                        break;
                    case 'I':
                        sb.Append("int");
                        break;
                    case 'J':
                        sb.Append("long");
                        break;
                    case 'F':
                        sb.Append("float");
                        break;
                    case 'D':
                        sb.Append("double");
                        break;
                    case 'L':
                        sb.Append(sig, i + 1, sig.IndexOf(';', i + 1) - (i + 1));
                        i = sig.IndexOf(';', i + 1);
                        break;
                }
                for (; dims != 0; dims--)
                {
                    sb.Append("[]");
                }
            }
            return sb.ToString();
        }

        internal static InstructionFlags[] ComputePartialReachability(CodeInfo codeInfo, ClassFile.Method.Instruction[] instructions, UntangledExceptionTable exceptions, int initialInstructionIndex, bool skipFaultBlocks)
        {
            InstructionFlags[] flags = new InstructionFlags[instructions.Length];
            flags[initialInstructionIndex] |= InstructionFlags.Reachable;
            UpdatePartialReachability(flags, codeInfo, instructions, exceptions, skipFaultBlocks);
            return flags;
        }

        private static void UpdatePartialReachability(InstructionFlags[] flags, CodeInfo codeInfo, ClassFile.Method.Instruction[] instructions, UntangledExceptionTable exceptions, bool skipFaultBlocks)
        {
            bool done = false;
            while (!done)
            {
                done = true;
                for (int i = 0; i < instructions.Length; i++)
                {
                    if ((flags[i] & (InstructionFlags.Reachable | InstructionFlags.Processed)) == InstructionFlags.Reachable)
                    {
                        done = false;
                        flags[i] |= InstructionFlags.Processed;
                        // mark the exception handlers reachable from this instruction
                        for (int j = 0; j < exceptions.Length; j++)
                        {
                            if (exceptions[j].StartIndex <= i && i < exceptions[j].EndIndex)
                            {
                                int idx = exceptions[j].HandlerIndex;
                                if (!skipFaultBlocks || !RuntimeVerifierJavaType.IsFaultBlockException(codeInfo.GetRawStackTypeWrapper(idx, 0)))
                                {
                                    flags[idx] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
                                }
                            }
                        }
                        MarkSuccessors(instructions, flags, i);
                    }
                }
            }
        }

        private static void MarkSuccessors(ClassFile.Method.Instruction[] code, InstructionFlags[] flags, int index)
        {
            switch (OpCodeMetaData.GetFlowControl(code[index].NormalizedOpCode))
            {
                case ByteCodeFlowControl.Switch:
                    {
                        for (int i = 0; i < code[index].SwitchEntryCount; i++)
                        {
                            flags[code[index].GetSwitchTargetIndex(i)] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
                        }
                        flags[code[index].DefaultTarget] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
                        break;
                    }
                case ByteCodeFlowControl.Branch:
                    flags[code[index].TargetIndex] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
                    break;
                case ByteCodeFlowControl.CondBranch:
                    flags[code[index].TargetIndex] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
                    flags[index + 1] |= InstructionFlags.Reachable;
                    break;
                case ByteCodeFlowControl.Return:
                case ByteCodeFlowControl.Throw:
                    break;
                case ByteCodeFlowControl.Next:
                    flags[index + 1] |= InstructionFlags.Reachable;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        internal static UntangledExceptionTable UntangleExceptionBlocks(RuntimeContext context, ClassFile classFile, ClassFile.Method method)
        {
            ClassFile.Method.Instruction[] instructions = method.Instructions;
            ExceptionTableEntry[] exceptionTable = method.ExceptionTable;
            List<ExceptionTableEntry> ar = new List<ExceptionTableEntry>(exceptionTable);

            // This optimization removes the recursive exception handlers that Java compiler place around
            // the exit of a synchronization block to be "safe" in the face of asynchronous exceptions.
            // (see http://weblog.ikvm.net/PermaLink.aspx?guid=3af9548e-4905-4557-8809-65a205ce2cd6)
            // We can safely remove them since the code we generate for this construct isn't async safe anyway,
            // but there is another reason why this optimization may be slightly controversial. In some
            // pathological cases it can cause observable differences, where the Sun JVM would spin in an
            // infinite loop, but we will throw an exception. However, the perf benefit is large enough to
            // warrant this "incompatibility".
            // Note that there is also code in the exception handler handling code that detects these bytecode
            // sequences to try to compile them into a fault block, instead of an exception handler.
            for (int i = 0; i < ar.Count; i++)
            {
                ExceptionTableEntry ei = ar[i];
                if (ei.StartIndex == ei.HandlerIndex && ei.CatchType.IsNil)
                {
                    int index = ei.StartIndex;
                    if (index + 2 < instructions.Length
                        && ei.EndIndex == index + 2
                        && instructions[index].NormalizedOpCode == NormalizedOpCode._aload
                        && instructions[index + 1].NormalizedOpCode == NormalizedOpCode._monitorexit
                        && instructions[index + 2].NormalizedOpCode == NormalizedOpCode._athrow)
                    {
                        // this is the async exception guard that Jikes and the Eclipse Java Compiler produce
                        ar.RemoveAt(i);
                        i--;
                    }
                    else if (index + 4 < instructions.Length
                        && ei.EndIndex == index + 3
                        && instructions[index].NormalizedOpCode == NormalizedOpCode._astore
                        && instructions[index + 1].NormalizedOpCode == NormalizedOpCode._aload
                        && instructions[index + 2].NormalizedOpCode == NormalizedOpCode._monitorexit
                        && instructions[index + 3].NormalizedOpCode == NormalizedOpCode._aload
                        && instructions[index + 4].NormalizedOpCode == NormalizedOpCode._athrow
                        && instructions[index].NormalizedArg1 == instructions[index + 3].NormalizedArg1)
                    {
                        // this is the async exception guard that javac produces
                        ar.RemoveAt(i);
                        i--;
                    }
                    else if (index + 1 < instructions.Length
                        && ei.EndIndex == index + 1
                        && instructions[index].NormalizedOpCode == NormalizedOpCode._astore)
                    {
                        // this is the finally guard that javac produces
                        ar.RemoveAt(i);
                        i--;
                    }
                }
            }

            // Modern versions of javac split try blocks when the try block contains a return statement.
            // Here we merge these exception blocks again, because it allows us to generate more efficient code.
            for (int i = 0; i < ar.Count - 1; i++)
            {
                if (ar[i].EndIndex + 1 == ar[i + 1].StartIndex
                    && ar[i].HandlerIndex == ar[i + 1].HandlerIndex
                    && ar[i].CatchType == ar[i + 1].CatchType
                    && IsReturn(instructions[ar[i].EndIndex].NormalizedOpCode))
                {
                    ar[i] = new ExceptionTableEntry(ar[i].StartIndex, ar[i + 1].EndIndex, ar[i].HandlerIndex, ar[i].CatchType, ar[i].Ordinal);
                    ar.RemoveAt(i + 1);
                    i--;
                }
            }

        restart:
            for (int i = 0; i < ar.Count; i++)
            {
                ExceptionTableEntry ei = ar[i];
                for (int j = 0; j < ar.Count; j++)
                {
                    ExceptionTableEntry ej = ar[j];
                    if (ei.StartIndex <= ej.StartIndex && ej.StartIndex < ei.EndIndex)
                    {
                        // 0006/test.j
                        if (ej.EndIndex > ei.EndIndex)
                        {
                            ExceptionTableEntry emi = new ExceptionTableEntry(ej.StartIndex, ei.EndIndex, ei.HandlerIndex, ei.CatchType, ei.Ordinal);
                            ExceptionTableEntry emj = new ExceptionTableEntry(ej.StartIndex, ei.EndIndex, ej.HandlerIndex, ej.CatchType, ej.Ordinal);
                            ei = new ExceptionTableEntry(ei.StartIndex, emi.StartIndex, ei.HandlerIndex, ei.CatchType, ei.Ordinal);
                            ej = new ExceptionTableEntry(emj.EndIndex, ej.EndIndex, ej.HandlerIndex, ej.CatchType, ej.Ordinal);
                            ar[i] = ei;
                            ar[j] = ej;
                            ar.Insert(j, emj);
                            ar.Insert(i + 1, emi);
                            goto restart;
                        }
                        // 0007/test.j
                        else if (j > i && ej.EndIndex < ei.EndIndex)
                        {
                            ExceptionTableEntry emi = new ExceptionTableEntry(ej.StartIndex, ej.EndIndex, ei.HandlerIndex, ei.CatchType, ei.Ordinal);
                            ExceptionTableEntry eei = new ExceptionTableEntry(ej.EndIndex, ei.EndIndex, ei.HandlerIndex, ei.CatchType, ei.Ordinal);
                            ei = new ExceptionTableEntry(ei.StartIndex, emi.StartIndex, ei.HandlerIndex, ei.CatchType, ei.Ordinal);
                            ar[i] = ei;
                            ar.Insert(i + 1, eei);
                            ar.Insert(i + 1, emi);
                            goto restart;
                        }
                    }
                }
            }
        // Split try blocks at branch targets (branches from outside the try block)
        restart_split:
            for (int i = 0; i < ar.Count; i++)
            {
                ExceptionTableEntry ei = ar[i];
                int start = ei.StartIndex;
                int end = ei.EndIndex;
                for (int j = 0; j < instructions.Length; j++)
                {
                    if (j < start || j >= end)
                    {
                        switch (instructions[j].NormalizedOpCode)
                        {
                            case NormalizedOpCode._tableswitch:
                            case NormalizedOpCode._lookupswitch:
                                // start at -1 to have an opportunity to handle the default offset
                                for (int k = -1; k < instructions[j].SwitchEntryCount; k++)
                                {
                                    int targetIndex = (k == -1 ? instructions[j].DefaultTarget : instructions[j].GetSwitchTargetIndex(k));
                                    if (ei.StartIndex < targetIndex && targetIndex < ei.EndIndex)
                                    {
                                        ExceptionTableEntry en = new ExceptionTableEntry(targetIndex, ei.EndIndex, ei.HandlerIndex, ei.CatchType, ei.Ordinal);
                                        ei = new ExceptionTableEntry(ei.StartIndex, targetIndex, ei.HandlerIndex, ei.CatchType, ei.Ordinal);
                                        ar[i] = ei;
                                        ar.Insert(i + 1, en);
                                        goto restart_split;
                                    }
                                }
                                break;
                            case NormalizedOpCode._ifeq:
                            case NormalizedOpCode._ifne:
                            case NormalizedOpCode._iflt:
                            case NormalizedOpCode._ifge:
                            case NormalizedOpCode._ifgt:
                            case NormalizedOpCode._ifle:
                            case NormalizedOpCode._if_icmpeq:
                            case NormalizedOpCode._if_icmpne:
                            case NormalizedOpCode._if_icmplt:
                            case NormalizedOpCode._if_icmpge:
                            case NormalizedOpCode._if_icmpgt:
                            case NormalizedOpCode._if_icmple:
                            case NormalizedOpCode._if_acmpeq:
                            case NormalizedOpCode._if_acmpne:
                            case NormalizedOpCode._ifnull:
                            case NormalizedOpCode._ifnonnull:
                            case NormalizedOpCode._goto:
                                {
                                    int targetIndex = instructions[j].Arg1;
                                    if (ei.StartIndex < targetIndex && targetIndex < ei.EndIndex)
                                    {
                                        ExceptionTableEntry en = new ExceptionTableEntry(targetIndex, ei.EndIndex, ei.HandlerIndex, ei.CatchType, ei.Ordinal);
                                        ei = new ExceptionTableEntry(ei.StartIndex, targetIndex, ei.HandlerIndex, ei.CatchType, ei.Ordinal);
                                        ar[i] = ei;
                                        ar.Insert(i + 1, en);
                                        goto restart_split;
                                    }
                                    break;
                                }
                        }
                    }
                }
            }
            // exception handlers are also a kind of jump, so we need to split try blocks around handlers as well
            for (int i = 0; i < ar.Count; i++)
            {
                ExceptionTableEntry ei = ar[i];
                for (int j = 0; j < ar.Count; j++)
                {
                    ExceptionTableEntry ej = ar[j];
                    if (ei.StartIndex < ej.HandlerIndex && ej.HandlerIndex < ei.EndIndex)
                    {
                        ExceptionTableEntry en = new ExceptionTableEntry(ej.HandlerIndex, ei.EndIndex, ei.HandlerIndex, ei.CatchType, ei.Ordinal);
                        ei = new ExceptionTableEntry(ei.StartIndex, ej.HandlerIndex, ei.HandlerIndex, ei.CatchType, ei.Ordinal);
                        ar[i] = ei;
                        ar.Insert(i + 1, en);
                        goto restart_split;
                    }
                }
            }
            // filter out zero length try blocks
            for (int i = 0; i < ar.Count; i++)
            {
                var ei = ar[i];
                if (ei.StartIndex == ei.EndIndex)
                {
                    ar.RemoveAt(i);
                    i--;
                }
                else
                {
                    // exception blocks that only contain harmless instructions (i.e. instructions that will *never* throw an exception)
                    // are also filtered out (to improve the quality of the generated code)
                    var exceptionType = ei.CatchType.IsNil ? context.JavaBase.TypeOfjavaLangThrowable : classFile.GetConstantPoolClassType(ei.CatchType);
                    if (exceptionType.IsUnloadable)
                    {
                        // we can't remove handlers for unloadable types
                    }
                    else if (context.MethodAnalyzerFactory.javaLangThreadDeath.IsAssignableTo(exceptionType))
                    {
                        // We only remove exception handlers that could catch ThreadDeath in limited cases, because it can be thrown
                        // asynchronously (and thus appear on any instruction). This is particularly important to ensure that
                        // we run finally blocks when a thread is killed.
                        // Note that even so, we aren't remotely async exception safe.
                        int start = ei.StartIndex;
                        int end = ei.EndIndex;
                        for (int j = start; j < end; j++)
                        {
                            switch (instructions[j].NormalizedOpCode)
                            {
                                case NormalizedOpCode._aload:
                                case NormalizedOpCode._iload:
                                case NormalizedOpCode._lload:
                                case NormalizedOpCode._fload:
                                case NormalizedOpCode._dload:
                                case NormalizedOpCode._astore:
                                case NormalizedOpCode._istore:
                                case NormalizedOpCode._lstore:
                                case NormalizedOpCode._fstore:
                                case NormalizedOpCode._dstore:
                                    break;
                                case NormalizedOpCode._dup:
                                case NormalizedOpCode._dup_x1:
                                case NormalizedOpCode._dup_x2:
                                case NormalizedOpCode._dup2:
                                case NormalizedOpCode._dup2_x1:
                                case NormalizedOpCode._dup2_x2:
                                case NormalizedOpCode._pop:
                                case NormalizedOpCode._pop2:
                                    break;
                                case NormalizedOpCode._return:
                                case NormalizedOpCode._areturn:
                                case NormalizedOpCode._ireturn:
                                case NormalizedOpCode._lreturn:
                                case NormalizedOpCode._freturn:
                                case NormalizedOpCode._dreturn:
                                    break;
                                case NormalizedOpCode._goto:
                                    // if there is a branch that stays inside the block, we should keep the block
                                    if (start <= instructions[j].TargetIndex && instructions[j].TargetIndex < end)
                                        goto next;
                                    break;
                                default:
                                    goto next;
                            }
                        }
                        ar.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        int start = ei.StartIndex;
                        int end = ei.EndIndex;
                        for (int j = start; j < end; j++)
                        {
                            if (OpCodeMetaData.CanThrowException(instructions[j].NormalizedOpCode))
                            {
                                goto next;
                            }
                        }
                        ar.RemoveAt(i);
                        i--;
                    }
                }
            next:;
            }

            ExceptionTableEntry[] exceptions = ar.ToArray();
            Array.Sort(exceptions, new ExceptionSorter());

            return new UntangledExceptionTable(exceptions);
        }

        private static bool IsReturn(NormalizedOpCode bc)
        {
            return bc == NormalizedOpCode._return
                || bc == NormalizedOpCode._areturn
                || bc == NormalizedOpCode._dreturn
                || bc == NormalizedOpCode._ireturn
                || bc == NormalizedOpCode._freturn
                || bc == NormalizedOpCode._lreturn;
        }

        static bool AnalyzePotentialFaultBlocks(CodeInfo codeInfo, ClassFile.Method method, UntangledExceptionTable exceptions)
        {
            var code = method.Instructions;
            var changed = false;

            var done = false;
            while (done == false)
            {
                done = true;

                var stack = new Stack<ExceptionTableEntry>();
                var current = new ExceptionTableEntry(0, code.Length, -1, new(ushort.MaxValue), -1);
                stack.Push(current);

                for (int i = 0; i < exceptions.Length; i++)
                {
                    while (exceptions[i].StartIndex >= current.EndIndex)
                        current = stack.Pop();

                    Debug.Assert(exceptions[i].StartIndex >= current.StartIndex && exceptions[i].EndIndex <= current.EndIndex);

                    if (exceptions[i].CatchType.IsNil && codeInfo.HasState(exceptions[i].HandlerIndex) && RuntimeVerifierJavaType.IsFaultBlockException(codeInfo.GetRawStackTypeWrapper(exceptions[i].HandlerIndex, 0)))
                    {
                        var flags = MethodAnalyzer.ComputePartialReachability(codeInfo, method.Instructions, exceptions, exceptions[i].HandlerIndex, true);
                        for (int j = 0; j < code.Length; j++)
                        {
                            if ((flags[j] & InstructionFlags.Reachable) != 0)
                            {
                                switch (code[j].NormalizedOpCode)
                                {
                                    case NormalizedOpCode._return:
                                    case NormalizedOpCode._areturn:
                                    case NormalizedOpCode._ireturn:
                                    case NormalizedOpCode._lreturn:
                                    case NormalizedOpCode._freturn:
                                    case NormalizedOpCode._dreturn:
                                        goto not_fault_block;

                                    case NormalizedOpCode._athrow:
                                        for (int k = i + 1; k < exceptions.Length; k++)
                                        {
                                            if (exceptions[k].StartIndex <= j && j < exceptions[k].EndIndex)
                                            {
                                                goto not_fault_block;
                                            }
                                        }

                                        if (RuntimeVerifierJavaType.IsFaultBlockException(codeInfo.GetRawStackTypeWrapper(j, 0)) && codeInfo.GetRawStackTypeWrapper(j, 0) != codeInfo.GetRawStackTypeWrapper(exceptions[i].HandlerIndex, 0))
                                        {
                                            goto not_fault_block;
                                        }

                                        break;
                                }
                                if (j < current.StartIndex || j >= current.EndIndex)
                                {
                                    goto not_fault_block;
                                }
                                else if (exceptions[i].StartIndex <= j && j < exceptions[i].EndIndex)
                                {
                                    goto not_fault_block;
                                }
                                else
                                {
                                    continue;
                                }
                            not_fault_block:
                                RuntimeVerifierJavaType.ClearFaultBlockException(codeInfo.GetRawStackTypeWrapper(exceptions[i].HandlerIndex, 0));
                                done = false;
                                changed = true;
                                break;
                            }
                        }
                    }

                    stack.Push(current);
                    current = exceptions[i];
                }
            }

            return changed;
        }

        private static void ConvertFinallyBlocks(CodeInfo codeInfo, ClassFile.Method method, UntangledExceptionTable exceptions)
        {
            ClassFile.Method.Instruction[] code = method.Instructions;
            InstructionFlags[] flags = ComputePartialReachability(codeInfo, code, exceptions, 0, false);
            for (int i = 0; i < exceptions.Length; i++)
            {
                if (exceptions[i].CatchType.IsNil && codeInfo.HasState(exceptions[i].HandlerIndex) && RuntimeVerifierJavaType.IsFaultBlockException(codeInfo.GetRawStackTypeWrapper(exceptions[i].HandlerIndex, 0)))
                {
                    if (IsSynchronizedBlockHandler(code, exceptions[i].HandlerIndex)
                        && exceptions[i].EndIndex - 2 >= exceptions[i].StartIndex
                        && TryFindSingleTryBlockExit(code, flags, exceptions, new ExceptionTableEntry(exceptions[i].StartIndex, exceptions[i].EndIndex - 2, exceptions[i].HandlerIndex, new(0), exceptions[i].Ordinal), i, out var exit)
                        && exit == exceptions[i].EndIndex - 2
                        && (flags[exit + 1] & InstructionFlags.BranchTarget) == 0
                        && MatchInstructions(code, exit, exceptions[i].HandlerIndex + 1)
                        && MatchInstructions(code, exit + 1, exceptions[i].HandlerIndex + 2)
                        && MatchExceptionCoverage(exceptions, i, exceptions[i].HandlerIndex + 1, exceptions[i].HandlerIndex + 3, exit, exit + 2)
                        && exceptions[i].HandlerIndex <= ushort.MaxValue)
                    {
                        code[exit].PatchOpCode(NormalizedOpCode.__goto_finally, exceptions[i].EndIndex, (short)exceptions[i].HandlerIndex);
                        exceptions.SetFinally(i);
                    }
                    else if (TryFindSingleTryBlockExit(code, flags, exceptions, exceptions[i], i, out exit)
                        // the stack must be empty
                        && codeInfo.GetStackHeight(exit) == 0
                        // the exit code must not be reachable (except from within the try-block),
                        // because we're going to patch it to jump around the exit code
                        && !IsReachableFromOutsideTryBlock(codeInfo, code, exceptions, exceptions[i], exit))
                    {
                        int exitHandlerEnd;
                        int faultHandlerEnd;
                        if (MatchFinallyBlock(codeInfo, code, exceptions, exceptions[i].HandlerIndex, exit, out exitHandlerEnd, out faultHandlerEnd))
                        {
                            if (exit != exitHandlerEnd
                                && codeInfo.GetStackHeight(exitHandlerEnd) == 0
                                && MatchExceptionCoverage(exceptions, -1, exceptions[i].HandlerIndex, faultHandlerEnd, exit, exitHandlerEnd))
                            {
                                // We use Arg2 (which is a short) to store the handler in the __goto_finally pseudo-opcode,
                                // so we can only do that if handlerIndex fits in a short (note that we can use the sign bit too).
                                if (exceptions[i].HandlerIndex <= ushort.MaxValue)
                                {
                                    code[exit].PatchOpCode(NormalizedOpCode.__goto_finally, exitHandlerEnd, (short)exceptions[i].HandlerIndex);
                                    exceptions.SetFinally(i);
                                }
                            }
                        }
                    }
                }
            }
        }

        private static bool IsSynchronizedBlockHandler(ClassFile.Method.Instruction[] code, int index)
        {
            return code[index].NormalizedOpCode == NormalizedOpCode._astore
                && code[index + 1].NormalizedOpCode == NormalizedOpCode._aload
                && code[index + 2].NormalizedOpCode == NormalizedOpCode._monitorexit
                && code[index + 3].NormalizedOpCode == NormalizedOpCode._aload && code[index + 3].Arg1 == code[index].Arg1
                && code[index + 4].NormalizedOpCode == NormalizedOpCode._athrow;
        }

        private static bool MatchExceptionCoverage(UntangledExceptionTable exceptions, int skipException, int startFault, int endFault, int startExit, int endExit)
        {
            for (int j = 0; j < exceptions.Length; j++)
            {
                if (j != skipException && ExceptionCovers(exceptions[j], startFault, endFault) != ExceptionCovers(exceptions[j], startExit, endExit))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool ExceptionCovers(ExceptionTableEntry exception, int start, int end)
        {
            return exception.StartIndex < end && exception.EndIndex > start;
        }

        private static bool MatchFinallyBlock(CodeInfo codeInfo, ClassFile.Method.Instruction[] code, UntangledExceptionTable exceptions, int faultHandler, int exitHandler, out int exitHandlerEnd, out int faultHandlerEnd)
        {
            exitHandlerEnd = -1;
            faultHandlerEnd = -1;
            if (code[faultHandler].NormalizedOpCode != NormalizedOpCode._astore)
            {
                return false;
            }
            int startFault = faultHandler;
            int faultLocal = code[faultHandler++].NormalizedArg1;
            for (; ; )
            {
                if (code[faultHandler].NormalizedOpCode == NormalizedOpCode._aload
                    && code[faultHandler].NormalizedArg1 == faultLocal
                    && code[faultHandler + 1].NormalizedOpCode == NormalizedOpCode._athrow)
                {
                    // make sure that instructions that we haven't covered aren't reachable
                    InstructionFlags[] flags = ComputePartialReachability(codeInfo, code, exceptions, startFault, false);
                    for (int i = 0; i < flags.Length; i++)
                    {
                        if ((i < startFault || i > faultHandler + 1) && (flags[i] & InstructionFlags.Reachable) != 0)
                        {
                            return false;
                        }
                    }
                    exitHandlerEnd = exitHandler;
                    faultHandlerEnd = faultHandler;
                    return true;
                }
                if (!MatchInstructions(code, faultHandler, exitHandler))
                {
                    return false;
                }
                faultHandler++;
                exitHandler++;
            }
        }

        private static bool MatchInstructions(ClassFile.Method.Instruction[] code, int i, int j)
        {
            if (code[i].NormalizedOpCode != code[j].NormalizedOpCode)
            {
                return false;
            }
            switch (OpCodeMetaData.GetFlowControl(code[i].NormalizedOpCode))
            {
                case ByteCodeFlowControl.Branch:
                case ByteCodeFlowControl.CondBranch:
                    if (code[i].Arg1 - i != code[j].Arg1 - j)
                    {
                        return false;
                    }
                    break;
                case ByteCodeFlowControl.Switch:
                    if (code[i].SwitchEntryCount != code[j].SwitchEntryCount)
                    {
                        return false;
                    }
                    for (int k = 0; k < code[i].SwitchEntryCount; k++)
                    {
                        if (code[i].GetSwitchTargetIndex(k) != code[j].GetSwitchTargetIndex(k))
                        {
                            return false;
                        }
                    }
                    if (code[i].DefaultTarget != code[j].DefaultTarget)
                    {
                        return false;
                    }
                    break;
                default:
                    if (code[i].Arg1 != code[j].Arg1)
                    {
                        return false;
                    }
                    if (code[i].Arg2 != code[j].Arg2)
                    {
                        return false;
                    }
                    break;
            }
            return true;
        }

        private static bool IsReachableFromOutsideTryBlock(CodeInfo codeInfo, ClassFile.Method.Instruction[] code, UntangledExceptionTable exceptions, ExceptionTableEntry tryBlock, int instructionIndex)
        {
            InstructionFlags[] flags = new InstructionFlags[code.Length];
            flags[0] |= InstructionFlags.Reachable;
            // We mark the first instruction of the try-block as already processed, so that UpdatePartialReachability will skip the try-block.
            // Note that we can do this, because it is not possible to jump into the middle of a try-block (after the exceptions have been untangled).
            flags[tryBlock.StartIndex] = InstructionFlags.Processed;
            // We mark the successor instructions of the instruction we're examinining as reachable,
            // to figure out if the code following the handler somehow branches back to it.
            MarkSuccessors(code, flags, instructionIndex);
            UpdatePartialReachability(flags, codeInfo, code, exceptions, false);
            return (flags[instructionIndex] & InstructionFlags.Reachable) != 0;
        }

        private static bool TryFindSingleTryBlockExit(ClassFile.Method.Instruction[] code, InstructionFlags[] flags, UntangledExceptionTable exceptions, ExceptionTableEntry exception, int exceptionIndex, out int exit)
        {
            exit = -1;
            bool fail = false;
            bool nextIsReachable = false;
            for (int i = exception.StartIndex; !fail && i < exception.EndIndex; i++)
            {
                if ((flags[i] & InstructionFlags.Reachable) != 0)
                {
                    nextIsReachable = false;
                    for (int j = 0; j < exceptions.Length; j++)
                    {
                        if (j != exceptionIndex && exceptions[j].StartIndex >= exception.StartIndex && exception.EndIndex <= exceptions[j].EndIndex)
                        {
                            UpdateTryBlockExit(exception, exceptions[j].HandlerIndex, ref exit, ref fail);
                        }
                    }
                    switch (OpCodeMetaData.GetFlowControl(code[i].NormalizedOpCode))
                    {
                        case ByteCodeFlowControl.Switch:
                            {
                                for (int j = 0; j < code[i].SwitchEntryCount; j++)
                                {
                                    UpdateTryBlockExit(exception, code[i].GetSwitchTargetIndex(j), ref exit, ref fail);
                                }
                                UpdateTryBlockExit(exception, code[i].DefaultTarget, ref exit, ref fail);
                                break;
                            }
                        case ByteCodeFlowControl.Branch:
                            UpdateTryBlockExit(exception, code[i].TargetIndex, ref exit, ref fail);
                            break;
                        case ByteCodeFlowControl.CondBranch:
                            UpdateTryBlockExit(exception, code[i].TargetIndex, ref exit, ref fail);
                            nextIsReachable = true;
                            break;
                        case ByteCodeFlowControl.Return:
                            fail = true;
                            break;
                        case ByteCodeFlowControl.Throw:
                            break;
                        case ByteCodeFlowControl.Next:
                            nextIsReachable = true;
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                }
            }
            if (nextIsReachable)
            {
                UpdateTryBlockExit(exception, exception.EndIndex, ref exit, ref fail);
            }
            return !fail && exit != -1;
        }

        private static void UpdateTryBlockExit(ExceptionTableEntry exception, int targetIndex, ref int exitIndex, ref bool fail)
        {
            if (exception.StartIndex <= targetIndex && targetIndex < exception.EndIndex)
            {
                // branch stays inside try block
            }
            else if (exitIndex == -1)
            {
                exitIndex = targetIndex;
            }
            else if (exitIndex != targetIndex)
            {
                fail = true;
            }
        }

        private void ConditionalPatchNoClassDefFoundError(ref ClassFile.Method.Instruction instruction, RuntimeJavaType tw)
        {
            RuntimeClassLoader loader = wrapper.GetClassLoader();
            if (loader.DisableDynamicBinding)
            {
                SetHardError(loader, ref instruction, HardError.NoClassDefFoundError, "{0}", tw.Name);
            }
        }

        private void SetHardError(RuntimeClassLoader classLoader, ref ClassFile.Method.Instruction instruction, HardError hardError, string message, params object[] args)
        {
            string text = string.Format(message, args);
#if IMPORTER
            Message msg;
            switch (hardError)
            {
                case HardError.NoClassDefFoundError:
                    msg = Message.EmittedNoClassDefFoundError;
                    break;
                case HardError.IllegalAccessError:
                    msg = Message.EmittedIllegalAccessError;
                    break;
                case HardError.InstantiationError:
                    msg = Message.EmittedIllegalAccessError;
                    break;
                case HardError.IncompatibleClassChangeError:
                case HardError.IllegalAccessException:
                    msg = Message.EmittedIncompatibleClassChangeError;
                    break;
                case HardError.NoSuchFieldError:
                    msg = Message.EmittedNoSuchFieldError;
                    break;
                case HardError.AbstractMethodError:
                    msg = Message.EmittedAbstractMethodError;
                    break;
                case HardError.NoSuchMethodError:
                    msg = Message.EmittedNoSuchMethodError;
                    break;
                case HardError.LinkageError:
                    msg = Message.EmittedLinkageError;
                    break;
                default:
                    throw new InvalidOperationException();
            }
            classLoader.IssueMessage(msg, classFile.Name + "." + method.Name + method.Signature, text);
#endif
            instruction.SetHardError(hardError, AllocErrorMessage(text));
        }

        void PatchInvoke(RuntimeJavaType wrapper, ref ClassFile.Method.Instruction instruction, StackState stack)
        {
            var cpi = VerifyGetMethodref(instruction.Arg1);
            var invoke = instruction.NormalizedOpCode;

            var isnew = false;
            RuntimeJavaType thisType;

            if (invoke == NormalizedOpCode._invokevirtual &&
                cpi is { Class: "java.lang.invoke.MethodHandle", Name: "invoke" or "invokeExact" or "invokeBasic" })
            {
                if (cpi.GetArgTypes().Length > 127 && context.MethodHandleUtil.SlotCount(cpi.GetArgTypes()) > 254)
                {
                    instruction.SetHardError(HardError.LinkageError, AllocErrorMessage("bad parameter count"));
                    return;
                }

                instruction.PatchOpCode(NormalizedOpCode.__methodhandle_invoke);
                return;
            }

            if (invoke == NormalizedOpCode._invokestatic &&
                cpi is { Class: "java.lang.invoke.MethodHandle", Name: "linkToVirtual" or "linkToStatic" or "linkToSpecial" or "linkToInterface" } &&
                context.JavaBase.TypeOfJavaLangInvokeMethodHandle.IsPackageAccessibleFrom(wrapper))
            {
                instruction.PatchOpCode(NormalizedOpCode.__methodhandle_link);
                return;
            }

            if (invoke == NormalizedOpCode._invokestatic)
            {
                thisType = null;
            }
            else
            {
                var args = cpi.GetArgTypes();
                for (int j = args.Length - 1; j >= 0; j--)
                    stack.PopType(args[j]);

                thisType = SigTypeToClassName(stack.PeekType(), cpi.GetClassType(), wrapper);
                if (ReferenceEquals(cpi.Name, StringConstants.INIT))
                {
                    var type = stack.PopType();
                    isnew = RuntimeVerifierJavaType.IsNew(type);
                }
            }

            if (cpi.GetClassType().IsUnloadable)
            {
                if (wrapper.GetClassLoader().DisableDynamicBinding)
                {
                    SetHardError(wrapper.GetClassLoader(), ref instruction, HardError.NoClassDefFoundError, "{0}", cpi.GetClassType().Name);
                }
                else
                {
                    switch (invoke)
                    {
                        case NormalizedOpCode._invokeinterface:
                            instruction.PatchOpCode(NormalizedOpCode.__dynamic_invokeinterface);
                            break;
                        case NormalizedOpCode._invokestatic:
                            instruction.PatchOpCode(NormalizedOpCode.__dynamic_invokestatic);
                            break;
                        case NormalizedOpCode._invokevirtual:
                            instruction.PatchOpCode(NormalizedOpCode.__dynamic_invokevirtual);
                            break;
                        case NormalizedOpCode._invokespecial:
                            if (isnew)
                                instruction.PatchOpCode(NormalizedOpCode.__dynamic_invokespecial);
                            else
                                throw new VerifyError("Invokespecial cannot call subclass methods");
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                }
            }
            else if (invoke == NormalizedOpCode._invokeinterface && !cpi.GetClassType().IsInterface)
            {
                SetHardError(wrapper.GetClassLoader(), ref instruction, HardError.IncompatibleClassChangeError, "invokeinterface on non-interface");
            }
            else if (cpi.GetClassType().IsInterface && invoke != NormalizedOpCode._invokeinterface && ((invoke != NormalizedOpCode._invokestatic && invoke != NormalizedOpCode._invokespecial) || classFile.MajorVersion < 52))
            {
                SetHardError(wrapper.GetClassLoader(), ref instruction, HardError.IncompatibleClassChangeError,
                    classFile.MajorVersion < 52
                        ? "interface method must be invoked using invokeinterface"
                        : "interface method must be invoked using invokeinterface, invokespecial or invokestatic");
            }
            else
            {
                var targetMethod = invoke == NormalizedOpCode._invokespecial ? cpi.GetMethodForInvokespecial() : cpi.GetMethod();
                if (targetMethod != null)
                {
                    var errmsg = CheckLoaderConstraints(cpi, targetMethod);
                    if (errmsg != null)
                    {
                        SetHardError(wrapper.GetClassLoader(), ref instruction, HardError.LinkageError, "{0}", errmsg);
                    }
                    else if (targetMethod.IsStatic == (invoke == NormalizedOpCode._invokestatic))
                    {
                        if (targetMethod.IsAbstract && invoke == NormalizedOpCode._invokespecial && (targetMethod.GetMethod() == null || targetMethod.GetMethod().IsAbstract))
                        {
                            SetHardError(wrapper.GetClassLoader(), ref instruction, HardError.AbstractMethodError, "{0}.{1}{2}", cpi.Class, cpi.Name, cpi.Signature);
                        }
                        else if (invoke == NormalizedOpCode._invokeinterface && targetMethod.IsPrivate)
                        {
                            SetHardError(wrapper.GetClassLoader(), ref instruction, HardError.IncompatibleClassChangeError, "private interface method requires invokespecial, not invokeinterface: method {0}.{1}{2}", cpi.Class, cpi.Name, cpi.Signature);
                        }
                        else if (targetMethod.IsAccessibleFrom(cpi.GetClassType(), wrapper, thisType))
                        {
                            return;
                        }
                        else if (host != null && targetMethod.IsAccessibleFrom(cpi.GetClassType(), host, thisType))
                        {
                            switch (invoke)
                            {
                                case NormalizedOpCode._invokespecial:
                                    instruction.PatchOpCode(NormalizedOpCode.__privileged_invokespecial);
                                    break;
                                case NormalizedOpCode._invokestatic:
                                    instruction.PatchOpCode(NormalizedOpCode.__privileged_invokestatic);
                                    break;
                                case NormalizedOpCode._invokevirtual:
                                    instruction.PatchOpCode(NormalizedOpCode.__privileged_invokevirtual);
                                    break;
                                default:
                                    throw new InvalidOperationException();
                            }

                            return;
                        }
                        else
                        {
                            // NOTE special case for incorrect invocation of Object.clone(), because this could mean
                            // we're calling clone() on an array
                            // (bug in javac, see http://developer.java.sun.com/developer/bugParade/bugs/4329886.html)
                            if (cpi.GetClassType() == context.JavaBase.TypeOfJavaLangObject &&
                                thisType.IsArray &&
                                ReferenceEquals(cpi.Name, StringConstants.CLONE))
                            {
                                // Patch the instruction, so that the compiler doesn't need to do this test again.
                                instruction.PatchOpCode(NormalizedOpCode.__clone_array);
                                return;
                            }

                            SetHardError(wrapper.GetClassLoader(), ref instruction, HardError.IllegalAccessError, "tried to access method {0}.{1}{2} from class {3}", ToSlash(targetMethod.DeclaringType.Name), cpi.Name, ToSlash(cpi.Signature), ToSlash(wrapper.Name));
                        }
                    }
                    else
                    {
                        SetHardError(wrapper.GetClassLoader(), ref instruction, HardError.IncompatibleClassChangeError, "static call to non-static method (or v.v.)");
                    }
                }
                else
                {
                    SetHardError(wrapper.GetClassLoader(), ref instruction, HardError.NoSuchMethodError, "{0}.{1}{2}", cpi.Class, cpi.Name, cpi.Signature);
                }
            }
        }

        static string ToSlash(string str)
        {
            return str.Replace('.', '/');
        }

        void PatchFieldAccess(RuntimeJavaType wrapper, RuntimeJavaMethod mw, ref ClassFile.Method.Instruction instr, StackState stack)
        {
            var fieldRef = classFile.GetFieldref(instr.Arg1);

            bool isStatic;
            bool write;
            RuntimeJavaType thisType;

            switch (instr.NormalizedOpCode)
            {
                case NormalizedOpCode._getfield:
                    isStatic = false;
                    write = false;
                    thisType = SigTypeToClassName(stack.PopObjectType(VerifyGetFieldref(instr.Arg1).GetClassType()), fieldRef.GetClassType(), wrapper);
                    break;
                case NormalizedOpCode._putfield:
                    stack.PopType(VerifyGetFieldref(instr.Arg1).GetFieldType());
                    isStatic = false;
                    write = true;
                    // putfield is allowed to access the unintialized this
                    if (stack.PeekType() == context.VerifierJavaTypeFactory.UninitializedThis
                        && wrapper.IsAssignableTo(VerifyGetFieldref(instr.Arg1).GetClassType()))
                    {
                        thisType = wrapper;
                    }
                    else
                    {
                        thisType = SigTypeToClassName(stack.PopObjectType(VerifyGetFieldref(instr.Arg1).GetClassType()), fieldRef.GetClassType(), wrapper);
                    }
                    break;
                case NormalizedOpCode._getstatic:
                    isStatic = true;
                    write = false;
                    thisType = null;
                    break;
                case NormalizedOpCode._putstatic:
                    // special support for when we're being called from IsSideEffectFreeStaticInitializer
                    if (mw == null)
                    {
                        switch (VerifyGetFieldref(instr.Arg1).Signature[0])
                        {
                            case 'B':
                            case 'Z':
                            case 'C':
                            case 'S':
                            case 'I':
                                stack.PopInt();
                                break;
                            case 'F':
                                stack.PopFloat();
                                break;
                            case 'D':
                                stack.PopDouble();
                                break;
                            case 'J':
                                stack.PopLong();
                                break;
                            case 'L':
                            case '[':
                                if (stack.PopAnyType() != context.VerifierJavaTypeFactory.Null)
                                {
                                    throw new VerifyError();
                                }
                                break;
                            default:
                                throw new InvalidOperationException();
                        }
                    }
                    else
                    {
                        stack.PopType(VerifyGetFieldref(instr.Arg1).GetFieldType());
                    }
                    isStatic = true;
                    write = true;
                    thisType = null;
                    break;
                default:
                    throw new InvalidOperationException();
            }
            if (mw == null)
            {
                // We're being called from IsSideEffectFreeStaticInitializer,
                // no further checks are possible (nor needed).
            }
            else if (fieldRef.GetClassType().IsUnloadable)
            {
                if (wrapper.GetClassLoader().DisableDynamicBinding)
                {
                    SetHardError(wrapper.GetClassLoader(), ref instr, HardError.NoClassDefFoundError, "{0}", fieldRef.GetClassType().Name);
                }
                else
                {
                    switch (instr.NormalizedOpCode)
                    {
                        case NormalizedOpCode._getstatic:
                            instr.PatchOpCode(NormalizedOpCode.__dynamic_getstatic);
                            break;
                        case NormalizedOpCode._putstatic:
                            instr.PatchOpCode(NormalizedOpCode.__dynamic_putstatic);
                            break;
                        case NormalizedOpCode._getfield:
                            instr.PatchOpCode(NormalizedOpCode.__dynamic_getfield);
                            break;
                        case NormalizedOpCode._putfield:
                            instr.PatchOpCode(NormalizedOpCode.__dynamic_putfield);
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                }
                return;
            }
            else
            {
                var field = fieldRef.GetField();
                if (field == null)
                {
                    SetHardError(wrapper.GetClassLoader(), ref instr, HardError.NoSuchFieldError, "{0}.{1}", fieldRef.Class, fieldRef.Name);
                    return;
                }

                if (false && fieldRef.GetFieldType() != field.FieldTypeWrapper && !fieldRef.GetFieldType().IsUnloadable & !field.FieldTypeWrapper.IsUnloadable)
                {
#if IMPORTER
                    StaticCompiler.LinkageError("Field \"{2}.{3}\" is of type \"{0}\" instead of type \"{1}\" as expected by \"{4}\"", field.FieldTypeWrapper, fieldRef.GetFieldType(), fieldRef.GetClassType().Name, fieldRef.Name, wrapper.Name);
#endif
                    SetHardError(wrapper.GetClassLoader(), ref instr, HardError.LinkageError, "Loader constraints violated: {0}.{1}", field.DeclaringType.Name, field.Name);
                    return;
                }

                if (field.IsStatic != isStatic)
                {
                    SetHardError(wrapper.GetClassLoader(), ref instr, HardError.IncompatibleClassChangeError, "Static field access to non-static field (or v.v.)");
                    return;
                }

                if (!field.IsAccessibleFrom(fieldRef.GetClassType(), wrapper, thisType))
                {
                    SetHardError(wrapper.GetClassLoader(), ref instr, HardError.IllegalAccessError, "Try to access field {0}.{1} from class {2}", field.DeclaringType.Name, field.Name, wrapper.Name);
                    return;
                }

                // are we trying to mutate a final field? (they are read-only from outside of the defining class)
                if (write && field.IsFinal && ((isStatic ? wrapper != fieldRef.GetClassType() : wrapper != thisType) || (wrapper.GetClassLoader().StrictFinalFieldSemantics && (isStatic ? (mw != null && mw.Name != "<clinit>") : (mw == null || mw.Name != "<init>")))))
                {
                    SetHardError(wrapper.GetClassLoader(), ref instr, HardError.IllegalAccessError, "Field {0}.{1} is final", field.DeclaringType.Name, field.Name);
                    return;
                }
            }
        }

        // TODO this method should have a better name
        RuntimeJavaType SigTypeToClassName(RuntimeJavaType type, RuntimeJavaType nullType, RuntimeJavaType wrapper)
        {
            if (type == context.VerifierJavaTypeFactory.UninitializedThis)
            {
                return wrapper;
            }
            else if (RuntimeVerifierJavaType.IsNew(type))
            {
                return ((RuntimeVerifierJavaType)type).UnderlyingType;
            }
            else if (type == context.VerifierJavaTypeFactory.Null)
            {
                return nullType;
            }
            else
            {
                return type;
            }
        }

        int AllocErrorMessage(string message)
        {
            errorMessages ??= new List<string>();
            var index = errorMessages.Count;
            errorMessages.Add(message);
            return index;
        }

        string CheckLoaderConstraints(ClassFile.ConstantPoolItemMI cpi, RuntimeJavaMethod mw)
        {
#if NETFRAMEWORK
            if (cpi.GetRetType() != mw.ReturnType && !cpi.GetRetType().IsUnloadable && !mw.ReturnType.IsUnloadable)
#else
            if (cpi.GetRetType() != mw.ReturnType && cpi.GetRetType().Name != mw.ReturnType.Name && !cpi.GetRetType().IsUnloadable && !mw.ReturnType.IsUnloadable)
#endif
            {
#if IMPORTER
                StaticCompiler.LinkageError("Method \"{2}.{3}{4}\" has a return type \"{0}\" instead of type \"{1}\" as expected by \"{5}\"", mw.ReturnType, cpi.GetRetType(), cpi.GetClassType().Name, cpi.Name, cpi.Signature, classFile.Name);
#endif
                return "Loader constraints violated (return type): " + mw.DeclaringType.Name + "." + mw.Name + mw.Signature;
            }

            var here = cpi.GetArgTypes();
            var there = mw.GetParameters();

            for (int i = 0; i < here.Length; i++)
            {
#if NETFRAMEWORK
                if (here[i] != there[i] && !here[i].IsUnloadable && !there[i].IsUnloadable)
#else
                if (here[i] != there[i] && here[i].Name != there[i].Name && !here[i].IsUnloadable && !there[i].IsUnloadable)
#endif
                {
#if IMPORTER
                    StaticCompiler.LinkageError("Method \"{2}.{3}{4}\" has a argument type \"{0}\" instead of type \"{1}\" as expected by \"{5}\"", there[i], here[i], cpi.GetClassType().Name, cpi.Name, cpi.Signature, classFile.Name);
#endif
                    return "Loader constraints violated (arg " + i + "): " + mw.DeclaringType.Name + "." + mw.Name + mw.Signature;
                }
            }

            return null;
        }

        /// <summary>
        /// Attempts to get an InvokeDynamic constant, or throws a <see cref="VerifyError"/>.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        /// <exception cref="VerifyError"></exception>
        ClassFile.ConstantPoolItemInvokeDynamic VerifyGetInvokeDynamic(InvokeDynamicConstantHandle handle)
        {
            try
            {
                var item = classFile.GetInvokeDynamic(handle);
                if (item != null)
                    return item;
            }
            catch (InvalidCastException)
            {

            }
            catch (IndexOutOfRangeException)
            {

            }

            throw new VerifyError("Illegal constant pool index");
        }

        /// <summary>
        /// Attempts to get an Methodref constant, or throws a <see cref="VerifyError"/>.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="VerifyError"></exception>
        ClassFile.ConstantPoolItemMI VerifyGetMethodref(int index)
        {
            try
            {
                var item = classFile.GetMethodref(index);
                if (item != null)
                    return item;
            }
            catch (InvalidCastException)
            {

            }
            catch (IndexOutOfRangeException)
            {

            }

            throw new VerifyError("Illegal constant pool index");
        }

        /// <summary>
        /// Attempts to get an Fieldref constant, or throws a <see cref="VerifyError"/>.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="VerifyError"></exception>
        ClassFile.ConstantPoolItemFieldref VerifyGetFieldref(int index)
        {
            try
            {
                var item = classFile.GetFieldref(index);
                if (item != null)
                    return item;
            }
            catch (InvalidCastException)
            {

            }
            catch (IndexOutOfRangeException)
            {

            }

            throw new VerifyError("Illegal constant pool index");
        }

        /// <summary>
        /// Attempts to get the type of a constant, or throws a <see cref="VerifyError"/>.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        /// <exception cref="VerifyError"></exception>
        ClassFile.ConstantType VerifyGetConstantPoolConstantType(ConstantHandle handle)
        {
            try
            {
                return classFile.GetConstantPoolConstantType(handle);
            }
            catch (IndexOutOfRangeException)
            {
                // constant pool index out of range
            }
            catch (InvalidOperationException)
            {
                // specified constant pool entry doesn't contain a constant
            }
            catch (NullReferenceException)
            {
                // specified constant pool entry is empty (entry 0 or the filler following a wide entry)
            }

            throw new VerifyError("Illegal constant pool index");
        }

        /// <summary>
        /// Attempts to get an Class constant's Java type, or throws a <see cref="VerifyError"/>.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        /// <exception cref="VerifyError"></exception>
        RuntimeJavaType VerifyGetConstantPoolClassType(ClassConstantHandle handle)
        {
            try
            {
                return classFile.GetConstantPoolClassType(handle);
            }
            catch (InvalidCastException)
            {

            }
            catch (IndexOutOfRangeException)
            {

            }
            catch (NullReferenceException)
            {

            }

            throw new VerifyError("Illegal constant pool index");
        }

        internal void ClearFaultBlockException(int instructionIndex)
        {
            Debug.Assert(state[instructionIndex].GetStackHeight() == 1);
            state[instructionIndex].ClearFaultBlockException();
        }

        static void DumpMethod(CodeInfo codeInfo, ClassFile.Method method, UntangledExceptionTable exceptions)
        {
            var code = method.Instructions;
            var flags = ComputePartialReachability(codeInfo, code, exceptions, 0, false);

            for (int i = 0; i < code.Length; i++)
            {
                var label = (flags[i] & InstructionFlags.BranchTarget) != 0;
                if (!label)
                {
                    for (int j = 0; j < exceptions.Length; j++)
                    {
                        if (exceptions[j].StartIndex == i || exceptions[j].EndIndex == i || exceptions[j].HandlerIndex == i)
                        {
                            label = true;
                            break;
                        }
                    }
                }

                if (label)
                    Console.WriteLine("label{0}:", i);

                if ((flags[i] & InstructionFlags.Reachable) != 0)
                {
                    Console.Write("  {1}", i, code[i].NormalizedOpCode.ToString().Substring(2));

                    switch (OpCodeMetaData.GetFlowControl(code[i].NormalizedOpCode))
                    {
                        case ByteCodeFlowControl.Branch:
                        case ByteCodeFlowControl.CondBranch:
                            Console.Write(" label{0}", code[i].Arg1);
                            break;
                    }

                    switch (code[i].NormalizedOpCode)
                    {
                        case NormalizedOpCode._iload:
                        case NormalizedOpCode._lload:
                        case NormalizedOpCode._fload:
                        case NormalizedOpCode._dload:
                        case NormalizedOpCode._aload:
                        case NormalizedOpCode._istore:
                        case NormalizedOpCode._lstore:
                        case NormalizedOpCode._fstore:
                        case NormalizedOpCode._dstore:
                        case NormalizedOpCode._astore:
                        case NormalizedOpCode.__iconst:
                            Console.Write(" {0}", code[i].Arg1);
                            break;
                        case NormalizedOpCode._ldc:
                        case NormalizedOpCode.__ldc_nothrow:
                        case NormalizedOpCode._getfield:
                        case NormalizedOpCode._getstatic:
                        case NormalizedOpCode._putfield:
                        case NormalizedOpCode._putstatic:
                        case NormalizedOpCode._invokeinterface:
                        case NormalizedOpCode._invokespecial:
                        case NormalizedOpCode._invokestatic:
                        case NormalizedOpCode._invokevirtual:
                        case NormalizedOpCode._new:
                            Console.Write(" #{0}", code[i].Arg1);
                            break;
                    }
                    Console.WriteLine();
                }
            }

            for (int i = 0; i < exceptions.Length; i++)
                Console.WriteLine(".catch #{0} from label{1} to label{2} using label{3}", exceptions[i].CatchType, exceptions[i].StartIndex, exceptions[i].EndIndex, exceptions[i].HandlerIndex);
        }

    }

}
