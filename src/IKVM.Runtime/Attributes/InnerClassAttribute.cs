/*
  Copyright (C) 2002-2014 Jeroen Frijters

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
using System;

using IKVM.Runtime;

namespace IKVM.Attributes
{

    // NOTE this attribute is also used by annotation attribute classes,
    // to give them a different name in the Java world ($Proxy[Annotation]).
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
	public sealed class InnerClassAttribute : Attribute
	{

		readonly string innerClassName;
		readonly Modifiers modifiers;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="innerClassName"></param>
        /// <param name="modifiers"></param>
		public InnerClassAttribute(string innerClassName, Modifiers modifiers)
		{
			this.innerClassName = UnicodeUtil.UnescapeInvalidSurrogates(innerClassName);
			this.modifiers = modifiers;
		}

        public string InnerClassName => innerClassName;

        public Modifiers Modifiers => modifiers;

    }

}
