/*
  Copyright (C) 2002-2010 Jeroen Frijters

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

using System.Xml.Serialization;

namespace IKVM.Internal.MapXml
{
    public class InstructionList
    {
        [XmlElement(typeof(Ldstr))]
        [XmlElement(typeof(Call))]
        [XmlElement(typeof(Callvirt))]
        [XmlElement(typeof(Ldftn))]
        [XmlElement(typeof(Ldvirtftn))]
        [XmlElement(typeof(Dup))]
        [XmlElement(typeof(Pop))]
        [XmlElement(typeof(IsInst))]
        [XmlElement(typeof(Castclass))]
        [XmlElement(typeof(Castclass_impl))]
        [XmlElement(typeof(Ldobj))]
        [XmlElement(typeof(Unbox))]
        [XmlElement(typeof(Box))]
        [XmlElement(typeof(BrFalse))]
        [XmlElement(typeof(BrTrue))]
        [XmlElement(typeof(Br))]
        [XmlElement(typeof(Beq))]
        [XmlElement(typeof(Bne_Un))]
        [XmlElement(typeof(Bge_Un))]
        [XmlElement(typeof(Ble_Un))]
        [XmlElement(typeof(Blt))]
        [XmlElement(typeof(Blt_Un))]
        [XmlElement(typeof(BrLabel))]
        [XmlElement(typeof(NewObj))]
        [XmlElement(typeof(StLoc))]
        [XmlElement(typeof(LdLoc))]
        [XmlElement(typeof(LdArga))]
        [XmlElement(typeof(LdArg_S))]
        [XmlElement(typeof(LdArg_0))]
        [XmlElement(typeof(LdArg_1))]
        [XmlElement(typeof(LdArg_2))]
        [XmlElement(typeof(LdArg_3))]
        [XmlElement(typeof(Ldind_i1))]
        [XmlElement(typeof(Ldind_i2))]
        [XmlElement(typeof(Ldind_i4))]
        [XmlElement(typeof(Ldind_i8))]
        [XmlElement(typeof(Ldind_r4))]
        [XmlElement(typeof(Ldind_r8))]
        [XmlElement(typeof(Ldind_ref))]
        [XmlElement(typeof(Stind_i1))]
        [XmlElement(typeof(Stind_i2))]
        [XmlElement(typeof(Stind_i4))]
        [XmlElement(typeof(Stind_i8))]
        [XmlElement(typeof(Stind_ref))]
        [XmlElement(typeof(Ret))]
        [XmlElement(typeof(Throw))]
        [XmlElement(typeof(Ldnull))]
        [XmlElement(typeof(Ldflda))]
        [XmlElement(typeof(Ldfld))]
        [XmlElement(typeof(Ldsfld))]
        [XmlElement(typeof(Stfld))]
        [XmlElement(typeof(Stsfld))]
        [XmlElement(typeof(Ldc_I4))]
        [XmlElement(typeof(Ldc_I4_0))]
        [XmlElement(typeof(Ldc_I4_1))]
        [XmlElement(typeof(Ldc_I4_M1))]
        [XmlElement(typeof(Conv_I))]
        [XmlElement(typeof(Conv_I1))]
        [XmlElement(typeof(Conv_U1))]
        [XmlElement(typeof(Conv_I2))]
        [XmlElement(typeof(Conv_U2))]
        [XmlElement(typeof(Conv_I4))]
        [XmlElement(typeof(Conv_U4))]
        [XmlElement(typeof(Conv_I8))]
        [XmlElement(typeof(Conv_U8))]
        [XmlElement(typeof(Ldlen))]
        [XmlElement(typeof(ExceptionBlock))]
        [XmlElement(typeof(Add))]
        [XmlElement(typeof(Sub))]
        [XmlElement(typeof(Mul))]
        [XmlElement(typeof(Div_Un))]
        [XmlElement(typeof(Rem_Un))]
        [XmlElement(typeof(And))]
        [XmlElement(typeof(Or))]
        [XmlElement(typeof(Xor))]
        [XmlElement(typeof(Not))]
        [XmlElement(typeof(Unaligned))]
        [XmlElement(typeof(Cpblk))]
        [XmlElement(typeof(Ceq))]
        [XmlElement(typeof(ConditionalInstruction))]
        [XmlElement(typeof(Volatile))]
        [XmlElement(typeof(Ldelema))]
        [XmlElement(typeof(Newarr))]
        [XmlElement(typeof(Ldtoken))]
        [XmlElement(typeof(Leave))]
        [XmlElement(typeof(Endfinally))]
        [XmlElement(typeof(RunClassInit))]
        [XmlElement(typeof(EmitExceptionMapping))]
        public Instruction[] invoke;

        internal void Generate(CodeGenContext context, CodeEmitter ilgen)
        {
            if (invoke != null)
            {
                for (int i = 0; i < invoke.Length; i++)
                {
                    if (invoke[i].LineNumber != -1)
                    {
                        ilgen.SetLineNumber((ushort)invoke[i].LineNumber);
                    }
                    invoke[i].Generate(context, ilgen);
                }
            }
        }

        internal void Emit(ClassLoaderWrapper loader, CodeEmitter ilgen)
        {
            Generate(new CodeGenContext(loader), ilgen);
        }
    }
}
