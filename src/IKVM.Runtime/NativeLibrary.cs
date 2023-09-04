using System;
using System.IO;
using System.Runtime.InteropServices;

namespace IKVM.Runtime
{

#if FIRST_PASS == false && IMPORTER == FALSE && EXPORTER == false

    /// <summary>
    /// Provides methods to load libraries from standard .NET search locations. This class is not used to load native libraries for JNI.
    /// On platforms that have native NativeLibrary support, that support is used. On other platforms, libikvm is used to invoke the
    /// platform loader.
    /// </summary>
    static partial class NativeLibrary
    {

        readonly static LibIkvm libikvm = LibIkvm.Instance;

        /// <summary>
        /// Returns a version of the library name with the OS specific prefix and suffix.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string MapLibraryName(string name)
        {
            if (name == null)
                return null;
            if (Path.HasExtension(name))
                return name;

            if (RuntimeUtil.IsWindows)
                return name + ".dll";
            else if (RuntimeUtil.IsOSX)
                return "lib" + name + ".dylib";
            else
                return "lib" + name + ".so";
        }

        /// <summary>
        /// Invokes the appropriate method to simulate dlopen.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        static nint LoadImpl(string path)
        {
#if NETFRAMEWORK
            return libikvm.dl_open(path);
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
                return LoadImpl(nameOrPath);

            // give native loader a chance to load the library
            if (LoadImpl(nameOrPath) is nint h1 and not 0)
                return h1;

            // start looking at assembly path, or path given by environmental variable
            var asml = typeof(NativeLibrary).Assembly.Location is string s ? Path.GetDirectoryName(s) : null;
            var root = Environment.GetEnvironmentVariable("IKVM_LIBRARY_PATH") ?? asml;

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
        /// Frees the given library in a platform dependent manner.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static void Free(nint handle)
        {
#if NETFRAMEWORK
            libikvm.dl_close(handle);
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
            return libikvm.dl_sym(handle, name);
#else
            return System.Runtime.InteropServices.NativeLibrary.TryGetExport(handle, name, out var h) ? h : 0;
#endif
        }

    }

#endif

}
