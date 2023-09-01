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
        [DllImport("ikvm", SetLastError = false)]
        static extern nint IKVM_dlopen(string path);

        /// <summary>
        /// Invokes the native 'dlsym' function.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="symbol"></param>
        /// <returns></returns>
        [DllImport("ikvm", SetLastError = false)]
        static extern nint IKVM_dlsym(nint handle, string symbol);

        /// <summary>
        /// Invokes the native 'dlclose' function.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("ikvm", SetLastError = false)]
        static extern void IKVM_dlclose(nint handle);

        /// <summary>
        /// Loads the given library in a platform dependent manner.
        /// </summary>
        /// <param name="nameOrPath"></param>
        /// <returns></returns>
        public static nint Load(string nameOrPath)
        {
            return IKVM_dlopen(nameOrPath);
        }

        /// <summary>
        /// Frees the given library in a platform dependent manner.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public static void Free(nint handle)
        {
            IKVM_dlclose(handle);
        }

        /// <summary>
        /// Gets a function pointer to the given named function.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static nint GetExport(nint handle, string name)
        {
            return IKVM_dlsym(handle, name);
        }

    }

}
