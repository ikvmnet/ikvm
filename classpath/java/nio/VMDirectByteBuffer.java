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

    static IntPtr GetDirectBufferAddress(ByteBuffer buf)
    {
        return ((DirectByteBufferImpl)buf).address.p();
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
        Marshal.Copy(address, ikvm.lang.ByteArrayHack.cast(dst), offset, length);
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
