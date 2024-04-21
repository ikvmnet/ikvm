/*
  Copyright (C) 2002-2014 Jeroen Frijters

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

namespace IKVM.Runtime
{

    /// <summary>
    /// Describes various options applied to a member.
    /// </summary>
    [Flags]
    enum MemberFlags : short
    {

        None = 0,

        /// <summary>
        /// Member should be hidden from Java reflection.
        /// </summary>
        HideFromReflection = 1,

        ExplicitOverride = 2,

        /// <summary>
        /// Member is a Java miranda method.
        /// </summary>
        MirandaMethod = 8,

        AccessStub = 16,

        /// <summary>
        /// Member should be generated with "internal" .NET access.
        /// </summary>
        InternalAccess = 32,

        /// <summary>
        /// Method represents a property accessor.
        /// </summary>
        PropertyAccessor = 64,

        Intrinsic = 128,

        CallerID = 256,

        NonPublicTypeInSignature = 512, // this flag is only available after linking and is not set for access stubs

        DelegateInvokeWithByRefParameter = 1024,

        Type2FinalField = 2048,

        /// <summary>
        /// Member is an empty static initializer.
        /// </summary>
        NoOp = 4096,

        /// <summary>
        /// Member is a module initializer method.
        /// </summary>
        ModuleInitializer = 8192,

    }

}
