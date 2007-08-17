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

package ikvm.internal;

public interface LibraryVMInterface
{
    Object newClass(Object wrapper, Object protectionDomain, Object classLoader);
    Object getWrapperFromClass(Object clazz);

    Object getWrapperFromClassLoader(Object classLoader);
    void setWrapperForClassLoader(Object classLoader, Object wrapper);

    Object box(Object val);
    Object unbox(Object val);

    Throwable mapException(Throwable t);

    cli.System.IntPtr getDirectBufferAddress(Object buffer);
    int getDirectBufferCapacity(Object buffer);

    void setProperties(cli.System.Collections.Hashtable props);

    boolean runFinalizersOnExit();

    Object newAnnotation(Object classLoader, Object definition);
    Object newAnnotationElementValue(Object classLoader, Object expectedClass, Object definition);

    Object newAssemblyClassLoader(cli.System.Reflection.Assembly assembly);

    void initProperties(java.util.Properties props);
    StackTraceElement[] getStackTrace(cli.System.Diagnostics.StackTrace stack);
}
