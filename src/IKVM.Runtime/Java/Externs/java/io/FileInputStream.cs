using System;
using System.IO;
using System.Security;

using IKVM.Internal;
using IKVM.Runtime.Accessors.Java.Io;
using IKVM.Runtime.Vfs;

namespace IKVM.Java.Externs.java.io
{

    /// <summary>
    /// Implements the native methods for 'FileInputStream'.
    /// </summary>
    static class FileInputStream
    {

#if FIRST_PASS == false

        static FileDescriptorAccessor fileDescriptorAccessor;
        static FileDescriptorAccessor FileDescriptorAccessor => JVM.BaseAccessors.Get(ref fileDescriptorAccessor);

        static FileInputStreamAccessor fileInputStreamAccessor;
        static FileInputStreamAccessor FileInputStreamAccessor => JVM.BaseAccessors.Get(ref fileInputStreamAccessor);

#endif

        /// <summary>
        /// Implements the native method 'open0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="name"></param>
        public static void open0(object self, string name)
        {
#if FIRST_PASS 
            throw new NotImplementedException();
#else
            var fd = FileInputStreamAccessor.GetFd(self);
            if (fd == null)
                throw new global::java.io.IOException("Stream closed.");

            try
            {
                var fileMode = FileMode.Open;
                var fileAccess = FileAccess.Read;

                if (VfsTable.Default.IsPath(name))
                {
                    FileDescriptorAccessor.SetStream(fd, VfsTable.Default.Open(name, fileMode, fileAccess));
                    return;
                }

                FileDescriptorAccessor.SetStream(fd, new FileStream(name, fileMode, fileAccess, FileShare.ReadWrite, 1, false));
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
                throw new global::java.io.IOException("Stream closed.");

            var stream = FileDescriptorAccessor.GetStream(fd);
            if (stream == null)
                throw new global::java.io.IOException("Stream closed.");
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
#endif
        }

        /// <summary>
        /// Implements the native method 'readBytes'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="b"></param>
        /// <param name="off"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static int readBytes(object self, byte[] b, int off, int len)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if ((off < 0) || (off > b.Length) || (len < 0) || (len > (b.Length - off)))
                throw new global::java.lang.IndexOutOfBoundsException();

            // user tried to read no bytes, we did
            if (len == 0)
                return 0;

            var fd = FileInputStreamAccessor.GetFd(self);
            if (fd == null)
                throw new global::java.io.IOException("Stream closed.");

            var stream = FileDescriptorAccessor.GetStream(fd);
            if (stream == null)
                throw new global::java.io.IOException("Stream closed.");
            if (stream.CanRead == false)
                throw new global::java.io.IOException("Read failed.");

            try
            {
                var n = stream.Read(b, off, len);
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
#endif
        }

        /// <summary>
        /// Implements the native method 'skip'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static long skip(object self, long n)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var fd = FileInputStreamAccessor.GetFd(self);
            if (fd == null)
                throw new global::java.io.IOException("Stream closed.");

            var stream = FileDescriptorAccessor.GetStream(fd);
            if (stream == null)
                throw new global::java.io.IOException("Stream closed.");
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
#endif
        }

        /// <summary>
        /// Implements the native method 'available'.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int available(object self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var fd = FileInputStreamAccessor.GetFd(self);
            if (fd == null)
                throw new global::java.io.IOException("Stream closed.");

            var stream = FileDescriptorAccessor.GetStream(fd);
            if (stream == null)
                throw new global::java.io.IOException("Stream closed.");
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
                throw new global::java.io.IOException("Stream closed.");

            var stream = FileDescriptorAccessor.GetStream(fd);
            if (stream == null)
                return;

            try
            {
                FileDescriptorAccessor.SetStream(fd, null);
                stream.Close();
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
#endif
        }

        /// <summary>
        /// Implements the native method 'initIDs'.
        /// </summary>
        public static void initIDs()
        {

        }

    }

}