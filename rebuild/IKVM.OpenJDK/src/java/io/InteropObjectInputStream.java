/*
 * Copyright (c) 1995, 2006, Oracle and/or its affiliates. All rights reserved.
 * Copyright 2009 Jeroen Frijters
 * DO NOT ALTER OR REMOVE COPYRIGHT NOTICES OR THIS FILE HEADER.
 *
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.  Oracle designates this
 * particular file as subject to the "Classpath" exception as provided
 * by Oracle in the LICENSE file that accompanied this code.
 *
 * This code is distributed in the hope that it will be useful, but WITHOUT
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
 * version 2 for more details (a copy is included in the LICENSE file that
 * accompanied this code).
 *
 * You should have received a copy of the GNU General Public License version
 * 2 along with this work; if not, write to the Free Software Foundation,
 * Inc., 51 Franklin St, Fifth Floor, Boston, MA 02110-1301 USA.
 *
 * Please contact Oracle, 500 Oracle Parkway, Redwood Shores, CA 94065 USA
 * or visit www.oracle.com if you need additional information or have any
 * questions.
 */
package java.io;

import cli.System.Runtime.Serialization.SerializationException;
import cli.System.Runtime.Serialization.SerializationInfo;
import java.util.concurrent.atomic.AtomicBoolean;

@ikvm.lang.Internal
public final class InteropObjectInputStream extends ObjectInputStream
{
    private ObjectDataInputStream dis;
    private CallbackContext curContext;
    
    public static void readObject(Object obj, SerializationInfo info)
    {
        try
        {
            new InteropObjectInputStream(obj, info);
        }
        catch (Exception x)
        {
            ikvm.runtime.Util.throwException(new SerializationException(x.getMessage(), x));
        }
    }
    
    private InteropObjectInputStream(Object obj, SerializationInfo info) throws IOException, ClassNotFoundException
    {
        dis = new ObjectDataInputStream(info);
        if (obj instanceof ObjectStreamClass)
        {
            switch (readByte())
            {
                case TC_PROXYCLASSDESC:
                    readProxyDesc((ObjectStreamClass)obj);
                    break;
                case TC_CLASSDESC:
                    readNonProxyDesc((ObjectStreamClass)obj);
                    break;
                default:
                    throw new StreamCorruptedException();
            }
        }
        else
        {
            readOrdinaryObject(obj);
        }
    }
    
    private ObjectStreamClass readClassDesc() throws IOException, ClassNotFoundException
    {
        return (ObjectStreamClass)readObject();
    }
    
    private void readProxyDesc(ObjectStreamClass desc) throws IOException, ClassNotFoundException
    {
        int numIfaces = readInt();
        Class[] ifaces = new Class[numIfaces];
        for (int i = 0; i < numIfaces; i++)
        {
            ifaces[i] = (Class)readObject();
        }
        Class cl = java.lang.reflect.Proxy.getProxyClass(Thread.currentThread().getContextClassLoader(), ifaces);
        desc.initProxy(cl, null, readClassDesc());
    }
    
    private void readNonProxyDesc(ObjectStreamClass desc) throws IOException, ClassNotFoundException
    {
        Class cl = (Class)readObject();
        ObjectStreamClass readDesc = new ObjectStreamClass();
        readDesc.readNonProxy(this);
        desc.initNonProxy(readDesc, cl, null, readClassDesc());
    }
    
    private void readOrdinaryObject(Object obj) throws IOException, ClassNotFoundException
    {
        ObjectStreamClass desc = readClassDesc();
        desc.checkDeserialize();

        if (obj instanceof InteropObjectOutputStream.DynamicProxy)
        {
            InteropObjectOutputStream.DynamicProxy pp = (InteropObjectOutputStream.DynamicProxy)obj;
            try
            {
                obj = desc.newInstance();
                pp.obj = obj;
            }
            catch (Exception ex)
            {
                throw (IOException) new InvalidClassException(
                    desc.forClass().getName(),
                    "unable to create instance").initCause(ex);
            }
        }
        
        if (desc.isExternalizable())
        {
            readExternalData((Externalizable)obj, desc);
        }
        else
        {
            readSerialData(obj, desc);
        }
    }
    
    private void readExternalData(Externalizable obj, ObjectStreamClass desc) throws IOException, ClassNotFoundException
    {
        CallbackContext oldContext = curContext;
        curContext = null;
        try {
            obj.readExternal(this);
            skipCustomData();
        } finally {
            curContext = oldContext;
        }
    }

    private void readSerialData(Object obj, ObjectStreamClass desc) throws IOException, ClassNotFoundException
    {
        ObjectStreamClass.ClassDataSlot[] slots = desc.getClassDataLayout();
        for (int i = 0; i < slots.length; i++)
        {
            ObjectStreamClass slotDesc = slots[i].desc;

            if (slots[i].hasData)
            {
                if (slotDesc.hasReadObjectMethod())
                {
                    CallbackContext oldContext = curContext;
                    try
                    {
                        curContext = new CallbackContext(obj, slotDesc);
                        slotDesc.invokeReadObject(obj, this);
                    }
                    finally
                    {
                        curContext.setUsed();
                        curContext = oldContext;
                    }
                }
                else
                {
                    defaultReadFields(obj, slotDesc);
                }
                if (slotDesc.hasWriteObjectData())
                {
                    skipCustomData();
                }
            }
            else if (slotDesc.hasReadObjectNoDataMethod())
            {
                slotDesc.invokeReadObjectNoData(obj);
            }
        }
    }
    
    private void skipCustomData() throws IOException
    {
        dis.skipMarker();
    }
    
    private void defaultReadFields(Object obj, ObjectStreamClass desc) throws IOException, ClassNotFoundException
    {
        byte[] primVals = new byte[desc.getPrimDataSize()];
        readFully(primVals);
        desc.setPrimFieldValues(obj, primVals);

        Object[] objVals = new Object[desc.getNumObjFields()];
        for (int i = 0; i < objVals.length; i++)
        {
            objVals[i] = readObject();
        }
        desc.setObjFieldValues(obj, objVals);
    }
    
    @Override
    public void defaultReadObject() throws IOException, ClassNotFoundException
    {
        if (curContext == null) {
            throw new NotActiveException("not in call to readObject");
        }
        Object curObj = curContext.getObj();
        ObjectStreamClass curDesc = curContext.getDesc();
        defaultReadFields(curObj, curDesc);
    }

    @Override
    protected Object readObjectOverride() throws IOException
    {
        return dis.readObject();
    }

    @Override
    public Object readUnshared()
    {
        throw new UnsupportedOperationException();
    }

    @Override
    public ObjectInputStream.GetField readFields() throws IOException, ClassNotFoundException
    {
        if (curContext == null) {
            throw new NotActiveException("not in call to readObject");
        }
        Object curObj = curContext.getObj();
        ObjectStreamClass curDesc = curContext.getDesc();
        GetFieldImpl getField = new GetFieldImpl(curDesc);
        getField.readFields();
        return getField;
    }

    @Override
    public void registerValidation(ObjectInputValidation obj, int prio)
    {
        throw new UnsupportedOperationException();
    }
    
    @Override
    public int read() throws IOException
    {
        return dis.read();
    }

    @Override
    public int read(byte[] buf, int off, int len) throws IOException
    {
        return dis.read(buf, off, len);
    }

    @Override
    public int available() throws IOException
    {
        return dis.available();
    }

    @Override
    public void close() throws IOException
    {
        throw new UnsupportedOperationException();
    }

    @Override
    public boolean readBoolean() throws IOException
    {
        return dis.readBoolean();
    }

    @Override
    public byte readByte() throws IOException
    {
        return dis.readByte();
    }

    @Override
    public int readUnsignedByte() throws IOException
    {
        return dis.readUnsignedByte();
    }

    @Override
    public char readChar() throws IOException
    {
        return dis.readChar();
    }

    @Override
    public short readShort() throws IOException
    {
        return dis.readShort();
    }

    @Override
    public int readUnsignedShort() throws IOException
    {
        return dis.readUnsignedShort();
    }

    @Override
    public int readInt() throws IOException
    {
        return dis.readInt();
    }

    @Override
    public long readLong() throws IOException
    {
        return dis.readLong();
    }

    @Override
    public float readFloat() throws IOException
    {
        return dis.readFloat();
    }

    @Override
    public double readDouble() throws IOException
    {
        return dis.readDouble();
    }

    @Override
    public void readFully(byte[] buf) throws IOException
    {
        readFully(buf, 0, buf.length);
    }

    @Override
    public void readFully(byte[] buf, int off, int len) throws IOException
    {
        dis.readFully(buf, off, len);
    }

    @Override
    public int skipBytes(int len) throws IOException
    {
        return dis.skipBytes(len);
    }

    @Deprecated
    @Override
    public String readLine() throws IOException
    {
        return dis.readLine();
    }

    @Override
    public String readUTF() throws IOException
    {
        return dis.readUTF();
    }
    
    // private API used by ObjectStreamClass
    @Override    
    String readTypeString() throws IOException
    {
        return (String)readObjectOverride();
    }

    private final class GetFieldImpl extends GetField {

        /** class descriptor describing serializable fields */
        private final ObjectStreamClass desc;
        /** primitive field values */
        private final byte[] primVals;
        /** object field values */
        private final Object[] objVals;
        /** object field value handles */
        private final int[] objHandles;

        /**
         * Creates GetFieldImpl object for reading fields defined in given
         * class descriptor.
         */
        GetFieldImpl(ObjectStreamClass desc) {
            this.desc = desc;
            primVals = new byte[desc.getPrimDataSize()];
            objVals = new Object[desc.getNumObjFields()];
            objHandles = new int[objVals.length];
        }

        public ObjectStreamClass getObjectStreamClass() {
            return desc;
        }

        public boolean defaulted(String name) throws IOException {
            return (getFieldOffset(name, null) < 0);
        }

        public boolean get(String name, boolean val) throws IOException {
            int off = getFieldOffset(name, Boolean.TYPE);
            return (off >= 0) ? Bits.getBoolean(primVals, off) : val;
        }

        public byte get(String name, byte val) throws IOException {
            int off = getFieldOffset(name, Byte.TYPE);
            return (off >= 0) ? primVals[off] : val;
        }

        public char get(String name, char val) throws IOException {
            int off = getFieldOffset(name, Character.TYPE);
            return (off >= 0) ? Bits.getChar(primVals, off) : val;
        }

        public short get(String name, short val) throws IOException {
            int off = getFieldOffset(name, Short.TYPE);
            return (off >= 0) ? Bits.getShort(primVals, off) : val;
        }

        public int get(String name, int val) throws IOException {
            int off = getFieldOffset(name, Integer.TYPE);
            return (off >= 0) ? Bits.getInt(primVals, off) : val;
        }

        public float get(String name, float val) throws IOException {
            int off = getFieldOffset(name, Float.TYPE);
            return (off >= 0) ? Bits.getFloat(primVals, off) : val;
        }

        public long get(String name, long val) throws IOException {
            int off = getFieldOffset(name, Long.TYPE);
            return (off >= 0) ? Bits.getLong(primVals, off) : val;
        }

        public double get(String name, double val) throws IOException {
            int off = getFieldOffset(name, Double.TYPE);
            return (off >= 0) ? Bits.getDouble(primVals, off) : val;
        }

        public Object get(String name, Object val) throws IOException {
            int off = getFieldOffset(name, Object.class);
            if (off >= 0) {
                return objVals[off];
            } else {
                return val;
            }
        }

        /**
         * Reads primitive and object field values from stream.
         */
        void readFields() throws IOException {
            readFully(primVals, 0, primVals.length);

            for (int i = 0; i < objVals.length; i++) {
                objVals[i] = readObjectOverride();
            }
        }

        /**
         * Returns offset of field with given name and type.  A specified type
         * of null matches all types, Object.class matches all non-primitive
         * types, and any other non-null type matches assignable types only.
         * If no matching field is found in the (incoming) class
         * descriptor but a matching field is present in the associated local
         * class descriptor, returns -1.  Throws IllegalArgumentException if
         * neither incoming nor local class descriptor contains a match.
         */
        private int getFieldOffset(String name, Class type) {
            ObjectStreamField field = desc.getField(name, type);
            if (field != null) {
                return field.getOffset();
            } else if (desc.getLocalDesc().getField(name, type) != null) {
                return -1;
            } else {
                throw new IllegalArgumentException("no such field " + name +
                                                   " with type " + type);
            }
        }
    }

    private final static class CallbackContext {
        private final Object obj;
        private final ObjectStreamClass desc;
        private final AtomicBoolean used = new AtomicBoolean();

        public CallbackContext(Object obj, ObjectStreamClass desc) {
            this.obj = obj;
            this.desc = desc;
        }

        public Object getObj() throws NotActiveException {
            checkAndSetUsed();
            return obj;
        }

        public ObjectStreamClass getDesc() {
            return desc;
        }

        private void checkAndSetUsed() throws NotActiveException {
            if (!used.compareAndSet(false, true)) {
                 throw new NotActiveException(
                      "not in readObject invocation or fields already read");
            }
        }

        public void setUsed() {
            used.set(true);
        }
    }

    private static final class ObjectDataInputStream extends DataInputStream
    {
        private final static class Inp extends FilterInputStream
        {
            private static final byte MARKER = 0;
            private static final byte OBJECTS = 10;
            private static final byte BYTES = 20;
            private final SerializationInfo info;
            private byte[] buf;
            private int pos;
            private int blockEnd = -1;
            private int objCount;
            private int objId;

            Inp(SerializationInfo info) throws IOException
            {
                super(null);
                this.info = info;
                buf = (byte[])info.GetValue("$data", ikvm.runtime.Util.getInstanceTypeFromClass(cli.System.Object.class));
                modeSwitch();
            }
            
            private void modeSwitch() throws IOException
            {
                if (pos == buf.length)
                {
                    // next read will report error
                    blockEnd = -1;
                    objCount = -1;
                }
                else if (buf[pos] == OBJECTS)
                {
                    pos++;
                    objCount = readPacked();
                    blockEnd = -1;
                }
                else if (buf[pos] == BYTES)
                {
                    pos++;
                    switchToData();
                }
                else if (buf[pos] == MARKER)
                {
                }
                else
                {
                    throw new StreamCorruptedException();
                }
            }
            
            private int packedLength(int val)
            {
                if (val < 0)
                {
                    // we only use packed integers for lengths or counts, so they can never be negative
                    throw new Error();
                }
                else if (val < 128)
                {
                    return 1;
                }
                else if (val < 16129)
                {
                    return 2;
                }
                else
                {
                    return 5;
                }
            }
            
            private int readPacked()
            {
                int b1 = buf[pos++] & 0xFF;
                if (b1 < 128)
                {
                    return b1;
                }
                else if (b1 != 128)
                {
                    return ((b1 & 127) << 7) + (buf[pos++] & 0xFF);
                }
                else
                {
                    int v = 0;
                    v |= (buf[pos++] & 0xFF) << 24;
                    v |= (buf[pos++] & 0xFF) << 16;
                    v |= (buf[pos++] & 0xFF) <<  8;
                    v |= (buf[pos++] & 0xFF) <<  0;
                    return v;
                }
            }
            
            private void switchToData() throws IOException
            {
                if (blockEnd != -1 || objCount != 0 || buf[pos] == MARKER)
                {
                    throw new StreamCorruptedException();
                }
                int len = readPacked();
                blockEnd = pos + len;
                if (blockEnd > buf.length)
                {
                    throw new StreamCorruptedException();
                }
            }

            public int read() throws IOException
            {
                if (blockEnd - pos < 1)
                {
                    switchToData();
                }
                return buf[pos++] & 0xFF;
            }
            
            public int read(byte b[], int off, int len) throws IOException
            {
                if (len == 0)
                {
                    return 0;
                }
                if (blockEnd - pos < len)
                {
                    switchToData();
                }
                System.arraycopy(buf, pos, b, off, len);
                pos += len;
                return len;
            }
            
            public long skip(long n) throws IOException
            {
                if (n == 0)
                {
                    return 0;
                }
                if (blockEnd - pos < n)
                {
                    switchToData();
                }
                pos += (int)n;
                return n;
            }
            
            public int available() throws IOException
            {
                return 0;
            }
            
            public void close() throws IOException
            {
            }
            
            public void mark(int readlimit)
            {
            }
            
            public void reset() throws IOException
            {
            }
            
            public boolean markSupported()
            {
                return false;
            }
            
            Object readObject() throws IOException
            {
                if (objCount <= 0)
                {
                    if (pos != blockEnd || buf[pos] == MARKER)
                    {
                        throw new StreamCorruptedException();
                    }
                    blockEnd = -1;
                    objCount = readPacked();
                }
                objCount--;
                return info.GetValue("$" + (objId++), ikvm.runtime.Util.getInstanceTypeFromClass(cli.System.Object.class));
            }
            
            void skipMarker() throws IOException
            {
                for (;;)
                {
                    if (objCount > 0)
                    {
                        objId += objCount;
                        objCount = 0;
                        if (buf[pos] == MARKER)
                        {
                            pos++;
                            modeSwitch();
                            break;
                        }
                        switchToData();
                    }
                    if (blockEnd != -1)
                    {
                        pos = blockEnd;
                        blockEnd = -1;
                    }
                    if (pos == buf.length)
                    {
                        throw new StreamCorruptedException();
                    }
                    if (buf[pos] == MARKER)
                    {
                        pos++;
                        modeSwitch();
                        break;
                    }
                    blockEnd = -1;
                    objCount = readPacked();
                }
            }
        }

        ObjectDataInputStream(SerializationInfo info) throws IOException
        {
            super(new Inp(info));
        }
        
        public Object readObject() throws IOException
        {
            return ((Inp)in).readObject();
        }
        
        public void skipMarker() throws IOException
        {
            ((Inp)in).skipMarker();
        }
    }
}
