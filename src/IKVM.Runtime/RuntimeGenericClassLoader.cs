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
using System.Text;

using IKVM.CoreLib.Runtime;

namespace IKVM.Runtime
{

    sealed class RuntimeGenericClassLoader : RuntimeClassLoader
    {

        readonly RuntimeClassLoader[] delegates;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="delegates"></param>
        /// <param name="javaClassLoader"></param>
        internal RuntimeGenericClassLoader(RuntimeContext context, RuntimeClassLoader[] delegates, object javaClassLoader) :
            base(context, CodeGenOptions.None, javaClassLoader)
        {
            this.delegates = delegates;
        }

        internal bool Matches(RuntimeClassLoader[] key)
        {
            if (key.Length == delegates.Length)
            {
                for (int i = 0; i < key.Length; i++)
                    if (key[i] != delegates[i])
                        return false;

                return true;
            }

            return false;
        }

        protected override RuntimeJavaType FindLoadedClassLazy(string name)
        {
            var tw1 = FindOrLoadGenericClass(name, LoadMode.Find);
            if (tw1 != null)
                return tw1;

            foreach (var loader in delegates)
            {
                var tw = loader.FindLoadedClass(name);
                if (tw != null && tw.ClassLoader == loader)
                    return tw;
            }

            return null;
        }

        internal string GetName()
        {
            var sb = new ValueStringBuilder();
            sb.Append('[');

            foreach (var loader in delegates)
            {
                sb.Append('[');

                if (loader is RuntimeGenericClassLoader gcl)
                    sb.Append(gcl.GetName());
                else
                    sb.Append(((RuntimeAssemblyClassLoader)loader).MainAssembly.FullName);

                sb.Append(']');
            }

            sb.Append(']');

            return sb.ToString();
        }

#if IMPORTER == false && EXPORTER == false

        internal java.util.Enumeration GetResources(string name)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var v = new java.util.Vector();
            foreach (var url in Context.ClassLoaderFactory.GetBootstrapClassLoader().GetResources(name))
                v.add(url);

            if (name.EndsWith(".class", StringComparison.Ordinal) && name.IndexOf('.') == name.Length - 6)
            {
                var tw = FindLoadedClass(name.Substring(0, name.Length - 6).Replace('/', '.'));
                if (tw != null && !tw.IsArray && !tw.IsDynamic)
                {
                    var loader = tw.ClassLoader;
                    if (loader is RuntimeGenericClassLoader)
                        v.add(new java.net.URL("ikvmres", "gen", Context.ClassLoaderFactory.GetGenericClassLoaderId(loader), "/" + name));
                    else if (loader is RuntimeAssemblyClassLoader acl)
                        foreach (java.net.URL url in acl.FindResources(name))
                            v.add(url);
                }
            }

            return v.elements();
#endif
        }

        internal java.net.URL FindResource(string name)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (name.EndsWith(".class", StringComparison.Ordinal) && name.IndexOf('.') == name.Length - 6)
            {
                var tw = FindLoadedClass(name.Substring(0, name.Length - 6).Replace('/', '.'));
                if (tw != null && tw.ClassLoader == this && !tw.IsArray && !tw.IsDynamic)
                    return new java.net.URL("ikvmres", "gen", Context.ClassLoaderFactory.GetGenericClassLoaderId(this), "/" + name);
            }

            return null;
#endif
        }

#endif

    }

}
