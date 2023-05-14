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

using System.Xml;
using System.Xml.Linq;

namespace IKVM.Tools.Importer.MapXml
{

    public abstract class MapXmlElement
    {

        /// <summary>
        /// Loads the XML element into the instance.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="element"></param>
        protected static void Load(MapXmlElement e, XElement element)
        {
            e.LineNumber = ((IXmlLineInfo)element).LineNumber;
            e.LinePosition = ((IXmlLineInfo)element).LinePosition;
        }

        /// <summary>
        /// Gets the line number in the source file where this element occurs.
        /// </summary>
        public int LineNumber { get; set; }

        /// <summary>
        /// Gets the line position in the source file where this element occurs.
        /// </summary>
        public int LinePosition { get; set; }

    }

}