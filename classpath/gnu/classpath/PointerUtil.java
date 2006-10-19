/*
  Copyright (C) 2006 Jeroen Frijters

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
package gnu.classpath;

import cli.System.IntPtr;

@ikvm.lang.Internal
public final class PointerUtil
{
    private PointerUtil() {}

    public static Pointer add(Pointer p, int offset)
    {
        if (p instanceof Pointer32)
        {
            return new Pointer32(((Pointer32)p).data + offset);
        }
        return new Pointer64(((Pointer64)p).data + offset);
    }

    public static IntPtr toIntPtr(Pointer p)
    {
        if (p instanceof Pointer32)
        {
            return new IntPtr(((Pointer32)p).data);
        }
        return new IntPtr(((Pointer64)p).data);
    }

    public static Pointer fromIntPtr(IntPtr p)
    {
        switch (IntPtr.get_Size())
        {
            case 4:
                return new Pointer32(p.ToInt32());
            case 8:
                return new Pointer64(p.ToInt64());
            default:
                throw new InternalError("Unsupported IntPtr size");
        }
    }
}
