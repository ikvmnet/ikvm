/*
  Copyright (C) 2002-2015 Jeroen Frijters

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
using System.Reflection;

using IKVM.Internal;

namespace IKVM.Java.Externs.ikvm.runtime
{
    static class AppDomainAssemblyClassLoader
    {
        public static object loadClassFromAssembly(Assembly asm, string className)
        {
            if (ReflectUtil.IsDynamicAssembly(asm))
            {
                return null;
            }
            TypeWrapper tw = Internal.AssemblyClassLoader.FromAssembly(asm).DoLoad(className);
            return tw != null ? tw.ClassObject : null;
        }

        private static IEnumerable<global::java.net.URL> FindResources(string name)
        {
            List<Internal.AssemblyClassLoader> done = new List<Internal.AssemblyClassLoader>();
            foreach (Assembly asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!ReflectUtil.IsDynamicAssembly(asm))
                {
                    Internal.AssemblyClassLoader acl = Internal.AssemblyClassLoader.FromAssembly(asm);
                    if (!done.Contains(acl))
                    {
                        done.Add(acl);
                        foreach (global::java.net.URL url in acl.FindResources(name))
                        {
                            yield return url;
                        }
                    }
                }
            }
        }

        public static global::java.net.URL findResource(object thisObj, string name)
        {
            foreach (global::java.net.URL url in FindResources(name))
            {
                return url;
            }
            return null;
        }

        public static void getResources(global::java.util.Vector v, string name)
        {
#if !FIRST_PASS
            foreach (global::java.net.URL url in FindResources(name))
            {
                if (url != null && !v.contains(url))
                {
                    v.add(url);
                }
            }
#endif
        }

    }

}
