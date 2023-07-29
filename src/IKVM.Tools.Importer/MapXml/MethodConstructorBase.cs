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

    public abstract class MethodConstructorBase : MethodBase
    {

        public static void Load(MethodConstructorBase o, XElement element)
        {
            Load((MethodBase)o, element);
            o.Sig = (string)element.Attribute("sig");
            o.Modifiers = MapXmlSerializer.ReadMapModifiers((string)element.Attribute("modifiers"));
            o.Parameters = element.Elements(MapXmlSerializer.NS + "parameter").Select(Parameter.Read).ToArray();
            o.AlternateBody = element.Elements(MapXmlSerializer.NS + "alternateBody").Select(InstructionList.Read).FirstOrDefault();
            o.Redirect = element.Elements(MapXmlSerializer.NS + "redirect").Select(Redirect.Read).FirstOrDefault();
        }

        public string Sig { get; set; }

        public MapModifiers Modifiers { get; set; }

        public Parameter[] Parameters { get; set; }

        public InstructionList AlternateBody { get; set; }

        public Redirect Redirect { get; set; }

        internal void Emit(RuntimeClassLoader loader, CodeEmitter ilgen)
        {
            if (Prologue != null)
                Prologue.Emit(loader, ilgen);

            if (Redirect != null)
                Redirect.Emit(loader, ilgen);
            else
                Body.Emit(loader, ilgen);
        }

    }

}
