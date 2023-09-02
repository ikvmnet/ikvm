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

        /// <summary>
        /// Holds a reference to the 'ikvm' native library. Only required for .NET Framework which has no NativeLibrary class.
        /// </summary>
        class LibIkvm
        {

            /// <summary>
            /// Holds the externs to prevent initialization.
            /// </summary>
            static class Externs
            {

                /// <summary>
                /// Invokes the native 'dlopen' function.
                /// </summary>
                /// <param name="path"></param>
                /// <returns></returns>
                [DllImport("ikvm", SetLastError = false)]
                internal static extern nint IKVM_dlopen(string path);

                /// <summary>
                /// Invokes the native 'dlsym' function.
                /// </summary>
                /// <param name="handle"></param>
                /// <param name="symbol"></param>
                /// <returns></returns>
                [DllImport("ikvm", SetLastError = false)]
                internal static extern nint IKVM_dlsym(nint handle, string symbol);

                /// <summary>
                /// Invokes the native 'dlclose' function.
                /// </summary>
                /// <param name="handle"></param>
                /// <returns></returns>
                [DllImport("ikvm", SetLastError = false)]
                internal static extern void IKVM_dlclose(nint handle);

            }

            /// <summary>
            /// Invokes the native 'LoadLibrary' function.
            /// </summary>
            /// <param name="lpLibFileName"></param>
            /// <returns></returns>
            [DllImport("kernel32")]
            static extern nint LoadLibrary(string lpLibFileName);

            /// <summary>
            /// Invokes the native 'FreeLibrary' function.
            /// </summary>
            /// <param name="handle"></param>
            /// <returns></returns>
            [DllImport("kernel32")]
            static extern nint FreeLibrary(nint handle);

            /// <summary>
            /// Holds an internal handle to the 'ikvm' library.
            /// </summary>
            nint handle = PreLoad();

            /// <summary>
            /// Loads the 'ikvm' library.
            /// </summary>
            static nint PreLoad()
            {
                // preload only required on non-Mono, which cannot use dllmap files
                if (RuntimeUtil.IsMono == false)
                {
                    // give native loader a chance to load the library
                    if (LoadLibrary("ikvm") is nint h1 and not 0)
                        return h1;

                    // start looking at assembly path, or path given by environmental variable
                    var asml = typeof(NativeLibrary).Assembly.Location is string s ? Path.GetDirectoryName(s) : null;
                    var root = Environment.GetEnvironmentVariable("IKVM_LIBRARY_PATH") ?? asml;

                    // scan supported RIDs for current platform
                    foreach (var rid in RuntimeUtil.SupportedRuntimeIdentifiers)
                        if (Path.Combine(root, "runtimes", rid, "native", "ikvm.dll") is string lib)
                            if (File.Exists(lib) && LoadLibrary(lib) is nint h2 and not 0)
                                return h2;
                }

                return 0;
            }

            /// <summary>
            /// Invokes the 'dlopen' function.
            /// </summary>
            /// <param name="path"></param>
            /// <returns></returns>
            public nint dlopen(string path) => Externs.IKVM_dlopen(path);

            /// <summary>
            /// Invokes the 'dlsym' function.
            /// </summary>
            /// <param name="handle"></param>
            /// <param name="symbol"></param>
            /// <returns></returns>
            public nint dlsym(nint handle, string symbol) => Externs.IKVM_dlsym(handle, symbol);

            /// <summary>
            /// Invokes the 'dlclose' function.
            /// </summary>
            /// <param name="handle"></param>
            /// <returns></returns>
            public void dlclose(nint handle) => Externs.IKVM_dlclose(handle);

            /// <summary>
            /// Releases the instance.
            /// </summary>
            ~LibIkvm()
            {
                if (handle != 0)
                {
                    FreeLibrary(handle);
                    handle = 0;
                }
            }

        }

        readonly static LibIkvm libikvm = new LibIkvm();

#endif

        /// <summary>
        /// Invokes the appropriate method to simulate dlopen.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static nint dlopen(string path)
        {
#if NETFRAMEWORK
            return libikvm.dlopen(path);
#else
            return System.Runtime.InteropServices.NativeLibrary.TryLoad(path, typeof(NativeLibrary).Assembly, DllImportSearchPath.SafeDirectories | DllImportSearchPath.UseDllDirectoryForDependencies, out nint h) ? h : 0;
#endif
        }

        /// <summary>
        /// Loads the given library in a platform dependent manner.
        /// </summary>
        /// <param name="nameOrPath"></param>
        /// <returns></returns>
        public static nint Load(string nameOrPath)
        {
            if (Path.IsPathRooted(nameOrPath))
                return dlopen(nameOrPath);

            // give native loader a chance to load the library
            if (dlopen(nameOrPath) is nint h1 and not 0)
                return h1;

            // start looking at assembly path, or path given by environmental variable
            var asml = typeof(NativeLibrary).Assembly.Location is string s ? Path.GetDirectoryName(s) : null;
            var root = Environment.GetEnvironmentVariable("IKVM_LIBRARY_PATH") ?? asml;

            // scan supported RIDs for current platform
            foreach (var rid in RuntimeUtil.SupportedRuntimeIdentifiers)
                if (Path.Combine(root, "runtimes", rid, "native", nameOrPath) is string lib)
                    if (File.Exists(lib) && dlopen(lib) is nint h2 and not 0)
                        return h2;

            return 0;
        }

        /// <summary>
        /// Frees the given library in a platform dependent manner.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static void Free(nint handle)
        {
#if NETFRAMEWORK
            libikvm.dlclose(handle);
#else
            System.Runtime.InteropServices.NativeLibrary.Free(handle);
#endif
        }

        /// <summary>
        /// Gets a function pointer to the given named function.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static nint GetExport(nint handle, string name)
        {
#if NETFRAMEWORK
            return libikvm.dlsym(handle, name);
#else
            return System.Runtime.InteropServices.NativeLibrary.TryGetExport(handle, name, out var h) ? h : 0;
#endif
        }

    }

}
