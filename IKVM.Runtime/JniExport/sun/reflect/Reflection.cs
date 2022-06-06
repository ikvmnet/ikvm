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
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

using IKVM.Attributes;
using IKVM.Internal;

namespace IKVM.Runtime.JniExport.sun.reflect
{

    static class Reflection
    {

#if CLASSGC

	    sealed class State
	    {
		    internal HideFromJavaFlags Value;
		    internal volatile bool HasValue;
	    }

	    private static readonly ConditionalWeakTable<MethodBase, State> isHideFromJavaCache = new ConditionalWeakTable<MethodBase, State>();

	    internal static HideFromJavaFlags GetHideFromJavaFlags(MethodBase mb)
	    {
		    if (mb.Name.StartsWith("__<", StringComparison.Ordinal))
		    {
			    return HideFromJavaFlags.All;
		    }
		    State state = isHideFromJavaCache.GetValue(mb, delegate { return new State(); });
		    if (!state.HasValue)
		    {
			    state.Value = AttributeHelper.GetHideFromJavaFlags(mb);
			    state.HasValue = true;
		    }
		    return state.Value;
	    }

#else

        private static readonly Dictionary<RuntimeMethodHandle, HideFromJavaFlags> isHideFromJavaCache = new Dictionary<RuntimeMethodHandle, HideFromJavaFlags>();

        internal static HideFromJavaFlags GetHideFromJavaFlags(MethodBase mb)
        {
            if (mb.Name.StartsWith("__<", StringComparison.Ordinal))
            {
                return HideFromJavaFlags.All;
            }
            RuntimeMethodHandle handle;
            try
            {
                handle = mb.MethodHandle;
            }
            catch (InvalidOperationException)
            {
                // DynamicMethods don't have a RuntimeMethodHandle and we always want to hide them anyway
                return HideFromJavaFlags.All;
            }
            catch (NotSupportedException)
            {
                // DynamicMethods don't have a RuntimeMethodHandle and we always want to hide them anyway
                return HideFromJavaFlags.All;
            }
            lock (isHideFromJavaCache)
            {
                HideFromJavaFlags cached;
                if (isHideFromJavaCache.TryGetValue(handle, out cached))
                {
                    return cached;
                }
            }
            HideFromJavaFlags flags = AttributeHelper.GetHideFromJavaFlags(mb);
            lock (isHideFromJavaCache)
            {
                isHideFromJavaCache[handle] = flags;
            }
            return flags;
        }
#endif

        internal static bool IsHideFromStackWalk(MethodBase mb)
        {
            Type type = mb.DeclaringType;
            return type == null
                || type.Assembly == typeof(object).Assembly
                || type.Assembly == typeof(Reflection).Assembly
                || type.Assembly == Java_java_lang_SecurityManager.jniAssembly
                || type == typeof(global::java.lang.reflect.Method)
                || type == typeof(global::java.lang.reflect.Constructor)
                || (GetHideFromJavaFlags(mb) & HideFromJavaFlags.StackWalk) != 0
                ;
        }

        public static global::java.lang.Class getCallerClass()
        {
#if FIRST_PASS
		return null;
#else
            throw new global::java.lang.InternalError("CallerSensitive annotation expected at frame 1");
#endif
        }

        // NOTE this method is hooked up explicitly through map.xml to prevent inlining of the native stub
        // and tail-call optimization in the native stub.
        public static global::java.lang.Class getCallerClass(int realFramesToSkip)
        {
#if FIRST_PASS
		return null;
#else
            if (realFramesToSkip <= 0)
            {
                return global::ikvm.@internal.ClassLiteral<global::sun.reflect.Reflection>.Value;
            }
            for (int i = 2; ;)
            {
                MethodBase method = new StackFrame(i++, false).GetMethod();
                if (method == null)
                {
                    return null;
                }
                if (IsHideFromStackWalk(method))
                {
                    continue;
                }
                // HACK we skip HideFromJavaFlags.StackTrace too because we want to skip the LambdaForm methods
                // that are used by late binding
                if ((GetHideFromJavaFlags(method) & HideFromJavaFlags.StackTrace) != 0)
                {
                    continue;
                }
                if (--realFramesToSkip == 0)
                {
                    return ClassLoaderWrapper.GetWrapperFromType(method.DeclaringType).ClassObject;
                }
            }
#endif
        }

        public static int getClassAccessFlags(global::java.lang.Class clazz)
        {
            // the mask comes from JVM_RECOGNIZED_CLASS_MODIFIERS in src/hotspot/share/vm/prims/jvm.h
            int mods = (int)TypeWrapper.FromClass(clazz).Modifiers & 0x7631;
            // interface implies abstract
            mods |= (mods & 0x0200) << 1;
            return mods;
        }

        public static bool checkInternalAccess(global::java.lang.Class currentClass, global::java.lang.Class memberClass)
        {
            TypeWrapper current = TypeWrapper.FromClass(currentClass);
            TypeWrapper member = TypeWrapper.FromClass(memberClass);
            return member.IsInternal && member.InternalsVisibleTo(current);
        }

    }

}