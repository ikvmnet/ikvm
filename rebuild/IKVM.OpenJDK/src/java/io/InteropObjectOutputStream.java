/*
 * Copyright (c) 1996, 2006, Oracle and/or its affiliates. All rights reserved.
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

import cli.System.Runtime.Serialization.IObjectReference;
import cli.System.Runtime.Serialization.SerializationException;
import cli.System.Runtime.Serialization.SerializationInfo;
import cli.System.Runtime.Serialization.StreamingContext;
import cli.System.SerializableAttribute;
import java.util.ArrayList;

@ikvm.lang.Internal
public final class InteropObjectOutputStream extends ObjectOutputStream
{
    private final ObjectDataOutputStream dos;
    private Object curObj;
    private ObjectStreamClass curDesc;
    private PutFieldImpl curPut;
    
    @SerializableAttribute.Annotation
    private static final class ReplaceProxy implements IObjectReference
    {
        private Object obj;

        @cli.System.Security.SecurityCriticalAttribute.Annotation
        public Object GetRealObject(StreamingContext context)
        {
            return obj;
        }
    }
    
    static final class DynamicProxy implements Serializable
    {
        Object obj;

        private Object readResolve()
        {
            return obj;
        }
    }
    
    public static void writeObject(Object obj, SerializationInfo info)
    {
        try
        {
            boolean replaced = false;
            Class cl = obj.getClass();
            ObjectStreamClass desc;
            for (;;)
            {
                Class repCl;
                desc = ObjectStreamClass.lookup(cl, true);
                if (!desc.hasWriteReplaceMethod() ||
                    (obj = desc.invokeWriteReplace(obj)) == null ||
                    (repCl = obj.getClass()) == cl)
                {
                    break;
                }
                cl = repCl;
                replaced = true;
            }
            if (replaced)
            {
                info.AddValue("obj", obj);
                info.SetType(ikvm.runtime.Util.getInstanceTypeFromClass(ReplaceProxy.class));
            }
            else
            {
                new InteropObjectOutputStream(info, obj, cl, desc);
            }
        }
        catch (IOException x)
        {
            ikvm.runtime.Util.throwException(new SerializationException(x.getMessage(), x));
        }
    }
    
    private InteropObjectOutputStream(SerializationInfo info, Object obj, Class cl, ObjectStreamClass desc) throws IOException
    {
        dos = new ObjectDataOutputStream(info);
        if (obj instanceof ObjectStreamClass)
        {
            ObjectStreamClass osc = (ObjectStreamClass)obj;
            if (osc.isProxy())
            {
                writeProxyDesc(osc);
            }
            else
            {
                writeNonProxyDesc(osc);
            }
        }
        else if (obj instanceof Serializable)
        {
            if (desc.isDynamicClass())
            {
                info.SetType(ikvm.runtime.Util.getInstanceTypeFromClass(DynamicProxy.class));
            }
            writeOrdinaryObject(obj, desc);
        }
        else
        {
            throw new NotSerializableException(cl.getName());
        }
        dos.close();
    }
    
    private void writeProxyDesc(ObjectStreamClass desc) throws IOException
    {
        writeByte(TC_PROXYCLASSDESC);
        Class cl = desc.forClass();
        Class[] ifaces = cl.getInterfaces();
        writeInt(ifaces.length);
        for (int i = 0; i < ifaces.length; i++)
        {
            writeObject(ifaces[i]);
        }
        writeObject(desc.getSuperDesc());
    }
    
    private void writeNonProxyDesc(ObjectStreamClass desc) throws IOException
    {
        writeByte(TC_CLASSDESC);
        writeObject(desc.forClass());
        desc.writeNonProxy(this);
        writeObject(desc.getSuperDesc());
    }
    
    private void writeOrdinaryObject(Object obj, ObjectStreamClass desc) throws IOException
    {
        desc.checkSerialize();
        writeObject(desc);
        if (desc.isExternalizable() && !desc.isProxy())
        {
            writeExternalData((Externalizable)obj);
        }
        else
        {
            writeSerialData(obj, desc);
        }
    }
    
    private void writeExternalData(Externalizable obj) throws IOException
    {
        Object oldObj = curObj;
        ObjectStreamClass oldDesc = curDesc;
        PutFieldImpl oldPut = curPut;
        curObj = obj;
        curDesc = null;
        curPut = null;

        obj.writeExternal(this);
        dos.writeMarker();

        curObj = oldObj;
        curDesc = oldDesc;
        curPut = oldPut;
    }
    
    private void writeSerialData(Object obj, ObjectStreamClass desc) throws IOException
    {
        ObjectStreamClass.ClassDataSlot[] slots = desc.getClassDataLayout();
        for (int i = 0; i < slots.length; i++)
        {
            ObjectStreamClass slotDesc = slots[i].desc;
            if (slotDesc.hasWriteObjectMethod())
            {
                Object oldObj = curObj;
                ObjectStreamClass oldDesc = curDesc;
                PutFieldImpl oldPut = curPut;
                curObj = obj;
                curDesc = slotDesc;
                curPut = null;
                slotDesc.invokeWriteObject(obj, this);
                dos.writeMarker();
                curObj = oldObj;
                curDesc = oldDesc;
                curPut = oldPut;
            }
            else
            {
                defaultWriteFields(obj, slotDesc);
            }
        }
    }

    private void defaultWriteFields(Object obj, ObjectStreamClass desc) throws IOException
    {
        desc.checkDefaultSerialize();

        byte[] primVals = new byte[desc.getPrimDataSize()];
        desc.getPrimFieldValues(obj, primVals);
        write(primVals);

        Object[] objVals = new Object[desc.getNumObjFields()];
        desc.getObjFieldValues(obj, objVals);
        for (int i = 0; i < objVals.length; i++)
        {
            writeObject(objVals[i]);
        }
    }

    @Override    
    public void defaultWriteObject() throws IOException
    {
        defaultWriteFields(curObj, curDesc);
    }

    @Override    
    public void close()
    {
        throw new UnsupportedOperationException();
    }

    @Override    
    public void flush()
    {
    }

    @Override    
    public ObjectOutputStream.PutField putFields() throws IOException
    {
        if (curPut == null)
        {
            if (curObj == null || curDesc == null)
            {
                throw new NotActiveException("not in call to writeObject");
            }
            curPut = new PutFieldImpl(curDesc);
        }
        return curPut;
    }

    @Override    
    public void reset() throws IOException
    {
        throw new IOException("stream active");
    }

    @Override    
    protected void writeObjectOverride(Object obj)
    {
        // TODO consider tagging ghost arrays
        dos.writeObject(obj);
    }

    @Override    
    public void writeFields() throws IOException
    {
        if (curPut == null)
        {
            throw new NotActiveException("no current PutField object");
        }
        curPut.writeFields();
    }

    @Override    
    public void writeUnshared(Object obj)
    {
        throw new UnsupportedOperationException();
    }

    @Override    
    public void useProtocolVersion(int version)
    {
        throw new IllegalStateException("stream non-empty");
    }

    @Override    
    public void write(byte[] buf) throws IOException
    {
        write(buf, 0, buf.length);
    }

    @Override    
    public void write(int val) throws IOException
    {
        dos.write(val);
    }

    @Override    
    public void write(byte[] buf, int off, int len) throws IOException
    {
        dos.write(buf, off, len);
    }

    @Override    
    public void writeBoolean(boolean val) throws IOException
    {
        dos.writeBoolean(val);
    }

    @Override    
    public void writeByte(int val) throws IOException
    {
        dos.writeByte(val);
    }

    @Override    
    public void writeBytes(String str) throws IOException
    {
        dos.writeBytes(str);
    }

    @Override    
    public void writeChar(int val) throws IOException
    {
        dos.writeChar(val);
    }

    @Override    
    public void writeChars(String str) throws IOException
    {
        dos.writeChars(str);
    }

    @Override    
    public void writeDouble(double val) throws IOException
    {
        dos.writeDouble(val);
    }

    @Override    
    public void writeFloat(float val) throws IOException
    {
        dos.writeFloat(val);
    }

    @Override    
    public void writeInt(int val) throws IOException
    {
        dos.writeInt(val);
    }

    @Override    
    public void writeLong(long val) throws IOException
    {
        dos.writeLong(val);
    }

    @Override    
    public void writeShort(int val) throws IOException
    {
        dos.writeShort(val);
    }

    @Override    
    public void writeUTF(String str) throws IOException
    {
        dos.writeUTF(str);
    }
    
    // private API used by ObjectStreamClass
    @Override    
    void writeTypeString(String str) throws IOException
    {
        writeObject(str);
    }

    private final class PutFieldImpl extends PutField {

        /** class descriptor describing serializable fields */
        private final ObjectStreamClass desc;
        /** primitive field values */
        private final byte[] primVals;
        /** object field values */
        private final Object[] objVals;

        /**
         * Creates PutFieldImpl object for writing fields defined in given
         * class descriptor.
         */
        PutFieldImpl(ObjectStreamClass desc) {
            this.desc = desc;
            primVals = new byte[desc.getPrimDataSize()];
            objVals = new Object[desc.getNumObjFields()];
        }

        public void put(String name, boolean val) {
            Bits.putBoolean(primVals, getFieldOffset(name, Boolean.TYPE), val);
        }

        public void put(String name, byte val) {
            primVals[getFieldOffset(name, Byte.TYPE)] = val;
        }

        public void put(String name, char val) {
            Bits.putChar(primVals, getFieldOffset(name, Character.TYPE), val);
        }

        public void put(String name, short val) {
            Bits.putShort(primVals, getFieldOffset(name, Short.TYPE), val);
        }

        public void put(String name, int val) {
            Bits.putInt(primVals, getFieldOffset(name, Integer.TYPE), val);
        }

        public void put(String name, float val) {
            Bits.putFloat(primVals, getFieldOffset(name, Float.TYPE), val);
        }

        public void put(String name, long val) {
            Bits.putLong(primVals, getFieldOffset(name, Long.TYPE), val);
        }

        public void put(String name, double val) {
            Bits.putDouble(primVals, getFieldOffset(name, Double.TYPE), val);
        }

        public void put(String name, Object val) {
            objVals[getFieldOffset(name, Object.class)] = val;
        }

        // deprecated in ObjectOutputStream.PutField
        public void write(ObjectOutput out) throws IOException {
            /*
             * Applications should *not* use this method to write PutField
             * data, as it will lead to stream corruption if the PutField
             * object writes any primitive data (since block data mode is not
             * unset/set properly, as is done in OOS.writeFields()).  This
             * broken implementation is being retained solely for behavioral
             * compatibility, in order to support applications which use
             * OOS.PutField.write() for writing only non-primitive data.
             *
             * Serialization of unshared objects is not implemented here since
             * it is not necessary for backwards compatibility; also, unshared
             * semantics may not be supported by the given ObjectOutput
             * instance.  Applications which write unshared objects using the
             * PutField API must use OOS.writeFields().
             */
            if (InteropObjectOutputStream.this != out) {
                throw new IllegalArgumentException("wrong stream");
            }
            out.write(primVals, 0, primVals.length);

            ObjectStreamField[] fields = desc.getFields(false);
            int numPrimFields = fields.length - objVals.length;
            // REMIND: warn if numPrimFields > 0?
            for (int i = 0; i < objVals.length; i++) {
                if (fields[numPrimFields + i].isUnshared()) {
                    throw new IOException("cannot write unshared object");
                }
                out.writeObject(objVals[i]);
            }
        }

        /**
         * Writes buffered primitive data and object fields to stream.
         */
        void writeFields() throws IOException {
            InteropObjectOutputStream.this.write(primVals, 0, primVals.length);

            ObjectStreamField[] fields = desc.getFields(false);
            int numPrimFields = fields.length - objVals.length;
            for (int i = 0; i < objVals.length; i++) {
                writeObject(objVals[i]);
            }
        }

        /**
         * Returns offset of field with given name and type.  A specified type
         * of null matches all types, Object.class matches all non-primitive
         * types, and any other non-null type matches assignable types only.
         * Throws IllegalArgumentException if no matching field found.
         */
        private int getFieldOffset(String name, Class type) {
            ObjectStreamField field = desc.getField(name, type);
            if (field == null) {
                throw new IllegalArgumentException("no such field " + name +
                                                   " with type " + type);
            }
            return field.getOffset();
        }
    }

    private static final class ObjectDataOutputStream extends DataOutputStream
    {
        /*
         * States:
         *  blockStart   objCount
         *          -1          0       Previous byte was a marker (or we're at the start of the stream), next byte will be MARKER, OBJECTS or BYTES
         *         >=0          0       We're currently writing a byte stream (that starts at blockStart + 1)
         *          -1         >0       We're currently writing objects (just a count of objects really)
         *
         */
        private static final byte MARKER = 0;
        private static final byte OBJECTS = 10;
        private static final byte BYTES = 20;
        private final SerializationInfo info;
        private byte[] buf = new byte[16];
        private int pos;
        private int blockStart = -1;
        private int objCount;
        private int objId;

        ObjectDataOutputStream(SerializationInfo info)
        {
            super(null);
            out = this;
            this.info = info;
        }
        
        private void grow(int minGrow)
        {
            int newSize = buf.length + Math.max(buf.length, minGrow);
            byte[] newBuf = new byte[newSize];
            System.arraycopy(buf, 0, newBuf, 0, pos);
            buf = newBuf;
        }
        
        private void switchToData()
        {
            if (objCount == 0)
            {
                if (pos == buf.length)
                {
                    grow(1);
                }
                buf[pos++] = BYTES;
            }
            else
            {
                int len = packedLength(objCount);
                if (pos + len >= buf.length)
                {
                    grow(len);
                }
                writePacked(pos, objCount);
                objCount = 0;
                pos += len;
            }
            blockStart = pos;
            // reserve space for the number of bytes
            // (we assume that one byte will suffice, but if it won't the endData code will copy the data to make room)
            if (pos == buf.length)
            {
                grow(1);
            }
            pos++;
        }
        
        private void endData()
        {
            if (blockStart == -1)
            {
                if (pos == buf.length)
                {
                    grow(1);
                }
                buf[pos++] = OBJECTS;
            }
            else
            {
                int len = (pos - blockStart) - 1;
                int lenlen = packedLength(len);
                if (lenlen > 1)
                {
                    lenlen--;
                    if (buf.length - pos <= lenlen)
                    {
                        grow(lenlen);
                    }
                    System.arraycopy(buf, blockStart + 1, buf, blockStart + 1 + lenlen, pos - (blockStart + 1));
                    pos += lenlen;
                }
                writePacked(blockStart, len);
                blockStart = -1;
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
        
        private void writePacked(int pos, int v)
        {
            if (v < 128)
            {
                buf[pos] = (byte)v;
            }
            else if (v < 16129)
            {
                buf[pos + 0] = (byte)(128 | (v >> 7));
                buf[pos + 1] = (byte)(v & 127);
            }
            else
            {
                buf[pos + 0] = (byte)128;
                buf[pos + 1] = (byte)(v >>> 24);
                buf[pos + 2] = (byte)(v >>> 16);
                buf[pos + 3] = (byte)(v >>>  8);
                buf[pos + 4] = (byte)(v >>>  0);
            }
        }
        
        @Override
        public void write(int b)
        {
            if (blockStart == -1)
            {
                switchToData();
            }
            if (pos == buf.length)
            {
                grow(1);
            }
            buf[pos++] = (byte)b;
        }
        
        @Override
        public void write(byte[] b, int off, int len)
        {
            if (len == 0)
            {
                return;
            }
            if (blockStart == -1)
            {
                switchToData();
            }
            if (pos + len >= buf.length)
            {
                grow(len);
            }
            System.arraycopy(b, off, buf, pos, len);
            pos += len;
        }
        
        public void writeObject(Object obj)
        {
            if (objCount == 0)
            {
                endData();
            }
            objCount++;
            info.AddValue("$" + (objId++), obj);
        }
        
        @Override
        public void close()
        {
            if (objCount == 0)
            {
                if (blockStart == -1)
                {
                    // we've just written a marker, so we don't need to do anything else
                }
                else
                {
                    endData();
                }
            }
            else
            {
                switchToData();
            }
            trim();
            info.AddValue("$data", buf);
        }
        
        private void trim()
        {
            if (buf.length != pos)
            {
                byte[] newBuf = new byte[pos];
                System.arraycopy(buf, 0, newBuf, 0, pos);
                buf = newBuf;
            }
        }
        
        @Override
        public void flush()
        {
        }
        
        public void writeMarker()
        {
            if (objCount == 0)
            {
                if (blockStart != -1)
                {
                    endData();
                }
                if (pos == buf.length)
                {
                    grow(1);
                }
                buf[pos++] = MARKER;
            }
            else
            {
                switchToData();
            }
            blockStart = -1;
        }
    }
}
