using System;
using System.IO;
using System.Security;

using IKVM.Internal;
using IKVM.Runtime.Accessors.Java.Io;
using IKVM.Runtime.Vfs;

namespace IKVM.Java.Externs.java.io
{

    /// <summary>
    /// Implements the native methods for 'RandomAccessFile'.
    /// </summary>
    static class RandomAccessFile
    {

#if FIRST_PASS == false

        static FileDescriptorAccessor fileDescriptorAccessor;
        static FileDescriptorAccessor FileDescriptorAccessor => JVM.BaseAccessors.Get(ref fileDescriptorAccessor);


        static RandomAccessFileAccessor randomAccessFileAccessor;
        static RandomAccessFileAccessor RandomAccessFileAccessor => JVM.BaseAccessors.Get(ref randomAccessFileAccessor);

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
            var fd = RandomAccessFileAccessor.GetFd(self);
            if (fd == null)
                throw new global::java.io.IOException("Stream closed.");

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
            var fd = RandomAccessFileAccessor.GetFd(self);
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

            var fd = RandomAccessFileAccessor.GetFd(self);
            if (fd == null)
                throw new global::java.io.IOException("Stream closed.");

            var stream = FileDescriptorAccessor.GetStream(fd);
            if (stream == null)
                throw new global::java.io.IOException("Stream closed.");
            if (stream.CanRead == false)
                throw new global::java.io.IOException("Read failed.");

            try
            {
                return stream.Read(b, off, len);
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
        /// Implements the native method 'write'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="b"></param>
        public static void write0(object self, int b)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var fd = RandomAccessFileAccessor.GetFd(self);
            if (fd == null)
                throw new global::java.io.IOException("Stream closed.");

            var stream = FileDescriptorAccessor.GetStream(fd);
            if (stream == null)
                throw new global::java.io.IOException("Stream closed.");
            if (stream.CanWrite == false)
                throw new global::java.io.IOException("Write failed.");

            try
            {
                stream.WriteByte((byte)b);
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
#endif
        }

        /// <summary>
        /// Implements the native method 'writeBytes'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="b"></param>
        /// <param name="off"></param>
        /// <param name="len"></param>
        public static void writeBytes(object self, byte[] b, int off, int len)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if ((off < 0) || (off > b.Length) || (len < 0) || (len > (b.Length - off)))
                throw new global::java.lang.IndexOutOfBoundsException();

            var fd = RandomAccessFileAccessor.GetFd(self);
            if (fd == null)
                throw new global::java.io.IOException("Stream closed.");

            var stream = FileDescriptorAccessor.GetStream(fd);
            if (stream == null)
                throw new global::java.io.IOException("Stream closed.");
            if (stream.CanWrite == false)
                throw new global::java.io.IOException("Write failed.");

            try
            {
                stream.Write(b, off, len);
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
#endif
        }

        /// <summary>
        /// Implements the native method 'getFilePointer'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="fd"></param>
        /// <returns></returns>
        public static long getFilePointer(object self)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            var fd = RandomAccessFileAccessor.GetFd(self);
            if (fd == null)
                throw new global::java.io.IOException("Stream closed.");

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
                throw new global::java.io.IOException("Stream closed.");

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
                throw new global::java.io.IOException("Stream closed.");

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
                throw new global::java.io.IOException("Stream closed.");

            var stream = FileDescriptorAccessor.GetStream(fd);
            if (stream == null)
                throw new global::java.io.IOException("Stream closed.");

            try
            {
                stream.SetLength(newLength);
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
            var fd = RandomAccessFileAccessor.GetFd(self);
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
