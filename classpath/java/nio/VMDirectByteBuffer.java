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

import gnu.classpath.RawData;
import cli.System.IntPtr;
import cli.System.Runtime.InteropServices.Marshal;

class VMDirectByteBuffer
{
    // this method is used by JNI.NewDirectByteBuffer
    static ByteBuffer NewDirectByteBuffer(IntPtr p, int capacity)
    {
        return new DirectByteBufferImpl.ReadWrite(null, new RawData(p), capacity, capacity, 0);
    }

    static IntPtr GetDirectBufferAddress(Buffer buf)
    {
        return buf.address != null ? buf.address.p() : IntPtr.Zero;
    }

    static RawData allocate(int capacity)
    {
        return new RawData(Marshal.AllocHGlobal(capacity));
    }

    static void free(RawData r)
    {
        Marshal.FreeHGlobal(r.p());
    }

    static byte get(RawData r, int index)
    {
        return r.ReadByte(index);
    }

    static void get(RawData r, int index, byte[] dst, int offset, int length)
    {
        IntPtr address = new IntPtr(r.p().ToInt64() + index);
        Marshal.Copy(address, dst, offset, length);
    }

    static void put(RawData r, int index, byte value)
    {
        r.WriteByte(index, value);
    }

    static void shiftDown(RawData r, int dst_offset, int src_offset, int count)
    {
        r.MoveMemory(dst_offset, src_offset, count);
    }

    static RawData adjustAddress(RawData r, int pos)
    {
        return new RawData(new IntPtr(r.p().ToInt64() + pos));
    }
}
