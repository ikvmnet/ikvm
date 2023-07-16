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

#if IMPORTER || EXPORTER
using Type = IKVM.Reflection.Type;
#endif

namespace IKVM.Attributes
{

    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method)]
	public sealed class ThrowsAttribute : Attribute
	{

		internal string[] classes;
		internal Type[] types;

		// this constructor is used by ikvmc, the other constructors are for use in other .NET languages
		public ThrowsAttribute(string[] classes)
		{
			this.classes = UnicodeUtil.UnescapeInvalidSurrogates(classes);
		}

		public ThrowsAttribute(Type type)
			: this(new Type[] { type })
		{
		}

		public ThrowsAttribute(params Type[] types)
		{
			this.types = types;
		}

        // dotted Java class names (e.g. java.lang.Throwable)
        [Obsolete]
        public string[] Classes => classes;

    }

}
