/*
  Copyright (C) 2007-2015 Jeroen Frijters
  Copyright (C) 2009 Volker Berlin (i-net software)

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
using System;
using System.Collections.Generic;

static class Java_java_util_jar_JarFile
{
    public static string[] getMetaInfEntryNames(object thisJarFile)
    {
#if FIRST_PASS
        return null;
#else
        java.util.zip.ZipFile zf = (java.util.zip.ZipFile)thisJarFile;
        java.util.Enumeration entries = zf.entries();
        List<string> list = null;
        while (entries.hasMoreElements())
        {
            java.util.zip.ZipEntry entry = (java.util.zip.ZipEntry)entries.nextElement();
            if (entry.getName().StartsWith("META-INF/", StringComparison.OrdinalIgnoreCase))
            {
                if (list == null)
                {
                    list = new List<string>();
                }
                list.Add(entry.getName());
            }
        }
        return list == null ? null : list.ToArray();
#endif
    }
}
