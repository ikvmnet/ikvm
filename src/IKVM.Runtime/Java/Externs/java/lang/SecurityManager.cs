﻿/*
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
using System.Diagnostics;

using IKVM.Runtime;

namespace IKVM.Java.Externs.java.lang
{

    static class SecurityManager
    {

        public static global::java.lang.Class[] getClassContext(object thisSecurityManager)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var stack = new List<global::java.lang.Class>();
            var trace = new StackTrace();
            for (int i = 0; i < trace.FrameCount; i++)
            {
                var frame = trace.GetFrame(i);
                var method = frame.GetMethod();

                if (IKVM.Java.Externs.sun.reflect.Reflection.IsHideFromStackWalk(method))
                    continue;

                var type = method.DeclaringType;
                if (type == typeof(global::java.lang.SecurityManager))
                    continue;

                stack.Add(JVM.Context.ClassLoaderFactory.GetJavaTypeFromType(JVM.Context.Resolver.GetSymbol(type)).ClassObject);
            }

            return stack.ToArray();
#endif
        }

        public static object currentClassLoader0(object thisSecurityManager)
        {
            var currentClass = currentLoadedClass0(thisSecurityManager);
            if (currentClass != null)
                return RuntimeJavaType.FromClass(currentClass).ClassLoader.GetJavaClassLoader();

            return null;
        }

        public static int classDepth(object thisSecurityManager, string name)
        {
            throw new NotImplementedException();
        }

        public static int classLoaderDepth0(object thisSecurityManager)
        {
            throw new NotImplementedException();
        }

        public static global::java.lang.Class currentLoadedClass0(object thisSecurityManager)
        {
            throw new NotImplementedException();
        }

    }

}
