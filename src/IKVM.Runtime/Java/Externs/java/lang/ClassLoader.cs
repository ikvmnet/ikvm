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
using System.Linq;

using IKVM.ByteCode;
using IKVM.CoreLib.Exceptions;
using IKVM.Runtime;

namespace IKVM.Java.Externs.java.lang
{

    static partial class ClassLoader
    {

        /// <summary>
        /// Implements the native method 'registerNatives'.
        /// </summary>
        public static void registerNatives()
        {

        }

        /// <summary>
        /// Implements the native method 'defineClass0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="name"></param>
        /// <param name="b"></param>
        /// <param name="off"></param>
        /// <param name="len"></param>
        /// <param name="pd"></param>
        /// <returns></returns>
        public static global::java.lang.Class defineClass0(global::java.lang.ClassLoader self, string name, byte[] b, int off, int len, global::java.security.ProtectionDomain pd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var buf = new byte[len];
            Array.Copy(b, off, buf, 0, len);
            return DefineClass(self, name, ReadClass(buf), pd, null);
#endif
        }

        /// <summary>
        /// Implements the native method 'defineClass1'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="name"></param>
        /// <param name="b"></param>
        /// <param name="off"></param>
        /// <param name="len"></param>
        /// <param name="pd"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <exception cref="global::java.lang.NoClassDefFoundError"></exception>
        public static global::java.lang.Class defineClass1(global::java.lang.ClassLoader self, string name, byte[] b, int off, int len, global::java.security.ProtectionDomain pd, string source)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var buf = new byte[len];
            Array.Copy(b, off, buf, 0, len);
            return DefineClass(self, name, ReadClass(buf), pd, source);
#endif
        }

        /// <summary>
        /// Implements the native method 'defineClass2'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="name"></param>
        /// <param name="bb"></param>
        /// <param name="off"></param>
        /// <param name="len"></param>
        /// <param name="pd"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static unsafe global::java.lang.Class defineClass2(global::java.lang.ClassLoader self, string name, global::java.nio.ByteBuffer bb, int off, int len, global::java.security.ProtectionDomain pd, string source)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var buf = new byte[bb.remaining()];
            bb.get(buf);
            return DefineClass(self, name, ReadClass(buf), pd, source);
#endif
        }

#if FIRST_PASS == false

        /// <summary>
        /// Attempts to read the class, handling exceptions.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        static IKVM.ByteCode.Decoding.ClassFile ReadClass(byte[] buffer)
        {
            try
            {
                return IKVM.ByteCode.Decoding.ClassFile.Read(buffer);
            }
            catch (InvalidClassMagicException)
            {
                throw new global::java.lang.ClassFormatError("Incompatible magic value");
            }
            catch (ByteCodeException e)
            {
                throw new global::java.lang.ClassFormatError(e.Message);
            }
        }

        /// <summary>
        /// Defines a new class with the specified information.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="name"></param>
        /// <param name="clazz"></param>
        /// <param name="pd"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        static global::java.lang.Class DefineClass(global::java.lang.ClassLoader self, string name, IKVM.ByteCode.Decoding.ClassFile clazz, global::java.security.ProtectionDomain pd, string source)
        {
            try
            {
                var runtimeClassLoader = JVM.Context.ClassLoaderFactory.GetClassLoaderWrapper(self);
                var classFile = new ClassFile(JVM.Context, clazz, name, runtimeClassLoader.ClassFileParseOptions, []);
                if (name != null && classFile.Name != name)
                    throw new global::java.lang.NoClassDefFoundError(name + " (wrong name: " + classFile.Name + ")");

                var type = runtimeClassLoader.DefineClass(classFile, pd);
                if (type == null)
                    throw new InternalException();

                return type.ClassObject;
            }
            catch (TranslatableJavaException e)
            {
                throw JVM.Context.ExceptionHelper.MapException<Exception>(e, true, false);
            }
        }

#endif

        /// <summary>
        /// Implements the native method 'resolveClass0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="clazz"></param>
        public static void resolveClass0(global::java.lang.ClassLoader self, global::java.lang.Class clazz)
        {

        }

        /// <summary>
        /// Implements the native method 'findBootstrapClass'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static global::java.lang.Class findBootstrapClass(global::java.lang.ClassLoader self, string name)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                return JVM.Context.ClassLoaderFactory.GetBootstrapClassLoader().TryLoadClassByName(name)?.ClassObject;
            }
            catch (TranslatableJavaException e)
            {
                throw JVM.Context.ExceptionHelper.MapException<Exception>(e, true, false);
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'findLoadedClass0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static global::java.lang.Class findLoadedClass0(global::java.lang.ClassLoader self, string name)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return name != null ? (JVM.Context.ClassLoaderFactory.GetClassLoaderWrapper(self).FindLoadedClass(name)?.ClassObject) : null;
#endif
        }

        /// <summary>
        /// Implements the native method 'getBootstrapResource'.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static global::java.net.URL getBootstrapResource(string name)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return JVM.Context.ClassLoaderFactory.GetBootstrapClassLoader().GetResources(name).FirstOrDefault();
#endif
        }

        /// <summary>
        /// Implements the native method 'getBootstrapResources'.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static global::java.util.Enumeration getBootstrapResources(string name)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return new global::ikvm.runtime.EnumerationWrapper(JVM.Context.ClassLoaderFactory.GetBootstrapClassLoader().GetResources(name));
#endif
        }

        /// <summary>
        /// Implements the native method 'retrieveDirectives'.
        /// </summary>
        /// <returns></returns>
        public static object retrieveDirectives()
        {
            return IKVM.Runtime.Assertions.RetrieveDirectives();
        }

    }

}
