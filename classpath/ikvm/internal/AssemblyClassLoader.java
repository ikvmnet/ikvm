/*
  Copyright (C) 2006-2013 Jeroen Frijters

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
// HACK because of historical reasons this class' source lives in ikvm/internal instead of ikvm/runtime
package ikvm.runtime;

import cli.System.Reflection.Assembly;
import gnu.java.util.EmptyEnumeration;
import ikvm.lang.Internal;
import java.io.IOException;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.Enumeration;
import java.util.Vector;
import java.util.jar.Attributes;
import java.util.jar.Manifest;

public final class AssemblyClassLoader extends ClassLoader
{
    private boolean packagesDefined;

    // This constructor is used to manually construct an AssemblyClassLoader that is used
    // as a delegation parent for custom assembly class loaders.
    //
    // In that case the class loader object graph looks like this:
    //
    //            +---------------------------------+
    //            |IKVM.Internal.AssemblyClassLoader|
    //            +---------------------------------+
    //              ||     /\                  /\
    //              \/     ||                  ||
    //    +-------------------+                ||
    //    |Custom Class Loader|      +--------------------------------+
    //    +-------------------+      |ikvm.runtime.AssemblyClassLoader|
    //                               +--------------------------------+
    //
    public AssemblyClassLoader(Assembly assembly)
    {
        super(null);
        setWrapper(assembly);
    }

    private native void setWrapper(Assembly assembly);

    // this constructor is used by the runtime and calls a privileged
    // ClassLoader constructor to avoid the security check
    AssemblyClassLoader()
    {
        super(null, null);
    }

    @Override
    protected native Class loadClass(String name, boolean resolve) throws ClassNotFoundException;

    @Override
    public native URL getResource(String name);

    @Override
    public native Enumeration<URL> getResources(String name) throws IOException;

    @Override
    protected native URL findResource(String name);

    @Override
    protected native Enumeration<URL> findResources(String name) throws IOException;

    private synchronized void lazyDefinePackagesCheck()
    {
        if(!packagesDefined)
        {
            packagesDefined = true;
            lazyDefinePackages();
        }
    }

    private native void lazyDefinePackages();

    @Override
    protected Package getPackage(String name)
    {
        lazyDefinePackagesCheck();
        return super.getPackage(name);
    }

    @Override
    protected Package[] getPackages()
    {
        lazyDefinePackagesCheck();
        return super.getPackages();
    }

    @Override
    public native String toString();

    // return the ClassLoader for the assembly. Note that this doesn't have to be an AssemblyClassLoader.
    public static native ClassLoader getAssemblyClassLoader(Assembly asm);
}

final class GenericClassLoader extends ClassLoader
{
    // this constructor avoids the security check in ClassLoader by passing in null as the security manager
    // to the IKVM specific constructor in ClassLoader
    GenericClassLoader()
    {
        super(null, null);
    }

    @Override
    public native String toString();

    @Override
    public URL getResource(String name)
    {
        Enumeration<URL> e = getResources(name);
        return e.hasMoreElements()
            ? e.nextElement()
            : null;
    }

    @Override
    public native Enumeration<URL> getResources(String name);

    @Override
    protected native URL findResource(String name);

    @Override
    protected Enumeration<URL> findResources(String name)
    {
        Vector<URL> v = new Vector<URL>();
        URL url = findResource(name);
        if (url != null)
        {
            v.add(url);
        }
        return v.elements();
    }
}
