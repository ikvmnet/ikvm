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

using System.Diagnostics;
using System.Xml.Serialization;

using IKVM.Reflection.Emit;

using Type = IKVM.Reflection.Type;

namespace IKVM.Internal.MapXml
{
    public abstract class TypeInstruction : Instruction
    {
        [XmlAttribute("type")]
        public string type;

        private OpCode opcode;
        private Type typeType;

        internal TypeInstruction(OpCode opcode)
        {
            this.opcode = opcode;
        }

        internal override void Generate(CodeGenContext context, CodeEmitter ilgen)
        {
            if (typeType == null)
            {
                Debug.Assert(type != null);
                typeType = StaticCompiler.GetTypeForMapXml(context.ClassLoader, type);
            }
            ilgen.Emit(opcode, typeType);
        }
    }
}
