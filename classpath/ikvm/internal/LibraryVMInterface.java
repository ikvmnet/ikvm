/*
  Copyright (C) 2004 Jeroen Frijters

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

public interface LibraryVMInterface
{
    Object loadClass(Object classLoader, String name) throws ClassNotFoundException;
    Object newClass(Object wrapper, Object protectionDomain);
    Object getWrapperFromClass(Object clazz);
    Object getWrapperFromField(Object field);
    Object getWrapperFromMethodOrConstructor(Object methodOrConstructor);

    Object getWrapperFromClassLoader(Object classLoader);
    void setWrapperForClassLoader(Object classLoader, Object wrapper);

    Object getSystemClassLoader();

    Object box(Object val);
    Object unbox(Object val);

    Throwable mapException(Throwable t);
    void printStackTrace(Throwable t);

    void jniWaitUntilLastThread();
    void jniDetach();
    void setThreadGroup(Object group);

    Object newConstructor(Object clazz, Object wrapper);
    Object newMethod(Object clazz, Object wrapper);
    Object newField(Object clazz, Object wrapper);

    Object newDirectByteBuffer(cli.System.IntPtr address, int capacity);
    cli.System.IntPtr getDirectBufferAddress(Object buffer);
    int getDirectBufferCapacity(Object buffer);

    void setProperties(cli.System.Collections.Hashtable props);

    boolean runFinalizersOnExit();

    Object newAnnotation(Object classLoader, Object definition);
    Object newAnnotationElementValue(Object classLoader, Object expectedClass, Object definition);

    Throwable newIllegalAccessError(String msg);
    Throwable newIllegalAccessException(String msg);
    Throwable newIncompatibleClassChangeError(String msg);
    Throwable newLinkageError(String msg);
    Throwable newVerifyError(String msg);
    Throwable newClassCircularityError(String msg);
    Throwable newClassFormatError(String msg);
    Throwable newUnsupportedClassVersionError(String msg);
    Throwable newNoClassDefFoundError(String msg);
    Throwable newClassNotFoundException(String msg);
    Throwable newUnsatisfiedLinkError(String msg);
    Throwable newIllegalArgumentException(String msg);
    Throwable newNegativeArraySizeException();
    Throwable newArrayStoreException();
    Throwable newIndexOutOfBoundsException(String msg);
    Throwable newStringIndexOutOfBoundsException();
    Throwable newInvocationTargetException(Throwable t);
    Throwable newUnknownHostException(String msg);
    Throwable newArrayIndexOutOfBoundsException();
    Throwable newNumberFormatException(String msg);
    Throwable newNullPointerException();
    Throwable newClassCastException(String msg);
    Throwable newNoSuchFieldError(String msg);
    Throwable newNoSuchMethodError(String msg);
    Throwable newInstantiationError(String msg);
    Throwable newInstantiationException(String msg);
    Throwable newInterruptedException();
    Throwable newIllegalMonitorStateException();
}
