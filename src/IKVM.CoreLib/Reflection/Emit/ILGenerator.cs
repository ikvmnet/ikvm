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

    /// <summary>
    /// Generates Microsoft intermediate language (MSIL) instructions.
    /// </summary>
    class ILGenerator
    {

        #region Const Members

        const int DefaultSize = 16;
        const int DefaultFixupArraySize = 8;
        const int DefaultLabelArraySize = 4;
        const int DefaultExceptionArraySize = 2;

        #endregion

        #region Internal Statics

        /// <summary>
        /// Returns a copy of the array with twice its length.
        /// </summary>
        /// <typeparam name="T">The type of the array elements.</typeparam>
        /// <param name="incoming">The array to enlarge.</param>
        /// <returns>A new array of twice the length, starting with the elements of <paramref name="incoming"/>.</returns>
        internal static T[] EnlargeArray<T>(T[] incoming) => EnlargeArray(incoming, incoming.Length * 2);

        /// <summary>
        /// Returns a copy of the array with the specified length.
        /// </summary>
        /// <typeparam name="T">The type of the array elements.</typeparam>
        /// <param name="incoming">The array to enlarge.</param>
        /// <param name="requiredSize">The length of the new array. Must be at least the length of <paramref name="incoming"/>.</param>
        /// <returns>A new array of the required length, starting with the elements of <paramref name="incoming"/>.</returns>
        internal static T[] EnlargeArray<T>(T[] incoming, int requiredSize)
        {
            Debug.Assert(incoming != null);

            T[] temp = new T[requiredSize];
            Array.Copy(incoming, temp, incoming.Length);
            return temp;
        }

        #endregion

        #region Internal Data Members

        int _length;
        byte[] _ilStream;

        __LabelInfo[]? _labelList;
        int _labelCount;

        __FixupData[]? _fixupData;

        int _fixupCount;

        int[]? _relocFixupList;
        int _relocFixupCount;

        int _exceptionCount;
        int _currExcStackCount;

        // the list of all of the exceptions in this IL stream
        __ExceptionInfo[]? _exceptions;

        // the stack of exceptions which we're currently in
        __ExceptionInfo[]? _currExcStack;

        /// <summary>
        /// Tracks all debugging scope information of the method body.
        /// </summary>
        internal ScopeTree ScopeTree;

        /// <summary>
        /// Tracks all line number information of the method body.
        /// </summary>
        internal LineNumberInfo LineNumberInfo;

        readonly MethodBuilder _methodBuilder;

        /// <summary>
        /// The number of local variables declared so far.
        /// </summary>
        internal int LocalCount;

        /// <summary>
        /// Builds the local variable signature of the method as local variables are declared.
        /// </summary>
        internal SignatureHelper LocalSignature;

        // current stack depth, with -1 meaning unknown
        int _curDepth;

        // stack depth at a target of the previous instruction (when it is branching)
        int _targetDepth;

        // running max of the stack depth
        int _maxDepth;

        // adjustment to add to _maxDepth for incorrect/invalid IL, for example when branch instructions with different
        // stack depths target the same label
        long _depthAdjustment;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="methodBuilder">The method whose body is generated.</param>
        /// <param name="size">The initial size of the IL stream, in bytes. Values smaller than the default size are raised to it.</param>
        internal ILGenerator(MethodBuilder methodBuilder, int size)
        {
            Debug.Assert(methodBuilder is not null);

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

        /// <summary>
        /// Records the current position in the IL stream as the location of a token that must be resolved when the
        /// module is written.
        /// </summary>
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

        /// <summary>
        /// Writes the opcode to the IL stream and updates the stack depth by the opcode's fixed stack change. The
        /// caller must ensure there is enough capacity.
        /// </summary>
        /// <param name="opcode">The opcode to write.</param>
        internal void InternalEmit(OpCode opcode)
        {
            var opcodeValue = opcode.Value;
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

        /// <summary>
        /// Updates the tracked stack depth requirements of the method for an instruction.
        /// </summary>
        /// <param name="opcode">The instruction that was emitted. If it cannot fall through, the current depth becomes unknown.</param>
        /// <param name="stackchange">The amount by which the stack depth changes.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void UpdateStackSize(OpCode opcode, int stackchange)
        {
            if (_curDepth < 0)
            {
                // current depth is "unknown". We get here when:
                // * this is unreachable code.
                // * the client uses explicit numeric offsets rather than Labels.
                _curDepth = 0;
            }

            _curDepth += stackchange;
            if (_curDepth < 0)
            {
                // stack underflow, assume our previous depth computation was flawed
                _depthAdjustment -= _curDepth;
                _curDepth = 0;
            }
            else if (_maxDepth < _curDepth)
                _maxDepth = _curDepth;
            Debug.Assert(_depthAdjustment >= 0);
            Debug.Assert(_curDepth >= 0);

            // record the stack depth at a "target" of this instruction
            _targetDepth = _curDepth;

            // if the current instruction can't fall through, set the depth to unknown
            if (opcode.EndsUncondJmpBlk())
                _curDepth = -1;
        }

        /// <summary>
        /// Gets the token to emit for a reference to a method or constructor.
        /// </summary>
        /// <param name="method">The method or constructor.</param>
        /// <param name="optionalParameterTypes">The types of the optional arguments of a varargs call, or <see langword="null"/>.</param>
        /// <returns>The token of the method, which may be a pseudo token.</returns>
        int GetMethodToken(MethodBase method, Type[]? optionalParameterTypes)
        {
            var mi = method is ConstructorInfo ctor ? ctor.GetMethodInfo() : (MethodInfo)method;
            if (optionalParameterTypes == null || optionalParameterTypes.Length == 0)
                return _methodBuilder.ModuleBuilder.GetMethodTokenForIL(mi).Token;
            else
                return _methodBuilder.ModuleBuilder.__GetMethodToken(mi, optionalParameterTypes, null).Token;
        }

        /// <summary>
        /// Builds the signature of a method reference, adding a sentinel and the optional argument types for a varargs
        /// call.
        /// </summary>
        /// <param name="call">The calling convention of the method.</param>
        /// <param name="returnType">The return type of the method.</param>
        /// <param name="parameterTypes">The types of the parameters of the method.</param>
        /// <param name="optionalParameterTypes">The types of the optional arguments of a varargs call, or <see langword="null"/>.</param>
        /// <returns>The signature helper holding the signature.</returns>
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

        /// <summary>
        /// Called by <see cref="MethodBuilder"/> to do all of the label fixups and return a new byte array representing
        /// the IL stream with labels resolved.
        /// </summary>
        /// <returns>The IL of the method body, or <see langword="null"/> if no IL was emitted.</returns>
        /// <exception cref="ArgumentException">An exception block is still open, or a label was used but never marked.</exception>
        /// <exception cref="NotSupportedException">A branch target is out of range for a one-byte branch instruction.</exception>
        internal byte[]? BakeByteArray()
        {
            if (_currExcStackCount != 0)
            {
                throw new ArgumentException("The IL Generator cannot be used while there are unclosed exceptions.");
            }

            if (_length == 0)
                return null;

            // allocate space for the new array
            var newBytes = new byte[_length];

            // copy the data from the old array
            Array.Copy(_ilStream, newBytes, _length);

            // do the fixups: this involves iterating over all of the labels and replacing them with their proper values
            for (var i = 0; i < _fixupCount; i++)
            {
                __FixupData fixupData = _fixupData![i];
                var updateAddr = GetLabelPos(fixupData.FixupLabel) - (fixupData.FixupPos + fixupData.FixupInstSize);

                // handle single byte instructions, throwing an exception if they're trying to store a jump in a single
                // byte instruction that doesn't fit
                if (fixupData.FixupInstSize == 1)
                {
                    // verify that our one-byte arg will fit into a signed byte
                    if (updateAddr < sbyte.MinValue || updateAddr > sbyte.MaxValue)
                    {
                        throw new NotSupportedException(string.Format("Illegal one-byte branch at position: {0}. Requested branch was: {1}.", fixupData.FixupPos, updateAddr));
                    }

                    // place the one-byte arg
                    newBytes[fixupData.FixupPos] = (byte)updateAddr;
                }
                else
                {
                    // place the four-byte arg
                    BinaryPrimitives.WriteInt32LittleEndian(newBytes.AsSpan(fixupData.FixupPos), updateAddr);
                }
            }
            return newBytes;
        }

        /// <summary>
        /// Returns the exception blocks of the method body, sorted so that inner blocks come before the blocks that
        /// enclose them.
        /// </summary>
        /// <returns>The exception blocks, or <see langword="null"/> if there are none.</returns>
        /// <exception cref="NotSupportedException">An exception block is still open.</exception>
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

        /// <summary>
        /// Guarantees that the IL stream can hold at least <paramref name="size"/> more bytes.
        /// </summary>
        /// <param name="size">The number of bytes about to be written.</param>
        internal void EnsureCapacity(int size)
        {
            if (_length + size >= _ilStream.Length)
            {
                IncreaseCapacity(size);
            }
        }

        /// <summary>
        /// Grows the IL stream to at least double its size, or to fit <paramref name="size"/> more bytes if that is
        /// larger.
        /// </summary>
        /// <param name="size">The number of bytes about to be written.</param>
        void IncreaseCapacity(int size)
        {
            var temp = new byte[Math.Max(_ilStream.Length * 2, _length + size)];
            Array.Copy(_ilStream, temp, _ilStream.Length);
            _ilStream = temp;
        }

        /// <summary>
        /// Writes a 32 bit little-endian integer to the IL stream. The caller must ensure there is enough capacity.
        /// </summary>
        /// <param name="value">The value to write.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void PutInteger4(int value)
        {
            BinaryPrimitives.WriteInt32LittleEndian(_ilStream.AsSpan(_length), value);
            _length += 4;
        }

        /// <summary>
        /// Gets the position in the stream of a particular label, verifying that the label exists and that it has been
        /// given a value.
        /// </summary>
        /// <param name="lbl">The label.</param>
        /// <returns>The offset in the IL stream at which the label was marked.</returns>
        /// <exception cref="ArgumentException">The label was not defined by this generator, or has not been marked.</exception>
        int GetLabelPos(Label lbl)
        {
            var index = lbl.Id;

            if (index < 0 || index >= _labelCount || _labelList is null)
                throw new ArgumentException("Bad label in ILGenerator.");

            var pos = _labelList[index].Pos;
            if (pos < 0)
                throw new ArgumentException("Bad label content in ILGenerator.");

            return pos;
        }

        /// <summary>
        /// Notes the label, position, and instruction size of a new fixup, expanding the fixup array as appropriate,
        /// and records the stack depth at the branch for the label.
        /// </summary>
        /// <param name="lbl">The label the branch targets.</param>
        /// <param name="pos">The offset in the IL stream of the branch operand.</param>
        /// <param name="instSize">The size of the branch operand, which is also the distance from the operand to the end of the instruction (or of the switch table).</param>
        /// <exception cref="ArgumentException">The label was not defined by this generator.</exception>
        void AddFixup(Label lbl, int pos, int instSize)
        {
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

            var labelIndex = lbl.Id;
            if (labelIndex < 0 || labelIndex >= _labelCount || _labelList is null)
                throw new ArgumentException("Bad label in ILGenerator.");

            var depth = _labelList[labelIndex].Depth;
            var targetDepth = _targetDepth;
            Debug.Assert(depth >= -1);
            Debug.Assert(targetDepth >= -1);
            if (depth < targetDepth)
            {
                // either unknown depth for this label or this branch location has a larger depth than previously
                // recorded; in the latter case, the IL is (likely) invalid, but we just compensate for it
                if (depth >= 0)
                    _depthAdjustment += targetDepth - depth;
                _labelList[labelIndex].Depth = targetDepth;
            }
        }

        /// <summary>
        /// Gets the maximum stack depth of the method body, including the adjustments made for invalid IL.
        /// </summary>
        /// <returns>The maximum stack depth, limited to 2^16 - 1, since the value is taken modulo 2^16 by other code.</returns>
        internal int GetMaxStackSize()
        {
            Debug.Assert(_depthAdjustment >= 0);
            return (int)Math.Min(ushort.MaxValue, _maxDepth + _depthAdjustment);
        }

        /// <summary>
        /// Sorts the exception blocks so that inner blocks come before the blocks that enclose them, which is the order
        /// in which the exception clauses must appear.
        /// </summary>
        /// <remarks>
        /// This is just a cheap selection sort. We don't expect many exceptions (&lt;10), where it beats QuickSort. If
        /// we have more exceptions than this in real life, we should consider moving to a QuickSort.
        /// </remarks>
        /// <param name="exceptions">The exception blocks to sort in place.</param>
        static void SortExceptions(__ExceptionInfo[] exceptions)
        {
            for (var i = 0; i < exceptions.Length; i++)
            {
                var least = i;
                for (var j = i + 1; j < exceptions.Length; j++)
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

        /// <summary>
        /// Returns the offsets in the IL stream of the tokens that must be resolved when the module is written.
        /// </summary>
        /// <returns>The offsets of the tokens, or <see langword="null"/> if there are none.</returns>
        internal int[]? GetTokenFixups()
        {
            if (_relocFixupCount == 0)
            {
                Debug.Assert(_relocFixupList == null);
                return null;
            }

            var narrowTokens = new int[_relocFixupCount];
            Array.Copy(_relocFixupList!, narrowTokens, _relocFixupCount);
            return narrowTokens;
        }

        #endregion

        #region Public Members

        #region Emit

        /// <summary>
        /// Puts the specified instruction onto the stream of instructions.
        /// </summary>
        /// <param name="opcode">The Microsoft Intermediate Language (MSIL) instruction to be put onto the stream.</param>
        public void Emit(OpCode opcode)
        {
            EnsureCapacity(3);
            InternalEmit(opcode);
        }

        /// <summary>
        /// Puts the specified instruction and character argument onto the Microsoft intermediate language (MSIL) stream
        /// of instructions.
        /// </summary>
        /// <param name="opcode">The MSIL instruction to be put onto the stream.</param>
        /// <param name="arg">The character argument pushed onto the stream immediately after the instruction.</param>
        public void Emit(OpCode opcode, byte arg)
        {
            EnsureCapacity(4);
            InternalEmit(opcode);
            _ilStream[_length++] = arg;
        }

        /// <summary>
        /// Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream
        /// of instructions.
        /// </summary>
        /// <param name="opcode">The MSIL instruction to be emitted onto the stream.</param>
        /// <param name="arg">The <see langword="short"/> argument pushed onto the stream immediately after the instruction.</param>
        public void Emit(OpCode opcode, short arg)
        {
            EnsureCapacity(5);
            InternalEmit(opcode);
            BinaryPrimitives.WriteInt16LittleEndian(_ilStream.AsSpan(_length), arg);
            _length += 2;
        }

        /// <summary>
        /// Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream
        /// of instructions. <c>ldc.i4</c>, <c>ldarg</c>, <c>ldarga</c> and <c>starg</c> are replaced by their shortest
        /// form for the argument.
        /// </summary>
        /// <param name="opcode">The MSIL instruction to be put onto the stream.</param>
        /// <param name="arg">The numerical argument pushed onto the stream immediately after the instruction.</param>
        public void Emit(OpCode opcode, int arg)
        {
            // special-case several opcodes that have shorter variants for common values
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

                // this will be true except on misuse of the opcode
                if ((uint)arg <= ushort.MaxValue)
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

                // this will be true except on misuse of the opcode
                if ((uint)arg <= ushort.MaxValue)
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

                // this will be true except on misuse of the opcode
                if ((uint)arg <= ushort.MaxValue)
                {
                    Emit(OpCodes.Starg, (short)arg);
                    return;
                }
            }

            // for everything else, put the opcode followed by the arg onto the stream of instructions
            EnsureCapacity(7);
            InternalEmit(opcode);
            PutInteger4(arg);
        }

        /// <summary>
        /// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream followed by the
        /// metadata token for the given method. <c>call</c>, <c>callvirt</c> and <c>newobj</c> are emitted as by
        /// <see cref="EmitCall"/>.
        /// </summary>
        /// <param name="opcode">The MSIL instruction to be emitted onto the stream.</param>
        /// <param name="meth">A <see cref="MethodInfo"/> representing a method.</param>
        /// <exception cref="ArgumentNullException"><paramref name="meth"/> is <see langword="null"/>.</exception>
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
                // reflection doesn't distinguish between these two concepts:
                //   1. A generic method definition: Foo`1
                //   2. A generic method definition instantiated over its own generic arguments: Foo`1<!!0>
                // in RefEmit, we always want 1 for Ld* opcodes and 2 for Call* and Newobj
                var tk = GetMethodToken(meth, null);

                EnsureCapacity(7);
                InternalEmit(opcode);

                UpdateStackSize(opcode, 0);
                RecordTokenFixup();
                PutInteger4(tk);
            }
        }

        /// <summary>
        /// Puts a <see cref="OpCodes.Calli"/> instruction onto the Microsoft intermediate language (MSIL) stream,
        /// specifying a managed calling convention for the indirect call.
        /// </summary>
        /// <param name="opcode">The MSIL instruction to be emitted onto the stream. Not used; <see cref="OpCodes.Calli"/> is always emitted.</param>
        /// <param name="callingConvention">The managed calling convention to be used.</param>
        /// <param name="returnType">The <see cref="Type"/> of the result.</param>
        /// <param name="parameterTypes">The types of the required arguments to the instruction.</param>
        /// <param name="optionalParameterTypes">The types of the optional arguments for varargs calls.</param>
        /// <exception cref="InvalidOperationException"><paramref name="optionalParameterTypes"/> is not <see langword="null"/>, but <paramref name="callingConvention"/> does not include <see cref="CallingConventions.VarArgs"/>.</exception>
        public void EmitCalli(OpCode opcode, CallingConventions callingConvention,
            Type? returnType, Type[]? parameterTypes, Type[]? optionalParameterTypes)
        {
            var stackchange = 0;
            if (optionalParameterTypes != null)
            {
                if ((callingConvention & CallingConventions.VarArgs) == 0)
                {
                    // client should not supply optional parameter in default calling convention
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

            // if there is a non-void return type, push one
            if (returnType != modBuilder.Universe.System_Void)
                stackchange++;
            // pop off arguments if any
            if (parameterTypes != null)
                stackchange -= parameterTypes.Length;
            // pop off vararg arguments
            if (optionalParameterTypes != null)
                stackchange -= optionalParameterTypes.Length;
            // pop the this parameter if the method has a this parameter
            if ((callingConvention & CallingConventions.HasThis) == CallingConventions.HasThis)
                stackchange--;
            // pop the native function pointer
            stackchange--;
            UpdateStackSize(OpCodes.Calli, stackchange);

            RecordTokenFixup();
            PutInteger4(modBuilder.GetSignatureToken(sig).Token);
        }

        /// <summary>
        /// Puts a <see cref="OpCodes.Calli"/> instruction onto the Microsoft intermediate language (MSIL) stream,
        /// specifying an unmanaged calling convention for the indirect call.
        /// </summary>
        /// <param name="opcode">The MSIL instruction to be emitted onto the stream. Not used; <see cref="OpCodes.Calli"/> is always emitted.</param>
        /// <param name="unmanagedCallConv">The unmanaged calling convention to be used.</param>
        /// <param name="returnType">The <see cref="Type"/> of the result.</param>
        /// <param name="parameterTypes">The types of the required arguments to the instruction.</param>
        public void EmitCalli(OpCode opcode, CallingConvention unmanagedCallConv, Type? returnType, Type[]? parameterTypes)
        {
            var stackchange = 0;
            var cParams = 0;

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
                for (var i = 0; i < cParams; i++)
                {
                    sig.AddArgument(parameterTypes[i]);
                }
            }

            // if there is a non-void return type, push one
            if (returnType != modBuilder.Universe.System_Void)
                stackchange++;

            // pop off arguments if any
            if (parameterTypes != null)
                stackchange -= cParams;

            // pop the native function pointer
            stackchange--;
            UpdateStackSize(OpCodes.Calli, stackchange);

            EnsureCapacity(7);
            Emit(OpCodes.Calli);
            RecordTokenFixup();
            PutInteger4(modBuilder.GetSignatureToken(sig).Token);
        }

        /// <summary>
        /// Puts a <c>call</c>, <c>callvirt</c> or <c>newobj</c> instruction onto the Microsoft intermediate language
        /// (MSIL) stream to call a method, which may be a varargs method.
        /// </summary>
        /// <param name="opcode">The MSIL instruction to be emitted onto the stream. Must be <see cref="OpCodes.Call"/>, <see cref="OpCodes.Callvirt"/>, or <see cref="OpCodes.Newobj"/>.</param>
        /// <param name="methodInfo">The method to be called.</param>
        /// <param name="optionalParameterTypes">The types of the optional arguments if the method is a varargs method; otherwise, <see langword="null"/>.</param>
        /// <exception cref="ArgumentNullException"><paramref name="methodInfo"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="opcode"/> does not specify a method call.</exception>
        public void EmitCall(OpCode opcode, MethodInfo methodInfo, Type[]? optionalParameterTypes)
        {
            if (methodInfo is null)
                throw new ArgumentNullException(nameof(methodInfo));

            if (!(opcode.Equals(OpCodes.Call) || opcode.Equals(OpCodes.Callvirt) || opcode.Equals(OpCodes.Newobj)))
                throw new ArgumentException("The specified opcode cannot be passed to EmitCall.", nameof(opcode));

            var stackchange = 0;
            var tk = GetMethodToken(methodInfo, optionalParameterTypes);

            EnsureCapacity(7);
            InternalEmit(opcode);

            // push the return value if there is one
            if (methodInfo.ReturnType != _methodBuilder.ModuleBuilder.Universe.System_Void)
                stackchange++;
            // pop the parameters
            Type[] parameters = methodInfo.GetParameterTypes();
            if (parameters != null)
                stackchange -= parameters.Length;

            // pop the this parameter if the method is non-static and the instruction is not newobj
            if (!methodInfo.IsStatic && !opcode.Equals(OpCodes.Newobj))
                stackchange--;
            // pop the optional parameters off the stack
            if (optionalParameterTypes != null)
                stackchange -= optionalParameterTypes.Length;
            UpdateStackSize(opcode, stackchange);

            RecordTokenFixup();
            PutInteger4(tk);
        }

        /// <summary>
        /// Puts the specified instruction and metadata token for the specified constructor onto the Microsoft
        /// intermediate language (MSIL) stream of instructions.
        /// </summary>
        /// <param name="opcode">The MSIL instruction to be emitted onto the stream.</param>
        /// <param name="con">A <see cref="ConstructorInfo"/> representing a constructor.</param>
        /// <exception cref="ArgumentNullException"><paramref name="con"/> is <see langword="null"/>.</exception>
        public void Emit(OpCode opcode, ConstructorInfo con)
        {
            if (con is null)
                throw new ArgumentNullException(nameof(con));

            var stackchange = 0;

            var tk = GetMethodToken(con, null);

            EnsureCapacity(7);
            InternalEmit(opcode);

            // make a conservative estimate by assuming a return type and no this parameter
            if (opcode.StackBehaviourPush == StackBehaviour.Varpush)
            {
                // instruction must be one of call or callvirt
                Debug.Assert(opcode.Equals(OpCodes.Call) ||
                                opcode.Equals(OpCodes.Callvirt),
                                "Unexpected opcode encountered for StackBehaviour of VarPush.");
                stackchange++;
            }
            if (opcode.StackBehaviourPop == StackBehaviour.Varpop)
            {
                // instruction must be one of call, callvirt or newobj
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

        /// <summary>
        /// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream followed by the
        /// metadata token for the given type. The location of the token is recorded so that it can be patched when the
        /// module is written.
        /// </summary>
        /// <param name="opcode">The MSIL instruction to be put onto the stream.</param>
        /// <param name="cls">A <see cref="Type"/>.</param>
        public void Emit(OpCode opcode, Type cls)
        {
            ModuleBuilder modBuilder = (ModuleBuilder)_methodBuilder.Module;
            var getGenericDefinition = (opcode == OpCodes.Ldtoken && cls != null && cls.IsGenericTypeDefinition);
            var tempVal = getGenericDefinition ? modBuilder.GetTypeToken(cls).Token : modBuilder.GetTypeTokenForMemberRef(cls);

            EnsureCapacity(7);
            InternalEmit(opcode);
            RecordTokenFixup();
            PutInteger4(tempVal);
        }

        /// <summary>
        /// Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream
        /// of instructions.
        /// </summary>
        /// <param name="opcode">The MSIL instruction to be put onto the stream.</param>
        /// <param name="arg">The numerical argument pushed onto the stream immediately after the instruction.</param>
        public void Emit(OpCode opcode, long arg)
        {
            EnsureCapacity(11);
            InternalEmit(opcode);
            BinaryPrimitives.WriteInt64LittleEndian(_ilStream.AsSpan(_length), arg);
            _length += 8;
        }

        /// <summary>
        /// Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream
        /// of instructions.
        /// </summary>
        /// <param name="opcode">The MSIL instruction to be put onto the stream.</param>
        /// <param name="arg">The <see langword="float"/> argument pushed onto the stream immediately after the instruction.</param>
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

        /// <summary>
        /// Puts the specified instruction and numerical argument onto the Microsoft intermediate language (MSIL) stream
        /// of instructions.
        /// </summary>
        /// <param name="opcode">The MSIL instruction to be put onto the stream.</param>
        /// <param name="arg">The numerical argument pushed onto the stream immediately after the instruction.</param>
        public void Emit(OpCode opcode, double arg)
        {
            EnsureCapacity(11);
            InternalEmit(opcode);
            BinaryPrimitives.WriteInt64LittleEndian(_ilStream.AsSpan(_length), BitConverter.DoubleToInt64Bits(arg));
            _length += 8;
        }

        /// <summary>
        /// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream and leaves space to
        /// include a label when fixes are done.
        /// </summary>
        /// <remarks>
        /// Labels are created using <see cref="DefineLabel()"/> and their location within the stream is fixed by using
        /// <see cref="MarkLabel"/>. If a single-byte instruction (designated by the <c>_S</c> suffix) is used, the
        /// label can represent a jump of at most 127 bytes along the stream. <paramref name="opcode"/> must represent a
        /// branch instruction (although this is not explicitly verified). Since branches are relative instructions, the
        /// label will be replaced with the correct offset to branch during the fixup process.
        /// </remarks>
        /// <param name="opcode">The MSIL instruction to be emitted onto the stream.</param>
        /// <param name="label">The label to which to branch from this location.</param>
        public void Emit(OpCode opcode, Label label)
        {
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

        /// <summary>
        /// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream and leaves space to
        /// include a label when fixes are done. This emits a switch table.
        /// </summary>
        /// <param name="opcode">The MSIL instruction to be emitted onto the stream.</param>
        /// <param name="labels">The array of label objects to which to branch from this location. All of the labels will be used.</param>
        /// <exception cref="ArgumentNullException"><paramref name="labels"/> is <see langword="null"/>.</exception>
        public void Emit(OpCode opcode, Label[] labels)
        {
            if (labels is null)
                throw new ArgumentNullException(nameof(labels));

            int i;

            // number of bytes remaining for this switch instruction to be subtracted for computing the offset
            int remaining;

            var count = labels.Length;

            EnsureCapacity(count * 4 + 7);
            InternalEmit(opcode);
            PutInteger4(count);
            for (remaining = count * 4, i = 0; remaining > 0; remaining -= 4, i++)
            {
                AddFixup(labels[i], _length, remaining);
                _length += 4;
            }
        }

        /// <summary>
        /// Puts the specified instruction and metadata token for the specified field onto the Microsoft intermediate
        /// language (MSIL) stream of instructions.
        /// </summary>
        /// <param name="opcode">The MSIL instruction to be emitted onto the stream.</param>
        /// <param name="field">A <see cref="FieldInfo"/> representing a field.</param>
        public void Emit(OpCode opcode, FieldInfo field)
        {
            ModuleBuilder modBuilder = (ModuleBuilder)_methodBuilder.Module;
            var tempVal = modBuilder.GetFieldToken(field).Token;
            EnsureCapacity(7);
            InternalEmit(opcode);
            RecordTokenFixup();
            PutInteger4(tempVal);
        }

        /// <summary>
        /// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream followed by the
        /// metadata token for the given string.
        /// </summary>
        /// <param name="opcode">The MSIL instruction to be emitted onto the stream.</param>
        /// <param name="str">The <see cref="string"/> to be emitted.</param>
        public void Emit(OpCode opcode, string str)
        {
            // the user string token is final, so no fixup is recorded for it
            ModuleBuilder modBuilder = (ModuleBuilder)_methodBuilder.Module;
            var tempVal = modBuilder.GetStringConstant(str).Token;
            EnsureCapacity(7);
            InternalEmit(opcode);
            PutInteger4(tempVal);
        }

        /// <summary>
        /// Puts the specified instruction onto the Microsoft intermediate language (MSIL) stream followed by the index
        /// of the given local variable. <c>ldloc</c>, <c>stloc</c> and <c>ldloca</c> are replaced by their shortest
        /// form for the index.
        /// </summary>
        /// <param name="opcode">The MSIL instruction to be emitted onto the stream.</param>
        /// <param name="local">A local variable.</param>
        /// <exception cref="ArgumentNullException"><paramref name="local"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException"><paramref name="local"/> was declared by a different method.</exception>
        /// <exception cref="InvalidOperationException"><paramref name="opcode"/> is a single-byte instruction, and <paramref name="local"/> has an index greater than 255.</exception>
        public void Emit(OpCode opcode, LocalBuilder local)
        {
            if (local is null)
                throw new ArgumentNullException(nameof(local));

            var tempVal = local.LocalIndex;
            if (local.Method != _methodBuilder)
            {
                throw new ArgumentException("Local passed in does not belong to this ILGenerator.", nameof(local));
            }
            // if the instruction is a ldloc, ldloca or stloc, morph it to the optimal form
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
                // a short form such as ldloc.s or stloc.s can only address the first 256 locals
                if (tempVal > byte.MaxValue)
                {
                    throw new InvalidOperationException("Opcodes using a short-form index cannot address a local position over 255.");
                }
                _ilStream[_length++] = (byte)tempVal;
            }
        }

        #endregion

        #region Exceptions

        /// <summary>
        /// Begins an exception block for a non-filtered exception. Creating an exception block records some
        /// information, but does not actually emit any IL onto the stream.
        /// </summary>
        /// <remarks>
        /// Exception blocks should be created and marked in the following form: emit some IL;
        /// <see cref="BeginExceptionBlock"/>; emit the IL which should appear within the try block;
        /// <see cref="BeginCatchBlock"/>; emit the IL which should appear within the catch block; optionally further
        /// <see cref="BeginCatchBlock"/> calls, which can be repeated an arbitrary number of times;
        /// <see cref="EndExceptionBlock"/>.
        /// </remarks>
        /// <returns>The label for the end of the block. This will leave you in the correct place to execute finally blocks or to finish the try.</returns>
        public Label BeginExceptionBlock()
        {
            // delay init
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

            // make this exception the current active exception
            _currExcStack[_currExcStackCount++] = exceptionInfo;

            // stack depth for "try" starts at zero
            _curDepth = 0;

            return endLabel;
        }

        /// <summary>
        /// Ends an exception block.
        /// </summary>
        /// <exception cref="NotSupportedException">The generator is not currently in an exception block.</exception>
        /// <exception cref="InvalidOperationException">The current exception block has no catch, fault or finally clause.</exception>
        public void EndExceptionBlock()
        {
            if (_currExcStackCount == 0)
            {
                throw new NotSupportedException("Not currently in an exception block.");
            }

            // pop the current exception block
            __ExceptionInfo current = _currExcStack![_currExcStackCount - 1];
            _currExcStack[--_currExcStackCount] = null!;

            Label endLabel = current.GetEndLabel();
            var state = current.GetCurrentState();

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

            // check if we've already set this label; the only reason why we might have set this is if we have a finally
            // block
            Label label = _labelList![endLabel.GetLabelValue()].Pos != -1
                ? current.FinallyEndLabel
                : endLabel;

            MarkLabel(label);

            current.Done(_length);
        }

        /// <summary>
        /// Begins a catch block. Emits a branch instruction to the end of the current exception block.
        /// </summary>
        /// <param name="exceptionType">The <see cref="Type"/> object that represents the exception, or <see langword="null"/> when the catch block follows a filter block.</param>
        /// <exception cref="NotSupportedException">The generator is not currently in an exception block.</exception>
        /// <exception cref="ArgumentException">The catch block follows a filter block and <paramref name="exceptionType"/> is not <see langword="null"/>.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="exceptionType"/> is <see langword="null"/> and the catch block does not follow a filter block.</exception>
        public void BeginCatchBlock(Type? exceptionType)
        {
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

            // stack depth for "catch" starts at one
            _curDepth = 1;
        }

        /// <summary>
        /// Begins an exception fault block in the Microsoft intermediate language (MSIL) stream.
        /// </summary>
        /// <exception cref="NotSupportedException">The generator is not currently in an exception block.</exception>
        public void BeginFaultBlock()
        {
            if (_currExcStackCount == 0)
            {
                throw new NotSupportedException("Not currently in an exception block.");
            }
            __ExceptionInfo current = _currExcStack![_currExcStackCount - 1];

            // emit the leave for the clause before this one
            Emit(OpCodes.Leave, current.GetEndLabel());

            current.MarkFaultAddr(_length);

            // stack depth for "fault" starts at zero
            _curDepth = 0;
        }

        /// <summary>
        /// Begins a finally block in the Microsoft intermediate language (MSIL) instruction stream.
        /// </summary>
        /// <exception cref="NotSupportedException">The generator is not currently in an exception block.</exception>
        /// <exception cref="ArgumentException">The current exception block already has a finally block.</exception>
        public void BeginFinallyBlock()
        {
            if (_currExcStackCount == 0)
            {
                throw new NotSupportedException("Not currently in an exception block.");
            }
            __ExceptionInfo current = _currExcStack![_currExcStackCount - 1];
            var state = current.GetCurrentState();
            Label endLabel = current.GetEndLabel();
            var catchEndAddr = 0;
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

            // stack depth for "finally" starts at zero
            _curDepth = 0;
        }

        #endregion

        #region Labels

        /// <summary>
        /// Declares a new label.
        /// </summary>
        /// <returns>A new label that can be used as a token for branching.</returns>
        public Label DefineLabel() =>
            // we don't know the stack depth at the label yet, so set it to -1
            DefineLabel(-1);

        /// <summary>
        /// Declares a new label with the specified stack depth. The label is just a token and does not yet represent
        /// any particular location within the stream; its position is set by <see cref="MarkLabel"/>.
        /// </summary>
        /// <param name="depth">The stack depth at the label, or -1 if it is not yet known.</param>
        /// <returns>The new label.</returns>
        Label DefineLabel(int depth)
        {
            Debug.Assert(depth >= -1);

            // delay init the label array in case we don't use it
            _labelList ??= new __LabelInfo[DefaultLabelArraySize];

            if (_labelCount >= _labelList.Length)
            {
                _labelList = EnlargeArray(_labelList);
            }
            _labelList[_labelCount].Pos = -1;
            _labelList[_labelCount].Depth = depth;
            return new Label(_labelCount++);
        }

        /// <summary>
        /// Marks the Microsoft intermediate language (MSIL) stream's current position with the given label. A label
        /// cannot be marked more than once.
        /// </summary>
        /// <param name="loc">The label for which to set an index.</param>
        /// <exception cref="ArgumentException"><paramref name="loc"/> does not belong to this generator, or has already been marked.</exception>
        public void MarkLabel(Label loc)
        {
            var labelIndex = loc.Id;

            // this should only happen if a label from another generator is used with this one
            if (_labelList is null || labelIndex < 0 || labelIndex >= _labelList.Length)
            {
                throw new ArgumentException("Invalid Label.");
            }

            if (_labelList[labelIndex].Pos != -1)
            {
                throw new ArgumentException("Label defined multiple times.");
            }

            _labelList[labelIndex].Pos = _length;

            var depth = _labelList[labelIndex].Depth;
            if (depth < 0)
            {
                // unknown depth for this label, indicating that it hasn't been used yet. If _curDepth is unknown, we're
                // in the backward branch constraint case, see ECMA-335 III.1.7.5. The _depthAdjustment field will
                // compensate for violations of this constraint, as we discover them. That is, here we assume a depth of
                // zero. If a (later) branch to this label has a positive stack depth, we'll record that as the new depth
                // and add the delta into _depthAdjustment.
                if (_curDepth < 0)
                    _curDepth = 0;
                _labelList[labelIndex].Depth = _curDepth;
            }
            else if (depth < _curDepth)
            {
                // a branch location with smaller stack targets this label; in this case, the IL is invalid, but we just
                // compensate for it
                _depthAdjustment += _curDepth - depth;
                _labelList[labelIndex].Depth = _curDepth;
            }
            else if (depth > _curDepth)
            {
                // either the current depth is unknown, or a branch location with larger stack targets this label, so
                // the IL is invalid; in either case, just adjust the current depth
                _curDepth = depth;
            }
        }

        #endregion

        #region Debug API

        /// <summary>
        /// Declares a local variable of the specified type.
        /// </summary>
        /// <param name="localType">A <see cref="Type"/> object that represents the type of the local variable.</param>
        /// <returns>The declared local variable.</returns>
        public LocalBuilder DeclareLocal(Type localType) => DeclareLocal(localType, false);

        /// <summary>
        /// Declares a local variable of the specified type, optionally pinning the object referred to by the variable.
        /// </summary>
        /// <param name="localType">A <see cref="Type"/> object that represents the type of the local variable.</param>
        /// <param name="pinned"><see langword="true"/> to pin the object in memory; otherwise, <see langword="false"/>.</param>
        /// <returns>A <see cref="LocalBuilder"/> object that represents the local variable.</returns>
        /// <exception cref="InvalidOperationException">The type that declares the method has been created, or the method has been completed.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="localType"/> is <see langword="null"/>.</exception>
        public LocalBuilder DeclareLocal(Type localType, bool pinned)
        {
            if (_methodBuilder.IsTypeCreated())
            {
                // cannot change method after its containing type has been created
                throw new InvalidOperationException("Unable to change after type has been created.");
            }

            if (localType is null)
                throw new ArgumentNullException(nameof(localType));

            if (_methodBuilder.IsBaked)
            {
                throw new InvalidOperationException("Type definition of the method is complete.");
            }

            // add the localType to local signature
            LocalSignature.AddArgument(localType, pinned);

            return new LocalBuilder(_methodBuilder, localType, LocalCount++, pinned);
        }

        /// <summary>
        /// Begins a lexical scope.
        /// </summary>
        public void BeginScope() => ScopeTree.AddScopeInfo(ScopeAction.Open, _length);

        /// <summary>
        /// Ends a lexical scope.
        /// </summary>
        /// <exception cref="ArgumentException">There is no open scope to end.</exception>
        public void EndScope() => ScopeTree.AddScopeInfo(ScopeAction.Close, _length);

        /// <summary>
        /// Marks a sequence point in the Microsoft intermediate language (MSIL) stream.
        /// </summary>
        /// <param name="document">The document for which the sequence point is being defined.</param>
        /// <param name="startLine">The line where the sequence point begins. Line numbers are 1 based.</param>
        /// <param name="startColumn">The column in the line where the sequence point begins. Column numbers are 0 based.</param>
        /// <param name="endLine">The line where the sequence point ends. Line numbers are 1 based.</param>
        /// <param name="endColumn">The column in the line where the sequence point ends. Column numbers are 0 based.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="startLine"/> or <paramref name="endLine"/> is less than or equal to zero.</exception>
        public virtual void MarkSequencePoint(
            ISymbolDocumentWriter document,
            int startLine,
            int startColumn,
            int endLine,
            int endColumn)
        {
            if (startLine == 0 || startLine < 0 || endLine == 0 || endLine < 0)
            {
                throw new ArgumentOutOfRangeException("startLine");
            }
            Contract.EndContractBlock();
            LineNumberInfo.AddLineNumberInfo(document, _length, startLine, startColumn, endLine, endColumn);
        }

        /// <summary>
        /// Gets the current offset, in bytes, in the Microsoft intermediate language (MSIL) stream that is being
        /// emitted by the <see cref="ILGenerator"/>.
        /// </summary>
        public int ILOffset => _length;

        /// <summary>
        /// Puts the specified instruction and character argument onto the Microsoft intermediate language (MSIL) stream
        /// of instructions.
        /// </summary>
        /// <param name="opcode">The MSIL instruction to be put onto the stream.</param>
        /// <param name="arg">The character argument pushed onto the stream immediately after the instruction.</param>
        public void Emit(OpCode opcode, sbyte arg) => Emit(opcode, (byte)arg);

        #endregion

        #endregion

    }

    /// <summary>
    /// Records the position and stack depth of a label defined by an <see cref="ILGenerator"/>.
    /// </summary>
    struct __LabelInfo
    {

        /// <summary>
        /// The position of the label in the IL stream, with -1 meaning it has not been marked yet.
        /// </summary>
        internal int Pos;

        /// <summary>
        /// The stack depth at the label, with -1 meaning unknown.
        /// </summary>
        internal int Depth;

    }

    /// <summary>
    /// Records a branch operand in the IL stream that must be replaced by the offset of its target label.
    /// </summary>
    struct __FixupData
    {

        /// <summary>
        /// The label the branch targets.
        /// </summary>
        internal Label FixupLabel;

        /// <summary>
        /// The offset of the branch operand in the IL stream.
        /// </summary>
        internal int FixupPos;

        /// <summary>
        /// The size of the branch operand, which is also the distance from the operand to the point the branch offset
        /// is relative to.
        /// </summary>
        internal int FixupInstSize;

    }

    /// <summary>
    /// Tracks an exception block of an <see cref="ILGenerator"/>: the try block and the offsets, kinds and exception
    /// types of its clauses.
    /// </summary>
    sealed class __ExceptionInfo
    {

        /// <summary>
        /// A typed catch clause (COR_ILEXCEPTION_CLAUSE_NONE).
        /// </summary>
        internal const int None = 0x0000;

        /// <summary>
        /// A filter clause (COR_ILEXCEPTION_CLAUSE_FILTER).
        /// </summary>
        internal const int Filter = 0x0001;

        /// <summary>
        /// A finally clause (COR_ILEXCEPTION_CLAUSE_FINALLY).
        /// </summary>
        internal const int Finally = 0x0002;

        /// <summary>
        /// A fault clause (COR_ILEXCEPTION_CLAUSE_FAULT).
        /// </summary>
        internal const int Fault = 0x0004;

        /// <summary>
        /// The generator is in the try block.
        /// </summary>
        internal const int State_Try = 0;

        /// <summary>
        /// The generator is in a filter block.
        /// </summary>
        internal const int State_Filter = 1;

        /// <summary>
        /// The generator is in a catch block.
        /// </summary>
        internal const int State_Catch = 2;

        /// <summary>
        /// The generator is in the finally block.
        /// </summary>
        internal const int State_Finally = 3;

        /// <summary>
        /// The generator is in a fault block.
        /// </summary>
        internal const int State_Fault = 4;

        /// <summary>
        /// The exception block has been ended.
        /// </summary>
        internal const int State_Done = 5;

        readonly int _startAddr;
        int[] _filterAddr;
        int[] _catchAddr;
        int[] _catchEndAddr;
        int[] _type;
        Type[] _catchClass;
        readonly Label _endLabel;

        /// <summary>
        /// The label marked at the end of the finally block, if the exception block has one.
        /// </summary>
        internal Label FinallyEndLabel;

        int _endAddr;
        int _endFinally;
        int _currentCatch;

        int _currentState;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="startAddr">The offset in the IL stream at which the try block starts.</param>
        /// <param name="endLabel">The label for the end of the exception block.</param>
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

        /// <summary>
        /// Records the start of a clause, ending the previous clause, and ends the try block if this is the first
        /// clause.
        /// </summary>
        /// <param name="catchorfilterAddr">The starting address of the clause.</param>
        /// <param name="catchEndAddr">The end address of the previous catch clause. Only used when a finally clause follows a catch clause.</param>
        /// <param name="catchClass">The exception type of a catch clause, or <see langword="null"/>.</param>
        /// <param name="type">The kind of clause.</param>
        void MarkHelper(
            int catchorfilterAddr,
            int catchEndAddr,
            Type? catchClass,
            int type)
        {
            var currentCatch = _currentCatch;
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
                // catch or fault clause
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

        /// <summary>
        /// Records the start of a fault clause.
        /// </summary>
        /// <param name="faultAddr">The offset in the IL stream at which the fault clause starts.</param>
        internal void MarkFaultAddr(int faultAddr)
        {
            _currentState = State_Fault;
            MarkHelper(faultAddr, faultAddr, null, Fault);
        }

        /// <summary>
        /// Records the start of a catch clause.
        /// </summary>
        /// <param name="catchAddr">The offset in the IL stream at which the catch clause starts.</param>
        /// <param name="catchException">The type of exception caught, or <see langword="null"/> for the handler of a filter.</param>
        internal void MarkCatchAddr(int catchAddr, Type? catchException)
        {
            _currentState = State_Catch;
            MarkHelper(catchAddr, catchAddr, catchException, None);
        }

        /// <summary>
        /// Records the start of the finally clause.
        /// </summary>
        /// <param name="finallyAddr">The offset in the IL stream at which the finally clause starts.</param>
        /// <param name="endCatchAddr">The offset in the IL stream at which the preceding clause ends.</param>
        /// <exception cref="ArgumentException">The exception block already has a finally clause.</exception>
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

        /// <summary>
        /// Ends the exception block, recording the end of its last clause.
        /// </summary>
        /// <param name="endAddr">The offset in the IL stream at which the exception block ends.</param>
        internal void Done(int endAddr)
        {
            Debug.Assert(_currentCatch > 0, "m_currentCatch > 0");
            Debug.Assert(_catchAddr[_currentCatch - 1] > 0, "m_catchAddr[m_currentCatch-1] > 0");
            Debug.Assert(_catchEndAddr[_currentCatch - 1] == -1, "m_catchEndAddr[m_currentCatch-1] == -1");
            _catchEndAddr[_currentCatch - 1] = endAddr;
            _currentState = State_Done;
        }

        /// <summary>
        /// Gets the offset in the IL stream at which the try block starts.
        /// </summary>
        /// <returns>The start offset of the try block.</returns>
        internal int GetStartAddress() => _startAddr;

        /// <summary>
        /// Gets the offset in the IL stream at which the try block ends, which is the start of the first clause.
        /// </summary>
        /// <returns>The end offset of the try block, or -1 if no clause has been started.</returns>
        internal int GetEndAddress() => _endAddr;

        /// <summary>
        /// Gets the offset in the IL stream at which the finally clause starts.
        /// </summary>
        /// <returns>The start offset of the finally clause, or -1 if there is none.</returns>
        internal int GetFinallyEndAddress() => _endFinally;

        /// <summary>
        /// Gets the label for the end of the exception block.
        /// </summary>
        /// <returns>The end label.</returns>
        internal Label GetEndLabel() => _endLabel;

        /// <summary>
        /// Gets the start offsets of the filter blocks, indexed by clause.
        /// </summary>
        /// <returns>The filter offsets. The array may be longer than the number of clauses.</returns>
        internal int[] GetFilterAddresses() => _filterAddr;

        /// <summary>
        /// Gets the start offsets of the handler blocks, indexed by clause.
        /// </summary>
        /// <returns>The handler offsets. The array may be longer than the number of clauses.</returns>
        internal int[] GetCatchAddresses() => _catchAddr;

        /// <summary>
        /// Gets the end offsets of the handler blocks, indexed by clause.
        /// </summary>
        /// <returns>The handler end offsets. The array may be longer than the number of clauses.</returns>
        internal int[] GetCatchEndAddresses() => _catchEndAddr;

        /// <summary>
        /// Gets the exception types of the catch clauses, indexed by clause.
        /// </summary>
        /// <returns>The exception types, with <see langword="null"/> for clauses that are not typed catch clauses. The array may be longer than the number of clauses.</returns>
        internal Type[] GetCatchClass() => _catchClass;

        /// <summary>
        /// Gets the number of clauses of the exception block.
        /// </summary>
        /// <returns>The number of catch, filter, fault and finally clauses.</returns>
        internal int GetNumberOfCatches() => _currentCatch;

        /// <summary>
        /// Gets the kinds of the clauses, indexed by clause.
        /// </summary>
        /// <returns>The clause kinds, one of <see cref="None"/>, <see cref="Filter"/>, <see cref="Finally"/> or <see cref="Fault"/>. The array may be longer than the number of clauses.</returns>
        internal int[] GetExceptionTypes() => _type;

        /// <summary>
        /// Sets the label marked at the end of the finally block.
        /// </summary>
        /// <param name="lbl">The label.</param>
        internal void SetFinallyEndLabel(Label lbl) => FinallyEndLabel = lbl;

        /// <summary>
        /// Specifies whether <paramref name="exc"/> is an inner exception block of this one. This is determined by
        /// comparing the end address of the last clause of both blocks; if they are the same, the end addresses of the
        /// try blocks are compared.
        /// </summary>
        /// <remarks>
        /// WARNING: This is not a generic function to determine the innerness of an exception. This is somewhat of a
        /// misnomer. This gives a random result for cases where the two exceptions being compared do not have a nesting
        /// relation.
        /// </remarks>
        /// <param name="exc">The exception block to compare with.</param>
        /// <returns><see langword="true"/> if <paramref name="exc"/> is nested inside this exception block; otherwise, <see langword="false"/>.</returns>
        internal bool IsInner(__ExceptionInfo exc)
        {
            Debug.Assert(exc != null);
            Debug.Assert(_currentCatch > 0, "m_currentCatch > 0");
            Debug.Assert(exc._currentCatch > 0, "exc.m_currentCatch > 0");

            var exclast = exc._currentCatch - 1;
            var last = _currentCatch - 1;

            if (exc._catchEndAddr[exclast] < _catchEndAddr[last])
                return true;

            if (exc._catchEndAddr[exclast] != _catchEndAddr[last])
                return false;
            Debug.Assert(exc.GetEndAddress() != GetEndAddress(),
                "exc.GetEndAddress() != GetEndAddress()");

            return exc.GetEndAddress() > GetEndAddress();
        }

        /// <summary>
        /// Gets the part of the exception block the generator is currently in.
        /// </summary>
        /// <returns>One of <see cref="State_Try"/>, <see cref="State_Filter"/>, <see cref="State_Catch"/>, <see cref="State_Finally"/>, <see cref="State_Fault"/> or <see cref="State_Done"/>.</returns>
        internal int GetCurrentState() => _currentState;

    }

    /// <summary>
    /// The actions recorded by a <see cref="ScopeTree"/>. The value of each action is the change it makes to the number
    /// of closes still to be matched when walking the actions backwards.
    /// </summary>
    enum ScopeAction : sbyte
    {

        /// <summary>
        /// Opens a lexical scope.
        /// </summary>
        Open = -0x1,

        /// <summary>
        /// Closes a lexical scope.
        /// </summary>
        Close = 0x1

    }

    /// <summary>
    /// Tracks the scope structure within a method body. It keeps two parallel arrays: one of actions, each of which
    /// opens or closes a scope, and one of the offsets at which the actions take place. The local variable symbol
    /// information of each scope is kept alongside.
    /// </summary>
    sealed class ScopeTree
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal ScopeTree()
        {
            // initialize data variables
            OpenScopeCount = 0;
            _count = 0;
        }

        /// <summary>
        /// Finds the current active lexical scope. For example, if we have "Open Open Open Close", we will return 1 as
        /// the second BeginScope is currently active.
        /// </summary>
        /// <returns>The index of the action that opened the current active scope, or -1 if no actions were recorded.</returns>
        internal int GetCurrentActiveScopeIndex()
        {
            if (_count == 0)
            {
                return -1;
            }

            var i = _count - 1;

            for (var cClose = 0; cClose > 0 || _scopeActions[i] == ScopeAction.Close; i--)
            {
                cClose += (sbyte)_scopeActions[i];
            }

            return i;
        }

        /// <summary>
        /// Adds symbol information for a local variable to the current active lexical scope.
        /// </summary>
        /// <param name="strName">The name of the local variable.</param>
        /// <param name="signature">The signature of the local variable.</param>
        /// <param name="slot">The index of the local variable.</param>
        /// <param name="startOffset">The offset in the IL stream at which the local variable comes into scope.</param>
        /// <param name="endOffset">The offset in the IL stream at which the local variable goes out of scope.</param>
        internal void AddLocalSymInfoToCurrentScope(
            string strName,
            byte[] signature,
            int slot,
            int startOffset,
            int endOffset)
        {
            var i = GetCurrentActiveScopeIndex();
            _localSymInfos[i] ??= new LocalSymInfo();
            _localSymInfos[i]!.AddLocalSymInfo(strName, signature, slot, startOffset, endOffset);
        }

        /// <summary>
        /// Records the opening or closing of a scope.
        /// </summary>
        /// <param name="sa">Whether a scope is opened or closed.</param>
        /// <param name="iOffset">The offset in the IL stream at which the action takes place.</param>
        /// <exception cref="ArgumentException"><paramref name="sa"/> closes a scope, but no scope is open.</exception>
        internal void AddScopeInfo(ScopeAction sa, int iOffset)
        {
            if (sa == ScopeAction.Close && OpenScopeCount <= 0)
            {
                throw new ArgumentException("Non-matching symbol scope.");
            }

            // make sure that arrays are large enough to hold additional info
            EnsureCapacity();

            _scopeActions[_count] = sa;
            _offsets[_count] = iOffset;
            _localSymInfos[_count] = null;
            checked
            { _count++; }

            OpenScopeCount += -(sbyte)sa;
        }

        /// <summary>
        /// Ensures the arrays are large enough to hold one more entry.
        /// </summary>
        internal void EnsureCapacity()
        {
            if (_count == 0)
            {
                // first time, allocate the arrays
                _offsets = new int[InitialSize];
                _scopeActions = new ScopeAction[InitialSize];
                _localSymInfos = new LocalSymInfo[InitialSize];
            }
            else if (_count == _offsets.Length)
            {
                // the arrays are full, enlarge the arrays; it would probably be simpler to just use Lists here
                var newSize = checked(_count * 2);
                var temp = new int[newSize];
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

        /// <summary>
        /// Writes the scopes, and the local variables declared in them, to the symbol writer.
        /// </summary>
        /// <param name="symWriter">The symbol writer of the module.</param>
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

        // array of offsets
        int[] _offsets = null!;

        // array of scope actions
        ScopeAction[] _scopeActions = null!;

        // how many entries in the arrays are occupied
        int _count;

        /// <summary>
        /// The number of scopes that are currently open.
        /// </summary>
        internal int OpenScopeCount;

        /// <summary>
        /// The initial length of the arrays.
        /// </summary>
        internal const int InitialSize = 16;

        // the local variable symbol information of each scope
        LocalSymInfo?[] _localSymInfos = null!;

    }

    /// <summary>
    /// Tracks the line number information of a method body, grouped by document.
    /// </summary>
    sealed class LineNumberInfo
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal LineNumberInfo()
        {
            // initialize data variables
            _documentCount = 0;
            _lastFound = 0;
        }

        /// <summary>
        /// Adds a sequence point.
        /// </summary>
        /// <param name="document">The document the sequence point refers to.</param>
        /// <param name="iOffset">The offset in the IL stream of the sequence point.</param>
        /// <param name="iStartLine">The line where the sequence point begins.</param>
        /// <param name="iStartColumn">The column where the sequence point begins.</param>
        /// <param name="iEndLine">The line where the sequence point ends.</param>
        /// <param name="iEndColumn">The column where the sequence point ends.</param>
        internal void AddLineNumberInfo(
            ISymbolDocumentWriter document,
            int iOffset,
            int iStartLine,
            int iStartColumn,
            int iEndLine,
            int iEndColumn)
        {
            int i;

            // find or add the entry for the document
            i = FindDocument(document);

            Contract.Assert(i < _documentCount, "Bad document look up!");
            _documents[i].AddLineNumberInfo(document, iOffset, iStartLine, iStartColumn, iEndLine, iEndColumn);
        }

        /// <summary>
        /// Finds the <see cref="REDocument"/> representing the document. If we cannot find one, we add a new entry
        /// into the array.
        /// </summary>
        /// <param name="document">The document to find.</param>
        /// <returns>The index of the entry for the document.</returns>
        int FindDocument(ISymbolDocumentWriter document)
        {
            int i;

            // this is an optimization: the chance that the previous line is coming from the same document is very high
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
            checked
            { _documentCount++; }
            return _lastFound;
        }

        /// <summary>
        /// Ensures the array is large enough to hold one more document.
        /// </summary>
        void EnsureCapacity()
        {
            if (_documentCount == 0)
            {
                // first time, allocate the arrays
                _documents = new REDocument[InitialSize];
            }
            else if (_documentCount == _documents.Length)
            {
                // the arrays are full, enlarge the arrays
                REDocument[] temp = new REDocument[_documentCount * 2];
                Array.Copy(_documents, temp, _documentCount);
                _documents = temp;
            }
        }

        /// <summary>
        /// Writes the sequence points of every document to the symbol writer.
        /// </summary>
        /// <param name="symWriter">The symbol writer of the module.</param>
        internal void EmitLineNumberInfo(ISymbolWriter symWriter)
        {
            for (var i = 0; i < _documentCount; i++)
                _documents[i].EmitLineNumberInfo(symWriter);
        }

        // how many documents that we have right now
        int _documentCount;

        // array of documents
        REDocument[] _documents;

        const int InitialSize = 16;
        int _lastFound;

    }

    /// <summary>
    /// Tracks the line number information of a method body for one document.
    /// </summary>
    sealed class REDocument
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="document">The document to track.</param>
        internal REDocument(ISymbolDocumentWriter document)
        {
            // initialize data variables
            _lineNumberCount = 0;
            Document = document;
        }

        /// <summary>
        /// Adds a sequence point.
        /// </summary>
        /// <param name="document">The document the sequence point refers to. Must be the tracked document.</param>
        /// <param name="iOffset">The offset in the IL stream of the sequence point.</param>
        /// <param name="iStartLine">The line where the sequence point begins.</param>
        /// <param name="iStartColumn">The column where the sequence point begins.</param>
        /// <param name="iEndLine">The line where the sequence point ends.</param>
        /// <param name="iEndColumn">The column where the sequence point ends.</param>
        internal void AddLineNumberInfo(
            ISymbolDocumentWriter document,
            int iOffset,
            int iStartLine,
            int iStartColumn,
            int iEndLine,
            int iEndColumn)
        {
            Contract.Assert(document == Document, "Bad document look up!");

            // make sure that arrays are large enough to hold additional info
            EnsureCapacity();

            _offsets[_lineNumberCount] = iOffset;
            _lines[_lineNumberCount] = iStartLine;
            _columns[_lineNumberCount] = iStartColumn;
            _endLines[_lineNumberCount] = iEndLine;
            _endColumns[_lineNumberCount] = iEndColumn;
            checked
            { _lineNumberCount++; }
        }

        /// <summary>
        /// Ensures the arrays are large enough to hold one more sequence point.
        /// </summary>
        void EnsureCapacity()
        {
            if (_lineNumberCount == 0)
            {
                // first time, allocate the arrays
                _offsets = new int[InitialSize];
                _lines = new int[InitialSize];
                _columns = new int[InitialSize];
                _endLines = new int[InitialSize];
                _endColumns = new int[InitialSize];
            }
            else if (_lineNumberCount == _offsets.Length)
            {
                // the arrays are full, enlarge the arrays; it would probably be simpler to just use Lists here
                var newSize = checked(_lineNumberCount * 2);
                var temp = new int[newSize];
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

        /// <summary>
        /// Writes the sequence points of the document to the symbol writer.
        /// </summary>
        /// <param name="symWriter">The symbol writer of the module.</param>
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

        // array of offsets
        int[] _offsets;

        // array of start lines
        int[] _lines;

        // array of start columns
        int[] _columns;

        // array of end lines
        int[] _endLines;

        // array of end columns
        int[] _endColumns;

        /// <summary>
        /// The document that this instance is tracking.
        /// </summary>
        internal ISymbolDocumentWriter Document;

        // how many entries in the arrays are occupied
        int _lineNumberCount;

        const int InitialSize = 16;

    }

}

#nullable restore
