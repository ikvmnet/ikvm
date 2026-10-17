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
    /// Specifies flags that control binding and the way in which the search for members and types is conducted by
    /// reflection.
    /// </summary>
    [Flags]
    enum BindingFlags
    {

        /// <summary>
        /// Specifies that no binding flags are defined.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Specifies that the case of the member name should not be considered when binding.
        /// </summary>
        IgnoreCase = 1,

        /// <summary>
        /// Specifies that only members declared at the level of the supplied type's hierarchy should be considered.
        /// Inherited members are not considered.
        /// </summary>
        DeclaredOnly = 2,

        /// <summary>
        /// Specifies that instance members are to be included in the search.
        /// </summary>
        Instance = 4,

        /// <summary>
        /// Specifies that static members are to be included in the search.
        /// </summary>
        Static = 8,

        /// <summary>
        /// Specifies that public members are to be included in the search.
        /// </summary>
        Public = 16,

        /// <summary>
        /// Specifies that non-public members are to be included in the search.
        /// </summary>
        NonPublic = 32,

        /// <summary>
        /// Specifies that public and protected static members up the hierarchy should be returned. Private static
        /// members in inherited classes are not returned.
        /// </summary>
        FlattenHierarchy = 64,

    }

}
