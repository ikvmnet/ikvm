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

package java.lang;

class VMMath
{
    static double sin(double d)
    {
        return cli.System.Math.Sin(d);
    }

    static double cos(double d)
    {
        return cli.System.Math.Cos(d);
    }

    static double tan(double d)
    {
        return cli.System.Math.Tan(d);
    }

    static double asin(double d)
    {
        return cli.System.Math.Asin(d);
    }

    static double acos(double d)
    {
        return cli.System.Math.Acos(d);
    }

    static double atan(double d)
    {
        return cli.System.Math.Atan(d);
    }

    static double atan2(double y, double x)
    {
        if(cli.System.Double.IsInfinity(y) && cli.System.Double.IsInfinity(x))
        {
            if(cli.System.Double.IsPositiveInfinity(y))
            {
                if(cli.System.Double.IsPositiveInfinity(x))
                {
                    return cli.System.Math.PI / 4.0;
                }
                else
                {
                    return cli.System.Math.PI * 3.0 / 4.0;
                }
            }
            else
            {
                if(cli.System.Double.IsPositiveInfinity(x))
                {
                    return - cli.System.Math.PI / 4.0;
                }
                else
                {
                    return - cli.System.Math.PI * 3.0 / 4.0;
                }
            }
        }
        return cli.System.Math.Atan2(y, x);
    }

    static double exp(double d)
    {
        return cli.System.Math.Exp(d);
    }

    static double log(double d)
    {
        return cli.System.Math.Log(d);
    }

    static double sqrt(double d)
    {
        return cli.System.Math.Sqrt(d);
    }

    static double pow(double x, double y)
    {
        if(cli.System.Math.Abs(x) == 1.0 && cli.System.Double.IsInfinity(y))
        {
            return Double.NaN;
        }
        return cli.System.Math.Pow(x, y);
    }

    static double IEEEremainder(double f1, double f2)
    {
        if(cli.System.Double.IsInfinity(f2) && !cli.System.Double.IsInfinity(f1))
        {
            return f1;
        }
        return cli.System.Math.IEEERemainder(f1, f2);
    }

    static double ceil(double d)
    {
        return cli.System.Math.Ceiling(d);
    }

    static double floor(double d)
    {
        return cli.System.Math.Floor(d);
    }

    static double rint(double d)
    {
        return cli.System.Math.Round(d);
    }

    static double cbrt(double d)
    {
        return cli.System.Math.Pow(d, 1.0 / 3.0);
    }

    static double cosh(double d)
    {
        return cli.System.Math.Cosh(d);
    }

    static double expm1(double d)
    {
        return cli.System.Math.Exp(d) - 1.0;
    }

    static double hypot(double a, double b)
    {
        return a * a  + b * b;
    }

    static double log10(double d)
    {
        return cli.System.Math.Log10(d);
    }

    static double log1p(double d)
    {
        return cli.System.Math.Log(d + 1.0);
    }

    static double sinh(double d)
    {
        return cli.System.Math.Sinh(d);
    }

    static double tanh(double d)
    {
        return cli.System.Math.Tanh(d);
    }
}
