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

package java.lang;

import java.io.IOException;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.Enumeration;
import java.util.Map;

class LangHelper
{
    private static boolean addedSystemPackages;

    private static void addSystemPackage(Map pkgMap)
    {
        // NOTE caller must have acquired lock on pkgMap
        if (!addedSystemPackages)
        {
            addedSystemPackages = true;
            String[] pkgs = ikvm.internal.AssemblyClassLoader.GetPackages(null);
            URL sealBase = null;
            try
            {
                sealBase = new URL(cli.System.Reflection.Assembly.GetExecutingAssembly().get_CodeBase());
            }
            catch (MalformedURLException _)
            {
            }
            for (int i = 0; i < pkgs.length; i++)
            {
                pkgMap.put(pkgs[i],
                    new Package(pkgs[i],
                    "Java Platform API Specification",             // specTitle
                    "1.6",                                         // specVersion
                    "Sun Microsystems, Inc.",                      // specVendor
                    "Hybrid GNU Classpath / OpenJDK",              // implTitle
                    gnu.classpath.Configuration.CLASSPATH_VERSION, // implVersion
                    "Free Software Foundation & Sun Microsystems", // implVendor
                    sealBase,                                      // sealBase
                    null));                                        // class loader
            }
        }
    }

    /* this method gets called by Package.getSystemPackage() via a redefined method in map.xml */
    static Package getSystemPackage(Map pkgs, String name)
    {
        synchronized (pkgs)
        {
            addSystemPackage(pkgs);
            return (Package)pkgs.get(name);
        }
    }

    /* this method gets called by Package.getSystemPackages() via a redefined method in map.xml */
    static Package[] getSystemPackages(Map pkgs)
    {
        synchronized (pkgs)
        {
            addSystemPackage(pkgs);
	    return (Package[])pkgs.values().toArray(new Package[pkgs.size()]);

        }
    }

    static URL getBootstrapResource(String name)
    {
	return ikvm.internal.AssemblyClassLoader.getResource(null, name);
    }

    static Enumeration getBootstrapResources(String name) throws IOException
    {
	return ikvm.internal.AssemblyClassLoader.getResources(null, name);
    }

    static
    {
        sun.misc.SharedSecrets.setJavaLangAccess(new sun.misc.JavaLangAccess() {
	    public sun.reflect.ConstantPool getConstantPool(Class klass) {
		return klass.getConstantPool();
	    }
	    public void setAnnotationType(Class klass, sun.reflect.annotation.AnnotationType type) {
		klass.setAnnotationType(type);
	    }
	    public sun.reflect.annotation.AnnotationType getAnnotationType(Class klass) {
		return klass.getAnnotationType();
	    }
	    public <E extends Enum<E>>
		    E[] getEnumConstantsShared(Class<E> klass) {
		return klass.getEnumConstantsShared();
	    }
	    public void blockedOn(Thread t, sun.nio.ch.Interruptible b) {
		throw new Error("not implemented");
	    }
	});
    }
}
