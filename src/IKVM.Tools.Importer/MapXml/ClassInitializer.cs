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

using System.Xml.Linq;

using IKVM.CoreLib.Runtime;
using IKVM.Runtime;

namespace IKVM.Tools.Importer.MapXml
{

    internal sealed class ClassInitializer : MethodBase
    {

        /// <summary>
        /// Reads the XML element into a new <see cref="ClassInitializer"/> instance.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public static ClassInitializer Read(XElement element)
        {
            var clinit = new ClassInitializer();
            Load(clinit, element);
            return clinit;
        }

        /// <summary>
        /// Loads the XML element into the instance.
        /// </summary>
        /// <param name="clinit"></param>
        /// <param name="element"></param>
        public static void Load(ClassInitializer clinit, XElement element)
        {
            Load((MethodBase)clinit, element);
        }

        internal override MethodKey ToMethodKey(string className)
        {
            return new MethodKey(className, StringConstants.CLINIT, StringConstants.SIG_VOID);
        }

    }

}
