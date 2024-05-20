using System;
using System.IO;
using System.Runtime.InteropServices;

using IKVM.Runtime.JNI;

namespace IKVM.Runtime
{

#if FIRST_PASS == false && IMPORTER == false && EXPORTER == false

    /// <summary>
    /// Required native methods available in libjvm.
    /// </summary>
    internal unsafe class LibJvm
    {

        /// <summary>
        /// Structure of callbacks passed to libjvm.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        unsafe struct JVMInvokeInterface
        {

            public nint JNI_GetDefaultJavaVMInitArgs;
            public nint JNI_GetCreatedJavaVMs;
            public nint JNI_CreateJavaVM;

            public nint JVM_ThrowException;
            public nint JVM_GetThreadInterruptEvent;
            public nint JVM_ActiveProcessorCount;

        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate int JNI_GetDefaultJavaVMInitArgsDelegate(void* vm_args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate int JNI_GetCreatedJavaVMsDelegate(JavaVM** vmBuf, int bufLen, int* nVMs);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate int JNI_CreateJavaVMDelegate(JavaVM** p_vm, void** p_env, void* vm_args);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void JVM_ThrowExceptionDelegate([MarshalAs(UnmanagedType.LPUTF8Str)] string name, [MarshalAs(UnmanagedType.LPUTF8Str)] string message);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate nint JVM_GetThreadInterruptEventDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate int JVM_ActiveProcessorCountDelegate();

        delegate void JVM_InitDelegate(JVMInvokeInterface* iface);

        delegate nint JVM_LoadLibraryDelegate([MarshalAs(UnmanagedType.LPUTF8Str)] string name);

        delegate void JVM_UnloadLibraryDelegate(nint handle);

        delegate nint JVM_FindLibraryEntryDelegate(nint handle, [MarshalAs(UnmanagedType.LPStr)] string name);

        /// <summary>
        /// Gets the default instance.
        /// </summary>
        public static readonly LibJvm Instance = new();

        readonly ikvm.@internal.CallerID callerID = ikvm.@internal.CallerID.create(typeof(LibJvm).TypeHandle);
        readonly JVMInvokeInterface* jvmii;

        readonly JVM_InitDelegate _JVM_Init;
        readonly JVM_LoadLibraryDelegate _JVM_LoadLibrary;
        readonly JVM_UnloadLibraryDelegate _JVM_UnloadLibrary;
        readonly JVM_FindLibraryEntryDelegate _JVM_FindLibraryEntry;

        readonly JNI_GetDefaultJavaVMInitArgsDelegate _JNI_GetDefaultJavaVMInitArgs;
        readonly JNI_GetCreatedJavaVMsDelegate _JNI_GetCreatedJavaVMs;
        readonly JNI_CreateJavaVMDelegate _JNI_CreateJavaVM;
        readonly JVM_ThrowExceptionDelegate _JVM_ThrowException;
        readonly JVM_GetThreadInterruptEventDelegate _JVM_GetThreadInterruptEvent;
        readonly JVM_ActiveProcessorCountDelegate _JVM_ActiveProcessorCount;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        LibJvm()
        {
            // load libjvm through IKVM native library functionality
            if ((Handle = NativeLibrary.Load(Path.Combine(JVM.Properties.HomePath, "bin", NativeLibrary.MapLibraryName("jvm")))) == null)
                throw new InternalException("Could not load libjvm.");
            // obtain delegates to functions declared in libjvm
            _JVM_Init = Marshal.GetDelegateForFunctionPointer<JVM_InitDelegate>(Handle.GetExport("JVM_Init", sizeof(nint)).Handle);
            _JVM_LoadLibrary = Marshal.GetDelegateForFunctionPointer<JVM_LoadLibraryDelegate>(Handle.GetExport("JVM_LoadLibrary", sizeof(nint)).Handle);
            _JVM_UnloadLibrary = Marshal.GetDelegateForFunctionPointer<JVM_UnloadLibraryDelegate>(Handle.GetExport("JVM_UnloadLibrary", sizeof(nint)).Handle);
            _JVM_FindLibraryEntry = Marshal.GetDelegateForFunctionPointer<JVM_FindLibraryEntryDelegate>(Handle.GetExport("JVM_FindLibraryEntry", sizeof(nint) + sizeof(nint)).Handle);

            // initialize invoke interface for calls from libjvm to IKVM
            jvmii = (JVMInvokeInterface*)Marshal.AllocHGlobal(sizeof(JVMInvokeInterface));
            jvmii->JNI_GetDefaultJavaVMInitArgs = Marshal.GetFunctionPointerForDelegate(_JNI_GetDefaultJavaVMInitArgs = JNIVM.GetDefaultJavaVMInitArgs);
            jvmii->JNI_GetCreatedJavaVMs = Marshal.GetFunctionPointerForDelegate(_JNI_GetCreatedJavaVMs = JNIVM.GetCreatedJavaVMs);
            jvmii->JNI_CreateJavaVM = Marshal.GetFunctionPointerForDelegate(_JNI_CreateJavaVM = JNIVM.CreateJavaVM);
            jvmii->JVM_ThrowException = Marshal.GetFunctionPointerForDelegate(_JVM_ThrowException = JVM_ThrowException);
            jvmii->JVM_GetThreadInterruptEvent = Marshal.GetFunctionPointerForDelegate(_JVM_GetThreadInterruptEvent = JVM_GetThreadInterruptEvent);
            jvmii->JVM_ActiveProcessorCount = Marshal.GetFunctionPointerForDelegate(_JVM_ActiveProcessorCount = JVM_ActiveProcessorCount);
            _JVM_Init(jvmii);
        }

        /// <summary>
        /// Gets a handle to the loaded libjvm library.
        /// </summary>
        public NativeLibraryHandle Handle { get; private set; }

        /// <summary>
        /// Invoked by the native code to register an exception to be thrown.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        void JVM_ThrowException(string name, string msg)
        {
            try
            {
                if (name == null)
                {
                    Tracer.Error(Tracer.Runtime, $"{nameof(LibJvm)}.{nameof(JVM_ThrowException)}: Missing name argument.");
                    return;
                }

                // find requested exception class
                var exceptionClass = RuntimeClassLoader.FromCallerID(callerID).TryLoadClassByName(name.Replace('/', '.'));
                if (exceptionClass == null)
                {
                    Tracer.Error(Tracer.Runtime, $"{nameof(LibJvm)}.{nameof(JVM_ThrowException)}: Could not find exception class {{0}}.", name);
                    return;
                }

                // find constructor
                var ctor = exceptionClass.GetMethodWrapper("<init>", msg == null ? "()V" : "(Ljava.lang.String;)V", false);
                if (ctor == null)
                {
                    Tracer.Error(Tracer.Runtime, $"{nameof(LibJvm)}.{nameof(JVM_ThrowException)}: Exception {{0}} missing constructor.", name);
                    return;
                }

                // invoke the constructor
                exceptionClass.Finish();

                var ctorMember = (java.lang.reflect.Constructor)ctor.ToMethodOrConstructor(false);
                var exception = (Exception)ctorMember.newInstance(msg == null ? Array.Empty<object>() : new object[] { msg }, callerID);
                Tracer.Verbose(Tracer.Runtime, $"{nameof(LibJvm)}.{nameof(JVM_ThrowException)}: Created exception {{0}} from libjvm.", name);
                JVM.SetPendingException(exception);
            }
            catch (Exception e)
            {
                Tracer.Error(Tracer.Runtime, $"{nameof(LibJvm)}.{nameof(JVM_ThrowException)}: Exception occurred creating exception {{0}}: {{1}}", name, e.Message);
                JVM.SetPendingException(e);
            }
        }

        /// <summary>
        /// Invoked by the native code to get an event handle to wait on for thread interruption.
        /// </summary>
        /// <returns></returns>
        nint JVM_GetThreadInterruptEvent()
        {
            try
            {
                return global::java.lang.Thread.currentThread().interruptEvent.SafeWaitHandle.DangerousGetHandle();
            }
            catch (Exception e)
            {
                Tracer.Error(Tracer.Runtime, $"{nameof(LibJvm)}.{nameof(JVM_GetThreadInterruptEvent)}: Exception occurred: {{0}}", e.Message);
                JVM.SetPendingException(e);
                return 0;
            }
        }

        /// <summary>
        /// Invoked by the native code to get the active number of processors.
        /// </summary>
        /// <returns></returns>
        int JVM_ActiveProcessorCount()
        {
            return Environment.ProcessorCount;
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

        /// <summary>
        /// Finalizes the instance.
        /// </summary>
        ~LibJvm()
        {
            if (jvmii != null)
                Marshal.FreeHGlobal((IntPtr)jvmii);
        }

    }

#endif

}