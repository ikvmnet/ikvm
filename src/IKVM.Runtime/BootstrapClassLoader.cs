/*
  Copyright (C) 2002-2013 Jeroen Frijters

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

using IKVM.CoreLib.Symbols;

namespace IKVM.Runtime
{

    /// <summary>
    /// Represents the bootstrap class loader of the system, containing only built-in .NET assemblies.
    /// </summary>
    sealed class BootstrapClassLoader : RuntimeAssemblyClassLoader
    {

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        internal BootstrapClassLoader(RuntimeContext context) :
            base(context, context.Resolver.ResolveBaseAssembly(), [typeof(object).Assembly.FullName, typeof(Uri).Assembly.FullName])
        {
#if FIRST_PASS == false && IMPORTER == false && EXPORTER == false
            RegisterNativeLibrary(LibJava.Instance.Handle);
#endif
        }

        internal override RuntimeJavaType GetJavaTypeFromAssemblyType(ITypeSymbol type)
        {
            // we have to special case the fake types here
            if (type.IsGenericType && !type.IsGenericTypeDefinition)
            {
                var outer = Context.ClassLoaderFactory.GetJavaTypeFromType(type.GetGenericArguments()[0]);

                foreach (var inner in outer.InnerClasses)
                {
                    if (inner.TypeAsTBD == type)
                        return inner;

                    foreach (var inner2 in inner.InnerClasses)
                        if (inner2.TypeAsTBD == type)
                            return inner2;
                }

                return null;
            }

            return base.GetJavaTypeFromAssemblyType(type);
        }

        protected override void CheckProhibitedPackage(string className)
        {

        }

#if !FIRST_PASS && !IMPORTER && !EXPORTER

        internal override java.lang.ClassLoader GetJavaClassLoader()
        {
            return null;
        }

        internal override java.security.ProtectionDomain GetProtectionDomain()
        {
            return null;
        }

        internal override IEnumerable<java.net.URL> GetResources(string name)
        {
            foreach (java.net.URL url in FindResources(name))
                yield return url;

            foreach (var res in FindDelegateResources(name))
                yield return res.URL;
        }

#endif

    }

}
