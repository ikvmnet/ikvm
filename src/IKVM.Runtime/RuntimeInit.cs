using System;
using System.Collections.Generic;
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
#if NETFRAMEWORK
            // Framework requires the proper IKVM.Java to be loaded
            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;
#endif

            // if our entry point is a Java application it will initialize the JVM as part of the launcher
            var isJavaApp = Assembly.GetEntryAssembly()?.ManifestModule?.GetCustomAttributes<JavaModuleAttribute>().Any() ?? false;
            if (isJavaApp == false)
                JVM.Init();
        }

#if NETFRAMEWORK

        /// <summary>
        /// Handles loading the base library.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        static Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assemblyName = new AssemblyName(args.Name);
            if (assemblyName.Name == "IKVM.Java")
                if (TryLoadBaseLib(out var assembly))
                    return assembly;

            return null;
        }

        /// <summary>
        /// Attempts to load the base library.
        /// </summary>
        /// <returns></returns>
        static bool TryLoadBaseLib(out Assembly assembly)
        {
            assembly = null;

            // scan the combination of base and relative search paths
            foreach (var basePath in GetBaseSearchPaths())
                foreach (var relativePath in GetRelativeSearchPaths())
                    if (Path.Combine(basePath, relativePath) is string directory && Directory.Exists(directory))
                        if (TryLoadImpl(Path.Combine(directory, "IKVM.Java.dll"), out assembly))
                            return true;

            return false;
        }

        /// <summary>
        /// Iterates the base library search paths.
        /// </summary>
        /// <returns></returns>
        static IEnumerable<string> GetBaseSearchPaths()
        {
            // start at the appdomain base directory, unless overridden
            if (Environment.GetEnvironmentVariable("IKVM_LIBRARY_PATH") is string env && Directory.Exists(env))
                yield return env;

            // search from location of IKVM.Runtime
            if (typeof(RuntimeInit).Assembly.Location is string l)
                yield return Path.GetDirectoryName(l);

            // search from appdomain base directory
            if (AppDomain.CurrentDomain.BaseDirectory is string app && Directory.Exists(app))
                yield return app;
        }

        /// <summary>
        /// Iterates the relative search paths.
        /// </summary>
        /// <returns></returns>
        static IEnumerable<string> GetRelativeSearchPaths()
        {
            foreach (var rid in RuntimeUtil.SupportedRuntimeIdentifiers.Append(""))
                foreach (var tfm in GetSupportedTargetFrameworks())
                    yield return Path.Combine("runtimes", rid, "lib", tfm);

            yield return "";
        }

        /// <summary>
        /// Iterates the supported target frameworks.
        /// </summary>
        /// <returns></returns>
        static IEnumerable<string> GetSupportedTargetFrameworks()
        {
#if NETFRAMEWORK
            yield return "net472";
#else
            yield return "net6.0";
#endif
            yield return "";
        }

        /// <summary>
        /// Attempts to load the specified assembly.
        /// </summary>
        /// <param name="assemblyFile"></param>
        /// <returns></returns>
        static bool TryLoadImpl(string assemblyFile, out Assembly assembly)
        {
            assembly = null;

            if (File.Exists(assemblyFile) == false)
                return false;

            try
            {
                assembly = Assembly.LoadFrom(assemblyFile);
                return true;
            }
            catch
            {
                // ignore
            }

            return false;
        }

#endif

#endif

    }

}
