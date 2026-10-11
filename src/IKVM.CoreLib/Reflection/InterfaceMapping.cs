/*
  Copyright (C) 2009 Jeroen Frijters

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
    /// Retrieves the mapping of an interface into the actual methods on a class that implements that interface.
    /// </summary>
    struct InterfaceMapping
    {

        /// <summary>
        /// Shows the methods that are defined on the interface.
        /// </summary>
        public MethodInfo[] InterfaceMethods;

        /// <summary>
        /// Shows the type that represents the interface.
        /// </summary>
        public Type InterfaceType;

        /// <summary>
        /// Shows the methods that implement the interface.
        /// </summary>
        public MethodInfo[] TargetMethods;

        /// <summary>
        /// Represents the type that was used to create the interface mapping.
        /// </summary>
        public Type TargetType;

    }

}
