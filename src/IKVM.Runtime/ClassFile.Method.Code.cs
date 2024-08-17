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

namespace IKVM.Runtime
{

    sealed partial class ClassFile
    {

        internal sealed partial class Method
        {

            struct Code
            {

                internal bool hasJsr;
                internal string verifyError;
                internal ushort max_stack;
                internal ushort max_locals;
                internal Instruction[] instructions;
                internal ExceptionTableEntry[] exception_table;
                internal int[] argmap;
                internal LineNumberTableEntry[] lineNumberTable;
                internal LocalVariableTableEntry[] localVariableTable;

                /// <summary>
                /// Populates the <see cref="Code"/> structure from the given <see cref="CodeAttribute"/>.
                /// </summary>
                /// <param name="classFile"></param>
                /// <param name="utf8_cp"></param>
                /// <param name="method"></param>
                /// <param name="attribute"></param>
                /// <param name="options"></param>
                /// <exception cref="ClassFormatError"></exception>
                internal void Read(ClassFile classFile, string[] utf8_cp, Method method, CodeAttribute attribute, ClassFileParseOptions options)
                {
                    max_stack = attribute.MaxStack;
                    max_locals = attribute.MaxLocals;
                    if (attribute.Code.Length == 0 || attribute.Code.Length > 65535)
                        throw new ClassFormatError("Invalid method Code length {1} in class file {0}", classFile.Name, attribute.Code.Length);

                    // we don't know how many instructions we will read until we read them, so first parse them into a temporary array
                    var _instructions = ArrayPool<Instruction>.Shared.Rent((int)attribute.Code.Length + 1);

                    try
                    {
                        int instructionCount = 0;

                        try
                        {
                            var decoder = new CodeDecoder(attribute.Code);

                            // read instructions and parse them into local structure
                            foreach (var instruction in decoder)
                            {
                                _instructions[instructionCount].Read(instruction, classFile);
                                hasJsr |= _instructions[instructionCount].NormalizedOpCode == NormalizedByteCode.__jsr;
                                instructionCount++;
                            }

                            // we add an additional nop instruction to make it easier for consumers of the code array
                            _instructions[instructionCount++].SetTermNop((ushort)decoder.Position.GetInteger());
                        }
                        catch (ArgumentOutOfRangeException e)
                        {
                            verifyError = e.Message;
                        }
                        catch (ClassFormatError e)
                        {
                            verifyError = e.Message;
                        }
                        catch (ByteCodeException e)
                        {
                            verifyError = e.Message;
                        }

                        // copy from temporary array into properly sized array
                        instructions = new Instruction[instructionCount];
                        Array.Copy(_instructions, 0, instructions, 0, instructionCount);

                        // build the pcIndexMap
                        var pcIndexMap = new int[instructions[instructionCount - 1].PC + 1];
                        for (int i = 0; i < pcIndexMap.Length; i++)
                            pcIndexMap[i] = -1;
                        for (int i = 0; i < instructionCount - 1; i++)
                            pcIndexMap[instructions[i].PC] = i;

                        // convert branch offsets to indexes
                        for (int i = 0; i < instructionCount - 1; i++)
                        {
                            switch (instructions[i].NormalizedOpCode)
                            {
                                case NormalizedByteCode.__ifeq:
                                case NormalizedByteCode.__ifne:
                                case NormalizedByteCode.__iflt:
                                case NormalizedByteCode.__ifge:
                                case NormalizedByteCode.__ifgt:
                                case NormalizedByteCode.__ifle:
                                case NormalizedByteCode.__if_icmpeq:
                                case NormalizedByteCode.__if_icmpne:
                                case NormalizedByteCode.__if_icmplt:
                                case NormalizedByteCode.__if_icmpge:
                                case NormalizedByteCode.__if_icmpgt:
                                case NormalizedByteCode.__if_icmple:
                                case NormalizedByteCode.__if_acmpeq:
                                case NormalizedByteCode.__if_acmpne:
                                case NormalizedByteCode.__ifnull:
                                case NormalizedByteCode.__ifnonnull:
                                case NormalizedByteCode.__goto:
                                case NormalizedByteCode.__jsr:
                                    this.instructions[i].SetTargetIndex(pcIndexMap[this.instructions[i].Arg1 + this.instructions[i].PC]);
                                    break;
                                case NormalizedByteCode.__tableswitch:
                                case NormalizedByteCode.__lookupswitch:
                                    this.instructions[i].MapSwitchTargets(pcIndexMap);
                                    break;
                            }
                        }

                        // read exception table
                        exception_table = new ExceptionTableEntry[attribute.ExceptionTable.Count];
                        for (int i = 0; i < attribute.ExceptionTable.Count; i++)
                        {
                            var handler = attribute.ExceptionTable[i];
                            var start_pc = handler.StartOffset;
                            var end_pc = handler.EndOffset;
                            var handler_pc = handler.HandlerOffset;
                            var catch_type = handler.CatchType;

                            if (start_pc >= end_pc || end_pc > attribute.Code.Length || handler_pc >= attribute.Code.Length || (catch_type.IsNotNil && !classFile.SafeIsConstantPoolClass(catch_type)))
                                throw new ClassFormatError("Illegal exception table: {0}.{1}{2}", classFile.Name, method.Name, method.Signature);

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
                            exception_table[i] = new ExceptionTableEntry(startIndex, endIndex, handlerIndex, catch_type, i);
                        }

                        foreach (var _attribute in attribute.Attributes)
                        {
                            switch (classFile.GetConstantPoolUtf8String(utf8_cp, _attribute.Name))
                            {
                                case AttributeName.LineNumberTable:
                                    var lnt = (IKVM.ByteCode.Decoding.LineNumberTableAttribute)_attribute;
                                    if ((options & ClassFileParseOptions.LineNumberTable) != 0)
                                    {
                                        lineNumberTable = new LineNumberTableEntry[lnt.LineNumbers.Count];
                                        for (int j = 0; j < lnt.LineNumbers.Count; j++)
                                        {
                                            var item = lnt.LineNumbers[j];
                                            lineNumberTable[j].start_pc = item.StartPc;
                                            lineNumberTable[j].line_number = item.LineNumber;
                                            if (lineNumberTable[j].start_pc >= attribute.Code.Length)
                                                throw new ClassFormatError("{0} (LineNumberTable has invalid pc)", classFile.Name);
                                        }
                                    }
                                    break;
                                case AttributeName.LocalVariableTable:
                                    var lvt = (IKVM.ByteCode.Decoding.LocalVariableTableAttribute)_attribute;
                                    if ((options & ClassFileParseOptions.LocalVariableTable) != 0)
                                    {
                                        localVariableTable = new LocalVariableTableEntry[lvt.LocalVariables.Count];
                                        for (int j = 0; j < lvt.LocalVariables.Count; j++)
                                        {
                                            var item = lvt.LocalVariables[j];
                                            localVariableTable[j].start_pc = item.StartPc;
                                            localVariableTable[j].length = item.Length;
                                            localVariableTable[j].name = classFile.GetConstantPoolUtf8String(utf8_cp, item.Name);
                                            localVariableTable[j].descriptor = classFile.GetConstantPoolUtf8String(utf8_cp, item.Descriptor).Replace('/', '.');
                                            localVariableTable[j].index = item.Slot;
                                        }
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
                        argmap = args.ToArray();

                        if (args.Count > max_locals)
                            throw new ClassFormatError("{0} (Arguments can't fit into locals)", classFile.Name);
                    }
                    catch (InvalidCodeException e)
                    {
                        throw new ClassFormatError(e.Message);
                    }
                    catch (ByteCodeException e)
                    {
                        throw new ClassFormatError(e.Message);
                    }
                    finally
                    {
                        ArrayPool<Instruction>.Shared.Return(_instructions);
                    }
                }

                internal bool IsEmpty => instructions == null;

            }

        }

    }

}
