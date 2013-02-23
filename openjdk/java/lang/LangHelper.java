/*
  Copyright (C) 2007-2011 Jeroen Frijters

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

import ikvm.runtime.AssemblyClassLoader;
import java.io.IOException;
import java.net.MalformedURLException;
import java.net.URL;
import java.security.AccessController;
import java.util.Enumeration;
import java.util.Map;
import sun.nio.ch.Interruptible;
import sun.reflect.annotation.AnnotationType;
import sun.security.action.GetPropertyAction;

@ikvm.lang.Internal
public class LangHelper
{
    private static boolean addedSystemPackages;

    private static void addSystemPackage(Map pkgMap)
    {
        // NOTE caller must have acquired lock on pkgMap
        if (!addedSystemPackages)
        {
            addedSystemPackages = true;
            String[] pkgs = getBootClassPackages();
            for (int i = 0; i < pkgs.length; i++)
            {
                pkgMap.put(pkgs[i],
                    new Package(pkgs[i],
                    VMSystemProperties.SPEC_TITLE,                 // specTitle
                    VMSystemProperties.SPEC_VERSION,               // specVersion
                    VMSystemProperties.SPEC_VENDOR,                // specVendor
                    "IKVM.NET OpenJDK",                            // implTitle
                    PropertyConstants.openjdk_version,             // implVersion
                    "Oracle Corporation & others",                 // implVendor
                    null,                                          // sealBase
                    null));                                        // class loader
            }
        }
    }
    
    private static native String[] getBootClassPackages();

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

    public static sun.misc.JavaLangAccess getJavaLangAccess()
    {
        return new sun.misc.JavaLangAccess() {
            public sun.reflect.ConstantPool getConstantPool(Class klass) {
                return null;
            }
            public void setAnnotationType(Class klass, AnnotationType type) {
                klass.setAnnotationType(type);
            }
            public AnnotationType getAnnotationType(Class klass) {
                return klass.getAnnotationType();
            }
            public <E extends Enum<E>>
                    E[] getEnumConstantsShared(Class<E> klass) {
                return klass.getEnumConstantsShared();
            }
            public void blockedOn(Thread t, Interruptible b) {
                t.blockedOn(b);
            }
            public void registerShutdownHook(int slot, boolean registerShutdownInProgress, Runnable hook) {
                Shutdown.add(slot, registerShutdownInProgress, hook);
            }
            public int getStackTraceDepth(Throwable t) {
                return t.getStackTraceDepth();
            }
            public StackTraceElement getStackTraceElement(Throwable t, int i) {
                return t.getStackTraceElement(i);
            }
            public int getStringHash32(String string) {
                return StringHelper.hash32(string);
            }
        };
    }
}
