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

        const int LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR = 0x00000100;
        const int LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00001000;

        /// <summary>
        /// Invokes the Windows LoadLibrary function.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="hFile"></param>
        /// <param name="dwFlags"></param>
        /// <returns></returns>
        [DllImport("kernel32", EntryPoint = "LoadLibraryEx", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern nint LoadLibraryEx(string path, nint hFile, int dwFlags);

        /// <summary>
        /// Invokes the Windows FreeLibrary function.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("kernel32", EntryPoint = "FreeLibrary", SetLastError = true)]
        static extern nint FreeLibrary(nint handle);

        /// <summary>
        /// Invokes the Windows GetProcAddress function.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [DllImport("kernel32", EntryPoint = "GetProcAddress", SetLastError = true)]
        static extern nint GetProcAddress(nint handle, string name);

        /// <summary>
        /// Invokes the Windows GetProcAddress function, handling 32-bit mangled names.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="name"></param>
        /// <param name="argl"></param>
        /// <returns></returns>
        static nint GetProcAddress32(nint handle, string name, int argl)
        {
            var h = GetWin32ExportName(name, argl) is string n ? GetProcAddress(handle, n) : 0;
            if (h == 0)
                h = GetProcAddress(handle, name);

            return h;
        }

        const int RTLD_NOW = 2;
        const int RTLD_GLOBAL = 8;

        /// <summary>
        /// Invokes the native 'dlopen' function.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        [DllImport("jvm", EntryPoint = "IKVM_dlopen")]
        static extern nint dlopen(string path, int flags);

        /// <summary>
        /// Invokes the native 'dlclose' function.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("jvm", EntryPoint = "IKVM_dlclose")]
        static extern int dlclose(nint handle);

        /// <summary>
        /// Invokes the naive 'dlsym' function.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        [DllImport("jvm", EntryPoint = "IKVM_dlsym")]
        static extern nint dlsym(nint handle, string symbol);

        /// <summary>
        /// Returns the symbol name, mangled for 32-bit Windows.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="argl"></param>
        /// <returns></returns>
        static string GetWin32ExportName(string name, int argl)
        {
            return name.Length <= 512 - 11 ? "_" + name + "@" + argl : name;
        }

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
                return LoadImpl(nameOrPath);

            // not a path, map the name to local OS convention
            var mappedLibraryName = nameOrPath;
            if (mappedLibraryName.Contains(Path.PathSeparator.ToString()) == false)
                mappedLibraryName = MapLibraryName(mappedLibraryName);

            // scan runtime native paths first
            foreach (var i in GetNativePaths())
                if (LoadImpl(Path.Combine(i, mappedLibraryName)) is nint h1 and not 0)
                    return h1;

            // let loader have a chance at the raw name, which may scan OS specific things
            if (LoadImpl(nameOrPath) is nint h2 and not 0)
                return h2;

            // let loader have a chance at the mapped name, which may be more accurate
            if (LoadImpl(mappedLibraryName) is nint h3 and not 0)
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
                return libname + ".dll";
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                return "lib" + libname + ".dylib";
            else
                return "lib" + libname + ".so";
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
        /// Loads the given library in a platform dependent manner.
        /// </summary>
        /// <param name="nameOrPath"></param>
        /// <returns></returns>
        static nint LoadImpl(string nameOrPath)
        {
#if NETFRAMEWORK
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return LoadLibraryEx(nameOrPath, 0, LOAD_LIBRARY_SEARCH_DEFAULT_DIRS | LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR);
            else
                return dlopen(nameOrPath, RTLD_NOW | RTLD_GLOBAL);
#else
            return System.Runtime.InteropServices.NativeLibrary.TryLoad(nameOrPath, typeof(NativeLibrary).Assembly, DllImportSearchPath.SafeDirectories | DllImportSearchPath.UseDllDirectoryForDependencies, out var h) ? h : 0;
#endif
        }

        /// <summary>
        /// Frees the given library in a platform dependent manner.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static void Free(nint handle)
        {
#if NETFRAMEWORK
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                FreeLibrary(handle);
            else
                dlclose(handle);
#else
            System.Runtime.InteropServices.NativeLibrary.Free(handle);
#endif
        }

        /// <summary>
        /// Gets a function pointer to the given named function.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="name"></param>
        /// <param name="argl"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public static unsafe nint GetExport(nint handle, string name, int argl)
        {
            try
            {
#if NETFRAMEWORK
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    return Environment.Is64BitProcess == false ? GetProcAddress32(handle, name, argl) : GetProcAddress(handle, name);
                else
                    return dlsym(handle, name);
#else
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    return System.Runtime.InteropServices.NativeLibrary.TryGetExport(handle, Environment.Is64BitProcess == false ? GetWin32ExportName(name, argl) : name, out var h1) ? h1 : 0;
                else
                    return System.Runtime.InteropServices.NativeLibrary.TryGetExport(handle, name, out var h2) ? h2 : 0;
#endif
            }
            catch (EntryPointNotFoundException)
            {
                // symbol not found, default to 0
            }

            return 0;
        }

    }

}
