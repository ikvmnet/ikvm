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
using System.IO;
using System.Reflection;

using IKVM.Runtime;
using IKVM.Runtime;

#if NETCOREAPP
using System.Runtime.Loader;
#endif

namespace IKVM.Java.Externs.sun.net.www.protocol.ikvmres
{

    static class Handler
    {

        public static byte[] GenerateStub(global::java.lang.Class c)
        {
            using var mem = new MemoryStream();
#if FIRST_PASS == false
            StubGen.StubGenerator.WriteClass(mem, RuntimeJavaType.FromClass(c), true, true, true, true, false);
#endif
            return mem.ToArray();
        }

        public static Stream ReadResourceFromAssemblyImpl(Assembly asm, string resource)
        {
            // chop off the leading slash
            resource = resource.Substring(1);
            string mangledName = JVM.MangleResourceName(resource);
            ManifestResourceInfo info = asm.GetManifestResourceInfo(mangledName);
            if (info != null && info.FileName != null)
            {
                return asm.GetManifestResourceStream(mangledName);
            }
            Stream s = asm.GetManifestResourceStream(mangledName);
            if (s == null)
            {
                Tracer.Warning(Tracer.ClassLoading, "Resource \"{0}\" not found in {1}", resource, asm.FullName);
                throw new FileNotFoundException("resource " + resource + " not found in assembly " + asm.FullName);
            }
            switch (s.ReadByte())
            {
                case 0:
                    Tracer.Info(Tracer.ClassLoading, "Reading resource \"{0}\" from {1}", resource, asm.FullName);
                    return s;
                case 1:
                    Tracer.Info(Tracer.ClassLoading, "Reading compressed resource \"{0}\" from {1}", resource, asm.FullName);
                    return new System.IO.Compression.DeflateStream(s, System.IO.Compression.CompressionMode.Decompress, false);
                default:
                    Tracer.Error(Tracer.ClassLoading, "Resource \"{0}\" in {1} has an unsupported encoding", resource, asm.FullName);
                    throw new IOException("Unsupported resource encoding for resource " + resource + " found in assembly " + asm.FullName);
            }
        }

        public static object LoadClassFromAssembly(Assembly asm, string className)
        {
            RuntimeJavaType tw = AssemblyClassLoader.FromAssembly(asm).LoadClassByDottedNameFast(className);
            if (tw != null)
            {
                return tw.ClassObject;
            }
            return null;
        }

        public static Assembly LoadAssembly(string name)
        {
#if NETFRAMEWORK
            return Assembly.Load(name);
#else
            return AssemblyLoadContext.GetLoadContext(typeof(Handler).Assembly).LoadFromAssemblyName(new AssemblyName(name));
#endif
        }

        public static global::java.lang.ClassLoader GetGenericClassLoaderById(int id)
        {
            return ClassLoaderWrapper.GetGenericClassLoaderById(id).GetJavaClassLoader();
        }

    }

}
