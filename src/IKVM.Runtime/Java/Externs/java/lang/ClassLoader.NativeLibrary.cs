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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;

using IKVM.Internal;

namespace IKVM.Java.Externs.java.lang
{

    static partial class ClassLoader
    {

        internal static class NativeLibrary
        {

            public static void load(object self, string name, bool isBuiltin)
            {
#if FIRST_PASS
                throw new NotImplementedException();
#else
                LoadImpl(self, name, isBuiltin);
#endif
            }

            /// <summary>
            /// </summary>
            /// <remarks>
            /// Method avoids inlining to ensure IKVM.Runtime.JNI does not get loaded.
            /// </remarks>
            /// <param name="self"></param>
            /// <param name="name"></param>
            /// <param name="isBuiltin"></param>
            [MethodImpl(MethodImplOptions.NoInlining)]
            [SecuritySafeCritical]
            static void LoadImpl(object self, string name, bool isBuiltin)
            {
#if FIRST_PASS
                throw new NotImplementedException();
#else
                var lib = (global::java.lang.ClassLoader.NativeLibrary)self;
                lib.handle = isBuiltin ? 0 : IKVM.Runtime.JniHelper.LoadLibrary(name, TypeWrapper.FromClass(global::java.lang.ClassLoader.NativeLibrary.getFromClass()).GetClassLoader());
                lib.loaded = true;
#endif
            }

            public static long find(object self, string name)
            {
                throw new NotImplementedException();
            }

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
                        IKVM.Runtime.JniHelper.UnloadLibrary(handle, TypeWrapper.FromClass(global::java.lang.ClassLoader.NativeLibrary.getFromClass()).GetClassLoader());
                }
#endif
            }

            /// <summary>
            /// Finds the builtin native library, or returns <c>null</c> if the given name isn't built in.
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            public static string findBuiltinLib(string name) => GetUnmappedLibraryName(name) switch
            {
                "net" => "net",
                _ => null
            };

            /// <summary>
            /// Takes a mapped library name and returns the unmapped verison.
            /// </summary>
            /// <param name="name"></param>
            /// <returns></returns>
            static object GetUnmappedLibraryName(string name)
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

        }

    }

}
