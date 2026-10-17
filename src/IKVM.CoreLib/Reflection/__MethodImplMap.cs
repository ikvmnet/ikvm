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
namespace IKVM.Reflection
{

    /// <summary>
    /// Describes the explicit method implementations (the MethodImpl table rows) of a type, grouped by method body.
    /// </summary>
    struct __MethodImplMap
    {

        /// <summary>
        /// The type whose method implementations are described.
        /// </summary>
        public Type TargetType;

        /// <summary>
        /// The distinct methods that implement one or more method declarations.
        /// </summary>
        public MethodInfo[] MethodBodies;

        /// <summary>
        /// For each entry in <see cref="MethodBodies"/>, the method declarations that the body at the same index
        /// implements.
        /// </summary>
        public MethodInfo[][] MethodDeclarations;

    }

}
