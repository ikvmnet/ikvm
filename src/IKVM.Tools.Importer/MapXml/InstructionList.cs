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

using System.Linq;
using System.Xml.Linq;

using IKVM.Runtime;

namespace IKVM.Tools.Importer.MapXml
{

    public class InstructionList : MapXmlElement
    {

        /// <summary>
        /// Reads the XML element into a new <see cref="InstructionList"/> instance.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static InstructionList Read(XElement element)
        {
            var list = new InstructionList();
            Load(list, element);
            return list;
        }

        /// <summary>
        /// Loads the XML element into a <see cref="InstructionList"/> instance.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static void Load(InstructionList list, XElement element)
        {
            Load((MapXmlElement)list, element);
            list.Instructions = element.Elements().Select(Instruction.Read).ToArray();
        }

        public Instruction[] Instructions { get; set; }

        internal void Generate(CodeGenContext context, CodeEmitter ilgen)
        {
            if (Instructions != null)
                for (int i = 0; i < Instructions.Length; i++)
                    Instructions[i].Generate(context, ilgen);
        }

        internal void Emit(ClassLoaderWrapper loader, CodeEmitter ilgen)
        {
            Generate(new CodeGenContext(loader), ilgen);
        }

    }

}
