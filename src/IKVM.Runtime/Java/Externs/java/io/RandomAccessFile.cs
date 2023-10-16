using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

using IKVM.Runtime;
using IKVM.Runtime.Accessors.Java.Io;
using IKVM.Runtime.JNI;

namespace IKVM.Java.Externs.java.io
{

    /// <summary>
    /// Implements the native methods for 'RandomAccessFile'.
    /// </summary>
    /// <remarks>
    /// These operations support both standard file operations, and operations against the IKVM VFS. In the case of
    /// files on the VFS, operations are implemented directly against the .NET stream object.
    /// </remarks>
    static unsafe class RandomAccessFile
    {

#if FIRST_PASS == false

        static FileDescriptorAccessor fileDescriptorAccessor;
        static FileDescriptorAccessor FileDescriptorAccessor => JVM.BaseAccessors.Get(ref fileDescriptorAccessor);


        static RandomAccessFileAccessor randomAccessFileAccessor;
        static RandomAccessFileAccessor RandomAccessFileAccessor => JVM.BaseAccessors.Get(ref randomAccessFileAccessor);

        static global::ikvm.@internal.CallerID __callerID;
        delegate void __jniDelegate__open0(IntPtr jniEnv, IntPtr self, IntPtr name, int mode);
        static __jniDelegate__open0 __jniPtr__open0;
        delegate int __jniDelegate__read0(IntPtr jniEnv, IntPtr self);
        static __jniDelegate__read0 __jniPtr__read0;
        delegate int __jniDelegate__readBytes(IntPtr jniEnv, IntPtr self, IntPtr bytes, int off, int len);
        static __jniDelegate__readBytes __jniPtr__readBytes;
        delegate void __jniDelegate__write0(IntPtr jniEnv, IntPtr self, int @byte);
        static __jniDelegate__write0 __jniPtr__write0;
        delegate void __jniDelegate__writeBytes(IntPtr jniEnv, IntPtr self, IntPtr bytes, int off, int len);
        static __jniDelegate__writeBytes __jniPtr__writeBytes;
        delegate long __jniDelegate__getFilePointer(IntPtr jniEnv, IntPtr self);
        static __jniDelegate__getFilePointer __jniPtr__getFilePointer;
        delegate void __jniDelegate__seek0(IntPtr jniEnv, IntPtr self, long pos);
        static __jniDelegate__seek0 __jniPtr__seek0;
        delegate long __jniDelegate__length(IntPtr jniEnv, IntPtr self);
        static __jniDelegate__length __jniPtr__length;
        delegate void __jniDelegate__setLength(IntPtr jniEnv, IntPtr self, long newLength);
        static __jniDelegate__setLength __jniPtr__setLength;
        delegate void __jniDelegate__close0(IntPtr jniEnv, IntPtr self);
        static __jniDelegate__close0 __jniPtr__close0;

#endif

        const int O_RDONLY = 1;
        const int O_RDWR = 2;
        const int O_SYNC = 4;
        const int O_DSYNC = 8;

        /// <summary>
        /// Implements the native method 'open0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="name"></param>
        /// <param name="mode"></param>
        public static void open0(object self, string name, int mode)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if (JVM.Vfs.IsPath(name))
            {
                var fd = RandomAccessFileAccessor.GetFd(self);
                if (fd == null)
                    throw new global::java.io.IOException("Invalid file handle.");

                try
                {
                    var fileMode = (FileMode)0;
                    if ((mode & O_RDONLY) == O_RDONLY)
                        fileMode |= FileMode.Open;
                    if ((mode & O_RDWR) == O_RDWR)
                        fileMode |= FileMode.OpenOrCreate;

                    var fileAccess = (FileAccess)0;
                    if ((mode & O_RDONLY) == O_RDONLY)
                        fileAccess |= FileAccess.Read;
                    if ((mode & O_RDWR) == O_RDWR)
                        fileAccess |= FileAccess.ReadWrite;

                    FileDescriptorAccessor.SetStream(fd, JVM.Vfs.Open(name, fileMode, fileAccess));
                    return;
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
                    throw new global::java.io.FileNotFoundException(name + " (Access is denied)");
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
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.io.RandomAccessFile).TypeHandle);
                __jniPtr__open0 ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__open0>(JNIFrame.GetFuncPtr(__callerID, "java/io/RandomAccessFile", nameof(open0), "(Ljava/lang/String;I)V"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    __jniPtr__open0(jniEnv, jniFrm.MakeLocalRef(self), jniFrm.MakeLocalRef(name), mode);
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
            var fd = RandomAccessFileAccessor.GetFd(self);
            if (fd == null)
                throw new global::java.io.IOException("Invalid file handle.");

            if (FileDescriptorAccessor.GetObj(fd) is not null and not FileStream)
            {
                var stream = FileDescriptorAccessor.GetStream(fd);
                if (stream == null)
                    throw new global::java.io.IOException("Stream closed.");
                if (stream.CanRead == false)
                    throw new global::java.io.IOException("Read failed.");

                try
                {
                    stream.Flush();
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
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.io.RandomAccessFile).TypeHandle);
                __jniPtr__read0 ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__read0>(JNIFrame.GetFuncPtr(__callerID, "java/io/RandomAccessFile", nameof(read0), "()I"));
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
            var fd = RandomAccessFileAccessor.GetFd(self);
            if (fd == null)
                throw new global::java.io.IOException("Invalid file handle.");

            if (FileDescriptorAccessor.GetObj(fd) is not null and not FileStream)
            {
                if (bytes == null)
                    throw new global::java.lang.NullPointerException();

                if ((off < 0) || (off > bytes.Length) || (len < 0) || (len > (bytes.Length - off)))
                    throw new global::java.lang.IndexOutOfBoundsException();

                if (len == 0)
                    return 0;

                var stream = FileDescriptorAccessor.GetStream(fd);
                if (stream == null)
                    throw new global::java.io.IOException("Stream closed.");
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
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.io.RandomAccessFile).TypeHandle);
                __jniPtr__readBytes ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__readBytes>(JNIFrame.GetFuncPtr(__callerID, "java/io/RandomAccessFile", nameof(readBytes), "([BII)I"));
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
        /// Implements the native method 'write'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="byte"></param>
        public static void write0(object self, int @byte)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var fd = RandomAccessFileAccessor.GetFd(self);
            if (fd == null)
                throw new global::java.io.IOException("Invalid file handle.");

            if (FileDescriptorAccessor.GetObj(fd) is not null and not FileStream)
            {
                var stream = FileDescriptorAccessor.GetStream(fd);
                if (stream == null)
                    throw new global::java.io.IOException("Stream closed.");
                if (stream.CanWrite == false)
                    throw new global::java.io.IOException("Write failed.");

                try
                {
                    stream.WriteByte((byte)@byte);
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
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.io.RandomAccessFile).TypeHandle);
                __jniPtr__write0 ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__write0>(JNIFrame.GetFuncPtr(__callerID, "java/io/RandomAccessFile", nameof(write0), "(I)V"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    __jniPtr__write0(jniEnv, jniFrm.MakeLocalRef(self), @byte);
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
        /// <param name="self"></param>
        /// <param name="bytes"></param>
        /// <param name="off"></param>
        /// <param name="len"></param>
        public static void writeBytes(object self, byte[] bytes, int off, int len)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var fd = RandomAccessFileAccessor.GetFd(self);
            if (fd == null)
                throw new global::java.io.IOException("Invalid file handle.");

            if (FileDescriptorAccessor.GetObj(fd) is not null and not FileStream)
            {
                if ((off < 0) || (off > bytes.Length) || (len < 0) || (len > (bytes.Length - off)))
                    throw new global::java.lang.IndexOutOfBoundsException();

                var stream = FileDescriptorAccessor.GetStream(fd);
                if (stream == null)
                    throw new global::java.io.IOException("Stream closed.");
                if (stream.CanWrite == false)
                    throw new global::java.io.IOException("Write failed.");

                try
                {
                    stream.Write(bytes, off, len);
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
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.io.RandomAccessFile).TypeHandle);
                __jniPtr__writeBytes ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__writeBytes>(JNIFrame.GetFuncPtr(__callerID, "java/io/RandomAccessFile", nameof(writeBytes), "([BII)V"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    __jniPtr__writeBytes(jniEnv, jniFrm.MakeLocalRef(self), jniFrm.MakeLocalRef(bytes), off, len);
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
        /// Implements the native method 'getFilePointer'.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static long getFilePointer(object self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var fd = RandomAccessFileAccessor.GetFd(self);
            if (fd == null)
                throw new global::java.io.IOException("Invalid file handle.");

            if (FileDescriptorAccessor.GetObj(fd) is not null and not FileStream)
            {
                var stream = FileDescriptorAccessor.GetStream(fd);
                if (stream == null)
                    throw new global::java.io.IOException("Stream closed.");

                try
                {
                    return stream.Position;
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
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.io.RandomAccessFile).TypeHandle);
                __jniPtr__getFilePointer ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__getFilePointer>(JNIFrame.GetFuncPtr(__callerID, "java/io/RandomAccessFile", nameof(getFilePointer), "()J"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    return __jniPtr__getFilePointer(jniEnv, jniFrm.MakeLocalRef(self));
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
        /// Implements the native method 'seek0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="pos"></param>
        public static void seek0(object self, long pos)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var fd = RandomAccessFileAccessor.GetFd(self);
            if (fd == null)
                throw new global::java.io.IOException("Invalid file handle.");

            if (FileDescriptorAccessor.GetObj(fd) is not null and not FileStream)
            {
                var stream = FileDescriptorAccessor.GetStream(fd);
                if (stream == null)
                    throw new global::java.io.IOException("Stream closed.");

                try
                {
                    stream.Position = pos;
                }
                catch (ObjectDisposedException e)
                {
                    throw new global::java.io.IOException(e.Message);
                }
                catch (NotSupportedException e)
                {
                    throw new global::java.io.IOException(e.Message);
                }
                catch (ArgumentOutOfRangeException)
                {
                    throw new global::java.io.IOException("Negative seek offset.");
                }
                catch (IOException e)
                {
                    throw new global::java.io.IOException(e.Message);
                }
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.io.RandomAccessFile).TypeHandle);
                __jniPtr__seek0 ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__seek0>(JNIFrame.GetFuncPtr(__callerID, "java/io/RandomAccessFile", nameof(seek0), "(J)V"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    __jniPtr__seek0(jniEnv, jniFrm.MakeLocalRef(self), pos);
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
        /// Implements the native method 'length'.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static long length(object self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var fd = RandomAccessFileAccessor.GetFd(self);
            if (fd == null)
                throw new global::java.io.IOException("Invalid file handle.");

            if (FileDescriptorAccessor.GetObj(fd) is not null and not FileStream)
            {
                var stream = FileDescriptorAccessor.GetStream(fd);
                if (stream == null)
                    throw new global::java.io.IOException("Stream closed.");

                try
                {
                    return stream.Length;
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
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.io.RandomAccessFile).TypeHandle);
                __jniPtr__length ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__length>(JNIFrame.GetFuncPtr(__callerID, "java/io/RandomAccessFile", nameof(length), "()J"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    return __jniPtr__length(jniEnv, jniFrm.MakeLocalRef(self));
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
        /// Implements the native method 'setLength'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="newLength"></param>
        public static void setLength(object self, long newLength)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var fd = RandomAccessFileAccessor.GetFd(self);
            if (fd == null)
                throw new global::java.io.IOException("Invalid file handle.");

            if (FileDescriptorAccessor.GetObj(fd) is not null and not FileStream)
            {
                var stream = FileDescriptorAccessor.GetStream(fd);
                if (stream == null)
                    throw new global::java.io.IOException("Stream closed.");

                var p = stream.Position;

                try
                {
                    stream.SetLength(newLength);
                    stream.Position = Math.Min(p, stream.Length);
                }
                catch (ObjectDisposedException e)
                {
                    stream.Position = p;
                    throw new global::java.io.IOException(e.Message);
                }
                catch (NotSupportedException e)
                {
                    stream.Position = p;
                    throw new global::java.io.IOException(e.Message);
                }
                catch (UnauthorizedAccessException e)
                {
                    stream.Position = p;
                    throw new global::java.io.IOException(e.Message);
                }
                catch (IOException e)
                {
                    stream.Position = p;
                    throw new global::java.io.IOException(e.Message);
                }
            }
            else
            {
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.io.RandomAccessFile).TypeHandle);
                __jniPtr__setLength ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__setLength>(JNIFrame.GetFuncPtr(__callerID, "java/io/RandomAccessFile", nameof(setLength), "(J)V"));
                var jniFrm = new JNIFrame();
                var jniEnv = jniFrm.Enter(__callerID);
                try
                {
                    __jniPtr__setLength(jniEnv, jniFrm.MakeLocalRef(self), newLength);
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
            var fd = RandomAccessFileAccessor.GetFd(self);
            if (fd == null)
                return;

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
                __callerID ??= global::ikvm.@internal.CallerID.create(typeof(global::java.io.RandomAccessFile).TypeHandle);
                __jniPtr__close0 ??= Marshal.GetDelegateForFunctionPointer<__jniDelegate__close0>(JNIFrame.GetFuncPtr(__callerID, "java/io/RandomAccessFile", nameof(close0), "()V"));
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
