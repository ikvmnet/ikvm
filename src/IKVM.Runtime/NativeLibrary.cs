using System;
using System.IO;
using System.Runtime.InteropServices;

namespace IKVM.Runtime
{

    /// <summary>
    /// Provides methods to load a library.
    /// </summary>
    static class NativeLibrary
    {

#if NETFRAMEWORK

        const int LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00001000;
        const int LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR = 0x00000100;

        /// <summary>
        /// Invokes the Windows LoadLibrary function.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "LoadLibraryEx", SetLastError = true)]
        static extern nint LoadLibraryEx(string path, nint hFile, int dwFlags);

        /// <summary>
        /// Invokes the Windows FreeLibrary function.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "FreeLibrary", SetLastError = true)]
        static extern nint FreeLibrary(nint handle);

        /// <summary>
        /// Invokes the Windows GetProcAddress function.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "GetProcAddress", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern nint GetProcAddress(nint handle, string name);

        const int RTLD_NOW = 2;
        const int RTLD_GLOBAL = 8;

        /// <summary>
        /// Invokes the native 'dlopen' function.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        [DllImport("dl", EntryPoint = "dlopen")]
        static extern nint dlopen(string path, int flags);

        /// <summary>
        /// Invokes the native 'dlclose' function.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("dl", EntryPoint = "dlclose")]
        static extern int dlclose(nint handle);

        /// <summary>
        /// Invokes the naive 'dlsym' function.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        [DllImport("dl", EntryPoint = "dlsym")]
        static extern nint dlsym(nint handle, string symbol);

#endif

        /// <summary>
        /// Loads the given library in a platform dependent manner.
        /// </summary>
        /// <param name="nameOrPath"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public static nint Load(string nameOrPath)
        {
            if (Path.IsPathRooted(nameOrPath))
                return LoadImpl(nameOrPath);

            // give native loader a chance to load the library
            if (LoadImpl(nameOrPath) is nint h1 and not 0)
                return h1;

            // start looking at assembly path, or path given by environmental variable
            var root = typeof(NativeLibrary).Assembly.Location is string s ? Path.GetDirectoryName(s) : null;

            // assembly possible loaded in memory: we have no available search path
            if (string.IsNullOrEmpty(root))
                return 0;

            // check in root directory
            if (Path.Combine(root, MapLibraryName(nameOrPath)) is string lib1)
                if (File.Exists(lib1) && LoadImpl(lib1) is nint h2 and not 0)
                    return h2;

            // scan supported RIDs for current platform
            foreach (var rid in RuntimeUtil.SupportedRuntimeIdentifiers)
                if (Path.Combine(root, "runtimes", rid, "native", MapLibraryName(nameOrPath)) is string lib2)
                    if (File.Exists(lib2) && LoadImpl(lib2) is nint h3 and not 0)
                        return h3;

            return 0;
        }

        /// <summary>
        /// Implements the native method 'mapLibraryName'.
        /// </summary>
        /// <returns></returns>
        static string MapLibraryName(string libname)
        {
            if (RuntimeUtil.IsWindows)
                return libname + ".dll";
            else if (RuntimeUtil.IsOSX)
                return "lib" + libname + ".dylib";
            else
                return "lib" + libname + ".so";
        }

        /// <summary>
        /// Loads the given library in a platform dependent manner.
        /// </summary>
        /// <param name="nameOrPath"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
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
        /// <exception cref="PlatformNotSupportedException"></exception>
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
        /// Returns the Win32 mangled procedure name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="argl"></param>
        /// <returns></returns>
        static string MangleWin32ExportName(string name, int argl)
        {
            return argl == -1 ? name : $"_{name}@{argl}";
        }

        /// <summary>
        /// Returns the mangled procedure name.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="argl"></param>
        /// <returns></returns>
        static string MangleExportName(string name, int argl)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && RuntimeInformation.ProcessArchitecture == Architecture.X86)
                return MangleWin32ExportName(name, argl);
            else
                return name;
        }

        /// <summary>
        /// Gets a function pointer to the given named function.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="name"></param>
        /// <param name="argl"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public static unsafe nint GetExport(nint handle, string name, int argl = -1)
        {
            name = MangleExportName(name, argl);

            try
            {
#if NETFRAMEWORK
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    return GetProcAddress(handle, name);
                else
                    return dlsym(handle, name);
#else
                return System.Runtime.InteropServices.NativeLibrary.TryGetExport(handle, name, out var h) ? h : 0;
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
