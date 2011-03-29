/*
  Copyright (C) 2005-2011 Jeroen Frijters

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
package ikvm.internal;

import cli.System.Reflection.BindingFlags;
import ikvm.lang.CIL;
import java.io.Serializable;
import java.lang.annotation.Annotation;
import java.lang.reflect.Array;
import java.lang.reflect.InvocationHandler;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.lang.reflect.Proxy;
import java.util.Arrays;
import java.util.Iterator;
import java.util.Map;
import java.util.HashMap;

public abstract class AnnotationAttributeBase
    extends cli.System.Attribute
    implements Annotation, Serializable
{
    private final HashMap values = new HashMap();
    private final Class annotationType;
    private boolean frozen;

    protected AnnotationAttributeBase(Class annotationType)
    {
        this.annotationType = annotationType;
    }

    protected final Object getValue(String name)
    {
	freeze();
        return values.get(name);
    }

    protected final byte getByteValue(String name)
    {
	freeze();
        return ((Byte)values.get(name)).byteValue();
    }

    protected final boolean getBooleanValue(String name)
    {
	freeze();
        return ((Boolean)values.get(name)).booleanValue();
    }

    protected final short getShortValue(String name)
    {
	freeze();
        return ((Short)values.get(name)).shortValue();
    }

    protected final char getCharValue(String name)
    {
	freeze();
        return ((Character)values.get(name)).charValue();
    }

    protected final int getIntValue(String name)
    {
	freeze();
        return ((Integer)values.get(name)).intValue();
    }

    protected final float getFloatValue(String name)
    {
	freeze();
        return ((Float)values.get(name)).floatValue();
    }

    protected final long getLongValue(String name)
    {
	freeze();
        return ((Long)values.get(name)).longValue();
    }

    protected final double getDoubleValue(String name)
    {
	freeze();
        return ((Double)values.get(name)).doubleValue();
    }

    protected final synchronized void setValue(String name, Object value)
    {
        if(frozen)
        {
            throw new IllegalStateException("Annotation properties have already been defined");
        }
        try
        {
            Class type = annotationType.getMethod(name).getReturnType();
            if(type.isEnum())
            {
                value = type.getMethod("valueOf", String.class).invoke(null, value.toString());
            }
            else if(type == Class.class)
            {
                value = ikvm.runtime.Util.getFriendlyClassFromType((cli.System.Type)value);
            }
            else if(type == boolean.class)
            {
                value = ikvm.lang.CIL.unbox_boolean(value);
            }
            else if(type == byte.class)
            {
                value = ikvm.lang.CIL.unbox_byte(value);
            }
            else if(type == short.class)
            {
                value = ikvm.lang.CIL.unbox_short(value);
            }
            else if(type == char.class)
            {
                value = ikvm.lang.CIL.unbox_char(value);
            }
            else if(type == int.class)
            {
                value = ikvm.lang.CIL.unbox_int(value);
            }
            else if(type == long.class)
            {
                value = ikvm.lang.CIL.unbox_long(value);
            }
            else if(type == float.class)
            {
                value = ikvm.lang.CIL.unbox_float(value);
            }
            else if(type == double.class)
            {
                value = ikvm.lang.CIL.unbox_double(value);
            }
            else if(type == String.class)
            {
                // no conversion needed
            }
            else if(type.isArray())
            {
                type = type.getComponentType();
                if(type.isEnum())
                {
                    Method valueOf = type.getMethod("valueOf", String.class);
                    cli.System.Array orgarray = (cli.System.Array)value;
                    Object[] array = (Object[])Array.newInstance(type, orgarray.get_Length());
                    for(int i = 0; i < array.length; i++)
                    {
                        array[i] = valueOf.invoke(null, orgarray.GetValue(i).toString());
                    }
                    value = array;
                }
                else if(type == Class.class)
                {
                    cli.System.Type[] orgarray = (cli.System.Type[])value;
                    Class[] array = new Class[orgarray.length];
                    for(int i = 0; i < array.length; i++)
                    {
                        array[i] = ikvm.runtime.Util.getFriendlyClassFromType(orgarray[i]);
                    }
                    value = array;
                }
                else
                {
                    // no conversion needed
                }
            }
            else
            {
                throw new InternalError("Invalid annotation type: " + type);
            }
            values.put(name, value);
        }
        catch (NoSuchMethodException x)
        {
            throw (NoSuchMethodError)new NoSuchMethodError().initCause(x);
        }
        catch (IllegalAccessException x)
        {
            throw (IllegalAccessError)new IllegalAccessError().initCause(x);
        }
        catch (InvocationTargetException x)
        {
            throw (InternalError)new InternalError().initCause(x);
        }
    }

    protected final synchronized void setDefinition(Object[] array)
    {
        if(frozen)
        {
            throw new IllegalStateException("Annotation properties have already been defined");
        }
        frozen = true;
        // TODO consider checking that the type matches
        // (or better yet (?), remove the first two redundant elements from the array)
        decodeValues(values, annotationType, annotationType.getClassLoader(), array);
    }

    @ikvm.lang.Internal
    public Map getValues()
    {
        return values;
    }

    @ikvm.lang.Internal
    public synchronized void freeze()
    {
        if(!frozen)
        {
            frozen = true;
            setDefaults(values, annotationType);
        }
    }

    private static void decodeValues(HashMap map, Class annotationClass, ClassLoader loader, Object[] array)
    {
        for (int i = 2; i < array.length; i += 2)
        {
            String name = (String)array[i];
            try
            {
                Method method = annotationClass.getDeclaredMethod(name, new Class[0]);
                map.put(name, decodeElementValue(array[i + 1], method.getReturnType(), loader));
            }
            catch (IllegalAccessException x)
            {
                // TODO this probably isn't the right exception
                throw new IncompatibleClassChangeError();
            }
            catch (NoSuchMethodException x)
            {
                // TODO this probably isn't the right exception
                throw new IncompatibleClassChangeError("Method " + name + " is missing in annotation " + annotationClass.getName());
            }
        }
        setDefaults(map, annotationClass);
    }

    private static void setDefaults(HashMap map, Class annotationClass)
    {
        for (Method m : annotationClass.getDeclaredMethods())
        {
            Object defaultValue = m.getDefaultValue();
            // TODO throw exception if default is missing for method that doesn't yet have a value
            if (defaultValue != null && !map.containsKey(m.getName()))
            {
                map.put(m.getName(), defaultValue);
            }
        }
    }

    private static Class classFromSig(ClassLoader loader, String name)
    {
        if (name.startsWith("L") && name.endsWith(";"))
        {
            name = name.substring(1, name.length() - 1).replace('/', '.');
        }
        else if (name.startsWith("["))
        {
            name = name.replace('/', '.');
        }
        else if (name.length() == 1)
        {
            switch (name.charAt(0))
            {
                case 'Z':
                    return Boolean.TYPE;
                case 'B':
                    return Byte.TYPE;
                case 'C':
                    return Character.TYPE;
                case 'S':
                    return Short.TYPE;
                case 'I':
                    return Integer.TYPE;
                case 'F':
                    return Float.TYPE;
                case 'J':
                    return Long.TYPE;
                case 'D':
                    return Double.TYPE;
                case 'V':
                    return Void.TYPE;
                default:
                    throw new TypeNotPresentException(name, null);
            }
        }
        try
        {
            return Class.forName(name, false, loader);
        }
        catch (ClassNotFoundException x)
        {
            throw new TypeNotPresentException(name, x);
        }
    }

    public static Object newAnnotation(ClassLoader loader, Object definition)
    {
        Object[] array = (Object[])definition;
        byte tag = CIL.unbox_byte(array[0]);
        if (tag != '@')
            throw new ClassCastException();
        Object classNameOrClass = array[1];
        Class annotationClass;
        if (classNameOrClass instanceof String)
        {
            annotationClass = classFromSig(loader, (String)classNameOrClass);
            array[1] = annotationClass;
        }
        else
        {
            annotationClass = (Class)classNameOrClass;
        }
        HashMap map = new HashMap();
        decodeValues(map, annotationClass, loader, array);
        return Proxy.newProxyInstance(annotationClass.getClassLoader(), new Class<?>[] { annotationClass }, newAnnotationInvocationHandler(annotationClass, map));
    }

    public static Object decodeElementValue(Object obj, Class type, ClassLoader loader)
        throws IllegalAccessException
    {
        if (obj instanceof Object[] && CIL.unbox_byte(((Object[])obj)[0]) == '?')
        {
            Throwable t;
            try
            {
                Object[] error = (Object[])obj;
                t = (Throwable)Class.forName((String)error[1]).getConstructor(String.class).newInstance(error[2]);
            }
            catch (Exception x)
            {
                t = x;
            }
            sun.misc.Unsafe.getUnsafe().throwException(t);
        }
        if (type == Byte.TYPE)
        {
            return new Byte(CIL.unbox_byte(obj));
        }
        else if (type == Boolean.TYPE)
        {
            return new Boolean(CIL.unbox_boolean(obj));
        }
        else if (type == Short.TYPE)
        {
            return new Short(CIL.unbox_short(obj));
        }
        else if (type == Character.TYPE)
        {
            return new Character(CIL.unbox_char(obj));
        }
        else if (type == Integer.TYPE)
        {
            return new Integer(CIL.unbox_int(obj));
        }
        else if (type == Float.TYPE)
        {
            return new Float(CIL.unbox_float(obj));
        }
        else if (type == Long.TYPE)
        {
            return new Long(CIL.unbox_long(obj));
        }
        else if (type == Double.TYPE)
        {
            return new Double(CIL.unbox_double(obj));
        }
        else if (type == String.class)
        {
            return (String)obj;
        }
        else if (type == Class.class)
        {
            Object[] array = (Object[])obj;
            byte tag = CIL.unbox_byte(array[0]);
            if (tag != 'c')
                throw new ClassCastException();
            return classFromSig(loader, (String)array[1]);
        }
        else if (type.isArray())
        {
            Object[] array = (Object[])obj;
            byte tag = CIL.unbox_byte(array[0]);
            if (tag != '[')
                throw new ClassCastException();
            type = type.getComponentType();
            Object dst = Array.newInstance(type, array.length - 1);
            for (int i = 0; i < array.length - 1; i++)
            {
                Array.set(dst, i, decodeElementValue(array[i + 1], type, loader));
            }
            return dst;
        }
        else if (type.isAnnotation())
        {
            return type.cast(newAnnotation(loader, obj));
        }
        else if (type.isEnum())
        {
            Object[] array = (Object[])obj;
            byte tag = CIL.unbox_byte(array[0]);
            if (tag != 'e')
                throw new ClassCastException();
            Class enumClass = classFromSig(loader, (String)array[1]);
            try
            {
                return Enum.valueOf(enumClass, (String)array[2]);
            }
            catch (IllegalArgumentException x)
            {
                throw new EnumConstantNotPresentException(enumClass, (String)array[2]);
            }
        }
        else
        {
            throw new ClassCastException();
        }
    }

    protected final Object writeReplace()
    {
	return Proxy.newProxyInstance(annotationType.getClassLoader(),
	    new Class[] { annotationType },
	    newAnnotationInvocationHandler(annotationType, values));
    }

    private static cli.System.Reflection.ConstructorInfo annotationInvocationHandlerConstructor;

    private static InvocationHandler newAnnotationInvocationHandler(Class type, Map memberValues)
    {
	if (annotationInvocationHandlerConstructor == null)
	{
	    cli.System.Type typeofClass = cli.System.Type.GetType("java.lang.Class");
	    cli.System.Type typeofMap = cli.System.Type.GetType("java.util.Map");
	    annotationInvocationHandlerConstructor = cli.System.Type.GetType("sun.reflect.annotation.AnnotationInvocationHandler")
		.GetConstructor(BindingFlags.wrap(BindingFlags.Instance | BindingFlags.NonPublic), null, new cli.System.Type[] { typeofClass, typeofMap }, null);
	}
	return (InvocationHandler)annotationInvocationHandlerConstructor.Invoke(new Object[] { type, memberValues });
    }

    public final Class<? extends Annotation> annotationType()
    {
        return annotationType;
    }

    public final boolean Equals(Object o)
    {
        return equals(annotationType, values, o);
    }

    public final int GetHashCode()
    {
        return hashCode(annotationType, values);
    }

    public final String ToString()
    {
        return toString(annotationType, values);
    }

    private static boolean equals(Class type, Map memberValues, Object other)
    {
        if (type.isInstance(other))
        {
            try
            {
                Method[] methods = type.getDeclaredMethods();
                if (methods.length == memberValues.size())
                {
                    for (int i = 0; i < methods.length; i++)
                    {
                        String key = methods[i].getName();
                        Object val = methods[i].invoke(other, new Object[0]);
                        if (! deepEquals(memberValues.get(key), val))
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            catch (IllegalAccessException _)
            {
                // Ignore exception, like the JDK
            }
            catch (InvocationTargetException _)
            {
                // Ignore exception, like the JDK
            }
        }
        return false;
    }

    private static boolean deepEquals(Object o1, Object o2)
    {
        if (o1 == o2)
            return true;

        if (o1 == null || o2 == null)
            return false;

        if (o1 instanceof boolean[] && o2 instanceof boolean[])
            return Arrays.equals((boolean[]) o1, (boolean[]) o2);

        if (o1 instanceof byte[] && o2 instanceof byte[])
            return Arrays.equals((byte[]) o1, (byte[]) o2);

        if (o1 instanceof char[] && o2 instanceof char[])
            return Arrays.equals((char[]) o1, (char[]) o2);

        if (o1 instanceof short[] && o2 instanceof short[])
            return Arrays.equals((short[]) o1, (short[]) o2);

        if (o1 instanceof int[] && o2 instanceof int[])
            return Arrays.equals((int[]) o1, (int[]) o2);

        if (o1 instanceof float[] && o2 instanceof float[])
            return Arrays.equals((float[]) o1, (float[]) o2);

        if (o1 instanceof long[] && o2 instanceof long[])
            return Arrays.equals((long[]) o1, (long[]) o2);

        if (o1 instanceof double[] && o2 instanceof double[])
            return Arrays.equals((double[]) o1, (double[]) o2);

        if (o1 instanceof Object[] && o2 instanceof Object[])
            return Arrays.equals((Object[]) o1, (Object[]) o2);

        return o1.equals(o2);
    }

    private static int deepHashCode(Object obj)
    {
        if (obj instanceof boolean[])
            return Arrays.hashCode((boolean[]) obj);

        if (obj instanceof byte[])
            return Arrays.hashCode((byte[]) obj);

        if (obj instanceof char[])
            return Arrays.hashCode((char[]) obj);

        if (obj instanceof short[])
            return Arrays.hashCode((short[]) obj);

        if (obj instanceof int[])
            return Arrays.hashCode((int[]) obj);

        if (obj instanceof float[])
            return Arrays.hashCode((float[]) obj);

        if (obj instanceof long[])
            return Arrays.hashCode((long[]) obj);

        if (obj instanceof double[])
            return Arrays.hashCode((double[]) obj);

        if (obj instanceof Object[])
            return Arrays.hashCode((Object[]) obj);

        return obj.hashCode();
    }

    private static int hashCode(Class type, Map memberValues)
    {
        int h = 0;
        Iterator iter = memberValues.keySet().iterator();
        while (iter.hasNext())
        {
            Object key = iter.next();
            Object val = memberValues.get(key);
            h += deepHashCode(val) ^ 127 * key.hashCode();
        }
        return h;
    }

    private static String deepToString(Object obj)
    {
        if (obj instanceof boolean[])
            return Arrays.toString((boolean[]) obj);

        if (obj instanceof byte[])
            return Arrays.toString((byte[]) obj);

        if (obj instanceof char[])
            return Arrays.toString((char[]) obj);

        if (obj instanceof short[])
            return Arrays.toString((short[]) obj);

        if (obj instanceof int[])
            return Arrays.toString((int[]) obj);

        if (obj instanceof float[])
            return Arrays.toString((float[]) obj);

        if (obj instanceof long[])
            return Arrays.toString((long[]) obj);

        if (obj instanceof double[])
            return Arrays.toString((double[]) obj);

        if (obj instanceof Object[])
            return Arrays.toString((Object[]) obj);

        return obj.toString();
    }

    private static String toString(Class type, Map memberValues)
    {
        StringBuffer sb = new StringBuffer();
        sb.append('@').append(type.getName()).append('(');
        String sep = "";
        Iterator iter = memberValues.keySet().iterator();
        while (iter.hasNext())
        {
            Object key = iter.next();
            Object val = memberValues.get(key);
            sb.append(sep).append(key).append('=').append(deepToString(val));
            sep = ", ";
        }
        sb.append(')');
        return sb.toString();
    }
}
