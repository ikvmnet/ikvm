/*
  Copyright (C) 2009-2013 Jeroen Frijters

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
    /// Describes the outcome of comparing two assembly identities, mirroring the result of the Fusion
    /// <c>CompareAssemblyIdentity</c> API.
    /// </summary>
    enum AssemblyComparisonResult
    {

        /// <summary>
        /// The result of the comparison is not known.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The identities are equivalent, all fields match.
        /// </summary>
        EquivalentFullMatch = 1,

        /// <summary>
        /// The identities are equivalent, and both are not strong named.
        /// </summary>
        EquivalentWeakNamed = 2,

        /// <summary>
        /// The identities are equivalent because the versions are unified by the framework.
        /// </summary>
        EquivalentFXUnified = 3,

        /// <summary>
        /// The identities are equivalent because the versions are unified by policy.
        /// </summary>
        EquivalentUnified = 4,

        /// <summary>
        /// The identities are not equivalent, because the versions do not match.
        /// </summary>
        NonEquivalentVersion = 5,

        /// <summary>
        /// The identities are not equivalent.
        /// </summary>
        NonEquivalent = 6,

        /// <summary>
        /// The partial identity is equivalent, all fields it specifies match.
        /// </summary>
        EquivalentPartialMatch = 7,

        /// <summary>
        /// The partial identity is equivalent, and both are not strong named.
        /// </summary>
        EquivalentPartialWeakNamed = 8,

        /// <summary>
        /// The partial identity is equivalent because the versions are unified by policy.
        /// </summary>
        EquivalentPartialUnified = 9,

        /// <summary>
        /// The partial identity is equivalent because the versions are unified by the framework.
        /// </summary>
        EquivalentPartialFXUnified = 10,

        /// <summary>
        /// The partial identity is not equivalent, because the versions do not match.
        /// </summary>
        NonEquivalentPartialVersion = 11,

    }

}
