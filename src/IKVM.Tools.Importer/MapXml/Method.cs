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

    public sealed class Method : MethodConstructorBase
    {

        /// <summary>
        /// Reads the XML element into a new <see cref="Method"/> instance.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static Method Read(XElement element)
        {
            var redirect = new Method();
            Load(redirect, element);
            return redirect;
        }

        /// <summary>
        /// Loads the XML element into the instance.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="element"></param>
        public static void Load(Method method, XElement element)
        {
            Load((MethodConstructorBase)method, element);
            method.Name = (string)element.Attribute("name");
            method.NoNullCheck = (bool?)element.Attribute("nonullcheck") ?? true;
            method.NonVirtualAlternateBody = element.Elements(MapXmlSerializer.NS + "nonvirtualAlternateBody").Select(InstructionList.Read).FirstOrDefault();
            method.Override = element.Elements(MapXmlSerializer.NS + "override").Select(Override.Read).FirstOrDefault();
        }

        public string Name { get; set; }

        public bool NoNullCheck { get; set; }

        public InstructionList NonVirtualAlternateBody { get; set; }

        public Override Override { get; set; }

        internal override MethodKey ToMethodKey(string className)
        {
            return new MethodKey(className, Name, Sig);
        }

    }

}
