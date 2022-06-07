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
using System.Reflection;

using IKVM.Internal;

static class Java_java_security_AccessController
{

    public static object getStackAccessControlContext(global::java.security.AccessControlContext context, global::ikvm.@internal.CallerID callerID)
    {
#if FIRST_PASS
		return null;
#else
        List<global::java.security.ProtectionDomain> array = new List<global::java.security.ProtectionDomain>();
        bool is_privileged = GetProtectionDomains(array, callerID, new StackTrace(1));
        if (array.Count == 0)
        {
            if (is_privileged && context == null)
            {
                return null;
            }
        }
        return CreateAccessControlContext(array, is_privileged, context);
#endif
    }

#if !FIRST_PASS
    private static bool GetProtectionDomains(List<global::java.security.ProtectionDomain> array, global::ikvm.@internal.CallerID callerID, StackTrace stack)
    {
        // first we have to skip all AccessController related frames, because we can be called from a doPrivileged implementation (not the privileged action)
        // in which case we should ignore the doPrivileged frame
        int skip = 0;
        for (; skip < stack.FrameCount; skip++)
        {
            Type type = stack.GetFrame(skip).GetMethod().DeclaringType;
            if (type != typeof(Java_java_security_AccessController) && type != typeof(global::java.security.AccessController))
            {
                break;
            }
        }
        global::java.security.ProtectionDomain previous_protection_domain = null;
        for (int i = skip; i < stack.FrameCount; i++)
        {
            bool is_privileged = false;
            global::java.security.ProtectionDomain protection_domain;
            MethodBase method = stack.GetFrame(i).GetMethod();
            if (method.DeclaringType == typeof(global::java.security.AccessController)
                && method.Name == "doPrivileged")
            {
                is_privileged = true;
                global::java.lang.Class caller = callerID.getCallerClass();
                protection_domain = caller == null ? null : IKVM.Runtime.JniExport.java.lang.Class.getProtectionDomain0(caller);
            }
            else if (IKVM.Runtime.JniExport.sun.reflect.Reflection.IsHideFromStackWalk(method))
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

    private static object CreateAccessControlContext(List<global::java.security.ProtectionDomain> context, bool is_privileged, global::java.security.AccessControlContext privileged_context)
    {
        global::java.security.AccessControlContext acc = new global::java.security.AccessControlContext(context == null || context.Count == 0 ? null : context.ToArray(), is_privileged);
        acc._privilegedContext(privileged_context);
        return acc;
    }

    private static global::java.security.ProtectionDomain GetProtectionDomainFromType(Type type)
    {
        if (type == null
            || type.Assembly == typeof(object).Assembly
            || type.Assembly == typeof(Java_java_security_AccessController).Assembly
            || type.Assembly == IKVM.Runtime.JniExport.java.lang.SecurityManager.jniAssembly
            || type.Assembly == typeof(global::java.lang.Thread).Assembly)
        {
            return null;
        }
        TypeWrapper tw = ClassLoaderWrapper.GetWrapperFromType(type);
        if (tw != null)
        {
            return IKVM.Runtime.JniExport.java.lang.Class.getProtectionDomain0(tw.ClassObject);
        }
        return null;
    }
#endif

    public static object getInheritedAccessControlContext()
    {
#if FIRST_PASS
		return null;
#else
        object inheritedAccessControlContext = global::java.lang.Thread.currentThread().inheritedAccessControlContext;
        global::java.security.AccessControlContext acc = inheritedAccessControlContext as global::java.security.AccessControlContext;
        if (acc != null)
        {
            return acc;
        }

        global::java.security.AccessController.LazyContext lc = inheritedAccessControlContext as global::java.security.AccessController.LazyContext;
        if (lc == null)
        {
            return null;
        }

        List<global::java.security.ProtectionDomain> list = new List<global::java.security.ProtectionDomain>();
        while (lc != null)
        {
            if (GetProtectionDomains(list, lc.callerID, lc.stackTrace))
            {
                return CreateAccessControlContext(list, true, lc.context);
            }
            lc = lc.parent;
        }

        return CreateAccessControlContext(list, false, null);
#endif
    }

}
