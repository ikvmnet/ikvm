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

using IKVM.ByteCode;
using IKVM.CoreLib.Linking;
using IKVM.CoreLib.Runtime;

namespace IKVM.CoreLib.Jvm
{

    /// <summary>
    /// Verifies the bytecode of a method by computing the verification state of every reachable instruction, and
    /// keeps that state for the code generator.
    /// </summary>
    /// <remarks>
    /// Construct an instance, then call <see cref="Verify"/>. A derived type may run further passes over the computed
    /// state, and may run the type flow analysis again with <see cref="AnalyzeTypeFlow"/> after changing the code.
    /// </remarks>
    internal class MethodVerifier<TTypeSystem, TType, TMember, TField, TMethod>
        where TTypeSystem : struct, IJvmTypeSystem<TType, TMember, TField, TMethod>
        where TType : class
        where TMember : class
        where TField : class, TMember
        where TMethod : class, TMember
    {

        protected readonly TTypeSystem _typeSystem;
        protected readonly TType? _host; // used by Unsafe.defineAnonymousClass() to provide access to private members of the host
        protected readonly TType _type;
        protected readonly ClassFile<TTypeSystem, TType, TMember, TField, TMethod> _classFile;
        protected readonly Method<TTypeSystem, TType, TMember, TField, TMethod> _classFileMethod;
        protected readonly VerificationType<TType> _thisType;
        protected readonly InstructionState<TTypeSystem, TType, TMember, TField, TMethod>[] _state;
        readonly bool _isMethodKnown;
        readonly TType? _returnType;
        readonly Dictionary<int, VerificationType<TType>> _newTypes = new Dictionary<int, VerificationType<TType>>();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="typeSystem"></param>
        /// <param name="host">The type whose private members the method may also access, or <c>null</c>.</param>
        /// <param name="type">The type that declares the method.</param>
        /// <param name="classFile"></param>
        /// <param name="classFileMethod"></param>
        /// <param name="argTypes">The argument types of the method, or <c>null</c> if the method is not known, in which case it is verified only as far as is possible without it.</param>
        /// <param name="returnType">The return type of the method, or <c>null</c> if the method is not known.</param>
        /// <exception cref="VerifyException"></exception>
        /// <exception cref="ClassFormatException"></exception>
        internal MethodVerifier(TTypeSystem typeSystem, TType? host, TType type, ClassFile<TTypeSystem, TType, TMember, TField, TMethod> classFile, Method<TTypeSystem, TType, TMember, TField, TMethod> classFileMethod, TType[]? argTypes, TType? returnType)
        {
            if (classFileMethod.VerifyError != null)
                throw new VerifyException(classFileMethod.VerifyError);

            _typeSystem = typeSystem;
            _host = host;
            _type = type ?? throw new ArgumentNullException(nameof(type));
            _classFile = classFile ?? throw new ArgumentNullException(nameof(classFile));
            _classFileMethod = classFileMethod;
            _isMethodKnown = returnType != null;
            _returnType = returnType;
            _state = new InstructionState<TTypeSystem, TType, TMember, TField, TMethod>[classFileMethod.Instructions.Length];

            // ensure that exception blocks and handlers start and end at instruction boundaries
            if (HasValidExceptionTable(classFileMethod) == false)
                throw new ClassFormatException($"Illegal exception table (class: {classFile.Name}, method: {classFileMethod.Name}, signature: {classFileMethod.Signature})");

            // start by computing the initial state, the stack is empty and the locals contain the arguments
            _state[0] = new InstructionState<TTypeSystem, TType, TMember, TField, TMethod>(typeSystem, _state, classFileMethod.MaxLocals, classFileMethod.MaxStack);
            int firstNonArgLocalIndex = 0;

            if (classFileMethod.IsStatic == false)
            {
                _thisType = VerificationType<TType>.This(type);

                // this reference. If we're a constructor, the this reference is uninitialized.
                if (classFileMethod.IsConstructor)
                {
                    _state[0].SetLocalType(firstNonArgLocalIndex++, VerificationType<TType>.UninitializedThis, -1);
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

            foreach (var argType in argTypes ?? [])
            {
                var argVerificationType = InstructionState<TTypeSystem, TType, TMember, TField, TMethod>.GetVerificationType(typeSystem, argType);
                _state[0].SetLocalType(firstNonArgLocalIndex++, argVerificationType, -1);
                if (argVerificationType.IsCategory2)
                    firstNonArgLocalIndex++;
            }
        }

        /// <summary>
        /// Returns whether every exception block and handler of the method starts and ends at an instruction boundary.
        /// </summary>
        /// <param name="method"></param>
        /// <returns></returns>
        internal static bool HasValidExceptionTable(Method<TTypeSystem, TType, TMember, TField, TMethod> method)
        {
            foreach (var entry in method.ExceptionTable)
                if (entry.StartIndex >= entry.EndIndex || entry.StartIndex == -1 || entry.EndIndex == -1 || entry.HandlerIndex <= 0)
                    return false;

            return true;
        }

        /// <summary>
        /// Verifies the method.
        /// </summary>
        /// <exception cref="VerifyException"></exception>
        internal void Verify()
        {
            AnalyzeTypeFlow();
            VerifyPassTwo();
            PatchLoadConstants();
        }

        /// <summary>
        /// Gets the verification state the analysis computed.
        /// </summary>
        /// <returns></returns>
        internal CodeInfo<TTypeSystem, TType, TMember, TField, TMethod> GetCodeInfo()
        {
            return new CodeInfo<TTypeSystem, TType, TMember, TField, TMethod>(_typeSystem, _state);
        }

        /// <summary>
        /// Returns the type the result of a call to an intrinsic method has, if it is more precise than its declared
        /// return type, or <c>null</c>.
        /// </summary>
        /// <param name="cpi">The method being invoked.</param>
        /// <param name="state">The state before the call, with the arguments on the stack.</param>
        /// <returns></returns>
        protected virtual TType? GetIntrinsicReturnType(ConstantPoolItemMI<TTypeSystem, TType, TMember, TField, TMethod> cpi, ref InstructionState<TTypeSystem, TType, TMember, TField, TMethod> state)
        {
            return null;
        }

        TType ArrayOf(TType elementType) => _typeSystem.MakeArrayType(elementType, 1);

        bool IsUnloadable(VerificationType<TType> type) => type.IsReference && _typeSystem.IsUnloadable(type.Type);

        bool IsNullOrUnloadable(VerificationType<TType> type) => type.IsNull || IsUnloadable(type);

        bool IsCategory2(TType type) => type == _typeSystem.TypeOfLong || type == _typeSystem.TypeOfDouble;

        bool IsIntOnStack(TType type) => type == _typeSystem.TypeOfInt || type == _typeSystem.TypeOfBoolean || type == _typeSystem.TypeOfByte || type == _typeSystem.TypeOfChar || type == _typeSystem.TypeOfShort;

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

        protected void AnalyzeTypeFlow()
        {
            var s = new InstructionState<TTypeSystem, TType, TMember, TField, TMethod>(_typeSystem, _state, _classFileMethod.MaxLocals, _classFileMethod.MaxStack);
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
                                            throw new VerifyException("Object reference expected");

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
                                            throw new VerifyException("Object reference expected");

                                        s.SetLocalType(inst.NormalizedArg1, type, i);
                                        break;
                                    }
                                case NormalizedOpCode.AconstNull:
                                    s.PushType(VerificationType<TType>.Null);
                                    break;
                                case NormalizedOpCode.Aaload:
                                    {
                                        s.PopInt();
                                        var type = s.PopArrayType();
                                        if (type.IsNull)
                                        {
                                            // if the array is null, we have use null as the element type, because
                                            // otherwise the rest of the code will not verify correctly
                                            s.PushType(VerificationType<TType>.Null);
                                        }
                                        else if (IsUnloadable(type))
                                        {
                                            s.PushType(_typeSystem.TypeOfVerifierUnloadable);
                                        }
                                        else
                                        {
                                            var elementType = _typeSystem.GetElementType(type.Type!);
                                            if (_typeSystem.IsPrimitive(elementType))
                                                throw new VerifyException("Object array expected");

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
                                        if (!IsNullOrUnloadable(type) && type.Type != ArrayOf(_typeSystem.TypeOfByte) && type.Type != ArrayOf(_typeSystem.TypeOfBoolean))
                                            throw new VerifyException();

                                        s.PushInt();
                                        break;
                                    }
                                case NormalizedOpCode.Bastore:
                                    {
                                        s.PopInt();
                                        s.PopInt();

                                        var type = s.PopArrayType();
                                        if (!IsNullOrUnloadable(type) && type.Type != ArrayOf(_typeSystem.TypeOfByte) && type.Type != ArrayOf(_typeSystem.TypeOfBoolean))
                                            throw new VerifyException();

                                        break;
                                    }
                                case NormalizedOpCode.Caload:
                                    s.PopInt();
                                    s.PopObjectType(ArrayOf(_typeSystem.TypeOfChar));
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode.Castore:
                                    s.PopInt();
                                    s.PopInt();
                                    s.PopObjectType(ArrayOf(_typeSystem.TypeOfChar));
                                    break;
                                case NormalizedOpCode.Saload:
                                    s.PopInt();
                                    s.PopObjectType(ArrayOf(_typeSystem.TypeOfShort));
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode.Sastore:
                                    s.PopInt();
                                    s.PopInt();
                                    s.PopObjectType(ArrayOf(_typeSystem.TypeOfShort));
                                    break;
                                case NormalizedOpCode.Iaload:
                                    s.PopInt();
                                    s.PopObjectType(ArrayOf(_typeSystem.TypeOfInt));
                                    s.PushInt();
                                    break;
                                case NormalizedOpCode.Iastore:
                                    s.PopInt();
                                    s.PopInt();
                                    s.PopObjectType(ArrayOf(_typeSystem.TypeOfInt));
                                    break;
                                case NormalizedOpCode.Laload:
                                    s.PopInt();
                                    s.PopObjectType(ArrayOf(_typeSystem.TypeOfLong));
                                    s.PushLong();
                                    break;
                                case NormalizedOpCode.Lastore:
                                    s.PopLong();
                                    s.PopInt();
                                    s.PopObjectType(ArrayOf(_typeSystem.TypeOfLong));
                                    break;
                                case NormalizedOpCode.Daload:
                                    s.PopInt();
                                    s.PopObjectType(ArrayOf(_typeSystem.TypeOfDouble));
                                    s.PushDouble();
                                    break;
                                case NormalizedOpCode.Dastore:
                                    s.PopDouble();
                                    s.PopInt();
                                    s.PopObjectType(ArrayOf(_typeSystem.TypeOfDouble));
                                    break;
                                case NormalizedOpCode.Faload:
                                    s.PopInt();
                                    s.PopObjectType(ArrayOf(_typeSystem.TypeOfFloat));
                                    s.PushFloat();
                                    break;
                                case NormalizedOpCode.Fastore:
                                    s.PopFloat();
                                    s.PopInt();
                                    s.PopObjectType(ArrayOf(_typeSystem.TypeOfFloat));
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
                                    if (!_isMethodKnown)
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
                                                throw new VerifyException();
                                            default:
                                                throw new InvalidOperationException();
                                        }
                                    }
                                    else
                                    {
                                        var cpi = GetFieldref(inst.Arg1);
                                        if (cpi.GetField() is { } field && _typeSystem.IsUnloadable(_typeSystem.GetFieldType(field)))
                                            s.PushType(_typeSystem.GetFieldType(field));
                                        else
                                            s.PushType(cpi.GetFieldType()!);
                                    }
                                    break;
                                case NormalizedOpCode.PutStatic:
                                case NormalizedOpCode.DynamicPutStatic:
                                    // special support for when we're being called from IsSideEffectFreeStaticInitializer
                                    if (!_isMethodKnown)
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
                                                    throw new VerifyException();
                                                break;
                                            default:
                                                throw new InvalidOperationException();
                                        }
                                    }
                                    else
                                    {
                                        s.PopType(GetFieldref(inst.Arg1).GetFieldType()!);
                                    }
                                    break;
                                case NormalizedOpCode.GetField:
                                case NormalizedOpCode.DynamicGetField:
                                    {
                                        s.PopObjectType(GetFieldref(inst.Arg1).GetClassType());

                                        var cpi = GetFieldref(inst.Arg1);
                                        if (cpi.GetField() is { } field && _typeSystem.IsUnloadable(_typeSystem.GetFieldType(field)))
                                            s.PushType(_typeSystem.GetFieldType(field));
                                        else
                                            s.PushType(cpi.GetFieldType()!);

                                        break;
                                    }
                                case NormalizedOpCode.PutField:
                                case NormalizedOpCode.DynamicPutField:
                                    s.PopType(GetFieldref(inst.Arg1).GetFieldType()!);

                                    // putfield is allowed to access the uninitialized this
                                    if (s.PeekType().IsUninitializedThis && _typeSystem.IsAssignableTo(_type, GetFieldref(inst.Arg1).GetClassType()))
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
                                                s.PushType(_typeSystem.TypeOfJavaLangString);
                                                break;
                                            case ConstantType.LiveObject:
                                                s.PushType(_typeSystem.TypeOfJavaLangObject);
                                                break;
                                            case ConstantType.Class:
                                                if (_classFile.MajorVersion < 49)
                                                    throw new VerifyException("Illegal type in constant pool");

                                                s.PushType(_typeSystem.TypeOfJavaLangClass);
                                                break;
                                            case ConstantType.MethodHandle:
                                                s.PushType(_typeSystem.TypeOfJavaLangInvokeMethodHandle);
                                                break;
                                            case ConstantType.MethodType:
                                                s.PushType(_typeSystem.TypeOfJavaLangInvokeMethodType);
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
                                        var retType = cpi.GetRetType()!;

                                        // the runtime may know a more precise return type for an intrinsic
                                        retType = GetIntrinsicReturnType(cpi, ref s) ?? retType;

                                        s.MultiPopAnyType(cpi.GetArgTypes()!.Length);

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

                                                    s.MarkInitialized(type, VerificationType<TType>.Reference(_type), i);
                                                    s.SetUnitializedThis(false);
                                                }
                                                else if (type.IsUninitializedNew)
                                                {
                                                    s.MarkInitialized(type, VerificationType<TType>.Reference(type.Type!), i);
                                                }
                                                else
                                                {
                                                    // This is a VerifyError, but it will be caught by our second pass
                                                }
                                            }
                                        }

                                        if (retType != _typeSystem.TypeOfVoid)
                                        {
                                            if (cpi.GetMethod() is { } method && _typeSystem.IsUnloadable(_typeSystem.GetReturnType(method)))
                                            {
                                                s.PushType(_typeSystem.GetReturnType(method));
                                            }
                                            else if (retType == _typeSystem.TypeOfDouble)
                                            {
                                                s.PushExtendedDouble();
                                            }
                                            else if (retType == _typeSystem.TypeOfFloat)
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
                                        s.MultiPopAnyType(cpi.GetArgTypes()!.Length);

                                        var retType = cpi.GetRetType()!;
                                        if (retType != _typeSystem.TypeOfVoid)
                                        {
                                            if (retType == _typeSystem.TypeOfDouble)
                                            {
                                                s.PushExtendedDouble();
                                            }
                                            else if (retType == _typeSystem.TypeOfFloat)
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
                                            if (_typeSystem.GetArrayRank(classType) > 0)
                                                throw new VerifyException("Illegal use of array type");

                                            type = VerificationType<TType>.Uninitialized(classType, i);
                                            _newTypes[i] = type;
                                        }

                                        s.PushType(type);
                                        break;
                                    }
                                case NormalizedOpCode.Multianewarray:
                                    {
                                        if (inst.Arg2 < 1)
                                            throw new VerifyException("Illegal dimension argument");

                                        for (int j = 0; j < inst.Arg2; j++)
                                            s.PopInt();

                                        var type = GetConstantPoolClassType(inst.Arg1);
                                        if (_typeSystem.GetArrayRank(type) < inst.Arg2)
                                            throw new VerifyException("Illegal dimension argument");

                                        s.PushType(type);
                                        break;
                                    }
                                case NormalizedOpCode.Anewarray:
                                    {
                                        s.PopInt();
                                        var type = GetConstantPoolClassType(inst.Arg1);
                                        if (_typeSystem.IsUnloadable(type))
                                            s.PushType(_typeSystem.CreateUnloadableType("[" + _typeSystem.GetSignatureName(type)));
                                        else
                                            s.PushType(_typeSystem.MakeArrayType(type, 1));

                                        break;
                                    }
                                case NormalizedOpCode.Newarray:
                                    s.PopInt();
                                    switch (inst.Arg1)
                                    {
                                        case 4:
                                            s.PushType(ArrayOf(_typeSystem.TypeOfBoolean));
                                            break;
                                        case 5:
                                            s.PushType(ArrayOf(_typeSystem.TypeOfChar));
                                            break;
                                        case 6:
                                            s.PushType(ArrayOf(_typeSystem.TypeOfFloat));
                                            break;
                                        case 7:
                                            s.PushType(ArrayOf(_typeSystem.TypeOfDouble));
                                            break;
                                        case 8:
                                            s.PushType(ArrayOf(_typeSystem.TypeOfByte));
                                            break;
                                        case 9:
                                            s.PushType(ArrayOf(_typeSystem.TypeOfShort));
                                            break;
                                        case 10:
                                            s.PushType(ArrayOf(_typeSystem.TypeOfInt));
                                            break;
                                        case 11:
                                            s.PushType(ArrayOf(_typeSystem.TypeOfLong));
                                            break;
                                        default:
                                            throw new VerifyException("Bad type");
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
                                    if (_isMethodKnown)
                                    {
                                        if (_returnType != _typeSystem.TypeOfVoid)
                                            throw new VerifyException("Wrong return type in function");

                                        // if we're a constructor, make sure we called the base class constructor
                                        s.CheckUninitializedThis();
                                    }
                                    break;
                                case NormalizedOpCode.Areturn:
                                    if (_isMethodKnown)
                                    {
                                        // null is assignable to any type, so check for a primitive return type explicitly
                                        if (_typeSystem.IsPrimitive(_returnType!))
                                            throw new VerifyException("Wrong return type in function");

                                        s.PopObjectType(_returnType!);
                                    }
                                    else
                                    {
                                        s.PopObjectType();
                                    }

                                    break;
                                case NormalizedOpCode.Ireturn:
                                    {
                                        s.PopInt();
                                        if (!IsIntOnStack(_returnType!))
                                            throw new VerifyException("Wrong return type in function");

                                        break;
                                    }
                                case NormalizedOpCode.Lreturn:
                                    s.PopLong();
                                    if (_returnType != _typeSystem.TypeOfLong)
                                        throw new VerifyException("Wrong return type in function");

                                    break;
                                case NormalizedOpCode.Freturn:
                                    s.PopFloat();
                                    if (_returnType != _typeSystem.TypeOfFloat)
                                        throw new VerifyException("Wrong return type in function");

                                    break;
                                case NormalizedOpCode.Dreturn:
                                    s.PopDouble();
                                    if (_returnType != _typeSystem.TypeOfDouble)
                                        throw new VerifyException("Wrong return type in function");

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
                                        s.PopObjectType(_typeSystem.TypeOfJavaLangThrowable);
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
                                        throw new VerifyException("Falling off the end of the code");
                                    break;
                                case NormalizedOpCode.StaticError:
                                    break;
                                case NormalizedOpCode.Jsr:
                                case NormalizedOpCode.Ret:
                                    throw new VerifyException("Bad instruction");
                                default:
                                    throw new NotImplementedException(inst.NormalizedOpCode.ToString());
                            }

                            if (s.GetStackHeight() > _classFileMethod.MaxStack)
                                throw new VerifyException("Stack size too large");

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
                                throw new VerifyException("Illegal target of jump or branch");
                            }
                        }

                        catch (VerifyException x)
                        {
                            var opcode = instructions[i].NormalizedOpCode.ToString();
                            if (opcode.StartsWith("__"))
                                opcode = opcode.Substring(2);

                            throw new VerifyException($"{x.Message} (class: {_classFile.Name}, method: {_classFileMethod.Name}, signature: {_classFileMethod.Signature}, offset: {instructions[i].PC}, instruction: {opcode})", x);
                        }
                    }
                }
            }
        }

        void MergeExceptionHandler(int exceptionIndex, ref InstructionState<TTypeSystem, TType, TMember, TField, TMethod> curr)
        {
            var idx = _classFileMethod.ExceptionTable[exceptionIndex].HandlerIndex;
            var exp = curr.CopyLocals();

            var catchType = _classFileMethod.ExceptionTable[exceptionIndex].CatchType;
            if (catchType.IsNil)
            {
                exp.PushType(VerificationType<TType>.FaultBlockException(_typeSystem.TypeOfJavaLangThrowable, idx));
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
                    catch (VerifyException x)
                    {
                        var opcode = instructions[i].NormalizedOpCode.ToString();
                        if (opcode.StartsWith("__"))
                            opcode = opcode.Substring(2);

                        throw new VerifyException($"{x.Message} (class: {_classFile.Name}, method: {_classFileMethod.Name}, signature: {_classFileMethod.Signature}, offset: {instructions[i].PC}, instruction: {opcode})", x);
                    }
                }
            }
        }

        void VerifyInvokePassTwo(int index)
        {
            var stack = new StackState<TTypeSystem, TType, TMember, TField, TMethod>(_state, index);
            var invoke = _classFileMethod.Instructions[index].NormalizedOpCode;
            var cpi = GetMethodref(_classFileMethod.Instructions[index].Arg1);
            if ((invoke == NormalizedOpCode.InvokeStatic || invoke == NormalizedOpCode.InvokeSpecial) && _classFile.MajorVersion >= 52)
            {
                // invokestatic and invokespecial may be used to invoke interface methods in Java 8
                // but invokespecial can only invoke methods in the current interface or a directly implemented interface
                if (invoke == NormalizedOpCode.InvokeSpecial && cpi is ConstantPoolItemInterfaceMethodref<TTypeSystem, TType, TMember, TField, TMethod>)
                {
                    if (cpi.GetClassType() == _host)
                    {
                        // ok
                    }
                    else if (cpi.GetClassType() != _type && Array.IndexOf(_typeSystem.GetInterfaces(_type), cpi.GetClassType()) == -1)
                    {
                        throw new VerifyException("Bad invokespecial instruction: interface method reference is in an indirect superinterface.");
                    }
                }
            }
            else if ((cpi is ConstantPoolItemInterfaceMethodref<TTypeSystem, TType, TMember, TField, TMethod>) != (invoke == NormalizedOpCode.InvokeInterface))
            {
                throw new VerifyException("Illegal constant pool index");
            }

            if (invoke != NormalizedOpCode.InvokeSpecial && ReferenceEquals(cpi.Name, StringConstants.INIT))
                throw new VerifyException("Must call initializers using invokespecial");

            if (ReferenceEquals(cpi.Name, StringConstants.CLINIT))
                throw new VerifyException("Illegal call to internal method");

            var args = cpi.GetArgTypes()!;
            for (int j = args.Length - 1; j >= 0; j--)
                stack.PopType(args[j]);

            if (invoke == NormalizedOpCode.InvokeInterface)
            {
                int argcount = args.Length + 1;
                for (int j = 0; j < args.Length; j++)
                    if (IsCategory2(args[j]))
                        argcount++;

                if (_classFileMethod.Instructions[index].Arg2 != argcount)
                    throw new VerifyException("Inconsistent args size");
            }

            if (invoke != NormalizedOpCode.InvokeStatic)
            {
                if (ReferenceEquals(cpi.Name, StringConstants.INIT))
                {
                    var type = stack.PopType();
                    var isnew = type.IsUninitializedNew;
                    if ((isnew && type.Type != cpi.GetClassType()) || (type.IsUninitializedThis && cpi.GetClassType() != _typeSystem.GetBaseType(_type) && cpi.GetClassType() != _type) || (!isnew && !type.IsUninitializedThis))
                    {
                        // TODO oddly enough, Java fails verification for the class without
                        // even running the constructor, so maybe constructors are always
                        // verified...
                        // NOTE when a constructor isn't verifiable, the static initializer
                        // doesn't run either
                        throw new VerifyException("Call to wrong initialization method");
                    }
                }
                else
                {
                    if (invoke != NormalizedOpCode.InvokeInterface)
                    {
                        var refType = stack.PopObjectType();
                        var targetType = cpi.GetClassType();

                        if (!IsNullOrUnloadable(refType) && !_typeSystem.IsUnloadable(targetType) && !_typeSystem.IsAssignableTo(refType.Type!, targetType))
                            throw new VerifyException("Incompatible object argument for function call");

                        // for invokespecial we also need to make sure we're calling ourself or a base class
                        if (invoke == NormalizedOpCode.InvokeSpecial)
                        {
                            if (IsNullOrUnloadable(refType))
                            {
                                // ok
                            }
                            else if (_typeSystem.IsSubTypeOf(refType.Type!, _type))
                            {
                                // ok
                            }
                            else if (_host != null && _typeSystem.IsSubTypeOf(refType.Type!, _host))
                            {
                                // ok
                            }
                            else
                            {
                                throw new VerifyException("Incompatible target object for invokespecial");
                            }
                            if (_typeSystem.IsUnloadable(targetType))
                            {
                                // ok
                            }
                            else if (_typeSystem.IsSubTypeOf(_type, targetType))
                            {
                                // ok
                            }
                            else if (_host != null && _typeSystem.IsSubTypeOf(_host, targetType))
                            {
                                // ok
                            }
                            else
                            {
                                throw new VerifyException("Invokespecial cannot call subclass methods");
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
                        if (!IsNullOrUnloadable(refType) && !_typeSystem.IsUnloadable(targetType) && !_typeSystem.IsAssignableTo(refType.Type!, targetType) && !_typeSystem.IsInterface(targetType))
                            throw new VerifyException("Incompatible object argument for function call");
                    }
                }
            }
        }

        void VerifyInvokeDynamic(int index)
        {
            var stack = new StackState<TTypeSystem, TType, TMember, TField, TMethod>(_state, index);
            var cpi = GetInvokeDynamic(_classFileMethod.Instructions[index].Arg1);
            var args = cpi.GetArgTypes()!;
            for (int j = args.Length - 1; j >= 0; j--)
                stack.PopType(args[j]);
        }

        protected ConstantPoolItemInvokeDynamic<TTypeSystem, TType, TMember, TField, TMethod> GetInvokeDynamic(int index)
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

            throw new VerifyException("Illegal constant pool index");
        }

        protected ConstantPoolItemMI<TTypeSystem, TType, TMember, TField, TMethod> GetMethodref(int index)
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

            throw new VerifyException("Illegal constant pool index");
        }

        protected ConstantPoolItemFieldref<TTypeSystem, TType, TMember, TField, TMethod> GetFieldref(int index)
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

            throw new VerifyException("Illegal constant pool index");
        }

        protected ConstantType GetConstantPoolConstantType(int slot)
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

            throw new VerifyException("Illegal constant pool index");
        }

        protected TType GetConstantPoolClassType(int slot)
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

            throw new VerifyException("Illegal constant pool index");
        }

        protected TType GetConstantPoolClassType(ClassConstantHandle handle)
        {
            return GetConstantPoolClassType(handle.Slot);
        }

    }

}
