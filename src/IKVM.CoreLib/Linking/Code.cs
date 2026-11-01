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
using System.Buffers;
using System.Collections.Generic;

using IKVM.ByteCode;
using IKVM.ByteCode.Decoding;
using IKVM.CoreLib.Jvm;

namespace IKVM.CoreLib.Linking
{

    struct Code<TType, TMember, TField, TMethod, TTypeSystem>
        where TType : class
        where TMember : class
        where TField : class, TMember
        where TMethod : class, TMember
        where TTypeSystem : struct, IJvmTypeSystem<TType, TMember, TField, TMethod>
    {

        internal bool _hasJsr;
        internal string _verifyError;
        internal ushort _maxStack;
        internal ushort _maxLocals;
        internal Instruction<TType, TMember, TField, TMethod, TTypeSystem>[] _instructions;
        internal ExceptionTableEntry[] _exceptionTable;
        internal int[] _argmap;
        internal LineNumberTable _lineNumberTable;
        internal LocalVariableTable _localVariableTable;

        /// <summary>
        /// Populates the <see cref="Code"/> structure from the given <see cref="CodeAttribute"/>.
        /// </summary>
        /// <param name="classFile"></param>
        /// <param name="utf8_cp"></param>
        /// <param name="method"></param>
        /// <param name="attribute"></param>
        /// <param name="options"></param>
        /// <exception cref="ClassFormatException"></exception>
        internal void Read(ClassFile<TType, TMember, TField, TMethod, TTypeSystem> classFile, Method<TType, TMember, TField, TMethod, TTypeSystem> method, CodeAttribute attribute)
        {
            _maxStack = attribute.MaxStack;
            _maxLocals = attribute.MaxLocals;
            if (attribute.Code.Length == 0 || attribute.Code.Length > 65535)
                throw new ClassFormatException("Invalid method Code length {1} in class file {0}", classFile.Name, attribute.Code.Length);

            // we don't know how many instructions we will read until we read them, so first parse them into a temporary array
            var _instructions = ArrayPool<Instruction<TType, TMember, TField, TMethod, TTypeSystem>>.Shared.Rent((int)attribute.Code.Length + 1);

            // a rented array contains whatever was left in it, and reading an instruction only sets the fields its opcode uses
            Array.Clear(_instructions, 0, (int)attribute.Code.Length + 1);

            try
            {
                int instructionCount = 0;

                try
                {
                    var decoder = new CodeDecoder(attribute.Code);
                    var lastIns = default(Instruction);

                    // read instructions and parse them into local structure
                    foreach (var instruction in decoder)
                    {
                        lastIns = instruction;

                        _instructions[instructionCount].Read(instruction, classFile);
                        _hasJsr |= _instructions[instructionCount].NormalizedOpCode == NormalizedOpCode.Jsr;
                        instructionCount++;
                    }

                    // we add an additional nop instruction to make it easier for consumers of the code array
                    _instructions[instructionCount++].SetTermNop((ushort)(lastIns.IsNotNil ? lastIns.Offset + lastIns.Data.Length : 0));
                }
                catch (ArgumentOutOfRangeException e)
                {
                    _verifyError = e.Message;
                }
                catch (LinkingException e)
                {
                    _verifyError = e.Message;
                }
                catch (ByteCodeException e)
                {
                    _verifyError = e.Message;
                }

                // copy from temporary array into properly sized array
                this._instructions = new Instruction<TType, TMember, TField, TMethod, TTypeSystem>[instructionCount];
                Array.Copy(_instructions, 0, this._instructions, 0, instructionCount);

                // build the pcIndexMap
                var pcIndexMap = new int[this._instructions[instructionCount - 1].PC + 1];
                pcIndexMap.AsSpan().Fill(-1);
                for (int i = 0; i < instructionCount - 1; i++)
                    pcIndexMap[this._instructions[i].PC] = i;

                // convert branch offsets to indexes
                for (int i = 0; i < instructionCount - 1; i++)
                {
                    switch (this._instructions[i].NormalizedOpCode)
                    {
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
                        case NormalizedOpCode.Jsr:
                            this._instructions[i].SetTargetIndex(pcIndexMap[this._instructions[i].Arg1 + this._instructions[i].PC]);
                            break;
                        case NormalizedOpCode.TableSwitch:
                        case NormalizedOpCode.LookupSwitch:
                            this._instructions[i].MapSwitchTargets(pcIndexMap);
                            break;
                    }
                }

                // read exception table
                _exceptionTable = new ExceptionTableEntry[attribute.ExceptionTable.Count];
                for (int i = 0; i < attribute.ExceptionTable.Count; i++)
                {
                    var handler = attribute.ExceptionTable[i];
                    var start_pc = handler.StartOffset;
                    var end_pc = handler.EndOffset;
                    var handler_pc = handler.HandlerOffset;
                    var catch_type = handler.CatchType;

                    if (start_pc >= end_pc || end_pc > attribute.Code.Length || handler_pc >= attribute.Code.Length || (catch_type.IsNotNil && !classFile.SafeIsConstantPoolClass(catch_type)))
                        throw new ClassFormatException("Illegal exception table: {0}.{1}{2}", classFile.Name, method.Name, method.Signature);

                    classFile.MarkLinkRequiredConstantPoolItem(catch_type);

                    // if start_pc, end_pc or handler_pc is invalid (i.e. doesn't point to the start of an instruction),
                    // the index will be -1 and this will be handled by the verifier
                    var startIndex = pcIndexMap[start_pc];
                    var endIndex = 0;
                    if (end_pc == attribute.Code.Length)
                    {
                        // it is legal for end_pc to point to just after the last instruction,
                        // but since there isn't an entry in our pcIndexMap for that, we have
                        // a special case for this
                        endIndex = instructionCount - 1;
                    }
                    else
                    {
                        endIndex = pcIndexMap[end_pc];
                    }

                    var handlerIndex = pcIndexMap[handler_pc];
                    _exceptionTable[i] = new ExceptionTableEntry(startIndex, endIndex, handlerIndex, catch_type, i);
                }

                foreach (var _attribute in attribute.Attributes)
                {
                    switch (classFile.GetConstantPoolUtf8String(_attribute.Name))
                    {
                        case AttributeName.LineNumberTable:
                            if ((classFile.Options & ClassFileParseOptions.LineNumberTable) != 0)
                            {
                                _lineNumberTable = ((LineNumberTableAttribute)_attribute).LineNumbers;
                                for (int j = 0; j < _lineNumberTable.Count; j++)
                                    if (_lineNumberTable[j].StartPc >= attribute.Code.Length)
                                        throw new ClassFormatException("{0} (LineNumberTable has invalid pc)", classFile.Name);
                            }
                            break;
                        case AttributeName.LocalVariableTable:
                            if ((classFile.Options & ClassFileParseOptions.LocalVariableTable) != 0)
                            {
                                _localVariableTable = ((LocalVariableTableAttribute)_attribute).LocalVariables;
                            }
                            break;
                        default:
                            break;
                    }
                }

                // build the argmap
                var sig = method.Signature;
                var args = new List<int>();
                int pos = 0;
                if (!method.IsStatic)
                    args.Add(pos++);

                for (int i = 1; sig[i] != ')'; i++)
                {
                    args.Add(pos++);
                    switch (sig[i])
                    {
                        case 'L':
                            i = sig.IndexOf(';', i);
                            break;
                        case 'D':
                        case 'J':
                            args.Add(-1);
                            break;
                        case '[':
                            {
                                while (sig[i] == '[')
                                {
                                    i++;
                                }
                                if (sig[i] == 'L')
                                {
                                    i = sig.IndexOf(';', i);
                                }
                                break;
                            }
                    }
                }
                _argmap = args.ToArray();

                if (args.Count > _maxLocals)
                    throw new ClassFormatException("{0} (Arguments can't fit into locals)", classFile.Name);
            }
            catch (InvalidCodeException e)
            {
                throw new ClassFormatException(e.Message);
            }
            catch (ByteCodeException e)
            {
                throw new ClassFormatException(e.Message);
            }
            finally
            {
                ArrayPool<Instruction<TType, TMember, TField, TMethod, TTypeSystem>>.Shared.Return(_instructions);
            }
        }

        internal readonly bool IsEmpty => _instructions == null;

    }

}
