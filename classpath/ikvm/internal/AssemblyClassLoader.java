/*
  Copyright (C) 2006 Jeroen Frijters

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

import cli.System.Reflection.Assembly;
import gnu.java.util.DoubleEnumeration;
import gnu.java.util.EmptyEnumeration;
import ikvm.lang.Internal;
import java.io.IOException;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.Enumeration;
import java.util.Vector;
import java.util.jar.Attributes;
import java.util.jar.Manifest;
import java.security.AllPermission;
import java.security.CodeSource;
import java.security.Permissions;
import java.security.ProtectionDomain;

@Internal
public final class AssemblyClassLoader extends ClassLoader
{
    private ProtectionDomain pd;

    public AssemblyClassLoader()
    {
        super(null);
    }

    protected synchronized Class loadClass(String name, boolean resolve)
        throws ClassNotFoundException
    {
        return LoadClass(this, name);
    }

    private static native Class LoadClass(Object classLoader, String name) throws ClassNotFoundException;

    public URL getResource(String name)
    {
        URL url = getResource(this, name);
        if(url == null)
        {
            url = super.getResource(name);
        }
        return url;
    }

    private static URL makeIkvmresURL(Assembly asm, String name) throws MalformedURLException
    {
        String assemblyName = asm.get_FullName();
        if(IsReflectionOnly(asm))
        {
            assemblyName += "[ReflectionOnly]";
        }
        return new URL("ikvmres", assemblyName, -1, "/" + name);
    }

    public static URL getResource(Object classLoader, String name)
    {
        Assembly asm = FindResourceAssembly(classLoader, name);
        if(asm != null)
        {
            try
            {
                return makeIkvmresURL(asm, name);
            }
            catch(MalformedURLException x)
            {
                throw (InternalError)new InternalError().initCause(x);
            }
        }
        else if(name.endsWith(".class") && name.indexOf('.') == name.length() - 6)
        {
            try
            {
                Class c = LoadClass(classLoader, name.substring(0, name.length() - 6).replace('/', '.'));
                if(c != null)
                {
                    try
                    {
                        return makeIkvmresURL(GetClassAssembly(c), name);
                    }
                    catch(MalformedURLException x)
                    {
                        throw (InternalError)new InternalError().initCause(x);
                    }
                }
            }
            catch(ClassNotFoundException _)
            {
            }
        }
        return null;
    }

    private static native boolean IsReflectionOnly(Assembly asm);
    private static native Assembly FindResourceAssembly(Object classLoader, String name);
    private static native Assembly[] FindResourceAssemblies(Object classLoader, String name);
    private static native Assembly GetClassAssembly(Class c);
    private static native Assembly GetAssembly(Object classLoader);
    // also used by VMClassLoader
    public static native String[] GetPackages(Object classLoader);

    public Enumeration getResources(String name) throws IOException
    {
        return new DoubleEnumeration(getResources(this, name), super.getResources(name));
    }

    public static Enumeration getResources(Object classLoader, String name) throws IOException
    {
        Assembly[] assemblies = FindResourceAssemblies(classLoader, name);
        if(assemblies != null)
        {
            Vector v = new Vector();
            for(int i = 0; i < assemblies.length; i++)
            {
                v.addElement(makeIkvmresURL(assemblies[i], name));
            }
            return v.elements();
        }
        return EmptyEnumeration.getInstance();
    }

    private static boolean packagesDefined;

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
            Assembly asm = GetAssembly(this);
            if(asm != null)
            {
                return new Manifest(gnu.java.net.protocol.ikvmres.Handler.readResourceFromAssembly(asm, "/META-INF/MANIFEST.MF"));
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
        URL sealBase = getCodeBase();
        Manifest manifest = getManifest();
        Attributes attr = null;
        if(manifest != null)
        {
            attr = manifest.getMainAttributes();
        }
        String[] packages = GetPackages(this);
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
                    sealBase);
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
        Assembly asm = GetAssembly(this);
        if(asm != null)
        {
            return asm.get_FullName();
        }
        // TODO make this string more meaningful
        return "GenericClassLoader";
    }

    private URL getCodeBase()
    {
        try
        {
            Assembly asm = GetAssembly(this);
            if(asm != null)
            {
                return new URL(asm.get_CodeBase());
            }
        }
        catch(MalformedURLException _)
        {
        }
        return null;
    }

    public synchronized ProtectionDomain getProtectionDomain()
    {
        if(pd == null)
        {
            Permissions permissions = new Permissions();
            permissions.add(new AllPermission());
            pd = new ProtectionDomain(new CodeSource(getCodeBase(), null), permissions, this, null);
        }
        return pd;
    }
}
