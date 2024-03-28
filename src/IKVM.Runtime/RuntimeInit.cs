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

        const string BASE_ASSEMBLY_NAME = "IVKM.Java";

        /// <summary>
        /// Invoked on module initialize to load the JVM.
        /// </summary>
#pragma warning disable CA2255
        [ModuleInitializer]
#pragma warning restore CA2255
        internal static void Init()
        {
#if NETFRAMEWORK
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
#endif

            // if our entry point is a Java application it will initialize the JVM as part of the launcher
            var isJavaApp = Assembly.GetEntryAssembly()?.ManifestModule?.GetCustomAttributes<JavaModuleAttribute>().Any() ?? false;
            if (isJavaApp == false)
                JVM.Init();
        }

#if NETFRAMEWORK

        /// <summary>
        /// Handles the <see cref="AppDomain.AssemblyResolve"/> event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        static Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name == BASE_ASSEMBLY_NAME || args.Name.StartsWith(BASE_ASSEMBLY_NAME) && args.Name.StartsWith($"{BASE_ASSEMBLY_NAME}, "))
                return LoadBaseAssembly();

            return null;
        }

        /// <summary>
        /// Attempts to resolve the given Java assembly.
        /// </summary>
        /// <returns></returns>
        static Assembly LoadBaseAssembly()
        {
            // check if already loaded
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                if (asm.GetName().Name == BASE_ASSEMBLY_NAME)
                    return asm;

            // start looking at assembly path, or path given by environmental variable
            var asml = typeof(RuntimeInit).Assembly.Location is string s ? Path.GetDirectoryName(s) : null;
            var root = Environment.GetEnvironmentVariable("IKVM_PLATFORM_PATH") ?? asml;

            // assembly possible loaded in memory: we have no available search path
            if (string.IsNullOrEmpty(root))
                return null;

            // scan supported RIDs for current platform
            foreach (var rid in RuntimeUtil.SupportedRuntimeIdentifiers)
                if (Path.Combine(root, "runtimes", rid, "lib", $"{BASE_ASSEMBLY_NAME}.dll") is string path2)
                    if (File.Exists(path2) && LoadAssembly(path2) is Assembly asm2)
                        return asm2;

            // check in root directory
            if (Path.Combine(root, $"{BASE_ASSEMBLY_NAME}.dll") is string path1)
                if (File.Exists(path1) && LoadAssembly(path1) is Assembly asm1)
                    return asm1;

            return null;
        }

        /// <summary>
        /// Loads the assembly at the given path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static Assembly LoadAssembly(string path)
        {
            return Assembly.LoadFrom(path);
        }

#endif

#endif

    }

}
