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
using System.Collections.Generic;
using System.Runtime.InteropServices;

using IKVM.Internal;

namespace IKVM.Runtime.JNI
{

    using jint = System.Int32;

    /// <summary>
    /// Manages the set of loaded native JNI libraries.
    /// </summary>
    static unsafe class JNINativeLoader
    {

        delegate jint JNI_OnLoadFunc(JavaVM* vm, void* reserved);
        delegate void JNI_OnUnloadFunc(JavaVM* vm, void* reserved);

        public static readonly object SyncRoot = new object();
        static readonly List<nint> loaded = new List<nint>();

        /// <summary>
        /// Initiates a load of the given JNI library by the specified class loader.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="loader"></param>
        /// <returns></returns>
        public static long LoadLibrary(string filename, ClassLoaderWrapper loader)
        {
            Tracer.Info(Tracer.Jni, "loadLibrary: {0}, class loader: {1}", filename, loader);

            lock (SyncRoot)
            {
                nint p = 0;

                try
                {
                    // attempt to load the native library
                    if ((p = NativeLibrary.Load(filename)) == 0)
                    {
                        Tracer.Info(Tracer.Jni, "Failed to load library: path = '{0}', message = {2}", filename, "NULL handle returned.");
                        return 0;
                    }

                    // find whether the native library was already loaded
                    foreach (var h in loader.GetNativeLibraries())
                    {
                        if (h == p)
                        {
                            NativeLibrary.Free(p);
                            Tracer.Warning(Tracer.Jni, "Library was already loaded, returning same reference.", filename);
                            return p;
                        }
                    }

                    // library was loaded by another classloader, that's a link error
                    if (loaded.Contains(p))
                    {
                        var msg = $"Native library '{filename}' already loaded in another classloader.";
                        Tracer.Error(Tracer.Jni, "UnsatisfiedLinkError: {0}", msg);
                        throw new java.lang.UnsatisfiedLinkError(msg);
                    }

                    Tracer.Info(Tracer.Jni, "Library loaded: {0}, handle = 0x{1:X}", filename, p);

                    try
                    {
                        var onload = NativeLibrary.GetExport(p, "JNI_OnLoad", sizeof(nint) * 2);
                        if (onload != 0)
                        {
                            Tracer.Info(Tracer.Jni, "Calling JNI_OnLoad on: {0}", filename);
                            var f = new JNIFrame();
                            int v;
                            var w = f.Enter(loader);
                            try
                            {
                                v = Marshal.GetDelegateForFunctionPointer<JNI_OnLoadFunc>(onload)(JavaVM.pJavaVM, null);
                                Tracer.Info(Tracer.Jni, "JNI_OnLoad returned: 0x{0:X8}", v);
                            }
                            finally
                            {
                                f.Leave(w);
                            }

                            if (JNIVM.IsSupportedJniVersion(v) == false)
                            {
                                var msg = $"Unsupported JNI version 0x{v:X} required by {filename}";
                                Tracer.Error(Tracer.Jni, "UnsatisfiedLinkError: {0}", msg);
                                throw new java.lang.UnsatisfiedLinkError(msg);
                            }
                        }
                    }
                    catch (EntryPointNotFoundException)
                    {
                        // JNI_OnLoad entry point doesn't exist and isn't required
                    }

                    // record addition of native library
                    loaded.Add(p);
                    loader.RegisterNativeLibrary(p);
                    return p;
                }
                catch (DllNotFoundException e)
                {
                    Tracer.Info(Tracer.Jni, "Failed to load library: path = '{0}', error = {1}, message = {2}", filename, "DllNotFoundException", e.Message);
                    return 0;
                }
                catch (Exception e)
                {
                    Tracer.Info(Tracer.Jni, "Failed to load library: path = '{0}', error = {1}, message = {2}", filename, "Exception", e.Message);
                    NativeLibrary.Free(p);
                    throw;
                }
            }
        }

        /// <summary>
        /// Initiates an unload of the given native library for the specified class loader.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="loader"></param>
        public static void UnloadLibrary(long handle, ClassLoaderWrapper loader)
        {
            var p = (nint)handle;

            lock (SyncRoot)
            {
                Tracer.Info(Tracer.Jni, "Unloading library: handle = 0x{0:X}, class loader = {1}", handle, loader);

                try
                {
                    var onunload = NativeLibrary.GetExport(p, "JNI_OnUnload", sizeof(nint) * 2);
                    if (onunload != 0)
                    {
                        Tracer.Info(Tracer.Jni, "Calling JNI_OnUnload on: handle = 0x{0:X}", handle);

                        var f = new JNIFrame();
                        var w = f.Enter(loader);
                        try
                        {
                            Marshal.GetDelegateForFunctionPointer<JNI_OnUnloadFunc>(onunload)(JavaVM.pJavaVM, null);
                        }
                        finally
                        {
                            f.Leave(w);
                        }
                    }
                }
                catch (EntryPointNotFoundException)
                {
                    // JNI_OnUnload entry point doesn't exist and isn't required
                }

                // remove record of native library
                loaded.Remove(p);
                loader.UnregisterNativeLibrary(p);
                NativeLibrary.Free(p);
            }
        }

    }

}
