/*
  Copyright (C) 2006, 2007 Jeroen Frijters

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

import java.lang.reflect.Field;
import java.lang.reflect.ReflectHelper;
import java.util.ArrayList;

public final class Unsafe
{
    public static final int INVALID_FIELD_OFFSET = -1;
    // NOTE sun.corba.Bridge actually access this field directly (via reflection),
    // so the name must match the JDK name.
    private static final Unsafe theUnsafe = new Unsafe();
    private static final ArrayList<Field> fields = new ArrayList<Field>();

    private Unsafe() { }

    public static Unsafe getUnsafe()
    {
	Class c = sun.reflect.Reflection.getCallerClass(2);
        if(c.getClassLoader() != null)
        {
            throw new SecurityException("Unsafe");
        }
	if(c == SharedSecrets.class)
	{
	    // HACK make sure that bootstrap has occurred before SharedSecrets.getJavaLangAccess() is called.
	    theUnsafe.ensureClassInitialized(System.class);
	}
	return theUnsafe;
    }

    // NOTE we have a really lame (and slow) implementation!
    public long objectFieldOffset(Field field)
    {
	return fieldOffset(field);
    }

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
        return 0;
    }

    public int arrayIndexScale(Class c)
    {
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

    public native void throwException(Throwable t);

    public native void ensureClassInitialized(Class clazz);

    public native Object allocateInstance(Class clazz) throws InstantiationException;
}
