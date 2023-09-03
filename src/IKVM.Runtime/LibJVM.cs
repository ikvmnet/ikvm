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

using System.IO;
using System.Runtime.InteropServices;

using jsize = System.Int32;

namespace IKVM.Runtime.JNI
{

#if FIRST_PASS == false && IMPORTER == false && EXPORTER == false

    /// <summary>
    /// Required native methods available in libjvm.
    /// </summary>
    internal unsafe class LibJVM
    {

        /// <summary>
        /// Holds the extern methods to prevent early loading.
        /// </summary>
        static class Externs
        {

            [DllImport("jvm", SetLastError = false)]
            public static extern void Set_JNI_GetDefaultJavaVMInitArgs(JNI_GetDefaultJavaVMInitArgsDelegate func);

            [DllImport("jvm", SetLastError = false)]
            public static extern void Set_JNI_GetCreatedJavaVMs(JNI_GetCreatedJavaVMsDelegate func);

            [DllImport("jvm", SetLastError = false)]
            public static extern void Set_JNI_CreateJavaVM(JNI_CreateJavaVMDelegate func);

            [DllImport("jvm", SetLastError = false)]
            public static extern nint JVM_LoadLibrary(string name);

            [DllImport("jvm", SetLastError = false)]
            public static extern nint JVM_UnloadLibrary(nint handle);

            [DllImport("jvm", SetLastError = false)]
            public static extern nint JVM_FindLibraryEntry(nint handle, string name);

        }

        /// <summary>
        /// Gets the default instance.
        /// </summary>
        public static LibJVM Instance = new LibJVM();

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        LibJVM()
        {

        }

        /// <summary>
        /// Gets a handle to the loaded libjvm library.
        /// </summary>
        public nint Handle { get; private set; } = NativeLibrary.Load(Path.Combine(JVM.Properties.HomePath, "bin", NativeLibrary.MapLibraryName("jvm")));

        public delegate int JNI_GetDefaultJavaVMInitArgsDelegate(void* vm_args);
        public delegate int JNI_GetCreatedJavaVMsDelegate(JavaVM** vmBuf, jsize bufLen, jsize* nVMs);
        public delegate int JNI_CreateJavaVMDelegate(JavaVM** p_vm, void** p_env, void* vm_args);

        /// <summary>
        /// Invokes the 'Set_JNI_GetDefaultJavaVMInitArgs' method from libjvm.
        /// </summary>
        /// <param name="func"></param>
        public void Set_JNI_GetDefaultJavaVMInitArgs(JNI_GetDefaultJavaVMInitArgsDelegate func) => Externs.Set_JNI_GetDefaultJavaVMInitArgs(func);

        /// <summary>
        /// Invokes the 'Set_JNI_GetCreatedJavaVMs' method from libjvm.
        /// </summary>
        /// <param name="func"></param>
        public void Set_JNI_GetCreatedJavaVMs(JNI_GetCreatedJavaVMsDelegate func) => Externs.Set_JNI_GetCreatedJavaVMs(func);

        /// <summary>
        /// Invokes the 'Set_JNI_CreateJavaVM' method from libjvm.
        /// </summary>
        /// <param name="func"></param>
        public void Set_JNI_CreateJavaVM(JNI_CreateJavaVMDelegate func) => Externs.Set_JNI_CreateJavaVM(func);

        /// <summary>
        /// Invokes the 'JVM_LoadLibrary' method from libjvm.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public nint JVM_LoadLibrary(string name) => Externs.JVM_LoadLibrary(name);

        /// <summary>
        /// Invokes the 'JVM_UnloadLibrary' method from libjvm.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public nint JVM_UnloadLibrary(nint handle) => Externs.JVM_UnloadLibrary(handle);

        /// <summary>
        /// Invokes the 'JVM_FindLibraryEntry' method from libjvm.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public nint JVM_FindLibraryEntry(nint handle, string name) => Externs.JVM_FindLibraryEntry(handle, name);

        /// <summary>
        /// Finalizes the instance.
        /// </summary>
        ~LibJVM()
        {
            if (Handle != 0)
            {
                NativeLibrary.Free(Handle);
                Handle = 0;
            }
        }

    }

#endif

}