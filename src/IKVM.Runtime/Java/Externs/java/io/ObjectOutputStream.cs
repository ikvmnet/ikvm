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
    static class ObjectOutputStream
    {
        public static void floatsToBytes(float[] src, int srcpos, byte[] dst, int dstpos, int nfloats)
        {
            IKVM.Runtime.FloatConverter converter = new IKVM.Runtime.FloatConverter();
            for (int i = 0; i < nfloats; i++)
            {
                int v = IKVM.Runtime.FloatConverter.ToInt(src[srcpos++], ref converter);
                dst[dstpos++] = (byte)(v >> 24);
                dst[dstpos++] = (byte)(v >> 16);
                dst[dstpos++] = (byte)(v >> 8);
                dst[dstpos++] = (byte)(v >> 0);
            }
        }

        public static void doublesToBytes(double[] src, int srcpos, byte[] dst, int dstpos, int ndoubles)
        {
            IKVM.Runtime.DoubleConverter converter = new IKVM.Runtime.DoubleConverter();
            for (int i = 0; i < ndoubles; i++)
            {
                long v = IKVM.Runtime.DoubleConverter.ToLong(src[srcpos++], ref converter);
                dst[dstpos++] = (byte)(v >> 56);
                dst[dstpos++] = (byte)(v >> 48);
                dst[dstpos++] = (byte)(v >> 40);
                dst[dstpos++] = (byte)(v >> 32);
                dst[dstpos++] = (byte)(v >> 24);
                dst[dstpos++] = (byte)(v >> 16);
                dst[dstpos++] = (byte)(v >> 8);
                dst[dstpos++] = (byte)(v >> 0);
            }
        }
    }

}