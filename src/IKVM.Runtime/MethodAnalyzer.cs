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
using System.Text;

using IKVM.ByteCode;
using IKVM.CoreLib.Linking;
using IKVM.CoreLib.Runtime;

#if IMPORTER
using IKVM.Tools.Importer;
#endif

namespace IKVM.Runtime
{

    sealed class MethodAnalyzer
    {

        readonly RuntimeContext _context;
        readonly RuntimeJavaType _host;  // used to by Unsafe.defineAnonymousClass() to provide access to private members of the host
        readonly RuntimeJavaType _type;
        readonly RuntimeJavaMethod _method;
        readonly ClassFile _classFile;
        readonly Method _classFileMethod;
        readonly RuntimeClassLoader _classLoader;
        readonly VerificationType _thisType;
        readonly InstructionState[] _state;
        List<string> _errorMessages;
        readonly Dictionary<int, VerificationType> _newTypes = new Dictionary<int, VerificationType>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        MethodAnalyzer(RuntimeContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Gets the <see cref="RuntimeContext"/> that hosts this method analyzer.
        /// </summary>
        public RuntimeContext Context => _context;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="host"></param>
        /// <param name="type"></param>
        /// <param name="method"></param>
        /// <param name="classFile"></param>
        /// <param name="classFileMethod"></param>
        /// <param name="classLoader"></param>
        /// <exception cref="VerifyError"></exception>
        /// <exception cref="ClassFormatError"></exception>
        internal MethodAnalyzer(RuntimeContext context, RuntimeJavaType host, RuntimeJavaType type, RuntimeJavaMethod method, ClassFile classFile, Method classFileMethod, RuntimeClassLoader classLoader) :
            this(context)
        {
            if (classFileMethod.VerifyError != null)
                throw new VerifyError(classFileMethod.VerifyError);

            _host = host;
            _type = type;
            _method = method;
            _classFile = classFile;
            _classFileMethod = classFileMethod;
            _classLoader = classLoader;
            _state = new InstructionState[classFileMethod.Instructions.Length];

            try
            {
                // ensure that exception blocks and handlers start and end at instruction boundaries
                for (int i = 0; i < classFileMethod.ExceptionTable.Length; i++)
                {
                    int start = classFileMethod.ExceptionTable[i].StartIndex;
                    int end = classFileMethod.ExceptionTable[i].EndIndex;
                    int handler = classFileMethod.ExceptionTable[i].HandlerIndex;
                    if (start >= end || start == -1 || end == -1 || handler <= 0)
                        throw new IndexOutOfRangeException();
                }
            }
            catch (IndexOutOfRangeException)
            {
                // TODO figure out if we should throw this during class loading
                throw new ClassFormatError($"Illegal exception table (class: {classFile.Name}, method: {classFileMethod.Name}, signature: {classFileMethod.Signature}");
            }

            // start by computing the initial state, the stack is empty and the locals contain the arguments
            _state[0] = new InstructionState(context, _state, classFileMethod.MaxLocals, classFileMethod.MaxStack);
            int firstNonArgLocalIndex = 0;

            if (classFileMethod.IsStatic == false)
            {
                _thisType = VerificationType.This(type);

                // this reference. If we're a constructor, the this reference is uninitialized.
                if (classFileMethod.IsConstructor)
                {
                    _state[0].SetLocalType(firstNonArgLocalIndex++, VerificationType.UninitializedThis, -1);
                    _state[0].SetUnitializedThis(true);
                }
                else
                {
                    _state[0].SetLocalType(firstNonArgLocalIndex++, _thisType, -1);
                }
            }
            else
            {
                _thisType = default;
            }

            // mw can be null when we're invoked from IsSideEffectFreeStaticInitializer
            var argTypes = method != null ? method.GetParameters() : [];
            for (int i = 0; i < argTypes.Length; i++)
            {
                var argType = argTypes[i];
                if (argType.IsIntOnStackPrimitive)
                    argType = context.PrimitiveJavaTypeFactory.INT;

                _state[0].SetLocalType(firstNonArgLocalIndex++, argType.ToVerificationType(), -1);
                if (argType.IsWidePrimitive)
                    firstNonArgLocalIndex++;
            }

            AnalyzeTypeFlow();
            VerifyPassTwo();
            PatchLoadConstants();
        }

        /// <summary>
        /// Replaces 'ldc' instructions with 'ldc_nothrow' pseudo instructions.
        /// </summary>
        void PatchLoadConstants()
        {
            var code = _classFileMethod.Instructions;
            for (int i = 0; i < code.Length; i++)
            {
                if (_state[i]._initialized)
                {
                    switch (code[i].NormalizedOpCode)
                    {
                        case NormalizedOpCode.Ldc:
                            switch (GetConstantPoolConstantType(code[i].Arg1))
                            {
                                case ConstantType.Double:
                                case ConstantType.Float:
                                case ConstantType.Integer:
                                case ConstantType.Long:
                                case ConstantType.String:
                                case ConstantType.LiveObject:
                                    code[i].PatchOpCode(NormalizedOpCode.LdcNothrow);
                                    break;
                            }

                            break;
                    }
                }
            }
        }

        internal CodeInfo GetCodeInfoAndErrors(UntangledExceptionTable exceptions, out List<string> errors)
        {
            var codeInfo = new CodeInfo(_context, _state);

            OptimizationPass(codeInfo, _classFile, _classFileMethod, exceptions, _type, _classLoader);
            PatchHardErrorsAndDynamicMemberAccess(_type, _method);
            errors = _errorMessages;

            if (AnalyzePotentialFaultBlocks(codeInfo, _classFileMethod, exceptions))
                AnalyzeTypeFlow();

            ConvertFinallyBlocks(codeInfo, _classFileMethod, exceptions);
            return codeInfo;
        }

        void AnalyzeTypeFlow()
        {
            var s = new InstructionState(_context, _state, _classFileMethod.MaxLocals, _classFileMethod.MaxStack);
            var done = false;
            var instructions = _classFileMethod.Instructions;

            while (done == false)
            {
                done = true;

                for (int i = 0; i < instructions.Length; i++)
                {
                    if (_state[i]._initialized && _state[i]._changed)
                    {
                        try
                        {
                            // we encountered a state that is marked as changed, so we will need a next loop
                            done = false;
                            _state[i]._changed = false;

                            // mark the exception handlers reachable from this instruction
                            for (int j = 0; j < _classFileMethod.ExceptionTable.Length; j++)
                                if (_classFileMethod.ExceptionTable[j].StartIndex <= i && i < _classFileMethod.ExceptionTable[j].EndIndex)
                                    MergeExceptionHandler(j, ref _state[i]);

                            // copy current frame to this frame
                            _state[i].CopyTo(ref s);

                            var inst = instructions[i];
                            switch (inst.NormalizedOpCode)
                            {
                                case NormalizedOpCode.Aload:
                                    {
                                        var type = s.GetLocalType(inst.NormalizedArg1);
                                        if (type.IsTop || type.IsPrimitive)
                                            throw new VerifyError("Object reference expected");

                                        s.PushType(type);
                                        break;
                                    }
                                case NormalizedOpCode.Astore:
                                    {
                                        if (s.PeekType().IsFaultBlockException)
                                        {
                                            s.SetLocalType(inst.NormalizedArg1, s.PopFaultBlockException(), i);
                                            break;
                                        }

                                        // NOTE since the reference can be uninitialized, we cannot use PopObjectType
                                        var type = s.PopType();
                                        if (type.IsPrimitive)
                                            throw new VerifyError("Object reference expected");

                                        s.SetLocalType(inst.NormalizedArg1, type, i);
                                        break;
                                    }
                                case NormalizedOpCode.AconstNull:
                                    s.PushType(VerificationType.Null);
                                    break;
                                case NormalizedOpCode.Aaload:
                                    {
                                        s.PopInt();
                                        var type = s.PopArrayType();
                                        if (type.IsNull)
                                        {
                                            // if the array is null, we have use null as the element type, because
                                            // otherwise the rest of the code will not verify correctly
                                            s.PushType(VerificationType.Null);
                                        }
                                        else if (type.IsUnloadable())
                                        {
                                            s.PushType(_context.VerifierJavaTypeFactory.Unloadable);
                                        }
                                        else
                                        {
                                            var elementType = type.Type.ElementTypeWrapper;
                                            if (elementType.IsPrimitive)
                                                throw new VerifyError("Object array expected");

                                            s.PushType(elementType);
                                        }
                                        break;
                                    }
                                case NormalizedOpCode.Aastore:
                                    s.PopObjectType();
                                    s.PopInt();
                                    s.PopArrayType();
                                    // TODO check that elem is assignable to the array
                                    break;
                                case NormalizedOpCode.Baload:
                                    {
                                        s.PopInt();

                                        var type = s.PopArrayType();
                                        if (!type.IsNullOrUnloadable() && type.Type != _context.MethodAnalyzerFactory.ByteArrayType && type.Type != _context.MethodAnalyzerFactory.BooleanArrayType)
                                            throw new VerifyError();

                                        s.PushInt();
                                        break;
                                    }
                                case NormalizedOpCode.Bastore:
                                    {
                                        s.PopInt();
                                        s.PopInt();

                                        var type = s.PopArrayType();
                                        if (!type.IsNullOrUnloadable() && type.Type != _context.MethodAnalyzerFactory.ByteArrayType && type.Type != _context.MethodAnalyzerFactory.BooleanArrayType)
                                            throw new VerifyError();

                                        break;
                                    }
                                case NormalizedOpCode.Caload:
                                    s.PopInt();
                                    s.PopObjectType(_context.MethodAnalyzerFactory.CharArrayType);
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode.Castore:
                                    s.PopInt();
                                    s.PopInt();
                                    s.PopObjectType(_context.MethodAnalyzerFactory.CharArrayType);
                                    break;
                                case NormalizedOpCode.Saload:
                                    s.PopInt();
                                    s.PopObjectType(_context.MethodAnalyzerFactory.ShortArrayType);
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode.Sastore:
                                    s.PopInt();
                                    s.PopInt();
                                    s.PopObjectType(_context.MethodAnalyzerFactory.ShortArrayType);
                                    break;
                                case NormalizedOpCode.Iaload:
                                    s.PopInt();
                                    s.PopObjectType(_context.MethodAnalyzerFactory.IntArrayType);
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode.Iastore:
                                    s.PopInt();
                                    s.PopInt();
                                    s.PopObjectType(_context.MethodAnalyzerFactory.IntArrayType);
                                    break;
                                case NormalizedOpCode.Laload:
                                    s.PopInt();
                                    s.PopObjectType(_context.MethodAnalyzerFactory.LongArrayType);
                                    s.PushLong();
                                    break;
                                case NormalizedOpCode.Lastore:
                                    s.PopLong();
                                    s.PopInt();
                                    s.PopObjectType(_context.MethodAnalyzerFactory.LongArrayType);
                                    break;
                                case NormalizedOpCode.Daload:
                                    s.PopInt();
                                    s.PopObjectType(_context.MethodAnalyzerFactory.DoubleArrayType);
                                    s.PushDouble();
                                    break;
                                case NormalizedOpCode.Dastore:
                                    s.PopDouble();
                                    s.PopInt();
                                    s.PopObjectType(_context.MethodAnalyzerFactory.DoubleArrayType);
                                    break;
                                case NormalizedOpCode.Faload:
                                    s.PopInt();
                                    s.PopObjectType(_context.MethodAnalyzerFactory.FloatArrayType);
                                    s.PushFloat();
                                    break;
                                case NormalizedOpCode.Fastore:
                                    s.PopFloat();
                                    s.PopInt();
                                    s.PopObjectType(_context.MethodAnalyzerFactory.FloatArrayType);
                                    break;
                                case NormalizedOpCode.Arraylength:
                                    s.PopArrayType();
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode.Iconst:
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode.IfIcmpeq:
                                case NormalizedOpCode.IfIcmpne:
                                case NormalizedOpCode.IfIcmplt:
                                case NormalizedOpCode.IfIcmpge:
                                case NormalizedOpCode.IfIcmpgt:
                                case NormalizedOpCode.IfIcmple:
                                    s.PopInt();
                                    s.PopInt();
                                    break;
                                case NormalizedOpCode.Ifeq:
                                case NormalizedOpCode.Ifge:
                                case NormalizedOpCode.Ifgt:
                                case NormalizedOpCode.Ifle:
                                case NormalizedOpCode.Iflt:
                                case NormalizedOpCode.Ifne:
                                    s.PopInt();
                                    break;
                                case NormalizedOpCode.IfNonNull:
                                case NormalizedOpCode.IfNull:
                                    // TODO it might be legal to use an unitialized ref here
                                    s.PopObjectType();
                                    break;
                                case NormalizedOpCode.IfAcmpeq:
                                case NormalizedOpCode.IfAcmpne:
                                    // TODO it might be legal to use an unitialized ref here
                                    s.PopObjectType();
                                    s.PopObjectType();
                                    break;
                                case NormalizedOpCode.GetStatic:
                                case NormalizedOpCode.DynamicGetStatic:
                                    // special support for when we're being called from IsSideEffectFreeStaticInitializer
                                    if (_method == null)
                                    {
                                        switch (GetFieldref(inst.Arg1).Signature[0])
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
                                        var cpi = GetFieldref(inst.Arg1);
                                        if (cpi.GetField() != null && cpi.GetField().FieldTypeWrapper.IsUnloadable)
                                            s.PushType(cpi.GetField().FieldTypeWrapper);
                                        else
                                            s.PushType(cpi.GetFieldType());
                                    }
                                    break;
                                case NormalizedOpCode.PutStatic:
                                case NormalizedOpCode.DynamicPutStatic:
                                    // special support for when we're being called from IsSideEffectFreeStaticInitializer
                                    if (_method == null)
                                    {
                                        switch (GetFieldref(inst.Arg1).Signature[0])
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
                                                if (!s.PopAnyType().IsNull)
                                                    throw new VerifyError();
                                                break;
                                            default:
                                                throw new InvalidOperationException();
                                        }
                                    }
                                    else
                                    {
                                        s.PopType(GetFieldref(inst.Arg1).GetFieldType());
                                    }
                                    break;
                                case NormalizedOpCode.GetField:
                                case NormalizedOpCode.DynamicGetField:
                                    {
                                        s.PopObjectType(GetFieldref(inst.Arg1).GetClassType());

                                        var cpi = GetFieldref(inst.Arg1);
                                        if (cpi.GetField() != null && cpi.GetField().FieldTypeWrapper.IsUnloadable)
                                            s.PushType(cpi.GetField().FieldTypeWrapper);
                                        else
                                            s.PushType(cpi.GetFieldType());

                                        break;
                                    }
                                case NormalizedOpCode.PutField:
                                case NormalizedOpCode.DynamicPutField:
                                    s.PopType(GetFieldref(inst.Arg1).GetFieldType());

                                    // putfield is allowed to access the uninitialized this
                                    if (s.PeekType().IsUninitializedThis && _type.IsAssignableTo(GetFieldref(inst.Arg1).GetClassType()))
                                        s.PopType();
                                    else
                                        s.PopObjectType(GetFieldref(inst.Arg1).GetClassType());

                                    break;
                                case NormalizedOpCode.LdcNothrow:
                                case NormalizedOpCode.Ldc:
                                    {
                                        switch (GetConstantPoolConstantType(inst.Arg1))
                                        {
                                            case ConstantType.Double:
                                                s.PushDouble();
                                                break;
                                            case ConstantType.Float:
                                                s.PushFloat();
                                                break;
                                            case ConstantType.Integer:
                                                s.PushInt();
                                                break;
                                            case ConstantType.Long:
                                                s.PushLong();
                                                break;
                                            case ConstantType.String:
                                                s.PushType(_context.JavaBase.TypeOfJavaLangString);
                                                break;
                                            case ConstantType.LiveObject:
                                                s.PushType(_context.JavaBase.TypeOfJavaLangObject);
                                                break;
                                            case ConstantType.Class:
                                                if (_classFile.MajorVersion < 49)
                                                    throw new VerifyError("Illegal type in constant pool");

                                                s.PushType(_context.JavaBase.TypeOfJavaLangClass);
                                                break;
                                            case ConstantType.MethodHandle:
                                                s.PushType(_context.JavaBase.TypeOfJavaLangInvokeMethodHandle);
                                                break;
                                            case ConstantType.MethodType:
                                                s.PushType(_context.JavaBase.TypeOfJavaLangInvokeMethodType);
                                                break;
                                            default:
                                                // NOTE this is not a VerifyError, because it cannot happen (unless we have
                                                // a bug in ClassFile.GetConstantPoolConstantType)
                                                throw new InvalidOperationException();
                                        }

                                        break;
                                    }
                                case NormalizedOpCode.CloneArray:
                                case NormalizedOpCode.InvokeVirtual:
                                case NormalizedOpCode.InvokeSpecial:
                                case NormalizedOpCode.InvokeInterface:
                                case NormalizedOpCode.InvokeStatic:
                                case NormalizedOpCode.DynamicInvokeVirtual:
                                case NormalizedOpCode.DynamicInvokeSpecial:
                                case NormalizedOpCode.DynamicInvokeInterface:
                                case NormalizedOpCode.DynamicInvokeStatic:
                                case NormalizedOpCode.PrivilegedInvokeVirtual:
                                case NormalizedOpCode.PrivilegedInvokeSpecial:
                                case NormalizedOpCode.PrivilegedInvokeStatic:
                                case NormalizedOpCode.MethodHandleInvoke:
                                case NormalizedOpCode.MethodHandleLink:
                                    {
                                        var cpi = GetMethodref(inst.Arg1);
                                        var retType = cpi.GetRetType();

                                        // HACK to allow the result of Unsafe.getObjectVolatile() (on an array)
                                        // to be used with Unsafe.putObject() we need to propagate the
                                        // element type here as the return type (instead of object)
                                        if (cpi.GetMethod() != null && cpi.GetMethod().IsIntrinsic && cpi.Class == "sun.misc.Unsafe" && cpi.Name == "getObjectVolatile" && s.GetStackSlot(1).IsReference && Context.Intrinsics.IsSupportedArrayTypeForUnsafeOperation(s.GetStackSlot(1).Type))
                                            retType = s.GetStackSlot(1).Type.ElementTypeWrapper;

                                        s.MultiPopAnyType(cpi.GetArgTypes().Length);

                                        if (inst.NormalizedOpCode != NormalizedOpCode.InvokeStatic && inst.NormalizedOpCode != NormalizedOpCode.DynamicInvokeStatic)
                                        {
                                            var type = s.PopType();
                                            if (ReferenceEquals(cpi.Name, StringConstants.INIT))
                                            {
                                                // after we've invoked the constructor, the uninitialized references
                                                // are now initialized
                                                if (type.IsUninitializedThis)
                                                {
                                                    if (s.GetLocalTypeEx(0) == type)
                                                        s.SetLocalType(0, _thisType, i);

                                                    s.MarkInitialized(type, VerificationType.Reference(_type), i);
                                                    s.SetUnitializedThis(false);
                                                }
                                                else if (type.IsUninitializedNew)
                                                {
                                                    s.MarkInitialized(type, VerificationType.Reference(type.Type), i);
                                                }
                                                else
                                                {
                                                    // This is a VerifyError, but it will be caught by our second pass
                                                }
                                            }
                                        }

                                        if (retType != _context.PrimitiveJavaTypeFactory.VOID)
                                        {
                                            if (cpi.GetMethod() != null && cpi.GetMethod().ReturnType.IsUnloadable)
                                            {
                                                s.PushType(cpi.GetMethod().ReturnType);
                                            }
                                            else if (retType == _context.PrimitiveJavaTypeFactory.DOUBLE)
                                            {
                                                s.PushExtendedDouble();
                                            }
                                            else if (retType == _context.PrimitiveJavaTypeFactory.FLOAT)
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
                                case NormalizedOpCode.InvokeDynamic:
                                    {
                                        var cpi = GetInvokeDynamic(inst.Arg1);
                                        s.MultiPopAnyType(cpi.GetArgTypes().Length);

                                        var retType = cpi.GetRetType();
                                        if (retType != _context.PrimitiveJavaTypeFactory.VOID)
                                        {
                                            if (retType == _context.PrimitiveJavaTypeFactory.DOUBLE)
                                            {
                                                s.PushExtendedDouble();
                                            }
                                            else if (retType == _context.PrimitiveJavaTypeFactory.FLOAT)
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
                                case NormalizedOpCode.Goto:
                                    break;
                                case NormalizedOpCode.Istore:
                                    s.PopInt();
                                    s.SetLocalInt(inst.NormalizedArg1, i);
                                    break;
                                case NormalizedOpCode.Iload:
                                    s.GetLocalInt(inst.NormalizedArg1);
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode.Ineg:
                                    s.PopInt();
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode.Iadd:
                                case NormalizedOpCode.Isub:
                                case NormalizedOpCode.Imul:
                                case NormalizedOpCode.Idiv:
                                case NormalizedOpCode.Irem:
                                case NormalizedOpCode.Iand:
                                case NormalizedOpCode.Ior:
                                case NormalizedOpCode.Ixor:
                                case NormalizedOpCode.Ishl:
                                case NormalizedOpCode.Ishr:
                                case NormalizedOpCode.Iushr:
                                    s.PopInt();
                                    s.PopInt();
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode.Lneg:
                                    s.PopLong();
                                    s.PushLong();
                                    break;
                                case NormalizedOpCode.Ladd:
                                case NormalizedOpCode.Lsub:
                                case NormalizedOpCode.Lmul:
                                case NormalizedOpCode.Ldiv:
                                case NormalizedOpCode.Lrem:
                                case NormalizedOpCode.Land:
                                case NormalizedOpCode.Lor:
                                case NormalizedOpCode.Lxor:
                                    s.PopLong();
                                    s.PopLong();
                                    s.PushLong();
                                    break;
                                case NormalizedOpCode.Lshl:
                                case NormalizedOpCode.Lshr:
                                case NormalizedOpCode.Lushr:
                                    s.PopInt();
                                    s.PopLong();
                                    s.PushLong();
                                    break;
                                case NormalizedOpCode.Fneg:
                                    if (s.PopFloat())
                                        s.PushExtendedFloat();
                                    else
                                        s.PushFloat();
                                    break;
                                case NormalizedOpCode.Fadd:
                                case NormalizedOpCode.Fsub:
                                case NormalizedOpCode.Fmul:
                                case NormalizedOpCode.Fdiv:
                                case NormalizedOpCode.Frem:
                                    s.PopFloat();
                                    s.PopFloat();
                                    s.PushExtendedFloat();
                                    break;
                                case NormalizedOpCode.Dneg:
                                    if (s.PopDouble())
                                        s.PushExtendedDouble();
                                    else
                                        s.PushDouble();

                                    break;
                                case NormalizedOpCode.Dadd:
                                case NormalizedOpCode.Dsub:
                                case NormalizedOpCode.Dmul:
                                case NormalizedOpCode.Ddiv:
                                case NormalizedOpCode.Drem:
                                    s.PopDouble();
                                    s.PopDouble();
                                    s.PushExtendedDouble();
                                    break;
                                case NormalizedOpCode.New:
                                    {
                                        // mark the type, so that we can ascertain that it is a "new object"
                                        if (!_newTypes.TryGetValue(i, out var type))
                                        {
                                            var classType = GetConstantPoolClassType(inst.Arg1);
                                            if (classType.IsArray)
                                                throw new VerifyError("Illegal use of array type");

                                            type = VerificationType.Uninitialized(classType, i);
                                            _newTypes[i] = type;
                                        }

                                        s.PushType(type);
                                        break;
                                    }
                                case NormalizedOpCode.Multianewarray:
                                    {
                                        if (inst.Arg2 < 1)
                                            throw new VerifyError("Illegal dimension argument");

                                        for (int j = 0; j < inst.Arg2; j++)
                                            s.PopInt();

                                        var type = GetConstantPoolClassType(inst.Arg1);
                                        if (type.ArrayRank < inst.Arg2)
                                            throw new VerifyError("Illegal dimension argument");

                                        s.PushType(type);
                                        break;
                                    }
                                case NormalizedOpCode.Anewarray:
                                    {
                                        s.PopInt();
                                        var type = GetConstantPoolClassType(inst.Arg1);
                                        if (type.IsUnloadable)
                                            s.PushType(new RuntimeUnloadableJavaType(_context, "[" + type.SignatureName));
                                        else
                                            s.PushType(type.MakeArrayType(1));

                                        break;
                                    }
                                case NormalizedOpCode.Newarray:
                                    s.PopInt();
                                    switch (inst.Arg1)
                                    {
                                        case 4:
                                            s.PushType(_context.MethodAnalyzerFactory.BooleanArrayType);
                                            break;
                                        case 5:
                                            s.PushType(_context.MethodAnalyzerFactory.CharArrayType);
                                            break;
                                        case 6:
                                            s.PushType(_context.MethodAnalyzerFactory.FloatArrayType);
                                            break;
                                        case 7:
                                            s.PushType(_context.MethodAnalyzerFactory.DoubleArrayType);
                                            break;
                                        case 8:
                                            s.PushType(_context.MethodAnalyzerFactory.ByteArrayType);
                                            break;
                                        case 9:
                                            s.PushType(_context.MethodAnalyzerFactory.ShortArrayType);
                                            break;
                                        case 10:
                                            s.PushType(_context.MethodAnalyzerFactory.IntArrayType);
                                            break;
                                        case 11:
                                            s.PushType(_context.MethodAnalyzerFactory.LongArrayType);
                                            break;
                                        default:
                                            throw new VerifyError("Bad type");
                                    }
                                    break;
                                case NormalizedOpCode.Swap:
                                    {
                                        var t1 = s.PopType();
                                        var t2 = s.PopType();
                                        s.PushType(t1);
                                        s.PushType(t2);
                                        break;
                                    }
                                case NormalizedOpCode.Dup:
                                    {
                                        var t = s.PopType();
                                        s.PushType(t);
                                        s.PushType(t);
                                        break;
                                    }
                                case NormalizedOpCode.Dup2:
                                    {
                                        var t = s.PopAnyType();
                                        if (t.IsCategory2)
                                        {
                                            s.PushType(t);
                                            s.PushType(t);
                                        }
                                        else
                                        {
                                            var t2 = s.PopType();
                                            s.PushType(t2);
                                            s.PushType(t);
                                            s.PushType(t2);
                                            s.PushType(t);
                                        }
                                        break;
                                    }
                                case NormalizedOpCode.DupX1:
                                    {
                                        var value1 = s.PopType();
                                        var value2 = s.PopType();
                                        s.PushType(value1);
                                        s.PushType(value2);
                                        s.PushType(value1);
                                        break;
                                    }
                                case NormalizedOpCode.Dup2X1:
                                    {
                                        var value1 = s.PopAnyType();
                                        if (value1.IsCategory2)
                                        {
                                            var value2 = s.PopType();
                                            s.PushType(value1);
                                            s.PushType(value2);
                                            s.PushType(value1);
                                        }
                                        else
                                        {
                                            var value2 = s.PopType();
                                            var value3 = s.PopType();
                                            s.PushType(value2);
                                            s.PushType(value1);
                                            s.PushType(value3);
                                            s.PushType(value2);
                                            s.PushType(value1);
                                        }
                                        break;
                                    }
                                case NormalizedOpCode.DupX2:
                                    {
                                        var value1 = s.PopType();
                                        var value2 = s.PopAnyType();
                                        if (value2.IsCategory2)
                                        {
                                            s.PushType(value1);
                                            s.PushType(value2);
                                            s.PushType(value1);
                                        }
                                        else
                                        {
                                            var value3 = s.PopType();
                                            s.PushType(value1);
                                            s.PushType(value3);
                                            s.PushType(value2);
                                            s.PushType(value1);
                                        }
                                        break;
                                    }
                                case NormalizedOpCode.Dup2X2:
                                    {
                                        var value1 = s.PopAnyType();
                                        if (value1.IsCategory2)
                                        {
                                            var value2 = s.PopAnyType();
                                            if (value2.IsCategory2)
                                            {
                                                // Form 4
                                                s.PushType(value1);
                                                s.PushType(value2);
                                                s.PushType(value1);
                                            }
                                            else
                                            {
                                                // Form 2
                                                var value3 = s.PopType();
                                                s.PushType(value1);
                                                s.PushType(value3);
                                                s.PushType(value2);
                                                s.PushType(value1);
                                            }
                                        }
                                        else
                                        {
                                            var value2 = s.PopType();
                                            var value3 = s.PopAnyType();
                                            if (value3.IsCategory2)
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
                                                var value4 = s.PopType();
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
                                case NormalizedOpCode.Pop:
                                    s.PopType();
                                    break;
                                case NormalizedOpCode.Pop2:
                                    {
                                        var type = s.PopAnyType();
                                        if (!type.IsCategory2)
                                            s.PopType();

                                        break;
                                    }
                                case NormalizedOpCode.MonitorEnter:
                                case NormalizedOpCode.MonitorExit:
                                    // TODO these bytecodes are allowed on an uninitialized object, but
                                    // we don't support that at the moment...
                                    s.PopObjectType();
                                    break;
                                case NormalizedOpCode.Return:
                                    // mw is null if we're called from IsSideEffectFreeStaticInitializer
                                    if (_method != null)
                                    {
                                        if (_method.ReturnType != _context.PrimitiveJavaTypeFactory.VOID)
                                            throw new VerifyError("Wrong return type in function");

                                        // if we're a constructor, make sure we called the base class constructor
                                        s.CheckUninitializedThis();
                                    }
                                    break;
                                case NormalizedOpCode.Areturn:
                                    s.PopObjectType(_method.ReturnType);
                                    break;
                                case NormalizedOpCode.Ireturn:
                                    {
                                        s.PopInt();
                                        if (!_method.ReturnType.IsIntOnStackPrimitive)
                                            throw new VerifyError("Wrong return type in function");

                                        break;
                                    }
                                case NormalizedOpCode.Lreturn:
                                    s.PopLong();
                                    if (_method.ReturnType != _context.PrimitiveJavaTypeFactory.LONG)
                                        throw new VerifyError("Wrong return type in function");

                                    break;
                                case NormalizedOpCode.Freturn:
                                    s.PopFloat();
                                    if (_method.ReturnType != _context.PrimitiveJavaTypeFactory.FLOAT)
                                        throw new VerifyError("Wrong return type in function");

                                    break;
                                case NormalizedOpCode.Dreturn:
                                    s.PopDouble();
                                    if (_method.ReturnType != _context.PrimitiveJavaTypeFactory.DOUBLE)
                                        throw new VerifyError("Wrong return type in function");

                                    break;
                                case NormalizedOpCode.Fload:
                                    s.GetLocalFloat(inst.NormalizedArg1);
                                    s.PushFloat();
                                    break;
                                case NormalizedOpCode.Fstore:
                                    s.PopFloat();
                                    s.SetLocalFloat(inst.NormalizedArg1, i);
                                    break;
                                case NormalizedOpCode.Dload:
                                    s.GetLocalDouble(inst.NormalizedArg1);
                                    s.PushDouble();
                                    break;
                                case NormalizedOpCode.Dstore:
                                    s.PopDouble();
                                    s.SetLocalDouble(inst.NormalizedArg1, i);
                                    break;
                                case NormalizedOpCode.Lload:
                                    s.GetLocalLong(inst.NormalizedArg1);
                                    s.PushLong();
                                    break;
                                case NormalizedOpCode.Lstore:
                                    s.PopLong();
                                    s.SetLocalLong(inst.NormalizedArg1, i);
                                    break;
                                case NormalizedOpCode.Lconst0:
                                case NormalizedOpCode.Lconst1:
                                    s.PushLong();
                                    break;
                                case NormalizedOpCode.Fconst0:
                                case NormalizedOpCode.Fconst1:
                                case NormalizedOpCode.Fconst2:
                                    s.PushFloat();
                                    break;
                                case NormalizedOpCode.Dconst0:
                                case NormalizedOpCode.Dconst1:
                                    s.PushDouble();
                                    break;
                                case NormalizedOpCode.Lcmp:
                                    s.PopLong();
                                    s.PopLong();
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode.Fcmpl:
                                case NormalizedOpCode.Fcmpg:
                                    s.PopFloat();
                                    s.PopFloat();
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode.Dcmpl:
                                case NormalizedOpCode.Dcmpg:
                                    s.PopDouble();
                                    s.PopDouble();
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode.Checkcast:
                                    s.PopObjectType();
                                    s.PushType(GetConstantPoolClassType(inst.Arg1));
                                    break;
                                case NormalizedOpCode.InstanceOf:
                                    s.PopObjectType();
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode.Iinc:
                                    s.GetLocalInt(inst.Arg1);
                                    break;
                                case NormalizedOpCode.Athrow:
                                    if (s.PeekType().IsFaultBlockException)
                                        s.PopFaultBlockException();
                                    else
                                        s.PopObjectType(_context.JavaBase.TypeOfjavaLangThrowable);
                                    break;
                                case NormalizedOpCode.TableSwitch:
                                case NormalizedOpCode.LookupSwitch:
                                    s.PopInt();
                                    break;
                                case NormalizedOpCode.I2b:
                                    s.PopInt();
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode.I2c:
                                    s.PopInt();
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode.I2s:
                                    s.PopInt();
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode.I2l:
                                    s.PopInt();
                                    s.PushLong();
                                    break;
                                case NormalizedOpCode.I2f:
                                    s.PopInt();
                                    s.PushFloat();
                                    break;
                                case NormalizedOpCode.I2d:
                                    s.PopInt();
                                    s.PushDouble();
                                    break;
                                case NormalizedOpCode.L2i:
                                    s.PopLong();
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode.L2f:
                                    s.PopLong();
                                    s.PushFloat();
                                    break;
                                case NormalizedOpCode.L2d:
                                    s.PopLong();
                                    s.PushDouble();
                                    break;
                                case NormalizedOpCode.F2i:
                                    s.PopFloat();
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode.F2l:
                                    s.PopFloat();
                                    s.PushLong();
                                    break;
                                case NormalizedOpCode.F2d:
                                    s.PopFloat();
                                    s.PushDouble();
                                    break;
                                case NormalizedOpCode.D2i:
                                    s.PopDouble();
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode.D2f:
                                    s.PopDouble();
                                    s.PushFloat();
                                    break;
                                case NormalizedOpCode.D2l:
                                    s.PopDouble();
                                    s.PushLong();
                                    break;
                                case NormalizedOpCode.Nop:
                                    if (i + 1 == instructions.Length)
                                        throw new VerifyError("Falling off the end of the code");
                                    break;
                                case NormalizedOpCode.StaticError:
                                    break;
                                case NormalizedOpCode.Jsr:
                                case NormalizedOpCode.Ret:
                                    throw new VerifyError("Bad instruction");
                                default:
                                    throw new NotImplementedException(inst.NormalizedOpCode.ToString());
                            }

                            if (s.GetStackHeight() > _classFileMethod.MaxStack)
                                throw new VerifyError("Stack size too large");

                            for (int j = 0; j < _classFileMethod.ExceptionTable.Length; j++)
                                if (_classFileMethod.ExceptionTable[j].EndIndex == i + 1)
                                    MergeExceptionHandler(j, ref s);

                            try
                            {
                                switch (OpCodeMetaData.GetFlowKind(inst.NormalizedOpCode))
                                {
                                    case OpCodeFlowControl.Switch:
                                        for (int j = 0; j < inst.SwitchEntryCount; j++)
                                            _state[inst.GetSwitchTargetIndex(j)] += s;

                                        _state[inst.DefaultTarget] += s;
                                        break;
                                    case OpCodeFlowControl.ConditionalBranch:
                                        _state[i + 1] += s;
                                        _state[inst.TargetIndex] += s;
                                        break;
                                    case OpCodeFlowControl.Branch:
                                        _state[inst.TargetIndex] += s;
                                        break;
                                    case OpCodeFlowControl.Return:
                                    case OpCodeFlowControl.Throw:
                                        break;
                                    case OpCodeFlowControl.Next:
                                        _state[i + 1] += s;
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
                            var opcode = instructions[i].NormalizedOpCode.ToString();
                            if (opcode.StartsWith("__"))
                                opcode = opcode.Substring(2);

                            throw new VerifyError($"{x.Message} (class: {_classFile.Name}, method: {_classFileMethod.Name}, signature: {_classFileMethod.Signature}, offset: {instructions[i].PC}, instruction: {opcode})", x);
                        }
                    }
                }
            }
        }

        void MergeExceptionHandler(int exceptionIndex, ref InstructionState curr)
        {
            var idx = _classFileMethod.ExceptionTable[exceptionIndex].HandlerIndex;
            var exp = curr.CopyLocals();

            var catchType = _classFileMethod.ExceptionTable[exceptionIndex].CatchType;
            if (catchType.IsNil)
            {
                exp.PushType(VerificationType.FaultBlockException(_context.JavaBase.TypeOfjavaLangThrowable, idx));
            }
            else
            {
                // TODO if the exception type is unloadable we should consider pushing
                // Throwable as the type and recording a loader constraint
                exp.PushType(GetConstantPoolClassType(catchType));
            }

            _state[idx] += exp;
        }

        // this verification pass must run on the unmodified bytecode stream
        void VerifyPassTwo()
        {
            var instructions = _classFileMethod.Instructions;
            for (int i = 0; i < instructions.Length; i++)
            {
                if (_state[i]._initialized)
                {
                    try
                    {
                        switch (instructions[i].NormalizedOpCode)
                        {
                            case NormalizedOpCode.InvokeInterface:
                            case NormalizedOpCode.InvokeSpecial:
                            case NormalizedOpCode.InvokeStatic:
                            case NormalizedOpCode.InvokeVirtual:
                                VerifyInvokePassTwo(i);
                                break;
                            case NormalizedOpCode.InvokeDynamic:
                                VerifyInvokeDynamic(i);
                                break;
                        }
                    }
                    catch (VerifyError x)
                    {
                        var opcode = instructions[i].NormalizedOpCode.ToString();
                        if (opcode.StartsWith("__"))
                            opcode = opcode.Substring(2);

                        throw new VerifyError($"{x.Message} (class: {_classFile.Name}, method: {_classFileMethod.Name}, signature: {_classFileMethod.Signature}, offset: {instructions[i].PC}, instruction: {opcode})", x);
                    }
                }
            }
        }

        void VerifyInvokePassTwo(int index)
        {
            var stack = new StackState(_state, index);
            var invoke = _classFileMethod.Instructions[index].NormalizedOpCode;
            var cpi = GetMethodref(_classFileMethod.Instructions[index].Arg1);
            if ((invoke == NormalizedOpCode.InvokeStatic || invoke == NormalizedOpCode.InvokeSpecial) && _classFile.MajorVersion >= 52)
            {
                // invokestatic and invokespecial may be used to invoke interface methods in Java 8
                // but invokespecial can only invoke methods in the current interface or a directly implemented interface
                if (invoke == NormalizedOpCode.InvokeSpecial && cpi is ConstantPoolItemInterfaceMethodref)
                {
                    if (cpi.GetClassType() == _host)
                    {
                        // ok
                    }
                    else if (cpi.GetClassType() != _type && Array.IndexOf(_type.Interfaces, cpi.GetClassType()) == -1)
                    {
                        throw new VerifyError("Bad invokespecial instruction: interface method reference is in an indirect superinterface.");
                    }
                }
            }
            else if ((cpi is ConstantPoolItemInterfaceMethodref) != (invoke == NormalizedOpCode.InvokeInterface))
            {
                throw new VerifyError("Illegal constant pool index");
            }

            if (invoke != NormalizedOpCode.InvokeSpecial && ReferenceEquals(cpi.Name, StringConstants.INIT))
                throw new VerifyError("Must call initializers using invokespecial");

            if (ReferenceEquals(cpi.Name, StringConstants.CLINIT))
                throw new VerifyError("Illegal call to internal method");

            var args = cpi.GetArgTypes();
            for (int j = args.Length - 1; j >= 0; j--)
                stack.PopType(args[j]);

            if (invoke == NormalizedOpCode.InvokeInterface)
            {
                int argcount = args.Length + 1;
                for (int j = 0; j < args.Length; j++)
                    if (args[j].IsWidePrimitive)
                        argcount++;

                if (_classFileMethod.Instructions[index].Arg2 != argcount)
                    throw new VerifyError("Inconsistent args size");
            }

            if (invoke != NormalizedOpCode.InvokeStatic)
            {
                if (ReferenceEquals(cpi.Name, StringConstants.INIT))
                {
                    var type = stack.PopType();
                    var isnew = type.IsUninitializedNew;
                    if ((isnew && type.Type != cpi.GetClassType()) || (type.IsUninitializedThis && cpi.GetClassType() != _type.BaseTypeWrapper && cpi.GetClassType() != _type) || (!isnew && !type.IsUninitializedThis))
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
                    if (invoke != NormalizedOpCode.InvokeInterface)
                    {
                        var refType = stack.PopObjectType();
                        var targetType = cpi.GetClassType();

                        if (!refType.IsNullOrUnloadable() && !targetType.IsUnloadable && !refType.Type.IsAssignableTo(targetType))
                            throw new VerifyError("Incompatible object argument for function call");

                        // for invokespecial we also need to make sure we're calling ourself or a base class
                        if (invoke == NormalizedOpCode.InvokeSpecial)
                        {
                            if (refType.IsNullOrUnloadable())
                            {
                                // ok
                            }
                            else if (refType.Type.IsSubTypeOf(_type))
                            {
                                // ok
                            }
                            else if (_host != null && refType.Type.IsSubTypeOf(_host))
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
                            else if (_type.IsSubTypeOf(targetType))
                            {
                                // ok
                            }
                            else if (_host != null && _host.IsSubTypeOf(targetType))
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
                        var refType = stack.PopObjectType();
                        var targetType = cpi.GetClassType();
                        if (!refType.IsNullOrUnloadable() && !targetType.IsUnloadable && !refType.Type.IsAssignableTo(targetType) && !targetType.IsInterface)
                            throw new VerifyError("Incompatible object argument for function call");
                    }
                }
            }
        }

        void VerifyInvokeDynamic(int index)
        {
            var stack = new StackState(_state, index);
            var cpi = GetInvokeDynamic(_classFileMethod.Instructions[index].Arg1);
            var args = cpi.GetArgTypes();
            for (int j = args.Length - 1; j >= 0; j--)
                stack.PopType(args[j]);
        }

        static void OptimizationPass(CodeInfo codeInfo, ClassFile classFile, Method method, UntangledExceptionTable exceptions, RuntimeJavaType wrapper, RuntimeClassLoader classLoader)
        {
            // optimization pass
            if (classLoader.RemoveAsserts)
            {
                // while the optimization is general, in practice it never happens that a getstatic is used on a final field,
                // so we only look for this if assert initialization has been optimized out
                if (classFile.HasAssertions)
                {
                    // compute branch targets
                    var flags = ComputePartialReachability(codeInfo, method.Instructions, exceptions, 0, false);
                    var instructions = method.Instructions;
                    for (int i = 0; i < instructions.Length; i++)
                    {
                        if (instructions[i].NormalizedOpCode == NormalizedOpCode.GetStatic &&
                            instructions[i + 1].NormalizedOpCode == NormalizedOpCode.Ifne &&
                            instructions[i + 1].TargetIndex > i &&
                            (flags[i + 1] & InstructionFlags.BranchTarget) == 0)
                        {
                            if (classFile.GetFieldref(checked((ushort)instructions[i].Arg1)).GetField() is RuntimeConstantJavaField field &&
                                field.FieldTypeWrapper == classLoader.Context.PrimitiveJavaTypeFactory.BOOLEAN &&
                                (bool)field.GetConstantValue())
                            {
                                // we know the branch will always be taken, so we replace the getstatic/ifne by a goto.
                                instructions[i].PatchOpCode(NormalizedOpCode.Goto, instructions[i + 1].TargetIndex);
                            }
                        }
                    }
                }
            }
        }

        void PatchHardErrorsAndDynamicMemberAccess(RuntimeJavaType wrapper, RuntimeJavaMethod mw)
        {
            // Now we do another pass to find "hard error" instructions
            if (true)
            {
                var instructions = _classFileMethod.Instructions;
                for (int i = 0; i < instructions.Length; i++)
                {
                    if (_state[i]._initialized)
                    {
                        var stack = new StackState(_state, i);

                        switch (instructions[i].NormalizedOpCode)
                        {
                            case NormalizedOpCode.InvokeInterface:
                            case NormalizedOpCode.InvokeSpecial:
                            case NormalizedOpCode.InvokeStatic:
                            case NormalizedOpCode.InvokeVirtual:
                                PatchInvoke(wrapper, ref instructions[i], stack);
                                break;
                            case NormalizedOpCode.GetField:
                            case NormalizedOpCode.PutField:
                            case NormalizedOpCode.GetStatic:
                            case NormalizedOpCode.PutStatic:
                                PatchFieldAccess(wrapper, mw, ref instructions[i], stack);
                                break;
                            case NormalizedOpCode.Ldc:
                                switch (_classFile.GetConstantPoolConstantType(instructions[i].Arg1))
                                {
                                    case ConstantType.Class:
                                        {
                                            var tw = _classFile.GetConstantPoolClassType(instructions[i].Arg1);
                                            if (tw.IsUnloadable)
                                                ConditionalPatchNoClassDefFoundError(ref instructions[i], tw);

                                            break;
                                        }
                                    case ConstantType.MethodType:
                                        {
                                            var cpi = _classFile.GetConstantPoolConstantMethodType(checked((ushort)instructions[i].Arg1));
                                            var args = cpi.GetArgTypes();
                                            var tw = cpi.GetRetType();
                                            for (int j = 0; !tw.IsUnloadable && j < args.Length; j++)
                                                tw = args[j];

                                            if (tw.IsUnloadable)
                                                ConditionalPatchNoClassDefFoundError(ref instructions[i], tw);

                                            break;
                                        }
                                    case ConstantType.MethodHandle:
                                        PatchLdcMethodHandle(ref instructions[i]);
                                        break;
                                }
                                break;
                            case NormalizedOpCode.New:
                                {
                                    var tw = _classFile.GetConstantPoolClassType(instructions[i].Arg1);
                                    if (tw.IsUnloadable)
                                    {
                                        ConditionalPatchNoClassDefFoundError(ref instructions[i], tw);
                                    }
                                    else if (!tw.IsAccessibleFrom(wrapper))
                                    {
                                        SetHardError(wrapper.ClassLoader, ref instructions[i], HardError.IllegalAccessError, "Try to access class {0} from class {1}", tw.Name, wrapper.Name);
                                    }
                                    else if (tw.IsAbstract)
                                    {
                                        SetHardError(wrapper.ClassLoader, ref instructions[i], HardError.InstantiationError, "{0}", tw.Name);
                                    }

                                    break;
                                }
                            case NormalizedOpCode.Multianewarray:
                            case NormalizedOpCode.Anewarray:
                                {
                                    var tw = _classFile.GetConstantPoolClassType(instructions[i].Arg1);
                                    if (tw.IsUnloadable)
                                    {
                                        ConditionalPatchNoClassDefFoundError(ref instructions[i], tw);
                                    }
                                    else if (!tw.IsAccessibleFrom(wrapper))
                                    {
                                        SetHardError(wrapper.ClassLoader, ref instructions[i], HardError.IllegalAccessError, "Try to access class {0} from class {1}", tw.Name, wrapper.Name);
                                    }

                                    break;
                                }
                            case NormalizedOpCode.Checkcast:
                            case NormalizedOpCode.InstanceOf:
                                {
                                    var tw = _classFile.GetConstantPoolClassType(instructions[i].Arg1);
                                    if (tw.IsUnloadable)
                                    {
                                        // If the type is unloadable, we always generate the dynamic code
                                        // (regardless of ClassLoaderWrapper.DisableDynamicBinding), because at runtime,
                                        // null references should always pass thru without attempting
                                        // to load the type (for Sun compatibility).
                                    }
                                    else if (!tw.IsAccessibleFrom(wrapper))
                                    {
                                        SetHardError(wrapper.ClassLoader, ref instructions[i], HardError.IllegalAccessError, "Try to access class {0} from class {1}", tw.Name, wrapper.Name);
                                    }

                                    break;
                                }
                            case NormalizedOpCode.Aaload:
                                {
                                    stack.PopInt();
                                    var tw = stack.PopArrayType();
                                    if (tw.IsUnloadable())
                                        ConditionalPatchNoClassDefFoundError(ref instructions[i], tw.Type);

                                    break;
                                }
                            case NormalizedOpCode.Aastore:
                                {
                                    stack.PopObjectType();
                                    stack.PopInt();
                                    var tw = stack.PopArrayType();
                                    if (tw.IsUnloadable())
                                    {
                                        ConditionalPatchNoClassDefFoundError(ref instructions[i], tw.Type);
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

        void PatchLdcMethodHandle(ref Instruction instr)
        {
            var cpi = _classFile.GetConstantPoolConstantMethodHandle(checked((ushort)instr.Arg1));
            if (cpi.GetClassType().IsUnloadable)
            {
                ConditionalPatchNoClassDefFoundError(ref instr, cpi.GetClassType());
            }
            else if (!cpi.GetClassType().IsAccessibleFrom(_type))
            {
                SetHardError(_type.ClassLoader, ref instr, HardError.IllegalAccessError, "tried to access class {0} from class {1}", cpi.Class, _type.Name);
            }
            else if (cpi.Kind == MethodHandleKind.InvokeVirtual && cpi.GetClassType() == _context.JavaBase.TypeOfJavaLangInvokeMethodHandle && (cpi.Name == "invoke" || cpi.Name == "invokeExact"))
            {
                // it's allowed to use ldc to create a MethodHandle invoker
            }
            else if (cpi.Member == null || cpi.Member.IsStatic != (cpi.Kind == MethodHandleKind.GetStatic || cpi.Kind == MethodHandleKind.PutStatic || cpi.Kind == MethodHandleKind.InvokeStatic))
            {
                HardError err;
                string msg;
                switch (cpi.Kind)
                {
                    case MethodHandleKind.GetField:
                    case MethodHandleKind.GetStatic:
                    case MethodHandleKind.PutField:
                    case MethodHandleKind.PutStatic:
                        err = HardError.NoSuchFieldError;
                        msg = cpi.Name;
                        break;
                    default:
                        err = HardError.NoSuchMethodError;
                        msg = cpi.Class + "." + cpi.Name + cpi.Signature;
                        break;
                }

                SetHardError(_type.ClassLoader, ref instr, err, msg, cpi.Class, cpi.Name, SigToString(cpi.Signature));
            }
            else if (!cpi.Member.IsAccessibleFrom(cpi.GetClassType(), _type, cpi.GetClassType()))
            {
                if (cpi.Member.IsProtected && _type.IsSubTypeOf(cpi.Member.DeclaringType))
                {
                    // this is allowed, the receiver will be narrowed to current type
                }
                else
                {
                    SetHardError(_type.ClassLoader, ref instr, HardError.IllegalAccessException, "member is private: {0}.{1}/{2}/{3}, from {4}", cpi.Class, cpi.Name, SigToString(cpi.Signature), cpi.Kind, _type.Name);
                }
            }
        }

        static string SigToString(string sig)
        {
            var sb = new ValueStringBuilder();
            var sep = "";
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
                        var j = sig.IndexOf(';', i + 1);
                        sb.Append(sig.AsSpan()[(i + 1)..j]);
                        i = j;
                        break;
                }

                for (; dims != 0; dims--)
                    sb.Append("[]");
            }

            return sb.ToString();
        }

        internal static InstructionFlags[] ComputePartialReachability(CodeInfo codeInfo, Instruction[] instructions, UntangledExceptionTable exceptions, int initialInstructionIndex, bool skipFaultBlocks)
        {
            var flags = new InstructionFlags[instructions.Length];
            flags[initialInstructionIndex] |= InstructionFlags.Reachable;
            UpdatePartialReachability(flags, codeInfo, instructions, exceptions, skipFaultBlocks);
            return flags;
        }

        static void UpdatePartialReachability(InstructionFlags[] flags, CodeInfo codeInfo, Instruction[] instructions, UntangledExceptionTable exceptions, bool skipFaultBlocks)
        {
            var done = false;

            while (done == false)
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
                                if (!skipFaultBlocks || !codeInfo.GetRawStackType(idx, 0).IsFaultBlockException)
                                    flags[idx] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
                            }
                        }

                        MarkSuccessors(instructions, flags, i);
                    }
                }
            }
        }

        static void MarkSuccessors(Instruction[] code, InstructionFlags[] flags, int index)
        {
            switch (OpCodeMetaData.GetFlowKind(code[index].NormalizedOpCode))
            {
                case OpCodeFlowControl.Switch:
                    {
                        for (int i = 0; i < code[index].SwitchEntryCount; i++)
                            flags[code[index].GetSwitchTargetIndex(i)] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;

                        flags[code[index].DefaultTarget] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
                        break;
                    }
                case OpCodeFlowControl.Branch:
                    flags[code[index].TargetIndex] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
                    break;
                case OpCodeFlowControl.ConditionalBranch:
                    flags[code[index].TargetIndex] |= InstructionFlags.Reachable | InstructionFlags.BranchTarget;
                    flags[index + 1] |= InstructionFlags.Reachable;
                    break;
                case OpCodeFlowControl.Return:
                case OpCodeFlowControl.Throw:
                    break;
                case OpCodeFlowControl.Next:
                    flags[index + 1] |= InstructionFlags.Reachable;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }

        internal static UntangledExceptionTable UntangleExceptionBlocks(RuntimeContext context, ClassFile classFile, Method method)
        {
            var instructions = method.Instructions;
            var ar = new List<ExceptionTableEntry>(method.ExceptionTable);

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
                var ei = ar[i];
                if (ei.StartIndex == ei.HandlerIndex && ei.CatchType.IsNil)
                {
                    var index = ei.StartIndex;
                    if (index + 2 < instructions.Length &&
                        ei.EndIndex == index + 2 &&
                        instructions[index].NormalizedOpCode == NormalizedOpCode.Aload &&
                        instructions[index + 1].NormalizedOpCode == NormalizedOpCode.MonitorExit &&
                        instructions[index + 2].NormalizedOpCode == NormalizedOpCode.Athrow)
                    {
                        // this is the async exception guard that Jikes and the Eclipse Java Compiler produce
                        ar.RemoveAt(i);
                        i--;
                    }
                    else if (index + 4 < instructions.Length &&
                        ei.EndIndex == index + 3 &&
                        instructions[index].NormalizedOpCode == NormalizedOpCode.Astore &&
                        instructions[index + 1].NormalizedOpCode == NormalizedOpCode.Aload &&
                        instructions[index + 2].NormalizedOpCode == NormalizedOpCode.MonitorExit &&
                        instructions[index + 3].NormalizedOpCode == NormalizedOpCode.Aload &&
                        instructions[index + 4].NormalizedOpCode == NormalizedOpCode.Athrow &&
                        instructions[index].NormalizedArg1 == instructions[index + 3].NormalizedArg1)
                    {
                        // this is the async exception guard that javac produces
                        ar.RemoveAt(i);
                        i--;
                    }
                    else if (index + 1 < instructions.Length &&
                        ei.EndIndex == index + 1 &&
                        instructions[index].NormalizedOpCode == NormalizedOpCode.Astore)
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
                if (ar[i].EndIndex + 1 == ar[i + 1].StartIndex &&
                    ar[i].HandlerIndex == ar[i + 1].HandlerIndex &&
                    ar[i].CatchType == ar[i + 1].CatchType &&
                    IsReturn(instructions[ar[i].EndIndex].NormalizedOpCode))
                {
                    ar[i] = new ExceptionTableEntry(ar[i].StartIndex, ar[i + 1].EndIndex, ar[i].HandlerIndex, ar[i].CatchType, ar[i].Ordinal);
                    ar.RemoveAt(i + 1);
                    i--;
                }
            }

        restart:
            for (int i = 0; i < ar.Count; i++)
            {
                var ei = ar[i];
                for (int j = 0; j < ar.Count; j++)
                {
                    var ej = ar[j];
                    if (ei.StartIndex <= ej.StartIndex && ej.StartIndex < ei.EndIndex)
                    {
                        // 0006/test.j
                        if (ej.EndIndex > ei.EndIndex)
                        {
                            var emi = new ExceptionTableEntry(ej.StartIndex, ei.EndIndex, ei.HandlerIndex, ei.CatchType, ei.Ordinal);
                            var emj = new ExceptionTableEntry(ej.StartIndex, ei.EndIndex, ej.HandlerIndex, ej.CatchType, ej.Ordinal);
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
                            var emi = new ExceptionTableEntry(ej.StartIndex, ej.EndIndex, ei.HandlerIndex, ei.CatchType, ei.Ordinal);
                            var eei = new ExceptionTableEntry(ej.EndIndex, ei.EndIndex, ei.HandlerIndex, ei.CatchType, ei.Ordinal);
                            ei = new ExceptionTableEntry(ei.StartIndex, emi.StartIndex, ei.HandlerIndex, ei.CatchType, ei.Ordinal);
                            ar[i] = ei;
                            ar.Insert(i + 1, eei);
                            ar.Insert(i + 1, emi);
                            goto restart;
                        }
                    }
                }
            }
            // Split try blocks at branch targets (branches from outside the try block) and at exception handlers (which
            // are also a kind of jump), because the CLR does not allow jumping into a try block. Splitting a block can
            // turn a branch inside it into a branch from one part into another, so the split points of a block are the
            // smallest set that leaves no part with a branch into it from outside, or a handler inside it. Splitting a
            // block does not affect the split points of any other block, so each block is split independently and its
            // parts take its place in the list.
            if (ar.Count > 0)
            {
                var branches = GetBranchesByTarget(instructions);
                var handlers = new HashSet<int>();
                foreach (var e in ar)
                    handlers.Add(e.HandlerIndex);

                for (int i = 0; i < ar.Count; i++)
                {
                    var ei = ar[i];
                    var splits = FindTryBlockSplits(ei.StartIndex, ei.EndIndex, branches, handlers);
                    if (splits == null)
                        continue;

                    ar[i] = new ExceptionTableEntry(ei.StartIndex, splits[0], ei.HandlerIndex, ei.CatchType, ei.Ordinal);
                    for (int k = 0; k < splits.Count; k++)
                        ar.Insert(i + 1 + k, new ExceptionTableEntry(splits[k], k + 1 < splits.Count ? splits[k + 1] : ei.EndIndex, ei.HandlerIndex, ei.CatchType, ei.Ordinal));

                    i += splits.Count;
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
                    else if (context.MethodAnalyzerFactory.JavaLangThreadDeathType.IsAssignableTo(exceptionType))
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
                                case NormalizedOpCode.Aload:
                                case NormalizedOpCode.Iload:
                                case NormalizedOpCode.Lload:
                                case NormalizedOpCode.Fload:
                                case NormalizedOpCode.Dload:
                                case NormalizedOpCode.Astore:
                                case NormalizedOpCode.Istore:
                                case NormalizedOpCode.Lstore:
                                case NormalizedOpCode.Fstore:
                                case NormalizedOpCode.Dstore:
                                    break;
                                case NormalizedOpCode.Dup:
                                case NormalizedOpCode.DupX1:
                                case NormalizedOpCode.DupX2:
                                case NormalizedOpCode.Dup2:
                                case NormalizedOpCode.Dup2X1:
                                case NormalizedOpCode.Dup2X2:
                                case NormalizedOpCode.Pop:
                                case NormalizedOpCode.Pop2:
                                    break;
                                case NormalizedOpCode.Return:
                                case NormalizedOpCode.Areturn:
                                case NormalizedOpCode.Ireturn:
                                case NormalizedOpCode.Lreturn:
                                case NormalizedOpCode.Freturn:
                                case NormalizedOpCode.Dreturn:
                                    break;
                                case NormalizedOpCode.Goto:
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
                            if (OpCodeMetaData.CanThrowException(instructions[j].NormalizedOpCode))
                                goto next;

                        ar.RemoveAt(i);
                        i--;
                    }
                }
            next:;
            }

            var exceptions = ar.ToArray();
            Array.Sort(exceptions, new ExceptionTableEntryComparer());
            return new UntangledExceptionTable(exceptions);
        }

        /// <summary>
        /// Returns every branch in the method as a (source, target) pair, ordered by target.
        /// </summary>
        static (int Source, int Target)[] GetBranchesByTarget(Instruction[] instructions)
        {
            var branches = new List<(int Source, int Target)>();
            for (int i = 0; i < instructions.Length; i++)
            {
                switch (instructions[i].NormalizedOpCode)
                {
                    case NormalizedOpCode.TableSwitch:
                    case NormalizedOpCode.LookupSwitch:
                        branches.Add((i, instructions[i].DefaultTarget));
                        for (int k = 0; k < instructions[i].SwitchEntryCount; k++)
                            branches.Add((i, instructions[i].GetSwitchTargetIndex(k)));
                        break;
                    case NormalizedOpCode.Ifeq:
                    case NormalizedOpCode.Ifne:
                    case NormalizedOpCode.Iflt:
                    case NormalizedOpCode.Ifge:
                    case NormalizedOpCode.Ifgt:
                    case NormalizedOpCode.Ifle:
                    case NormalizedOpCode.IfIcmpeq:
                    case NormalizedOpCode.IfIcmpne:
                    case NormalizedOpCode.IfIcmplt:
                    case NormalizedOpCode.IfIcmpge:
                    case NormalizedOpCode.IfIcmpgt:
                    case NormalizedOpCode.IfIcmple:
                    case NormalizedOpCode.IfAcmpeq:
                    case NormalizedOpCode.IfAcmpne:
                    case NormalizedOpCode.IfNull:
                    case NormalizedOpCode.IfNonNull:
                    case NormalizedOpCode.Goto:
                        branches.Add((i, instructions[i].Arg1));
                        break;
                }
            }

            var array = branches.ToArray();
            Array.Sort(array, (x, y) => x.Target.CompareTo(y.Target));
            return array;
        }

        /// <summary>
        /// Returns the ordered indexes at which the try block [<paramref name="start"/>, <paramref name="end"/>) must be
        /// split so that no part is entered by a branch from outside it and no part contains an exception handler other
        /// than at its start, or <c>null</c> if it needs no splitting.
        /// </summary>
        static List<int> FindTryBlockSplits(int start, int end, (int Source, int Target)[] branchesByTarget, HashSet<int> handlers)
        {
            List<int> splits = null;

            foreach (var handler in handlers)
                if (start < handler && handler < end)
                    AddTryBlockSplit(ref splits, handler);

            // the branches that target the inside of the block, excluding its start
            var first = FindFirstBranchWithTargetAfter(branchesByTarget, start);
            var last = first;
            while (last < branchesByTarget.Length && branchesByTarget[last].Target < end)
                last++;

            // a branch splits the block at its target when the source is in a different part, which can change when the
            // block is split elsewhere, so repeat until no more splits are found
            var changed = true;
            while (changed)
            {
                changed = false;
                for (int i = first; i < last; i++)
                {
                    var (source, target) = branchesByTarget[i];
                    if (splits != null && splits.BinarySearch(target) >= 0)
                        continue;

                    if (source < start || source >= end || HasTryBlockSplitBetween(splits, source, target))
                    {
                        AddTryBlockSplit(ref splits, target);
                        changed = true;
                    }
                }
            }

            return splits;
        }

        /// <summary>
        /// Returns the index of the first branch whose target is greater than <paramref name="index"/>.
        /// </summary>
        static int FindFirstBranchWithTargetAfter((int Source, int Target)[] branchesByTarget, int index)
        {
            int lo = 0;
            int hi = branchesByTarget.Length;
            while (lo < hi)
            {
                int mid = lo + (hi - lo) / 2;
                if (branchesByTarget[mid].Target > index)
                    hi = mid;
                else
                    lo = mid + 1;
            }

            return lo;
        }

        /// <summary>
        /// Adds a split point, keeping the list ordered.
        /// </summary>
        static void AddTryBlockSplit(ref List<int> splits, int index)
        {
            splits ??= new List<int>();
            var i = splits.BinarySearch(index);
            if (i < 0)
                splits.Insert(~i, index);
        }

        /// <summary>
        /// Returns whether the instructions at <paramref name="a"/> and <paramref name="b"/> are in different parts, that
        /// is, whether a part starts after the lower of the two and at or before the higher of the two.
        /// </summary>
        static bool HasTryBlockSplitBetween(List<int> splits, int a, int b)
        {
            if (splits == null)
                return false;

            var lo = Math.Min(a, b);
            var hi = Math.Max(a, b);
            var i = splits.BinarySearch(lo + 1);
            if (i < 0)
                i = ~i;

            return i < splits.Count && splits[i] <= hi;
        }

        /// <summary>
        /// Returns <c>true</c> if the given byte code is a return byte code.
        /// </summary>
        /// <param name="bc"></param>
        /// <returns></returns>
        static bool IsReturn(NormalizedOpCode bc)
        {
            return bc is
                NormalizedOpCode.Return or
                NormalizedOpCode.Areturn or
                NormalizedOpCode.Dreturn or
                NormalizedOpCode.Ireturn or
                NormalizedOpCode.Freturn or
                NormalizedOpCode.Lreturn;
        }

        static bool AnalyzePotentialFaultBlocks(CodeInfo codeInfo, Method method, UntangledExceptionTable exceptions)
        {
            var code = method.Instructions;
            var changed = false;
            var done = false;

            while (done == false)
            {
                done = true;
                var stack = new Stack<ExceptionTableEntry>();
                var current = new ExceptionTableEntry(0, code.Length, -1, new ClassConstantHandle(ushort.MaxValue), -1);
                stack.Push(current);

                for (int i = 0; i < exceptions.Length; i++)
                {
                    while (exceptions[i].StartIndex >= current.EndIndex)
                        current = stack.Pop();

                    Debug.Assert(exceptions[i].StartIndex >= current.StartIndex && exceptions[i].EndIndex <= current.EndIndex);
                    if (exceptions[i].CatchType.IsNil && codeInfo.HasState(exceptions[i].HandlerIndex) && codeInfo.GetRawStackType(exceptions[i].HandlerIndex, 0).IsFaultBlockException)
                    {
                        var flags = ComputePartialReachability(codeInfo, method.Instructions, exceptions, exceptions[i].HandlerIndex, true);
                        for (int j = 0; j < code.Length; j++)
                        {
                            if ((flags[j] & InstructionFlags.Reachable) != 0)
                            {
                                switch (code[j].NormalizedOpCode)
                                {
                                    case NormalizedOpCode.Return:
                                    case NormalizedOpCode.Areturn:
                                    case NormalizedOpCode.Ireturn:
                                    case NormalizedOpCode.Lreturn:
                                    case NormalizedOpCode.Freturn:
                                    case NormalizedOpCode.Dreturn:
                                        goto not_fault_block;
                                    case NormalizedOpCode.Athrow:
                                        for (int k = i + 1; k < exceptions.Length; k++)
                                            if (exceptions[k].StartIndex <= j && j < exceptions[k].EndIndex)
                                                goto not_fault_block;

                                        if (codeInfo.GetRawStackType(j, 0).IsFaultBlockException && codeInfo.GetRawStackType(j, 0) != codeInfo.GetRawStackType(exceptions[i].HandlerIndex, 0))
                                            goto not_fault_block;

                                        break;
                                }

                                if (j < current.StartIndex || j >= current.EndIndex)
                                    goto not_fault_block;
                                else if (exceptions[i].StartIndex <= j && j < exceptions[i].EndIndex)
                                    goto not_fault_block;
                                else
                                    continue;

                                not_fault_block:
                                codeInfo.ClearFaultBlockException(exceptions[i].HandlerIndex);
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

        static void ConvertFinallyBlocks(CodeInfo codeInfo, Method method, UntangledExceptionTable exceptions)
        {
            var code = method.Instructions;
            var flags = ComputePartialReachability(codeInfo, code, exceptions, 0, false);
            for (int i = 0; i < exceptions.Length; i++)
            {
                if (exceptions[i].CatchType.IsNil && codeInfo.HasState(exceptions[i].HandlerIndex) && codeInfo.GetRawStackType(exceptions[i].HandlerIndex, 0).IsFaultBlockException)
                {
                    if (IsSynchronizedBlockHandler(code, exceptions[i].HandlerIndex) &&
                        exceptions[i].EndIndex - 2 >= exceptions[i].StartIndex &&
                        TryFindSingleTryBlockExit(code, flags, exceptions, new ExceptionTableEntry(exceptions[i].StartIndex, exceptions[i].EndIndex - 2, exceptions[i].HandlerIndex, ClassConstantHandle.Nil, exceptions[i].Ordinal), i, out var exit) &&
                        exit == exceptions[i].EndIndex - 2 &&
                        (flags[exit + 1] & InstructionFlags.BranchTarget) == 0 &&
                        MatchInstructions(code, exit, exceptions[i].HandlerIndex + 1) &&
                        MatchInstructions(code, exit + 1, exceptions[i].HandlerIndex + 2) &&
                        MatchExceptionCoverage(exceptions, i, exceptions[i].HandlerIndex + 1, exceptions[i].HandlerIndex + 3, exit, exit + 2) &&
                        exceptions[i].HandlerIndex <= ushort.MaxValue)
                    {
                        code[exit].PatchOpCode(NormalizedOpCode.GotoFinally, exceptions[i].EndIndex, (short)exceptions[i].HandlerIndex);
                        exceptions.SetFinally(i);
                        continue;
                    }

                    if (TryFindSingleTryBlockExit(code, flags, exceptions, exceptions[i], i, out exit) &&
                        // the stack must be empty
                        codeInfo.GetStackHeight(exit) == 0 &&
                        // the exit code must not be reachable (except from within the try-block),
                        // because we're going to patch it to jump around the exit code
                        !IsReachableFromOutsideTryBlock(codeInfo, code, exceptions, exceptions[i], exit))
                    {
                        if (MatchFinallyBlock(codeInfo, code, exceptions, exceptions[i].HandlerIndex, exit, out var exitHandlerEnd, out var faultHandlerEnd))
                        {
                            if (exit != exitHandlerEnd &&
                                codeInfo.GetStackHeight(exitHandlerEnd) == 0 &&
                                MatchExceptionCoverage(exceptions, -1, exceptions[i].HandlerIndex, faultHandlerEnd, exit, exitHandlerEnd))
                            {
                                // We use Arg2 (which is a short) to store the handler in the __goto_finally pseudo-opcode,
                                // so we can only do that if handlerIndex fits in a short (note that we can use the sign bit too).
                                if (exceptions[i].HandlerIndex <= ushort.MaxValue)
                                {
                                    code[exit].PatchOpCode(NormalizedOpCode.GotoFinally, exitHandlerEnd, (short)exceptions[i].HandlerIndex);
                                    exceptions.SetFinally(i);
                                }
                            }
                        }

                        continue;
                    }
                }
            }
        }

        static bool IsSynchronizedBlockHandler(Instruction[] code, int index)
        {
            return
                code[index].NormalizedOpCode == NormalizedOpCode.Astore &&
                code[index + 1].NormalizedOpCode == NormalizedOpCode.Aload &&
                code[index + 2].NormalizedOpCode == NormalizedOpCode.MonitorExit &&
                code[index + 3].NormalizedOpCode == NormalizedOpCode.Aload &&
                code[index + 3].Arg1 == code[index].Arg1 &&
                code[index + 4].NormalizedOpCode == NormalizedOpCode.Athrow;
        }

        static bool MatchExceptionCoverage(UntangledExceptionTable exceptions, int skipException, int startFault, int endFault, int startExit, int endExit)
        {
            for (int j = 0; j < exceptions.Length; j++)
                if (j != skipException && ExceptionCovers(exceptions[j], startFault, endFault) != ExceptionCovers(exceptions[j], startExit, endExit))
                    return false;

            return true;
        }

        static bool ExceptionCovers(ExceptionTableEntry exception, int start, int end)
        {
            return exception.StartIndex < end && exception.EndIndex > start;
        }

        static bool MatchFinallyBlock(CodeInfo codeInfo, Instruction[] code, UntangledExceptionTable exceptions, int faultHandler, int exitHandler, out int exitHandlerEnd, out int faultHandlerEnd)
        {
            exitHandlerEnd = -1;
            faultHandlerEnd = -1;
            if (code[faultHandler].NormalizedOpCode != NormalizedOpCode.Astore)
                return false;

            int startFault = faultHandler;
            int faultLocal = code[faultHandler++].NormalizedArg1;
            for (; ; )
            {
                if (code[faultHandler].NormalizedOpCode == NormalizedOpCode.Aload &&
                    code[faultHandler].NormalizedArg1 == faultLocal &&
                    code[faultHandler + 1].NormalizedOpCode == NormalizedOpCode.Athrow)
                {
                    // make sure that instructions that we haven't covered aren't reachable
                    var flags = ComputePartialReachability(codeInfo, code, exceptions, startFault, false);
                    for (int i = 0; i < flags.Length; i++)
                        if ((i < startFault || i > faultHandler + 1) && (flags[i] & InstructionFlags.Reachable) != 0)
                            return false;

                    exitHandlerEnd = exitHandler;
                    faultHandlerEnd = faultHandler;
                    return true;
                }

                if (!MatchInstructions(code, faultHandler, exitHandler))
                    return false;

                faultHandler++;
                exitHandler++;
            }
        }

        static bool MatchInstructions(Instruction[] code, int i, int j)
        {
            if (code[i].NormalizedOpCode != code[j].NormalizedOpCode)
                return false;

            switch (OpCodeMetaData.GetFlowKind(code[i].NormalizedOpCode))
            {
                case OpCodeFlowControl.Branch:
                case OpCodeFlowControl.ConditionalBranch:
                    if (code[i].Arg1 - i != code[j].Arg1 - j)
                        return false;

                    break;
                case OpCodeFlowControl.Switch:
                    if (code[i].SwitchEntryCount != code[j].SwitchEntryCount)
                        return false;

                    for (int k = 0; k < code[i].SwitchEntryCount; k++)
                        if (code[i].GetSwitchTargetIndex(k) != code[j].GetSwitchTargetIndex(k))
                            return false;

                    if (code[i].DefaultTarget != code[j].DefaultTarget)
                        return false;

                    break;
                default:
                    if (code[i].Arg1 != code[j].Arg1)
                        return false;
                    if (code[i].Arg2 != code[j].Arg2)
                        return false;

                    break;
            }

            return true;
        }

        static bool IsReachableFromOutsideTryBlock(CodeInfo codeInfo, Instruction[] code, UntangledExceptionTable exceptions, ExceptionTableEntry tryBlock, int instructionIndex)
        {
            var flags = new InstructionFlags[code.Length];
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

        static bool TryFindSingleTryBlockExit(Instruction[] code, InstructionFlags[] flags, UntangledExceptionTable exceptions, ExceptionTableEntry exception, int exceptionIndex, out int exit)
        {
            exit = -1;
            var fail = false;
            var nextIsReachable = false;

            for (int i = exception.StartIndex; !fail && i < exception.EndIndex; i++)
            {
                if ((flags[i] & InstructionFlags.Reachable) != 0)
                {
                    nextIsReachable = false;
                    for (int j = 0; j < exceptions.Length; j++)
                        if (j != exceptionIndex && exceptions[j].StartIndex >= exception.StartIndex && exception.EndIndex <= exceptions[j].EndIndex)
                            UpdateTryBlockExit(exception, exceptions[j].HandlerIndex, ref exit, ref fail);

                    switch (OpCodeMetaData.GetFlowKind(code[i].NormalizedOpCode))
                    {
                        case OpCodeFlowControl.Switch:
                            {
                                for (int j = 0; j < code[i].SwitchEntryCount; j++)
                                    UpdateTryBlockExit(exception, code[i].GetSwitchTargetIndex(j), ref exit, ref fail);

                                UpdateTryBlockExit(exception, code[i].DefaultTarget, ref exit, ref fail);
                                break;
                            }
                        case OpCodeFlowControl.Branch:
                            UpdateTryBlockExit(exception, code[i].TargetIndex, ref exit, ref fail);
                            break;
                        case OpCodeFlowControl.ConditionalBranch:
                            UpdateTryBlockExit(exception, code[i].TargetIndex, ref exit, ref fail);
                            nextIsReachable = true;
                            break;
                        case OpCodeFlowControl.Return:
                            fail = true;
                            break;
                        case OpCodeFlowControl.Throw:
                            break;
                        case OpCodeFlowControl.Next:
                            nextIsReachable = true;
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                }
            }

            if (nextIsReachable)
                UpdateTryBlockExit(exception, exception.EndIndex, ref exit, ref fail);

            return !fail && exit != -1;
        }

        static void UpdateTryBlockExit(ExceptionTableEntry exception, int targetIndex, ref int exitIndex, ref bool fail)
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

        void ConditionalPatchNoClassDefFoundError(ref Instruction instruction, RuntimeJavaType tw)
        {
            var loader = _type.ClassLoader;
            if (loader.DisableDynamicBinding)
                SetHardError(loader, ref instruction, HardError.NoClassDefFoundError, "{0}", tw.Name);
        }

        void SetHardError(RuntimeClassLoader classLoader, ref Instruction instruction, HardError hardError, string message, params object[] args)
        {
            var text = string.Format(message, args);

            switch (hardError)
            {
                case HardError.NoClassDefFoundError:
                    classLoader.Diagnostics.EmittedNoClassDefFoundError(_classFile.Name + "." + _classFileMethod.Name + _classFileMethod.Signature, text);
                    break;
                case HardError.IllegalAccessError:
                    classLoader.Diagnostics.EmittedIllegalAccessError(_classFile.Name + "." + _classFileMethod.Name + _classFileMethod.Signature, text);
                    break;
                case HardError.InstantiationError:
                    classLoader.Diagnostics.EmittedInstantiationError(_classFile.Name + "." + _classFileMethod.Name + _classFileMethod.Signature, text);
                    break;
                case HardError.IncompatibleClassChangeError:
                    classLoader.Diagnostics.EmittedIncompatibleClassChangeError(_classFile.Name + "." + _classFileMethod.Name + _classFileMethod.Signature, text);
                    break;
                case HardError.IllegalAccessException:
                    classLoader.Diagnostics.EmittedIllegalAccessError(_classFile.Name + "." + _classFileMethod.Name + _classFileMethod.Signature, text);
                    break;
                case HardError.NoSuchFieldError:
                    classLoader.Diagnostics.EmittedNoSuchFieldError(_classFile.Name + "." + _classFileMethod.Name + _classFileMethod.Signature, text);
                    break;
                case HardError.AbstractMethodError:
                    classLoader.Diagnostics.EmittedAbstractMethodError(_classFile.Name + "." + _classFileMethod.Name + _classFileMethod.Signature, text);
                    break;
                case HardError.NoSuchMethodError:
                    classLoader.Diagnostics.EmittedNoSuchMethodError(_classFile.Name + "." + _classFileMethod.Name + _classFileMethod.Signature, text);
                    break;
                case HardError.LinkageError:
                    classLoader.Diagnostics.EmittedLinkageError(_classFile.Name + "." + _classFileMethod.Name + _classFileMethod.Signature, text);
                    break;
                default:
                    throw new InvalidOperationException();
            }

            instruction.SetHardError(hardError, AllocErrorMessage(text));
        }

        void PatchInvoke(RuntimeJavaType wrapper, ref Instruction instr, StackState stack)
        {
            var cpi = GetMethodref(instr.Arg1);
            var invoke = instr.NormalizedOpCode;
            var isnew = false;

            if (invoke == NormalizedOpCode.InvokeVirtual &&
                cpi is { Class: "java.lang.invoke.MethodHandle", Name: "invoke" or "invokeExact" or "invokeBasic" })
            {
                if (cpi.GetArgTypes().Length > 127 && _context.MethodHandleUtil.SlotCount(cpi.GetArgTypes()) > 254)
                {
                    instr.SetHardError(HardError.LinkageError, AllocErrorMessage("bad parameter count"));
                    return;
                }

                instr.PatchOpCode(NormalizedOpCode.MethodHandleInvoke);
                return;
            }

            if (invoke == NormalizedOpCode.InvokeStatic &&
                cpi is { Class: "java.lang.invoke.MethodHandle", Name: "linkToVirtual" or "linkToStatic" or "linkToSpecial" or "linkToInterface" } &&
                _context.JavaBase.TypeOfJavaLangInvokeMethodHandle.IsPackageAccessibleFrom(wrapper))
            {
                instr.PatchOpCode(NormalizedOpCode.MethodHandleLink);
                return;
            }

            RuntimeJavaType thisType;
            if (invoke == NormalizedOpCode.InvokeStatic)
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
                    isnew = type.IsUninitializedNew;
                }
            }

            if (cpi.GetClassType().IsUnloadable)
            {
                if (wrapper.ClassLoader.DisableDynamicBinding)
                {
                    SetHardError(wrapper.ClassLoader, ref instr, HardError.NoClassDefFoundError, "{0}", cpi.GetClassType().Name);
                }
                else
                {
                    switch (invoke)
                    {
                        case NormalizedOpCode.InvokeInterface:
                            instr.PatchOpCode(NormalizedOpCode.DynamicInvokeInterface);
                            break;
                        case NormalizedOpCode.InvokeStatic:
                            instr.PatchOpCode(NormalizedOpCode.DynamicInvokeStatic);
                            break;
                        case NormalizedOpCode.InvokeVirtual:
                            instr.PatchOpCode(NormalizedOpCode.DynamicInvokeVirtual);
                            break;
                        case NormalizedOpCode.InvokeSpecial:
                            if (isnew)
                                instr.PatchOpCode(NormalizedOpCode.DynamicInvokeSpecial);
                            else
                                throw new VerifyError("Invokespecial cannot call subclass methods");

                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                }
            }
            else if (invoke == NormalizedOpCode.InvokeInterface && !cpi.GetClassType().IsInterface)
            {
                SetHardError(wrapper.ClassLoader, ref instr, HardError.IncompatibleClassChangeError, "invokeinterface on non-interface");
            }
            else if (cpi.GetClassType().IsInterface && invoke != NormalizedOpCode.InvokeInterface && ((invoke != NormalizedOpCode.InvokeStatic && invoke != NormalizedOpCode.InvokeSpecial) || _classFile.MajorVersion < 52))
            {
                SetHardError(wrapper.ClassLoader, ref instr, HardError.IncompatibleClassChangeError,
                    _classFile.MajorVersion < 52
                        ? "interface method must be invoked using invokeinterface"
                        : "interface method must be invoked using invokeinterface, invokespecial or invokestatic");
            }
            else
            {
                var targetMethod = invoke == NormalizedOpCode.InvokeSpecial ? cpi.GetMethodForInvokespecial() : cpi.GetMethod();
                if (targetMethod != null)
                {
                    string errmsg = CheckLoaderConstraints(cpi, targetMethod);
                    if (errmsg != null)
                    {
                        SetHardError(wrapper.ClassLoader, ref instr, HardError.LinkageError, "{0}", errmsg);
                    }
                    else if (targetMethod.IsStatic == (invoke == NormalizedOpCode.InvokeStatic))
                    {
                        if (targetMethod.IsAbstract && invoke == NormalizedOpCode.InvokeSpecial && (targetMethod.GetMethod() == null || targetMethod.GetMethod().IsAbstract))
                        {
                            SetHardError(wrapper.ClassLoader, ref instr, HardError.AbstractMethodError, "{0}.{1}{2}", cpi.Class, cpi.Name, cpi.Signature);
                        }
                        else if (invoke == NormalizedOpCode.InvokeInterface && targetMethod.IsPrivate)
                        {
                            SetHardError(wrapper.ClassLoader, ref instr, HardError.IncompatibleClassChangeError, "private interface method requires invokespecial, not invokeinterface: method {0}.{1}{2}", cpi.Class, cpi.Name, cpi.Signature);
                        }
                        else if (targetMethod.IsAccessibleFrom(cpi.GetClassType(), wrapper, thisType))
                        {
                            return;
                        }
                        else if (_host != null && targetMethod.IsAccessibleFrom(cpi.GetClassType(), _host, thisType))
                        {
                            switch (invoke)
                            {
                                case NormalizedOpCode.InvokeSpecial:
                                    instr.PatchOpCode(NormalizedOpCode.PrivilegedInvokeSpecial);
                                    break;
                                case NormalizedOpCode.InvokeStatic:
                                    instr.PatchOpCode(NormalizedOpCode.PrivilegedInvokeStatic);
                                    break;
                                case NormalizedOpCode.InvokeVirtual:
                                    instr.PatchOpCode(NormalizedOpCode.PrivilegedInvokeVirtual);
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
                            if (cpi.GetClassType() == _context.JavaBase.TypeOfJavaLangObject && thisType.IsArray && ReferenceEquals(cpi.Name, StringConstants.CLONE))
                            {
                                // Patch the instruction, so that the compiler doesn't need to do this test again.
                                instr.PatchOpCode(NormalizedOpCode.CloneArray);
                                return;
                            }
                            SetHardError(wrapper.ClassLoader, ref instr, HardError.IllegalAccessError, "tried to access method {0}.{1}{2} from class {3}", ToSlash(targetMethod.DeclaringType.Name), cpi.Name, ToSlash(cpi.Signature), ToSlash(wrapper.Name));
                        }
                    }
                    else
                    {
                        SetHardError(wrapper.ClassLoader, ref instr, HardError.IncompatibleClassChangeError, "static call to non-static method (or v.v.)");
                    }
                }
                else
                {
                    SetHardError(wrapper.ClassLoader, ref instr, HardError.NoSuchMethodError, "{0}.{1}{2}", cpi.Class, cpi.Name, cpi.Signature);
                }
            }
        }

        static string ToSlash(string str)
        {
            return str.Replace('.', '/');
        }

        void PatchFieldAccess(RuntimeJavaType wrapper, RuntimeJavaMethod mw, ref Instruction instr, StackState stack)
        {
            var cpi = GetFieldref(instr.Arg1);
            bool isStatic;
            bool write;
            RuntimeJavaType thisType;
            switch (instr.NormalizedOpCode)
            {
                case NormalizedOpCode.GetField:
                    isStatic = false;
                    write = false;
                    thisType = SigTypeToClassName(stack.PopObjectType(GetFieldref(instr.Arg1).GetClassType()), cpi.GetClassType(), wrapper);
                    break;
                case NormalizedOpCode.PutField:
                    stack.PopType(GetFieldref(instr.Arg1).GetFieldType());
                    isStatic = false;
                    write = true;
                    // putfield is allowed to access the unintialized this
                    if (stack.PeekType().IsUninitializedThis && wrapper.IsAssignableTo(GetFieldref(instr.Arg1).GetClassType()))
                    {
                        thisType = wrapper;
                    }
                    else
                    {
                        thisType = SigTypeToClassName(stack.PopObjectType(GetFieldref(instr.Arg1).GetClassType()), cpi.GetClassType(), wrapper);
                    }
                    break;
                case NormalizedOpCode.GetStatic:
                    isStatic = true;
                    write = false;
                    thisType = null;
                    break;
                case NormalizedOpCode.PutStatic:
                    // special support for when we're being called from IsSideEffectFreeStaticInitializer
                    if (mw == null)
                    {
                        switch (GetFieldref(instr.Arg1).Signature[0])
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
                                if (!stack.PopAnyType().IsNull)
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
                        stack.PopType(GetFieldref(instr.Arg1).GetFieldType());
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
            else if (cpi.GetClassType().IsUnloadable)
            {
                if (wrapper.ClassLoader.DisableDynamicBinding)
                {
                    SetHardError(wrapper.ClassLoader, ref instr, HardError.NoClassDefFoundError, "{0}", cpi.GetClassType().Name);
                }
                else
                {
                    switch (instr.NormalizedOpCode)
                    {
                        case NormalizedOpCode.GetStatic:
                            instr.PatchOpCode(NormalizedOpCode.DynamicGetStatic);
                            break;
                        case NormalizedOpCode.PutStatic:
                            instr.PatchOpCode(NormalizedOpCode.DynamicPutStatic);
                            break;
                        case NormalizedOpCode.GetField:
                            instr.PatchOpCode(NormalizedOpCode.DynamicGetField);
                            break;
                        case NormalizedOpCode.PutField:
                            instr.PatchOpCode(NormalizedOpCode.DynamicPutField);
                            break;
                        default:
                            throw new InvalidOperationException();
                    }
                }
                return;
            }
            else
            {
                var field = cpi.GetField();
                if (field == null)
                {
                    SetHardError(wrapper.ClassLoader, ref instr, HardError.NoSuchFieldError, "{0}.{1}", cpi.Class, cpi.Name);
                    return;
                }
                if (false && cpi.GetFieldType() != field.FieldTypeWrapper && !cpi.GetFieldType().IsUnloadable & !field.FieldTypeWrapper.IsUnloadable)
                {
#if IMPORTER
                    StaticCompiler.LinkageError("Field \"{2}.{3}\" is of type \"{0}\" instead of type \"{1}\" as expected by \"{4}\"", field.FieldTypeWrapper, cpi.GetFieldType(), cpi.GetClassType().Name, cpi.Name, wrapper.Name);
#endif
                    SetHardError(wrapper.ClassLoader, ref instr, HardError.LinkageError, "Loader constraints violated: {0}.{1}", field.DeclaringType.Name, field.Name);
                    return;
                }
                if (field.IsStatic != isStatic)
                {
                    SetHardError(wrapper.ClassLoader, ref instr, HardError.IncompatibleClassChangeError, "Static field access to non-static field (or v.v.)");
                    return;
                }
                if (!field.IsAccessibleFrom(cpi.GetClassType(), wrapper, thisType))
                {
                    SetHardError(wrapper.ClassLoader, ref instr, HardError.IllegalAccessError, "Try to access field {0}.{1} from class {2}", field.DeclaringType.Name, field.Name, wrapper.Name);
                    return;
                }
                // are we trying to mutate a final field? (they are read-only from outside of the defining class)
                if (write && field.IsFinal
                    && ((isStatic ? wrapper != cpi.GetClassType() : wrapper != thisType) || (wrapper.ClassLoader.StrictFinalFieldSemantics && (isStatic ? (mw != null && mw.Name != "<clinit>") : (mw == null || mw.Name != "<init>")))))
                {
                    SetHardError(wrapper.ClassLoader, ref instr, HardError.IllegalAccessError, "Field {0}.{1} is final", field.DeclaringType.Name, field.Name);
                    return;
                }
            }
        }

        // TODO this method should have a better name
        RuntimeJavaType SigTypeToClassName(VerificationType type, RuntimeJavaType nullType, RuntimeJavaType wrapper)
        {
            if (type.IsUninitializedThis)
            {
                return wrapper;
            }
            else if (type.IsUninitializedNew)
            {
                return type.Type;
            }
            else if (type.IsNull)
            {
                return nullType;
            }
            else
            {
                return type.Type;
            }
        }

        int AllocErrorMessage(string message)
        {
            _errorMessages ??= new List<string>();
            int index = _errorMessages.Count;
            _errorMessages.Add(message);
            return index;
        }

        string CheckLoaderConstraints(ConstantPoolItemMI cpi, RuntimeJavaMethod mw)
        {
#if NETFRAMEWORK
            if (cpi.GetRetType() != mw.ReturnType && !cpi.GetRetType().IsUnloadable && !mw.ReturnType.IsUnloadable)
#else
            if (cpi.GetRetType() != mw.ReturnType && cpi.GetRetType().Name != mw.ReturnType.Name && !cpi.GetRetType().IsUnloadable && !mw.ReturnType.IsUnloadable)
#endif
            {
#if IMPORTER
                StaticCompiler.LinkageError("Method \"{2}.{3}{4}\" has a return type \"{0}\" instead of type \"{1}\" as expected by \"{5}\"", mw.ReturnType, cpi.GetRetType(), cpi.GetClassType().Name, cpi.Name, cpi.Signature, _classFile.Name);
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
                    StaticCompiler.LinkageError("Method \"{2}.{3}{4}\" has a argument type \"{0}\" instead of type \"{1}\" as expected by \"{5}\"", there[i], here[i], cpi.GetClassType().Name, cpi.Name, cpi.Signature, _classFile.Name);
#endif
                    return "Loader constraints violated (arg " + i + "): " + mw.DeclaringType.Name + "." + mw.Name + mw.Signature;
                }
            }

            return null;
        }

        ConstantPoolItemInvokeDynamic GetInvokeDynamic(int index)
        {
            try
            {
                var item = _classFile.GetInvokeDynamic(new InvokeDynamicConstantHandle(checked((ushort)index)));
                if (item != null)
                {
                    return item;
                }
            }
            catch (OverflowException)
            {
                // constant pool index out of range
            }
            catch (InvalidCastException)
            {
                // constant pool index not of proper type
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

        ConstantPoolItemMI GetMethodref(int index)
        {
            try
            {
                var item = _classFile.GetMethodref(new MethodrefConstantHandle(checked((ushort)index)));
                if (item != null)
                    return item;
            }
            catch (OverflowException)
            {
                // constant pool index out of range
            }
            catch (InvalidCastException)
            {
                // constant pool index not of proper type
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

        ConstantPoolItemFieldref GetFieldref(int index)
        {
            try
            {
                var item = _classFile.GetFieldref(new FieldrefConstantHandle(checked((ushort)index)));
                if (item != null)
                    return item;
            }
            catch (OverflowException)
            {
                // constant pool index out of range
            }
            catch (InvalidCastException)
            {
                // constant pool index not of proper type
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

        ConstantType GetConstantPoolConstantType(int slot)
        {
            try
            {
                return _classFile.GetConstantPoolConstantType(new ConstantHandle(ConstantKind.Unknown, checked((ushort)slot)));
            }
            catch (OverflowException)
            {
                // constant pool index out of range
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

        RuntimeJavaType GetConstantPoolClassType(int slot)
        {
            try
            {
                return _classFile.GetConstantPoolClassType(new ClassConstantHandle(checked((ushort)slot)));
            }
            catch (OverflowException)
            {
                // constant pool index out of range
            }
            catch (InvalidCastException)
            {
                // constant pool index out of range
            }
            catch (IndexOutOfRangeException)
            {
                // specified constant pool entry doesn't contain a constant
            }
            catch (NullReferenceException)
            {
                // specified constant pool entry is empty (entry 0 or the filler following a wide entry)
            }

            throw new VerifyError("Illegal constant pool index");
        }

        RuntimeJavaType GetConstantPoolClassType(ClassConstantHandle handle)
        {
            return GetConstantPoolClassType(handle.Slot);
        }

    }

}
