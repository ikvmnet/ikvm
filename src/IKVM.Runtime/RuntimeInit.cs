using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using IKVM.Attributes;

namespace IKVM.Runtime
{

    /// <summary>
    /// Initializes the JVM at module load.
    /// </summary>
    static class RuntimeInit
    {

#if FIRST_PASS == false && IMPORTER == false && EXPORTER == false

        /// <summary>
        /// Invoked on module initialize to load the JVM.
        /// </summary>
#pragma warning disable CA2255
        [ModuleInitializer]
#pragma warning restore CA2255
        internal static void Init()
        {
            // if our entry point is a Java application it will initialize the JVM as part of the launcher
            var isJavaApp = Assembly.GetEntryAssembly()?.ManifestModule?.GetCustomAttributes<JavaModuleAttribute>().Any() ?? false;
            if (isJavaApp == false)
                JVM.Init();
        }

#endif

    }

}
