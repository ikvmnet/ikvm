/*
  Copyright (C) 2007-2014 Jeroen Frijters

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
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

using IKVM.Internal;
using IKVM.Runtime.JNI;

namespace IKVM.Java.Externs.java.lang
{

    static partial class ClassLoader
    {

        /// <summary>
        /// Finds the builtin native library, or returns <c>null</c> if the given name isn't built in.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string findBuiltinLib(string name)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (name == null)
                throw new global::java.lang.InternalError("NULL filename for native library.");

            var l = GetUnmappedLibraryName(name);
            return IsBuiltinLib(l) ? l : null;
#endif
        }

        /// <summary>
        /// Returns <c>true</c> if the given short library name is a builtin library.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static bool IsBuiltinLib(string name)
        {
            switch (name)
            {
                case "net":
                case "nio":
                case "jaas_nt":
                case "awt":
                case "splashscreen":
                case "jit":
                case "w2k_lsa_auth":
                case "osxkrb5":
                case "osx":
                case "management":
                case "zip":
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Takes a mapped library name and returns the unmapped verison.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static string GetUnmappedLibraryName(string name)
        {
            if (name == null)
                return null;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return Path.GetFileNameWithoutExtension(name);
            }
            else
            {
                if (name.StartsWith("lib"))
                    return Path.GetFileNameWithoutExtension(name).Substring(3);
                else
                    return Path.GetFileNameWithoutExtension(name);
            }
        }

        internal static class NativeLibrary
        {

            /// <summary>
            /// Implements the backing for the native 'load' method.
            /// </summary>
            /// <param name="self"></param>
            /// <param name="name"></param>
            /// <param name="isBuiltin"></param>
            public static void load(object self, string name, bool isBuiltin)
            {
#if FIRST_PASS
                throw new NotImplementedException();
#else
                var lib = (global::java.lang.ClassLoader.NativeLibrary)self;
                lib.handle = isBuiltin ? 0 : JNINativeLoader.LoadLibrary(name, TypeWrapper.FromClass(global::java.lang.ClassLoader.NativeLibrary.getFromClass()).GetClassLoader());
                lib.loaded = true;
#endif
            }

            /// <summary>
            /// Implements the backing for the native 'find' method.
            /// </summary>
            /// <param name="self"></param>
            /// <param name="name"></param>
            /// <returns></returns>
            /// <exception cref="NotImplementedException"></exception>
            public static long find(object self, string name)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Implements the backing for the native 'unload' method.
            /// </summary>
            /// <param name="thisNativeLibrary"></param>
            /// <param name="name"></param>
            /// <param name="isBuiltin"></param>
            [SecuritySafeCritical]
            public static void unload(object thisNativeLibrary, string name, bool isBuiltin)
            {
#if FIRST_PASS
                throw new NotImplementedException();
#else
                if (isBuiltin == false)
                {
                    var lib = (global::java.lang.ClassLoader.NativeLibrary)thisNativeLibrary;
                    var handle = Interlocked.Exchange(ref lib.handle, 0);
                    if (handle != 0)
                        JNINativeLoader.UnloadLibrary(handle, TypeWrapper.FromClass(global::java.lang.ClassLoader.NativeLibrary.getFromClass()).GetClassLoader());
                }
#endif
            }

        }

    }

}
