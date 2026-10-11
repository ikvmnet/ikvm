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
    /// Defines the attributes that can be associated with a parameter. These are defined in CorHdr.h.
    /// </summary>
    [Flags]
    enum ParameterAttributes
    {

        /// <summary>
        /// Specifies that there is no parameter attribute.
        /// </summary>
        None = 0,

        /// <summary>
        /// Specifies that the parameter is an input parameter.
        /// </summary>
        In = 1,

        /// <summary>
        /// Specifies that the parameter is an output parameter.
        /// </summary>
        Out = 2,

        /// <summary>
        /// Specifies that the parameter is a locale identifier (lcid).
        /// </summary>
        Lcid = 4,

        /// <summary>
        /// Specifies that the parameter is a return value.
        /// </summary>
        Retval = 8,

        /// <summary>
        /// Specifies that the parameter is optional.
        /// </summary>
        Optional = 16,

        /// <summary>
        /// Specifies that the parameter has a default value.
        /// </summary>
        HasDefault = 4096,

        /// <summary>
        /// Specifies that the parameter has field marshaling information.
        /// </summary>
        HasFieldMarshal = 8192,

        /// <summary>
        /// Reserved.
        /// </summary>
        Reserved3 = 16384,

        /// <summary>
        /// Reserved.
        /// </summary>
        Reserved4 = 32768,

        /// <summary>
        /// Specifies that the parameter is reserved.
        /// </summary>
        ReservedMask = 61440,

    }

}
