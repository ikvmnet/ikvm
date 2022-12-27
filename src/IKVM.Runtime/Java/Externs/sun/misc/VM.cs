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
using System.Diagnostics;

using IKVM.Internal;

namespace IKVM.Java.Externs.sun.misc
{

    static class VM
    {

        public static void initialize()
        {

        }

        public static global::java.lang.ClassLoader latestUserDefinedLoader()
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var trace = new StackTrace(2, false);
            for (var i = 0; i < trace.FrameCount; i++)
            {
                var f = trace.GetFrame(i);
                var m = f.GetMethod();
                if (m == null)
                    continue;

                // not to be considered from Java
                if (global::IKVM.Java.Externs.sun.reflect.Reflection.IsHideFromStackWalk(m))
                    continue;

                if (m.DeclaringType != null && ClassLoaderWrapper.GetWrapperFromType(m.DeclaringType) is TypeWrapper tw and not null)
                {
                    // check that the assembly isn't java.base or the IKVM runtime
                    var clw = tw.GetClassLoader();
                    if (clw is AssemblyClassLoader acl)
                        if (acl.GetAssembly(tw) == typeof(object).Assembly || acl.GetAssembly(tw) == typeof(VM).Assembly)
                            continue;

                    // associated Java class loader is our nearest
                    var cl = clw.GetJavaClassLoader();
                    if (cl != null)
                        return cl;
                }
            }

            return null;
#endif
        }

    }

}