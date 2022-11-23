using System;
using System.Runtime.InteropServices;

namespace IKVM.Runtime
{

    /// <summary>
    /// Provides methods to load a library.
    /// </summary>
    static class NativeLibrary
    {

#if NETFRAMEWORK

        /// <summary>
        /// Invokes the Windows LoadLibrary function.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "LoadLibrary", SetLastError = true)]
        static extern nint LoadLibrary(string path);

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
        [DllImport("kernel32.dll", EntryPoint = "GetProcAddress", SetLastError = true)]
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
        /// <returns></returns>
        [DllImport("dl", EntryPoint = "dlopen")]
        static extern nint dlopen(string path, int flags);

        /// <summary>
        /// Invokes the native 'dlclose' function.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [DllImport("dl", EntryPoint = "dlclose")]
        static extern int dlclose(nint handle);

        /// <summary>
        /// Invokes the naive 'dlsym' function.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [DllImport("dl", EntryPoint = "dlsym")]
        static extern nint dlsym(nint handle, string symbol);

#endif

        /// <summary>
        /// Invokes the Windows GetProcAddress function, handling 32-bit mangled names.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="argl"></param>
        /// <returns></returns>
        static string GetWin32ExportName(string name, int argl)
        {
            return name.Length <= 512 - 11 ? "_" + name + "@" + argl : null;
        }

        /// <summary>
        /// Loads the given library in a platform dependent manner.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public static nint Load(string path)
        {
#if NETFRAMEWORK
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return LoadLibrary(path);
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return dlopen(path, RTLD_NOW | RTLD_GLOBAL);
            else
                throw new PlatformNotSupportedException();
#else
            return System.Runtime.InteropServices.NativeLibrary.TryLoad(path, out var h) ? h : 0;
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
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                dlclose(handle);
            else
                throw new PlatformNotSupportedException();
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
                nint h = 0;

#if NETFRAMEWORK
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    h = Environment.Is64BitProcess == false ? GetProcAddress32(handle, name, argl) : GetProcAddress(handle, name);
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                    h = dlsym(handle, name);
                else
                    throw new PlatformNotSupportedException();
#else
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    if (Environment.Is64BitProcess == false && GetWin32ExportName(name, argl) is string n)
                        System.Runtime.InteropServices.NativeLibrary.TryGetExport(handle, n, out h);

                if (h == 0)
                    System.Runtime.InteropServices.NativeLibrary.TryGetExport(handle, name, out h);
#endif

                return h;
            }
            catch (EntryPointNotFoundException)
            {
                return 0;
            }
        }

    }

}
