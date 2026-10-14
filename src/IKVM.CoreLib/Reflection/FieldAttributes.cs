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
    /// Specifies flags that describe the attributes of a field.
    /// </summary>
    [Flags]
    enum FieldAttributes
    {

        /// <summary>
        /// Specifies that the field cannot be referenced.
        /// </summary>
        PrivateScope = 0,

        /// <summary>
        /// Specifies that the field is accessible only by the parent type.
        /// </summary>
        Private = 1,

        /// <summary>
        /// Specifies that the field is accessible only by subtypes in this assembly.
        /// </summary>
        FamANDAssem = 2,

        /// <summary>
        /// Specifies that the field is accessible throughout the assembly.
        /// </summary>
        Assembly = 3,

        /// <summary>
        /// Specifies that the field is accessible only by type and subtypes.
        /// </summary>
        Family = 4,

        /// <summary>
        /// Specifies that the field is accessible by subtypes anywhere, as well as throughout this assembly.
        /// </summary>
        FamORAssem = 5,

        /// <summary>
        /// Specifies that the field is accessible by any member for whom this scope is visible.
        /// </summary>
        Public = 6,

        /// <summary>
        /// Specifies the access level of a given field.
        /// </summary>
        FieldAccessMask = 7,

        /// <summary>
        /// Specifies that the field represents the defined type, or else it is per-instance.
        /// </summary>
        Static = 16,

        /// <summary>
        /// Specifies that the field is initialized only, and can be set only in the body of a constructor.
        /// </summary>
        InitOnly = 32,

        /// <summary>
        /// Specifies that the field's value is a compile-time (static or early bound) constant.
        /// </summary>
        Literal = 64,

        /// <summary>
        /// Specifies that the field does not have to be serialized when the type is remoted.
        /// </summary>
        NotSerialized = 128,

        /// <summary>
        /// Specifies that the field has a relative virtual address (RVA).
        /// </summary>
        HasFieldRVA = 256,

        /// <summary>
        /// Specifies a special method, with the name describing how the method is special.
        /// </summary>
        SpecialName = 512,

        /// <summary>
        /// Specifies that the common language runtime (metadata internal APIs) should check the name encoding.
        /// </summary>
        RTSpecialName = 1024,

        /// <summary>
        /// Specifies that the field has marshaling information.
        /// </summary>
        HasFieldMarshal = 4096,

        /// <summary>
        /// Reserved for future use.
        /// </summary>
        PinvokeImpl = 8192,

        /// <summary>
        /// Specifies that the field has a default value.
        /// </summary>
        HasDefault = 32768,

        /// <summary>
        /// Reserved.
        /// </summary>
        ReservedMask = 38144,

    }

}
