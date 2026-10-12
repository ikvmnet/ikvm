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
    /// Specifies flags for method attributes.
    /// </summary>
    [Flags]
    enum MethodAttributes
    {

        /// <summary>
        /// Retrieves accessibility information.
        /// </summary>
        MemberAccessMask = 0x0007,

        /// <summary>
        /// Indicates that the member cannot be referenced.
        /// </summary>
        PrivateScope = 0x0000,

        /// <summary>
        /// Indicates that the method is accessible only to the current class.
        /// </summary>
        Private = 0x0001,

        /// <summary>
        /// Indicates that the method is accessible to members of this type and its derived types that are in this
        /// assembly only.
        /// </summary>
        FamANDAssem = 0x0002,

        /// <summary>
        /// Indicates that the method is accessible to any class of this assembly.
        /// </summary>
        Assembly = 0x0003,

        /// <summary>
        /// Indicates that the method is accessible only to members of this class and its derived classes.
        /// </summary>
        Family = 0x0004,

        /// <summary>
        /// Indicates that the method is accessible to derived classes anywhere, as well as to any class in the
        /// assembly.
        /// </summary>
        FamORAssem = 0x0005,

        /// <summary>
        /// Indicates that the method is accessible to any object for which this object is in scope.
        /// </summary>
        Public = 0x0006,

        /// <summary>
        /// Indicates that the method is defined on the type; otherwise, it is defined per instance.
        /// </summary>
        Static = 0x0010,

        /// <summary>
        /// Indicates that the method cannot be overridden.
        /// </summary>
        Final = 0x0020,

        /// <summary>
        /// Indicates that the method is virtual.
        /// </summary>
        Virtual = 0x0040,

        /// <summary>
        /// Indicates that the method hides by name and signature; otherwise, by name only.
        /// </summary>
        HideBySig = 0x0080,

        /// <summary>
        /// Retrieves vtable attributes.
        /// </summary>
        VtableLayoutMask = 0x0100,

        /// <summary>
        /// Indicates that the method will reuse an existing slot in the vtable. This is the default behavior.
        /// </summary>
        ReuseSlot = 0x0000,

        /// <summary>
        /// Indicates that the method always gets a new slot in the vtable.
        /// </summary>
        NewSlot = 0x0100,

        /// <summary>
        /// Indicates that the method can only be overridden when it is also accessible.
        /// </summary>
        CheckAccessOnOverride = 0x0200,

        /// <summary>
        /// Indicates that the class does not provide an implementation of this method.
        /// </summary>
        Abstract = 0x0400,

        /// <summary>
        /// Indicates that the method is special. The name describes how this method is special.
        /// </summary>
        SpecialName = 0x0800,

        /// <summary>
        /// Indicates that the method implementation is forwarded through PInvoke (Platform Invocation Services).
        /// </summary>
        PinvokeImpl = 0x2000,

        /// <summary>
        /// Indicates that the managed method is exported by thunk to unmanaged code.
        /// </summary>
        UnmanagedExport = 0x0008,

        /// <summary>
        /// Indicates that the common language runtime checks the name encoding.
        /// </summary>
        RTSpecialName = 0x1000,

        /// <summary>
        /// Indicates that the method has security associated with it. Reserved flag for runtime use only.
        /// </summary>
        HasSecurity = 0x4000,

        /// <summary>
        /// Indicates that the method calls another method containing security code. Reserved flag for runtime use only.
        /// </summary>
        RequireSecObject = 0x8000,

        /// <summary>
        /// Indicates a reserved flag for runtime use only.
        /// </summary>
        ReservedMask = 0xd000,

    }

}
