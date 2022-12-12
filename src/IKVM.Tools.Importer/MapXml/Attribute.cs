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

    public sealed class Attribute : MapXmlElement
    {

        /// <summary>
        /// Reads the XML element into a new <see cref="Attribute"/> instance.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Attribute Read(XElement element)
        {
            var attribute = new Attribute();
            Load(attribute, element);
            return attribute;
        }

        /// <summary>
        /// Loads the XML element into the instance.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="element"></param>
        public static void Load(Attribute attribute, XElement element)
        {
            Load((MapXmlElement)attribute, element);
            attribute.Type = (string)element.Attribute("type");
            attribute.Class = (string)element.Attribute("class");
            attribute.Sig = (string)element.Attribute("sig");
            attribute.Params = element.Elements(MapXmlSerializer.NS + "parameter").Select(Param.Read).ToArray();
            attribute.Properties = element.Elements(MapXmlSerializer.NS + "property").Select(Param.Read).ToArray();
            attribute.Fields = element.Elements(MapXmlSerializer.NS + "field").Select(Param.Read).ToArray();
        }

        public string Type { get; set; }

        public string Class { get; set; }

        public string Sig { get; set; }

        public Param[] Params { get; set; }

        public Param[] Properties { get; set; }

        public Param[] Fields { get; set; }

    }

}
