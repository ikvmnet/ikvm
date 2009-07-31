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
public final class InteropObjectOutputStream extends ObjectOutputStream
{
    private final Object obj;
    private final SerializationInfo info;
    private final ObjectStreamClass desc;
    
    public InteropObjectOutputStream(Object obj, SerializationInfo info, Class clazz) throws IOException
    {
        this.obj = obj;
        this.info = info;
        this.desc = ObjectStreamClass.lookup(clazz);
    }

    @Override    
    public void defaultWriteObject() throws IOException
    {
        desc.checkDefaultSerialize();
        byte[] primVals = new byte[desc.getPrimDataSize()];
        desc.getPrimFieldValues(obj, primVals);
        Object[] objVals = new Object[desc.getNumObjFields()];
        desc.getObjFieldValues(obj, objVals);
        
        // TODO consider if we should loop thru objVals to look for ghost arrays
        // TODO consider writing desc
        
        info.AddValue(desc.getName() + ":p", primVals);
        info.AddValue(desc.getName() + ":o", objVals);
    }

    @Override    
    public void close()
    {
        throw new UnsupportedOperationException();
    }

    @Override    
    public void flush()
    {
        throw new UnsupportedOperationException();
    }

    @Override    
    public PutField putFields()
    {
        throw new UnsupportedOperationException();
    }

    @Override    
    public void reset()
    {
        throw new UnsupportedOperationException();
    }

    @Override    
    protected void writeObjectOverride(Object obj)
    {
        throw new UnsupportedOperationException();
    }

    @Override    
    public void writeFields()
    {
        throw new UnsupportedOperationException();
    }

    @Override    
    public void writeUnshared(Object obj)
    {
        throw new UnsupportedOperationException();
    }

    @Override    
    public void useProtocolVersion(int version)
    {
        throw new UnsupportedOperationException();
    }

    @Override    
    public void write(byte[] buf)
    {
        throw new UnsupportedOperationException();
    }

    @Override    
    public void write(int val)
    {
        throw new UnsupportedOperationException();
    }

    @Override    
    public void write(byte[] buf, int off, int len)
    {
        throw new UnsupportedOperationException();
    }

    @Override    
    public void writeBoolean(boolean val)
    {
        throw new UnsupportedOperationException();
    }

    @Override    
    public void writeByte(int val)
    {
        throw new UnsupportedOperationException();
    }

    @Override    
    public void writeBytes(String str)
    {
        throw new UnsupportedOperationException();
    }

    @Override    
    public void writeChar(int val)
    {
        throw new UnsupportedOperationException();
    }

    @Override    
    public void writeChars(String str)
    {
        throw new UnsupportedOperationException();
    }

    @Override    
    public void writeDouble(double val)
    {
        throw new UnsupportedOperationException();
    }

    @Override    
    public void writeFloat(float val)
    {
        throw new UnsupportedOperationException();
    }

    @Override    
    public void writeInt(int val)
    {
        throw new UnsupportedOperationException();
    }

    @Override    
    public void writeLong(long val)
    {
        throw new UnsupportedOperationException();
    }

    @Override    
    public void writeShort(int val)
    {
        throw new UnsupportedOperationException();
    }

    @Override    
    public void writeUTF(String str)
    {
        throw new UnsupportedOperationException();
    }
}
