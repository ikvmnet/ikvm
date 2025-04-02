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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using IKVM.Runtime;

namespace IKVM.Java.Externs.ikvm.runtime
{

    static class AppDomainAssemblyClassLoader
    {

#if FIRST_PASS == false

        /// <summary>
        /// Returns the appropriate wrapper type for the given <see cref="global::java.util.Enumeration"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumeration"></param>
        /// <returns></returns>
        static IEnumerable<T> AsEnumerable<T>(this global::java.util.Enumeration enumeration)
        {
            while (enumeration.hasMoreElements())
                yield return (T)enumeration.nextElement();
        }

        /// <summary>
        /// Attempts to load the given class from the specified <see cref="Assembly"/>.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        static global::java.lang.Class LoadClassFromAssembly(Assembly assembly, string className)
        {
            return assembly.IsDynamic == false ? (JVM.Context.AssemblyClassLoaderFactory.FromAssembly(assembly).DoLoad(className)?.ClassObject) : null;
        }

        /// <summary>
        /// Attempts to locate a resource in the loaded assemblies.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static IEnumerable<global::java.net.URL> FindResources(string name)
        {
            var done = new HashSet<RuntimeAssemblyClassLoader>();

            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (asm.IsDynamic)
                    continue;

                var acl = JVM.Context.AssemblyClassLoaderFactory.FromAssembly(asm);
                if (done.Add(acl))
                    foreach (var url in acl.FindResources(name))
                        yield return url;
            }
        }

        /// <summary>
        /// Finds the resource(s) with the given name and adds them to the vector.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        static IEnumerable<global::java.net.URL> ConcatResources(IEnumerable<global::java.net.URL> parent, string name)
        {
            var hs = new HashSet<global::java.net.URL>();

            foreach (var url in parent)
                if (url != null && hs.Add(url))
                    yield return url;

            foreach (var url in FindResources(name))
                if (url != null && hs.Add(url))
                    yield return url;
        }

#endif

        /// <summary>
        /// Finds the class with the given name within the AppDomain.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ClassNotFoundException"></exception>
        public static object findClass(object self, string name)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                if (LoadClassFromAssembly(assembly, name) is { } c)
                    return c;

            throw new global::java.lang.ClassNotFoundException(name);
#endif
        }

        /// <summary>
        /// Finds the first matching resource.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object findResource(object self, string name)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            foreach (var url in FindResources(name))
                return url;

            return null;
#endif
        }

        /// <summary>
        /// Concats resources from this loader with those given by <paramref name="existing"/>.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="existing"></param>
        /// <param name="name"></param>
        public static object concatResources(object self, object existing, string name)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return new global::ikvm.runtime.EnumerationWrapper(ConcatResources(AsEnumerable<global::java.net.URL>((global::java.util.Enumeration)existing), name));
#endif
        }

    }

}
