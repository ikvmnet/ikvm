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
package ikvm.internal;

import cli.System.IFormatProvider;
import cli.System.IFormattable;
import ikvm.lang.CIL;
import ikvm.lang.Internal;

@Internal
public final class Formatter
{
    private Formatter() {}

    public static String ToString(Byte b, String format, IFormatProvider provider)
    {
        return CIL.box_sbyte(b.byteValue()).ToString(format, provider);
    }

    public static String ToString(Short s, String format, IFormatProvider provider)
    {
        return CIL.box_short(s.shortValue()).ToString(format, provider);
    }

    public static String ToString(Integer i, String format, IFormatProvider provider)
    {
        return CIL.box_int(i.intValue()).ToString(format, provider);
    }

    public static String ToString(Long l, String format, IFormatProvider provider)
    {
        return CIL.box_long(l.longValue()).ToString(format, provider);
    }

    public static String ToString(Float f, String format, IFormatProvider provider)
    {
        return CIL.box_float(f.floatValue()).ToString(format, provider);
    }

    public static String ToString(Double d, String format, IFormatProvider provider)
    {
        return CIL.box_double(d.doubleValue()).ToString(format, provider);
    }
}
