﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

using IKVM.Runtime;
using IKVM.Runtime.Accessors.Java.Io;
using IKVM.Runtime.JNI;

namespace IKVM.Java.Externs.java.io
{

    /// <summary>
    /// Implements the native methods for 'FileInputStream'.
    /// </summary>
    static class FileInputStream
    {

#if FIRST_PASS == false && IMPORTER == false && EXPORTER == false

        static FileDescriptorAccessor fileDescriptorAccessor;
        static FileInputStreamAccessor fileInputStreamAccessor;

        static FileDescriptorAccessor FileDescriptorAccessor => JVM.Internal.BaseAccessors.Get(ref fileDescriptorAccessor);

        static FileInputStreamAccessor FileInputStreamAccessor => JVM.Internal.BaseAccessors.Get(ref fileInputStreamAccessor);

        static global::ikvm.@internal.CallerID __callerID;
        delegate void __jniDelegate__open0(IntPtr jniEnv, IntPtr self, IntPtr path);
        static __jniDelegate__open0 __jniPtr__open0;
        delegate int __jniDelegate__read0(IntPtr jniEnv, IntPtr self);
        static __jniDelegate__read0 __jniPtr__read0;
        delegate int __jniDelegate__readBytes(IntPtr jniEnv, IntPtr self, IntPtr bytes, int off, int len);
        static __jniDelegate__readBytes __jniPtr__readBytes;
        delegate long __jniDelegate__skip(IntPtr jniEnv, IntPtr self, long toSkip);
        static __jniDelegate__skip __jniPtr__skip0;
        delegate int __jniDelegate__available(IntPtr jniEnv, IntPtr self);
        static __jniDelegate__available __jniPtr__available0;
        delegate int __jniDelegate__close0(IntPtr jniEnv, IntPtr self);
        static __jniDelegate__close0 __jniPtr__close0;

#endif

        /// <summary>
        /// Implements the native method 'open0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="path"></param>
        public static void open0(object self, string path)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (JVM.Vfs.IsPath(path))
            {
                try
                {
                    var fd = FileInputStreamAccessor.GetFd(self);
                    if (fd == null)
                        throw new global::java.io.IOException("The handle is invalid.");

                    FileDescriptorAccessor.SetPtr(fd, -1);
                    FileDescriptorAccessor.SetStream(fd, JVM.Vfs.Open(path, FileMode.Open, FileAccess.Read));
                }
                catch (ObjectDisposedException e)
                {
                    throw new global::java.io.IOException(e.Message);
                }
                catch (ArgumentException e)
                {
                    throw new global::java.io.FileNotFoundException(e.Message);
                }
                catch (SecurityException e)
                {
                    throw new global::java.lang.SecurityException(e.Message);
                }
                catch (UnauthorizedAccessException)
                {
                    throw new global::java.io.FileNotFoundException(path + " (Access is denied)");
                }
                catch (IOException e)
                {
                    throw new global::java.io.FileNotFoundException(e.Message);
                }
                catch (NotSupportedException e)
                {
                    throw new global::java.io.FileNotFoundException(e.Message);
                }
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.io.FileInputStream).TypeHandle);
                __jniPtr__open0 ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__open0>(JNIFrame.GetFuncPtr(__callerID, "java/io/FileInputStream", nameof(open0), "(Ljava/lang/String;)V"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    __jniPtr__open0(jniEnv, jniFrm.MakeLocalRef(self), jniFrm.MakeLocalRef(path));
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
        /// Implements the native method 'read0'.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int read0(object self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var fd = FileInputStreamAccessor.GetFd(self);
            if (fd == null)
                throw new global::java.io.IOException("The handle is invalid.");

            if (FileDescriptorAccessor.GetStream(fd) is Stream stream)
            {
                if (stream.CanRead == false)
                    throw new global::java.io.IOException("Read failed.");

                try
                {
                    return stream.ReadByte();
                }
                catch (ObjectDisposedException e)
                {
                    throw new global::java.io.IOException(e.Message);
                }
                catch (NotSupportedException e)
                {
                    throw new global::java.io.IOException(e.Message);
                }
                catch (IOException e)
                {
                    throw new global::java.io.IOException(e.Message);
                }
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.io.FileInputStream).TypeHandle);
                __jniPtr__read0 ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__read0>(JNIFrame.GetFuncPtr(__callerID, "java/io/FileInputStream", nameof(read0), "()I"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    return __jniPtr__read0(jniEnv, jniFrm.MakeLocalRef(self));
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
        /// Implements the native method 'readBytes'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="bytes"></param>
        /// <param name="off"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static int readBytes(object self, byte[] bytes, int off, int len)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var fd = FileInputStreamAccessor.GetFd(self);
            if (fd == null)
                throw new global::java.io.IOException("The handle is invalid.");

            if (FileDescriptorAccessor.GetStream(fd) is Stream stream)
            {
                if (len == 0)
                    return 0;

                if ((off < 0) || (off > bytes.Length) || (len < 0) || (len > (bytes.Length - off)))
                    throw new global::java.lang.IndexOutOfBoundsException();

                if (stream.CanRead == false)
                    throw new global::java.io.IOException("Read failed.");

                try
                {
                    var n = stream.Read(bytes, off, len);
                    return n == 0 ? -1 : n;
                }
                catch (ObjectDisposedException e)
                {
                    throw new global::java.io.IOException(e.Message);
                }
                catch (NotSupportedException e)
                {
                    throw new global::java.io.IOException(e.Message);
                }
                catch (IOException e)
                {
                    throw new global::java.io.IOException(e.Message);
                }
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.io.FileInputStream).TypeHandle);
                __jniPtr__readBytes ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__readBytes>(JNIFrame.GetFuncPtr(__callerID, "java/io/FileInputStream", nameof(readBytes), "([BII)I"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    return __jniPtr__readBytes(jniEnv, jniFrm.MakeLocalRef(self), jniFrm.MakeLocalRef(bytes), off, len);
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
        /// Implements the native method 'skip0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static long skip0(object self, long n)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var fd = FileInputStreamAccessor.GetFd(self);
            if (fd == null)
                throw new global::java.io.IOException("The handle is invalid.");

            if (FileDescriptorAccessor.GetStream(fd) is Stream stream)
            {
                if (stream.CanSeek == false)
                    throw new global::java.io.IOException("The handle is invalid.");

                try
                {
                    long cur = stream.Position;
                    long end = stream.Seek(n, SeekOrigin.Current);
                    return end - cur;
                }
                catch (ObjectDisposedException e)
                {
                    throw new global::java.io.IOException(e.Message);
                }
                catch (NotSupportedException e)
                {
                    throw new global::java.io.IOException(e.Message);
                }
                catch (IOException e)
                {
                    throw new global::java.io.IOException(e.Message);
                }
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.io.FileInputStream).TypeHandle);
                __jniPtr__skip0 ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__skip>(JNIFrame.GetFuncPtr(__callerID, "java/io/FileInputStream", nameof(skip0), "(J)J"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    return __jniPtr__skip0(jniEnv, jniFrm.MakeLocalRef(self), n);
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
        /// Implements the native method 'available0'.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int available0(object self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var fd = FileInputStreamAccessor.GetFd(self);
            if (fd == null)
                throw new global::java.io.IOException("The handle is invalid.");

            if (FileDescriptorAccessor.GetStream(fd) is Stream stream)
            {
                if (stream.CanSeek == false)
                    return 0;

                try
                {
                    return (int)Math.Min(int.MaxValue, Math.Max(0, stream.Length - stream.Position));
                }
                catch (ObjectDisposedException e)
                {
                    throw new global::java.io.IOException(e.Message);
                }
                catch (NotSupportedException e)
                {
                    throw new global::java.io.IOException(e.Message);
                }
                catch (IOException e)
                {
                    throw new global::java.io.IOException(e.Message);
                }
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.io.FileInputStream).TypeHandle);
                __jniPtr__available0 ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__available>(JNIFrame.GetFuncPtr(__callerID, "java/io/FileInputStream", nameof(available0), "()I"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    return __jniPtr__available0(jniEnv, jniFrm.MakeLocalRef(self));
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
        /// Implements the native method 'close0'.
        /// </summary>
        /// <param name="self"></param>
        public static void close0(object self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var fd = FileInputStreamAccessor.GetFd(self);
            if (fd == null)
                throw new global::java.io.IOException("The handle is invalid.");

            if (FileDescriptorAccessor.GetStream(fd) is Stream stream)
            {
                try
                {
                    stream.Close();
                    FileDescriptorAccessor.SetPtr(fd, -1);
                }
                catch
                {
                    // ignore
                }
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.io.FileInputStream).TypeHandle);
                __jniPtr__close0 ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__close0>(JNIFrame.GetFuncPtr(__callerID, "java/io/FileInputStream", nameof(close0), "()V"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    __jniPtr__close0(jniEnv, jniFrm.MakeLocalRef(self));
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

    }

}