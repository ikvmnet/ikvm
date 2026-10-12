/*
  Copyright (C) 2012 Jeroen Frijters

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
using System.Collections.Generic;

namespace IKVM.Reflection
{

    /// <summary>
    /// Represents type declarations for class types, interface types, array types, value types, enumeration types, type
    /// parameters, generic type definitions, and open or closed constructed generic types.
    /// </summary>
    abstract class TypeInfo : Type, IReflectableType
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal TypeInfo()
        {
        }

        /// <summary>
        /// Initializes a new instance that wraps another type and shares its type flags.
        /// </summary>
        /// <param name="underlyingType">The type this instance stands in for.</param>
        internal TypeInfo(Type underlyingType) :
            base(underlyingType)
        {
        }

        /// <summary>
        /// Initializes a new instance for a type that is encoded in signatures by a single element type byte.
        /// </summary>
        /// <param name="sigElementType">The signature element type (an <c>ELEMENT_TYPE_*</c> value) of the type.</param>
        internal TypeInfo(byte sigElementType) :
            base(sigElementType)
        {
        }

    }

}
