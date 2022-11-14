using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace IKVM.Tests
{

    /// <summary>
    /// Assists in discovering the native test library.
    /// </summary>
    static unsafe class Native
    {

        public const string LIB_NAME = "ikvm-tests-native";

        /// <summary>
        /// Ensures the native library is loaded.
        /// </summary>
        public static string GetLibraryPath()
        {
            foreach (var path in GetLibraryPaths(LIB_NAME))
                if (File.Exists(path))
                    return path;

            return null;
        }

        /// <summary>
        /// Gets the RID architecture.
        /// </summary>
        /// <returns></returns>
        static string GetRuntimeIdentifierArch()
        {
            if (RuntimeInformation.ProcessArchitecture == Architecture.X86)
                return "x86";
            else if (RuntimeInformation.ProcessArchitecture == Architecture.X64)
                return "x64";
            else if (RuntimeInformation.ProcessArchitecture == Architecture.Arm)
                return "arm";
            else if (RuntimeInformation.ProcessArchitecture == Architecture.Arm64)
                return "arm64";
            else
                return null;
        }

        /// <summary>
        /// Gets the runtime identifiers of the current platform.
        /// </summary>
        /// <returns></returns>
        static IEnumerable<string> GetRuntimeIdentifiers()
        {
            var arch = GetRuntimeIdentifierArch();

#if NETFRAMEWORK
            yield return $"win-{arch}";
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                yield return $"win-{arch}";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                yield return $"linux-{arch}";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                yield return $"osx-{arch}";
#endif
        }

        /// <summary>
        /// Gets the appropriate 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static string GetLibraryFileName(string name)
        {
#if NETFRAMEWORK
            return $"{name}.dll";
#else
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return $"{name}.dll";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return $"lib{name}.so";

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return $"lib{name}.dynlib";
#endif

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

    }

}
