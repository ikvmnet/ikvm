/*
  Copyright (C) 2004, 2005 Jeroen Frijters

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
package java.nio;

import gnu.classpath.Pointer;
import cli.System.IntPtr;
import cli.System.Runtime.InteropServices.Marshal;

@ikvm.lang.Internal
public class VMDirectByteBuffer
{
    // this method is used by JNI.NewDirectByteBuffer
    public static ByteBuffer NewDirectByteBuffer(IntPtr p, int capacity)
    {
        return new DirectByteBufferImpl.ReadWrite(null, new Pointer(p), capacity, capacity, 0);
    }

    public static IntPtr GetDirectBufferAddress(Buffer buf)
    {
        return buf.address != null ? buf.address.p() : IntPtr.Zero;
    }

    static Pointer allocate(int capacity)
    {
        return new Pointer(Marshal.AllocHGlobal(capacity));
    }

    static void free(Pointer r)
    {
        Marshal.FreeHGlobal(r.p());
    }

    static byte get(Pointer r, int index)
    {
        return r.ReadByte(index);
    }

    static void get(Pointer r, int index, byte[] dst, int offset, int length)
    {
        IntPtr address = new IntPtr(r.p().ToInt64() + index);
        Marshal.Copy(address, dst, offset, length);
    }

    static void put(Pointer r, int index, byte value)
    {
        r.WriteByte(index, value);
    }

    static void put(Pointer r, int index, byte[] src, int offset, int length)
    {
        IntPtr address = new IntPtr(r.p().ToInt64() + index);
        Marshal.Copy(src, offset, address, length);
    }

    static void shiftDown(Pointer r, int dst_offset, int src_offset, int count)
    {
        r.MoveMemory(dst_offset, src_offset, count);
    }

    static Pointer adjustAddress(Pointer r, int pos)
    {
        return new Pointer(new IntPtr(r.p().ToInt64() + pos));
    }
}
