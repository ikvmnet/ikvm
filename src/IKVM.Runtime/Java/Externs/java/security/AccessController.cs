/*
  Copyright (C) 2007-2013 Jeroen Frijters

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

namespace IKVM.Java.Externs.java.security
{

    static class AccessController
    {

        /// <summary>
        /// Implements the native method 'getStackAccessControlContext'.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="callerID"></param>
        /// <returns></returns>
        public static object getStackAccessControlContext(global::java.security.AccessControlContext context, global::ikvm.@internal.CallerID callerID)
        {
#if FIRST_PASS
		    throw new NotImplementedException();
#else
            var array = new List<global::java.security.ProtectionDomain>();
            var is_privileged = GetProtectionDomains(array, callerID, new StackTrace(1));
            if (array.Count == 0)
                if (is_privileged && context == null)
                    return null;

            return CreateAccessControlContext(array, is_privileged, context);
#endif
        }

#if !FIRST_PASS

        static bool GetProtectionDomains(List<global::java.security.ProtectionDomain> array, global::ikvm.@internal.CallerID callerID, StackTrace stack)
        {
            // first we have to skip all AccessController related frames, because we can be called from a doPrivileged implementation (not the privileged action)
            // in which case we should ignore the doPrivileged frame
            int skip = 0;
            for (; skip < stack.FrameCount; skip++)
            {
                Type type = stack.GetFrame(skip).GetMethod().DeclaringType;
                if (type != typeof(AccessController) && type != typeof(global::java.security.AccessController))
                {
                    break;
                }
            }
            global::java.security.ProtectionDomain previous_protection_domain = null;
            for (int i = skip; i < stack.FrameCount; i++)
            {
                bool is_privileged = false;
                global::java.security.ProtectionDomain protection_domain;
                var method = stack.GetFrame(i).GetMethod();
                if (method.DeclaringType == typeof(global::java.security.AccessController) && method.Name == "doPrivileged")
                {
                    is_privileged = true;
                    global::java.lang.Class caller = callerID.getCallerClass();
                    protection_domain = caller == null ? null : IKVM.Java.Externs.java.lang.Class.getProtectionDomain0(caller);
                }
                else if (IKVM.Java.Externs.sun.reflect.Reflection.IsHideFromStackWalk(method))
                {
                    continue;
                }
                else
                {
                    protection_domain = GetProtectionDomainFromType(method.DeclaringType);
                }

                if (previous_protection_domain != protection_domain && protection_domain != null)
                {
                    previous_protection_domain = protection_domain;
                    array.Add(protection_domain);
                }

                if (is_privileged)
                {
                    return true;
                }
            }

            return false;
        }

        static object CreateAccessControlContext(List<global::java.security.ProtectionDomain> context, bool is_privileged, global::java.security.AccessControlContext privileged_context)
        {
            var acc = new global::java.security.AccessControlContext(context == null || context.Count == 0 ? null : context.ToArray(), is_privileged);
            acc._privilegedContext(privileged_context);
            return acc;
        }

        static global::java.security.ProtectionDomain GetProtectionDomainFromType(Type type)
        {
            if (type == null || type.Assembly == typeof(object).Assembly || type.Assembly == typeof(AccessController).Assembly || type.Assembly == typeof(global::java.lang.Thread).Assembly)
                return null;

            var tw = RuntimeClassLoaderFactory.GetJavaTypeFromType(type);
            if (tw != null)
                return IKVM.Java.Externs.java.lang.Class.getProtectionDomain0(tw.ClassObject);

            return null;
        }

#endif

        /// <summary>
        /// Implements the native method 'getInheritedAccessControlContext'.
        /// </summary>
        /// <returns></returns>
        public static object getInheritedAccessControlContext()
        {
#if FIRST_PASS
		    throw new NotImplementedException();
#else
            var inheritedAccessControlContext = global::java.lang.Thread.currentThread().inheritedAccessControlContext;
            var acc = inheritedAccessControlContext as global::java.security.AccessControlContext;
            if (acc != null)
                return acc;

            var lc = inheritedAccessControlContext as global::java.security.AccessController.LazyContext;
            if (lc == null)
                return null;

            var list = new List<global::java.security.ProtectionDomain>();
            while (lc != null)
            {
                if (GetProtectionDomains(list, lc.callerID, lc.stackTrace))
                    return CreateAccessControlContext(list, true, lc.context);

                lc = lc.parent;
            }

            return CreateAccessControlContext(list, false, null);
#endif
        }

    }

}
