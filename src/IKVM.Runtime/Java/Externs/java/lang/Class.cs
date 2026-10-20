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

using IKVM.Attributes;
using IKVM.CoreLib.Exceptions;
using IKVM.Runtime;
using IKVM.Runtime.Accessors.Java.Lang;

namespace IKVM.Java.Externs.java.lang
{

    /// <summary>
    /// Implements the native methods for 'java.lang.Class'.
    /// </summary>
    static class Class
    {

#if FIRST_PASS == false

        static ClassLoaderAccessor classLoaderAccessor;

        static ClassLoaderAccessor ClassLoaderAccessor => JVM.Internal.BaseAccessors.Get(ref classLoaderAccessor);

#endif

        /// <summary>
        /// Implements the native method 'forName0'.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="initialize"></param>
        /// <param name="loader"></param>
        /// <param name="caller"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        /// <exception cref="global::java.lang.ClassNotFoundException"></exception>
        public static global::java.lang.Class forName0(string name, bool initialize, global::java.lang.ClassLoader loader, global::java.lang.Class caller)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (name == null)
                throw new global::java.lang.NullPointerException();

            RuntimeJavaType javaType = null;
            if (name.IndexOf(',') > 0)
            {
                // we essentially require full trust before allowing arbitrary types to be loaded,
                // hence we do the "createClassLoader" permission check
                var sm = global::java.lang.System.getSecurityManager();
                sm?.checkPermission(new global::java.lang.RuntimePermission("createClassLoader"));

                var type = Type.GetType(name);
                if (type != null)
                    javaType = JVM.Context.ClassLoaderFactory.GetJavaTypeFromType(type);

                if (javaType == null)
                {
                    global::java.lang.Throwable.suppressFillInStackTrace = true;
                    throw new global::java.lang.ClassNotFoundException(name);
                }
            }
            else
            {
                try
                {
                    javaType = JVM.Context.ClassLoaderFactory.GetClassLoaderWrapper(loader).LoadClassByName(name);
                }
                catch (ClassNotFoundException e)
                {
                    global::java.lang.Throwable.suppressFillInStackTrace = true;
                    throw new global::java.lang.ClassNotFoundException(e.Message);
                }
                catch (ClassLoadingException e)
                {
                    throw e.InnerException;
                }
                catch (TranslatableJavaException e)
                {
                    throw JVM.Context.ExceptionHelper.MapException<Exception>(e, true, false);
                }
            }

            if (loader != null && caller != null && getProtectionDomain0(caller) is global::java.security.ProtectionDomain pd)
                ClassLoaderAccessor.InvokeCheckPackageAccess(loader, javaType.ClassObject, pd);

            if (initialize && javaType.IsArray == false)
            {
                try
                {
                    javaType.Finish();
                }
                catch (TranslatableJavaException e)
                {
                    throw JVM.Context.ExceptionHelper.MapException<Exception>(e, true, false);
                }

                javaType.RunClassInit();
            }

            return javaType.ClassObject;
#endif
        }

        /// <summary>
        /// Implements the native method 'getRawTypeAnnotations'.
        /// </summary>
        /// <param name="thisClass"></param>
        /// <returns></returns>
        public static byte[] getRawTypeAnnotations(global::java.lang.Class thisClass)
        {
            return RuntimeJavaType.FromClass(thisClass).GetRawTypeAnnotations();
        }

#if !FIRST_PASS

        sealed class ConstantPoolImpl : global::sun.reflect.ConstantPool
        {

            readonly object[] constantPool;

            /// <summary>
            /// Initialzes a new instance.
            /// </summary>
            /// <param name="constantPool"></param>
            internal ConstantPoolImpl(object[] constantPool)
            {
                this.constantPool = constantPool;
            }

            public override string getUTF8At(int index)
            {
                return (string)constantPool[index];
            }

            public override int getIntAt(int index)
            {
                return (int)constantPool[index];
            }

            public override long getLongAt(int index)
            {
                return (long)constantPool[index];
            }

            public override float getFloatAt(int index)
            {
                return (float)constantPool[index];
            }

            public override double getDoubleAt(int index)
            {
                return (double)constantPool[index];
            }
        }

#endif

        /// <summary>
        /// Implements the native method 'getConstantPool'.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static object getConstantPool(global::java.lang.Class self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return new ConstantPoolImpl(RuntimeJavaType.FromClass(self).GetConstantPool());
#endif
        }

        /// <summary>
        /// Implements the native method 'isInstance'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool isInstance(global::java.lang.Class self, object obj)
        {
            return RuntimeJavaType.FromClass(self).IsInstance(obj);
        }

        /// <summary>
        /// Implements the native method 'isAssignableFrom'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="otherClass"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.NullPointerException"></exception>
        public static bool isAssignableFrom(global::java.lang.Class self, global::java.lang.Class otherClass)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (otherClass == null)
                throw new global::java.lang.NullPointerException();

            return RuntimeJavaType.FromClass(otherClass).IsAssignableTo(RuntimeJavaType.FromClass(self));
#endif
        }

        /// <summary>
        /// Implements the native method 'isInterface'.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool isInterface(global::java.lang.Class self)
        {
            return RuntimeJavaType.FromClass(self).IsInterface;
        }

        public static bool isArray(global::java.lang.Class thisClass)
        {
            return RuntimeJavaType.FromClass(thisClass).IsArray;
        }

        public static bool isPrimitive(global::java.lang.Class thisClass)
        {
            return RuntimeJavaType.FromClass(thisClass).IsPrimitive;
        }

        public static string getName0(global::java.lang.Class thisClass)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var type = RuntimeJavaType.FromClass(thisClass);
            if (type.IsPrimitive)
            {
                if (type == JVM.Context.PrimitiveJavaTypeFactory.BYTE)
                    return "byte";
                else if (type == JVM.Context.PrimitiveJavaTypeFactory.CHAR)
                    return "char";
                else if (type == JVM.Context.PrimitiveJavaTypeFactory.DOUBLE)
                    return "double";
                else if (type == JVM.Context.PrimitiveJavaTypeFactory.FLOAT)
                    return "float";
                else if (type == JVM.Context.PrimitiveJavaTypeFactory.INT)
                    return "int";
                else if (type == JVM.Context.PrimitiveJavaTypeFactory.LONG)
                    return "long";
                else if (type == JVM.Context.PrimitiveJavaTypeFactory.SHORT)
                    return "short";
                else if (type == JVM.Context.PrimitiveJavaTypeFactory.BOOLEAN)
                    return "boolean";
                else if (type == JVM.Context.PrimitiveJavaTypeFactory.VOID)
                    return "void";
            }

            if (type.IsUnsafeAnonymous)
            {
                // for OpenJDK compatibility and debugging convenience we modify the class name to
                // include the identity hashcode of the class object
                return type.Name + "/" + global::java.lang.System.identityHashCode(thisClass);
            }

            return type.Name;
#endif
        }

        public static string getSigName(global::java.lang.Class thisClass)
        {
            return RuntimeJavaType.FromClass(thisClass).SignatureName;
        }

        public static global::java.lang.ClassLoader getClassLoader0(global::java.lang.Class thisClass)
        {
            return RuntimeJavaType.FromClass(thisClass).ClassLoader.GetJavaClassLoader();
        }

        public static global::java.lang.Class getSuperclass(global::java.lang.Class thisClass)
        {
            return RuntimeJavaType.FromClass(thisClass).BaseTypeWrapper?.ClassObject;
        }

        public static global::java.lang.Class[] getInterfaces0(global::java.lang.Class thisClass)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var ifaces = RuntimeJavaType.FromClass(thisClass).Interfaces;
            var interfaces = new global::java.lang.Class[ifaces.Length];
            for (int i = 0; i < ifaces.Length; i++)
                interfaces[i] = ifaces[i].ClassObject;

            return interfaces;
#endif
        }

        public static global::java.lang.Class getComponentType(global::java.lang.Class thisClass)
        {
            var type = RuntimeJavaType.FromClass(thisClass);
            return type.IsArray ? type.ElementTypeWrapper.ClassObject : null;
        }

        public static int getModifiers(global::java.lang.Class thisClass)
        {
            // the 0x7FFF mask comes from JVM_ACC_WRITTEN_FLAGS in hotspot\src\share\vm\utilities\accessFlags.hpp
            // masking out ACC_SUPER comes from instanceKlass::compute_modifier_flags() in hotspot\src\share\vm\oops\instanceKlass.cpp
            const int mask = 0x7FFF & (int)~Modifiers.Super;
            return (int)RuntimeJavaType.FromClass(thisClass).ReflectiveModifiers & mask;
        }

        public static object[] getSigners(global::java.lang.Class thisClass)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return thisClass.signers;
#endif
        }

        public static void setSigners(global::java.lang.Class thisClass, object[] signers)
        {
#if !FIRST_PASS
            thisClass.signers = signers;
#endif
        }

        public static object[] getEnclosingMethod0(global::java.lang.Class thisClass)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                var type = RuntimeJavaType.FromClass(thisClass);
                type.Finish();

                var enc = type.GetEnclosingMethod();
                if (enc == null)
                    return null;

                return [type.ClassLoader.LoadClassByName(enc[0]).ClassObject, enc[1], enc[2]?.Replace('.', '/')];
            }
            catch (TranslatableJavaException e)
            {
                throw JVM.Context.ExceptionHelper.MapException<Exception>(e, true, false);
            }
#endif
        }

        public static global::java.lang.Class getDeclaringClass0(global::java.lang.Class thisClass)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                var type = RuntimeJavaType.FromClass(thisClass);
                type.Finish();

                var declaringType = type.DeclaringTypeWrapper;
                if (declaringType == null)
                    return null;

                declaringType = declaringType.EnsureLoadable(type.ClassLoader);
                if (declaringType.IsAccessibleFrom(type) == false)
                    throw new IllegalAccessError(string.Format("tried to access class {0} from class {1}", declaringType.Name, type.Name));

                declaringType.Finish();

                foreach (var declaringTypeInnerType in declaringType.InnerClasses)
                    if (declaringTypeInnerType.Name == type.Name && declaringTypeInnerType.EnsureLoadable(declaringType.ClassLoader) == type)
                        return declaringType.ClassObject;

                throw new IncompatibleClassChangeError(string.Format("{0} and {1} disagree on InnerClasses attribute", declaringType.Name, type.Name));
            }
            catch (TranslatableJavaException e)
            {
                throw JVM.Context.ExceptionHelper.MapException<Exception>(e, true, false);
            }
#endif
        }

        public static global::java.security.ProtectionDomain getProtectionDomain0(global::java.lang.Class thisClass)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var type = RuntimeJavaType.FromClass(thisClass);
            if (type.IsArray)
                return null;

            var pd = type.ClassObject.pd;
            if (pd == null)
            {
                // The protection domain for statically compiled code is created lazily (not at global::java.lang.Class creation time),
                // to work around boot strap issues.
                if (type.ClassLoader is RuntimeAssemblyClassLoader acl)
                {
                    pd = acl.GetProtectionDomain();
                }
                else if (type is RuntimeAnonymousJavaType)
                {
                    // dynamically compiled intrinsified lamdba anonymous types end up here and should get their
                    // protection domain from the host class
                    pd = JVM.Context.ClassLoaderFactory.GetJavaTypeFromType(type.TypeAsTBD.DeclaringType).ClassObject.pd;
                }
            }

            return pd;
#endif
        }

        /// <summary>
        /// Implements the native method for 'getPrimitiveClass'.
        /// </summary>
        /// <remarks>
        /// This method isn't used anymore (because it is an intrinsic (during core class library compilation))
        /// it still remains for compat because it might be invoked through reflection by evil code
        /// </remarks>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static global::java.lang.Class getPrimitiveClass(string name)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return name switch
            {
                "byte" => JVM.Context.PrimitiveJavaTypeFactory.BYTE.ClassObject,
                "char" => JVM.Context.PrimitiveJavaTypeFactory.CHAR.ClassObject,
                "double" => JVM.Context.PrimitiveJavaTypeFactory.DOUBLE.ClassObject,
                "float" => JVM.Context.PrimitiveJavaTypeFactory.FLOAT.ClassObject,
                "int" => JVM.Context.PrimitiveJavaTypeFactory.INT.ClassObject,
                "long" => JVM.Context.PrimitiveJavaTypeFactory.LONG.ClassObject,
                "short" => JVM.Context.PrimitiveJavaTypeFactory.SHORT.ClassObject,
                "boolean" => JVM.Context.PrimitiveJavaTypeFactory.BOOLEAN.ClassObject,
                "void" => JVM.Context.PrimitiveJavaTypeFactory.VOID.ClassObject,
                _ => throw new ArgumentException(name),
            };
#endif
        }

        public static string getGenericSignature0(global::java.lang.Class thisClass)
        {
            var type = RuntimeJavaType.FromClass(thisClass);
            type.Finish();
            return type.GetGenericSignature();
        }

        internal static object AnnotationsToMap(RuntimeClassLoader loader, object[] objAnn)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var map = new global::java.util.LinkedHashMap();
            if (objAnn != null)
            {
                foreach (object obj in objAnn)
                {
                    var a = obj as global::java.lang.annotation.Annotation;
                    if (a != null)
                    {
                        map.put(a.annotationType(), FreezeOrWrapAttribute(a));
                    }
                    else if (obj is DynamicAnnotationAttribute)
                    {
                        a = (global::java.lang.annotation.Annotation)JVM.NewAnnotation(loader.GetJavaClassLoader(), ((DynamicAnnotationAttribute)obj).Definition);
                        if (a != null)
                            map.put(a.annotationType(), a);
                    }
                }
            }

            return map;
#endif
        }

#if !FIRST_PASS

        internal static global::java.lang.annotation.Annotation FreezeOrWrapAttribute(global::java.lang.annotation.Annotation ann)
        {
            var attr = ann as global::ikvm.@internal.AnnotationAttributeBase;
            if (attr != null)
            {
#if DONT_WRAP_ANNOTATION_ATTRIBUTES
			    attr.freeze();
#else
                // freeze to make sure the defaults are set
                attr.freeze();
                ann = global::sun.reflect.annotation.AnnotationParser.annotationForMap(attr.annotationType(), attr.getValues());
#endif
            }
            return ann;
        }
#endif

        public static object getDeclaredAnnotationsImpl(global::java.lang.Class thisClass)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var type = RuntimeJavaType.FromClass(thisClass);

            try
            {
                type.Finish();
            }
            catch (TranslatableJavaException e)
            {
                throw JVM.Context.ExceptionHelper.MapException<Exception>(e, true, false);
            }

            return AnnotationsToMap(type.ClassLoader, type.GetDeclaredAnnotations());
#endif
        }

        public static object getDeclaredFields0(global::java.lang.Class thisClass, bool publicOnly)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                var type = RuntimeJavaType.FromClass(thisClass);
                type.Finish();

                var fields = type.GetFields();
                var list = new List<global::java.lang.reflect.Field>(fields.Length);

                for (int i = 0; i < fields.Length; i++)
                {
                    if (fields[i].IsHideFromReflection)
                    {
                        // skip
                    }
                    else if (publicOnly && !fields[i].IsPublic)
                    {
                        // caller is only asking for public field, so we don't return this non-public field
                    }
                    else
                    {
                        list.Add((global::java.lang.reflect.Field)fields[i].ToField(false, i));
                    }
                }

                return list.ToArray();
            }
            catch (TranslatableJavaException e)
            {
                throw JVM.Context.ExceptionHelper.MapException<Exception>(e, true, false);
            }
#endif
        }

        public static object getDeclaredMethods0(global::java.lang.Class thisClass, bool publicOnly)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                var type = RuntimeJavaType.FromClass(thisClass);
                type.Finish();

                if (type.HasVerifyError)
                    throw new VerifyError();

                if (type.HasClassFormatError)
                    throw new ClassFormatError(type.Name);

                var methods = type.GetMethods();
                var list = new List<global::java.lang.reflect.Method>(methods.Length);
                foreach (var method in methods)
                {
                    // we don't want to expose "hideFromReflection" methods (one reason is that it would mess up the serialVersionUID computation)
                    if (!method.IsHideFromReflection && !method.IsConstructor && !method.IsClassInitializer && (!publicOnly || method.IsPublic))
                        list.Add((global::java.lang.reflect.Method)method.ToMethodOrConstructor(false));
                }

                return list.ToArray();
            }
            catch (TranslatableJavaException e)
            {
                throw JVM.Context.ExceptionHelper.MapException<Exception>(e, true, false);
            }
#endif
        }

        public static object getDeclaredConstructors0(global::java.lang.Class thisClass, bool publicOnly)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                var type = RuntimeJavaType.FromClass(thisClass);
                type.Finish();

                if (type.HasVerifyError)
                    throw new VerifyError();

                if (type.HasClassFormatError)
                    throw new ClassFormatError(type.Name);

                var methods = type.GetMethods();
                var list = new List<global::java.lang.reflect.Constructor>(methods.Length);
                foreach (var method in methods)
                {
                    // we don't want to expose "hideFromReflection" methods (one reason is that it would mess up the serialVersionUID computation)
                    if (method.IsHideFromReflection == false && method.IsConstructor)
                        if (publicOnly == false || method.IsPublic)
                            list.Add((global::java.lang.reflect.Constructor)method.ToMethodOrConstructor(false));
                }

                return list.ToArray();
            }
            catch (TranslatableJavaException e)
            {
                throw JVM.Context.ExceptionHelper.MapException<Exception>(e, true, false);
            }
#endif
        }

        public static global::java.lang.Class[] getDeclaredClasses0(global::java.lang.Class thisClass)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                var type = RuntimeJavaType.FromClass(thisClass);
                type.Finish();

                var innerTypes = type.InnerClasses;
                var list = new global::java.lang.Class[innerTypes.Length];
                for (int i = 0; i < list.Length; i++)
                {
                    var innerType = innerTypes[i].EnsureLoadable(type.ClassLoader);
                    if (innerType.IsAccessibleFrom(type) == false)
                        throw new IllegalAccessError(string.Format("tried to access class {0} from class {1}", innerType.Name, type.Name));

                    innerType.Finish();
                    list[i] = innerType.ClassObject;
                }

                return list;
            }
            catch (TranslatableJavaException e)
            {
                throw JVM.Context.ExceptionHelper.MapException<Exception>(e, true, false);
            }
#endif
        }

        public static bool desiredAssertionStatus0(global::java.lang.Class clazz)
        {
            return Assertions.IsEnabled(RuntimeJavaType.FromClass(clazz));
        }

    }

}
