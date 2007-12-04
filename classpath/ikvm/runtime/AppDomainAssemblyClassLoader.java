/*
  Copyright (C) 2007 Jeroen Frijters

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
package ikvm.runtime;

import cli.System.AppDomain;
import cli.System.Reflection.Assembly;
import java.io.IOException;
import java.net.URL;
import java.util.Enumeration;
import java.util.Vector;

public final class AppDomainAssemblyClassLoader extends ClassLoader
{
    public AppDomainAssemblyClassLoader(Assembly assembly)
    {
	super(new AssemblyClassLoader(assembly));
    }

    protected Class findClass(String name) throws ClassNotFoundException
    {
	Assembly[] assemblies = AppDomain.get_CurrentDomain().GetAssemblies();
	for (int i = 0; i < assemblies.length; i++)
	{
	    Class c = loadClassFromAssembly(assemblies[i], name);
	    if (c != null)
	    {
		return c;
	    }
	}
	throw new ClassNotFoundException(name);
    }

    private static native Class loadClassFromAssembly(Assembly asm, String className);

    protected URL findResource(String name)
    {
	Assembly[] assemblies = AppDomain.get_CurrentDomain().GetAssemblies();
	for (int i = 0; i < assemblies.length; i++)
	{
	    if (findResourceInAssembly(assemblies[i], name))
	    {
		return AssemblyClassLoader.makeIkvmresURL(assemblies[i], name);
	    }
	}
	return null;
    }

    private static native boolean findResourceInAssembly(Assembly asm, String resourceName);

    // we override getResources() instead of findResources() to be able to filter duplicates
    public Enumeration<URL> getResources(String name) throws IOException
    {
	Vector<URL> v = new Vector<URL>();
	for (Enumeration<URL> e = super.getResources(name); e.hasMoreElements(); )
	{
	    v.add(e.nextElement());
	}
	Assembly[] assemblies = AppDomain.get_CurrentDomain().GetAssemblies();
	for (int i = 0; i < assemblies.length; i++)
	{
	    if (findResourceInAssembly(assemblies[i], name))
	    {
		URL url = AssemblyClassLoader.makeIkvmresURL(assemblies[i], name);
		if (!v.contains(url))
		{
		    v.add(url);
		}
	    }
	}
	return v.elements();
    }
}
