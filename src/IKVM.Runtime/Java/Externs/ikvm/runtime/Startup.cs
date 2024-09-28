using System;
using System.Reflection;

using IKVM.Runtime;

namespace IKVM.Java.Externs.ikvm.runtime
{

    /// <summary>
    /// Implements the backing support for <see cref="global::ikvm.runtime.Startup"/>.
    /// </summary>
    static class Startup
    {

        public static void addBootClassPathAssembly(Assembly asm)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            JVM.Context.ClassLoaderFactory.GetBootstrapClassLoader().AddDelegate(JVM.Context.AssemblyClassLoaderFactory.FromAssembly(JVM.Context.Resolver.GetSymbol(asm)));
#endif
        }

    }

}
