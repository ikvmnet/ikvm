/* DirectByteBufferImpl.java --
   Copyright (C) 2003, 2004, 2006 Free Software Foundation, Inc.

This file is part of GNU Classpath.

GNU Classpath is free software; you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation; either version 2, or (at your option)
any later version.

GNU Classpath is distributed in the hope that it will be useful, but
WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
General Public License for more details.

You should have received a copy of the GNU General Public License
along with GNU Classpath; see the file COPYING.  If not, write to the
Free Software Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA
02110-1301 USA.

Linking this library statically or dynamically with other modules is
making a combined work based on this library.  Thus, the terms and
conditions of the GNU General Public License cover the whole
combination.

As a special exception, the copyright holders of this library give you
permission to link this library with independent modules to produce an
executable, regardless of the license terms of these independent
modules, and to copy and distribute the resulting executable under
terms of your choice, provided that you also meet, for each linked
independent module, the terms and conditions of the license of that
module.  An independent module is a module which is not derived from
or based on this library.  If you modify this library, you may extend
this exception to your version of the library, but you are not
obligated to do so.  If you do not wish to do so, delete this
exception statement from your version. */


package java.nio;

import cli.System.IO.FileStream;
import cli.System.IntPtr;
import cli.System.Runtime.InteropServices.Marshal;
import gnu.classpath.PointerUtil;
import gnu.java.nio.FileChannelImpl;
import java.io.IOException;
import java.lang.ref.ReferenceQueue;
import java.lang.ref.PhantomReference;
import java.util.Vector;

@ikvm.lang.Internal
public class DirectByteBufferImpl extends MappedByteBuffer
{
    /**
     * The owner is used to keep alive the object that actually owns the
     * memory. (For DirectByteBufferImpl instances that are a view onto another DirectByteBufferImpl instance.)
     */
    private final Object owner;
    private final IntPtr ptr;
    private boolean isFileMapping;

    static final class ReadOnly extends DirectByteBufferImpl
    {
        ReadOnly(Object owner, IntPtr ptr, int capacity, int limit, int position)
        {
            super(owner, ptr, capacity, limit, position);
        }

        public ByteBuffer put(byte value)
        {
            throw new ReadOnlyBufferException();
        }

        public ByteBuffer put(int index, byte value)
        {
            throw new ReadOnlyBufferException();
        }

        public ByteBuffer put(byte[] src, int offset, int length)
        {
            throw new ReadOnlyBufferException();
        }

        public ByteBuffer compact()
        {
            throw new ReadOnlyBufferException();
        }

        public boolean isReadOnly()
        {
            return true;
        }
    }

    private static final class Cleanup extends PhantomReference
    {
        // TODO on Whidbey we should use a global critical finalizable object
        // to drain the keepAlive vector on AppDomain unload
        private static final Vector keepAlive = new Vector();
        private static final ReferenceQueue queue = new ReferenceQueue();
        volatile IntPtr ptr;
        final boolean isFileMapping;
        final int size;

        static
        {
            // TODO it sucks to burn a thread on this
            Thread t = new Thread("DirectByteBuffer cleanup") {
                public void run() {
                    for (;;) {
                        try {
                            Cleanup obj = (Cleanup)queue.remove();
                            keepAlive.remove(obj);
                            obj.free();
                        } catch (InterruptedException _) {}
                    }
                }
            };
            t.setDaemon(true);
            t.start();
        }

        Cleanup(Object obj, boolean isFileMapping, int size)
        {
            super(obj, queue);
            this.isFileMapping = isFileMapping;
            this.size = size;
            keepAlive.add(this);
        }

        void free()
        {
            if (isFileMapping)
            {
                FileChannelImpl.unmapViewOfFile(ptr, size);
            }
            else
            {
                Marshal.FreeHGlobal(ptr);
            }
        }
    }

    DirectByteBufferImpl(int capacity)
    {
        super(capacity, capacity, 0, -1);
        this.owner = this;
        Cleanup c = new Cleanup(this, false, 0);
        // TODO this should be a CER
        this.ptr = c.ptr = Marshal.AllocHGlobal(capacity);
        this.address = PointerUtil.fromIntPtr(ptr);
        // Marshal.AllocHGlobal doesn't clear the memory, so we have to do that manually
        for(int i = 0; i < capacity; i++)
        {
            Marshal.WriteByte(ptr, i, (byte)0);
        }
        cli.System.Threading.Thread.MemoryBarrier();
        cli.System.GC.KeepAlive(this);
    }

    DirectByteBufferImpl(Object owner, IntPtr ptr, int capacity, int limit, int position)
    {
        super(capacity, limit, position, -1);
        this.owner = owner;
        this.ptr = ptr;
        this.address = PointerUtil.fromIntPtr(ptr);
    }

    public static DirectByteBufferImpl map(FileStream fs, boolean writeable, boolean copy_on_write, long position, int size) throws IOException
    {
        // TODO this should be a CER
        IntPtr ptr = FileChannelImpl.mapViewOfFile(fs, writeable, copy_on_write, position, size);
        DirectByteBufferImpl impl = writeable ?
            new DirectByteBufferImpl(null, ptr, size, size, 0) :
            new DirectByteBufferImpl.ReadOnly(null, ptr, size, size, 0);
        impl.isFileMapping = true;
        Cleanup c = new Cleanup(impl, true, size);
        c.ptr = ptr;
        cli.System.Threading.Thread.MemoryBarrier();
        cli.System.GC.KeepAlive(impl);
        return impl;
    }

    void forceImpl()
    {
        if (isFileMapping)
            FileChannelImpl.flushViewOfFile(ptr, capacity());
    }

    boolean isLoadedImpl()
    {
        return false;
    }

    void loadImpl()
    {
    }

    private static IntPtr add(IntPtr ptr, int offset)
    {
        return new IntPtr(ptr.ToInt64() + offset);
    }

    public static ByteBuffer allocate(int capacity)
    {
        return new DirectByteBufferImpl(capacity);
    }

    public boolean isReadOnly()
    {
        return false;
    }

    public byte get()
    {
        checkForUnderflow();

        int pos = position();
        byte result = Marshal.ReadByte(ptr, pos);
        position(pos + 1);
        return result;
    }

    public byte get(int index)
    {
        checkIndex(index);

        return Marshal.ReadByte(ptr, index);
    }

    public ByteBuffer get(byte[] dst, int offset, int length)
    {
        checkArraySize(dst.length, offset, length);
        checkForUnderflow(length);

        int index = position();
        Marshal.Copy(add(ptr, index), dst, offset, length);
        position(index+length);

        return this;
    }

    public ByteBuffer put(byte value)
    {
        checkForOverflow();

        int pos = position();
        Marshal.WriteByte(ptr, pos, value);
        position(pos + 1);
        return this;
    }

    public ByteBuffer put(int index, byte value)
    {
        checkIndex(index);

        Marshal.WriteByte(ptr, index, value);
        return this;
    }

    public ByteBuffer put(byte[] src, int offset, int length)
    {
        checkArraySize(src.length, offset, length);
        checkForUnderflow(length);

        int index = position();
        Marshal.Copy(src, offset, add(ptr, index), length);
        position(index + length);

        return this;
    }

    public ByteBuffer compact()
    {
        mark = -1;
        int pos = position();
        if (pos > 0)
        {
            int count = remaining();
            for (int i = 0; i < count; i++)
            {
                Marshal.WriteByte(ptr, i, Marshal.ReadByte(ptr, i + pos));
            }
            position(count);
            limit(capacity());
        }
        else
        {
            position(limit());
            limit(capacity());
        }
        return this;
    }

    public ByteBuffer slice()
    {
        int rem = remaining();
        if (isReadOnly())
            return new DirectByteBufferImpl.ReadOnly(owner, add(ptr, position()), rem, rem, 0);
        else
            return new DirectByteBufferImpl(owner, add(ptr, position()), rem, rem, 0);
    }

    private ByteBuffer duplicate(boolean readOnly)
    {
        int pos = position();
        if (this.mark != -1)
            reset();
        int mark = position();
        position(pos);
        DirectByteBufferImpl result;
        if (readOnly)
            result = new DirectByteBufferImpl.ReadOnly(owner, ptr, capacity(), limit(), pos);
        else
            result = new DirectByteBufferImpl(owner, ptr, capacity(), limit(), pos);

        if (mark != pos)
        {
            result.position(mark);
            result.mark();
            result.position(pos);
        }
        return result;
    }

    public ByteBuffer duplicate()
    {
        return duplicate(isReadOnly());
    }

    public ByteBuffer asReadOnlyBuffer()
    {
        return duplicate(true);
    }

    public boolean isDirect()
    {
        return true;
    }

    public CharBuffer asCharBuffer()
    {
        return new CharViewBufferImpl(this, remaining() >> 1);
    }

    public ShortBuffer asShortBuffer()
    {
        return new ShortViewBufferImpl(this, remaining() >> 1);
    }

    public IntBuffer asIntBuffer()
    {
        return new IntViewBufferImpl(this, remaining() >> 2);
    }

    public LongBuffer asLongBuffer()
    {
        return new LongViewBufferImpl(this, remaining() >> 3);
    }

    public FloatBuffer asFloatBuffer()
    {
        return new FloatViewBufferImpl(this, remaining() >> 2);
    }

    public DoubleBuffer asDoubleBuffer()
    {
        return new DoubleViewBufferImpl(this, remaining() >> 3);
    }

    public char getChar()
    {
        return ByteBufferHelper.getChar(this, order());
    }

    public ByteBuffer putChar(char value)
    {
        ByteBufferHelper.putChar(this, value, order());
        return this;
    }

    public char getChar(int index)
    {
        return ByteBufferHelper.getChar(this, index, order());
    }

    public ByteBuffer putChar(int index, char value)
    {
        ByteBufferHelper.putChar(this, index, value, order());
        return this;
    }

    public short getShort()
    {
        return ByteBufferHelper.getShort(this, order());
    }

    public ByteBuffer putShort(short value)
    {
        ByteBufferHelper.putShort(this, value, order());
        return this;
    }

    public short getShort(int index)
    {
        return ByteBufferHelper.getShort(this, index, order());
    }

    public ByteBuffer putShort(int index, short value)
    {
        ByteBufferHelper.putShort(this, index, value, order());
        return this;
    }

    public int getInt()
    {
        return ByteBufferHelper.getInt(this, order());
    }

    public ByteBuffer putInt(int value)
    {
        ByteBufferHelper.putInt(this, value, order());
        return this;
    }

    public int getInt(int index)
    {
        return ByteBufferHelper.getInt(this, index, order());
    }

    public ByteBuffer putInt(int index, int value)
    {
        ByteBufferHelper.putInt(this, index, value, order());
        return this;
    }

    public long getLong()
    {
        return ByteBufferHelper.getLong(this, order());
    }

    public ByteBuffer putLong(long value)
    {
        ByteBufferHelper.putLong(this, value, order());
        return this;
    }

    public long getLong(int index)
    {
        return ByteBufferHelper.getLong(this, index, order());
    }

    public ByteBuffer putLong(int index, long value)
    {
        ByteBufferHelper.putLong(this, index, value, order());
        return this;
    }

    public float getFloat()
    {
        return ByteBufferHelper.getFloat(this, order());
    }

    public ByteBuffer putFloat(float value)
    {
        ByteBufferHelper.putFloat(this, value, order());
        return this;
    }

    public float getFloat(int index)
    {
        return ByteBufferHelper.getFloat(this, index, order());
    }

    public ByteBuffer putFloat(int index, float value)
    {
        ByteBufferHelper.putFloat(this, index, value, order());
        return this;
    }

    public double getDouble()
    {
        return ByteBufferHelper.getDouble(this, order());
    }

    public ByteBuffer putDouble(double value)
    {
        ByteBufferHelper.putDouble(this, value, order());
        return this;
    }

    public double getDouble(int index)
    {
        return ByteBufferHelper.getDouble(this, index, order());
    }

    public ByteBuffer putDouble(int index, double value)
    {
        ByteBufferHelper.putDouble(this, index, value, order());
        return this;
    }
}
