/*
  Copyright (C) 2009 Jeroen Frijters

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
package java.io;

import cli.System.Runtime.Serialization.SerializationInfo;

@ikvm.lang.Internal
public final class InteropObjectInputStream extends ObjectInputStream
{
    private final Object obj;
    private final SerializationInfo info;
    private final ObjectStreamClass desc;
    
    public InteropObjectInputStream(Object obj, SerializationInfo info, Class clazz) throws IOException
    {
        this.obj = obj;
        this.info = info;
        this.desc = ObjectStreamClass.lookup(clazz);
    }
    
    @Override
    public void defaultReadObject()
    {
        byte[] primVals = (byte[])info.GetValue(desc.getName() + ":p", ikvm.runtime.Util.getInstanceTypeFromClass(cli.System.Object.class));
        Object[] objVals = (Object[])info.GetValue(desc.getName() + ":o", ikvm.runtime.Util.getInstanceTypeFromClass(cli.System.Object.class));
        desc.setPrimFieldValues(obj, primVals);
        desc.setObjFieldValues(obj, objVals);
    }

    @Override
    protected Object readObjectOverride()
    {
        throw new UnsupportedOperationException();
    }

    @Override
    public Object readUnshared()
    {
        throw new UnsupportedOperationException();
    }

    @Override
    public ObjectInputStream.GetField readFields()
    {
        throw new UnsupportedOperationException();
    }

    @Override
    public void registerValidation(ObjectInputValidation obj, int prio)
    {
        throw new UnsupportedOperationException();
    }

    @Override
    public int read()
    {
        throw new UnsupportedOperationException();
    }

    @Override
    public int read(byte[] buf, int off, int len)
    {
        throw new UnsupportedOperationException();
    }

    @Override
    public int available()
    {
        throw new UnsupportedOperationException();
    }

    @Override
    public void close()
    {
        throw new UnsupportedOperationException();
    }

    @Override
    public boolean readBoolean()
    {
        throw new UnsupportedOperationException();
    }

    @Override
    public byte readByte()
    {
        throw new UnsupportedOperationException();
    }

    @Override
    public int readUnsignedByte()
    {
        throw new UnsupportedOperationException();
    }

    @Override
    public char readChar()
    {
        throw new UnsupportedOperationException();
    }

    @Override
    public short readShort()
    {
        throw new UnsupportedOperationException();
    }

    @Override
    public int readUnsignedShort()
    {
        throw new UnsupportedOperationException();
    }

    @Override
    public int readInt()
    {
        throw new UnsupportedOperationException();
    }

    @Override
    public long readLong()
    {
        throw new UnsupportedOperationException();
    }

    @Override
    public float readFloat()
    {
        throw new UnsupportedOperationException();
    }

    @Override
    public double readDouble()
    {
        throw new UnsupportedOperationException();
    }

    @Override
    public void readFully(byte[] buf)
    {
        throw new UnsupportedOperationException();
    }

    @Override
    public void readFully(byte[] buf, int off, int len)
    {
        throw new UnsupportedOperationException();
    }

    @Override
    public int skipBytes(int len)
    {
        throw new UnsupportedOperationException();
    }

    @Deprecated
    @Override
    public String readLine()
    {
        throw new UnsupportedOperationException();
    }

    @Override
    public String readUTF()
    {
        throw new UnsupportedOperationException();
    }
}
