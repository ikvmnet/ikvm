/* java.lang.reflect.Field - reflection of Java fields
   Copyright (C) 1998, 2001 Free Software Foundation, Inc.

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
Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA
02111-1307 USA.

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


package java.lang.reflect;

import cli.System.Diagnostics.StackFrame;
import gnu.classpath.VMStackWalker;
import ikvm.lang.CIL;

/**
 * The Field class represents a member variable of a class. It also allows
 * dynamic access to a member, via reflection. This works for both
 * static and instance fields. Operations on Field objects know how to
 * do widening conversions, but throw {@link IllegalArgumentException} if
 * a narrowing conversion would be necessary. You can query for information
 * on this Field regardless of location, but get and set access may be limited
 * by Java language access controls. If you can't do it in the compiler, you
 * can't normally do it here either.<p>
 *
 * <B>Note:</B> This class returns and accepts types as Classes, even
 * primitive types; there are Class types defined that represent each
 * different primitive type.  They are <code>java.lang.Boolean.TYPE,
 * java.lang.Byte.TYPE,</code>, also available as <code>boolean.class,
 * byte.class</code>, etc.  These are not to be confused with the
 * classes <code>java.lang.Boolean, java.lang.Byte</code>, etc., which are
 * real classes.<p>
 *
 * Also note that this is not a serializable class.  It is entirely feasible
 * to make it serializable using the Externalizable interface, but this is
 * on Sun, not me.
 *
 * @author John Keiser
 * @author Eric Blake <ebb9@email.byu.edu>
 * @see Member
 * @see Class
 * @see Class#getField(String)
 * @see Class#getDeclaredField(String)
 * @see Class#getFields()
 * @see Class#getDeclaredFields()
 * @since 1.1
 * @status updated to 1.4
 */
public final class Field extends AccessibleObject implements Member
{
	private Class declaringClass;
        // package accessible (actually "assembly") to allow map.xml implementation
        // of LibraryVMInterfaceImpl.getWrapperFromField() to access it.
	Object fieldCookie;
	private int modifiers;
        private boolean classIsPublic;
        private FieldImpl impl;

    private static native Object GetValue(Object fieldCookie, Object o);
    private static native void SetValue(Object fieldCookie, Object o, Object value, boolean accessible);

    abstract static class FieldImpl
    {
        private Object fieldCookie;

        FieldImpl(Object fieldCookie)
        {
            this.fieldCookie = fieldCookie;
        }

        final Object getImpl(Object obj)
        {
            return Field.GetValue(fieldCookie, obj);
        }

        final void setImpl(Object obj, Object val, boolean accessible)
        {
            Field.SetValue(fieldCookie, obj, val, accessible);
        }

        abstract Object get(Object obj);

        boolean getBoolean(Object obj)
        {
            throw new IllegalArgumentException();
        }
        byte getByte(Object obj)
        {
            throw new IllegalArgumentException();
        }
        char getChar(Object obj)
        {
            throw new IllegalArgumentException();
        }
        short getShort(Object obj)
        {
            throw new IllegalArgumentException();
        }
        int getInt(Object obj)
        {
            throw new IllegalArgumentException();
        }
        float getFloat(Object obj)
        {
            throw new IllegalArgumentException();
        }
        long getLong(Object obj)
        {
            throw new IllegalArgumentException();
        }
        double getDouble(Object obj)
        {
            throw new IllegalArgumentException();
        }

        abstract void set(Object obj, Object val, boolean accessible);

        void setBoolean(Object obj, boolean val, boolean accessible)
        {
            throw new IllegalArgumentException();
        }
        void setByte(Object obj, byte val, boolean accessible)
        {
            throw new IllegalArgumentException();
        }
        void setChar(Object obj, char val, boolean accessible)
        {
            throw new IllegalArgumentException();
        }
        void setShort(Object obj, short val, boolean accessible)
        {
            throw new IllegalArgumentException();
        }
        void setInt(Object obj, int val, boolean accessible)
        {
            throw new IllegalArgumentException();
        }
        void setFloat(Object obj, float val, boolean accessible)
        {
            throw new IllegalArgumentException();
        }
        void setLong(Object obj, long val, boolean accessible)
        {
            throw new IllegalArgumentException();
        }
        void setDouble(Object obj, double val, boolean accessible)
        {
            throw new IllegalArgumentException();
        }
    }

    final static class ObjectFieldImpl extends FieldImpl
    {
        ObjectFieldImpl(Object fieldCookie)
        {
            super(fieldCookie);
        }

        Object get(Object obj)
        {
            return getImpl(obj);
        }

        void set(Object obj, Object val, boolean accessible)
        {
            setImpl(obj, val, accessible);
        }
    }

    final static class BooleanFieldImpl extends FieldImpl
    {
        BooleanFieldImpl(Object fieldCookie)
        {
            super(fieldCookie);
        }

        Object get(Object obj)
        {
            return getBoolean(obj) ? Boolean.TRUE : Boolean.FALSE;
        }

        boolean getBoolean(Object obj)
        {
            return CIL.unbox_boolean(getImpl(obj));
        }

        void set(Object obj, Object val, boolean accessible)
        {
            if(! (val instanceof Boolean))
              throw new IllegalArgumentException();
            setBoolean(obj, ((Boolean)val).booleanValue(), accessible);
        }

        void setBoolean(Object obj, boolean val, boolean accessible)
        {
            setImpl(obj, CIL.box_boolean(val), accessible);
        }
    }

    final static class ByteFieldImpl extends FieldImpl
    {
        ByteFieldImpl(Object fieldCookie)
        {
            super(fieldCookie);
        }

        Object get(Object obj)
        {
            return new Byte(getByte(obj));
        }

        byte getByte(Object obj)
        {
            return CIL.unbox_byte(getImpl(obj));
        }

        short getShort(Object obj)
        {
            return getByte(obj);
        }

        int getInt(Object obj)
        {
            return getByte(obj);
        }

        float getFloat(Object obj)
        {
            return getByte(obj);
        }

        long getLong(Object obj)
        {
            return getByte(obj);
        }

        double getDouble(Object obj)
        {
            return getByte(obj);
        }

        void set(Object obj, Object val, boolean accessible)
        {
            if(! (val instanceof Byte))
              throw new IllegalArgumentException();
            setByte(obj, ((Byte)val).byteValue(), accessible);
        }

        void setByte(Object obj, byte val, boolean accessible)
        {
            setImpl(obj, CIL.box_byte(val), accessible);
        }
    }

    final static class CharFieldImpl extends FieldImpl
    {
        CharFieldImpl(Object fieldCookie)
        {
            super(fieldCookie);
        }

        Object get(Object obj)
        {
            return new Character(getChar(obj));
        }

        char getChar(Object obj)
        {
            return CIL.unbox_char(getImpl(obj));
        }

        int getInt(Object obj)
        {
            return getChar(obj);
        }

        float getFloat(Object obj)
        {
            return getChar(obj);
        }

        long getLong(Object obj)
        {
            return getChar(obj);
        }

        double getDouble(Object obj)
        {
            return getChar(obj);
        }

        void set(Object obj, Object val, boolean accessible)
        {
            if(! (val instanceof Character))
              throw new IllegalArgumentException();
            setChar(obj, ((Character)val).charValue(), accessible);
        }

        void setChar(Object obj, char val, boolean accessible)
        {
            setImpl(obj, CIL.box_char(val), accessible);
        }
    }

    final static class ShortFieldImpl extends FieldImpl
    {
        ShortFieldImpl(Object fieldCookie)
        {
            super(fieldCookie);
        }

        Object get(Object obj)
        {
            return new Short(getShort(obj));
        }

        short getShort(Object obj)
        {
            return CIL.unbox_short(getImpl(obj));
        }

        int getInt(Object obj)
        {
            return getShort(obj);
        }

        float getFloat(Object obj)
        {
            return getShort(obj);
        }

        long getLong(Object obj)
        {
            return getShort(obj);
        }

        double getDouble(Object obj)
        {
            return getShort(obj);
        }

        void set(Object obj, Object val, boolean accessible)
        {
            if(! (val instanceof Short
                 || val instanceof Byte))
              throw new IllegalArgumentException();
            setShort(obj, ((Number)val).shortValue(), accessible);
        }

        void setShort(Object obj, short val, boolean accessible)
        {
            setImpl(obj, CIL.box_short(val), accessible);
        }

        void setByte(Object obj, byte val, boolean accessible)
        {
            setShort(obj, val, accessible);
        }
    }

    final static class IntFieldImpl extends FieldImpl
    {
        IntFieldImpl(Object fieldCookie)
        {
            super(fieldCookie);
        }

        Object get(Object obj)
        {
            return new Integer(getInt(obj));
        }

        int getInt(Object obj)
        {
            return CIL.unbox_int(getImpl(obj));
        }

        float getFloat(Object obj)
        {
            return getInt(obj);
        }

        long getLong(Object obj)
        {
            return getInt(obj);
        }

        double getDouble(Object obj)
        {
            return getInt(obj);
        }

        void set(Object obj, Object val, boolean accessible)
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

        void setInt(Object obj, int val, boolean accessible)
        {
            setImpl(obj, CIL.box_int(val), accessible);
        }

        void setByte(Object obj, byte val, boolean accessible)
        {
            setInt(obj, val, accessible);
        }

        void setChar(Object obj, char val, boolean accessible)
        {
            setInt(obj, val, accessible);
        }

        void setShort(Object obj, short val, boolean accessible)
        {
            setInt(obj, val, accessible);
        }
    }

    final static class FloatFieldImpl extends FieldImpl
    {
        FloatFieldImpl(Object fieldCookie)
        {
            super(fieldCookie);
        }

        Object get(Object obj)
        {
            return new Float(getFloat(obj));
        }

        float getFloat(Object obj)
        {
            return CIL.unbox_float(getImpl(obj));
        }

        double getDouble(Object obj)
        {
            return getFloat(obj);
        }

        void set(Object obj, Object val, boolean accessible)
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

        void setFloat(Object obj, float val, boolean accessible)
        {
            setImpl(obj, CIL.box_float(val), accessible);
        }

        void setByte(Object obj, byte val, boolean accessible)
        {
            setFloat(obj, val, accessible);
        }

        void setChar(Object obj, char val, boolean accessible)
        {
            setFloat(obj, val, accessible);
        }

        void setShort(Object obj, short val, boolean accessible)
        {
            setFloat(obj, val, accessible);
        }

        void setInt(Object obj, int val, boolean accessible)
        {
            setFloat(obj, val, accessible);
        }

        void setLong(Object obj, long val, boolean accessible)
        {
            setFloat(obj, val, accessible);
        }
    }

    final static class LongFieldImpl extends FieldImpl
    {
        LongFieldImpl(Object fieldCookie)
        {
            super(fieldCookie);
        }

        Object get(Object obj)
        {
            return new Long(getLong(obj));
        }

        long getLong(Object obj)
        {
            return CIL.unbox_long(getImpl(obj));
        }

        float getFloat(Object obj)
        {
            return getLong(obj);
        }

        double getDouble(Object obj)
        {
            return getLong(obj);
        }

        void set(Object obj, Object val, boolean accessible)
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

        void setLong(Object obj, long val, boolean accessible)
        {
            setImpl(obj, CIL.box_long(val), accessible);
        }

        void setByte(Object obj, byte val, boolean accessible)
        {
            setLong(obj, val, accessible);
        }

        void setChar(Object obj, char val, boolean accessible)
        {
            setLong(obj, val, accessible);
        }

        void setShort(Object obj, short val, boolean accessible)
        {
            setLong(obj, val, accessible);
        }

        void setInt(Object obj, int val, boolean accessible)
        {
            setLong(obj, val, accessible);
        }
    }

    final static class DoubleFieldImpl extends FieldImpl
    {
        DoubleFieldImpl(Object fieldCookie)
        {
            super(fieldCookie);
        }

        Object get(Object obj)
        {
            return new Double(getDouble(obj));
        }

        double getDouble(Object obj)
        {
            return CIL.unbox_double(getImpl(obj));
        }

        void set(Object obj, Object val, boolean accessible)
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

        void setDouble(Object obj, double val, boolean accessible)
        {
            setImpl(obj, CIL.box_double(val), accessible);
        }

        void setByte(Object obj, byte val, boolean accessible)
        {
            setDouble(obj, val, accessible);
        }

        void setChar(Object obj, char val, boolean accessible)
        {
            setDouble(obj, val, accessible);
        }

        void setShort(Object obj, short val, boolean accessible)
        {
            setDouble(obj, val, accessible);
        }

        void setInt(Object obj, int val, boolean accessible)
        {
            setDouble(obj, val, accessible);
        }

        void setFloat(Object obj, float val, boolean accessible)
        {
            setDouble(obj, val, accessible);
        }

        void setLong(Object obj, long val, boolean accessible)
        {
            setDouble(obj, val, accessible);
        }
    }

	/**
	 * This class is uninstantiable except natively.
	 */
	Field(Class declaringClass, Object fieldCookie)
	{
	    this.declaringClass = declaringClass;
	    this.fieldCookie = fieldCookie;
	    modifiers = GetModifiers(fieldCookie);
	    classIsPublic = (Method.GetRealModifiers(declaringClass) & Modifier.PUBLIC) != 0;
            Class type = getType();
            if (type == Boolean.TYPE)
              impl = new BooleanFieldImpl(fieldCookie);
            else if (type == Byte.TYPE)
              impl = new ByteFieldImpl(fieldCookie);
            else if (type == Character.TYPE)
              impl = new CharFieldImpl(fieldCookie);
            else if (type == Short.TYPE)
              impl = new ShortFieldImpl(fieldCookie);
            else if (type == Integer.TYPE)
              impl = new IntFieldImpl(fieldCookie);
            else if (type == Float.TYPE)
              impl = new FloatFieldImpl(fieldCookie);
            else if (type == Long.TYPE)
              impl = new LongFieldImpl(fieldCookie);
            else if (type == Double.TYPE)
              impl = new DoubleFieldImpl(fieldCookie);
            else
              impl = new ObjectFieldImpl(fieldCookie);
	}
        private static native int GetModifiers(Object fieldCookie);

	/**
	 * Gets the class that declared this field, or the class where this field
	 * is a non-inherited member.
	 * @return the class that declared this member
	 */
	public Class getDeclaringClass()
	{
		return declaringClass;
	}

	/**
	 * Gets the name of this field.
	 * @return the name of this field
	 */
	public String getName()
	{
		return GetName(fieldCookie);
	}
	private static native String GetName(Object fieldCookie);


	/**
	 * Gets the modifiers this field uses.  Use the <code>Modifier</code>
	 * class to interpret the values.  A field can only have a subset of the
	 * following modifiers: public, private, protected, static, final,
	 * transient, and volatile.
	 *
	 * @return an integer representing the modifiers to this Member
	 * @see Modifier
	 */
	public int getModifiers()
	{
	    return modifiers;
	}

	/**
	 * Gets the type of this field.
	 * @return the type of this field
	 */
	public Class getType()
	{
		return (Class)GetFieldType(fieldCookie);
	}
	private static native Object GetFieldType(Object fieldCookie);

	/**
	 * Compare two objects to see if they are semantically equivalent.
	 * Two Fields are semantically equivalent if they have the same declaring
	 * class, name, and type.
	 *
	 * @param o the object to compare to
	 * @return <code>true</code> if they are equal; <code>false</code> if not
	 */
	public boolean equals(Object o)
	{
		if(!(o instanceof Field))
			return false;

		Field f = (Field)o;
		if(!getName().equals(f.getName()))
			return false;

		if(declaringClass != f.declaringClass)
			return false;

		if(getType() != f.getType())
			return false;

		return true;
	}

	/**
	 * Get the hash code for the Field. The Field hash code is the hash code
	 * of its name XOR'd with the hash code of its class name.
	 *
	 * @return the hash code for the object.
	 */
	public int hashCode()
	{
		return getDeclaringClass().getName().hashCode() ^ getName().hashCode();
	}

	/**
	 * Get a String representation of the Field. A Field's String
	 * representation is "&lt;modifiers&gt; &lt;type&gt;
	 * &lt;class&gt;.&lt;fieldname&gt;".<br> Example:
	 * <code>public transient boolean gnu.parse.Parser.parseComplete</code>
	 *
	 * @return the String representation of the Field
	 */
	public String toString()
	{
		StringBuffer sb = new StringBuffer();
		Modifier.toString(getModifiers(), sb).append(' ');
		sb.append(getType().getName()).append(' ');
		sb.append(getDeclaringClass().getName()).append('.');
		sb.append(getName());
		return sb.toString();
	}

	/**
	 * Get the value of this Field.  If it is primitive, it will be wrapped
	 * in the appropriate wrapper type (boolean = java.lang.Boolean).<p>
	 *
	 * If the field is static, <code>o</code> will be ignored. Otherwise, if
	 * <code>o</code> is null, you get a <code>NullPointerException</code>,
	 * and if it is incompatible with the declaring class of the field, you
	 * get an <code>IllegalArgumentException</code>.<p>
	 *
	 * Next, if this Field enforces access control, your runtime context is
	 * evaluated, and you may have an <code>IllegalAccessException</code> if
	 * you could not access this field in similar compiled code. If the field
	 * is static, and its class is uninitialized, you trigger class
	 * initialization, which may end in a
	 * <code>ExceptionInInitializerError</code>.<p>
	 *
	 * Finally, the field is accessed, and primitives are wrapped (but not
	 * necessarily in new objects). This method accesses the field of the
	 * declaring class, even if the instance passed in belongs to a subclass
	 * which declares another field to hide this one.
	 *
	 * @param o the object to get the value of this Field from
	 * @return the value of the Field
	 * @throws IllegalAccessException if you could not normally access this field
	 *         (i.e. it is not public)
	 * @throws IllegalArgumentException if <code>o</code> is not an instance of
	 *         the class or interface declaring this field
	 * @throws NullPointerException if <code>o</code> is null and this field
	 *         requires an instance
	 * @throws ExceptionInInitializerError if accessing a static field triggered
	 *         class initialization, which then failed
	 * @see #getBoolean(Object)
	 * @see #getByte(Object)
	 * @see #getChar(Object)
	 * @see #getShort(Object)
	 * @see #getInt(Object)
	 * @see #getLong(Object)
	 * @see #getFloat(Object)
	 * @see #getDouble(Object)
	 */
	public Object get(Object o)
		throws IllegalAccessException
	{
	    if(!isAccessible() && (!Modifier.isPublic(modifiers) || !classIsPublic))
		checkAccess(modifiers, o, declaringClass, VMStackWalker.getCallingClass());
            if(o != null && !declaringClass.isInstance(o))
                throw new IllegalArgumentException();
	    return impl.get(o);
	}

	/**
	 * Get the value of this boolean Field. If the field is static,
	 * <code>o</code> will be ignored.
	 *
	 * @param o the object to get the value of this Field from
	 * @return the value of the Field
	 * @throws IllegalAccessException if you could not normally access this field
	 *         (i.e. it is not public)
	 * @throws IllegalArgumentException if this is not a boolean field of
	 *         <code>o</code>, or if <code>o</code> is not an instance of the
	 *         declaring class of this field
	 * @throws NullPointerException if <code>o</code> is null and this field
	 *         requires an instance
	 * @throws ExceptionInInitializerError if accessing a static field triggered
	 *         class initialization, which then failed
	 * @see #get(Object)
	 */
	public boolean getBoolean(Object o)
		throws IllegalAccessException
	{
	    if(!isAccessible() && (!Modifier.isPublic(modifiers) || !classIsPublic))
		checkAccess(modifiers, o, declaringClass, VMStackWalker.getCallingClass());
            if(o != null && !declaringClass.isInstance(o))
                throw new IllegalArgumentException();
            return impl.getBoolean(o);
	}

	/**
	 * Get the value of this byte Field. If the field is static,
	 * <code>o</code> will be ignored.
	 *
	 * @param o the object to get the value of this Field from
	 * @return the value of the Field
	 * @throws IllegalAccessException if you could not normally access this field
	 *         (i.e. it is not public)
	 * @throws IllegalArgumentException if this is not a byte field of
	 *         <code>o</code>, or if <code>o</code> is not an instance of the
	 *         declaring class of this field
	 * @throws NullPointerException if <code>o</code> is null and this field
	 *         requires an instance
	 * @throws ExceptionInInitializerError if accessing a static field triggered
	 *         class initialization, which then failed
	 * @see #get(Object)
	 */
	public byte getByte(Object o)
		throws IllegalAccessException
	{
	    if(!isAccessible() && (!Modifier.isPublic(modifiers) || !classIsPublic))
		checkAccess(modifiers, o, declaringClass, VMStackWalker.getCallingClass());
            if(o != null && !declaringClass.isInstance(o))
                throw new IllegalArgumentException();
            return impl.getByte(o);
	}

	/**
	 * Get the value of this Field as a char. If the field is static,
	 * <code>o</code> will be ignored.
	 *
	 * @throws IllegalAccessException if you could not normally access this field
	 *         (i.e. it is not public)
	 * @throws IllegalArgumentException if this is not a char field of
	 *         <code>o</code>, or if <code>o</code> is not an instance
	 *         of the declaring class of this field
	 * @throws NullPointerException if <code>o</code> is null and this field
	 *         requires an instance
	 * @throws ExceptionInInitializerError if accessing a static field triggered
	 *         class initialization, which then failed
	 * @see #get(Object)
	 */
	public char getChar(Object o)
		throws IllegalAccessException
	{
	    if(!isAccessible() && (!Modifier.isPublic(modifiers) || !classIsPublic))
		checkAccess(modifiers, o, declaringClass, VMStackWalker.getCallingClass());
            if(o != null && !declaringClass.isInstance(o))
                throw new IllegalArgumentException();
            return impl.getChar(o);
	}

	/**
	 * Get the value of this Field as a short. If the field is static,
	 * <code>o</code> will be ignored.
	 *
	 * @param o the object to get the value of this Field from
	 * @return the value of the Field
	 * @throws IllegalAccessException if you could not normally access this field
	 *         (i.e. it is not public)
	 * @throws IllegalArgumentException if this is not a byte or short
	 *         field of <code>o</code>, or if <code>o</code> is not an instance
	 *         of the declaring class of this field
	 * @throws NullPointerException if <code>o</code> is null and this field
	 *         requires an instance
	 * @throws ExceptionInInitializerError if accessing a static field triggered
	 *         class initialization, which then failed
	 * @see #get(Object)
	 */
	public short getShort(Object o)
		throws IllegalAccessException
	{
	    if(!isAccessible() && (!Modifier.isPublic(modifiers) || !classIsPublic))
		checkAccess(modifiers, o, declaringClass, VMStackWalker.getCallingClass());
            if(o != null && !declaringClass.isInstance(o))
                throw new IllegalArgumentException();
            return impl.getShort(o);
	}

	/**
	 * Get the value of this Field as an int. If the field is static,
	 * <code>o</code> will be ignored.
	 *
	 * @param o the object to get the value of this Field from
	 * @return the value of the Field
	 * @throws IllegalAccessException if you could not normally access this field
	 *         (i.e. it is not public)
	 * @throws IllegalArgumentException if this is not a byte, short, char, or
	 *         int field of <code>o</code>, or if <code>o</code> is not an
	 *         instance of the declaring class of this field
	 * @throws NullPointerException if <code>o</code> is null and this field
	 *         requires an instance
	 * @throws ExceptionInInitializerError if accessing a static field triggered
	 *         class initialization, which then failed
	 * @see #get(Object)
	 */
	public int getInt(Object o)
		throws IllegalAccessException
	{
	    if(!isAccessible() && (!Modifier.isPublic(modifiers) || !classIsPublic))
		checkAccess(modifiers, o, declaringClass, VMStackWalker.getCallingClass());
            if(o != null && !declaringClass.isInstance(o))
                throw new IllegalArgumentException();
            return impl.getInt(o);
	}

	/**
	 * Get the value of this Field as a long. If the field is static,
	 * <code>o</code> will be ignored.
	 *
	 * @param o the object to get the value of this Field from
	 * @return the value of the Field
	 * @throws IllegalAccessException if you could not normally access this field
	 *         (i.e. it is not public)
	 * @throws IllegalArgumentException if this is not a byte, short, char, int,
	 *         or long field of <code>o</code>, or if <code>o</code> is not an
	 *         instance of the declaring class of this field
	 * @throws NullPointerException if <code>o</code> is null and this field
	 *         requires an instance
	 * @throws ExceptionInInitializerError if accessing a static field triggered
	 *         class initialization, which then failed
	 * @see #get(Object)
	 */
	public long getLong(Object o)
		throws IllegalAccessException
	{
	    if(!isAccessible() && (!Modifier.isPublic(modifiers) || !classIsPublic))
		checkAccess(modifiers, o, declaringClass, VMStackWalker.getCallingClass());
            if(o != null && !declaringClass.isInstance(o))
                throw new IllegalArgumentException();
            return impl.getLong(o);
	}

	/**
	 * Get the value of this Field as a float. If the field is static,
	 * <code>o</code> will be ignored.
	 *
	 * @param o the object to get the value of this Field from
	 * @return the value of the Field
	 * @throws IllegalAccessException if you could not normally access this field
	 *         (i.e. it is not public)
	 * @throws IllegalArgumentException if this is not a byte, short, char, int,
	 *         long, or float field of <code>o</code>, or if <code>o</code> is
	 *         not an instance of the declaring class of this field
	 * @throws NullPointerException if <code>o</code> is null and this field
	 *         requires an instance
	 * @throws ExceptionInInitializerError if accessing a static field triggered
	 *         class initialization, which then failed
	 * @see #get(Object)
	 */
	public float getFloat(Object o)
		throws IllegalAccessException
	{
	    if(!isAccessible() && (!Modifier.isPublic(modifiers) || !classIsPublic))
		checkAccess(modifiers, o, declaringClass, VMStackWalker.getCallingClass());
            if(o != null && !declaringClass.isInstance(o))
                throw new IllegalArgumentException();
            return impl.getFloat(o);
	}

	/**
	 * Get the value of this Field as a double. If the field is static,
	 * <code>o</code> will be ignored.
	 *
	 * @param o the object to get the value of this Field from
	 * @return the value of the Field
	 * @throws IllegalAccessException if you could not normally access this field
	 *         (i.e. it is not public)
	 * @throws IllegalArgumentException if this is not a byte, short, char, int,
	 *         long, float, or double field of <code>o</code>, or if
	 *         <code>o</code> is not an instance of the declaring class of this
	 *         field
	 * @throws NullPointerException if <code>o</code> is null and this field
	 *         requires an instance
	 * @throws ExceptionInInitializerError if accessing a static field triggered
	 *         class initialization, which then failed
	 * @see #get(Object)
	 */
	public double getDouble(Object o)
		throws IllegalAccessException
	{
	    if(!isAccessible() && (!Modifier.isPublic(modifiers) || !classIsPublic))
		checkAccess(modifiers, o, declaringClass, VMStackWalker.getCallingClass());
            if(o != null && !declaringClass.isInstance(o))
                throw new IllegalArgumentException();
            return impl.getDouble(o);
	}

	/**
	 * Set the value of this Field.  If it is a primitive field, the value
	 * will be unwrapped from the passed object (boolean = java.lang.Boolean).<p>
	 *
	 * If the field is static, <code>o</code> will be ignored. Otherwise, if
	 * <code>o</code> is null, you get a <code>NullPointerException</code>,
	 * and if it is incompatible with the declaring class of the field, you
	 * get an <code>IllegalArgumentException</code>.<p>
	 *
	 * Next, if this Field enforces access control, your runtime context is
	 * evaluated, and you may have an <code>IllegalAccessException</code> if
	 * you could not access this field in similar compiled code. This also
	 * occurs whether or not there is access control if the field is final.
	 * If the field is primitive, and unwrapping your argument fails, you will
	 * get an <code>IllegalArgumentException</code>; likewise, this error
	 * happens if <code>value</code> cannot be cast to the correct object type.
	 * If the field is static, and its class is uninitialized, you trigger class
	 * initialization, which may end in a
	 * <code>ExceptionInInitializerError</code>.<p>
	 *
	 * Finally, the field is set with the widened value. This method accesses
	 * the field of the declaring class, even if the instance passed in belongs
	 * to a subclass which declares another field to hide this one.
	 *
	 * @param o the object to set this Field on
	 * @param value the value to set this Field to
	 * @throws IllegalAccessException if you could not normally access this field
	 *         (i.e. it is not public)
	 * @throws IllegalArgumentException if <code>value</code> cannot be
	 *         converted by a widening conversion to the underlying type of
	 *         the Field, or if <code>o</code> is not an instance of the class
	 *         declaring this field
	 * @throws NullPointerException if <code>o</code> is null and this field
	 *         requires an instance
	 * @throws ExceptionInInitializerError if accessing a static field triggered
	 *         class initialization, which then failed
	 * @see #setBoolean(Object, boolean)
	 * @see #setByte(Object, byte)
	 * @see #setChar(Object, char)
	 * @see #setShort(Object, short)
	 * @see #setInt(Object, int)
	 * @see #setLong(Object, long)
	 * @see #setFloat(Object, float)
	 * @see #setDouble(Object, double)
	 */
	public void set(Object o, Object value)
		throws IllegalAccessException
	{
	    if(!isAccessible() && (!Modifier.isPublic(modifiers) || !classIsPublic))
		checkAccess(modifiers, o, declaringClass, VMStackWalker.getCallingClass());
            if(o != null && !declaringClass.isInstance(o))
                throw new IllegalArgumentException();
            impl.set(o, value, isAccessible());
	}

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
	private static native boolean isSamePackage(Class a, Class b);

	/**
	 * Set this boolean Field. If the field is static, <code>o</code> will be
	 * ignored.
	 *
	 * @param o the object to set this Field on
	 * @param value the value to set this Field to
	 * @throws IllegalAccessException if you could not normally access this field
	 *         (i.e. it is not public)
	 * @throws IllegalArgumentException if this is not a boolean field, or if
	 *         <code>o</code> is not an instance of the class declaring this
	 *         field
	 * @throws NullPointerException if <code>o</code> is null and this field
	 *         requires an instance
	 * @throws ExceptionInInitializerError if accessing a static field triggered
	 *         class initialization, which then failed
	 * @see #set(Object, Object)
	 */
	public void setBoolean(Object o, boolean value)
		throws IllegalAccessException
	{
	    if(!isAccessible() && (!Modifier.isPublic(modifiers) || !classIsPublic))
		checkAccess(modifiers, o, declaringClass, VMStackWalker.getCallingClass());
            if(o != null && !declaringClass.isInstance(o))
                throw new IllegalArgumentException();
            impl.setBoolean(o, value, isAccessible());
	}

	/**
	 * Set this byte Field. If the field is static, <code>o</code> will be
	 * ignored.
	 *
	 * @param o the object to set this Field on
	 * @param value the value to set this Field to
	 * @throws IllegalAccessException if you could not normally access this field
	 *         (i.e. it is not public)
	 * @throws IllegalArgumentException if this is not a byte, short, int, long,
	 *         float, or double field, or if <code>o</code> is not an instance
	 *         of the class declaring this field
	 * @throws NullPointerException if <code>o</code> is null and this field
	 *         requires an instance
	 * @throws ExceptionInInitializerError if accessing a static field triggered
	 *         class initialization, which then failed
	 * @see #set(Object, Object)
	 */
	public void setByte(Object o, byte value)
		throws IllegalAccessException
	{
	    if(!isAccessible() && (!Modifier.isPublic(modifiers) || !classIsPublic))
		checkAccess(modifiers, o, declaringClass, VMStackWalker.getCallingClass());
            if(o != null && !declaringClass.isInstance(o))
                throw new IllegalArgumentException();
            impl.setByte(o, value, isAccessible());
	}

	/**
	 * Set this char Field. If the field is static, <code>o</code> will be
	 * ignored.
	 *
	 * @param o the object to set this Field on
	 * @param value the value to set this Field to
	 * @throws IllegalAccessException if you could not normally access this field
	 *         (i.e. it is not public)
	 * @throws IllegalArgumentException if this is not a char, int, long,
	 *         float, or double field, or if <code>o</code> is not an instance
	 *         of the class declaring this field
	 * @throws NullPointerException if <code>o</code> is null and this field
	 *         requires an instance
	 * @throws ExceptionInInitializerError if accessing a static field triggered
	 *         class initialization, which then failed
	 * @see #set(Object, Object)
	 */
	public void setChar(Object o, char value)
		throws IllegalAccessException
	{
	    if(!isAccessible() && (!Modifier.isPublic(modifiers) || !classIsPublic))
		checkAccess(modifiers, o, declaringClass, VMStackWalker.getCallingClass());
            if(o != null && !declaringClass.isInstance(o))
                throw new IllegalArgumentException();
            impl.setChar(o, value, isAccessible());
	}

	/**
	 * Set this short Field. If the field is static, <code>o</code> will be
	 * ignored.
	 *
	 * @param o the object to set this Field on
	 * @param value the value to set this Field to
	 * @throws IllegalAccessException if you could not normally access this field
	 *         (i.e. it is not public)
	 * @throws IllegalArgumentException if this is not a short, int, long,
	 *         float, or double field, or if <code>o</code> is not an instance
	 *         of the class declaring this field
	 * @throws NullPointerException if <code>o</code> is null and this field
	 *         requires an instance
	 * @throws ExceptionInInitializerError if accessing a static field triggered
	 *         class initialization, which then failed
	 * @see #set(Object, Object)
	 */
	public void setShort(Object o, short value)
		throws IllegalAccessException
	{
	    if(!isAccessible() && (!Modifier.isPublic(modifiers) || !classIsPublic))
		checkAccess(modifiers, o, declaringClass, VMStackWalker.getCallingClass());
            if(o != null && !declaringClass.isInstance(o))
                throw new IllegalArgumentException();
            impl.setShort(o, value, isAccessible());
	}

	/**
	 * Set this int Field. If the field is static, <code>o</code> will be
	 * ignored.
	 *
	 * @param o the object to set this Field on
	 * @param value the value to set this Field to
	 * @throws IllegalAccessException if you could not normally access this field
	 *         (i.e. it is not public)
	 * @throws IllegalArgumentException if this is not an int, long, float, or
	 *         double field, or if <code>o</code> is not an instance of the
	 *         class declaring this field
	 * @throws NullPointerException if <code>o</code> is null and this field
	 *         requires an instance
	 * @throws ExceptionInInitializerError if accessing a static field triggered
	 *         class initialization, which then failed
	 * @see #set(Object, Object)
	 */
	public void setInt(Object o, int value)
		throws IllegalAccessException
	{
	    if(!isAccessible() && (!Modifier.isPublic(modifiers) || !classIsPublic))
		checkAccess(modifiers, o, declaringClass, VMStackWalker.getCallingClass());
            if(o != null && !declaringClass.isInstance(o))
                throw new IllegalArgumentException();
            impl.setInt(o, value, isAccessible());
	}

	/**
	 * Set this long Field. If the field is static, <code>o</code> will be
	 * ignored.
	 *
	 * @param o the object to set this Field on
	 * @param value the value to set this Field to
	 * @throws IllegalAccessException if you could not normally access this field
	 *         (i.e. it is not public)
	 * @throws IllegalArgumentException if this is not a long, float, or double
	 *         field, or if <code>o</code> is not an instance of the class
	 *         declaring this field
	 * @throws NullPointerException if <code>o</code> is null and this field
	 *         requires an instance
	 * @throws ExceptionInInitializerError if accessing a static field triggered
	 *         class initialization, which then failed
	 * @see #set(Object, Object)
	 */
	public void setLong(Object o, long value)
		throws IllegalAccessException
	{
	    if(!isAccessible() && (!Modifier.isPublic(modifiers) || !classIsPublic))
		checkAccess(modifiers, o, declaringClass, VMStackWalker.getCallingClass());
            if(o != null && !declaringClass.isInstance(o))
                throw new IllegalArgumentException();
            impl.setLong(o, value, isAccessible());
	}

	/**
	 * Set this float Field. If the field is static, <code>o</code> will be
	 * ignored.
	 *
	 * @param o the object to set this Field on
	 * @param value the value to set this Field to
	 * @throws IllegalAccessException if you could not normally access this field
	 *         (i.e. it is not public)
	 * @throws IllegalArgumentException if this is not a float or long field, or
	 *         if <code>o</code> is not an instance of the class declaring this
	 *         field
	 * @throws NullPointerException if <code>o</code> is null and this field
	 *         requires an instance
	 * @throws ExceptionInInitializerError if accessing a static field triggered
	 *         class initialization, which then failed
	 * @see #set(Object, Object)
	 */
	public void setFloat(Object o, float value)
		throws IllegalAccessException
	{
	    if(!isAccessible() && (!Modifier.isPublic(modifiers) || !classIsPublic))
		checkAccess(modifiers, o, declaringClass, VMStackWalker.getCallingClass());
            if(o != null && !declaringClass.isInstance(o))
                throw new IllegalArgumentException();
            impl.setFloat(o, value, isAccessible());
	}

	/**
	 * Set this double Field. If the field is static, <code>o</code> will be
	 * ignored.
	 *
	 * @param o the object to set this Field on
	 * @param value the value to set this Field to
	 * @throws IllegalAccessException if you could not normally access this field
	 *         (i.e. it is not public)
	 * @throws IllegalArgumentException if this is not a double field, or if
	 *         <code>o</code> is not an instance of the class declaring this
	 *         field
	 * @throws NullPointerException if <code>o</code> is null and this field
	 *         requires an instance
	 * @throws ExceptionInInitializerError if accessing a static field triggered
	 *         class initialization, which then failed
	 * @see #set(Object, Object)
	 */
	public void setDouble(Object o, double value)
		throws IllegalAccessException
	{
	    if(!isAccessible() && (!Modifier.isPublic(modifiers) || !classIsPublic))
		checkAccess(modifiers, o, declaringClass, VMStackWalker.getCallingClass());
            if(o != null && !declaringClass.isInstance(o))
                throw new IllegalArgumentException();
            impl.setDouble(o, value, isAccessible());
	}
}
