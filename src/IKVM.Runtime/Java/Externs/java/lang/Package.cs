/*
  Copyright (C) 2007-2014 Jeroen Frijters

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
using System.IO;
using System.Threading;

using IKVM.Runtime;
using IKVM.Runtime.Vfs;

namespace IKVM.Java.Externs.java.lang
{

	static class Package
    {

        static Dictionary<string, string> systemPackages;

        /// <summary>
        /// Initializes the system packages.
        /// </summary>
        static void LazyInitSystemPackages()
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (systemPackages == null)
            {
                var dict = new Dictionary<string, string>();
                var path = Path.Combine(VfsTable.GetAssemblyResourcesPath(JVM.Vfs.Context, JVM.Context.Resolver.ResolveBaseAssembly().AsReflection(), JVM.Properties.HomePath), "resources.jar");
                foreach (var pkgs in JVM.Context.ClassLoaderFactory.GetBootstrapClassLoader().GetPackageInfo())
                    foreach (var pkg in pkgs.Value)
                        dict[pkg.Replace('.', '/') + "/"] = path;

                Interlocked.CompareExchange(ref systemPackages, dict, null);
            }
#endif
        }

        public static string getSystemPackage0(string name)
        {
            LazyInitSystemPackages();
            systemPackages.TryGetValue(name, out string path);
            return path;
        }

        public static string[] getSystemPackages0()
        {
            LazyInitSystemPackages();
            var pkgs = new string[systemPackages.Count];
            systemPackages.Keys.CopyTo(pkgs, 0);
            return pkgs;
        }

    }

}
