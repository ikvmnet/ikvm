using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace IKVM.Runtime.JNI
{

    /// <summary>
    /// Provides access to the native companion methods.
    /// </summary>
    static unsafe class Native
    {

        public const string LIB_NAME = "ikvm-native";

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static Native()
        {
#if NETFRAMEWORK
            Load();
#endif
        }

        /// <summary>
        /// Preloads the native DLL for down-level platforms.
        /// </summary>
        static nint Load()
        {
            nint h;

            // attempt to load with default loader
            if ((h = NativeLibrary.Load(LIB_NAME)) != 0)
                return h;

            // scan known paths
            foreach (var path in GetLibraryPaths(LIB_NAME))
                if ((h = NativeLibrary.Load(path)) != 0)
                    return h;

            return 0;
        }

        /// <summary>
        /// Gets the RID architecture.
        /// </summary>
        /// <returns></returns>
        static string GetRuntimeIdentifierArch() => RuntimeInformation.ProcessArchitecture switch
        {
            Architecture.X86 => "x86",
            Architecture.X64 => "x64",
            Architecture.Arm => "arm",
            Architecture.Arm64 => "arm64",
            _ => null,
        };

        /// <summary>
        /// Gets the runtime identifiers of the current platform.
        /// </summary>
        /// <returns></returns>
        static IEnumerable<string> GetRuntimeIdentifiers()
        {
            var arch = GetRuntimeIdentifierArch();
            if (arch == null)
                yield break;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var v = Environment.OSVersion.Version;

                // Windows 10
                if (v.Major > 10 || (v.Major == 10 && v.Minor >= 0))
                    yield return $"win10-{arch}";

                // Windows 8.1
                if (v.Major > 6 || (v.Major == 6 && v.Minor >= 3))
                    yield return $"win81-{arch}";

                // Windows 7
                if (v.Major > 6 || (v.Major == 6 && v.Minor >= 1))
                    yield return $"win7-{arch}";

                // fallback
                yield return $"win-{arch}";
            }

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                yield return $"linux-{arch}";
            }
        }

        /// <summary>
        /// Gets the appropriate 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static string GetLibraryFileName(string name)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return $"{name}.dll";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return $"lib{name}.so";

            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets some library paths to check.
        /// </summary>
        /// <returns></returns>
        static IEnumerable<string> GetLibraryPaths(string name)
        {
            var self = Directory.GetParent(typeof(Native).Assembly.Location)?.FullName;
            if (self == null)
                yield break;

            var file = GetLibraryFileName(name);

            // search in runtime specific directories
            foreach (var rid in GetRuntimeIdentifiers())
                yield return Path.Combine(self, "runtimes", rid, "native", file);
        }

        /// <summary>
        /// Attempts to get the given named export.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="argl"></param>
        /// <returns></returns>
        public static nint GetExport(string name, int argl)
        {
            return NativeLibrary.GetExport(NativeLibrary.Load(LIB_NAME), name, argl);
        }

    }

}
