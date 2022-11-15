using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace IKVM.Runtime
{

    /// <summary>
    /// Provides access to the native companion methods.
    /// </summary>
    static unsafe class Native
    {

        public const string LIB_NAME = "ikvm-native";

#if NETFRAMEWORK

        /// <summary>
        /// Native Win32 LoadLibrary method.
        /// </summary>
        /// <param name="dllToLoad"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibrary(string dllToLoad);

#endif

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static Native()
        {
#if NETFRAMEWORK
            LegacyImportDll();
#else
            NativeLibrary.SetDllImportResolver(typeof(Native).Assembly, DllImportResolver);
#endif
        }

#if NETFRAMEWORK

        /// <summary>
        /// Preloads the native DLL for down-level platforms.
        /// </summary>
        static void LegacyImportDll()
        {
            // attempt to load with default loader
            var h = LoadLibrary(LIB_NAME);
            if (h != IntPtr.Zero)
                return;

            // scan known paths
            foreach (var path in GetLibraryPaths(LIB_NAME))
            {
                h = LoadLibrary(path);
                if (h != IntPtr.Zero)
                    return;
            }
        }

#endif

#if NETCOREAPP

        /// <summary>
        /// Attempts to resolve the specified assembly when running on .NET Core 3.1 and above.
        /// </summary>
        /// <param name="libraryName"></param>
        /// <param name="assembly"></param>
        /// <param name="searchPath"></param>
        /// <returns></returns>
        static IntPtr DllImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            if (libraryName == LIB_NAME)
            {
                // attempt to load with default loader
                if (NativeLibrary.TryLoad(libraryName, out var h) && h != IntPtr.Zero)
                    return h;

                // scan known paths
                foreach (var path in GetLibraryPaths(libraryName))
                    if (NativeLibrary.TryLoad(path, out h) && h != IntPtr.Zero)
                        return h;
            }

            return IntPtr.Zero;
        }

#endif

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

            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                yield return $"osx-{arch}";
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
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return $"lib{name}.so";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return $"lib{name}.dynlib";

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

        [DllImport(LIB_NAME, EntryPoint = "ikvm_GetJNIEnvVTable")]
        public static extern void** ikvm_GetJNIEnvVTable();

    }

}
