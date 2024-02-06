using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

using IKVM.Runtime;
using IKVM.Runtime.Accessors.Java.Io;
using IKVM.Runtime.JNI;


namespace IKVM.Java.Externs.java.io
{

    static class FileOutputStream
    {

#if FIRST_PASS == false

        static FileDescriptorAccessor fileDescriptorAccessor;
        static FileOutputStreamAccessor fileOutputStreamAccessor;

        static FileDescriptorAccessor FileDescriptorAccessor => JVM.Internal.BaseAccessors.Get(ref fileDescriptorAccessor);

        static FileOutputStreamAccessor FileOutputStreamAccessor => JVM.Internal.BaseAccessors.Get(ref fileOutputStreamAccessor);

        static global::ikvm.@internal.CallerID __callerID;
        delegate void __jniDelegate__open0(IntPtr jniEnv, IntPtr this_, IntPtr path, sbyte append);
        static __jniDelegate__open0 __jniPtr__open0;
        delegate void __jniDelegate__write(IntPtr jniEnv, IntPtr this_, int byte_, sbyte append);
        static __jniDelegate__write __jniPtr__write;
        delegate void __jniDelegate__writeBytes(IntPtr jniEnv, IntPtr this_, IntPtr bytes, int off, int len, sbyte append);
        static __jniDelegate__writeBytes __jniPtr__writeBytes;
        delegate int __jniDelegate__close0(IntPtr jniEnv, IntPtr this_);
        static __jniDelegate__close0 __jniPtr__close0;

#endif

        /// <summary>
        /// Implements the native method 'open0'.
        /// </summary>
        /// <param name="this_"></param>
        /// <param name="path"></param>
        /// <param name="append"></param>
        public static void open0(object this_, string path, bool append)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (JVM.Vfs.IsPath(path))
            {
                try
                {
                    var fd = FileOutputStreamAccessor.GetFd(this_);
                    if (fd == null)
                        throw new global::java.io.IOException("The handle is invalid.");

                    FileDescriptorAccessor.SetObj(fd, JVM.Vfs.Open(path, append ? FileMode.Append : FileMode.Create, FileAccess.Write));
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
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.io.FileOutputStream).TypeHandle);
                __jniPtr__open0 ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__open0>(JNIFrame.GetFuncPtr(__callerID, "java/io/FileOutputStream", nameof(open0), "(Ljava/lang/String;Z)V"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    var selfRef = jniFrm.MakeLocalRef(this_);
                    var pathRef = jniFrm.MakeLocalRef(path);
                    __jniPtr__open0(jniEnv, selfRef, pathRef, append ? JNIEnv.JNI_TRUE : JNIEnv.JNI_FALSE);
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
        /// Implements the native method 'write'.
        /// </summary>
        /// <param name="this_"></param>
        /// <param name="byte_"></param>
        /// <param name="append"></param>
        public static void write(object this_, int byte_, bool append)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var fd = FileOutputStreamAccessor.GetFd(this_);
            if (fd == null)
                throw new global::java.io.IOException("The handle is invalid.");

            var stream = FileDescriptorAccessor.GetStream(fd);
            if (stream == null)
                throw new global::java.io.IOException("Stream closed.");

            if (stream is not FileStream fs)
            {
                if (stream.CanWrite == false)
                    throw new global::java.io.IOException("The handle is invalid.");

                try
                {
                    stream.WriteByte((byte)byte_);
                    stream.Flush();
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
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.io.FileOutputStream).TypeHandle);
                __jniPtr__write ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__write>(JNIFrame.GetFuncPtr(__callerID, "java/io/FileOutputStream", nameof(write), "(IZ)V"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    var selfRef = jniFrm.MakeLocalRef(this_);
                    __jniPtr__write(jniEnv, selfRef, byte_, append ? JNIEnv.JNI_TRUE : JNIEnv.JNI_FALSE);
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
        /// Implements the native method 'writeBytes'.
        /// </summary>
        /// <param name="this_"></param>
        /// <param name="bytes"></param>
        /// <param name="off"></param>
        /// <param name="len"></param>
        /// <param name="append"></param>
        public static void writeBytes(object this_, byte[] bytes, int off, int len, bool append)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var fd = FileOutputStreamAccessor.GetFd(this_);
            if (fd == null)
                throw new global::java.io.IOException("The handle is invalid.");

            var stream = FileDescriptorAccessor.GetStream(fd);
            if (stream == null)
                throw new global::java.io.IOException("Stream closed.");

            if (stream is not FileStream fs)
            {
                if ((off < 0) || (off > bytes.Length) || (len < 0) || (len > (bytes.Length - off)))
                    throw new global::java.lang.IndexOutOfBoundsException();

                if (stream.CanWrite == false)
                    throw new global::java.io.IOException("The handle is invalid.");

                try
                {
                    stream.Write(bytes, off, len);
                    stream.Flush();
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
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.io.FileOutputStream).TypeHandle);
                __jniPtr__writeBytes ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__writeBytes>(JNIFrame.GetFuncPtr(__callerID, "java/io/FileOutputStream", nameof(writeBytes), "([BIIZ)V"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    __jniPtr__writeBytes(jniEnv, jniFrm.MakeLocalRef(this_), jniFrm.MakeLocalRef(bytes), off, len, append ? JNIEnv.JNI_TRUE : JNIEnv.JNI_FALSE);
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
        /// <param name="this_"></param>
        public static void close0(object this_)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var fd = FileOutputStreamAccessor.GetFd(this_);
            if (fd == null)
                throw new global::java.io.IOException("The handle is invalid.");
                
            if (FileDescriptorAccessor.GetObj(fd) is not null and not FileStream)
            {
                try
                {
                    var h = FileDescriptorAccessor.GetHandle(fd);
                    FileDescriptorAccessor.SetHandle(fd, -1);
                    LibIkvm.Instance.io_close_file(h);
                }
                catch
                {
                    // ignore
                }
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.io.FileOutputStream).TypeHandle);
                __jniPtr__close0 ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__close0>(JNIFrame.GetFuncPtr(__callerID, "java/io/FileOutputStream", nameof(close0), "()V"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    __jniPtr__close0(jniEnv, jniFrm.MakeLocalRef(this_));
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