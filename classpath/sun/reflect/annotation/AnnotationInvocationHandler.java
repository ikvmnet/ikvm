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

package sun.reflect.annotation;

import java.io.Serializable;
import java.lang.reflect.*;
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
        // TODO
        return false;
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
        if (method.getName().equals("toString") && (args == null || args.length == 0))
        {
            return toString(type, memberValues);
        }
            // TODO hashCode, equals, annotationType, ...
        else
        {
            return memberValues.get(method.getName());
        }
    }
}
