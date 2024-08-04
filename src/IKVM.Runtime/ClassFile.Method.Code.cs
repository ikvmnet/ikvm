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

using IKVM.ByteCode.Reading;

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

                internal void Read(ClassFile classFile, string[] utf8_cp, Method method, CodeAttributeReader reader, ClassFileParseOptions options)
                {
                    max_stack = reader.MaxStack;
                    max_locals = reader.MaxLocals;
                    var code_length = (uint)reader.Code.Length;
                    if (code_length == 0 || code_length > 65535)
                        throw new ClassFormatError("Invalid method Code length {1} in class file {0}", classFile.Name, code_length);

                    var instructions = new Instruction[code_length + 1];
                    int basePosition = 0;
                    int instructionIndex = 0;

                    try
                    {
                        var rdr = new BigEndianBinaryReader(reader.Code);
                        while (rdr.IsAtEnd == false)
                        {
                            instructions[instructionIndex].Read((ushort)(rdr.Position - basePosition), rdr, classFile);
                            hasJsr |= instructions[instructionIndex].NormalizedOpCode == NormalizedByteCode.__jsr;
                            instructionIndex++;
                        }

                        // we add an additional nop instruction to make it easier for consumers of the code array
                        instructions[instructionIndex++].SetTermNop((ushort)(rdr.Position - basePosition));
                    }
                    catch (ClassFormatError x)
                    {
                        // any class format errors in the code block are actually verify errors
                        verifyError = x.Message;
                    }

                    this.instructions = new Instruction[instructionIndex];
                    Array.Copy(instructions, 0, this.instructions, 0, instructionIndex);

                    // build the pcIndexMap
                    var pcIndexMap = new int[this.instructions[instructionIndex - 1].PC + 1];
                    for (int i = 0; i < pcIndexMap.Length; i++)
                        pcIndexMap[i] = -1;
                    for (int i = 0; i < instructionIndex - 1; i++)
                        pcIndexMap[this.instructions[i].PC] = i;

                    // convert branch offsets to indexes
                    for (int i = 0; i < instructionIndex - 1; i++)
                    {
                        switch (this.instructions[i].NormalizedOpCode)
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
                    exception_table = new ExceptionTableEntry[reader.ExceptionTable.Count];
                    for (int i = 0; i < reader.ExceptionTable.Count; i++)
                    {
                        var handler = reader.ExceptionTable[i];
                        var start_pc = handler.StartOffset;
                        var end_pc = handler.EndOffset;
                        var handler_pc = handler.HandlerOffset;
                        var catch_type = handler.CatchType.Index;

                        if (start_pc >= end_pc || end_pc > code_length || handler_pc >= code_length || (catch_type != 0 && !classFile.SafeIsConstantPoolClass(catch_type)))
                            throw new ClassFormatError("Illegal exception table: {0}.{1}{2}", classFile.Name, method.Name, method.Signature);

                        classFile.MarkLinkRequiredConstantPoolItem(catch_type);

                        // if start_pc, end_pc or handler_pc is invalid (i.e. doesn't point to the start of an instruction),
                        // the index will be -1 and this will be handled by the verifier
                        var startIndex = pcIndexMap[start_pc];
                        var endIndex = 0;
                        if (end_pc == code_length)
                        {
                            // it is legal for end_pc to point to just after the last instruction,
                            // but since there isn't an entry in our pcIndexMap for that, we have
                            // a special case for this
                            endIndex = instructionIndex - 1;
                        }
                        else
                        {
                            endIndex = pcIndexMap[end_pc];
                        }

                        var handlerIndex = pcIndexMap[handler_pc];
                        exception_table[i] = new ExceptionTableEntry(startIndex, endIndex, handlerIndex, catch_type, i);
                    }

                    for (int i = 0; i < reader.Attributes.Count; i++)
                    {
                        var attribute = reader.Attributes[i];

                        switch (classFile.GetConstantPoolUtf8String(utf8_cp, attribute.Info.Record.Name.Index))
                        {
                            case "LineNumberTable":
                                if (attribute is not LineNumberTableAttributeReader lnt)
                                    throw new ClassFormatError("Invalid reader for line number table.");

                                if ((options & ClassFileParseOptions.LineNumberTable) != 0)
                                {
                                    lineNumberTable = new LineNumberTableEntry[lnt.Record.Items.Length];
                                    for (int j = 0; j < lnt.Record.Items.Length; j++)
                                    {
                                        var item = lnt.Record.Items[j];
                                        lineNumberTable[j].start_pc = item.CodeOffset;
                                        lineNumberTable[j].line_number = item.LineNumber;
                                        if (lineNumberTable[j].start_pc >= code_length)
                                            throw new ClassFormatError("{0} (LineNumberTable has invalid pc)", classFile.Name);
                                    }
                                }
                                break;
                            case "LocalVariableTable":
                                if (attribute is not LocalVariableTableAttributeReader lvt)
                                    throw new ClassFormatError("Invalid reader for local variable table.");

                                if ((options & ClassFileParseOptions.LocalVariableTable) != 0)
                                {
                                    localVariableTable = new LocalVariableTableEntry[lvt.Record.Items.Length];
                                    for (int j = 0; j < lvt.Record.Items.Length; j++)
                                    {
                                        var item = lvt.Record.Items[j];
                                        localVariableTable[j].start_pc = item.CodeOffset;
                                        localVariableTable[j].length = item.CodeLength;
                                        localVariableTable[j].name = classFile.GetConstantPoolUtf8String(utf8_cp, item.Name.Index);
                                        localVariableTable[j].descriptor = classFile.GetConstantPoolUtf8String(utf8_cp, item.Descriptor.Index).Replace('/', '.');
                                        localVariableTable[j].index = item.Index;
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

                internal bool IsEmpty => instructions == null;

            }

        }

    }

}
