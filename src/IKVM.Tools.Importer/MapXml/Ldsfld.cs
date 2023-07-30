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

using System.Xml.Linq;

using IKVM.Reflection;
using IKVM.Reflection.Emit;
using IKVM.Runtime;

namespace IKVM.Tools.Importer.MapXml
{

    [Instruction("ldsfld")]
    public sealed class Ldsfld : Instruction
    {

        /// <summary>
        /// Reads the XML element into a new <see cref="Ldsfld"/> instance.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static new Ldsfld Read(XElement element)
        {
            var inst = new Ldsfld();
            Load(inst, element);
            return inst;
        }

        /// <summary>
        /// Loads the XML element into the instruction.
        /// </summary>
        /// <param name="inst"></param>
        /// <param name="element"></param>
        public static void Load(Ldsfld inst, XElement element)
        {
            Load((Instruction)inst, element);
            inst.Class = (string)element.Attribute("class");
            inst.Type = (string)element.Attribute("type");
            inst.Name = (string)element.Attribute("name");
            inst.Sig = (string)element.Attribute("sig");
        }

        public string Class { get; set; }

        public string Type { get; set; }

        public string Name { get; set; }

        public string Sig { get; set; }

        internal override void Generate(CodeGenContext context, CodeEmitter ilgen)
        {
            if (Type != null)
            {
                ilgen.Emit(OpCodes.Ldsfld, context.ClassLoader.Context.StaticCompiler.GetTypeForMapXml(context.ClassLoader, Type).GetField(Name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));
            }
            else
            {
                // we don't use fw.EmitGet because we don't want automatic unboxing and whatever
                ilgen.Emit(OpCodes.Ldsfld, context.ClassLoader.Context.StaticCompiler.GetFieldForMapXml(context.ClassLoader, Class, Name, Sig).GetField());
            }
        }

    }

}
