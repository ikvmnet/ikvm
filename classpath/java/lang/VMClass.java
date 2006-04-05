/*
  Copyright (C) 2002, 2003, 2004, 2005 Jeroen Frijters

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
package java.lang;

import java.lang.annotation.Annotation;
import java.lang.reflect.Constructor;
import java.lang.reflect.Field;
import java.lang.reflect.VMFieldImpl;
import java.lang.reflect.Method;
import java.lang.reflect.Modifier;
import java.util.ArrayList;
import java.security.AccessController;
import java.security.PrivilegedAction;

@ikvm.lang.Internal
public abstract class VMClass
{
    private VMClass() {}

    public static Object getWrapper(Class c)
    {
        return c.vmdata;
    }

    // this method is used by the map.xml implementation of Class.newInstance
    static boolean isPublic(Constructor c)
    {
        return Modifier.isPublic(c.getModifiers())
            && Modifier.isPublic(GetModifiers(c.getDeclaringClass().vmdata, true));
    }

    // this method is used by the map.xml implementation of Class.newInstance
    static void checkAccess(Constructor c, Class caller) throws IllegalAccessException
    {
        VMFieldImpl.checkAccess(c.methodCookie, null, caller);
    }

    // this method is used by the map.xml implementation of Class.newInstance
    static Constructor getConstructor(Class c) throws InstantiationException
    {
        Constructor constructor = null;
        Constructor[] constructors = getDeclaredConstructors(c, false);
        for (int i = 0; i < constructors.length; i++)
        {
            if (constructors[i].getParameterTypes().length == 0)
            {
                constructor = constructors[i];
                break;
            }
        }
        if (constructor == null)
            throw new InstantiationException(c.getName());
        if (!Modifier.isPublic(constructor.getModifiers())
            || !Modifier.isPublic(GetModifiers(c.vmdata, true)))
        {
            final Constructor finalConstructor = constructor;
            AccessController.doPrivileged(new PrivilegedAction()
            {
                public Object run()
                {
                    finalConstructor.setAccessible(true);
                    return null;
                }
            });
        }
        return constructor;
    }

    static boolean isInstance(Class clazz, Object o)
    {
	return o != null && clazz.isAssignableFrom(o.getClass());
    }

    static boolean isAssignableFrom(Class clazz, Class c)
    {
	// this is implemented by the "native" code, because
	// remapped types can appear to implement interfaces that they don't
	// actually implement
	return IsAssignableFrom(clazz.vmdata, c.vmdata);
    }
    private static native boolean IsAssignableFrom(Object w1, Object w2);

    static boolean isInterface(Class clazz)
    {
	return IsInterface(clazz.vmdata);
    }
    private static native boolean IsInterface(Object wrapper);

    static boolean isPrimitive(Class clazz)
    {
	return clazz == boolean.class ||
	    clazz == byte.class ||
	    clazz == char.class ||
	    clazz == short.class ||
	    clazz == int.class ||
	    clazz == long.class ||
	    clazz == float.class ||
	    clazz == double.class ||
	    clazz == void.class;
    }

    static String getName(Class clazz)
    {
	// getName() is used by the classloader, so it shouldn't trigger a resolve of the class
	return GetName(clazz.vmdata);
    }
    private static native String GetName(Object wrapper);

    static Class getSuperclass(Class clazz)
    {
	return (Class)GetSuperClassFromWrapper(clazz.vmdata);
    }
    private native static Object GetSuperClassFromWrapper(Object wrapper);

    static Class[] getInterfaces(Class clazz)
    {
	Object[] interfaces = GetInterfaces(clazz.vmdata);
	Class[] interfacesClass = new Class[interfaces.length];
	System.arraycopy(interfaces, 0, interfacesClass, 0, interfaces.length);
	return interfacesClass;
    }
    private static native Object[] GetInterfaces(Object wrapper);

    static Class getComponentType(Class clazz)
    {
	// .NET array types can have unfinished element types, but we don't
	// want to expose those, so we may need to finish the type
	return (Class)getComponentClassFromWrapper(clazz.vmdata);
    }
    private static native Object getComponentClassFromWrapper(Object wrapper);

    static int getModifiers(Class clazz, boolean ignoreInnerClassesAttribute)
    {
	return GetModifiers(clazz.vmdata, ignoreInnerClassesAttribute);
    }
    private static native int GetModifiers(Object wrapper, boolean ignoreInnerClassesAttribute);

    static Class getDeclaringClass(Class clazz)
    {
	return (Class)GetDeclaringClass(clazz.vmdata);
    }
    private native static Object GetDeclaringClass(Object wrapper);

    static Class[] getDeclaredClasses(Class clazz, boolean publicOnly)
    {
	Object[] classes = GetDeclaredClasses(clazz.vmdata, publicOnly);
	Class[] classesClass = new Class[classes.length];
	System.arraycopy(classes, 0, classesClass, 0, classes.length);
	return classesClass;
    }
    private static native Object[] GetDeclaredClasses(Object wrapper, boolean publicOnly);

    static Field[] getDeclaredFields(Class clazz, boolean publicOnly)
    {
	Object[] fieldCookies = GetDeclaredFields(clazz.vmdata, publicOnly);
	Field[] fields = new Field[fieldCookies.length];
	for(int i = 0; i < fields.length; i++)
	{
	    fields[i] = VMFieldImpl.newField(clazz, fieldCookies[i]);
	}
	return fields;
    }
    private static native Object[] GetDeclaredFields(Object wrapper, boolean publicOnly);

    static Method[] getDeclaredMethods(Class clazz, boolean publicOnly)
    {
	Object[] methodCookies = GetDeclaredMethods(clazz.vmdata, true, publicOnly);
	Method[] methods = new Method[methodCookies.length];
	for(int i = 0; i < methodCookies.length; i++)
	{
	    methods[i] = new Method(clazz, methodCookies[i]);
	}
	return methods;
    }
    private static native Object[] GetDeclaredMethods(Object wrapper, boolean methods, boolean publicOnly);

    static Constructor[] getDeclaredConstructors(Class clazz, boolean publicOnly)
    {
	Object[] methodCookies = GetDeclaredMethods(clazz.vmdata, false, publicOnly);
	Constructor[] constructors = new Constructor[methodCookies.length];
	for(int i = 0; i < methodCookies.length; i++)
	{
	    constructors[i] = new Constructor(clazz, methodCookies[i]);
	}
	return constructors;
    }

    static ClassLoader getClassLoader(Class clazz)
    {
	// getClassLoader() can be used by the classloader, so it shouldn't trigger a resolve of the class
	return getClassLoader0(clazz.vmdata);
    }
    private static native ClassLoader getClassLoader0(Object wrapper);

    static native Class forName(String name, boolean initialize, ClassLoader loader);

    static native void throwException(Throwable t);

    static boolean isArray(Class clazz)
    {
	return IsArray(clazz.vmdata);
    }
    private static native boolean IsArray(Object wrapper);

    static Object cast(Object obj, Class k)
    {
        if(obj == null || k.isInstance(obj))
        {
            return obj;
        }
        throw new ClassCastException();
    }

    private static final int ACC_SYNTHETIC = 0x1000;
    private static final int ACC_ANNOTATION = 0x2000;
    private static final int ACC_ENUM = 0x4000;

    static boolean isSynthetic(Class clazz)
    {
        return (GetRawModifiers(clazz.vmdata) & ACC_SYNTHETIC) != 0;
    }

    static boolean isAnnotation(Class clazz)
    {
        return (GetRawModifiers(clazz.vmdata) & ACC_ANNOTATION) != 0;
    }

    static boolean isEnum(Class clazz)
    {
        return (GetRawModifiers(clazz.vmdata) & ACC_ENUM) != 0;
    }

    private static native int GetRawModifiers(Object wrapper);

  static String getSimpleName(Class clazz)
  {
    if (isArray(clazz))
      {
	return getSimpleName(getComponentType(clazz)) + "[]";
      }
    // TODO instead of this hack, we should figure out the proper way to do this
    String fullName = getName(clazz);
    return fullName.substring(Math.max(fullName.lastIndexOf("."), fullName.lastIndexOf("$")) + 1);
  }

  static Object[] getEnumConstants(Class clazz)
  {
    if (isEnum(clazz))
      {
        throw new Error("Not implemented");
      }
    else
      {
	return null;
      }
  }

  static Annotation[] getDeclaredAnnotations(Class clazz)
  {
    Object[] annotations = GetDeclaredAnnotations(clazz.vmdata);
    if (annotations == null)
    {
        return new Annotation[0];
    }
    // For the time being we filter out the .NET attributes
    // (that don't implement Annotation)
    ArrayList list = new ArrayList(annotations.length);
    for(int i = 0; i < annotations.length; i++)
    {
        if(annotations[i] instanceof Annotation)
        {
            list.add(annotations[i]);
        }
    }
    Annotation[] ar = new Annotation[list.size()];
    list.toArray(ar);
    return ar;
  }
  private static native Object[] GetDeclaredAnnotations(Object wrapper);

  static String getCanonicalName(Class clazz)
  {
    if (isArray(clazz))
      {
	String componentName = getCanonicalName(getComponentType(clazz));
	if (componentName != null)
	  return componentName + "[]";
      }
    if (isMemberClass(clazz))
      {
	String memberName = getCanonicalName(getDeclaringClass(clazz));
	if (memberName != null)
	  return memberName + "." + getSimpleName(clazz);
      }
    if (isLocalClass(clazz) || isAnonymousClass(clazz))
      return null;
    return getName(clazz);
  }

  static String getClassSignature(Class clazz)
  {
    return GetClassSignature(clazz.vmdata);
  }
  private static native String GetClassSignature(Object wrapper);

  static Class getEnclosingClass(Class clazz)
  {
    return (Class)GetEnclosingClass(clazz.vmdata);
  }
  private static native Object GetEnclosingClass(Object wrapper);

  static Constructor getEnclosingConstructor(Class clazz)
  {
    return (Constructor)GetEnclosingConstructor(clazz.vmdata);
  }
  private static native Object GetEnclosingConstructor(Object wrapper);

  static Method getEnclosingMethod(Class clazz)
  {
    return (Method)GetEnclosingMethod(clazz.vmdata);
  }
  private static native Object GetEnclosingMethod(Object wrapper);

  static boolean isAnonymousClass(Class clazz) { throw new Error("Not implemented"); }
  static boolean isLocalClass(Class clazz) { throw new Error("Not implemented"); }
  static boolean isMemberClass(Class clazz) { throw new Error("Not implemented"); }
}
