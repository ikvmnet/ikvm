// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// This file was forked from the .NET RuntimeILGenerator implementation as of 1/24/2024, and updated to emit debug
// symbols as it used to on .NET Framework.

#nullable enable

using System;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Diagnostics.SymbolStore;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace IKVM.Reflection.Emit
{
    internal class ILGenerator
    {

        #region Const Members
        private const int DefaultSize = 16;
        private const int DefaultFixupArraySize = 8;
        private const int DefaultLabelArraySize = 4;
        private const int DefaultExceptionArraySize = 2;
        #endregion

        #region Internal Statics
        internal static T[] EnlargeArray<T>(T[] incoming)
        {
            return EnlargeArray(incoming, incoming.Length * 2);
        }

        internal static T[] EnlargeArray<T>(T[] incoming, int requiredSize)
        {
            Debug.Assert(incoming != null);

            T[] temp = new T[requiredSize];
            Array.Copy(incoming, temp, incoming.Length);
            return temp;
        }

        #endregion

        #region Internal Data Members
        private int _length;
        private byte[] _ilStream;

        private __LabelInfo[]? _labelList;
        private int _labelCount;

        private __FixupData[]? _fixupData;

        private int _fixupCount;

        private int[]? _relocFixupList;
        private int _relocFixupCount;

        private int _exceptionCount;
        private int _currExcStackCount;
        private __ExceptionInfo[]? _exceptions;           // This is the list of all of the exceptions in this ILStream.
        private __ExceptionInfo[]? _currExcStack;         // This is the stack of exceptions which we're currently in.

        internal ScopeTree ScopeTree;                 // this variable tracks all debugging scope information
        internal LineNumberInfo LineNumberInfo;       // this variable tracks all line number information

        MethodBuilder _methodBuilder;
        internal int LocalCount;
        internal SignatureHelper LocalSignature;

        private int _curDepth; // Current stack depth, with -1 meaning unknown.
        private int _targetDepth; // Stack depth at a target of the previous instruction (when it is branching).
        private int _maxDepth; // Running max of the stack depth.

        // Adjustment to add to m_maxDepth for incorrect/invalid IL. For example, when branch instructions
        // with different stack depths target the same label.
        private long _depthAdjustment;

#endregion
#region Constructor

        internal ILGenerator(MethodBuilder methodBuilder, int size)
        {
            Debug.Assert(methodBuilder != null);
            Debug.Assert(methodBuilder is MethodBuilder);

            _ilStream = new byte[Math.Max(size, DefaultSize)];

            // initialize the scope tree
            ScopeTree = new ScopeTree();
            LineNumberInfo = new LineNumberInfo();
            _methodBuilder = methodBuilder;

            // initialize local signature
            LocalSignature = SignatureHelper.GetLocalVarSigHelper(methodBuilder.ModuleBuilder);
        }

        #endregion

        #region Internal Members
        internal virtual void RecordTokenFixup()
        {
            if (_relocFixupList == null)
            {
                _relocFixupList = new int[DefaultFixupArraySize];
            }
            else if (_relocFixupList.Length <= _relocFixupCount)
            {
                _relocFixupList = EnlargeArray(_relocFixupList);
            }

            _relocFixupList[_relocFixupCount++] = _length;
        }

        internal void InternalEmit(OpCode opcode)
        {
            short opcodeValue = opcode.Value;
            if (opcode.Size != 1)
            {
                BinaryPrimitives.WriteInt16BigEndian(_ilStream.AsSpan(_length), opcodeValue);
                _length += 2;
            }
            else
            {
                _ilStream[_length++] = (byte)opcodeValue;
            }

            UpdateStackSize(opcode, opcode.StackChange());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void UpdateStackSize(OpCode opcode, int stackchange)
        {
            // Updates internal variables for keeping track of the stack size
            // requirements for the function.  stackchange specifies the amount
            // by which the stacksize needs to be updated.

            if (_curDepth < 0)
            {
                // Current depth is "unknown". We get here when:
                // * this is unreachable code.
                // * the client uses explicit numeric offsets rather than Labels.
                _curDepth = 0;
            }

            _curDepth += stackchange;
            if (_curDepth < 0)
            {
                // Stack underflow. Assume our previous depth computation was flawed.
                _depthAdjustment -= _curDepth;
                _curDepth = 0;
            }
            else if (_maxDepth < _curDepth)
                _maxDepth = _curDepth;
            Debug.Assert(_depthAdjustment >= 0);
            Debug.Assert(_curDepth >= 0);

            // Record the stack depth at a "target" of this instruction.
            _targetDepth = _curDepth;

            // If the current instruction can't fall through, set the depth to unknown.
            if (opcode.EndsUncondJmpBlk())
                _curDepth = -1;
        }

        private int GetMethodToken(MethodBase method, Type[]? optionalParameterTypes, bool useMethodDef)
        {
            var mi = method is ConstructorInfo ctor ? ctor.GetMethodInfo() : (MethodInfo)method;
            if (optionalParameterTypes == null || optionalParameterTypes.Length == 0)
                return _methodBuilder.ModuleBuilder.GetMethodTokenForIL(mi).Token;
            else
                return _methodBuilder.ModuleBuilder.__GetMethodToken(mi, optionalParameterTypes, null).Token;
        }

        internal SignatureHelper GetMemberRefSignature(
            CallingConventions call,
            Type? returnType,
            Type[]? parameterTypes,
            Type[]? optionalParameterTypes)
        {
            var sig = SignatureHelper.GetMethodSigHelper(_methodBuilder.Module, call, returnType);
            sig.AddArguments(parameterTypes, null, null);
            if (optionalParameterTypes != null && optionalParameterTypes.Length != 0)
            {
                sig.AddSentinel();
                sig.AddArguments(optionalParameterTypes, null, null);
            }

            return sig;
        }

        internal byte[]? BakeByteArray()
        {
            // BakeByteArray is an internal function designed to be called by MethodBuilder to do
            // all of the fixups and return a new byte array representing the byte stream with labels resolved, etc.

            if (_currExcStackCount != 0)
            {
                throw new ArgumentException("The IL Generator cannot be used while there are unclosed exceptions.");
            }

            if (_length == 0)
                return null;

            // Allocate space for the new array.
            byte[] newBytes = new byte[_length];

            // Copy the data from the old array
            Array.Copy(_ilStream, newBytes, _length);

            // Do the fixups.
            // This involves iterating over all of the labels and
            // replacing them with their proper values.
            for (int i = 0; i < _fixupCount; i++)
            {
                __FixupData fixupData = _fixupData![i];
                int updateAddr = GetLabelPos(fixupData.FixupLabel) - (fixupData.FixupPos + fixupData.FixupInstSize);

                // Handle single byte instructions
                // Throw an exception if they're trying to store a jump in a single byte instruction that doesn't fit.
                if (fixupData.FixupInstSize == 1)
                {
                    // Verify that our one-byte arg will fit into a Signed Byte.
                    if (updateAddr < sbyte.MinValue || updateAddr > sbyte.MaxValue)
                    {
                        throw new NotSupportedException(string.Format("Illegal one-byte branch at position: {0}. Requested branch was: {1}.", fixupData.FixupPos, updateAddr));
                    }

                    // Place the one-byte arg
                    newBytes[fixupData.FixupPos] = (byte)updateAddr;
                }
                else
                {
                    // Place the four-byte arg
                    BinaryPrimitives.WriteInt32LittleEndian(newBytes.AsSpan(fixupData.FixupPos), updateAddr);
                }
            }
            return newBytes;
        }

        internal __ExceptionInfo[]? GetExceptions()
        {
            if (_currExcStackCount != 0)
            {
                throw new NotSupportedException("The IL Generator cannot be used while there are unclosed exceptions.");
            }

            if (_exceptionCount == 0)
            {
                return null;
            }

            var temp = new __ExceptionInfo[_exceptionCount];
            Array.Copy(_exceptions!, temp, _exceptionCount);
            SortExceptions(temp);
            return temp;
        }

        internal void EnsureCapacity(int size)
        {
            // Guarantees an array capable of holding at least size elements.
            if (_length + size >= _ilStream.Length)
            {
                IncreaseCapacity(size);
            }
        }

        private void IncreaseCapacity(int size)
        {
            byte[] temp = new byte[Math.Max(_ilStream.Length * 2, _length + size)];
            Array.Copy(_ilStream, temp, _ilStream.Length);
            _ilStream = temp;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void PutInteger4(int value)
        {
            BinaryPrimitives.WriteInt32LittleEndian(_ilStream.AsSpan(_length), value);
            _length += 4;
        }

        private int GetLabelPos(Label lbl)
        {
            // Gets the position in the stream of a particular label.
            // Verifies that the label exists and that it has been given a value.

            int index = lbl.Id;

            if (index < 0 || index >= _labelCount || _labelList is null)
                throw new ArgumentException("Bad label in ILGenerator.");

            int pos = _labelList[index].Pos;
            if (pos < 0)
                throw new ArgumentException("Bad label content in ILGenerator.");

            return pos;
        }

        private void AddFixup(Label lbl, int pos, int instSize)
        {
            // Notes the label, position, and instruction size of a new fixup.  Expands
            // all of the fixup arrays as appropriate.

            if (_fixupData == null)
            {
                _fixupData = new __FixupData[DefaultFixupArraySize];
            }
            else if (_fixupData.Length <= _fixupCount)
            {
                _fixupData = EnlargeArray(_fixupData);
            }

            _fixupData[_fixupCount++] = new __FixupData
            {
                FixupPos = pos,
                FixupLabel = lbl,
                FixupInstSize = instSize
            };

            int labelIndex = lbl.Id;
            if (labelIndex < 0 || labelIndex >= _labelCount || _labelList is null)
                throw new ArgumentException("Bad label in ILGenerator.");

            int depth = _labelList[labelIndex].Depth;
            int targetDepth = _targetDepth;
            Debug.Assert(depth >= -1);
            Debug.Assert(targetDepth >= -1);
            if (depth < targetDepth)
            {
                // Either unknown depth for this label or this branch location has a larger depth than previously recorded.
                // In the latter case, the IL is (likely) invalid, but we just compensate for it.
                if (depth >= 0)
                    _depthAdjustment += targetDepth - depth;
                _labelList[labelIndex].Depth = targetDepth;
            }
        }

        internal int GetMaxStackSize()
        {
            // Limit the computed max stack to 2^16 - 1, since the value is mod`ed by 2^16 by other code.
            Debug.Assert(_depthAdjustment >= 0);
            return (int)Math.Min(ushort.MaxValue, _maxDepth + _depthAdjustment);
        }

        private static void SortExceptions(__ExceptionInfo[] exceptions)
        {
            // In order to call exceptions properly we have to sort them in ascending order by their end position.
            // Just a cheap insertion sort.  We don't expect many exceptions (<10), where InsertionSort beats QuickSort.
            // If we have more exceptions than this in real life, we should consider moving to a QuickSort.

            for (int i = 0; i < exceptions.Length; i++)
            {
                int least = i;
                for (int j = i + 1; j < exceptions.Length; j++)
                {
                    if (exceptions[least].IsInner(exceptions[j]))
                    {
                        least = j;
                    }
                }
                __ExceptionInfo temp = exceptions[i];
                exceptions[i] = exceptions[least];
                exceptions[least] = temp;
            }
        }

        internal int[]? GetTokenFixups()
        {
            if (_relocFixupCount == 0)
            {
                Debug.Assert(_relocFixupList == null);
                return null;
            }

            int[] narrowTokens = new int[_relocFixupCount];
            Array.Copy(_relocFixupList!, narrowTokens, _relocFixupCount);
            return narrowTokens;
        }
        #endregion

        #region Public Members

        #region Emit
        public void Emit(OpCode opcode)
        {
            EnsureCapacity(3);
            InternalEmit(opcode);
        }

        public void Emit(OpCode opcode, byte arg)
        {
            EnsureCapacity(4);
            InternalEmit(opcode);
            _ilStream[_length++] = arg;
        }

        public void Emit(OpCode opcode, short arg)
        {
            // Puts opcode onto the stream of instructions followed by arg
            EnsureCapacity(5);
            InternalEmit(opcode);
            BinaryPrimitives.WriteInt16LittleEndian(_ilStream.AsSpan(_length), arg);
            _length += 2;
        }

        public void Emit(OpCode opcode, int arg)
        {
            // Special-case several opcodes that have shorter variants for common values.
            if (opcode.Equals(OpCodes.Ldc_I4))
            {
                if (arg >= -1 && arg <= 8)
                {
                    opcode = arg switch
                    {
                        -1 => OpCodes.Ldc_I4_M1,
                        0 => OpCodes.Ldc_I4_0,
                        1 => OpCodes.Ldc_I4_1,
                        2 => OpCodes.Ldc_I4_2,
                        3 => OpCodes.Ldc_I4_3,
                        4 => OpCodes.Ldc_I4_4,
                        5 => OpCodes.Ldc_I4_5,
                        6 => OpCodes.Ldc_I4_6,
                        7 => OpCodes.Ldc_I4_7,
                        _ => OpCodes.Ldc_I4_8,
                    };
                    Emit(opcode);
                    return;
                }

                if (arg >= -128 && arg <= 127)
                {
                    Emit(OpCodes.Ldc_I4_S, (sbyte)arg);
                    return;
                }
            }
            else if (opcode.Equals(OpCodes.Ldarg))
            {
                if ((uint)arg <= 3)
                {
                    Emit(arg switch
                    {
                        0 => OpCodes.Ldarg_0,
                        1 => OpCodes.Ldarg_1,
                        2 => OpCodes.Ldarg_2,
                        _ => OpCodes.Ldarg_3,
                    });
                    return;
                }

                if ((uint)arg <= byte.MaxValue)
                {
                    Emit(OpCodes.Ldarg_S, (byte)arg);
                    return;
                }

                if ((uint)arg <= ushort.MaxValue) // this will be true except on misuse of the opcode
                {
                    Emit(OpCodes.Ldarg, (short)arg);
                    return;
                }
            }
            else if (opcode.Equals(OpCodes.Ldarga))
            {
                if ((uint)arg <= byte.MaxValue)
                {
                    Emit(OpCodes.Ldarga_S, (byte)arg);
                    return;
                }

                if ((uint)arg <= ushort.MaxValue) // this will be true except on misuse of the opcode
                {
                    Emit(OpCodes.Ldarga, (short)arg);
                    return;
                }
            }
            else if (opcode.Equals(OpCodes.Starg))
            {
                if ((uint)arg <= byte.MaxValue)
                {
                    Emit(OpCodes.Starg_S, (byte)arg);
                    return;
                }

                if ((uint)arg <= ushort.MaxValue) // this will be true except on misuse of the opcode
                {
                    Emit(OpCodes.Starg, (short)arg);
                    return;
                }
            }

            // For everything else, put the opcode followed by the arg onto the stream of instructions.
            EnsureCapacity(7);
            InternalEmit(opcode);
            PutInteger4(arg);
        }

        public void Emit(OpCode opcode, MethodInfo meth)
        {
            if (meth is null)
                throw new ArgumentNullException(nameof(meth));

            if (opcode.Equals(OpCodes.Call) || opcode.Equals(OpCodes.Callvirt) || opcode.Equals(OpCodes.Newobj))
            {
                EmitCall(opcode, meth, null);
            }
            else
            {
                // Reflection doesn't distinguish between these two concepts:
                //   1. A generic method definition: Foo`1
                //   2. A generic method definition instantiated over its own generic arguments: Foo`1<!!0>
                // In RefEmit, we always want 1 for Ld* opcodes and 2 for Call* and Newobj.
                bool useMethodDef = opcode.Equals(OpCodes.Ldtoken) || opcode.Equals(OpCodes.Ldftn) || opcode.Equals(OpCodes.Ldvirtftn);
                int tk = GetMethodToken(meth, null, useMethodDef);

                EnsureCapacity(7);
                InternalEmit(opcode);

                UpdateStackSize(opcode, 0);
                RecordTokenFixup();
                PutInteger4(tk);
            }
        }

        public void EmitCalli(OpCode opcode, CallingConventions callingConvention,
            Type? returnType, Type[]? parameterTypes, Type[]? optionalParameterTypes)
        {
            int stackchange = 0;
            if (optionalParameterTypes != null)
            {
                if ((callingConvention & CallingConventions.VarArgs) == 0)
                {
                    // Client should not supply optional parameter in default calling convention
                    throw new InvalidOperationException("Calling convention must be VarArgs.");
                }
            }

            ModuleBuilder modBuilder = (ModuleBuilder)_methodBuilder.ModuleBuilder;
            SignatureHelper sig = GetMemberRefSignature(callingConvention,
                returnType,
                parameterTypes,
                optionalParameterTypes);

            EnsureCapacity(7);
            Emit(OpCodes.Calli);

            // If there is a non-void return type, push one.
            if (returnType != modBuilder.Universe.System_Void)
                stackchange++;
            // Pop off arguments if any.
            if (parameterTypes != null)
                stackchange -= parameterTypes.Length;
            // Pop off vararg arguments.
            if (optionalParameterTypes != null)
                stackchange -= optionalParameterTypes.Length;
            // Pop the this parameter if the method has a this parameter.
            if ((callingConvention & CallingConventions.HasThis) == CallingConventions.HasThis)
                stackchange--;
            // Pop the native function pointer.
            stackchange--;
            UpdateStackSize(OpCodes.Calli, stackchange);

            RecordTokenFixup();
            PutInteger4(modBuilder.GetSignatureToken(sig).Token);
        }

        public void EmitCalli(OpCode opcode, CallingConvention unmanagedCallConv, Type? returnType, Type[]? parameterTypes)
        {
            int stackchange = 0;
            int cParams = 0;

            ModuleBuilder modBuilder = (ModuleBuilder)_methodBuilder.Module;

            if (parameterTypes != null)
            {
                cParams = parameterTypes.Length;
            }

            SignatureHelper sig = SignatureHelper.GetMethodSigHelper(
                modBuilder,
                unmanagedCallConv,
                returnType);

            if (parameterTypes != null)
            {
                for (int i = 0; i < cParams; i++)
                {
                    sig.AddArgument(parameterTypes[i]);
                }
            }

            // If there is a non-void return type, push one.
            if (returnType != modBuilder.Universe.System_Void)
                stackchange++;

            // Pop off arguments if any.
            if (parameterTypes != null)
                stackchange -= cParams;

            // Pop the native function pointer.
            stackchange--;
            UpdateStackSize(OpCodes.Calli, stackchange);

            EnsureCapacity(7);
            Emit(OpCodes.Calli);
            RecordTokenFixup();
            PutInteger4(modBuilder.GetSignatureToken(sig).Token);
        }

        public void EmitCall(OpCode opcode, MethodInfo methodInfo, Type[]? optionalParameterTypes)
        {
            if (methodInfo is null)
                throw new ArgumentNullException(nameof(methodInfo));

            if (!(opcode.Equals(OpCodes.Call) || opcode.Equals(OpCodes.Callvirt) || opcode.Equals(OpCodes.Newobj)))
                throw new ArgumentException("The specified opcode cannot be passed to EmitCall.", nameof(opcode));

            int stackchange = 0;
            int tk = GetMethodToken(methodInfo, optionalParameterTypes, false);

            EnsureCapacity(7);
            InternalEmit(opcode);

            // Push the return value if there is one.
            if (methodInfo.ReturnType != _methodBuilder.ModuleBuilder.Universe.System_Void)
                stackchange++;
            // Pop the parameters.
            Type[] parameters = methodInfo.GetParameterTypes();
            if (parameters != null)
                stackchange -= parameters.Length;

            // Pop the this parameter if the method is non-static and the
            // instruction is not newobj.
            if (/*!(methodInfo is SymbolMethod) && */!methodInfo.IsStatic && !opcode.Equals(OpCodes.Newobj))
                stackchange--;
            // Pop the optional parameters off the stack.
            if (optionalParameterTypes != null)
                stackchange -= optionalParameterTypes.Length;
            UpdateStackSize(opcode, stackchange);

            RecordTokenFixup();
            PutInteger4(tk);
        }

        public void Emit(OpCode opcode, ConstructorInfo con)
        {
            if (con is null)
                throw new ArgumentNullException(nameof(con));

            int stackchange = 0;

            // Constructors cannot be generic so the value of UseMethodDef doesn't matter.
            int tk = GetMethodToken(con, null, true);

            EnsureCapacity(7);
            InternalEmit(opcode);

            // Make a conservative estimate by assuming a return type and no
            // this parameter.
            if (opcode.StackBehaviourPush == StackBehaviour.Varpush)
            {
                // Instruction must be one of call or callvirt.
                Debug.Assert(opcode.Equals(OpCodes.Call) ||
                                opcode.Equals(OpCodes.Callvirt),
                                "Unexpected opcode encountered for StackBehaviour of VarPush.");
                stackchange++;
            }
            if (opcode.StackBehaviourPop == StackBehaviour.Varpop)
            {
                // Instruction must be one of call, callvirt or newobj.
                Debug.Assert(opcode.Equals(OpCodes.Call) ||
                                opcode.Equals(OpCodes.Callvirt) ||
                                opcode.Equals(OpCodes.Newobj),
                                "Unexpected opcode encountered for StackBehaviour of VarPop.");

                Type[] parameters = con.GetParameterTypes();
                if (parameters != null)
                    stackchange -= parameters.Length;
            }
            UpdateStackSize(opcode, stackchange);

            RecordTokenFixup();
            PutInteger4(tk);
        }

        public void Emit(OpCode opcode, Type cls)
        {
            // Puts opcode onto the stream and then the metadata token represented
            // by cls.  The location of cls is recorded so that the token can be
            // patched if necessary when persisting the module to a PE.

            ModuleBuilder modBuilder = (ModuleBuilder)_methodBuilder.Module;
            bool getGenericDefinition = (opcode == OpCodes.Ldtoken && cls != null && cls.IsGenericTypeDefinition);
            int tempVal = getGenericDefinition ? modBuilder.GetTypeToken(cls).Token : modBuilder.GetTypeTokenForMemberRef(cls);

            EnsureCapacity(7);
            InternalEmit(opcode);
            RecordTokenFixup();
            PutInteger4(tempVal);
        }

        public void Emit(OpCode opcode, long arg)
        {
            EnsureCapacity(11);
            InternalEmit(opcode);
            BinaryPrimitives.WriteInt64LittleEndian(_ilStream.AsSpan(_length), arg);
            _length += 8;
        }

        public void Emit(OpCode opcode, float arg)
        {
            EnsureCapacity(7);
            InternalEmit(opcode);
            BinaryPrimitives.WriteInt32LittleEndian(_ilStream.AsSpan(_length), SingleToInt32Bits(arg));
            _length += 4;
        }

        /// <summary>
        /// Converts the specified single-precision floating point number to a 32-bit signed integer.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>A 32-bit signed integer whose bits are identical to <paramref name="value"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe int SingleToInt32Bits(float value)
        {
#if NETFRAMEWORK
            return *((int*)&value);
#else
            return BitConverter.SingleToInt32Bits(value);
#endif
        }

        public void Emit(OpCode opcode, double arg)
        {
            EnsureCapacity(11);
            InternalEmit(opcode);
            BinaryPrimitives.WriteInt64LittleEndian(_ilStream.AsSpan(_length), BitConverter.DoubleToInt64Bits(arg));
            _length += 8;
        }

        public void Emit(OpCode opcode, Label label)
        {
            // Puts opcode onto the stream and leaves space to include label
            // when fixups are done.  Labels are created using ILGenerator.DefineLabel and
            // their location within the stream is fixed by using ILGenerator.MarkLabel.
            // If a single-byte instruction (designated by the _S suffix in OpCodes.cs) is used,
            // the label can represent a jump of at most 127 bytes along the stream.
            //
            // opcode must represent a branch instruction (although we don't explicitly
            // verify this).  Since branches are relative instructions, label will be replaced with the
            // correct offset to branch during the fixup process.

            EnsureCapacity(7);

            InternalEmit(opcode);
            if (OpCodes.TakesSingleByteArgument(opcode))
            {
                AddFixup(label, _length++, 1);
            }
            else
            {
                AddFixup(label, _length, 4);
                _length += 4;
            }
        }

        public void Emit(OpCode opcode, Label[] labels)
        {
            if (labels is null)
                throw new ArgumentNullException(nameof(labels));

            // Emitting a switch table

            int i;
            int remaining;                  // number of bytes remaining for this switch instruction to be subtracted
            // for computing the offset

            int count = labels.Length;

            EnsureCapacity(count * 4 + 7);
            InternalEmit(opcode);
            PutInteger4(count);
            for (remaining = count * 4, i = 0; remaining > 0; remaining -= 4, i++)
            {
                AddFixup(labels[i], _length, remaining);
                _length += 4;
            }
        }

        public void Emit(OpCode opcode, FieldInfo field)
        {
            ModuleBuilder modBuilder = (ModuleBuilder)_methodBuilder.Module;
            int tempVal = modBuilder.GetFieldToken(field).Token;
            EnsureCapacity(7);
            InternalEmit(opcode);
            RecordTokenFixup();
            PutInteger4(tempVal);
        }

        public void Emit(OpCode opcode, string str)
        {
            // Puts the opcode onto the IL stream followed by the metadata token
            // represented by str.  The location of str is recorded for future
            // fixups if the module is persisted to a PE.

            ModuleBuilder modBuilder = (ModuleBuilder)_methodBuilder.Module;
            int tempVal = modBuilder.GetStringConstant(str).Token;
            EnsureCapacity(7);
            InternalEmit(opcode);
            PutInteger4(tempVal);
        }

        public void Emit(OpCode opcode, LocalBuilder local)
        {
            if (local is null)
                throw new ArgumentNullException(nameof(local));

            // Puts the opcode onto the IL stream followed by the information for local variable local.
            int tempVal = local.LocalIndex;
            if (local.Method != _methodBuilder)
            {
                throw new ArgumentException("Local passed in does not belong to this ILGenerator.", nameof(local));
            }
            // If the instruction is a ldloc, ldloca a stloc, morph it to the optimal form.
            if (opcode.Equals(OpCodes.Ldloc))
            {
                switch (tempVal)
                {
                    case 0:
                        opcode = OpCodes.Ldloc_0;
                        break;
                    case 1:
                        opcode = OpCodes.Ldloc_1;
                        break;
                    case 2:
                        opcode = OpCodes.Ldloc_2;
                        break;
                    case 3:
                        opcode = OpCodes.Ldloc_3;
                        break;
                    default:
                        if (tempVal <= 255)
                            opcode = OpCodes.Ldloc_S;
                        break;
                }
            }
            else if (opcode.Equals(OpCodes.Stloc))
            {
                switch (tempVal)
                {
                    case 0:
                        opcode = OpCodes.Stloc_0;
                        break;
                    case 1:
                        opcode = OpCodes.Stloc_1;
                        break;
                    case 2:
                        opcode = OpCodes.Stloc_2;
                        break;
                    case 3:
                        opcode = OpCodes.Stloc_3;
                        break;
                    default:
                        if (tempVal <= 255)
                            opcode = OpCodes.Stloc_S;
                        break;
                }
            }
            else if (opcode.Equals(OpCodes.Ldloca))
            {
                if (tempVal <= 255)
                    opcode = OpCodes.Ldloca_S;
            }

            EnsureCapacity(7);
            InternalEmit(opcode);

            if (opcode.OperandType == OperandType.InlineNone)
                return;

            if (!OpCodes.TakesSingleByteArgument(opcode))
            {
                BinaryPrimitives.WriteInt16LittleEndian(_ilStream.AsSpan(_length), (short)tempVal);
                _length += 2;
            }
            else
            {
                // Handle stloc_1, ldloc_1
                if (tempVal > byte.MaxValue)
                {
                    throw new InvalidOperationException("Opcodes using a short-form index cannot address a local position over 255.");
                }
                _ilStream[_length++] = (byte)tempVal;
            }
        }

#endregion
#region Exceptions

        public Label BeginExceptionBlock()
        {
            // Begin an Exception block.  Creating an Exception block records some information,
            // but does not actually emit any IL onto the stream.  Exceptions should be created and
            // marked in the following form:
            //
            // Emit Some IL
            // BeginExceptionBlock
            // Emit the IL which should appear within the "try" block
            // BeginCatchBlock
            // Emit the IL which should appear within the "catch" block
            // Optional: BeginCatchBlock (this can be repeated an arbitrary number of times
            // EndExceptionBlock

            // Delay init
            _exceptions ??= new __ExceptionInfo[DefaultExceptionArraySize];
            _currExcStack ??= new __ExceptionInfo[DefaultExceptionArraySize];

            if (_exceptionCount >= _exceptions.Length)
            {
                _exceptions = EnlargeArray(_exceptions);
            }

            if (_currExcStackCount >= _currExcStack.Length)
            {
                _currExcStack = EnlargeArray(_currExcStack);
            }

            Label endLabel = DefineLabel(0);
            __ExceptionInfo exceptionInfo = new __ExceptionInfo(_length, endLabel);

            // add the exception to the tracking list
            _exceptions[_exceptionCount++] = exceptionInfo;

            // Make this exception the current active exception
            _currExcStack[_currExcStackCount++] = exceptionInfo;

            // Stack depth for "try" starts at zero.
            _curDepth = 0;

            return endLabel;
        }

        public void EndExceptionBlock()
        {
            if (_currExcStackCount == 0)
            {
                throw new NotSupportedException("Not currently in an exception block.");
            }

            // Pop the current exception block
            __ExceptionInfo current = _currExcStack![_currExcStackCount - 1];
            _currExcStack[--_currExcStackCount] = null!;

            Label endLabel = current.GetEndLabel();
            int state = current.GetCurrentState();

            if (state == __ExceptionInfo.State_Filter ||
                state == __ExceptionInfo.State_Try)
            {
                throw new InvalidOperationException("Incorrect code generation for exception block.");
            }

            if (state == __ExceptionInfo.State_Catch)
            {
                Emit(OpCodes.Leave, endLabel);
            }
            else if (state == __ExceptionInfo.State_Finally || state == __ExceptionInfo.State_Fault)
            {
                Emit(OpCodes.Endfinally);
            }

            // Check if we've already set this label.
            // The only reason why we might have set this is if we have a finally block.

            Label label = _labelList![endLabel.GetLabelValue()].Pos != -1
                ? current.FinallyEndLabel
                : endLabel;

            MarkLabel(label);

            current.Done(_length);
        }

        public void BeginCatchBlock(Type? exceptionType)
        {
            Debug.Assert(ModuleBuilder.IsPseudoToken(_methodBuilder.ModuleBuilder.GetTypeTokenForMemberRef(exceptionType)) == false);

            // Begins a catch block.  Emits a branch instruction to the end of the current exception block.

            if (_currExcStackCount == 0)
            {
                throw new NotSupportedException("Not currently in an exception block.");
            }
            __ExceptionInfo current = _currExcStack![_currExcStackCount - 1];

            if (current.GetCurrentState() == __ExceptionInfo.State_Filter)
            {
                if (exceptionType != null)
                {
                    throw new ArgumentException("Should not specify exception type for catch clause for filter block.");
                }

                Emit(OpCodes.Endfilter);
            }
            else
            {
                // execute this branch if previous clause is Catch or Fault
                if (exceptionType is null)
                    throw new ArgumentNullException(nameof(exceptionType));

                Emit(OpCodes.Leave, current.GetEndLabel());
            }

            current.MarkCatchAddr(_length, exceptionType);

            // Stack depth for "catch" starts at one.
            _curDepth = 1;
        }

        public void BeginFaultBlock()
        {
            if (_currExcStackCount == 0)
            {
                throw new NotSupportedException("Not currently in an exception block.");
            }
            __ExceptionInfo current = _currExcStack![_currExcStackCount - 1];

            // emit the leave for the clause before this one.
            Emit(OpCodes.Leave, current.GetEndLabel());

            current.MarkFaultAddr(_length);

            // Stack depth for "fault" starts at zero.
            _curDepth = 0;
        }

        public void BeginFinallyBlock()
        {
            if (_currExcStackCount == 0)
            {
                throw new NotSupportedException("Not currently in an exception block.");
            }
            __ExceptionInfo current = _currExcStack![_currExcStackCount - 1];
            int state = current.GetCurrentState();
            Label endLabel = current.GetEndLabel();
            int catchEndAddr = 0;
            if (state != __ExceptionInfo.State_Try)
            {
                // generate leave for any preceding catch clause
                Emit(OpCodes.Leave, endLabel);
                catchEndAddr = _length;
            }

            MarkLabel(endLabel);

            Label finallyEndLabel = DefineLabel(0);
            current.SetFinallyEndLabel(finallyEndLabel);

            // generate leave for try clause
            Emit(OpCodes.Leave, finallyEndLabel);
            if (catchEndAddr == 0)
                catchEndAddr = _length;
            current.MarkFinallyAddr(_length, catchEndAddr);

            // Stack depth for "finally" starts at zero.
            _curDepth = 0;
        }

        #endregion

        #region Labels
        public Label DefineLabel()
        {
            // We don't know the stack depth at the label yet, so set it to -1.
            return DefineLabel(-1);
        }

        private Label DefineLabel(int depth)
        {
            // Declares a new Label.  This is just a token and does not yet represent any particular location
            // within the stream.  In order to set the position of the label within the stream, you must call
            // Mark Label.
            Debug.Assert(depth >= -1);

            // Delay init the label array in case we dont use it
            _labelList ??= new __LabelInfo[DefaultLabelArraySize];

            if (_labelCount >= _labelList.Length)
            {
                _labelList = EnlargeArray(_labelList);
            }
            _labelList[_labelCount].Pos = -1;
            _labelList[_labelCount].Depth = depth;
            return new Label(_labelCount++);
        }

        public void MarkLabel(Label loc)
        {
            // Defines a label by setting the position where that label is found within the stream.
            // Does not allow a label to be defined more than once.

            int labelIndex = loc.Id;

            // This should only happen if a label from another generator is used with this one.
            if (_labelList is null || labelIndex < 0 || labelIndex >= _labelList.Length)
            {
                throw new ArgumentException("Invalid Label.");
            }

            if (_labelList[labelIndex].Pos != -1)
            {
                throw new ArgumentException("Label defined multiple times.");
            }

            _labelList[labelIndex].Pos = _length;

            int depth = _labelList[labelIndex].Depth;
            if (depth < 0)
            {
                // Unknown depth for this label, indicating that it hasn't been used yet.
                // If m_curDepth is unknown, we're in the Backward branch constraint case. See ECMA-335 III.1.7.5.
                // The m_depthAdjustment field will compensate for violations of this constraint, as we
                // discover them. That is, here we assume a depth of zero. If a (later) branch to this label
                // has a positive stack depth, we'll record that as the new depth and add the delta into
                // m_depthAdjustment.
                if (_curDepth < 0)
                    _curDepth = 0;
                _labelList[labelIndex].Depth = _curDepth;
            }
            else if (depth < _curDepth)
            {
                // A branch location with smaller stack targets this label. In this case, the IL is
                // invalid, but we just compensate for it.
                _depthAdjustment += _curDepth - depth;
                _labelList[labelIndex].Depth = _curDepth;
            }
            else if (depth > _curDepth)
            {
                // Either the current depth is unknown, or a branch location with larger stack targets
                // this label, so the IL is invalid. In either case, just adjust the current depth.
                _curDepth = depth;
            }
        }

        #endregion

        #region Debug API

        public LocalBuilder DeclareLocal(Type localType)
        {
            return DeclareLocal(localType, false);
        }

        public LocalBuilder DeclareLocal(Type localType, bool pinned)
        {
            // Declare a local of type "local". The current active lexical scope
            // will be the scope that local will live.

            if (_methodBuilder is not MethodBuilder methodBuilder)
                throw new NotSupportedException();

            if (methodBuilder.IsTypeCreated())
            {
                // cannot change method after its containing type has been created
                throw new InvalidOperationException("Unable to change after type has been created.");
            }

            if (localType is null)
                throw new ArgumentNullException(nameof(localType));

            if (methodBuilder.IsBaked)
            {
                throw new InvalidOperationException("Type definition of the method is complete.");
            }

            // add the localType to local signature
            LocalSignature.AddArgument(localType, pinned);

            return new LocalBuilder(_methodBuilder, localType, LocalCount++, pinned);
        }

        public void BeginScope()
        {
            ScopeTree.AddScopeInfo(ScopeAction.Open, _length);
        }

        public void EndScope()
        {
            ScopeTree.AddScopeInfo(ScopeAction.Close, _length);
        }

        public virtual void MarkSequencePoint(
            ISymbolDocumentWriter document,
            int startLine,       // line number is 1 based
            int startColumn,     // column is 0 based
            int endLine,         // line number is 1 based
            int endColumn)       // column is 0 based
        {
            if (startLine == 0 || startLine < 0 || endLine == 0 || endLine < 0)
            {
                throw new ArgumentOutOfRangeException("startLine");
            }
            Contract.EndContractBlock();
            LineNumberInfo.AddLineNumberInfo(document, _length, startLine, startColumn, endLine, endColumn);
        }

        public int ILOffset => _length;

        public void Emit(OpCode opcode, sbyte arg) => Emit(opcode, (byte)arg);

        #endregion

        #endregion
    }

    internal struct __LabelInfo
    {
        internal int Pos; // Position in the il stream, with -1 meaning unknown.
        internal int Depth; // Stack depth, with -1 meaning unknown.
    }

    internal struct __FixupData
    {
        internal Label FixupLabel;
        internal int FixupPos;

        internal int FixupInstSize;
    }

    internal sealed class __ExceptionInfo
    {
        internal const int None = 0x0000;  // COR_ILEXCEPTION_CLAUSE_NONE
        internal const int Filter = 0x0001;  // COR_ILEXCEPTION_CLAUSE_FILTER
        internal const int Finally = 0x0002;  // COR_ILEXCEPTION_CLAUSE_FINALLY
        internal const int Fault = 0x0004;  // COR_ILEXCEPTION_CLAUSE_FAULT

        internal const int State_Try = 0;
        internal const int State_Filter = 1;
        internal const int State_Catch = 2;
        internal const int State_Finally = 3;
        internal const int State_Fault = 4;
        internal const int State_Done = 5;

        int _startAddr;
        int[] _filterAddr;
        int[] _catchAddr;
        int[] _catchEndAddr;
        int[] _type;
        Type[] _catchClass;
        Label _endLabel;
        internal Label FinallyEndLabel;
        int _endAddr;
        int _endFinally;
        int _currentCatch;

        private int _currentState;

        internal __ExceptionInfo(int startAddr, Label endLabel)
        {
            _startAddr = startAddr;
            _endAddr = -1;
            _filterAddr = new int[4];
            _catchAddr = new int[4];
            _catchEndAddr = new int[4];
            _catchClass = new Type[4];
            _currentCatch = 0;
            _endLabel = endLabel;
            _type = new int[4];
            _endFinally = -1;
            _currentState = State_Try;
        }

        private void MarkHelper(
            int catchorfilterAddr,      // the starting address of a clause
            int catchEndAddr,           // the end address of a previous catch clause. Only use when finally is following a catch
            Type? catchClass,             // catch exception type
            int type)                   // kind of clause
        {
            int currentCatch = _currentCatch;
            if (currentCatch >= _catchAddr.Length)
            {
                _filterAddr = ILGenerator.EnlargeArray(_filterAddr);
                _catchAddr = ILGenerator.EnlargeArray(_catchAddr);
                _catchEndAddr = ILGenerator.EnlargeArray(_catchEndAddr);
                _catchClass = ILGenerator.EnlargeArray(_catchClass);
                _type = ILGenerator.EnlargeArray(_type);
            }
            if (type == Filter)
            {
                _type[currentCatch] = type;
                _filterAddr[currentCatch] = catchorfilterAddr;
                _catchAddr[currentCatch] = -1;
                if (currentCatch > 0)
                {
                    Debug.Assert(_catchEndAddr[currentCatch - 1] == -1, "m_catchEndAddr[m_currentCatch-1] == -1");
                    _catchEndAddr[currentCatch - 1] = catchorfilterAddr;
                }
            }
            else
            {
                // catch or Fault clause
                _catchClass[currentCatch] = catchClass!;
                if (_type[currentCatch] != Filter)
                {
                    _type[currentCatch] = type;
                }
                _catchAddr[currentCatch] = catchorfilterAddr;
                if (currentCatch > 0)
                {
                    if (_type[currentCatch] != Filter)
                    {
                        Debug.Assert(_catchEndAddr[currentCatch - 1] == -1, "m_catchEndAddr[m_currentCatch-1] == -1");
                        _catchEndAddr[currentCatch - 1] = catchEndAddr;
                    }
                }
                _catchEndAddr[currentCatch] = -1;
                _currentCatch++;
            }

            if (_endAddr == -1)
            {
                _endAddr = catchorfilterAddr;
            }
        }

        internal void MarkFaultAddr(int faultAddr)
        {
            _currentState = State_Fault;
            MarkHelper(faultAddr, faultAddr, null, Fault);
        }

        internal void MarkCatchAddr(int catchAddr, Type? catchException)
        {
            _currentState = State_Catch;
            MarkHelper(catchAddr, catchAddr, catchException, None);
        }

        internal void MarkFinallyAddr(int finallyAddr, int endCatchAddr)
        {
            if (_endFinally != -1)
            {
                throw new ArgumentException("Exception blocks may have at most one finally clause.");
            }

            _currentState = State_Finally;
            _endFinally = finallyAddr;
            MarkHelper(finallyAddr, endCatchAddr, null, Finally);
        }

        internal void Done(int endAddr)
        {
            Debug.Assert(_currentCatch > 0, "m_currentCatch > 0");
            Debug.Assert(_catchAddr[_currentCatch - 1] > 0, "m_catchAddr[m_currentCatch-1] > 0");
            Debug.Assert(_catchEndAddr[_currentCatch - 1] == -1, "m_catchEndAddr[m_currentCatch-1] == -1");
            _catchEndAddr[_currentCatch - 1] = endAddr;
            _currentState = State_Done;
        }

        internal int GetStartAddress()
        {
            return _startAddr;
        }

        internal int GetEndAddress()
        {
            return _endAddr;
        }

        internal int GetFinallyEndAddress()
        {
            return _endFinally;
        }

        internal Label GetEndLabel()
        {
            return _endLabel;
        }

        internal int[] GetFilterAddresses()
        {
            return _filterAddr;
        }

        internal int[] GetCatchAddresses()
        {
            return _catchAddr;
        }

        internal int[] GetCatchEndAddresses()
        {
            return _catchEndAddr;
        }

        internal Type[] GetCatchClass()
        {
            return _catchClass;
        }

        internal int GetNumberOfCatches()
        {
            return _currentCatch;
        }

        internal int[] GetExceptionTypes()
        {
            return _type;
        }

        internal void SetFinallyEndLabel(Label lbl)
        {
            FinallyEndLabel = lbl;
        }

        // Specifies whether exc is an inner exception for "this".  The way
        // its determined is by comparing the end address for the last catch
        // clause for both exceptions.  If they're the same, the start address
        // for the exception is compared.
        // WARNING: This is not a generic function to determine the innerness
        // of an exception.  This is somewhat of a mis-nomer.  This gives a
        // random result for cases where the two exceptions being compared do
        // not having a nesting relation.
        internal bool IsInner(__ExceptionInfo exc)
        {
            Debug.Assert(exc != null);
            Debug.Assert(_currentCatch > 0, "m_currentCatch > 0");
            Debug.Assert(exc._currentCatch > 0, "exc.m_currentCatch > 0");

            int exclast = exc._currentCatch - 1;
            int last = _currentCatch - 1;

            if (exc._catchEndAddr[exclast] < _catchEndAddr[last])
                return true;

            if (exc._catchEndAddr[exclast] != _catchEndAddr[last])
                return false;
            Debug.Assert(exc.GetEndAddress() != GetEndAddress(),
                "exc.GetEndAddress() != GetEndAddress()");

            return exc.GetEndAddress() > GetEndAddress();
        }

        // 0 indicates in a try block
        // 1 indicates in a filter block
        // 2 indicates in a catch block
        // 3 indicates in a finally block
        // 4 indicates Done
        internal int GetCurrentState()
        {
            return _currentState;
        }
    }

    /// <summary>
    /// Scope Tree is a class that track the scope structure within a method body
    /// It keeps track two parallel array. m_ScopeAction keeps track the action. It can be
    /// OpenScope or CloseScope. m_iOffset records the offset where the action
    /// takes place.
    /// </summary>
    internal enum ScopeAction : sbyte
    {
        Open = -0x1,
        Close = 0x1
    }

    internal sealed class ScopeTree
    {
        internal ScopeTree()
        {
            // initialize data variables
            OpenScopeCount = 0;
            _count = 0;
        }

        /// <summary>
        /// Find the current active lexical scope. For example, if we have
        /// "Open Open Open Close",
        /// we will return 1 as the second BeginScope is currently active.
        /// </summary>
        internal int GetCurrentActiveScopeIndex()
        {
            if (_count == 0)
            {
                return -1;
            }

            int i = _count - 1;

            for (int cClose = 0; cClose > 0 || _scopeActions[i] == ScopeAction.Close; i--)
            {
                cClose += (sbyte)_scopeActions[i];
            }

            return i;
        }

        internal void AddLocalSymInfoToCurrentScope(
            string strName,
            byte[] signature,
            int slot,
            int startOffset,
            int endOffset)
        {
            int i = GetCurrentActiveScopeIndex();
            _localSymInfos[i] ??= new LocalSymInfo();
            _localSymInfos[i]!.AddLocalSymInfo(strName, signature, slot, startOffset, endOffset);
        }

        internal void AddScopeInfo(ScopeAction sa, int iOffset)
        {
            if (sa == ScopeAction.Close && OpenScopeCount <= 0)
            {
                throw new ArgumentException("Non-matching symbol scope.");
            }

            // make sure that arrays are large enough to hold addition info
            EnsureCapacity();

            _scopeActions[_count] = sa;
            _offsets[_count] = iOffset;
            _localSymInfos[_count] = null;
            checked { _count++; }

            OpenScopeCount += -(sbyte)sa;
        }

        /// <summary>
        /// Helper to ensure arrays are large enough
        /// </summary>
        internal void EnsureCapacity()
        {
            if (_count == 0)
            {
                // First time. Allocate the arrays.
                _offsets = new int[InitialSize];
                _scopeActions = new ScopeAction[InitialSize];
                _localSymInfos = new LocalSymInfo[InitialSize];
            }
            else if (_count == _offsets.Length)
            {
                // the arrays are full. Enlarge the arrays
                // It would probably be simpler to just use Lists here.
                int newSize = checked(_count * 2);
                int[] temp = new int[newSize];
                Array.Copy(_offsets, temp, _count);
                _offsets = temp;

                ScopeAction[] tempSA = new ScopeAction[newSize];
                Array.Copy(_scopeActions, tempSA, _count);
                _scopeActions = tempSA;

                LocalSymInfo[] tempLSI = new LocalSymInfo[newSize];
                Array.Copy(_localSymInfos, tempLSI, _count);
                _localSymInfos = tempLSI;
            }
        }
        internal void EmitScopeTree(ISymbolWriter symWriter)
        {
            int i;
            for (i = 0; i < _count; i++)
            {
                if (_scopeActions != null && _scopeActions[i] == ScopeAction.Open)
                {
                    symWriter.OpenScope(_offsets[i]);
                }
                else
                {
                    symWriter.CloseScope(_offsets[i]);
                }
                if (_localSymInfos != null && _localSymInfos[i] != null)
                {
                    _localSymInfos[i]!.EmitLocalSymInfo(symWriter);
                }
            }
        }

        int[] _offsets = null!;                 // array of offsets
        ScopeAction[] _scopeActions = null!;             // array of scope actions
        int _count;                   // how many entries in the arrays are occupied
        internal int OpenScopeCount;          // keep track how many scopes are open
        internal const int InitialSize = 16;
        LocalSymInfo?[] _localSymInfos = null!;            // keep track debugging local information
    }

    /***************************
    *
    * This class tracks the line number info
    *
    ***************************/
    internal sealed class LineNumberInfo
    {
        internal LineNumberInfo()
        {
            // initialize data variables
            _documentCount = 0;
            _lastFound = 0;
        }

        internal void AddLineNumberInfo(
            ISymbolDocumentWriter document,
            int iOffset,
            int iStartLine,
            int iStartColumn,
            int iEndLine,
            int iEndColumn)
        {
            int i;

            // make sure that arrays are large enough to hold addition info
            i = FindDocument(document);

            Contract.Assert(i < _documentCount, "Bad document look up!");
            _documents[i].AddLineNumberInfo(document, iOffset, iStartLine, iStartColumn, iEndLine, iEndColumn);
        }

        // Find a REDocument representing document. If we cannot find one, we will add a new entry into
        // the REDocument array.
        private int FindDocument(ISymbolDocumentWriter document)
        {
            int i;

            // This is an optimization. The chance that the previous line is coming from the same
            // document is very high.
            if (_lastFound < _documentCount && _documents[_lastFound].Document == document)
                return _lastFound;

            for (i = 0; i < _documentCount; i++)
            {
                if (_documents[i].Document == document)
                {
                    _lastFound = i;
                    return _lastFound;
                }
            }

            // cannot find an existing document so add one to the array                                       
            EnsureCapacity();
            _lastFound = _documentCount;
            _documents[_lastFound] = new REDocument(document);
            checked { _documentCount++; }
            return _lastFound;
        }

        /**************************
        *
        * Helper to ensure arrays are large enough
        *
        **************************/
        private void EnsureCapacity()
        {
            if (_documentCount == 0)
            {
                // First time. Allocate the arrays.
                _documents = new REDocument[InitialSize];
            }
            else if (_documentCount == _documents.Length)
            {
                // the arrays are full. Enlarge the arrays
                REDocument[] temp = new REDocument[_documentCount * 2];
                Array.Copy(_documents, temp, _documentCount);
                _documents = temp;
            }
        }

        internal void EmitLineNumberInfo(ISymbolWriter symWriter)
        {
            for (int i = 0; i < _documentCount; i++)
                _documents[i].EmitLineNumberInfo(symWriter);
        }

        private int _documentCount;         // how many documents that we have right now
        private REDocument[] _documents;             // array of documents
        private const int InitialSize = 16;
        private int _lastFound;
    }

    /***************************
    *
    * This class tracks the line number info
    *
    ***************************/
    internal sealed class REDocument
    {
        internal REDocument(ISymbolDocumentWriter document)
        {
            // initialize data variables
            _lineNumberCount = 0;
            Document = document;
        }

        internal void AddLineNumberInfo(
            ISymbolDocumentWriter document,
            int iOffset,
            int iStartLine,
            int iStartColumn,
            int iEndLine,
            int iEndColumn)
        {
            Contract.Assert(document == Document, "Bad document look up!");

            // make sure that arrays are large enough to hold addition info
            EnsureCapacity();

            _offsets[_lineNumberCount] = iOffset;
            _lines[_lineNumberCount] = iStartLine;
            _columns[_lineNumberCount] = iStartColumn;
            _endLines[_lineNumberCount] = iEndLine;
            _endColumns[_lineNumberCount] = iEndColumn;
            checked { _lineNumberCount++; }
        }

        /**************************
        *
        * Helper to ensure arrays are large enough
        *
        **************************/
        private void EnsureCapacity()
        {
            if (_lineNumberCount == 0)
            {
                // First time. Allocate the arrays.
                _offsets = new int[InitialSize];
                _lines = new int[InitialSize];
                _columns = new int[InitialSize];
                _endLines = new int[InitialSize];
                _endColumns = new int[InitialSize];
            }
            else if (_lineNumberCount == _offsets.Length)
            {
                // the arrays are full. Enlarge the arrays
                // It would probably be simpler to just use Lists here
                int newSize = checked(_lineNumberCount * 2);
                int[] temp = new int[newSize];
                Array.Copy(_offsets, temp, _lineNumberCount);
                _offsets = temp;

                temp = new int[newSize];
                Array.Copy(_lines, temp, _lineNumberCount);
                _lines = temp;

                temp = new int[newSize];
                Array.Copy(_columns, temp, _lineNumberCount);
                _columns = temp;

                temp = new int[newSize];
                Array.Copy(_endLines, temp, _lineNumberCount);
                _endLines = temp;

                temp = new int[newSize];
                Array.Copy(_endColumns, temp, _lineNumberCount);
                _endColumns = temp;
            }
        }

        internal void EmitLineNumberInfo(ISymbolWriter symWriter)
        {
            int[] iOffsetsTemp;
            int[] iLinesTemp;
            int[] iColumnsTemp;
            int[] iEndLinesTemp;
            int[] iEndColumnsTemp;

            if (_lineNumberCount == 0)
                return;
            // reduce the array size to be exact
            iOffsetsTemp = new int[_lineNumberCount];
            Array.Copy(_offsets, iOffsetsTemp, _lineNumberCount);

            iLinesTemp = new int[_lineNumberCount];
            Array.Copy(_lines, iLinesTemp, _lineNumberCount);

            iColumnsTemp = new int[_lineNumberCount];
            Array.Copy(_columns, iColumnsTemp, _lineNumberCount);

            iEndLinesTemp = new int[_lineNumberCount];
            Array.Copy(_endLines, iEndLinesTemp, _lineNumberCount);

            iEndColumnsTemp = new int[_lineNumberCount];
            Array.Copy(_endColumns, iEndColumnsTemp, _lineNumberCount);

            symWriter.DefineSequencePoints(Document, iOffsetsTemp, iLinesTemp, iColumnsTemp, iEndLinesTemp, iEndColumnsTemp);
        }

        private int[] _offsets;                 // array of offsets
        private int[] _lines;                   // array of offsets
        private int[] _columns;                 // array of offsets
        private int[] _endLines;                // array of offsets
        private int[] _endColumns;              // array of offsets
        internal ISymbolDocumentWriter Document;       // The ISymbolDocumentWriter that this REDocument is tracking.
        private int _lineNumberCount;         // how many entries in the arrays are occupied
        private const int InitialSize = 16;
    }       // end of REDocument

}

#nullable restore
