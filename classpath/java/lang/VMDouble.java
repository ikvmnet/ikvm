/*
  Copyright (C) 2003, 2005, 2006 Jeroen Frijters

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

package java.lang;

import cli.System.BitConverter;
import cli.System.Globalization.CultureInfo;
import ikvm.lang.CIL;

final class VMDouble
{
    static double longBitsToDouble(long v)
    {
	return BitConverter.Int64BitsToDouble(v);
    }

    static long doubleToLongBits(double v)
    {
	if(Double.isNaN(v))
	{
	    return 0x7ff8000000000000L;
	}
	return BitConverter.DoubleToInt64Bits(v);
    }

    static long doubleToRawLongBits(double v)
    {
	return BitConverter.DoubleToInt64Bits(v);
    }

    static String toString(double d, boolean isFloat)
    {
        if(isFloat)
        {
            float f = (float)d;
            if(Float.isNaN(f))
            {
                return "NaN";
            }
            if(Float.isInfinite(f))
            {
                return f < 0f ? "-Infinity" : "Infinity";
            }
            if(f == 0f)
            {
                return BitConverter.DoubleToInt64Bits(d) < 0 ? "-0.0" : "0.0";
            }
            // TODO this is not correct, we need to use the Java algorithm of converting a float to string
            // HACK really lame hack to approximate the Java behavior a little bit
            String s = CIL.box_float(f).ToString(CultureInfo.get_InvariantCulture());
            if(s.indexOf('.') == -1)
            {
                int e = s.indexOf('E');
                if(e == -1)
                {
                    s += ".0";
                }
                else
                {
                    int plus = s.charAt(e + 1) == '+' ? 1 : 0;
                    s = s.substring(0, e) + ".0E" + Integer.parseInt(s.substring(e + 1 + plus));
                }
            }
            else
            {
                int e = s.indexOf('E');
                if(e != -1)
                {
                    int plus = s.charAt(e + 1) == '+' ? 1 : 0;
                    s = s.substring(0, e) + "E" + Integer.parseInt(s.substring(e + 1 + plus));
                }
            }
            return s;
        }
        else
        {
            StringBuffer sb = new StringBuffer();
            DoubleToString.append(sb, d);
            return sb.toString();
        }
    }

    static native double parseDouble(String s);
}
