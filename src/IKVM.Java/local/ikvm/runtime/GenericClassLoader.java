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
