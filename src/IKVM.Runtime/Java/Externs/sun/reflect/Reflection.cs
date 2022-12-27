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
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

using IKVM.Attributes;
using IKVM.Internal;

namespace IKVM.Java.Externs.sun.reflect
{

    /// <summary>
    /// Provides the implementations of the native methods in <see cref="global::sun.reflect.Reflection"/>.
    /// </summary>
    static class Reflection
    {

        static readonly ConditionalWeakTable<MethodBase, Lazy<HideFromJavaFlags>> hideFromJavaFlagCache = new ConditionalWeakTable<MethodBase, Lazy<HideFromJavaFlags>>();

        /// <summary>
        /// Gets the <see cref="HideFromJavaFlags"/> that should be considered applied to the given method.
        /// </summary>
        /// <param name="mb"></param>
        /// <returns></returns>
        internal static HideFromJavaFlags GetHideFromJavaFlags(MethodBase mb)
        {
            return hideFromJavaFlagCache.GetValue(mb, _ => new Lazy<HideFromJavaFlags>(() => GetHideFromJavaFlagsImpl(_), true)).Value;
        }

        /// <summary>
        /// Gets the <see cref="HideFromJavaFlags"/> that should be considered applied to the given method.
        /// </summary>
        /// <param name="mb"></param>
        /// <returns></returns>
        static HideFromJavaFlags GetHideFromJavaFlagsImpl(MethodBase mb)
        {
            return mb.Name.StartsWith("__<", StringComparison.Ordinal) ? HideFromJavaFlags.All : AttributeHelper.GetHideFromJavaFlags(mb);
        }

        /// <summary>
        /// Returns <c>true</c> if the given method should not be considered in walks of the stack from the point of view of Java.
        /// </summary>
        /// <param name="mb"></param>
        /// <returns></returns>
        internal static bool IsHideFromStackWalk(MethodBase mb)
        {
            var type = mb.DeclaringType;
            if (type == null ||
                type.Assembly == typeof(object).Assembly ||
                type.Assembly == typeof(Reflection).Assembly ||
                type == typeof(global::java.lang.reflect.Method) ||
                type == typeof(global::java.lang.reflect.Constructor) ||
                (GetHideFromJavaFlags(mb) & HideFromJavaFlags.StackWalk) != 0)
                return true;

            return false;
        }

        public static global::java.lang.Class getCallerClass()
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            throw new global::java.lang.InternalError("CallerSensitive annotation expected at frame 1");
#endif
        }

        // NOTE this method is hooked up explicitly through map.xml to prevent inlining of the native stub
        // and tail-call optimization in the native stub.
        public static global::java.lang.Class getCallerClass(int realFramesToSkip)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (realFramesToSkip <= 0)
                return global::ikvm.@internal.ClassLiteral<global::sun.reflect.Reflection>.Value;

            for (int i = 2; ;)
            {
                var method = new StackFrame(i++, false).GetMethod();
                if (method == null)
                    return null;

                if (IsHideFromStackWalk(method))
                    continue;

                // HACK we skip HideFromJavaFlags.StackTrace too because we want to skip the LambdaForm methods
                // that are used by late binding
                if ((GetHideFromJavaFlags(method) & HideFromJavaFlags.StackTrace) != 0)
                    continue;

                if (--realFramesToSkip == 0)
                    return ClassLoaderWrapper.GetWrapperFromType(method.DeclaringType).ClassObject;
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
            var current = TypeWrapper.FromClass(currentClass);
            var member = TypeWrapper.FromClass(memberClass);
            return member.IsInternal && member.InternalsVisibleTo(current);
        }

    }

}