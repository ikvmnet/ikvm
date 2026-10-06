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
using System;

namespace IKVM.Reflection
{

    /// <summary>
    /// Options that configure a <see cref="Universe"/>.
    /// </summary>
    [Flags]
    internal enum UniverseOptions
    {

        /// <summary>
        /// Default behavior, most compatible with System.Reflection.
        /// </summary>
        None = 0,

        /// <summary>
        /// Represents function pointers in signatures as first class types (<see cref="Type.IsFunctionPointer"/>)
        /// instead of replacing them by System.IntPtr.
        /// </summary>
        EnableFunctionPointers = 1,

        /// <summary>
        /// Resolves references to missing assemblies, types and members to placeholders instead of throwing.
        /// </summary>
        ResolveMissingMembers = 32,

        /// <summary>
        /// Makes the output depend only on the input: the PE time stamp is zero and the module version id is derived
        /// from the content.
        /// </summary>
        DeterministicOutput = 256,

    }

}
