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

namespace IKVM.Runtime
{

    sealed partial class ClassFile
    {
        internal sealed partial class Method
        {
            internal struct Instruction
            {
                private ushort pc;
                private NormalizedByteCode normopcode;
                private int arg1;
                private short arg2;
                private SwitchEntry[] switch_entries;

                struct SwitchEntry
                {
                    internal int value;
                    internal int target;
                }

                internal void SetHardError(HardError error, int messageId)
                {
                    normopcode = NormalizedByteCode.__static_error;
                    arg2 = (short)error;
                    arg1 = messageId;
                }

                internal HardError HardError
                {
                    get
                    {
                        return (HardError)arg2;
                    }
                }

                internal int HandlerIndex
                {
                    get { return (ushort)arg2; }
                }

                internal int HardErrorMessageId
                {
                    get
                    {
                        return arg1;
                    }
                }

                internal void PatchOpCode(NormalizedByteCode bc)
                {
                    this.normopcode = bc;
                }

                internal void PatchOpCode(NormalizedByteCode bc, int arg1)
                {
                    this.normopcode = bc;
                    this.arg1 = arg1;
                }

                internal void PatchOpCode(NormalizedByteCode bc, int arg1, short arg2)
                {
                    this.normopcode = bc;
                    this.arg1 = arg1;
                    this.arg2 = arg2;
                }

                internal void SetPC(int pc)
                {
                    this.pc = (ushort)pc;
                }

                internal void SetTargetIndex(int targetIndex)
                {
                    this.arg1 = targetIndex;
                }

                internal void SetTermNop(ushort pc)
                {
                    // TODO what happens if we already have exactly the maximum number of instructions?
                    this.pc = pc;
                    this.normopcode = NormalizedByteCode.__nop;
                }

                internal void MapSwitchTargets(int[] pcIndexMap)
                {
                    arg1 = pcIndexMap[arg1 + pc];
                    for (int i = 0; i < switch_entries.Length; i++)
                    {
                        switch_entries[i].target = pcIndexMap[switch_entries[i].target + pc];
                    }
                }

                internal void Read(ushort pc, BigEndianBinaryReader br, ClassFile classFile)
                {
                    this.pc = pc;
                    var bc = (ByteCode)br.ReadByte();
                    switch (ByteCodeMetaData.GetMode(bc))
                    {
                        case ByteCodeMode.Simple:
                            break;
                        case ByteCodeMode.Constant_1:
                            arg1 = br.ReadByte();
                            classFile.MarkLinkRequiredConstantPoolItem(arg1);
                            break;
                        case ByteCodeMode.Local_1:
                            arg1 = br.ReadByte();
                            break;
                        case ByteCodeMode.Constant_2:
                            arg1 = br.ReadUInt16();
                            classFile.MarkLinkRequiredConstantPoolItem(arg1);
                            break;
                        case ByteCodeMode.Branch_2:
                            arg1 = br.ReadInt16();
                            break;
                        case ByteCodeMode.Branch_4:
                            arg1 = br.ReadInt32();
                            break;
                        case ByteCodeMode.Constant_2_1_1:
                            arg1 = br.ReadUInt16();
                            classFile.MarkLinkRequiredConstantPoolItem(arg1);
                            arg2 = br.ReadByte();
                            if (br.ReadByte() != 0)
                            {
                                throw new ClassFormatError("invokeinterface filler must be zero");
                            }
                            break;
                        case ByteCodeMode.Immediate_1:
                            arg1 = br.ReadSByte();
                            break;
                        case ByteCodeMode.Immediate_2:
                            arg1 = br.ReadInt16();
                            break;
                        case ByteCodeMode.Local_1_Immediate_1:
                            arg1 = br.ReadByte();
                            arg2 = br.ReadSByte();
                            break;
                        case ByteCodeMode.Constant_2_Immediate_1:
                            arg1 = br.ReadUInt16();
                            classFile.MarkLinkRequiredConstantPoolItem(arg1);
                            arg2 = br.ReadSByte();
                            break;
                        case ByteCodeMode.Tableswitch:
                            {
                                // skip the padding
                                uint p = pc + 1u;
                                uint align = ((p + 3) & 0x7ffffffc) - p;
                                br.Skip(align);
                                int default_offset = br.ReadInt32();
                                this.arg1 = default_offset;
                                int low = br.ReadInt32();
                                int high = br.ReadInt32();
                                if (low > high || high > 16384L + low)
                                {
                                    throw new ClassFormatError("Incorrect tableswitch");
                                }
                                SwitchEntry[] entries = new SwitchEntry[high - low + 1];
                                for (int i = low; i < high; i++)
                                {
                                    entries[i - low].value = i;
                                    entries[i - low].target = br.ReadInt32();
                                }
                                // do the last entry outside the loop, to avoid overflowing "i", if high == int.MaxValue
                                entries[high - low].value = high;
                                entries[high - low].target = br.ReadInt32();
                                this.switch_entries = entries;
                                break;
                            }
                        case ByteCodeMode.Lookupswitch:
                            {
                                // skip the padding
                                uint p = pc + 1u;
                                uint align = ((p + 3) & 0x7ffffffc) - p;
                                br.Skip(align);
                                int default_offset = br.ReadInt32();
                                this.arg1 = default_offset;
                                int count = br.ReadInt32();
                                if (count < 0 || count > 16384)
                                {
                                    throw new ClassFormatError("Incorrect lookupswitch");
                                }
                                SwitchEntry[] entries = new SwitchEntry[count];
                                for (int i = 0; i < count; i++)
                                {
                                    entries[i].value = br.ReadInt32();
                                    entries[i].target = br.ReadInt32();
                                }
                                this.switch_entries = entries;
                                break;
                            }
                        case ByteCodeMode.WidePrefix:
                            bc = (ByteCode)br.ReadByte();
                            // NOTE the PC of a wide instruction is actually the PC of the
                            // wide prefix, not the following instruction (vmspec 4.9.2)
                            switch (ByteCodeMetaData.GetWideMode(bc))
                            {
                                case ByteCodeModeWide.Local_2:
                                    arg1 = br.ReadUInt16();
                                    break;
                                case ByteCodeModeWide.Local_2_Immediate_2:
                                    arg1 = br.ReadUInt16();
                                    arg2 = br.ReadInt16();
                                    break;
                                default:
                                    throw new ClassFormatError("Invalid wide prefix on opcode: {0}", bc);
                            }
                            break;
                        default:
                            throw new ClassFormatError("Invalid opcode: {0}", bc);
                    }
                    this.normopcode = ByteCodeMetaData.GetNormalizedByteCode(bc);
                    arg1 = ByteCodeMetaData.GetArg(bc, arg1);
                }

                internal int PC
                {
                    get
                    {
                        return pc;
                    }
                }

                internal NormalizedByteCode NormalizedOpCode
                {
                    get
                    {
                        return normopcode;
                    }
                }

                internal int Arg1
                {
                    get
                    {
                        return arg1;
                    }
                }

                internal int TargetIndex
                {
                    get
                    {
                        return arg1;
                    }
                    set
                    {
                        arg1 = value;
                    }
                }

                internal int Arg2
                {
                    get
                    {
                        return arg2;
                    }
                }

                internal int NormalizedArg1
                {
                    get
                    {
                        return arg1;
                    }
                }

                internal int DefaultTarget
                {
                    get
                    {
                        return arg1;
                    }
                    set
                    {
                        arg1 = value;
                    }
                }

                internal int SwitchEntryCount
                {
                    get
                    {
                        return switch_entries.Length;
                    }
                }

                internal int GetSwitchValue(int i)
                {
                    return switch_entries[i].value;
                }

                internal int GetSwitchTargetIndex(int i)
                {
                    return switch_entries[i].target;
                }

                internal void SetSwitchTargets(int[] targets)
                {
                    SwitchEntry[] newEntries = (SwitchEntry[])switch_entries.Clone();
                    for (int i = 0; i < newEntries.Length; i++)
                    {
                        newEntries[i].target = targets[i];
                    }
                    switch_entries = newEntries;
                }
            }
        }
    }

}
