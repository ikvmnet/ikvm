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

import gnu.classpath.VMSystemProperties;
import java.io.IOException;
import java.net.MalformedURLException;
import java.net.URL;
import java.security.AccessController;
import java.util.Enumeration;
import java.util.Map;
import sun.security.action.GetPropertyAction;

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
	    String openJdkVersion = AccessController.doPrivileged(new GetPropertyAction("openjdk.version", "unknown"));
            for (int i = 0; i < pkgs.length; i++)
            {
                pkgMap.put(pkgs[i],
                    new Package(pkgs[i],
		    VMSystemProperties.SPEC_TITLE,                 // specTitle
		    VMSystemProperties.SPEC_VERSION,               // specVersion
		    VMSystemProperties.SPEC_VENDOR,                // specVendor
                    "IKVM.NET OpenJDK",                            // implTitle
                    openJdkVersion,                                // implVersion
                    "Sun Microsystems, Inc. & others",             // implVendor
                    null,                                          // sealBase
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

    // this method is called from the System.<clinit> method in map.xml
    static void init()
    {
	cli.System.AppDomain.get_CurrentDomain().add_ProcessExit(new cli.System.EventHandler(new cli.System.EventHandler.Method() {
	    public void Invoke(Object sender, cli.System.EventArgs e) {
		Shutdown.shutdown();
	    }
	}));
    }
}
