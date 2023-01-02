/*
  Copyright (C) 2007-2013 Jeroen Frijters

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
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace IKVM.Java.Externs.java.nio
{

    [SecurityCritical]
    static class Bits
    {

        public static void copyFromShortArray(object src, long srcPos, long dstAddr, long length)
        {
#if !FIRST_PASS
            short[] shortArray = src as short[];
            if (shortArray != null)
            {
                int index = ((int)srcPos) >> 1;
                while (length > 0)
                {
                    short v = global::java.lang.Short.reverseBytes(shortArray[index++]);
                    Marshal.WriteInt16((IntPtr)dstAddr, v);
                    dstAddr += 2;
                    length -= 2;
                }
            }
            else
            {
                char[] charArray = (char[])src;
                int index = ((int)srcPos) >> 1;
                while (length > 0)
                {
                    short v = global::java.lang.Short.reverseBytes((short)charArray[index++]);
                    Marshal.WriteInt16((IntPtr)dstAddr, v);
                    dstAddr += 2;
                    length -= 2;
                }
            }
#endif
        }

        public static void copyToShortArray(long srcAddr, object dst, long dstPos, long length)
        {
#if !FIRST_PASS
            short[] shortArray = dst as short[];
            if (shortArray != null)
            {
                int index = ((int)dstPos) >> 1;
                while (length > 0)
                {
                    short v = Marshal.ReadInt16((IntPtr)srcAddr);
                    shortArray[index++] = global::java.lang.Short.reverseBytes(v);
                    srcAddr += 2;
                    length -= 2;
                }
            }
            else
            {
                char[] charArray = (char[])dst;
                int index = ((int)dstPos) >> 1;
                while (length > 0)
                {
                    short v = Marshal.ReadInt16((IntPtr)srcAddr);
                    charArray[index++] = (char)global::java.lang.Short.reverseBytes(v);
                    srcAddr += 2;
                    length -= 2;
                }
            }
#endif
        }

        public static void copyFromIntArray(object src, long srcPos, long dstAddr, long length)
        {
#if !FIRST_PASS
            int[] intArray = src as int[];
            if (intArray != null)
            {
                int index = ((int)srcPos) >> 2;
                while (length > 0)
                {
                    int v = global::java.lang.Integer.reverseBytes(intArray[index++]);
                    Marshal.WriteInt32((IntPtr)dstAddr, v);
                    dstAddr += 4;
                    length -= 4;
                }
            }
            else
            {
                float[] floatArray = (float[])src;
                int index = ((int)srcPos) >> 2;
                while (length > 0)
                {
                    int v = global::java.lang.Integer.reverseBytes(global::java.lang.Float.floatToRawIntBits(floatArray[index++]));
                    Marshal.WriteInt32((IntPtr)dstAddr, v);
                    dstAddr += 4;
                    length -= 4;
                }
            }
#endif
        }

        public static void copyToIntArray(long srcAddr, object dst, long dstPos, long length)
        {
#if !FIRST_PASS
            int[] intArray = dst as int[];
            if (intArray != null)
            {
                int index = ((int)dstPos) >> 2;
                while (length > 0)
                {
                    int v = Marshal.ReadInt32((IntPtr)srcAddr);
                    intArray[index++] = global::java.lang.Integer.reverseBytes(v);
                    srcAddr += 4;
                    length -= 4;
                }
            }
            else
            {
                float[] floatArray = (float[])dst;
                int index = ((int)dstPos) >> 2;
                while (length > 0)
                {
                    int v = Marshal.ReadInt32((IntPtr)srcAddr);
                    floatArray[index++] = global::java.lang.Float.intBitsToFloat(global::java.lang.Integer.reverseBytes(v));
                    srcAddr += 4;
                    length -= 4;
                }
            }
#endif
        }

        public static void copyFromLongArray(object src, long srcPos, long dstAddr, long length)
        {
#if !FIRST_PASS
            long[] longArray = src as long[];
            if (longArray != null)
            {
                int index = ((int)srcPos) >> 3;
                while (length > 0)
                {
                    long v = global::java.lang.Long.reverseBytes(longArray[index++]);
                    Marshal.WriteInt64((IntPtr)dstAddr, v);
                    dstAddr += 8;
                    length -= 8;
                }
            }
            else
            {
                double[] doubleArray = (double[])src;
                int index = ((int)srcPos) >> 3;
                while (length > 0)
                {
                    long v = global::java.lang.Long.reverseBytes(BitConverter.DoubleToInt64Bits(doubleArray[index++]));
                    Marshal.WriteInt64((IntPtr)dstAddr, v);
                    dstAddr += 8;
                    length -= 8;
                }
            }
#endif
        }

        public static void copyToLongArray(long srcAddr, object dst, long dstPos, long length)
        {
#if !FIRST_PASS
            long[] longArray = dst as long[];
            if (longArray != null)
            {
                int index = ((int)dstPos) >> 3;
                while (length > 0)
                {
                    long v = Marshal.ReadInt64((IntPtr)srcAddr);
                    longArray[index++] = global::java.lang.Long.reverseBytes(v);
                    srcAddr += 8;
                    length -= 8;
                }
            }
            else
            {
                double[] doubleArray = (double[])dst;
                int index = ((int)dstPos) >> 3;
                while (length > 0)
                {
                    long v = Marshal.ReadInt64((IntPtr)srcAddr);
                    doubleArray[index++] = BitConverter.Int64BitsToDouble(global::java.lang.Long.reverseBytes(v));
                    srcAddr += 8;
                    length -= 8;
                }
            }
#endif
        }

    }

}