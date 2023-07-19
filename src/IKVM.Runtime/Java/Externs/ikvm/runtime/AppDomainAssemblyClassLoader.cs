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

namespace IKVM.Java.Externs.ikvm.runtime
{

    static class AppDomainAssemblyClassLoader
    {

        /// <summary>
        /// Attempts to load the given class from the specified <see cref="Assembly"/>.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        static object LoadClassFromAssembly(Assembly assembly, string className)
        {
            return assembly.IsDynamic == false ? (IKVM.Runtime.RuntimeAssemblyClassLoaderFactory.FromAssembly(assembly).DoLoad(className)?.ClassObject) : null;
        }

        /// <summary>
        /// Finds the class with the given name within the AppDomain.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ClassNotFoundException"></exception>
        public static global::java.lang.Class findClass(object thisObj, string name)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                if (LoadClassFromAssembly(assembly, name) is global::java.lang.Class c)
                    return c;

            throw new global::java.lang.ClassNotFoundException(name);
#endif
        }

        /// <summary>
        /// Attempts to locate a resource in the loaded assemblies.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static IEnumerable<global::java.net.URL> FindResources(string name)
        {
            var done = new HashSet<IKVM.Runtime.RuntimeAssemblyClassLoader>();

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (asm.IsDynamic)
                    continue;

                var acl = IKVM.Runtime.RuntimeAssemblyClassLoaderFactory.FromAssembly(asm);
                if (done.Add(acl))
                    foreach (var url in acl.FindResources(name))
                        yield return url;
            }
        }

        /// <summary>
        /// Finds the first matching resource.
        /// </summary>
        /// <param name="thisObj"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static global::java.net.URL findResource(object thisObj, string name)
        {
            foreach (global::java.net.URL url in FindResources(name))
                return url;

            return null;
        }

        /// <summary>
        /// Finds the resource(s) with the given name and adds them to the vector.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="name"></param>
        public static void getResources(global::java.util.Vector v, string name)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            foreach (var url in FindResources(name))
                if (url != null && !v.contains(url))
                    v.add(url);
#endif
        }

    }

}
