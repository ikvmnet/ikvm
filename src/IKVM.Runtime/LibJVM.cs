using System;
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

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void IKVM_ThrowExceptionFunc([MarshalAs(UnmanagedType.LPUTF8Str)] string name, [MarshalAs(UnmanagedType.LPUTF8Str)] string message);

        delegate void Set_JNI_GetDefaultJavaVMInitArgsDelegate(JNI_GetDefaultJavaVMInitArgsFunc func);
        delegate void Set_JNI_GetCreatedJavaVMsDelegate(JNI_GetCreatedJavaVMsFunc func);
        delegate void Set_JNI_CreateJavaVMDelegate(JNI_CreateJavaVMFunc func);

        delegate void Set_IKVM_ThrowExceptionDelegate(IKVM_ThrowExceptionFunc func);
        delegate nint JVM_LoadLibraryDelegate([MarshalAs(UnmanagedType.LPUTF8Str)] string name);
        delegate void JVM_UnloadLibraryDelegate(nint handle);
        delegate nint JVM_FindLibraryEntryDelegate(nint handle, [MarshalAs(UnmanagedType.LPStr)] string name);

        /// <summary>
        /// Gets the default instance.
        /// </summary>
        public static readonly LibJvm Instance = new();

        readonly ikvm.@internal.CallerID callerID = ikvm.@internal.CallerID.create(typeof(LibJvm).TypeHandle);

        readonly Set_JNI_GetDefaultJavaVMInitArgsDelegate _Set_JNI_GetDefaultJavaVMInitArgs;
        readonly Set_JNI_GetCreatedJavaVMsDelegate _Set_JNI_GetCreatedJavaVMs;
        readonly Set_JNI_CreateJavaVMDelegate _Set_JNI_CreateJavaVM;

        readonly Set_IKVM_ThrowExceptionDelegate _Set_IKVM_ThrowException;
        readonly JVM_LoadLibraryDelegate _JVM_LoadLibrary;
        readonly JVM_UnloadLibraryDelegate _JVM_UnloadLibrary;
        readonly JVM_FindLibraryEntryDelegate _JVM_FindLibraryEntry;

        readonly IKVM_ThrowExceptionFunc _IKVM_ThrowException;

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

            _Set_IKVM_ThrowException = Marshal.GetDelegateForFunctionPointer<Set_IKVM_ThrowExceptionDelegate>(Handle.GetExport("Set_IKVM_ThrowException", sizeof(nint) + sizeof(nint)).Handle);
            _JVM_LoadLibrary = Marshal.GetDelegateForFunctionPointer<JVM_LoadLibraryDelegate>(Handle.GetExport("JVM_LoadLibrary", sizeof(nint)).Handle);
            _JVM_UnloadLibrary = Marshal.GetDelegateForFunctionPointer<JVM_UnloadLibraryDelegate>(Handle.GetExport("JVM_UnloadLibrary", sizeof(nint)).Handle);
            _JVM_FindLibraryEntry = Marshal.GetDelegateForFunctionPointer<JVM_FindLibraryEntryDelegate>(Handle.GetExport("JVM_FindLibraryEntry", sizeof(nint) + sizeof(nint)).Handle);

            Set_IKVM_ThrowException(_IKVM_ThrowException = IKVM_ThrowException);
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
        /// Invokes the 'Set_IKVM_ThrowException' method from libjvm.
        /// </summary>
        /// <param name="func"></param>
        public void Set_IKVM_ThrowException(IKVM_ThrowExceptionFunc func) => _Set_IKVM_ThrowException(func);

        /// <summary>
        /// Invoked by the native code to register an exception to be thrown.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        void IKVM_ThrowException(string name, string msg)
        {
            if (name == null)
            {
                Tracer.Error(Tracer.Runtime, $"{nameof(LibJvm)}.{nameof(IKVM_ThrowException)}: Missing name argument.");
                return;
            }

            // find requested exception class
            var exceptionClass = RuntimeClassLoader.FromCallerID(callerID).TryLoadClassByName(name.Replace('/', '.'));
            if (exceptionClass == null)
            {
                Tracer.Error(Tracer.Runtime, $"{nameof(LibJvm)}.{nameof(IKVM_ThrowException)}: Could not find exception class {{0}}.", name);
                return;
            }

            // find constructor
            var ctor = exceptionClass.GetMethodWrapper("<init>", msg == null ? "()V" : "(Ljava.lang.String;)V", false);
            if (ctor == null)
            {
                Tracer.Error(Tracer.Runtime, $"{nameof(LibJvm)}.{nameof(IKVM_ThrowException)}: Exception {{0}} missing constructor.", name);
                return;
            }

            // invoke the constructor
            exceptionClass.Finish();

            try
            {
                var ctorMember = (java.lang.reflect.Constructor)ctor.ToMethodOrConstructor(false);
                var exception = (Exception)ctorMember.newInstance(msg == null ? Array.Empty<object>() : new object[] { msg }, callerID);
                Tracer.Verbose(Tracer.Runtime, $"{nameof(LibJvm)}.{nameof(IKVM_ThrowException)}: Created exception {{0}} from libjvm.", name);
                JVM.SetPendingException(exception);
            }
            catch (Exception e)
            {
                Tracer.Error(Tracer.Runtime, $"{nameof(LibJvm)}.{nameof(IKVM_ThrowException)}: Exception occurred creating exception {{0}}: {{1}}", name, e.Message);
                JVM.SetPendingException(e);
            }
        }

        /// <summary>
        /// Invokes the 'JVM_LoadLibrary' method from libjvm.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public nint JVM_LoadLibrary(string name)
        {
            try
            {
                Tracer.Verbose(Tracer.Jni, $"{nameof(LibJvm)}.{nameof(JVM_LoadLibrary)}: {{0}}", name);
                var h = _JVM_LoadLibrary(name);
                Tracer.Verbose(Tracer.Jni, $"{nameof(LibJvm)}.{nameof(JVM_LoadLibrary)}: {{0}} => {{1}}", name, h);
                return h;
            }
            finally
            {
                JVM.ThrowPendingException();
            }
        }

        /// <summary>
        /// Invokes the 'JVM_UnloadLibrary' method from libjvm.
        /// </summary>
        /// <param name="handle"></param>
        public void JVM_UnloadLibrary(nint handle)
        {
            try
            {
                Tracer.Verbose(Tracer.Jni, $"{nameof(LibJvm)}.{nameof(JVM_UnloadLibrary)}: start {{0}}", handle);
                _JVM_UnloadLibrary(handle);
                Tracer.Verbose(Tracer.Jni, $"{nameof(LibJvm)}.{nameof(JVM_UnloadLibrary)}: finish {{0}} => {{1}}", handle);
            }
            finally
            {
                JVM.ThrowPendingException();
            }
        }

        /// <summary>
        /// Invokes the 'JVM_FindLibraryEntry' method from libjvm.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public nint JVM_FindLibraryEntry(nint handle, string name)
        {
            try
            {
                Tracer.Verbose(Tracer.Jni, $"{nameof(LibJvm)}.{nameof(JVM_FindLibraryEntry)}: {{0}} {{1}}", handle, name);
                var h = _JVM_FindLibraryEntry(handle, name);
                Tracer.Verbose(Tracer.Jni, $"{nameof(LibJvm)}.{nameof(JVM_FindLibraryEntry)}: {{0}} {{1}} => {{2}}", handle, name, h);
                return h;
            }
            finally
            {
                JVM.ThrowPendingException();
            }
        }

    }

#endif

}