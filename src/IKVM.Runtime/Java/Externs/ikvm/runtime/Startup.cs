using System.Reflection;

using IKVM.Internal;

namespace IKVM.Java.Externs.ikvm.runtime
{

    /// <summary>
    /// Implements the backing support for <see cref="global::ikvm.runtime.Startup"/>.
    /// </summary>
    static class Startup
    {

        public static void addBootClassPathAssembly(Assembly asm)
        {
            ClassLoaderWrapper.GetBootstrapClassLoader().AddDelegate(IKVM.Internal.AssemblyClassLoader.FromAssembly(asm));
        }

    }

}
