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
    internal unsafe class LibJvm
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
        public static LibJvm Instance = new();

        readonly NativeLibraryExport Set_JNI_GetDefaultJavaVMInitArgs_Ptr;
        readonly NativeLibraryExport Set_JNI_GetCreatedJavaVMs_Ptr;
        readonly NativeLibraryExport Set_JNI_CreateJavaVM_Ptr;
        readonly NativeLibraryExport JVM_LoadLibrary_Ptr;
        readonly NativeLibraryExport JVM_UnloadLibrary_Ptr;
        readonly NativeLibraryExport JVM_FindLibraryEntry_Ptr;

        readonly Set_JNI_GetDefaultJavaVMInitArgsDelegate _Set_JNI_GetDefaultJavaVMInitArgs;
        readonly Set_JNI_GetCreatedJavaVMsDelegate _Set_JNI_GetCreatedJavaVMs;
        readonly Set_JNI_CreateJavaVMDelegate _Set_JNI_CreateJavaVM;
        readonly JVM_LoadLibraryDelegate _JVM_LoadLibrary;
        readonly JVM_UnloadLibraryDelegate _JVM_UnloadLibrary;
        readonly JVM_FindLibraryEntryDelegate _JVM_FindLibraryEntry;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        LibJvm()
        {
            // load libjvm through IKVM native library functionality
            if ((Handle = NativeLibrary.Load(Path.Combine(JVM.Properties.HomePath, "bin", NativeLibrary.MapLibraryName("jvm")))) == null)
                throw new InternalException("Could not load libjvm.");

            _Set_JNI_GetDefaultJavaVMInitArgs = Marshal.GetDelegateForFunctionPointer<Set_JNI_GetDefaultJavaVMInitArgsDelegate>((Set_JNI_GetDefaultJavaVMInitArgs_Ptr = Handle.GetExport("Set_JNI_GetDefaultJavaVMInitArgs")).Handle);
            _Set_JNI_GetCreatedJavaVMs = Marshal.GetDelegateForFunctionPointer<Set_JNI_GetCreatedJavaVMsDelegate>((Set_JNI_GetCreatedJavaVMs_Ptr = Handle.GetExport("Set_JNI_GetCreatedJavaVMs")).Handle);
            _Set_JNI_CreateJavaVM = Marshal.GetDelegateForFunctionPointer<Set_JNI_CreateJavaVMDelegate>((Set_JNI_CreateJavaVM_Ptr = Handle.GetExport("Set_JNI_CreateJavaVM")).Handle);
            _JVM_LoadLibrary = Marshal.GetDelegateForFunctionPointer<JVM_LoadLibraryDelegate>((JVM_LoadLibrary_Ptr = Handle.GetExport("JVM_LoadLibrary")).Handle);
            _JVM_UnloadLibrary = Marshal.GetDelegateForFunctionPointer<JVM_UnloadLibraryDelegate>((JVM_UnloadLibrary_Ptr = Handle.GetExport("JVM_UnloadLibrary")).Handle);
            _JVM_FindLibraryEntry = Marshal.GetDelegateForFunctionPointer<JVM_FindLibraryEntryDelegate>((JVM_FindLibraryEntry_Ptr = Handle.GetExport("JVM_FindLibraryEntry")).Handle);
        }

        /// <summary>
        /// Gets a handle to the loaded libjvm library.
        /// </summary>
        public NativeLibraryHandle Handle { get; private set; }

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

    }

#endif

}
