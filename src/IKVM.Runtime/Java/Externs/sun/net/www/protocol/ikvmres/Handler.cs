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
using System.IO;
using System.Reflection;

using IKVM.CoreLib.Diagnostics;
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
#if FIRST_PASS
            throw new NotImplementedException();
#else
            using var mem = new MemoryStream();
            JVM.Context.StubGenerator.Write(mem, RuntimeJavaType.FromClass(c), true, true, true, true, false);
            return mem.ToArray();
#endif
        }

        public static Stream ReadResourceFromAssemblyImpl(Assembly asm, string resource)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            // chop off the leading slash
            resource = resource.Substring(1);
            var mangledName = JVM.MangleResourceName(resource);
            var info = asm.GetManifestResourceInfo(mangledName);
            if (info != null && info.FileName != null)
                return asm.GetManifestResourceStream(mangledName);

            var s = asm.GetManifestResourceStream(mangledName);
            if (s == null)
            {
                JVM.Context.ReportEvent(Diagnostic.GenericClassLoadingWarning.Event([$"Resource \"{resource}\" not found in {asm.FullName}"]));
                throw new FileNotFoundException("resource " + resource + " not found in assembly " + asm.FullName);
            }

            switch (s.ReadByte())
            {
                case 0:
                    JVM.Context.ReportEvent(Diagnostic.GenericClassLoadingInfo.Event([$"Reading resource \"{resource}\" from {asm.FullName}"]));
                    return s;
                case 1:
                    JVM.Context.ReportEvent(Diagnostic.GenericClassLoadingInfo.Event([$"Reading compressed resource \"{resource}\" from {asm.FullName}"]));
                    return new System.IO.Compression.DeflateStream(s, System.IO.Compression.CompressionMode.Decompress, false);
                default:
                    JVM.Context.ReportEvent(Diagnostic.GenericClassLoadingError.Event([$"Resource \"{resource}\" in {asm.FullName} has an unsupported encoding"]));
                    throw new IOException("Unsupported resource encoding for resource " + resource + " found in assembly " + asm.FullName);
            }
#endif
        }

        public static object LoadClassFromAssembly(Assembly asm, string className)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var tw = JVM.Context.AssemblyClassLoaderFactory.FromAssembly(asm).TryLoadClassByName(className);
            if (tw != null)
                return tw.ClassObject;

            return null;
#endif
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
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return JVM.Context.ClassLoaderFactory.GetGenericClassLoaderById(id).GetJavaClassLoader();
#endif
        }

    }

}
