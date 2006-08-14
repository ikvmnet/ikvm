/*
  Copyright (C) 2004, 2005, 2006 Jeroen Frijters

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

import java.lang.reflect.Field;
import java.lang.reflect.VMFieldImpl;
import java.lang.reflect.Method;
import java.lang.reflect.Constructor;
import java.lang.reflect.InvocationTargetException;
import java.security.AccessController;
import java.security.PrivilegedAction;
import ikvm.internal.AnnotationAttributeBase;
import ikvm.internal.AssemblyClassLoader;
import ikvm.lang.CIL;

class LibraryVMInterfaceImpl implements ikvm.internal.LibraryVMInterface
{
    public Object loadClass(Object classLoader, String name) throws ClassNotFoundException
    {
        return ((ClassLoader)classLoader).loadClass(name).vmdata;
    }

    public Object newClass(Object wrapper, Object protectionDomain)
    {
        return new Class(wrapper, (java.security.ProtectionDomain)protectionDomain);
    }

    public Object newField(Object clazz, Object wrapper)
    {
        return VMFieldImpl.newField((Class)clazz, wrapper);
    }

    public Object newConstructor(Object clazz, Object wrapper)
    {
        return new Constructor((Class)clazz, wrapper);
    }

    public Object newMethod(Object clazz, Object wrapper)
    {
        return new Method((Class)clazz, wrapper);
    }

    public Object getWrapperFromClass(Object clazz)
    {
        return ((Class)clazz).vmdata;
    }

    public Object getWrapperFromField(Object field)
    {
        return ((Field)field).impl.fieldCookie;
    }

    public Object getWrapperFromMethodOrConstructor(Object methodOrConstructor)
    {
        if(methodOrConstructor instanceof Method)
        {
            return ((Method)methodOrConstructor).methodCookie;
        }
        else
        {
            return ((Constructor)methodOrConstructor).methodCookie;
        }
    }

    public Object getWrapperFromClassLoader(Object classLoader)
    {
        return ((ClassLoader)classLoader).vmdata;
    }

    public void setWrapperForClassLoader(Object classLoader, Object wrapper)
    {
        ((ClassLoader)classLoader).vmdata = wrapper;
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

    public void printStackTrace(Throwable t)
    {
        t.printStackTrace();
    }

    public void jniWaitUntilLastThread()
    {
        VMThread.jniWaitUntilLastThread();
    }

    public void jniDetach()
    {
        VMThread.jniDetach();
    }

    public void setThreadGroup(Object group)
    {
        VMThread.setThreadGroup((ThreadGroup)group);
    }

    public Object newDirectByteBuffer(cli.System.IntPtr address, int capacity)
    {
        return java.nio.VMDirectByteBuffer.NewDirectByteBuffer(address, capacity);
    }

    public cli.System.IntPtr getDirectBufferAddress(Object buffer)
    {
        return java.nio.VMDirectByteBuffer.GetDirectBufferAddress((java.nio.Buffer)buffer);
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
        return VMRuntime.runFinalizersOnExitFlag;
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

    public Throwable newIllegalAccessError(String msg)
    {
        return new IllegalAccessError(msg);
    }

    public Throwable newIllegalAccessException(String msg)
    {
        return new IllegalAccessException(msg);
    }

    public Throwable newIncompatibleClassChangeError(String msg)
    {
        return new IncompatibleClassChangeError(msg);
    }

    public Throwable newLinkageError(String msg)
    {
        return new LinkageError(msg);
    }

    public Throwable newVerifyError(String msg)
    {
        return new VerifyError(msg);
    }

    public Throwable newClassCircularityError(String msg)
    {
        return new ClassCircularityError(msg);
    }

    public Throwable newClassFormatError(String msg)
    {
        return new ClassFormatError(msg);
    }

    public Throwable newUnsupportedClassVersionError(String msg)
    {
        return new UnsupportedClassVersionError(msg);
    }

    public Throwable newNoClassDefFoundError(String msg)
    {
        return new NoClassDefFoundError(msg);
    }

    public Throwable newClassNotFoundException(String msg)
    {
        return new ClassNotFoundException(msg);
    }

    public Throwable newUnsatisfiedLinkError(String msg)
    {
        return new UnsatisfiedLinkError(msg);
    }

    public Throwable newIllegalArgumentException(String msg)
    {
        return new IllegalArgumentException(msg);
    }

    public Throwable newNegativeArraySizeException()
    {
        return new NegativeArraySizeException();
    }

    public Throwable newArrayStoreException()
    {
        return new ArrayStoreException();
    }

    public Throwable newIndexOutOfBoundsException(String msg)
    {
        return new IndexOutOfBoundsException(msg);
    }

    public Throwable newStringIndexOutOfBoundsException()
    {
        return new StringIndexOutOfBoundsException();
    }

    public Throwable newInvocationTargetException(Throwable t)
    {
        return new InvocationTargetException(t);
    }

    public Throwable newUnknownHostException(String msg)
    {
        return new java.net.UnknownHostException(msg);
    }

    public Throwable newArrayIndexOutOfBoundsException()
    {
        return new ArrayIndexOutOfBoundsException();
    }

    public Throwable newNumberFormatException(String msg)
    {
        return new NumberFormatException(msg);
    }

    public Throwable newNullPointerException()
    {
        return new NullPointerException();
    }

    public Throwable newClassCastException(String msg)
    {
        return new ClassCastException(msg);
    }

    public Throwable newNoSuchFieldError(String msg)
    {
        return new NoSuchFieldError(msg);
    }

    public Throwable newNoSuchMethodError(String msg)
    {
        return new NoSuchMethodError(msg);
    }

    public Throwable newInstantiationError(String msg)
    {
        return new InstantiationError(msg);
    }

    public Throwable newInstantiationException(String msg)
    {
        return new InstantiationException(msg);
    }

    public Throwable newInterruptedException()
    {
        return new InterruptedException();
    }

    public Throwable newIllegalMonitorStateException()
    {
        return new IllegalMonitorStateException();
    }

    public Object newAssemblyClassLoader()
    {
        return AccessController.doPrivileged(new PrivilegedAction() {
            public Object run() {
                return new AssemblyClassLoader();
            }
        });
    }
}
