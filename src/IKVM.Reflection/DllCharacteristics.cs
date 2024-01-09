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

    // IKVM.Reflection specific type
    [Flags]
    public enum DllCharacteristics
    {

        /// <summary>
        /// IMAGE_DLLCHARACTERISTICS_HIGH_ENTROPY_VA
        /// </summary>
        HighEntropyVA = 0x0020,

        /// <summary>
        /// IMAGE_DLLCHARACTERISTICS_DYNAMIC_BASE
        /// </summary>
        DynamicBase = 0x0040,

        /// <summary>
        /// IMAGE_DLLCHARACTERISTICS_NO_SEH
        /// </summary>
        NoSEH = 0x0400,

        /// <summary>
        /// IMAGE_DLLCHARACTERISTICS_NX_COMPAT
        /// </summary>
        NXCompat = 0x0100,

        /// <summary>
        /// IMAGE_DLLCHARACTERISTICS_APPCONTAINER
        /// </summary>
        AppContainer = 0x1000,

        /// <summary>
        /// IMAGE_DLLCHARACTERISTICS_TERMINAL_SERVER_AWARE
        /// </summary>
        TerminalServerAware = 0x8000,

    }

}
