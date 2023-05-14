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

    public sealed class Property
    {

        /// <summary>
        /// Reads the XML element into a new <see cref="Property"/> instance.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Property Read(XElement element)
        {
            var property = new Property();
            Load(property, element);
            return property;
        }

        /// <summary>
        /// Loads the XML element into the class.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="element"></param>
        public static void Load(Property property, XElement element)
        {
            property.Name = (string)element.Attribute("name");
            property.Sig = (string)element.Attribute("sig");
            property.Getter = element.Elements(MapXmlSerializer.NS + "getter").Select(Method.Read).FirstOrDefault();
            property.Setter = element.Elements(MapXmlSerializer.NS + "setter").Select(Method.Read).FirstOrDefault();
            property.Attributes = element.Elements(MapXmlSerializer.NS + "attribute").Select(Attribute.Read).ToArray();
        }

        public string Name { get; set; }

        public string Sig { get; set; }

        public Method Getter { get; set; }

        public Method Setter { get; set; }

        public Attribute[] Attributes { get; set; }

    }

}
