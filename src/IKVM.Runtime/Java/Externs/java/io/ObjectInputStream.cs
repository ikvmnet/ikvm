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

namespace IKVM.Java.Externs.java.io
{

    static class ObjectInputStream
    {

        public static void bytesToFloats(byte[] src, int srcpos, float[] dst, int dstpos, int nfloats)
        {
            IKVM.Runtime.FloatConverter converter = new IKVM.Runtime.FloatConverter();
            for (int i = 0; i < nfloats; i++)
            {
                int v = src[srcpos++];
                v = (v << 8) | src[srcpos++];
                v = (v << 8) | src[srcpos++];
                v = (v << 8) | src[srcpos++];
                dst[dstpos++] = IKVM.Runtime.FloatConverter.ToFloat(v, ref converter);
            }
        }

        public static void bytesToDoubles(byte[] src, int srcpos, double[] dst, int dstpos, int ndoubles)
        {
            IKVM.Runtime.DoubleConverter converter = new IKVM.Runtime.DoubleConverter();
            for (int i = 0; i < ndoubles; i++)
            {
                long v = src[srcpos++];
                v = (v << 8) | src[srcpos++];
                v = (v << 8) | src[srcpos++];
                v = (v << 8) | src[srcpos++];
                v = (v << 8) | src[srcpos++];
                v = (v << 8) | src[srcpos++];
                v = (v << 8) | src[srcpos++];
                v = (v << 8) | src[srcpos++];
                dst[dstpos++] = IKVM.Runtime.DoubleConverter.ToDouble(v, ref converter);
            }
        }

    }

}