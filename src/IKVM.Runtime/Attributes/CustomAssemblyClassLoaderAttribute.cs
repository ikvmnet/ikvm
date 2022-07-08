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

#if STATIC_COMPILER || STUB_GENERATOR
using Type = IKVM.Reflection.Type;
#endif

namespace IKVM.Attributes
{

    /// <summary>
    /// Marks an assembly such that it's types are considered to be loaded by the specified class loader type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
	public sealed class CustomAssemblyClassLoaderAttribute : Attribute
	{

		readonly Type type;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="type"></param>
		public CustomAssemblyClassLoaderAttribute(Type type)
		{
			this.type = type ?? throw new ArgumentNullException(nameof(type));
		}

        /// <summary>
        /// Gets the type of the class loader.
        /// </summary>
        public Type Type => type;

    }

}
