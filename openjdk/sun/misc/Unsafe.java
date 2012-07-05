/*
  Copyright (C) 2006-2012 Jeroen Frijters

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

package sun.misc;

import cli.System.IntPtr;
import cli.System.Runtime.InteropServices.Marshal;
import cli.System.Security.Permissions.SecurityAction;
import cli.System.Security.Permissions.SecurityPermissionAttribute;
import ikvm.lang.Internal;
import java.lang.reflect.Field;
import java.lang.reflect.Modifier;
import java.lang.reflect.ReflectHelper;
import java.security.ProtectionDomain;
import java.util.ArrayList;

public final class Unsafe
{
    public static final int INVALID_FIELD_OFFSET = -1;
    public static final int ARRAY_BYTE_BASE_OFFSET = 0;
    // NOTE sun.corba.Bridge actually access this field directly (via reflection),
    // so the name must match the JDK name.
    private static final Unsafe theUnsafe = new Unsafe();
    private static final ArrayList<Field> fields = new ArrayList<Field>();

    private Unsafe() { }

    @ikvm.internal.HasCallerID
    public static Unsafe getUnsafe()
    {
        if(ikvm.internal.CallerID.getCallerID().getCallerClassLoader() != null)
        {
            throw new SecurityException("Unsafe");
        }
        return theUnsafe;
    }

    // this is the intrinsified version of objectFieldOffset(XXX.class.getDeclaredField("xxx"))
    public long objectFieldOffset(Class c, String field)
    {
        return fieldOffset(ReflectHelper.createFieldAndMakeAccessible(c, field));
    }

    // NOTE we have a really lame (and slow) implementation!
    public long objectFieldOffset(Field field)
    {
        if(Modifier.isStatic(field.getModifiers()))
        {
            throw new IllegalArgumentException();
        }
        return fieldOffset(field);
    }

    @Deprecated
    public int fieldOffset(Field original)
    {
        Field copy = ReflectHelper.copyFieldAndMakeAccessible(original);
        synchronized(fields)
        {
            int id = fields.size();
            fields.add(copy);
            return id;
        }
    }

    public int arrayBaseOffset(Class c)
    {
        // don't change this, the Unsafe intrinsics depend on this value
        return 0;
    }

    public int arrayIndexScale(Class c)
    {
        // don't change this, the Unsafe intrinsics depend on this value
        return 1;
    }

    private static Field getField(long offset)
    {
        synchronized(fields)
        {
            return fields.get((int)offset);
        }
    }

    public boolean compareAndSwapObject(Object obj, long offset, Object expect, Object update)
    {
        if(obj instanceof Object[])
        {
            Object[] arr = (Object[])obj;
            int index = (int)offset;
            synchronized(this)
            {
                if(arr[index] == expect)
                {
                    arr[index] = update;
                    return true;
                }
                return false;
            }
        }
        else
        {
            Field field = getField(offset);
            synchronized(field)
            {
                try
                {
                    if(field.get(obj) == expect)
                    {
                        field.set(obj, update);
                        return true;
                    }
                    return false;
                }
                catch(IllegalAccessException x)
                {
                    throw (InternalError)new InternalError().initCause(x);
                }
            }
        }
    }

    public void putObjectVolatile(Object obj, long offset, Object newValue)
    {
        if(obj instanceof Object[])
        {
            synchronized(this)
            {
                ((Object[])obj)[(int)offset] = newValue;
            }
        }
        else
        {
            Field field = getField(offset);
            synchronized(field)
            {
                try
                {
                    field.set(obj, newValue);
                }
                catch(IllegalAccessException x)
                {
                    throw (InternalError)new InternalError().initCause(x);
                }
            }
        }
    }

    public void putOrderedObject(Object obj, long offset, Object newValue)
    {
        putObjectVolatile(obj, offset, newValue);
    }

    public Object getObjectVolatile(Object obj, long offset)
    {
        if(obj instanceof Object[])
        {
            synchronized(this)
            {
                return ((Object[])obj)[(int)offset];
            }
        }
        else
        {
            Field field = getField(offset);
            synchronized(field)
            {
                try
                {
                    return field.get(obj);
                }
                catch(IllegalAccessException x)
                {
                    throw (InternalError)new InternalError().initCause(x);
                }
            }
        }
    }

    public boolean compareAndSwapInt(Object obj, long offset, int expect, int update)
    {
        if(obj instanceof int[])
        {
            int[] arr = (int[])obj;
            int index = (int)offset;
            synchronized(this)
            {
                if(arr[index] == expect)
                {
                    arr[index] = update;
                    return true;
                }
                return false;
            }
        }
        else
        {
            Field field = getField(offset);
            synchronized(field)
            {
                try
                {
                    if(field.getInt(obj) == expect)
                    {
                        field.setInt(obj, update);
                        return true;
                    }
                    return false;
                }
                catch(IllegalAccessException x)
                {
                    throw (InternalError)new InternalError().initCause(x);
                }
            }
        }
    }

    public void putIntVolatile(Object obj, long offset, int newValue)
    {
        if(obj instanceof int[])
        {
            synchronized(this)
            {
                ((int[])obj)[(int)offset] = newValue;
            }
        }
        else
        {
            Field field = getField(offset);
            synchronized(field)
            {
                try
                {
                    field.setInt(obj, newValue);
                }
                catch(IllegalAccessException x)
                {
                    throw (InternalError)new InternalError().initCause(x);
                }
            }
        }
    }

    public void putOrderedInt(Object obj, long offset, int newValue)
    {
        putIntVolatile(obj, offset, newValue);
    }

    public int getIntVolatile(Object obj, long offset)
    {
        if(obj instanceof int[])
        {
            synchronized(this)
            {
                return ((int[])obj)[(int)offset];
            }
        }
        else
        {
            Field field = getField(offset);
            synchronized(field)
            {
                try
                {
                    return field.getInt(obj);
                }
                catch(IllegalAccessException x)
                {
                    throw (InternalError)new InternalError().initCause(x);
                }
            }
        }
    }

    public boolean compareAndSwapLong(Object obj, long offset, long expect, long update)
    {
        if(obj instanceof long[])
        {
            long[] arr = (long[])obj;
            int index = (int)offset;
            synchronized(this)
            {
                if(arr[index] == expect)
                {
                    arr[index] = update;
                    return true;
                }
                return false;
            }
        }
        else
        {
            Field field = getField(offset);
            synchronized(field)
            {
                try
                {
                    if(field.getLong(obj) == expect)
                    {
                        field.setLong(obj, update);
                        return true;
                    }
                    return false;
                }
                catch(IllegalAccessException x)
                {
                    throw (InternalError)new InternalError().initCause(x);
                }
            }
        }
    }

    public void putLongVolatile(Object obj, long offset, long newValue)
    {
        if(obj instanceof long[])
        {
            synchronized(this)
            {
                ((long[])obj)[(int)offset] = newValue;
            }
        }
        else
        {
            Field field = getField(offset);
            synchronized(field)
            {
                try
                {
                    field.setLong(obj, newValue);
                }
                catch(IllegalAccessException x)
                {
                    throw (InternalError)new InternalError().initCause(x);
                }
            }
        }
    }

    public void putOrderedLong(Object obj, long offset, long newValue)
    {
        putLongVolatile(obj, offset, newValue);
    }

    public long getLongVolatile(Object obj, long offset)
    {
        if(obj instanceof long[])
        {
            synchronized(this)
            {
                return ((long[])obj)[(int)offset];
            }
        }
        else
        {
            Field field = getField(offset);
            synchronized(field)
            {
                try
                {
                    return field.getLong(obj);
                }
                catch(IllegalAccessException x)
                {
                    throw (InternalError)new InternalError().initCause(x);
                }
            }
        }
    }

    public void putBoolean(Object obj, long offset, boolean newValue)
    {
        if (obj instanceof boolean[])
        {
            ((boolean[])obj)[(int)offset] = newValue;
        }
        else
        {
            try
            {
                getField(offset).setBoolean(obj, newValue);
            }
            catch (IllegalAccessException x)
            {
                throw (InternalError)new InternalError().initCause(x);
            }
        }
    }

    public boolean getBoolean(Object obj, long offset)
    {
        if (obj instanceof boolean[])
        {
            return ((boolean[])obj)[(int)offset];
        }
        else
        {
            try
            {
                return getField(offset).getBoolean(obj);
            }
            catch (IllegalAccessException x)
            {
                throw (InternalError)new InternalError().initCause(x);
            }
        }
    }

    public void putByte(Object obj, long offset, byte newValue)
    {
        if (obj instanceof byte[])
        {
            ((byte[])obj)[(int)offset] = newValue;
        }
        else
        {
            try
            {
                getField(offset).setByte(obj, newValue);
            }
            catch (IllegalAccessException x)
            {
                throw (InternalError)new InternalError().initCause(x);
            }
        }
    }

    public byte getByte(Object obj, long offset)
    {
        if (obj instanceof byte[])
        {
            return ((byte[])obj)[(int)offset];
        }
        else
        {
            try
            {
                return getField(offset).getByte(obj);
            }
            catch (IllegalAccessException x)
            {
                throw (InternalError)new InternalError().initCause(x);
            }
        }
    }

    public void putChar(Object obj, long offset, char newValue)
    {
        if (obj instanceof char[])
        {
            ((char[])obj)[(int)offset] = newValue;
        }
        else
        {
            try
            {
                getField(offset).setChar(obj, newValue);
            }
            catch (IllegalAccessException x)
            {
                throw (InternalError)new InternalError().initCause(x);
            }
        }
    }

    public char getChar(Object obj, long offset)
    {
        if (obj instanceof char[])
        {
            return ((char[])obj)[(int)offset];
        }
        else
        {
            try
            {
                return getField(offset).getChar(obj);
            }
            catch (IllegalAccessException x)
            {
                throw (InternalError)new InternalError().initCause(x);
            }
        }
    }

    public void putShort(Object obj, long offset, short newValue)
    {
        if (obj instanceof short[])
        {
            ((short[])obj)[(int)offset] = newValue;
        }
        else
        {
            try
            {
                getField(offset).setShort(obj, newValue);
            }
            catch (IllegalAccessException x)
            {
                throw (InternalError)new InternalError().initCause(x);
            }
        }
    }

    public short getShort(Object obj, long offset)
    {
        if (obj instanceof short[])
        {
            return ((short[])obj)[(int)offset];
        }
        else
        {
            try
            {
                return getField(offset).getShort(obj);
            }
            catch (IllegalAccessException x)
            {
                throw (InternalError)new InternalError().initCause(x);
            }
        }
    }

    public void putInt(Object obj, long offset, int newValue)
    {
        if (obj instanceof int[])
        {
            ((int[])obj)[(int)offset] = newValue;
        }
        else
        {
            try
            {
                getField(offset).setInt(obj, newValue);
            }
            catch (IllegalAccessException x)
            {
                throw (InternalError)new InternalError().initCause(x);
            }
        }
    }

    public int getInt(Object obj, long offset)
    {
        if (obj instanceof int[])
        {
            return ((int[])obj)[(int)offset];
        }
        else
        {
            try
            {
                return getField(offset).getInt(obj);
            }
            catch (IllegalAccessException x)
            {
                throw (InternalError)new InternalError().initCause(x);
            }
        }
    }

    public void putFloat(Object obj, long offset, float newValue)
    {
        if (obj instanceof float[])
        {
            ((float[])obj)[(int)offset] = newValue;
        }
        else
        {
            try
            {
                getField(offset).setFloat(obj, newValue);
            }
            catch (IllegalAccessException x)
            {
                throw (InternalError)new InternalError().initCause(x);
            }
        }
    }

    public float getFloat(Object obj, long offset)
    {
        if (obj instanceof float[])
        {
            return ((float[])obj)[(int)offset];
        }
        else
        {
            try
            {
                return getField(offset).getFloat(obj);
            }
            catch (IllegalAccessException x)
            {
                throw (InternalError)new InternalError().initCause(x);
            }
        }
    }

    public void putLong(Object obj, long offset, long newValue)
    {
        if (obj instanceof long[])
        {
            ((long[])obj)[(int)offset] = newValue;
        }
        else
        {
            try
            {
                getField(offset).setLong(obj, newValue);
            }
            catch (IllegalAccessException x)
            {
                throw (InternalError)new InternalError().initCause(x);
            }
        }
    }

    public long getLong(Object obj, long offset)
    {
        if (obj instanceof long[])
        {
            return ((long[])obj)[(int)offset];
        }
        else
        {
            try
            {
                return getField(offset).getLong(obj);
            }
            catch (IllegalAccessException x)
            {
                throw (InternalError)new InternalError().initCause(x);
            }
        }
    }

    public void putDouble(Object obj, long offset, double newValue)
    {
        if (obj instanceof double[])
        {
            ((double[])obj)[(int)offset] = newValue;
        }
        else
        {
            try
            {
                getField(offset).setDouble(obj, newValue);
            }
            catch (IllegalAccessException x)
            {
                throw (InternalError)new InternalError().initCause(x);
            }
        }
    }

    public double getDouble(Object obj, long offset)
    {
        if (obj instanceof double[])
        {
            return ((double[])obj)[(int)offset];
        }
        else
        {
            try
            {
                return getField(offset).getDouble(obj);
            }
            catch (IllegalAccessException x)
            {
                throw (InternalError)new InternalError().initCause(x);
            }
        }
    }

    public void putObject(Object obj, long offset, Object newValue)
    {
        if (obj instanceof Object[])
        {
            ((Object[])obj)[(int)offset] = newValue;
        }
        else
        {
            try
            {
                getField(offset).set(obj, newValue);
            }
            catch (IllegalAccessException x)
            {
                throw (InternalError)new InternalError().initCause(x);
            }
        }
    }

    public Object getObject(Object obj, long offset)
    {
        if (obj instanceof Object[])
        {
            return ((Object[])obj)[(int)offset];
        }
        else
        {
            try
            {
                return getField(offset).get(obj);
            }
            catch (IllegalAccessException x)
            {
                throw (InternalError)new InternalError().initCause(x);
            }
        }
    }

    @Deprecated
    public int getInt(Object o, int offset)
    {
        return getInt(o, (long)offset);
    }

    @Deprecated
    public void putInt(Object o, int offset, int x)
    {
        putInt(o, (long)offset, x);
    }

    @Deprecated
    public Object getObject(Object o, int offset)
    {
        return getObject(o, (long)offset);
    }

    @Deprecated
    public void putObject(Object o, int offset, Object x)
    {
        putObject(o, (long)offset, x);
    }

    @Deprecated
    public boolean getBoolean(Object o, int offset)
    {
        return getBoolean(o, (long)offset);
    }

    @Deprecated
    public void putBoolean(Object o, int offset, boolean x)
    {
        putBoolean(o, (long)offset, x);
    }

    @Deprecated
    public byte getByte(Object o, int offset)
    {
        return getByte(o, (long)offset);
    }

    @Deprecated
    public void putByte(Object o, int offset, byte x)
    {
        putByte(o, (long)offset, x);
    }

    @Deprecated
    public short getShort(Object o, int offset)
    {
        return getShort(o, (long)offset);
    }

    @Deprecated
    public void putShort(Object o, int offset, short x)
    {
        putShort(o, (long)offset, x);
    }

    @Deprecated
    public char getChar(Object o, int offset)
    {
        return getChar(o, (long)offset);
    }

    @Deprecated
    public void putChar(Object o, int offset, char x)
    {
        putChar(o, (long)offset, x);
    }

    @Deprecated
    public long getLong(Object o, int offset)
    {
        return getLong(o, (long)offset);
    }

    @Deprecated
    public void putLong(Object o, int offset, long x)
    {
        putLong(o, (long)offset, x);
    }

    @Deprecated
    public float getFloat(Object o, int offset)
    {
        return getFloat(o, (long)offset);
    }

    @Deprecated
    public void putFloat(Object o, int offset, float x)
    {
        putFloat(o, (long)offset, x);
    }

    @Deprecated
    public double getDouble(Object o, int offset)
    {
        return getDouble(o, (long)offset);
    }

    @Deprecated
    public void putDouble(Object o, int offset, double x)
    {
        putDouble(o, (long)offset, x);
    }

    public native void throwException(Throwable t);

    public native void ensureClassInitialized(Class clazz);

    @SecurityPermissionAttribute.Annotation(value = SecurityAction.__Enum.LinkDemand, SerializationFormatter = true)
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    public native Object allocateInstance(Class clazz) throws InstantiationException;

    public int addressSize()
    {
        return IntPtr.get_Size();
    }

    public int pageSize()
    {
        return 4096;
    }

    // The really unsafe methods start here. They are all have a LinkDemand for unmanaged code.

    @SecurityPermissionAttribute.Annotation(value = SecurityAction.__Enum.LinkDemand, UnmanagedCode = true)
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    public long allocateMemory(long bytes)
    {
        try
        {
            if (false) throw new cli.System.OutOfMemoryException();
            return Marshal.AllocHGlobal(IntPtr.op_Explicit(bytes)).ToInt64();
        }
        catch (cli.System.OutOfMemoryException x)
        {
            throw new OutOfMemoryError(x.get_Message());
        }
    }

    @SecurityPermissionAttribute.Annotation(value = SecurityAction.__Enum.LinkDemand, UnmanagedCode = true)
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    public void freeMemory(long address)
    {
        Marshal.FreeHGlobal(IntPtr.op_Explicit(address));
    }

    @SecurityPermissionAttribute.Annotation(value = SecurityAction.__Enum.LinkDemand, UnmanagedCode = true)
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    public void setMemory(long address, long bytes, byte value)
    {
        while (bytes-- > 0)
        {
            putByte(address++, value);
        }
    }

    @SecurityPermissionAttribute.Annotation(value = SecurityAction.__Enum.LinkDemand, UnmanagedCode = true)
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    public void copyMemory(long srcAddress, long destAddress, long bytes)
    {
	while (bytes-- > 0)
	{
	    putByte(destAddress++, getByte(srcAddress++));
	}
    }
    
    @SecurityPermissionAttribute.Annotation(value = SecurityAction.__Enum.LinkDemand, UnmanagedCode = true)
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    public void copyMemory(Object srcBase, long srcOffset, Object destBase, long destOffset, long bytes)
    {
        if (srcBase == null)
        {
            if (destBase instanceof byte[])
            {
                cli.System.Runtime.InteropServices.Marshal.Copy(IntPtr.op_Explicit(srcOffset), (byte[])destBase, (int)destOffset, (int)bytes);
            }
            else if (destBase instanceof boolean[])
            {
                byte[] tmp = new byte[(int)bytes];
                copyMemory(srcBase, srcOffset, tmp, 0, bytes);
                copyMemory(tmp, 0, destBase, destOffset, bytes);
            }
            else if (destBase instanceof short[])
            {
                cli.System.Runtime.InteropServices.Marshal.Copy(IntPtr.op_Explicit(srcOffset), (short[])destBase, (int)(destOffset >> 1), (int)(bytes >> 1));
            }
            else if (destBase instanceof char[])
            {
                cli.System.Runtime.InteropServices.Marshal.Copy(IntPtr.op_Explicit(srcOffset), (char[])destBase, (int)(destOffset >> 1), (int)(bytes >> 1));
            }
            else if (destBase instanceof int[])
            {
                cli.System.Runtime.InteropServices.Marshal.Copy(IntPtr.op_Explicit(srcOffset), (int[])destBase, (int)(destOffset >> 2), (int)(bytes >> 2));
            }
            else if (destBase instanceof float[])
            {
                cli.System.Runtime.InteropServices.Marshal.Copy(IntPtr.op_Explicit(srcOffset), (float[])destBase, (int)(destOffset >> 2), (int)(bytes >> 2));
            }
            else if (destBase instanceof long[])
            {
                cli.System.Runtime.InteropServices.Marshal.Copy(IntPtr.op_Explicit(srcOffset), (long[])destBase, (int)(destOffset >> 3), (int)(bytes >> 3));
            }
            else if (destBase instanceof double[])
            {
                cli.System.Runtime.InteropServices.Marshal.Copy(IntPtr.op_Explicit(srcOffset), (double[])destBase, (int)(destOffset >> 3), (int)(bytes >> 3));
            }
            else if (destBase == null)
            {
                copyMemory(srcOffset, destOffset, bytes);
            }
            else
            {
                throw new IllegalArgumentException();
            }
        }
        else if (srcBase instanceof cli.System.Array && destBase instanceof cli.System.Array)
        {
            cli.System.Buffer.BlockCopy((cli.System.Array)srcBase, (int)srcOffset, (cli.System.Array)destBase, (int)destOffset, (int)bytes);
        }
        else
        {
            if (srcBase instanceof byte[])
            {
                cli.System.Runtime.InteropServices.Marshal.Copy((byte[])srcBase, (int)srcOffset, IntPtr.op_Explicit(destOffset), (int)bytes);
            }
            else if (srcBase instanceof boolean[])
            {
                byte[] tmp = new byte[(int)bytes];
                copyMemory(srcBase, srcOffset, tmp, 0, bytes);
                copyMemory(tmp, 0, destBase, destOffset, bytes);
            }
            else if (srcBase instanceof short[])
            {
                cli.System.Runtime.InteropServices.Marshal.Copy((short[])srcBase, (int)(srcOffset >> 1), IntPtr.op_Explicit(destOffset), (int)(bytes >> 1));
            }
            else if (srcBase instanceof char[])
            {
                cli.System.Runtime.InteropServices.Marshal.Copy((char[])srcBase, (int)(srcOffset >> 1), IntPtr.op_Explicit(destOffset), (int)(bytes >> 1));
            }
            else if (srcBase instanceof int[])
            {
                cli.System.Runtime.InteropServices.Marshal.Copy((int[])srcBase, (int)(srcOffset >> 2), IntPtr.op_Explicit(destOffset), (int)(bytes >> 2));
            }
            else if (srcBase instanceof float[])
            {
                cli.System.Runtime.InteropServices.Marshal.Copy((float[])srcBase, (int)(srcOffset >> 2), IntPtr.op_Explicit(destOffset), (int)(bytes >> 2));
            }
            else if (srcBase instanceof long[])
            {
                cli.System.Runtime.InteropServices.Marshal.Copy((long[])srcBase, (int)(srcOffset >> 3), IntPtr.op_Explicit(destOffset), (int)(bytes >> 3));
            }
            else if (srcBase instanceof double[])
            {
                cli.System.Runtime.InteropServices.Marshal.Copy((double[])srcBase, (int)(srcOffset >> 3), IntPtr.op_Explicit(destOffset), (int)(bytes >> 3));
            }
            else
            {
                throw new IllegalArgumentException();
            }
        }
    }

    @SecurityPermissionAttribute.Annotation(value = SecurityAction.__Enum.LinkDemand, UnmanagedCode = true)
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    public byte getByte(long address)
    {
	return cli.System.Runtime.InteropServices.Marshal.ReadByte(IntPtr.op_Explicit(address));
    }

    @SecurityPermissionAttribute.Annotation(value = SecurityAction.__Enum.LinkDemand, UnmanagedCode = true)
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    public void putByte(long address, byte x)
    {
	cli.System.Runtime.InteropServices.Marshal.WriteByte(IntPtr.op_Explicit(address), x);
    }

    @SecurityPermissionAttribute.Annotation(value = SecurityAction.__Enum.LinkDemand, UnmanagedCode = true)
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    public short getShort(long address)
    {
	return cli.System.Runtime.InteropServices.Marshal.ReadInt16(IntPtr.op_Explicit(address));
    }

    @SecurityPermissionAttribute.Annotation(value = SecurityAction.__Enum.LinkDemand, UnmanagedCode = true)
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    public void putShort(long address, short x)
    {
	cli.System.Runtime.InteropServices.Marshal.WriteInt16(IntPtr.op_Explicit(address), x);
    }

    @SecurityPermissionAttribute.Annotation(value = SecurityAction.__Enum.LinkDemand, UnmanagedCode = true)
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    public char getChar(long address)
    {
        return (char)cli.System.Runtime.InteropServices.Marshal.ReadInt16(IntPtr.op_Explicit(address));
    }

    @SecurityPermissionAttribute.Annotation(value = SecurityAction.__Enum.LinkDemand, UnmanagedCode = true)
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    public void putChar(long address, char x)
    {
        cli.System.Runtime.InteropServices.Marshal.WriteInt16(IntPtr.op_Explicit(address), (short)x);
    }

    @SecurityPermissionAttribute.Annotation(value = SecurityAction.__Enum.LinkDemand, UnmanagedCode = true)
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    public int getInt(long address)
    {
	return cli.System.Runtime.InteropServices.Marshal.ReadInt32(IntPtr.op_Explicit(address));
    }

    @SecurityPermissionAttribute.Annotation(value = SecurityAction.__Enum.LinkDemand, UnmanagedCode = true)
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    public void putInt(long address, int x)
    {
	cli.System.Runtime.InteropServices.Marshal.WriteInt32(IntPtr.op_Explicit(address), x);
    }

    @SecurityPermissionAttribute.Annotation(value = SecurityAction.__Enum.LinkDemand, UnmanagedCode = true)
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    public long getLong(long address)
    {
	return cli.System.Runtime.InteropServices.Marshal.ReadInt64(IntPtr.op_Explicit(address));
    }

    @SecurityPermissionAttribute.Annotation(value = SecurityAction.__Enum.LinkDemand, UnmanagedCode = true)
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    public void putLong(long address, long x)
    {
	cli.System.Runtime.InteropServices.Marshal.WriteInt64(IntPtr.op_Explicit(address), x);
    }

    @SecurityPermissionAttribute.Annotation(value = SecurityAction.__Enum.LinkDemand, UnmanagedCode = true)
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    public long getAddress(long address)
    {
	return cli.System.Runtime.InteropServices.Marshal.ReadIntPtr(IntPtr.op_Explicit(address)).ToInt64();
    }

    @SecurityPermissionAttribute.Annotation(value = SecurityAction.__Enum.LinkDemand, UnmanagedCode = true)
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    public void putAddress(long address, long x)
    {
	cli.System.Runtime.InteropServices.Marshal.WriteIntPtr(IntPtr.op_Explicit(address), IntPtr.op_Explicit(x));
    }

    @SecurityPermissionAttribute.Annotation(value = SecurityAction.__Enum.LinkDemand, UnmanagedCode = true)
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    public float getFloat(long address)
    {
        return Float.intBitsToFloat(getInt(address));
    }

    @SecurityPermissionAttribute.Annotation(value = SecurityAction.__Enum.LinkDemand, UnmanagedCode = true)
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    public void putFloat(long address, float x)
    {
        putInt(address, Float.floatToIntBits(x));
    }

    @SecurityPermissionAttribute.Annotation(value = SecurityAction.__Enum.LinkDemand, UnmanagedCode = true)
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    public double getDouble(long address)
    {
        return Double.longBitsToDouble(getLong(address));
    }

    @SecurityPermissionAttribute.Annotation(value = SecurityAction.__Enum.LinkDemand, UnmanagedCode = true)
    @cli.System.Security.SecurityCriticalAttribute.Annotation
    public void putDouble(long address, double x)
    {
        putLong(address, Double.doubleToLongBits(x));
    }
    
    public int getLoadAverage(double[] loadavg, int nelems)
    {
        return -1;
    }

    public void park(boolean isAbsolute, long time)
    {
        if (isAbsolute)
        {
            java.util.concurrent.locks.LockSupport.parkUntil(time);
        }
        else
        {
            java.util.concurrent.locks.LockSupport.parkNanos(time);
        }
    }

    public void unpark(Object thread)
    {
        java.util.concurrent.locks.LockSupport.unpark((Thread)thread);
    }

    public Object staticFieldBase(Field f)
    {
        return null;
    }

    public native Class defineClass(String name, byte[] buf, int offset, int length, ClassLoader cl, ProtectionDomain pd);

    public void monitorEnter(Object o)
    {
        cli.System.Threading.Monitor.Enter(o);
    }

    public void monitorExit(Object o)
    {
        cli.System.Threading.Monitor.Exit(o);
    }

    public boolean tryMonitorEnter(Object o)
    {
        return cli.System.Threading.Monitor.TryEnter(o);
    }
}
