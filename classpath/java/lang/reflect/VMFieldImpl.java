/*
  Copyright (C) 2005 Jeroen Frijters

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
package java.lang.reflect;

import cli.System.Diagnostics.StackFrame;
import gnu.java.lang.reflect.VMField;
import ikvm.lang.CIL;

abstract class VMFieldImpl extends VMField
{
    private static native Object GetValue(Object fieldCookie, Object o);
    private static native void SetValue(Object fieldCookie, Object o, Object value);
    private static native int GetModifiers(Object fieldCookie);
    private static native String GetName(Object fieldCookie);
    private static native Object GetFieldType(Object fieldCookie);
    private static native boolean isSamePackage(Class a, Class b);
    private static native void RunClassInit(Class clazz);

    static void checkAccess(int modifiers, Object o, Class declaringClass, Class caller) throws IllegalAccessException
    {
        // when we're invoking a constructor, modifiers will not be static, but o will be null.
        Class actualClass = Modifier.isStatic(modifiers) || o == null ? declaringClass : o.getClass();
        boolean declaringClassIsPublic = (Method.GetRealModifiers(declaringClass) & Modifier.PUBLIC) != 0;
        if((!Modifier.isPublic(modifiers) || !declaringClassIsPublic) && declaringClass != caller)
        {
            // if the caller is a global method, the class returned will be null
            if(caller == null)
            {
                throw new IllegalAccessException();
            }
            if(Modifier.isProtected(modifiers) && actualClass.isAssignableFrom(caller))
            {
            }
            else if(!isSamePackage(declaringClass, caller) || Modifier.isPrivate(modifiers))
            {
                throw new IllegalAccessException("Class " + caller.getName() +
                    " can not access a member of class " + declaringClass.getName() +
                    " with modifiers \"" + Modifier.toString(modifiers & (Modifier.PRIVATE | Modifier.PROTECTED)) + "\"");
            }
        }
    }

    static Field newField(Class declaringClass, Object fieldCookie)
    {
        Class type = (Class)GetFieldType(fieldCookie);
        if (type == Boolean.TYPE)
            return new BooleanFieldImpl(declaringClass, fieldCookie).newField();
        else if (type == Byte.TYPE)
            return new ByteFieldImpl(declaringClass, fieldCookie).newField();
        else if (type == Character.TYPE)
            return new CharFieldImpl(declaringClass, fieldCookie).newField();
        else if (type == Short.TYPE)
            return new ShortFieldImpl(declaringClass, fieldCookie).newField();
        else if (type == Integer.TYPE)
            return new IntFieldImpl(declaringClass, fieldCookie).newField();
        else if (type == Float.TYPE)
            return new FloatFieldImpl(declaringClass, fieldCookie).newField();
        else if (type == Long.TYPE)
            return new LongFieldImpl(declaringClass, fieldCookie).newField();
        else if (type == Double.TYPE)
            return new DoubleFieldImpl(declaringClass, fieldCookie).newField();
        else
            return new ObjectFieldImpl(declaringClass, fieldCookie).newField();
    }

    VMFieldImpl(Class declaringClass, Object fieldCookie)
    {
        this.declaringClass = declaringClass;
        this.fieldCookie = fieldCookie;
        this.modifiers = GetModifiers(fieldCookie);
        isPublic = Modifier.isPublic(modifiers)
            && Modifier.isPublic(Method.GetRealModifiers(declaringClass));
    }

    public final Field newField()
    {
        return new Field(this);
    }

    public final void checkAccess(Object o, Class caller) throws IllegalAccessException
    {
        checkAccess(modifiers, o, declaringClass, caller);
    }

    public final String getName()
    {
        return GetName(fieldCookie);
    }

    public final Class getType()
    {
        return (Class)GetFieldType(fieldCookie);
    }

    final void checkObject(Object obj)
    {
        if(!Modifier.isStatic(modifiers))
        {
            if(!declaringClass.isAssignableFrom(obj.getClass()))
            {
                throw new IllegalArgumentException();
            }
        }
    }

    final void checkWrite(boolean accessible) throws IllegalAccessException
    {
        if(Modifier.isFinal(modifiers))
        {
            // Starting with JDK 1.5, it is legal to change final instance fields
            // (see JSR-133), but they have to be made "accessible".
            if(Modifier.isStatic(modifiers) || !accessible)
            {
                // even though the field access will fail with an IllegalAccessException,
                // we run the <clinit> for declaringClass for compatibility with the JDK.
                RunClassInit(declaringClass);
                throw new IllegalAccessException("Field is final");
            }
        }
    }

    final Object getImpl(Object obj)
    {
        checkObject(obj);
        return GetValue(fieldCookie, obj);
    }

    final void setImpl(Object obj, Object val, boolean accessible) throws IllegalAccessException
    {
        checkWrite(accessible);
        checkObject(obj);
        SetValue(fieldCookie, obj, val);
    }

    public boolean getBoolean(Object obj)
    {
        throw new IllegalArgumentException();
    }
    public byte getByte(Object obj)
    {
        throw new IllegalArgumentException();
    }
    public char getChar(Object obj)
    {
        throw new IllegalArgumentException();
    }
    public short getShort(Object obj)
    {
        throw new IllegalArgumentException();
    }
    public int getInt(Object obj)
    {
        throw new IllegalArgumentException();
    }
    public float getFloat(Object obj)
    {
        throw new IllegalArgumentException();
    }
    public long getLong(Object obj)
    {
        throw new IllegalArgumentException();
    }
    public double getDouble(Object obj)
    {
        throw new IllegalArgumentException();
    }

    public void setBoolean(Object obj, boolean val, boolean accessible) throws IllegalAccessException
    {
        throw new IllegalArgumentException();
    }
    public void setByte(Object obj, byte val, boolean accessible) throws IllegalAccessException
    {
        throw new IllegalArgumentException();
    }
    public void setChar(Object obj, char val, boolean accessible) throws IllegalAccessException
    {
        throw new IllegalArgumentException();
    }
    public void setShort(Object obj, short val, boolean accessible) throws IllegalAccessException
    {
        throw new IllegalArgumentException();
    }
    public void setInt(Object obj, int val, boolean accessible) throws IllegalAccessException
    {
        throw new IllegalArgumentException();
    }
    public void setFloat(Object obj, float val, boolean accessible) throws IllegalAccessException
    {
        throw new IllegalArgumentException();
    }
    public void setLong(Object obj, long val, boolean accessible) throws IllegalAccessException
    {
        throw new IllegalArgumentException();
    }
    public void setDouble(Object obj, double val, boolean accessible) throws IllegalAccessException
    {
        throw new IllegalArgumentException();
    }
}

final class ObjectFieldImpl extends VMFieldImpl
{
    ObjectFieldImpl(Class declaringClass, Object fieldCookie)
    {
        super(declaringClass, fieldCookie);
    }

    public Object get(Object obj)
    {
        return getImpl(obj);
    }

    public void set(Object obj, Object val, boolean accessible) throws IllegalAccessException
    {
        setImpl(obj, val, accessible);
    }
}

final class BooleanFieldImpl extends VMFieldImpl
{
    BooleanFieldImpl(Class declaringClass, Object fieldCookie)
    {
        super(declaringClass, fieldCookie);
    }

    public Object get(Object obj)
    {
        return getBoolean(obj) ? Boolean.TRUE : Boolean.FALSE;
    }

    public boolean getBoolean(Object obj)
    {
        return CIL.unbox_boolean(getImpl(obj));
    }

    public void set(Object obj, Object val, boolean accessible) throws IllegalAccessException
    {
        if(! (val instanceof Boolean))
            throw new IllegalArgumentException();
        setBoolean(obj, ((Boolean)val).booleanValue(), accessible);
    }

    public void setBoolean(Object obj, boolean val, boolean accessible) throws IllegalAccessException
    {
        setImpl(obj, CIL.box_boolean(val), accessible);
    }
}

final class ByteFieldImpl extends VMFieldImpl
{
    ByteFieldImpl(Class declaringClass, Object fieldCookie)
    {
        super(declaringClass, fieldCookie);
    }

    public Object get(Object obj)
    {
        return new Byte(getByte(obj));
    }

    public byte getByte(Object obj)
    {
        return CIL.unbox_byte(getImpl(obj));
    }

    public short getShort(Object obj)
    {
        return getByte(obj);
    }

    public int getInt(Object obj)
    {
        return getByte(obj);
    }

    public float getFloat(Object obj)
    {
        return getByte(obj);
    }

    public long getLong(Object obj)
    {
        return getByte(obj);
    }

    public double getDouble(Object obj)
    {
        return getByte(obj);
    }

    public void set(Object obj, Object val, boolean accessible) throws IllegalAccessException
    {
        if(! (val instanceof Byte))
            throw new IllegalArgumentException();
        setByte(obj, ((Byte)val).byteValue(), accessible);
    }

    public void setByte(Object obj, byte val, boolean accessible) throws IllegalAccessException
    {
        setImpl(obj, CIL.box_byte(val), accessible);
    }
}

final class CharFieldImpl extends VMFieldImpl
{
    CharFieldImpl(Class declaringClass, Object fieldCookie)
    {
        super(declaringClass, fieldCookie);
    }

    public Object get(Object obj)
    {
        return new Character(getChar(obj));
    }

    public char getChar(Object obj)
    {
        return CIL.unbox_char(getImpl(obj));
    }

    public int getInt(Object obj)
    {
        return getChar(obj);
    }

    public float getFloat(Object obj)
    {
        return getChar(obj);
    }

    public long getLong(Object obj)
    {
        return getChar(obj);
    }

    public double getDouble(Object obj)
    {
        return getChar(obj);
    }

    public void set(Object obj, Object val, boolean accessible) throws IllegalAccessException
    {
        if(! (val instanceof Character))
            throw new IllegalArgumentException();
        setChar(obj, ((Character)val).charValue(), accessible);
    }

    public void setChar(Object obj, char val, boolean accessible) throws IllegalAccessException
    {
        setImpl(obj, CIL.box_char(val), accessible);
    }
}

final class ShortFieldImpl extends VMFieldImpl
{
    ShortFieldImpl(Class declaringClass, Object fieldCookie)
    {
        super(declaringClass, fieldCookie);
    }

    public Object get(Object obj)
    {
        return new Short(getShort(obj));
    }

    public short getShort(Object obj)
    {
        return CIL.unbox_short(getImpl(obj));
    }

    public int getInt(Object obj)
    {
        return getShort(obj);
    }

    public float getFloat(Object obj)
    {
        return getShort(obj);
    }

    public long getLong(Object obj)
    {
        return getShort(obj);
    }

    public double getDouble(Object obj)
    {
        return getShort(obj);
    }

    public void set(Object obj, Object val, boolean accessible) throws IllegalAccessException
    {
        if(! (val instanceof Short
            || val instanceof Byte))
            throw new IllegalArgumentException();
        setShort(obj, ((Number)val).shortValue(), accessible);
    }

    public void setShort(Object obj, short val, boolean accessible) throws IllegalAccessException
    {
        setImpl(obj, CIL.box_short(val), accessible);
    }

    public void setByte(Object obj, byte val, boolean accessible) throws IllegalAccessException
    {
        setShort(obj, val, accessible);
    }
}

final class IntFieldImpl extends VMFieldImpl
{
    IntFieldImpl(Class declaringClass, Object fieldCookie)
    {
        super(declaringClass, fieldCookie);
    }

    public Object get(Object obj)
    {
        return new Integer(getInt(obj));
    }

    public int getInt(Object obj)
    {
        return CIL.unbox_int(getImpl(obj));
    }

    public float getFloat(Object obj)
    {
        return getInt(obj);
    }

    public long getLong(Object obj)
    {
        return getInt(obj);
    }

    public double getDouble(Object obj)
    {
        return getInt(obj);
    }

    public void set(Object obj, Object val, boolean accessible) throws IllegalAccessException
    {
        if (val instanceof Integer
            || val instanceof Byte
            || val instanceof Short)
            setInt(obj, ((Number)val).intValue(), accessible);
        else if (val instanceof Character)
            setInt(obj, ((Character)val).charValue(), accessible);
        else
            throw new IllegalArgumentException();
    }

    public void setInt(Object obj, int val, boolean accessible) throws IllegalAccessException
    {
        setImpl(obj, CIL.box_int(val), accessible);
    }

    public void setByte(Object obj, byte val, boolean accessible) throws IllegalAccessException
    {
        setInt(obj, val, accessible);
    }

    public void setChar(Object obj, char val, boolean accessible) throws IllegalAccessException
    {
        setInt(obj, val, accessible);
    }

    public void setShort(Object obj, short val, boolean accessible) throws IllegalAccessException
    {
        setInt(obj, val, accessible);
    }
}

final class FloatFieldImpl extends VMFieldImpl
{
    FloatFieldImpl(Class declaringClass, Object fieldCookie)
    {
        super(declaringClass, fieldCookie);
    }

    public Object get(Object obj)
    {
        return new Float(getFloat(obj));
    }

    public float getFloat(Object obj)
    {
        return CIL.unbox_float(getImpl(obj));
    }

    public double getDouble(Object obj)
    {
        return getFloat(obj);
    }

    public void set(Object obj, Object val, boolean accessible) throws IllegalAccessException
    {
        if (val instanceof Float
            || val instanceof Byte
            || val instanceof Short
            || val instanceof Integer
            || val instanceof Long)
            setFloat(obj, ((Number)val).floatValue(), accessible);
        else if (val instanceof Character)
            setFloat(obj, ((Character)val).charValue(), accessible);
        else
            throw new IllegalArgumentException();
    }

    public void setFloat(Object obj, float val, boolean accessible) throws IllegalAccessException
    {
        setImpl(obj, CIL.box_float(val), accessible);
    }

    public void setByte(Object obj, byte val, boolean accessible) throws IllegalAccessException
    {
        setFloat(obj, val, accessible);
    }

    public void setChar(Object obj, char val, boolean accessible) throws IllegalAccessException
    {
        setFloat(obj, val, accessible);
    }

    public void setShort(Object obj, short val, boolean accessible) throws IllegalAccessException
    {
        setFloat(obj, val, accessible);
    }

    public void setInt(Object obj, int val, boolean accessible) throws IllegalAccessException
    {
        setFloat(obj, val, accessible);
    }

    public void setLong(Object obj, long val, boolean accessible) throws IllegalAccessException
    {
        setFloat(obj, val, accessible);
    }
}

final class LongFieldImpl extends VMFieldImpl
{
    LongFieldImpl(Class declaringClass, Object fieldCookie)
    {
        super(declaringClass, fieldCookie);
    }

    public Object get(Object obj)
    {
        return new Long(getLong(obj));
    }

    public long getLong(Object obj)
    {
        return CIL.unbox_long(getImpl(obj));
    }

    public float getFloat(Object obj)
    {
        return getLong(obj);
    }

    public double getDouble(Object obj)
    {
        return getLong(obj);
    }

    public void set(Object obj, Object val, boolean accessible) throws IllegalAccessException
    {
        if (val instanceof Long
            || val instanceof Byte
            || val instanceof Short
            || val instanceof Integer)
            setLong(obj, ((Number)val).longValue(), accessible);
        else if (val instanceof Character)
            setLong(obj, ((Character)val).charValue(), accessible);
        else
            throw new IllegalArgumentException();
    }

    public void setLong(Object obj, long val, boolean accessible) throws IllegalAccessException
    {
        setImpl(obj, CIL.box_long(val), accessible);
    }

    public void setByte(Object obj, byte val, boolean accessible) throws IllegalAccessException
    {
        setLong(obj, val, accessible);
    }

    public void setChar(Object obj, char val, boolean accessible) throws IllegalAccessException
    {
        setLong(obj, val, accessible);
    }

    public void setShort(Object obj, short val, boolean accessible) throws IllegalAccessException
    {
        setLong(obj, val, accessible);
    }

    public void setInt(Object obj, int val, boolean accessible) throws IllegalAccessException
    {
        setLong(obj, val, accessible);
    }
}

final class DoubleFieldImpl extends VMFieldImpl
{
    DoubleFieldImpl(Class declaringClass, Object fieldCookie)
    {
        super(declaringClass, fieldCookie);
    }

    public Object get(Object obj)
    {
        return new Double(getDouble(obj));
    }

    public double getDouble(Object obj)
    {
        return CIL.unbox_double(getImpl(obj));
    }

    public void set(Object obj, Object val, boolean accessible) throws IllegalAccessException
    {
        if (val instanceof Double
            || val instanceof Byte
            || val instanceof Short
            || val instanceof Integer
            || val instanceof Float
            || val instanceof Long)
            setDouble(obj, ((Number)val).doubleValue(), accessible);
        else if (val instanceof Character)
            setDouble(obj, ((Character)val).charValue(), accessible);
        else
            throw new IllegalArgumentException();
    }

    public void setDouble(Object obj, double val, boolean accessible) throws IllegalAccessException
    {
        setImpl(obj, CIL.box_double(val), accessible);
    }

    public void setByte(Object obj, byte val, boolean accessible) throws IllegalAccessException
    {
        setDouble(obj, val, accessible);
    }

    public void setChar(Object obj, char val, boolean accessible) throws IllegalAccessException
    {
        setDouble(obj, val, accessible);
    }

    public void setShort(Object obj, short val, boolean accessible) throws IllegalAccessException
    {
        setDouble(obj, val, accessible);
    }

    public void setInt(Object obj, int val, boolean accessible) throws IllegalAccessException
    {
        setDouble(obj, val, accessible);
    }

    public void setFloat(Object obj, float val, boolean accessible) throws IllegalAccessException
    {
        setDouble(obj, val, accessible);
    }

    public void setLong(Object obj, long val, boolean accessible) throws IllegalAccessException
    {
        setDouble(obj, val, accessible);
    }
}
