/*
  Copyright (C) 2005, 2006 Jeroen Frijters

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

import ikvm.lang.CIL;
import java.io.Serializable;
import java.lang.annotation.Annotation;
import java.lang.reflect.Array;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.lang.reflect.Proxy;
import java.util.Map;
import java.util.HashMap;
import sun.reflect.annotation.AnnotationInvocationHandler;

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
        return values.get(name);
    }

    protected final byte getByteValue(String name)
    {
        return ((Byte)values.get(name)).byteValue();
    }

    protected final boolean getBooleanValue(String name)
    {
        return ((Boolean)values.get(name)).booleanValue();
    }

    protected final short getShortValue(String name)
    {
        return ((Short)values.get(name)).shortValue();
    }

    protected final char getCharValue(String name)
    {
        return ((Character)values.get(name)).charValue();
    }

    protected final int getIntValue(String name)
    {
        return ((Integer)values.get(name)).intValue();
    }

    protected final float getFloatValue(String name)
    {
        return ((Float)values.get(name)).floatValue();
    }

    protected final long getLongValue(String name)
    {
        return ((Long)values.get(name)).longValue();
    }

    protected final double getDoubleValue(String name)
    {
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
    public static void freeze(Object ann)
    {
        if(ann instanceof AnnotationAttributeBase)
        {
            ((AnnotationAttributeBase)ann).freeze();
        }
    }

    private synchronized void freeze()
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
        return Proxy.newProxyInstance(annotationClass.getClassLoader(), new Class<?>[] { annotationClass }, new AnnotationInvocationHandler(annotationClass, map));
    }

    public static Object decodeElementValue(Object obj, Class type, ClassLoader loader)
        throws IllegalAccessException
    {
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
            new AnnotationInvocationHandler(annotationType, values));
    }

    public final Class<? extends Annotation> annotationType()
    {
        return annotationType;
    }

    public final boolean Equals(Object o)
    {
        return sun.reflect.annotation.AnnotationInvocationHandler.equals(annotationType, values, o);
    }

    public final int GetHashCode()
    {
        return sun.reflect.annotation.AnnotationInvocationHandler.hashCode(annotationType, values);
    }

    public final String ToString()
    {
        return sun.reflect.annotation.AnnotationInvocationHandler.toString(annotationType, values);
    }
}
