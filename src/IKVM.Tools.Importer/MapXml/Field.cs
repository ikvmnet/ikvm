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

namespace IKVM.Tools.Importer.MapXml
{

    public sealed class Field : MapXmlElement
    {

        /// <summary>
        /// Reads the XML element into a new <see cref="Field"/> instance.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Field Read(XElement element)
        {
            var field = new Field();
            Load(field, element);
            return field;
        }

        /// <summary>
        /// Loads the XML element into the instruction.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="element"></param>
        public static void Load(Field field, XElement element)
        {
            Load((MapXmlElement)field, element);
            field.Name = (string)element.Attribute("name");
            field.Sig = (string)element.Attribute("sig");
            field.Modifiers = MapXmlSerializer.ReadMapModifiers((string)element.Attribute("modifiers"));
            field.Constant = (string)element.Attribute("constant");
            field.Attributes = element.Elements(MapXmlSerializer.NS + "attribute").Select(Attribute.Read).ToArray();
        }

        public string Name { get; set; }

        public string Sig { get; set; }

        public MapModifiers Modifiers { get; set; }

        public string Constant { get; set; }

        public Attribute[] Attributes { get; set; }

    }

}
