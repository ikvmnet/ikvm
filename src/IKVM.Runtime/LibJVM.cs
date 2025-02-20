﻿using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

using IKVM.Runtime.Accessors.Java.Util;

namespace IKVM.Runtime
{
    using IKVM.CoreLib.Diagnostics;

#if FIRST_PASS == false && IMPORTER == false && EXPORTER == false

    using IKVM.Runtime.JNI;

    /// <summary>
    /// Required native methods available in libjvm.
    /// </summary>
    internal unsafe class LibJvm
    {

        static PropertiesAccessor propertiesAccessor;

        static PropertiesAccessor PropertiesAccessor => JVM.Internal.BaseAccessors.Get(ref propertiesAccessor);

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
            public nint JVM_IHashCode;
            public nint JVM_ArrayCopy;
            public nint JVM_InitProperties;
            public nint JVM_RawMonitorCreate;
            public nint JVM_RawMonitorDestroy;
            public nint JVM_RawMonitorEnter;
            public nint JVM_RawMonitorExit;
            public nint JVM_CopySwapMemory;

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

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate int JVM_IHashCodeDelegate(JNIEnv* env, nint handle);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void JVM_ArrayCopyDelegate(JNIEnv* env, nint ignored, nint src, int src_pos, nint dst, int dst_pos, int length);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate nint JVM_InitPropertiesDelegate(JNIEnv* env, nint props);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate nint JVM_RawMonitorCreateDelegate();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void JVM_RawMonitorDestroyDelegate(nint mon);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate int JVM_RawMonitorEnterDelegate(nint mon);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void JVM_RawMonitorExitDelegate(nint mon);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void JVM_CopySwapMemoryDelegate(JNIEnv* env, nint srcObj, long srcOffset, nint dstObj, long dstOffset, long size, long elemSize);

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
        readonly JVM_IHashCodeDelegate _JVM_IHashCode;
        readonly JVM_ArrayCopyDelegate _JVM_ArrayCopy;
        readonly JVM_InitPropertiesDelegate _JVM_InitProperties;
        readonly JVM_RawMonitorCreateDelegate _JVM_RawMonitorCreate;
        readonly JVM_RawMonitorDestroyDelegate _JVM_RawMonitorDestroy;
        readonly JVM_RawMonitorEnterDelegate _JVM_RawMonitorEnter;
        readonly JVM_RawMonitorExitDelegate _JVM_RawMonitorExit;
        readonly JVM_CopySwapMemoryDelegate _JVM_CopySwapMemory;

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
            jvmii->JVM_IHashCode = Marshal.GetFunctionPointerForDelegate(_JVM_IHashCode = JVM_IHashCode);
            jvmii->JVM_ArrayCopy = Marshal.GetFunctionPointerForDelegate(_JVM_ArrayCopy = JVM_ArrayCopy);
            jvmii->JVM_InitProperties = Marshal.GetFunctionPointerForDelegate(_JVM_InitProperties = JVM_InitProperties);
            jvmii->JVM_RawMonitorCreate = Marshal.GetFunctionPointerForDelegate(_JVM_RawMonitorCreate = JVM_RawMonitorCreate);
            jvmii->JVM_RawMonitorDestroy = Marshal.GetFunctionPointerForDelegate(_JVM_RawMonitorDestroy = JVM_RawMonitorDestroy);
            jvmii->JVM_RawMonitorEnter = Marshal.GetFunctionPointerForDelegate(_JVM_RawMonitorEnter = JVM_RawMonitorEnter);
            jvmii->JVM_RawMonitorExit = Marshal.GetFunctionPointerForDelegate(_JVM_RawMonitorExit = JVM_RawMonitorExit);
            jvmii->JVM_CopySwapMemory = Marshal.GetFunctionPointerForDelegate(_JVM_CopySwapMemory = JVM_CopySwapMemory);
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
                    JVM.Context.Diagnostics.GenericJniError($"{nameof(LibJvm)}.{nameof(JVM_ThrowException)}: Missing name argument.");
                    return;
                }

                // find requested exception class
                var exceptionClass = RuntimeClassLoader.FromCallerID(callerID).TryLoadClassByName(name.Replace('/', '.'));
                if (exceptionClass == null)
                {
                    JVM.Context.Diagnostics.GenericJniError($"{nameof(LibJvm)}.{nameof(JVM_ThrowException)}: Could not find exception class {name}.");
                    return;
                }

                // find constructor
                var ctor = exceptionClass.GetMethod("<init>", msg == null ? "()V" : "(Ljava.lang.String;)V", false);
                if (ctor == null)
                {
                    JVM.Context.Diagnostics.GenericJniError($"{nameof(LibJvm)}.{nameof(JVM_ThrowException)}: Exception {name} missing constructor.");
                    return;
                }

                // invoke the constructor
                exceptionClass.Finish();

                var ctorMember = (java.lang.reflect.Constructor)ctor.ToMethodOrConstructor(false);
                var exception = (Exception)ctorMember.newInstance(msg == null ? Array.Empty<object>() : new object[] { msg }, callerID);
                JVM.Context.Diagnostics.GenericJniTrace($"{nameof(LibJvm)}.{nameof(JVM_ThrowException)}: Created exception {name} from libjvm.");
                JVM.SetPendingException(exception);
            }
            catch (Exception e)
            {
                JVM.Context.Diagnostics.GenericJniError($"{nameof(LibJvm)}.{nameof(JVM_ThrowException)}: Exception occurred creating exception {name}: {e.Message}");
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
            catch (global::java.lang.Throwable t)
            {
                JVM.SetPendingException(t);
                return 0;
            }
            catch (Exception e)
            {
                JVM.Context.Diagnostics.GenericJniError($"{nameof(LibJvm)}.{nameof(JVM_GetThreadInterruptEvent)}: Exception occurred: {e.Message}");
                JVM.SetPendingException(new global::java.lang.InternalError(e));
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
        /// Invoked by the native code to get the hashcode of an object.
        /// </summary>
        /// <param name="env"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        int JVM_IHashCode(JNIEnv* env, nint handle)
        {
            return handle == 0 ? 0 : RuntimeHelpers.GetHashCode(env->UnwrapRef(handle));
        }

        /// <summary>
        /// Invoked by the native code to copy an array.
        /// </summary>
        /// <param name="env"></param>
        /// <param name="ignored"></param>
        /// <param name="src"></param>
        /// <param name="src_pos"></param>
        /// <param name="dst"></param>
        /// <param name="dst_pos"></param>
        /// <param name="length"></param>
        void JVM_ArrayCopy(JNIEnv* env, nint ignored, nint src, int src_pos, nint dst, int dst_pos, int length)
        {
            try
            {
                if (src == 0 || dst == 0)
                    throw new java.lang.NullPointerException();

                var s = env->UnwrapRef(src) as Array;
                var d = env->UnwrapRef(dst) as Array;
                if (s is null || d is null)
                    throw new java.lang.ArrayStoreException();

                if (src_pos < 0 || dst_pos < 0 || length < 0)
                    throw new java.lang.ArrayIndexOutOfBoundsException();

                if (((length + src_pos) > s.Length) || ((length + dst_pos) > d.Length))
                    throw new java.lang.ArrayIndexOutOfBoundsException();

                if (length == 0)
                    return;

                Array.Copy(s, src_pos, d, dst_pos, length);
            }
            catch (java.lang.Throwable t)
            {
                JVM.SetPendingException(t);
            }
            catch (ArrayTypeMismatchException)
            {
                JVM.SetPendingException(new java.lang.ArrayStoreException());
            }
            catch (Exception e)
            {
                JVM.SetPendingException(new java.lang.InternalError(e));
            }
        }

        /// <summary>
        /// Invoked by the native code to collect the system properties from the JVM.
        /// </summary>
        /// <param name="env"></param>
        /// <param name="ignored"></param>
        /// <param name="src"></param>
        /// <param name="src_pos"></param>
        /// <param name="dst"></param>
        /// <param name="dst_pos"></param>
        /// <param name="length"></param>
        nint JVM_InitProperties(JNIEnv* env, nint props)
        {
            try
            {
                var p = env->UnwrapRef(props);
                foreach (var kvp in JVM.Properties.Init)
                    if (kvp.Value is string v)
                        PropertiesAccessor.InvokeSetProperty(p, kvp.Key, v);

                return props;
            }
            catch (java.lang.Throwable t)
            {
                JVM.SetPendingException(t);
                return 0;
            }
            catch (Exception e)
            {
                JVM.SetPendingException(new java.lang.InternalError(e));
                return 0;
            }
        }

        /// <summary>
        /// Invoked by the native code to create a monitor lock.
        /// </summary>
        /// <returns></returns>
        nint JVM_RawMonitorCreate()
        {
            try
            {
                return (IntPtr)GCHandle.Alloc(new object(), GCHandleType.Normal);
            }
            catch (Exception e)
            {
                JVM.SetPendingException(new java.lang.InternalError(e));
                return 0;
            }
        }

        /// <summary>
        /// Invoked by the native code to destroy a monitor lock.
        /// </summary>v
        /// <returns></returns>
        void JVM_RawMonitorDestroy(nint mon)
        {
            try
            {
                GCHandle.FromIntPtr(mon).Free();
            }
            catch (Exception e)
            {
                JVM.SetPendingException(new java.lang.InternalError(e));
            }
        }

        /// <summary>
        /// Invoked by the native code to enter a monitor lock.
        /// </summary>
        /// <param name="mon"></param>
        /// <returns></returns>
        int JVM_RawMonitorEnter(nint mon)
        {
            try
            {
                Monitor.Enter(GCHandle.FromIntPtr(mon).Target);
            }
            catch (Exception e)
            {
                JVM.SetPendingException(new java.lang.InternalError(e));
            }

            return 0;
        }

        /// <summary>
        /// Invoked by the native code to exit a monitor lock.
        /// </summary>
        /// <param name="mon"></param>
        /// <returns></returns>
        void JVM_RawMonitorExit(nint mon)
        {
            try
            {
                Monitor.Exit(GCHandle.FromIntPtr(mon).Target);
            }
            catch (Exception e)
            {
                JVM.SetPendingException(new java.lang.InternalError(e));
            }
        }

        /// <summary>
        /// Invoked by the native code to copy and swap memory.
        /// </summary>
        /// <param name="env"></param>
        /// <param name="srcObj"></param>
        /// <param name="srcOffset"></param>
        /// <param name="dstObj"></param>
        /// <param name="dstOffset"></param>
        /// <param name="size"></param>
        /// <param name="elemSize"></param>
        unsafe void JVM_CopySwapMemory(JNIEnv* env, nint srcObj, long srcOffset, nint dstObj, long dstOffset, long size, long elemSize)
        {
            JVM_CopySwapMemory((Array)env->UnwrapRef(srcObj), srcOffset, (Array)env->UnwrapRef(dstObj), dstOffset, size, elemSize);
        }

        /// <summary>
        /// Invoked by the native code to copy and swap memory.
        /// </summary>
        /// <param name="srcObj"></param>
        /// <param name="srcOffset"></param>
        /// <param name="dstObj"></param>
        /// <param name="dstOffset"></param>
        /// <param name="size"></param>
        /// <param name="elemSize"></param>
        internal static unsafe void JVM_CopySwapMemory(Array srcObj, long srcOffset, Array dstObj, long dstOffset, long size, long elemSize)
        {
            if (size == 0)
                return;

            try
            {
                if (size % elemSize != 0)
                    throw new java.lang.InternalError($"size {size} must be multiple of element size {elemSize}");

                static unsafe Span<T> AsSpan<T>(Array obj, long off, long size) where T : unmanaged
                {
                    if (obj != null)
                        return new Span<T>(Unsafe.As<T[]>(obj)).Slice((int)(off / sizeof(T)), (int)(size / sizeof(T)));
                    else if (off != 0)
                        return new Span<T>((void*)(nint)off, (int)(size / sizeof(T)));
                    else
                        throw new java.lang.InternalError($"address must not be NULL");
                }

                switch (elemSize)
                {
                    case 2:
                        ReverseEndianness(AsSpan<short>(srcObj, srcOffset, size), AsSpan<short>(dstObj, dstOffset, size));
                        break;
                    case 4:
                        ReverseEndianness(AsSpan<int>(srcObj, srcOffset, size), AsSpan<int>(dstObj, dstOffset, size));
                        break;
                    case 8:
                        ReverseEndianness(AsSpan<long>(srcObj, srcOffset, size), AsSpan<long>(dstObj, dstOffset, size));
                        break;
                    default:
                        throw new java.lang.InternalError($"incorrect element size: {elemSize}");
                }
            }
            catch (java.lang.Throwable e)
            {
                JVM.SetPendingException(e);
            }
            catch (Exception e)
            {
                JVM.SetPendingException(new java.lang.InternalError(e));
            }
        }

        /// <summary>
        /// Reverses the endianness of the given short items.
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        static void ReverseEndianness(ReadOnlySpan<short> src, Span<short> dst)
        {
#if NET7_0_OR_GREATER
            BinaryPrimitives.ReverseEndianness(src, dst);
#else
            ref short srcRef = ref MemoryMarshal.GetReference(src);
            ref short dstRef = ref MemoryMarshal.GetReference(dst);
            if (Unsafe.AreSame(ref srcRef, ref dstRef) || !src.Overlaps(dst, out int elementOffset) || elementOffset < 0)
                for (int i = 0; i < src.Length; i++)
                    dst[i] = BinaryPrimitives.ReverseEndianness(src[i]);
            else
                for (int i = src.Length - 1; i >= 0; i--)
                    dst[i] = BinaryPrimitives.ReverseEndianness(src[i]);
#endif
        }

        /// <summary>
        /// Reverses the endianness of the given int items.
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        static void ReverseEndianness(ReadOnlySpan<int> src, Span<int> dst)
        {
#if NET7_0_OR_GREATER
            BinaryPrimitives.ReverseEndianness(src, dst);
#else
            ref int srcRef = ref MemoryMarshal.GetReference(src);
            ref int dstRef = ref MemoryMarshal.GetReference(dst);
            if (Unsafe.AreSame(ref srcRef, ref dstRef) || !src.Overlaps(dst, out int elementOffset) || elementOffset < 0)
                for (int i = 0; i < src.Length; i++)
                    dst[i] = BinaryPrimitives.ReverseEndianness(src[i]);
            else
                for (int i = src.Length - 1; i >= 0; i--)
                    dst[i] = BinaryPrimitives.ReverseEndianness(src[i]);
#endif
        }

        /// <summary>
        /// Reverses the endianness of the given int items.
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        static void ReverseEndianness(ReadOnlySpan<long> src, Span<long> dst)
        {
#if NET7_0_OR_GREATER
            BinaryPrimitives.ReverseEndianness(src, dst);
#else
            ref long srcRef = ref MemoryMarshal.GetReference(src);
            ref long dstRef = ref MemoryMarshal.GetReference(dst);
            if (Unsafe.AreSame(ref srcRef, ref dstRef) || !src.Overlaps(dst, out int elementOffset) || elementOffset < 0)
                for (int i = 0; i < src.Length; i++)
                    dst[i] = BinaryPrimitives.ReverseEndianness(src[i]);
            else
                for (int i = src.Length - 1; i >= 0; i--)
                    dst[i] = BinaryPrimitives.ReverseEndianness(src[i]);
#endif
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
                JVM.Context.Diagnostics.GenericJniTrace($"{nameof(LibJvm)}.{nameof(JVM_LoadLibrary)}: {name}");
                var h = _JVM_LoadLibrary(name);
                JVM.Context.Diagnostics.GenericJniTrace($"{nameof(LibJvm)}.{nameof(JVM_LoadLibrary)}: {name} => {h}");
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
                JVM.Context.Diagnostics.GenericJniTrace($"{nameof(LibJvm)}.{nameof(JVM_UnloadLibrary)}: start {handle}");
                _JVM_UnloadLibrary(handle);
                JVM.Context.Diagnostics.GenericJniTrace($"{nameof(LibJvm)}.{nameof(JVM_UnloadLibrary)}: finish {handle}");
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
                JVM.Context.Diagnostics.GenericJniTrace($"{nameof(LibJvm)}.{nameof(JVM_FindLibraryEntry)}: {handle} {name}");
                var h = _JVM_FindLibraryEntry(handle, name);
                JVM.Context.Diagnostics.GenericJniTrace($"{nameof(LibJvm)}.{nameof(JVM_FindLibraryEntry)}: {handle} {name} => {h}");
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