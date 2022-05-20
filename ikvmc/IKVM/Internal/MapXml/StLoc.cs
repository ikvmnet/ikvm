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
    [XmlType("stloc")]
    public sealed class StLoc : Instruction
    {
        [XmlAttribute("name")]
        public string Name;
        [XmlAttribute("class")]
        public string Class;
        [XmlAttribute("type")]
        public string type;

        private TypeWrapper typeWrapper;
        private Type typeType;

        internal override void Generate(CodeGenContext context, CodeEmitter ilgen)
        {
            CodeEmitterLocal lb = (CodeEmitterLocal)context[Name];
            if (lb == null)
            {
                if (typeWrapper == null && typeType == null)
                {
                    Debug.Assert(Class == null ^ type == null);
                    if (type != null)
                    {
                        typeType = StaticCompiler.GetTypeForMapXml(context.ClassLoader, type);
                    }
                    else
                    {
                        typeWrapper = context.ClassLoader.LoadClassByDottedName(Class);
                    }
                }
                lb = ilgen.DeclareLocal(typeType != null ? typeType : typeWrapper.TypeAsTBD);
                context[Name] = lb;
            }
            ilgen.Emit(OpCodes.Stloc, lb);
        }
    }
}
