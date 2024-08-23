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

using IKVM.CoreLib.Diagnostics;

namespace IKVM.Runtime.JNI
{

#if FIRST_PASS == false && IMPORTER == false && EXPORTER == false


    /// Manages the set of loaded native JNI libraries.
    /// </summary>
    static unsafe class JNINativeLoader
    {

        delegate int JNI_OnLoadFunc(JavaVM* vm, void* reserved);
        delegate void JNI_OnUnloadFunc(JavaVM* vm, void* reserved);

        public static readonly object SyncRoot = new object();
        static readonly List<nint> loaded = new();

        /// <summary>
        /// Initiates a load of the given JNI library by the specified class loader.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="fromClass"></param>
        /// <returns></returns>
        public static long LoadLibrary(string filename, RuntimeJavaType fromClass)
        {
            JVM.Context.ReportEvent(Diagnostic.GenericJniInfo.Event([$"loadLibrary: '{filename}' fromClass '{fromClass}'"]));

            lock (SyncRoot)
            {
                nint p = 0;

                try
                {
                    // attempt to load the native library
                    if ((p = LibJvm.Instance.JVM_LoadLibrary(filename)) == 0)
                    {
                        JVM.Context.ReportEvent(Diagnostic.GenericJniInfo.Event([$"Failed to load library: path = '{filename}', message = {"NULL handle returned."}"]));
                        return 0;
                    }

                    // find whether the native library was already loaded
                    foreach (var h in fromClass.GetClassLoader().GetNativeLibraries())
                    {
                        if (h == p)
                        {
                            LibJvm.Instance.JVM_UnloadLibrary(p);
                            JVM.Context.ReportEvent(Diagnostic.GenericJniWarning.Event(["Library was already loaded, returning same reference"]));
                            return p;
                        }
                    }

                    // library was loaded by another classloader, that's a link error
                    if (loaded.Contains(p))
                    {
                        var msg = $"Native library '{filename}' already loaded in another classloader.";
                        JVM.Context.ReportEvent(Diagnostic.GenericJniError.Event([$"UnsatisfiedLinkError: {msg}"]));
                        throw new java.lang.UnsatisfiedLinkError(msg);
                    }

                    JVM.Context.ReportEvent(Diagnostic.GenericJniInfo.Event([$"Library loaded: {filename}, handle = 0x{p:X}"]));

                    try
                    {
                        var onload = LibJvm.Instance.JVM_FindLibraryEntry(p, NativeLibrary.MangleExportName("JNI_OnLoad", sizeof(nint) + sizeof(nint)));
                        if (onload != 0)
                        {
                            JVM.Context.ReportEvent(Diagnostic.GenericJniInfo.Event([$"Calling JNI_OnLoad on: {filename}"]));
                            var f = new JNIFrame();
                            int v;
                            var w = f.Enter(ikvm.@internal.CallerID.create(fromClass.ClassObject, fromClass.ClassObject.getClassLoader()));
                            try
                            {
                                v = Marshal.GetDelegateForFunctionPointer<JNI_OnLoadFunc>(onload)(JavaVM.pJavaVM, null);
                                JVM.Context.ReportEvent(Diagnostic.GenericJniInfo.Event([$"JNI_OnLoad returned: 0x{v:X8}"]));
                            }
                            finally
                            {
                                f.Leave();
                            }

                            if (JNIVM.IsSupportedJNIVersion(v) == false)
                            {
                                var msg = $"Unsupported JNI version 0x{v:X} required by {filename}";
                                JVM.Context.ReportEvent(Diagnostic.GenericJniError.Event([$"UnsatisfiedLinkError: {msg}"]));
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
                    fromClass.GetClassLoader().RegisterNativeLibrary(p);
                    return p;
                }
                catch (DllNotFoundException e)
                {
                    JVM.Context.ReportEvent(Diagnostic.GenericJniInfo.Event([$"Failed to load library: path = '{filename}', error = DllNotFoundException, message = {e.Message}"]));
                    return 0;
                }
                catch (Exception e)
                {
                    JVM.Context.ReportEvent(Diagnostic.GenericJniInfo.Event([$"Failed to load library: path = '{filename}', error = Exception, message = {e.Message}"]));
                    LibJvm.Instance.JVM_UnloadLibrary(p);
                    throw;
                }
            }
        }

        /// <summary>
        /// Initiates an unload of the given native library for the specified class loader.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="loader"></param>
        public static void UnloadLibrary(long handle, RuntimeJavaType fromClass)
        {
            var p = (nint)handle;

            lock (SyncRoot)
            {
                JVM.Context.ReportEvent(Diagnostic.GenericJniInfo.Event([$"Unloading library: handle = 0x{handle:X}, class = {fromClass}"]));

                try
                {
                    var onunload = LibJvm.Instance.JVM_FindLibraryEntry(p, NativeLibrary.MangleExportName("JNI_OnUnload", sizeof(nint) + sizeof(nint)));
                    if (onunload != 0)
                    {
                        JVM.Context.ReportEvent(Diagnostic.GenericJniInfo.Event([$"Calling JNI_OnUnload on: handle = 0x{handle:X}"]));

                        var f = new JNIFrame();
                        var w = f.Enter(ikvm.@internal.CallerID.create(fromClass.ClassObject, fromClass.ClassObject.getClassLoader()));
                        try
                        {
                            Marshal.GetDelegateForFunctionPointer<JNI_OnUnloadFunc>(onunload)(JavaVM.pJavaVM, null);
                        }
                        finally
                        {
                            f.Leave();
                        }
                    }
                }
                catch (EntryPointNotFoundException)
                {
                    // JNI_OnUnload entry point doesn't exist and isn't required
                }

                // remove record of native library
                loaded.Remove(p);
                fromClass.GetClassLoader().UnregisterNativeLibrary(p);
                LibJvm.Instance.JVM_UnloadLibrary(p);
            }
        }

    }

#endif

}
