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
    sealed class JniHelper
    {
        private static List<IntPtr> nativeLibraries = new List<IntPtr>();
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

        private unsafe static long LoadLibrary(string filename, ClassLoaderWrapper loader)
        {
            Tracer.Info(Tracer.Jni, "loadLibrary: {0}, class loader: {1}", filename, loader);
            lock (JniLock)
            {
                IntPtr p = Native.ikvm_LoadLibrary(filename);
                if (p == IntPtr.Zero)
                {
                    Tracer.Info(Tracer.Jni, "Failed to load library: path = '{0}', error = {1}, message = {2}", filename,
                        Marshal.GetLastWin32Error(), new System.ComponentModel.Win32Exception().Message);
                    return 0;
                }
                try
                {
                    foreach (IntPtr tmp in loader.GetNativeLibraries())
                    {
                        if (tmp == p)
                        {
                            // the library was already loaded by the current class loader,
                            // no need to do anything
                            Native.ikvm_FreeLibrary(p);
                            Tracer.Warning(Tracer.Jni, "Library was already loaded: {0}", filename);
                            return p.ToInt64();
                        }
                    }
                    if (nativeLibraries.Contains(p))
                    {
                        string msg = string.Format("Native library {0} already loaded in another classloader", filename);
                        Tracer.Error(Tracer.Jni, "UnsatisfiedLinkError: {0}", msg);
                        throw new java.lang.UnsatisfiedLinkError(msg);
                    }
                    Tracer.Info(Tracer.Jni, "Library loaded: {0}, handle = 0x{1:X}", filename, p.ToInt64());
                    IntPtr onload = Native.ikvm_GetProcAddress(p, "JNI_OnLoad", IntPtr.Size * 2);
                    if (onload != IntPtr.Zero)
                    {
                        Tracer.Info(Tracer.Jni, "Calling JNI_OnLoad on: {0}", filename);
                        JNI.Frame f = new JNI.Frame();
                        int version;
                        ClassLoaderWrapper prevLoader = f.Enter(loader);
                        try
                        {
                            // TODO on Whidbey we should be able to use Marshal.GetDelegateForFunctionPointer to call OnLoad
                            version = Native.ikvm_CallOnLoad(onload, JavaVM.pJavaVM, null);
                            Tracer.Info(Tracer.Jni, "JNI_OnLoad returned: 0x{0:X8}", version);
                        }
                        finally
                        {
                            f.Leave(prevLoader);
                        }
                        if (!JNI.IsSupportedJniVersion(version))
                        {
                            string msg = string.Format("Unsupported JNI version 0x{0:X} required by {1}", version, filename);
                            Tracer.Error(Tracer.Jni, "UnsatisfiedLinkError: {0}", msg);
                            throw new java.lang.UnsatisfiedLinkError(msg);
                        }
                    }
                    nativeLibraries.Add(p);
                    loader.RegisterNativeLibrary(p);
                    return p.ToInt64();
                }
                catch
                {
                    Native.ikvm_FreeLibrary(p);
                    throw;
                }
            }
        }

        private unsafe static void UnloadLibrary(long handle, ClassLoaderWrapper loader)
        {
            lock (JniLock)
            {
                Tracer.Info(Tracer.Jni, "Unloading library: handle = 0x{0:X}, class loader = {1}", handle, loader);
                IntPtr p = (IntPtr)handle;
                IntPtr onunload = Native.ikvm_GetProcAddress(p, "JNI_OnUnload", IntPtr.Size * 2);
                if (onunload != IntPtr.Zero)
                {
                    Tracer.Info(Tracer.Jni, "Calling JNI_OnUnload on: handle = 0x{0:X}", handle);
                    JNI.Frame f = new JNI.Frame();
                    ClassLoaderWrapper prevLoader = f.Enter(loader);
                    try
                    {
                        // TODO on Whidbey we should be able to use Marshal.GetDelegateForFunctionPointer to call OnLoad
                        Native.ikvm_CallOnLoad(onunload, JavaVM.pJavaVM, null);
                    }
                    finally
                    {
                        f.Leave(prevLoader);
                    }
                }
                nativeLibraries.Remove(p);
                loader.UnregisterNativeLibrary(p);
                Native.ikvm_FreeLibrary((IntPtr)handle);
            }
        }
    }
}
