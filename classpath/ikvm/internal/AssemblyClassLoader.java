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
    private final Assembly assembly;
    private ProtectionDomain pd;
    private boolean packagesDefined;

    public AssemblyClassLoader(Assembly assembly)
    {
        super(null);
        this.assembly = assembly;
    }

    protected Class loadClass(String name, boolean resolve) throws ClassNotFoundException
    {
        return LoadClass(this, name);
    }

    private static native Class LoadClass(Object classLoader, String name) throws ClassNotFoundException;

    public URL getResource(String name)
    {
        // for consistency with class loading, we change the delegation order for .class files
        // (this also helps make ikvmstub work reliably)
        if(name.endsWith(".class"))
        {
            URL url = getResource(this, name);
            if(url != null)
            {
                return url;
            }
        }
        return getResource(this, name);
    }

    public Enumeration getResources(String name) throws IOException
    {
        return getResources(this, name);
    }

    protected URL findResource(String name)
    {
        return getResource(this, name);
    }

    protected Enumeration findResources(String name) throws IOException
    {
        return getResources(this, name);
    }

    private static URL makeIkvmresURL(Assembly asm, String name)
    {
        String assemblyName = asm.get_FullName();
        if(IsReflectionOnly(asm))
        {
            assemblyName += "[ReflectionOnly]";
        }
        try
        {
            return new URL("ikvmres", assemblyName, -1, "/" + name);
        }
        catch(MalformedURLException x)
        {
            throw (InternalError)new InternalError().initCause(x);
        }
    }

    public static URL getResource(AssemblyClassLoader classLoader, String name)
    {
        Assembly[] asm = FindResourceAssemblies(classLoader, name, true);
        if(asm != null && asm.length > 0)
        {
            return makeIkvmresURL(asm[0], name);
        }
        else if(name.endsWith(".class") && name.indexOf('.') == name.length() - 6)
        {
            Class c = null;
            try
            {
                c = LoadClass(classLoader, name.substring(0, name.length() - 6).replace('/', '.'));
            }
            catch(ClassNotFoundException _)
            {
            }
            catch(LinkageError _)
            {
            }
            if(c != null)
            {
                return makeIkvmresURL(GetClassAssembly(c), name);
            }
        }
        return null;
    }

    private static native boolean IsReflectionOnly(Assembly asm);
    private static native Assembly[] FindResourceAssemblies(Object classLoader, String name, boolean firstOnly);
    private static native Assembly GetClassAssembly(Class c);
    // also used by VMClassLoader
    public static native String[] GetPackages(Object classLoader);

    public static Enumeration getResources(Object classLoader, String name) throws IOException
    {
        Assembly[] assemblies = FindResourceAssemblies(classLoader, name, false);
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
        if(assembly != null)
        {
            return assembly.get_FullName();
        }
        // TODO make this string more meaningful
        return "GenericClassLoader";
    }

    private URL getCodeBase()
    {
        try
        {
            if(assembly != null)
            {
                return new URL(assembly.get_CodeBase());
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
