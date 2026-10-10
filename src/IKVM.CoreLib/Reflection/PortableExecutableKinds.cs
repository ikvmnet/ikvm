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
    /// Identifies the nature of the code in an executable file.
    /// </summary>
    [Flags]
    enum PortableExecutableKinds
    {

        /// <summary>
        /// The file is not in portable executable (PE) file format.
        /// </summary>
        NotAPortableExecutableImage = 0,

        /// <summary>
        /// The executable contains only Microsoft intermediate language (MSIL), and is therefore neutral with respect to
        /// 32-bit or 64-bit platforms.
        /// </summary>
        ILOnly = 1,

        /// <summary>
        /// The executable can be run on a 32-bit platform, or in the 32-bit Windows on Windows (WOW) environment on a
        /// 64-bit platform.
        /// </summary>
        Required32Bit = 2,

        /// <summary>
        /// The executable requires a 64-bit platform.
        /// </summary>
        PE32Plus = 4,

        /// <summary>
        /// The executable contains pure unmanaged code.
        /// </summary>
        Unmanaged32Bit = 8,

        /// <summary>
        /// The executable is platform-agnostic but should be run on a 32-bit platform whenever possible.
        /// </summary>
        Preferred32Bit = 16,

    }

}
