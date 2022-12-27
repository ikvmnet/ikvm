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

using System.Linq;
using System.Xml.Linq;

namespace IKVM.Tools.Importer.MapXml
{

    public sealed class Root : MapXmlElement
    {

        /// <summary>
        /// Reads the XML element into a new <see cref="Root"/> instance.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Root Read(XElement element)
        {
            var root = new Root();
            Load(root, element);
            return root;
        }

        /// <summary>
        /// Loads the XML element into the instruction.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="element"></param>
        public static void Load(Root root, XElement element)
        {
            Load((MapXmlElement)root, element);
            root.Assembly = element.Elements(MapXmlSerializer.NS + "assembly").Select(Assembly.Read).FirstOrDefault();
            root.ExceptionMappings = element.Elements(MapXmlSerializer.NS + "exceptionMappings").Elements(MapXmlSerializer.NS + "exception").Select(ExceptionMapping.Read).ToArray();
        }

        public Assembly Assembly { get; set; }

        public ExceptionMapping[] ExceptionMappings { get; set; }

    }

}
