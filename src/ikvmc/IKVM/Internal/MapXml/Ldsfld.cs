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

using IKVM.Reflection;
using IKVM.Reflection.Emit;

namespace IKVM.Internal.MapXml
{
    [XmlType("ldsfld")]
    public sealed class Ldsfld : Instruction
    {
        [XmlAttribute("class")]
        public string Class;
        [XmlAttribute("type")]
        public string Type;
        [XmlAttribute("name")]
        public string Name;
        [XmlAttribute("sig")]
        public string Sig;

        internal override void Generate(CodeGenContext context, CodeEmitter ilgen)
        {
            if (Type != null)
            {
                ilgen.Emit(OpCodes.Ldsfld, StaticCompiler.GetTypeForMapXml(context.ClassLoader, Type).GetField(Name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));
            }
            else
            {
                // we don't use fw.EmitGet because we don't want automatic unboxing and whatever
                ilgen.Emit(OpCodes.Ldsfld, StaticCompiler.GetFieldForMapXml(context.ClassLoader, Class, Name, Sig).GetField());
            }
        }
    }
}
