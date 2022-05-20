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

import cli.System.Reflection.Assembly;
import java.io.File;
import java.net.URL;
import java.net.URLClassLoader;
import java.util.ArrayList;
import java.util.StringTokenizer;
import sun.net.www.ParseUtil;

public final class ClassPathAssemblyClassLoader extends URLClassLoader
{
    public ClassPathAssemblyClassLoader(Assembly assembly)
    {
	super(buildURLs(), new AssemblyClassLoader(assembly));
    }

    private static URL[] buildURLs()
    {
	// we can assume we're already runnning in a privileged context
	// (otherwise you wouldn't be able to construct a ClassLoader anyway)
	String classpath = System.getProperty("java.class.path", "");
	if (classpath.length() == 0)
	{
	    classpath = System.getenv("CLASSPATH");
	    if (classpath == null || classpath.length() == 0)
	    {
		classpath = ".";
	    }
	}
	// Copyright note: this code is identical to the code in GNU Classpath's ClassLoader,
	// but I wrote that code so I co-own the copyright and can dual license it to use
	// here under the zlib license.
	StringTokenizer tok = new StringTokenizer(classpath, File.pathSeparator, true);
	ArrayList list = new ArrayList();
	while (tok.hasMoreTokens())
	{
	    String s = tok.nextToken();
	    if (s.equals(File.pathSeparator))
	    {
		addFileURL(list, ".");
	    }
	    else
	    {
		addFileURL(list, s);
		if (tok.hasMoreTokens())
		{
		    // Skip the separator.
		    tok.nextToken();
		    // If the classpath ended with a separator,
		    // append the current directory.
		    if (!tok.hasMoreTokens())
		    {
			addFileURL(list, ".");
		    }
		}
	    }
	}
	URL[] urls = new URL[list.size()];
	list.toArray(urls);
	return urls;
    }

    private static void addFileURL(ArrayList list, String pathname)
    {
	try
	{
	    File file = new File(pathname);
	    try
	    {
		file = file.getCanonicalFile();
	    }
	    catch (java.io.IOException _)
	    {
	    }
	    list.add(ParseUtil.fileToEncodedURL(file));
	}
	catch (java.net.MalformedURLException x)
	{
	    throw new InternalError();
	}
    }
}
