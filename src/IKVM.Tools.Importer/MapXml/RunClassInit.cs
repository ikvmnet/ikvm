﻿/*
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

using IKVM.Internal;

namespace IKVM.Tools.Importer.MapXml
{

    [Instruction("runclassinit")]
    public sealed class RunClassInit : Instruction
    {

        /// <summary>
        /// Reads the XML element into a new <see cref="RunClassInit"/> instance.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static new RunClassInit Read(XElement element)
        {
            var inst = new RunClassInit();
            Load(inst, element);
            return inst;
        }

        /// <summary>
        /// Loads the XML element into the instruction.
        /// </summary>
        /// <param name="inst"></param>
        /// <param name="element"></param>
        public static void Load(RunClassInit inst, XElement element)
        {
            Load((Instruction)inst, element);
            inst.Class = (string)element.Attribute("class");
        }

        public string Class { get; set; }

        internal override void Generate(CodeGenContext context, CodeEmitter ilgen)
        {
            context.ClassLoader.LoadClassByDottedName(Class).EmitRunClassConstructor(ilgen);
        }

    }

}
