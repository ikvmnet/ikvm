using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace IKVM.Runtime
{

    /// <summary>
    /// Provides methods to load a library.
    /// </summary>
    static class NativeLibrary
    {

        /// <summary>
        /// Invokes the native 'dlopen' function.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [DllImport("jvm", EntryPoint = "JVM_LoadLibrary", SetLastError = false)]
        static extern nint JVM_LoadLibrary(string path);

        /// <summary>
        /// Invokes the native 'dlsym' function.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        [DllImport("jvm", EntryPoint = "JVM_FindLibraryEntry", SetLastError = false)]
        static extern nint JVM_FindLibraryEntry(nint handle, string symbol);

        /// <summary>
        /// Invokes the native 'dlclose' function.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("jvm", EntryPoint = "JVM_UnloadLibrary", SetLastError = false)]
        static extern void JVM_UnloadLibrary(nint handle);

        /// <summary>
        /// Loads the given library in a platform dependent manner.
        /// </summary>
        /// <param name="nameOrPath"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public static nint Load(string nameOrPath)
        {
            // always resolve full paths without modification
            if (Path.IsPathRooted(nameOrPath))
                return JVM_LoadLibrary(nameOrPath);

            // not a path, map the name to local OS convention
            var mappedLibraryName = nameOrPath;
            if (mappedLibraryName.Contains(Path.PathSeparator.ToString()) == false)
                mappedLibraryName = MapLibraryName(mappedLibraryName);

            // scan runtime native paths first
            foreach (var i in GetNativePaths())
                if (JVM_LoadLibrary(Path.Combine(i, mappedLibraryName)) is nint h1 and not 0)
                    return h1;

            // let loader have a chance at the raw name, which may scan OS specific things
            if (JVM_LoadLibrary(nameOrPath) is nint h2 and not 0)
                return h2;

            // let loader have a chance at the mapped name, which may be more accurate
            if (JVM_LoadLibrary(mappedLibraryName) is nint h3 and not 0)
                return h3;

            return 0;
        }

        /// <summary>
        /// Implements the native method 'mapLibraryName'.
        /// </summary>
        /// <returns></returns>
        static string MapLibraryName(string libname)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return $"{libname}.dll";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return $"lib{libname}.so";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return $"lib{libname}.dylib";

            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Gets the boot library paths.
        /// </summary>
        /// <returns></returns>
        static IEnumerable<string> GetNativePaths()
        {
            var self = Directory.GetParent(typeof(NativeLibrary).Assembly.Location)?.FullName;
            if (self == null)
                yield break;

            // search in runtime specific directories
            foreach (var rid in RuntimeUtil.SupportedRuntimeIdentifiers)
                yield return Path.Combine(self, "runtimes", rid, "native");
        }

        /// <summary>
        /// Frees the given library in a platform dependent manner.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static void Free(nint handle)
        {
            JVM_UnloadLibrary(handle);
        }

        /// <summary>
        /// Gets a function pointer to the given named function.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static nint GetExport(nint handle, string name)
        {
            return JVM_FindLibraryEntry(handle, name);
        }

    }

}
