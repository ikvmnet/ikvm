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

using IKVM.Attributes;
using IKVM.Runtime;

namespace IKVM.Java.Externs.ikvm.runtime
{

    static class Util
    {

        public static global::java.lang.Class getClassFromObject(object o)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return GetTypeWrapperFromObject(JVM.Context, o).ClassObject;
#endif
        }

        internal static RuntimeJavaType GetTypeWrapperFromObject(RuntimeContext context, object o)
        {
            var ghostType = GhostTag.GetTag(o);
            if (ghostType != null)
                return ghostType;

            var t = o.GetType();
            if (t.IsPrimitive || context.ClassLoaderFactory.IsRemappedType(t) && !t.IsSealed)
                return context.ManagedJavaTypeFactory.GetJavaTypeFromManagedType(t);

            for (; ; )
            {
                // if GetWrapperFromType returns null (or if tw.IsAbstract), that
                // must mean that the Type of the object is an implementation helper class
                // (e.g. an AtomicReferenceFieldUpdater or ThreadLocal instrinsic subclass)
                var tw = context.ClassLoaderFactory.GetJavaTypeFromType(t);
                if (tw != null && (!tw.IsAbstract || tw.IsArray))
                    return tw;

                t = t.BaseType;
            }
        }

        public static global::java.lang.Class getClassFromTypeHandle(RuntimeTypeHandle handle)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var t = Type.GetTypeFromHandle(handle);
            if (t.IsPrimitive || JVM.Context.ClassLoaderFactory.IsRemappedType(t) || t == typeof(void))
                return JVM.Context.ManagedJavaTypeFactory.GetJavaTypeFromManagedType(t).ClassObject;

            if (!IsVisibleAsClass(t))
                return null;

            var tw = JVM.Context.ClassLoaderFactory.GetJavaTypeFromType(t);
            if (tw != null)
                return tw.ClassObject;

            return null;
#endif
        }

        public static global::java.lang.Class getClassFromTypeHandle(RuntimeTypeHandle handle, int rank)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var t = Type.GetTypeFromHandle(handle);
            if (t.IsPrimitive || JVM.Context.ClassLoaderFactory.IsRemappedType(t) || t == typeof(void))
                return JVM.Context.ManagedJavaTypeFactory.GetJavaTypeFromManagedType(t).MakeArrayType(rank).ClassObject;

            if (!IsVisibleAsClass(t))
                return null;

            var tw = JVM.Context.ClassLoaderFactory.GetJavaTypeFromType(t);
            if (tw != null)
                return tw.MakeArrayType(rank).ClassObject;

            return null;
#endif
        }

        public static global::java.lang.Class getFriendlyClassFromType(Type type)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            int rank = 0;
            while (ReflectUtil.IsVector(type))
            {
                type = type.GetElementType();
                rank++;
            }

            if (type.DeclaringType != null && JVM.Context.AttributeHelper.IsGhostInterface(type.DeclaringType))
                type = type.DeclaringType;

            if (!IsVisibleAsClass(type))
                return null;

            var wrapper = JVM.Context.ClassLoaderFactory.GetJavaTypeFromType(type);
            if (wrapper == null)
                return null;

            if (rank > 0)
                wrapper = wrapper.MakeArrayType(rank);

            return wrapper.ClassObject;
#endif
        }

        private static bool IsVisibleAsClass(Type type)
        {
            while (type.HasElementType)
            {
                if (type.IsPointer || type.IsByRef)
                {
                    return false;
                }
                type = type.GetElementType();
            }
            if (type.ContainsGenericParameters && !type.IsGenericTypeDefinition)
            {
                return false;
            }
            System.Reflection.Emit.TypeBuilder tb = type as System.Reflection.Emit.TypeBuilder;
            if (tb != null && !tb.IsCreated())
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets the underlying CLR <see cref="Type"/> instance.
        /// </summary>
        /// <param name="classObject"></param>
        /// <returns></returns>
        public static Type getInstanceTypeFromClass(global::java.lang.Class classObject)
        {
            var wrapper = RuntimeJavaType.FromClass(classObject);
            if (wrapper.IsRemapped && wrapper.IsFinal)
                return wrapper.TypeAsTBD;
            else
                return wrapper.TypeAsBaseType;
        }

        /// <summary>
        /// Gets the underlying CLR <see cref="Type"/> instance resovled fully at runtime.
        /// </summary>
        /// <param name="classObject"></param>
        /// <returns></returns>
        public static Type getRuntimeTypeFromClass(global::java.lang.Class classObject)
        {
            var wrapper = RuntimeJavaType.FromClass(classObject);
            wrapper.Finish();

            if (wrapper.IsRemapped && wrapper.IsFinal)
                return wrapper.TypeAsTBD;
            else
                return wrapper.TypeAsBaseType;
        }

        [HideFromJava]
        public static Exception mapException(Exception e)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return JVM.Context.ExceptionHelper.MapException<Exception>(e, true, false);
#endif
        }

        public static Exception unmapException(Exception e)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return ExceptionHelper.UnmapException(e);
#endif
        }

    }

}
