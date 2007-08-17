/*
  Copyright (C) 2004, 2005, 2006, 2007 Jeroen Frijters

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

import cli.System.Reflection.BindingFlags;
import java.lang.reflect.Field;
import java.lang.reflect.Method;
import java.lang.reflect.Constructor;
import java.lang.reflect.InvocationTargetException;
import java.security.AccessController;
import java.security.PrivilegedAction;
import ikvm.internal.AnnotationAttributeBase;
import ikvm.internal.AssemblyClassLoader;
import ikvm.lang.CIL;
import ikvm.lang.Internal;

@Internal
public class LibraryVMInterfaceImpl implements ikvm.internal.LibraryVMInterface
{
    private static final cli.System.Reflection.ConstructorInfo classConstructor;
    private static final cli.System.Reflection.FieldInfo classTypeWrapperField;
    private static final cli.System.Reflection.FieldInfo classProtectionDomainField;
    private static final cli.System.Reflection.FieldInfo classLoaderWrapperField;
    // this flag is set by Shutdown.runAllFinalizers() in map.xml
    static volatile boolean runFinalizersOnExitFlag;

    static
    {
	cli.System.Type classType = cli.System.Type.GetType("java.lang.Class");
	classConstructor = classType.GetConstructor(BindingFlags.wrap(BindingFlags.NonPublic | BindingFlags.Instance), null, new cli.System.Type[0], null);
	classTypeWrapperField = classType.GetField("typeWrapper", BindingFlags.wrap(BindingFlags.NonPublic | BindingFlags.Instance));
	classProtectionDomainField = classType.GetField("pd", BindingFlags.wrap(BindingFlags.NonPublic | BindingFlags.Instance));

	cli.System.Type classLoaderType = cli.System.Type.GetType("java.lang.ClassLoader");
	classLoaderWrapperField = classLoaderType.GetField("wrapper", BindingFlags.wrap(BindingFlags.NonPublic | BindingFlags.Instance));
    }

    public Object newClass(Object wrapper, Object protectionDomain, Object classLoader)
    {
	Object clazz = classConstructor.Invoke(null);
	classTypeWrapperField.SetValue(clazz, wrapper);
	classProtectionDomainField.SetValue(clazz, protectionDomain);
	return clazz;
    }

    public Object getWrapperFromClass(Object clazz)
    {
	return classTypeWrapperField.GetValue(clazz);
    }

    public Object getWrapperFromClassLoader(Object classLoader)
    {
	return classLoaderWrapperField.GetValue(classLoader);
    }

    public void setWrapperForClassLoader(Object classLoader, Object wrapper)
    {
	classLoaderWrapperField.SetValue(classLoader, wrapper);
    }

    public Object box(Object val)
    {
        if(val instanceof cli.System.Byte)
        {
            return new Byte(CIL.unbox_byte(val));
        }
        else if(val instanceof cli.System.Boolean)
        {
            return new Boolean(CIL.unbox_boolean(val));
        }
        else if(val instanceof cli.System.Int16)
        {
            return new Short(CIL.unbox_short(val));
        }
        else if(val instanceof cli.System.Char)
        {
            return new Character(CIL.unbox_char(val));
        }
        else if(val instanceof cli.System.Int32)
        {
            return new Integer(CIL.unbox_int(val));
        }
        else if(val instanceof cli.System.Single)
        {
            return new Float(CIL.unbox_float(val));
        }
        else if(val instanceof cli.System.Int64)
        {
            return new Long(CIL.unbox_long(val));
        }
        else if(val instanceof cli.System.Double)
        {
            return new Double(CIL.unbox_double(val));
        }
        else
        {
            throw new IllegalArgumentException();
        }
    }
    
    public Object unbox(Object val)
    {
        if(val instanceof Byte)
        {
            return CIL.box_byte(((Byte)val).byteValue());
        }
        else if(val instanceof Boolean)
        {
            return CIL.box_boolean(((Boolean)val).booleanValue());
        }
        else if(val instanceof Short)
        {
            return CIL.box_short(((Short)val).shortValue());
        }
        else if(val instanceof Character)
        {
            return CIL.box_char(((Character)val).charValue());
        }
        else if(val instanceof Integer)
        {
            return CIL.box_int(((Integer)val).intValue());
        }
        else if(val instanceof Float)
        {
            return CIL.box_float(((Float)val).floatValue());
        }
        else if(val instanceof Long)
        {
            return CIL.box_long(((Long)val).longValue());
        }
        else if(val instanceof Double)
        {
            return CIL.box_double(((Double)val).doubleValue());
        }
        else
        {
            throw new IllegalArgumentException();
        }
    }

    public Throwable mapException(Throwable t)
    {
        return ExceptionHelper.MapExceptionFast(t, true);
    }

    public cli.System.IntPtr getDirectBufferAddress(Object buffer)
    {
        return cli.System.IntPtr.op_Explicit(((sun.nio.ch.DirectBuffer)buffer).address());
    }
    
    public int getDirectBufferCapacity(Object buffer)
    {
        return ((java.nio.Buffer)buffer).capacity();
    }

    public void setProperties(cli.System.Collections.Hashtable props)
    {
        gnu.classpath.VMSystemProperties.props = props;
    }

    public boolean runFinalizersOnExit()
    {
        return runFinalizersOnExitFlag;
    }

    public Object newAnnotation(Object classLoader, Object definition)
    {
        return AnnotationAttributeBase.newAnnotation((ClassLoader)classLoader, definition);
    }

    public Object newAnnotationElementValue(Object classLoader, Object expectedClass, Object definition)
    {
        try
        {
            return AnnotationAttributeBase.decodeElementValue(definition, (Class)expectedClass, (ClassLoader)classLoader);
        }
        catch (IllegalAccessException x)
        {
            // TODO this shouldn't be here
            return null;
        }
    }

    public Object newAssemblyClassLoader(final cli.System.Reflection.Assembly assembly)
    {
        return AccessController.doPrivileged(new PrivilegedAction() {
            public Object run() {
                return new AssemblyClassLoader(assembly);
            }
        });
    }

    public void initProperties(java.util.Properties props)
    {
	gnu.classpath.VMSystemProperties.initOpenJDK(props);
    }

    public StackTraceElement[] getStackTrace(cli.System.Diagnostics.StackTrace stack)
    {
	return ExceptionHelper.getStackTrace(stack, Integer.MAX_VALUE);
    }
}
