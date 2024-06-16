using System;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace IKVM.JTReg.TestAdapter
{

    static class JTRegInit
    {

        /// <summary>
        /// Initializes the module. We install a assembly resolver that attempts to locate assemblies in the same directory as the adapter.
        /// </summary>
        [ModuleInitializer]
        public static void Init()
        {
#if NETFRAMEWORK
            AppDomain.CurrentDomain.AssemblyResolve += (s, a) =>
            {
                var l = typeof(JTRegInit).Assembly.Location;
                if (string.IsNullOrWhiteSpace(l))
                    return null;

                var n = new AssemblyName(a.Name);
                var p = Path.Combine(Path.GetDirectoryName(l), n.Name + ".dll");
                if (File.Exists(p) == false)
                    return null;

                try
                {
                    return Assembly.LoadFrom(p);
                }
                catch
                {
                    // ignore failure to load
                }

                return null;
            };
#endif
        }

    }

}
