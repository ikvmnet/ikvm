using System.IO;
using System.Runtime.InteropServices;

using IKVM.Runtime.JNI;

using jsize = System.Int32;

namespace IKVM.Runtime
{

#if FIRST_PASS == false && IMPORTER == false && EXPORTER == false

    /// <summary>
    /// Required native methods available in libjvm.
    /// </summary>
    internal unsafe class LibJVM
    {

        delegate void Set_JNI_GetDefaultJavaVMInitArgsDelegate(JNI_GetDefaultJavaVMInitArgsDelegate func);
        delegate void Set_JNI_GetCreatedJavaVMsDelegate(JNI_GetCreatedJavaVMsDelegate func);
        delegate void Set_JNI_CreateJavaVMDelegate(JNI_CreateJavaVMDelegate func);
        delegate nint JVM_LoadLibraryDelegate(string name);
        delegate nint JVM_UnloadLibraryDelegate(nint handle);
        delegate nint JVM_FindLibraryEntryDelegate(nint handle, string name);

        public delegate int JNI_GetDefaultJavaVMInitArgsDelegate(void* vm_args);
        public delegate int JNI_GetCreatedJavaVMsDelegate(JavaVM** vmBuf, jsize bufLen, jsize* nVMs);
        public delegate int JNI_CreateJavaVMDelegate(JavaVM** p_vm, void** p_env, void* vm_args);

        /// <summary>
        /// Gets the default instance.
        /// </summary>
        public static LibJVM Instance = new LibJVM();

        readonly Set_JNI_GetDefaultJavaVMInitArgsDelegate _Set_JNI_GetDefaultJavaVMInitArgs;
        readonly Set_JNI_GetCreatedJavaVMsDelegate _Set_JNI_GetCreatedJavaVMs;
        readonly Set_JNI_CreateJavaVMDelegate _Set_JNI_CreateJavaVM;
        readonly JVM_LoadLibraryDelegate _JVM_LoadLibrary;
        readonly JVM_UnloadLibraryDelegate _JVM_UnloadLibrary;
        readonly JVM_FindLibraryEntryDelegate _JVM_FindLibraryEntry;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        LibJVM()
        {
            Handle = NativeLibrary.Load(Path.Combine(JVM.Properties.HomePath, "bin", NativeLibrary.MapLibraryName("jvm")));
            _Set_JNI_GetDefaultJavaVMInitArgs = Marshal.GetDelegateForFunctionPointer<Set_JNI_GetDefaultJavaVMInitArgsDelegate>(NativeLibrary.GetExport(Handle, "Set_JNI_GetDefaultJavaVMInitArgs"));
            _Set_JNI_GetCreatedJavaVMs = Marshal.GetDelegateForFunctionPointer<Set_JNI_GetCreatedJavaVMsDelegate>(NativeLibrary.GetExport(Handle, "Set_JNI_GetCreatedJavaVMs"));
            _Set_JNI_CreateJavaVM = Marshal.GetDelegateForFunctionPointer<Set_JNI_CreateJavaVMDelegate>(NativeLibrary.GetExport(Handle, "Set_JNI_CreateJavaVM"));
            _JVM_LoadLibrary = Marshal.GetDelegateForFunctionPointer<JVM_LoadLibraryDelegate>(NativeLibrary.GetExport(Handle, "JVM_LoadLibrary"));
            _JVM_UnloadLibrary = Marshal.GetDelegateForFunctionPointer<JVM_UnloadLibraryDelegate>(NativeLibrary.GetExport(Handle, "JVM_UnloadLibrary"));
            _JVM_FindLibraryEntry = Marshal.GetDelegateForFunctionPointer<JVM_FindLibraryEntryDelegate>(NativeLibrary.GetExport(Handle, "JVM_FindLibraryEntry"));
        }

        /// <summary>
        /// Gets a handle to the loaded libjvm library.
        /// </summary>
        public readonly nint Handle;

        /// <summary>
        /// Invokes the 'Set_JNI_GetDefaultJavaVMInitArgs' method from libjvm.
        /// </summary>
        /// <param name="func"></param>
        public void Set_JNI_GetDefaultJavaVMInitArgs(JNI_GetDefaultJavaVMInitArgsDelegate func) => _Set_JNI_GetDefaultJavaVMInitArgs(func);

        /// <summary>
        /// Invokes the 'Set_JNI_GetCreatedJavaVMs' method from libjvm.
        /// </summary>
        /// <param name="func"></param>
        public void Set_JNI_GetCreatedJavaVMs(JNI_GetCreatedJavaVMsDelegate func) => _Set_JNI_GetCreatedJavaVMs(func);

        /// <summary>
        /// Invokes the 'Set_JNI_CreateJavaVM' method from libjvm.
        /// </summary>
        /// <param name="func"></param>
        public void Set_JNI_CreateJavaVM(JNI_CreateJavaVMDelegate func) => _Set_JNI_CreateJavaVM(func);

        /// <summary>
        /// Invokes the 'JVM_LoadLibrary' method from libjvm.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public nint JVM_LoadLibrary(string name) => _JVM_LoadLibrary(name);

        /// <summary>
        /// Invokes the 'JVM_UnloadLibrary' method from libjvm.
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        public nint JVM_UnloadLibrary(nint handle) => _JVM_UnloadLibrary(handle);

        /// <summary>
        /// Invokes the 'JVM_FindLibraryEntry' method from libjvm.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public nint JVM_FindLibraryEntry(nint handle, string name) => _JVM_FindLibraryEntry(handle, name);

        /// <summary>
        /// Finalizes the instance.
        /// </summary>
        ~LibJVM()
        {
            if (Handle != 0)
                NativeLibrary.Free(Handle);
        }

    }

#endif

}