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

using IKVM.Runtime;

namespace IKVM.Java.Externs.sun.invoke.util
{

    static class VerifyAccess
    {

        // called from map.xml as a replacement for Class.getClassLoader() in sun.invoke.util.VerifyAccess.isTypeVisible()
        public static global::java.lang.ClassLoader Class_getClassLoader(global::java.lang.Class clazz)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var tw = RuntimeJavaType.FromClass(clazz);
            if (JVM.Context.ClassLoaderFactory.GetBootstrapClassLoader().TryLoadClassByName(tw.Name) == tw)
            {
                // if a class is visible from the bootstrap class loader, we have to return null to allow the visibility check to succeed
                return null;
            }

            return tw.GetClassLoader().GetJavaClassLoader();
#endif
        }

    }

}