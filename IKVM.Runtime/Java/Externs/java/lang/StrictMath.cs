/*
  Copyright (C) 2007-2014 Jeroen Frijters

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

using IKVM.Runtime.Util.Java.Lang;

namespace IKVM.Java.Externs.java.lang
{

    static class StrictMath
    {

        public static double sin(double d)
        {
#if FIRST_PASS
            return 0;
#else
            return global::ikvm.@internal.JMath.sin(d);
#endif
        }

        public static double cos(double d)
        {
#if FIRST_PASS
            return 0;
#else
            return global::ikvm.@internal.JMath.cos(d);
#endif
        }

        public static double tan(double d)
        {
            return fdlibm.tan(d);
        }

        public static double asin(double d)
        {
#if FIRST_PASS
            return 0;
#else
            return global::ikvm.@internal.JMath.asin(d);
#endif
        }

        public static double acos(double d)
        {
#if FIRST_PASS
        return 0;
#else
            return global::ikvm.@internal.JMath.acos(d);
#endif
        }

        public static double atan(double d)
        {
#if FIRST_PASS
            return 0;
#else
            return global::ikvm.@internal.JMath.atan(d);
#endif
        }

        public static double exp(double d)
        {
#if FIRST_PASS
            return 0;
#else
            return global::ikvm.@internal.JMath.exp(d);
#endif
        }

        public static double log(double d)
        {
            // FPU behavior is correct
            return Math.Log(d);
        }

        public static double log10(double d)
        {
            // FPU behavior is correct
            return Math.Log10(d);
        }

        public static double sqrt(double d)
        {
            // FPU behavior is correct
            return Math.Sqrt(d);
        }

        public static double cbrt(double d)
        {
            return fdlibm.cbrt(d);
        }

        public static double IEEEremainder(double f1, double f2)
        {
#if FIRST_PASS
            return 0;
#else
            return global::ikvm.@internal.JMath.IEEEremainder(f1, f2);
#endif
        }

        public static double atan2(double y, double x)
        {
#if FIRST_PASS
            return 0;
#else
            return global::ikvm.@internal.JMath.atan2(y, x);
#endif
        }

        public static double pow(double x, double y)
        {
            return fdlibm.__ieee754_pow(x, y);
        }

        public static double sinh(double d)
        {
            return Math.Sinh(d);
        }

        public static double cosh(double d)
        {
            return Math.Cosh(d);
        }

        public static double tanh(double d)
        {
            return Math.Tanh(d);
        }

        public static double hypot(double a, double b)
        {
            return fdlibm.__ieee754_hypot(a, b);
        }

        public static double expm1(double d)
        {
            return fdlibm.expm1(d);
        }

        public static double log1p(double d)
        {
            return fdlibm.log1p(d);
        }

    }

}
