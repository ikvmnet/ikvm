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

using IKVM.Internal;

namespace IKVM.Java.Externs.java.lang
{

    static class Class
    {

        public static global::java.lang.Class forName0(string name, bool initialize, global::java.lang.ClassLoader loader, global::java.lang.Class caller)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (name == null)
                throw new global::java.lang.NullPointerException();

            TypeWrapper tw = null;
            if (name.IndexOf(',') > 0)
            {
                // we essentially require full trust before allowing arbitrary types to be loaded,
                // hence we do the "createClassLoader" permission check
                var sm = global::java.lang.System.getSecurityManager();
                sm?.checkPermission(new global::java.lang.RuntimePermission("createClassLoader"));

                var type = Type.GetType(name);
                if (type != null)
                    tw = ClassLoaderWrapper.GetWrapperFromType(type);

                if (tw == null)
                {
                    global::java.lang.Throwable.suppressFillInStackTrace = true;
                    throw new global::java.lang.ClassNotFoundException(name);
                }
            }
            else
            {
                try
                {
                    tw = ClassLoaderWrapper.GetClassLoaderWrapper(loader).LoadClassByDottedName(name);
                }
                catch (ClassNotFoundException x)
                {
                    global::java.lang.Throwable.suppressFillInStackTrace = true;
                    throw new global::java.lang.ClassNotFoundException(x.Message);
                }
                catch (ClassLoadingException x)
                {
                    throw x.InnerException;
                }
                catch (RetargetableJavaException x)
                {
                    throw x.ToJava();
                }
            }

            if (loader != null && caller != null && getProtectionDomain0(caller) is global::java.security.ProtectionDomain pd)
                loader.checkPackageAccess(tw.ClassObject, pd);

            if (initialize && tw.IsArray == false)
            {
                try
                {
                    tw.Finish();
                }
                catch (RetargetableJavaException x)
                {
                    throw x.ToJava();
                }

                tw.RunClassInit();
            }

            return tw.ClassObject;
#endif
        }

        public static byte[] getRawTypeAnnotations(global::java.lang.Class thisClass)
        {
            return TypeWrapper.FromClass(thisClass).GetRawTypeAnnotations();
        }

#if !FIRST_PASS

        private sealed class ConstantPoolImpl : global::sun.reflect.ConstantPool
        {
            private readonly object[] constantPool;

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

        public static object getConstantPool(global::java.lang.Class thisClass)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return new ConstantPoolImpl(TypeWrapper.FromClass(thisClass).GetConstantPool());
#endif
        }

        public static bool isInstance(global::java.lang.Class thisClass, object obj)
        {
            return TypeWrapper.FromClass(thisClass).IsInstance(obj);
        }

        public static bool isAssignableFrom(global::java.lang.Class thisClass, global::java.lang.Class otherClass)
        {
#if !FIRST_PASS
            if (otherClass == null)
                throw new global::java.lang.NullPointerException();
#endif
            return TypeWrapper.FromClass(otherClass).IsAssignableTo(TypeWrapper.FromClass(thisClass));
        }

        public static bool isInterface(global::java.lang.Class thisClass)
        {
            return TypeWrapper.FromClass(thisClass).IsInterface;
        }

        public static bool isArray(global::java.lang.Class thisClass)
        {
            return TypeWrapper.FromClass(thisClass).IsArray;
        }

        public static bool isPrimitive(global::java.lang.Class thisClass)
        {
            return TypeWrapper.FromClass(thisClass).IsPrimitive;
        }

        public static string getName0(global::java.lang.Class thisClass)
        {
            var tw = TypeWrapper.FromClass(thisClass);
            if (tw.IsPrimitive)
            {
                if (tw == PrimitiveTypeWrapper.BYTE)
                    return "byte";
                else if (tw == PrimitiveTypeWrapper.CHAR)
                    return "char";
                else if (tw == PrimitiveTypeWrapper.DOUBLE)
                    return "double";
                else if (tw == PrimitiveTypeWrapper.FLOAT)
                    return "float";
                else if (tw == PrimitiveTypeWrapper.INT)
                    return "int";
                else if (tw == PrimitiveTypeWrapper.LONG)
                    return "long";
                else if (tw == PrimitiveTypeWrapper.SHORT)
                    return "short";
                else if (tw == PrimitiveTypeWrapper.BOOLEAN)
                    return "boolean";
                else if (tw == PrimitiveTypeWrapper.VOID)
                    return "void";
            }

            if (tw.IsUnsafeAnonymous)
            {
#if !FIRST_PASS
                // for OpenJDK compatibility and debugging convenience we modify the class name to
                // include the identity hashcode of the class object
                return tw.Name + "/" + global::java.lang.System.identityHashCode(thisClass);
#endif
            }
            return tw.Name;
        }

        public static string getSigName(global::java.lang.Class thisClass)
        {
            return TypeWrapper.FromClass(thisClass).SigName;
        }

        public static global::java.lang.ClassLoader getClassLoader0(global::java.lang.Class thisClass)
        {
            return TypeWrapper.FromClass(thisClass).GetClassLoader().GetJavaClassLoader();
        }

        public static global::java.lang.Class getSuperclass(global::java.lang.Class thisClass)
        {
            var super = TypeWrapper.FromClass(thisClass).BaseTypeWrapper;
            return super != null ? super.ClassObject : null;
        }

        public static global::java.lang.Class[] getInterfaces0(global::java.lang.Class thisClass)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var ifaces = TypeWrapper.FromClass(thisClass).Interfaces;
            var interfaces = new global::java.lang.Class[ifaces.Length];
            for (int i = 0; i < ifaces.Length; i++)
                interfaces[i] = ifaces[i].ClassObject;

            return interfaces;
#endif
        }

        public static global::java.lang.Class getComponentType(global::java.lang.Class thisClass)
        {
            var tw = TypeWrapper.FromClass(thisClass);
            return tw.IsArray ? tw.ElementTypeWrapper.ClassObject : null;
        }

        public static int getModifiers(global::java.lang.Class thisClass)
        {
            // the 0x7FFF mask comes from JVM_ACC_WRITTEN_FLAGS in hotspot\src\share\vm\utilities\accessFlags.hpp
            // masking out ACC_SUPER comes from instanceKlass::compute_modifier_flags() in hotspot\src\share\vm\oops\instanceKlass.cpp
            const int mask = 0x7FFF & (int)~IKVM.Attributes.Modifiers.Super;
            return (int)TypeWrapper.FromClass(thisClass).ReflectiveModifiers & mask;
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
            try
            {
                var tw = TypeWrapper.FromClass(thisClass);
                tw.Finish();
                var enc = tw.GetEnclosingMethod();
                if (enc == null)
                    return null;

                return new object[] { tw.GetClassLoader().LoadClassByDottedName(enc[0]).ClassObject, enc[1], enc[2] == null ? null : enc[2].Replace('.', '/') };
            }
            catch (RetargetableJavaException x)
            {
                throw x.ToJava();
            }
        }

        public static global::java.lang.Class getDeclaringClass0(global::java.lang.Class thisClass)
        {
            try
            {
                var wrapper = TypeWrapper.FromClass(thisClass);
                wrapper.Finish();
                var decl = wrapper.DeclaringTypeWrapper;
                if (decl == null)
                    return null;

                decl = decl.EnsureLoadable(wrapper.GetClassLoader());
                if (!decl.IsAccessibleFrom(wrapper))
                    throw new IllegalAccessError(string.Format("tried to access class {0} from class {1}", decl.Name, wrapper.Name));

                decl.Finish();
                TypeWrapper[] declInner = decl.InnerClasses;
                for (int i = 0; i < declInner.Length; i++)
                {
                    if (declInner[i].Name == wrapper.Name && declInner[i].EnsureLoadable(decl.GetClassLoader()) == wrapper)
                    {
                        return decl.ClassObject;
                    }
                }
                throw new IncompatibleClassChangeError(string.Format("{0} and {1} disagree on InnerClasses attribute", decl.Name, wrapper.Name));
            }
            catch (RetargetableJavaException x)
            {
                throw x.ToJava();
            }
        }

        public static global::java.security.ProtectionDomain getProtectionDomain0(global::java.lang.Class thisClass)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var wrapper = TypeWrapper.FromClass(thisClass);
            if (wrapper.IsArray)
                return null;

            var pd = wrapper.ClassObject.pd;
            if (pd == null)
            {
                // The protection domain for statically compiled code is created lazily (not at global::java.lang.Class creation time),
                // to work around boot strap issues.
                var acl = wrapper.GetClassLoader() as AssemblyClassLoader;
                if (acl != null)
                    pd = acl.GetProtectionDomain();
                else if (wrapper is AnonymousTypeWrapper)
                {
                    // dynamically compiled intrinsified lamdba anonymous types end up here and should get their
                    // protection domain from the host class
                    pd = ClassLoaderWrapper.GetWrapperFromType(wrapper.TypeAsTBD.DeclaringType).ClassObject.pd;
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
        public static global::java.lang.Class getPrimitiveClass(string name) => name switch
        {
            "byte" => PrimitiveTypeWrapper.BYTE.ClassObject,
            "char" => PrimitiveTypeWrapper.CHAR.ClassObject,
            "double" => PrimitiveTypeWrapper.DOUBLE.ClassObject,
            "float" => PrimitiveTypeWrapper.FLOAT.ClassObject,
            "int" => PrimitiveTypeWrapper.INT.ClassObject,
            "long" => PrimitiveTypeWrapper.LONG.ClassObject,
            "short" => PrimitiveTypeWrapper.SHORT.ClassObject,
            "boolean" => PrimitiveTypeWrapper.BOOLEAN.ClassObject,
            "void" => PrimitiveTypeWrapper.VOID.ClassObject,
            _ => throw new ArgumentException(name),
        };

        public static string getGenericSignature0(global::java.lang.Class thisClass)
        {
            TypeWrapper tw = TypeWrapper.FromClass(thisClass);
            tw.Finish();
            return tw.GetGenericSignature();
        }

        internal static object AnnotationsToMap(ClassLoaderWrapper loader, object[] objAnn)
        {
#if FIRST_PASS
            return null;
#else
            global::java.util.LinkedHashMap map = new global::java.util.LinkedHashMap();
            if (objAnn != null)
            {
                foreach (object obj in objAnn)
                {
                    global::java.lang.annotation.Annotation a = obj as global::java.lang.annotation.Annotation;
                    if (a != null)
                    {
                        map.put(a.annotationType(), FreezeOrWrapAttribute(a));
                    }
                    else if (obj is IKVM.Attributes.DynamicAnnotationAttribute)
                    {
                        a = (global::java.lang.annotation.Annotation)JVM.NewAnnotation(loader.GetJavaClassLoader(), ((IKVM.Attributes.DynamicAnnotationAttribute)obj).Definition);
                        if (a != null)
                        {
                            map.put(a.annotationType(), a);
                        }
                    }
                }
            }
            return map;
#endif
        }

#if !FIRST_PASS
        internal static global::java.lang.annotation.Annotation FreezeOrWrapAttribute(global::java.lang.annotation.Annotation ann)
        {
            global::ikvm.@internal.AnnotationAttributeBase attr = ann as global::ikvm.@internal.AnnotationAttributeBase;
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
            return null;
#else
            TypeWrapper wrapper = TypeWrapper.FromClass(thisClass);
            try
            {
                wrapper.Finish();
            }
            catch (RetargetableJavaException x)
            {
                throw x.ToJava();
            }
            return AnnotationsToMap(wrapper.GetClassLoader(), wrapper.GetDeclaredAnnotations());
#endif
        }

        public static object getDeclaredFields0(global::java.lang.Class thisClass, bool publicOnly)
        {
#if FIRST_PASS
            return null;
#else
            Profiler.Enter("Class.getDeclaredFields0");
            try
            {
                TypeWrapper wrapper = TypeWrapper.FromClass(thisClass);
                // we need to finish the type otherwise all fields will not be in the field map yet
                wrapper.Finish();
                FieldWrapper[] fields = wrapper.GetFields();
                List<global::java.lang.reflect.Field> list = new List<global::java.lang.reflect.Field>();
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
            catch (RetargetableJavaException x)
            {
                throw x.ToJava();
            }
            finally
            {
                Profiler.Leave("Class.getDeclaredFields0");
            }
#endif
        }

        public static object getDeclaredMethods0(global::java.lang.Class thisClass, bool publicOnly)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            Profiler.Enter("Class.getDeclaredMethods0");
            try
            {
                TypeWrapper wrapper = TypeWrapper.FromClass(thisClass);
                wrapper.Finish();
                if (wrapper.HasVerifyError)
                {
                    // TODO we should get the message from somewhere
                    throw new VerifyError();
                }
                if (wrapper.HasClassFormatError)
                {
                    // TODO we should get the message from somewhere
                    throw new ClassFormatError(wrapper.Name);
                }
                MethodWrapper[] methods = wrapper.GetMethods();
                List<global::java.lang.reflect.Method> list = new List<global::java.lang.reflect.Method>(methods.Length);
                for (int i = 0; i < methods.Length; i++)
                {
                    // we don't want to expose "hideFromReflection" methods (one reason is that it would
                    // mess up the serialVersionUID computation)
                    if (!methods[i].IsHideFromReflection
                        && !methods[i].IsConstructor
                        && !methods[i].IsClassInitializer
                        && (!publicOnly || methods[i].IsPublic))
                    {
                        list.Add((global::java.lang.reflect.Method)methods[i].ToMethodOrConstructor(false));
                    }
                }
                return list.ToArray();
            }
            catch (RetargetableJavaException x)
            {
                throw x.ToJava();
            }
            finally
            {
                Profiler.Leave("Class.getDeclaredMethods0");
            }
#endif
        }

        public static object getDeclaredConstructors0(global::java.lang.Class thisClass, bool publicOnly)
        {
#if FIRST_PASS
            return null;
#else
            Profiler.Enter("Class.getDeclaredConstructors0");
            try
            {
                TypeWrapper wrapper = TypeWrapper.FromClass(thisClass);
                wrapper.Finish();
                if (wrapper.HasVerifyError)
                {
                    // TODO we should get the message from somewhere
                    throw new VerifyError();
                }
                if (wrapper.HasClassFormatError)
                {
                    // TODO we should get the message from somewhere
                    throw new ClassFormatError(wrapper.Name);
                }
                MethodWrapper[] methods = wrapper.GetMethods();
                List<global::java.lang.reflect.Constructor> list = new List<global::java.lang.reflect.Constructor>();
                for (int i = 0; i < methods.Length; i++)
                {
                    // we don't want to expose "hideFromReflection" methods (one reason is that it would
                    // mess up the serialVersionUID computation)
                    if (!methods[i].IsHideFromReflection
                        && methods[i].IsConstructor
                        && (!publicOnly || methods[i].IsPublic))
                    {
                        list.Add((global::java.lang.reflect.Constructor)methods[i].ToMethodOrConstructor(false));
                    }
                }
                return list.ToArray();
            }
            catch (RetargetableJavaException x)
            {
                throw x.ToJava();
            }
            finally
            {
                Profiler.Leave("Class.getDeclaredConstructors0");
            }
#endif
        }

        public static global::java.lang.Class[] getDeclaredClasses0(global::java.lang.Class thisClass)
        {
#if FIRST_PASS
            return null;
#else
            try
            {
                TypeWrapper wrapper = TypeWrapper.FromClass(thisClass);
                // NOTE to get at the InnerClasses we need to finish the type
                wrapper.Finish();
                TypeWrapper[] wrappers = wrapper.InnerClasses;
                global::java.lang.Class[] innerclasses = new global::java.lang.Class[wrappers.Length];
                for (int i = 0; i < innerclasses.Length; i++)
                {
                    TypeWrapper tw = wrappers[i].EnsureLoadable(wrapper.GetClassLoader());
                    if (!tw.IsAccessibleFrom(wrapper))
                    {
                        throw new IllegalAccessError(string.Format("tried to access class {0} from class {1}", tw.Name, wrapper.Name));
                    }
                    tw.Finish();
                    innerclasses[i] = tw.ClassObject;
                }
                return innerclasses;
            }
            catch (RetargetableJavaException x)
            {
                throw x.ToJava();
            }
#endif
        }

        public static bool desiredAssertionStatus0(global::java.lang.Class clazz)
        {
            return IKVM.Runtime.Assertions.IsEnabled(TypeWrapper.FromClass(clazz));
        }

    }

}
