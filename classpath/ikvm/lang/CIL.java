/*
  Copyright (C) 2003, 2004, 2005 Jeroen Frijters

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
package ikvm.lang;

public class CIL
{
    public static native byte unbox_byte(Object o);
    public static native boolean unbox_boolean(Object o);
    public static native short unbox_short(Object o);
    public static native char unbox_char(Object o);
    public static native int unbox_int(Object o);
    public static native float unbox_float(Object o);
    public static native long unbox_long(Object o);
    public static native double unbox_double(Object o);

    public static native cli.System.Byte box_byte(byte v);
    public static native cli.System.Boolean box_boolean(boolean v);
    public static native cli.System.Int16 box_short(short v);
    public static native cli.System.Char box_char(char v);
    public static native cli.System.Int32 box_int(int v);
    public static native cli.System.Single box_float(float v);
    public static native cli.System.Int64 box_long(long v);
    public static native cli.System.Double box_double(double v);

    public static native cli.System.SByte box_sbyte(byte v);
    public static native cli.System.UInt16 box_ushort(short v);
    public static native cli.System.UInt32 box_uint(int v);
    public static native cli.System.UInt64 box_ulong(long v);

    public static native byte unbox_sbyte(cli.System.SByte v);
    public static native short unbox_ushort(cli.System.UInt16 v);
    public static native int unbox_uint(cli.System.UInt32 v);
    public static native long unbox_ulong(cli.System.UInt64 v);
}
