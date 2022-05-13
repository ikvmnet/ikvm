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
package sun.misc;

import cli.System.Reflection.Assembly;
import java.io.IOException;

class MiscHelper
{
    // map.xml replaces ExtClassLoader.getExtClassLoader() invocation in Launcher constructor with a call to this method
    static Launcher.ExtClassLoader getExtClassLoader() throws IOException
    {
        if ("".equals(System.getProperty("java.ext.dirs")) && "".equals(System.getProperty("java.class.path")))
        {
            return null;
        }
        return Launcher.ExtClassLoader.getExtClassLoader();
    }

    // map.xml replaces AppClassLoader.getAppClassLoader() invocation in Launcher constructor with a call to this method
    static ClassLoader getAppClassLoader(ClassLoader extcl) throws IOException
    {
        Assembly entryAssembly = Assembly.GetEntryAssembly();
        if (entryAssembly != null)
        {
            ClassLoader acl = getAssemblyClassLoader(entryAssembly, extcl);
            if (acl != null)
            {
                // assembly has a custom assembly class loader,
                // that overrides the Launcher.AppClassLoader
                return acl;
            }
        }
        return Launcher.AppClassLoader.getAppClassLoader(extcl);
    }

    private static native ClassLoader getAssemblyClassLoader(Assembly asm, ClassLoader extcl);
}
