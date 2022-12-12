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

using IKVM.Reflection;

namespace IKVM.Tools.Importer.MapXml
{

    public abstract class MethodBase : MapXmlElement
    {

        /// <summary>
        /// Loads the XML element into the instance.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="element"></param>
        public static void Load(MethodBase method, XElement element)
        {
            Load((MapXmlElement)method, element);
            method.MethodAttributes = MapXmlSerializer.ReadMethodAttributes((string)element.Attribute("attributes"));
            method.Body = element.Elements(MapXmlSerializer.NS + "body").Select(InstructionList.Read).FirstOrDefault();
            method.Throws = element.Elements(MapXmlSerializer.NS + "throws").Select(MapXml.Throws.Read).ToArray();
            method.Attributes = element.Elements(MapXmlSerializer.NS + "attribute").Select(Attribute.Read).ToArray();
            method.ReplaceMethodCalls = element.Elements(MapXmlSerializer.NS + "replace-method-call").Select(ReplaceMethodCall.Read).ToArray();
            method.Prologue = element.Elements(MapXmlSerializer.NS + "prologue").Select(InstructionList.Read).FirstOrDefault();
        }

        public MethodAttributes MethodAttributes { get; set; }

        public InstructionList Body { get; set; }

        public Throws[] Throws { get; set; }

        public Attribute[] Attributes { get; set; }

        public ReplaceMethodCall[] ReplaceMethodCalls { get; set; }

        public InstructionList Prologue { get; set; }

        internal abstract MethodKey ToMethodKey(string className);

    }

}
