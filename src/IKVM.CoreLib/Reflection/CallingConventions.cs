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
    /// Defines the valid calling conventions for a method.
    /// </summary>
    [Flags]
    enum CallingConventions
    {

        /// <summary>
        /// Specifies the default calling convention as determined by the common language runtime.
        /// </summary>
        Standard = 1,

        /// <summary>
        /// Specifies the calling convention for methods with variable arguments.
        /// </summary>
        VarArgs = 2,

        /// <summary>
        /// Specifies that either the <see cref="Standard"/> or the <see cref="VarArgs"/> calling convention may be used.
        /// </summary>
        Any = 3,

        /// <summary>
        /// Specifies an instance or virtual method (not a static method).
        /// </summary>
        HasThis = 32,

        /// <summary>
        /// Specifies that the signature is a function-pointer signature, representing a call to an instance or virtual
        /// method whose first explicit parameter is the <c>this</c> pointer.
        /// </summary>
        ExplicitThis = 64,

    }

}
