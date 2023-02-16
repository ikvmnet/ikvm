using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

using IKVM.Internal;
using IKVM.Runtime.Accessors.Java.Io;

using static IKVM.Internal.CoreClasses;

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

#endif

        /// <summary>
        /// Implements the native method 'open0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="name"></param>
        /// <param name="mode"></param>
        /// <param name="fd"></param>
        /// <param name="O_RDWR"></param>
        public static void open0(object self, string name, int mode, object fd, int O_RDWR)
        {
#if FIRST_PASS 
            throw new NotImplementedException();
#else
            try
            {
                if ((mode & O_RDWR) == O_RDWR)
                    FileDescriptorAccessor.SetStream(fd, File.Open(name, FileMode.OpenOrCreate, FileAccess.ReadWrite));
                else
                    FileDescriptorAccessor.SetStream(fd, File.Open(name, FileMode.OpenOrCreate, FileAccess.Read));

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
        /// <param name="fd"></param>
        /// <returns></returns>
        public static int read0(object self, object fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                var stream = FileDescriptorAccessor.GetStream(fd);
                if (stream == null)
                    throw new global::java.io.IOException("Stream closed.");
                if (stream.CanRead == false)
                    throw new global::java.io.IOException("Read failed.");

                return (int)stream.ReadByte();
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
        /// <param name="fd"></param>
        /// <returns></returns>
        public static int readBytes(object self, byte[] b, int off, int len, object fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if ((off < 0) || (off > b.Length) || (len < 0) || (len > (b.Length - offset)))
                throw new global::java.lang.IndexOutOfBoundsException();

            try
            {
                var stream = FileDescriptorAccessor.GetStream(fd);
                if (stream == null)
                    throw new global::java.io.IOException("Stream closed.");
                if (stream.CanRead == false)
                    throw new global::java.io.IOException("Read failed.");

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
        /// <param name="fd"></param>
        public static void write0(object self, int b, object fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                var stream = FileDescriptorAccessor.GetStream(fd);
                if (stream == null)
                    throw new global::java.io.IOException("Stream closed.");
                if (stream.CanWrite == false)
                    throw new global::java.io.IOException("Write failed.");

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
        /// <param name="fd"></param>
        public static void writeBytes(object self, byte[] b, int off, int len, object fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            if ((off < 0) || (off > b.Length) || (len < 0) || (len > (b.Length - off)))
                throw new global::java.lang.IndexOutOfBoundsException();

            try
            {
                var stream = FileDescriptorAccessor.GetStream(fd);
                if (stream == null)
                    throw new global::java.io.IOException("Stream closed.");
                if (stream.CanWrite == false)
                    throw new global::java.io.IOException("Write failed.");

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
        public static long getFilePointer(object self, object fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                var stream = FileDescriptorAccessor.GetStream(fd);
                if (stream == null)
                    throw new global::java.io.IOException("Stream closed.");

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
        /// <param name="fd"></param>
        public static void seek0(object self, long pos, object fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                var stream = FileDescriptorAccessor.GetStream(fd);
                if (stream == null)
                    throw new global::java.io.IOException("Stream closed.");

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
        /// <param name="fd"></param>
        /// <returns></returns>
        public static long length(object self, object fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                var stream = FileDescriptorAccessor.GetStream(fd);
                if (stream == null)
                    throw new global::java.io.IOException("Stream closed.");

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
        /// <param name="fd"></param>
        public static void setLength(object self, long newLength, object fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                var stream = FileDescriptorAccessor.GetStream(fd);
                if (stream == null)
                    throw new global::java.io.IOException("Stream closed.");

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
        /// <param name="fd"></param>
        public static void close0(object self, object fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                var stream = FileDescriptorAccessor.GetStream(fd);
                if (stream == null)
                    return;

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