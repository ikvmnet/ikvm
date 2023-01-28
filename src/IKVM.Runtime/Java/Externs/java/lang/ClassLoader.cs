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

using IKVM.ByteCode.Reading;
using IKVM.Internal;

namespace IKVM.Java.Externs.java.lang
{

    static partial class ClassLoader
    {

        public static global::java.net.URL getBootstrapResource(string name)
        {
            foreach (global::java.net.URL url in ClassLoaderWrapper.GetBootstrapClassLoader().GetResources(name))
                return url;

            return null;
        }

        public static global::java.util.Enumeration getBootstrapResources(string name)
        {
#if FIRST_PASS
            return null;
#else
            return new global::ikvm.runtime.EnumerationWrapper(ClassLoaderWrapper.GetBootstrapClassLoader().GetResources(name));
#endif
        }

        public static global::java.lang.Class defineClass0(global::java.lang.ClassLoader thisClassLoader, string name, byte[] b, int off, int len, global::java.security.ProtectionDomain pd)
        {
            return defineClass1(thisClassLoader, name, b, off, len, pd, null);
        }

        public static global::java.lang.Class defineClass1(global::java.lang.ClassLoader thisClassLoader, string name, byte[] b, int off, int len, global::java.security.ProtectionDomain pd, string source)
        {
            // it appears the source argument is only used for trace messages in HotSpot. We'll just ignore it for now.
            Profiler.Enter("ClassLoader.defineClass");
            try
            {
                try
                {
                    var classLoaderWrapper = ClassLoaderWrapper.GetClassLoaderWrapper(thisClassLoader);
                    var classFile = new ClassFile(ClassReader.Read(new ReadOnlyMemory<byte>(b, off, len)), name, classLoaderWrapper.ClassFileParseOptions, null);
                    if (name != null && classFile.Name != name)
                    {
#if !FIRST_PASS
                        throw new global::java.lang.NoClassDefFoundError(name + " (wrong name: " + classFile.Name + ")");
#endif
                    }

                    var type = classLoaderWrapper.DefineClass(classFile, pd);
                    return type.ClassObject;
                }
                catch (RetargetableJavaException x)
                {
                    throw x.ToJava();
                }
            }
            finally
            {
                Profiler.Leave("ClassLoader.defineClass");
            }
        }

        public static global::java.lang.Class defineClass2(global::java.lang.ClassLoader thisClassLoader, string name, global::java.nio.ByteBuffer bb, int off, int len, global::java.security.ProtectionDomain pd, string source)
        {
#if FIRST_PASS
            return null;
#else
            byte[] buf = new byte[bb.remaining()];
            bb.get(buf);
            return defineClass1(thisClassLoader, name, buf, 0, buf.Length, pd, source);
#endif
        }

        public static void resolveClass0(global::java.lang.ClassLoader thisClassLoader, global::java.lang.Class clazz)
        {
            // no-op
        }

        public static global::java.lang.Class findBootstrapClass(global::java.lang.ClassLoader thisClassLoader, string name)
        {
#if FIRST_PASS
            return null;
#else
            TypeWrapper tw;
            try
            {
                tw = ClassLoaderWrapper.GetBootstrapClassLoader().LoadClassByDottedNameFast(name);
            }
            catch (RetargetableJavaException x)
            {
                throw x.ToJava();
            }
            return tw != null ? tw.ClassObject : null;
#endif
        }

        public static global::java.lang.Class findLoadedClass0(global::java.lang.ClassLoader thisClassLoader, string name)
        {
            if (name == null)
            {
                return null;
            }
            ClassLoaderWrapper loader = ClassLoaderWrapper.GetClassLoaderWrapper(thisClassLoader);
            TypeWrapper tw = loader.FindLoadedClass(name);
            return tw != null ? tw.ClassObject : null;
        }

        public static object retrieveDirectives()
        {
            return IKVM.Runtime.Assertions.RetrieveDirectives();
        }

    }

}
