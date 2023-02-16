using System;
using System.IO;
using System.Security;

using IKVM.Internal;
using IKVM.Runtime.Accessors.Java.Io;

namespace IKVM.Java.Externs.java.io
{

    static class FileOutputStream
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
        /// <param name="append"></param>
        /// <param name="fd"></param>
        public static void open0(object self, string name, bool append, object fd)
        {
#if FIRST_PASS 
            throw new NotImplementedException();
#else
            try
            {
                if (append)
                    FileDescriptorAccessor.SetStream(fd, File.Open(name, FileMode.Append, FileAccess.Write));
                else
                    FileDescriptorAccessor.SetStream(fd, File.Open(name, FileMode.Create, FileAccess.Write));
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
        /// Implements the native method 'write'.
        /// </summary>
        /// <param name="self"></param>
        /// <param name="b"></param>
        /// <param name="append"></param>
        /// <param name="fd"></param>
        public static void write(object self, int b, bool append, object fd)
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
        /// <param name="append"></param>
        /// <param name="fd"></param>
        public static void writeBytes(object self, byte[] b, int off, int len, bool append, object fd)
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