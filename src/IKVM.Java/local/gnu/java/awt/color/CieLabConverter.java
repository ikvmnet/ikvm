/*
 * Copyright (C) 2011 http://stackoverflow.com/users/12048/finnw
 * Copyright (C) 2015 Daniel Wilson
 *
 * License:
 * http://creativecommons.org/licenses/by-sa/2.5/
 *
 * Based on:
 * http://stackoverflow.com/questions/4593469/java-how-to-convert-rgb-color-to-cie-lab/5021831#5021831
 *
 */
package gnu.java.awt.color;

public class CieLabConverter implements ColorSpaceConverter
{
    @Override
    public float[] fromCIEXYZ(float[] colorvalue) {
        double l = f(colorvalue[1]);
        double L = 116.0 * l - 16.0;
        double a = 500.0 * (f(colorvalue[0]) - l);
        double b = 200.0 * (l - f(colorvalue[2]));
        return new float[] {(float) L, (float) a, (float) b};
    }

    @Override
    public float[] fromRGB(float[] rgbvalue) {
        float[] xyz = SrgbConverter.RGBtoXYZ(rgbvalue);
        return fromCIEXYZ(xyz);
    }
	
    @Override
    public float[] toCIEXYZ(float[] colorvalue) {
        double i = (colorvalue[0] + 16.0) * (1.0 / 116.0);
        double X = fInv(i + colorvalue[1] * (1.0 / 500.0));
        double Y = fInv(i);
        double Z = fInv(i - colorvalue[2] * (1.0 / 200.0));
        return new float[] {(float) X, (float) Y, (float) Z};
    }

    @Override
    public float[] toRGB(float[] colorvalue) {
        float[] xyz = toCIEXYZ(colorvalue);
		return SrgbConverter.XYZtoRGB(xyz);
    }
	
    private static double f(double x) {
        if (x > 216.0 / 24389.0) {
            return Math.cbrt(x);
        } else {
            return (841.0 / 108.0) * x + N;
        }
    }

    private static double fInv(double x) {
        if (x > 6.0 / 29.0) {
            return x*x*x;
        } else {
            return (108.0 / 841.0) * (x - N);
        }
    }

    private static final double N = 4.0 / 29.0;
}
