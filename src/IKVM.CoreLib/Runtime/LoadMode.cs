/*
  Copyright (C) 2002-2015 Jeroen Frijters

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

namespace IKVM.CoreLib.Runtime
{

    [Flags]
    internal enum LoadMode
    {

        // These are the modes that should be used
        Find = ReturnNull,
        LoadOrNull = Load | ReturnNull,
        LoadOrThrow = Load | ThrowClassNotFound,
        Link = Load | ReturnUnloadable | SuppressExceptions,

        // call into Java class loader
        Load = 0x0001,

        // return value
        DontReturnUnloadable = 0x0002,  // This is used with a bitwise OR to disable returning unloadable
        ReturnUnloadable = 0x0004,
        ReturnNull = 0x0004 | DontReturnUnloadable,
        ThrowClassNotFound = 0x0008 | DontReturnUnloadable,
        MaskReturn = ReturnUnloadable | ReturnNull | ThrowClassNotFound,

        // exceptions (not ClassNotFoundException)
        SuppressExceptions = 0x0010,

        // warnings
        WarnClassNotFound = 0x0020,

    }

}
