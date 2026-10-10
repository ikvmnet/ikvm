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
    /// Specifies flags for the attributes of a method implementation.
    /// </summary>
    [Flags]
    enum MethodImplAttributes
    {

        /// <summary>
        /// Specifies flags about code type.
        /// </summary>
        CodeTypeMask = 0x0003,

        /// <summary>
        /// Specifies that the method implementation is in Microsoft intermediate language (MSIL).
        /// </summary>
        IL = 0x0000,

        /// <summary>
        /// Specifies that the method implementation is native.
        /// </summary>
        Native = 0x0001,

        /// <summary>
        /// Specifies that the method implementation is in Optimized Intermediate Language (OPTIL).
        /// </summary>
        OPTIL = 0x0002,

        /// <summary>
        /// Specifies that the method implementation is provided by the runtime.
        /// </summary>
        Runtime = 0x0003,

        /// <summary>
        /// Specifies whether the method is implemented in managed or unmanaged code.
        /// </summary>
        ManagedMask = 0x0004,

        /// <summary>
        /// Specifies that the method is implemented in unmanaged code.
        /// </summary>
        Unmanaged = 0x0004,

        /// <summary>
        /// Specifies that the method is implemented in managed code.
        /// </summary>
        Managed = 0x0000,

        /// <summary>
        /// Specifies that the method is not defined.
        /// </summary>
        ForwardRef = 0x0010,

        /// <summary>
        /// Specifies that the method signature is exported exactly as declared.
        /// </summary>
        PreserveSig = 0x0080,

        /// <summary>
        /// Specifies an internal call.
        /// </summary>
        InternalCall = 0x1000,

        /// <summary>
        /// Specifies that the method is single-threaded through the body.
        /// </summary>
        Synchronized = 0x0020,

        /// <summary>
        /// Specifies that the method cannot be inlined.
        /// </summary>
        NoInlining = 0x0008,

        /// <summary>
        /// Specifies that the method is not optimized by the just-in-time (JIT) compiler or by native code generation
        /// when debugging possible code generation problems.
        /// </summary>
        NoOptimization = 0x0040,

        /// <summary>
        /// Specifies that the method should be inlined wherever possible.
        /// </summary>
        AggressiveInlining = 0x0100,

        /// <summary>
        /// Specifies a range check value.
        /// </summary>
        MaxMethodImplVal = 0xffff,

    }

}
