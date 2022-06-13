/*
 * -------------------------------------------------------------------------
 *        $Id: JMath.java 4369 2007-01-16 16:58:08Z fviale $
 * -------------------------------------------------------------------------
 *        Copyright (c) 1999 Visual Numerics Inc. All Rights Reserved.
 *
 * Permission to use, copy, modify, and distribute this software is freely
 * granted by Visual Numerics, Inc., provided that the copyright notice
 * above and the following warranty disclaimer are preserved in human
 * readable form.
 *
 * Because this software is licenses free of charge, it is provided
 * "AS IS", with NO WARRANTY.  TO THE EXTENT PERMITTED BY LAW, VNI
 * DISCLAIMS ALL WARRANTIES, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
 * TO ITS PERFORMANCE, MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.
 * VNI WILL NOT BE LIABLE FOR ANY DAMAGES WHATSOEVER ARISING OUT OF THE USE
 * OF OR INABILITY TO USE THIS SOFTWARE, INCLUDING BUT NOT LIMITED TO DIRECT,
 * INDIRECT, SPECIAL, CONSEQUENTIAL, PUNITIVE, AND EXEMPLARY DAMAGES, EVEN
 * IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGES.
 *
 *
 * This Java code is based on C code in the package fdlibm,
 * which can be obtained from www.netlib.org.
 * The original fdlibm C code contains the following notice.
 *
 * Copyright (C) 1993 by Sun Microsystems, Inc. All rights reserved.
 *
 * Developed at SunSoft, a Sun Microsystems, Inc. business.
 * Permission to use, copy, modify, and distribute this
 * software is freely granted, provided that this notice
 * is preserved.
 *
 *--------------------------------------------------------------------------
 */

package ikvm.internal;
import java.util.Random;


/*
 *        Pure Java implementation of the standard java.lang.Math class.
 *        This Java code is based on C code in the package fdlibm,
 *        which can be obtained from www.netlib.org.
 *
 *        @author Sun Microsystems (original C code in fdlibm)
 *        @author        John F. Brophy (translated from C to Java)
 */
@ikvm.lang.Internal
public final class JMath {
    static public final double PI = 0x1.921fb54442d18p1; /* 3.14159265358979323846 */
    static public final double E = 2.7182818284590452354;
    static private Random random;

    /**
     *        Returns the absolute value of its argument.
     *        @param        x        The argument, an integer.
     *        @return        Returns |x|.
     */
    strictfp public static int abs(int x) {
        return ((x < 0) ? (-x) : x);
    }

    /**
     *        Returns the absolute value of its argument.
     *        @param        x        The argument, a long.
     *        @return        Returns |x|.
     */
    strictfp public static long abs(long x) {
        return ((x < 0L) ? (-x) : x);
    }

    /**
     *        Returns the absolute value of its argument.
     *        @param        x        The argument, a float.
     *        @return        Returns |x|.
     */
    strictfp public static float abs(float x) {
        return ((x <= 0.0f) ? (0.0f - x) : x);
    }

    /**
     *        Returns the absolute value of its argument.
     *        @param        x        The argument, a double.
     *        @return        Returns |x|.
     */
    strictfp public static double abs(double x) {
        return ((x <= 0.0) ? (0.0 - x) : x);
    }

    /**
     *        Returns the smaller of its two arguments.
     *        @param        x        The first argument, an integer.
     *        @param        y        The second argument, an integer.
     *        @return        Returns the smaller of x and y.
     */
    strictfp public static int min(int x, int y) {
        return ((x < y) ? x : y);
    }

    /**
     *        Returns the smaller of its two arguments.
     *        @param        x        The first argument, a long.
     *        @param        y        The second argument, a long.
     *        @return        Returns the smaller of x and y.
     */
    strictfp public static long min(long x, long y) {
        return ((x < y) ? x : y);
    }

    /**
     *        Returns the smaller of its two arguments.
     *        @param        x        The first argument, a float.
     *        @param        y        The second argument, a float.
     *        @return        Returns the smaller of x and y.
     *                        This function considers -0.0f to
     *                        be less than 0.0f.
     */
    strictfp public static float min(float x, float y) {
        if (Float.isNaN(x)) {
            return x;
        }
        float ans = ((x <= y) ? x : y);
        if ((ans == 0.0f) && (Float.floatToIntBits(y) == 0x80000000)) {
            ans = y;
        }
        return ans;
    }

    /**
     *        Returns the smaller of its two arguments.
     *        @param        x        The first argument, a double.
     *        @param        y        The second argument, a double.
     *        @return        Returns the smaller of x and y.
     *                        This function considers -0.0 to
     *                        be less than 0.0.
     */
    strictfp public static double min(double x, double y) {
        if (Double.isNaN(x)) {
            return x;
        }
        double ans = ((x <= y) ? x : y);
        if ((x == 0.0) && (y == 0.0) &&
                (Double.doubleToLongBits(y) == 0x8000000000000000L)) {
            ans = y;
        }
        return ans;
    }

    /**
     *        Returns the larger of its two arguments.
     *        @param        x        The first argument, an integer.
     *        @param        y        The second argument, an integer.
     *        @return        Returns the larger of x and y.
     */
    strictfp public static int max(int x, int y) {
        return ((x > y) ? x : y);
    }

    /**
     *        Returns the larger of its two arguments.
     *        @param        x        The first argument, a long.
     *        @param        y        The second argument, a long.
     *        @return        Returns the larger of x and y.
     */
    strictfp public static long max(long x, long y) {
        return ((x > y) ? x : y);
    }

    /**
     *        Returns the larger of its two arguments.
     *        @param        x        The first argument, a float.
     *        @param        y        The second argument, a float.
     *        @return        Returns the larger of x and y.
     *                        This function considers -0.0f to
     *                        be less than 0.0f.
     */
    strictfp public static float max(float x, float y) {
        if (Float.isNaN(x)) {
            return x;
        }
        float ans = ((x >= y) ? x : y);
        if ((ans == 0.0f) && (Float.floatToIntBits(x) == 0x80000000)) {
            ans = y;
        }
        return ans;
    }

    /**
     *        Returns the larger of its two arguments.
     *        @param        x        The first argument, a double.
     *        @param        y        The second argument, a double.
     *        @return        Returns the larger of x and y.
     *                        This function considers -0.0 to
     *                        be less than 0.0.
     */
    strictfp public static double max(double x, double y) {
        if (Double.isNaN(x)) {
            return x;
        }
        double ans = ((x >= y) ? x : y);
        if ((x == 0.0) && (y == 0.0) &&
                (Double.doubleToLongBits(x) == 0x8000000000000000L)) {
            ans = y;
        }
        return ans;
    }

    /**
     *        Returns the integer closest to the arguments.
     *        @param        x        The argument, a float.
     *        @return        Returns the integer closest to x.
     */
    strictfp public static int round(float x) {
        return (int) floor(x + 0.5f);
    }

    /**
     *        Returns the long closest to the arguments.
     *        @param        x        The argument, a double.
     *        @return        Returns the long closest to x.
     */
    strictfp public static long round(double x) {
        return (long) floor(x + 0.5);
    }

    /**
     *        Returns the random number.
     *        @return        Returns a random number from a uniform distribution.
     */
    synchronized strictfp public static double random() {
        if (random == null) {
            random = new Random();
        }
        return random.nextDouble();
    }

    /*
     *        This following code is derived from fdlibm, which contained
     *        the following notice.
     * ====================================================
     * Copyright (C) 1993 by Sun Microsystems, Inc. All rights reserved.
     *
     * Developed at SunSoft, a Sun Microsystems, Inc. business.
     * Permission to use, copy, modify, and distribute this
     * software is freely granted, provided that this notice
     * is preserved.
     * ====================================================
     *
     * Constants:
     * The hexadecimal values are the intended ones for the following
     * constants. The decimal values may be used, provided that the
     * compiler will convert from decimal to binary accurately enough
     * to produce the hexadecimal values shown.
     */
    static private final double huge = 1.0e+300;
    static private final double tiny = 1.0e-300;

    /**
     *        Returns the value of its argument rounded toward
     *        positive infinity to an integral value.
     *        @param        x        The argument, a double.
     *        @return        Returns the smallest double, not less than x,
     *                        that is an integral value.
     */
    static public double ceil(double x) {
        int exp;
        int sign;
        long ix;

        if (x == 0) {
            return x;
        }

        ix = Double.doubleToLongBits(x);
        sign = (int) ((ix >> 63) & 1);
        exp = ((int) (ix >> 52) & 0x7ff) - 0x3ff;

        if (exp < 0) {
            if (x < 0.0) {
                return NEGATIVE_ZERO;
            } else if (x == 0.0) {
                return x;
            } else {
                return 1.0;
            }
        } else if (exp < 53) {
            long mask = (0x000fffffffffffffL >>> exp);
            if ((mask & ix) == 0) {
                return x; // x is integral
            }
            if (x > 0.0) {
                ix += (0x0010000000000000L >> exp);
            }
            ix = ix & (~mask);
        } else if (exp == 1024) { // infinity
            return x;
        }

        return Double.longBitsToDouble(ix);
    }

    /**
     *        Returns the value of its argument rounded toward
     *        negative infinity to an integral value.
     *        @param        x        The argument, a double.
     *        @return        Returns the smallest double, not greater than x,
     *                        that is an integral value.
     */
    static public double floor(double x) {
        int exp;
        int sign;
        long ix;

        if (x == 0) {
            return x;
        }

        ix = Double.doubleToLongBits(x);
        sign = (int) ((ix >> 63) & 1);
        exp = ((int) (ix >> 52) & 0x7ff) - 0x3ff;

        if (exp < 0) {
            if (x < 0.0) {
                return -1.0;
            } else if (x == 0.0) {
                return x;
            } else {
                return 0.0;
            }
        } else if (exp < 53) {
            long mask = (0x000fffffffffffffL >>> exp);
            if ((mask & ix) == 0) {
                return x; // x is integral
            }
            if (x < 0.0) {
                ix += (0x0010000000000000L >> exp);
            }
            ix = ix & (~mask);
        } else if (exp == 1024) { // infinity
            return x;
        }

        return Double.longBitsToDouble(ix);
    }

    static private final double[] TWO52 = {
            0x1.0p52, /*  4.50359962737049600000e+15 */
            -0x1.0p52 /* -4.50359962737049600000e+15 */
        };
    static private final double NEGATIVE_ZERO = -0x0.0p0;

    /**
     *        Returns the value of its argument rounded toward
     *        the closest integral value.
     *        @param        x        The argument, a double.
     *        @return        Returns the double closest to x
     *                        that is an integral value.
     */
    static public double rint(double x) {
        int exp;
        int sign;
        long ix;
        double w;

        if (x == 0) {
            return x;
        }

        ix = Double.doubleToLongBits(x);
        sign = (int) ((ix >> 63) & 1);
        exp = ((int) (ix >> 52) & 0x7ff) - 0x3ff;

        if (exp < 0) {
            if (x < -0.5) {
                return -1.0;
            } else if (x > 0.5) {
                return 1.0;
            } else if (sign == 0) {
                return 0.0;
            } else {
                return NEGATIVE_ZERO;
            }
        } else if (exp < 53) {
            long mask = (0x000fffffffffffffL >>> exp);
            if ((mask & ix) == 0) {
                return x; // x is integral
            }
        } else if (exp == 1024) { // infinity
            return x;
        }

        x = Double.longBitsToDouble(ix);
        w = TWO52[sign] + x;
        return w - TWO52[sign];
    }

    /**
     *        Returns  x REM p =  x - [x/p]*p as if in infinite
     *         precise arithmetic, where [x/p] is the (infinite bit)
     *        integer nearest x/p (in half way case choose the even one).
     *        @param        x        The dividend.
     *        @param        y        The divisor.
     *        @return        The remainder computed according to the IEEE 754 standard.
     */
    static public double IEEEremainder(double x, double p) {
        int hx;
        int hp;
        int sx; // unsigned
        int lx; // unsigned
        int lp; // unsigned
        double p_half;

        hx = __HI(x); /* high word of x */
        lx = __LO(x); /* low  word of x */
        hp = __HI(p); /* high word of p */
        lp = __LO(p); /* low  word of p */
        sx = hx & 0x80000000;
        hp &= 0x7fffffff;
        hx &= 0x7fffffff;

        /* purge off exception values */
        if ((hp | lp) == 0) {
            return (x * p) / (x * p); /* p = 0 */
        }

        if ((hx >= 0x7ff00000) || /* x not finite */
                ((hp >= 0x7ff00000) && /* p is NaN */
                (((hp - 0x7ff00000) | lp) != 0))) {
            return (x * p) / (x * p);
        }

        if (hp <= 0x7fdfffff) {
            x = x % (p + p); /* now x < 2p */
        }

        if (((hx - hp) | (lx - lp)) == 0) {
            return zero * x;
        }

        x = abs(x);
        p = abs(p);
        if (hp < 0x00200000) {
            if ((x + x) > p) {
                x -= p;
                if ((x + x) >= p) {
                    x -= p;
                }
            }
        } else {
            p_half = 0.5 * p;
            if (x > p_half) {
                x -= p;
                if (x >= p_half) {
                    x -= p;
                }
            }
        }
        lx = __HI(x);
        lx ^= sx;
        return setHI(x, lx);
    }

    /* sqrt(x)
     * Return correctly rounded sqrt.
     *   ------------------------------------------
     *   |  Use the hardware sqrt if you have one |
     *   ------------------------------------------
     * Method:
     *   Bit by bit method using integer arithmetic. (Slow, but portable)
     *   1. Normalization
     *  Scale x to y in [1,4) with even powers of 2:
     *  find an integer k such that  1 <= (y=x*2^(2k)) < 4, then
     *      sqrt(x) = 2^k * sqrt(y)
     *   2. Bit by bit computation
     *  Let q  = sqrt(y) truncated to i bit after binary point (q = 1),
     *       i                                                   0
     *                             i+1         2
     *      s  = 2*q , and  y  =  2   * ( y - q  ).     (1)
     *       i      i            i                 i
     *
     *  To compute q    from q , one checks whether
     *              i+1       i
     *
     *                -(i+1) 2
     *          (q + 2      ) <= y.         (2)
     *                i
     *                         -(i+1)
     *  If (2) is false, then q   = q ; otherwise q   = q  + 2      .
     *                         i+1   i             i+1   i
     *
     *  With some algebric manipulation, it is not difficult to see
     *  that (2) is equivalent to
     *                             -(i+1)
     *          s  +  2       <= y          (3)
     *           i                i
     *
     *  The advantage of (3) is that s  and y  can be computed by
     *                    i      i
     *  the following recurrence formula:
     *      if (3) is false
     *
     *      s     =  s  ,   y    = y   ;            (4)
     *       i+1      i      i+1    i
     *
     *      otherwise,
     *                     -i                      -(i+1)
     *      s     =  s  + 2  ,  y    = y  -  s  - 2         (5)
     *       i+1      i          i+1    i     i
     *
     *  One may easily use induction to prove (4) and (5).
     *  Note. Since the left hand side of (3) contain only i+2 bits,
     *        it does not necessary to do a full (53-bit) comparison
     *        in (3).
     *   3. Final rounding
     *  After generating the 53 bits result, we compute one more bit.
     *  Together with the remainder, we can decide whether the
     *  result is exact, bigger than 1/2ulp, or less than 1/2ulp
     *  (it will never equal to 1/2ulp).
     *  The rounding mode can be detected by checking whether
     *  huge + tiny is equal to huge, and whether huge - tiny is
     *  equal to huge for some floating point number "huge" and "tiny".
     *
     *
     * Special cases:
     *  sqrt(+-0) = +-0     ... exact
     *  sqrt(inf) = inf
     *  sqrt(-ve) = NaN     ... with invalid signal
     *  sqrt(NaN) = NaN     ... with invalid signal for signaling NaN
     */

    /**
     *        Returns the square root of its argument.
     *        @param        x        The argument, a double.
     *        @return        Returns the square root of x.
     */
    static public double sqrt(double x) {
        long ix = Double.doubleToLongBits(x);

        /* take care of Inf and NaN */
        if ((ix & 0x7ff0000000000000L) == 0x7ff0000000000000L) {

            /* sqrt(NaN)=NaN, sqrt(+inf)=+inf  sqrt(-inf)=sNaN */
            return (x * x) + x;
        }

        /* take care of zero */
        if (x < 0.0) {
            return Double.NaN;
        } else if (x == 0.0) {
            return x; /* sqrt(+-0) = +-0 */
        }

        /* normalize x */
        long m = (ix >> 52);
        ix &= 0x000fffffffffffffL;

        /* add implicit bit, if not sub-normal */
        if (m != 0) {
            ix |= 0x0010000000000000L;
        }

        m -= 1023L; /* unbias exponent */
        if ((m & 1) != 0) { /* odd m, double x to make it even */
            ix += ix;
        }
        m >>= 1; /* m = [m/2] */
        m += 1023L;

        /* generate sqrt(x) bit by bit */
        ix += ix;
        long q = 0L; /* q = sqrt(x) */
        long s = 0L;
        long r = 0x0020000000000000L; /* r = moving bit from right to left */

        while (r != 0) {
            long t = s + r;
            if (t <= ix) {
                s = t + r;
                ix -= t;
                q += r;
            }
            ix += ix;
            r >>= 1;
        }

        /* round */
        if (ix != 0) {
            q += (q & 1L);
        }

        /* assemble result */
        ix = (m << 52) | (0x000fffffffffffffL & (q >> 1));
        return Double.longBitsToDouble(ix);
    }

    static private final double[] halF = { 0.5, -0.5 };
    static private final double twom1000 = 0x1.0p-1000; /* 2**-1000=9.33263618503218878990e-302 */
    static private final double o_threshold = 0x1.62e42fefa39efp9; /* 7.09782712893383973096e+02 */
    static private final double u_threshold = -0x1.74910d52d3051p9; /* -7.45133219101941108420e+02 */
    static private final double[] ln2HI = {
            0x1.62e42feep-1, /*  6.93147180369123816490e-01 */
            -0x1.62e42feep-1
        }; /* -6.93147180369123816490e-01 */
    static private final double[] ln2LO = {
            0x1.a39ef35793c76p-33, /*  1.90821492927058770002e-10 */
            -0x1.a39ef35793c76p-33
        }; /* -1.90821492927058770002e-10 */
    static private final double invln2 = 0x1.71547652b82fep0; /* 1.44269504088896338700e+00 */
    static private final double P1 = 0x1.555555555553ep-3; /*  1.66666666666666019037e-01 */
    static private final double P2 = -0x1.6c16c16bebd93p-9; /* -2.77777777770155933842e-03 */
    static private final double P3 = 0x1.1566aaf25de2cp-14; /*  6.61375632143793436117e-05 */
    static private final double P4 = -0x1.bbd41c5d26bf1p-20; /* -1.65339022054652515390e-06 */
    static private final double P5 = 0x1.6376972bea4dp-25; /*  4.13813679705723846039e-08 */

    /* exp(x)
     * Returns the exponential of x.
     *
     * Method
     *   1. Argument reduction:
     *      Reduce x to an r so that |r| <= 0.5*ln2 ~ 0.34658.
     *        Given x, find r and integer k such that
     *
     *               x = k*ln2 + r,  |r| <= 0.5*ln2.
     *
     *      Here r will be represented as r = hi-lo for better
     *         accuracy.
     *
     *   2. Approximation of exp(r) by a special rational function on
     *        the interval [0,0.34658]:
     *        Write
     *            R(r**2) = r*(exp(r)+1)/(exp(r)-1) = 2 + r*r/6 - r**4/360 + ...
     *      We use a special Reme algorithm on [0,0.34658] to generate
     *         a polynomial of degree 5 to approximate R. The maximum error
     *          of this polynomial approximation is bounded by 2**-59. In
     *        other words,
     *              R(z) ~ 2.0 + P1*z + P2*z**2 + P3*z**3 + P4*z**4 + P5*z**5
     *         (where z=r*r, and the values of P1 to P5 are listed below)
     *        and
     *            |                  5          |     -59
     *             | 2.0+P1*z+...+P5*z   -  R(z) | <= 2
     *            |                             |
     *        The computation of exp(r) thus becomes
     *                             2*r
     *                exp(r) = 1 + -------
     *                              R - r
     *                          r*R1(r)
     *                       = 1 + r + ----------- (for better accuracy)
     *                                  2 - R1(r)
     *         where
     *                                   2       4             10
     *                R1(r) = r - (P1*r  + P2*r  + ... + P5*r   ).
     *
     *   3. Scale back to obtain exp(x):
     *        From step 1, we have
     *           exp(x) = 2^k * exp(r)
     *
     * Special cases:
     *        exp(INF) is INF, exp(NaN) is NaN;
     *        exp(-INF) is 0, and
     *        for finite argument, only exp(0)=1 is exact.
     *
     * Accuracy:
     *        according to an error analysis, the error is always less than
     *        1 ulp (unit in the last place).
     *
     * Misc. info.
     *        For IEEE double
     *            if x >  7.09782712893383973096e+02 then exp(x) overflow
     *            if x < -7.45133219101941108420e+02 then exp(x) underflow
     */

    /**
     *        Returns the exponential of its argument.
     *        @param        x        The argument, a double.
     *        @return        Returns e to the power x.
     */
    static public double exp(double x) {
        double y;
        double hi = 0;
        double lo = 0;
        double c;
        double t;
        int k = 0;
        int xsb;
        int hx;

        hx = __HI(x); /* high word of x */
        xsb = (hx >>> 31) & 1; /* sign bit of x */
        hx &= 0x7fffffff; /* high word of |x| */

        /* filter out non-finite argument */
        if (hx >= 0x40862E42) { /* if |x|>=709.78... */
            if (hx >= 0x7ff00000) {
                if (((hx & 0xfffff) | __LO(x)) != 0) {
                    return x + x; /* NaN */
                } else {
                    return ((xsb == 0) ? x : 0.0); /* exp(+-inf)={inf,0} */
                }
            }
            if (x > o_threshold) {
                return huge * huge; /* overflow */
            }
            if (x < u_threshold) {
                return twom1000 * twom1000; /* underflow */
            }
        }

        /* argument reduction */
        if (hx > 0x3fd62e42) { /* if  |x| > 0.5 ln2 */
            if (hx < 0x3FF0A2B2) { /* and |x| < 1.5 ln2 */
                hi = x - ln2HI[xsb];
                lo = ln2LO[xsb];
                k = 1 - xsb - xsb;
            } else {
                k = (int) ((invln2 * x) + halF[xsb]);
                t = k;
                hi = x - (t * ln2HI[0]); /* t*ln2HI is exact here */
                lo = t * ln2LO[0];
            }
            x = hi - lo;
        } else if (hx < 0x3e300000) { /* when |x|<2**-28 */
            if ((huge + x) > one) {
                return one + x; /* trigger inexact */
            }
        } else {
            k = 0;
        }

        /* x is now in primary range */
        t = x * x;
        c = x - (t * (P1 + (t * (P2 + (t * (P3 + (t * (P4 + (t * P5)))))))));
        if (k == 0) {
            return one - (((x * c) / (c - 2.0)) - x);
        } else {
            y = one - ((lo - ((x * c) / (2.0 - c))) - hi);
        }

        long iy = Double.doubleToLongBits(y);
        if (k >= -1021) {
            iy += ((long) k << 52);
        } else {
            iy += ((k + 1000L) << 52);
        }
        return Double.longBitsToDouble(iy);
    }

    static private final double ln2_hi = 0x1.62e42feep-1; /* 6.93147180369123816490e-01 */
    static private final double ln2_lo = 0x1.a39ef35793c76p-33; /* 1.90821492927058770002e-10 */
    static private final double Lg1 = 0x1.5555555555593p-1; /* 6.666666666666735130e-01 */
    static private final double Lg2 = 0x1.999999997fa04p-2; /* 3.999999999940941908e-01 */
    static private final double Lg3 = 0x1.2492494229359p-2; /* 2.857142874366239149e-01 */
    static private final double Lg4 = 0x1.c71c51d8e78afp-3; /* 2.222219843214978396e-01 */
    static private final double Lg5 = 0x1.7466496cb03dep-3; /* 1.818357216161805012e-01 */
    static private final double Lg6 = 0x1.39a09d078c69fp-3; /* 1.531383769920937332e-01 */
    static private final double Lg7 = 0x1.2f112df3e5244p-3; /* 1.479819860511658591e-01 */

    /*
     * Return the logrithm of x
     *
     * Method :
     *   1. Argument Reduction: find k and f such that
     *                        x = 2^k * (1+f),
     *           where  sqrt(2)/2 < 1+f < sqrt(2) .
     *
     *   2. Approximation of log(1+f).
     *        Let s = f/(2+f) ; based on log(1+f) = log(1+s) - log(1-s)
     *                 = 2s + 2/3 s**3 + 2/5 s**5 + .....,
     *                      = 2s + s*R
     *      We use a special Reme algorithm on [0,0.1716] to generate
     *         a polynomial of degree 14 to approximate R The maximum error
     *        of this polynomial approximation is bounded by 2**-58.45. In
     *        other words,
     *                        2      4      6      8      10      12      14
     *            R(z) ~ Lg1*s +Lg2*s +Lg3*s +Lg4*s +Lg5*s  +Lg6*s  +Lg7*s
     *          (the values of Lg1 to Lg7 are listed in the program)
     *        and
     *            |      2          14          |     -58.45
     *            | Lg1*s +...+Lg7*s    -  R(z) | <= 2
     *            |                             |
     *        Note that 2s = f - s*f = f - hfsq + s*hfsq, where hfsq = f*f/2.
     *        In order to guarantee error in log below 1ulp, we compute log
     *        by
     *                log(1+f) = f - s*(f - R)        (if f is not too large)
     *                log(1+f) = f - (hfsq - s*(hfsq+R)).        (better accuracy)
     *
     *        3. Finally,  log(x) = k*ln2 + log(1+f).
     *                            = k*ln2_hi+(f-(hfsq-(s*(hfsq+R)+k*ln2_lo)))
     *           Here ln2 is split into two floating point number:
     *                        ln2_hi + ln2_lo,
     *           where n*ln2_hi is always exact for |n| < 2000.
     *
     * Special cases:
     *        log(x) is NaN with signal if x < 0 (including -INF) ;
     *        log(+INF) is +INF; log(0) is -INF with signal;
     *        log(NaN) is that NaN with no signal.
     *
     * Accuracy:
     *        according to an error analysis, the error is always less than
     *        1 ulp (unit in the last place).
     */

    /**
     *        Returns the natural logarithm of its argument.
     *        @param        x        The argument, a double.
     *        @return        Returns the natural (base e) logarithm of x.
     */
    static public double log(double x) {
        double hfsq;
        double f;
        double s;
        double z;
        double R;
        double w;
        double t1;
        double t2;
        double dk;
        int k;
        int hx;
        int i;
        int j;
        int lx;

        hx = __HI(x); /* high word of x */
        lx = __LO(x); /* low  word of x */

        k = 0;
        if (hx < 0x00100000) { /* x < 2**-1022  */
            if (((hx & 0x7fffffff) | lx) == 0) {
                return -two54 / zero; /* log(+-0)=-inf */
            }
            if (hx < 0) {
                return (x - x) / zero; /* log(-#) = NaN */
            }
            k -= 54;
            x *= two54; /* subnormal number, scale up x */
            hx = __HI(x); /* high word of x */
        }
        if (hx >= 0x7ff00000) {
            return x + x;
        }
        k += ((hx >> 20) - 1023);
        hx &= 0x000fffff;
        i = (hx + 0x95f64) & 0x100000;
        x = setHI(x, hx | (i ^ 0x3ff00000)); /* normalize x or x/2 */
        k += (i >> 20);
        f = x - 1.0;
        if ((0x000fffff & (2 + hx)) < 3) { /* |f| < 2**-20 */
            if (f == zero) {
                if (k == 0) {
                    return zero;
                } else {
                    dk = (double) k;
                }
                return (dk * ln2_hi) + (dk * ln2_lo);
            }
            R = f * f * (0.5 - (0.33333333333333333 * f));
            if (k == 0) {
                return f - R;
            } else {
                dk = (double) k;
                return (dk * ln2_hi) - ((R - (dk * ln2_lo)) - f);
            }
        }
        s = f / (2.0 + f);
        dk = (double) k;
        z = s * s;
        i = hx - 0x6147a;
        w = z * z;
        j = 0x6b851 - hx;
        t1 = w * (Lg2 + (w * (Lg4 + (w * Lg6))));
        t2 = z * (Lg1 + (w * (Lg3 + (w * (Lg5 + (w * Lg7))))));
        i |= j;
        R = t2 + t1;
        if (i > 0) {
            hfsq = 0.5 * f * f;
            if (k == 0) {
                return f - (hfsq - (s * (hfsq + R)));
            } else {
                return (dk * ln2_hi) -
                ((hfsq - ((s * (hfsq + R)) + (dk * ln2_lo))) - f);
            }
        } else {
            if (k == 0) {
                return f - (s * (f - R));
            } else {
                return (dk * ln2_hi) - (((s * (f - R)) - (dk * ln2_lo)) - f);
            }
        }
    }

    /**
     *        Returns the sine of its argument.
     *        @param        x        The argument, a double, assumed to be in radians.
     *        @return        Returns the sine of x.
     */
    static public double sin(double x) {
        double z = 0.0;
        int n;
        int ix = __HI(x);

        ix &= 0x7fffffff; /* |x| ~< pi/4 */

        if (ix <= 0x3fe921fb) {
            return __kernel_sin(x, z, 0);
        } else if (ix >= 0x7ff00000) {

            /* sin(Inf or NaN) is NaN */
            return x - x;
        } else {

            /* argument reduction needed */
            double[] y = new double[2];
            n = __ieee754_rem_pio2(x, y);
            switch (n & 3) {
            case 0:
                return __kernel_sin(y[0], y[1], 1);
            case 1:
                return __kernel_cos(y[0], y[1]);
            case 2:
                return -__kernel_sin(y[0], y[1], 1);
            default:
                return -__kernel_cos(y[0], y[1]);
            }
        }
    }

    static private final double S1 = -1.66666666666666324348e-01; /* 0xBFC55555, 0x55555549 */
    static private final double S2 = 8.33333333332248946124e-03; /* 0x3F811111, 0x1110F8A6 */
    static private final double S3 = -1.98412698298579493134e-04; /* 0xBF2A01A0, 0x19C161D5 */
    static private final double S4 = 2.75573137070700676789e-06; /* 0x3EC71DE3, 0x57B1FE7D */
    static private final double S5 = -2.50507602534068634195e-08; /* 0xBE5AE5E6, 0x8A2B9CEB */
    static private final double S6 = 1.58969099521155010221e-10; /* 0x3DE5D93A, 0x5ACFD57C */

    /*
     * kernel sin function on [-pi/4, pi/4], pi/4 ~ 0.7854
     * Input x is assumed to be bounded by ~pi/4 in magnitude.
     * Input y is the tail of x. * Input iy indicates whether y is 0. (if iy=0, y assume to be 0).
     *
     * Algorithm
     *        1. Since sin(-x) = -sin(x), we need only to consider positive x.
     *        2. if x < 2^-27 (hx<0x3e400000 0), return x with inexact if x!=0.
     *        3. sin(x) is approximated by a polynomial of degree 13 on
     *           [0,pi/4]
     *                                   3            13
     *                   sin(x) ~ x + S1*x + ... + S6*x
     *           where
     *
     *         |sin(x)         2     4     6     8     10     12  |     -58
     *         |----- - (1+S1*x +S2*x +S3*x +S4*x +S5*x  +S6*x   )| <= 2
     *         |  x                                                    |
     *
     *        4. sin(x+y) = sin(x) + sin'(x')*y
     *                    ~ sin(x) + (1-x*x/2)*y
     *           For better accuracy, let
     *                     3      2      2      2      2
     *                r = x *(S2+x *(S3+x *(S4+x *(S5+x *S6))))
     *           then                   3    2
     *                sin(x) = x + (S1*x + (x *(r-y/2)+y))
     */
    static double __kernel_sin(double x, double y, int iy) {
        double z;
        double r;
        double v;
        int ix;
        ix = __HI(x) & 0x7fffffff; /* high word of x */
        if (ix < 0x3e400000) { /* |x| < 2**-27 */
            if ((int) x == 0) {
                return x; /* generate inexact */
            }
        }
        z = x * x;
        v = z * x;
        r = S2 + (z * (S3 + (z * (S4 + (z * (S5 + (z * S6)))))));
        if (iy == 0) {
            return x + (v * (S1 + (z * r)));
        } else {
            return x - (((z * ((half * y) - (v * r))) - y) - (v * S1));
        }
    }

    /**
     *        Returns the cosine of its argument.
     *        @param        x        The argument, a double, assumed to be in radians.
     *        @return        Returns the cosine of x.
     */
    static public double cos(double x) {
        double z = 0.0;
        int n;
        int ix;

        /* High word of x. */
        ix = __HI(x);

        /* |x| ~< pi/4 */
        ix &= 0x7fffffff;
        if (ix <= 0x3fe921fb) {
            return __kernel_cos(x, z);

            /* cos(Inf or NaN) is NaN */
        } else if (ix >= 0x7ff00000) {
            return x - x;

            /* argument reduction needed */
        } else {
            double[] y = new double[2];
            n = __ieee754_rem_pio2(x, y);
            switch (n & 3) {
            case 0:
                return __kernel_cos(y[0], y[1]);
            case 1:
                return -__kernel_sin(y[0], y[1], 1);
            case 2:
                return -__kernel_cos(y[0], y[1]);
            default:
                return __kernel_sin(y[0], y[1], 1);
            }
        }
    }

    static private final double one = 0x1.0p0; /*  1.00000000000000000000e+00 */
    static private final double C1 = 0x1.555555555554cp-5; /*  4.16666666666666019037e-02 */
    static private final double C2 = -0x1.6c16c16c15177p-10; /* -1.38888888888741095749e-03 */
    static private final double C3 = 0x1.a01a019cb159p-16; /*  2.48015872894767294178e-05 */
    static private final double C4 = -0x1.27e4f809c52adp-22; /* -2.75573143513906633035e-07 */
    static private final double C5 = 0x1.1ee9ebdb4b1c4p-29; /*  2.08757232129817482790e-09 */
    static private final double C6 = -0x1.8fae9be8838d4p-37; /* -1.13596475577881948265e-11 */

    /*
     * kernel cos function on [-pi/4, pi/4], pi/4 ~ 0.785398164
     * Input x is assumed to be bounded by ~pi/4 in magnitude.
     * Input y is the tail of x.
     *
     * Algorithm
     *        1. Since cos(-x) = cos(x), we need only to consider positive x.
     *        2. if x < 2^-27 (hx<0x3e400000 0), return 1 with inexact if x!=0.
     *        3. cos(x) is approximated by a polynomial of degree 14 on
     *           [0,pi/4]
     *                               4            14
     *                   cos(x) ~ 1 - x*x/2 + C1*x + ... + C6*x
     *           where the remez error is
     *
     *         |              2     4     6     8     10    12     14 |     -58
     *         |cos(x)-(1-.5*x +C1*x +C2*x +C3*x +C4*x +C5*x  +C6*x  )| <= 2
     *         |                                                           |
     *
     *                        4     6     8     10    12     14
     *        4. let r = C1*x +C2*x +C3*x +C4*x +C5*x  +C6*x  , then
     *               cos(x) = 1 - x*x/2 + r
     *           since cos(x+y) ~ cos(x) - sin(x)*y
     *                          ~ cos(x) - x*y,
     *           a correction term is necessary in cos(x) and hence
     *                cos(x+y) = 1 - (x*x/2 - (r - x*y))
     *           For better accuracy when x > 0.3, let qx = |x|/4 with
     *           the last 32 bits mask off, and if x > 0.78125, let qx = 0.28125.
     *           Then
     *                cos(x+y) = (1-qx) - ((x*x/2-qx) - (r-x*y)).
     *           Note that 1-qx and (x*x/2-qx) is EXACT here, and the
     *           magnitude of the latter is at least a quarter of x*x/2,
     *           thus, reducing the rounding error in the subtraction.
     */
    static private double __kernel_cos(double x, double y) {
        double a;
        double hz;
        double z;
        double r;
        double qx = zero;
        int ix;
        ix = __HI(x) & 0x7fffffff; /* ix = |x|'s high word*/
        if (ix < 0x3e400000) {

            /* if x < 2**27 */
            if (((int) x) == 0) {
                return one; /* generate inexact */
            }
        }
        z = x * x;
        r = z * (C1 +
            (z * (C2 + (z * (C3 + (z * (C4 + (z * (C5 + (z * C6))))))))));
        if (ix < 0x3FD33333) {

            /* if |x| < 0.3 */
            return one - ((0.5 * z) - ((z * r) - (x * y)));
        } else {
            if (ix > 0x3fe90000) { /* x > 0.78125 */
                qx = 0.28125;
            } else {
                qx = set(ix - 0x00200000, 0); /* x/4 */
            }
            hz = (0.5 * z) - qx;
            a = one - qx;
            return a - (hz - ((z * r) - (x * y)));
        }
    }

    /**
     *        Returns the tangent of its argument.
     *        @param        x        The argument, a double, assumed to be in radians.
     *        @return        Returns the tangent of x.
     */
    static public double tan(double x) {
        double z = zero;

        int n;
        int ix = __HI(x);

        /* |x| ~< pi/4 */
        ix &= 0x7fffffff;
        if (ix <= 0x3fe921fb) {
            return __kernel_tan(x, z, 1);
        } else if (ix >= 0x7ff00000) {

            /* tan(Inf or NaN) is NaN */
            return x - x; /* NaN */
        } else {

            /* argument reduction needed */
            double[] y = new double[2];
            n = __ieee754_rem_pio2(x, y);

            /*   1 -- n even -1 -- n odd */
            return __kernel_tan(y[0], y[1], 1 - ((n & 1) << 1));
        }
    }

    static private final double pio4 = 0x1.921fb54442d18p-1; /* 7.85398163397448278999e-01 */
    static private final double pio4lo = 0x1.1a62633145c07p-55; /* 3.06161699786838301793e-17 */
    static private final double[] T = {
            0x1.5555555555563p-2, /* 3.33333333333334091986e-01 */
            0x1.111111110fe7ap-3, /* 1.33333333333201242699e-01 */
            0x1.ba1ba1bb341fep-5, /* 5.39682539762260521377e-02 */
            0x1.664f48406d637p-6, /* 2.18694882948595424599e-02 */
            0x1.226e3e96e8493p-7, /* 8.86323982359930005737e-03 */
            0x1.d6d22c9560328p-9, /* 3.59207910759131235356e-03 */
            0x1.7dbc8fee08315p-10, /* 1.45620945432529025516e-03 */
            0x1.344d8f2f26501p-11, /* 5.88041240820264096874e-04 */
            0x1.026f71a8d1068p-12, /* 2.46463134818469906812e-04 */
            0x1.47e88a03792a6p-14, /* 7.81794442939557092300e-05 */
            0x1.2b80f32f0a7e9p-14, /* 7.14072491382608190305e-05 */
            -0x1.375cbdb605373p-16, /* -1.85586374855275456654e-05 */
            0x1.b2a7074bf7ad4p-16 /* 2.59073051863633712884e-05 */
        };

    /*
     *        __kernel_tan( x, y, k )
     * kernel tan function on [-pi/4, pi/4], pi/4 ~ 0.7854
     * Input x is assumed to be bounded by ~pi/4 in magnitude.
     * Input y is the tail of x. * Input k indicates whether tan (if k=1) or
     * -1/tan (if k= -1) is returned. * * Algorithm
     *        1. Since tan(-x) = -tan(x), we need only to consider positive x.
     *        2. if x < 2^-28 (hx<0x3e300000 0), return x with inexact if x!=0.
     *        3. tan(x) is approximated by a odd polynomial of degree 27 on
     *           [0,0.67434]
     *                       3             27
     *                   tan(x) ~ x + T1*x + ... + T13*x
     *           where
     *
     *                 |tan(x)         2     4            26   |     -59.2
     *                 |----- - (1+T1*x +T2*x +.... +T13*x    )| <= 2
     *          |  x                                    |
     *
     *           Note: tan(x+y) = tan(x) + tan'(x)*y
     *                          ~ tan(x) + (1+x*x)*y
     *           Therefore, for better accuracy in computing tan(x+y), let
     *                     3      2      2       2       2
     *                r = x *(T2+x *(T3+x *(...+x *(T12+x *T13))))
     *           then
     *                                     3    2
     *                tan(x+y) = x + (T1*x + (x *(r+y)+y)) *
     *   4. For x in [0.67434,pi/4],  let y = pi/4 - x, then
     *                tan(x) = tan(pi/4-y) = (1-tan(y))/(1+tan(y))
     *                       = 1 - 2*(tan(y) - (tan(y)^2)/(1+tan(y)))
     */
    static private double __kernel_tan(double x, double y, int iy) {
        double z;
        double r;
        double v;
        double w;
        double s;
        int ix;
        int hx;

        hx = __HI(x); /* high word of x */
        ix = hx & 0x7fffffff; /* high word of |x| */
        if (ix < 0x3e300000) { /* x < 2**-28 */
            if ((int) x == 0) { /* generate inexact */
                if (((ix | __LO(x)) | (iy + 1)) == 0) {
                    return one / abs(x);
                } else {
                    return (iy == 1) ? x : (-one / x);
                }
            }
        }
        if (ix >= 0x3FE59428) {

            /* |x|>=0.6744 */
            if (hx < 0) {
                x = -x;
                y = -y;
            }
            z = pio4 - x;
            w = pio4lo - y;
            x = z + w;
            y = 0.0;
        }
        z = x * x;
        w = z * z;

        /*
         *        Break x^5*(T[1]+x^2*T[2]+...) into
         *          x^5(T[1]+x^4*T[3]+...+x^20*T[11]) +
         *          x^5(x^2*(T[2]+x^4*T[4]+...+x^22*[T12]))
         */
        r = T[1] +
            (w * (T[3] +
            (w * (T[5] + (w * (T[7] + (w * (T[9] + (w * T[11])))))))));
        v = z * (T[2] +
            (w * (T[4] +
            (w * (T[6] + (w * (T[8] + (w * (T[10] + (w * T[12]))))))))));
        s = z * x;
        r = y + (z * ((s * (r + v)) + y));
        r += (T[0] * s);
        w = x + r;
        if (ix >= 0x3FE59428) {
            v = (double) iy;
            return (double) (1 - ((hx >> 30) & 2)) * (v -
            (2.0 * (x - (((w * w) / (w + v)) - r))));
        }
        if (iy == 1) {
            return w;
        } else {

            /* if allow error up to 2 ulp, simply return -1.0/(x+r) here */
            /*  compute -1.0/(x+r) accurately */
            double a;

            /* if allow error up to 2 ulp, simply return -1.0/(x+r) here */
            /*  compute -1.0/(x+r) accurately */
            double t;
            z = w;
            z = setLO(z, 0);
            v = r - (z - x);

            /* z+v = r+x */
            t = a = -1.0 / w;

            /* a = -1.0/w */
            t = setLO(t, 0);
            s = 1.0 + (t * z);
            return t + (a * (s + (t * v)));
        }
    }

    static private final double pio2_hi = 0x1.921fb54442d18p0; /* 1.57079632679489655800e+00 */
    static private final double pio2_lo = 0x1.1a62633145c07p-54; /* 6.12323399573676603587e-17 */
    static private final double pio4_hi = 0x1.921fb54442d18p-1; /* 7.85398163397448278999e-01 */

    /* coefficient for R(x^2) */
    static private final double pS0 = 0x1.5555555555555p-3; /*  1.66666666666666657415e-01 */
    static private final double pS1 = -0x1.4d61203eb6f7dp-2; /* -3.25565818622400915405e-01 */
    static private final double pS2 = 0x1.9c1550e884455p-3; /*  2.01212532134862925881e-01 */
    static private final double pS3 = -0x1.48228b5688f3bp-5; /* -4.00555345006794114027e-02 */
    static private final double pS4 = 0x1.9efe07501b288p-11; /*  7.91534994289814532176e-04 */
    static private final double pS5 = 0x1.23de10dfdf709p-15; /*  3.47933107596021167570e-05 */
    static private final double qS1 = -0x1.33a271c8a2d4bp1; /* -2.40339491173441421878e+00 */
    static private final double qS2 = 0x1.02ae59c598ac8p1; /*  2.02094576023350569471e+00 */
    static private final double qS3 = -0x1.6066c1b8d0159p-1; /* -6.88283971605453293030e-01 */
    static private final double qS4 = 0x1.3b8c5b12e9282p-4; /*  7.70381505559019352791e-02 */

    /*
     *        asin(x)
     * Method :
     *        Since  asin(x) = x + x^3/6 + x^5*3/40 + x^7*15/336 + ...
     *        we approximate asin(x) on [0,0.5] by
     *                asin(x) = x + x*x^2*R(x^2)
     *        where
     *                R(x^2) is a rational approximation of (asin(x)-x)/x^3
     *        and its remez error is bounded by
     *                |(asin(x)-x)/x^3 - R(x^2)| < 2^(-58.75)
     *
     *        For x in [0.5,1]
     *                asin(x) = pi/2-2*asin(sqrt((1-x)/2))
     *        Let y = (1-x), z = y/2, s := sqrt(z), and pio2_hi+pio2_lo=pi/2;
     *        then for x>0.98
     *                asin(x) = pi/2 - 2*(s+s*z*R(z))
     *                        = pio2_hi - (2*(s+s*z*R(z)) - pio2_lo)
     *        For x<=0.98, let pio4_hi = pio2_hi/2, then
     *                f = hi part of s;
     *                c = sqrt(z) - f = (z-f*f)/(s+f)         ...f+c=sqrt(z)
     *        and
     *                asin(x) = pi/2 - 2*(s+s*z*R(z))
     *                        = pio4_hi+(pio4-2s)-(2s*z*R(z)-pio2_lo)
     *                        = pio4_hi+(pio4-2f)-(2s*z*R(z)-(pio2_lo+2c))
     *
     * Special cases:
     *        if x is NaN, return x itself;
     *        if |x|>1, return NaN with invalid signal.
     *
     */

    /**
     *        Returns the inverse (arc) sine of its argument.
     *        @param        x        The argument, a double.
     *        @return        Returns the angle, in radians, whose sine is x.
     *                        It is in the range [-pi/2,pi/2].
     */
    static public double asin(double x) {
        double t = zero;
        double w;
        double p;
        double q;
        double c;
        double r;
        double s;
        int hx;
        int ix;
        hx = __HI(x);

        ix = hx & 0x7fffffff;
        if (ix >= 0x3ff00000) { /* |x|>= 1 */
            if (((ix - 0x3ff00000) | __LO(x)) == 0) {

                /* asin(1)=+-pi/2 with inexact */
                return (x * pio2_hi) + (x * pio2_lo);
            }
            return (x - x) / (x - x); /* asin(|x|>1) is NaN */
        } else if (ix < 0x3fe00000) { /* |x|<0.5 */
            if (ix < 0x3e400000) { /* if |x| < 2**-27 */
                if ((huge + x) > one) {
                    return x; /* return x with inexact if x!=0*/
                }
            } else {
                t = x * x;
            }
            p = t * (pS0 +
                (t * (pS1 +
                (t * (pS2 + (t * (pS3 + (t * (pS4 + (t * pS5))))))))));
            q = one + (t * (qS1 + (t * (qS2 + (t * (qS3 + (t * qS4)))))));
            w = p / q;
            return x + (x * w);
        }

        /* 1> |x|>= 0.5 */
        w = one - abs(x);
        t = w * 0.5;
        p = t * (pS0 +
            (t * (pS1 + (t * (pS2 + (t * (pS3 + (t * (pS4 + (t * pS5))))))))));
        q = one + (t * (qS1 + (t * (qS2 + (t * (qS3 + (t * qS4)))))));
        s = sqrt(t);
        if (ix >= 0x3FEF3333) { /* if |x| > 0.975 */
            w = p / q;
            t = pio2_hi - ((2.0 * (s + (s * w))) - pio2_lo);
        } else {
            w = s;
            w = setLO(w, 0);
            c = (t - (w * w)) / (s + w);
            r = p / q;
            p = (2.0 * s * r) - (pio2_lo - (2.0 * c));
            q = pio4_hi - (2.0 * w);
            t = pio4_hi - (p - q);
        }
        return ((hx > 0) ? t : (-t));
    }

    /*
     * Method :
     *        acos(x)  = pi/2 - asin(x)
     *        acos(-x) = pi/2 + asin(x)
     * For |x|<=0.5
     *        acos(x) = pi/2 - (x + x*x^2*R(x^2))        (see asin.c)
     * For x>0.5
     *         acos(x) = pi/2 - (pi/2 - 2asin(sqrt((1-x)/2)))
     *                = 2asin(sqrt((1-x)/2))
     *                = 2s + 2s*z*R(z)         ...z=(1-x)/2, s=sqrt(z)
     *                = 2f + (2c + 2s*z*R(z))
     *     where f=hi part of s, and c = (z-f*f)/(s+f) is the correction term
     *     for f so that f+c ~ sqrt(z).
     * For x<-0.5
     *        acos(x) = pi - 2asin(sqrt((1-|x|)/2))
     *          = pi - 0.5*(s+s*z*R(z)), where z=(1-|x|)/2,s=sqrt(z)
     *
     * Special cases:
     *        if x is NaN, return x itself;
     *        if |x|>1, return NaN with invalid signal.
     *
     * Function needed: sqrt
     */

    /**
     *        Returns the inverse (arc) cosine of its argument.
     *        @param        x        The argument, a double.
     *        @return        Returns the angle, in radians, whose cosine is x.
     *                        It is in the range [0,pi].
     */
    static public double acos(double x) {
        double z;
        double p;
        double q;
        double r;
        double w;
        double s;
        double c;
        double df;
        int hx;
        int ix;
        hx = __HI(x);
        ix = hx & 0x7fffffff;
        if (ix >= 0x3ff00000) { /* |x| >= 1 */
            if (((ix - 0x3ff00000) | __LO(x)) == 0) { /* |x|==1 */
                if (hx > 0) {
                    return 0.0; /* acos(1) = 0  */
                } else {
                    return PI + (2.0 * pio2_lo); /* acos(-1)= pi */
                }
            }
            return (x - x) / (x - x); /* acos(|x|>1) is NaN */
        }
        if (ix < 0x3fe00000) { /* |x| < 0.5 */
            if (ix <= 0x3c600000) {
                return pio2_hi + pio2_lo; /*if|x|<2**-57*/
            }
            z = x * x;
            p = z * (pS0 +
                (z * (pS1 +
                (z * (pS2 + (z * (pS3 + (z * (pS4 + (z * pS5))))))))));
            q = one + (z * (qS1 + (z * (qS2 + (z * (qS3 + (z * qS4)))))));
            r = p / q;
            return pio2_hi - (x - (pio2_lo - (x * r)));
        } else if (hx < 0) { /* x < -0.5 */
            z = (one + x) * 0.5;
            p = z * (pS0 +
                (z * (pS1 +
                (z * (pS2 + (z * (pS3 + (z * (pS4 + (z * pS5))))))))));
            q = one + (z * (qS1 + (z * (qS2 + (z * (qS3 + (z * qS4)))))));
            s = sqrt(z);
            r = p / q;
            w = (r * s) - pio2_lo;
            return PI - (2.0 * (s + w));
        } else { /* x > 0.5 */
            z = (one - x) * 0.5;
            s = sqrt(z);
            df = s;
            df = setLO(df, 0);
            c = (z - (df * df)) / (s + df);
            p = z * (pS0 +
                (z * (pS1 +
                (z * (pS2 + (z * (pS3 + (z * (pS4 + (z * pS5))))))))));
            q = one + (z * (qS1 + (z * (qS2 + (z * (qS3 + (z * qS4)))))));
            r = p / q;
            w = (r * s) + c;
            return 2.0 * (df + w);
        }
    }

    static private final double[] atanhi = {
            0x1.dac670561bb4fp-2, /* 4.63647609000806093515e-01 atan(0.5)hi  */
            0x1.921fb54442d18p-1, /* 7.85398163397448278999e-01 atan(1.0)hi  */
            0x1.f730bd281f69bp-1, /* 9.82793723247329054082e-01 atan(1.5)hi  */
            0x1.921fb54442d18p0   /* 1.57079632679489655800e+00 atan(inf)hi  */
        };
    static private final double[] atanlo = {
            0x1.a2b7f222f65e2p-56, /* 2.26987774529616870924e-17 atan(0.5)lo  */
            0x1.1a62633145c07p-55, /* 3.06161699786838301793e-17 atan(1.0)lo  */
            0x1.007887af0cbbdp-56, /* 1.39033110312309984516e-17 atan(1.5)lo  */
            0x1.1a62633145c07p-54  /* 6.12323399573676603587e-17 atan(inf)lo  */
        };
    static private final double[] aT = {
            0x1.555555555550dp-2,  /*  3.33333333333329318027e-01 */
            -0x1.999999998ebc4p-3, /* -1.99999999998764832476e-01 */
            0x1.24924920083ffp-3,  /*  1.42857142725034663711e-01 */
            -0x1.c71c6fe231671p-4, /* -1.11111104054623557880e-01 */
            0x1.745cdc54c206ep-4,  /*  9.09088713343650656196e-02 */
            -0x1.3b0f2af749a6dp-4, /* -7.69187620504482999495e-02 */
            0x1.10d66a0d03d51p-4,  /*  6.66107313738753120669e-02 */
            -0x1.dde2d52defd9ap-5, /* -5.83357013379057348645e-02 */
            0x1.97b4b24760debp-5,  /*  4.97687799461593236017e-02 */
            -0x1.2b4442c6a6c2fp-5, /* -3.65315727442169155270e-02 */
            0x1.0ad3ae322da11p-6   /*  1.62858201153657823623e-02 */
        };

    /*
     * Method
     *   1. Reduce x to positive by atan(x) = -atan(-x).
     *   2. According to the integer k=4t+0.25 chopped, t=x, the argument
     *      is further reduced to one of the following intervals and the
     *      arctangent of t is evaluated by the corresponding formula:
     *
     *      [0,7/16]      atan(x) = t-t^3*(a1+t^2*(a2+...(a10+t^2*a11)...)
     *      [7/16,11/16]  atan(x) = atan(1/2) + atan( (t-0.5)/(1+t/2) )
     *      [11/16.19/16] atan(x) = atan( 1 ) + atan( (t-1)/(1+t) )
     *      [19/16,39/16] atan(x) = atan(3/2) + atan( (t-1.5)/(1+1.5t) )
     *      [39/16,INF]   atan(x) = atan(INF) + atan( -1/t )
     *
     * Constants:
     * The hexadecimal values are the intended ones for the following
     * constants. The decimal values may be used, provided that the
     * compiler will convert from decimal to binary accurately enough
     * to produce the hexadecimal values shown.
     */

    /**
     *        Returns the inverse (arc) tangent of its argument.
     *        @param        x        The argument, a double.
     *        @return        Returns the angle, in radians, whose tangent is x.
     *                        It is in the range [-pi/2,pi/2].
     */
    static public double atan(double x) {
        double w;
        double s1;
        double s2;
        double z;
        int ix;
        int hx;
        int id;

        hx = __HI(x);
        ix = hx & 0x7fffffff;
        if (ix >= 0x44100000) { /* if |x| >= 2^66 */
            if ((ix > 0x7ff00000) || ((ix == 0x7ff00000) && (__LO(x) != 0))) {
                return x + x; /* NaN */
            }
            if (hx > 0) {
                return atanhi[3] + atanlo[3];
            } else {
                return -atanhi[3] - atanlo[3];
            }
        }
        if (ix < 0x3fdc0000) { /* |x| < 0.4375 */
            if (ix < 0x3e200000) { /* |x| < 2^-29 */
                if ((huge + x) > one) {
                    return x; /* raise inexact */
                }
            }
            id = -1;
        } else {
            x = abs(x);
            if (ix < 0x3ff30000) { /* |x| < 1.1875 */
                if (ix < 0x3fe60000) { /* 7/16 <=|x|<11/16 */
                    id = 0;
                    x = ((2.0 * x) - one) / (2.0 + x);
                } else { /* 11/16<=|x|< 19/16 */
                    id = 1;
                    x = (x - one) / (x + one);
                }
            } else {
                if (ix < 0x40038000) { /* |x| < 2.4375 */
                    id = 2;
                    x = (x - 1.5) / (one + (1.5 * x));
                } else { /* 2.4375 <= |x| < 2^66 */
                    id = 3;
                    x = -1.0 / x;
                }
            }
        }

        /* end of argument reduction */
        z = x * x;
        w = z * z;

        /* break sum from i=0 to 10 aT[i]z**(i+1) into odd and even poly */
        s1 = z * (aT[0] +
            (w * (aT[2] +
            (w * (aT[4] + (w * (aT[6] + (w * (aT[8] + (w * aT[10]))))))))));
        s2 = w * (aT[1] +
            (w * (aT[3] + (w * (aT[5] + (w * (aT[7] + (w * aT[9]))))))));
        if (id < 0) {
            return x - (x * (s1 + s2));
        } else {
            z = atanhi[id] - (((x * (s1 + s2)) - atanlo[id]) - x);
            return (hx < 0) ? (-z) : z;
        }
    }

    static private final double pi_o_4 = 0x1.921fb54442d18p-1; /* 7.8539816339744827900e-01 */
    static private final double pi_o_2 = 0x1.921fb54442d18p0; /* 1.5707963267948965580e+00 */
    static private final double pi_lo = 0x1.1a62633145c07p-53; /* 1.2246467991473531772e-16 */

    /*
     * Method :
     *        1. Reduce y to positive by atan2(y,x)=-atan2(-y,x).
     *        2. Reduce x to positive by (if x and y are unexceptional):
     *                ARG (x+iy) = arctan(y/x)              ... if x > 0,
     *                ARG (x+iy) = pi - arctan[y/(-x)]   ... if x < 0,
     *
     * Special cases:
     *
     *        ATAN2((anything), NaN ) is NaN;
     *        ATAN2(NAN , (anything) ) is NaN;
     *        ATAN2(+-0, +(anything but NaN)) is +-0  ;
     *        ATAN2(+-0, -(anything but NaN)) is +-pi ;
     *        ATAN2(+-(anything but 0 and NaN), 0) is +-pi/2;
     *        ATAN2(+-(anything but INF and NaN), +INF) is +-0 ;
     *        ATAN2(+-(anything but INF and NaN), -INF) is +-pi;
     *        ATAN2(+-INF,+INF ) is +-pi/4 ;
     *        ATAN2(+-INF,-INF ) is +-3pi/4;
     *        ATAN2(+-INF, (anything but,0,NaN, and INF)) is +-pi/2;
     *
     * Constants:
     * The hexadecimal values are the intended ones for the following
     * constants. The decimal values may be used, provided that the
     * compiler will convert from decimal to binary accurately enough
     * to produce the hexadecimal values shown.
     */

    /**
     *        Returns angle corresponding to a Cartesian point.
     *        @param        x        The first argument, a double.
     *        @param        y        The second argument, a double.
     *        @return        Returns the angle, in radians, the the line
     *                        from (0,0) to (x,y) makes with the x-axis.
     *                        It is in the range [-pi,pi].
     */
    static public double atan2(double y, double x) {
        double z;
        int k;
        int m;
        int hx;
        int hy;
        int ix;
        int iy;
        int lx;
        int ly;

        hx = __HI(x);
        ix = hx & 0x7fffffff;
        lx = __LO(x);
        hy = __HI(y);
        iy = hy & 0x7fffffff;
        ly = __LO(y);
        if (((ix | ((lx | -lx) >>> 31)) > 0x7ff00000) ||
                ((iy | ((ly | -ly) >>> 31)) > 0x7ff00000)) { /* x or y is NaN */
            return x + y;
        }
        if (((hx - 0x3ff00000) | lx) == 0) {
            return atan(y); /* x=1.0 */
        }
        m = ((hy >> 31) & 1) | ((hx >> 30) & 2); /* 2*sign(x)+sign(y) */

        /* when y = 0 */
        if ((iy | ly) == 0) {
            switch (m) {
            case 0:
            case 1:
                return y; /* atan(+-0,+anything)=+-0 */
            case 2:
                return PI + tiny; /* atan(+0,-anything) = pi */
            case 3:
                return -PI - tiny; /* atan(-0,-anything) =-pi */
            }
        }

        /* when x = 0 */
        if ((ix | lx) == 0) {
            return ((hy < 0) ? (-pi_o_2 - tiny) : (pi_o_2 + tiny));
        }

        /* when x is INF */
        if (ix == 0x7ff00000) {
            if (iy == 0x7ff00000) {
                switch (m) {
                case 0:
                    return pi_o_4 + tiny; /* atan(+INF,+INF) */
                case 1:
                    return -pi_o_4 - tiny; /* atan(-INF,+INF) */
                case 2:
                    return (3.0 * pi_o_4) + tiny; /*atan(+INF,-INF)*/
                case 3:
                    return (-3.0 * pi_o_4) - tiny; /*atan(-INF,-INF)*/
                }
            } else {
                switch (m) {
                case 0:
                    return zero; /* atan(+...,+INF) */
                case 1:
                    return -zero; /* atan(-...,+INF) */
                case 2:
                    return PI + tiny; /* atan(+...,-INF) */
                case 3:
                    return -PI - tiny; /* atan(-...,-INF) */
                }
            }
        }

        /* when y is INF */
        if (iy == 0x7ff00000) {
            return (hy < 0) ? (-pi_o_2 - tiny) : (pi_o_2 + tiny);
        }

        /* compute y/x */
        k = (iy - ix) >> 20;
        if (k > 60) {

            /* |y/x| >  2**60 */
            z = pi_o_2 + (0.5 * pi_lo);
        } else if ((hx < 0) && (k < -60)) {

            /* |y|/x < -2**60 */
            z = 0.0;
        } else {

            /* safe to do y/x */
            z = atan(abs(y / x));
        }

        switch (m) {
        case 0:
            return z; /* atan(+,+) */
        case 1:
            return setHI(z, __HI(z) ^ 0x80000000); /* atan(-,+) */
        case 2:
            return PI - (z - pi_lo); /* atan(+,-) */
        default: /* case 3 */
            return (z - pi_lo) - PI; /* atan(-,-) */
        }
    }

    /*
     * kernel function:
     *        __kernel_sin                ... sine function on [-pi/4,pi/4]
     *        __ieee754_rem_pio2        ... argument reduction routine
     *
     * Method.
     *      Let S,C and T denote the sin, cos and tan respectively on
     *        [-PI/4, +PI/4]. Reduce the argument x to y1+y2 = x-k*pi/2
     *        in [-pi/4 , +pi/4], and let n = k mod 4.
     *        We have
     *
     *          n        sin(x)      cos(x)        tan(x)
     *     ----------------------------------------------------------
     *            0               S           C                 T
     *            1               C          -S                -1/T
     *            2              -S          -C                 T
     *            3              -C           S                -1/T
     *     ----------------------------------------------------------
     *
     * Special cases:
     *      Let trig be any of sin, cos, or tan.
     *      trig(+-INF)  is NaN, with signals;
     *      trig(NaN)    is that NaN;
     *
     * Accuracy:
     *        TRIG(x) returns trig(x) nearly rounded
     */
    /*
     * Table of constants for 2/pi, 396 Hex digits (476 decimal) of 2/pi
     */
    static private final int[] two_over_pi = {
            0xa2f983, 0x6e4e44, 0x1529fc, 0x2757d1, 0xf534dd, 0xc0db62, 0x95993c,
            0x439041, 0xfe5163, 0xabdebb, 0xc561b7, 0x246e3a, 0x424dd2, 0xe00649,
            0x2eea09, 0xd1921c, 0xfe1deb, 0x1cb129, 0xa73ee8, 0x8235f5, 0x2ebb44,
            0x84e99c, 0x7026b4, 0x5f7e41, 0x3991d6, 0x398353, 0x39f49c, 0x845f8b,
            0xbdf928, 0x3b1ff8, 0x97ffde, 0x05980f, 0xef2f11, 0x8b5a0a, 0x6d1f6d,
            0x367ecf, 0x27cb09, 0xb74f46, 0x3f669e, 0x5fea2d, 0x7527ba, 0xc7ebe5,
            0xf17b3d, 0x0739f7, 0x8a5292, 0xea6bfb, 0x5fb11f, 0x8d5d08, 0x560330,
            0x46fc7b, 0x6babf0, 0xcfbc20, 0x9af436, 0x1da9e3, 0x91615e, 0xe61b08,
            0x659985, 0x5f14a0, 0x68408d, 0xffd880, 0x4d7327, 0x310606, 0x1556ca,
            0x73a8c9, 0x60e27b, 0xc08c6b
        };
    static private final int[] npio2_hw = {
            0x3ff921fb, 0x400921fb, 0x4012d97c, 0x401921fb, 0x401f6a7a,
            0x4022d97c, 0x4025fdbb, 0x402921fb, 0x402c463a, 0x402f6a7a,
            0x4031475c, 0x4032d97c, 0x40346b9c, 0x4035fdbb, 0x40378fdb,
            0x403921fb, 0x403ab41b, 0x403c463a, 0x403dd85a, 0x403f6a7a,
            0x40407e4c, 0x4041475c, 0x4042106c, 0x4042d97c, 0x4043a28c,
            0x40446b9c, 0x404534ac, 0x4045fdbb, 0x4046c6cb, 0x40478fdb,
            0x404858eb, 0x404921fb
        };
    static private final double zero = 0.00000000000000000000e+00; // 0x0000000000000000 
    static private final double half = 0x1.0p-1; /* 5.00000000000000000000e-01 */
    static private final double two24 = 0x1.0p24; /* 1.67772160000000000000e+07 */
    static private final double invpio2 = 0x1.45f306dc9c883p-1; /* 6.36619772367581382433e-01 53 bits of 2/pi */
    static private final double pio2_1 = 0x1.921fb544p0; /* 1.57079632673412561417e+00 first  33 bit of pi/2 */
    static private final double pio2_1t = 0x1.0b4611a626331p-34; /* 6.07710050650619224932e-11 pi/2 - pio2_1 */
    static private final double pio2_2 = 0x1.0b4611a6p-34; /* 6.07710050630396597660e-11 second 33 bit of pi/2 */
    static private final double pio2_2t = 0x1.3198a2e037073p-69; /* 2.02226624879595063154e-21 pi/2 - (pio2_1+pio2_2) */
    static private final double pio2_3 = 0x1.3198a2ep-69; /* 2.02226624871116645580e-21 third  33 bit of pi/2 */
    static private final double pio2_3t = 0x1.b839a252049c1p-104; /* 8.47842766036889956997e-32 pi/2 - (pio2_1+pio2_2+pio2_3) */

    /*
     *        Return the remainder of x % pi/2 in y[0]+y[1]
     */
    static private int __ieee754_rem_pio2(double x, double[] y) {
        double z = zero;
        double w;
        double t;
        double r;
        double fn;
        int i;
        int j;
        int nx;
        int n;
        int ix;
        int hx;

        hx = __HI(x); /* high word of x */
        ix = hx & 0x7fffffff;
        if (ix <= 0x3fe921fb) {

            /* |x| ~<= pi/4 , no need for reduction */
            y[0] = x;
            y[1] = 0;
            return 0;
        }

        if (ix < 0x4002d97c) {

            /* |x| < 3pi/4, special case with n=+-1 */
            if (hx > 0) {
                z = x - pio2_1;
                if (ix != 0x3ff921fb) { /* 33+53 bit pi is good enough */
                    y[0] = z - pio2_1t;
                    y[1] = (z - y[0]) - pio2_1t;
                } else { /* near pi/2, use 33+33+53 bit pi */
                    z -= pio2_2;
                    y[0] = z - pio2_2t;
                    y[1] = (z - y[0]) - pio2_2t;
                }
                return 1;
            } else { /* negative x */
                z = x + pio2_1;
                if (ix != 0x3ff921fb) { /* 33+53 bit pi is good enough */
                    y[0] = z + pio2_1t;
                    y[1] = (z - y[0]) + pio2_1t;
                } else { /* near pi/2, use 33+33+53 bit pi */
                    z += pio2_2;
                    y[0] = z + pio2_2t;
                    y[1] = (z - y[0]) + pio2_2t;
                }
                return -1;
            }
        }

        if (ix <= 0x413921fb) { /* |x| ~<= 2^19*(pi/2), medium size */
            t = abs(x);
            n = (int) ((t * invpio2) + half);
            fn = (double) n;
            r = t - (fn * pio2_1);
            w = fn * pio2_1t; /* 1st round good to 85 bit */
            if ((n < 32) && (ix != npio2_hw[n - 1])) {
                y[0] = r - w; /* quick check no cancellation */
            } else {
                j = ix >> 20;
                y[0] = r - w;
                i = j - (((__HI(y[0])) >> 20) & 0x7ff);
                if (i > 16) { /* 2nd iteration needed, good to 118 */
                    t = r;
                    w = fn * pio2_2;
                    r = t - w;
                    w = (fn * pio2_2t) - ((t - r) - w);
                    y[0] = r - w;
                    i = j - (((__HI(y[0])) >> 20) & 0x7ff);
                    if (i > 49) { /* 3rd iteration need, 151 bits acc */
                        t = r; /* will cover all possible cases */
                        w = fn * pio2_3;
                        r = t - w;
                        w = (fn * pio2_3t) - ((t - r) - w);
                        y[0] = r - w;
                    }
                }
            }
            y[1] = (r - y[0]) - w;
            if (hx < 0) {
                y[0] = -y[0];
                y[1] = -y[1];
                return -n;
            } else {
                return n;
            }
        }

        /*
         * all other (large) arguments
         */
        if (ix >= 0x7ff00000) {

            /* x is inf or NaN */
            y[0] = y[1] = x - x;
            return 0;
        }

        /* set z = scalbn(|x|,ilogb(x)-23) */
        double[] tx = new double[3];
        long lx = Double.doubleToLongBits(x);
        long exp = (0x7ff0000000000000L & lx) >> 52;
        exp -= 1046;
        lx -= (exp << 52);
        lx &= 0x7fffffffffffffffL;
        z = Double.longBitsToDouble(lx);
        for (i = 0; i < 2; i++) {
            tx[i] = (double) ((int) (z));
            z = (z - tx[i]) * two24;
        }
        tx[2] = z;
        nx = 3;
        while (tx[nx - 1] == zero)
            nx--; /* skip zero term */
        n = __kernel_rem_pio2(tx, y, (int) exp, nx);
        //System.out.println("KERNEL");
        //System.out.println("tx "+tx[0]+"  "+tx[1]+"  "+tx[2]);
        //System.out.println("y "+y[0]+"  "+y[1]);
        //System.out.println("exp "+(int)exp+"  nx "+nx+"   n "+n);
        if (hx < 0) {
            y[0] = -y[0];
            y[1] = -y[1];
            return -n;
        }
        return n;
    }

    /*
     * __kernel_rem_pio2(x,y,e0,nx)
     * double x[],y[]; int e0,nx; int two_over_pi[];
     *
     * __kernel_rem_pio2 return the last three digits of N with
     *                y = x - N*pi/2
     * so that |y| < pi/2.
     *
     * The method is to compute the integer (mod 8) and fraction parts of
     * (2/pi)*x without doing the full multiplication. In general we
     * skip the part of the product that are known to be a huge integer
     * (more accurately, = 0 mod 8 ). Thus the number of operations are
     * independent of the exponent of the input.
     *
     * (2/pi) is represented by an array of 24-bit integers in two_over_pi[].
     *
     * Input parameters:
     *         x[]        The input value (must be positive) is broken into nx
     *                pieces of 24-bit integers in double precision format.
     *                x[i] will be the i-th 24 bit of x. The scaled exponent
     *                of x[0] is given in input parameter e0 (i.e., x[0]*2^e0
     *                match x's up to 24 bits.
     *
     *                Example of breaking a double positive z into x[0]+x[1]+x[2]:
     *                        e0 = ilogb(z)-23
     *                        z  = scalbn(z,-e0)
     *                for i = 0,1,2
     *                        x[i] = floor(z)
     *                        z    = (z-x[i])*2**24
     *
     *
     *        y[]        ouput result in an array of double precision numbers.
     *                The dimension of y[] is:
     *                        24-bit  precision        1
     *                        53-bit  precision        2
     *                        64-bit  precision        2
     *                        113-bit precision        3
     *                The actual value is the sum of them. Thus for 113-bit
     *                precison, one may have to do something like:
     *
     *                long double t,w,r_head, r_tail;
     *                t = (long double)y[2] + (long double)y[1];
     *                w = (long double)y[0];
     *                r_head = t+w;
     *                r_tail = w - (r_head - t);
     *
     *        e0        The exponent of x[0]
     *
     *        nx        dimension of x[]
     *
     *          prec        an integer indicating the precision:
     *                        0        24  bits (single)
     *                        1        53  bits (double)
     *                        2        64  bits (extended)
     *                        3        113 bits (quad)
     *
     *        two_over_pi[]
     *                integer array, contains the (24*i)-th to (24*i+23)-th
     *                bit of 2/pi after binary point. The corresponding
     *                floating value is
     *
     *                        two_over_pi[i] * 2^(-24(i+1)).
     *
     * External function:
     *        double scalbn(), floor();
     *
     *
     * Here is the description of some local variables:
     *
     *         jk        jk+1 is the initial number of terms of two_over_pi[] needed
     *                in the computation. The recommended value is 2,3,4,
     *                6 for single, double, extended,and quad.
     *
     *         jz        local integer variable indicating the number of
     *                terms of two_over_pi[] used.
     *
     *        jx        nx - 1
     *
     *        jv        index for pointing to the suitable two_over_pi[] for the
     *                computation. In general, we want
     *                        ( 2^e0*x[0] * two_over_pi[jv-1]*2^(-24jv) )/8
     *                is an integer. Thus
     *                        e0-3-24*jv >= 0 or (e0-3)/24 >= jv
     *                Hence jv = max(0,(e0-3)/24).
     *
     *        jp        jp+1 is the number of terms in PIo2[] needed, jp = jk.
     *
     *         q[]        double array with integral value, representing the
     *                24-bits chunk of the product of x and 2/pi.
     *
     *        q0        the corresponding exponent of q[0]. Note that the
     *                exponent for q[i] would be q0-24*i.
     *
     *        PIo2[]        double precision array, obtained by cutting pi/2
     *                into 24 bits chunks.
     *
     *        f[]        two_over_pi[] in floating point
     *
     *        iq[]        integer array by breaking up q[] in 24-bits chunk.
     *
     *        fq[]        final product of x*(2/pi) in fq[0],..,fq[jk]
     *
     *        ih        integer. If >0 it indicates q[] is >= 0.5, hence
     *                it also indicates the *sign* of the result.
     *
     */
    /*
     * Constants:
     * The hexadecimal values are the intended ones for the following
     * constants. The decimal values may be used, provided that the
     * compiler will convert from decimal to binary accurately enough
     * to produce the hexadecimal values shown.
     */
    static final private double[] PIo2 = {
            0x1.921fb4p0, /* 1.57079625129699707031e+00 */
            0x1.4442dp-24, /* 7.54978941586159635335e-08 */
            0x1.846988p-48, /* 5.39030252995776476554e-15 */
            0x1.8cc516p-72, /* 3.28200341580791294123e-22 */
            0x1.01b838p-96, /* 1.27065575308067607349e-29 */
            0x1.a25204p-120, /* 1.22933308981111328932e-36 */
            0x1.382228p-145, /* 2.73370053816464559624e-44 */
            0x1.9f31dp-169 /* 2.16741683877804819444e-51 */
        };
    static final private double twon24 = 0x1.0p-24; /* 5.96046447753906250000e-08 */

    static private int __kernel_rem_pio2(double[] x, double[] y, int e0, int nx) {
        int jz;
        int jx;
        int jv;
        int jp;
        int jk;
        int carry;
        int n;
        int i;
        int j;
        int k;
        int m;
        int q0;
        int ih;
        double z;
        double fw;
        double[] f = new double[20];
        double[] q = new double[20];
        double[] fq = new double[20];
        int[] iq = new int[20];

        /* initialize jk*/
        jk = 4;
        jp = jk;

        /* determine jx,jv,q0, note that 3>q0 */
        jx = nx - 1;
        jv = (e0 - 3) / 24;
        if (jv < 0) {
            jv = 0;
        }
        q0 = e0 - (24 * (jv + 1));

        /* set up f[0] to f[jx+jk] where f[jx+jk] = two_over_pi[jv+jk] */
        j = jv - jx;
        m = jx + jk;
        for (i = 0; i <= m; i++, j++)
            f[i] = ((j < 0) ? zero : (double) two_over_pi[j]);

        /* compute q[0],q[1],...q[jk] */
        for (i = 0; i <= jk; i++) {
            for (j = 0, fw = 0.0; j <= jx; j++)
                fw += (x[j] * f[(jx + i) - j]);
            q[i] = fw;
        }

        jz = jk;

        while (true) { // recompute:

            /* distill q[] into iq[] reversingly */
            for (i = 0, j = jz, z = q[jz]; j > 0; i++, j--) {
                fw = (double) ((int) (twon24 * z));
                iq[i] = (int) (z - (two24 * fw));
                z = q[j - 1] + fw;
            }

            /* compute n */
            z = scalbn(z, q0); /* actual value of z */
            z -= (8.0 * floor(z * 0.125)); /* trim off integer >= 8 */
            n = (int) z;
            z -= (double) n;
            ih = 0;
            if (q0 > 0) { /* need iq[jz-1] to determine n */
                i = (iq[jz - 1] >> (24 - q0));
                n += i;
                iq[jz - 1] -= (i << (24 - q0));
                ih = iq[jz - 1] >> (23 - q0);
            } else if (q0 == 0) {
                ih = iq[jz - 1] >> 23;
            } else if (z >= 0.5) {
                ih = 2;
            }
            if (ih > 0) { /* q > 0.5 */
                n += 1;
                carry = 0;
                for (i = 0; i < jz; i++) { /* compute 1-q */
                    j = iq[i];
                    if (carry == 0) {
                        if (j != 0) {
                            carry = 1;
                            iq[i] = 0x1000000 - j;
                        }
                    } else {
                        iq[i] = 0xffffff - j;
                    }
                }
                if (q0 > 0) { /* rare case: chance is 1 in 12 */
                    switch (q0) {
                    case 1:
                        iq[jz - 1] &= 0x7fffff;
                        break;
                    case 2:
                        iq[jz - 1] &= 0x3fffff;
                        break;
                    }
                }
                if (ih == 2) {
                    z = one - z;
                    if (carry != 0) {
                        z -= scalbn(one, q0);
                    }
                }
            }

            /* check if recomputation is needed */
            if (z == zero) {
                j = 0;
                for (i = jz - 1; i >= jk; i--)
                    j |= iq[i];
                if (j == 0) { /* need recomputation */
                    for (k = 1; iq[jk - k] == 0; k++)
                        ; /* k = no. of terms needed */for (i = jz + 1;
                            i <= (jz + k); i++) { /* add q[jz+1] to q[jz+k] */
                        f[jx + i] = (double) two_over_pi[jv + i];
                        for (j = 0, fw = 0.0; j <= jx; j++)
                            fw += (x[j] * f[(jx + i) - j]);
                        q[i] = fw;
                    }
                    jz += k;
                    continue; //goto recompute;
                }
            }
            break;
        }

        /* chop off zero terms */
        if (z == 0.0) {
            jz--;
            q0 -= 24;
            while (iq[jz] == 0) {
                jz--;
                q0 -= 24;
            }
        } else { /* break z into 24-bit if necessary */
            z = scalbn(z, -q0);
            if (z >= two24) {
                fw = (double) ((int) (twon24 * z));
                iq[jz] = (int) (z - (two24 * fw));
                jz++;
                q0 += 24;
                iq[jz] = (int) fw;
            } else {
                iq[jz] = (int) z;
            }
        }

        /* convert integer "bit" chunk to floating-point value */
        fw = scalbn(one, q0);
        for (i = jz; i >= 0; i--) {
            q[i] = fw * (double) iq[i];
            fw *= twon24;
        }

        /* compute PIo2[0,...,jp]*q[jz,...,0] */
        for (i = jz; i >= 0; i--) {
            for (fw = 0.0, k = 0; (k <= jp) && (k <= (jz - i)); k++)
                fw += (PIo2[k] * q[i + k]);
            fq[jz - i] = fw;
        }

        /* compress fq[] into y[] */
        fw = 0.0;
        for (i = jz; i >= 0; i--)
            fw += fq[i];
        y[0] = (ih == 0) ? fw : (-fw);
        fw = fq[0] - fw;
        for (i = 1; i <= jz; i++)
            fw += fq[i];
        y[1] = ((ih == 0) ? fw : (-fw));
        return n & 7;
    }

    static final private double[] bp = { 1.0, 1.5, };
    static final private double[] dp_h = {
            0.0, 0x1.2b8034p-1
        }; /* 5.84962487220764160156e-01 */
    static final private double[] dp_l = {
            0.0, 0x1.cfdeb43cfd006p-27
        }; /* 1.35003920212974897128e-08 */
    static final private double two53 = 0x1.0p53; /* 9007199254740992.0 */

    /* poly coefs for (3/2)*(log(x)-2s-2/3*s**3 */
    static final private double L1 = 0x1.3333333333303p-1; /* 5.99999999999994648725e-01 */
    static final private double L2 = 0x1.b6db6db6fabffp-2; /*  4.28571428578550184252e-01 */
    static final private double L3 = 0x1.55555518f264dp-2; /*  3.33333329818377432918e-01 */
    static final private double L4 = 0x1.17460a91d4101p-2; /*  2.72728123808534006489e-01 */
    static final private double L5 = 0x1.d864a93c9db65p-3; /*  2.30660745775561754067e-01 */
    static final private double L6 = 0x1.a7e284a454eefp-3; /*  2.06975017800338417784e-01 */
    static final private double lg2 = 0x1.62e42fefa39efp-1; /*  6.93147180559945286227e-01 */
    static final private double lg2_h = 0x1.62e43p-1; /*  6.93147182464599609375e-01 */
    static final private double lg2_l = -1.90465429995776804525e-09; /* 0xbe205c610ca86c39 */
    static final private double ovt = 8.0085662595372944372e-17; /* -(1024-log2(ovfl+.5ulp)) */
    static final private double cp = 0x1.ec709dc3a03fdp-1; /*  9.61796693925975554329e-01 = 2/(3ln2) */
    static final private double cp_h = 0x1.ec709ep-1; /*  9.61796700954437255859e-01 = (float)cp */
    static final private double cp_l = -0x1.e2fe0145b01f5p-28; /* -7.02846165095275826516e-09 = tail of cp_h*/
    static final private double ivln2 = 0x1.71547652b82fep0; /*  1.44269504088896338700e+00 = 1/ln2 */
    static final private double ivln2_h = 0x1.715476p0; /*  1.44269502162933349609e+00 = 24b 1/ln2*/
    static final private double ivln2_l = 0x1.4ae0bf85ddf44p-26; /*  1.92596299112661746887e-08 = 1/ln2 tail*/

    /*
     *        Returns x to the power y
     *
     *                      n
     * Method:  Let x =  2   * (1+f)
     *        1. Compute and return log2(x) in two pieces:
     *                log2(x) = w1 + w2,
     *           where w1 has 53-24 = 29 bit trailing zeros.
     *        2. Perform y*log2(x) = n+y' by simulating muti-precision
     *           arithmetic, where |y'|<=0.5.
     *        3. Return x**y = 2**n*exp(y'*log2)
     *
     * Special cases:
     *        1.  (anything) ** 0  is 1
     *        2.  (anything) ** 1  is itself
     *        3.  (anything) ** NAN is NAN
     *        4.  NAN ** (anything except 0) is NAN
     *        5.  +-(|x| > 1) **  +INF is +INF
     *        6.  +-(|x| > 1) **  -INF is +0
     *        7.  +-(|x| < 1) **  +INF is +0
     *        8.  +-(|x| < 1) **  -INF is +INF
     *        9.  +-1         ** +-INF is NAN
     *        10. +0 ** (+anything except 0, NAN)               is +0
     *        11. -0 ** (+anything except 0, NAN, odd integer)  is +0
     *        12. +0 ** (-anything except 0, NAN)               is +INF
     *        13. -0 ** (-anything except 0, NAN, odd integer)  is +INF
     *        14. -0 ** (odd integer) = -( +0 ** (odd integer) )
     *        15. +INF ** (+anything except 0,NAN) is +INF
     *        16. +INF ** (-anything except 0,NAN) is +0
     *        17. -INF ** (anything)  = -0 ** (-anything)
     *        18. (-anything) ** (integer) is (-1)**(integer)*(+anything**integer)
     *        19. (-anything except 0 and inf) ** (non-integer) is NAN
     *
     * Accuracy:
     *        pow(x,y) returns x**y nearly rounded. In particular
     *                        pow(integer,integer)
     *        always returns the correct integer provided it is
     *        representable.
     */

    /**
     *        Returns x to the power y.
     *        @param        x        The base.
     *        @param        y        The exponent.
     *        @return        x to the power y.
     */
    static public double pow(double x, double y) {
        double z;
        double ax;
        double z_h;
        double z_l;
        double p_h;
        double p_l;
        double y1;
        double t1;
        double t2;
        double r;
        double s;
        double t;
        double u;
        double v;
        double w;
        int i;
        int j;
        int k;
        int yisint;
        int n;
        int hx;
        int hy;
        int ix;
        int iy;
        int lx;
        int ly;

        hx = __HI(x);
        lx = __LO(x);
        hy = __HI(y);
        ly = __LO(y);
        ix = hx & 0x7fffffff;
        iy = hy & 0x7fffffff;

        /* y==zero: x**0 = 1 */
        if ((iy | ly) == 0) {
            return one;
        }

        /* +-NaN return x+y */
        if ((ix > 0x7ff00000) || ((ix == 0x7ff00000) && (lx != 0)) ||
                (iy > 0x7ff00000) || ((iy == 0x7ff00000) && (ly != 0))) {
            return x + y;
        }

        /* determine if y is an odd int when x < 0
         * yisint = 0        ... y is not an integer
         * yisint = 1        ... y is an odd int
         * yisint = 2        ... y is an even int
         */
        yisint = 0;
        if (hx < 0) {
            if (iy >= 0x43400000) {
                yisint = 2; /* even integer y */
            } else if (iy >= 0x3ff00000) {
                k = (iy >> 20) - 0x3ff; /* exponent */
                if (k > 20) {
                    j = ly >>> (52 - k);
                    if ((j << (52 - k)) == ly) {
                        yisint = 2 - (j & 1);
                    }
                } else if (ly == 0) {
                    j = iy >> (20 - k);
                    if ((j << (20 - k)) == iy) {
                        yisint = 2 - (j & 1);
                    }
                }
            }
        }

        /* special value of y */
        if (ly == 0) {
            if (iy == 0x7ff00000) { /* y is +-inf */
                if (((ix - 0x3ff00000) | lx) == 0) {
                    return y - y; /* inf**+-1 is NaN */
                } else if (ix >= 0x3ff00000) { /* (|x|>1)**+-inf = inf,0 */
                    return (hy >= 0) ? y : zero;
                } else { /* (|x|<1)**-,+inf = inf,0 */
                    return (hy < 0) ? (-y) : zero;
                }
            }
            if (iy == 0x3ff00000) { /* y is  +-1 */
                if (hy < 0) {
                    return one / x;
                } else {
                    return x;
                }
            }
            if (hy == 0x40000000) {
                return x * x; /* y is  2 */
            }
            if (hy == 0x3fe00000) { /* y is  0.5 */
                if (hx >= 0) { /* x >= +0 */
                    return sqrt(x);
                }
            }
        }

        ax = abs(x);

        /* special value of x */
        if (lx == 0) {
            if ((ix == 0x7ff00000) || (ix == 0) || (ix == 0x3ff00000)) {
                z = ax; /*x is +-0,+-inf,+-1*/
                if (hy < 0) {
                    z = one / z; /* z = (1/|x|) */
                }
                if (hx < 0) {
                    if (((ix - 0x3ff00000) | yisint) == 0) {
                        z = (z - z) / (z - z); /* (-1)**non-int is NaN */
                    } else if (yisint == 1) {
                        z = -z; /* (x<0)**odd = -(|x|**odd) */
                    }
                }
                return z;
            }
        }

        /* (x<0)**(non-int) is NaN */
        if ((((hx >> 31) + 1) | yisint) == 0) {
            return (x - x) / (x - x);
        }

        /* |y| is huge */
        if (iy > 0x41e00000) { /* if |y| > 2**31 */
            if (iy > 0x43f00000) { /* if |y| > 2**64, must o/uflow */
                if (ix <= 0x3fefffff) {
                    return ((hy < 0) ? (huge * huge) : (tiny * tiny));
                }
                if (ix >= 0x3ff00000) {
                    return ((hy > 0) ? (huge * huge) : (tiny * tiny));
                }
            }

            /* over/underflow if x is not close to one */
            if (ix < 0x3fefffff) {
                return ((hy < 0) ? (huge * huge) : (tiny * tiny));
            }
            if (ix > 0x3ff00000) {
                return ((hy > 0) ? (huge * huge) : (tiny * tiny));
            }

            /* now |1-x| is tiny <= 2**-20, suffice to compute
               log(x) by x-x^2/2+x^3/3-x^4/4 */
            t = x - 1; /* t has 20 trailing zeros */
            w = (t * t) * (0.5 - (t * (0.3333333333333333333333 - (t * 0.25))));
            u = ivln2_h * t; /* ivln2_h has 21 sig. bits */
            v = (t * ivln2_l) - (w * ivln2);
            t1 = u + v;
            t1 = setLO(t1, 0);
            t2 = v - (t1 - u);
        } else {
            double s2;
            double s_h;
            double s_l;
            double t_h;
            double t_l;
            n = 0;

            /* take care subnormal number */
            if (ix < 0x00100000) {
                ax *= two53;
                n -= 53;
                ix = __HI(ax);
            }
            n += (((ix) >> 20) - 0x3ff);
            j = ix & 0x000fffff;

            /* determine interval */
            ix = j | 0x3ff00000; /* normalize ix */
            if (j <= 0x3988E) {
                k = 0; /* |x|<sqrt(3/2) */
            } else if (j < 0xBB67A) {
                k = 1; /* |x|<sqrt(3)   */
            } else {
                k = 0;
                n += 1;
                ix -= 0x00100000;
            }
            ax = setHI(ax, ix);

            /* compute s = s_h+s_l = (x-1)/(x+1) or (x-1.5)/(x+1.5) */
            u = ax - bp[k]; /* bp[0]=1.0, bp[1]=1.5 */
            v = one / (ax + bp[k]);
            s = u * v;
            s_h = s;
            s_h = setLO(s_h, 0);

            /* t_h=ax+bp[k] High */
            t_h = zero;
            t_h = setHI(t_h, ((ix >> 1) | 0x20000000) + 0x00080000 + (k << 18));
            t_l = ax - (t_h - bp[k]);
            s_l = v * ((u - (s_h * t_h)) - (s_h * t_l));

            /* compute log(ax) */
            s2 = s * s;
            r = s2 * s2 * (L1 +
                (s2 * (L2 +
                (s2 * (L3 + (s2 * (L4 + (s2 * (L5 + (s2 * L6))))))))));
            r += (s_l * (s_h + s));
            s2 = s_h * s_h;
            t_h = 3.0 + s2 + r;
            t_h = setLO(t_h, 0);
            t_l = r - ((t_h - 3.0) - s2);

            /* u+v = s*(1+...) */
            u = s_h * t_h;
            v = (s_l * t_h) + (t_l * s);

            /* 2/(3log2)*(s+...) */
            p_h = u + v;
            p_h = setLO(p_h, 0);
            p_l = v - (p_h - u);
            z_h = cp_h * p_h; /* cp_h+cp_l = 2/(3*log2) */
            z_l = (cp_l * p_h) + (p_l * cp) + dp_l[k];

            /* log2(ax) = (s+..)*2/(3*log2) = n + dp_h + z_h + z_l */
            t = (double) n;
            t1 = (((z_h + z_l) + dp_h[k]) + t);
            t1 = setLO(t1, 0);
            t2 = z_l - (((t1 - t) - dp_h[k]) - z_h);
        }

        s = one; /* s (sign of result -ve**odd) = -1 else = 1 */
        if ((((hx >> 31) + 1) | (yisint - 1)) == 0) {
            s = -one; /* (-ve)**(odd int) */
        }

        /* split up y into y1+y2 and compute (y1+y2)*(t1+t2) */
        y1 = y;
        y1 = setLO(y1, 0);
        p_l = ((y - y1) * t1) + (y * t2);
        p_h = y1 * t1;
        z = p_l + p_h;
        j = __HI(z);
        i = __LO(z);
        if (j >= 0x40900000) { /* z >= 1024 */
            if (((j - 0x40900000) | i) != 0) { /* if z > 1024 */
                return s * huge * huge; /* overflow */
            } else {
                if ((p_l + ovt) > (z - p_h)) {
                    return s * huge * huge; /* overflow */
                }
            }
        } else if ((j & 0x7fffffff) >= 0x4090cc00) { /* z <= -1075 */
            if (((j - 0xc090cc00) | i) != 0) { /* z < -1075 */
                return s * tiny * tiny; /* underflow */
            } else {
                if (p_l <= (z - p_h)) {
                    return s * tiny * tiny; /* underflow */
                }
            }
        }

        /*
         * compute 2**(p_h+p_l)
         */
        i = j & 0x7fffffff;
        k = (i >> 20) - 0x3ff;
        n = 0;
        if (i > 0x3fe00000) { /* if |z| > 0.5, set n = [z+0.5] */
            n = j + (0x00100000 >> (k + 1));
            k = ((n & 0x7fffffff) >> 20) - 0x3ff; /* new k for n */
            t = zero;
            t = setHI(t, (n & ~(0x000fffff >> k)));
            n = ((n & 0x000fffff) | 0x00100000) >> (20 - k);
            if (j < 0) {
                n = -n;
            }
            p_h -= t;
        }
        t = p_l + p_h;
        t = setLO(t, 0);
        u = t * lg2_h;
        v = ((p_l - (t - p_h)) * lg2) + (t * lg2_l);
        z = u + v;
        w = v - (z - u);
        t = z * z;
        t1 = z - (t * (P1 + (t * (P2 + (t * (P3 + (t * (P4 + (t * P5)))))))));
        r = ((z * t1) / (t1 - 2.0)) - (w + (z * w));
        z = one - (r - z);
        j = __HI(z);
        j += (n << 20);
        if ((j >> 20) <= 0) {
            z = scalbn(z, n); /* subnormal output */
        } else {
            i = __HI(z);
            i += (n << 20);
            z = setHI(z, i);
        }
        return s * z;
    }

    /*
     * copysign(double x, double y)
     * copysign(x,y) returns a value with the magnitude of x and
     * with the sign bit of y.
     */
    static private double copysign(double x, double y) {
        long ix = Double.doubleToRawLongBits(x);
        long iy = Double.doubleToRawLongBits(y);
        ix = (0x7fffffffffffffffL & ix) | (0x8000000000000000L & iy);
        return Double.longBitsToDouble(ix);
    }

    static private final double two54 = 0x1.0p54; /*  1.80143985094819840000e+16 */
    static private final double twom54 = 0x1.0p-54; /*  5.55111512312578270212e-17 */

    /*
     * scalbn (double x, int n)
     * scalbn(x,n) returns x* 2**n  computed by  exponent
     * manipulation rather than by actually performing an
     * exponentiation or a multiplication.
     */
    static private double scalbn(double x, int n) {
        int k;
        int hx;
        int lx;
        hx = __HI(x);
        lx = __LO(x);
        k = (hx & 0x7ff00000) >> 20; /* extract exponent */
        if (k == 0) { /* 0 or subnormal x */
            if ((lx | (hx & 0x7fffffff)) == 0) {
                return x; /* +-0 */
            }
            x *= two54;
            hx = __HI(x);
            k = ((hx & 0x7ff00000) >> 20) - 54;
            if (n < -50000) {
                return tiny * x; /*underflow*/
            }
        }
        if (k == 0x7ff) {
            return x + x; /* NaN or Inf */
        }
        k = k + n;
        if (k > 0x7fe) {
            return huge * copysign(huge, x); /* overflow  */
        }
        if (k > 0) {

            /* normal result */
            return setHI(x, (hx & 0x800fffff) | (k << 20));
        }
        if (k <= -54) {
            if (n > 50000) { /* in case integer overflow in n+k */
                return huge * copysign(huge, x); /*overflow*/
            }
        } else {
            return tiny * copysign(tiny, x); /*underflow*/
        }
        k += 54; /* subnormal result */
        return twom54 * setHI(x, (hx & 0x800fffff) | (k << 20));
    }

    static private double set(int newHiPart, int newLowPart) {
        return Double.longBitsToDouble((((long) newHiPart) << 32) | newLowPart);
    }

    static private double setLO(double x, int newLowPart) {
        long lx = Double.doubleToRawLongBits(x);
        lx &= 0xFFFFFFFF00000000L;
        lx |= newLowPart;
        return Double.longBitsToDouble(lx);
    }

    static private double setHI(double x, int newHiPart) {
        long lx = Double.doubleToRawLongBits(x);
        lx &= 0x00000000FFFFFFFFL;
        lx |= (((long) newHiPart) << 32);
        return Double.longBitsToDouble(lx);
    }

    static private int __HI(double x) {
        return (int) (0xFFFFFFFF & (Double.doubleToRawLongBits(x) >> 32));
    }

    static private int __LO(double x) {
        return (int) (0xFFFFFFFF & Double.doubleToRawLongBits(x));
    }
} 