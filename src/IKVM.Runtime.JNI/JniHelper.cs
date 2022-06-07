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

namespace IKVM.Runtime
{

    sealed partial class JniHelper
    {

        static List<IntPtr> nativeLibraries = new List<IntPtr>();
        internal static readonly object JniLock = new object();

        // MONOBUG with mcs we can't pass ClassLoaderWrapper from IKVM.Runtime.dll to IKVM.Runtime.JNI.dll
        internal unsafe static long LoadLibrary(string filename, object loader)
        {
            return LoadLibrary(filename, (ClassLoaderWrapper)loader);
        }

        // MONOBUG with mcs we can't pass ClassLoaderWrapper from IKVM.Runtime.dll to IKVM.Runtime.JNI.dll
        internal static void UnloadLibrary(long handle, object loader)
        {
            UnloadLibrary(handle, (ClassLoaderWrapper)loader);
        }

        unsafe static long LoadLibrary(string filename, ClassLoaderWrapper loader)
        {
            Tracer.Info(Tracer.Jni, "loadLibrary: {0}, class loader: {1}", filename, loader);

            lock (JniLock)
            {
                var p = IntPtr.Zero;

                try
                {
                    // attempt to load the native library
                    p = JniNativeLibrary.Load(filename);
                    if (p == IntPtr.Zero)
                    {
                        Tracer.Info(Tracer.Jni, "Failed to load library: path = '{0}', message = {2}", filename, "NULL handle returned.");
                        return 0;
                    }

                    // find whether the native library was already loaded
                    foreach (var tmp in loader.GetNativeLibraries())
                    {
                        if (tmp == p)
                        {
                            JniNativeLibrary.Free(p);
                            Tracer.Warning(Tracer.Jni, "Library was already loaded, returning same reference.", filename);
                            return p.ToInt64();
                        }
                    }

                    // library was loaded by another classloader, that's a link error
                    if (nativeLibraries.Contains(p))
                    {
                        var msg = $"Native library '{filename}' already loaded in another classloader.";
                        Tracer.Error(Tracer.Jni, "UnsatisfiedLinkError: {0}", msg);
                        throw new java.lang.UnsatisfiedLinkError(msg);
                    }

                    Tracer.Info(Tracer.Jni, "Library loaded: {0}, handle = 0x{1:X}", filename, p.ToInt64());

                    try
                    {
                        var onload = JniNativeLibrary.GetExport(p, "JNI_OnLoad", IntPtr.Size * 2);
                        if (onload != IntPtr.Zero)
                        {
                            Tracer.Info(Tracer.Jni, "Calling JNI_OnLoad on: {0}", filename);
                            var f = new JNI.Frame();
                            int v;
                            var w = f.Enter(loader);
                            try
                            {
                                v = Marshal.GetDelegateForFunctionPointer<Func<IntPtr, IntPtr, int>>(onload)((IntPtr)JavaVM.pJavaVM, IntPtr.Zero);
                                Tracer.Info(Tracer.Jni, "JNI_OnLoad returned: 0x{0:X8}", v);
                            }
                            finally
                            {
                                f.Leave(w);
                            }

                            if (JNI.IsSupportedJniVersion(v) == false)
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
                    nativeLibraries.Add(p);
                    loader.RegisterNativeLibrary(p);
                    return p.ToInt64();
                }
                catch (DllNotFoundException e)
                {
                    Tracer.Info(Tracer.Jni, "Failed to load library: path = '{0}', error = {1}, message = {2}", filename, "DllNotFoundException", e.Message);
                    return 0;
                }
                catch (Exception e)
                {
                    Tracer.Info(Tracer.Jni, "Failed to load library: path = '{0}', error = {1}, message = {2}", filename, "Exception", e.Message);
                    JniNativeLibrary.Free(p);
                    throw;
                }
            }
        }

        private unsafe static void UnloadLibrary(long handle, ClassLoaderWrapper loader)
        {
            var p = (IntPtr)handle;

            lock (JniLock)
            {
                Tracer.Info(Tracer.Jni, "Unloading library: handle = 0x{0:X}, class loader = {1}", handle, loader);

                try
                {
                    var onunload = JniNativeLibrary.GetExport(p, "JNI_OnUnload", IntPtr.Size * 2);
                    if (onunload != IntPtr.Zero)
                    {
                        Tracer.Info(Tracer.Jni, "Calling JNI_OnUnload on: handle = 0x{0:X}", handle);

                        var f = new JNI.Frame();
                        var w = f.Enter(loader);
                        try
                        {
                            Marshal.GetDelegateForFunctionPointer<Func<IntPtr, IntPtr, int>>(onunload)((IntPtr)JavaVM.pJavaVM, IntPtr.Zero);
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
                nativeLibraries.Remove(p);
                loader.UnregisterNativeLibrary(p);
                JniNativeLibrary.Free((IntPtr)handle);
            }
        }

    }

}
