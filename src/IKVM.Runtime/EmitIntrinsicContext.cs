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

using IKVM.ByteCode;
using IKVM.CoreLib.Linking;

namespace IKVM.Runtime
{

    sealed class EmitIntrinsicContext
    {

        internal readonly RuntimeJavaMethod Method;
        internal readonly RuntimeByteCodeJavaType.FinishContext Context;
        internal readonly CodeEmitter Emitter;
        readonly CodeInfo ma;
        internal readonly int OpcodeIndex;
        internal readonly RuntimeJavaMethod Caller;
        internal readonly ClassFile ClassFile;
        internal readonly Instruction[] Code;
        internal readonly InstructionFlags[] Flags;
        internal bool NonLeaf = true;

        internal EmitIntrinsicContext(RuntimeJavaMethod method, RuntimeByteCodeJavaType.FinishContext context, CodeEmitter ilgen, CodeInfo ma, int opcodeIndex, RuntimeJavaMethod caller, ClassFile classFile, Instruction[] code, InstructionFlags[] flags)
        {
            this.Method = method;
            this.Context = context;
            this.Emitter = ilgen;
            this.ma = ma;
            this.OpcodeIndex = opcodeIndex;
            this.Caller = caller;
            this.ClassFile = classFile;
            this.Code = code;
            this.Flags = flags;
        }

        internal bool MatchRange(int offset, int length)
        {
            if (OpcodeIndex + offset < 0)
                return false;

            if (OpcodeIndex + offset + length > Code.Length)
                return false;

            // we check for branches *into* the range, the start of the range may be a branch target
            for (int i = OpcodeIndex + offset + 1, end = OpcodeIndex + offset + length; i < end; i++)
                if ((Flags[i] & InstructionFlags.BranchTarget) != 0)
                    return false;

            return true;
        }

        internal bool Match(int offset, NormalizedOpCode opcode)
        {
            return Code[OpcodeIndex + offset].NormalizedOpCode == opcode;
        }

        internal bool Match(int offset, NormalizedOpCode opcode, int arg)
        {
            return Code[OpcodeIndex + offset].NormalizedOpCode == opcode && Code[OpcodeIndex + offset].Arg1 == arg;
        }

        internal RuntimeJavaType GetStackTypeWrapper(int offset, int pos)
        {
            return ma.GetStackTypeWrapper(OpcodeIndex + offset, pos);
        }

        internal ConstantPoolItemMI GetMethodref(int offset)
        {
            return ClassFile.GetMethodref(checked((ushort)Code[OpcodeIndex + offset].Arg1));
        }

        internal ConstantPoolItemFieldref GetFieldref(int offset)
        {
            return ClassFile.GetFieldref(checked((ushort)Code[OpcodeIndex + offset].Arg1));
        }

        internal RuntimeJavaType GetClassLiteral(int offset)
        {
            return ClassFile.GetConstantPoolClassType(new ClassConstantHandle(checked((ushort)Code[OpcodeIndex + offset].Arg1)));
        }

        internal string GetStringLiteral(int offset)
        {
            return ClassFile.GetConstantPoolConstantString(new StringConstantHandle(checked((ushort)Code[OpcodeIndex + offset].Arg1)));
        }

        internal ConstantType GetConstantType(int offset)
        {
            return ClassFile.GetConstantPoolConstantType(new ConstantHandle(ConstantKind.Unknown, checked((ushort)Code[OpcodeIndex + offset].Arg1)));
        }

        internal void PatchOpCode(int offset, NormalizedOpCode opc)
        {
            Code[OpcodeIndex + offset].PatchOpCode(opc);
        }

    }

}
