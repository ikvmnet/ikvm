using System;
using System.Buffers;
using System.IO;
using System.Runtime.InteropServices;

using IKVM.Runtime;
using IKVM.Runtime.Accessors.Java.Io;
using IKVM.Runtime.JNI;

namespace IKVM.Java.Externs.sun.nio.ch
{

    /// <summary>
    /// Implements the native methods for <see cref="global::sun.nio.ch.FileDispatcherImpl"/>.
    /// </summary>
    static partial class FileDispatcherImpl
    {

#if FIRST_PASS == false

        static FileDescriptorAccessor fileDescriptorAccessor;

        static FileDescriptorAccessor FileDescriptorAccessor => JVM.Internal.BaseAccessors.Get(ref fileDescriptorAccessor);

        static global::ikvm.@internal.CallerID __callerID;
        delegate int __jniDelegate__read0(IntPtr jniEnv, IntPtr clazz, IntPtr fdo, long address, int len);
        static __jniDelegate__read0 __jniPtr__read0;
        delegate int __jniDelegate__pread0(IntPtr jniEnv, IntPtr clazz, IntPtr fdo, long address, int len, long position);
        static __jniDelegate__pread0 __jniPtr__pread0;
        delegate long __jniDelegate__readv0(IntPtr jniEnv, IntPtr clazz, IntPtr fdo, long address, int len);
        static __jniDelegate__readv0 __jniPtr__readv0;
        delegate long __jniDelegate__size0(IntPtr jniEnv, IntPtr clazz, IntPtr fdo);
        static __jniDelegate__size0 __jniPtr__size0;

#endif

        [StructLayout(LayoutKind.Sequential)]
        struct iovec
        {

            public IntPtr iov_base;
            public int iov_len;

        }

        /// <summary>
        /// Implements the native method 'read0'.
        /// </summary>
        /// <param name="fdo"></param>
        /// <param name="address"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        /// <exception cref="global::java.io.IOException"></exception>
        public static unsafe int read0(object fdo, long address, int len)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (FileDescriptorAccessor.GetObj(fdo) is not null and not FileStream)
            {
                if (len == 0)
                    return 0;

                var stream = FileDescriptorAccessor.GetStream(fdo);
                if (stream == null)
                    throw new global::java.io.IOException("Stream closed.");

                if (stream.CanRead == false)
                    return global::sun.nio.ch.IOStatus.UNSUPPORTED;

                try
                {
                    int r = -1;

#if NETFRAMEWORK
                var buf = ArrayPool<byte>.Shared.Rent(len);

                try
                {
                    r = stream.Read(buf, 0, len);
                    Marshal.Copy(buf, 0, (IntPtr)address, r);
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(buf);
                }
#else
                    r = stream.Read(new Span<byte>((byte*)(IntPtr)address, len));
#endif

                    return r == 0 ? global::sun.nio.ch.IOStatus.EOF : r;
                }
                catch (EndOfStreamException)
                {
                    return global::sun.nio.ch.IOStatus.EOF;
                }
                catch (NotSupportedException)
                {
                    return global::sun.nio.ch.IOStatus.UNSUPPORTED;
                }
                catch (ObjectDisposedException)
                {
                    return global::sun.nio.ch.IOStatus.UNAVAILABLE;
                }
                catch (Exception e)
                {
                    throw new global::java.io.IOException("Read failed.", e);
                }
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::sun.nio.ch.FileDispatcherImpl).TypeHandle); ;
                __jniPtr__read0 ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__read0>(JNIFrame.GetFuncPtr(__callerID, "sun/nio/ch/FileDispatcherImpl", nameof(read0), "(Ljava/io/FileDescriptor;JI)I"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    return __jniPtr__read0(jniEnv, jniFrm.MakeLocalRef(ClassLiteral<global::sun.nio.ch.FileDispatcherImpl>.Value), jniFrm.MakeLocalRef(fdo), address, len);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("*** exception in native code ***");
                    System.Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    jniFrm.Leave();
                }
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'pread0'.
        /// </summary>
        /// <param name="fdo"></param>
        /// <param name="address"></param>
        /// <param name="len"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        /// <exception cref="global::java.io.IOException"></exception>
        public static unsafe int pread0(object fdo, long address, int len, long position)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (FileDescriptorAccessor.GetObj(fdo) is not null and not FileStream)
            {
                if (len == 0)
                    return 0;

                var stream = FileDescriptorAccessor.GetStream(fdo);
                if (stream == null)
                    throw new global::java.io.IOException("Stream closed.");

                if (stream.CanRead == false)
                    return global::sun.nio.ch.IOStatus.UNSUPPORTED;
                if (stream.CanSeek == false)
                    return global::sun.nio.ch.IOStatus.UNSUPPORTED;

                var p = stream.Position;

                try
                {
                    stream.Seek(position, SeekOrigin.Begin);
                }
                catch (EndOfStreamException)
                {
                    return global::sun.nio.ch.IOStatus.EOF;
                }
                catch (NotSupportedException)
                {
                    return global::sun.nio.ch.IOStatus.UNSUPPORTED;
                }
                catch (ObjectDisposedException)
                {
                    return global::sun.nio.ch.IOStatus.UNAVAILABLE;
                }
                catch (Exception e)
                {
                    throw new global::java.io.IOException("Seek failed.", e);
                }

                try
                {
                    int length = -1;

#if NETFRAMEWORK
                    var buf = ArrayPool<byte>.Shared.Rent(len);

                    try
                    {
                        length = stream.Read(buf, 0, len);
                        Marshal.Copy(buf, 0, (IntPtr)address, length);
                    }
                    finally
                    {
                        ArrayPool<byte>.Shared.Return(buf);
                    }
#else
                    length = stream.Read(new Span<byte>((void*)(IntPtr)address, len));
#endif

                    stream.Seek(p, SeekOrigin.Begin);
                    return length == 0 ? global::sun.nio.ch.IOStatus.EOF : length;
                }
                catch (EndOfStreamException)
                {
                    return global::sun.nio.ch.IOStatus.EOF;
                }
                catch (NotSupportedException)
                {
                    return global::sun.nio.ch.IOStatus.UNSUPPORTED;
                }
                catch (ObjectDisposedException)
                {
                    return global::sun.nio.ch.IOStatus.UNAVAILABLE;
                }
                catch (Exception e)
                {
                    throw new global::java.io.IOException("Read failed.", e);
                }
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::sun.nio.ch.FileDispatcherImpl).TypeHandle); ;
                __jniPtr__pread0 ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__pread0>(JNIFrame.GetFuncPtr(__callerID, "sun/nio/ch/FileDispatcherImpl", nameof(pread0), "(Ljava/io/FileDescriptor;JIJ)I"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    return __jniPtr__pread0(jniEnv, jniFrm.MakeLocalRef(ClassLiteral<global::sun.nio.ch.FileDispatcherImpl>.Value), jniFrm.MakeLocalRef(fdo), address, len, position);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("*** exception in native code ***");
                    System.Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    jniFrm.Leave();
                }
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'readv0'.
        /// </summary>
        /// <param name="fdo"></param>
        /// <param name="address"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static unsafe long readv0(object fdo, long address, int len)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (FileDescriptorAccessor.GetObj(fdo) is not null and not FileStream)
            {
                var stream = FileDescriptorAccessor.GetStream(fdo);
                if (stream == null)
                    throw new global::java.io.IOException("Stream closed.");

                if (stream.CanRead == false)
                    return global::sun.nio.ch.IOStatus.UNSUPPORTED;

                try
                {
                    // map io vecors to read into
                    var vecs = new ReadOnlySpan<iovec>((byte*)(IntPtr)address, len);
                    var length = 0;

                    // issue independent reads for each vector
                    for (int i = 0; i < vecs.Length; i++)
                    {
                        int l;

#if NETFRAMEWORK
                        var buf = ArrayPool<byte>.Shared.Rent(vecs[i].iov_len);

                        try
                        {
                            l = stream.Read(buf, 0, vecs[i].iov_len);
                            Marshal.Copy(buf, 0, vecs[i].iov_base, l);
                            length += l;

                        }
                        finally
                        {
                            ArrayPool<byte>.Shared.Return(buf);
                        }
#else
                        l = stream.Read(new Span<byte>((byte*)vecs[i].iov_base, vecs[i].iov_len));
                        length += l;
#endif

                        // we failed to read up to the requested amount, so we must be at the end
                        if (l < vecs[i].iov_len)
                            break;
                    }

                    // if we read a total of zero bytes, we must be at the end of the file
                    return length == 0 ? global::sun.nio.ch.IOStatus.EOF : length;
                }
                catch (EndOfStreamException)
                {
                    return global::sun.nio.ch.IOStatus.EOF;
                }
                catch (NotSupportedException)
                {
                    return global::sun.nio.ch.IOStatus.UNSUPPORTED;
                }
                catch (ObjectDisposedException)
                {
                    return global::sun.nio.ch.IOStatus.UNAVAILABLE;
                }
                catch (Exception e)
                {
                    throw new global::java.io.IOException("Read failed.", e);
                }
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::sun.nio.ch.FileDispatcherImpl).TypeHandle); ;
                __jniPtr__readv0 ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__readv0>(JNIFrame.GetFuncPtr(__callerID, "sun/nio/ch/FileDispatcherImpl", nameof(readv0), "(Ljava/io/FileDescriptor;JI)J"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    return __jniPtr__readv0(jniEnv, jniFrm.MakeLocalRef(ClassLiteral<global::sun.nio.ch.FileDispatcherImpl>.Value), jniFrm.MakeLocalRef(fdo), address, len);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("*** exception in native code ***");
                    System.Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    jniFrm.Leave();
                }
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'size'.
        /// </summary>
        /// <param name="fdo"></param>
        /// <returns></returns>
        public static long size0(object fdo)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (FileDescriptorAccessor.GetObj(fdo) is not null and not FileStream)
            {
                var stream = FileDescriptorAccessor.GetStream(fdo);
                if (stream == null)
                    throw new global::java.io.IOException("Stream closed.");

                try
                {
                    return stream.Length;
                }
                catch (EndOfStreamException)
                {
                    return global::sun.nio.ch.IOStatus.EOF;
                }
                catch (NotSupportedException)
                {
                    return global::sun.nio.ch.IOStatus.UNSUPPORTED;
                }
                catch (ObjectDisposedException)
                {
                    return global::sun.nio.ch.IOStatus.UNAVAILABLE;
                }
                catch (Exception e)
                {
                    throw new global::java.io.IOException("Size failed.", e);
                }
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::sun.nio.ch.FileDispatcherImpl).TypeHandle); ;
                __jniPtr__size0 ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__size0>(JNIFrame.GetFuncPtr(__callerID, "sun/nio/ch/FileDispatcherImpl", nameof(size0), "(Ljava/io/FileDescriptor;)J"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    return __jniPtr__size0(jniEnv, jniFrm.MakeLocalRef(ClassLiteral<global::sun.nio.ch.FileDispatcherImpl>.Value), jniFrm.MakeLocalRef(fdo));
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("*** exception in native code ***");
                    System.Console.WriteLine(ex);
                    throw;
                }
                finally
                {
                    jniFrm.Leave();
                }
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'close'.
        /// </summary>
        /// <param name="fdo"></param>
        public static void close0(object fdo)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (fdo == null)
                return;

            try
            {
                var h = FileDescriptorAccessor.GetHandle(fdo);
                FileDescriptorAccessor.SetHandle(fdo, -1);
                LibIkvm.Instance.io_close_file(h);
            }
            catch
            {
                // ignore
            }
#endif
        }

        /// <summary>
        /// Implements the native method 'transferToDirectlyNeedsPositionLock0'.
        /// </summary>
        /// <returns></returns>
        public static bool transferToDirectlyNeedsPositionLock0()
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            return RuntimeUtil.IsWindows;
#endif
        }

        /// <summary>
        /// Implements the native method 'duplicateForMapping'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fdo"></param>
        /// <returns></returns>
        public static object duplicateForMapping(object self, object fdo)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var newfd = FileDescriptorAccessor.Init();
            return newfd;
#endif
        }

    }

}
