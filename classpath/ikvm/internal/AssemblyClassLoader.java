/*
  Copyright (C) 2006, 2007, 2010 Jeroen Frijters

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
    // NOTE assembly is null for "generics" class loader instances
    private final Assembly assembly;
    private boolean packagesDefined;

    public AssemblyClassLoader(Assembly assembly)
    {
        this(assembly, System.getSecurityManager());
    }

    // this constructor is used by the runtime to avoid the security check (by passing in null as the security manager)    
    AssemblyClassLoader(Assembly assembly, SecurityManager security)
    {
        super(null, security);
        this.assembly = assembly;
    }

    protected Class loadClass(String name, boolean resolve) throws ClassNotFoundException
    {
        return LoadClass(this, assembly, name);
    }

    private static native Class LoadClass(ClassLoader classLoader, Assembly assembly, String name) throws ClassNotFoundException;

    public URL getResource(String name)
    {
        return getResource(this, assembly, name);
    }

    public Enumeration getResources(String name) throws IOException
    {
        return getResources(this, assembly, name);
    }

    protected URL findResource(String name)
    {
        return getResource(this, assembly, name);
    }

    protected Enumeration findResources(String name) throws IOException
    {
        return getResources(this, assembly, name);
    }

    @Internal
    public static native URL getResource(ClassLoader classLoader, Assembly assembly, String name);
    
    @Internal
    public static native Enumeration getResources(ClassLoader classLoader, Assembly assembly, String name) throws IOException;

    private static native String GetGenericClassLoaderName(Object classLoader);
    // also used by java.lang.LangHelper
    @Internal
    public static native String[] GetPackages(Assembly assembly);

    private synchronized void lazyDefinePackagesCheck()
    {
        if(!packagesDefined)
        {
            packagesDefined = true;
            lazyDefinePackages();
        }
    }

    private static String getAttributeValue(Attributes.Name name, Attributes first, Attributes second)
    {
        String result = null;
        if(first != null)
        {
            result = first.getValue(name);
        }
        if(second != null && result == null)
        {
            result = second.getValue(name);
        }
        return result;
    }

    private Manifest getManifest()
    {
        try
        {
            if(assembly != null)
            {
                return new Manifest(gnu.java.net.protocol.ikvmres.Handler.readResourceFromAssembly(assembly, "/META-INF/MANIFEST.MF"));
            }
        }
        catch (MalformedURLException _)
        {
        }
        catch (IOException _)
        {
        }
        return null;
    }

    private void lazyDefinePackages()
    {
	if(assembly == null)
	{
	    // generic class loader (doesn't support packages)
	    return;
	}
        URL sealBase = getCodeBase();
        Manifest manifest = getManifest();
        Attributes attr = null;
        if(manifest != null)
        {
            attr = manifest.getMainAttributes();
        }
        String[] packages = GetPackages(assembly);
        for(int i = 0; i < packages.length; i++)
        {
            String name = packages[i];
            if(super.getPackage(name) == null)
            {
                Attributes entryAttr = null;
                if(manifest != null)
                {
                    entryAttr = manifest.getAttributes(name.replace('.', '/') + '/');
                }
                definePackage(name,
                    getAttributeValue(Attributes.Name.SPECIFICATION_TITLE, entryAttr, attr),
                    getAttributeValue(Attributes.Name.SPECIFICATION_VERSION, entryAttr, attr),
                    getAttributeValue(Attributes.Name.SPECIFICATION_VENDOR, entryAttr, attr),
                    getAttributeValue(Attributes.Name.IMPLEMENTATION_TITLE, entryAttr, attr),
                    getAttributeValue(Attributes.Name.IMPLEMENTATION_VERSION, entryAttr, attr),
                    getAttributeValue(Attributes.Name.IMPLEMENTATION_VENDOR, entryAttr, attr),
                    "true".equalsIgnoreCase(getAttributeValue(Attributes.Name.SEALED, entryAttr, attr)) ? sealBase : null);
            }
        }
    }

    protected Package getPackage(String name)
    {
        lazyDefinePackagesCheck();
        return super.getPackage(name);
    }

    protected Package[] getPackages()
    {
        lazyDefinePackagesCheck();
        return super.getPackages();
    }

    public String toString()
    {
        if(assembly != null)
        {
            return assembly.get_FullName();
        }
        return GetGenericClassLoaderName(this);
    }

    private URL getCodeBase()
    {
        try
        {
            if(assembly != null)
            {
                if(false) throw new cli.System.NotSupportedException();
                return new URL(assembly.get_CodeBase());
            }
        }
        catch(cli.System.NotSupportedException _)
        {
        }
        catch(MalformedURLException _)
        {
        }
        return null;
    }
    
    // return the ClassLoader for the assembly. Note that this doesn't have to be an AssemblyClassLoader.
    public static native ClassLoader getAssemblyClassLoader(Assembly asm);
}
