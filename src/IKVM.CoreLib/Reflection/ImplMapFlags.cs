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
    /// Specifies the flags of an <c>ImplMap</c> metadata row, which describe how a platform invoke method is bound to its
    /// unmanaged entry point. This type has no System.Reflection counterpart; it is specific to IKVM.Reflection.
    /// </summary>
    [Flags]
    enum ImplMapFlags
    {

        /// <summary>
        /// The entry point name is used exactly as specified, without probing for decorated names.
        /// </summary>
        NoMangle = 0x0001,

        /// <summary>
        /// Selects the character set flags.
        /// </summary>
        CharSetMask = 0x0006,

        /// <summary>
        /// No character set is specified.
        /// </summary>
        CharSetNotSpec = 0x0000,

        /// <summary>
        /// Strings are marshaled as ANSI.
        /// </summary>
        CharSetAnsi = 0x0002,

        /// <summary>
        /// Strings are marshaled as Unicode.
        /// </summary>
        CharSetUnicode = 0x0004,

        /// <summary>
        /// The character set is chosen automatically by the platform.
        /// </summary>
        CharSetAuto = 0x0006,

        /// <summary>
        /// The callee calls <c>SetLastError</c> before returning.
        /// </summary>
        SupportsLastError = 0x0040,

        /// <summary>
        /// Selects the calling convention flags.
        /// </summary>
        CallConvMask = 0x0700,

        /// <summary>
        /// The platform's default calling convention.
        /// </summary>
        CallConvWinapi = 0x0100,

        /// <summary>
        /// The C calling convention.
        /// </summary>
        CallConvCdecl = 0x0200,

        /// <summary>
        /// The standard calling convention.
        /// </summary>
        CallConvStdcall = 0x0300,

        /// <summary>
        /// The <c>thiscall</c> calling convention.
        /// </summary>
        CallConvThiscall = 0x0400,

        /// <summary>
        /// The <c>fastcall</c> calling convention.
        /// </summary>
        CallConvFastcall = 0x0500,

        // non-standard flags (i.e. CLR specific)

        /// <summary>
        /// Best-fit mapping of Unicode characters to ANSI is enabled.
        /// </summary>
        BestFitOn = 0x0010,

        /// <summary>
        /// Best-fit mapping of Unicode characters to ANSI is disabled.
        /// </summary>
        BestFitOff = 0x0020,

        /// <summary>
        /// An exception is thrown when an unmappable Unicode character is converted to ANSI.
        /// </summary>
        CharMapErrorOn = 0x1000,

        /// <summary>
        /// No exception is thrown when an unmappable Unicode character is converted to ANSI.
        /// </summary>
        CharMapErrorOff = 0x2000,

    }

}
