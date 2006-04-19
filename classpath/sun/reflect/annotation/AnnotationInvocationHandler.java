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

package sun.reflect.annotation;

import java.io.Serializable;
import java.lang.reflect.*;
import java.util.Arrays;
import java.util.Iterator;
import java.util.Map;

public class AnnotationInvocationHandler implements InvocationHandler, Serializable
{
    private static final long serialVersionUID = 6182022883658399397L;
    private Class type;
    private Map memberValues;

    public AnnotationInvocationHandler(Class type, Map memberValues)
    {
        this.type = type;
        this.memberValues = memberValues;
    }

    public static boolean equals(Class type, Map memberValues, Object other)
    {
        if (type.isInstance(other))
        {
            try
            {
                Method[] methods = type.getDeclaredMethods();
                for (int i = 0; i < methods.length; i++)
                {
                    Object val = methods[i].invoke(other, new Object[0]);
                    if (!deepEquals(val, memberValues.get(methods[i].getName())))
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (IllegalAccessException _)
            {
            }
            catch (InvocationTargetException _)
            {
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
            return Arrays.equals((boolean[])o1, (boolean[])o2);

        if (o1 instanceof byte[] && o2 instanceof byte[])
            return Arrays.equals((byte[])o1, (byte[])o2);

        if (o1 instanceof char[] && o2 instanceof char[])
            return Arrays.equals((char[])o1, (char[])o2);

        if (o1 instanceof short[] && o2 instanceof short[])
            return Arrays.equals((short[])o1, (short[])o2);

        if (o1 instanceof int[] && o2 instanceof int[])
            return Arrays.equals((int[])o1, (int[])o2);

        if (o1 instanceof float[] && o2 instanceof float[])
            return Arrays.equals((float[])o1, (float[])o2);

        if (o1 instanceof long[] && o2 instanceof long[])
            return Arrays.equals((long[])o1, (long[])o2);

        if (o1 instanceof double[] && o2 instanceof double[])
            return Arrays.equals((double[])o1, (double[])o2);

        if (o1 instanceof Object[] && o2 instanceof Object[])
            return Arrays.equals((Object[])o1, (Object[])o2);

        return o1.equals(o2);
    }

    public static int hashCode(Class type, Map memberValues)
    {
        // TODO
        return 0;
    }

    public static String toString(Class type, Map memberValues)
    {
        StringBuffer sb = new StringBuffer();
        sb.append('@').append(type.getName()).append('(');
        String sep = "";
        // TODO instead of keying of the map, we should iterate the methods
        Iterator iter = memberValues.keySet().iterator();
        while (iter.hasNext())
        {
            String name = (String)iter.next();
            sb.append(sep).append(name).append('=').append(memberValues.get(name));
            sep = ", ";
        }
        sb.append(')');
        return sb.toString();
    }

    public Object invoke(Object proxy, Method method, Object[] args) throws Throwable
    {
        String methodName = method.getName().intern();
        if (args == null || args.length == 0)
        {
            if (methodName == "toString")
            {
                return toString(type, memberValues);
            }
            else if (methodName == "hashCode")
            {
                return Integer.valueOf(hashCode(type, memberValues));
            }
            else if (methodName == "annotationType")
            {
                return type;
            }
            else
            {
                return memberValues.get(methodName);
            }
        }
        else if (args.length == 1)
        {
            if (methodName == "equals")
            {
                return Boolean.valueOf(equals(type, memberValues, args[0]));
            }
        }
        throw new Error("invalid annotation proxy");
    }
}
