using System;
using System.IO;
using System.Runtime.InteropServices;

namespace IKVM.Runtime
{

#if FIRST_PASS == false && IMPORTER == FALSE && EXPORTER == false

    /// <summary>
    /// Holds a reference to the 'ikvm' native library. This class cannot depend on NativeLibrary as that class can
    /// potentially use libikvm on down-level platforms.
    /// </summary>
    internal class LibIkvm
    {

        /// <summary>
        /// Holds the externs to prevent initialization.
        /// </summary>
        static class Externs
        {

            /// <summary>
            /// Invokes the native 'IKVM_dl_open' function.
            /// </summary>
            /// <param name="path"></param>
            /// <returns></returns>
            [DllImport("ikvm", SetLastError = false)]
            internal static extern nint IKVM_dl_open(string path);

            /// <summary>
            /// Invokes the native 'IKVM_dl_sym' function.
            /// </summary>
            /// <param name="handle"></param>
            /// <param name="symbol"></param>
            /// <returns></returns>
            [DllImport("ikvm", SetLastError = false)]
            internal static extern nint IKVM_dl_sym(nint handle, string symbol);

            /// <summary>
            /// Invokes the native 'IKVM_dl_close' function.
            /// </summary>
            /// <param name="handle"></param>
            /// <returns></returns>
            [DllImport("ikvm", SetLastError = false)]
            internal static extern void IKVM_dl_close(nint handle);

            /// <summary>
            /// Invokes the native 'IKVM_io_is_file' function.
            /// </summary>
            /// <param name="handle"></param>
            /// <returns></returns>
            [DllImport("ikvm", SetLastError = false)]
            internal static extern long IKVM_io_is_file(long handle);

            /// <summary>
            /// Invokes the native 'IKVM_io_is_socket' function.
            /// </summary>
            /// <param name="handle"></param>
            /// <returns></returns>
            [DllImport("ikvm", SetLastError = false)]
            internal static extern long IKVM_io_is_socket(long handle);

            /// <summary>
            /// Invokes the native 'IKVM_io_duplicate_file' function.
            /// </summary>
            /// <param name="handle"></param>
            /// <returns></returns>
            [DllImport("ikvm", SetLastError = false)]
            internal static extern long IKVM_io_duplicate_file(long handle);

            /// <summary>
            /// Invokes the native 'IKVM_io_duplicate_socket' function.
            /// </summary>
            /// <param name="handle"></param>
            /// <returns></returns>
            [DllImport("ikvm", SetLastError = false)]
            internal static extern long IKVM_io_duplicate_socket(long handle);

            /// <summary>
            /// Invokes the native 'IKVM_io_close_file' function.
            /// </summary>
            /// <param name="handle"></param>
            /// <returns></returns>
            [DllImport("ikvm", SetLastError = false)]
            internal static extern void IKVM_io_close_file(long handle);

            /// <summary>
            /// Invokes the native 'IKVM_io_close_socket' function.
            /// </summary>
            /// <param name="handle"></param>
            /// <returns></returns>
            [DllImport("ikvm", SetLastError = false)]
            internal static extern void IKVM_io_close_socket(long handle);

        }

        /// <summary>
        /// Invokes the native 'LoadLibrary' function.
        /// </summary>
        /// <param name="lpLibFileName"></param>
        /// <returns></returns>
        [DllImport("kernel32", SetLastError = false)]
        static extern nint LoadLibrary(string lpLibFileName);

        /// <summary>
        /// Invokes the native 'FreeLibrary' function.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("kernel32", SetLastError = false)]
        static extern nint FreeLibrary(nint handle);

        /// <summary>
        /// Implements the native OS load functionality for 'libikvm'.
        /// </summary>
        /// <param name="nameOrPath"></param>
        /// <returns></returns>
        static nint LoadImpl(string nameOrPath)
        {
#if NETFRAMEWORK
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return LoadLibrary(nameOrPath);
            }
            else
            {
                // Mono (Framework on non-Windows), just P/Invoke, we can remap with dllmap
                return Externs.IKVM_dl_open(nameOrPath);
            }
#else
            return System.Runtime.InteropServices.NativeLibrary.TryLoad(nameOrPath, out var h) ? h : 0;
#endif
        }

        /// <summary>
        /// Implements the native OS free functionality.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        static void FreeImpl(nint handle)
        {
#if NETFRAMEWORK
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                FreeLibrary(handle);
            }
            else
            {
                // Mono (Framework on non-Windows), just P/Invoke, we can remap with dllmap
                // this should be a secondary reference, as the DllImports should own the primary
                Externs.IKVM_dl_close(handle);
            }
#else
            System.Runtime.InteropServices.NativeLibrary.Free(handle);
#endif
        }

        /// <summary>
        /// Gets the instance of libikvm.
        /// </summary>
        public static readonly LibIkvm Instance = new();

        /// <summary>
        /// Holds an internal handle to the 'ikvm' library.
        /// </summary>
        public nint Handle { get; private set; }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        LibIkvm()
        {
            if ((Handle = Load()) == 0)
                throw new InternalException("Could not load ikvm native library.");
        }

        /// <summary>
        /// Loads the 'ikvm' library.
        /// </summary>
        static nint Load()
        {
            // give native loader a chance to load the library
            if (LoadImpl("ikvm") is nint h1 and not 0)
                return h1;

            // start looking at assembly path, or path given by environmental variable
            var asml = typeof(NativeLibrary).Assembly.Location is string s ? Path.GetDirectoryName(s) : null;
            var root = Environment.GetEnvironmentVariable("IKVM_LIBRARY_PATH") ?? asml;

            // assembly possible loaded in memory: we have no available search path
            if (string.IsNullOrEmpty(root))
                return 0;

            // check in root directory
            if (Path.Combine(root, NativeLibrary.MapLibraryName("ikvm")) is string lib1)
                if (File.Exists(lib1) && LoadImpl(lib1) is nint h2 and not 0)
                    return h2;

            // scan supported RIDs for current platform
            foreach (var rid in RuntimeUtil.SupportedRuntimeIdentifiers)
                if (Path.Combine(root, "runtimes", rid, "native", NativeLibrary.MapLibraryName("ikvm")) is string lib2)
                    if (File.Exists(lib2) && LoadImpl(lib2) is nint h3 and not 0)
                        return h3;

            return 0;
        }

        /// <summary>
        /// Invokes the 'dl_open' function.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public nint dl_open(string path) => Externs.IKVM_dl_open(path);

        /// <summary>
        /// Invokes the 'dl_sym' function.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public nint dl_sym(nint handle, string symbol) => Externs.IKVM_dl_sym(handle, symbol);

        /// <summary>
        /// Invokes the 'dl_close' function.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public void dl_close(nint handle) => Externs.IKVM_dl_close(handle);

        /// <summary>
        /// Invokes the 'io_is_file' function.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public bool io_is_file(long handle) => Externs.IKVM_io_is_file(handle) != 0;

        /// <summary>
        /// Invokes the 'io_is_socket' function.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public bool io_is_socket(long handle) => Externs.IKVM_io_is_socket(handle) != 0;

        /// <summary>
        /// Invokes the 'io_close_file' function.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public void io_close_file(long handle) => Externs.IKVM_io_close_file(handle);

        /// <summary>
        /// Invokes the 'io_close_socket' function.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public void io_close_socket(long handle) => Externs.IKVM_io_close_socket(handle);

        /// <summary>
        /// Invokes the 'io_duplicate_file' function.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public long io_duplicate_file(long handle) => Externs.IKVM_io_duplicate_file(handle);

        /// <summary>
        /// Invokes the 'io_duplicate_socket' function.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public long io_duplicate_socket(long handle) => Externs.IKVM_io_duplicate_socket(handle);

        /// <summary>
        /// Releases the instance.
        /// </summary>
        ~LibIkvm()
        {
            if (Handle != 0)
            {
                var h = Handle;
                Handle = 0;
                FreeImpl(h);
            }
        }

    }

#endif

}
