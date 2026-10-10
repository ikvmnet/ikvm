/*
  Copyright (C) 2009-2012 Jeroen Frijters

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

namespace IKVM.Reflection
{

    /// <summary>
    /// Defines the attributes that can be associated with a property.
    /// </summary>
    [Flags]
    enum PropertyAttributes
    {

        /// <summary>
        /// Specifies that no attributes are associated with a property.
        /// </summary>
        None = 0,

        /// <summary>
        /// Specifies that the property is special, with the name describing how the property is special.
        /// </summary>
        SpecialName = 512,

        /// <summary>
        /// Specifies that the metadata internal APIs check the name encoding.
        /// </summary>
        RTSpecialName = 1024,

        /// <summary>
        /// Specifies that the property has a default value.
        /// </summary>
        HasDefault = 4096,

    }

}
