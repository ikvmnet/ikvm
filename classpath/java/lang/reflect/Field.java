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
	private Object fieldCookie;

	/**
	 * This class is uninstantiable except natively.
	 */
	public Field(Class declaringClass, Object fieldCookie)
	{
		this.declaringClass = declaringClass;
		this.fieldCookie = fieldCookie;
	}

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
		return GetModifiers(fieldCookie);
	}
	private static native int GetModifiers(Object fieldCookie);

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
		return GetValue(fieldCookie, o);
	}
	private static native Object GetValue(Object fieldCookie, Object o);

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
		return ((Boolean)get(o)).booleanValue();
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
		return ((Byte)get(o)).byteValue();
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
		return ((Character)get(o)).charValue();
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
		return ((Short)get(o)).shortValue();
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
		return ((Integer)get(o)).intValue();
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
		return ((Long)get(o)).longValue();
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
		return ((Float)get(o)).floatValue();
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
		return ((Double)get(o)).doubleValue();
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
		SetValue(fieldCookie, o, value);
	}
	private static native void SetValue(Object fieldCookie, Object o, Object value);

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
		set(o, new Boolean(value));
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
		set(o, new Byte(value));
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
		set(o, new Character(value));
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
		set(o, new Short(value));
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
		set(o, new Integer(value));
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
		set(o, new Long(value));
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
		set(o, new Float(value));
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
		set(o, new Double(value));
	}
}
