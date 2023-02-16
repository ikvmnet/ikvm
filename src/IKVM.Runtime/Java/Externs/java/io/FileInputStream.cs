using System;
using System.IO;
using System.Security;

using IKVM.Internal;
using IKVM.Runtime.Accessors.Java.Io;

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

#endif

        /// <summary>
        /// Implements the native method 'open0'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="name"></param>
        /// <param name="fd"></param>
        public static void open0(object self, string name, object fd)
        {
#if FIRST_PASS 
            throw new NotImplementedException();
#else
            try
            {
                FileDescriptorAccessor.SetStream(fd, File.OpenRead(name));
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
        /// Implements the native method 'skip'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="n"></param>
        /// <param name="fd"></param>
        /// <returns></returns>
        public static long skip(object self, long n, object fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                var stream = FileDescriptorAccessor.GetStream(fd);
                if (stream == null)
                    throw new global::java.io.IOException("Stream closed.");
                if (stream.CanSeek == false)
                    throw new global::java.io.IOException("The handle is invalid.");

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
        /// <param name="fd"></param>
        /// <returns></returns>
        public static int available(object self, object fd)
        {
#if FIRST_PASS
            throw new NotImplementedException();
#else
            try
            {
                var stream = FileDescriptorAccessor.GetStream(fd);
                if (stream == null)
                    throw new global::java.io.IOException("Stream closed.");
                if (stream.CanSeek == false)
                    return 0;

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