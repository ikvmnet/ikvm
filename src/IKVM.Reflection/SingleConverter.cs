﻿/*
  Copyright (C) 2008-2011 Jeroen Frijters

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

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Explicit)]
    struct SingleConverter
    {

        [System.Runtime.InteropServices.FieldOffset(0)]
        int i;
        [System.Runtime.InteropServices.FieldOffset(0)]
        float f;

        internal static int SingleToInt32Bits(float v)
        {
            var c = new SingleConverter();
            c.f = v;
            return c.i;
        }

        internal static float Int32BitsToSingle(int v)
        {
            var c = new SingleConverter();
            c.i = v;
            return c.f;
        }

    }

}
