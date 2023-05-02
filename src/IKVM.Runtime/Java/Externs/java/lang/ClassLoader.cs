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

using IKVM.ByteCode.Reading;
using IKVM.Internal;

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
            return defineClass1(self, name, b, off, len, pd, null);
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

            try
            {
                var classLoaderWrapper = ClassLoaderWrapper.GetClassLoaderWrapper(self);
                var classFile = new ClassFile(ClassReader.Read(new ReadOnlyMemory<byte>(b, off, len)), name, classLoaderWrapper.ClassFileParseOptions, null);
                if (name != null && classFile.Name != name)
                    throw new global::java.lang.NoClassDefFoundError(name + " (wrong name: " + classFile.Name + ")");

                var type = classLoaderWrapper.DefineClass(classFile, pd);
                return type.ClassObject;
            }
            catch (RetargetableJavaException x)
            {
                throw x.ToJava();
            }
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
        public static global::java.lang.Class defineClass2(global::java.lang.ClassLoader self, string name, global::java.nio.ByteBuffer bb, int off, int len, global::java.security.ProtectionDomain pd, string source)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var buf = new byte[bb.remaining()];
            bb.get(buf);
            return defineClass1(self, name, buf, 0, buf.Length, pd, source);
#endif
        }

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
                return ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedNameFast(name)?.ClassObject;
            }
            catch (RetargetableJavaException x)
            {
                throw x.ToJava();
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
            return name != null ? (ClassLoaderWrapper.GetClassLoaderWrapper(self).FindLoadedClass(name)?.ClassObject) : null;
        }

        /// <summary>
        /// Implements the native method 'getBootstrapResource'.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static global::java.net.URL getBootstrapResource(string name)
        {
            return ClassLoaderWrapper.GetBootstrapClassLoader().GetResources(name).FirstOrDefault();
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
            return new global::ikvm.runtime.EnumerationWrapper(ClassLoaderWrapper.GetBootstrapClassLoader().GetResources(name));
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
