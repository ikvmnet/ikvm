/*
  Copyright (C) 2002-2014 Jeroen Frijters

  This software is provided 'as-is', without any express or implied
  warranty.  In no event will the authors be held liable for any damages
  arising from the use of this software.

  Permission is granted to anyone to use this software for any purpose,
  including commercial applications, and to alter it and redistribute it
  freely, subject to the following restrictions:

  1. The origin of this software must not be misrepresented; you must not
     claim that you wrote the original software. If you use this software
     in a product, an acknowledgment in the product documentation would be
     appreciated but is not required.
  2. Altered source versions must be plainly marked as such, and must not be
     misrepresented as being the original software.
  3. This notice may not be removed or altered from any source distribution.

  Jeroen Frijters
  jeroen@frijters.net
  
*/
using System;
using System.Runtime.InteropServices;

namespace IKVM.Runtime
{

    /// <summary>
    /// Provides methods to load a library.
    /// </summary>
    static class JniNativeLibrary
    {

#if NETFRAMEWORK

        /// <summary>
        /// Invokes the Windows LoadLibrary function.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "LoadLibrary", SetLastError = true)]
        static extern IntPtr LoadLibrary(string path);

        /// <summary>
        /// Invokes the Windows FreeLibrary function.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "FreeLibrary", SetLastError = true)]
        static extern IntPtr FreeLibrary(IntPtr handle);

        /// <summary>
        /// Invokes the Windows GetProcAddress function.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll", EntryPoint = "GetProcAddress", SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr handle, string name);

        /// <summary>
        /// Invokes the Windows GetProcAddress function, handling 32-bit mangled names.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="name"></param>
        /// <param name="argl"></param>
        /// <returns></returns>
        static IntPtr GetProcAddress32(IntPtr handle, string name, int argl)
        {
            var h = GetWin32ExportName(name, argl) is string n ? GetProcAddress(handle, n) : IntPtr.Zero;
            if (h == IntPtr.Zero)
                h = GetProcAddress(handle, name);

            return h;
        }

#endif

        /// <summary>
        /// Invokes the Windows GetProcAddress function, handling 32-bit mangled names.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="argl"></param>
        /// <returns></returns>
        static string GetWin32ExportName(string name, int argl)
        {
            // long paths not supported on Win32
            return name.Length <= 512 - 11 ? "_" + name + "@" + argl : null;
        }

        /// <summary>
        /// Loads the given library in a platform dependent manner.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public static IntPtr Load(string path)
        {
#if NETFRAMEWORK
            return LoadLibrary(path);
#else
            return NativeLibrary.Load(path);
#endif
        }

        /// <summary>
        /// Frees the given library in a platform dependent manner.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public static void Free(IntPtr handle)
        {
#if NETFRAMEWORK
            FreeLibrary(handle);
#else
            NativeLibrary.Free(handle);
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
        public static IntPtr GetExport(IntPtr handle, string name, int argl)
        {
            try
            {
                var h = IntPtr.Zero;

#if NETFRAMEWORK
                h = IntPtr.Size == 4 ? GetProcAddress32(handle, name, argl) : GetProcAddress(handle, name);
#else
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    h = IntPtr.Size == 4 && GetWin32ExportName(name, argl) is string n ? NativeLibrary.GetExport(handle, n) : IntPtr.Zero;

                if (h == IntPtr.Zero)
                    h = NativeLibrary.GetExport(handle, name);
#endif

                return h;
            }
            catch (EntryPointNotFoundException)
            {
                return IntPtr.Zero;
            }
        }

    }

}
