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
    /// Marks each type of member that is defined as a derived class of <see cref="MemberInfo"/>.
    /// </summary>
    [Flags]
    enum MemberTypes
    {

        /// <summary>
        /// Specifies that the member is a constructor.
        /// </summary>
        Constructor = 0x01,

        /// <summary>
        /// Specifies that the member is an event.
        /// </summary>
        Event = 0x02,

        /// <summary>
        /// Specifies that the member is a field.
        /// </summary>
        Field = 0x04,

        /// <summary>
        /// Specifies that the member is a method.
        /// </summary>
        Method = 0x08,

        /// <summary>
        /// Specifies that the member is a property.
        /// </summary>
        Property = 0x10,

        /// <summary>
        /// Specifies that the member is a type.
        /// </summary>
        TypeInfo = 0x20,

        /// <summary>
        /// Specifies that the member is a custom member type.
        /// </summary>
        Custom = 0x40,

        /// <summary>
        /// Specifies that the member is a nested type.
        /// </summary>
        NestedType = 0x80,

        /// <summary>
        /// Specifies all member types.
        /// </summary>
        All = Constructor | Event | Field | Method | Property | TypeInfo | NestedType

    }

}
