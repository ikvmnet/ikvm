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
    /// Provides information about an assembly reference.
    /// </summary>
    [Flags]
    enum AssemblyNameFlags
    {

        /// <summary>
        /// Specifies that no flags are in effect.
        /// </summary>
        None = 0,

        /// <summary>
        /// Specifies that a public key is formed from the full public key rather than the public key token.
        /// </summary>
        PublicKey = 1,

        /// <summary>
        /// Specifies that the assembly can be retargeted at runtime to an assembly from a different publisher.
        /// </summary>
        Retargetable = 256,

        /// <summary>
        /// Specifies that just-in-time (JIT) compiler optimization is disabled for the assembly.
        /// </summary>
        EnableJITcompileOptimizer = 16384,

        /// <summary>
        /// Specifies that just-in-time (JIT) compiler tracking is enabled for the assembly.
        /// </summary>
        EnableJITcompileTracking = 32768,

    }

}
