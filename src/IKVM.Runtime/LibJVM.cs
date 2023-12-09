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

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int JNI_GetDefaultJavaVMInitArgsFunc(void* vm_args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int JNI_GetCreatedJavaVMsFunc(JavaVM** vmBuf, jsize bufLen, jsize* nVMs);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int JNI_CreateJavaVMFunc(JavaVM** p_vm, void** p_env, void* vm_args);

        delegate void Set_JNI_GetDefaultJavaVMInitArgsDelegate(JNI_GetDefaultJavaVMInitArgsFunc func);
        delegate void Set_JNI_GetCreatedJavaVMsDelegate(JNI_GetCreatedJavaVMsFunc func);
        delegate void Set_JNI_CreateJavaVMDelegate(JNI_CreateJavaVMFunc func);
        delegate nint JVM_LoadLibraryDelegate(string name);
        delegate nint JVM_UnloadLibraryDelegate(nint handle);
        delegate nint JVM_FindLibraryEntryDelegate(nint handle, string name);

        /// <summary>
        /// Gets the default instance.
        /// </summary>
        public static readonly LibJvm Instance = new();

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

            _Set_JNI_GetDefaultJavaVMInitArgs = Marshal.GetDelegateForFunctionPointer<Set_JNI_GetDefaultJavaVMInitArgsDelegate>(Handle.GetExport("Set_JNI_GetDefaultJavaVMInitArgs", sizeof(nint)).Handle);
            _Set_JNI_GetCreatedJavaVMs = Marshal.GetDelegateForFunctionPointer<Set_JNI_GetCreatedJavaVMsDelegate>(Handle.GetExport("Set_JNI_GetCreatedJavaVMs", sizeof(nint)).Handle);
            _Set_JNI_CreateJavaVM = Marshal.GetDelegateForFunctionPointer<Set_JNI_CreateJavaVMDelegate>(Handle.GetExport("Set_JNI_CreateJavaVM", sizeof(nint)).Handle);
            _JVM_LoadLibrary = Marshal.GetDelegateForFunctionPointer<JVM_LoadLibraryDelegate>(Handle.GetExport("JVM_LoadLibrary", sizeof(nint)).Handle);
            _JVM_UnloadLibrary = Marshal.GetDelegateForFunctionPointer<JVM_UnloadLibraryDelegate>(Handle.GetExport("JVM_UnloadLibrary", sizeof(nint)).Handle);
            _JVM_FindLibraryEntry = Marshal.GetDelegateForFunctionPointer<JVM_FindLibraryEntryDelegate>(Handle.GetExport("JVM_FindLibraryEntry", sizeof(nint) + sizeof(nint)).Handle);
        }

        /// <summary>
        /// Gets a handle to the loaded libjvm library.
        /// </summary>
        public NativeLibraryHandle Handle { get; private set; }

        /// <summary>
        /// Invokes the 'Set_JNI_GetDefaultJavaVMInitArgs' method from libjvm.
        /// </summary>
        /// <param name="func"></param>
        public void Set_JNI_GetDefaultJavaVMInitArgs(JNI_GetDefaultJavaVMInitArgsFunc func) => _Set_JNI_GetDefaultJavaVMInitArgs(func);

        /// <summary>
        /// Invokes the 'Set_JNI_GetCreatedJavaVMs' method from libjvm.
        /// </summary>
        /// <param name="func"></param>
        public void Set_JNI_GetCreatedJavaVMs(JNI_GetCreatedJavaVMsFunc func) => _Set_JNI_GetCreatedJavaVMs(func);

        /// <summary>
        /// Invokes the 'Set_JNI_CreateJavaVM' method from libjvm.
        /// </summary>
        /// <param name="func"></param>
        public void Set_JNI_CreateJavaVM(JNI_CreateJavaVMFunc func) => _Set_JNI_CreateJavaVM(func);

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
